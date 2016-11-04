using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Text;
using System.Xml.Linq;
using PhotoshopFile;

namespace Converter
{
	public partial class FormMain : Form
	{
		private IslandConverter m_islandConverter = new IslandConverter();
		private BreakableConverter m_breakableConverter = new BreakableConverter();

		public FormMain()
		{
			InitializeComponent();
		}

		private void tabControl_Selected(object sender, TabControlEventArgs e)
		{
			if( e.TabPage == tabBreakables)
			{
				rescanBreakableFiles();
			}
			else
			{
				rescanIslandFiles();
			}
		}
		
		private void buttonBreakableBrowseSource_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textBreakableSource.Text;
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textBreakableSource.Text = openFileDialog.FileName;
				rescanBreakableFiles();
			}
		}

		private void buttonBreakableBrowseDestination_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textBreakableDestination.Text;
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textBreakableDestination.Text = openFileDialog.FileName;
				rescanBreakableFiles();
			}

		}

		private void listBreakables_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void buttonBreakableConvert_Click(object sender, EventArgs e)
		{
			m_breakableConverter.ConvertFiles();
		}

		private void buttonIslandBrowseSource_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textIslandSource.Text;
			if( openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textIslandSource.Text = openFileDialog.FileName;
				rescanIslandFiles();
			}
		}

		private void buttonIslandBrowseDestination_Click(object sender, EventArgs e)
		{
			openFileDialog.InitialDirectory = textIslandDestination.Text;
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				textIslandDestination.Text = openFileDialog.FileName;
				rescanIslandFiles();
			}
		}

		private void buttonIslandConvert_Click(object sender, EventArgs e)
		{
			m_islandConverter.ConvertFiles();
		}

		private void listIslands_SelectedIndexChanged(object sender, EventArgs e)
		{
			IslandFile file = listIslands.SelectedItem as IslandFile;
			if (file != null)
			{
				pictureIsland.ImageLocation = file.ColorFileName;
			}
		}

		private void rescanBreakableFiles()
		{
			m_breakableConverter.Source = textIslandSource.Text;
			m_breakableConverter.Destination = textIslandDestination.Text;

			m_breakableConverter.RescanFiles();

			listBreakables.DataSource = m_breakableConverter.Files;
			listBreakables.DisplayMember = "Name";
		}

		private void rescanIslandFiles()
		{
			m_islandConverter.Source = textIslandSource.Text;
			m_islandConverter.Destination = textIslandDestination.Text;

			m_islandConverter.RescanFiles();

			listIslands.DataSource = m_islandConverter.Files;
			listIslands.DisplayMember = "Name";
		}
	}
}
