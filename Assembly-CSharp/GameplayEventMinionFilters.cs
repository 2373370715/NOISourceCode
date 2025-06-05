using System;
using Database;

// Token: 0x020007BB RID: 1979
public class GameplayEventMinionFilters
{
	// Token: 0x17000105 RID: 261
	// (get) Token: 0x0600231D RID: 8989 RVA: 0x000BB4AE File Offset: 0x000B96AE
	public static GameplayEventMinionFilters Instance
	{
		get
		{
			if (GameplayEventMinionFilters._instance == null)
			{
				GameplayEventMinionFilters._instance = new GameplayEventMinionFilters();
			}
			return GameplayEventMinionFilters._instance;
		}
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x001D1968 File Offset: 0x001CFB68
	public GameplayEventMinionFilter HasMasteredSkill(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasMasteredSkill(skill.Id)),
			id = "HasMasteredSkill"
		};
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x001D19A4 File Offset: 0x001CFBA4
	public GameplayEventMinionFilter HasSkillAptitude(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasSkillAptitude(skill)),
			id = "HasSkillAptitude"
		};
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x001D19E0 File Offset: 0x001CFBE0
	public GameplayEventMinionFilter HasChoreGroupPriorityOrHigher(ChoreGroup choreGroup, int priority)
	{
		return new GameplayEventMinionFilter
		{
			filter = delegate(MinionIdentity minion)
			{
				ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
				return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
			},
			id = "HasChoreGroupPriorityOrHigher"
		};
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x001D1A24 File Offset: 0x001CFC24
	public GameplayEventMinionFilter AgeRange(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.arrivalTime >= min && minion.arrivalTime <= max),
			id = "AgeRange"
		};
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000BB4C6 File Offset: 0x000B96C6
	public GameplayEventMinionFilter PriorityIn()
	{
		GameplayEventMinionFilter gameplayEventMinionFilter = new GameplayEventMinionFilter();
		gameplayEventMinionFilter.filter = ((MinionIdentity minion) => true);
		gameplayEventMinionFilter.id = "PriorityIn";
		return gameplayEventMinionFilter;
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x001D1A68 File Offset: 0x001CFC68
	public GameplayEventMinionFilter Not(GameplayEventMinionFilter filter)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => !filter.filter(minion)),
			id = "Not[" + filter.id + "]"
		};
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x001D1ABC File Offset: 0x001CFCBC
	public GameplayEventMinionFilter Or(GameplayEventMinionFilter precondition1, GameplayEventMinionFilter precondition2)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => precondition1.filter(minion) || precondition2.filter(minion)),
			id = string.Concat(new string[]
			{
				"[",
				precondition1.id,
				"]-OR-[",
				precondition2.id,
				"]"
			})
		};
	}

	// Token: 0x04001796 RID: 6038
	private static GameplayEventMinionFilters _instance;
}
