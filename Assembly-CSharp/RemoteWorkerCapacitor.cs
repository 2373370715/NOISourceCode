using System;
using KSerialization;
using UnityEngine;

// Token: 0x020017A5 RID: 6053
public class RemoteWorkerCapacitor : StateMachineComponent<RemoteWorkerCapacitor.StatesInstance>
{
	// Token: 0x06007C6C RID: 31852 RVA: 0x000F64FF File Offset: 0x000F46FF
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06007C6D RID: 31853 RVA: 0x0032D9C4 File Offset: 0x0032BBC4
	public float ApplyDeltaEnergy(float delta)
	{
		float num = this.charge;
		this.charge = Mathf.Clamp(this.charge + delta, 0f, 60f);
		return this.charge - num;
	}

	// Token: 0x170007CD RID: 1997
	// (get) Token: 0x06007C6E RID: 31854 RVA: 0x000F6512 File Offset: 0x000F4712
	public float ChargeRatio
	{
		get
		{
			return this.charge / 60f;
		}
	}

	// Token: 0x170007CE RID: 1998
	// (get) Token: 0x06007C6F RID: 31855 RVA: 0x000F6520 File Offset: 0x000F4720
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x170007CF RID: 1999
	// (get) Token: 0x06007C70 RID: 31856 RVA: 0x000F6528 File Offset: 0x000F4728
	public bool IsLowPower
	{
		get
		{
			return this.charge < 12f;
		}
	}

	// Token: 0x170007D0 RID: 2000
	// (get) Token: 0x06007C71 RID: 31857 RVA: 0x000F6537 File Offset: 0x000F4737
	public bool IsOutOfPower
	{
		get
		{
			return this.charge < float.Epsilon;
		}
	}

	// Token: 0x04005DB3 RID: 23987
	[Serialize]
	private float charge;

	// Token: 0x04005DB4 RID: 23988
	public const float LOW_LEVEL = 12f;

	// Token: 0x04005DB5 RID: 23989
	public const float POWER_USE_RATE_J_PER_S = -0.1f;

	// Token: 0x04005DB6 RID: 23990
	public const float POWER_CHARGE_RATE_J_PER_S = 7.5f;

	// Token: 0x04005DB7 RID: 23991
	public const float CAPACITY_J = 60f;

	// Token: 0x020017A6 RID: 6054
	public class StatesInstance : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.GameInstance
	{
		// Token: 0x06007C73 RID: 31859 RVA: 0x000F654E File Offset: 0x000F474E
		public StatesInstance(RemoteWorkerCapacitor master) : base(master)
		{
		}
	}

	// Token: 0x020017A7 RID: 6055
	public class States : GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor>
	{
		// Token: 0x06007C74 RID: 31860 RVA: 0x0032DA00 File Offset: 0x0032BC00
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.InitializeStates(out default_state);
			default_state = this.ok;
			this.root.ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerCapacitorStatus, (RemoteWorkerCapacitor.StatesInstance smi) => smi.master);
			this.ok.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms);
			this.low_power.Transition(this.out_of_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOutOfPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerLowPower, null);
			this.out_of_power.Transition(this.low_power, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsLowPower), UpdateRate.SIM_200ms).Transition(this.ok, new StateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.Transition.ConditionCallback(RemoteWorkerCapacitor.States.IsOkForPower), UpdateRate.SIM_200ms).ToggleStatusItem(Db.Get().DuplicantStatusItems.RemoteWorkerOutOfPower, null);
		}

		// Token: 0x06007C75 RID: 31861 RVA: 0x000F6557 File Offset: 0x000F4757
		public static bool IsOkForPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return !smi.master.IsLowPower;
		}

		// Token: 0x06007C76 RID: 31862 RVA: 0x000F6567 File Offset: 0x000F4767
		public static bool IsLowPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsLowPower && !smi.master.IsOutOfPower;
		}

		// Token: 0x06007C77 RID: 31863 RVA: 0x000F6586 File Offset: 0x000F4786
		public static bool IsOutOfPower(RemoteWorkerCapacitor.StatesInstance smi)
		{
			return smi.master.IsOutOfPower;
		}

		// Token: 0x04005DB8 RID: 23992
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State ok;

		// Token: 0x04005DB9 RID: 23993
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State low_power;

		// Token: 0x04005DBA RID: 23994
		private GameStateMachine<RemoteWorkerCapacitor.States, RemoteWorkerCapacitor.StatesInstance, RemoteWorkerCapacitor, object>.State out_of_power;
	}
}
