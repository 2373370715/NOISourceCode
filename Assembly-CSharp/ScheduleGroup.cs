using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

// Token: 0x02000BC7 RID: 3015
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	// Token: 0x17000295 RID: 661
	// (get) Token: 0x0600390E RID: 14606 RVA: 0x000C95F9 File Offset: 0x000C77F9
	// (set) Token: 0x0600390F RID: 14607 RVA: 0x000C9601 File Offset: 0x000C7801
	public int defaultSegments { get; private set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06003910 RID: 14608 RVA: 0x000C960A File Offset: 0x000C780A
	// (set) Token: 0x06003911 RID: 14609 RVA: 0x000C9612 File Offset: 0x000C7812
	public string description { get; private set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06003912 RID: 14610 RVA: 0x000C961B File Offset: 0x000C781B
	// (set) Token: 0x06003913 RID: 14611 RVA: 0x000C9623 File Offset: 0x000C7823
	public string notificationTooltip { get; private set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06003914 RID: 14612 RVA: 0x000C962C File Offset: 0x000C782C
	// (set) Token: 0x06003915 RID: 14613 RVA: 0x000C9634 File Offset: 0x000C7834
	public List<ScheduleBlockType> allowedTypes { get; private set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06003916 RID: 14614 RVA: 0x000C963D File Offset: 0x000C783D
	// (set) Token: 0x06003917 RID: 14615 RVA: 0x000C9645 File Offset: 0x000C7845
	public bool alarm { get; private set; }

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x000C964E File Offset: 0x000C784E
	// (set) Token: 0x06003919 RID: 14617 RVA: 0x000C9656 File Offset: 0x000C7856
	public Color uiColor { get; private set; }

	// Token: 0x0600391A RID: 14618 RVA: 0x000C965F File Offset: 0x000C785F
	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, Color uiColor, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
		this.uiColor = uiColor;
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x000C969A File Offset: 0x000C789A
	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x000C96A8 File Offset: 0x000C78A8
	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
