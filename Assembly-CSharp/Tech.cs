using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x020017FC RID: 6140
public class Tech : Resource
{
	// Token: 0x170007FB RID: 2043
	// (get) Token: 0x06007E49 RID: 32329 RVA: 0x000F79F2 File Offset: 0x000F5BF2
	public bool FoundNode
	{
		get
		{
			return this.node != null;
		}
	}

	// Token: 0x170007FC RID: 2044
	// (get) Token: 0x06007E4A RID: 32330 RVA: 0x000F79FD File Offset: 0x000F5BFD
	public Vector2 center
	{
		get
		{
			return this.node.center;
		}
	}

	// Token: 0x170007FD RID: 2045
	// (get) Token: 0x06007E4B RID: 32331 RVA: 0x000F7A0A File Offset: 0x000F5C0A
	public float width
	{
		get
		{
			return this.node.width;
		}
	}

	// Token: 0x170007FE RID: 2046
	// (get) Token: 0x06007E4C RID: 32332 RVA: 0x000F7A17 File Offset: 0x000F5C17
	public float height
	{
		get
		{
			return this.node.height;
		}
	}

	// Token: 0x170007FF RID: 2047
	// (get) Token: 0x06007E4D RID: 32333 RVA: 0x000F7A24 File Offset: 0x000F5C24
	public List<ResourceTreeNode.Edge> edges
	{
		get
		{
			return this.node.edges;
		}
	}

	// Token: 0x06007E4E RID: 32334 RVA: 0x003369E8 File Offset: 0x00334BE8
	public Tech(string id, List<string> unlockedItemIDs, Techs techs, Dictionary<string, float> overrideDefaultCosts = null) : base(id, techs, Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".NAME"))
	{
		this.desc = Strings.Get("STRINGS.RESEARCH.TECHS." + id.ToUpper() + ".DESC");
		this.unlockedItemIDs = unlockedItemIDs;
		if (overrideDefaultCosts != null && DlcManager.IsExpansion1Active())
		{
			foreach (KeyValuePair<string, float> keyValuePair in overrideDefaultCosts)
			{
				this.costsByResearchTypeID.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}

	// Token: 0x06007E4F RID: 32335 RVA: 0x00336AEC File Offset: 0x00334CEC
	public void AddUnlockedItemIDs(params string[] ids)
	{
		foreach (string item in ids)
		{
			this.unlockedItemIDs.Add(item);
		}
	}

	// Token: 0x06007E50 RID: 32336 RVA: 0x00336B1C File Offset: 0x00334D1C
	public void RemoveUnlockedItemIDs(params string[] ids)
	{
		foreach (string text in ids)
		{
			if (!this.unlockedItemIDs.Remove(text))
			{
				DebugUtil.DevLogError("Tech item '" + text + "' does not exist to remove");
			}
		}
	}

	// Token: 0x06007E51 RID: 32337 RVA: 0x000F7A31 File Offset: 0x000F5C31
	public bool RequiresResearchType(string type)
	{
		return this.costsByResearchTypeID.ContainsKey(type) && this.costsByResearchTypeID[type] > 0f;
	}

	// Token: 0x06007E52 RID: 32338 RVA: 0x000F7A56 File Offset: 0x000F5C56
	public void SetNode(ResourceTreeNode node, string categoryID)
	{
		this.node = node;
		this.category = categoryID;
	}

	// Token: 0x06007E53 RID: 32339 RVA: 0x00336B60 File Offset: 0x00334D60
	public bool CanAfford(ResearchPointInventory pointInventory)
	{
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			if (pointInventory.PointsByTypeID[keyValuePair.Key] < keyValuePair.Value)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06007E54 RID: 32340 RVA: 0x00336BD0 File Offset: 0x00334DD0
	public string CostString(ResearchTypes types)
	{
		string text = "";
		foreach (KeyValuePair<string, float> keyValuePair in this.costsByResearchTypeID)
		{
			text += string.Format("{0}:{1}", types.GetResearchType(keyValuePair.Key).name.ToString(), keyValuePair.Value.ToString());
			text += "\n";
		}
		return text;
	}

	// Token: 0x06007E55 RID: 32341 RVA: 0x00336C68 File Offset: 0x00334E68
	public bool IsComplete()
	{
		if (Research.Instance != null)
		{
			TechInstance techInstance = Research.Instance.Get(this);
			return techInstance != null && techInstance.IsComplete();
		}
		return false;
	}

	// Token: 0x06007E56 RID: 32342 RVA: 0x00336C9C File Offset: 0x00334E9C
	public bool ArePrerequisitesComplete()
	{
		using (List<Tech>.Enumerator enumerator = this.requiredTech.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsComplete())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06007E57 RID: 32343 RVA: 0x000F7A66 File Offset: 0x000F5C66
	public void AddSearchTerms(string newSearchTerms)
	{
		SearchUtil.AddCommaDelimitedSearchTerms(newSearchTerms, this.searchTerms);
	}

	// Token: 0x04005FFA RID: 24570
	public List<Tech> requiredTech = new List<Tech>();

	// Token: 0x04005FFB RID: 24571
	public List<Tech> unlockedTech = new List<Tech>();

	// Token: 0x04005FFC RID: 24572
	public List<TechItem> unlockedItems = new List<TechItem>();

	// Token: 0x04005FFD RID: 24573
	public List<string> unlockedItemIDs = new List<string>();

	// Token: 0x04005FFE RID: 24574
	public int tier;

	// Token: 0x04005FFF RID: 24575
	public Dictionary<string, float> costsByResearchTypeID = new Dictionary<string, float>();

	// Token: 0x04006000 RID: 24576
	public string desc;

	// Token: 0x04006001 RID: 24577
	public string category;

	// Token: 0x04006002 RID: 24578
	public List<string> searchTerms = new List<string>();

	// Token: 0x04006003 RID: 24579
	private ResourceTreeNode node;
}
