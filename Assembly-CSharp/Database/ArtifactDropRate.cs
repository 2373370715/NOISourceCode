using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02002180 RID: 8576
	public class ArtifactDropRate : Resource
	{
		// Token: 0x0600B690 RID: 46736 RVA: 0x0011AEF3 File Offset: 0x001190F3
		public void AddItem(ArtifactTier tier, float weight)
		{
			this.rates.Add(new global::Tuple<ArtifactTier, float>(tier, weight));
			this.totalWeight += weight;
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x00457700 File Offset: 0x00455900
		public float GetTierWeight(ArtifactTier tier)
		{
			float result = 0f;
			foreach (global::Tuple<ArtifactTier, float> tuple in this.rates)
			{
				if (tuple.first == tier)
				{
					result = tuple.second;
				}
			}
			return result;
		}

		// Token: 0x04009076 RID: 36982
		public List<global::Tuple<ArtifactTier, float>> rates = new List<global::Tuple<ArtifactTier, float>>();

		// Token: 0x04009077 RID: 36983
		public float totalWeight;
	}
}
