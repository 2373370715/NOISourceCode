using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018CD RID: 6349
[AddComponentMenu("KMonoBehaviour/scripts/SituationalAnim")]
public class SituationalAnim : KMonoBehaviour
{
	// Token: 0x06008351 RID: 33617 RVA: 0x0034EB38 File Offset: 0x0034CD38
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SituationalAnim.Situation situation = this.GetSituation();
		DebugUtil.LogArgs(new object[]
		{
			"Situation is",
			situation
		});
		this.SetAnimForSituation(situation);
	}

	// Token: 0x06008352 RID: 33618 RVA: 0x0034EB78 File Offset: 0x0034CD78
	private void SetAnimForSituation(SituationalAnim.Situation situation)
	{
		foreach (global::Tuple<SituationalAnim.Situation, string> tuple in this.anims)
		{
			if ((tuple.first & situation) == tuple.first)
			{
				DebugUtil.LogArgs(new object[]
				{
					"Chose Anim",
					tuple.first,
					tuple.second
				});
				this.SetAnim(tuple.second);
				break;
			}
		}
	}

	// Token: 0x06008353 RID: 33619 RVA: 0x000FAD5A File Offset: 0x000F8F5A
	private void SetAnim(string animName)
	{
		base.GetComponent<KBatchedAnimController>().Play(animName, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06008354 RID: 33620 RVA: 0x0034EC0C File Offset: 0x0034CE0C
	private SituationalAnim.Situation GetSituation()
	{
		SituationalAnim.Situation situation = (SituationalAnim.Situation)0;
		Extents extents = base.GetComponent<Building>().GetExtents();
		int x = extents.x;
		int num = extents.x + extents.width - 1;
		int y = extents.y;
		int num2 = extents.y + extents.height - 1;
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, y - 1, y - 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Bottom;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x - 1, x - 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Left;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(x, num, num2 + 1, num2 + 1), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Top;
		}
		if (this.DoesSatisfy(this.GetSatisfactionForEdge(num + 1, num + 1, y, num2), this.mustSatisfy))
		{
			situation |= SituationalAnim.Situation.Right;
		}
		return situation;
	}

	// Token: 0x06008355 RID: 33621 RVA: 0x000FAD78 File Offset: 0x000F8F78
	private bool DoesSatisfy(SituationalAnim.MustSatisfy result, SituationalAnim.MustSatisfy requirement)
	{
		if (requirement == SituationalAnim.MustSatisfy.All)
		{
			return result == SituationalAnim.MustSatisfy.All;
		}
		if (requirement == SituationalAnim.MustSatisfy.Any)
		{
			return result > SituationalAnim.MustSatisfy.None;
		}
		return result == SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x06008356 RID: 33622 RVA: 0x0034ECE0 File Offset: 0x0034CEE0
	private SituationalAnim.MustSatisfy GetSatisfactionForEdge(int minx, int maxx, int miny, int maxy)
	{
		bool flag = false;
		bool flag2 = true;
		for (int i = minx; i <= maxx; i++)
		{
			for (int j = miny; j <= maxy; j++)
			{
				int arg = Grid.XYToCell(i, j);
				if (this.test(arg))
				{
					flag = true;
				}
				else
				{
					flag2 = false;
				}
			}
		}
		if (flag2)
		{
			return SituationalAnim.MustSatisfy.All;
		}
		if (flag)
		{
			return SituationalAnim.MustSatisfy.Any;
		}
		return SituationalAnim.MustSatisfy.None;
	}

	// Token: 0x04006401 RID: 25601
	public List<global::Tuple<SituationalAnim.Situation, string>> anims;

	// Token: 0x04006402 RID: 25602
	public Func<int, bool> test;

	// Token: 0x04006403 RID: 25603
	public SituationalAnim.MustSatisfy mustSatisfy;

	// Token: 0x020018CE RID: 6350
	[Flags]
	public enum Situation
	{
		// Token: 0x04006405 RID: 25605
		Left = 1,
		// Token: 0x04006406 RID: 25606
		Right = 2,
		// Token: 0x04006407 RID: 25607
		Top = 4,
		// Token: 0x04006408 RID: 25608
		Bottom = 8
	}

	// Token: 0x020018CF RID: 6351
	public enum MustSatisfy
	{
		// Token: 0x0400640A RID: 25610
		None,
		// Token: 0x0400640B RID: 25611
		Any,
		// Token: 0x0400640C RID: 25612
		All
	}
}
