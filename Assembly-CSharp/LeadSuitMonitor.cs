using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020014CF RID: 5327
public class LeadSuitMonitor : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance>
{
	// Token: 0x06006E42 RID: 28226 RVA: 0x002FD034 File Offset: 0x002FB234
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.wearingSuit;
		base.Target(this.owner);
		this.wearingSuit.DefaultState(this.wearingSuit.hasBattery);
		this.wearingSuit.hasBattery.Update(new Action<LeadSuitMonitor.Instance, float>(LeadSuitMonitor.CoolSuit), UpdateRate.SIM_200ms, false).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.noBattery, false);
		this.wearingSuit.noBattery.Enter(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Add(modifier);
				}
			}
		}).Exit(delegate(LeadSuitMonitor.Instance smi)
		{
			Attributes attributes = smi.sm.owner.Get(smi).GetAttributes();
			if (attributes != null)
			{
				foreach (AttributeModifier modifier in smi.noBatteryModifiers)
				{
					attributes.Remove(modifier);
				}
			}
		}).TagTransition(GameTags.SuitBatteryOut, this.wearingSuit.hasBattery, true);
	}

	// Token: 0x06006E43 RID: 28227 RVA: 0x002FD10C File Offset: 0x002FB30C
	public static void CoolSuit(LeadSuitMonitor.Instance smi, float dt)
	{
		if (!smi.navigator)
		{
			return;
		}
		GameObject gameObject = smi.sm.owner.Get(smi);
		if (!gameObject)
		{
			return;
		}
		ScaldingMonitor.Instance smi2 = gameObject.GetSMI<ScaldingMonitor.Instance>();
		if (smi2 != null && smi2.AverageExternalTemperature >= smi.lead_suit_tank.coolingOperationalTemperature)
		{
			smi.lead_suit_tank.batteryCharge -= 1f / smi.lead_suit_tank.batteryDuration * dt;
			if (smi.lead_suit_tank.IsEmpty())
			{
				gameObject.AddTag(GameTags.SuitBatteryOut);
			}
		}
	}

	// Token: 0x04005324 RID: 21284
	public LeadSuitMonitor.WearingSuit wearingSuit;

	// Token: 0x04005325 RID: 21285
	public StateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.TargetParameter owner;

	// Token: 0x020014D0 RID: 5328
	public class WearingSuit : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005326 RID: 21286
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State hasBattery;

		// Token: 0x04005327 RID: 21287
		public GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.State noBattery;
	}

	// Token: 0x020014D1 RID: 5329
	public new class Instance : GameStateMachine<LeadSuitMonitor, LeadSuitMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006E46 RID: 28230 RVA: 0x002FD19C File Offset: 0x002FB39C
		public Instance(IStateMachineTarget master, GameObject owner) : base(master)
		{
			base.sm.owner.Set(owner, base.smi, false);
			this.navigator = owner.GetComponent<Navigator>();
			this.lead_suit_tank = master.GetComponent<LeadSuitTank>();
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.INSULATION, (float)(-(float)TUNING.EQUIPMENT.SUITS.LEADSUIT_INSULATION), STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
			this.noBatteryModifiers.Add(new AttributeModifier(TUNING.EQUIPMENT.ATTRIBUTE_MOD_IDS.THERMAL_CONDUCTIVITY_BARRIER, -TUNING.EQUIPMENT.SUITS.LEADSUIT_THERMAL_CONDUCTIVITY_BARRIER, STRINGS.EQUIPMENT.PREFABS.LEAD_SUIT.SUIT_OUT_OF_BATTERIES, false, false, true));
		}

		// Token: 0x04005328 RID: 21288
		public Navigator navigator;

		// Token: 0x04005329 RID: 21289
		public LeadSuitTank lead_suit_tank;

		// Token: 0x0400532A RID: 21290
		public List<AttributeModifier> noBatteryModifiers = new List<AttributeModifier>();
	}
}
