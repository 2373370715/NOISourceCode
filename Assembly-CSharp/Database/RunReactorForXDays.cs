using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02002226 RID: 8742
	public class RunReactorForXDays : ColonyAchievementRequirement
	{
		// Token: 0x0600B9EB RID: 47595 RVA: 0x0011C514 File Offset: 0x0011A714
		public RunReactorForXDays(int numCycles)
		{
			this.numCycles = numCycles;
		}

		// Token: 0x0600B9EC RID: 47596 RVA: 0x00478424 File Offset: 0x00476624
		public override string GetProgress(bool complete)
		{
			int num = 0;
			foreach (Reactor reactor in Components.NuclearReactors.Items)
			{
				if (reactor.numCyclesRunning > num)
				{
					num = reactor.numCyclesRunning;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.RUN_A_REACTOR, complete ? this.numCycles : num, this.numCycles);
		}

		// Token: 0x0600B9ED RID: 47597 RVA: 0x004784B4 File Offset: 0x004766B4
		public override bool Success()
		{
			using (List<Reactor>.Enumerator enumerator = Components.NuclearReactors.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.numCyclesRunning >= this.numCycles)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040097A2 RID: 38818
		private int numCycles;
	}
}
