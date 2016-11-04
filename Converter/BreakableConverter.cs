using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TikiEngine;
using TikiEngine.Elements.Physic;

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

		public BreakableConverter()
		{
		}

		public void RescanFiles()
		{
			m_files = new List<BreakableFile>();

			foreach (string file in Directory.GetFiles(m_source))
			{
				if (Path.GetExtension(file) != ".bin")
				{
					continue;
				}
				string fileName = Path.GetFileNameWithoutExtension(file);
				string name = fileName.Substring(0, fileName.Length - 2).ToLower();

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

		private void writeGenericData(string genericFilename, string xassetFilename, string sourceName)
		{
			PhysicBodyBreakable body = DataManager.LoadObject<PhysicBodyBreakable>(sourceName, true);

			XElement transformObject = new XElement("object", new XAttribute("type", "Transform2dComponentInitData"));
			XElement transformTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Transform}"));
			XElement transformInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), transformObject);
			XElement transformComponent = new XElement("object", new XAttribute("type", "EntityComponent"), transformTypeField, transformInitDataField);
			XElement transformElement = new XElement("element", new XAttribute("type", "EntityComponent"), transformComponent);

			//XElement textureOffsetXField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "x"), new XAttribute("value", textureOffset.X.ToString().Replace(',', '.')));
			//XElement textureOffsetYField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "y"), new XAttribute("value", textureOffset.Y.ToString().Replace(',', '.')));
			//XElement textureOffsetObject = new XElement("object", new XAttribute("type", "float2"), textureOffsetXField, textureOffsetYField);

			//XElement textureTextureField = new XElement("field", new XAttribute("type", "{reference Texture}"), new XAttribute("name", "texture"), new XAttribute("value", "TEXR:" + textureFilename));
			//XElement textureOffsetField = new XElement("field", new XAttribute("type", "float2"), new XAttribute("name", "offset"), textureOffsetObject);
			XElement textureLayerField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "layerId"), new XAttribute("value", "5"));
			XElement textureObject = new XElement("object", new XAttribute("type", "TextureComponentInitData"), textureLayerField);
			XElement textureTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Texture}"));
			XElement textureInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), textureObject);
			XElement textureComponent = new XElement("object", new XAttribute("type", "EntityComponent"), textureTypeField, textureInitDataField);
			XElement textureElement = new XElement("element", new XAttribute("type", "EntityComponent"), textureComponent);

			XElement bodyDensityField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "density"), new XAttribute("value", "10.0"));
			XElement bodyFrictionField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "friction"), new XAttribute("value", "1.0"));
			XElement bodyFreeRoationField = new XElement("field", new XAttribute("type", "bool"), new XAttribute("name", "freeRotation"), new XAttribute("value", "true"));
			XElement bodyMaterialField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "materialId"), new XAttribute("value", "{enum MechanicaMaterialId.Island}"));

			XElement bodyShapeVerticesArray = new XElement("array", new XAttribute("type", "float2"));
			//foreach (Vector2 point in collisionVertices)
			//{
			//	XElement xField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "x"), new XAttribute("value", point.X.ToString().Replace(',', '.')));
			//	XElement yField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "y"), new XAttribute("value", point.Y.ToString().Replace(',', '.')));
			//	XElement vertexObject = new XElement("object", new XAttribute("type", "float2"), xField, yField);
			//	XElement vertexElement = new XElement("element", new XAttribute("type", "float2"), vertexObject);
			//	bodyShapeVerticesArray.Add(vertexElement);
			//}

			XElement bodyShapeTypeField = new XElement("field", new XAttribute("type", "Physics2dShapeType"), new XAttribute("name", "type"), new XAttribute("value", "{enum Physics2dShapeType.Polygon}"));
			XElement bodyShapeVerticesField = new XElement("field", new XAttribute("type", "{array float2}"), new XAttribute("name", "vertices"), bodyShapeVerticesArray);
			XElement bodyShapeObject = new XElement("object", new XAttribute("type", "Physics2dComponentShapeInitData"), bodyShapeTypeField, bodyShapeVerticesField);
			XElement bodyShapeField = new XElement("field", new XAttribute("type", "Physics2dComponentShapeInitData"), new XAttribute("name", "shape"), bodyShapeObject);

			XElement bodyObject = new XElement("object", new XAttribute("type", "Physics2dBodyComponentInitData"), bodyDensityField, bodyFrictionField, bodyFreeRoationField, bodyMaterialField, bodyShapeField);
			XElement bodyTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Physics2dComponentType.Body}"));
			XElement bodyInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), bodyObject);
			XElement bodyComponent = new XElement("object", new XAttribute("type", "EntityComponent"), bodyTypeField, bodyInitDataField);
			XElement bodyElement = new XElement("element", new XAttribute("type", "EntityComponent"), bodyComponent);

			XElement wiggleObject = new XElement("object", new XAttribute("type", "WiggleComponentInitData"));
			XElement wiggleTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum MechanicaComponentType.Wiggle}"));
			XElement wiggleInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), wiggleObject);
			XElement wiggleComponent = new XElement("object", new XAttribute("type", "EntityComponent"), wiggleTypeField, wiggleInitDataField);
			XElement wiggleElement = new XElement("element", new XAttribute("type", "EntityComponent"), wiggleComponent);

			XElement componentsArray = new XElement("array", new XAttribute("type", "EntityComponent"), transformElement, textureElement, bodyElement, wiggleElement);
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