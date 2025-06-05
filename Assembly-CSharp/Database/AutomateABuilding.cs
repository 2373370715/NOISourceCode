using System;
using STRINGS;
using UnityEngine;

namespace Database
{
	// Token: 0x02002210 RID: 8720
	public class AutomateABuilding : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B99E RID: 47518 RVA: 0x00477870 File Offset: 0x00475A70
		public override bool Success()
		{
			foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
			{
				LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
				if (logicCircuitNetwork.Receivers.Count > 0 && logicCircuitNetwork.Senders.Count > 0)
				{
					bool flag = false;
					foreach (ILogicEventReceiver logicEventReceiver in logicCircuitNetwork.Receivers)
					{
						if (!logicEventReceiver.IsNullOrDestroyed())
						{
							GameObject gameObject = Grid.Objects[logicEventReceiver.GetLogicCell(), 1];
							if (gameObject != null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag = true;
								break;
							}
						}
					}
					bool flag2 = false;
					foreach (ILogicEventSender logicEventSender in logicCircuitNetwork.Senders)
					{
						if (!logicEventSender.IsNullOrDestroyed())
						{
							GameObject gameObject2 = Grid.Objects[logicEventSender.GetLogicCell(), 1];
							if (gameObject2 != null && !gameObject2.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
							{
								flag2 = true;
								break;
							}
						}
					}
					if (flag && flag2)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600B99F RID: 47519 RVA: 0x000AA038 File Offset: 0x000A8238
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x0600B9A0 RID: 47520 RVA: 0x0011C1CF File Offset: 0x0011A3CF
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
		}
	}
}
