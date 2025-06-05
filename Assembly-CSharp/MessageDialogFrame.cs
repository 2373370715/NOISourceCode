using System;
using UnityEngine;

// Token: 0x02001E54 RID: 7764
public class MessageDialogFrame : KScreen
{
	// Token: 0x0600A28A RID: 41610 RVA: 0x0010E0D4 File Offset: 0x0010C2D4
	public override float GetSortKey()
	{
		return 15f;
	}

	// Token: 0x0600A28B RID: 41611 RVA: 0x003EDC3C File Offset: 0x003EBE3C
	protected override void OnActivate()
	{
		this.closeButton.onClick += this.OnClickClose;
		this.nextMessageButton.onClick += this.OnClickNextMessage;
		MultiToggle multiToggle = this.dontShowAgainButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.OnClickDontShowAgain));
		bool flag = KPlayerPrefs.GetInt("HideTutorial_CheckState", 0) == 1;
		this.dontShowAgainButton.ChangeState(flag ? 0 : 1);
		base.Subscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
		this.OnMessagesChanged(null);
	}

	// Token: 0x0600A28C RID: 41612 RVA: 0x0010E0DB File Offset: 0x0010C2DB
	protected override void OnDeactivate()
	{
		base.Unsubscribe(Messenger.Instance.gameObject, -599791736, new Action<object>(this.OnMessagesChanged));
	}

	// Token: 0x0600A28D RID: 41613 RVA: 0x0010E0FE File Offset: 0x0010C2FE
	private void OnClickClose()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600A28E RID: 41614 RVA: 0x0010E111 File Offset: 0x0010C311
	private void OnClickNextMessage()
	{
		this.TryDontShowAgain();
		UnityEngine.Object.Destroy(base.gameObject);
		NotificationScreen.Instance.OnClickNextMessage();
	}

	// Token: 0x0600A28F RID: 41615 RVA: 0x003EDCE8 File Offset: 0x003EBEE8
	private void OnClickDontShowAgain()
	{
		this.dontShowAgainButton.NextState();
		bool flag = this.dontShowAgainButton.CurrentState == 0;
		KPlayerPrefs.SetInt("HideTutorial_CheckState", flag ? 1 : 0);
	}

	// Token: 0x0600A290 RID: 41616 RVA: 0x0010E12E File Offset: 0x0010C32E
	private void OnMessagesChanged(object data)
	{
		this.nextMessageButton.gameObject.SetActive(Messenger.Instance.Count != 0);
	}

	// Token: 0x0600A291 RID: 41617 RVA: 0x003EDD20 File Offset: 0x003EBF20
	public void SetMessage(MessageDialog dialog, Message message)
	{
		this.title.text = message.GetTitle().ToUpper();
		dialog.GetComponent<RectTransform>().SetParent(this.body.GetComponent<RectTransform>());
		RectTransform component = dialog.GetComponent<RectTransform>();
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		dialog.transform.SetLocalPosition(Vector3.zero);
		dialog.SetMessage(message);
		dialog.OnClickAction();
		if (dialog.CanDontShowAgain)
		{
			this.dontShowAgainElement.SetActive(true);
			this.dontShowAgainDelegate = new System.Action(dialog.OnDontShowAgain);
			return;
		}
		this.dontShowAgainElement.SetActive(false);
		this.dontShowAgainDelegate = null;
	}

	// Token: 0x0600A292 RID: 41618 RVA: 0x0010E14D File Offset: 0x0010C34D
	private void TryDontShowAgain()
	{
		if (this.dontShowAgainDelegate != null && this.dontShowAgainButton.CurrentState == 0)
		{
			this.dontShowAgainDelegate();
		}
	}

	// Token: 0x04007F44 RID: 32580
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007F45 RID: 32581
	[SerializeField]
	private KToggle nextMessageButton;

	// Token: 0x04007F46 RID: 32582
	[SerializeField]
	private GameObject dontShowAgainElement;

	// Token: 0x04007F47 RID: 32583
	[SerializeField]
	private MultiToggle dontShowAgainButton;

	// Token: 0x04007F48 RID: 32584
	[SerializeField]
	private LocText title;

	// Token: 0x04007F49 RID: 32585
	[SerializeField]
	private RectTransform body;

	// Token: 0x04007F4A RID: 32586
	private System.Action dontShowAgainDelegate;
}
