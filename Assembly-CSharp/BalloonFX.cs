using System;
using Database;
using UnityEngine;

// Token: 0x02000C21 RID: 3105
public class BalloonFX : GameStateMachine<BalloonFX, BalloonFX.Instance>
{
	// Token: 0x06003ADD RID: 15069 RVA: 0x00236840 File Offset: 0x00234A40
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.Target(this.fx);
		this.root.Exit("DestroyFX", delegate(BalloonFX.Instance smi)
		{
			smi.DestroyFX();
		});
	}

	// Token: 0x040028A9 RID: 10409
	public StateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x040028AA RID: 10410
	public KAnimFile defaultAnim = Assets.GetAnim("balloon_anim_kanim");

	// Token: 0x040028AB RID: 10411
	private KAnimFile defaultBalloon = Assets.GetAnim("balloon_basic_red_kanim");

	// Token: 0x040028AC RID: 10412
	private const string defaultAnimName = "balloon_anim_kanim";

	// Token: 0x040028AD RID: 10413
	private const string balloonAnimName = "balloon_basic_red_kanim";

	// Token: 0x040028AE RID: 10414
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x040028AF RID: 10415
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x02000C22 RID: 3106
	public new class Instance : GameStateMachine<BalloonFX, BalloonFX.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06003ADF RID: 15071 RVA: 0x00236894 File Offset: 0x00234A94
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.balloonAnimController = FXHelpers.CreateEffectOverride(new string[]
			{
				"balloon_anim_kanim",
				"balloon_basic_red_kanim"
			}, master.gameObject.transform.GetPosition() + new Vector3(0f, 0.3f, 1f), master.transform, true, Grid.SceneLayer.Creatures, false);
			base.sm.fx.Set(this.balloonAnimController.gameObject, base.smi, false);
			this.balloonAnimController.defaultAnim = "idle_default";
			master.GetComponent<KBatchedAnimController>().GetSynchronizer().Add(this.balloonAnimController.GetComponent<KBatchedAnimController>());
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0023694C File Offset: 0x00234B4C
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverride)
		{
			KAnimFile kanimFile = balloonOverride.animFile.IsSome() ? balloonOverride.animFile.Unwrap() : base.smi.sm.defaultBalloon;
			this.balloonAnimController.SwapAnims(new KAnimFile[]
			{
				base.smi.sm.defaultAnim,
				kanimFile
			});
			SymbolOverrideController component = this.balloonAnimController.GetComponent<SymbolOverrideController>();
			if (this.currentBodyOverrideSymbol.IsSome())
			{
				component.RemoveSymbolOverride("body", 0);
			}
			if (balloonOverride.symbol.IsNone())
			{
				if (this.currentBodyOverrideSymbol.IsSome())
				{
					component.AddSymbolOverride("body", base.smi.sm.defaultAnim.GetData().build.GetSymbol("body"), 0);
				}
				this.balloonAnimController.SetBatchGroupOverride(HashedString.Invalid);
			}
			else
			{
				component.AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
				this.balloonAnimController.SetBatchGroupOverride(kanimFile.batchTag);
			}
			this.currentBodyOverrideSymbol = balloonOverride;
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x000CA788 File Offset: 0x000C8988
		public void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x040028B0 RID: 10416
		private KBatchedAnimController balloonAnimController;

		// Token: 0x040028B1 RID: 10417
		private Option<BalloonOverrideSymbol> currentBodyOverrideSymbol;
	}
}
