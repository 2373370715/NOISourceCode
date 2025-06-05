using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D58 RID: 7512
public class InfoDialogScreen : KModalScreen
{
	// Token: 0x06009CCC RID: 40140 RVA: 0x0010A778 File Offset: 0x00108978
	public InfoScreenPlainText GetSubHeaderPrefab()
	{
		return this.subHeaderTemplate;
	}

	// Token: 0x06009CCD RID: 40141 RVA: 0x0010A780 File Offset: 0x00108980
	public InfoScreenPlainText GetPlainTextPrefab()
	{
		return this.plainTextTemplate;
	}

	// Token: 0x06009CCE RID: 40142 RVA: 0x0010A788 File Offset: 0x00108988
	public InfoScreenLineItem GetLineItemPrefab()
	{
		return this.lineItemTemplate;
	}

	// Token: 0x06009CCF RID: 40143 RVA: 0x0010A790 File Offset: 0x00108990
	public GameObject GetPrimaryButtonPrefab()
	{
		return this.leftButtonPrefab;
	}

	// Token: 0x06009CD0 RID: 40144 RVA: 0x0010A798 File Offset: 0x00108998
	public GameObject GetSecondaryButtonPrefab()
	{
		return this.rightButtonPrefab;
	}

	// Token: 0x06009CD1 RID: 40145 RVA: 0x001086E3 File Offset: 0x001068E3
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06009CD2 RID: 40146 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009CD3 RID: 40147 RVA: 0x003D3664 File Offset: 0x003D1864
	public override void OnKeyDown(KButtonEvent e)
	{
		if (!this.escapeCloses)
		{
			e.TryConsume(global::Action.Escape);
			return;
		}
		if (e.TryConsume(global::Action.Escape))
		{
			this.Deactivate();
			return;
		}
		if (PlayerController.Instance != null && PlayerController.Instance.ConsumeIfNotDragging(e, global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009CD4 RID: 40148 RVA: 0x0010A7A0 File Offset: 0x001089A0
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (!show && this.onDeactivateFn != null)
		{
			this.onDeactivateFn();
		}
	}

	// Token: 0x06009CD5 RID: 40149 RVA: 0x0010A7BF File Offset: 0x001089BF
	public InfoDialogScreen AddDefaultOK(bool escapeCloses = false)
	{
		this.AddOption(UI.CONFIRMDIALOG.OK, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, true);
		this.escapeCloses = escapeCloses;
		return this;
	}

	// Token: 0x06009CD6 RID: 40150 RVA: 0x0010A7FA File Offset: 0x001089FA
	public InfoDialogScreen AddDefaultCancel()
	{
		this.AddOption(UI.CONFIRMDIALOG.CANCEL, delegate(InfoDialogScreen d)
		{
			d.Deactivate();
		}, false);
		this.escapeCloses = true;
		return this;
	}

	// Token: 0x06009CD7 RID: 40151 RVA: 0x003D36BC File Offset: 0x003D18BC
	public InfoDialogScreen AddOption(string text, Action<InfoDialogScreen> action, bool rightSide = false)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		gameObject.gameObject.GetComponentInChildren<LocText>().text = text;
		gameObject.gameObject.GetComponent<KButton>().onClick += delegate()
		{
			action(this);
		};
		return this;
	}

	// Token: 0x06009CD8 RID: 40152 RVA: 0x003D3734 File Offset: 0x003D1934
	public InfoDialogScreen AddOption(bool rightSide, out KButton button, out LocText buttonText)
	{
		GameObject gameObject = Util.KInstantiateUI(rightSide ? this.rightButtonPrefab : this.leftButtonPrefab, rightSide ? this.rightButtonPanel : this.leftButtonPanel, true);
		button = gameObject.GetComponent<KButton>();
		buttonText = gameObject.GetComponentInChildren<LocText>();
		return this;
	}

	// Token: 0x06009CD9 RID: 40153 RVA: 0x0010A835 File Offset: 0x00108A35
	public InfoDialogScreen SetHeader(string header)
	{
		this.header.text = header;
		return this;
	}

	// Token: 0x06009CDA RID: 40154 RVA: 0x0010A844 File Offset: 0x00108A44
	public InfoDialogScreen AddSprite(Sprite sprite)
	{
		Util.KInstantiateUI<InfoScreenSpriteItem>(this.spriteItemTemplate.gameObject, this.contentContainer, false).SetSprite(sprite);
		return this;
	}

	// Token: 0x06009CDB RID: 40155 RVA: 0x0010A864 File Offset: 0x00108A64
	public InfoDialogScreen AddPlainText(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.plainTextTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x06009CDC RID: 40156 RVA: 0x0010A884 File Offset: 0x00108A84
	public InfoDialogScreen AddLineItem(string text, string tooltip)
	{
		InfoScreenLineItem infoScreenLineItem = Util.KInstantiateUI<InfoScreenLineItem>(this.lineItemTemplate.gameObject, this.contentContainer, false);
		infoScreenLineItem.SetText(text);
		infoScreenLineItem.SetTooltip(tooltip);
		return this;
	}

	// Token: 0x06009CDD RID: 40157 RVA: 0x0010A8AB File Offset: 0x00108AAB
	public InfoDialogScreen AddSubHeader(string text)
	{
		Util.KInstantiateUI<InfoScreenPlainText>(this.subHeaderTemplate.gameObject, this.contentContainer, false).SetText(text);
		return this;
	}

	// Token: 0x06009CDE RID: 40158 RVA: 0x003D377C File Offset: 0x003D197C
	public InfoDialogScreen AddSpacer(float height)
	{
		GameObject gameObject = new GameObject("spacer");
		gameObject.SetActive(false);
		gameObject.transform.SetParent(this.contentContainer.transform, false);
		LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
		layoutElement.minHeight = height;
		layoutElement.preferredHeight = height;
		layoutElement.flexibleHeight = 0f;
		gameObject.SetActive(true);
		return this;
	}

	// Token: 0x06009CDF RID: 40159 RVA: 0x0010A8CB File Offset: 0x00108ACB
	public InfoDialogScreen AddUI<T>(T prefab, out T spawn) where T : MonoBehaviour
	{
		spawn = Util.KInstantiateUI<T>(prefab.gameObject, this.contentContainer, true);
		return this;
	}

	// Token: 0x06009CE0 RID: 40160 RVA: 0x003D37D8 File Offset: 0x003D19D8
	public InfoDialogScreen AddDescriptors(List<Descriptor> descriptors)
	{
		for (int i = 0; i < descriptors.Count; i++)
		{
			this.AddLineItem(descriptors[i].IndentedText(), descriptors[i].tooltipText);
		}
		return this;
	}

	// Token: 0x04007AD4 RID: 31444
	[SerializeField]
	private InfoScreenPlainText subHeaderTemplate;

	// Token: 0x04007AD5 RID: 31445
	[SerializeField]
	private InfoScreenPlainText plainTextTemplate;

	// Token: 0x04007AD6 RID: 31446
	[SerializeField]
	private InfoScreenLineItem lineItemTemplate;

	// Token: 0x04007AD7 RID: 31447
	[SerializeField]
	private InfoScreenSpriteItem spriteItemTemplate;

	// Token: 0x04007AD8 RID: 31448
	[Space(10f)]
	[SerializeField]
	private LocText header;

	// Token: 0x04007AD9 RID: 31449
	[SerializeField]
	private GameObject contentContainer;

	// Token: 0x04007ADA RID: 31450
	[SerializeField]
	private GameObject leftButtonPrefab;

	// Token: 0x04007ADB RID: 31451
	[SerializeField]
	private GameObject rightButtonPrefab;

	// Token: 0x04007ADC RID: 31452
	[SerializeField]
	private GameObject leftButtonPanel;

	// Token: 0x04007ADD RID: 31453
	[SerializeField]
	private GameObject rightButtonPanel;

	// Token: 0x04007ADE RID: 31454
	private bool escapeCloses;

	// Token: 0x04007ADF RID: 31455
	public System.Action onDeactivateFn;
}
