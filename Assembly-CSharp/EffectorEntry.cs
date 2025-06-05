using System;
using STRINGS;

// Token: 0x020012B0 RID: 4784
internal struct EffectorEntry
{
	// Token: 0x060061C4 RID: 25028 RVA: 0x000E42CF File Offset: 0x000E24CF
	public EffectorEntry(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x060061C5 RID: 25029 RVA: 0x002C27E4 File Offset: 0x002C09E4
	public override string ToString()
	{
		string arg = "";
		if (this.count > 1)
		{
			arg = string.Format(UI.OVERLAYS.DECOR.COUNT, this.count);
		}
		return string.Format(UI.OVERLAYS.DECOR.ENTRY, GameUtil.GetFormattedDecor(this.value, false), this.name, arg);
	}

	// Token: 0x040045DB RID: 17883
	public string name;

	// Token: 0x040045DC RID: 17884
	public int count;

	// Token: 0x040045DD RID: 17885
	public float value;
}
