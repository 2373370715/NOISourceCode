using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020017FF RID: 6143
public class TechItem : Resource, IHasDlcRestrictions
{
	// Token: 0x06007E60 RID: 32352 RVA: 0x000F7AF0 File Offset: 0x000F5CF0
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x06007E61 RID: 32353 RVA: 0x000F7AF8 File Offset: 0x000F5CF8
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x06007E62 RID: 32354 RVA: 0x00336EF8 File Offset: 0x003350F8
	[Obsolete("Use constructor with requiredDlcIds and forbiddenDlcIds")]
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] dlcIds, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		DlcManager.ConvertAvailableToRequireAndForbidden(dlcIds, out this.requiredDlcIds, out this.forbiddenDlcIds);
	}

	// Token: 0x06007E63 RID: 32355 RVA: 0x00336F4C File Offset: 0x0033514C
	public TechItem(string id, ResourceSet parent, string name, string description, Func<string, bool, Sprite> getUISprite, string parentTechId, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, bool isPOIUnlock = false) : base(id, parent, name)
	{
		this.description = description;
		this.getUISprite = getUISprite;
		this.parentTechId = parentTechId;
		this.isPOIUnlock = isPOIUnlock;
		this.requiredDlcIds = requiredDlcIds;
		this.forbiddenDlcIds = forbiddenDlcIds;
	}

	// Token: 0x17000800 RID: 2048
	// (get) Token: 0x06007E64 RID: 32356 RVA: 0x000F7B00 File Offset: 0x000F5D00
	public Tech ParentTech
	{
		get
		{
			return Db.Get().Techs.Get(this.parentTechId);
		}
	}

	// Token: 0x06007E65 RID: 32357 RVA: 0x000F7B17 File Offset: 0x000F5D17
	public Sprite UISprite()
	{
		return this.getUISprite("ui", false);
	}

	// Token: 0x06007E66 RID: 32358 RVA: 0x000F7B2A File Offset: 0x000F5D2A
	public bool IsComplete()
	{
		return this.ParentTech.IsComplete() || this.IsPOIUnlocked();
	}

	// Token: 0x06007E67 RID: 32359 RVA: 0x00336FA0 File Offset: 0x003351A0
	private bool IsPOIUnlocked()
	{
		if (this.isPOIUnlock)
		{
			TechInstance techInstance = Research.Instance.Get(this.ParentTech);
			if (techInstance != null)
			{
				return techInstance.UnlockedPOITechIds.Contains(this.Id);
			}
		}
		return false;
	}

	// Token: 0x06007E68 RID: 32360 RVA: 0x00336FDC File Offset: 0x003351DC
	public void POIUnlocked()
	{
		DebugUtil.DevAssert(this.isPOIUnlock, "Trying to unlock tech item " + this.Id + " via POI and it's not marked as POI unlockable.", null);
		if (this.isPOIUnlock && !this.IsComplete())
		{
			Research.Instance.Get(this.ParentTech).UnlockPOITech(this.Id);
		}
	}

	// Token: 0x06007E69 RID: 32361 RVA: 0x00337038 File Offset: 0x00335238
	public void AddSearchTerms(List<string> newSearchTerms)
	{
		foreach (string item in newSearchTerms)
		{
			this.searchTerms.Add(item);
		}
	}

	// Token: 0x06007E6A RID: 32362 RVA: 0x000F7B41 File Offset: 0x000F5D41
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x0400600D RID: 24589
	public string description;

	// Token: 0x0400600E RID: 24590
	public Func<string, bool, Sprite> getUISprite;

	// Token: 0x0400600F RID: 24591
	public string parentTechId;

	// Token: 0x04006010 RID: 24592
	public bool isPOIUnlock;

	// Token: 0x04006011 RID: 24593
	[Obsolete("Use required/forbidden instead")]
	public string[] dlcIds;

	// Token: 0x04006012 RID: 24594
	public string[] requiredDlcIds;

	// Token: 0x04006013 RID: 24595
	public string[] forbiddenDlcIds;

	// Token: 0x04006014 RID: 24596
	public List<string> searchTerms = new List<string>();
}
