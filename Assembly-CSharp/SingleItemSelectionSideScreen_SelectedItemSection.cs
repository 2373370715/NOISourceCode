using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200203C RID: 8252
public class SingleItemSelectionSideScreen_SelectedItemSection : KMonoBehaviour
{
	// Token: 0x17000B3E RID: 2878
	// (get) Token: 0x0600AF06 RID: 44806 RVA: 0x00116582 File Offset: 0x00114782
	// (set) Token: 0x0600AF05 RID: 44805 RVA: 0x00116579 File Offset: 0x00114779
	public Tag Item { get; private set; }

	// Token: 0x0600AF07 RID: 44807 RVA: 0x0011658A File Offset: 0x0011478A
	public void Clear()
	{
		this.SetItem(null);
	}

	// Token: 0x0600AF08 RID: 44808 RVA: 0x00428DDC File Offset: 0x00426FDC
	public void SetItem(Tag item)
	{
		this.Item = item;
		if (this.Item != GameTags.Void)
		{
			this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.TITLE);
			this.SetContentText(this.Item.ProperName());
			global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(this.Item, "ui", false);
			this.SetImage(uisprite.first, uisprite.second);
			return;
		}
		this.SetTitleText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_TITLE);
		this.SetContentText(UI.UISIDESCREENS.SINGLEITEMSELECTIONSIDESCREEN.CURRENT_ITEM_SELECTED_SECTION.NO_ITEM_MESSAGE);
		this.SetImage(null, Color.white);
	}

	// Token: 0x0600AF09 RID: 44809 RVA: 0x00116598 File Offset: 0x00114798
	private void SetTitleText(string text)
	{
		this.title.text = text;
	}

	// Token: 0x0600AF0A RID: 44810 RVA: 0x001165A6 File Offset: 0x001147A6
	private void SetContentText(string text)
	{
		this.contentText.text = text;
	}

	// Token: 0x0600AF0B RID: 44811 RVA: 0x001165B4 File Offset: 0x001147B4
	private void SetImage(Sprite sprite, Color color)
	{
		this.image.sprite = sprite;
		this.image.color = color;
		this.image.gameObject.SetActive(sprite != null);
	}

	// Token: 0x0400899A RID: 35226
	[Header("References")]
	[SerializeField]
	private LocText title;

	// Token: 0x0400899B RID: 35227
	[SerializeField]
	private LocText contentText;

	// Token: 0x0400899C RID: 35228
	[SerializeField]
	private KImage image;
}
