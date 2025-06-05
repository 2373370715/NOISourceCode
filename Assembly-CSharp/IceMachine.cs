using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001445 RID: 5189
[SerializationConfig(MemberSerialization.OptIn)]
public class IceMachine : StateMachineComponent<IceMachine.StatesInstance>, FewOptionSideScreen.IFewOptionSideScreen
{
	// Token: 0x06006A62 RID: 27234 RVA: 0x000EA254 File Offset: 0x000E8454
	public void SetStorages(Storage waterStorage, Storage iceStorage)
	{
		this.waterStorage = waterStorage;
		this.iceStorage = iceStorage;
	}

	// Token: 0x06006A63 RID: 27235 RVA: 0x002EBD3C File Offset: 0x002E9F3C
	private bool CanMakeIce()
	{
		bool flag = this.waterStorage != null && this.waterStorage.GetMassAvailable(SimHashes.Water) >= 0.1f;
		bool flag2 = this.iceStorage != null && this.iceStorage.IsFull();
		return flag && !flag2;
	}

	// Token: 0x06006A64 RID: 27236 RVA: 0x002EBD9C File Offset: 0x002E9F9C
	private void MakeIce(IceMachine.StatesInstance smi, float dt)
	{
		float num = this.heatRemovalRate * dt / (float)this.waterStorage.items.Count;
		foreach (GameObject gameObject in this.waterStorage.items)
		{
			GameUtil.DeltaThermalEnergy(gameObject.GetComponent<PrimaryElement>(), -num, smi.master.targetTemperature);
		}
		for (int i = this.waterStorage.items.Count; i > 0; i--)
		{
			GameObject gameObject2 = this.waterStorage.items[i - 1];
			if (gameObject2 && gameObject2.GetComponent<PrimaryElement>().Temperature < gameObject2.GetComponent<PrimaryElement>().Element.lowTemp)
			{
				PrimaryElement component = gameObject2.GetComponent<PrimaryElement>();
				this.waterStorage.AddOre(this.targetProductionElement, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, false, true);
				this.waterStorage.ConsumeIgnoringDisease(gameObject2);
			}
		}
		smi.UpdateIceState();
	}

	// Token: 0x06006A65 RID: 27237 RVA: 0x000EA264 File Offset: 0x000E8464
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06006A66 RID: 27238 RVA: 0x002EBEC4 File Offset: 0x002EA0C4
	public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
	{
		FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[IceMachineConfig.ELEMENT_OPTIONS.Length];
		for (int i = 0; i < array.Length; i++)
		{
			string tooltipText = Strings.Get("STRINGS.BUILDINGS.PREFABS.ICEMACHINE.OPTION_TOOLTIPS." + IceMachineConfig.ELEMENT_OPTIONS[i].ToString().ToUpper());
			array[i] = new FewOptionSideScreen.IFewOptionSideScreen.Option(IceMachineConfig.ELEMENT_OPTIONS[i], ElementLoader.GetElement(IceMachineConfig.ELEMENT_OPTIONS[i]).name, Def.GetUISprite(IceMachineConfig.ELEMENT_OPTIONS[i], "ui", false), tooltipText);
		}
		return array;
	}

	// Token: 0x06006A67 RID: 27239 RVA: 0x000EA277 File Offset: 0x000E8477
	public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
	{
		this.targetProductionElement = ElementLoader.GetElementID(option.tag);
	}

	// Token: 0x06006A68 RID: 27240 RVA: 0x000EA28A File Offset: 0x000E848A
	public Tag GetSelectedOption()
	{
		return this.targetProductionElement.CreateTag();
	}

	// Token: 0x040050AE RID: 20654
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040050AF RID: 20655
	public Storage waterStorage;

	// Token: 0x040050B0 RID: 20656
	public Storage iceStorage;

	// Token: 0x040050B1 RID: 20657
	public float targetTemperature;

	// Token: 0x040050B2 RID: 20658
	public float heatRemovalRate;

	// Token: 0x040050B3 RID: 20659
	private static StatusItem iceStorageFullStatusItem;

	// Token: 0x040050B4 RID: 20660
	[Serialize]
	public SimHashes targetProductionElement = SimHashes.Ice;

	// Token: 0x02001446 RID: 5190
	public class StatesInstance : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.GameInstance
	{
		// Token: 0x06006A6A RID: 27242 RVA: 0x002EBF68 File Offset: 0x002EA168
		public StatesInstance(IceMachine smi) : base(smi)
		{
			this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_OL",
				"meter_frame",
				"meter_fill"
			});
			this.UpdateMeter();
			base.Subscribe(-1697596308, new Action<object>(this.OnStorageChange));
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x000EA2AA File Offset: 0x000E84AA
		private void OnStorageChange(object data)
		{
			this.UpdateMeter();
		}

		// Token: 0x06006A6C RID: 27244 RVA: 0x000EA2B2 File Offset: 0x000E84B2
		public void UpdateMeter()
		{
			this.meter.SetPositionPercent(Mathf.Clamp01(base.smi.master.iceStorage.MassStored() / base.smi.master.iceStorage.Capacity()));
		}

		// Token: 0x06006A6D RID: 27245 RVA: 0x002EBFDC File Offset: 0x002EA1DC
		public void UpdateIceState()
		{
			bool value = false;
			for (int i = base.smi.master.waterStorage.items.Count; i > 0; i--)
			{
				GameObject gameObject = base.smi.master.waterStorage.items[i - 1];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= base.smi.master.targetTemperature)
				{
					value = true;
				}
			}
			base.sm.doneFreezingIce.Set(value, this, false);
		}

		// Token: 0x040050B5 RID: 20661
		private MeterController meter;

		// Token: 0x040050B6 RID: 20662
		public Chore emptyChore;
	}

	// Token: 0x02001447 RID: 5191
	public class States : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine>
	{
		// Token: 0x06006A6E RID: 27246 RVA: 0x002EC06C File Offset: 0x002EA26C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (IceMachine.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (IceMachine.StatesInstance smi) => !smi.master.operational.IsOperational).DefaultState(this.on.waiting);
			this.on.waiting.EventTransition(GameHashes.OnStorageChange, this.on.working_pre, (IceMachine.StatesInstance smi) => smi.master.CanMakeIce());
			this.on.working_pre.Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.UpdateIceState();
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.on.working);
			this.on.working.QueueAnim("working_loop", true, null).Update("UpdateWorking", delegate(IceMachine.StatesInstance smi, float dt)
			{
				smi.master.MakeIce(smi, dt);
			}, UpdateRate.SIM_200ms, false).ParamTransition<bool>(this.doneFreezingIce, this.on.working_pst, GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.IsTrue).Enter(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(true, "Working");
			}).Exit(delegate(IceMachine.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
				smi.master.gameObject.GetComponent<ManualDeliveryKG>().Pause(false, "Done Working");
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.CoolingWater, null);
			this.on.working_pst.Exit(new StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State.Callback(this.DoTransfer)).PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x06006A6F RID: 27247 RVA: 0x002EC290 File Offset: 0x002EA490
		private void DoTransfer(IceMachine.StatesInstance smi)
		{
			for (int i = smi.master.waterStorage.items.Count - 1; i >= 0; i--)
			{
				GameObject gameObject = smi.master.waterStorage.items[i];
				if (gameObject && gameObject.GetComponent<PrimaryElement>().Temperature <= smi.master.targetTemperature)
				{
					smi.master.waterStorage.Transfer(gameObject, smi.master.iceStorage, false, true);
				}
			}
			smi.UpdateMeter();
		}

		// Token: 0x040050B7 RID: 20663
		public StateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.BoolParameter doneFreezingIce;

		// Token: 0x040050B8 RID: 20664
		public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State off;

		// Token: 0x040050B9 RID: 20665
		public IceMachine.States.OnStates on;

		// Token: 0x02001448 RID: 5192
		public class OnStates : GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State
		{
			// Token: 0x040050BA RID: 20666
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State waiting;

			// Token: 0x040050BB RID: 20667
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pre;

			// Token: 0x040050BC RID: 20668
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working;

			// Token: 0x040050BD RID: 20669
			public GameStateMachine<IceMachine.States, IceMachine.StatesInstance, IceMachine, object>.State working_pst;
		}
	}
}
