using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001847 RID: 6215
public class Room : IAssignableIdentity
{
	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06007FEA RID: 32746 RVA: 0x000F8C58 File Offset: 0x000F6E58
	public List<KPrefabID> buildings
	{
		get
		{
			return this.cavity.buildings;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06007FEB RID: 32747 RVA: 0x000F8C65 File Offset: 0x000F6E65
	public List<KPrefabID> plants
	{
		get
		{
			return this.cavity.plants;
		}
	}

	// Token: 0x06007FEC RID: 32748 RVA: 0x000F8C72 File Offset: 0x000F6E72
	public string GetProperName()
	{
		return this.roomType.Name;
	}

	// Token: 0x06007FED RID: 32749 RVA: 0x0033E0A4 File Offset: 0x0033C2A4
	public List<Ownables> GetOwners()
	{
		this.current_owners.Clear();
		foreach (KPrefabID kprefabID in this.GetPrimaryEntities())
		{
			if (kprefabID != null)
			{
				Ownable component = kprefabID.GetComponent<Ownable>();
				if (component != null && component.assignee != null && component.assignee != this)
				{
					foreach (Ownables item in component.assignee.GetOwners())
					{
						if (!this.current_owners.Contains(item))
						{
							this.current_owners.Add(item);
						}
					}
				}
			}
		}
		return this.current_owners;
	}

	// Token: 0x06007FEE RID: 32750 RVA: 0x0033E190 File Offset: 0x0033C390
	public List<GameObject> GetBuildingsOnFloor()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.buildings.Count; i++)
		{
			if (!Grid.Solid[Grid.PosToCell(this.buildings[i])] && Grid.Solid[Grid.CellBelow(Grid.PosToCell(this.buildings[i]))])
			{
				list.Add(this.buildings[i].gameObject);
			}
		}
		return list;
	}

	// Token: 0x06007FEF RID: 32751 RVA: 0x0033E210 File Offset: 0x0033C410
	public Ownables GetSoleOwner()
	{
		List<Ownables> owners = this.GetOwners();
		if (owners.Count <= 0)
		{
			return null;
		}
		return owners[0];
	}

	// Token: 0x06007FF0 RID: 32752 RVA: 0x0033E238 File Offset: 0x0033C438
	public bool HasOwner(Assignables owner)
	{
		return this.GetOwners().Find((Ownables x) => x == owner) != null;
	}

	// Token: 0x06007FF1 RID: 32753 RVA: 0x000F8C7F File Offset: 0x000F6E7F
	public int NumOwners()
	{
		return this.GetOwners().Count;
	}

	// Token: 0x06007FF2 RID: 32754 RVA: 0x0033E270 File Offset: 0x0033C470
	public List<KPrefabID> GetPrimaryEntities()
	{
		this.primary_buildings.Clear();
		RoomType roomType = this.roomType;
		if (roomType.primary_constraint != null)
		{
			foreach (KPrefabID kprefabID in this.buildings)
			{
				if (kprefabID != null && roomType.primary_constraint.building_criteria(kprefabID))
				{
					this.primary_buildings.Add(kprefabID);
				}
			}
			foreach (KPrefabID kprefabID2 in this.plants)
			{
				if (kprefabID2 != null && roomType.primary_constraint.building_criteria(kprefabID2))
				{
					this.primary_buildings.Add(kprefabID2);
				}
			}
		}
		return this.primary_buildings;
	}

	// Token: 0x06007FF3 RID: 32755 RVA: 0x0033E36C File Offset: 0x0033C56C
	public void RetriggerBuildings()
	{
		foreach (KPrefabID kprefabID in this.buildings)
		{
			if (!(kprefabID == null))
			{
				kprefabID.Trigger(144050788, this);
			}
		}
		foreach (KPrefabID kprefabID2 in this.plants)
		{
			if (!(kprefabID2 == null))
			{
				kprefabID2.Trigger(144050788, this);
			}
		}
	}

	// Token: 0x06007FF4 RID: 32756 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsNull()
	{
		return false;
	}

	// Token: 0x06007FF5 RID: 32757 RVA: 0x000F8C8C File Offset: 0x000F6E8C
	public void CleanUp()
	{
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
	}

	// Token: 0x04006128 RID: 24872
	public CavityInfo cavity;

	// Token: 0x04006129 RID: 24873
	public RoomType roomType;

	// Token: 0x0400612A RID: 24874
	private List<KPrefabID> primary_buildings = new List<KPrefabID>();

	// Token: 0x0400612B RID: 24875
	private List<Ownables> current_owners = new List<Ownables>();
}
