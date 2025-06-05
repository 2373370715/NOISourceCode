using System;
using System.Collections.Generic;
using Klei;

namespace KMod
{
	// Token: 0x02002248 RID: 8776
	public interface IFileSource
	{
		// Token: 0x0600BA4C RID: 47692
		string GetRoot();

		// Token: 0x0600BA4D RID: 47693
		bool Exists();

		// Token: 0x0600BA4E RID: 47694
		bool Exists(string relative_path);

		// Token: 0x0600BA4F RID: 47695
		void GetTopLevelItems(List<FileSystemItem> file_system_items, string relative_root = "");

		// Token: 0x0600BA50 RID: 47696
		IFileDirectory GetFileSystem();

		// Token: 0x0600BA51 RID: 47697
		void CopyTo(string path, List<string> extensions = null);

		// Token: 0x0600BA52 RID: 47698
		string Read(string relative_path);

		// Token: 0x0600BA53 RID: 47699
		void Dispose();
	}
}
