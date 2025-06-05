using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000E38 RID: 3640
[SerializationConfig(MemberSerialization.OptIn)]
public class IceCooledFan : StateMachineComponent<IceCooledFan.StatesInstance>
{
	// Token: 0x06004720 RID: 18208 RVA: 0x000D281E File Offset: 0x000D0A1E
	public bool HasMaterial()
	{
		this.UpdateMeter();
		return this.iceStorage.MassStored() > 0f;
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x000D2838 File Offset: 0x000D0A38
	public void CheckWorking()
	{
		if (base.smi.master.workable.worker == null)
		{
			base.smi.GoTo(base.smi.sm.unworkable);
		}
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x0025F4F0 File Offset: 0x0025D6F0
	private void UpdateUnworkableStatusItems()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (!base.smi.EnvironmentNeedsCooling())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, this.minCooledTemperature);
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.CannotCoolFurther, false);
		}
		if (!base.smi.EnvironmentHighEnoughPressure())
		{
			if (!component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.UnderPressure, this.minEnvironmentMass);
				return;
			}
		}
		else if (component.HasStatusItem(Db.Get().BuildingStatusItems.UnderPressure))
		{
			component.RemoveStatusItem(Db.Get().BuildingStatusItems.UnderPressure, false);
		}
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x0025F5F0 File Offset: 0x0025D7F0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_waterbody",
			"meter_waterlevel"
		});
		base.smi.StartSM();
		base.GetComponent<ManualDeliveryKG>().SetStorage(this.iceStorage);
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x0025F65C File Offset: 0x0025D85C
	private void UpdateMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			num += component.Temperature;
		}
		num /= (float)this.iceStorage.items.Count;
		float num2 = Mathf.Clamp01((num - this.LOW_ICE_TEMP) / (this.targetTemperature - this.LOW_ICE_TEMP));
		this.meter.SetPositionPercent(1f - num2);
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x0025F704 File Offset: 0x0025D904
	private void DoCooling(float dt)
	{
		float kilowatts = this.coolingRate * dt;
		foreach (GameObject gameObject in this.iceStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), kilowatts, this.targetTemperature);
		}
		for (int i = this.iceStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.iceStorage.items[i - 1];
			if (gameObject2 != null && gameObject2.GetComponent<PrimaryElement>().Temperature > gameObject2.GetComponent<PrimaryElement>().Element.highTemp && gameObject2.GetComponent<PrimaryElement>().Element.HasTransitionUp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.iceStorage.AddLiquid(component.Element.highTempTransitionTarget, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.iceStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		for (int j = this.iceStorage.items.Count; j > 0; j--)
		{
			GameObject gameObject3 = this.iceStorage.items[j - 1];
			if (gameObject3 != null && gameObject3.GetComponent<PrimaryElement>().Temperature >= this.targetTemperature)
			{
				this.iceStorage.Transfer(gameObject3, this.liquidStorage, true, true);
			}
		}
		if (!this.liquidStorage.IsEmpty())
		{
			this.liquidStorage.DropAll(false, false, new Vector3(1f, 0f, 0f), true, null);
		}
		this.UpdateMeter();
	}

	// Token: 0x040031B4 RID: 12724
	[SerializeField]
	public float minCooledTemperature;

	// Token: 0x040031B5 RID: 12725
	[SerializeField]
	public float minEnvironmentMass;

	// Token: 0x040031B6 RID: 12726
	[SerializeField]
	public float coolingRate;

	// Token: 0x040031B7 RID: 12727
	[SerializeField]
	public float targetTemperature;

	// Token: 0x040031B8 RID: 12728
	[SerializeField]
	public Vector2I minCoolingRange;

	// Token: 0x040031B9 RID: 12729
	[SerializeField]
	public Vector2I maxCoolingRange;

	// Token: 0x040031BA RID: 12730
	[SerializeField]
	public Storage iceStorage;

	// Token: 0x040031BB RID: 12731
	[SerializeField]
	public Storage liquidStorage;

	// Token: 0x040031BC RID: 12732
	[SerializeField]
	public Tag consumptionTag;

	// Token: 0x040031BD RID: 12733
	[MyCmpAdd]
	private ManuallySetRemoteWorkTargetComponent remoteChore;

	// Token: 0x040031BE RID: 12734
	private float LOW_ICE_TEMP = 173.15f;

	// Token: 0x040031BF RID: 12735
	[MyCmpAdd]
	private IceCooledFanWorkable workable;

	// Token: 0x040031C0 RID: 12736
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040031C1 RID: 12737
	private MeterController meter;

	// Token: 0x02000E39 RID: 3641
	public class StatesInstance : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.GameInstance
	{
		// Token: 0x06004727 RID: 18215 RVA: 0x000D2885 File Offset: 0x000D0A85
		public StatesInstance(IceCooledFan smi) : base(smi)
		{
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x0025F8C4 File Offset: 0x0025DAC4
		public bool IsWorkable()
		{
			bool result = false;
			if (base.master.operational.IsOperational && this.EnvironmentNeedsCooling() && base.smi.master.HasMaterial() && base.smi.EnvironmentHighEnoughPressure())
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0025F910 File Offset: 0x0025DB10
		public bool EnvironmentNeedsCooling()
		{
			bool result = false;
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Temperature[i2] > base.master.minCooledTemperature)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0025F9B8 File Offset: 0x0025DBB8
		public bool EnvironmentHighEnoughPressure()
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			for (int i = base.master.minCoolingRange.y; i < base.master.maxCoolingRange.y; i++)
			{
				for (int j = base.master.minCoolingRange.x; j < base.master.maxCoolingRange.x; j++)
				{
					CellOffset offset = new CellOffset(j, i);
					int i2 = Grid.OffsetCell(cell, offset);
					if (Grid.Mass[i2] >= base.master.minEnvironmentMass)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	// Token: 0x02000E3A RID: 3642
	public class States : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan>
	{
		// Token: 0x0600472B RID: 18219 RVA: 0x0025FA58 File Offset: 0x0025DC58
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unworkable;
			this.root.Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.workable.SetWorkTime(float.PositiveInfinity);
			});
			this.workable.ToggleChore(new Func<IceCooledFan.StatesInstance, Chore>(IceCooledFan.States.CreateUseChore), new Action<IceCooledFan.StatesInstance, Chore>(IceCooledFan.States.SetRemoteChore), this.work_pst).EventTransition(GameHashes.ActiveChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).EventTransition(GameHashes.OperationalChanged, this.workable.cooling, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker != null).Transition(this.unworkable, (IceCooledFan.StatesInstance smi) => !smi.IsWorkable(), UpdateRate.SIM_200ms);
			this.workable.cooling.EventTransition(GameHashes.OperationalChanged, this.unworkable, (IceCooledFan.StatesInstance smi) => smi.master.workable.worker == null).EventHandler(GameHashes.ActiveChanged, delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.CheckWorking();
			}).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
				if (!smi.EnvironmentNeedsCooling() || !smi.master.HasMaterial() || !smi.EnvironmentHighEnoughPressure())
				{
					smi.GoTo(this.unworkable);
				}
			}).Update("IceCooledFanCooling", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.DoCooling(dt);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				if (!smi.master.HasMaterial())
				{
					smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Working");
				}
				smi.master.liquidStorage.DropAll(false, false, default(Vector3), true, null);
			});
			this.work_pst.ScheduleGoTo(2f, this.unworkable);
			this.unworkable.Update("IceFanUnworkableStatusItems", delegate(IceCooledFan.StatesInstance smi, float dt)
			{
				smi.master.UpdateUnworkableStatusItems();
			}, UpdateRate.SIM_200ms, false).Transition(this.workable.waiting, (IceCooledFan.StatesInstance smi) => smi.IsWorkable(), UpdateRate.SIM_200ms).Enter(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			}).Exit(delegate(IceCooledFan.StatesInstance smi)
			{
				smi.master.UpdateUnworkableStatusItems();
			});
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x000D288E File Offset: 0x000D0A8E
		private static void SetRemoteChore(IceCooledFan.StatesInstance smi, Chore chore)
		{
			smi.master.remoteChore.SetChore(chore);
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0025FCDC File Offset: 0x0025DEDC
		private static Chore CreateUseChore(IceCooledFan.StatesInstance smi)
		{
			return new WorkChore<IceCooledFanWorkable>(Db.Get().ChoreTypes.IceCooledFan, smi.master.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x040031C2 RID: 12738
		public IceCooledFan.States.Workable workable;

		// Token: 0x040031C3 RID: 12739
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State unworkable;

		// Token: 0x040031C4 RID: 12740
		public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State work_pst;

		// Token: 0x02000E3B RID: 3643
		public class Workable : GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State
		{
			// Token: 0x040031C5 RID: 12741
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State waiting;

			// Token: 0x040031C6 RID: 12742
			public GameStateMachine<IceCooledFan.States, IceCooledFan.StatesInstance, IceCooledFan, object>.State cooling;
		}
	}
}
