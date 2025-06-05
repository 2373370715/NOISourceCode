using System;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020012F4 RID: 4852
public class EquippableBalloon : StateMachineComponent<EquippableBalloon.StatesInstance>
{
	// Token: 0x0600637F RID: 25471 RVA: 0x000E55B9 File Offset: 0x000E37B9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.smi.transitionTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.JOY_REACTION_DURATION;
	}

	// Token: 0x06006380 RID: 25472 RVA: 0x000E55DC File Offset: 0x000E37DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06006381 RID: 25473 RVA: 0x000E55F5 File Offset: 0x000E37F5
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06006382 RID: 25474 RVA: 0x000E55FD File Offset: 0x000E37FD
	public void SetBalloonOverride(BalloonOverrideSymbol balloonOverride)
	{
		base.smi.facadeAnim = balloonOverride.animFileID;
		base.smi.symbolID = balloonOverride.animFileSymbolID;
		this.ApplyBalloonOverrideToBalloonFx();
	}

	// Token: 0x06006383 RID: 25475 RVA: 0x002C8D14 File Offset: 0x002C6F14
	public void ApplyBalloonOverrideToBalloonFx()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (!component.IsNullOrDestroyed() && !component.assignee.IsNullOrDestroyed())
		{
			Ownables soleOwner = component.assignee.GetSoleOwner();
			if (soleOwner.IsNullOrDestroyed())
			{
				return;
			}
			BalloonFX.Instance smi = ((KMonoBehaviour)soleOwner.GetComponent<MinionAssignablesProxy>().target).GetSMI<BalloonFX.Instance>();
			if (!smi.IsNullOrDestroyed())
			{
				new BalloonOverrideSymbol(base.smi.facadeAnim, base.smi.symbolID).ApplyTo(smi);
			}
		}
	}

	// Token: 0x020012F5 RID: 4853
	public class StatesInstance : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.GameInstance
	{
		// Token: 0x06006385 RID: 25477 RVA: 0x000E562F File Offset: 0x000E382F
		public StatesInstance(EquippableBalloon master) : base(master)
		{
		}

		// Token: 0x04004759 RID: 18265
		[Serialize]
		public float transitionTime;

		// Token: 0x0400475A RID: 18266
		[Serialize]
		public string facadeAnim;

		// Token: 0x0400475B RID: 18267
		[Serialize]
		public string symbolID;
	}

	// Token: 0x020012F6 RID: 4854
	public class States : GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon>
	{
		// Token: 0x06006386 RID: 25478 RVA: 0x002C8D94 File Offset: 0x002C6F94
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (EquippableBalloon.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.transitionTime, UpdateRate.SIM_200ms);
			this.destroy.Enter(delegate(EquippableBalloon.StatesInstance smi)
			{
				smi.master.GetComponent<Equippable>().Unassign();
			});
		}

		// Token: 0x0400475C RID: 18268
		public GameStateMachine<EquippableBalloon.States, EquippableBalloon.StatesInstance, EquippableBalloon, object>.State destroy;
	}
}
