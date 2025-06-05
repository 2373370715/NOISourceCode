using System;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001FFD RID: 8189
public class OwnablesSecondSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x0600AD19 RID: 44313 RVA: 0x00114F05 File Offset: 0x00113105
	// (set) Token: 0x0600AD18 RID: 44312 RVA: 0x00114EFC File Offset: 0x001130FC
	public AssignableSlotInstance minionSlotInstance { get; private set; }

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600AD1B RID: 44315 RVA: 0x00114F16 File Offset: 0x00113116
	// (set) Token: 0x0600AD1A RID: 44314 RVA: 0x00114F0D File Offset: 0x0011310D
	public Assignable item { get; private set; }

	// Token: 0x0600AD1C RID: 44316 RVA: 0x00420BFC File Offset: 0x0041EDFC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.toggle = base.GetComponent<MultiToggle>();
		MultiToggle multiToggle = this.toggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnMultitoggleClicked));
		this.eyeButton.onClick.AddListener(new UnityAction(this.FocusCameraOnAssignedItem));
	}

	// Token: 0x0600AD1D RID: 44317 RVA: 0x00420C60 File Offset: 0x0041EE60
	public void SetData(AssignableSlotInstance minion, Assignable item_assignable)
	{
		this.minionSlotInstance = minion;
		this.item = item_assignable;
		this.changeAssignmentListenerIDX = this.item.Subscribe(684616645, new Action<object>(this._OnItemAssignationChanged));
		this.destroyListenerIDX = this.item.Subscribe(1969584890, new Action<object>(this._OnRowItemDestroyed));
		this.customTooltipFunc = this.item.customAssignmentUITooltipFunc;
		this.Refresh();
	}

	// Token: 0x0600AD1E RID: 44318 RVA: 0x00420CD8 File Offset: 0x0041EED8
	public void Refresh()
	{
		if (this.item != null)
		{
			this.item.PrefabID();
			string properName = this.item.GetProperName();
			this.nameLabel.text = properName;
			this.icon.sprite = Def.GetUISprite(this.item.gameObject, "ui", false).first;
			bool flag = this.item.IsAssigned() && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable != this.item;
			if (this.item.IsAssigned())
			{
				this.statusLabel.SetText(string.Format(flag ? OwnablesSecondSideScreenRow.ASSIGNED_TO_OTHER : OwnablesSecondSideScreenRow.ASSIGNED_TO_SELF, this.item.assignee.GetProperName()));
			}
			else
			{
				this.statusLabel.SetText(OwnablesSecondSideScreenRow.NOT_ASSIGNED);
			}
			if (this.customTooltipFunc == null)
			{
				InfoDescription component = this.item.gameObject.GetComponent<InfoDescription>();
				bool flag2 = component != null && !string.IsNullOrEmpty(component.description);
				string simpleTooltip = flag2 ? component.description : properName;
				this.tooltip.SizingSetting = (flag2 ? ToolTip.ToolTipSizeSetting.MaxWidthWrapContent : ToolTip.ToolTipSizeSetting.DynamicWidthNoWrap);
				this.tooltip.SetSimpleTooltip(simpleTooltip);
			}
			else
			{
				this.tooltip.SizingSetting = ToolTip.ToolTipSizeSetting.MaxWidthWrapContent;
				this.tooltip.SetSimpleTooltip(this.customTooltipFunc(this.minionSlotInstance.assignables));
			}
		}
		else
		{
			this.nameLabel.text = OwnablesSecondSideScreenRow.NO_DATA_MESSAGE;
			this.tooltip.SetSimpleTooltip(null);
		}
		bool flag3 = this.item != null && this.minionSlotInstance != null && !this.minionSlotInstance.IsUnassigning() && this.minionSlotInstance.assignable == this.item;
		this.toggle.ChangeState(flag3 ? 1 : 0);
		this.emptyIcon.gameObject.SetActive(this.item == null);
		this.icon.gameObject.SetActive(this.item != null);
		this.eyeButton.gameObject.SetActive(this.item != null);
		this.statusLabel.gameObject.SetActive(this.item != null);
	}

	// Token: 0x0600AD1F RID: 44319 RVA: 0x00420F34 File Offset: 0x0041F134
	public void ClearData()
	{
		if (this.item != null)
		{
			if (this.destroyListenerIDX != -1)
			{
				this.item.Unsubscribe(this.destroyListenerIDX);
			}
			if (this.changeAssignmentListenerIDX != -1)
			{
				this.item.Unsubscribe(this.changeAssignmentListenerIDX);
			}
		}
		this.minionSlotInstance = null;
		this.item = null;
		this.destroyListenerIDX = -1;
		this.changeAssignmentListenerIDX = -1;
		this.Refresh();
	}

	// Token: 0x0600AD20 RID: 44320 RVA: 0x00114F1E File Offset: 0x0011311E
	private void _OnItemAssignationChanged(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemAssigneeChanged = this.OnRowItemAssigneeChanged;
		if (onRowItemAssigneeChanged == null)
		{
			return;
		}
		onRowItemAssigneeChanged(this);
	}

	// Token: 0x0600AD21 RID: 44321 RVA: 0x00114F31 File Offset: 0x00113131
	private void _OnRowItemDestroyed(object o)
	{
		Action<OwnablesSecondSideScreenRow> onRowItemDestroyed = this.OnRowItemDestroyed;
		if (onRowItemDestroyed == null)
		{
			return;
		}
		onRowItemDestroyed(this);
	}

	// Token: 0x0600AD22 RID: 44322 RVA: 0x00114F44 File Offset: 0x00113144
	private void OnMultitoggleClicked()
	{
		Action<OwnablesSecondSideScreenRow> onRowClicked = this.OnRowClicked;
		if (onRowClicked == null)
		{
			return;
		}
		onRowClicked(this);
	}

	// Token: 0x0600AD23 RID: 44323 RVA: 0x00420FA8 File Offset: 0x0041F1A8
	private void FocusCameraOnAssignedItem()
	{
		if (this.item != null)
		{
			GameObject gameObject = this.item.gameObject;
			if (this.item.HasTag(GameTags.Equipped))
			{
				gameObject = this.item.assignee.GetOwners()[0].GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			}
			GameUtil.FocusCamera(gameObject.transform, false, true);
		}
	}

	// Token: 0x04008834 RID: 34868
	public static string NO_DATA_MESSAGE = UI.UISIDESCREENS.OWNABLESSIDESCREEN.NO_ITEM_FOUND;

	// Token: 0x04008835 RID: 34869
	public static string NOT_ASSIGNED = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.NOT_ASSIGNED;

	// Token: 0x04008836 RID: 34870
	public static string ASSIGNED_TO_SELF = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_SELF_STATUS;

	// Token: 0x04008837 RID: 34871
	public static string ASSIGNED_TO_OTHER = UI.UISIDESCREENS.OWNABLESSECONDSIDESCREEN.ASSIGNED_TO_OTHER_STATUS;

	// Token: 0x04008838 RID: 34872
	public KImage icon;

	// Token: 0x04008839 RID: 34873
	public KImage emptyIcon;

	// Token: 0x0400883A RID: 34874
	public LocText nameLabel;

	// Token: 0x0400883B RID: 34875
	public LocText statusLabel;

	// Token: 0x0400883C RID: 34876
	public Button eyeButton;

	// Token: 0x0400883D RID: 34877
	public ToolTip tooltip;

	// Token: 0x0400883E RID: 34878
	public Action<OwnablesSecondSideScreenRow> OnRowItemAssigneeChanged;

	// Token: 0x0400883F RID: 34879
	public Action<OwnablesSecondSideScreenRow> OnRowItemDestroyed;

	// Token: 0x04008840 RID: 34880
	public Action<OwnablesSecondSideScreenRow> OnRowClicked;

	// Token: 0x04008841 RID: 34881
	public Func<Assignables, string> customTooltipFunc;

	// Token: 0x04008844 RID: 34884
	private MultiToggle toggle;

	// Token: 0x04008845 RID: 34885
	private int changeAssignmentListenerIDX = -1;

	// Token: 0x04008846 RID: 34886
	private int destroyListenerIDX = -1;
}
