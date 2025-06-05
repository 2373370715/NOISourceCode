using System;
using System.Collections.Generic;
using Database;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C55 RID: 7253
[AddComponentMenu("KMonoBehaviour/scripts/AsteroidDescriptorPanel")]
public class AsteroidDescriptorPanel : KMonoBehaviour
{
	// Token: 0x060096AC RID: 38572 RVA: 0x00106928 File Offset: 0x00104B28
	public bool HasDescriptors()
	{
		return this.labels.Count > 0;
	}

	// Token: 0x060096AD RID: 38573 RVA: 0x00106938 File Offset: 0x00104B38
	public void EnableClusterDetails(bool setActive)
	{
		this.clusterNameLabel.gameObject.SetActive(setActive);
		this.clusterDifficultyLabel.gameObject.SetActive(setActive);
	}

	// Token: 0x060096AE RID: 38574 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060096AF RID: 38575 RVA: 0x003AD2C4 File Offset: 0x003AB4C4
	public void SetClusterDetailLabels(ColonyDestinationAsteroidBeltData cluster)
	{
		StringEntry stringEntry;
		Strings.TryGet(cluster.properName, out stringEntry);
		this.clusterNameLabel.SetText((stringEntry == null) ? "" : string.Format(WORLDS.SURVIVAL_CHANCE.CLUSTERNAME, stringEntry.String));
		int index = Mathf.Clamp(cluster.difficulty, 0, ColonyDestinationAsteroidBeltData.survivalOptions.Count - 1);
		global::Tuple<string, string, string> tuple = ColonyDestinationAsteroidBeltData.survivalOptions[index];
		string text = string.Format(WORLDS.SURVIVAL_CHANCE.TITLE, tuple.first, tuple.third);
		text = text.Trim('\n');
		this.clusterDifficultyLabel.SetText(text);
	}

	// Token: 0x060096B0 RID: 38576 RVA: 0x003AD360 File Offset: 0x003AB560
	public void SetParameterDescriptors(IList<AsteroidDescriptor> descriptors)
	{
		for (int i = 0; i < this.parameterWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.parameterWidgets[i]);
		}
		this.parameterWidgets.Clear();
		for (int j = 0; j < descriptors.Count; j++)
		{
			GameObject gameObject = global::Util.KInstantiateUI(this.prefabParameterWidget, base.gameObject, true);
			gameObject.GetComponent<LocText>().SetText(descriptors[j].text);
			ToolTip component = gameObject.GetComponent<ToolTip>();
			if (!string.IsNullOrEmpty(descriptors[j].tooltip))
			{
				component.SetSimpleTooltip(descriptors[j].tooltip);
			}
			this.parameterWidgets.Add(gameObject);
		}
	}

	// Token: 0x060096B1 RID: 38577 RVA: 0x003AD414 File Offset: 0x003AB614
	private void ClearTraitDescriptors()
	{
		for (int i = 0; i < this.traitWidgets.Count; i++)
		{
			UnityEngine.Object.Destroy(this.traitWidgets[i]);
		}
		this.traitWidgets.Clear();
		for (int j = 0; j < this.traitCategoryWidgets.Count; j++)
		{
			UnityEngine.Object.Destroy(this.traitCategoryWidgets[j]);
		}
		this.traitCategoryWidgets.Clear();
	}

	// Token: 0x060096B2 RID: 38578 RVA: 0x003AD488 File Offset: 0x003AB688
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, List<string> stories, bool includeDescriptions = true)
	{
		foreach (string id in stories)
		{
			WorldTrait storyTrait = Db.Get().Stories.Get(id).StoryTrait;
			string tooltip = DlcManager.IsPureVanilla() ? Strings.Get(storyTrait.description + "_SHORT") : Strings.Get(storyTrait.description);
			descriptors.Add(new AsteroidDescriptor(Strings.Get(storyTrait.name).String, tooltip, Color.white, null, storyTrait.icon));
		}
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
		if (stories.Count != 0)
		{
			this.storyTraitHeader.rectTransform().SetSiblingIndex(this.storyTraitHeader.rectTransform().parent.childCount - stories.Count - 1);
			this.storyTraitHeader.SetActive(true);
			return;
		}
		this.storyTraitHeader.SetActive(false);
	}

	// Token: 0x060096B3 RID: 38579 RVA: 0x0010695C File Offset: 0x00104B5C
	public void SetTraitDescriptors(IList<AsteroidDescriptor> descriptors, bool includeDescriptions = true)
	{
		this.SetTraitDescriptors(new List<IList<AsteroidDescriptor>>
		{
			descriptors
		}, includeDescriptions, null);
	}

	// Token: 0x060096B4 RID: 38580 RVA: 0x003AD5A0 File Offset: 0x003AB7A0
	public void SetTraitDescriptors(List<IList<AsteroidDescriptor>> descriptorSets, bool includeDescriptions = true, List<global::Tuple<string, Sprite>> headerData = null)
	{
		this.ClearTraitDescriptors();
		for (int i = 0; i < descriptorSets.Count; i++)
		{
			IList<AsteroidDescriptor> list = descriptorSets[i];
			GameObject gameObject = base.gameObject;
			if (descriptorSets.Count > 1)
			{
				global::Debug.Assert(headerData != null, "Asteroid Header data is null - traits wont have their world as contex in the selection UI");
				GameObject gameObject2 = global::Util.KInstantiate(this.prefabTraitCategoryWidget, base.gameObject, null);
				HierarchyReferences component = gameObject2.GetComponent<HierarchyReferences>();
				gameObject2.transform.localScale = Vector3.one;
				StringEntry stringEntry;
				string text = Strings.TryGet(headerData[i].first, out stringEntry) ? stringEntry.String : headerData[i].first;
				component.GetReference<LocText>("NameLabel").SetText(text);
				component.GetReference<Image>("Icon").sprite = headerData[i].second;
				gameObject2.SetActive(true);
				gameObject = component.GetReference<RectTransform>("Contents").gameObject;
				this.traitCategoryWidgets.Add(gameObject2);
			}
			for (int j = 0; j < list.Count; j++)
			{
				GameObject gameObject3 = global::Util.KInstantiate(this.prefabTraitWidget, gameObject, null);
				HierarchyReferences component2 = gameObject3.GetComponent<HierarchyReferences>();
				gameObject3.SetActive(true);
				component2.GetReference<LocText>("NameLabel").SetText("<b>" + list[j].text + "</b>");
				Image reference = component2.GetReference<Image>("Icon");
				reference.color = list[j].associatedColor;
				if (list[j].associatedIcon != null)
				{
					Sprite sprite = Assets.GetSprite(list[j].associatedIcon);
					if (sprite != null)
					{
						reference.sprite = sprite;
					}
				}
				if (gameObject3.GetComponent<ToolTip>() != null)
				{
					gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(list[j].tooltip);
				}
				LocText reference2 = component2.GetReference<LocText>("DescLabel");
				if (includeDescriptions && !string.IsNullOrEmpty(list[j].tooltip))
				{
					reference2.SetText(list[j].tooltip);
				}
				else
				{
					reference2.gameObject.SetActive(false);
				}
				gameObject3.transform.localScale = new Vector3(1f, 1f, 1f);
				gameObject3.SetActive(true);
				this.traitWidgets.Add(gameObject3);
			}
		}
	}

	// Token: 0x060096B5 RID: 38581 RVA: 0x003AD800 File Offset: 0x003ABA00
	public void EnableClusterLocationLabels(bool enable)
	{
		this.startingAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.nearbyAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
		this.distantAsteroidRowContainer.transform.parent.gameObject.SetActive(enable);
	}

	// Token: 0x060096B6 RID: 38582 RVA: 0x003AD860 File Offset: 0x003ABA60
	public void RefreshAsteroidLines(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> storyTraits)
	{
		cluster.RemixClusterLayout();
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			if (!keyValuePair.Value.IsNullOrDestroyed())
			{
				UnityEngine.Object.Destroy(keyValuePair.Value);
			}
		}
		this.asteroidLines.Clear();
		this.SpawnAsteroidLine(cluster.GetStartWorld, this.startingAsteroidRowContainer, cluster);
		for (int i = 0; i < cluster.worlds.Count; i++)
		{
			ProcGen.World world = cluster.worlds[i];
			WorldPlacement worldPlacement = null;
			for (int j = 0; j < cluster.Layout.worldPlacements.Count; j++)
			{
				if (cluster.Layout.worldPlacements[j].world == world.filePath)
				{
					worldPlacement = cluster.Layout.worldPlacements[j];
					break;
				}
			}
			this.SpawnAsteroidLine(world, (worldPlacement.locationType == WorldPlacement.LocationType.InnerCluster) ? this.nearbyAsteroidRowContainer : this.distantAsteroidRowContainer, cluster);
		}
		using (Dictionary<ProcGen.World, GameObject>.Enumerator enumerator = this.asteroidLines.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ProcGen.World, GameObject> line = enumerator.Current;
				MultiToggle component = line.Value.GetComponent<MultiToggle>();
				component.onClick = (System.Action)Delegate.Combine(component.onClick, new System.Action(delegate()
				{
					this.SelectAsteroidInCluster(line.Key, cluster, selectedAsteroidDetailsPanel);
				}));
			}
		}
		this.SelectWholeClusterDetails(cluster, selectedAsteroidDetailsPanel, storyTraits);
	}

	// Token: 0x060096B7 RID: 38583 RVA: 0x003ADA70 File Offset: 0x003ABC70
	private void SelectAsteroidInCluster(ProcGen.World asteroid, ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(true);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(0);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == asteroid) ? 1 : 0);
			if (keyValuePair.Key == asteroid)
			{
				this.SetSelectedAsteroid(keyValuePair.Key, selectedAsteroidDetailsPanel, cluster.GenerateTraitDescriptors(keyValuePair.Key, true));
			}
		}
	}

	// Token: 0x060096B8 RID: 38584 RVA: 0x003ADB20 File Offset: 0x003ABD20
	public void SelectWholeClusterDetails(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel selectedAsteroidDetailsPanel, List<string> stories)
	{
		selectedAsteroidDetailsPanel.SpacedOutContentContainer.SetActive(false);
		foreach (KeyValuePair<ProcGen.World, GameObject> keyValuePair in this.asteroidLines)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState(0);
		}
		this.SetSelectedCluster(cluster, selectedAsteroidDetailsPanel, stories);
		this.clusterDetailsButton.GetComponent<MultiToggle>().ChangeState(1);
	}

	// Token: 0x060096B9 RID: 38585 RVA: 0x003ADBA4 File Offset: 0x003ABDA4
	private void SpawnAsteroidLine(ProcGen.World asteroid, GameObject parentContainer, ColonyDestinationAsteroidBeltData cluster)
	{
		if (this.asteroidLines.ContainsKey(asteroid))
		{
			return;
		}
		GameObject gameObject = global::Util.KInstantiateUI(this.prefabAsteroidLine, parentContainer.gameObject, true);
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		Image reference = component.GetReference<Image>("Icon");
		LocText reference2 = component.GetReference<LocText>("Label");
		RectTransform reference3 = component.GetReference<RectTransform>("TraitsRow");
		LocText reference4 = component.GetReference<LocText>("TraitLabel");
		ToolTip component2 = gameObject.GetComponent<ToolTip>();
		Image component3 = gameObject.transform.Find("DlcBanner").GetComponent<Image>();
		Sprite uisprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		reference.sprite = uisprite;
		reference2.SetText(asteroid.GetProperName());
		List<WorldTrait> worldTraits = cluster.GetWorldTraits(asteroid);
		reference4.gameObject.SetActive(worldTraits.Count == 0);
		reference4.SetText(UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS);
		RectTransform reference5 = component.GetReference<RectTransform>("TraitIconPrefab");
		foreach (WorldTrait worldTrait in worldTraits)
		{
			Image component4 = global::Util.KInstantiateUI(reference5.gameObject, reference3.gameObject, true).GetComponent<Image>();
			Sprite sprite = Assets.GetSprite(worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1));
			if (sprite != null)
			{
				component4.sprite = sprite;
			}
			component4.color = global::Util.ColorFromHex(worldTrait.colorHex);
		}
		string text = "";
		if (worldTraits.Count > 0)
		{
			for (int i = 0; i < worldTraits.Count; i++)
			{
				StringEntry stringEntry;
				Strings.TryGet(worldTraits[i].name, out stringEntry);
				StringEntry stringEntry2;
				Strings.TryGet(worldTraits[i].description, out stringEntry2);
				text = string.Concat(new string[]
				{
					text,
					"<color=#",
					worldTraits[i].colorHex,
					">",
					stringEntry.String,
					"</color>\n",
					stringEntry2.String
				});
				if (i != worldTraits.Count - 1)
				{
					text += "\n\n";
				}
			}
		}
		else
		{
			text = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			text = text + "\n\n" + string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.MIXING_TOOLTIP_DLC_CONTENT, DlcManager.GetDlcTitle(asteroid.dlcIdFrom));
		}
		component2.SetSimpleTooltip(text);
		if (DlcManager.IsDlcId(asteroid.dlcIdFrom))
		{
			component3.color = DlcManager.GetDlcBannerColor(asteroid.dlcIdFrom);
			component3.gameObject.SetActive(true);
		}
		else
		{
			component3.gameObject.SetActive(false);
		}
		this.asteroidLines.Add(asteroid, gameObject);
	}

	// Token: 0x060096BA RID: 38586 RVA: 0x003ADE84 File Offset: 0x003AC084
	private void SetSelectedAsteroid(ProcGen.World asteroid, AsteroidDescriptorPanel detailPanel, List<AsteroidDescriptor> traitDescriptors)
	{
		detailPanel.SetTraitDescriptors(traitDescriptors, true);
		detailPanel.selectedAsteroidIcon.sprite = ColonyDestinationAsteroidBeltData.GetUISprite(asteroid.asteroidIcon);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(true);
		detailPanel.selectedAsteroidLabel.SetText(asteroid.GetProperName());
		detailPanel.selectedAsteroidDescription.SetText(asteroid.GetProperDescription());
	}

	// Token: 0x060096BB RID: 38587 RVA: 0x003ADEE4 File Offset: 0x003AC0E4
	private void SetSelectedCluster(ColonyDestinationAsteroidBeltData cluster, AsteroidDescriptorPanel detailPanel, List<string> stories)
	{
		List<IList<AsteroidDescriptor>> list = new List<IList<AsteroidDescriptor>>();
		List<global::Tuple<string, Sprite>> list2 = new List<global::Tuple<string, Sprite>>();
		List<AsteroidDescriptor> list3 = cluster.GenerateTraitDescriptors(cluster.GetStartWorld, false);
		if (list3.Count != 0)
		{
			list2.Add(new global::Tuple<string, Sprite>(cluster.GetStartWorld.name, ColonyDestinationAsteroidBeltData.GetUISprite(cluster.GetStartWorld.asteroidIcon)));
			list.Add(list3);
		}
		foreach (ProcGen.World world in cluster.worlds)
		{
			List<AsteroidDescriptor> list4 = cluster.GenerateTraitDescriptors(world, false);
			if (list4.Count != 0)
			{
				list2.Add(new global::Tuple<string, Sprite>(world.name, ColonyDestinationAsteroidBeltData.GetUISprite(world.asteroidIcon)));
				list.Add(list4);
			}
		}
		list2.Add(new global::Tuple<string, Sprite>("STRINGS.UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER", Assets.GetSprite("codexIconStoryTraits")));
		List<AsteroidDescriptor> list5 = new List<AsteroidDescriptor>();
		foreach (string id in stories)
		{
			Story story = Db.Get().Stories.Get(id);
			string icon = story.StoryTrait.icon;
			AsteroidDescriptor item = new AsteroidDescriptor(Strings.Get(story.StoryTrait.name).String, Strings.Get(story.StoryTrait.description).String, Color.white, null, icon);
			list5.Add(item);
		}
		list.Add(list5);
		detailPanel.SetTraitDescriptors(list, false, list2);
		detailPanel.selectedAsteroidIcon.gameObject.SetActive(false);
		string text = cluster.properName;
		StringEntry stringEntry;
		if (Strings.TryGet(cluster.properName, out stringEntry))
		{
			text = stringEntry.String;
		}
		detailPanel.selectedAsteroidLabel.SetText(text);
		detailPanel.selectedAsteroidDescription.SetText("");
	}

	// Token: 0x04007515 RID: 29973
	[Header("Destination Details")]
	[SerializeField]
	private GameObject customLabelPrefab;

	// Token: 0x04007516 RID: 29974
	[SerializeField]
	private GameObject prefabTraitWidget;

	// Token: 0x04007517 RID: 29975
	[SerializeField]
	private GameObject prefabTraitCategoryWidget;

	// Token: 0x04007518 RID: 29976
	[SerializeField]
	private GameObject prefabParameterWidget;

	// Token: 0x04007519 RID: 29977
	[SerializeField]
	private GameObject startingAsteroidRowContainer;

	// Token: 0x0400751A RID: 29978
	[SerializeField]
	private GameObject nearbyAsteroidRowContainer;

	// Token: 0x0400751B RID: 29979
	[SerializeField]
	private GameObject distantAsteroidRowContainer;

	// Token: 0x0400751C RID: 29980
	[SerializeField]
	private LocText clusterNameLabel;

	// Token: 0x0400751D RID: 29981
	[SerializeField]
	private LocText clusterDifficultyLabel;

	// Token: 0x0400751E RID: 29982
	[SerializeField]
	public LocText headerLabel;

	// Token: 0x0400751F RID: 29983
	[SerializeField]
	public MultiToggle clusterDetailsButton;

	// Token: 0x04007520 RID: 29984
	[SerializeField]
	public GameObject storyTraitHeader;

	// Token: 0x04007521 RID: 29985
	private List<GameObject> labels = new List<GameObject>();

	// Token: 0x04007522 RID: 29986
	[Header("Selected Asteroid Details")]
	[SerializeField]
	private GameObject SpacedOutContentContainer;

	// Token: 0x04007523 RID: 29987
	public Image selectedAsteroidIcon;

	// Token: 0x04007524 RID: 29988
	public LocText selectedAsteroidLabel;

	// Token: 0x04007525 RID: 29989
	public LocText selectedAsteroidDescription;

	// Token: 0x04007526 RID: 29990
	[SerializeField]
	private GameObject prefabAsteroidLine;

	// Token: 0x04007527 RID: 29991
	private Dictionary<ProcGen.World, GameObject> asteroidLines = new Dictionary<ProcGen.World, GameObject>();

	// Token: 0x04007528 RID: 29992
	private List<GameObject> traitWidgets = new List<GameObject>();

	// Token: 0x04007529 RID: 29993
	private List<GameObject> traitCategoryWidgets = new List<GameObject>();

	// Token: 0x0400752A RID: 29994
	private List<GameObject> parameterWidgets = new List<GameObject>();
}
