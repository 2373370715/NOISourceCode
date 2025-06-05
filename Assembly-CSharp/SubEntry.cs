using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001C7C RID: 7292
public class SubEntry : IHasDlcRestrictions
{
	// Token: 0x060097BF RID: 38847 RVA: 0x001074EB File Offset: 0x001056EB
	public SubEntry()
	{
	}

	// Token: 0x060097C0 RID: 38848 RVA: 0x003B6120 File Offset: 0x003B4320
	public SubEntry(string id, string parentEntryID, List<ContentContainer> contentContainers, string name)
	{
		this.id = id;
		this.parentEntryID = parentEntryID;
		this.name = name;
		this.contentContainers = contentContainers;
		if (!string.IsNullOrEmpty(this.lockID))
		{
			foreach (ContentContainer contentContainer in contentContainers)
			{
				contentContainer.lockID = this.lockID;
			}
		}
		if (string.IsNullOrEmpty(this.sortString))
		{
			if (!string.IsNullOrEmpty(this.title))
			{
				this.sortString = UI.StripLinkFormatting(this.title);
				return;
			}
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x170009D3 RID: 2515
	// (get) Token: 0x060097C1 RID: 38849 RVA: 0x001074FE File Offset: 0x001056FE
	// (set) Token: 0x060097C2 RID: 38850 RVA: 0x00107506 File Offset: 0x00105706
	public List<ContentContainer> contentContainers { get; set; }

	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060097C3 RID: 38851 RVA: 0x0010750F File Offset: 0x0010570F
	// (set) Token: 0x060097C4 RID: 38852 RVA: 0x00107517 File Offset: 0x00105717
	public string parentEntryID { get; set; }

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060097C5 RID: 38853 RVA: 0x00107520 File Offset: 0x00105720
	// (set) Token: 0x060097C6 RID: 38854 RVA: 0x00107528 File Offset: 0x00105728
	public string id { get; set; }

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060097C7 RID: 38855 RVA: 0x00107531 File Offset: 0x00105731
	// (set) Token: 0x060097C8 RID: 38856 RVA: 0x00107539 File Offset: 0x00105739
	public string name { get; set; }

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060097C9 RID: 38857 RVA: 0x00107542 File Offset: 0x00105742
	// (set) Token: 0x060097CA RID: 38858 RVA: 0x0010754A File Offset: 0x0010574A
	public string title { get; set; }

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x060097CB RID: 38859 RVA: 0x00107553 File Offset: 0x00105753
	// (set) Token: 0x060097CC RID: 38860 RVA: 0x0010755B File Offset: 0x0010575B
	public string subtitle { get; set; }

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x060097CD RID: 38861 RVA: 0x00107564 File Offset: 0x00105764
	// (set) Token: 0x060097CE RID: 38862 RVA: 0x0010756C File Offset: 0x0010576C
	public Sprite icon { get; set; }

	// Token: 0x170009DA RID: 2522
	// (get) Token: 0x060097CF RID: 38863 RVA: 0x00107575 File Offset: 0x00105775
	// (set) Token: 0x060097D0 RID: 38864 RVA: 0x0010757D File Offset: 0x0010577D
	public int layoutPriority { get; set; }

	// Token: 0x170009DB RID: 2523
	// (get) Token: 0x060097D1 RID: 38865 RVA: 0x00107586 File Offset: 0x00105786
	// (set) Token: 0x060097D2 RID: 38866 RVA: 0x0010758E File Offset: 0x0010578E
	public bool disabled { get; set; }

	// Token: 0x170009DC RID: 2524
	// (get) Token: 0x060097D3 RID: 38867 RVA: 0x00107597 File Offset: 0x00105797
	// (set) Token: 0x060097D4 RID: 38868 RVA: 0x0010759F File Offset: 0x0010579F
	public string lockID { get; set; }

	// Token: 0x170009DD RID: 2525
	// (get) Token: 0x060097D5 RID: 38869 RVA: 0x001075A8 File Offset: 0x001057A8
	// (set) Token: 0x060097D6 RID: 38870 RVA: 0x001075B0 File Offset: 0x001057B0
	public string[] requiredDlcIds { get; set; }

	// Token: 0x170009DE RID: 2526
	// (get) Token: 0x060097D7 RID: 38871 RVA: 0x001075B9 File Offset: 0x001057B9
	// (set) Token: 0x060097D8 RID: 38872 RVA: 0x001075C1 File Offset: 0x001057C1
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x060097D9 RID: 38873 RVA: 0x001075CA File Offset: 0x001057CA
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x060097DA RID: 38874 RVA: 0x001075D2 File Offset: 0x001057D2
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x170009DF RID: 2527
	// (get) Token: 0x060097DB RID: 38875 RVA: 0x001075DA File Offset: 0x001057DA
	// (set) Token: 0x060097DC RID: 38876 RVA: 0x001075E2 File Offset: 0x001057E2
	public string sortString { get; set; }

	// Token: 0x0400762E RID: 30254
	public ContentContainer lockedContentContainer;

	// Token: 0x04007635 RID: 30261
	public Color iconColor = Color.white;
}
