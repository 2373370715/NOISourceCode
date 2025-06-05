using System;

// Token: 0x02002067 RID: 8295
public class HatListable : IListableOption
{
	// Token: 0x0600B070 RID: 45168 RVA: 0x0011756B File Offset: 0x0011576B
	public HatListable(string name, string hat)
	{
		this.name = name;
		this.hat = hat;
	}

	// Token: 0x17000B4F RID: 2895
	// (get) Token: 0x0600B071 RID: 45169 RVA: 0x00117581 File Offset: 0x00115781
	// (set) Token: 0x0600B072 RID: 45170 RVA: 0x00117589 File Offset: 0x00115789
	public string name { get; private set; }

	// Token: 0x17000B50 RID: 2896
	// (get) Token: 0x0600B073 RID: 45171 RVA: 0x00117592 File Offset: 0x00115792
	// (set) Token: 0x0600B074 RID: 45172 RVA: 0x0011759A File Offset: 0x0011579A
	public string hat { get; private set; }

	// Token: 0x0600B075 RID: 45173 RVA: 0x001175A3 File Offset: 0x001157A3
	public string GetProperName()
	{
		return this.name;
	}
}
