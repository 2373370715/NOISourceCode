using System;
using System.Collections.Generic;

// Token: 0x02001846 RID: 6214
public class RoleSlotUnlock
{
	// Token: 0x1700081F RID: 2079
	// (get) Token: 0x06007FDF RID: 32735 RVA: 0x000F8BD6 File Offset: 0x000F6DD6
	// (set) Token: 0x06007FE0 RID: 32736 RVA: 0x000F8BDE File Offset: 0x000F6DDE
	public string id { get; protected set; }

	// Token: 0x17000820 RID: 2080
	// (get) Token: 0x06007FE1 RID: 32737 RVA: 0x000F8BE7 File Offset: 0x000F6DE7
	// (set) Token: 0x06007FE2 RID: 32738 RVA: 0x000F8BEF File Offset: 0x000F6DEF
	public string name { get; protected set; }

	// Token: 0x17000821 RID: 2081
	// (get) Token: 0x06007FE3 RID: 32739 RVA: 0x000F8BF8 File Offset: 0x000F6DF8
	// (set) Token: 0x06007FE4 RID: 32740 RVA: 0x000F8C00 File Offset: 0x000F6E00
	public string description { get; protected set; }

	// Token: 0x17000822 RID: 2082
	// (get) Token: 0x06007FE5 RID: 32741 RVA: 0x000F8C09 File Offset: 0x000F6E09
	// (set) Token: 0x06007FE6 RID: 32742 RVA: 0x000F8C11 File Offset: 0x000F6E11
	public List<global::Tuple<string, int>> slots { get; protected set; }

	// Token: 0x17000823 RID: 2083
	// (get) Token: 0x06007FE7 RID: 32743 RVA: 0x000F8C1A File Offset: 0x000F6E1A
	// (set) Token: 0x06007FE8 RID: 32744 RVA: 0x000F8C22 File Offset: 0x000F6E22
	public Func<bool> isSatisfied { get; protected set; }

	// Token: 0x06007FE9 RID: 32745 RVA: 0x000F8C2B File Offset: 0x000F6E2B
	public RoleSlotUnlock(string id, string name, string description, List<global::Tuple<string, int>> slots, Func<bool> isSatisfied)
	{
		this.id = id;
		this.name = name;
		this.description = description;
		this.slots = slots;
		this.isSatisfied = isSatisfied;
	}
}
