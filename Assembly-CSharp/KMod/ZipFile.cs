using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x0200224A RID: 8778
	internal struct ZipFile : IFileSource
	{
		// Token: 0x0600BA5E RID: 47710 RVA: 0x0011C876 File Offset: 0x0011AA76
		public ZipFile(string filename)
		{
			this.filename = filename;
			this.zipfile = ZipFile.Read(filename);
			this.file_system = new ZipFileDirectory(this.zipfile.Name, this.zipfile, Application.streamingAssetsPath, true);
		}

		// Token: 0x0600BA5F RID: 47711 RVA: 0x0011C8AD File Offset: 0x0011AAAD
		public string GetRoot()
		{
			return this.filename;
		}

		// Token: 0x0600BA60 RID: 47712 RVA: 0x0011C8B5 File Offset: 0x0011AAB5
		public bool Exists()
		{
			return File.Exists(this.GetRoot());
		}

		// Token: 0x0600BA61 RID: 47713 RVA: 0x0047EBAC File Offset: 0x0047CDAC
		public bool Exists(string relative_path)
		{
			if (!this.Exists())
			{
				return false;
			}
			using (IEnumerator<ZipEntry> enumerator = this.zipfile.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (FileSystem.Normalize(enumerator.Current.FileName).StartsWith(relative_path))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BA62 RID: 47714 RVA: 0x0047EC14 File Offset: 0x0047CE14
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			HashSetPool<string, ZipFile>.PooledHashSet pooledHashSet = HashSetPool<string, ZipFile>.Allocate();
			string[] array;
			if (!string.IsNullOrEmpty(relative_root))
			{
				relative_root = (relative_root ?? "");
				relative_root = FileSystem.Normalize(relative_root);
				array = relative_root.Split('/', StringSplitOptions.None);
			}
			else
			{
				array = new string[0];
			}
			foreach (ZipEntry zipEntry in this.zipfile)
			{
				List<string> list = (from part in FileSystem.Normalize(zipEntry.FileName).Split('/', StringSplitOptions.None)
				where !string.IsNullOrEmpty(part)
				select part).ToList<string>();
				if (this.IsSharedRoot(array, list))
				{
					list = list.GetRange(array.Length, list.Count - array.Length);
					if (list.Count != 0)
					{
						string text = list[0];
						if (pooledHashSet.Add(text))
						{
							file_system_items.Add(new FileSystemItem
							{
								name = text,
								type = ((1 < list.Count) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
							});
						}
					}
				}
			}
			pooledHashSet.Recycle();
		}

		// Token: 0x0600BA63 RID: 47715 RVA: 0x0047ED3C File Offset: 0x0047CF3C
		private bool IsSharedRoot(string[] root_path, List<string> check_path)
		{
			for (int i = 0; i < root_path.Length; i++)
			{
				if (i >= check_path.Count || root_path[i] != check_path[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BA64 RID: 47716 RVA: 0x0011C8C2 File Offset: 0x0011AAC2
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x0600BA65 RID: 47717 RVA: 0x0047ED74 File Offset: 0x0047CF74
		public void CopyTo(string path, List<string> extensions = null)
		{
			foreach (ZipEntry zipEntry in this.zipfile.Entries)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					foreach (string value in extensions)
					{
						if (zipEntry.FileName.ToLower().EndsWith(value))
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					string path2 = FileSystem.Normalize(Path.Combine(path, zipEntry.FileName));
					string directoryName = Path.GetDirectoryName(path2);
					if (string.IsNullOrEmpty(directoryName) || FileUtil.CreateDirectory(directoryName, 0))
					{
						using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
						{
							zipEntry.Extract(memoryStream);
							using (FileStream fileStream = FileUtil.Create(path2, 0))
							{
								fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600BA66 RID: 47718 RVA: 0x0047EEC4 File Offset: 0x0047D0C4
		public string Read(string relative_path)
		{
			ICollection<ZipEntry> collection = this.zipfile.SelectEntries(relative_path);
			if (collection.Count == 0)
			{
				return string.Empty;
			}
			foreach (ZipEntry zipEntry in collection)
			{
				using (MemoryStream memoryStream = new MemoryStream((int)zipEntry.UncompressedSize))
				{
					zipEntry.Extract(memoryStream);
					return Encoding.UTF8.GetString(memoryStream.GetBuffer());
				}
			}
			return string.Empty;
		}

		// Token: 0x0600BA67 RID: 47719 RVA: 0x0011C8CA File Offset: 0x0011AACA
		public void Dispose()
		{
			this.zipfile.Dispose();
		}

		// Token: 0x040098CD RID: 39117
		private string filename;

		// Token: 0x040098CE RID: 39118
		private ZipFile zipfile;

		// Token: 0x040098CF RID: 39119
		private ZipFileDirectory file_system;
	}
}
