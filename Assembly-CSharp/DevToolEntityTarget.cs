using System;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BDD RID: 3037
public abstract class DevToolEntityTarget
{
	// Token: 0x0600399B RID: 14747
	public abstract string GetTag();

	// Token: 0x0600399C RID: 14748
	[return: TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})]
	public abstract Option<ValueTuple<Vector2, Vector2>> GetScreenRect();

	// Token: 0x0600399D RID: 14749 RVA: 0x000C9BB4 File Offset: 0x000C7DB4
	public string GetDebugName()
	{
		return "[" + this.GetTag() + "] " + this.ToString();
	}

	// Token: 0x02000BDE RID: 3038
	public class ForUIGameObject : DevToolEntityTarget
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x000C9BD1 File Offset: 0x000C7DD1
		public ForUIGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x0022CE0C File Offset: 0x0022B00C
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			RectTransform component = this.gameObject.GetComponent<RectTransform>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Canvas componentInParent = this.gameObject.GetComponentInParent<Canvas>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			if (!componentInParent.worldCamera.IsNullOrDestroyed())
			{
				DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
				CS$<>8__locals1.camera = componentInParent.worldCamera;
				Vector3[] array = new Vector3[4];
				component.GetWorldCorners(array);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[0]), ref CS$<>8__locals1), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(array[2]), ref CS$<>8__locals1));
			}
			if (componentInParent.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				Vector3[] array2 = new Vector3[4];
				component.GetWorldCorners(array2);
				return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[0]), DevToolEntityTarget.ForUIGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_1(array2[2]));
			}
			return Option.None;
		}

		// Token: 0x060039A1 RID: 14753 RVA: 0x000C9BE0 File Offset: 0x000C7DE0
		public override string GetTag()
		{
			return "UI";
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x000C9BE7 File Offset: 0x000C7DE7
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x000C9BF4 File Offset: 0x000C7DF4
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForUIGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x000C9C14 File Offset: 0x000C7E14
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_1(Vector2 coord)
		{
			return new Vector2(coord.x, (float)Screen.height - coord.y);
		}

		// Token: 0x040027C2 RID: 10178
		public GameObject gameObject;
	}

	// Token: 0x02000BE0 RID: 3040
	public class ForWorldGameObject : DevToolEntityTarget
	{
		// Token: 0x060039A5 RID: 14757 RVA: 0x000C9C2E File Offset: 0x000C7E2E
		public ForWorldGameObject(GameObject gameObject)
		{
			this.gameObject = gameObject;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x0022CF30 File Offset: 0x0022B130
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			if (this.gameObject.IsNullOrDestroyed())
			{
				return Option.None;
			}
			DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			KCollider2D component = this.gameObject.GetComponent<KCollider2D>();
			if (component.IsNullOrDestroyed())
			{
				return Option.None;
			}
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.min), ref CS$<>8__locals1), DevToolEntityTarget.ForWorldGameObject.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(component.bounds.max), ref CS$<>8__locals1));
		}

		// Token: 0x060039A7 RID: 14759 RVA: 0x000C9C3D File Offset: 0x000C7E3D
		public override string GetTag()
		{
			return "World";
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x000C9C44 File Offset: 0x000C7E44
		public override string ToString()
		{
			return DevToolEntity.GetNameFor(this.gameObject);
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x000C9C51 File Offset: 0x000C7E51
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForWorldGameObject.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x040027C4 RID: 10180
		public GameObject gameObject;
	}

	// Token: 0x02000BE2 RID: 3042
	public class ForSimCell : DevToolEntityTarget
	{
		// Token: 0x060039AA RID: 14762 RVA: 0x000C9C71 File Offset: 0x000C7E71
		public ForSimCell(int cellIndex)
		{
			this.cellIndex = cellIndex;
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0022CFEC File Offset: 0x0022B1EC
		[return: TupleElementNames(new string[]
		{
			"cornerA",
			"cornerB"
		})]
		public override Option<ValueTuple<Vector2, Vector2>> GetScreenRect()
		{
			DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.camera = Camera.main;
			if (CS$<>8__locals1.camera.IsNullOrDestroyed())
			{
				return Option.None;
			}
			Vector2 a = Grid.CellToPosCCC(this.cellIndex, Grid.SceneLayer.Background);
			Vector2 b = Grid.HalfCellSizeInMeters * Vector2.one;
			Vector2 v = a - b;
			Vector2 v2 = a + b;
			return new ValueTuple<Vector2, Vector2>(DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v), ref CS$<>8__locals1), DevToolEntityTarget.ForSimCell.<GetScreenRect>g__ScreenPointToScreenPosition|2_0(CS$<>8__locals1.camera.WorldToScreenPoint(v2), ref CS$<>8__locals1));
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x000C9C80 File Offset: 0x000C7E80
		public override string GetTag()
		{
			return "Sim Cell";
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x000C9C87 File Offset: 0x000C7E87
		public override string ToString()
		{
			return this.cellIndex.ToString();
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x000C9C94 File Offset: 0x000C7E94
		[CompilerGenerated]
		internal static Vector2 <GetScreenRect>g__ScreenPointToScreenPosition|2_0(Vector2 coord, ref DevToolEntityTarget.ForSimCell.<>c__DisplayClass2_0 A_1)
		{
			return new Vector2(coord.x, (float)A_1.camera.pixelHeight - coord.y);
		}

		// Token: 0x040027C6 RID: 10182
		public int cellIndex;
	}
}
