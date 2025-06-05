using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D0E RID: 7438
[AddComponentMenu("KMonoBehaviour/scripts/DropDown")]
public class DropDown : KMonoBehaviour
{
	// Token: 0x17000A38 RID: 2616
	// (get) Token: 0x06009B5B RID: 39771 RVA: 0x001099A8 File Offset: 0x00107BA8
	// (set) Token: 0x06009B5C RID: 39772 RVA: 0x001099B0 File Offset: 0x00107BB0
	public bool open { get; private set; }

	// Token: 0x17000A39 RID: 2617
	// (get) Token: 0x06009B5D RID: 39773 RVA: 0x001099B9 File Offset: 0x00107BB9
	public List<IListableOption> Entries
	{
		get
		{
			return this.entries;
		}
	}

	// Token: 0x06009B5E RID: 39774 RVA: 0x003CC3C4 File Offset: 0x003CA5C4
	public void Initialize(IEnumerable<IListableOption> contentKeys, Action<IListableOption, object> onEntrySelectedAction, Func<IListableOption, IListableOption, object, int> sortFunction = null, Action<DropDownEntry, object> refreshAction = null, bool displaySelectedValueWhenClosed = true, object targetData = null)
	{
		this.targetData = targetData;
		this.sortFunction = sortFunction;
		this.onEntrySelectedAction = onEntrySelectedAction;
		this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
		this.rowRefreshAction = refreshAction;
		this.ChangeContent(contentKeys);
		this.openButton.ClearOnClick();
		this.openButton.onClick += delegate()
		{
			this.OnClick();
		};
		this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
	}

	// Token: 0x06009B5F RID: 39775 RVA: 0x001099C1 File Offset: 0x00107BC1
	public void CustomizeEmptyRow(string txt, Sprite icon)
	{
		this.emptyRowLabel = txt;
		this.emptyRowSprite = icon;
	}

	// Token: 0x06009B60 RID: 39776 RVA: 0x001099D1 File Offset: 0x00107BD1
	public void OnClick()
	{
		if (!this.open)
		{
			this.Open();
			return;
		}
		this.Close();
	}

	// Token: 0x06009B61 RID: 39777 RVA: 0x003CC438 File Offset: 0x003CA638
	public void ChangeContent(IEnumerable<IListableOption> contentKeys)
	{
		this.entries.Clear();
		foreach (IListableOption item in contentKeys)
		{
			this.entries.Add(item);
		}
		this.built = false;
	}

	// Token: 0x06009B62 RID: 39778 RVA: 0x003CC498 File Offset: 0x003CA698
	private void Update()
	{
		if (!this.open)
		{
			return;
		}
		if (!Input.GetMouseButtonDown(0) && Input.GetAxis("Mouse ScrollWheel") == 0f && !KInputManager.steamInputInterpreter.GetSteamInputActionIsDown(global::Action.MouseLeft))
		{
			return;
		}
		float canvasScale = this.canvasScaler.GetCanvasScale();
		if (this.scrollRect.rectTransform().GetPosition().x + this.scrollRect.rectTransform().sizeDelta.x * canvasScale < KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().x > KInputManager.GetMousePos().x || this.scrollRect.rectTransform().GetPosition().y - this.scrollRect.rectTransform().sizeDelta.y * canvasScale > KInputManager.GetMousePos().y || this.scrollRect.rectTransform().GetPosition().y < KInputManager.GetMousePos().y)
		{
			this.Close();
		}
	}

	// Token: 0x06009B63 RID: 39779 RVA: 0x003CC59C File Offset: 0x003CA79C
	private void Build(List<IListableOption> contentKeys)
	{
		this.built = true;
		for (int i = this.contentContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.contentContainer.GetChild(i));
		}
		this.rowLookup.Clear();
		if (this.addEmptyRow)
		{
			this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			this.emptyRow.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(null, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = (this.emptyRowLabel ?? UI.DROPDOWN.NONE);
				}
				this.Close();
			};
			string text = this.emptyRowLabel ?? UI.DROPDOWN.NONE;
			this.emptyRow.GetComponent<DropDownEntry>().label.text = text;
			if (this.emptyRowSprite != null)
			{
				this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
			}
		}
		for (int j = 0; j < contentKeys.Count; j++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
			IListableOption id = contentKeys[j];
			gameObject.GetComponent<DropDownEntry>().entryData = id;
			gameObject.GetComponent<KButton>().onClick += delegate()
			{
				this.onEntrySelectedAction(id, this.targetData);
				if (this.displaySelectedValueWhenClosed)
				{
					this.selectedLabel.text = id.GetProperName();
				}
				this.Close();
			};
			this.rowLookup.Add(id, gameObject);
		}
		this.RefreshEntries();
		this.Close();
		this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
		this.scrollRect.gameObject.SetActive(false);
	}

	// Token: 0x06009B64 RID: 39780 RVA: 0x003CC73C File Offset: 0x003CA93C
	private void RefreshEntries()
	{
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
			component.label.text = keyValuePair.Key.GetProperName();
			if (component.portrait != null && keyValuePair.Key is IAssignableIdentity)
			{
				component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
			}
		}
		if (this.sortFunction != null)
		{
			this.entries.Sort((IListableOption a, IListableOption b) => this.sortFunction(a, b, this.targetData));
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.rowLookup[this.entries[i]].transform.SetAsFirstSibling();
			}
			if (this.emptyRow != null)
			{
				this.emptyRow.transform.SetAsFirstSibling();
			}
		}
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair2 in this.rowLookup)
		{
			DropDownEntry component2 = keyValuePair2.Value.GetComponent<DropDownEntry>();
			this.rowRefreshAction(component2, this.targetData);
		}
		if (this.emptyRow != null)
		{
			this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
		}
	}

	// Token: 0x06009B65 RID: 39781 RVA: 0x001099E8 File Offset: 0x00107BE8
	protected override void OnCleanUp()
	{
		Util.KDestroyGameObject(this.scrollRect);
		base.OnCleanUp();
	}

	// Token: 0x06009B66 RID: 39782 RVA: 0x003CC8E0 File Offset: 0x003CAAE0
	public void Open()
	{
		if (this.open)
		{
			return;
		}
		if (!this.built)
		{
			this.Build(this.entries);
		}
		else
		{
			this.RefreshEntries();
		}
		this.open = true;
		this.scrollRect.gameObject.SetActive(true);
		this.scrollRect.rectTransform().localScale = Vector3.one;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(true);
		}
		float num = Mathf.Max(32f, this.rowEntryPrefab.GetComponent<LayoutElement>().preferredHeight);
		this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, num * (float)Mathf.Min(this.contentContainer.childCount, 8));
		Vector3 vector = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0f);
		Vector2 v = new Vector2(Mathf.Min(0f, (float)Screen.width - (vector.x + (this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.x))), -Mathf.Min(0f, vector.y - (this.scrollRect.rectTransform().sizeDelta.y * this.canvasScaler.GetCanvasScale() + DropDown.edgePadding.y)));
		vector += v;
		this.scrollRect.rectTransform().SetPosition(vector);
	}

	// Token: 0x06009B67 RID: 39783 RVA: 0x003CCAC4 File Offset: 0x003CACC4
	public void Close()
	{
		if (!this.open)
		{
			return;
		}
		this.open = false;
		foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
		{
			keyValuePair.Value.SetActive(false);
		}
		this.scrollRect.SetActive(false);
	}

	// Token: 0x04007972 RID: 31090
	public GameObject targetDropDownContainer;

	// Token: 0x04007973 RID: 31091
	public LocText selectedLabel;

	// Token: 0x04007975 RID: 31093
	public KButton openButton;

	// Token: 0x04007976 RID: 31094
	public Transform contentContainer;

	// Token: 0x04007977 RID: 31095
	public GameObject scrollRect;

	// Token: 0x04007978 RID: 31096
	public RectTransform dropdownAlignmentTarget;

	// Token: 0x04007979 RID: 31097
	public GameObject rowEntryPrefab;

	// Token: 0x0400797A RID: 31098
	public bool addEmptyRow = true;

	// Token: 0x0400797B RID: 31099
	private static Vector2 edgePadding = new Vector2(8f, 8f);

	// Token: 0x0400797C RID: 31100
	public object targetData;

	// Token: 0x0400797D RID: 31101
	private List<IListableOption> entries = new List<IListableOption>();

	// Token: 0x0400797E RID: 31102
	private Action<IListableOption, object> onEntrySelectedAction;

	// Token: 0x0400797F RID: 31103
	private Action<DropDownEntry, object> rowRefreshAction;

	// Token: 0x04007980 RID: 31104
	public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();

	// Token: 0x04007981 RID: 31105
	private Func<IListableOption, IListableOption, object, int> sortFunction;

	// Token: 0x04007982 RID: 31106
	private GameObject emptyRow;

	// Token: 0x04007983 RID: 31107
	private string emptyRowLabel;

	// Token: 0x04007984 RID: 31108
	private Sprite emptyRowSprite;

	// Token: 0x04007985 RID: 31109
	private bool built;

	// Token: 0x04007986 RID: 31110
	private bool displaySelectedValueWhenClosed = true;

	// Token: 0x04007987 RID: 31111
	private const int ROWS_BEFORE_SCROLL = 8;

	// Token: 0x04007988 RID: 31112
	private KCanvasScaler canvasScaler;
}
