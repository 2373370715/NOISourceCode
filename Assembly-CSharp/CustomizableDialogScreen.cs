using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CE3 RID: 7395
public class CustomizableDialogScreen : KModalScreen
{
	// Token: 0x06009A3D RID: 39485 RVA: 0x00108CBE File Offset: 0x00106EBE
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<CustomizableDialogScreen.Button>();
	}

	// Token: 0x06009A3E RID: 39486 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009A3F RID: 39487 RVA: 0x003C674C File Offset: 0x003C494C
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new CustomizableDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x06009A40 RID: 39488 RVA: 0x003C6798 File Offset: 0x003C4998
	public void PopupConfirmDialog(string text, string title_text = null, Sprite image_sprite = null)
	{
		foreach (CustomizableDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (image_sprite != null)
		{
			this.image.sprite = image_sprite;
			this.image.gameObject.SetActive(true);
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x06009A41 RID: 39489 RVA: 0x00108CDD File Offset: 0x00106EDD
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04007859 RID: 30809
	public System.Action onDeactivateCB;

	// Token: 0x0400785A RID: 30810
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x0400785B RID: 30811
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x0400785C RID: 30812
	[SerializeField]
	private LocText titleText;

	// Token: 0x0400785D RID: 30813
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x0400785E RID: 30814
	[SerializeField]
	private Image image;

	// Token: 0x0400785F RID: 30815
	private List<CustomizableDialogScreen.Button> buttons;

	// Token: 0x02001CE4 RID: 7396
	private struct Button
	{
		// Token: 0x04007860 RID: 30816
		public System.Action action;

		// Token: 0x04007861 RID: 30817
		public GameObject gameObject;

		// Token: 0x04007862 RID: 30818
		public string label;
	}
}
