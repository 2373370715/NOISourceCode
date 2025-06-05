using System;
using System.Collections;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001D11 RID: 7441
public class EditableTitleBar : TitleBar
{
	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06009B70 RID: 39792 RVA: 0x003CCBEC File Offset: 0x003CADEC
	// (remove) Token: 0x06009B71 RID: 39793 RVA: 0x003CCC24 File Offset: 0x003CAE24
	public event Action<string> OnNameChanged;

	// Token: 0x1400002C RID: 44
	// (add) Token: 0x06009B72 RID: 39794 RVA: 0x003CCC5C File Offset: 0x003CAE5C
	// (remove) Token: 0x06009B73 RID: 39795 RVA: 0x003CCC94 File Offset: 0x003CAE94
	public event System.Action OnStartedEditing;

	// Token: 0x06009B74 RID: 39796 RVA: 0x003CCCCC File Offset: 0x003CAECC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.onClick += this.GenerateRandomName;
		}
		if (this.editNameButton != null)
		{
			this.EnableEditButtonClick();
		}
		if (this.inputField != null)
		{
			this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		}
	}

	// Token: 0x06009B75 RID: 39797 RVA: 0x003CCD44 File Offset: 0x003CAF44
	public void UpdateRenameTooltip(GameObject target)
	{
		if (this.editNameButton != null && target != null)
		{
			if (target.GetComponent<MinionBrain>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAME;
			}
			if (target.GetComponent<ClustercraftExteriorDoor>() != null || target.GetComponent<CommandModule>() != null)
			{
				this.editNameButton.GetComponent<ToolTip>().toolTip = UI.TOOLTIPS.EDITNAMEROCKET;
				return;
			}
			this.editNameButton.GetComponent<ToolTip>().toolTip = string.Format(UI.TOOLTIPS.EDITNAMEGENERIC, target.GetProperName());
		}
	}

	// Token: 0x06009B76 RID: 39798 RVA: 0x003CCDF4 File Offset: 0x003CAFF4
	private void OnEndEdit(string finalStr)
	{
		finalStr = Localization.FilterDirtyWords(finalStr);
		this.SetEditingState(false);
		if (string.IsNullOrEmpty(finalStr))
		{
			return;
		}
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(finalStr);
		}
		this.titleText.text = finalStr;
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		if (base.gameObject.activeInHierarchy && base.enabled)
		{
			this.postEndEdit = base.StartCoroutine(this.PostOnEndEditRoutine());
		}
	}

	// Token: 0x06009B77 RID: 39799 RVA: 0x00109A5A File Offset: 0x00107C5A
	private IEnumerator PostOnEndEditRoutine()
	{
		int i = 0;
		while (i < 10)
		{
			int num = i;
			i = num + 1;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.EnableEditButtonClick();
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06009B78 RID: 39800 RVA: 0x00109A69 File Offset: 0x00107C69
	private IEnumerator PreToggleNameEditingRoutine()
	{
		yield return SequenceUtil.WaitForEndOfFrame;
		this.ToggleNameEditing();
		this.preToggleNameEditing = null;
		yield break;
	}

	// Token: 0x06009B79 RID: 39801 RVA: 0x00109A78 File Offset: 0x00107C78
	private void EnableEditButtonClick()
	{
		this.editNameButton.onClick += delegate()
		{
			if (this.preToggleNameEditing != null)
			{
				return;
			}
			this.preToggleNameEditing = base.StartCoroutine(this.PreToggleNameEditingRoutine());
		};
	}

	// Token: 0x06009B7A RID: 39802 RVA: 0x003CCE74 File Offset: 0x003CB074
	private void GenerateRandomName()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		string text = GameUtil.GenerateRandomDuplicantName();
		if (this.OnNameChanged != null)
		{
			this.OnNameChanged(text);
		}
		this.titleText.text = text;
		this.SetEditingState(true);
	}

	// Token: 0x06009B7B RID: 39803 RVA: 0x003CCEC4 File Offset: 0x003CB0C4
	private void ToggleNameEditing()
	{
		this.editNameButton.ClearOnClick();
		bool flag = !this.inputField.gameObject.activeInHierarchy;
		if (this.randomNameButton != null)
		{
			this.randomNameButton.gameObject.SetActive(flag);
		}
		this.SetEditingState(flag);
	}

	// Token: 0x06009B7C RID: 39804 RVA: 0x003CCF18 File Offset: 0x003CB118
	private void SetEditingState(bool state)
	{
		this.titleText.gameObject.SetActive(!state);
		if (this.setCameraControllerState)
		{
			CameraController.Instance.DisableUserCameraControl = state;
		}
		if (this.inputField == null)
		{
			return;
		}
		this.inputField.gameObject.SetActive(state);
		if (state)
		{
			this.inputField.text = this.titleText.text;
			this.inputField.Select();
			this.inputField.ActivateInputField();
			if (this.OnStartedEditing != null)
			{
				this.OnStartedEditing();
				return;
			}
		}
		else
		{
			this.inputField.DeactivateInputField();
		}
	}

	// Token: 0x06009B7D RID: 39805 RVA: 0x00109A91 File Offset: 0x00107C91
	public void ForceStopEditing()
	{
		if (this.postEndEdit != null)
		{
			base.StopCoroutine(this.postEndEdit);
		}
		this.editNameButton.ClearOnClick();
		this.SetEditingState(false);
		this.EnableEditButtonClick();
	}

	// Token: 0x06009B7E RID: 39806 RVA: 0x00109ABF File Offset: 0x00107CBF
	public void SetUserEditable(bool editable)
	{
		this.userEditable = editable;
		this.editNameButton.gameObject.SetActive(editable);
		this.editNameButton.ClearOnClick();
		this.EnableEditButtonClick();
	}

	// Token: 0x04007991 RID: 31121
	public KButton editNameButton;

	// Token: 0x04007992 RID: 31122
	public KButton randomNameButton;

	// Token: 0x04007993 RID: 31123
	public KInputTextField inputField;

	// Token: 0x04007996 RID: 31126
	private Coroutine postEndEdit;

	// Token: 0x04007997 RID: 31127
	private Coroutine preToggleNameEditing;
}
