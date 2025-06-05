using System;

// Token: 0x020012B1 RID: 4785
internal struct EffectorEntryDecibel
{
	// Token: 0x060061C6 RID: 25030 RVA: 0x000E42E6 File Offset: 0x000E24E6
	public EffectorEntryDecibel(string name, float value)
	{
		this.name = name;
		this.value = value;
		this.count = 1;
	}

	// Token: 0x040045DE RID: 17886
	public string name;

	// Token: 0x040045DF RID: 17887
	public int count;

	// Token: 0x040045E0 RID: 17888
	public float value;
}
