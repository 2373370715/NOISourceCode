using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CB3 RID: 15539
	[SerializationConfig(MemberSerialization.OptIn)]
	public class GameplaySeasonInstance : ISaveLoadable
	{
		// Token: 0x17000C5E RID: 3166
		// (get) Token: 0x0600EE7A RID: 61050 RVA: 0x00144745 File Offset: 0x00142945
		public float NextEventTime
		{
			get
			{
				return this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x17000C5F RID: 3167
		// (get) Token: 0x0600EE7B RID: 61051 RVA: 0x00144754 File Offset: 0x00142954
		public GameplaySeason Season
		{
			get
			{
				if (this._season == null)
				{
					this._season = Db.Get().GameplaySeasons.TryGet(this.seasonId);
				}
				return this._season;
			}
		}

		// Token: 0x0600EE7C RID: 61052 RVA: 0x004E6D0C File Offset: 0x004E4F0C
		public GameplaySeasonInstance(GameplaySeason season, int worldId)
		{
			this.seasonId = season.Id;
			this.worldId = worldId;
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			if (season.synchronizedToPeriod)
			{
				float seasonPeriod = this.Season.GetSeasonPeriod();
				this.nextPeriodTime = (Mathf.Floor(currentTimeInCycles / seasonPeriod) + 1f) * seasonPeriod;
			}
			else
			{
				this.nextPeriodTime = currentTimeInCycles;
			}
			this.CalculateNextEventTime();
		}

		// Token: 0x0600EE7D RID: 61053 RVA: 0x004E6D74 File Offset: 0x004E4F74
		private void CalculateNextEventTime()
		{
			float seasonPeriod = this.Season.GetSeasonPeriod();
			this.randomizedNextTime = UnityEngine.Random.Range(this.Season.randomizedEventStartTime.min, this.Season.randomizedEventStartTime.max);
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			float num = this.nextPeriodTime + this.randomizedNextTime;
			while (num < currentTimeInCycles || num < this.Season.minCycle)
			{
				this.nextPeriodTime += seasonPeriod;
				num = this.nextPeriodTime + this.randomizedNextTime;
			}
		}

		// Token: 0x0600EE7E RID: 61054 RVA: 0x004E6DFC File Offset: 0x004E4FFC
		public bool StartEvent(bool ignorePreconditions = false)
		{
			bool result = false;
			this.CalculateNextEventTime();
			this.numStartEvents++;
			List<GameplayEvent> list;
			if (!ignorePreconditions)
			{
				list = (from x in this.Season.events
				where x.IsAllowed()
				select x).ToList<GameplayEvent>();
			}
			else
			{
				list = this.Season.events;
			}
			List<GameplayEvent> list2 = list;
			if (list2.Count > 0)
			{
				list2.ForEach(delegate(GameplayEvent x)
				{
					x.CalculatePriority();
				});
				list2.Sort();
				int maxExclusive = Mathf.Min(list2.Count, 5);
				GameplayEvent eventType = list2[UnityEngine.Random.Range(0, maxExclusive)];
				GameplayEventManager.Instance.StartNewEvent(eventType, this.worldId, new Action<StateMachine.Instance>(this.Season.AdditionalEventInstanceSetup));
				result = true;
			}
			this.allEventWillNotRunAgain = true;
			using (List<GameplayEvent>.Enumerator enumerator = this.Season.events.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.WillNeverRunAgain())
					{
						this.allEventWillNotRunAgain = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600EE7F RID: 61055 RVA: 0x004E6F38 File Offset: 0x004E5138
		public bool ShouldGenerateEvents()
		{
			WorldContainer world = ClusterManager.Instance.GetWorld(this.worldId);
			if (!world.IsDupeVisited && !world.IsRoverVisted)
			{
				return false;
			}
			if ((this.Season.finishAfterNumEvents != -1 && this.numStartEvents >= this.Season.finishAfterNumEvents) || this.allEventWillNotRunAgain)
			{
				return false;
			}
			float currentTimeInCycles = GameUtil.GetCurrentTimeInCycles();
			return currentTimeInCycles > this.Season.minCycle && currentTimeInCycles < this.Season.maxCycle;
		}

		// Token: 0x0400EA57 RID: 59991
		public const int LIMIT_SELECTION = 5;

		// Token: 0x0400EA58 RID: 59992
		[Serialize]
		public int numStartEvents;

		// Token: 0x0400EA59 RID: 59993
		[Serialize]
		public int worldId;

		// Token: 0x0400EA5A RID: 59994
		[Serialize]
		private readonly string seasonId;

		// Token: 0x0400EA5B RID: 59995
		[Serialize]
		private float nextPeriodTime;

		// Token: 0x0400EA5C RID: 59996
		[Serialize]
		private float randomizedNextTime;

		// Token: 0x0400EA5D RID: 59997
		private bool allEventWillNotRunAgain;

		// Token: 0x0400EA5E RID: 59998
		private GameplaySeason _season;
	}
}
