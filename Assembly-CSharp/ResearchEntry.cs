using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02001F24 RID: 7972
[AddComponentMenu("KMonoBehaviour/scripts/ResearchEntry")]
public class ResearchEntry : KMonoBehaviour
{
	// Token: 0x0600A7AC RID: 42924 RVA: 0x00405CB8 File Offset: 0x00403EB8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techLineMap = new Dictionary<Tech, UILineRenderer>();
		this.BG.color = this.defaultColor;
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			float num = this.targetTech.width / 2f + 18f;
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			if (tech.center.y > this.targetTech.center.y + 2f)
			{
				zero = new Vector2(0f, 20f);
				zero2 = new Vector2(0f, -20f);
			}
			else if (tech.center.y < this.targetTech.center.y - 2f)
			{
				zero = new Vector2(0f, -20f);
				zero2 = new Vector2(0f, 20f);
			}
			UILineRenderer component = Util.KInstantiateUI(this.linePrefab, this.lineContainer.gameObject, true).GetComponent<UILineRenderer>();
			float num2 = 32f;
			component.Points = new Vector2[]
			{
				new Vector2(0f, 0f) + zero,
				new Vector2(-num2, 0f) + zero,
				new Vector2(-num2, tech.center.y - this.targetTech.center.y) + zero2,
				new Vector2(-(this.targetTech.center.x - num - (tech.center.x + num)) + 2f, tech.center.y - this.targetTech.center.y) + zero2
			};
			component.LineThickness = (float)this.lineThickness_inactive;
			component.color = this.inactiveLineColor;
			this.techLineMap.Add(tech, component);
		}
		this.QueueStateChanged(false);
		if (this.targetTech != null)
		{
			using (List<TechInstance>.Enumerator enumerator2 = Research.Instance.GetResearchQueue().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.tech == this.targetTech)
					{
						this.QueueStateChanged(true);
					}
				}
			}
		}
	}

	// Token: 0x0600A7AD RID: 42925 RVA: 0x00405F78 File Offset: 0x00404178
	public void SetTech(Tech newTech)
	{
		if (newTech == null)
		{
			global::Debug.LogError("The research provided is null!");
			return;
		}
		if (this.targetTech == newTech)
		{
			return;
		}
		foreach (ResearchType researchType in Research.Instance.researchTypes.Types)
		{
			if (newTech.costsByResearchTypeID.ContainsKey(researchType.id) && newTech.costsByResearchTypeID[researchType.id] > 0f)
			{
				GameObject gameObject = Util.KInstantiateUI(this.progressBarPrefab, this.progressBarContainer.gameObject, true);
				Image image = gameObject.GetComponentsInChildren<Image>()[2];
				Image component = gameObject.transform.Find("Icon").GetComponent<Image>();
				image.color = researchType.color;
				component.sprite = researchType.sprite;
				this.progressBarsByResearchTypeID[researchType.id] = gameObject;
			}
		}
		if (this.researchScreen == null)
		{
			this.researchScreen = base.transform.parent.GetComponentInParent<ResearchScreen>();
		}
		if (newTech.IsComplete())
		{
			this.ResearchCompleted(false);
		}
		this.targetTech = newTech;
		this.researchName.text = this.targetTech.Name;
		string text = "";
		foreach (TechItem techItem in this.targetTech.unlockedItems)
		{
			if (Game.IsCorrectDlcActiveForCurrentSave(techItem))
			{
				HierarchyReferences component2 = this.GetFreeIcon().GetComponent<HierarchyReferences>();
				if (text != "")
				{
					text += ", ";
				}
				text += techItem.Name;
				component2.GetReference<KImage>("Icon").sprite = techItem.UISprite();
				component2.GetReference<KImage>("Background");
				KImage reference = component2.GetReference<KImage>("DLCOverlay");
				bool flag = techItem.requiredDlcIds != null;
				reference.gameObject.SetActive(flag);
				if (flag)
				{
					reference.color = DlcManager.GetDlcBannerColor(techItem.requiredDlcIds[techItem.requiredDlcIds.Length - 1]);
				}
				string text2 = string.Format("{0}\n{1}", techItem.Name, techItem.description);
				if (flag)
				{
					text2 += "\n";
					foreach (string dlcId in techItem.requiredDlcIds)
					{
						text2 += string.Format(RESEARCH.MESSAGING.DLC.DLC_CONTENT, DlcManager.GetDlcTitle(dlcId));
					}
				}
				component2.GetComponent<ToolTip>().toolTip = text2;
			}
		}
		text = string.Format(UI.RESEARCHSCREEN_UNLOCKSTOOLTIP, text);
		this.researchName.GetComponent<ToolTip>().toolTip = string.Format("{0}\n{1}\n\n{2}", this.targetTech.Name, this.targetTech.desc, text);
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.toggle.onPointerEnter += delegate()
		{
			this.researchScreen.TurnEverythingOff();
			this.OnHover(true, this.targetTech);
		};
		this.toggle.soundPlayer.AcceptClickCondition = (() => !this.targetTech.IsComplete());
		this.toggle.onPointerExit += delegate()
		{
			this.researchScreen.TurnEverythingOff();
		};
	}

	// Token: 0x0600A7AE RID: 42926 RVA: 0x00406308 File Offset: 0x00404508
	public void SetEverythingOff()
	{
		if (!this.isOn)
		{
			return;
		}
		this.borderHighlight.gameObject.SetActive(false);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_inactive;
			keyValuePair.Value.color = this.inactiveLineColor;
		}
		this.isOn = false;
	}

	// Token: 0x0600A7AF RID: 42927 RVA: 0x0040639C File Offset: 0x0040459C
	public void SetEverythingOn()
	{
		if (this.isOn)
		{
			return;
		}
		this.UpdateProgressBars();
		this.borderHighlight.gameObject.SetActive(true);
		foreach (KeyValuePair<Tech, UILineRenderer> keyValuePair in this.techLineMap)
		{
			keyValuePair.Value.LineThickness = (float)this.lineThickness_active;
			keyValuePair.Value.color = this.activeLineColor;
		}
		base.transform.SetAsLastSibling();
		this.isOn = true;
	}

	// Token: 0x0600A7B0 RID: 42928 RVA: 0x00406440 File Offset: 0x00404640
	public void OnHover(bool entered, Tech hoverSource)
	{
		this.SetEverythingOn();
		foreach (Tech tech in this.targetTech.requiredTech)
		{
			ResearchEntry entry = this.researchScreen.GetEntry(tech);
			if (entry != null)
			{
				entry.OnHover(entered, this.targetTech);
			}
		}
	}

	// Token: 0x0600A7B1 RID: 42929 RVA: 0x004064BC File Offset: 0x004046BC
	private void OnResearchClicked()
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch != null && activeResearch.tech != this.targetTech)
		{
			this.researchScreen.CancelResearch();
		}
		Research.Instance.SetActiveResearch(this.targetTech, true);
		if (DebugHandler.InstantBuildMode)
		{
			Research.Instance.CompleteQueue();
		}
		this.UpdateProgressBars();
	}

	// Token: 0x0600A7B2 RID: 42930 RVA: 0x00406518 File Offset: 0x00404718
	private void OnResearchCanceled()
	{
		if (this.targetTech.IsComplete())
		{
			return;
		}
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		this.researchScreen.CancelResearch();
		Research.Instance.CancelResearch(this.targetTech, true);
	}

	// Token: 0x0600A7B3 RID: 42931 RVA: 0x00406574 File Offset: 0x00404774
	public void QueueStateChanged(bool isSelected)
	{
		if (isSelected)
		{
			if (!this.targetTech.IsComplete())
			{
				this.toggle.isOn = true;
				this.BG.color = this.pendingColor;
				this.titleBG.color = this.pendingHeaderColor;
				this.toggle.ClearOnClick();
				this.toggle.onClick += this.OnResearchCanceled;
			}
			else
			{
				this.toggle.isOn = false;
			}
			foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
			{
				keyValuePair.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		if (this.targetTech.IsComplete())
		{
			this.toggle.isOn = false;
			this.BG.color = this.completedColor;
			this.titleBG.color = this.completedHeaderColor;
			this.defaultColor = this.completedColor;
			this.toggle.ClearOnClick();
			foreach (KeyValuePair<string, GameObject> keyValuePair2 in this.progressBarsByResearchTypeID)
			{
				keyValuePair2.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = Color.white;
			}
			Image[] componentsInChildren = this.iconPanel.GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].material = this.StandardUIMaterial;
			}
			return;
		}
		this.toggle.isOn = false;
		this.BG.color = this.defaultColor;
		this.titleBG.color = this.incompleteHeaderColor;
		this.toggle.ClearOnClick();
		this.toggle.onClick += this.OnResearchClicked;
		foreach (KeyValuePair<string, GameObject> keyValuePair3 in this.progressBarsByResearchTypeID)
		{
			keyValuePair3.Value.transform.GetChild(0).GetComponentsInChildren<Image>()[1].color = new Color(0.52156866f, 0.52156866f, 0.52156866f);
		}
	}

	// Token: 0x0600A7B4 RID: 42932 RVA: 0x00406818 File Offset: 0x00404A18
	public void UpdateFilterState(bool state)
	{
		this.filterLowlight.gameObject.SetActive(!state);
	}

	// Token: 0x0600A7B5 RID: 42933 RVA: 0x000AA038 File Offset: 0x000A8238
	public void SetPercentage(float percent)
	{
	}

	// Token: 0x0600A7B6 RID: 42934 RVA: 0x0040683C File Offset: 0x00404A3C
	public void UpdateProgressBars()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.progressBarsByResearchTypeID)
		{
			Transform child = keyValuePair.Value.transform.GetChild(0);
			float fillAmount;
			if (this.targetTech.IsComplete())
			{
				fillAmount = 1f;
				child.GetComponentInChildren<LocText>().text = this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
			}
			else
			{
				TechInstance orAdd = Research.Instance.GetOrAdd(this.targetTech);
				if (orAdd == null)
				{
					continue;
				}
				child.GetComponentInChildren<LocText>().text = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key].ToString() + "/" + this.targetTech.costsByResearchTypeID[keyValuePair.Key].ToString();
				fillAmount = orAdd.progressInventory.PointsByTypeID[keyValuePair.Key] / this.targetTech.costsByResearchTypeID[keyValuePair.Key];
			}
			child.GetComponentsInChildren<Image>()[2].fillAmount = fillAmount;
			child.GetComponent<ToolTip>().SetSimpleTooltip(Research.Instance.researchTypes.GetResearchType(keyValuePair.Key).description);
		}
	}

	// Token: 0x0600A7B7 RID: 42935 RVA: 0x001114CD File Offset: 0x0010F6CD
	private GameObject GetFreeIcon()
	{
		GameObject gameObject = Util.KInstantiateUI(this.iconPrefab, this.iconPanel, false);
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x0600A7B8 RID: 42936 RVA: 0x001114E8 File Offset: 0x0010F6E8
	private Image GetFreeLine()
	{
		return Util.KInstantiateUI<Image>(this.linePrefab.gameObject, base.gameObject, false);
	}

	// Token: 0x0600A7B9 RID: 42937 RVA: 0x004069F4 File Offset: 0x00404BF4
	public void ResearchCompleted(bool notify = true)
	{
		this.BG.color = this.completedColor;
		this.titleBG.color = this.completedHeaderColor;
		this.defaultColor = this.completedColor;
		if (notify)
		{
			this.unlockedTechMetric[ResearchEntry.UnlockedTechKey] = this.targetTech.Id;
			ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.unlockedTechMetric, "ResearchCompleted");
		}
		this.toggle.ClearOnClick();
		if (notify)
		{
			ResearchCompleteMessage message = new ResearchCompleteMessage(this.targetTech);
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x040083A2 RID: 33698
	[Header("Labels")]
	[SerializeField]
	private LocText researchName;

	// Token: 0x040083A3 RID: 33699
	[Header("Transforms")]
	[SerializeField]
	private Transform progressBarContainer;

	// Token: 0x040083A4 RID: 33700
	[SerializeField]
	private Transform lineContainer;

	// Token: 0x040083A5 RID: 33701
	[Header("Prefabs")]
	[SerializeField]
	private GameObject iconPanel;

	// Token: 0x040083A6 RID: 33702
	[SerializeField]
	private GameObject iconPrefab;

	// Token: 0x040083A7 RID: 33703
	[SerializeField]
	private GameObject linePrefab;

	// Token: 0x040083A8 RID: 33704
	[SerializeField]
	private GameObject progressBarPrefab;

	// Token: 0x040083A9 RID: 33705
	[Header("Graphics")]
	[SerializeField]
	private Image BG;

	// Token: 0x040083AA RID: 33706
	[SerializeField]
	private Image titleBG;

	// Token: 0x040083AB RID: 33707
	[SerializeField]
	private Image borderHighlight;

	// Token: 0x040083AC RID: 33708
	[SerializeField]
	private Image filterHighlight;

	// Token: 0x040083AD RID: 33709
	[SerializeField]
	private Image filterLowlight;

	// Token: 0x040083AE RID: 33710
	[SerializeField]
	private Sprite hoverBG;

	// Token: 0x040083AF RID: 33711
	[SerializeField]
	private Sprite completedBG;

	// Token: 0x040083B0 RID: 33712
	[Header("Colors")]
	[SerializeField]
	private Color defaultColor = Color.blue;

	// Token: 0x040083B1 RID: 33713
	[SerializeField]
	private Color completedColor = Color.yellow;

	// Token: 0x040083B2 RID: 33714
	[SerializeField]
	private Color pendingColor = Color.magenta;

	// Token: 0x040083B3 RID: 33715
	[SerializeField]
	private Color completedHeaderColor = Color.grey;

	// Token: 0x040083B4 RID: 33716
	[SerializeField]
	private Color incompleteHeaderColor = Color.grey;

	// Token: 0x040083B5 RID: 33717
	[SerializeField]
	private Color pendingHeaderColor = Color.grey;

	// Token: 0x040083B6 RID: 33718
	private Sprite defaultBG;

	// Token: 0x040083B7 RID: 33719
	[MyCmpGet]
	private KToggle toggle;

	// Token: 0x040083B8 RID: 33720
	private ResearchScreen researchScreen;

	// Token: 0x040083B9 RID: 33721
	private Dictionary<Tech, UILineRenderer> techLineMap;

	// Token: 0x040083BA RID: 33722
	private Tech targetTech;

	// Token: 0x040083BB RID: 33723
	private bool isOn = true;

	// Token: 0x040083BC RID: 33724
	private Coroutine fadeRoutine;

	// Token: 0x040083BD RID: 33725
	public Color activeLineColor;

	// Token: 0x040083BE RID: 33726
	public Color inactiveLineColor;

	// Token: 0x040083BF RID: 33727
	public int lineThickness_active = 6;

	// Token: 0x040083C0 RID: 33728
	public int lineThickness_inactive = 2;

	// Token: 0x040083C1 RID: 33729
	public Material StandardUIMaterial;

	// Token: 0x040083C2 RID: 33730
	private Dictionary<string, GameObject> progressBarsByResearchTypeID = new Dictionary<string, GameObject>();

	// Token: 0x040083C3 RID: 33731
	public static readonly string UnlockedTechKey = "UnlockedTech";

	// Token: 0x040083C4 RID: 33732
	private Dictionary<string, object> unlockedTechMetric = new Dictionary<string, object>
	{
		{
			ResearchEntry.UnlockedTechKey,
			null
		}
	};
}
