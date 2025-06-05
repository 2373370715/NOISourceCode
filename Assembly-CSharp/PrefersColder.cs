using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x0200169F RID: 5791
[SkipSaveFileSerialization]
public class PrefersColder : StateMachineComponent<PrefersColder.StatesInstance>
{
	// Token: 0x06007793 RID: 30611 RVA: 0x000F31DD File Offset: 0x000F13DD
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020016A0 RID: 5792
	public class StatesInstance : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder, object>.GameInstance
	{
		// Token: 0x06007795 RID: 30613 RVA: 0x000F31F2 File Offset: 0x000F13F2
		public StatesInstance(PrefersColder master) : base(master)
		{
		}
	}

	// Token: 0x020016A1 RID: 5793
	public class States : GameStateMachine<PrefersColder.States, PrefersColder.StatesInstance, PrefersColder>
	{
		// Token: 0x06007796 RID: 30614 RVA: 0x000F31FB File Offset: 0x000F13FB
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier(DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (PrefersColder.StatesInstance smi) => this.modifier, null);
		}

		// Token: 0x04005A0A RID: 23050
		private AttributeModifier modifier = new AttributeModifier("ThermalConductivityBarrier", DUPLICANTSTATS.STANDARD.Temperature.Conductivity_Barrier_Modification.PUDGY, DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, false, false, true);
	}
}
