using System;
using UnityEngine;

// Token: 0x02000D8D RID: 3469
public class ElectrobankCharger : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>
{
	// Token: 0x06004364 RID: 17252 RVA: 0x00252BC8 File Offset: 0x00250DC8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.noBattery;
		this.noBattery.PlayAnim("off").EventHandler(GameHashes.OnStorageChange, delegate(ElectrobankCharger.Instance smi, object data)
		{
			smi.QueueElectrobank(null);
		}).ParamTransition<bool>(this.hasElectrobank, this.charging, GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.IsTrue).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
		});
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.charging, (ElectrobankCharger.Instance smi) => smi.master.GetComponent<Operational>().IsOperational);
		this.charging.QueueAnim("working_pre", false, null).QueueAnim("working_loop", true, null).Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.QueueElectrobank(null);
			smi.master.GetComponent<Operational>().SetActive(true, false);
		}).Exit(delegate(ElectrobankCharger.Instance smi)
		{
			smi.master.GetComponent<Operational>().SetActive(false, false);
		}).ToggleStatusItem(Db.Get().BuildingStatusItems.PowerBankChargerInProgress, null).Update(delegate(ElectrobankCharger.Instance smi, float dt)
		{
			smi.ChargeInternal(smi, dt);
		}, UpdateRate.SIM_EVERY_TICK, false).EventTransition(GameHashes.OperationalChanged, this.inoperational, (ElectrobankCharger.Instance smi) => !smi.master.GetComponent<Operational>().IsOperational).ParamTransition<float>(this.internalChargeAmount, this.full, (ElectrobankCharger.Instance smi, float dt) => this.internalChargeAmount.Get(smi) >= 120000f);
		this.full.PlayAnim("working_pst").Enter(delegate(ElectrobankCharger.Instance smi)
		{
			smi.TransferChargeToElectrobank();
		}).OnAnimQueueComplete(this.noBattery);
	}

	// Token: 0x04002E99 RID: 11929
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State noBattery;

	// Token: 0x04002E9A RID: 11930
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State inoperational;

	// Token: 0x04002E9B RID: 11931
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State charging;

	// Token: 0x04002E9C RID: 11932
	public GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.State full;

	// Token: 0x04002E9D RID: 11933
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.FloatParameter internalChargeAmount;

	// Token: 0x04002E9E RID: 11934
	public StateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.BoolParameter hasElectrobank;

	// Token: 0x02000D8E RID: 3470
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D8F RID: 3471
	public new class Instance : GameStateMachine<ElectrobankCharger, ElectrobankCharger.Instance, IStateMachineTarget, ElectrobankCharger.Def>.GameInstance
	{
		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06004368 RID: 17256 RVA: 0x000CFF98 File Offset: 0x000CE198
		public Storage Storage
		{
			get
			{
				if (this.storage == null)
				{
					this.storage = base.GetComponent<Storage>();
				}
				return this.storage;
			}
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x000CFFBA File Offset: 0x000CE1BA
		public Instance(IStateMachineTarget master, ElectrobankCharger.Def def) : base(master, def)
		{
			this.meterController = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
		}

		// Token: 0x0600436A RID: 17258 RVA: 0x000CFFE7 File Offset: 0x000CE1E7
		public void ChargeInternal(ElectrobankCharger.Instance smi, float dt)
		{
			smi.sm.internalChargeAmount.Delta(dt * 400f, smi);
			this.UpdateMeter();
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x000D0008 File Offset: 0x000CE208
		public void UpdateMeter()
		{
			this.meterController.SetPositionPercent(base.sm.internalChargeAmount.Get(base.smi) / 120000f);
		}

		// Token: 0x0600436C RID: 17260 RVA: 0x000D0031 File Offset: 0x000CE231
		public void TransferChargeToElectrobank()
		{
			this.targetElectrobank = Electrobank.ReplaceEmptyWithCharged(this.targetElectrobank, true);
			this.DequeueElectrobank();
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00252DC8 File Offset: 0x00250FC8
		public void DequeueElectrobank()
		{
			this.targetElectrobank = null;
			base.smi.sm.hasElectrobank.Set(false, base.smi, false);
			base.smi.sm.internalChargeAmount.Set(0f, base.smi, false);
			this.UpdateMeter();
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00252E24 File Offset: 0x00251024
		public void QueueElectrobank(object data = null)
		{
			if (this.targetElectrobank == null)
			{
				for (int i = 0; i < this.Storage.items.Count; i++)
				{
					GameObject gameObject = this.Storage.items[i];
					if (gameObject != null && gameObject.HasTag(GameTags.EmptyPortableBattery))
					{
						this.targetElectrobank = gameObject;
						base.smi.sm.hasElectrobank.Set(true, base.smi, false);
						break;
					}
				}
			}
			this.UpdateMeter();
		}

		// Token: 0x04002E9F RID: 11935
		private Storage storage;

		// Token: 0x04002EA0 RID: 11936
		public GameObject targetElectrobank;

		// Token: 0x04002EA1 RID: 11937
		private MeterController meterController;
	}
}
