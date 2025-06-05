using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001431 RID: 5169
[SerializationConfig(MemberSerialization.OptIn)]
public class HotTub : StateMachineComponent<HotTub.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06006A00 RID: 27136 RVA: 0x000E9F3A File Offset: 0x000E813A
	public float PercentFull
	{
		get
		{
			return 100f * this.waterStorage.GetMassAvailable(SimHashes.Water) / this.hotTubCapacity;
		}
	}

	// Token: 0x06006A01 RID: 27137 RVA: 0x002EAB20 File Offset: 0x002E8D20
	protected override void OnSpawn()
	{
		base.OnSpawn();
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
		this.workables = new HotTubWorkable[this.choreOffsets.Length];
		this.chores = new Chore[this.choreOffsets.Length];
		for (int i = 0; i < this.workables.Length; i++)
		{
			Vector3 pos = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(this), this.choreOffsets[i]), Grid.SceneLayer.Move);
			GameObject go = ChoreHelpers.CreateLocator("HotTubWorkable", pos);
			KSelectable kselectable = go.AddOrGet<KSelectable>();
			kselectable.SetName(this.GetProperName());
			kselectable.IsSelectable = false;
			HotTubWorkable hotTubWorkable = go.AddOrGet<HotTubWorkable>();
			int player_index = i;
			HotTubWorkable hotTubWorkable2 = hotTubWorkable;
			hotTubWorkable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(hotTubWorkable2.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(delegate(Workable workable, Workable.WorkableEvent ev)
			{
				this.OnWorkableEvent(player_index, ev);
			}));
			this.workables[i] = hotTubWorkable;
			this.workables[i].hotTub = this;
		}
		this.waterMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_water_target", "meter_water", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_water_target"
		});
		base.smi.UpdateWaterMeter();
		this.tempMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_temperature_target", "meter_temp", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_temperature_target"
		});
		base.smi.TestWaterTemperature();
		base.smi.StartSM();
	}

	// Token: 0x06006A02 RID: 27138 RVA: 0x002EACB8 File Offset: 0x002E8EB8
	protected override void OnCleanUp()
	{
		this.UpdateChores(false);
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

	// Token: 0x06006A03 RID: 27139 RVA: 0x002EAD0C File Offset: 0x002E8F0C
	private Chore CreateChore(int i)
	{
		Workable workable = this.workables[i];
		ChoreType relax = Db.Get().ChoreTypes.Relax;
		IStateMachineTarget target = workable;
		ChoreProvider chore_provider = null;
		bool run_until_complete = true;
		Action<Chore> on_complete = null;
		Action<Chore> on_begin = null;
		ScheduleBlockType recreation = Db.Get().ScheduleBlockTypes.Recreation;
		WorkChore<HotTubWorkable> workChore = new WorkChore<HotTubWorkable>(relax, target, chore_provider, run_until_complete, on_complete, on_begin, new Action<Chore>(this.OnSocialChoreEnd), false, recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, workable);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotABionic, workable);
		return workChore;
	}

	// Token: 0x06006A04 RID: 27140 RVA: 0x000E9F59 File Offset: 0x000E8159
	private void OnSocialChoreEnd(Chore chore)
	{
		if (base.gameObject.HasTag(GameTags.Operational))
		{
			this.UpdateChores(true);
		}
	}

	// Token: 0x06006A05 RID: 27141 RVA: 0x002EAD88 File Offset: 0x002E8F88
	public void UpdateChores(bool update = true)
	{
		for (int i = 0; i < this.choreOffsets.Length; i++)
		{
			Chore chore = this.chores[i];
			if (update)
			{
				if (chore == null || chore.isComplete)
				{
					this.chores[i] = this.CreateChore(i);
				}
			}
			else if (chore != null)
			{
				chore.Cancel("locator invalidated");
				this.chores[i] = null;
			}
		}
	}

	// Token: 0x06006A06 RID: 27142 RVA: 0x002EADE8 File Offset: 0x002E8FE8
	public void OnWorkableEvent(int player, Workable.WorkableEvent ev)
	{
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			this.occupants.Add(player);
		}
		else
		{
			this.occupants.Remove(player);
		}
		base.smi.sm.userCount.Set(this.occupants.Count, base.smi, false);
	}

	// Token: 0x06006A07 RID: 27143 RVA: 0x002EAE40 File Offset: 0x002E9040
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Element element = ElementLoader.FindElementByHash(SimHashes.Water);
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), BUILDINGS.PREFABS.HOTTUB.WATER_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{amount}", GameUtil.GetFormattedMass(this.hotTubCapacity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), BUILDINGS.PREFABS.HOTTUB.TEMPERATURE_REQUIREMENT_TOOLTIP.Replace("{element}", element.name).Replace("{temperature}", GameUtil.GetFormattedTemperature(this.minimumWaterTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Requirement, false));
		list.Add(new Descriptor(Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_NAME"), Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + "WarmTouch".ToUpper() + ".PROVIDERS_TOOLTIP"), Descriptor.DescriptorType.Effect, false));
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		Effect.AddModifierDescriptions(base.gameObject, list, this.specificEffect, true);
		return list;
	}

	// Token: 0x0400505C RID: 20572
	public string specificEffect;

	// Token: 0x0400505D RID: 20573
	public string trackingEffect;

	// Token: 0x0400505E RID: 20574
	public int basePriority;

	// Token: 0x0400505F RID: 20575
	public CellOffset[] choreOffsets = new CellOffset[]
	{
		new CellOffset(-1, 0),
		new CellOffset(1, 0),
		new CellOffset(0, 0),
		new CellOffset(2, 0)
	};

	// Token: 0x04005060 RID: 20576
	private HotTubWorkable[] workables;

	// Token: 0x04005061 RID: 20577
	private Chore[] chores;

	// Token: 0x04005062 RID: 20578
	public HashSet<int> occupants = new HashSet<int>();

	// Token: 0x04005063 RID: 20579
	public float waterCoolingRate;

	// Token: 0x04005064 RID: 20580
	public float hotTubCapacity = 100f;

	// Token: 0x04005065 RID: 20581
	public float minimumWaterTemperature;

	// Token: 0x04005066 RID: 20582
	public float bleachStoneConsumption;

	// Token: 0x04005067 RID: 20583
	public float maxOperatingTemperature;

	// Token: 0x04005068 RID: 20584
	[MyCmpGet]
	public Storage waterStorage;

	// Token: 0x04005069 RID: 20585
	private MeterController waterMeter;

	// Token: 0x0400506A RID: 20586
	private MeterController tempMeter;

	// Token: 0x02001432 RID: 5170
	public class States : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub>
	{
		// Token: 0x06006A09 RID: 27145 RVA: 0x002EB030 File Offset: 0x002E9230
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.ready;
			this.root.Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.SapHeatFromWater(dt);
				smi.TestWaterTemperature();
			}, UpdateRate.SIM_4000ms, false).EventHandler(GameHashes.OnStorageChange, delegate(HotTub.StatesInstance smi)
			{
				smi.UpdateWaterMeter();
				smi.TestWaterTemperature();
			});
			this.unoperational.TagTransition(GameTags.Operational, this.off, false).PlayAnim("off");
			this.off.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.off.filling);
			this.off.filling.DefaultState(this.off.filling.normal).Transition(this.ready, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) >= smi.master.hotTubCapacity, UpdateRate.SIM_200ms).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitConsumer>().SetOnState(false);
			}).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubFilling, (HotTub.StatesInstance smi) => smi.master);
			this.off.filling.normal.ParamTransition<bool>(this.waterTooCold, this.off.filling.too_cold, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue);
			this.off.filling.too_cold.ParamTransition<bool>(this.waterTooCold, this.off.filling.normal, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master);
			this.off.draining.Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.GetMassAvailable(SimHashes.Water) <= 0f, UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubWaterTooCold, (HotTub.StatesInstance smi) => smi.master).PlayAnim("off").Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(true);
			}).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.GetComponent<ConduitDispenser>().SetOnState(false);
			});
			this.off.too_hot.Transition(this.ready, (HotTub.StatesInstance smi) => !smi.IsTubTooHot(), UpdateRate.SIM_200ms).PlayAnim("overheated").ToggleMainStatusItem(Db.Get().BuildingStatusItems.HotTubTooHot, (HotTub.StatesInstance smi) => smi.master);
			this.off.awaiting_delivery.EventTransition(GameHashes.OnStorageChange, this.ready, (HotTub.StatesInstance smi) => smi.HasBleachStone());
			this.ready.DefaultState(this.ready.idle).Enter("CreateChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(true);
			}).Exit("CancelChore", delegate(HotTub.StatesInstance smi)
			{
				smi.master.UpdateChores(false);
			}).TagTransition(GameTags.Operational, this.unoperational, true).ParamTransition<bool>(this.waterTooCold, this.off.draining, GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, this.off.awaiting_delivery, (HotTub.StatesInstance smi) => !smi.HasBleachStone()).Transition(this.off.filling, (HotTub.StatesInstance smi) => smi.master.waterStorage.IsEmpty(), UpdateRate.SIM_200ms).Transition(this.off.too_hot, (HotTub.StatesInstance smi) => smi.IsTubTooHot(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Normal, null);
			this.ready.idle.PlayAnim("on").ParamTransition<int>(this.userCount, this.ready.on.pre, (HotTub.StatesInstance smi, int p) => p > 0);
			this.ready.on.Enter(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(true);
			}).Update(delegate(HotTub.StatesInstance smi, float dt)
			{
				smi.ConsumeBleachstone(dt);
			}, UpdateRate.SIM_4000ms, false).Exit(delegate(HotTub.StatesInstance smi)
			{
				smi.SetActive(false);
			});
			this.ready.on.pre.PlayAnim("working_pre").OnAnimQueueComplete(this.ready.on.relaxing);
			this.ready.on.relaxing.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing_together, (HotTub.StatesInstance smi, int p) => p > 1);
			this.ready.on.relaxing_together.PlayAnim("working_loop", KAnim.PlayMode.Loop).ParamTransition<int>(this.userCount, this.ready.on.post, (HotTub.StatesInstance smi, int p) => p == 0).ParamTransition<int>(this.userCount, this.ready.on.relaxing, (HotTub.StatesInstance smi, int p) => p == 1);
			this.ready.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.ready.idle);
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x002EB740 File Offset: 0x002E9940
		private string GetRelaxingAnim(HotTub.StatesInstance smi)
		{
			bool flag = smi.master.occupants.Contains(0);
			bool flag2 = smi.master.occupants.Contains(1);
			if (flag && !flag2)
			{
				return "working_loop_one_p";
			}
			if (flag2 && !flag)
			{
				return "working_loop_two_p";
			}
			return "working_loop_coop_p";
		}

		// Token: 0x0400506B RID: 20587
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.IntParameter userCount;

		// Token: 0x0400506C RID: 20588
		public StateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.BoolParameter waterTooCold;

		// Token: 0x0400506D RID: 20589
		public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State unoperational;

		// Token: 0x0400506E RID: 20590
		public HotTub.States.OffStates off;

		// Token: 0x0400506F RID: 20591
		public HotTub.States.ReadyStates ready;

		// Token: 0x02001433 RID: 5171
		public class OffStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04005070 RID: 20592
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State draining;

			// Token: 0x04005071 RID: 20593
			public HotTub.States.FillingStates filling;

			// Token: 0x04005072 RID: 20594
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_hot;

			// Token: 0x04005073 RID: 20595
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State awaiting_delivery;
		}

		// Token: 0x02001434 RID: 5172
		public class OnStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04005074 RID: 20596
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State pre;

			// Token: 0x04005075 RID: 20597
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing;

			// Token: 0x04005076 RID: 20598
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State relaxing_together;

			// Token: 0x04005077 RID: 20599
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State post;
		}

		// Token: 0x02001435 RID: 5173
		public class ReadyStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x04005078 RID: 20600
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State idle;

			// Token: 0x04005079 RID: 20601
			public HotTub.States.OnStates on;
		}

		// Token: 0x02001436 RID: 5174
		public class FillingStates : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State
		{
			// Token: 0x0400507A RID: 20602
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State normal;

			// Token: 0x0400507B RID: 20603
			public GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.State too_cold;
		}
	}

	// Token: 0x02001438 RID: 5176
	public class StatesInstance : GameStateMachine<HotTub.States, HotTub.StatesInstance, HotTub, object>.GameInstance
	{
		// Token: 0x06006A2D RID: 27181 RVA: 0x000EA0A4 File Offset: 0x000E82A4
		public StatesInstance(HotTub smi) : base(smi)
		{
			this.operational = base.master.GetComponent<Operational>();
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x000EA0BE File Offset: 0x000E82BE
		public void SetActive(bool active)
		{
			this.operational.SetActive(this.operational.IsOperational && active, false);
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x002EB790 File Offset: 0x002E9990
		public void UpdateWaterMeter()
		{
			base.smi.master.waterMeter.SetPositionPercent(Mathf.Clamp(base.smi.master.waterStorage.GetMassAvailable(SimHashes.Water) / base.smi.master.hotTubCapacity, 0f, 1f));
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x002EB7EC File Offset: 0x002E99EC
		public void UpdateTemperatureMeter(float waterTemp)
		{
			Element element = ElementLoader.GetElement(SimHashes.Water.CreateTag());
			base.smi.master.tempMeter.SetPositionPercent(Mathf.Clamp((waterTemp - base.smi.master.minimumWaterTemperature) / (element.highTemp - base.smi.master.minimumWaterTemperature), 0f, 1f));
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x002EB858 File Offset: 0x002E9A58
		public void TestWaterTemperature()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(1836671383));
			float num = 0f;
			if (!gameObject)
			{
				this.UpdateTemperatureMeter(num);
				base.smi.sm.waterTooCold.Set(false, base.smi, false);
				return;
			}
			num = gameObject.GetComponent<PrimaryElement>().Temperature;
			this.UpdateTemperatureMeter(num);
			if (num < base.smi.master.minimumWaterTemperature)
			{
				base.smi.sm.waterTooCold.Set(true, base.smi, false);
				return;
			}
			base.smi.sm.waterTooCold.Set(false, base.smi, false);
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x000EA0D9 File Offset: 0x000E82D9
		public bool IsTubTooHot()
		{
			return base.smi.master.GetComponent<PrimaryElement>().Temperature > base.smi.master.maxOperatingTemperature;
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x002EB91C File Offset: 0x002E9B1C
		public bool HasBleachStone()
		{
			GameObject gameObject = base.smi.master.waterStorage.FindFirst(new Tag(-839728230));
			return gameObject != null && gameObject.GetComponent<PrimaryElement>().Mass > 0f;
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x002EB968 File Offset: 0x002E9B68
		public void SapHeatFromWater(float dt)
		{
			float num = base.smi.master.waterCoolingRate * dt / (float)base.smi.master.waterStorage.items.Count;
			foreach (GameObject gameObject in base.smi.master.waterStorage.items)
			{
				GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, base.smi.master.minimumWaterTemperature);
				GameUtil.DeltaThermalEnergy(base.GetComponent<PrimaryElement>(), num, base.GetComponent<PrimaryElement>().Element.highTemp);
			}
		}

		// Token: 0x06006A35 RID: 27189 RVA: 0x000EA102 File Offset: 0x000E8302
		public void ConsumeBleachstone(float dt)
		{
			base.smi.master.waterStorage.ConsumeIgnoringDisease(new Tag(-839728230), base.smi.master.bleachStoneConsumption * dt);
		}

		// Token: 0x04005098 RID: 20632
		private Operational operational;
	}
}
