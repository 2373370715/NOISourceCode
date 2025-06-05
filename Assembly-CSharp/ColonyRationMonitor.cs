using System;
using System.Collections.Generic;

// Token: 0x02001582 RID: 5506
public class ColonyRationMonitor : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance>
{
	// Token: 0x060072B4 RID: 29364 RVA: 0x0030D684 File Offset: 0x0030B884
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Update("UpdateOutOfRations", delegate(ColonyRationMonitor.Instance smi, float dt)
		{
			smi.UpdateIsOutOfRations();
		}, UpdateRate.SIM_200ms, false);
		this.satisfied.ParamTransition<bool>(this.isOutOfRations, this.outofrations, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsTrue).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
		this.outofrations.ParamTransition<bool>(this.isOutOfRations, this.satisfied, GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.IsFalse).TriggerOnEnter(GameHashes.ColonyHasRationsChanged, null);
	}

	// Token: 0x04005602 RID: 22018
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005603 RID: 22019
	public GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.State outofrations;

	// Token: 0x04005604 RID: 22020
	private StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter isOutOfRations = new StateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001583 RID: 5507
	public new class Instance : GameStateMachine<ColonyRationMonitor, ColonyRationMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060072B6 RID: 29366 RVA: 0x000EFA4E File Offset: 0x000EDC4E
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.UpdateIsOutOfRations();
		}

		// Token: 0x060072B7 RID: 29367 RVA: 0x0030D71C File Offset: 0x0030B91C
		public void UpdateIsOutOfRations()
		{
			bool value = true;
			using (List<Edible>.Enumerator enumerator = Components.Edibles.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent<Pickupable>().UnreservedAmount > 0f)
					{
						value = false;
						break;
					}
				}
			}
			base.smi.sm.isOutOfRations.Set(value, base.smi, false);
		}

		// Token: 0x060072B8 RID: 29368 RVA: 0x000EFA5D File Offset: 0x000EDC5D
		public bool IsOutOfRations()
		{
			return base.smi.sm.isOutOfRations.Get(base.smi);
		}
	}
}
