using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Klei.AI
{
	// Token: 0x02003CB1 RID: 15537
	[DebuggerDisplay("{base.Id}")]
	public class GameplaySeason : Resource
	{
		// Token: 0x0600EE75 RID: 61045 RVA: 0x004E6C0C File Offset: 0x004E4E0C
		public GameplaySeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1) : base(id, null, null)
		{
			this.type = type;
			this.dlcId = dlcId;
			this.period = period;
			this.synchronizedToPeriod = synchronizedToPeriod;
			global::Debug.Assert(period > 0f, "Season " + id + "'s Period cannot be 0 or negative");
			if (randomizedEventStartTime == -1f)
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(--0f * period, 0f * period);
			}
			else
			{
				this.randomizedEventStartTime = new MathUtil.MinMax(-randomizedEventStartTime, randomizedEventStartTime);
				DebugUtil.DevAssert((this.randomizedEventStartTime.max - this.randomizedEventStartTime.min) * 0.4f < period, string.Format("Season {0} randomizedEventStartTime is greater than {1}% of its period.", id, 0.4f), null);
			}
			this.startActive = startActive;
			this.finishAfterNumEvents = finishAfterNumEvents;
			this.minCycle = minCycle;
			this.maxCycle = maxCycle;
			this.events = new List<GameplayEvent>();
			this.numEventsToStartEachPeriod = numEventsToStartEachPeriod;
		}

		// Token: 0x0600EE76 RID: 61046 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
		}

		// Token: 0x0600EE77 RID: 61047 RVA: 0x00144725 File Offset: 0x00142925
		public virtual float GetSeasonPeriod()
		{
			return this.period;
		}

		// Token: 0x0600EE78 RID: 61048 RVA: 0x0014472D File Offset: 0x0014292D
		public GameplaySeason AddEvent(GameplayEvent evt)
		{
			this.events.Add(evt);
			return this;
		}

		// Token: 0x0600EE79 RID: 61049 RVA: 0x0014473C File Offset: 0x0014293C
		public virtual GameplaySeasonInstance Instantiate(int worldId)
		{
			return new GameplaySeasonInstance(this, worldId);
		}

		// Token: 0x0400EA45 RID: 59973
		public const float DEFAULT_PERCENTAGE_RANDOMIZED_EVENT_START = 0f;

		// Token: 0x0400EA46 RID: 59974
		public const float PERCENTAGE_WARNING = 0.4f;

		// Token: 0x0400EA47 RID: 59975
		public const float USE_DEFAULT = -1f;

		// Token: 0x0400EA48 RID: 59976
		public const int INFINITE = -1;

		// Token: 0x0400EA49 RID: 59977
		public float period;

		// Token: 0x0400EA4A RID: 59978
		public bool synchronizedToPeriod;

		// Token: 0x0400EA4B RID: 59979
		public MathUtil.MinMax randomizedEventStartTime;

		// Token: 0x0400EA4C RID: 59980
		public int finishAfterNumEvents = -1;

		// Token: 0x0400EA4D RID: 59981
		public bool startActive;

		// Token: 0x0400EA4E RID: 59982
		public int numEventsToStartEachPeriod;

		// Token: 0x0400EA4F RID: 59983
		public float minCycle;

		// Token: 0x0400EA50 RID: 59984
		public float maxCycle;

		// Token: 0x0400EA51 RID: 59985
		public List<GameplayEvent> events;

		// Token: 0x0400EA52 RID: 59986
		public GameplaySeason.Type type;

		// Token: 0x0400EA53 RID: 59987
		public string dlcId;

		// Token: 0x02003CB2 RID: 15538
		public enum Type
		{
			// Token: 0x0400EA55 RID: 59989
			World,
			// Token: 0x0400EA56 RID: 59990
			Cluster
		}
	}
}
