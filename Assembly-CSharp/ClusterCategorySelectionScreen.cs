using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C69 RID: 7273
public class ClusterCategorySelectionScreen : NewGameFlowScreen
{
	// Token: 0x06009735 RID: 38709 RVA: 0x003B25BC File Offset: 0x003B07BC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += base.NavigateBackward;
		int num = 0;
		using (Dictionary<string, ClusterLayout>.ValueCollection.Enumerator enumerator = SettingsCache.clusterLayouts.clusterCache.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.clusterCategory == ClusterLayout.ClusterCategory.Special)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			this.eventStyle.button.gameObject.SetActive(true);
			this.eventStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.EVENT_TITLE);
			MultiToggle button = this.eventStyle.button;
			button.onClick = (System.Action)Delegate.Combine(button.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.Special);
			}));
		}
		if (DlcManager.IsExpansion1Active())
		{
			this.classicStyle.button.gameObject.SetActive(true);
			this.classicStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.CLASSIC_TITLE);
			MultiToggle button2 = this.classicStyle.button;
			button2.onClick = (System.Action)Delegate.Combine(button2.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutVanillaStyle);
			}));
			this.spacedOutStyle.button.gameObject.SetActive(true);
			this.spacedOutStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.SPACEDOUT_TITLE);
			MultiToggle button3 = this.spacedOutStyle.button;
			button3.onClick = (System.Action)Delegate.Combine(button3.onClick, new System.Action(delegate()
			{
				this.OnClickOption(ClusterLayout.ClusterCategory.SpacedOutStyle);
			}));
			this.panel.sizeDelta = ((num > 0) ? new Vector2(622f, this.panel.sizeDelta.y) : new Vector2(480f, this.panel.sizeDelta.y));
			return;
		}
		this.vanillaStyle.button.gameObject.SetActive(true);
		this.vanillaStyle.Init(this.descriptionArea, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_DESC, UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.VANILLA_TITLE);
		MultiToggle button4 = this.vanillaStyle.button;
		button4.onClick = (System.Action)Delegate.Combine(button4.onClick, new System.Action(delegate()
		{
			this.OnClickOption(ClusterLayout.ClusterCategory.Vanilla);
		}));
		this.panel.sizeDelta = new Vector2(480f, this.panel.sizeDelta.y);
		this.eventStyle.kanim.Play("lab_asteroid_standard", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06009736 RID: 38710 RVA: 0x00106F34 File Offset: 0x00105134
	private void OnClickOption(ClusterLayout.ClusterCategory clusterCategory)
	{
		this.Deactivate();
		DestinationSelectPanel.ChosenClusterCategorySetting = (int)clusterCategory;
		base.NavigateForward();
	}

	// Token: 0x040075B1 RID: 30129
	public ClusterCategorySelectionScreen.ButtonConfig vanillaStyle;

	// Token: 0x040075B2 RID: 30130
	public ClusterCategorySelectionScreen.ButtonConfig classicStyle;

	// Token: 0x040075B3 RID: 30131
	public ClusterCategorySelectionScreen.ButtonConfig spacedOutStyle;

	// Token: 0x040075B4 RID: 30132
	public ClusterCategorySelectionScreen.ButtonConfig eventStyle;

	// Token: 0x040075B5 RID: 30133
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x040075B6 RID: 30134
	[SerializeField]
	private KButton closeButton;

	// Token: 0x040075B7 RID: 30135
	[SerializeField]
	private RectTransform panel;

	// Token: 0x02001C6A RID: 7274
	[Serializable]
	public class ButtonConfig
	{
		// Token: 0x0600973C RID: 38716 RVA: 0x003B2878 File Offset: 0x003B0A78
		public void Init(LocText descriptionArea, string hoverDescriptionText, string headerText)
		{
			this.descriptionArea = descriptionArea;
			this.hoverDescriptionText = hoverDescriptionText;
			this.headerLabel.SetText(headerText);
			MultiToggle multiToggle = this.button;
			multiToggle.onEnter = (System.Action)Delegate.Combine(multiToggle.onEnter, new System.Action(this.OnHoverEnter));
			MultiToggle multiToggle2 = this.button;
			multiToggle2.onExit = (System.Action)Delegate.Combine(multiToggle2.onExit, new System.Action(this.OnHoverExit));
			HierarchyReferences component = this.button.GetComponent<HierarchyReferences>();
			this.headerImage = component.GetReference<RectTransform>("HeaderBackground").GetComponent<Image>();
			this.selectionFrame = component.GetReference<RectTransform>("SelectionFrame").GetComponent<Image>();
		}

		// Token: 0x0600973D RID: 38717 RVA: 0x003B2928 File Offset: 0x003B0B28
		private void OnHoverEnter()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(1f);
			this.headerImage.color = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
			this.descriptionArea.text = this.hoverDescriptionText;
		}

		// Token: 0x0600973E RID: 38718 RVA: 0x003B298C File Offset: 0x003B0B8C
		private void OnHoverExit()
		{
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Mouseover", false));
			this.selectionFrame.SetAlpha(0f);
			this.headerImage.color = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
			this.descriptionArea.text = UI.FRONTEND.CLUSTERCATEGORYSELECTSCREEN.BLANK_DESC;
		}

		// Token: 0x040075B8 RID: 30136
		public MultiToggle button;

		// Token: 0x040075B9 RID: 30137
		public Image headerImage;

		// Token: 0x040075BA RID: 30138
		public LocText headerLabel;

		// Token: 0x040075BB RID: 30139
		public Image selectionFrame;

		// Token: 0x040075BC RID: 30140
		public KAnimControllerBase kanim;

		// Token: 0x040075BD RID: 30141
		private string hoverDescriptionText;

		// Token: 0x040075BE RID: 30142
		private LocText descriptionArea;
	}
}
