using System;
using System.IO;
using Klei;
using STRINGS;

namespace KMod
{
	// Token: 0x02002242 RID: 8770
	public class Local : IDistributionPlatform
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x0600BA3E RID: 47678 RVA: 0x0011C7AA File Offset: 0x0011A9AA
		// (set) Token: 0x0600BA3F RID: 47679 RVA: 0x0011C7B2 File Offset: 0x0011A9B2
		public string folder { get; private set; }

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600BA40 RID: 47680 RVA: 0x0011C7BB File Offset: 0x0011A9BB
		// (set) Token: 0x0600BA41 RID: 47681 RVA: 0x0011C7C3 File Offset: 0x0011A9C3
		public Label.DistributionPlatform distribution_platform { get; private set; }

		// Token: 0x0600BA42 RID: 47682 RVA: 0x0011C7CC File Offset: 0x0011A9CC
		public string GetDirectory()
		{
			return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.folder));
		}

		// Token: 0x0600BA43 RID: 47683 RVA: 0x0047E45C File Offset: 0x0047C65C
		private void Subscribe(string directoryName, long timestamp, IFileSource file_source, bool isDevMod)
		{
			Label label = new Label
			{
				id = directoryName,
				distribution_platform = this.distribution_platform,
				version = (long)directoryName.GetHashCode(),
				title = directoryName
			};
			KModHeader header = KModUtil.GetHeader(file_source, label.defaultStaticID, directoryName, directoryName, isDevMod);
			label.title = header.title;
			Mod mod = new Mod(label, header.staticID, header.description, file_source, UI.FRONTEND.MODS.TOOLTIPS.MANAGE_LOCAL_MOD, delegate()
			{
				App.OpenWebURL("file://" + file_source.GetRoot());
			});
			if (file_source.GetType() == typeof(Directory))
			{
				mod.status = Mod.Status.Installed;
			}
			Global.Instance.modManager.Subscribe(mod, this);
		}

		// Token: 0x0600BA44 RID: 47684 RVA: 0x0047E530 File Offset: 0x0047C730
		public Local(string folder, Label.DistributionPlatform distribution_platform, bool isDevFolder)
		{
			this.folder = folder;
			this.distribution_platform = distribution_platform;
			DirectoryInfo directoryInfo = new DirectoryInfo(this.GetDirectory());
			if (!directoryInfo.Exists)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				this.Subscribe(name, directoryInfo2.LastWriteTime.ToFileTime(), new Directory(directoryInfo2.FullName), isDevFolder);
			}
		}
	}
}
