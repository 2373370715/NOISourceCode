using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000E04 RID: 3588
public class GourmetCookingStation : ComplexFabricator, IGameObjectEffectDescriptor
{
	// Token: 0x06004615 RID: 17941 RVA: 0x0025C11C File Offset: 0x0025A31C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.keepAdditionalTag = this.fuelTag;
		this.choreType = Db.Get().ChoreTypes.Cook;
		this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
	}

	// Token: 0x06004616 RID: 17942 RVA: 0x0025C16C File Offset: 0x0025A36C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
		this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
		this.workable.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_cookstation_gourtmet_kanim")
		};
		this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		ComplexFabricatorWorkable workable = this.workable;
		workable.OnWorkTickActions = (Action<WorkerBase, float>)Delegate.Combine(workable.OnWorkTickActions, new Action<WorkerBase, float>(delegate(WorkerBase worker, float dt)
		{
			global::Debug.Assert(worker != null, "How did we get a null worker?");
			if (this.diseaseCountKillRate > 0)
			{
				PrimaryElement component = base.GetComponent<PrimaryElement>();
				int num = Math.Max(1, (int)((float)this.diseaseCountKillRate * dt));
				component.ModifyDiseaseCount(-num, "GourmetCookingStation");
			}
		}));
		this.smi = new GourmetCookingStation.StatesInstance(this);
		this.smi.StartSM();
		base.GetComponent<ComplexFabricator>().workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorCooking;
	}

	// Token: 0x06004617 RID: 17943 RVA: 0x000D1C75 File Offset: 0x000CFE75
	public float GetAvailableFuel()
	{
		return this.inStorage.GetAmountAvailable(this.fuelTag);
	}

	// Token: 0x06004618 RID: 17944 RVA: 0x0025C28C File Offset: 0x0025A48C
	protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = base.SpawnOrderProduct(recipe);
		foreach (GameObject gameObject in list)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ModifyDiseaseCount(-component.DiseaseCount, "GourmetCookingStation.CompleteOrder");
		}
		base.GetComponent<Operational>().SetActive(false, false);
		return list;
	}

	// Token: 0x06004619 RID: 17945 RVA: 0x000CF0DD File Offset: 0x000CD2DD
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.REMOVES_DISEASE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
		return descriptors;
	}

	// Token: 0x040030E2 RID: 12514
	private static readonly Operational.Flag gourmetCookingStationFlag = new Operational.Flag("gourmet_cooking_station", Operational.Flag.Type.Requirement);

	// Token: 0x040030E3 RID: 12515
	public float GAS_CONSUMPTION_RATE;

	// Token: 0x040030E4 RID: 12516
	public float GAS_CONVERSION_RATIO = 0.1f;

	// Token: 0x040030E5 RID: 12517
	public const float START_FUEL_MASS = 5f;

	// Token: 0x040030E6 RID: 12518
	public Tag fuelTag;

	// Token: 0x040030E7 RID: 12519
	[SerializeField]
	private int diseaseCountKillRate = 150;

	// Token: 0x040030E8 RID: 12520
	private GourmetCookingStation.StatesInstance smi;

	// Token: 0x02000E05 RID: 3589
	public class StatesInstance : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.GameInstance
	{
		// Token: 0x0600461D RID: 17949 RVA: 0x000D1CB8 File Offset: 0x000CFEB8
		public StatesInstance(GourmetCookingStation smi) : base(smi)
		{
		}
	}

	// Token: 0x02000E06 RID: 3590
	public class States : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation>
	{
		// Token: 0x0600461E RID: 17950 RVA: 0x0025C34C File Offset: 0x0025A54C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			if (GourmetCookingStation.States.waitingForFuelStatus == null)
			{
				GourmetCookingStation.States.waitingForFuelStatus = new StatusItem("waitingForFuelStatus", BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022, true, null);
				GourmetCookingStation.States.waitingForFuelStatus.resolveStringCallback = delegate(string str, object obj)
				{
					GourmetCookingStation gourmetCookingStation = (GourmetCookingStation)obj;
					return string.Format(str, gourmetCookingStation.fuelTag.ProperName(), GameUtil.GetFormattedMass(5f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				};
			}
			default_state = this.waitingForFuel;
			this.waitingForFuel.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, false);
			}).ToggleStatusItem(GourmetCookingStation.States.waitingForFuelStatus, (GourmetCookingStation.StatesInstance smi) => smi.master).EventTransition(GameHashes.OnStorageChange, this.ready, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() >= 5f);
			this.ready.Enter(delegate(GourmetCookingStation.StatesInstance smi)
			{
				smi.master.SetQueueDirty();
				smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, true);
			}).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (GourmetCookingStation.StatesInstance smi) => smi.master.GetAvailableFuel() <= 0f);
		}

		// Token: 0x040030E9 RID: 12521
		public static StatusItem waitingForFuelStatus;

		// Token: 0x040030EA RID: 12522
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State waitingForFuel;

		// Token: 0x040030EB RID: 12523
		public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State ready;
	}
}
