using System;
using UnityEngine;

// Token: 0x02002051 RID: 8273
[AddComponentMenu("KMonoBehaviour/scripts/TreeFilterableSideScreenElement")]
public class TreeFilterableSideScreenElement : KMonoBehaviour
{
	// Token: 0x0600AFCB RID: 45003 RVA: 0x00116E5B File Offset: 0x0011505B
	public Tag GetElementTag()
	{
		return this.elementTag;
	}

	// Token: 0x17000B45 RID: 2885
	// (get) Token: 0x0600AFCC RID: 45004 RVA: 0x00116E63 File Offset: 0x00115063
	public bool IsSelected
	{
		get
		{
			return this.checkBox.CurrentState == 1;
		}
	}

	// Token: 0x0600AFCD RID: 45005 RVA: 0x00116E73 File Offset: 0x00115073
	public MultiToggle GetCheckboxToggle()
	{
		return this.checkBox;
	}

	// Token: 0x17000B46 RID: 2886
	// (get) Token: 0x0600AFCE RID: 45006 RVA: 0x00116E7B File Offset: 0x0011507B
	// (set) Token: 0x0600AFCF RID: 45007 RVA: 0x00116E83 File Offset: 0x00115083
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

	// Token: 0x0600AFD0 RID: 45008 RVA: 0x00116E8C File Offset: 0x0011508C
	private void Initialize()
	{
		if (this.initialized)
		{
			return;
		}
		this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
		this.checkBox.onClick = new System.Action(this.CheckBoxClicked);
		this.initialized = true;
	}

	// Token: 0x0600AFD1 RID: 45009 RVA: 0x00116ECB File Offset: 0x001150CB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Initialize();
	}

	// Token: 0x0600AFD2 RID: 45010 RVA: 0x0042BF58 File Offset: 0x0042A158
	public Sprite GetStorageObjectSprite(Tag t)
	{
		Sprite result = null;
		GameObject prefab = Assets.GetPrefab(t);
		if (prefab != null)
		{
			KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
			if (component != null)
			{
				result = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, "");
			}
		}
		return result;
	}

	// Token: 0x0600AFD3 RID: 45011 RVA: 0x0042BFA4 File Offset: 0x0042A1A4
	public void SetSprite(Tag t)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(t, "ui", false);
		this.elementImg.sprite = uisprite.first;
		this.elementImg.color = uisprite.second;
		this.elementImg.gameObject.SetActive(true);
	}

	// Token: 0x0600AFD4 RID: 45012 RVA: 0x0042BFF8 File Offset: 0x0042A1F8
	public void SetTag(Tag newTag)
	{
		this.Initialize();
		this.elementTag = newTag;
		this.SetSprite(this.elementTag);
		string text = this.elementTag.ProperName();
		if (this.parent.IsStorage)
		{
			float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
			text = text + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		}
		this.elementName.text = text;
	}

	// Token: 0x0600AFD5 RID: 45013 RVA: 0x00116ED9 File Offset: 0x001150D9
	private void CheckBoxClicked()
	{
		this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
	}

	// Token: 0x0600AFD6 RID: 45014 RVA: 0x00116EF5 File Offset: 0x001150F5
	public void SetCheckBox(bool checkBoxState)
	{
		this.checkBox.ChangeState(checkBoxState ? 1 : 0);
		this.checkBoxImg.enabled = checkBoxState;
		if (this.OnSelectionChanged != null)
		{
			this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
		}
	}

	// Token: 0x04008A24 RID: 35364
	[SerializeField]
	private LocText elementName;

	// Token: 0x04008A25 RID: 35365
	[SerializeField]
	private MultiToggle checkBox;

	// Token: 0x04008A26 RID: 35366
	[SerializeField]
	private KImage elementImg;

	// Token: 0x04008A27 RID: 35367
	private KImage checkBoxImg;

	// Token: 0x04008A28 RID: 35368
	private Tag elementTag;

	// Token: 0x04008A29 RID: 35369
	public Action<Tag, bool> OnSelectionChanged;

	// Token: 0x04008A2A RID: 35370
	private TreeFilterableSideScreen parent;

	// Token: 0x04008A2B RID: 35371
	private bool initialized;
}
