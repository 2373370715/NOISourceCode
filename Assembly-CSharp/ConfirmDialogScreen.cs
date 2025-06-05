using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CCC RID: 7372
public class ConfirmDialogScreen : KModalScreen
{
	// Token: 0x060099CC RID: 39372 RVA: 0x001086E3 File Offset: 0x001068E3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060099CD RID: 39373 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x060099CE RID: 39374 RVA: 0x001086F7 File Offset: 0x001068F7
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.OnSelect_CANCEL();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060099CF RID: 39375 RVA: 0x003C5168 File Offset: 0x003C3368
	public void PopupConfirmDialog(string text, System.Action on_confirm, System.Action on_cancel, string configurable_text = null, System.Action on_configurable_clicked = null, string title_text = null, string confirm_text = null, string cancel_text = null, Sprite image_sprite = null)
	{
		while (base.transform.parent.GetComponent<Canvas>() == null && base.transform.parent.parent != null)
		{
			base.transform.SetParent(base.transform.parent.parent);
		}
		base.transform.SetAsLastSibling();
		this.confirmAction = on_confirm;
		this.cancelAction = on_cancel;
		this.configurableAction = on_configurable_clicked;
		int num = 0;
		if (this.confirmAction != null)
		{
			num++;
		}
		if (this.cancelAction != null)
		{
			num++;
		}
		if (this.configurableAction != null)
		{
			num++;
		}
		this.confirmButton.GetComponentInChildren<LocText>().text = ((confirm_text == null) ? UI.CONFIRMDIALOG.OK.text : confirm_text);
		this.cancelButton.GetComponentInChildren<LocText>().text = ((cancel_text == null) ? UI.CONFIRMDIALOG.CANCEL.text : cancel_text);
		this.confirmButton.GetComponent<KButton>().onClick += this.OnSelect_OK;
		this.cancelButton.GetComponent<KButton>().onClick += this.OnSelect_CANCEL;
		this.configurableButton.GetComponent<KButton>().onClick += this.OnSelect_third;
		this.cancelButton.SetActive(on_cancel != null);
		if (this.configurableButton != null)
		{
			this.configurableButton.SetActive(this.configurableAction != null);
			if (configurable_text != null)
			{
				this.configurableButton.GetComponentInChildren<LocText>().text = configurable_text;
			}
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.key = "";
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x060099D0 RID: 39376 RVA: 0x00108710 File Offset: 0x00106910
	public void OnSelect_OK()
	{
		if (this.deactivateOnConfirmAction)
		{
			this.Deactivate();
		}
		if (this.confirmAction != null)
		{
			this.confirmAction();
		}
	}

	// Token: 0x060099D1 RID: 39377 RVA: 0x00108733 File Offset: 0x00106933
	public void OnSelect_CANCEL()
	{
		if (this.deactivateOnCancelAction)
		{
			this.Deactivate();
		}
		if (this.cancelAction != null)
		{
			this.cancelAction();
		}
	}

	// Token: 0x060099D2 RID: 39378 RVA: 0x00108756 File Offset: 0x00106956
	public void OnSelect_third()
	{
		if (this.deactivateOnConfigurableAction)
		{
			this.Deactivate();
		}
		if (this.configurableAction != null)
		{
			this.configurableAction();
		}
	}

	// Token: 0x060099D3 RID: 39379 RVA: 0x00108779 File Offset: 0x00106979
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x040077F8 RID: 30712
	private System.Action confirmAction;

	// Token: 0x040077F9 RID: 30713
	private System.Action cancelAction;

	// Token: 0x040077FA RID: 30714
	private System.Action configurableAction;

	// Token: 0x040077FB RID: 30715
	public bool deactivateOnConfigurableAction = true;

	// Token: 0x040077FC RID: 30716
	public bool deactivateOnConfirmAction = true;

	// Token: 0x040077FD RID: 30717
	public bool deactivateOnCancelAction = true;

	// Token: 0x040077FE RID: 30718
	public System.Action onDeactivateCB;

	// Token: 0x040077FF RID: 30719
	[SerializeField]
	private GameObject confirmButton;

	// Token: 0x04007800 RID: 30720
	[SerializeField]
	private GameObject cancelButton;

	// Token: 0x04007801 RID: 30721
	[SerializeField]
	private GameObject configurableButton;

	// Token: 0x04007802 RID: 30722
	[SerializeField]
	private LocText titleText;

	// Token: 0x04007803 RID: 30723
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04007804 RID: 30724
	[SerializeField]
	private Image image;
}
