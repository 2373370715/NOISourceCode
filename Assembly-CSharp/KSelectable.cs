using System;
using UnityEngine;

// Token: 0x02000AA4 RID: 2724
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KSelectable")]
public class KSelectable : KMonoBehaviour
{
	// Token: 0x17000201 RID: 513
	// (get) Token: 0x060031A9 RID: 12713 RVA: 0x000C4B06 File Offset: 0x000C2D06
	public bool IsSelected
	{
		get
		{
			return this.selected;
		}
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x060031AA RID: 12714 RVA: 0x000C4B0E File Offset: 0x000C2D0E
	// (set) Token: 0x060031AB RID: 12715 RVA: 0x000C4B20 File Offset: 0x000C2D20
	public bool IsSelectable
	{
		get
		{
			return this.selectable && base.isActiveAndEnabled;
		}
		set
		{
			this.selectable = value;
		}
	}

	// Token: 0x17000203 RID: 515
	// (get) Token: 0x060031AC RID: 12716 RVA: 0x000C4B29 File Offset: 0x000C2D29
	public bool DisableSelectMarker
	{
		get
		{
			return this.disableSelectMarker;
		}
	}

	// Token: 0x060031AD RID: 12717 RVA: 0x0020D6E8 File Offset: 0x0020B8E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.statusItemGroup = new StatusItemGroup(base.gameObject);
		base.GetComponent<KPrefabID>() != null;
		if (this.entityName == null || this.entityName.Length <= 0)
		{
			this.SetName(base.name);
		}
		if (this.entityGender == null)
		{
			this.entityGender = "NB";
		}
	}

	// Token: 0x060031AE RID: 12718 RVA: 0x0020D750 File Offset: 0x0020B950
	public virtual string GetName()
	{
		if (this.entityName == null || this.entityName == "" || this.entityName.Length <= 0)
		{
			global::Debug.Log("Warning Item has blank name!", base.gameObject);
			return base.name;
		}
		return this.entityName;
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x000C4B31 File Offset: 0x000C2D31
	public void SetStatusIndicatorOffset(Vector3 offset)
	{
		if (this.statusItemGroup == null)
		{
			return;
		}
		this.statusItemGroup.SetOffset(offset);
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x000C4B48 File Offset: 0x000C2D48
	public void SetName(string name)
	{
		this.entityName = name;
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x000C4B51 File Offset: 0x000C2D51
	public void SetGender(string Gender)
	{
		this.entityGender = Gender;
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x0020D7A4 File Offset: 0x0020B9A4
	public float GetZoom()
	{
		Bounds bounds = Util.GetBounds(base.gameObject);
		return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x0020D7E0 File Offset: 0x0020B9E0
	public Vector3 GetPortraitLocation()
	{
		return Util.GetBounds(base.gameObject).center;
	}

	// Token: 0x060031B4 RID: 12724 RVA: 0x0020D800 File Offset: 0x0020BA00
	private void ClearHighlight()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(0f, 0f, 0f, 0f);
		}
		base.Trigger(-1201923725, false);
	}

	// Token: 0x060031B5 RID: 12725 RVA: 0x0020D854 File Offset: 0x0020BA54
	private void ApplyHighlight(float highlight)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.HighlightColour = new Color(highlight, highlight, highlight, highlight);
		}
		base.Trigger(-1201923725, true);
	}

	// Token: 0x060031B6 RID: 12726 RVA: 0x0020D898 File Offset: 0x0020BA98
	public void Select()
	{
		this.selected = true;
		this.ClearHighlight();
		this.ApplyHighlight(0.2f);
		base.Trigger(-1503271301, true);
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		int childCount = base.transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			int childCount2 = base.transform.GetChild(i).childCount;
			for (int j = 0; j < childCount2; j++)
			{
				if (base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>() != null)
				{
					base.transform.GetChild(i).transform.GetChild(j).GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
				}
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x0020D9B0 File Offset: 0x0020BBB0
	public void Unselect()
	{
		if (this.selected)
		{
			this.selected = false;
			this.ClearHighlight();
			base.Trigger(-1503271301, false);
		}
		if (base.GetComponent<LoopingSounds>() != null)
		{
			base.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		if (base.transform.GetComponentInParent<LoopingSounds>() != null)
		{
			base.transform.GetComponentInParent<LoopingSounds>().UpdateObjectSelection(this.selected);
		}
		foreach (object obj in base.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponent<LoopingSounds>() != null)
			{
				transform.GetComponent<LoopingSounds>().UpdateObjectSelection(this.selected);
			}
		}
		this.UpdateWorkerSelection(this.selected);
		this.UpdateWorkableSelection(this.selected);
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x000C4B5A File Offset: 0x000C2D5A
	public void Hover(bool playAudio)
	{
		this.ClearHighlight();
		if (!DebugHandler.HideUI)
		{
			this.ApplyHighlight(0.25f);
		}
		if (playAudio)
		{
			this.PlayHoverSound();
		}
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x000C4B7D File Offset: 0x000C2D7D
	private void PlayHoverSound()
	{
		if (CellSelectionObject.IsSelectionObject(base.gameObject))
		{
			return;
		}
		UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x000C4B93 File Offset: 0x000C2D93
	public void Unhover()
	{
		if (!this.selected)
		{
			this.ClearHighlight();
		}
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x000C4BA3 File Offset: 0x000C2DA3
	public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
	{
		if (on)
		{
			return this.AddStatusItem(status_item, data);
		}
		return this.RemoveStatusItem(status_item, false);
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x000C4BB9 File Offset: 0x000C2DB9
	public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
	{
		if (show)
		{
			if (guid != Guid.Empty)
			{
				return guid;
			}
			return this.AddStatusItem(status_item, data);
		}
		else
		{
			if (guid != Guid.Empty)
			{
				return this.RemoveStatusItem(guid, false);
			}
			return guid;
		}
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x000C4BEE File Offset: 0x000C2DEE
	public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.SetStatusItem(category, status_item, data);
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x000C4C0C File Offset: 0x000C2E0C
	public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		if (guid != Guid.Empty)
		{
			this.statusItemGroup.RemoveStatusItem(guid, false);
		}
		return this.AddStatusItem(status_item, data);
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x000C4C3F File Offset: 0x000C2E3F
	public Guid AddStatusItem(StatusItem status_item, object data = null)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		return this.statusItemGroup.AddStatusItem(status_item, data, null);
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x000C4C5D File Offset: 0x000C2E5D
	public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(status_item, immediate);
		return Guid.Empty;
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x000C4C80 File Offset: 0x000C2E80
	public Guid RemoveStatusItem(Guid guid, bool immediate = false)
	{
		if (this.statusItemGroup == null)
		{
			return Guid.Empty;
		}
		this.statusItemGroup.RemoveStatusItem(guid, immediate);
		return Guid.Empty;
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x000C4CA3 File Offset: 0x000C2EA3
	public bool HasStatusItem(StatusItem status_item)
	{
		return this.statusItemGroup != null && this.statusItemGroup.HasStatusItem(status_item);
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x000C4CBB File Offset: 0x000C2EBB
	public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
	{
		return this.statusItemGroup.GetStatusItem(category);
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x000C4CC9 File Offset: 0x000C2EC9
	public StatusItemGroup GetStatusItemGroup()
	{
		return this.statusItemGroup;
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x0020DAA8 File Offset: 0x0020BCA8
	public void UpdateWorkerSelection(bool selected)
	{
		Workable[] components = base.GetComponents<Workable>();
		if (components.Length != 0)
		{
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].worker != null && components[i].GetComponent<LoopingSounds>() != null)
				{
					components[i].GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
				}
			}
		}
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x0020DAFC File Offset: 0x0020BCFC
	public void UpdateWorkableSelection(bool selected)
	{
		WorkerBase component = base.GetComponent<WorkerBase>();
		if (component != null && component.GetWorkable() != null)
		{
			Workable workable = base.GetComponent<WorkerBase>().GetWorkable();
			if (workable.GetComponent<LoopingSounds>() != null)
			{
				workable.GetComponent<LoopingSounds>().UpdateObjectSelection(selected);
			}
		}
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x000C4CD1 File Offset: 0x000C2ED1
	protected override void OnLoadLevel()
	{
		this.OnCleanUp();
		base.OnLoadLevel();
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x0020DB50 File Offset: 0x0020BD50
	protected override void OnCleanUp()
	{
		if (this.statusItemGroup != null)
		{
			this.statusItemGroup.Destroy();
			this.statusItemGroup = null;
		}
		if (this.selected && SelectTool.Instance != null)
		{
			if (SelectTool.Instance.selected == this)
			{
				SelectTool.Instance.Select(null, true);
			}
			else
			{
				this.Unselect();
			}
		}
		base.OnCleanUp();
	}

	// Token: 0x0400220D RID: 8717
	private const float hoverHighlight = 0.25f;

	// Token: 0x0400220E RID: 8718
	private const float selectHighlight = 0.2f;

	// Token: 0x0400220F RID: 8719
	public string entityName;

	// Token: 0x04002210 RID: 8720
	public string entityGender;

	// Token: 0x04002211 RID: 8721
	private bool selected;

	// Token: 0x04002212 RID: 8722
	[SerializeField]
	private bool selectable = true;

	// Token: 0x04002213 RID: 8723
	[SerializeField]
	private bool disableSelectMarker;

	// Token: 0x04002214 RID: 8724
	private StatusItemGroup statusItemGroup;
}
