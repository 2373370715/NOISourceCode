using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200135F RID: 4959
[SerializationConfig(MemberSerialization.OptIn)]
public class GameFlowManager : StateMachineComponent<GameFlowManager.StatesInstance>, ISaveLoadable
{
	// Token: 0x060065A4 RID: 26020 RVA: 0x000E6C8B File Offset: 0x000E4E8B
	public static void DestroyInstance()
	{
		GameFlowManager.Instance = null;
	}

	// Token: 0x060065A5 RID: 26021 RVA: 0x000E6C93 File Offset: 0x000E4E93
	protected override void OnPrefabInit()
	{
		GameFlowManager.Instance = this;
	}

	// Token: 0x060065A6 RID: 26022 RVA: 0x000E6C9B File Offset: 0x000E4E9B
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x060065A7 RID: 26023 RVA: 0x000E6CA8 File Offset: 0x000E4EA8
	public bool IsGameOver()
	{
		return base.smi.IsInsideState(base.smi.sm.gameover);
	}

	// Token: 0x04004994 RID: 18836
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04004995 RID: 18837
	public static GameFlowManager Instance;

	// Token: 0x02001360 RID: 4960
	public class StatesInstance : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.GameInstance
	{
		// Token: 0x060065A9 RID: 26025 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool IsIncapacitated(GameObject go)
		{
			return false;
		}

		// Token: 0x060065AA RID: 26026 RVA: 0x002D2EF0 File Offset: 0x002D10F0
		public void CheckForGameOver()
		{
			if (!Game.Instance.GameStarted())
			{
				return;
			}
			if (GenericGameSettings.instance.disableGameOver)
			{
				return;
			}
			bool flag = false;
			if (Components.LiveMinionIdentities.Count == 0)
			{
				flag = true;
			}
			else
			{
				flag = true;
				foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
				{
					if (!this.IsIncapacitated(minionIdentity.gameObject))
					{
						flag = false;
						break;
					}
				}
			}
			if (flag)
			{
				this.GoTo(base.sm.gameover.pending);
			}
		}

		// Token: 0x060065AB RID: 26027 RVA: 0x002D2F9C File Offset: 0x002D119C
		public StatesInstance(GameFlowManager smi) : base(smi)
		{
		}

		// Token: 0x04004996 RID: 18838
		public Notification colonyLostNotification = new Notification(MISC.NOTIFICATIONS.COLONYLOST.NAME, NotificationType.Bad, null, null, false, 0f, null, null, null, true, false, false);
	}

	// Token: 0x02001361 RID: 4961
	public class States : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager>
	{
		// Token: 0x060065AC RID: 26028 RVA: 0x002D2FD4 File Offset: 0x002D11D4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.loading;
			this.loading.ScheduleGoTo(4f, this.running);
			this.running.Update("CheckForGameOver", delegate(GameFlowManager.StatesInstance smi, float dt)
			{
				smi.CheckForGameOver();
			}, UpdateRate.SIM_200ms, false);
			this.gameover.TriggerOnEnter(GameHashes.GameOver, null).ToggleNotification((GameFlowManager.StatesInstance smi) => smi.colonyLostNotification);
			this.gameover.pending.Enter("Goto(gameover.active)", delegate(GameFlowManager.StatesInstance smi)
			{
				UIScheduler.Instance.Schedule("Goto(gameover.active)", 4f, delegate(object d)
				{
					smi.GoTo(this.gameover.active);
				}, null, null);
			});
			this.gameover.active.Enter(delegate(GameFlowManager.StatesInstance smi)
			{
				if (GenericGameSettings.instance.demoMode)
				{
					DemoTimer.Instance.EndDemo();
					return;
				}
				GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.GameOverScreen, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<KScreen>().Show(true);
			});
		}

		// Token: 0x04004997 RID: 18839
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State loading;

		// Token: 0x04004998 RID: 18840
		public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State running;

		// Token: 0x04004999 RID: 18841
		public GameFlowManager.States.GameOverState gameover;

		// Token: 0x02001362 RID: 4962
		public class GameOverState : GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State
		{
			// Token: 0x0400499A RID: 18842
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State pending;

			// Token: 0x0400499B RID: 18843
			public GameStateMachine<GameFlowManager.States, GameFlowManager.StatesInstance, GameFlowManager, object>.State active;
		}
	}
}
