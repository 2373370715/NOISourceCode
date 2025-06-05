using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001B1C RID: 6940
public class FileNameDialog : KModalScreen
{
	// Token: 0x06009162 RID: 37218 RVA: 0x0010374B File Offset: 0x0010194B
	public override float GetSortKey()
	{
		return 150f;
	}

	// Token: 0x06009163 RID: 37219 RVA: 0x00103752 File Offset: 0x00101952
	public void SetTextAndSelect(string text)
	{
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.text = text;
		this.inputField.Select();
	}

	// Token: 0x06009164 RID: 37220 RVA: 0x0038D390 File Offset: 0x0038B590
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.confirmButton.onClick += this.OnConfirm;
		this.cancelButton.onClick += this.OnCancel;
		this.closeButton.onClick += this.OnCancel;
		this.inputField.onValueChanged.AddListener(delegate(string <p0>)
		{
			Util.ScrubInputField(this.inputField, false, false);
		});
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
	}

	// Token: 0x06009165 RID: 37221 RVA: 0x0010377A File Offset: 0x0010197A
	protected override void OnActivate()
	{
		base.OnActivate();
		this.inputField.Select();
		this.inputField.ActivateInputField();
		CameraController.Instance.DisableUserCameraControl = true;
	}

	// Token: 0x06009166 RID: 37222 RVA: 0x001037A3 File Offset: 0x001019A3
	protected override void OnDeactivate()
	{
		CameraController.Instance.DisableUserCameraControl = false;
		base.OnDeactivate();
	}

	// Token: 0x06009167 RID: 37223 RVA: 0x0038D420 File Offset: 0x0038B620
	public void OnConfirm()
	{
		if (this.onConfirm != null && !string.IsNullOrEmpty(this.inputField.text))
		{
			string text = this.inputField.text;
			if (!text.EndsWith(".sav"))
			{
				text += ".sav";
			}
			this.onConfirm(text);
			this.Deactivate();
		}
	}

	// Token: 0x06009168 RID: 37224 RVA: 0x001037B6 File Offset: 0x001019B6
	private void OnEndEdit(string str)
	{
		if (Localization.HasDirtyWords(str))
		{
			this.inputField.text = "";
		}
	}

	// Token: 0x06009169 RID: 37225 RVA: 0x001037D0 File Offset: 0x001019D0
	public void OnCancel()
	{
		if (this.onCancel != null)
		{
			this.onCancel();
		}
		this.Deactivate();
	}

	// Token: 0x0600916A RID: 37226 RVA: 0x001037EB File Offset: 0x001019EB
	public override void OnKeyUp(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
		}
		else if (e.TryConsume(global::Action.DialogSubmit))
		{
			this.OnConfirm();
		}
		e.Consumed = true;
	}

	// Token: 0x0600916B RID: 37227 RVA: 0x00103818 File Offset: 0x00101A18
	public override void OnKeyDown(KButtonEvent e)
	{
		e.Consumed = true;
	}

	// Token: 0x04006E05 RID: 28165
	public Action<string> onConfirm;

	// Token: 0x04006E06 RID: 28166
	public System.Action onCancel;

	// Token: 0x04006E07 RID: 28167
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04006E08 RID: 28168
	[SerializeField]
	private KButton confirmButton;

	// Token: 0x04006E09 RID: 28169
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x04006E0A RID: 28170
	[SerializeField]
	private KButton closeButton;
}
