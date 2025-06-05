using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000BC0 RID: 3008
[DebuggerDisplay("{IdHash}")]
public class ChoreType : Resource
{
	// Token: 0x1700027F RID: 639
	// (get) Token: 0x060038D7 RID: 14551 RVA: 0x000C9441 File Offset: 0x000C7641
	// (set) Token: 0x060038D8 RID: 14552 RVA: 0x000C9449 File Offset: 0x000C7649
	public Urge urge { get; private set; }

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x060038D9 RID: 14553 RVA: 0x000C9452 File Offset: 0x000C7652
	// (set) Token: 0x060038DA RID: 14554 RVA: 0x000C945A File Offset: 0x000C765A
	public ChoreGroup[] groups { get; private set; }

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x060038DB RID: 14555 RVA: 0x000C9463 File Offset: 0x000C7663
	// (set) Token: 0x060038DC RID: 14556 RVA: 0x000C946B File Offset: 0x000C766B
	public int priority { get; private set; }

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x060038DD RID: 14557 RVA: 0x000C9474 File Offset: 0x000C7674
	// (set) Token: 0x060038DE RID: 14558 RVA: 0x000C947C File Offset: 0x000C767C
	public int interruptPriority { get; set; }

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x060038DF RID: 14559 RVA: 0x000C9485 File Offset: 0x000C7685
	// (set) Token: 0x060038E0 RID: 14560 RVA: 0x000C948D File Offset: 0x000C768D
	public int explicitPriority { get; private set; }

	// Token: 0x060038E1 RID: 14561 RVA: 0x000C9496 File Offset: 0x000C7696
	private string ResolveStringCallback(string str, object data)
	{
		return ((Chore)data).ResolveString(str);
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x00229DB4 File Offset: 0x00227FB4
	public ChoreType(string id, ResourceSet parent, string[] chore_groups, string urge, string name, string status_message, string tooltip, IEnumerable<Tag> interrupt_exclusion, int implicit_priority, int explicit_priority) : base(id, parent, name)
	{
		this.statusItem = new StatusItem(id, status_message, tooltip, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
		this.statusItem.resolveStringCallback = new Func<string, object, string>(this.ResolveStringCallback);
		this.tags.Add(TagManager.Create(id));
		this.interruptExclusion = new HashSet<Tag>(interrupt_exclusion);
		Db.Get().DuplicantStatusItems.Add(this.statusItem);
		List<ChoreGroup> list = new List<ChoreGroup>();
		for (int i = 0; i < chore_groups.Length; i++)
		{
			ChoreGroup choreGroup = Db.Get().ChoreGroups.TryGet(chore_groups[i]);
			if (choreGroup != null)
			{
				if (!choreGroup.choreTypes.Contains(this))
				{
					choreGroup.choreTypes.Add(this);
				}
				list.Add(choreGroup);
			}
		}
		this.groups = list.ToArray();
		if (!string.IsNullOrEmpty(urge))
		{
			this.urge = Db.Get().Urges.Get(urge);
		}
		this.priority = implicit_priority;
		this.explicitPriority = explicit_priority;
	}

	// Token: 0x04002753 RID: 10067
	public StatusItem statusItem;

	// Token: 0x04002758 RID: 10072
	public HashSet<Tag> tags = new HashSet<Tag>();

	// Token: 0x04002759 RID: 10073
	public HashSet<Tag> interruptExclusion;

	// Token: 0x0400275B RID: 10075
	public string reportName;
}
