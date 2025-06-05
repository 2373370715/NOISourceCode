using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020015B7 RID: 5559
public class GameplayEventMonitor : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>
{
	// Token: 0x06007376 RID: 29558 RVA: 0x0030F674 File Offset: 0x0030D874
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
		default_state = this.idle;
		this.root.EventHandler(GameHashes.GameplayEventMonitorStart, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorStart(data);
		}).EventHandler(GameHashes.GameplayEventMonitorEnd, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnMonitorEnd(data);
		}).EventHandler(GameHashes.GameplayEventMonitorChanged, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			this.UpdateFX(smi);
		});
		this.idle.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.HasEvents)).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
		this.activeState.DefaultState(this.activeState.unseenEvents);
		this.activeState.unseenEvents.ToggleFX(new Func<GameplayEventMonitor.Instance, StateMachine.Instance>(this.CreateFX)).EventHandler(GameHashes.SelectObject, delegate(GameplayEventMonitor.Instance smi, object data)
		{
			smi.OnSelect(data);
		}).EventTransition(GameHashes.GameplayEventMonitorChanged, this.activeState.seenAllEvents, new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll));
		this.activeState.seenAllEvents.EventTransition(GameHashes.GameplayEventMonitorStart, this.activeState.unseenEvents, GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Not(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.Transition.ConditionCallback(this.SeenAll))).Enter(new StateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State.Callback(this.UpdateEventDisplay));
	}

	// Token: 0x06007377 RID: 29559 RVA: 0x000F0270 File Offset: 0x000EE470
	private bool HasEvents(GameplayEventMonitor.Instance smi)
	{
		return smi.events.Count > 0;
	}

	// Token: 0x06007378 RID: 29560 RVA: 0x000F0280 File Offset: 0x000EE480
	private bool SeenAll(GameplayEventMonitor.Instance smi)
	{
		return smi.UnseenCount() == 0;
	}

	// Token: 0x06007379 RID: 29561 RVA: 0x000F028B File Offset: 0x000EE48B
	private void UpdateFX(GameplayEventMonitor.Instance smi)
	{
		if (smi.fx != null)
		{
			smi.fx.sm.notificationCount.Set(smi.UnseenCount(), smi.fx, false);
		}
	}

	// Token: 0x0600737A RID: 29562 RVA: 0x000F02B8 File Offset: 0x000EE4B8
	private GameplayEventFX.Instance CreateFX(GameplayEventMonitor.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new GameplayEventFX.Instance(smi.master, new Vector3(0f, 0f, -0.1f));
			return smi.fx;
		}
		return null;
	}

	// Token: 0x0600737B RID: 29563 RVA: 0x0030F7F4 File Offset: 0x0030D9F4
	public void UpdateEventDisplay(GameplayEventMonitor.Instance smi)
	{
		if (smi.events.Count == 0 || smi.UnseenCount() > 0)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, false, null, null);
			return;
		}
		int num = -1;
		GameplayEvent gameplayEvent = null;
		foreach (GameplayEventInstance gameplayEventInstance in smi.events)
		{
			Sprite displaySprite = gameplayEventInstance.gameplayEvent.GetDisplaySprite();
			if (gameplayEventInstance.gameplayEvent.importance > num && displaySprite != null)
			{
				num = gameplayEventInstance.gameplayEvent.importance;
				gameplayEvent = gameplayEventInstance.gameplayEvent;
			}
		}
		if (gameplayEvent != null)
		{
			NameDisplayScreen.Instance.SetGameplayEventDisplay(smi.master.gameObject, true, gameplayEvent.GetDisplayString(), gameplayEvent.GetDisplaySprite());
		}
	}

	// Token: 0x040056A3 RID: 22179
	public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State idle;

	// Token: 0x040056A4 RID: 22180
	public GameplayEventMonitor.ActiveState activeState;

	// Token: 0x020015B8 RID: 5560
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020015B9 RID: 5561
	public class ActiveState : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State
	{
		// Token: 0x040056A5 RID: 22181
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State unseenEvents;

		// Token: 0x040056A6 RID: 22182
		public GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.State seenAllEvents;
	}

	// Token: 0x020015BA RID: 5562
	public new class Instance : GameStateMachine<GameplayEventMonitor, GameplayEventMonitor.Instance, IStateMachineTarget, GameplayEventMonitor.Def>.GameInstance
	{
		// Token: 0x06007380 RID: 29568 RVA: 0x000F0308 File Offset: 0x000EE508
		public Instance(IStateMachineTarget master, GameplayEventMonitor.Def def) : base(master, def)
		{
			NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x0030F8D0 File Offset: 0x0030DAD0
		public void OnMonitorStart(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (!this.events.Contains(gameplayEventInstance))
			{
				this.events.Add(gameplayEventInstance);
				gameplayEventInstance.RegisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x0030F938 File Offset: 0x0030DB38
		public void OnMonitorEnd(object data)
		{
			GameplayEventInstance gameplayEventInstance = data as GameplayEventInstance;
			if (this.events.Contains(gameplayEventInstance))
			{
				this.events.Remove(gameplayEventInstance);
				gameplayEventInstance.UnregisterMonitorCallback(base.gameObject);
			}
			base.smi.sm.UpdateFX(base.smi);
			base.smi.sm.UpdateEventDisplay(base.smi);
			if (this.events.Count == 0)
			{
				base.smi.GoTo(base.sm.idle);
			}
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x0030F9C4 File Offset: 0x0030DBC4
		public void OnSelect(object data)
		{
			if (!(bool)data)
			{
				return;
			}
			foreach (GameplayEventInstance gameplayEventInstance in this.events)
			{
				if (!gameplayEventInstance.seenNotification && gameplayEventInstance.GetEventPopupData != null)
				{
					gameplayEventInstance.seenNotification = true;
					EventInfoScreen.ShowPopup(gameplayEventInstance.GetEventPopupData());
					break;
				}
			}
			if (this.UnseenCount() == 0)
			{
				base.smi.GoTo(base.sm.activeState.seenAllEvents);
			}
		}

		// Token: 0x06007384 RID: 29572 RVA: 0x000F032F File Offset: 0x000EE52F
		public int UnseenCount()
		{
			return this.events.Count((GameplayEventInstance evt) => !evt.seenNotification);
		}

		// Token: 0x040056A7 RID: 22183
		public List<GameplayEventInstance> events = new List<GameplayEventInstance>();

		// Token: 0x040056A8 RID: 22184
		public GameplayEventFX.Instance fx;
	}
}
