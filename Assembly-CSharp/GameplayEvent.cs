using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020007B4 RID: 1972
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>
{
	// Token: 0x17000101 RID: 257
	// (get) Token: 0x060022ED RID: 8941 RVA: 0x000BB284 File Offset: 0x000B9484
	// (set) Token: 0x060022EE RID: 8942 RVA: 0x000BB28C File Offset: 0x000B948C
	public int importance { get; private set; }

	// Token: 0x060022EF RID: 8943 RVA: 0x001D1510 File Offset: 0x001CF710
	public virtual bool IsAllowed()
	{
		if (this.WillNeverRunAgain())
		{
			return false;
		}
		if (!this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
		{
			return false;
		}
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (gameplayEventPrecondition.required && !gameplayEventPrecondition.condition())
			{
				return false;
			}
		}
		float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
		return GameUtil.GetCurrentTimeInCycles() >= sleepTimer;
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000BB295 File Offset: 0x000B9495
	public void SetSleepTimer(float timeToSleepUntil)
	{
		GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000BB2A3 File Offset: 0x000B94A3
	public virtual bool WillNeverRunAgain()
	{
		return this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents(this.Id) >= this.numTimesAllowed;
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000BB2D0 File Offset: 0x000B94D0
	public int GetCashedPriority()
	{
		return this.calculatedPriority;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000BB2D8 File Offset: 0x000B94D8
	public virtual int CalculatePriority()
	{
		this.calculatedPriority = this.basePriority + this.CalculateBoost();
		return this.calculatedPriority;
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x001D15B0 File Offset: 0x001CF7B0
	public int CalculateBoost()
	{
		int num = 0;
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (!gameplayEventPrecondition.required && gameplayEventPrecondition.condition())
			{
				num += gameplayEventPrecondition.priorityModifier;
			}
		}
		return num;
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000BB2F3 File Offset: 0x000B94F3
	public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
	{
		precondition.required = true;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000BB309 File Offset: 0x000B9509
	public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
	{
		precondition.required = false;
		precondition.priorityModifier = priorityBoost;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000BB326 File Offset: 0x000B9526
	public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
	{
		this.minionFilters.Add(filter);
		return this;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000BB335 File Offset: 0x000B9535
	public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
	{
		this.successEvents.Add(evt);
		return this;
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000BB344 File Offset: 0x000B9544
	public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
	{
		this.failureEvents.Add(evt);
		return this;
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000BB353 File Offset: 0x000B9553
	public GameplayEvent SetVisuals(HashedString animFileName)
	{
		this.animFileName = animFileName;
		return this;
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual Sprite GetDisplaySprite()
	{
		return null;
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual string GetDisplayString()
	{
		return null;
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x001D1620 File Offset: 0x001CF820
	public MinionIdentity GetRandomFilteredMinion()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		using (List<GameplayEventMinionFilter>.Enumerator enumerator = this.minionFilters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameplayEventMinionFilter filter = enumerator.Current;
				list.RemoveAll((MinionIdentity x) => !filter.filter(x));
			}
		}
		if (list.Count != 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x001D16B8 File Offset: 0x001CF8B8
	public MinionIdentity GetRandomMinionPrioritizeFiltered()
	{
		MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
		if (!(randomFilteredMinion == null))
		{
			return randomFilteredMinion;
		}
		return Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x001D16FC File Offset: 0x001CF8FC
	public int CompareTo(GameplayEvent other)
	{
		return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x001D1720 File Offset: 0x001CF920
	public GameplayEvent(string id, int priority, int importance) : base(id, null, null)
	{
		this.tags = new List<Tag>();
		this.basePriority = priority;
		this.preconditions = new List<GameplayEventPrecondition>();
		this.minionFilters = new List<GameplayEventMinionFilter>();
		this.successEvents = new List<HashedString>();
		this.failureEvents = new List<HashedString>();
		this.importance = importance;
		this.animFileName = id;
	}

	// Token: 0x06002301 RID: 8961
	public abstract StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance);

	// Token: 0x06002302 RID: 8962 RVA: 0x001D1790 File Offset: 0x001CF990
	public GameplayEventInstance CreateInstance(int worldId)
	{
		GameplayEventInstance gameplayEventInstance = new GameplayEventInstance(this, worldId);
		if (this.tags != null)
		{
			gameplayEventInstance.tags.AddRange(this.tags);
		}
		return gameplayEventInstance;
	}

	// Token: 0x0400177C RID: 6012
	public const int INFINITE = -1;

	// Token: 0x0400177D RID: 6013
	public int numTimesAllowed = -1;

	// Token: 0x0400177E RID: 6014
	public bool allowMultipleEventInstances;

	// Token: 0x0400177F RID: 6015
	protected int basePriority;

	// Token: 0x04001780 RID: 6016
	protected int calculatedPriority;

	// Token: 0x04001782 RID: 6018
	public List<GameplayEventPrecondition> preconditions;

	// Token: 0x04001783 RID: 6019
	public List<GameplayEventMinionFilter> minionFilters;

	// Token: 0x04001784 RID: 6020
	public List<HashedString> successEvents;

	// Token: 0x04001785 RID: 6021
	public List<HashedString> failureEvents;

	// Token: 0x04001786 RID: 6022
	public string title;

	// Token: 0x04001787 RID: 6023
	public string description;

	// Token: 0x04001788 RID: 6024
	public HashedString animFileName;

	// Token: 0x04001789 RID: 6025
	public List<Tag> tags;
}
