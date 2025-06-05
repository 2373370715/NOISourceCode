using System;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02002026 RID: 8230
public class SearchBar : KMonoBehaviour
{
	// Token: 0x17000B28 RID: 2856
	// (get) Token: 0x0600AE48 RID: 44616 RVA: 0x00115BEC File Offset: 0x00113DEC
	public string CurrentSearchValue
	{
		get
		{
			if (!string.IsNullOrEmpty(this.inputField.text))
			{
				return this.inputField.text;
			}
			return "";
		}
	}

	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x0600AE49 RID: 44617 RVA: 0x00115C11 File Offset: 0x00113E11
	public bool IsInputFieldEmpty
	{
		get
		{
			return this.inputField.text == "";
		}
	}

	// Token: 0x17000B2A RID: 2858
	// (get) Token: 0x0600AE4B RID: 44619 RVA: 0x00115C31 File Offset: 0x00113E31
	// (set) Token: 0x0600AE4A RID: 44618 RVA: 0x00115C28 File Offset: 0x00113E28
	public bool isEditing { get; protected set; }

	// Token: 0x0600AE4C RID: 44620 RVA: 0x00115C39 File Offset: 0x00113E39
	public virtual void SetPlaceholder(string text)
	{
		this.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = text;
	}

	// Token: 0x0600AE4D RID: 44621 RVA: 0x00425F00 File Offset: 0x00424100
	protected override void OnSpawn()
	{
		this.inputField.ActivateInputField();
		KInputTextField kinputTextField = this.inputField;
		kinputTextField.onFocus = (System.Action)Delegate.Combine(kinputTextField.onFocus, new System.Action(this.OnFocus));
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
		this.clearButton.onClick += this.ClearSearch;
		this.SetPlaceholder(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.SEARCH_PLACEHOLDER);
	}

	// Token: 0x0600AE4E RID: 44622 RVA: 0x00115C51 File Offset: 0x00113E51
	protected void SetEditingState(bool editing)
	{
		this.isEditing = editing;
		Action<bool> editingStateChanged = this.EditingStateChanged;
		if (editingStateChanged != null)
		{
			editingStateChanged(this.isEditing);
		}
		KScreenManager.Instance.RefreshStack();
	}

	// Token: 0x0600AE4F RID: 44623 RVA: 0x00115C7B File Offset: 0x00113E7B
	protected virtual void OnValueChanged(string value)
	{
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x0600AE50 RID: 44624 RVA: 0x00115C8E File Offset: 0x00113E8E
	protected virtual void OnEndEdit(string value)
	{
		this.SetEditingState(false);
	}

	// Token: 0x0600AE51 RID: 44625 RVA: 0x00115C97 File Offset: 0x00113E97
	protected virtual void OnFocus()
	{
		this.SetEditingState(true);
		UISounds.PlaySound(UISounds.Sound.ClickHUD);
		System.Action focused = this.Focused;
		if (focused == null)
		{
			return;
		}
		focused();
	}

	// Token: 0x0600AE52 RID: 44626 RVA: 0x00115CB6 File Offset: 0x00113EB6
	public virtual void ClearSearch()
	{
		this.SetValue("");
	}

	// Token: 0x0600AE53 RID: 44627 RVA: 0x00115CC3 File Offset: 0x00113EC3
	public void SetValue(string value)
	{
		this.inputField.text = value;
		Action<string> valueChanged = this.ValueChanged;
		if (valueChanged == null)
		{
			return;
		}
		valueChanged(value);
	}

	// Token: 0x04008926 RID: 35110
	[SerializeField]
	protected KInputTextField inputField;

	// Token: 0x04008927 RID: 35111
	[SerializeField]
	protected KButton clearButton;

	// Token: 0x04008929 RID: 35113
	public Action<string> ValueChanged;

	// Token: 0x0400892A RID: 35114
	public Action<bool> EditingStateChanged;

	// Token: 0x0400892B RID: 35115
	public System.Action Focused;
}
