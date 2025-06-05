using System;

// Token: 0x02001313 RID: 4883
public class Expectation
{
	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x0600640A RID: 25610 RVA: 0x000E5C13 File Offset: 0x000E3E13
	// (set) Token: 0x0600640B RID: 25611 RVA: 0x000E5C1B File Offset: 0x000E3E1B
	public string id { get; protected set; }

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x0600640C RID: 25612 RVA: 0x000E5C24 File Offset: 0x000E3E24
	// (set) Token: 0x0600640D RID: 25613 RVA: 0x000E5C2C File Offset: 0x000E3E2C
	public string name { get; protected set; }

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x0600640E RID: 25614 RVA: 0x000E5C35 File Offset: 0x000E3E35
	// (set) Token: 0x0600640F RID: 25615 RVA: 0x000E5C3D File Offset: 0x000E3E3D
	public string description { get; protected set; }

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06006410 RID: 25616 RVA: 0x000E5C46 File Offset: 0x000E3E46
	// (set) Token: 0x06006411 RID: 25617 RVA: 0x000E5C4E File Offset: 0x000E3E4E
	public Action<MinionResume> OnApply { get; protected set; }

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06006412 RID: 25618 RVA: 0x000E5C57 File Offset: 0x000E3E57
	// (set) Token: 0x06006413 RID: 25619 RVA: 0x000E5C5F File Offset: 0x000E3E5F
	public Action<MinionResume> OnRemove { get; protected set; }

	// Token: 0x06006414 RID: 25620 RVA: 0x000E5C68 File Offset: 0x000E3E68
	public Expectation(string id, string name, string description, Action<MinionResume> OnApply, Action<MinionResume> OnRemove)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.OnApply = OnApply;
		this.OnRemove = OnRemove;
	}
}
