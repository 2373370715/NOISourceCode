using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C60 RID: 15456
	public class Amounts : Modifications<Amount, AmountInstance>
	{
		// Token: 0x0600ECEF RID: 60655 RVA: 0x00143905 File Offset: 0x00141B05
		public Amounts(GameObject go) : base(go, null)
		{
		}

		// Token: 0x0600ECF0 RID: 60656 RVA: 0x0014390F File Offset: 0x00141B0F
		public float GetValue(string amount_id)
		{
			return base.Get(amount_id).value;
		}

		// Token: 0x0600ECF1 RID: 60657 RVA: 0x0014391D File Offset: 0x00141B1D
		public void SetValue(string amount_id, float value)
		{
			base.Get(amount_id).value = value;
		}

		// Token: 0x0600ECF2 RID: 60658 RVA: 0x0014392C File Offset: 0x00141B2C
		public override AmountInstance Add(AmountInstance instance)
		{
			instance.Activate();
			return base.Add(instance);
		}

		// Token: 0x0600ECF3 RID: 60659 RVA: 0x0014393B File Offset: 0x00141B3B
		public override void Remove(AmountInstance instance)
		{
			instance.Deactivate();
			base.Remove(instance);
		}

		// Token: 0x0600ECF4 RID: 60660 RVA: 0x004DF440 File Offset: 0x004DD640
		public void Cleanup()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Deactivate();
			}
		}
	}
}
