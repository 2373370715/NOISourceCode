using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000BC6 RID: 3014
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06003909 RID: 14601 RVA: 0x000C95BC File Offset: 0x000C77BC
	// (set) Token: 0x0600390A RID: 14602 RVA: 0x000C95C4 File Offset: 0x000C77C4
	public Color color { get; private set; }

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600390B RID: 14603 RVA: 0x000C95CD File Offset: 0x000C77CD
	// (set) Token: 0x0600390C RID: 14604 RVA: 0x000C95D5 File Offset: 0x000C77D5
	public string description { get; private set; }

	// Token: 0x0600390D RID: 14605 RVA: 0x000C95DE File Offset: 0x000C77DE
	public ScheduleBlockType(string id, ResourceSet parent, string name, string description, Color color) : base(id, parent, name)
	{
		this.color = color;
		this.description = description;
	}
}
