using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Converter
{
	internal class BreakableFile
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
				if (Path.GetExtension(file) != ".png")
				{
					continue;
				}
				string fileName = Path.GetFileNameWithoutExtension(file);
				string name = fileName.Substring(0, fileName.Length - 2).ToLower();

				BreakableFile islandFile = m_files.FirstOrDefault(f => f.Name == name);
				if (islandFile == null)
				{
					islandFile = new BreakableFile();
					m_files.Add(islandFile);

					islandFile.Name = name;
					islandFile.TextureFileName = islandFile.Name + ".texture";
					islandFile.GenericDataFileName = Path.Combine(m_destination, "genericdata\\entities", islandFile.Name + ".tikigenericobjects");
					islandFile.GenericDataXassetFileName = Path.Combine(m_destination, "genericdata\\entities", islandFile.Name + ".entity.xasset");
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
			foreach (BreakableFile file in m_files)
			{
				//writeTexture(file.ColorFileName, file.TextureInputFileName, file.TextureXassetFileName);
				//writeGenericData(file.GenericDataFileName, file.GenericDataXassetFileName, file.CollisionFileName, file.TextureFileName);
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
	}
}