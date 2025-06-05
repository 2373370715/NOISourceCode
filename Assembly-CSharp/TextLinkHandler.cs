using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02002095 RID: 8341
public class TextLinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x0600B1E2 RID: 45538 RVA: 0x0043A6F0 File Offset: 0x004388F0
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (!this.text.AllowLinks)
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null);
		if (num != -1)
		{
			string text = CodexCache.FormatLinkID(this.text.textInfo.linkInfo[num].GetLinkID());
			if (this.overrideLinkAction == null || this.overrideLinkAction(text))
			{
				if (!CodexCache.entries.ContainsKey(text))
				{
					SubEntry subEntry = CodexCache.FindSubEntry(text);
					if (subEntry == null || subEntry.disabled)
					{
						text = "PAGENOTFOUND";
					}
				}
				else if (CodexCache.entries[text].disabled)
				{
					text = "PAGENOTFOUND";
				}
				if (!ManagementMenu.Instance.codexScreen.gameObject.activeInHierarchy)
				{
					ManagementMenu.Instance.ToggleCodex();
				}
				ManagementMenu.Instance.codexScreen.ChangeArticle(text, true, default(Vector3), CodexScreen.HistoryDirection.NewArticle);
			}
		}
	}

	// Token: 0x0600B1E3 RID: 45539 RVA: 0x001182E0 File Offset: 0x001164E0
	private void Update()
	{
		this.CheckMouseOver();
		if (TextLinkHandler.hoveredText == this && this.text.AllowLinks)
		{
			PlayerController.Instance.ActiveTool.SetLinkCursor(this.hoverLink);
		}
	}

	// Token: 0x0600B1E4 RID: 45540 RVA: 0x00118317 File Offset: 0x00116517
	private void OnEnable()
	{
		this.CheckMouseOver();
	}

	// Token: 0x0600B1E5 RID: 45541 RVA: 0x0011831F File Offset: 0x0011651F
	private void OnDisable()
	{
		this.ClearState();
	}

	// Token: 0x0600B1E6 RID: 45542 RVA: 0x00118327 File Offset: 0x00116527
	private void Awake()
	{
		this.text = base.GetComponent<LocText>();
		if (this.text.AllowLinks && !this.text.raycastTarget)
		{
			this.text.raycastTarget = true;
		}
	}

	// Token: 0x0600B1E7 RID: 45543 RVA: 0x0011835B File Offset: 0x0011655B
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.SetMouseOver();
	}

	// Token: 0x0600B1E8 RID: 45544 RVA: 0x0011831F File Offset: 0x0011651F
	public void OnPointerExit(PointerEventData eventData)
	{
		this.ClearState();
	}

	// Token: 0x0600B1E9 RID: 45545 RVA: 0x0043A7E0 File Offset: 0x004389E0
	private void ClearState()
	{
		if (this == null || this.Equals(null))
		{
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			if (this.hoverLink && PlayerController.Instance != null && PlayerController.Instance.ActiveTool != null)
			{
				PlayerController.Instance.ActiveTool.SetLinkCursor(false);
			}
			TextLinkHandler.hoveredText = null;
			this.hoverLink = false;
		}
	}

	// Token: 0x0600B1EA RID: 45546 RVA: 0x0043A854 File Offset: 0x00438A54
	public void CheckMouseOver()
	{
		if (this.text == null)
		{
			return;
		}
		if (TMP_TextUtilities.FindIntersectingLink(this.text, KInputManager.GetMousePos(), null) != -1)
		{
			this.SetMouseOver();
			this.hoverLink = true;
			return;
		}
		if (TextLinkHandler.hoveredText == this)
		{
			this.hoverLink = false;
		}
	}

	// Token: 0x0600B1EB RID: 45547 RVA: 0x00118363 File Offset: 0x00116563
	private void SetMouseOver()
	{
		if (TextLinkHandler.hoveredText != null && TextLinkHandler.hoveredText != this)
		{
			TextLinkHandler.hoveredText.hoverLink = false;
		}
		TextLinkHandler.hoveredText = this;
	}

	// Token: 0x04008C42 RID: 35906
	private static TextLinkHandler hoveredText;

	// Token: 0x04008C43 RID: 35907
	[MyCmpGet]
	private LocText text;

	// Token: 0x04008C44 RID: 35908
	private bool hoverLink;

	// Token: 0x04008C45 RID: 35909
	public Func<string, bool> overrideLinkAction;
}
