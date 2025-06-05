using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x020021D1 RID: 8657
	public class Sicknesses : ResourceSet<Sickness>
	{
		// Token: 0x0600B893 RID: 47251 RVA: 0x00470D40 File Offset: 0x0046EF40
		public Sicknesses(ResourceSet parent) : base("Sicknesses", parent)
		{
			this.FoodSickness = base.Add(new FoodSickness());
			this.SlimeSickness = base.Add(new SlimeSickness());
			this.ZombieSickness = base.Add(new ZombieSickness());
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationSickness = base.Add(new RadiationSickness());
			}
			this.Allergies = base.Add(new Allergies());
			this.Sunburn = base.Add(new Sunburn());
		}

		// Token: 0x0600B894 RID: 47252 RVA: 0x00470DC8 File Offset: 0x0046EFC8
		public static bool IsValidID(string id)
		{
			bool result = false;
			using (List<Sickness>.Enumerator enumerator = Db.Get().Sicknesses.resources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == id)
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x04009697 RID: 38551
		public Sickness FoodSickness;

		// Token: 0x04009698 RID: 38552
		public Sickness SlimeSickness;

		// Token: 0x04009699 RID: 38553
		public Sickness ZombieSickness;

		// Token: 0x0400969A RID: 38554
		public Sickness Allergies;

		// Token: 0x0400969B RID: 38555
		public Sickness RadiationSickness;

		// Token: 0x0400969C RID: 38556
		public Sickness Sunburn;
	}
}
