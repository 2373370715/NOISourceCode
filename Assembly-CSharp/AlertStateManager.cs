using System;
using System.Collections.Generic;
using STRINGS;

// Token: 0x0200152E RID: 5422
public class AlertStateManager : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>
{
	// Token: 0x060070CC RID: 28876 RVA: 0x00308538 File Offset: 0x00306738
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsTrue);
		this.on.Exit("VignetteOff", delegate(AlertStateManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).ParamTransition<bool>(this.isRedAlert, this.on.red, (AlertStateManager.Instance smi, bool p) => this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isRedAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isYellowAlert, this.on.yellow, (AlertStateManager.Instance smi, bool p) => this.isYellowAlert.Get(smi) && !this.isRedAlert.Get(smi)).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.IsFalse);
		this.on.red.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(1585324898, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-1393151672, null);
		}).Enter("SoundsOnRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		}).ToggleNotification((AlertStateManager.Instance smi) => smi.redAlertNotification);
		this.on.yellow.Enter("EnterEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(AlertStateManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("SoundsOnYellowAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_ON", false));
		}).Exit("SoundsOffRedAlert", delegate(AlertStateManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("YellowAlert_OFF", false));
		});
	}

	// Token: 0x040054C3 RID: 21699
	public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State off;

	// Token: 0x040054C4 RID: 21700
	public AlertStateManager.OnStates on;

	// Token: 0x040054C5 RID: 21701
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isRedAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x040054C6 RID: 21702
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isYellowAlert = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x040054C7 RID: 21703
	public StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter isOn = new StateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.BoolParameter();

	// Token: 0x0200152F RID: 5423
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001530 RID: 5424
	public class OnStates : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State
	{
		// Token: 0x040054C8 RID: 21704
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State yellow;

		// Token: 0x040054C9 RID: 21705
		public GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.State red;
	}

	// Token: 0x02001531 RID: 5425
	public new class Instance : GameStateMachine<AlertStateManager, AlertStateManager.Instance, IStateMachineTarget, AlertStateManager.Def>.GameInstance
	{
		// Token: 0x060070D3 RID: 28883 RVA: 0x003087A4 File Offset: 0x003069A4
		public Instance(IStateMachineTarget master, AlertStateManager.Def def) : base(master, def)
		{
		}

		// Token: 0x060070D4 RID: 28884 RVA: 0x003087FC File Offset: 0x003069FC
		public void UpdateState(float dt)
		{
			if (this.IsRedAlert())
			{
				base.smi.GoTo(base.sm.on.red);
				return;
			}
			if (this.IsYellowAlert())
			{
				base.smi.GoTo(base.sm.on.yellow);
				return;
			}
			if (!this.IsOn())
			{
				base.smi.GoTo(base.sm.off);
			}
		}

		// Token: 0x060070D5 RID: 28885 RVA: 0x000EE358 File Offset: 0x000EC558
		public bool IsOn()
		{
			return base.sm.isYellowAlert.Get(base.smi) || base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x000EE38A File Offset: 0x000EC58A
		public bool IsRedAlert()
		{
			return base.sm.isRedAlert.Get(base.smi);
		}

		// Token: 0x060070D7 RID: 28887 RVA: 0x000EE3A2 File Offset: 0x000EC5A2
		public bool IsYellowAlert()
		{
			return base.sm.isYellowAlert.Get(base.smi);
		}

		// Token: 0x060070D8 RID: 28888 RVA: 0x000EE3BA File Offset: 0x000EC5BA
		public bool IsRedAlertToggledOn()
		{
			return this.isToggled;
		}

		// Token: 0x060070D9 RID: 28889 RVA: 0x000EE3C2 File Offset: 0x000EC5C2
		public void ToggleRedAlert(bool on)
		{
			this.isToggled = on;
			this.Refresh();
		}

		// Token: 0x060070DA RID: 28890 RVA: 0x000EE3D1 File Offset: 0x000EC5D1
		public void SetHasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x00308870 File Offset: 0x00306A70
		private void Refresh()
		{
			base.sm.isYellowAlert.Set(this.hasTopPriorityChore, base.smi, false);
			base.sm.isRedAlert.Set(this.isToggled, base.smi, false);
			base.sm.isOn.Set(this.hasTopPriorityChore || this.isToggled, base.smi, false);
		}

		// Token: 0x040054CA RID: 21706
		private bool isToggled;

		// Token: 0x040054CB RID: 21707
		private bool hasTopPriorityChore;

		// Token: 0x040054CC RID: 21708
		public Notification redAlertNotification = new Notification(MISC.NOTIFICATIONS.REDALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.REDALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
