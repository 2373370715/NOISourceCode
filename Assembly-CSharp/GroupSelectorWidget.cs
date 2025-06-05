using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020C9 RID: 8393
public class GroupSelectorWidget : MonoBehaviour
{
	// Token: 0x0600B2F0 RID: 45808 RVA: 0x00118E0D File Offset: 0x0011700D
	public void Initialize(object widget_id, IList<GroupSelectorWidget.ItemData> options, GroupSelectorWidget.ItemCallbacks item_callbacks)
	{
		this.widgetID = widget_id;
		this.options = options;
		this.itemCallbacks = item_callbacks;
		this.addItemButton.onClick += this.OnAddItemClicked;
	}

	// Token: 0x0600B2F1 RID: 45809 RVA: 0x0043F164 File Offset: 0x0043D364
	public void Reconfigure(IList<int> selected_option_indices)
	{
		this.selectedOptionIndices.Clear();
		this.selectedOptionIndices.AddRange(selected_option_indices);
		this.selectedOptionIndices.Sort();
		this.addItemButton.isInteractable = (this.selectedOptionIndices.Count < this.options.Count);
		this.RebuildSelectedVisualizers();
	}

	// Token: 0x0600B2F2 RID: 45810 RVA: 0x0043F1BC File Offset: 0x0043D3BC
	private void OnAddItemClicked()
	{
		if (!this.IsSubPanelOpen())
		{
			if (this.RebuildSubPanelOptions() > 0)
			{
				this.unselectedItemsPanel.GetComponent<GridLayoutGroup>().constraintCount = Mathf.Min(this.numExpectedPanelColumns, this.unselectedItemsPanel.childCount);
				this.unselectedItemsPanel.gameObject.SetActive(true);
				this.unselectedItemsPanel.GetComponent<Selectable>().Select();
				return;
			}
		}
		else
		{
			this.CloseSubPanel();
		}
	}

	// Token: 0x0600B2F3 RID: 45811 RVA: 0x00118E3B File Offset: 0x0011703B
	private void OnItemAdded(int option_idx)
	{
		if (this.itemCallbacks.onItemAdded != null)
		{
			this.itemCallbacks.onItemAdded(this.widgetID, this.options[option_idx].userData);
			this.RebuildSubPanelOptions();
		}
	}

	// Token: 0x0600B2F4 RID: 45812 RVA: 0x00118E78 File Offset: 0x00117078
	private void OnItemRemoved(int option_idx)
	{
		if (this.itemCallbacks.onItemRemoved != null)
		{
			this.itemCallbacks.onItemRemoved(this.widgetID, this.options[option_idx].userData);
		}
	}

	// Token: 0x0600B2F5 RID: 45813 RVA: 0x0043F228 File Offset: 0x0043D428
	private void RebuildSelectedVisualizers()
	{
		foreach (GameObject original in this.selectedVisualizers)
		{
			Util.KDestroyGameObject(original);
		}
		this.selectedVisualizers.Clear();
		foreach (int idx in this.selectedOptionIndices)
		{
			GameObject item = this.CreateItem(idx, new Action<int>(this.OnItemRemoved), this.selectedItemsPanel.gameObject, true);
			this.selectedVisualizers.Add(item);
		}
	}

	// Token: 0x0600B2F6 RID: 45814 RVA: 0x0043F2EC File Offset: 0x0043D4EC
	private GameObject CreateItem(int idx, Action<int> on_click, GameObject parent, bool is_selected_item)
	{
		GameObject gameObject = Util.KInstantiateUI(this.itemTemplate, parent, true);
		KButton component = gameObject.GetComponent<KButton>();
		component.onClick += delegate()
		{
			on_click(idx);
		};
		component.fgImage.sprite = this.options[idx].sprite;
		if (parent == this.selectedItemsPanel.gameObject)
		{
			HierarchyReferences component2 = component.GetComponent<HierarchyReferences>();
			if (component2 != null)
			{
				Component reference = component2.GetReference("CancelImg");
				if (reference != null)
				{
					reference.gameObject.SetActive(true);
				}
			}
		}
		gameObject.GetComponent<ToolTip>().OnToolTip = (() => this.itemCallbacks.getItemHoverText(this.widgetID, this.options[idx].userData, is_selected_item));
		return gameObject;
	}

	// Token: 0x0600B2F7 RID: 45815 RVA: 0x00118EAE File Offset: 0x001170AE
	public bool IsSubPanelOpen()
	{
		return this.unselectedItemsPanel.gameObject.activeSelf;
	}

	// Token: 0x0600B2F8 RID: 45816 RVA: 0x00118EC0 File Offset: 0x001170C0
	public void CloseSubPanel()
	{
		this.ClearSubPanelOptions();
		this.unselectedItemsPanel.gameObject.SetActive(false);
	}

	// Token: 0x0600B2F9 RID: 45817 RVA: 0x0043F3C0 File Offset: 0x0043D5C0
	private void ClearSubPanelOptions()
	{
		foreach (object obj in this.unselectedItemsPanel.transform)
		{
			Util.KDestroyGameObject(((Transform)obj).gameObject);
		}
	}

	// Token: 0x0600B2FA RID: 45818 RVA: 0x0043F420 File Offset: 0x0043D620
	private int RebuildSubPanelOptions()
	{
		IList<int> list = this.itemCallbacks.getSubPanelDisplayIndices(this.widgetID);
		if (list.Count > 0)
		{
			this.ClearSubPanelOptions();
			using (IEnumerator<int> enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int num = enumerator.Current;
					if (!this.selectedOptionIndices.Contains(num))
					{
						this.CreateItem(num, new Action<int>(this.OnItemAdded), this.unselectedItemsPanel.gameObject, false);
					}
				}
				goto IL_7E;
			}
		}
		this.CloseSubPanel();
		IL_7E:
		return list.Count;
	}

	// Token: 0x04008D67 RID: 36199
	[SerializeField]
	private GameObject itemTemplate;

	// Token: 0x04008D68 RID: 36200
	[SerializeField]
	private RectTransform selectedItemsPanel;

	// Token: 0x04008D69 RID: 36201
	[SerializeField]
	private RectTransform unselectedItemsPanel;

	// Token: 0x04008D6A RID: 36202
	[SerializeField]
	private KButton addItemButton;

	// Token: 0x04008D6B RID: 36203
	[SerializeField]
	private int numExpectedPanelColumns = 3;

	// Token: 0x04008D6C RID: 36204
	private object widgetID;

	// Token: 0x04008D6D RID: 36205
	private GroupSelectorWidget.ItemCallbacks itemCallbacks;

	// Token: 0x04008D6E RID: 36206
	private IList<GroupSelectorWidget.ItemData> options;

	// Token: 0x04008D6F RID: 36207
	private List<int> selectedOptionIndices = new List<int>();

	// Token: 0x04008D70 RID: 36208
	private List<GameObject> selectedVisualizers = new List<GameObject>();

	// Token: 0x020020CA RID: 8394
	[Serializable]
	public struct ItemData
	{
		// Token: 0x0600B2FC RID: 45820 RVA: 0x00118EFE File Offset: 0x001170FE
		public ItemData(Sprite sprite, object user_data)
		{
			this.sprite = sprite;
			this.userData = user_data;
		}

		// Token: 0x04008D71 RID: 36209
		public Sprite sprite;

		// Token: 0x04008D72 RID: 36210
		public object userData;
	}

	// Token: 0x020020CB RID: 8395
	public struct ItemCallbacks
	{
		// Token: 0x04008D73 RID: 36211
		public Func<object, IList<int>> getSubPanelDisplayIndices;

		// Token: 0x04008D74 RID: 36212
		public Action<object, object> onItemAdded;

		// Token: 0x04008D75 RID: 36213
		public Action<object, object> onItemRemoved;

		// Token: 0x04008D76 RID: 36214
		public Func<object, object, bool, string> getItemHoverText;
	}
}
