using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;

// Token: 0x020007C5 RID: 1989
public class GameplayEventPreconditions
{
	// Token: 0x17000106 RID: 262
	// (get) Token: 0x0600233A RID: 9018 RVA: 0x000BB595 File Offset: 0x000B9795
	public static GameplayEventPreconditions Instance
	{
		get
		{
			if (GameplayEventPreconditions._instance == null)
			{
				GameplayEventPreconditions._instance = new GameplayEventPreconditions();
			}
			return GameplayEventPreconditions._instance;
		}
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x001D1B78 File Offset: 0x001CFD78
	public GameplayEventPrecondition LiveMinions(int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Components.LiveMinionIdentities.Count >= count),
			description = string.Format("At least {0} dupes alive", count)
		};
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x001D1BC4 File Offset: 0x001CFDC4
	public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
			description = string.Format("{0} {1} has been built", count, buildingId)
		};
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x001D1C20 File Offset: 0x001CFE20
	public GameplayEventPrecondition ResearchCompleted(string techName)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
			description = "Has researched " + techName + "."
		};
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x001D1C6C File Offset: 0x001CFE6C
	public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => SaveGame.Instance.ColonyAchievementTracker.IsAchievementUnlocked(achievement)),
			description = "Unlocked the " + achievement.Id + " achievement"
		};
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x001D1CC0 File Offset: 0x001CFEC0
	public GameplayEventPrecondition RoomBuilt(RoomType roomType)
	{
		Predicate<Room> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				List<Room> rooms = Game.Instance.roomProber.rooms;
				Predicate<Room> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((Room match) => match.roomType == roomType));
				}
				return rooms.Exists(match2);
			},
			description = "Built a " + roomType.Id + " room"
		};
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x001D1D14 File Offset: 0x001CFF14
	public GameplayEventPrecondition CycleRestriction(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameUtil.GetCurrentTimeInCycles() >= min && GameUtil.GetCurrentTimeInCycles() <= max),
			description = string.Format("After cycle {0} and before cycle {1}", min, max)
		};
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x001D1D74 File Offset: 0x001CFF74
	public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<Effects>().Get(effectId) != null));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} effect applied", count, effectId)
		};
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x001D1DD0 File Offset: 0x001CFFD0
	public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<KSelectable>().HasStatusItem(statusItem)));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} status item", count, statusItem)
		};
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x001D1E2C File Offset: 0x001D002C
	public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(ChoreGroup choreGroup, int count, int priority)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(MinionIdentity minion)
					{
						ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
						return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
					});
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have their {1} set to {2} or higher.", count, choreGroup.Name, priority)
		};
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x001D1E9C File Offset: 0x001D009C
	public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evtId) >= count),
			description = string.Format("The {0} event has triggered {1} times.", evtId, count)
		};
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x001D1EF8 File Offset: 0x001D00F8
	public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
			description = string.Format("The {0} event has triggered {1} times and is not active.", evt.Id, count)
		};
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x001D1F58 File Offset: 0x001D0158
	public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => !precondition.condition()),
			description = "Not[" + precondition.description + "]"
		};
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x001D1FAC File Offset: 0x001D01AC
	public GameplayEventPrecondition Or(GameplayEventPrecondition precondition1, GameplayEventPrecondition precondition2)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => precondition1.condition() || precondition2.condition()),
			description = string.Concat(new string[]
			{
				"[",
				precondition1.description,
				"]-OR-[",
				precondition2.description,
				"]"
			})
		};
	}

	// Token: 0x040017A6 RID: 6054
	private static GameplayEventPreconditions _instance;
}
