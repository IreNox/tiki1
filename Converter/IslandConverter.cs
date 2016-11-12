using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TikiEngine;

namespace Converter
{
	internal class IslandFile
	{
		public string Name { get; set; }
		public string ColorFileName { get; set; }
		public string CollisionFileName { get; set; }
		public string GenericDataFileName { get; set; }
		public string GenericDataXassetFileName { get; set; }
		public string TextureFileName { get; set; }
		public string TextureInputFileName { get; set; }
		public string TextureXassetFileName { get; set; }
	}

	internal class IslandConverter
	{
		private string m_source;
		private string m_destination;

		private List<IslandFile> m_files = new List<IslandFile>();

		public IslandConverter()
		{
		}

		public void RescanFiles()
		{
			m_files = new List<IslandFile>();

			foreach (string file in Directory.GetFiles(m_source))
			{
				if (Path.GetExtension(file) != ".png")
				{
					continue;
				}
				string fileName = Path.GetFileNameWithoutExtension(file);
				string name = fileName.Substring(0, fileName.Length - 2).ToLower();

				IslandFile islandFile = m_files.FirstOrDefault(f => f.Name == name);
				if (islandFile == null)
				{
					islandFile = new IslandFile();
					m_files.Add(islandFile);

					islandFile.Name = name;
					islandFile.TextureFileName = islandFile.Name + ".texture";
					islandFile.GenericDataFileName = Path.Combine(m_destination, "genericdata\\entities\\islands", islandFile.Name + ".tikigenericobjects");
					islandFile.GenericDataXassetFileName = Path.Combine(m_destination, "genericdata\\entities\\islands", islandFile.Name + ".entity.xasset");
					islandFile.TextureInputFileName = Path.Combine(m_destination, "textures\\islands", islandFile.Name + ".png");
					islandFile.TextureXassetFileName = Path.Combine(m_destination, "textures\\islands", islandFile.Name + ".texture.xasset");
				}

				if (fileName.EndsWith("_n"))
				{
					islandFile.ColorFileName = file;
				}
				else
				{
					islandFile.CollisionFileName = file;
				}
			}

			m_files.RemoveAll(f => f.CollisionFileName == null || f.ColorFileName == null);
		}

		public void ConvertFiles()
		{
			foreach (IslandFile file in m_files)
			{
				writeTexture(file.ColorFileName, file.TextureInputFileName, file.TextureXassetFileName);
				writeGenericData(file.GenericDataFileName, file.GenericDataXassetFileName, file.CollisionFileName, file.TextureFileName);
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

		internal List<IslandFile> Files
		{
			get { return m_files; }
		}

		private unsafe FarseerPhysics.Common.Vertices calculateVerticesFromCollision(string collisionFilename, out Vector2 textureOffset)
		{
			int width = 0;
			int height = 0;
			uint[] imageData = null;
			{
				Bitmap collisionImage = (Bitmap)Bitmap.FromFile(collisionFilename);
				BitmapData collisionData = collisionImage.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, collisionImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				uint* pCollisionRawData = (uint*)collisionData.Scan0.ToPointer();

				width = collisionImage.Width;
				height = collisionImage.Height;
				imageData = new uint[collisionImage.Width * collisionImage.Height];
				for (int j = 0; j < imageData.Length; j++)
				{
					imageData[j] = pCollisionRawData[j];
				}
				collisionImage.UnlockBits(collisionData);
				collisionImage.Dispose();
			}

			FarseerPhysics.Common.Vertices textureVertices = FarseerPhysics.Common.PolygonTools.CreatePolygon(imageData, width, true);

			Vector2 centroid = -textureVertices.GetCentroid();
			textureVertices.Translate(ref centroid);

			textureOffset = -centroid;
			textureOffset.X -= width / 2.0f;
			textureOffset.Y -= height / 2.0f;
			textureOffset /= 100.0f;

			int i = 1;
			while (textureVertices.Count > 8)
			{
				if (i == 1)
				{
					textureVertices = SimplifyTools.CollinearSimplify(textureVertices);
				}
				else
				{
					textureVertices = SimplifyTools.ReduceByDistance(textureVertices, i);
				}

				i *= 2;
			}
			if (i != 1) textureVertices = SimplifyTools.CollinearSimplify(textureVertices);

			Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1.0));
			textureVertices.Scale(ref vertScale);

			return textureVertices;
		}

		private void writeGenericData(string genericFilename, string xassetFilename, string collisionFilename, string textureFilename)
		{
			Vector2 textureOffset;
			FarseerPhysics.Common.Vertices collisionVertices = calculateVerticesFromCollision(collisionFilename, out textureOffset);

			XElement transformObject = new XElement("object", new XAttribute("type", "Transform2dComponentInitData"));
			XElement transformTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Transform}"));
			XElement transformInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), transformObject);
			XElement transformComponent = new XElement("object", new XAttribute("type", "EntityComponent"), transformTypeField, transformInitDataField);
			XElement transformElement = new XElement("element", new XAttribute("type", "EntityComponent"), transformComponent);

			XElement textureOffsetXField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "x"), new XAttribute("value", textureOffset.X.ToString().Replace(',', '.')));
			XElement textureOffsetYField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "y"), new XAttribute("value", textureOffset.Y.ToString().Replace(',', '.')));
			XElement textureOffsetObject = new XElement("object", new XAttribute("type", "float2"), textureOffsetXField, textureOffsetYField);

			XElement spriteTextureField = new XElement("field", new XAttribute("type", "{reference Texture}"), new XAttribute("name", "texture"), new XAttribute("value", "TEXR:" + textureFilename));
			XElement spriteLayerField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "layerId"), new XAttribute("value", "{enum MechanicaRenderLayer.Islands}"));
			XElement spriteObject = new XElement("object", new XAttribute("type", "SpriteComponentInitData"), spriteLayerField);
			XElement spriteTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Sprite}"));
			XElement spriteInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), spriteObject);
			XElement spriteComponent = new XElement("object", new XAttribute("type", "EntityComponent"), spriteTypeField, spriteInitDataField);
			XElement spriteElement = new XElement("element", new XAttribute("type", "EntityComponent"), spriteComponent);

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

			XElement wiggleObject = new XElement("object", new XAttribute("type", "WiggleComponentInitData"));
			XElement wiggleTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum MechanicaComponentType.Wiggle}"));
			XElement wiggleInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), wiggleObject);
			XElement wiggleComponent = new XElement("object", new XAttribute("type", "EntityComponent"), wiggleTypeField, wiggleInitDataField);
			XElement wiggleElement = new XElement("element", new XAttribute("type", "EntityComponent"), wiggleComponent);

			XElement componentsArray = new XElement("array", new XAttribute("type", "EntityComponent"), transformElement, spriteElement, bodyElement, wiggleElement);
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

		private void writeTexture(string sourceFilename, string inputFilename, string xassetFilename)
		{
			Bitmap colorImage = (Bitmap)Image.FromFile(sourceFilename);

			//ResolutionInfo resInfo = new ResolutionInfo();
			//resInfo.WidthDisplayUnit = ResolutionInfo.Unit.Inches;
			//resInfo.HeightDisplayUnit = ResolutionInfo.Unit.Inches;
			//resInfo.HResDisplayUnit = ResolutionInfo.ResUnit.PxPerInch;
			//resInfo.VResDisplayUnit = ResolutionInfo.ResUnit.PxPerInch;
			//resInfo.HDpi = new UFixed16_16(72.0);
			//resInfo.VDpi = new UFixed16_16(72.0);

			//PsdFile psdFile = new PsdFile();
			//psdFile.RowCount = colorImage.Height;
			//psdFile.ColumnCount = colorImage.Width;
			//psdFile.ChannelCount = 3;
			//psdFile.ColorMode = PsdColorMode.RGB;
			//psdFile.BitDepth = 8;
			//psdFile.Resolution = resInfo;
			//psdFile.ImageCompression = ImageCompression.Rle;

			//int imageSize = psdFile.RowCount * psdFile.ColumnCount;
			//for (short i = 0; i < psdFile.ChannelCount; i++)
			//{
			//	Channel channel = new Channel(i, psdFile.BaseLayer);
			//	channel.ImageData = new byte[imageSize];
			//	channel.ImageCompression = psdFile.ImageCompression;
			//	psdFile.BaseLayer.Channels.Add(channel);
			//}

			//Channel[] channels = psdFile.BaseLayer.Channels.ToIdArray();
			//for (int y = 0; y < colorImage.Height; y++)
			//{
			//	int destRowIndex = y * colorImage.Width;
			//	for (int x = 0; x < colorImage.Width; x++)
			//	{
			//		int destIndex = destRowIndex + x;

			//		System.Drawing.Color color = colorImage.GetPixel(x, y);
			//		channels[0].ImageData[destIndex] = color.R;
			//		channels[1].ImageData[destIndex] = color.G;
			//		channels[2].ImageData[destIndex] = color.B;
			//		//channels[3].ImageData[destIndex] = color.A;
			//	}
			//}

			//psdFile.Save(inputFilename, Encoding.Default);
			colorImage.Save(inputFilename, ImageFormat.Png);

			XElement input = new XElement("input", new XAttribute("file", Path.GetFileName(inputFilename)), new XAttribute("type", "texture"));
			XElement root = new XElement("tikiasset", input);
			XDocument doc = new XDocument(root);
			File.WriteAllText(xassetFilename, doc.ToString());
		}
	}
}
