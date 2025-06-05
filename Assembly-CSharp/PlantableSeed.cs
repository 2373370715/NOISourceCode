using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020011F7 RID: 4599
[AddComponentMenu("KMonoBehaviour/scripts/PlantableSeed")]
public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
	// Token: 0x17000586 RID: 1414
	// (get) Token: 0x06005D6F RID: 23919 RVA: 0x000E1619 File Offset: 0x000DF819
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06005D70 RID: 23920 RVA: 0x000E1621 File Offset: 0x000DF821
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
	}

	// Token: 0x06005D71 RID: 23921 RVA: 0x000E163E File Offset: 0x000DF83E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.PlantableSeeds.Add(this);
	}

	// Token: 0x06005D72 RID: 23922 RVA: 0x000E1651 File Offset: 0x000DF851
	protected override void OnCleanUp()
	{
		Components.PlantableSeeds.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005D73 RID: 23923 RVA: 0x002ACB7C File Offset: 0x002AAD7C
	public void TryPlant(bool allow_plant_from_storage = false)
	{
		this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
		if (!allow_plant_from_storage && base.gameObject.HasTag(GameTags.Stored))
		{
			return;
		}
		int cell = Grid.PosToCell(base.gameObject);
		if (this.TestSuitableGround(cell))
		{
			Vector3 position = Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), position, Grid.SceneLayer.BuildingFront, null, 0);
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			if (component != null)
			{
				base.GetComponent<MutantPlant>().CopyMutationsTo(component);
			}
			gameObject.SetActive(true);
			Pickupable pickupable = this.pickupable.Take(1f);
			if (pickupable != null)
			{
				gameObject.GetComponent<Crop>() != null;
				Util.KDestroyGameObject(pickupable.gameObject);
				return;
			}
			KCrashReporter.Assert(false, "Seed has fractional total amount < 1f", null);
		}
	}

	// Token: 0x06005D74 RID: 23924 RVA: 0x002ACC50 File Offset: 0x002AAE50
	public bool TestSuitableGround(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		int num;
		if (this.Direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			num = Grid.CellAbove(cell);
		}
		else
		{
			num = Grid.CellBelow(cell);
		}
		if (!Grid.IsValidCell(num))
		{
			return false;
		}
		if (Grid.Foundation[num])
		{
			return false;
		}
		if (Grid.Element[num].hardness >= 150)
		{
			return false;
		}
		if (this.replantGroundTag.IsValid && !Grid.Element[num].HasTag(this.replantGroundTag))
		{
			return false;
		}
		GameObject prefab = Assets.GetPrefab(this.PlantID);
		EntombVulnerable component = prefab.GetComponent<EntombVulnerable>();
		if (component != null && !component.IsCellSafe(cell))
		{
			return false;
		}
		DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
		if (component2 != null && !component2.IsCellSafe(cell))
		{
			return false;
		}
		TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
		if (component3 != null && !component3.IsCellSafe(cell) && Grid.Element[cell].id != SimHashes.Vacuum)
		{
			return false;
		}
		UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
		if (component4 != null && !component4.IsSuitableFoundation(cell))
		{
			return false;
		}
		OccupyArea component5 = prefab.GetComponent<OccupyArea>();
		return !(component5 != null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
	}

	// Token: 0x06005D75 RID: 23925 RVA: 0x002ACD84 File Offset: 0x002AAF84
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
		{
			Descriptor item = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement, false);
			list.Add(item);
		}
		else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
		{
			Descriptor item2 = new Descriptor(UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x04004299 RID: 17049
	public Tag PlantID;

	// Token: 0x0400429A RID: 17050
	public Tag PreviewID;

	// Token: 0x0400429B RID: 17051
	[Serialize]
	public float timeUntilSelfPlant;

	// Token: 0x0400429C RID: 17052
	public Tag replantGroundTag;

	// Token: 0x0400429D RID: 17053
	public string domesticatedDescription;

	// Token: 0x0400429E RID: 17054
	public SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x0400429F RID: 17055
	[MyCmpGet]
	private Pickupable pickupable;
}
