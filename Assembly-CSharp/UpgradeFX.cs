using System;
using UnityEngine;

// Token: 0x02000C3C RID: 3132
public class UpgradeFX : GameStateMachine<UpgradeFX, UpgradeFX.Instance>
{
	// Token: 0x06003B37 RID: 15159 RVA: 0x00237E30 File Offset: 0x00236030
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.PlayAnim("upgrade").OnAnimQueueComplete(null).ToggleReactable((UpgradeFX.Instance smi) => smi.CreateReactable()).Exit("DestroyFX", delegate(UpgradeFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028FC RID: 10492
	public StateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x02000C3D RID: 3133
	public new class Instance : GameStateMachine<UpgradeFX, UpgradeFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003B39 RID: 15161 RVA: 0x00237EB8 File Offset: 0x002360B8
		public Instance(IStateMachineTarget master, Vector3 offset) : base(master)
		{
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("upgrade_fx_kanim", master.gameObject.transform.GetPosition() + offset, master.gameObject.transform, true, Grid.SceneLayer.Front, false);
			base.sm.fx.Set(kbatchedAnimController.gameObject, base.smi, false);
		}

		// Token: 0x06003B3A RID: 15162 RVA: 0x000CAAC4 File Offset: 0x000C8CC4
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x06003B3B RID: 15163 RVA: 0x00237F1C File Offset: 0x0023611C
		public Reactable CreateReactable()
		{
			return new EmoteReactable(base.master.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
		}
	}
}
