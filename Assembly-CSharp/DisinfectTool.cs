using System;
using UnityEngine;

// Token: 0x02001467 RID: 5223
public class DisinfectTool : DragTool
{
	// Token: 0x06006BBB RID: 27579 RVA: 0x000EB2ED File Offset: 0x000E94ED
	public static void DestroyInstance()
	{
		DisinfectTool.Instance = null;
	}

	// Token: 0x06006BBC RID: 27580 RVA: 0x000EB2F5 File Offset: 0x000E94F5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DisinfectTool.Instance = this;
		this.interceptNumberKeysForPriority = true;
		this.viewMode = OverlayModes.Disease.ID;
	}

	// Token: 0x06006BBD RID: 27581 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006BBE RID: 27582 RVA: 0x002F1D54 File Offset: 0x002EFF54
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		for (int i = 0; i < 45; i++)
		{
			GameObject gameObject = Grid.Objects[cell, i];
			if (gameObject != null)
			{
				Disinfectable component = gameObject.GetComponent<Disinfectable>();
				if (component != null && component.GetComponent<PrimaryElement>().DiseaseCount > 0)
				{
					component.MarkForDisinfect(false);
				}
			}
		}
	}

	// Token: 0x040051A0 RID: 20896
	public static DisinfectTool Instance;
}
