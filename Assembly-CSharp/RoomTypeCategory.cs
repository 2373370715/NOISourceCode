using System;

// Token: 0x02000BC3 RID: 3011
public class RoomTypeCategory : Resource
{
	// Token: 0x17000284 RID: 644
	// (get) Token: 0x060038E4 RID: 14564 RVA: 0x000C94A4 File Offset: 0x000C76A4
	// (set) Token: 0x060038E5 RID: 14565 RVA: 0x000C94AC File Offset: 0x000C76AC
	public string colorName { get; private set; }

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x060038E6 RID: 14566 RVA: 0x000C94B5 File Offset: 0x000C76B5
	// (set) Token: 0x060038E7 RID: 14567 RVA: 0x000C94BD File Offset: 0x000C76BD
	public string icon { get; private set; }

	// Token: 0x060038E8 RID: 14568 RVA: 0x000C94C6 File Offset: 0x000C76C6
	public RoomTypeCategory(string id, string name, string colorName, string icon) : base(id, name)
	{
		this.colorName = colorName;
		this.icon = icon;
	}
}
