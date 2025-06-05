using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x02002173 RID: 8563
	public class FertilityModifiers : ResourceSet<FertilityModifier>
	{
		// Token: 0x0600B670 RID: 46704 RVA: 0x00456118 File Offset: 0x00454318
		public List<FertilityModifier> GetForTag(Tag searchTag)
		{
			List<FertilityModifier> list = new List<FertilityModifier>();
			foreach (FertilityModifier fertilityModifier in this.resources)
			{
				if (fertilityModifier.TargetTag == searchTag)
				{
					list.Add(fertilityModifier);
				}
			}
			return list;
		}
	}
}
