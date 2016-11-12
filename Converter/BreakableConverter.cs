using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TikiEngine;
using TikiEngine.Elements.Physic;
using System;
using FarseerPhysics.Common.PolygonManipulation;
using System.Diagnostics;

namespace Converter
{
	internal class BreakableFile
	{
		public string Name { get; set; }
		public string SourceFileName { get; set; }
		public string GenericDataFileName { get; set; }
		public string GenericDataXassetFileName { get; set; }
	}

	public class BreakableConverter
	{
		private string m_source;
		private string m_destination;

		private List<BreakableFile> m_files = new List<BreakableFile>();

		private class Line
		{
			public Vector2 start;
			public Vector2 end;
		}

		private class VectorComparer : IComparer<Vector2>
		{
			private Vector2 m_center;

			public VectorComparer(Vector2 center)
			{
				m_center = center;
			}

			public int Compare(Vector2 a, Vector2 b)
			{
				if (a.X - m_center.X >= 0 && b.X - m_center.X < 0)
				{
					return -1;
				}

				if (a.X - m_center.X < 0 && b.X - m_center.X >= 0)
				{
					return 1;
				}

				if (a.X - m_center.X == 0 && b.X - m_center.X == 0)
				{
					if (a.Y - m_center.Y >= 0 || b.Y - m_center.Y >= 0)
					{
						return a.Y > b.Y ? -1 : 1;
					}

					return b.Y > a.Y ? -1 : 1;
				}

				// compute the cross product of vectors (center -> a) x (center -> b)
				float det = (a.X - m_center.X) * (b.Y - m_center.Y) - (b.X - m_center.X) * (a.Y - m_center.Y);
				if (det < 0)
				{
					return -1;
				}

				if (det > 0)
				{
					return 1;
				}

				// points a and b are on the same line from the center
				// check which point is closer to the center
				float d1 = (a.X - m_center.X) * (a.X - m_center.X) + (a.Y - m_center.Y) * (a.Y - m_center.Y);
				float d2 = (b.X - m_center.X) * (b.X - m_center.X) + (b.Y - m_center.Y) * (b.Y - m_center.Y);

				return d1 > d2 ? -1 : 1;
			}
		}

		public BreakableConverter()
		{
		}

		public void RescanFiles()
		{
			m_files = new List<BreakableFile>();

			foreach (string file in Directory.GetFiles(m_source))
			{
				if (Path.GetExtension(file) != ".bin" || !Path.GetFileNameWithoutExtension(file).EndsWith("_p"))
				{
					continue;
				}
				string fileName = Path.GetFileNameWithoutExtension(file);
				string name = fileName.ToLower();

				BreakableFile islandFile = new BreakableFile();
				islandFile.Name = fileName;
				islandFile.SourceFileName = file;
				islandFile.GenericDataFileName = Path.Combine(m_destination, "genericdata\\entities\\breakables", name + ".tikigenericobjects");
				islandFile.GenericDataXassetFileName = Path.Combine(m_destination, "genericdata\\entities\\breakables", name + ".entity.xasset");

				m_files.Add(islandFile);
			}
		}

		public void ConvertFiles()
		{
			foreach (BreakableFile file in m_files)
			{
				writeGenericData(file.GenericDataFileName, file.GenericDataXassetFileName, file.Name);
			}
		}

		public string Source
		{
			get { return m_source; }
			set { m_source = value; }
		}

		public string Destination
		{
			get { return m_destination; }
			set { m_destination = value; }
		}

		internal List<BreakableFile> Files
		{
			get { return m_files; }
		}

		private List<Vector2> mergePolygons(List<FarseerPhysics.Common.Vertices> vertices)
		{
			FarseerPhysics.Common.Vertices result = new FarseerPhysics.Common.Vertices();

			List<Line> lines = new List<Line>();
			foreach (FarseerPhysics.Common.Vertices polygon in vertices)
			{
				for (int j = 0; j < polygon.Count - 1; ++j)
				{
					Line line = new Line();
					line.start = polygon[j];
					line.end = polygon[j + 1];

					lines.Add(line);
				}

				Line lastLine = new Line();
				lastLine.start = polygon[polygon.Count - 1];
				lastLine.end = polygon[0];

				lines.Add(lastLine);
			}

			foreach (Line line in lines.ToArray())
			{
				Vector2 intersect;
				Line equalsLine = lines.FirstOrDefault(l => l != line && FarseerPhysics.Common.LineTools.LineIntersect(l.start, l.end, line.start, line.end, out intersect) && intersect != l.start && intersect != l.end);
				if(equalsLine != null)
				{
					lines.Remove(line);
					lines.Remove(equalsLine);
				}
			}

			foreach (Line line in lines)
			{
				result.Add(line.start);
			}
			
			VectorComparer comparer = new VectorComparer(result.GetPolygonCenter());
			result.Sort(comparer);

			int i = 1;
			while (result.Count > 8)
			{
				if (i == 1)
				{
					result = SimplifyTools.CollinearSimplify(result);
				}
				else
				{
					result = SimplifyTools.ReduceByDistance(result, i);
				}

				i *= 2;
			}
			if (i != 1) result = SimplifyTools.CollinearSimplify(result);

			return result;
		}

		private void writeGenericData(string genericFilename, string xassetFilename, string sourceName)
		{
			PhysicBodyBreakable body = DataManager.LoadObject<PhysicBodyBreakable>(sourceName, true);
			List<Vector2> collisionVertices = mergePolygons(body.Vertices);

			// tranform
			XElement transformObject = new XElement("object", new XAttribute("type", "Transform2dComponentInitData"));
			XElement transformTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Transform}"));
			XElement transformInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), transformObject);
			XElement transformComponent = new XElement("object", new XAttribute("type", "EntityComponent"), transformTypeField, transformInitDataField);
			XElement transformElement = new XElement("element", new XAttribute("type", "EntityComponent"), transformComponent);

			// texture
			string textureFilename = System.IO.Path.GetFileNameWithoutExtension(body.TextureFile) + ".texture";
			XElement spriteTextureField = new XElement("field", new XAttribute("type", "{reference Texture}"), new XAttribute("name", "texture"), new XAttribute("value", "TEXR:" + textureFilename));
			XElement spriteLayerField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "layerId"), new XAttribute("value", "{enum MechanicaRenderLayer.Islands}"));
			XElement spriteObject = new XElement("object", new XAttribute("type", "SpriteComponentInitData"), spriteLayerField);
			XElement spriteTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Sprite}"));
			XElement spriteInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), spriteObject);
			XElement spriteComponent = new XElement("object", new XAttribute("type", "EntityComponent"), spriteTypeField, spriteInitDataField);
			XElement spriteElement = new XElement("element", new XAttribute("type", "EntityComponent"), spriteComponent);

			// body
			XElement bodyDensityField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "density"), new XAttribute("value", "10.0"));
			XElement bodyFrictionField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "friction"), new XAttribute("value", "1.0"));
			XElement bodyFreeRoationField = new XElement("field", new XAttribute("type", "bool"), new XAttribute("name", "freeRotation"), new XAttribute("value", "true"));
			XElement bodyMaterialField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "materialId"), new XAttribute("value", "{enum MechanicaMaterialId.Island}"));

			XElement bodyShapeVerticesArray = new XElement("array", new XAttribute("type", "float2"));
			foreach (Vector2 point in collisionVertices)
			{
				XElement xField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "x"), new XAttribute("value", point.X.ToString().Replace(',', '.')));
				XElement yField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "y"), new XAttribute("value", point.Y.ToString().Replace(',', '.')));
				XElement vertexObject = new XElement("object", new XAttribute("type", "float2"), xField, yField);
				XElement vertexElement = new XElement("element", new XAttribute("type", "float2"), vertexObject);
				bodyShapeVerticesArray.Add(vertexElement);
			}

			XElement bodyShapeTypeField = new XElement("field", new XAttribute("type", "Physics2dShapeType"), new XAttribute("name", "type"), new XAttribute("value", "{enum Physics2dShapeType.Polygon}"));
			XElement bodyShapeVerticesField = new XElement("field", new XAttribute("type", "{array float2}"), new XAttribute("name", "vertices"), bodyShapeVerticesArray);
			XElement bodyShapeObject = new XElement("object", new XAttribute("type", "Physics2dComponentShapeInitData"), bodyShapeTypeField, bodyShapeVerticesField);
			XElement bodyShapeField = new XElement("field", new XAttribute("type", "Physics2dComponentShapeInitData"), new XAttribute("name", "shape"), bodyShapeObject);

			XElement bodyObject = new XElement("object", new XAttribute("type", "Physics2dBodyComponentInitData"), bodyDensityField, bodyFrictionField, bodyFreeRoationField, bodyMaterialField, bodyShapeField);
			XElement bodyTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Physics2dComponentType.Body}"));
			XElement bodyInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), bodyObject);
			XElement bodyComponent = new XElement("object", new XAttribute("type", "EntityComponent"), bodyTypeField, bodyInitDataField);
			XElement bodyElement = new XElement("element", new XAttribute("type", "EntityComponent"), bodyComponent);

			// breakable
			XElement breakableFragmentsArray = new XElement("array", new XAttribute("type", "BreakableFragment"));
			foreach (FarseerPhysics.Common.Vertices fragmentPoints in body.Vertices)
			{
				Debug.Assert(fragmentPoints.Count <= 8);
				XElement fragmentPointsArray = new XElement("array", new XAttribute("type", "float2"));

				foreach (Vector2 point in fragmentPoints)
				{
					XElement xField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "x"), new XAttribute("value", point.X.ToString().Replace(',', '.')));
					XElement yField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "y"), new XAttribute("value", point.Y.ToString().Replace(',', '.')));
					XElement vertexObject = new XElement("object", new XAttribute("type", "float2"), xField, yField);
					XElement vertexElement = new XElement("element", new XAttribute("type", "float2"), vertexObject);

					fragmentPointsArray.Add(vertexElement);
				}

				XElement fragmentPointsField = new XElement("field", new XAttribute("type", "{array float2}"), new XAttribute("name", "points"), fragmentPointsArray);
				XElement fragmentObject = new XElement("object", new XAttribute("type", "BreakableFragment"), fragmentPointsField);
				XElement fragmentElement = new XElement("element", new XAttribute("type", "BreakableFragment"), fragmentObject);
				breakableFragmentsArray.Add(fragmentElement);
			}

			XElement breakabkeDestructionForceField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "destructionForce"), new XAttribute("value", "10.0"));
			XElement breakabkeFragmentMaterialField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "fragmentMaterialId"), new XAttribute("value", "{enum MechanicaMaterialId.Island}"));
			XElement breakableFragmentsField = new XElement("field", new XAttribute("type", "{array BreakableFragment}"), new XAttribute("name", "fragments"), breakableFragmentsArray);

			XElement breakableObject = new XElement("object", new XAttribute("type", "BreakableComponentInitData"), breakabkeDestructionForceField, breakabkeFragmentMaterialField, breakableFragmentsField);
			XElement breakableTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum MechanicaComponentType.Breakable}"));
			XElement breakableInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), breakableObject);
			XElement breakableComponent = new XElement("object", new XAttribute("type", "EntityComponent"), breakableTypeField, breakableInitDataField);
			XElement breakableElement = new XElement("element", new XAttribute("type", "EntityComponent"), breakableComponent);

			// wiggle
			XElement wiggleObject = new XElement("object", new XAttribute("type", "WiggleComponentInitData"));
			XElement wiggleTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum MechanicaComponentType.Wiggle}"));
			XElement wiggleInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), wiggleObject);
			XElement wiggleComponent = new XElement("object", new XAttribute("type", "EntityComponent"), wiggleTypeField, wiggleInitDataField);
			XElement wiggleElement = new XElement("element", new XAttribute("type", "EntityComponent"), wiggleComponent);

			// entity
			XElement componentsArray = new XElement("array", new XAttribute("type", "EntityComponent"), transformElement, spriteElement, bodyElement, breakableElement, wiggleElement);
			XElement componentsField = new XElement("field", new XAttribute("type", "{array EntityComponent}"), new XAttribute("name", "components"), componentsArray);
			XElement templateObject = new XElement("object", new XAttribute("type", "EntityTemplateData"), componentsField);
			XElement resource = new XElement("resource", new XAttribute("type", "EntityTemplate"), templateObject);
			XElement root = new XElement("tikigenericobjects", resource);
			XDocument doc = new XDocument(root);
			File.WriteAllText(genericFilename, doc.ToString());

			XElement input = new XElement("input", new XAttribute("file", Path.GetFileName(genericFilename)), new XAttribute("type", "entity"));
			XElement xassetRoot = new XElement("tikiasset", input);
			XDocument xassetDoc = new XDocument(xassetRoot);
			File.WriteAllText(xassetFilename, xassetDoc.ToString());
		}
	}
}