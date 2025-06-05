using System;
using STRINGS;

namespace Database
{
	// Token: 0x0200220D RID: 8717
	public class VentXKG : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x0600B990 RID: 47504 RVA: 0x0011C16F File Offset: 0x0011A36F
		public VentXKG(SimHashes element, float kilogramsToVent)
		{
			this.element = element;
			this.kilogramsToVent = kilogramsToVent;
		}

		// Token: 0x0600B991 RID: 47505 RVA: 0x004773C4 File Offset: 0x004755C4
		public override bool Success()
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return num >= this.kilogramsToVent;
		}

		// Token: 0x0600B992 RID: 47506 RVA: 0x0011C185 File Offset: 0x0011A385
		public void Deserialize(IReader reader)
		{
			this.element = (SimHashes)reader.ReadInt32();
			this.kilogramsToVent = reader.ReadSingle();
		}

		// Token: 0x0600B993 RID: 47507 RVA: 0x0047748C File Offset: 0x0047568C
		public override string GetProgress(bool complete)
		{
			float num = 0f;
			foreach (UtilityNetwork utilityNetwork in Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
			{
				FlowUtilityNetwork flowUtilityNetwork = utilityNetwork as FlowUtilityNetwork;
				if (flowUtilityNetwork != null)
				{
					foreach (FlowUtilityNetwork.IItem item in flowUtilityNetwork.sinks)
					{
						Vent component = item.GameObject.GetComponent<Vent>();
						if (component != null)
						{
							num += component.GetVentedMass(this.element);
						}
					}
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.VENTED_MASS, GameUtil.GetFormattedMass(complete ? this.kilogramsToVent : num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), GameUtil.GetFormattedMass(this.kilogramsToVent, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
		}

		// Token: 0x0400978B RID: 38795
		private SimHashes element;

		// Token: 0x0400978C RID: 38796
		private float kilogramsToVent;
	}
}
