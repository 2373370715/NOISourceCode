using System;
using UnityEngine;

// Token: 0x02000AA9 RID: 2729
[AddComponentMenu("KMonoBehaviour/scripts/LightColorMenu")]
public class LightColorMenu : KMonoBehaviour
{
	// Token: 0x060031D5 RID: 12757 RVA: 0x000C4D9A File Offset: 0x000C2F9A
	protected override void OnPrefabInit()
	{
		base.Subscribe<LightColorMenu>(493375141, LightColorMenu.OnRefreshUserMenuDelegate);
		this.SetColor(0);
	}

	// Token: 0x060031D6 RID: 12758 RVA: 0x0020DC74 File Offset: 0x0020BE74
	private void OnRefreshUserMenu(object data)
	{
		if (this.lightColors.Length != 0)
		{
			int num = this.lightColors.Length;
			for (int i = 0; i < num; i++)
			{
				if (i != this.currentColor)
				{
					int new_color = i;
					Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(this.lightColors[i].name, this.lightColors[i].name, delegate()
					{
						this.SetColor(new_color);
					}, global::Action.NumActions, null, null, null, "", true), 1f);
				}
			}
		}
	}

	// Token: 0x060031D7 RID: 12759 RVA: 0x0020DD1C File Offset: 0x0020BF1C
	private void SetColor(int color_index)
	{
		if (this.lightColors.Length != 0 && color_index < this.lightColors.Length)
		{
			Light2D[] componentsInChildren = base.GetComponentsInChildren<Light2D>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Color = this.lightColors[color_index].color;
			}
			MeshRenderer[] componentsInChildren2 = base.GetComponentsInChildren<MeshRenderer>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				foreach (Material material in componentsInChildren2[i].materials)
				{
					if (material.name.StartsWith("matScriptedGlow01"))
					{
						material.color = this.lightColors[color_index].color;
					}
				}
			}
		}
		this.currentColor = color_index;
	}

	// Token: 0x0400221A RID: 8730
	public LightColorMenu.LightColor[] lightColors;

	// Token: 0x0400221B RID: 8731
	private int currentColor;

	// Token: 0x0400221C RID: 8732
	private static readonly EventSystem.IntraObjectHandler<LightColorMenu> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<LightColorMenu>(delegate(LightColorMenu component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x02000AAA RID: 2730
	[Serializable]
	public struct LightColor
	{
		// Token: 0x060031DA RID: 12762 RVA: 0x000C4DD0 File Offset: 0x000C2FD0
		public LightColor(string name, Color color)
		{
			this.name = name;
			this.color = color;
		}

		// Token: 0x0400221D RID: 8733
		public string name;

		// Token: 0x0400221E RID: 8734
		public Color color;
	}
}
