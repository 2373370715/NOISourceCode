using System;

// Token: 0x020007F9 RID: 2041
public struct NavOffset
{
	// Token: 0x060023F6 RID: 9206 RVA: 0x000BBD03 File Offset: 0x000B9F03
	public NavOffset(NavType nav_type, int x, int y)
	{
		this.navType = nav_type;
		this.offset.x = x;
		this.offset.y = y;
	}

	// Token: 0x04001855 RID: 6229
	public NavType navType;

	// Token: 0x04001856 RID: 6230
	public CellOffset offset;
}
