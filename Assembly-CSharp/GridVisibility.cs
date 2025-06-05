using System;
using UnityEngine;

// Token: 0x02001410 RID: 5136
[AddComponentMenu("KMonoBehaviour/scripts/GridVisibility")]
public class GridVisibility : KMonoBehaviour
{
	// Token: 0x06006907 RID: 26887 RVA: 0x002E816C File Offset: 0x002E636C
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "GridVisibility.OnSpawn");
		this.OnCellChange();
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		if (myWorld != null && !base.gameObject.HasTag(GameTags.Stored))
		{
			myWorld.SetDiscovered(false);
		}
	}

	// Token: 0x06006908 RID: 26888 RVA: 0x002E81D0 File Offset: 0x002E63D0
	private void OnCellChange()
	{
		if (base.gameObject.HasTag(GameTags.Dead))
		{
			return;
		}
		int num = Grid.PosToCell(this);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Revealed[num])
		{
			int baseX;
			int baseY;
			Grid.PosToXY(base.transform.GetPosition(), out baseX, out baseY);
			GridVisibility.Reveal(baseX, baseY, this.radius, this.innerRadius);
			Grid.Revealed[num] = true;
		}
		FogOfWarMask.ClearMask(num);
	}

	// Token: 0x06006909 RID: 26889 RVA: 0x002E8248 File Offset: 0x002E6448
	public static void Reveal(int baseX, int baseY, int radius, float innerRadius)
	{
		int num = (int)Grid.WorldIdx[baseY * Grid.WidthInCells + baseX];
		for (int i = -radius; i <= radius; i++)
		{
			for (int j = -radius; j <= radius; j++)
			{
				int num2 = baseY + i;
				int num3 = baseX + j;
				if (num2 >= 0 && Grid.HeightInCells - 1 >= num2 && num3 >= 0 && Grid.WidthInCells - 1 >= num3)
				{
					int num4 = num2 * Grid.WidthInCells + num3;
					if (Grid.Visible[num4] < 255 && num == (int)Grid.WorldIdx[num4])
					{
						Vector2 vector = new Vector2((float)j, (float)i);
						float num5 = Mathf.Lerp(1f, 0f, (vector.magnitude - innerRadius) / ((float)radius - innerRadius));
						Grid.Reveal(num4, (byte)(255f * num5), false);
					}
				}
			}
		}
	}

	// Token: 0x0600690A RID: 26890 RVA: 0x000E93BD File Offset: 0x000E75BD
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
	}

	// Token: 0x04004FB6 RID: 20406
	public int radius = 18;

	// Token: 0x04004FB7 RID: 20407
	public float innerRadius = 16.5f;
}
