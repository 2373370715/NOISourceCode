using System;
using UnityEngine;

// Token: 0x02001197 RID: 4503
public class CropTendingMonitor : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>
{
	// Token: 0x06005BA3 RID: 23459 RVA: 0x000E01DA File Offset: 0x000DE3DA
	private bool InterestedInTendingCrops(CropTendingMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.Hungry) || UnityEngine.Random.value <= smi.def.unsatisfiedTendChance;
	}

	// Token: 0x06005BA4 RID: 23460 RVA: 0x002A6894 File Offset: 0x002A4A94
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.cooldown.ParamTransition<float>(this.cooldownTimer, this.lookingForCrop, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && this.InterestedInTendingCrops(smi)).ParamTransition<float>(this.cooldownTimer, this.reset, (CropTendingMonitor.Instance smi, float p) => this.cooldownTimer.Get(smi) <= 0f && !this.InterestedInTendingCrops(smi)).Update(delegate(CropTendingMonitor.Instance smi, float dt)
		{
			this.cooldownTimer.Delta(-dt, smi);
		}, UpdateRate.SIM_1000ms, false);
		this.lookingForCrop.ToggleBehaviour(GameTags.Creatures.WantsToTendCrops, (CropTendingMonitor.Instance smi) => true, delegate(CropTendingMonitor.Instance smi)
		{
			smi.GoTo(this.reset);
		});
		this.reset.Exit(delegate(CropTendingMonitor.Instance smi)
		{
			this.cooldownTimer.Set(600f / smi.def.numCropsTendedPerCycle, smi, false);
		}).GoTo(this.cooldown);
	}

	// Token: 0x04004133 RID: 16691
	private StateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.FloatParameter cooldownTimer;

	// Token: 0x04004134 RID: 16692
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State cooldown;

	// Token: 0x04004135 RID: 16693
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State lookingForCrop;

	// Token: 0x04004136 RID: 16694
	private GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.State reset;

	// Token: 0x02001198 RID: 4504
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004137 RID: 16695
		public float numCropsTendedPerCycle = 8f;

		// Token: 0x04004138 RID: 16696
		public float unsatisfiedTendChance = 0.5f;
	}

	// Token: 0x02001199 RID: 4505
	public new class Instance : GameStateMachine<CropTendingMonitor, CropTendingMonitor.Instance, IStateMachineTarget, CropTendingMonitor.Def>.GameInstance
	{
		// Token: 0x06005BAC RID: 23468 RVA: 0x000E02A3 File Offset: 0x000DE4A3
		public Instance(IStateMachineTarget master, CropTendingMonitor.Def def) : base(master, def)
		{
		}
	}
}
