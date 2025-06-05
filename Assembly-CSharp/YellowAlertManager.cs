using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001677 RID: 5751
public class YellowAlertManager : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance>
{
	// Token: 0x060076E0 RID: 30432 RVA: 0x00319AC4 File Offset: 0x00317CC4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.off.ParamTransition<bool>(this.isOn, this.on, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsTrue);
		this.on.Enter("EnterEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-741654735, null);
		}).Exit("ExitEvent", delegate(YellowAlertManager.Instance smi)
		{
			Game.Instance.Trigger(-2062778933, null);
		}).Enter("EnableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.SetColor(new Color(1f, 1f, 0f, 0.1f));
		}).Exit("DisableVignette", delegate(YellowAlertManager.Instance smi)
		{
			Vignette.Instance.Reset();
		}).Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_ON", false));
		}).ToggleLoopingSound(GlobalAssets.GetSound("RedAlert_LP", false), null, true, true, true).ToggleNotification((YellowAlertManager.Instance smi) => smi.notification).ParamTransition<bool>(this.isOn, this.off, GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.IsFalse);
		this.on_pst.Enter("Sounds", delegate(YellowAlertManager.Instance smi)
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("RedAlert_OFF", false));
		});
	}

	// Token: 0x04005966 RID: 22886
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x04005967 RID: 22887
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x04005968 RID: 22888
	public GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.State on_pst;

	// Token: 0x04005969 RID: 22889
	public StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter isOn = new StateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.BoolParameter();

	// Token: 0x02001678 RID: 5752
	public new class Instance : GameStateMachine<YellowAlertManager, YellowAlertManager.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060076E2 RID: 30434 RVA: 0x000F2AD4 File Offset: 0x000F0CD4
		public static void DestroyInstance()
		{
			YellowAlertManager.Instance.instance = null;
		}

		// Token: 0x060076E3 RID: 30435 RVA: 0x000F2ADC File Offset: 0x000F0CDC
		public static YellowAlertManager.Instance Get()
		{
			return YellowAlertManager.Instance.instance;
		}

		// Token: 0x060076E4 RID: 30436 RVA: 0x00319C50 File Offset: 0x00317E50
		public Instance(IStateMachineTarget master) : base(master)
		{
			YellowAlertManager.Instance.instance = this;
		}

		// Token: 0x060076E5 RID: 30437 RVA: 0x000F2AE3 File Offset: 0x000F0CE3
		public bool IsOn()
		{
			return base.sm.isOn.Get(base.smi);
		}

		// Token: 0x060076E6 RID: 30438 RVA: 0x000F2AFB File Offset: 0x000F0CFB
		public void HasTopPriorityChore(bool on)
		{
			this.hasTopPriorityChore = on;
			this.Refresh();
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x000F2B0A File Offset: 0x000F0D0A
		private void Refresh()
		{
			base.sm.isOn.Set(this.hasTopPriorityChore, base.smi, false);
		}

		// Token: 0x0400596A RID: 22890
		private static YellowAlertManager.Instance instance;

		// Token: 0x0400596B RID: 22891
		private bool hasTopPriorityChore;

		// Token: 0x0400596C RID: 22892
		public Notification notification = new Notification(MISC.NOTIFICATIONS.YELLOWALERT.NAME, NotificationType.Bad, (List<Notification> notificationList, object data) => MISC.NOTIFICATIONS.YELLOWALERT.TOOLTIP, null, false, 0f, null, null, null, true, false, false);
	}
}
