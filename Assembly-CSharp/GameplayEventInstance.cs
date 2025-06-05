using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020007B7 RID: 1975
[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06002306 RID: 8966 RVA: 0x000BB37E File Offset: 0x000B957E
	// (set) Token: 0x06002307 RID: 8967 RVA: 0x000BB386 File Offset: 0x000B9586
	public StateMachine.Instance smi { get; private set; }

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06002308 RID: 8968 RVA: 0x000BB38F File Offset: 0x000B958F
	// (set) Token: 0x06002309 RID: 8969 RVA: 0x000BB397 File Offset: 0x000B9597
	public bool seenNotification
	{
		get
		{
			return this._seenNotification;
		}
		set
		{
			this._seenNotification = value;
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600230A RID: 8970 RVA: 0x000BB3B7 File Offset: 0x000B95B7
	public GameplayEvent gameplayEvent
	{
		get
		{
			if (this._gameplayEvent == null)
			{
				this._gameplayEvent = Db.Get().GameplayEvents.TryGet(this.eventID);
			}
			return this._gameplayEvent;
		}
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000BB3E2 File Offset: 0x000B95E2
	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		this.eventID = gameplayEvent.Id;
		this.tags = new List<Tag>();
		this.eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000BB418 File Offset: 0x000B9618
	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		this.smi = this.gameplayEvent.GetSMI(manager, this);
		return this.smi;
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x001D17C0 File Offset: 0x001CF9C0
	public void StartEvent()
	{
		GameplayEventManager.Instance.Trigger(1491341646, this);
		StateMachine.Instance smi = this.smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStop));
		this.smi.StartSM();
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000BB433 File Offset: 0x000B9633
	public void RegisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		if (!this.monitorCallbackObjects.Contains(go))
		{
			this.monitorCallbackObjects.Add(go);
		}
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000BB462 File Offset: 0x000B9662
	public void UnregisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		this.monitorCallbackObjects.Remove(go);
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x001D1810 File Offset: 0x001CFA10
	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (this.monitorCallbackObjects != null)
		{
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		if (status == StateMachine.Status.Success)
		{
			using (List<HashedString>.Enumerator enumerator = this.gameplayEvent.successEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HashedString hashedString = enumerator.Current;
					GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(hashedString);
					DebugUtil.DevAssert(gameplayEvent != null, string.Format("GameplayEvent {0} is null", hashedString), null);
					if (gameplayEvent != null && gameplayEvent.IsAllowed())
					{
						GameplayEventManager.Instance.StartNewEvent(gameplayEvent, -1, null);
					}
				}
				return;
			}
		}
		if (status == StateMachine.Status.Failed)
		{
			foreach (HashedString hashedString2 in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(hashedString2);
				DebugUtil.DevAssert(gameplayEvent2 != null, string.Format("GameplayEvent {0} is null", hashedString2), null);
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2, -1, null);
				}
			}
		}
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000BB484 File Offset: 0x000B9684
	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - this.eventStartTime;
	}

	// Token: 0x0400178C RID: 6028
	[Serialize]
	public readonly HashedString eventID;

	// Token: 0x0400178D RID: 6029
	[Serialize]
	public List<Tag> tags;

	// Token: 0x0400178E RID: 6030
	[Serialize]
	public float eventStartTime;

	// Token: 0x0400178F RID: 6031
	[Serialize]
	public readonly int worldId;

	// Token: 0x04001790 RID: 6032
	[Serialize]
	private bool _seenNotification;

	// Token: 0x04001791 RID: 6033
	public List<GameObject> monitorCallbackObjects;

	// Token: 0x04001792 RID: 6034
	public GameplayEventInstance.GameplayEventPopupDataCallback GetEventPopupData;

	// Token: 0x04001793 RID: 6035
	private GameplayEvent _gameplayEvent;

	// Token: 0x020007B8 RID: 1976
	// (Invoke) Token: 0x06002315 RID: 8981
	public delegate EventInfoData GameplayEventPopupDataCallback();
}
