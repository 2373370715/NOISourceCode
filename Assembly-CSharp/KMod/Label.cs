using System;
using System.Diagnostics;
using System.IO;
using Klei;
using Newtonsoft.Json;

namespace KMod
{
	// Token: 0x0200224C RID: 8780
	[JsonObject(MemberSerialization.Fields)]
	[DebuggerDisplay("{title}")]
	public struct Label
	{
		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x0600BA6B RID: 47723 RVA: 0x0011C8E3 File Offset: 0x0011AAE3
		[JsonIgnore]
		private string distribution_platform_name
		{
			get
			{
				return this.distribution_platform.ToString();
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x0600BA6C RID: 47724 RVA: 0x0011C8F6 File Offset: 0x0011AAF6
		[JsonIgnore]
		public string install_path
		{
			get
			{
				return FileSystem.Normalize(Path.Combine(Manager.GetDirectory(), this.distribution_platform_name, this.id));
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x0600BA6D RID: 47725 RVA: 0x0011C913 File Offset: 0x0011AB13
		[JsonIgnore]
		public string defaultStaticID
		{
			get
			{
				return this.id + "." + this.distribution_platform.ToString();
			}
		}

		// Token: 0x0600BA6E RID: 47726 RVA: 0x0011C936 File Offset: 0x0011AB36
		public override string ToString()
		{
			return this.title;
		}

		// Token: 0x0600BA6F RID: 47727 RVA: 0x0011C93E File Offset: 0x0011AB3E
		public bool Match(Label rhs)
		{
			return this.id == rhs.id && this.distribution_platform == rhs.distribution_platform;
		}

		// Token: 0x040098D2 RID: 39122
		public Label.DistributionPlatform distribution_platform;

		// Token: 0x040098D3 RID: 39123
		public string id;

		// Token: 0x040098D4 RID: 39124
		public string title;

		// Token: 0x040098D5 RID: 39125
		public long version;

		// Token: 0x0200224D RID: 8781
		public enum DistributionPlatform
		{
			// Token: 0x040098D7 RID: 39127
			Local,
			// Token: 0x040098D8 RID: 39128
			Steam,
			// Token: 0x040098D9 RID: 39129
			Epic,
			// Token: 0x040098DA RID: 39130
			Rail,
			// Token: 0x040098DB RID: 39131
			Dev
		}
	}
}
