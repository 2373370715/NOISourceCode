using System;
using System.Collections.Generic;
using KSerialization;
using ProcGen;
using UnityEngine;

// Token: 0x020013C4 RID: 5060
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/GermExposureTracker")]
public class GermExposureTracker : KMonoBehaviour
{
	// Token: 0x060067D8 RID: 26584 RVA: 0x000E8555 File Offset: 0x000E6755
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(GermExposureTracker.Instance == null);
		GermExposureTracker.Instance = this;
	}

	// Token: 0x060067D9 RID: 26585 RVA: 0x000E856D File Offset: 0x000E676D
	protected override void OnSpawn()
	{
		this.rng = new SeededRandom(GameClock.Instance.GetCycle());
	}

	// Token: 0x060067DA RID: 26586 RVA: 0x000E8584 File Offset: 0x000E6784
	protected override void OnForcedCleanUp()
	{
		GermExposureTracker.Instance = null;
	}

	// Token: 0x060067DB RID: 26587 RVA: 0x002E2CB8 File Offset: 0x002E0EB8
	public void AddExposure(ExposureType exposure_type, float amount)
	{
		float num;
		this.accumulation.TryGetValue(exposure_type.germ_id, out num);
		float num2 = num + amount;
		if (num2 > 1f)
		{
			using (List<MinionIdentity>.Enumerator enumerator = Components.LiveMinionIdentities.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MinionIdentity cmp = enumerator.Current;
					GermExposureMonitor.Instance smi = cmp.GetSMI<GermExposureMonitor.Instance>();
					if (smi.GetExposureState(exposure_type.germ_id) == GermExposureMonitor.ExposureState.Exposed)
					{
						float exposureWeight = cmp.GetSMI<GermExposureMonitor.Instance>().GetExposureWeight(exposure_type.germ_id);
						if (exposureWeight > 0f)
						{
							this.exposure_candidates.Add(new GermExposureTracker.WeightedExposure
							{
								weight = exposureWeight,
								monitor = smi
							});
						}
					}
				}
				goto IL_F8;
			}
			IL_AF:
			num2 -= 1f;
			if (this.exposure_candidates.Count > 0)
			{
				GermExposureTracker.WeightedExposure weightedExposure = WeightedRandom.Choose<GermExposureTracker.WeightedExposure>(this.exposure_candidates, this.rng);
				this.exposure_candidates.Remove(weightedExposure);
				weightedExposure.monitor.ContractGerms(exposure_type.germ_id);
			}
			IL_F8:
			if (num2 > 1f)
			{
				goto IL_AF;
			}
		}
		this.accumulation[exposure_type.germ_id] = num2;
		this.exposure_candidates.Clear();
	}

	// Token: 0x04004E77 RID: 20087
	public static GermExposureTracker Instance;

	// Token: 0x04004E78 RID: 20088
	[Serialize]
	private Dictionary<HashedString, float> accumulation = new Dictionary<HashedString, float>();

	// Token: 0x04004E79 RID: 20089
	private SeededRandom rng;

	// Token: 0x04004E7A RID: 20090
	private List<GermExposureTracker.WeightedExposure> exposure_candidates = new List<GermExposureTracker.WeightedExposure>();

	// Token: 0x020013C5 RID: 5061
	private class WeightedExposure : IWeighted
	{
		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x060067DD RID: 26589 RVA: 0x000E85AA File Offset: 0x000E67AA
		// (set) Token: 0x060067DE RID: 26590 RVA: 0x000E85B2 File Offset: 0x000E67B2
		public float weight { get; set; }

		// Token: 0x04004E7C RID: 20092
		public GermExposureMonitor.Instance monitor;
	}
}
