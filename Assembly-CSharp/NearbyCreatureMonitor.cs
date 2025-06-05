using System;
using System.Collections.Generic;

// Token: 0x020011F0 RID: 4592
public class NearbyCreatureMonitor : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget>
{
	// Token: 0x06005D53 RID: 23891 RVA: 0x000E14F1 File Offset: 0x000DF6F1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update("UpdateNearbyCreatures", delegate(NearbyCreatureMonitor.Instance smi, float dt)
		{
			smi.UpdateNearbyCreatures(dt);
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x020011F1 RID: 4593
	public new class Instance : GameStateMachine<NearbyCreatureMonitor, NearbyCreatureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x14000019 RID: 25
		// (add) Token: 0x06005D55 RID: 23893 RVA: 0x002AC37C File Offset: 0x002AA57C
		// (remove) Token: 0x06005D56 RID: 23894 RVA: 0x002AC3B4 File Offset: 0x002AA5B4
		public event Action<float, List<KPrefabID>, List<KPrefabID>> OnUpdateNearbyCreatures;

		// Token: 0x06005D57 RID: 23895 RVA: 0x000E1535 File Offset: 0x000DF735
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x002AC3EC File Offset: 0x002AA5EC
		public void UpdateNearbyCreatures(float dt)
		{
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
			if (cavityForCell != null)
			{
				this.OnUpdateNearbyCreatures(dt, cavityForCell.creatures, cavityForCell.eggs);
			}
		}
	}
}
