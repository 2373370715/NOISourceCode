using System;
using System.Diagnostics;
using Klei.CustomSettings;

namespace Klei.AI
{
	// Token: 0x02003CBC RID: 15548
	[DebuggerDisplay("{base.Id}")]
	public class MeteorShowerSeason : GameplaySeason
	{
		// Token: 0x0600EEAF RID: 61103 RVA: 0x004E7A20 File Offset: 0x004E5C20
		public MeteorShowerSeason(string id, GameplaySeason.Type type, string dlcId, float period, bool synchronizedToPeriod, float randomizedEventStartTime = -1f, bool startActive = false, int finishAfterNumEvents = -1, float minCycle = 0f, float maxCycle = float.PositiveInfinity, int numEventsToStartEachPeriod = 1, bool affectedByDifficultySettings = true, float clusterTravelDuration = -1f) : base(id, type, dlcId, period, synchronizedToPeriod, randomizedEventStartTime, startActive, finishAfterNumEvents, minCycle, maxCycle, numEventsToStartEachPeriod)
		{
			this.affectedByDifficultySettings = affectedByDifficultySettings;
			this.clusterTravelDuration = clusterTravelDuration;
		}

		// Token: 0x0600EEB0 RID: 61104 RVA: 0x0014496B File Offset: 0x00142B6B
		public override void AdditionalEventInstanceSetup(StateMachine.Instance generic_smi)
		{
			(generic_smi as MeteorShowerEvent.StatesInstance).clusterTravelDuration = this.clusterTravelDuration;
		}

		// Token: 0x0600EEB1 RID: 61105 RVA: 0x004E7A68 File Offset: 0x004E5C68
		public override float GetSeasonPeriod()
		{
			SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
			float num = base.GetSeasonPeriod();
			if (this.affectedByDifficultySettings && currentQualitySetting != null)
			{
				string id = currentQualitySetting.id;
				if (!(id == "Infrequent"))
				{
					if (!(id == "Intense"))
					{
						if (id == "Doomed")
						{
							num *= 1f;
						}
					}
					else
					{
						num *= 1f;
					}
				}
				else
				{
					num *= 2f;
				}
			}
			return num;
		}

		// Token: 0x0400EA84 RID: 60036
		public bool affectedByDifficultySettings = true;

		// Token: 0x0400EA85 RID: 60037
		public float clusterTravelDuration = -1f;
	}
}
