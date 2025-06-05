using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001A3F RID: 6719
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
	// Token: 0x06008BFB RID: 35835 RVA: 0x00370134 File Offset: 0x0036E334
	public static Tinkerable MakePowerTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerable.onCompleteSFX = "Generator_Microchip_installed";
		tinkerable.boostSymbolNames = new string[]
		{
			"booster",
			"blue_light_bloom"
		};
		tinkerable.SetWorkTime(30f);
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
		tinkerable.addedEffect = "PowerTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
		tinkerable.effectMultiplier = 0.025f;
		tinkerable.multitoolContext = "powertinker";
		tinkerable.multitoolHitEffectTag = "fx_powertinker_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06008BFC RID: 35836 RVA: 0x003702BC File Offset: 0x0036E4BC
	public static Tinkerable MakeFarmTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.addedEffect = "FarmTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
		tinkerable.effectMultiplier = 0.1f;
		tinkerable.SetWorkTime(15f);
		tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerable.multitoolContext = "tend";
		tinkerable.multitoolHitEffectTag = "fx_tend_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06008BFD RID: 35837 RVA: 0x00370428 File Offset: 0x0036E628
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		base.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
		base.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
		base.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
		base.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
	}

	// Token: 0x06008BFE RID: 35838 RVA: 0x00100169 File Offset: 0x000FE369
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
		this.prioritizableAdded = true;
		base.Subscribe<Tinkerable>(493375141, Tinkerable.OnRefreshUserMenuDelegate);
		this.UpdateVisual();
	}

	// Token: 0x06008BFF RID: 35839 RVA: 0x0010019A File Offset: 0x000FE39A
	protected override void OnCleanUp()
	{
		this.UpdateMaterialReservation(false);
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		if (this.prioritizableAdded)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		base.OnCleanUp();
	}

	// Token: 0x06008C00 RID: 35840 RVA: 0x001001D4 File Offset: 0x000FE3D4
	private void OnOperationalChanged(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008C01 RID: 35841 RVA: 0x001001D4 File Offset: 0x000FE3D4
	private void OnEffectRemoved(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008C02 RID: 35842 RVA: 0x001001D4 File Offset: 0x000FE3D4
	private void OnUpdateRoom(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06008C03 RID: 35843 RVA: 0x001001DC File Offset: 0x000FE3DC
	private void OnStorageChange(object data)
	{
		if (((GameObject)data).IsPrefabID(this.tinkerMaterialTag))
		{
			this.QueueUpdateChore();
		}
	}

	// Token: 0x06008C04 RID: 35844 RVA: 0x003704D8 File Offset: 0x0036E6D8
	private void QueueUpdateChore()
	{
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), null, null);
	}

	// Token: 0x06008C05 RID: 35845 RVA: 0x001001F7 File Offset: 0x000FE3F7
	private void UpdateChoreCallback(object obj)
	{
		this.UpdateChore();
	}

	// Token: 0x06008C06 RID: 35846 RVA: 0x00370528 File Offset: 0x0036E728
	private void UpdateChore()
	{
		Operational component = base.GetComponent<Operational>();
		bool flag = component == null || component.IsFunctional;
		bool flag2 = this.HasEffect();
		bool flag3 = this.HasCorrectRoom();
		bool flag4 = !flag2 && flag && flag3 && this.userMenuAllowed;
		bool flag5 = flag2 || !flag3 || !this.userMenuAllowed;
		if (this.chore == null && flag4)
		{
			this.UpdateMaterialReservation(true);
			if (this.HasMaterial())
			{
				this.chore = new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				if (component != null)
				{
					this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, component);
				}
			}
			else
			{
				this.chore = new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount, new HashSet<Tag>
				{
					this.tinkerMaterialTag
				}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), null, null, Operational.State.Functional, 0);
			}
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
			if (!string.IsNullOrEmpty(base.GetComponent<RoomTracker>().requiredRoomType))
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, Grid.PosToCell(base.transform.GetPosition()));
				return;
			}
		}
		else if (this.chore != null && flag5)
		{
			this.UpdateMaterialReservation(false);
			this.chore.Cancel("No longer needed");
			this.chore = null;
		}
	}

	// Token: 0x06008C07 RID: 35847 RVA: 0x001001FF File Offset: 0x000FE3FF
	private bool HasCorrectRoom()
	{
		return this.roomTracker.IsInCorrectRoom();
	}

	// Token: 0x06008C08 RID: 35848 RVA: 0x003706D0 File Offset: 0x0036E8D0
	private bool RoomHasTinkerstation()
	{
		if (!this.roomTracker.IsInCorrectRoom())
		{
			return false;
		}
		if (this.roomTracker.room == null)
		{
			return false;
		}
		foreach (KPrefabID kprefabID in this.roomTracker.room.buildings)
		{
			if (!(kprefabID == null))
			{
				TinkerStation component = kprefabID.GetComponent<TinkerStation>();
				if (component != null && component.outputPrefab == this.tinkerMaterialTag)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06008C09 RID: 35849 RVA: 0x00370778 File Offset: 0x0036E978
	private void UpdateMaterialReservation(bool shouldReserve)
	{
		if (shouldReserve && !this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
			return;
		}
		if (!shouldReserve && this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
		}
	}

	// Token: 0x06008C0A RID: 35850 RVA: 0x0010020C File Offset: 0x000FE40C
	private void OnFetchComplete(Chore data)
	{
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x06008C0B RID: 35851 RVA: 0x003707E4 File Offset: 0x0036E9E4
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
		float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
		this.effects.Add(this.addedEffect, true).timeRemaining *= 1f + totalValue * this.effectMultiplier;
		this.UpdateVisual();
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
		string sound = GlobalAssets.GetSound(this.onCompleteSFX, false);
		if (sound != null)
		{
			SoundEvent.EndOneShot(SoundEvent.BeginOneShot(sound, base.transform.position, 1f, false));
		}
	}

	// Token: 0x06008C0C RID: 35852 RVA: 0x003708A8 File Offset: 0x0036EAA8
	private void UpdateVisual()
	{
		if (this.boostSymbolNames == null)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool is_visible = this.effects.HasEffect(this.addedEffect);
		foreach (string str in this.boostSymbolNames)
		{
			component.SetSymbolVisiblity(str, is_visible);
		}
	}

	// Token: 0x06008C0D RID: 35853 RVA: 0x00100222 File Offset: 0x000FE422
	private bool HasMaterial()
	{
		return this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= this.tinkerMaterialAmount;
	}

	// Token: 0x06008C0E RID: 35854 RVA: 0x00100240 File Offset: 0x000FE440
	private bool HasEffect()
	{
		return this.effects.HasEffect(this.addedEffect);
	}

	// Token: 0x06008C0F RID: 35855 RVA: 0x00370900 File Offset: 0x0036EB00
	private void OnRefreshUserMenu(object data)
	{
		if (this.roomTracker.IsInCorrectRoom())
		{
			string name = Db.Get().effects.Get(this.addedEffect).Name;
			string properName = this.GetProperName();
			KIconButtonMenu.ButtonInfo button = this.userMenuAllowed ? new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.DISALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, string.Format(UI.USERMENUACTIONS.TINKER.TOOLTIP_DISALLOW, name, properName), true) : new KIconButtonMenu.ButtonInfo("action_switch_toggle", UI.USERMENUACTIONS.TINKER.ALLOW, new System.Action(this.OnClickToggleTinker), global::Action.NumActions, null, null, null, string.Format(UI.USERMENUACTIONS.TINKER.TOOLTIP_ALLOW, name, properName), true);
			Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
		}
	}

	// Token: 0x06008C10 RID: 35856 RVA: 0x00100253 File Offset: 0x000FE453
	private void OnClickToggleTinker()
	{
		this.userMenuAllowed = !this.userMenuAllowed;
		this.UpdateChore();
	}

	// Token: 0x040069AE RID: 27054
	private Chore chore;

	// Token: 0x040069AF RID: 27055
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040069B0 RID: 27056
	[MyCmpGet]
	private Effects effects;

	// Token: 0x040069B1 RID: 27057
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x040069B2 RID: 27058
	public Tag tinkerMaterialTag;

	// Token: 0x040069B3 RID: 27059
	public float tinkerMaterialAmount;

	// Token: 0x040069B4 RID: 27060
	public string addedEffect;

	// Token: 0x040069B5 RID: 27061
	public string effectAttributeId;

	// Token: 0x040069B6 RID: 27062
	public float effectMultiplier;

	// Token: 0x040069B7 RID: 27063
	public string[] boostSymbolNames;

	// Token: 0x040069B8 RID: 27064
	public string onCompleteSFX;

	// Token: 0x040069B9 RID: 27065
	public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;

	// Token: 0x040069BA RID: 27066
	public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;

	// Token: 0x040069BB RID: 27067
	[Serialize]
	private bool userMenuAllowed = true;

	// Token: 0x040069BC RID: 27068
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnEffectRemoved(data);
	});

	// Token: 0x040069BD RID: 27069
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x040069BE RID: 27070
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x040069BF RID: 27071
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x040069C0 RID: 27072
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040069C1 RID: 27073
	private bool prioritizableAdded;

	// Token: 0x040069C2 RID: 27074
	private SchedulerHandle updateHandle;

	// Token: 0x040069C3 RID: 27075
	private bool hasReservedMaterial;
}
