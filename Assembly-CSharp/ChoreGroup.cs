using System;
using System.Collections.Generic;
using System.Diagnostics;
using Klei.AI;

// Token: 0x02000BBF RID: 3007
[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
	// Token: 0x1700027E RID: 638
	// (get) Token: 0x060038D5 RID: 14549 RVA: 0x000C9439 File Offset: 0x000C7639
	public int DefaultPersonalPriority
	{
		get
		{
			return this.defaultPersonalPriority;
		}
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x00229D50 File Offset: 0x00227F50
	public ChoreGroup(string id, string name, Klei.AI.Attribute attribute, string sprite, int default_personal_priority, bool user_prioritizable = true) : base(id, name)
	{
		this.attribute = attribute;
		this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
		this.sprite = sprite;
		this.defaultPersonalPriority = default_personal_priority;
		this.userPrioritizable = user_prioritizable;
	}

	// Token: 0x0400274D RID: 10061
	public List<ChoreType> choreTypes = new List<ChoreType>();

	// Token: 0x0400274E RID: 10062
	public Klei.AI.Attribute attribute;

	// Token: 0x0400274F RID: 10063
	public string description;

	// Token: 0x04002750 RID: 10064
	public string sprite;

	// Token: 0x04002751 RID: 10065
	private int defaultPersonalPriority;

	// Token: 0x04002752 RID: 10066
	public bool userPrioritizable;
}
