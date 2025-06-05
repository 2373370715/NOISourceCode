using System;

namespace Klei.CustomSettings
{
	// Token: 0x02003C4F RID: 15439
	public class SettingLevel
	{
		// Token: 0x0600EC76 RID: 60534 RVA: 0x0014339D File Offset: 0x0014159D
		public SettingLevel(string id, string label, string tooltip, long coordinate_value = 0L, object userdata = null)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.userdata = userdata;
			this.coordinate_value = coordinate_value;
		}

		// Token: 0x17000C25 RID: 3109
		// (get) Token: 0x0600EC77 RID: 60535 RVA: 0x001433CA File Offset: 0x001415CA
		// (set) Token: 0x0600EC78 RID: 60536 RVA: 0x001433D2 File Offset: 0x001415D2
		public string id { get; private set; }

		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x0600EC79 RID: 60537 RVA: 0x001433DB File Offset: 0x001415DB
		// (set) Token: 0x0600EC7A RID: 60538 RVA: 0x001433E3 File Offset: 0x001415E3
		public string tooltip { get; private set; }

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x0600EC7B RID: 60539 RVA: 0x001433EC File Offset: 0x001415EC
		// (set) Token: 0x0600EC7C RID: 60540 RVA: 0x001433F4 File Offset: 0x001415F4
		public string label { get; private set; }

		// Token: 0x17000C28 RID: 3112
		// (get) Token: 0x0600EC7D RID: 60541 RVA: 0x001433FD File Offset: 0x001415FD
		// (set) Token: 0x0600EC7E RID: 60542 RVA: 0x00143405 File Offset: 0x00141605
		public object userdata { get; private set; }

		// Token: 0x17000C29 RID: 3113
		// (get) Token: 0x0600EC7F RID: 60543 RVA: 0x0014340E File Offset: 0x0014160E
		// (set) Token: 0x0600EC80 RID: 60544 RVA: 0x00143416 File Offset: 0x00141616
		public long coordinate_value { get; private set; }
	}
}
