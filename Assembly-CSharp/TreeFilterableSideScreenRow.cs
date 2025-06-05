using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02002052 RID: 8274
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenRow")]
public class TreeFilterableSideScreenRow : KMonoBehaviour
{
	// Token: 0x17000B47 RID: 2887
	// (get) Token: 0x0600AFD8 RID: 45016 RVA: 0x00116F2F File Offset: 0x0011512F
	// (set) Token: 0x0600AFD9 RID: 45017 RVA: 0x00116F37 File Offset: 0x00115137
	public bool ArrowExpanded { get; private set; }

	// Token: 0x17000B48 RID: 2888
	// (get) Token: 0x0600AFDA RID: 45018 RVA: 0x00116F40 File Offset: 0x00115140
	// (set) Token: 0x0600AFDB RID: 45019 RVA: 0x00116F48 File Offset: 0x00115148
	public TreeFilterableSideScreen Parent
	{
		get
		{
			return this.parent;
		}
		set
		{
			this.parent = value;
		}
	}

	// Token: 0x0600AFDC RID: 45020 RVA: 0x0042C070 File Offset: 0x0042A270
	public TreeFilterableSideScreenRow.State GetState()
	{
		bool flag = false;
		bool flag2 = false;
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			if (this.parent.GetElementTagAcceptedState(treeFilterableSideScreenElement.GetElementTag()))
			{
				flag = true;
			}
			else
			{
				flag2 = true;
			}
		}
		if (flag && !flag2)
		{
			return TreeFilterableSideScreenRow.State.On;
		}
		if (!flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		if (flag && flag2)
		{
			return TreeFilterableSideScreenRow.State.Mixed;
		}
		if (this.rowElements.Count <= 0)
		{
			return TreeFilterableSideScreenRow.State.Off;
		}
		return TreeFilterableSideScreenRow.State.On;
	}

	// Token: 0x0600AFDD RID: 45021 RVA: 0x00116F51 File Offset: 0x00115151
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.checkBoxToggle;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (this.parent.CurrentSearchValue == "")
			{
				TreeFilterableSideScreenRow.State state = this.GetState();
				if (state > TreeFilterableSideScreenRow.State.Mixed)
				{
					if (state == TreeFilterableSideScreenRow.State.On)
					{
						this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.Off);
						return;
					}
				}
				else
				{
					this.ChangeCheckBoxState(TreeFilterableSideScreenRow.State.On);
				}
			}
		}));
	}

	// Token: 0x0600AFDE RID: 45022 RVA: 0x00116F80 File Offset: 0x00115180
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SetArrowToggleState(this.GetState() > TreeFilterableSideScreenRow.State.Off);
	}

	// Token: 0x0600AFDF RID: 45023 RVA: 0x00116F97 File Offset: 0x00115197
	protected override void OnCmpDisable()
	{
		this.SetArrowToggleState(false);
		base.OnCmpDisable();
	}

	// Token: 0x0600AFE0 RID: 45024 RVA: 0x000C4795 File Offset: 0x000C2995
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x0600AFE1 RID: 45025 RVA: 0x00116FA6 File Offset: 0x001151A6
	public void UpdateCheckBoxVisualState()
	{
		this.checkBoxToggle.ChangeState((int)this.GetState());
		this.visualDirty = false;
	}

	// Token: 0x0600AFE2 RID: 45026 RVA: 0x0042C104 File Offset: 0x0042A304
	public void ChangeCheckBoxState(TreeFilterableSideScreenRow.State newState)
	{
		switch (newState)
		{
		case TreeFilterableSideScreenRow.State.Off:
			for (int i = 0; i < this.rowElements.Count; i++)
			{
				this.rowElements[i].SetCheckBox(false);
			}
			break;
		case TreeFilterableSideScreenRow.State.On:
			for (int j = 0; j < this.rowElements.Count; j++)
			{
				this.rowElements[j].SetCheckBox(true);
			}
			break;
		}
		this.visualDirty = true;
	}

	// Token: 0x0600AFE3 RID: 45027 RVA: 0x00116FC0 File Offset: 0x001151C0
	private void ArrowToggleClicked()
	{
		this.SetArrowToggleState(!this.ArrowExpanded);
		this.RefreshArrowToggleState();
	}

	// Token: 0x0600AFE4 RID: 45028 RVA: 0x00116FD7 File Offset: 0x001151D7
	public void SetArrowToggleState(bool state)
	{
		this.ArrowExpanded = state;
		this.RefreshArrowToggleState();
	}

	// Token: 0x0600AFE5 RID: 45029 RVA: 0x00116FE6 File Offset: 0x001151E6
	private void RefreshArrowToggleState()
	{
		this.arrowToggle.ChangeState(this.ArrowExpanded ? 1 : 0);
		this.elementGroup.SetActive(this.ArrowExpanded);
		this.bgImg.enabled = this.ArrowExpanded;
	}

	// Token: 0x0600AFE6 RID: 45030 RVA: 0x00102F16 File Offset: 0x00101116
	private void ArrowToggleDisabledClick()
	{
		KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
	}

	// Token: 0x0600AFE7 RID: 45031 RVA: 0x00117021 File Offset: 0x00115221
	public void ShowToggleBox(bool show)
	{
		this.checkBoxToggle.gameObject.SetActive(show);
	}

	// Token: 0x0600AFE8 RID: 45032 RVA: 0x00117034 File Offset: 0x00115234
	private void OnElementSelectionChanged(Tag t, bool state)
	{
		if (state)
		{
			this.parent.AddTag(t);
		}
		else
		{
			this.parent.RemoveTag(t);
		}
		this.visualDirty = true;
	}

	// Token: 0x0600AFE9 RID: 45033 RVA: 0x0042C180 File Offset: 0x0042A380
	public void SetElement(Tag mainElementTag, bool state, Dictionary<Tag, bool> filterMap)
	{
		this.subTags.Clear();
		this.rowElements.Clear();
		this.elementName.text = mainElementTag.ProperName();
		this.bgImg.enabled = false;
		string simpleTooltip = string.Format(UI.UISIDESCREENS.TREEFILTERABLESIDESCREEN.CATEGORYBUTTONTOOLTIP, mainElementTag.ProperName());
		this.checkBoxToggle.GetComponent<ToolTip>().SetSimpleTooltip(simpleTooltip);
		if (filterMap.Count == 0)
		{
			if (this.elementGroup.activeInHierarchy)
			{
				this.elementGroup.SetActive(false);
			}
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleDisabledClick);
			this.arrowToggle.ChangeState(0);
		}
		else
		{
			this.arrowToggle.onClick = new System.Action(this.ArrowToggleClicked);
			this.arrowToggle.ChangeState(0);
			foreach (KeyValuePair<Tag, bool> keyValuePair in filterMap)
			{
				TreeFilterableSideScreenElement freeElement = this.parent.elementPool.GetFreeElement(this.elementGroup, true);
				freeElement.Parent = this.parent;
				freeElement.SetTag(keyValuePair.Key);
				freeElement.SetCheckBox(keyValuePair.Value);
				freeElement.OnSelectionChanged = new Action<Tag, bool>(this.OnElementSelectionChanged);
				freeElement.SetCheckBox(this.parent.IsTagAllowed(keyValuePair.Key));
				this.rowElements.Add(freeElement);
				this.subTags.Add(keyValuePair.Key);
			}
		}
		this.UpdateCheckBoxVisualState();
	}

	// Token: 0x0600AFEA RID: 45034 RVA: 0x0042C320 File Offset: 0x0042A520
	public void RefreshRowElements()
	{
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			treeFilterableSideScreenElement.SetCheckBox(this.parent.IsTagAllowed(treeFilterableSideScreenElement.GetElementTag()));
		}
	}

	// Token: 0x0600AFEB RID: 45035 RVA: 0x0042C384 File Offset: 0x0042A584
	public void FilterAgainstSearch(Tag thisCategoryTag, string search)
	{
		bool flag = false;
		bool flag2 = thisCategoryTag.ProperNameStripLink().ToUpper().Contains(search.ToUpper());
		search = search.ToUpper();
		foreach (TreeFilterableSideScreenElement treeFilterableSideScreenElement in this.rowElements)
		{
			bool flag3 = flag2 || treeFilterableSideScreenElement.GetElementTag().ProperNameStripLink().ToUpper().Contains(search.ToUpper());
			treeFilterableSideScreenElement.gameObject.SetActive(flag3);
			flag = (flag || flag3);
		}
		base.gameObject.SetActive(flag);
		if (search != "" && flag && this.arrowToggle.CurrentState == 0)
		{
			this.SetArrowToggleState(true);
		}
	}

	// Token: 0x04008A2C RID: 35372
	public bool visualDirty;

	// Token: 0x04008A2D RID: 35373
	public bool standardCommodity = true;

	// Token: 0x04008A2E RID: 35374
	[SerializeField]
	private LocText elementName;

	// Token: 0x04008A2F RID: 35375
	[SerializeField]
	private GameObject elementGroup;

	// Token: 0x04008A30 RID: 35376
	[SerializeField]
	private MultiToggle checkBoxToggle;

	// Token: 0x04008A31 RID: 35377
	[SerializeField]
	private MultiToggle arrowToggle;

	// Token: 0x04008A32 RID: 35378
	[SerializeField]
	private KImage bgImg;

	// Token: 0x04008A33 RID: 35379
	private List<Tag> subTags = new List<Tag>();

	// Token: 0x04008A34 RID: 35380
	private List<TreeFilterableSideScreenElement> rowElements = new List<TreeFilterableSideScreenElement>();

	// Token: 0x04008A35 RID: 35381
	private TreeFilterableSideScreen parent;

	// Token: 0x02002053 RID: 8275
	public enum State
	{
		// Token: 0x04008A38 RID: 35384
		Off,
		// Token: 0x04008A39 RID: 35385
		Mixed,
		// Token: 0x04008A3A RID: 35386
		On
	}
}
