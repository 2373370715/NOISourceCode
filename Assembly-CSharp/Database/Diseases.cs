using System;
using System.Collections.Generic;
using Klei.AI;

namespace Database
{
	// Token: 0x0200219F RID: 8607
	public class Diseases : ResourceSet<Disease>
	{
		// Token: 0x0600B7B8 RID: 47032 RVA: 0x00466C88 File Offset: 0x00464E88
		public Diseases(ResourceSet parent, bool statsOnly = false) : base("Diseases", parent)
		{
			this.FoodGerms = base.Add(new FoodGerms(statsOnly));
			this.SlimeGerms = base.Add(new SlimeGerms(statsOnly));
			this.PollenGerms = base.Add(new PollenGerms(statsOnly));
			this.ZombieSpores = base.Add(new ZombieSpores(statsOnly));
			if (DlcManager.FeatureRadiationEnabled())
			{
				this.RadiationPoisoning = base.Add(new RadiationPoisoning(statsOnly));
			}
		}

		// Token: 0x0600B7B9 RID: 47033 RVA: 0x00466D04 File Offset: 0x00464F04
		public bool IsValidID(string id)
		{
			bool result = false;
			using (List<Disease>.Enumerator enumerator = this.resources.GetEnumerator())
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

		// Token: 0x0600B7BA RID: 47034 RVA: 0x00466D64 File Offset: 0x00464F64
		public byte GetIndex(int hash)
		{
			byte b = 0;
			while ((int)b < this.resources.Count)
			{
				Disease disease = this.resources[(int)b];
				if (hash == disease.id.GetHashCode())
				{
					return b;
				}
				b += 1;
			}
			return byte.MaxValue;
		}

		// Token: 0x0600B7BB RID: 47035 RVA: 0x0011B329 File Offset: 0x00119529
		public byte GetIndex(HashedString id)
		{
			return this.GetIndex(id.GetHashCode());
		}

		// Token: 0x040093FE RID: 37886
		public Disease FoodGerms;

		// Token: 0x040093FF RID: 37887
		public Disease SlimeGerms;

		// Token: 0x04009400 RID: 37888
		public Disease PollenGerms;

		// Token: 0x04009401 RID: 37889
		public Disease ZombieSpores;

		// Token: 0x04009402 RID: 37890
		public Disease RadiationPoisoning;
	}
}
