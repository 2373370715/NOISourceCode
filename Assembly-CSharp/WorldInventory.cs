using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x02001AAE RID: 6830
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/WorldInventory")]
public class WorldInventory : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x06008ED7 RID: 36567 RVA: 0x00101C11 File Offset: 0x000FFE11
	public WorldContainer WorldContainer
	{
		get
		{
			if (this.m_worldContainer == null)
			{
				this.m_worldContainer = base.GetComponent<WorldContainer>();
			}
			return this.m_worldContainer;
		}
	}

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x00101C33 File Offset: 0x000FFE33
	public bool HasValidCount
	{
		get
		{
			return this.hasValidCount;
		}
	}

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06008ED9 RID: 36569 RVA: 0x0037BF28 File Offset: 0x0037A128
	private int worldId
	{
		get
		{
			WorldContainer worldContainer = this.WorldContainer;
			if (!(worldContainer != null))
			{
				return -1;
			}
			return worldContainer.id;
		}
	}

	// Token: 0x06008EDA RID: 36570 RVA: 0x0037BF50 File Offset: 0x0037A150
	protected override void OnPrefabInit()
	{
		base.Subscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Subscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.Subscribe<WorldInventory>(631075836, WorldInventory.OnNewDayDelegate);
		this.m_worldContainer = base.GetComponent<WorldContainer>();
	}

	// Token: 0x06008EDB RID: 36571 RVA: 0x0037BFC0 File Offset: 0x0037A1C0
	protected override void OnCleanUp()
	{
		base.Unsubscribe(Game.Instance.gameObject, -1588644844, new Action<object>(this.OnAddedFetchable));
		base.Unsubscribe(Game.Instance.gameObject, -1491270284, new Action<object>(this.OnRemovedFetchable));
		base.OnCleanUp();
	}

	// Token: 0x06008EDC RID: 36572 RVA: 0x0037C018 File Offset: 0x0037A218
	private void GenerateInventoryReport(object data)
	{
		int num = 0;
		int num2 = 0;
		foreach (Brain brain in Components.Brains.GetWorldItems(this.worldId, false))
		{
			CreatureBrain creatureBrain = brain as CreatureBrain;
			if (creatureBrain != null)
			{
				if (creatureBrain.HasTag(GameTags.Creatures.Wild))
				{
					num++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.WildCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
				else
				{
					num2++;
					ReportManager.Instance.ReportValue(ReportManager.ReportType.DomesticatedCritters, 1f, creatureBrain.GetProperName(), creatureBrain.GetProperName());
				}
			}
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer component = base.GetComponent<WorldContainer>();
			if (component != null && component.IsModuleInterior)
			{
				Clustercraft clustercraft = component.GetComponent<ClusterGridEntity>() as Clustercraft;
				if (clustercraft != null && clustercraft.Status != Clustercraft.CraftStatus.Grounded)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, clustercraft.Name, null);
					return;
				}
			}
		}
		else
		{
			foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
			{
				if (spacecraft.state != Spacecraft.MissionState.Grounded && spacecraft.state != Spacecraft.MissionState.Destroyed)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.RocketsInFlight, 1f, spacecraft.rocketName, null);
				}
			}
		}
	}

	// Token: 0x06008EDD RID: 36573 RVA: 0x00101C3B File Offset: 0x000FFE3B
	protected override void OnSpawn()
	{
		this.Prober = MinionGroupProber.Get();
		base.StartCoroutine(this.InitialRefresh());
	}

	// Token: 0x06008EDE RID: 36574 RVA: 0x00101C55 File Offset: 0x000FFE55
	private IEnumerator InitialRefresh()
	{
		int num;
		for (int i = 0; i < 1; i = num)
		{
			yield return null;
			num = i + 1;
		}
		for (int j = 0; j < Components.Pickupables.Count; j++)
		{
			Pickupable pickupable = Components.Pickupables[j];
			if (pickupable != null)
			{
				ReachabilityMonitor.Instance smi = pickupable.GetSMI<ReachabilityMonitor.Instance>();
				if (smi != null)
				{
					smi.UpdateReachability();
				}
			}
		}
		yield break;
	}

	// Token: 0x06008EDF RID: 36575 RVA: 0x00101C5D File Offset: 0x000FFE5D
	public bool IsReachable(Pickupable pickupable)
	{
		return this.Prober.IsReachable(pickupable);
	}

	// Token: 0x06008EE0 RID: 36576 RVA: 0x0037C1A8 File Offset: 0x0037A3A8
	public float GetTotalAmount(Tag tag, bool includeRelatedWorlds)
	{
		float result = 0f;
		this.accessibleAmounts.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x06008EE1 RID: 36577 RVA: 0x0037C1CC File Offset: 0x0037A3CC
	public ICollection<Pickupable> GetPickupables(Tag tag, bool includeRelatedWorlds = false)
	{
		if (!includeRelatedWorlds)
		{
			HashSet<Pickupable> result = null;
			this.Inventory.TryGetValue(tag, out result);
			return result;
		}
		return ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
	}

	// Token: 0x06008EE2 RID: 36578 RVA: 0x0037C1F8 File Offset: 0x0037A3F8
	public List<Pickupable> CreatePickupablesList(Tag tag)
	{
		HashSet<Pickupable> hashSet = null;
		this.Inventory.TryGetValue(tag, out hashSet);
		if (hashSet == null)
		{
			return null;
		}
		return hashSet.ToList<Pickupable>();
	}

	// Token: 0x06008EE3 RID: 36579 RVA: 0x0037C224 File Offset: 0x0037A424
	public float GetAmount(Tag tag, bool includeRelatedWorlds)
	{
		float num;
		if (!includeRelatedWorlds)
		{
			num = this.GetTotalAmount(tag, includeRelatedWorlds);
			num -= MaterialNeeds.GetAmount(tag, this.worldId, includeRelatedWorlds);
		}
		else
		{
			num = ClusterUtil.GetAmountFromRelatedWorlds(this, tag);
		}
		return Mathf.Max(num, 0f);
	}

	// Token: 0x06008EE4 RID: 36580 RVA: 0x0037C26C File Offset: 0x0037A46C
	public int GetCountWithAdditionalTag(Tag tag, Tag additionalTag, bool includeRelatedWorlds = false)
	{
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		int num = 0;
		if (collection2 != null)
		{
			if (additionalTag.IsValid)
			{
				using (IEnumerator<Pickupable> enumerator = collection2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasTag(additionalTag))
						{
							num++;
						}
					}
					return num;
				}
			}
			num = collection2.Count;
		}
		return num;
	}

	// Token: 0x06008EE5 RID: 36581 RVA: 0x0037C2E8 File Offset: 0x0037A4E8
	public float GetAmountWithoutTag(Tag tag, bool includeRelatedWorlds = false, Tag[] forbiddenTags = null)
	{
		if (forbiddenTags == null)
		{
			return this.GetAmount(tag, includeRelatedWorlds);
		}
		float num = 0f;
		ICollection<Pickupable> collection;
		if (!includeRelatedWorlds)
		{
			collection = this.GetPickupables(tag, false);
		}
		else
		{
			ICollection<Pickupable> pickupablesFromRelatedWorlds = ClusterUtil.GetPickupablesFromRelatedWorlds(this, tag);
			collection = pickupablesFromRelatedWorlds;
		}
		ICollection<Pickupable> collection2 = collection;
		if (collection2 != null)
		{
			foreach (Pickupable pickupable in collection2)
			{
				if (pickupable != null && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate) && !pickupable.KPrefabID.HasAnyTags(forbiddenTags))
				{
					num += pickupable.TotalAmount;
				}
			}
		}
		return num;
	}

	// Token: 0x06008EE6 RID: 36582 RVA: 0x0037C390 File Offset: 0x0037A590
	private void Update()
	{
		int num = 0;
		Dictionary<Tag, HashSet<Pickupable>>.Enumerator enumerator = this.Inventory.GetEnumerator();
		int worldId = this.worldId;
		while (enumerator.MoveNext())
		{
			KeyValuePair<Tag, HashSet<Pickupable>> keyValuePair = enumerator.Current;
			if (num == this.accessibleUpdateIndex || this.firstUpdate)
			{
				Tag key = keyValuePair.Key;
				IEnumerable<Pickupable> value = keyValuePair.Value;
				float num2 = 0f;
				foreach (Pickupable pickupable in value)
				{
					if (pickupable != null && pickupable.GetMyWorldId() == worldId && !pickupable.KPrefabID.HasTag(GameTags.StoredPrivate))
					{
						num2 += pickupable.TotalAmount;
					}
				}
				if (!this.hasValidCount && this.accessibleUpdateIndex + 1 >= this.Inventory.Count)
				{
					this.hasValidCount = true;
					if (this.worldId == ClusterManager.Instance.activeWorldId)
					{
						this.hasValidCount = true;
						PinnedResourcesPanel.Instance.ClearExcessiveNewItems();
						PinnedResourcesPanel.Instance.Refresh();
					}
				}
				this.accessibleAmounts[key] = num2;
				this.accessibleUpdateIndex = (this.accessibleUpdateIndex + 1) % this.Inventory.Count;
				break;
			}
			num++;
		}
		this.firstUpdate = false;
	}

	// Token: 0x06008EE7 RID: 36583 RVA: 0x000F5860 File Offset: 0x000F3A60
	protected override void OnLoadLevel()
	{
		base.OnLoadLevel();
	}

	// Token: 0x06008EE8 RID: 36584 RVA: 0x0037C4EC File Offset: 0x0037A6EC
	private void OnAddedFetchable(object data)
	{
		GameObject gameObject = (GameObject)data;
		KPrefabID component = gameObject.GetComponent<KPrefabID>();
		if (component.HasAnyTags(WorldInventory.NonCritterEntitiesTags))
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2.GetMyWorldId() != this.worldId)
		{
			return;
		}
		Tag tag = component.PrefabID();
		if (!this.Inventory.ContainsKey(tag))
		{
			Tag categoryForEntity = DiscoveredResources.GetCategoryForEntity(component);
			DebugUtil.DevAssertArgs(categoryForEntity.IsValid, new object[]
			{
				component2.name,
				"was found by worldinventory but doesn't have a category! Add it to the element definition."
			});
			DiscoveredResources.Instance.Discover(tag, categoryForEntity);
		}
		HashSet<Pickupable> hashSet;
		if (!this.Inventory.TryGetValue(tag, out hashSet))
		{
			hashSet = new HashSet<Pickupable>();
			this.Inventory[tag] = hashSet;
		}
		hashSet.Add(component2);
		foreach (Tag key in component.Tags)
		{
			if (!this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet = new HashSet<Pickupable>();
				this.Inventory[key] = hashSet;
			}
			hashSet.Add(component2);
		}
	}

	// Token: 0x06008EE9 RID: 36585 RVA: 0x0037C618 File Offset: 0x0037A818
	private void OnRemovedFetchable(object data)
	{
		Pickupable component = ((GameObject)data).GetComponent<Pickupable>();
		KPrefabID kprefabID = component.KPrefabID;
		HashSet<Pickupable> hashSet;
		if (this.Inventory.TryGetValue(kprefabID.PrefabTag, out hashSet))
		{
			hashSet.Remove(component);
		}
		foreach (Tag key in kprefabID.Tags)
		{
			if (this.Inventory.TryGetValue(key, out hashSet))
			{
				hashSet.Remove(component);
			}
		}
	}

	// Token: 0x06008EEA RID: 36586 RVA: 0x00101C6B File Offset: 0x000FFE6B
	public Dictionary<Tag, float> GetAccessibleAmounts()
	{
		return this.accessibleAmounts;
	}

	// Token: 0x04006BA6 RID: 27558
	private WorldContainer m_worldContainer;

	// Token: 0x04006BA7 RID: 27559
	[Serialize]
	public List<Tag> pinnedResources = new List<Tag>();

	// Token: 0x04006BA8 RID: 27560
	[Serialize]
	public List<Tag> notifyResources = new List<Tag>();

	// Token: 0x04006BA9 RID: 27561
	private Dictionary<Tag, HashSet<Pickupable>> Inventory = new Dictionary<Tag, HashSet<Pickupable>>();

	// Token: 0x04006BAA RID: 27562
	private MinionGroupProber Prober;

	// Token: 0x04006BAB RID: 27563
	private Dictionary<Tag, float> accessibleAmounts = new Dictionary<Tag, float>();

	// Token: 0x04006BAC RID: 27564
	private bool hasValidCount;

	// Token: 0x04006BAD RID: 27565
	private static readonly EventSystem.IntraObjectHandler<WorldInventory> OnNewDayDelegate = new EventSystem.IntraObjectHandler<WorldInventory>(delegate(WorldInventory component, object data)
	{
		component.GenerateInventoryReport(data);
	});

	// Token: 0x04006BAE RID: 27566
	private int accessibleUpdateIndex;

	// Token: 0x04006BAF RID: 27567
	private bool firstUpdate = true;

	// Token: 0x04006BB0 RID: 27568
	private static Tag[] NonCritterEntitiesTags = new Tag[]
	{
		GameTags.DupeBrain,
		GameTags.Robot
	};
}
