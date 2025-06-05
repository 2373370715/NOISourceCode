using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200207C RID: 8316
public class SpriteListDialogScreen : KModalScreen
{
	// Token: 0x0600B10A RID: 45322 RVA: 0x00117AC8 File Offset: 0x00115CC8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
		this.buttons = new List<SpriteListDialogScreen.Button>();
	}

	// Token: 0x0600B10B RID: 45323 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x0600B10C RID: 45324 RVA: 0x00117AE7 File Offset: 0x00115CE7
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600B10D RID: 45325 RVA: 0x00434C6C File Offset: 0x00432E6C
	public void AddOption(string text, System.Action action)
	{
		GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.buttonPanel, true);
		this.buttons.Add(new SpriteListDialogScreen.Button
		{
			label = text,
			action = action,
			gameObject = gameObject
		});
	}

	// Token: 0x0600B10E RID: 45326 RVA: 0x00434CB8 File Offset: 0x00432EB8
	public void AddListRow(Sprite sprite, string text, float width = -1f, float height = -1f)
	{
		GameObject gameObject = Util.KInstantiateUI(this.listPrefab, this.listPanel, true);
		gameObject.GetComponentInChildren<LocText>().text = text;
		Image componentInChildren = gameObject.GetComponentInChildren<Image>();
		componentInChildren.sprite = sprite;
		if (sprite == null)
		{
			Color color = componentInChildren.color;
			color.a = 0f;
			componentInChildren.color = color;
		}
		if (width >= 0f || height >= 0f)
		{
			componentInChildren.GetComponent<AspectRatioFitter>().enabled = false;
			LayoutElement component = componentInChildren.GetComponent<LayoutElement>();
			component.minWidth = width;
			component.preferredWidth = width;
			component.minHeight = height;
			component.preferredHeight = height;
			return;
		}
		AspectRatioFitter component2 = componentInChildren.GetComponent<AspectRatioFitter>();
		float aspectRatio = (sprite == null) ? 1f : (sprite.rect.width / sprite.rect.height);
		component2.aspectRatio = aspectRatio;
	}

	// Token: 0x0600B10F RID: 45327 RVA: 0x00434D90 File Offset: 0x00432F90
	public void PopupConfirmDialog(string text, string title_text = null)
	{
		foreach (SpriteListDialogScreen.Button button in this.buttons)
		{
			button.gameObject.GetComponentInChildren<LocText>().text = button.label;
			button.gameObject.GetComponent<KButton>().onClick += button.action;
		}
		if (title_text != null)
		{
			this.titleText.text = title_text;
		}
		this.popupMessage.text = text;
	}

	// Token: 0x0600B110 RID: 45328 RVA: 0x00117B00 File Offset: 0x00115D00
	protected override void OnDeactivate()
	{
		if (this.onDeactivateCB != null)
		{
			this.onDeactivateCB();
		}
		base.OnDeactivate();
	}

	// Token: 0x04008B6C RID: 35692
	public System.Action onDeactivateCB;

	// Token: 0x04008B6D RID: 35693
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04008B6E RID: 35694
	[SerializeField]
	private GameObject buttonPanel;

	// Token: 0x04008B6F RID: 35695
	[SerializeField]
	private LocText titleText;

	// Token: 0x04008B70 RID: 35696
	[SerializeField]
	private LocText popupMessage;

	// Token: 0x04008B71 RID: 35697
	[SerializeField]
	private GameObject listPanel;

	// Token: 0x04008B72 RID: 35698
	[SerializeField]
	private GameObject listPrefab;

	// Token: 0x04008B73 RID: 35699
	private List<SpriteListDialogScreen.Button> buttons;

	// Token: 0x0200207D RID: 8317
	private struct Button
	{
		// Token: 0x04008B74 RID: 35700
		public System.Action action;

		// Token: 0x04008B75 RID: 35701
		public GameObject gameObject;

		// Token: 0x04008B76 RID: 35702
		public string label;
	}
}
