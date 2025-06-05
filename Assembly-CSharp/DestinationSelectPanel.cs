using System;
using System.Collections.Generic;
using System.Linq;
using Klei.CustomSettings;
using ProcGen;
using UnityEngine;

// Token: 0x02001CF6 RID: 7414
[AddComponentMenu("KMonoBehaviour/scripts/DestinationSelectPanel")]
public class DestinationSelectPanel : KMonoBehaviour
{
	// Token: 0x17000A2F RID: 2607
	// (get) Token: 0x06009AB3 RID: 39603 RVA: 0x00109296 File Offset: 0x00107496
	// (set) Token: 0x06009AB4 RID: 39604 RVA: 0x0010929D File Offset: 0x0010749D
	public static int ChosenClusterCategorySetting
	{
		get
		{
			return DestinationSelectPanel.chosenClusterCategorySetting;
		}
		set
		{
			DestinationSelectPanel.chosenClusterCategorySetting = value;
		}
	}

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06009AB5 RID: 39605 RVA: 0x003C8D68 File Offset: 0x003C6F68
	// (remove) Token: 0x06009AB6 RID: 39606 RVA: 0x003C8DA0 File Offset: 0x003C6FA0
	public event Action<ColonyDestinationAsteroidBeltData> OnAsteroidClicked;

	// Token: 0x17000A30 RID: 2608
	// (get) Token: 0x06009AB7 RID: 39607 RVA: 0x003C8DD8 File Offset: 0x003C6FD8
	private float min
	{
		get
		{
			return this.asteroidContainer.rect.x + this.offset;
		}
	}

	// Token: 0x17000A31 RID: 2609
	// (get) Token: 0x06009AB8 RID: 39608 RVA: 0x003C8E00 File Offset: 0x003C7000
	private float max
	{
		get
		{
			return this.min + this.asteroidContainer.rect.width;
		}
	}

	// Token: 0x06009AB9 RID: 39609 RVA: 0x003C8E28 File Offset: 0x003C7028
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.dragTarget.onBeginDrag += this.BeginDrag;
		this.dragTarget.onDrag += this.Drag;
		this.dragTarget.onEndDrag += this.EndDrag;
		MultiToggle multiToggle = this.leftArrowButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(this.ClickLeft));
		MultiToggle multiToggle2 = this.rightArrowButton;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(this.ClickRight));
	}

	// Token: 0x06009ABA RID: 39610 RVA: 0x001092A5 File Offset: 0x001074A5
	private void BeginDrag()
	{
		this.dragStartPos = KInputManager.GetMousePos();
		this.dragLastPos = this.dragStartPos;
		this.isDragging = true;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Start", false));
	}

	// Token: 0x06009ABB RID: 39611 RVA: 0x003C8ED0 File Offset: 0x003C70D0
	private void Drag()
	{
		Vector2 vector = KInputManager.GetMousePos();
		float num = vector.x - this.dragLastPos.x;
		this.dragLastPos = vector;
		this.offset += num;
		int num2 = this.selectedIndex;
		this.selectedIndex = Mathf.RoundToInt(-this.offset / this.asteroidXSeparation);
		this.selectedIndex = Mathf.Clamp(this.selectedIndex, 0, this.clusterStartWorlds.Count - 1);
		if (num2 != this.selectedIndex)
		{
			this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
			KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll", false));
		}
	}

	// Token: 0x06009ABC RID: 39612 RVA: 0x001092DA File Offset: 0x001074DA
	private void EndDrag()
	{
		this.Drag();
		this.isDragging = false;
		KFMOD.PlayUISound(GlobalAssets.GetSound("DestinationSelect_Scroll_Stop", false));
	}

	// Token: 0x06009ABD RID: 39613 RVA: 0x003C8F90 File Offset: 0x003C7190
	private void ClickLeft()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex - 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06009ABE RID: 39614 RVA: 0x003C8FE8 File Offset: 0x003C71E8
	private void ClickRight()
	{
		this.selectedIndex = Mathf.Clamp(this.selectedIndex + 1, 0, this.clusterKeys.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[this.selectedIndex]]);
	}

	// Token: 0x06009ABF RID: 39615 RVA: 0x001092F9 File Offset: 0x001074F9
	public void Init()
	{
		this.clusterKeys = new List<string>();
		this.clusterStartWorlds = new Dictionary<string, string>();
		this.UpdateDisplayedClusters();
	}

	// Token: 0x06009AC0 RID: 39616 RVA: 0x000AA038 File Offset: 0x000A8238
	public void Uninit()
	{
	}

	// Token: 0x06009AC1 RID: 39617 RVA: 0x003C9040 File Offset: 0x003C7240
	private void Update()
	{
		if (!this.isDragging)
		{
			float num = this.offset + (float)this.selectedIndex * this.asteroidXSeparation;
			float num2 = 0f;
			if (num != 0f)
			{
				num2 = -num;
			}
			num2 = Mathf.Clamp(num2, -this.asteroidXSeparation * 2f, this.asteroidXSeparation * 2f);
			if (num2 != 0f)
			{
				float num3 = this.centeringSpeed * Time.unscaledDeltaTime;
				float num4 = num2 * this.centeringSpeed * Time.unscaledDeltaTime;
				if (num4 > 0f && num4 < num3)
				{
					num4 = Mathf.Min(num3, num2);
				}
				else if (num4 < 0f && num4 > -num3)
				{
					num4 = Mathf.Max(-num3, num2);
				}
				this.offset += num4;
			}
		}
		float x = this.asteroidContainer.rect.min.x;
		float x2 = this.asteroidContainer.rect.max.x;
		this.offset = Mathf.Clamp(this.offset, (float)(-(float)(this.clusterStartWorlds.Count - 1)) * this.asteroidXSeparation + x, x2);
		this.RePlaceAsteroids();
		for (int i = 0; i < this.moonContainer.transform.childCount; i++)
		{
			this.moonContainer.transform.GetChild(i).GetChild(0).SetLocalPosition(new Vector3(0f, 1.5f + 3f * Mathf.Sin(((float)i + Time.realtimeSinceStartup) * 1.25f), 0f));
		}
	}

	// Token: 0x06009AC2 RID: 39618 RVA: 0x003C91DC File Offset: 0x003C73DC
	public void UpdateDisplayedClusters()
	{
		this.clusterKeys.Clear();
		this.clusterStartWorlds.Clear();
		this.asteroidData.Clear();
		foreach (KeyValuePair<string, ClusterLayout> keyValuePair in SettingsCache.clusterLayouts.clusterCache)
		{
			if ((!DlcManager.FeatureClusterSpaceEnabled() || !(keyValuePair.Key == "clusters/SandstoneDefault")) && keyValuePair.Value.clusterCategory == (ClusterLayout.ClusterCategory)DestinationSelectPanel.ChosenClusterCategorySetting)
			{
				this.clusterKeys.Add(keyValuePair.Key);
				ColonyDestinationAsteroidBeltData value = new ColonyDestinationAsteroidBeltData(keyValuePair.Value.GetStartWorld(), 0, keyValuePair.Key);
				this.asteroidData[keyValuePair.Key] = value;
				this.clusterStartWorlds.Add(keyValuePair.Key, keyValuePair.Value.GetStartWorld());
			}
		}
		this.clusterKeys.Sort((string a, string b) => SettingsCache.clusterLayouts.clusterCache[a].menuOrder.CompareTo(SettingsCache.clusterLayouts.clusterCache[b].menuOrder));
	}

	// Token: 0x06009AC3 RID: 39619 RVA: 0x003C9308 File Offset: 0x003C7508
	[ContextMenu("RePlaceAsteroids")]
	public void RePlaceAsteroids()
	{
		this.BeginAsteroidDrawing();
		for (int i = 0; i < this.clusterKeys.Count; i++)
		{
			float x = this.offset + (float)i * this.asteroidXSeparation;
			string text = this.clusterKeys[i];
			float iconScale = this.asteroidData[text].GetStartWorld.iconScale;
			this.GetAsteroid(text, (i == this.selectedIndex) ? (this.asteroidFocusScale * iconScale) : iconScale).transform.SetLocalPosition(new Vector3(x, (i == this.selectedIndex) ? (5f + 10f * Mathf.Sin(Time.realtimeSinceStartup * 1f)) : 0f, 0f));
		}
		this.EndAsteroidDrawing();
	}

	// Token: 0x06009AC4 RID: 39620 RVA: 0x00109317 File Offset: 0x00107517
	private void BeginAsteroidDrawing()
	{
		this.numAsteroids = 0;
	}

	// Token: 0x06009AC5 RID: 39621 RVA: 0x003C93D0 File Offset: 0x003C75D0
	private void ShowMoons(ColonyDestinationAsteroidBeltData asteroid)
	{
		if (asteroid.worlds.Count > 0)
		{
			while (this.moonContainer.transform.childCount < asteroid.worlds.Count)
			{
				UnityEngine.Object.Instantiate<GameObject>(this.moonPrefab, this.moonContainer.transform);
			}
			for (int i = 0; i < asteroid.worlds.Count; i++)
			{
				KBatchedAnimController componentInChildren = this.moonContainer.transform.GetChild(i).GetComponentInChildren<KBatchedAnimController>();
				int index = (-1 + i + asteroid.worlds.Count / 2) % asteroid.worlds.Count;
				ProcGen.World world = asteroid.worlds[index];
				KAnimFile anim = Assets.GetAnim(world.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : world.asteroidIcon);
				if (anim != null)
				{
					componentInChildren.SetVisiblity(true);
					componentInChildren.SwapAnims(new KAnimFile[]
					{
						anim
					});
					componentInChildren.initialMode = KAnim.PlayMode.Loop;
					componentInChildren.initialAnim = "idle_loop";
					componentInChildren.gameObject.SetActive(true);
					if (componentInChildren.HasAnimation(componentInChildren.initialAnim))
					{
						componentInChildren.Play(componentInChildren.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
					}
					componentInChildren.transform.parent.gameObject.SetActive(true);
				}
			}
			for (int j = asteroid.worlds.Count; j < this.moonContainer.transform.childCount; j++)
			{
				KBatchedAnimController componentInChildren2 = this.moonContainer.transform.GetChild(j).GetComponentInChildren<KBatchedAnimController>();
				if (componentInChildren2 != null)
				{
					componentInChildren2.SetVisiblity(false);
				}
				this.moonContainer.transform.GetChild(j).gameObject.SetActive(false);
			}
			return;
		}
		KBatchedAnimController[] componentsInChildren = this.moonContainer.GetComponentsInChildren<KBatchedAnimController>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].SetVisiblity(false);
		}
	}

	// Token: 0x06009AC6 RID: 39622 RVA: 0x003C95CC File Offset: 0x003C77CC
	private DestinationAsteroid2 GetAsteroid(string name, float scale)
	{
		DestinationAsteroid2 destinationAsteroid;
		if (this.numAsteroids < this.asteroids.Count)
		{
			destinationAsteroid = this.asteroids[this.numAsteroids];
		}
		else
		{
			destinationAsteroid = global::Util.KInstantiateUI<DestinationAsteroid2>(this.asteroidPrefab, this.asteroidContainer.gameObject, false);
			destinationAsteroid.OnClicked += this.OnAsteroidClicked;
			this.asteroids.Add(destinationAsteroid);
		}
		destinationAsteroid.SetAsteroid(this.asteroidData[name]);
		this.asteroidData[name].TargetScale = scale;
		this.asteroidData[name].Scale += (this.asteroidData[name].TargetScale - this.asteroidData[name].Scale) * this.focusScaleSpeed * Time.unscaledDeltaTime;
		destinationAsteroid.transform.localScale = Vector3.one * this.asteroidData[name].Scale;
		this.numAsteroids++;
		return destinationAsteroid;
	}

	// Token: 0x06009AC7 RID: 39623 RVA: 0x003C96D4 File Offset: 0x003C78D4
	private void EndAsteroidDrawing()
	{
		for (int i = 0; i < this.asteroids.Count; i++)
		{
			this.asteroids[i].gameObject.SetActive(i < this.numAsteroids);
		}
	}

	// Token: 0x06009AC8 RID: 39624 RVA: 0x00109320 File Offset: 0x00107520
	public ColonyDestinationAsteroidBeltData SelectCluster(string name, int seed)
	{
		this.selectedIndex = this.clusterKeys.IndexOf(name);
		this.asteroidData[name].ReInitialize(seed);
		return this.asteroidData[name];
	}

	// Token: 0x06009AC9 RID: 39625 RVA: 0x003C9718 File Offset: 0x003C7918
	public string GetDefaultAsteroid()
	{
		foreach (string text in this.clusterKeys)
		{
			if (this.asteroidData[text].Layout.menuOrder == 0)
			{
				return text;
			}
		}
		return this.clusterKeys.First<string>();
	}

	// Token: 0x06009ACA RID: 39626 RVA: 0x003C9790 File Offset: 0x003C7990
	public ColonyDestinationAsteroidBeltData SelectDefaultAsteroid(int seed)
	{
		this.selectedIndex = 0;
		string key = this.asteroidData.Keys.First<string>();
		this.asteroidData[key].ReInitialize(seed);
		return this.asteroidData[key];
	}

	// Token: 0x06009ACB RID: 39627 RVA: 0x003C97D4 File Offset: 0x003C79D4
	public void ScrollLeft()
	{
		int index = Mathf.Max(this.selectedIndex - 1, 0);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06009ACC RID: 39628 RVA: 0x003C9814 File Offset: 0x003C7A14
	public void ScrollRight()
	{
		int index = Mathf.Min(this.selectedIndex + 1, this.clusterStartWorlds.Count - 1);
		this.OnAsteroidClicked(this.asteroidData[this.clusterKeys[index]]);
	}

	// Token: 0x06009ACD RID: 39629 RVA: 0x003C9860 File Offset: 0x003C7A60
	private void DebugCurrentSetting()
	{
		ColonyDestinationAsteroidBeltData colonyDestinationAsteroidBeltData = this.asteroidData[this.clusterKeys[this.selectedIndex]];
		string text = "{world}: {seed} [{traits}] {{settings}}";
		string startWorldName = colonyDestinationAsteroidBeltData.startWorldName;
		string newValue = colonyDestinationAsteroidBeltData.seed.ToString();
		text = text.Replace("{world}", startWorldName);
		text = text.Replace("{seed}", newValue);
		List<AsteroidDescriptor> traitDescriptors = colonyDestinationAsteroidBeltData.GetTraitDescriptors();
		string[] array = new string[traitDescriptors.Count];
		for (int i = 0; i < traitDescriptors.Count; i++)
		{
			array[i] = traitDescriptors[i].text;
		}
		string newValue2 = string.Join(", ", array);
		text = text.Replace("{traits}", newValue2);
		CustomGameSettings.CustomGameMode customGameMode = CustomGameSettings.Instance.customGameMode;
		if (customGameMode != CustomGameSettings.CustomGameMode.Survival)
		{
			if (customGameMode != CustomGameSettings.CustomGameMode.Nosweat)
			{
				if (customGameMode == CustomGameSettings.CustomGameMode.Custom)
				{
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, SettingConfig> keyValuePair in CustomGameSettings.Instance.QualitySettings)
					{
						if (keyValuePair.Value.coordinate_range >= 0L)
						{
							SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(keyValuePair.Key);
							if (currentQualitySetting.id != keyValuePair.Value.GetDefaultLevelId())
							{
								list.Add(string.Format("{0}={1}", keyValuePair.Value.label, currentQualitySetting.label));
							}
						}
					}
					text = text.Replace("{settings}", string.Join(", ", list.ToArray()));
				}
			}
			else
			{
				text = text.Replace("{settings}", "Nosweat");
			}
		}
		else
		{
			text = text.Replace("{settings}", "Survival");
		}
		global::Debug.Log(text);
	}

	// Token: 0x040078CE RID: 30926
	[SerializeField]
	private GameObject asteroidPrefab;

	// Token: 0x040078CF RID: 30927
	[SerializeField]
	private KButtonDrag dragTarget;

	// Token: 0x040078D0 RID: 30928
	[SerializeField]
	private MultiToggle leftArrowButton;

	// Token: 0x040078D1 RID: 30929
	[SerializeField]
	private MultiToggle rightArrowButton;

	// Token: 0x040078D2 RID: 30930
	[SerializeField]
	private RectTransform asteroidContainer;

	// Token: 0x040078D3 RID: 30931
	[SerializeField]
	private float asteroidFocusScale = 2f;

	// Token: 0x040078D4 RID: 30932
	[SerializeField]
	private float asteroidXSeparation = 240f;

	// Token: 0x040078D5 RID: 30933
	[SerializeField]
	private float focusScaleSpeed = 0.5f;

	// Token: 0x040078D6 RID: 30934
	[SerializeField]
	private float centeringSpeed = 0.5f;

	// Token: 0x040078D7 RID: 30935
	[SerializeField]
	private GameObject moonContainer;

	// Token: 0x040078D8 RID: 30936
	[SerializeField]
	private GameObject moonPrefab;

	// Token: 0x040078D9 RID: 30937
	private static int chosenClusterCategorySetting;

	// Token: 0x040078DB RID: 30939
	private float offset;

	// Token: 0x040078DC RID: 30940
	private int selectedIndex = -1;

	// Token: 0x040078DD RID: 30941
	private List<DestinationAsteroid2> asteroids = new List<DestinationAsteroid2>();

	// Token: 0x040078DE RID: 30942
	private int numAsteroids;

	// Token: 0x040078DF RID: 30943
	private List<string> clusterKeys;

	// Token: 0x040078E0 RID: 30944
	private Dictionary<string, string> clusterStartWorlds;

	// Token: 0x040078E1 RID: 30945
	private Dictionary<string, ColonyDestinationAsteroidBeltData> asteroidData = new Dictionary<string, ColonyDestinationAsteroidBeltData>();

	// Token: 0x040078E2 RID: 30946
	private Vector2 dragStartPos;

	// Token: 0x040078E3 RID: 30947
	private Vector2 dragLastPos;

	// Token: 0x040078E4 RID: 30948
	private bool isDragging;

	// Token: 0x040078E5 RID: 30949
	private const string debugFmt = "{world}: {seed} [{traits}] {{settings}}";
}
