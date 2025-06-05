using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CBE RID: 7358
public class CodexElementCategoryList : CodexCollapsibleHeader
{
	// Token: 0x17000A1D RID: 2589
	// (get) Token: 0x0600995E RID: 39262 RVA: 0x0010824E File Offset: 0x0010644E
	// (set) Token: 0x0600995F RID: 39263 RVA: 0x00108256 File Offset: 0x00106456
	public Tag categoryTag { get; set; }

	// Token: 0x06009960 RID: 39264 RVA: 0x0010825F File Offset: 0x0010645F
	public CodexElementCategoryList() : base(UI.CODEX.CATEGORYNAMES.ELEMENTS, null)
	{
	}

	// Token: 0x06009961 RID: 39265 RVA: 0x003C2D54 File Offset: 0x003C0F54
	public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
	{
		HierarchyReferences component = contentGameObject.GetComponent<HierarchyReferences>();
		base.ContentsGameObject = component.GetReference<RectTransform>("ContentContainer").gameObject;
		base.Configure(contentGameObject, displayPane, textStyles);
		Component reference = component.GetReference<RectTransform>("HeaderLabel");
		RectTransform reference2 = component.GetReference<RectTransform>("PrefabLabelWithIcon");
		this.ClearPanel(reference2.transform.parent, reference2);
		reference.GetComponent<LocText>().SetText(UI.CODEX.CATEGORYNAMES.ELEMENTS);
		foreach (Element element in ElementLoader.elements)
		{
			if (element.HasTag(this.categoryTag) && !element.disabled)
			{
				GameObject gameObject = Util.KInstantiateUI(reference2.gameObject, reference2.parent.gameObject, true);
				Image componentInChildren = gameObject.GetComponentInChildren<Image>();
				global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(element, "ui", false);
				componentInChildren.sprite = uisprite.first;
				componentInChildren.color = uisprite.second;
				gameObject.GetComponentInChildren<LocText>().SetText(element.tag.ProperName());
				this.rows.Add(gameObject);
			}
		}
	}

	// Token: 0x06009962 RID: 39266 RVA: 0x003C2E8C File Offset: 0x003C108C
	private void ClearPanel(Transform containerToClear, Transform skipDestroyingPrefab)
	{
		skipDestroyingPrefab.SetAsFirstSibling();
		for (int i = containerToClear.childCount - 1; i >= 1; i--)
		{
			UnityEngine.Object.Destroy(containerToClear.GetChild(i).gameObject);
		}
		for (int j = this.rows.Count - 1; j >= 0; j--)
		{
			UnityEngine.Object.Destroy(this.rows[j].gameObject);
		}
		this.rows.Clear();
	}

	// Token: 0x0400774A RID: 30538
	private List<GameObject> rows = new List<GameObject>();
}
