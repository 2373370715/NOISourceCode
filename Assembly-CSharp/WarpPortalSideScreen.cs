using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002056 RID: 8278
public class WarpPortalSideScreen : SideScreenContent
{
	// Token: 0x0600B001 RID: 45057 RVA: 0x0042C720 File Offset: 0x0042A920
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
		this.cancelButtonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
		this.button.onClick += this.OnButtonClick;
		this.cancelButton.onClick += this.OnCancelClick;
		this.Refresh(null);
	}

	// Token: 0x0600B002 RID: 45058 RVA: 0x00117157 File Offset: 0x00115357
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<WarpPortal>() != null;
	}

	// Token: 0x0600B003 RID: 45059 RVA: 0x0042C794 File Offset: 0x0042A994
	public override void SetTarget(GameObject target)
	{
		WarpPortal component = target.GetComponent<WarpPortal>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a WarpPortal associated with it.");
			return;
		}
		this.target = component;
		target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
		this.Refresh(null);
	}

	// Token: 0x0600B004 RID: 45060 RVA: 0x0042C7E4 File Offset: 0x0042A9E4
	private void Update()
	{
		if (this.progressBar.activeSelf)
		{
			RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
			float num = this.target.rechargeProgress / 3000f;
			rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
			this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
	}

	// Token: 0x0600B005 RID: 45061 RVA: 0x00117165 File Offset: 0x00115365
	private void OnButtonClick()
	{
		if (this.target.ReadyToWarp)
		{
			this.target.StartWarpSequence();
			this.Refresh(null);
		}
	}

	// Token: 0x0600B006 RID: 45062 RVA: 0x00117186 File Offset: 0x00115386
	private void OnCancelClick()
	{
		this.target.CancelAssignment();
		this.Refresh(null);
	}

	// Token: 0x0600B007 RID: 45063 RVA: 0x0042C860 File Offset: 0x0042AA60
	private void Refresh(object data = null)
	{
		this.progressBar.SetActive(false);
		this.cancelButton.gameObject.SetActive(false);
		if (!(this.target != null))
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
			this.button.gameObject.SetActive(false);
			return;
		}
		if (this.target.ReadyToWarp)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
			this.button.gameObject.SetActive(true);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.button.gameObject.SetActive(false);
			this.progressBar.SetActive(true);
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
			return;
		}
		if (this.target.IsWorking)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
			this.button.gameObject.SetActive(false);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
		this.button.gameObject.SetActive(false);
	}

	// Token: 0x04008A48 RID: 35400
	[SerializeField]
	private LocText label;

	// Token: 0x04008A49 RID: 35401
	[SerializeField]
	private KButton button;

	// Token: 0x04008A4A RID: 35402
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04008A4B RID: 35403
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04008A4C RID: 35404
	[SerializeField]
	private LocText cancelButtonLabel;

	// Token: 0x04008A4D RID: 35405
	[SerializeField]
	private WarpPortal target;

	// Token: 0x04008A4E RID: 35406
	[SerializeField]
	private GameObject contents;

	// Token: 0x04008A4F RID: 35407
	[SerializeField]
	private GameObject progressBar;

	// Token: 0x04008A50 RID: 35408
	[SerializeField]
	private LocText progressLabel;
}
