using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using PhotoshopFile;

namespace Converter
{
	public partial class FormMain : Form
	{
		private class IslandFile
		{
			public string Name { get; set; }
			public string ColorFileName { get; set; }
			public string CollisionFileName { get; set; }
			public string GenericDataFileName { get; set; }
			public string GenericDataXassetFileName { get; set; }
			public string TextureFileName { get; set; }
			public string TexturePsdFileName { get; set; }
			public string TextureXassetFileName { get; set; }
		}

		private List<IslandFile> m_files;

		public FormMain()
		{
			InitializeComponent();
			rescanFiles();
		}

		private void buttonBrowseSource_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textSource.Text;
			if( openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textSource.Text = openFileDialog.FileName;
				rescanFiles();
			}
		}

		private void buttonBrowseDestination_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textDestination.Text;
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textDestination.Text = openFileDialog.FileName;
				rescanFiles();
			}
		}

		private void buttonConvert_Click(object sender, EventArgs e)
		{
			foreach( IslandFile file in m_files)
			{
				convertFile(file);
			}
		}

		private void listFiles_SelectedIndexChanged(object sender, EventArgs e)
		{
			IslandFile file = listFiles.SelectedItem as IslandFile;
			if (file != null)
			{
				pictureBox.ImageLocation = file.ColorFileName;
			}
		}
		
		private void rescanFiles()
		{
			m_files = new List<IslandFile>();

			foreach (string file in Directory.GetFiles(textSource.Text))
			{
				if( Path.GetExtension(file) != ".png")
				{
					continue;
				}
				string fileName = Path.GetFileNameWithoutExtension(file);
				string name = fileName.Substring(0, fileName.Length - 2).ToLower();

				IslandFile islandFile = m_files.FirstOrDefault(f => f.Name == name);
				if(islandFile == null)
				{
					islandFile = new IslandFile();
					m_files.Add(islandFile);

					islandFile.Name = name;
					islandFile.TextureFileName = islandFile.Name + ".texture";
					islandFile.GenericDataFileName = Path.Combine(textDestination.Text, "genericdata\\entities", islandFile.Name + ".tikigenericobjects");
					islandFile.GenericDataXassetFileName = Path.Combine(textDestination.Text, "genericdata\\entities", islandFile.Name + ".entity.xasset");
					islandFile.TexturePsdFileName = Path.Combine(textDestination.Text, "textures\\islands", islandFile.Name + ".psd");
					islandFile.TextureXassetFileName = Path.Combine(textDestination.Text, "textures\\islands", islandFile.Name + ".texture.xasset");
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

			listFiles.DataSource = m_files;
			listFiles.DisplayMember = "Name";
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
			textureOffset /= -200.0f;

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

			Vector2 vertScale = new Vector2(UnitConverter.ToSimUnits(1.0));
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

			XElement textureTextureField = new XElement("field", new XAttribute("type", "{reference Texture}"), new XAttribute("name", "texture"), new XAttribute("value", "TEXR:" + textureFilename));
			XElement textureOffsetField = new XElement("field", new XAttribute("type", "float2"), new XAttribute("name", "offset"), textureOffsetObject);
			XElement textureLayerField = new XElement("field", new XAttribute("type", "uint32"), new XAttribute("name", "layerId"), new XAttribute("value", "5"));
			XElement textureObject = new XElement("object", new XAttribute("type", "TextureComponentInitData"), textureTextureField, textureOffsetField, textureLayerField);
			XElement textureTypeField = new XElement("field", new XAttribute("type", "crc32"), new XAttribute("name", "componentType"), new XAttribute("value", "{enum Components2dType.Texture}"));
			XElement textureInitDataField = new XElement("field", new XAttribute("type", "{pointer ComponentInitData}"), new XAttribute("name", "initData"), textureObject);
			XElement textureComponent = new XElement("object", new XAttribute("type", "EntityComponent"), textureTypeField, textureInitDataField);
			XElement textureElement = new XElement("element", new XAttribute("type", "EntityComponent"), textureComponent);

			XElement bodyDensityField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "density"), new XAttribute("value", "10.0"));
			XElement bodyFrictionField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "friction"), new XAttribute("value", "1.0"));
			XElement bodyFreeRoationField = new XElement("field", new XAttribute("type", "bool"), new XAttribute("name", "freeRotation"), new XAttribute("value", "true"));

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

			XElement bodyObject = new XElement("object", new XAttribute("type", "Physics2dBodyComponentInitData"), bodyDensityField, bodyFrictionField, bodyFreeRoationField, bodyShapeField);
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

		private void writeTexture(string sourceFilename, string psdFilename, string xassetFilename)
		{
			Bitmap colorImage = (Bitmap)Image.FromFile(sourceFilename);

			ResolutionInfo resInfo = new ResolutionInfo();
			resInfo.WidthDisplayUnit = ResolutionInfo.Unit.Inches;
			resInfo.HeightDisplayUnit = ResolutionInfo.Unit.Inches;
			resInfo.HResDisplayUnit = ResolutionInfo.ResUnit.PxPerInch;
			resInfo.VResDisplayUnit = ResolutionInfo.ResUnit.PxPerInch;
			resInfo.HDpi = new UFixed16_16(72.0);
			resInfo.VDpi = new UFixed16_16(72.0);

			PsdFile psdFile = new PsdFile();
			psdFile.RowCount = colorImage.Height;
			psdFile.ColumnCount = colorImage.Width;
			psdFile.ChannelCount = 3;
			psdFile.ColorMode = PsdColorMode.RGB;
			psdFile.BitDepth = 8;
			psdFile.Resolution = resInfo;
			psdFile.ImageCompression = ImageCompression.Rle;

			int imageSize = psdFile.RowCount * psdFile.ColumnCount;
			for (short i = 0; i < psdFile.ChannelCount; i++)
			{
				Channel channel = new Channel(i, psdFile.BaseLayer);
				channel.ImageData = new byte[imageSize];
				channel.ImageCompression = psdFile.ImageCompression;
				psdFile.BaseLayer.Channels.Add(channel);
			}

			Channel[] channels = psdFile.BaseLayer.Channels.ToIdArray();
			for (int y = 0; y < colorImage.Height; y++)
			{
				int destRowIndex = y * colorImage.Width;
				for (int x = 0; x < colorImage.Width; x++)
				{
					int destIndex = destRowIndex + x;

					System.Drawing.Color color = colorImage.GetPixel(x, y);
					channels[0].ImageData[destIndex] = color.R;
					channels[1].ImageData[destIndex] = color.G;
					channels[2].ImageData[destIndex] = color.B;
					//channels[3].ImageData[destIndex] = color.A;
				}
			}

			psdFile.Save(psdFilename, Encoding.Default);

			XElement input = new XElement("input", new XAttribute("file", Path.GetFileName(psdFilename)), new XAttribute("type", "texture"));
			XElement root = new XElement("tikiasset", input);
			XDocument doc = new XDocument(root);
			File.WriteAllText(xassetFilename, doc.ToString());
		}

		private void convertFile(IslandFile file)
		{
			//writeTexture(file.ColorFileName, file.TexturePsdFileName, file.TextureXassetFileName);
			writeGenericData(file.GenericDataFileName, file.GenericDataXassetFileName, file.CollisionFileName, file.TextureFileName);
		}
	}
}
