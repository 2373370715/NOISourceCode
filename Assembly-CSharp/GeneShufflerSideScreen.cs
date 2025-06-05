using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001FD2 RID: 8146
public class GeneShufflerSideScreen : SideScreenContent
{
	// Token: 0x0600AC19 RID: 44057 RVA: 0x001144EB File Offset: 0x001126EB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.button.onClick += this.OnButtonClick;
		this.Refresh();
	}

	// Token: 0x0600AC1A RID: 44058 RVA: 0x00114510 File Offset: 0x00112710
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<GeneShuffler>() != null;
	}

	// Token: 0x0600AC1B RID: 44059 RVA: 0x0041C394 File Offset: 0x0041A594
	public override void SetTarget(GameObject target)
	{
		GeneShuffler component = target.GetComponent<GeneShuffler>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a GeneShuffler associated with it.");
			return;
		}
		this.target = component;
		this.Refresh();
	}

	// Token: 0x0600AC1C RID: 44060 RVA: 0x0041C3CC File Offset: 0x0041A5CC
	private void OnButtonClick()
	{
		if (this.target.WorkComplete)
		{
			this.target.SetWorkTime(0f);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.target.RequestRecharge(!this.target.RechargeRequested);
			this.Refresh();
		}
	}

	// Token: 0x0600AC1D RID: 44061 RVA: 0x0041C424 File Offset: 0x0041A624
	private void Refresh()
	{
		if (!(this.target != null))
		{
			this.contents.SetActive(false);
			return;
		}
		if (this.target.WorkComplete)
		{
			this.contents.SetActive(true);
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
			this.button.gameObject.SetActive(true);
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
			return;
		}
		if (this.target.IsConsumed)
		{
			this.contents.SetActive(true);
			this.button.gameObject.SetActive(true);
			if (this.target.RechargeRequested)
			{
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
				this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
				return;
			}
			this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
			this.buttonLabel.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
			return;
		}
		else
		{
			if (this.target.IsWorking)
			{
				this.contents.SetActive(true);
				this.label.text = UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
				this.button.gameObject.SetActive(false);
				return;
			}
			this.contents.SetActive(false);
			return;
		}
	}

	// Token: 0x04008786 RID: 34694
	[SerializeField]
	private LocText label;

	// Token: 0x04008787 RID: 34695
	[SerializeField]
	private KButton button;

	// Token: 0x04008788 RID: 34696
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x04008789 RID: 34697
	[SerializeField]
	private GeneShuffler target;

	// Token: 0x0400878A RID: 34698
	[SerializeField]
	private GameObject contents;
}
