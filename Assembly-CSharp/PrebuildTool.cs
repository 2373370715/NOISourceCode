using System;
using UnityEngine;

// Token: 0x02001475 RID: 5237
public class PrebuildTool : InterfaceTool
{
	// Token: 0x06006C50 RID: 27728 RVA: 0x000EB99A File Offset: 0x000E9B9A
	public static void DestroyInstance()
	{
		PrebuildTool.Instance = null;
	}

	// Token: 0x06006C51 RID: 27729 RVA: 0x000EB9A2 File Offset: 0x000E9BA2
	protected override void OnPrefabInit()
	{
		PrebuildTool.Instance = this;
	}

	// Token: 0x06006C52 RID: 27730 RVA: 0x000EB9AA File Offset: 0x000E9BAA
	protected override void OnActivateTool()
	{
		this.viewMode = this.def.ViewMode;
		base.OnActivateTool();
	}

	// Token: 0x06006C53 RID: 27731 RVA: 0x000EB9C3 File Offset: 0x000E9BC3
	public void Activate(BuildingDef def, string errorMessage)
	{
		this.def = def;
		PlayerController.Instance.ActivateTool(this);
		PrebuildToolHoverTextCard component = base.GetComponent<PrebuildToolHoverTextCard>();
		component.errorMessage = errorMessage;
		component.currentDef = def;
	}

	// Token: 0x06006C54 RID: 27732 RVA: 0x000EB9EA File Offset: 0x000E9BEA
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		UISounds.PlaySound(UISounds.Sound.Negative);
		base.OnLeftClickDown(cursor_pos);
	}

	// Token: 0x040051F6 RID: 20982
	public static PrebuildTool Instance;

	// Token: 0x040051F7 RID: 20983
	private BuildingDef def;
}
