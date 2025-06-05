using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000D83 RID: 3459
[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
	// Token: 0x06004333 RID: 17203 RVA: 0x0025210C File Offset: 0x0025030C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoReplaceEntity = true;
		this.choreType = Db.Get().ChoreTypes.RanchingFetch;
		this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
		this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
		this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
		this.synchronizeAnims = false;
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("egg_target", false);
		this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		base.Subscribe<EggIncubator>(-905833192, EggIncubator.OnCopySettingsDelegate);
	}

	// Token: 0x06004334 RID: 17204 RVA: 0x002521F0 File Offset: 0x002503F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.occupyingObject)
		{
			if (base.occupyingObject.HasTag(GameTags.Creature))
			{
				this.storage.allowItemRemoval = true;
			}
			this.storage.RenotifyAll();
			this.PositionOccupyingObject();
		}
		base.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
		base.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
		base.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
		this.smi = new EggIncubatorStates.Instance(this);
		this.smi.StartSM();
	}

	// Token: 0x06004335 RID: 17205 RVA: 0x0025228C File Offset: 0x0025048C
	private void OnCopySettings(object data)
	{
		EggIncubator component = ((GameObject)data).GetComponent<EggIncubator>();
		if (component != null)
		{
			this.autoReplaceEntity = component.autoReplaceEntity;
			if (base.occupyingObject == null)
			{
				if (!(this.requestedEntityTag == component.requestedEntityTag) || !(this.requestedEntityAdditionalFilterTag == component.requestedEntityAdditionalFilterTag))
				{
					base.CancelActiveRequest();
				}
				if (this.fetchChore == null)
				{
					Tag requestedEntityTag = component.requestedEntityTag;
					this.CreateOrder(requestedEntityTag, component.requestedEntityAdditionalFilterTag);
				}
			}
			if (base.occupyingObject != null)
			{
				Prioritizable component2 = base.GetComponent<Prioritizable>();
				if (component2 != null)
				{
					Prioritizable component3 = base.occupyingObject.GetComponent<Prioritizable>();
					if (component3 != null)
					{
						component3.SetMasterPriority(component2.GetMasterPriority());
					}
				}
			}
		}
	}

	// Token: 0x06004336 RID: 17206 RVA: 0x000CFDB0 File Offset: 0x000CDFB0
	protected override void OnCleanUp()
	{
		this.smi.StopSM("cleanup");
		base.OnCleanUp();
	}

	// Token: 0x06004337 RID: 17207 RVA: 0x00252358 File Offset: 0x00250558
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		if (base.occupyingObject != null)
		{
			this.tracker = base.occupyingObject.AddComponent<KBatchedAnimTracker>();
			this.tracker.symbol = "egg_target";
			this.tracker.forceAlwaysVisible = true;
		}
		this.UpdateProgress();
	}

	// Token: 0x06004338 RID: 17208 RVA: 0x000CFDC8 File Offset: 0x000CDFC8
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		this.UpdateProgress();
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x002523B4 File Offset: 0x002505B4
	private void OnOperationalChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x000CFDE8 File Offset: 0x000CDFE8
	private void OnOccupantChanged(object data = null)
	{
		if (!base.occupyingObject)
		{
			this.storage.allowItemRemoval = false;
		}
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x000CFE03 File Offset: 0x000CE003
	private void OnStorageChange(object data = null)
	{
		if (base.occupyingObject && !this.storage.items.Contains(base.occupyingObject))
		{
			this.UnsubscribeFromOccupant();
			this.ClearOccupant();
		}
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x002523E8 File Offset: 0x002505E8
	protected override void ClearOccupant()
	{
		bool flag = false;
		if (base.occupyingObject != null)
		{
			flag = !base.occupyingObject.HasTag(GameTags.Egg);
		}
		base.ClearOccupant();
		if (this.autoReplaceEntity && flag && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, Tag.Invalid);
		}
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x00252448 File Offset: 0x00250648
	protected override void PositionOccupyingObject()
	{
		base.PositionOccupyingObject();
		base.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
		KSelectable component = base.occupyingObject.GetComponent<KSelectable>();
		if (component != null)
		{
			component.IsSelectable = true;
		}
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x0025248C File Offset: 0x0025068C
	public override void OrderRemoveOccupant()
	{
		UnityEngine.Object.Destroy(this.tracker);
		this.tracker = null;
		if (base.occupyingObject != null && base.occupyingObject.HasTag(GameTags.Egg))
		{
			this.requestedEntityTag = Tag.Invalid;
		}
		this.storage.DropAll(false, false, default(Vector3), true, null);
		base.occupyingObject = null;
		this.ClearOccupant();
	}

	// Token: 0x0600433F RID: 17215 RVA: 0x002524FC File Offset: 0x002506FC
	public float GetProgress()
	{
		float result = 0f;
		if (base.occupyingObject)
		{
			AmountInstance amountInstance = base.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
			if (amountInstance != null)
			{
				result = amountInstance.value / amountInstance.GetMax();
			}
			else
			{
				result = 1f;
			}
		}
		return result;
	}

	// Token: 0x06004340 RID: 17216 RVA: 0x000CFE36 File Offset: 0x000CE036
	private void UpdateProgress()
	{
		this.meter.SetPositionPercent(this.GetProgress());
	}

	// Token: 0x06004341 RID: 17217 RVA: 0x000CFE49 File Offset: 0x000CE049
	public void Sim1000ms(float dt)
	{
		this.UpdateProgress();
		this.UpdateChore();
	}

	// Token: 0x06004342 RID: 17218 RVA: 0x00252558 File Offset: 0x00250758
	public void StoreBaby(GameObject baby)
	{
		this.UnsubscribeFromOccupant();
		this.storage.DropAll(false, false, default(Vector3), true, null);
		this.storage.allowItemRemoval = true;
		this.storage.Store(baby, false, false, true, false);
		base.occupyingObject = baby;
		this.SubscribeToOccupant();
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x06004343 RID: 17219 RVA: 0x002525C0 File Offset: 0x002507C0
	private void UpdateChore()
	{
		if (this.operational.IsOperational && this.EggNeedsAttention())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("now is not the time for song");
			this.chore = null;
		}
	}

	// Token: 0x06004344 RID: 17220 RVA: 0x0025263C File Offset: 0x0025083C
	private bool EggNeedsAttention()
	{
		if (!base.Occupant)
		{
			return false;
		}
		IncubationMonitor.Instance instance = base.Occupant.GetSMI<IncubationMonitor.Instance>();
		return instance != null && !instance.HasSongBuff();
	}

	// Token: 0x04002E7C RID: 11900
	[MyCmpAdd]
	private EggIncubatorWorkable workable;

	// Token: 0x04002E7D RID: 11901
	[MyCmpAdd]
	private CopyBuildingSettings copySettings;

	// Token: 0x04002E7E RID: 11902
	private Chore chore;

	// Token: 0x04002E7F RID: 11903
	private EggIncubatorStates.Instance smi;

	// Token: 0x04002E80 RID: 11904
	private KBatchedAnimTracker tracker;

	// Token: 0x04002E81 RID: 11905
	private MeterController meter;

	// Token: 0x04002E82 RID: 11906
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002E83 RID: 11907
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnOccupantChanged(data);
	});

	// Token: 0x04002E84 RID: 11908
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002E85 RID: 11909
	private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<EggIncubator>(delegate(EggIncubator component, object data)
	{
		component.OnCopySettings(data);
	});
}
