using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001779 RID: 6009
[SkipSaveFileSerialization]
public class RadiationEater : StateMachineComponent<RadiationEater.StatesInstance>
{
	// Token: 0x06007BA4 RID: 31652 RVA: 0x000F5C1C File Offset: 0x000F3E1C
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0200177A RID: 6010
	public class StatesInstance : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater, object>.GameInstance
	{
		// Token: 0x06007BA6 RID: 31654 RVA: 0x000F5C31 File Offset: 0x000F3E31
		public StatesInstance(RadiationEater master) : base(master)
		{
			this.radiationEating = new AttributeModifier(Db.Get().Attributes.RadiationRecovery.Id, TRAITS.RADIATION_EATER_RECOVERY, DUPLICANTS.TRAITS.RADIATIONEATER.NAME, false, false, true);
		}

		// Token: 0x06007BA7 RID: 31655 RVA: 0x0032B494 File Offset: 0x00329694
		public void OnEatRads(float radsEaten)
		{
			float delta = Mathf.Abs(radsEaten) * TRAITS.RADS_TO_CALS;
			base.smi.master.gameObject.GetAmounts().Get(Db.Get().Amounts.Calories).ApplyDelta(delta);
		}

		// Token: 0x04005D2D RID: 23853
		public AttributeModifier radiationEating;
	}

	// Token: 0x0200177B RID: 6011
	public class States : GameStateMachine<RadiationEater.States, RadiationEater.StatesInstance, RadiationEater>
	{
		// Token: 0x06007BA8 RID: 31656 RVA: 0x0032B4E0 File Offset: 0x003296E0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.ToggleAttributeModifier("Radiation Eating", (RadiationEater.StatesInstance smi) => smi.radiationEating, null).EventHandler(GameHashes.RadiationRecovery, delegate(RadiationEater.StatesInstance smi, object data)
			{
				float radsEaten = (float)data;
				smi.OnEatRads(radsEaten);
			});
		}
	}
}
