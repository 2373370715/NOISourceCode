using System;
using Klei.AI;
using STRINGS;
using TUNING;

// Token: 0x020013CB RID: 5067
[SkipSaveFileSerialization]
public class GlowStick : StateMachineComponent<GlowStick.StatesInstance>
{
	// Token: 0x0600680B RID: 26635 RVA: 0x000E87A1 File Offset: 0x000E69A1
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x020013CC RID: 5068
	public class StatesInstance : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick, object>.GameInstance
	{
		// Token: 0x0600680D RID: 26637 RVA: 0x002E56F4 File Offset: 0x002E38F4
		public StatesInstance(GlowStick master) : base(master)
		{
			this._radiationEmitter.emitRads = 100f;
			this._radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
			this._radiationEmitter.emitRate = 0.5f;
			this._radiationEmitter.emitRadiusX = 3;
			this._radiationEmitter.emitRadiusY = 3;
			this.radiationResistance = new AttributeModifier(Db.Get().Attributes.RadiationResistance.Id, TRAITS.GLOWSTICK_RADIATION_RESISTANCE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
			this.luminescenceModifier = new AttributeModifier(Db.Get().Attributes.Luminescence.Id, TRAITS.GLOWSTICK_LUX_VALUE, DUPLICANTS.TRAITS.GLOWSTICK.NAME, false, false, true);
		}

		// Token: 0x04004E95 RID: 20117
		[MyCmpAdd]
		private RadiationEmitter _radiationEmitter;

		// Token: 0x04004E96 RID: 20118
		public AttributeModifier radiationResistance;

		// Token: 0x04004E97 RID: 20119
		public AttributeModifier luminescenceModifier;
	}

	// Token: 0x020013CD RID: 5069
	public class States : GameStateMachine<GlowStick.States, GlowStick.StatesInstance, GlowStick>
	{
		// Token: 0x0600680E RID: 26638 RVA: 0x002E57B0 File Offset: 0x002E39B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleComponent<RadiationEmitter>(false).ToggleAttributeModifier("Radiation Resistance", (GlowStick.StatesInstance smi) => smi.radiationResistance, null).ToggleAttributeModifier("Luminescence Modifier", (GlowStick.StatesInstance smi) => smi.luminescenceModifier, null);
		}
	}
}
