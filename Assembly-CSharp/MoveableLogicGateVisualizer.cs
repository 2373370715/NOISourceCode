using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E85 RID: 3717
[SkipSaveFileSerialization]
public class MoveableLogicGateVisualizer : LogicGateBase
{
	// Token: 0x06004941 RID: 18753 RVA: 0x0026791C File Offset: 0x00265B1C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cell = -1;
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
		base.Subscribe<MoveableLogicGateVisualizer>(-1643076535, MoveableLogicGateVisualizer.OnRotatedDelegate);
	}

	// Token: 0x06004942 RID: 18754 RVA: 0x000D3F79 File Offset: 0x000D2179
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06004943 RID: 18755 RVA: 0x000D3FAD File Offset: 0x000D21AD
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			this.Register();
			return;
		}
		this.Unregister();
	}

	// Token: 0x06004944 RID: 18756 RVA: 0x000D3FC9 File Offset: 0x000D21C9
	private void OnRotated(object data)
	{
		this.Unregister();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06004945 RID: 18757 RVA: 0x00267980 File Offset: 0x00265B80
	private void Update()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell)
		{
			return;
		}
		this.cell = num;
		this.Unregister();
		this.Register();
	}

	// Token: 0x06004946 RID: 18758 RVA: 0x002679CC File Offset: 0x00265BCC
	private GameObject CreateUIElem(int cell, bool is_input)
	{
		GameObject gameObject = Util.KInstantiate(LogicGateBase.uiSrcData.prefab, Grid.CellToPosCCC(cell, Grid.SceneLayer.Front), Quaternion.identity, GameScreenManager.Instance.worldSpaceCanvas, null, true, 0);
		Image component = gameObject.GetComponent<Image>();
		component.sprite = (is_input ? LogicGateBase.uiSrcData.inputSprite : LogicGateBase.uiSrcData.outputSprite);
		component.raycastTarget = false;
		return gameObject;
	}

	// Token: 0x06004947 RID: 18759 RVA: 0x00267A30 File Offset: 0x00265C30
	private void Register()
	{
		if (this.visChildren.Count > 0)
		{
			return;
		}
		base.enabled = true;
		this.visChildren.Add(this.CreateUIElem(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.OutputCellTwo, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellThree, false));
			this.visChildren.Add(this.CreateUIElem(base.OutputCellFour, false));
		}
		this.visChildren.Add(this.CreateUIElem(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.InputCellTwo, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellThree, true));
			this.visChildren.Add(this.CreateUIElem(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(this.CreateUIElem(base.ControlCellOne, true));
			this.visChildren.Add(this.CreateUIElem(base.ControlCellTwo, true));
		}
	}

	// Token: 0x06004948 RID: 18760 RVA: 0x00267B80 File Offset: 0x00265D80
	private void Unregister()
	{
		if (this.visChildren.Count <= 0)
		{
			return;
		}
		base.enabled = false;
		this.cell = -1;
		foreach (GameObject original in this.visChildren)
		{
			Util.KDestroyGameObject(original);
		}
		this.visChildren.Clear();
	}

	// Token: 0x040033C2 RID: 13250
	private int cell;

	// Token: 0x040033C3 RID: 13251
	protected List<GameObject> visChildren = new List<GameObject>();

	// Token: 0x040033C4 RID: 13252
	private static readonly EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer> OnRotatedDelegate = new EventSystem.IntraObjectHandler<MoveableLogicGateVisualizer>(delegate(MoveableLogicGateVisualizer component, object data)
	{
		component.OnRotated(data);
	});
}
