using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020016A2 RID: 5794
[SkipSaveFileSerialization]
public class PrefersWarmer : StateMachineComponent<PrefersWarmer.StatesInstance>
{
	// Token: 0x06007799 RID: 30617 RVA: 0x000F3269 File Offset: 0x000F1469
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020016A3 RID: 5795
	public class StatesInstance : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer, object>.GameInstance
	{
		// Token: 0x0600779B RID: 30619 RVA: 0x000F327E File Offset: 0x000F147E
		public StatesInstance(PrefersWarmer master) : base(master)
		{
		}
	}

	// Token: 0x020016A4 RID: 5796
	public class States : GameStateMachine<PrefersWarmer.States, PrefersWarmer.StatesInstance, PrefersWarmer>
	{
		// Token: 0x0600779C RID: 30620 RVA: 0x000F3287 File Offset: 0x000F1487
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (PrefersWarmer.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04005A0B RID: 23051
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.SKINNY, DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, false, false, true);
	}
}
