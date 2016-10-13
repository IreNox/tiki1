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

namespace Converter
{
	public partial class FormMain : Form
	{
		private class IslandFile
		{
			public string Name { get; set; }
			public string ColorFileName { get; set; }
			public string CollisionFileName { get; set; }
			public string DestinationFileName { get; set; }
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
					islandFile.DestinationFileName = Path.Combine(textDestination.Text, islandFile.Name + ".tikigenericobjects");
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

		private unsafe FarseerPhysics.Common.Vertices calculateVerticesFromCollision(string collisionFilename)
		{
			int width = 0;
			uint[] imageData = null;
			{
				Bitmap collisionImage = (Bitmap)Bitmap.FromFile(collisionFilename);
				BitmapData collisionData = collisionImage.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, collisionImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				uint* pCollisionRawData = (uint*)collisionData.Scan0.ToPointer();

				width = collisionImage.Width;
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

			int i = 1;
			while (textureVertices.Count > 100)
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

			//Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));

			return textureVertices;
		}

		private void convertFile(IslandFile file)
		{
			FarseerPhysics.Common.Vertices collisionVertices = calculateVerticesFromCollision(file.CollisionFileName);

			StringBuilder builder = new StringBuilder();

			XElement transformObject = new XElement("object", new XAttribute("type", "Transform2dComponentInitData"), new XAttribute("name", "component0_initdata"));
			XElement textureObject = new XElement("object", new XAttribute("type", "TextureComponentInitData"), new XAttribute("name", "component1_initdata"));
			XElement bodyObject = new XElement("object", new XAttribute("type", "Physics2dBodyComponentInitData"), new XAttribute("name", "component2_initdata"));

			XElement textureField = new XElement("field", new XAttribute("type", "{reference Texture}"), new XAttribute("name", "texture"), new XAttribute("value", "TEXR:..."));
			textureObject.Add(textureField);

			XElement bodyDensityField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "density"), new XAttribute("value", "10.0"));
			XElement bodyFrictionField = new XElement("field", new XAttribute("type", "float"), new XAttribute("name", "friction"), new XAttribute("value", "1.0"));
			XElement bodyFreeRoationField = new XElement("field", new XAttribute("type", "bool"), new XAttribute("name", "freeRotation"), new XAttribute("value", "true"));
			bodyObject.Add(bodyDensityField, bodyFrictionField, bodyFreeRoationField);

			XElement root = new XElement("tikigenericobjects", transformObject, textureObject, bodyObject);
			XDocument doc = new XDocument(root);

			string bla = doc.ToString();
		}
	}
}
