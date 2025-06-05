using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Klei;
using UnityEngine;

namespace KMod
{
	// Token: 0x02002249 RID: 8777
	internal struct Directory : IFileSource
	{
		// Token: 0x0600BA54 RID: 47700 RVA: 0x0011C816 File Offset: 0x0011AA16
		public Directory(string root)
		{
			this.root = root;
			this.file_system = new AliasDirectory(root, root, Application.streamingAssetsPath, true);
		}

		// Token: 0x0600BA55 RID: 47701 RVA: 0x0011C832 File Offset: 0x0011AA32
		public string GetRoot()
		{
			return this.root;
		}

		// Token: 0x0600BA56 RID: 47702 RVA: 0x0011C83A File Offset: 0x0011AA3A
		public bool Exists()
		{
			return Directory.Exists(this.GetRoot());
		}

		// Token: 0x0600BA57 RID: 47703 RVA: 0x0011C847 File Offset: 0x0011AA47
		public bool Exists(string relative_path)
		{
			return this.Exists() && new DirectoryInfo(FileSystem.Normalize(Path.Combine(this.root, relative_path))).Exists;
		}

		// Token: 0x0600BA58 RID: 47704 RVA: 0x0047E910 File Offset: 0x0047CB10
		public void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root)
		{
			relative_root = (relative_root ?? "");
			string text = FileSystem.Normalize(Path.Combine(this.root, relative_root));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			if (!directoryInfo.Exists)
			{
				global::Debug.LogError("Cannot iterate over $" + text + ", this directory does not exist");
				return;
			}
			foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
			{
				file_system_items.Add(new FileSystemItem
				{
					name = fileSystemInfo.Name,
					type = ((fileSystemInfo is DirectoryInfo) ? FileSystemItem.ItemType.Directory : FileSystemItem.ItemType.File)
				});
			}
		}

		// Token: 0x0600BA59 RID: 47705 RVA: 0x0011C86E File Offset: 0x0011AA6E
		public IFileDirectory GetFileSystem()
		{
			return this.file_system;
		}

		// Token: 0x0600BA5A RID: 47706 RVA: 0x0047E9AC File Offset: 0x0047CBAC
		public void CopyTo(string path, List<string> extensions = null)
		{
			try
			{
				Directory.CopyDirectory(this.root, path, extensions);
			}
			catch (UnauthorizedAccessException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.UnauthorizedAccess, path, null, null);
			}
			catch (IOException)
			{
				FileUtil.ErrorDialog(FileUtil.ErrorType.IOError, path, null, null);
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x0600BA5B RID: 47707 RVA: 0x0047EA0C File Offset: 0x0047CC0C
		public string Read(string relative_path)
		{
			string result;
			try
			{
				using (FileStream fileStream = File.OpenRead(Path.Combine(this.root, relative_path)))
				{
					byte[] array = new byte[fileStream.Length];
					fileStream.Read(array, 0, (int)fileStream.Length);
					result = Encoding.UTF8.GetString(array);
				}
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x0600BA5C RID: 47708 RVA: 0x0047EA88 File Offset: 0x0047CC88
		private static int CopyDirectory(string sourceDirName, string destDirName, List<string> extensions)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			if (!directoryInfo.Exists)
			{
				return 0;
			}
			if (!FileUtil.CreateDirectory(destDirName, 0))
			{
				return 0;
			}
			FileInfo[] files = directoryInfo.GetFiles();
			int num = 0;
			foreach (FileInfo fileInfo in files)
			{
				bool flag = extensions == null || extensions.Count == 0;
				if (extensions != null)
				{
					using (List<string>.Enumerator enumerator = extensions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == Path.GetExtension(fileInfo.Name).ToLower())
							{
								flag = true;
								break;
							}
						}
					}
				}
				if (flag)
				{
					string destFileName = Path.Combine(destDirName, fileInfo.Name);
					fileInfo.CopyTo(destFileName, false);
					num++;
				}
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
				num += Directory.CopyDirectory(directoryInfo2.FullName, destDirName2, extensions);
			}
			if (num == 0)
			{
				FileUtil.DeleteDirectory(destDirName, 0);
			}
			return num;
		}

		// Token: 0x0600BA5D RID: 47709 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Dispose()
		{
		}

		// Token: 0x040098CB RID: 39115
		private AliasDirectory file_system;

		// Token: 0x040098CC RID: 39116
		private string root;
	}
}
