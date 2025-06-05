using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F53 RID: 3923
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantablePlot : SingleEntityReceptacle, ISaveLoadable, IGameObjectEffectDescriptor
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06004E93 RID: 20115 RVA: 0x000D76E1 File Offset: 0x000D58E1
	// (set) Token: 0x06004E94 RID: 20116 RVA: 0x000D76EE File Offset: 0x000D58EE
	public KPrefabID plant
	{
		get
		{
			return this.plantRef.Get();
		}
		set
		{
			this.plantRef.Set(value);
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06004E95 RID: 20117 RVA: 0x000D76FC File Offset: 0x000D58FC
	public bool ValidPlant
	{
		get
		{
			return this.plantPreview == null || this.plantPreview.Valid;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06004E96 RID: 20118 RVA: 0x000D7719 File Offset: 0x000D5919
	public bool AcceptsFertilizer
	{
		get
		{
			return this.accepts_fertilizer;
		}
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06004E97 RID: 20119 RVA: 0x000D7721 File Offset: 0x000D5921
	public bool AcceptsIrrigation
	{
		get
		{
			return this.accepts_irrigation;
		}
	}

	// Token: 0x06004E98 RID: 20120 RVA: 0x00276D90 File Offset: 0x00274F90
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!DlcManager.FeaturePlantMutationsEnabled())
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
			return;
		}
		if (this.requestedEntityTag.IsValid && this.requestedEntityAdditionalFilterTag.IsValid && !PlantSubSpeciesCatalog.Instance.IsValidPlantableSeed(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag))
		{
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
	}

	// Token: 0x06004E99 RID: 20121 RVA: 0x00276DF0 File Offset: 0x00274FF0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.FarmFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
		this.plantRef = new Ref<KPrefabID>();
		base.Subscribe<PlantablePlot>(-905833192, PlantablePlot.OnCopySettingsDelegate);
		base.Subscribe<PlantablePlot>(144050788, PlantablePlot.OnUpdateRoomDelegate);
		if (this.HasTag(GameTags.FarmTiles))
		{
			this.storage.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			DropAllWorkable component = base.GetComponent<DropAllWorkable>();
			if (component != null)
			{
				component.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
			Toggleable component2 = base.GetComponent<Toggleable>();
			if (component2 != null)
			{
				component2.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
			}
		}
	}

	// Token: 0x06004E9A RID: 20122 RVA: 0x00276ED8 File Offset: 0x002750D8
	private void OnCopySettings(object data)
	{
		PlantablePlot component = ((GameObject)data).GetComponent<PlantablePlot>();
		if (component != null)
		{
			if (base.occupyingObject == null && (this.requestedEntityTag != component.requestedEntityTag || this.requestedEntityAdditionalFilterTag != component.requestedEntityAdditionalFilterTag || component.occupyingObject != null))
			{
				Tag entityTag = component.requestedEntityTag;
				Tag additionalFilterTag = component.requestedEntityAdditionalFilterTag;
				if (component.occupyingObject != null)
				{
					SeedProducer component2 = component.occupyingObject.GetComponent<SeedProducer>();
					if (component2 != null)
					{
						entityTag = TagManager.Create(component2.seedInfo.seedId);
						MutantPlant component3 = component.occupyingObject.GetComponent<MutantPlant>();
						additionalFilterTag = (component3 ? component3.SubSpeciesID : Tag.Invalid);
					}
				}
				base.CancelActiveRequest();
				this.CreateOrder(entityTag, additionalFilterTag);
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component4 = base.GetComponent<Prioritizable>();
				if (component4 != null)
				{
					Prioritizable component5 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component5 != null)
					{
						component5.SetMasterPriority(component4.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x06004E9B RID: 20123 RVA: 0x000D7729 File Offset: 0x000D5929
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.SetPreview(entityTag, false);
		if (this.ValidPlant)
		{
			base.CreateOrder(entityTag, additionalFilterTag);
			return;
		}
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x06004E9C RID: 20124 RVA: 0x00276FFC File Offset: 0x002751FC
	private void SyncPriority(PrioritySetting priority)
	{
		Prioritizable component = base.GetComponent<Prioritizable>();
		if (!object.Equals(component.GetMasterPriority(), priority))
		{
			component.SetMasterPriority(priority);
		}
		if (base.occupyingObject != null)
		{
			Prioritizable component2 = base.occupyingObject.GetComponent<Prioritizable>();
			if (component2 != null && !object.Equals(component2.GetMasterPriority(), priority))
			{
				component2.SetMasterPriority(component.GetMasterPriority());
			}
		}
	}

	// Token: 0x06004E9D RID: 20125 RVA: 0x00277078 File Offset: 0x00275278
	protected override void OnSpawn()
	{
		if (this.plant != null)
		{
			this.RegisterWithPlant(this.plant.gameObject);
		}
		base.OnSpawn();
		this.autoReplaceEntity = false;
		Components.PlantablePlots.Add(base.gameObject.GetMyWorldId(), this);
		Prioritizable component = base.GetComponent<Prioritizable>();
		component.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(component.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
	}

	// Token: 0x06004E9E RID: 20126 RVA: 0x000D7750 File Offset: 0x000D5950
	public void SetFertilizationFlags(bool fertilizer, bool liquid_piping)
	{
		this.accepts_fertilizer = fertilizer;
		this.has_liquid_pipe_input = liquid_piping;
	}

	// Token: 0x06004E9F RID: 20127 RVA: 0x002770F0 File Offset: 0x002752F0
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.plantPreview != null)
		{
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (base.occupyingObject)
		{
			base.occupyingObject.Trigger(-216549700, null);
		}
		Components.PlantablePlots.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x06004EA0 RID: 20128 RVA: 0x00277158 File Offset: 0x00275358
	protected override GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		PlantableSeed component = depositedEntity.GetComponent<PlantableSeed>();
		if (component != null)
		{
			Vector3 position = Grid.CellToPosCBC(Grid.PosToCell(this), this.plantLayer);
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(component.PlantID), position, this.plantLayer, null, 0);
			MutantPlant component2 = gameObject.GetComponent<MutantPlant>();
			if (component2 != null)
			{
				component.GetComponent<MutantPlant>().CopyMutationsTo(component2);
			}
			gameObject.SetActive(true);
			this.destroyEntityOnDeposit = true;
			return gameObject;
		}
		this.destroyEntityOnDeposit = false;
		return depositedEntity;
	}

	// Token: 0x06004EA1 RID: 20129 RVA: 0x002771D4 File Offset: 0x002753D4
	protected override void ConfigureOccupyingObject(GameObject newPlant)
	{
		KPrefabID component = newPlant.GetComponent<KPrefabID>();
		this.plantRef.Set(component);
		this.RegisterWithPlant(newPlant);
		UprootedMonitor component2 = newPlant.GetComponent<UprootedMonitor>();
		if (component2)
		{
			component2.canBeUprooted = false;
		}
		this.autoReplaceEntity = false;
		Prioritizable component3 = base.GetComponent<Prioritizable>();
		if (component3 != null)
		{
			Prioritizable component4 = newPlant.GetComponent<Prioritizable>();
			if (component4 != null)
			{
				component4.SetMasterPriority(component3.GetMasterPriority());
				Prioritizable prioritizable = component4;
				prioritizable.onPriorityChanged = (Action<PrioritySetting>)Delegate.Combine(prioritizable.onPriorityChanged, new Action<PrioritySetting>(this.SyncPriority));
			}
		}
	}

	// Token: 0x06004EA2 RID: 20130 RVA: 0x000D7760 File Offset: 0x000D5960
	public void ReplacePlant(GameObject plant, bool keepStorage)
	{
		if (keepStorage)
		{
			this.UnsubscribeFromOccupant();
			base.occupyingObject = null;
		}
		base.ForceDeposit(plant);
	}

	// Token: 0x06004EA3 RID: 20131 RVA: 0x00277268 File Offset: 0x00275468
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		KBatchedAnimController component = base.occupyingObject.GetComponent<KBatchedAnimController>();
		component.SetSceneLayer(this.plantLayer);
		this.OffsetAnim(component, this.occupyingObjectVisualOffset);
	}

	// Token: 0x06004EA4 RID: 20132 RVA: 0x002772A0 File Offset: 0x002754A0
	private void RegisterWithPlant(GameObject plant)
	{
		base.occupyingObject = plant;
		ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
		if (component)
		{
			if (this.tagOnPlanted != Tag.Invalid)
			{
				component.AddTag(this.tagOnPlanted);
			}
			component.SetReceptacle(this);
		}
		plant.Trigger(1309017699, this.storage);
	}

	// Token: 0x06004EA5 RID: 20133 RVA: 0x000D7779 File Offset: 0x000D5979
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			base.Subscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x06004EA6 RID: 20134 RVA: 0x000D77AD File Offset: 0x000D59AD
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		if (base.occupyingObject != null)
		{
			base.Unsubscribe(base.occupyingObject, -216549700, new Action<object>(this.OnOccupantUprooted));
		}
	}

	// Token: 0x06004EA7 RID: 20135 RVA: 0x000D77E0 File Offset: 0x000D59E0
	private void OnOccupantUprooted(object data)
	{
		this.autoReplaceEntity = false;
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
	}

	// Token: 0x06004EA8 RID: 20136 RVA: 0x002772FC File Offset: 0x002754FC
	public override void OrderRemoveOccupant()
	{
		if (base.Occupant == null)
		{
			return;
		}
		Uprootable component = base.Occupant.GetComponent<Uprootable>();
		if (component == null)
		{
			return;
		}
		component.MarkForUproot(true);
	}

	// Token: 0x06004EA9 RID: 20137 RVA: 0x00277338 File Offset: 0x00275538
	public override void SetPreview(Tag entityTag, bool solid = false)
	{
		PlantableSeed plantableSeed = null;
		if (entityTag.IsValid)
		{
			GameObject prefab = Assets.GetPrefab(entityTag);
			if (prefab == null)
			{
				DebugUtil.LogWarningArgs(base.gameObject, new object[]
				{
					"Planter tried previewing a tag with no asset! If this was the 'Empty' tag, ignore it, that will go away in new save games. Otherwise... Eh? Tag was: ",
					entityTag
				});
				return;
			}
			plantableSeed = prefab.GetComponent<PlantableSeed>();
		}
		if (this.plantPreview != null)
		{
			KPrefabID component = this.plantPreview.GetComponent<KPrefabID>();
			if (plantableSeed != null && component != null && component.PrefabTag == plantableSeed.PreviewID)
			{
				return;
			}
			this.plantPreview.gameObject.Unsubscribe(-1820564715, new Action<object>(this.OnValidChanged));
			Util.KDestroyGameObject(this.plantPreview.gameObject);
		}
		if (plantableSeed != null)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PreviewID), Grid.SceneLayer.Front, null, 0);
			this.plantPreview = gameObject.GetComponent<EntityPreview>();
			gameObject.transform.SetPosition(Vector3.zero);
			gameObject.transform.SetParent(base.gameObject.transform, false);
			gameObject.transform.SetLocalPosition(Vector3.zero);
			if (this.rotatable != null)
			{
				if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
				{
					gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
				}
				else if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R90));
				}
				else
				{
					gameObject.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R180));
				}
			}
			else
			{
				gameObject.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
			}
			KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
			this.OffsetAnim(component2, this.occupyingObjectVisualOffset);
			gameObject.SetActive(true);
			gameObject.Subscribe(-1820564715, new Action<object>(this.OnValidChanged));
			if (solid)
			{
				this.plantPreview.SetSolid();
			}
			this.plantPreview.UpdateValidity();
		}
	}

	// Token: 0x06004EAA RID: 20138 RVA: 0x000D77FF File Offset: 0x000D59FF
	private void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
	{
		if (this.rotatable != null)
		{
			offset = this.rotatable.GetRotatedOffset(offset);
		}
		kanim.Offset = offset;
	}

	// Token: 0x06004EAB RID: 20139 RVA: 0x000D7824 File Offset: 0x000D5A24
	private void OnValidChanged(object obj)
	{
		base.Trigger(-1820564715, obj);
		if (!this.plantPreview.Valid && base.GetActiveRequest != null)
		{
			base.CancelActiveRequest();
		}
	}

	// Token: 0x06004EAC RID: 20140 RVA: 0x00277528 File Offset: 0x00275728
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.ENABLESDOMESTICGROWTH, UI.BUILDINGEFFECTS.TOOLTIPS.ENABLESDOMESTICGROWTH, Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x0400372D RID: 14125
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400372E RID: 14126
	public Tag tagOnPlanted = Tag.Invalid;

	// Token: 0x0400372F RID: 14127
	[Serialize]
	private Ref<KPrefabID> plantRef;

	// Token: 0x04003730 RID: 14128
	public Vector3 occupyingObjectVisualOffset = Vector3.zero;

	// Token: 0x04003731 RID: 14129
	public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;

	// Token: 0x04003732 RID: 14130
	private EntityPreview plantPreview;

	// Token: 0x04003733 RID: 14131
	[SerializeField]
	private bool accepts_fertilizer;

	// Token: 0x04003734 RID: 14132
	[SerializeField]
	private bool accepts_irrigation = true;

	// Token: 0x04003735 RID: 14133
	[SerializeField]
	public bool has_liquid_pipe_input;

	// Token: 0x04003736 RID: 14134
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003737 RID: 14135
	private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>(delegate(PlantablePlot component, object data)
	{
		if (component.plantRef.Get() != null)
		{
			component.plantRef.Get().Trigger(144050788, data);
		}
	});
}
