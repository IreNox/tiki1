using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
	}
}
