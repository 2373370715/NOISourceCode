using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020017F7 RID: 6135
public class ResearchPointInventory
{
	// Token: 0x06007E35 RID: 32309 RVA: 0x003363DC File Offset: 0x003345DC
	public ResearchPointInventory()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			this.PointsByTypeID.Add(researchType.id, 0f);
		}
	}

	// Token: 0x06007E36 RID: 32310 RVA: 0x00336458 File Offset: 0x00334658
	public void AddResearchPoints(string researchTypeID, float points)
	{
		if (!this.PointsByTypeID.ContainsKey(researchTypeID))
		{
			Debug.LogWarning("Research inventory is missing research point key " + researchTypeID);
			return;
		}
		Dictionary<string, float> pointsByTypeID = this.PointsByTypeID;
		pointsByTypeID[researchTypeID] += points;
	}

	// Token: 0x06007E37 RID: 32311 RVA: 0x000F7964 File Offset: 0x000F5B64
	public void RemoveResearchPoints(string researchTypeID, float points)
	{
		this.AddResearchPoints(researchTypeID, -points);
	}

	// Token: 0x06007E38 RID: 32312 RVA: 0x003364A0 File Offset: 0x003346A0
	[OnDeserialized]
	private void OnDeserialized()
	{
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (!this.PointsByTypeID.ContainsKey(researchType.id))
			{
				this.PointsByTypeID.Add(researchType.id, 0f);
			}
		}
	}

	// Token: 0x04005FEC RID: 24556
	public Dictionary<string, float> PointsByTypeID = new Dictionary<string, float>();
}
