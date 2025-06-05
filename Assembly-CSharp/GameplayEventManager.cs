using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x0200139A RID: 5018
public class GameplayEventManager : KMonoBehaviour
{
	// Token: 0x060066DF RID: 26335 RVA: 0x000E792C File Offset: 0x000E5B2C
	public static void DestroyInstance()
	{
		GameplayEventManager.Instance = null;
	}

	// Token: 0x060066E0 RID: 26336 RVA: 0x000E7934 File Offset: 0x000E5B34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameplayEventManager.Instance = this;
		this.notifier = base.GetComponent<Notifier>();
	}

	// Token: 0x060066E1 RID: 26337 RVA: 0x000E794E File Offset: 0x000E5B4E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreEvents();
	}

	// Token: 0x060066E2 RID: 26338 RVA: 0x000E795C File Offset: 0x000E5B5C
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameplayEventManager.Instance = null;
	}

	// Token: 0x060066E3 RID: 26339 RVA: 0x002DF9D0 File Offset: 0x002DDBD0
	private void RestoreEvents()
	{
		this.activeEvents.RemoveAll((GameplayEventInstance x) => Db.Get().GameplayEvents.TryGet(x.eventID) == null);
		for (int i = this.activeEvents.Count - 1; i >= 0; i--)
		{
			GameplayEventInstance gameplayEventInstance = this.activeEvents[i];
			if (gameplayEventInstance.smi == null)
			{
				this.StartEventInstance(gameplayEventInstance, null);
			}
		}
	}

	// Token: 0x060066E4 RID: 26340 RVA: 0x000E796A File Offset: 0x000E5B6A
	public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
	{
		this.sleepTimers[eventType.IdHash] = time;
	}

	// Token: 0x060066E5 RID: 26341 RVA: 0x002DFA40 File Offset: 0x002DDC40
	public float GetSleepTimer(GameplayEvent eventType)
	{
		float num = 0f;
		this.sleepTimers.TryGetValue(eventType.IdHash, out num);
		this.sleepTimers[eventType.IdHash] = num;
		return num;
	}

	// Token: 0x060066E6 RID: 26342 RVA: 0x002DFA7C File Offset: 0x002DDC7C
	public bool IsGameplayEventActive(GameplayEvent eventType)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventType.IdHash) != null;
	}

	// Token: 0x060066E7 RID: 26343 RVA: 0x002DFAB0 File Offset: 0x002DDCB0
	public bool IsGameplayEventRunningWithTag(Tag tag)
	{
		using (List<GameplayEventInstance>.Enumerator enumerator = this.activeEvents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.tags.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060066E8 RID: 26344 RVA: 0x002DFB10 File Offset: 0x002DDD10
	public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.worldId == worldID && gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x060066E9 RID: 26345 RVA: 0x002DFB84 File Offset: 0x002DDD84
	public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x060066EA RID: 26346 RVA: 0x000E797E File Offset: 0x000E5B7E
	private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
	{
		return gameplayEvent.CreateInstance(worldId);
	}

	// Token: 0x060066EB RID: 26347 RVA: 0x002DFBF0 File Offset: 0x002DDDF0
	public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventID && (worldId == -1 || e.worldId == worldId));
	}

	// Token: 0x060066EC RID: 26348 RVA: 0x002DFC28 File Offset: 0x002DDE28
	public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
	{
		GameplayEventInstance gameplayEventInstance = this.GetGameplayEventInstance(eventType.Id, worldId);
		if (gameplayEventInstance == null)
		{
			gameplayEventInstance = this.StartNewEvent(eventType, worldId, null);
		}
		return gameplayEventInstance;
	}

	// Token: 0x060066ED RID: 26349 RVA: 0x002DFC58 File Offset: 0x002DDE58
	public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
	{
		GameplayEventInstance gameplayEventInstance = this.activeEvents.Find((GameplayEventInstance x) => x == eventInstance);
		if (gameplayEventInstance != null)
		{
			if (gameplayEventInstance.smi != null)
			{
				gameplayEventInstance.smi.StopSM(reason);
				return;
			}
			this.activeEvents.Remove(gameplayEventInstance);
		}
	}

	// Token: 0x060066EE RID: 26350 RVA: 0x002DFCB0 File Offset: 0x002DDEB0
	public GameplayEventInstance StartNewEvent(GameplayEvent eventType, int worldId = -1, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		GameplayEventInstance gameplayEventInstance = this.CreateGameplayEvent(eventType, worldId);
		this.StartEventInstance(gameplayEventInstance, setupActionsBeforeStart);
		this.activeEvents.Add(gameplayEventInstance);
		int num;
		this.pastEvents.TryGetValue(gameplayEventInstance.eventID, out num);
		this.pastEvents[gameplayEventInstance.eventID] = num + 1;
		return gameplayEventInstance;
	}

	// Token: 0x060066EF RID: 26351 RVA: 0x002DFD04 File Offset: 0x002DDF04
	private void StartEventInstance(GameplayEventInstance gameplayEventInstance, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
		StateMachine.Instance instance2 = instance;
		instance2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance2.OnStop, new Action<string, StateMachine.Status>(delegate(string reason, StateMachine.Status status)
		{
			this.activeEvents.Remove(gameplayEventInstance);
		}));
		if (setupActionsBeforeStart != null)
		{
			setupActionsBeforeStart(instance);
		}
		gameplayEventInstance.StartEvent();
	}

	// Token: 0x060066F0 RID: 26352 RVA: 0x002DFD6C File Offset: 0x002DDF6C
	public int NumberOfPastEvents(HashedString eventID)
	{
		int result;
		this.pastEvents.TryGetValue(eventID, out result);
		return result;
	}

	// Token: 0x060066F1 RID: 26353 RVA: 0x002DFD8C File Offset: 0x002DDF8C
	public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"eventPopup is null in CreateStandardCancelledNotification"
			});
			return null;
		}
		eventInfoData.FinalizeText();
		return new Notification(string.Format(GAMEPLAY_EVENTS.CANCELED, eventInfoData.title), NotificationType.Event, (List<Notification> list, object data) => string.Format(GAMEPLAY_EVENTS.CANCELED_TOOLTIP, eventInfoData.title), null, true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04004DBB RID: 19899
	public static GameplayEventManager Instance;

	// Token: 0x04004DBC RID: 19900
	public Notifier notifier;

	// Token: 0x04004DBD RID: 19901
	[Serialize]
	private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();

	// Token: 0x04004DBE RID: 19902
	[Serialize]
	private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();

	// Token: 0x04004DBF RID: 19903
	[Serialize]
	private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();
}
