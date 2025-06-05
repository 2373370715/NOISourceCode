using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A8D RID: 6797
[SerializationConfig(MemberSerialization.OptIn)]
public class WaterCooler : StateMachineComponent<WaterCooler.StatesInstance>, IApproachable, IGameObjectEffectDescriptor, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x17000940 RID: 2368
	// (get) Token: 0x06008DB1 RID: 36273 RVA: 0x001011A8 File Offset: 0x000FF3A8
	// (set) Token: 0x06008DB2 RID: 36274 RVA: 0x00376E0C File Offset: 0x0037500C
	public Tag ChosenBeverage
	{
		get
		{
			return this.chosenBeverage;
		}
		set
		{
			if (this.chosenBeverage != value)
			{
				this.chosenBeverage = value;
				base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
				this.storage.DropAll(false, false, default(Vector3), true, null);
			}
		}
	}

	// Token: 0x06008DB3 RID: 36275 RVA: 0x00376E58 File Offset: 0x00375058
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<ManualDeliveryKG>().RequestedItemTag = this.chosenBeverage;
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new SocialGatheringPointWorkable[this.socializeOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.socializeOffsets[i]), Grid.SceneLayer.Move);
			SocialGatheringPointWorkable socialGatheringPointWorkable = ChoreHelpers.CreateLocator("WaterCoolerWorkable", pos).AddOrGet<SocialGatheringPointWorkable>();
			socialGatheringPointWorkable.specificEffect = "Socialized";
			socialGatheringPointWorkable.SetWorkTime(this.workTime);
			this.workables[i] = socialGatheringPointWorkable;
		}
		this.chores = new Chore[this.socializeOffsets.Length];
		Extents extents = new Extents(Grid.PosToCell(this), this.socializeOffsets);
		this.validNavCellChangedPartitionerEntry = GameScenePartitioner.Instance.Add("WaterCooler", this, extents, GameScenePartitioner.Instance.validNavCellChangedLayer, new Action<object>(this.OnCellChanged));
		base.Subscribe<WaterCooler>(-1697596308, WaterCooler.OnStorageChangeDelegate);
		base.smi.StartSM();
	}

	// Token: 0x06008DB4 RID: 36276 RVA: 0x00376F98 File Offset: 0x00375198
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.validNavCellChangedPartitionerEntry);
		this.CancelDrinkChores();
		for (int i = 0; i < this.workables.Length; i++)
		{
			if (this.workables[i])
			{
				Util.KDestroyGameObject(this.workables[i]);
				this.workables[i] = null;
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x06008DB5 RID: 36277 RVA: 0x00376FFC File Offset: 0x003751FC
	public void UpdateDrinkChores(bool force = true)
	{
		if (!force && !this.choresDirty)
		{
			return;
		}
		float num = this.storage.GetMassAvailable(this.ChosenBeverage);
		int num2 = 0;
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			CellOffset offset = this.socializeOffsets[i];
			Chore chore = this.chores[i];
			if (num2 < this.choreCount && this.IsOffsetValid(offset) && num >= 1f)
			{
				num2++;
				num -= 1f;
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = new WaterCoolerChore(this, this.workables[i], null, null, new Action<Chore>(this.OnChoreEnd));
				}
			}
			else if (chore != null)
			{
				chore.Cancel("invalid");
				this.chores[i] = null;
			}
		}
		this.choresDirty = false;
	}

	// Token: 0x06008DB6 RID: 36278 RVA: 0x003770DC File Offset: 0x003752DC
	public void CancelDrinkChores()
	{
		for (int i = 0; i < this.socializeOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (chore != null)
			{
				chore.Cancel("cancelled");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06008DB7 RID: 36279 RVA: 0x0037711C File Offset: 0x0037531C
	private bool IsOffsetValid(CellOffset offset)
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
		int anchor_cell = Grid.CellBelow(cell);
		return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
	}

	// Token: 0x06008DB8 RID: 36280 RVA: 0x001011B0 File Offset: 0x000FF3B0
	private void OnChoreEnd(Chore chore)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008DB9 RID: 36281 RVA: 0x001011B0 File Offset: 0x000FF3B0
	private void OnCellChanged(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008DBA RID: 36282 RVA: 0x001011B0 File Offset: 0x000FF3B0
	private void OnStorageChange(object data)
	{
		this.choresDirty = true;
	}

	// Token: 0x06008DBB RID: 36283 RVA: 0x001011B9 File Offset: 0x000FF3B9
	public CellOffset[] GetOffsets()
	{
		return this.drinkOffsets;
	}

	// Token: 0x06008DBC RID: 36284 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06008DBD RID: 36285 RVA: 0x002C93D8 File Offset: 0x002C75D8
	private void AddRequirementDesc(List<Descriptor> descs, Tag tag, float mass)
	{
		string arg = tag.ProperName();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
		descs.Add(item);
	}

	// Token: 0x06008DBE RID: 36286 RVA: 0x00377144 File Offset: 0x00375344
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		Effect.AddModifierDescriptions(base.gameObject, list, "Socialized", true);
		foreach (global::Tuple<Tag, string> tuple in WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
		{
			this.AddRequirementDesc(list, tuple.first, 1f);
		}
		return list;
	}

	// Token: 0x06008DBF RID: 36287 RVA: 0x003771C4 File Offset: 0x003753C4
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		Effect.CreateTooltip(Db.Get().effects.Get("DuplicantGotMilk"), true, "\n    • ", true);
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string text = Strings.Get("STRINGS.BUILDINGS.PREFABS.WATERCOOLER.OPTION_TOOLTIPS." + WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first.ToString().ToUpper());
			if (!WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second.IsNullOrWhiteSpace())
			{
				text = text + "\n\n" + Effect.CreateTooltip(Db.Get().effects.Get(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].second), false, "\n    • ", true);
			}
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, ElementLoader.GetElement(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first).name, Def.GetUISprite(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS[i].first, "ui", false), text);
		}
		return array;
	}

	// Token: 0x06008DC0 RID: 36288 RVA: 0x001011C1 File Offset: 0x000FF3C1
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.ChosenBeverage = option.tag;
	}

	// Token: 0x06008DC1 RID: 36289 RVA: 0x001011CF File Offset: 0x000FF3CF
	public Tag GetSelectedOption()
	{
		return this.ChosenBeverage;
	}

	// Token: 0x04006AFE RID: 27390
	public const float DRINK_MASS = 1f;

	// Token: 0x04006AFF RID: 27391
	public const string SPECIFIC_EFFECT = "Socialized";

	// Token: 0x04006B00 RID: 27392
	public CellOffset[] socializeOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(2, 0),
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04006B01 RID: 27393
	public int choreCount = 2;

	// Token: 0x04006B02 RID: 27394
	public float workTime = 5f;

	// Token: 0x04006B03 RID: 27395
	private CellOffset[] drinkOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0)
	};

	// Token: 0x04006B04 RID: 27396
	public static Action<GameObject, GameObject> OnDuplicantDrank;

	// Token: 0x04006B05 RID: 27397
	private Chore[] chores;

	// Token: 0x04006B06 RID: 27398
	private HandleVector<int>.Handle validNavCellChangedPartitionerEntry;

	// Token: 0x04006B07 RID: 27399
	private SocialGatheringPointWorkable[] workables;

	// Token: 0x04006B08 RID: 27400
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04006B09 RID: 27401
	public bool choresDirty;

	// Token: 0x04006B0A RID: 27402
	[Serialize]
	private Tag chosenBeverage = GameTags.Water;

	// Token: 0x04006B0B RID: 27403
	private static readonly EventSystem.IntraObjectHandler<WaterCooler> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<WaterCooler>(delegate(WaterCooler component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x02001A8E RID: 6798
	public class States : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler>
	{
		// Token: 0x06008DC4 RID: 36292 RVA: 0x00377374 File Offset: 0x00375574
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.TagTransition(GameTags.Operational, this.waitingfordelivery, false).PlayAnim("off");
			this.waitingfordelivery.TagTransition(GameTags.Operational, this.unoperational, true).Transition(this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OnStorageChange, this.dispensing, (WaterCooler.StatesInstance smi) => smi.HasMinimumMass()).PlayAnim("off");
			this.dispensing.Enter("StartMeter", delegate(WaterCooler.StatesInstance smi)
			{
				smi.StartMeter();
			}).Enter("Set Active", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(true);
			}).Enter("UpdateDrinkChores.force", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.UpdateDrinkChores(true);
			}).Update("UpdateDrinkChores", delegate(WaterCooler.StatesInstance smi, float dt)
			{
				smi.master.UpdateDrinkChores(true);
			}, UpdateRate.SIM_200ms, false).Exit("CancelDrinkChores", delegate(WaterCooler.StatesInstance smi)
			{
				smi.master.CancelDrinkChores();
			}).Exit("Set Inactive", delegate(WaterCooler.StatesInstance smi)
			{
				smi.SetOperationalActiveState(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).EventTransition(GameHashes.OnStorageChange, this.waitingfordelivery, (WaterCooler.StatesInstance smi) => !smi.HasMinimumMass()).PlayAnim("working");
		}

		// Token: 0x04006B0C RID: 27404
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State unoperational;

		// Token: 0x04006B0D RID: 27405
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State waitingfordelivery;

		// Token: 0x04006B0E RID: 27406
		public GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.State dispensing;
	}

	// Token: 0x02001A90 RID: 6800
	public class StatesInstance : GameStateMachine<WaterCooler.States, WaterCooler.StatesInstance, WaterCooler, object>.GameInstance
	{
		// Token: 0x06008DD1 RID: 36305 RVA: 0x00377570 File Offset: 0x00375770
		public StatesInstance(WaterCooler smi) : base(smi)
		{
			this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_bottle", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_bottle"
			});
			this.storage = base.master.GetComponent<Storage>();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x06008DD2 RID: 36306 RVA: 0x003775D8 File Offset: 0x003757D8
		public void Drink(GameObject druplicant, bool triggerOnDrinkCallback = true)
		{
			if (!this.HasMinimumMass())
			{
				return;
			}
			Tag tag = this.storage.items[0].PrefabID();
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.storage.ConsumeAndGetDisease(tag, 1f, out num, out diseaseInfo, out num2);
			GermExposureMonitor.Instance smi = druplicant.GetSMI<GermExposureMonitor.Instance>();
			if (smi != null)
			{
				smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, tag, Sickness.InfectionVector.Digestion);
			}
			Effects component = druplicant.GetComponent<Effects>();
			if (tag == SimHashes.Milk.CreateTag())
			{
				component.Add("DuplicantGotMilk", true);
			}
			if (triggerOnDrinkCallback)
			{
				Action<GameObject, GameObject> onDuplicantDrank = WaterCooler.OnDuplicantDrank;
				if (onDuplicantDrank == null)
				{
					return;
				}
				onDuplicantDrank(druplicant, base.gameObject);
			}
		}

		// Token: 0x06008DD3 RID: 36307 RVA: 0x00377680 File Offset: 0x00375880
		private void OnStorageChange(object data)
		{
			float positionPercent = Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg);
			this.meter.SetPositionPercent(positionPercent);
		}

		// Token: 0x06008DD4 RID: 36308 RVA: 0x0010124F File Offset: 0x000FF44F
		public void SetOperationalActiveState(bool isActive)
		{
			this.operational.SetActive(isActive, false);
		}

		// Token: 0x06008DD5 RID: 36309 RVA: 0x003776B8 File Offset: 0x003758B8
		public void StartMeter()
		{
			PrimaryElement primaryElement = this.storage.FindFirstWithMass(base.smi.master.ChosenBeverage, 0f);
			if (primaryElement == null)
			{
				return;
			}
			this.meter.SetSymbolTint(new KAnimHashedString("meter_water"), primaryElement.Element.substance.colour);
			this.OnStorageChange(null);
		}

		// Token: 0x06008DD6 RID: 36310 RVA: 0x0010125E File Offset: 0x000FF45E
		public bool HasMinimumMass()
		{
			return this.storage.GetMassAvailable(ElementLoader.GetElement(base.smi.master.ChosenBeverage).id) >= 1f;
		}

		// Token: 0x04006B19 RID: 27417
		[MyCmpGet]
		private Operational operational;

		// Token: 0x04006B1A RID: 27418
		private Storage storage;

		// Token: 0x04006B1B RID: 27419
		private MeterController meter;
	}
}
