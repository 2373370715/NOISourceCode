using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
[AddComponentMenu("KMonoBehaviour/scripts/EffectArea")]
public class EffectArea : KMonoBehaviour
{
	// Token: 0x06003082 RID: 12418 RVA: 0x000C3F94 File Offset: 0x000C2194
	protected override void OnPrefabInit()
	{
		this.Effect = Db.Get().effects.Get(this.EffectName);
	}

	// Token: 0x06003083 RID: 12419 RVA: 0x00209D8C File Offset: 0x00207F8C
	private void Update()
	{
		int num = 0;
		int num2 = 0;
		Grid.PosToXY(base.transform.GetPosition(), out num, out num2);
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			int num3 = 0;
			int num4 = 0;
			Grid.PosToXY(minionIdentity.transform.GetPosition(), out num3, out num4);
			if (Math.Abs(num3 - num) <= this.Area && Math.Abs(num4 - num2) <= this.Area)
			{
				minionIdentity.GetComponent<Effects>().Add(this.Effect, true);
			}
		}
	}

	// Token: 0x0400214C RID: 8524
	public string EffectName;

	// Token: 0x0400214D RID: 8525
	public int Area;

	// Token: 0x0400214E RID: 8526
	private Effect Effect;
}
