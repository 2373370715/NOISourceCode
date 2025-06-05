using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Token: 0x02001D1A RID: 7450
public class EventInfoScreen : KModalScreen
{
	// Token: 0x06009BA0 RID: 39840 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06009BA1 RID: 39841 RVA: 0x003CD364 File Offset: 0x003CB564
	public void SetEventData(EventInfoData data)
	{
		data.FinalizeText();
		this.eventHeader.text = data.title;
		this.eventDescriptionLabel.text = data.description;
		this.eventLocationLabel.text = data.location;
		this.eventTimeLabel.text = data.whenDescription;
		if (data.location.IsNullOrWhiteSpace() && data.location.IsNullOrWhiteSpace())
		{
			this.timeGroup.gameObject.SetActive(false);
		}
		if (data.options.Count == 0)
		{
			data.AddDefaultOption(null);
		}
		this.artSection.gameObject.SetActive(data.animFileName != HashedString.Invalid);
		this.SetEventDataOptions(data);
		this.SetEventDataVisuals(data);
	}

	// Token: 0x06009BA2 RID: 39842 RVA: 0x003CD42C File Offset: 0x003CB62C
	private void SetEventDataOptions(EventInfoData data)
	{
		using (List<EventInfoData.Option>.Enumerator enumerator = data.options.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				EventInfoData.Option option = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.optionPrefab, this.buttonsGroup, false);
				gameObject.name = "Option: " + option.mainText;
				KButton component = gameObject.GetComponent<KButton>();
				component.isInteractable = option.allowed;
				component.onClick += delegate()
				{
					if (option.callback != null)
					{
						option.callback();
					}
					this.Deactivate();
				};
				if (!option.tooltip.IsNullOrWhiteSpace())
				{
					gameObject.GetComponent<ToolTip>().SetSimpleTooltip(option.tooltip);
				}
				else
				{
					gameObject.GetComponent<ToolTip>().enabled = false;
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					this.CreateOptionIcon(gameObject, optionIcon);
				}
				global::Util.KInstantiateUI(this.optionTextPrefab, gameObject, false).GetComponent<LocText>().text = ((option.description == null) ? ("<b>" + option.mainText + "</b>") : string.Concat(new string[]
				{
					"<b>",
					option.mainText,
					"</b>\n<i>(",
					option.description,
					")</i>"
				}));
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					this.CreateOptionIcon(gameObject, optionIcon2);
				}
				gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06009BA3 RID: 39843 RVA: 0x00109BE9 File Offset: 0x00107DE9
	public override void Deactivate()
	{
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().EventPopupSnapshot, STOP_MODE.ALLOWFADEOUT);
		base.Deactivate();
	}

	// Token: 0x06009BA4 RID: 39844 RVA: 0x003CD65C File Offset: 0x003CB85C
	private void CreateOptionIcon(GameObject option, EventInfoData.OptionIcon optionIcon)
	{
		GameObject gameObject = global::Util.KInstantiateUI(this.optionIconPrefab, option, false);
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(optionIcon.tooltip);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Mask");
		Image reference2 = component.GetReference<Image>("Border");
		Image reference3 = component.GetReference<Image>("Icon");
		if (optionIcon.sprite != null)
		{
			reference3.transform.localScale *= optionIcon.scale;
		}
		Color32 c = Color.white;
		switch (optionIcon.containerType)
		{
		case EventInfoData.OptionIcon.ContainerType.Neutral:
			reference.sprite = Assets.GetSprite("container_fill_neutral");
			reference2.sprite = Assets.GetSprite("container_border_neutral");
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("knob");
			}
			c = GlobalAssets.Instance.colorSet.eventNeutral;
			break;
		case EventInfoData.OptionIcon.ContainerType.Positive:
			reference.sprite = Assets.GetSprite("container_fill_positive");
			reference2.sprite = Assets.GetSprite("container_border_positive");
			reference3.rectTransform.localPosition += Vector3.down * 1f;
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("icon_positive");
			}
			c = GlobalAssets.Instance.colorSet.eventPositive;
			break;
		case EventInfoData.OptionIcon.ContainerType.Negative:
			reference.sprite = Assets.GetSprite("container_fill_negative");
			reference2.sprite = Assets.GetSprite("container_border_negative");
			reference3.rectTransform.localPosition += Vector3.up * 1f;
			c = GlobalAssets.Instance.colorSet.eventNegative;
			if (optionIcon.sprite == null)
			{
				optionIcon.sprite = Assets.GetSprite("cancel");
			}
			break;
		case EventInfoData.OptionIcon.ContainerType.Information:
			reference.sprite = Assets.GetSprite("requirements");
			reference2.enabled = false;
			break;
		}
		reference.color = c;
		reference3.sprite = optionIcon.sprite;
		if (optionIcon.sprite == null)
		{
			reference3.gameObject.SetActive(false);
		}
	}

	// Token: 0x06009BA5 RID: 39845 RVA: 0x003CD8C4 File Offset: 0x003CBAC4
	private void SetEventDataVisuals(EventInfoData data)
	{
		this.createdAnimations.ForEach(delegate(KBatchedAnimController x)
		{
			UnityEngine.Object.Destroy(x);
		});
		this.createdAnimations.Clear();
		KAnimFile anim = Assets.GetAnim(data.animFileName);
		if (anim == null)
		{
			global::Debug.LogWarning("Event " + data.title + " has no anim data");
			return;
		}
		KBatchedAnimController component = this.CreateAnimLayer(this.midgroundGroup, anim, data.mainAnim, null, null, null).transform.GetComponent<KBatchedAnimController>();
		if (data.minions != null)
		{
			for (int i = 0; i < data.minions.Length; i++)
			{
				if (data.minions[i] == null)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						string.Format("EventInfoScreen unable to display minion {0}", i)
					});
				}
				string s = string.Format("dupe{0:D2}", i + 1);
				if (component.HasAnimation(s))
				{
					this.CreateAnimLayer(this.midgroundGroup, anim, s, data.minions[i], null, null);
				}
			}
		}
		if (data.artifact != null)
		{
			string s2 = "artifact";
			if (component.HasAnimation(s2))
			{
				this.CreateAnimLayer(this.midgroundGroup, anim, s2, null, data.artifact, null);
			}
		}
	}

	// Token: 0x06009BA6 RID: 39846 RVA: 0x003CDA24 File Offset: 0x003CBC24
	private GameObject CreateAnimLayer(Transform parent, KAnimFile animFile, HashedString animName, GameObject minion = null, GameObject artifact = null, string targetSymbol = null)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.animPrefab, parent);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		this.createdAnimations.Add(component);
		if (minion != null)
		{
			component.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("body_comp_default_kanim"),
				Assets.GetAnim("head_swap_kanim"),
				Assets.GetAnim("body_swap_kanim"),
				animFile
			};
		}
		else
		{
			component.AnimFiles = new KAnimFile[]
			{
				animFile
			};
		}
		gameObject.SetActive(true);
		if (minion != null)
		{
			if (this.loadMinionFromPersonalities)
			{
				component.GetComponent<UIDupeSymbolOverride>().Apply(minion.GetComponent<MinionIdentity>());
			}
			else
			{
				SymbolOverrideController component2 = component.GetComponent<SymbolOverrideController>();
				foreach (SymbolOverrideController.SymbolEntry symbolEntry in minion.GetComponent<SymbolOverrideController>().GetSymbolOverrides)
				{
					component2.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, symbolEntry.priority);
				}
			}
			BaseMinionConfig.CopyVisibleSymbols(gameObject, minion);
		}
		if (artifact != null)
		{
			SymbolOverrideController component3 = component.GetComponent<SymbolOverrideController>();
			KBatchedAnimController component4 = artifact.GetComponent<KBatchedAnimController>();
			string text = component4.initialAnim;
			text = text.Replace("idle_", "artifact_");
			text = text.Replace("_loop", "");
			KAnim.Build.Symbol symbol = component4.AnimFiles[0].GetData().build.GetSymbol(text);
			if (symbol != null)
			{
				component3.AddSymbolOverride("snapTo_artifact", symbol, 0);
			}
		}
		if (targetSymbol != null)
		{
			gameObject.AddOrGet<KBatchedAnimTracker>().symbol = targetSymbol;
		}
		component.Play(animName, KAnim.PlayMode.Loop, 1f, 0f);
		component.animScale = this.baseCharacterScale;
		return gameObject;
	}

	// Token: 0x06009BA7 RID: 39847 RVA: 0x003CDBE8 File Offset: 0x003CBDE8
	public static EventInfoScreen ShowPopup(EventInfoData eventInfoData)
	{
		EventInfoScreen eventInfoScreen = (EventInfoScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.eventInfoScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject);
		eventInfoScreen.SetEventData(eventInfoData);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().EventPopupSnapshot);
		KFMOD.PlayUISound(GlobalAssets.GetSound("StoryTrait_Activation_Popup_short", false));
		if (eventInfoData.showCallback != null)
		{
			eventInfoData.showCallback();
		}
		if (eventInfoData.clickFocus != null)
		{
			WorldContainer myWorld = eventInfoData.clickFocus.gameObject.GetMyWorld();
			if (myWorld != null && myWorld.IsDiscovered)
			{
				GameUtil.FocusCameraOnWorld(myWorld.id, eventInfoData.clickFocus.position, 10f, null, true);
			}
		}
		return eventInfoScreen;
	}

	// Token: 0x06009BA8 RID: 39848 RVA: 0x003CDCB0 File Offset: 0x003CBEB0
	public static Notification CreateNotification(EventInfoData eventInfoData, Notification.ClickCallback clickCallback = null)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"eventPopup is null in CreateStandardEventNotification"
			});
			return null;
		}
		eventInfoData.FinalizeText();
		Notification notification = new Notification(eventInfoData.title, NotificationType.Event, null, null, false, 0f, null, null, eventInfoData.clickFocus, true, false, false);
		if (clickCallback == null)
		{
			notification.customClickCallback = delegate(object data)
			{
				EventInfoScreen.ShowPopup(eventInfoData);
			};
		}
		else
		{
			notification.customClickCallback = clickCallback;
		}
		return notification;
	}

	// Token: 0x040079B2 RID: 31154
	[SerializeField]
	private float baseCharacterScale = 0.0057f;

	// Token: 0x040079B3 RID: 31155
	[FormerlySerializedAs("midgroundPrefab")]
	[FormerlySerializedAs("mid")]
	[Header("Prefabs")]
	[SerializeField]
	private GameObject animPrefab;

	// Token: 0x040079B4 RID: 31156
	[SerializeField]
	private GameObject optionPrefab;

	// Token: 0x040079B5 RID: 31157
	[SerializeField]
	private GameObject optionIconPrefab;

	// Token: 0x040079B6 RID: 31158
	[SerializeField]
	private GameObject optionTextPrefab;

	// Token: 0x040079B7 RID: 31159
	[Header("Groups")]
	[SerializeField]
	private Transform artSection;

	// Token: 0x040079B8 RID: 31160
	[SerializeField]
	private Transform midgroundGroup;

	// Token: 0x040079B9 RID: 31161
	[SerializeField]
	private GameObject timeGroup;

	// Token: 0x040079BA RID: 31162
	[SerializeField]
	private GameObject buttonsGroup;

	// Token: 0x040079BB RID: 31163
	[SerializeField]
	private GameObject chainGroup;

	// Token: 0x040079BC RID: 31164
	[Header("Text")]
	[SerializeField]
	private LocText eventHeader;

	// Token: 0x040079BD RID: 31165
	[SerializeField]
	private LocText eventTimeLabel;

	// Token: 0x040079BE RID: 31166
	[SerializeField]
	private LocText eventLocationLabel;

	// Token: 0x040079BF RID: 31167
	[SerializeField]
	private LocText eventDescriptionLabel;

	// Token: 0x040079C0 RID: 31168
	[SerializeField]
	private bool loadMinionFromPersonalities = true;

	// Token: 0x040079C1 RID: 31169
	[SerializeField]
	private LocText chainCount;

	// Token: 0x040079C2 RID: 31170
	[Header("Button Colour Styles")]
	[SerializeField]
	private ColorStyleSetting neutralButtonSetting;

	// Token: 0x040079C3 RID: 31171
	[SerializeField]
	private ColorStyleSetting badButtonSetting;

	// Token: 0x040079C4 RID: 31172
	[SerializeField]
	private ColorStyleSetting goodButtonSetting;

	// Token: 0x040079C5 RID: 31173
	private List<KBatchedAnimController> createdAnimations = new List<KBatchedAnimController>();
}
