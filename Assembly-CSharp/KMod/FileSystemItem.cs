using System;

namespace KMod
{
	// Token: 0x02002246 RID: 8774
	public struct FileSystemItem
	{
		// Token: 0x040098C6 RID: 39110
		public string name;

		// Token: 0x040098C7 RID: 39111
		public FileSystemItem.ItemType type;

		// Token: 0x02002247 RID: 8775
		public enum ItemType
		{
			// Token: 0x040098C9 RID: 39113
			Directory,
			// Token: 0x040098CA RID: 39114
			File
		}
	}
}
