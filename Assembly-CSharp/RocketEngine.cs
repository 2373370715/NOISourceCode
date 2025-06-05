using System;
using KSerialization;
using STRINGS;

// Token: 0x02001980 RID: 6528
[SerializationConfig(MemberSerialization.OptIn)]
public class RocketEngine : StateMachineComponent<RocketEngine.StatesInstance>
{
	// Token: 0x060087FA RID: 34810 RVA: 0x00360E54 File Offset: 0x0035F054
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.mainEngine)
		{
			base.GetComponent<RocketModule>().AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new RequireAttachedComponent(base.gameObject.GetComponent<AttachableBuilding>(), typeof(FuelTank), UI.STARMAP.COMPONENT.FUEL_TANK));
		}
	}

	// Token: 0x040066FD RID: 26365
	public float exhaustEmitRate = 50f;

	// Token: 0x040066FE RID: 26366
	public float exhaustTemperature = 1500f;

	// Token: 0x040066FF RID: 26367
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04006700 RID: 26368
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x04006701 RID: 26369
	public Tag fuelTag;

	// Token: 0x04006702 RID: 26370
	public float efficiency = 1f;

	// Token: 0x04006703 RID: 26371
	public bool requireOxidizer = true;

	// Token: 0x04006704 RID: 26372
	public bool mainEngine = true;

	// Token: 0x02001981 RID: 6529
	public class StatesInstance : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.GameInstance
	{
		// Token: 0x060087FC RID: 34812 RVA: 0x000FD8B8 File Offset: 0x000FBAB8
		public StatesInstance(RocketEngine smi) : base(smi)
		{
		}
	}

	// Token: 0x02001982 RID: 6530
	public class States : GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine>
	{
		// Token: 0x060087FD RID: 34813 RVA: 0x00360EFC File Offset: 0x0035F0FC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
			this.burning.EventTransition(GameHashes.RocketLanded, this.burnComplete, null).PlayAnim("launch_pre").QueueAnim("launch_loop", true, null).Update(delegate(RocketEngine.StatesInstance smi, float dt)
			{
				int num = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num))
				{
					SimMessages.EmitMass(num, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				int num2 = 10;
				for (int i = 1; i < num2; i++)
				{
					int num3 = Grid.OffsetCell(num, -1, -i);
					int num4 = Grid.OffsetCell(num, 0, -i);
					int num5 = Grid.OffsetCell(num, 1, -i);
					if (Grid.IsValidCell(num3))
					{
						SimMessages.ModifyEnergy(num3, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num4))
					{
						SimMessages.ModifyEnergy(num4, smi.master.exhaustTemperature / (float)i, 3200f, SimMessages.EnergySourceID.Burner);
					}
					if (Grid.IsValidCell(num5))
					{
						SimMessages.ModifyEnergy(num5, smi.master.exhaustTemperature / (float)(i + 1), 3200f, SimMessages.EnergySourceID.Burner);
					}
				}
			}, UpdateRate.SIM_200ms, false);
			this.burnComplete.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.IgniteEngine, this.burning, null);
		}

		// Token: 0x04006705 RID: 26373
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State idle;

		// Token: 0x04006706 RID: 26374
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burning;

		// Token: 0x04006707 RID: 26375
		public GameStateMachine<RocketEngine.States, RocketEngine.StatesInstance, RocketEngine, object>.State burnComplete;
	}
}
