using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CC4 RID: 7364
public class ColonyDiagnosticScreen : KScreen, ISim1000ms
{
	// Token: 0x0600999E RID: 39326 RVA: 0x003C4290 File Offset: 0x003C2490
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ColonyDiagnosticScreen.Instance = this;
		this.RefreshSingleWorld(null);
		Game.Instance.Subscribe(1983128072, new Action<object>(this.RefreshSingleWorld));
		MultiToggle multiToggle = this.seeAllButton;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			bool flag = !AllDiagnosticsScreen.Instance.isHiddenButActive;
			AllDiagnosticsScreen.Instance.Show(!flag);
		}));
	}

	// Token: 0x0600999F RID: 39327 RVA: 0x00108531 File Offset: 0x00106731
	protected override void OnForcedCleanUp()
	{
		ColonyDiagnosticScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x060099A0 RID: 39328 RVA: 0x003C4308 File Offset: 0x003C2508
	private void RefreshSingleWorld(object data = null)
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			diagnosticRow.OnCleanUp();
			Util.KDestroyGameObject(diagnosticRow.gameObject);
		}
		this.diagnosticRows.Clear();
		this.SpawnTrackerLines(ClusterManager.Instance.activeWorldId);
	}

	// Token: 0x060099A1 RID: 39329 RVA: 0x003C4380 File Offset: 0x003C2580
	private void SpawnTrackerLines(int world)
	{
		this.AddDiagnostic<BreathabilityDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FoodDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<StressDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RadiationDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ReactorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID"))
		{
			if (Game.IsDlcActiveForCurrentSave("EXPANSION1_ID"))
			{
				this.AddDiagnostic<SelfChargingElectrobankDiagnostic>(world, this.contentContainer, this.diagnosticRows);
			}
			this.AddDiagnostic<BionicBatteryDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		}
		this.AddDiagnostic<FloatingRocketDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketFuelDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketOxidizerDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<FarmDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<ToiletDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<IdleDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<TrappedDuplicantDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<EntombedDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<PowerUseDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<BatteryDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<RocketsInOrbitDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		this.AddDiagnostic<MeteorDiagnostic>(world, this.contentContainer, this.diagnosticRows);
		List<ColonyDiagnosticScreen.DiagnosticRow> list = new List<ColonyDiagnosticScreen.DiagnosticRow>();
		foreach (ColonyDiagnosticScreen.DiagnosticRow item in this.diagnosticRows)
		{
			list.Add(item);
		}
		list.Sort((ColonyDiagnosticScreen.DiagnosticRow a, ColonyDiagnosticScreen.DiagnosticRow b) => a.diagnostic.name.CompareTo(b.diagnostic.name));
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in list)
		{
			diagnosticRow.gameObject.transform.SetAsLastSibling();
		}
		list.Clear();
		this.seeAllButton.transform.SetAsLastSibling();
		this.RefreshAll();
	}

	// Token: 0x060099A2 RID: 39330 RVA: 0x003C4608 File Offset: 0x003C2808
	private GameObject AddDiagnostic<T>(int worldID, GameObject parent, List<ColonyDiagnosticScreen.DiagnosticRow> parentCollection) where T : ColonyDiagnostic
	{
		T diagnostic = ColonyDiagnosticUtility.Instance.GetDiagnostic<T>(worldID);
		if (diagnostic == null)
		{
			return null;
		}
		GameObject gameObject = Util.KInstantiateUI(this.linePrefab, parent, true);
		parentCollection.Add(new ColonyDiagnosticScreen.DiagnosticRow(worldID, gameObject, diagnostic));
		return gameObject;
	}

	// Token: 0x060099A3 RID: 39331 RVA: 0x0010853F File Offset: 0x0010673F
	public static void SetIndication(ColonyDiagnostic.DiagnosticResult.Opinion opinion, GameObject indicatorGameObject)
	{
		indicatorGameObject.GetComponentInChildren<Image>().color = ColonyDiagnosticScreen.GetDiagnosticIndicationColor(opinion);
	}

	// Token: 0x060099A4 RID: 39332 RVA: 0x00108552 File Offset: 0x00106752
	public static Color GetDiagnosticIndicationColor(ColonyDiagnostic.DiagnosticResult.Opinion opinion)
	{
		switch (opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
			return Constants.NEGATIVE_COLOR;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			return Constants.WARNING_COLOR;
		}
		return Color.white;
	}

	// Token: 0x060099A5 RID: 39333 RVA: 0x0010858F File Offset: 0x0010678F
	public void Sim1000ms(float dt)
	{
		this.RefreshAll();
	}

	// Token: 0x060099A6 RID: 39334 RVA: 0x003C4650 File Offset: 0x003C2850
	public void RefreshAll()
	{
		foreach (ColonyDiagnosticScreen.DiagnosticRow diagnosticRow in this.diagnosticRows)
		{
			if (diagnosticRow.worldID == ClusterManager.Instance.activeWorldId)
			{
				this.UpdateDiagnosticRow(diagnosticRow);
			}
		}
		ColonyDiagnosticScreen.SetIndication(ColonyDiagnosticUtility.Instance.GetWorldDiagnosticResult(ClusterManager.Instance.activeWorldId), this.rootIndicator);
		this.seeAllButton.GetComponentInChildren<LocText>().SetText(string.Format(UI.DIAGNOSTICS_SCREEN.SEE_ALL, AllDiagnosticsScreen.Instance.GetRowCount()));
	}

	// Token: 0x060099A7 RID: 39335 RVA: 0x003C4704 File Offset: 0x003C2904
	private ColonyDiagnostic.DiagnosticResult.Opinion UpdateDiagnosticRow(ColonyDiagnosticScreen.DiagnosticRow row)
	{
		ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult = row.currentDisplayedResult;
		bool activeInHierarchy = row.gameObject.activeInHierarchy;
		if (ColonyDiagnosticUtility.Instance.IsDiagnosticTutorialDisabled(row.diagnostic.id))
		{
			this.SetRowActive(row, false);
		}
		else
		{
			switch (ColonyDiagnosticUtility.Instance.diagnosticDisplaySettings[row.worldID][row.diagnostic.id])
			{
			case ColonyDiagnosticUtility.DisplaySetting.Always:
				this.SetRowActive(row, true);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.AlertOnly:
				this.SetRowActive(row, row.diagnostic.LatestResult.opinion < ColonyDiagnostic.DiagnosticResult.Opinion.Normal);
				break;
			case ColonyDiagnosticUtility.DisplaySetting.Never:
				this.SetRowActive(row, false);
				break;
			}
			if (row.gameObject.activeInHierarchy && (row.currentDisplayedResult < currentDisplayedResult || (row.currentDisplayedResult < ColonyDiagnostic.DiagnosticResult.Opinion.Normal && !activeInHierarchy)) && row.CheckAllowVisualNotification())
			{
				row.TriggerVisualNotification();
			}
		}
		return row.diagnostic.LatestResult.opinion;
	}

	// Token: 0x060099A8 RID: 39336 RVA: 0x00108597 File Offset: 0x00106797
	private void SetRowActive(ColonyDiagnosticScreen.DiagnosticRow row, bool active)
	{
		if (row.gameObject.activeSelf != active)
		{
			row.gameObject.SetActive(active);
			row.ResolveNotificationRoutine();
		}
	}

	// Token: 0x0400777F RID: 30591
	public GameObject linePrefab;

	// Token: 0x04007780 RID: 30592
	public static ColonyDiagnosticScreen Instance;

	// Token: 0x04007781 RID: 30593
	private List<ColonyDiagnosticScreen.DiagnosticRow> diagnosticRows = new List<ColonyDiagnosticScreen.DiagnosticRow>();

	// Token: 0x04007782 RID: 30594
	public GameObject header;

	// Token: 0x04007783 RID: 30595
	public GameObject contentContainer;

	// Token: 0x04007784 RID: 30596
	public GameObject rootIndicator;

	// Token: 0x04007785 RID: 30597
	public MultiToggle seeAllButton;

	// Token: 0x04007786 RID: 30598
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsActive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Active_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Active_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Active_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Active_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Active_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Active_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x04007787 RID: 30599
	public static Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string> notificationSoundsInactive = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, string>
	{
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening,
			"Diagnostic_Inactive_DuplicantThreatening"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Bad,
			"Diagnostic_Inactive_Bad"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Warning,
			"Diagnostic_Inactive_Warning"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Concern,
			"Diagnostic_Inactive_Concern"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion,
			"Diagnostic_Inactive_Suggestion"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial,
			"Diagnostic_Inactive_Tutorial"
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Normal,
			""
		},
		{
			ColonyDiagnostic.DiagnosticResult.Opinion.Good,
			""
		}
	};

	// Token: 0x02001CC5 RID: 7365
	private class DiagnosticRow : ISim4000ms
	{
		// Token: 0x060099AB RID: 39339 RVA: 0x003C48D4 File Offset: 0x003C2AD4
		public DiagnosticRow(int worldID, GameObject gameObject, ColonyDiagnostic diagnostic)
		{
			global::Debug.Assert(diagnostic != null);
			HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
			this.worldID = worldID;
			this.sparkLayer = component.GetReference<SparkLayer>("SparkLayer");
			this.diagnostic = diagnostic;
			this.titleLabel = component.GetReference<LocText>("TitleLabel");
			this.valueLabel = component.GetReference<LocText>("ValueLabel");
			this.indicator = component.GetReference<Image>("Indicator");
			this.image = component.GetReference<Image>("Image");
			this.tooltip = gameObject.GetComponent<ToolTip>();
			this.gameObject = gameObject;
			this.titleLabel.SetText(diagnostic.name);
			this.sparkLayer.colorRules.setOwnColor = false;
			if (diagnostic.tracker == null)
			{
				this.sparkLayer.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.sparkLayer.ClearLines();
				global::Tuple<float, float>[] points = diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.NewLine(points, diagnostic.name);
			}
			this.button = gameObject.GetComponent<MultiToggle>();
			MultiToggle multiToggle = this.button;
			multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
			{
				KSelectable kselectable = null;
				Vector3 pos = Vector3.zero;
				if (diagnostic.LatestResult.clickThroughTarget != null)
				{
					pos = diagnostic.LatestResult.clickThroughTarget.first;
					kselectable = ((diagnostic.LatestResult.clickThroughTarget.second == null) ? null : diagnostic.LatestResult.clickThroughTarget.second.GetComponent<KSelectable>());
				}
				else
				{
					GameObject nextClickThroughObject = diagnostic.GetNextClickThroughObject();
					if (nextClickThroughObject != null)
					{
						kselectable = nextClickThroughObject.GetComponent<KSelectable>();
						pos = nextClickThroughObject.transform.GetPosition();
					}
				}
				if (kselectable == null)
				{
					CameraController.Instance.ActiveWorldStarWipe(diagnostic.worldID, null);
					return;
				}
				SelectTool.Instance.SelectAndFocus(pos, kselectable);
			}));
			this.defaultIndicatorSizeDelta = Vector2.zero;
			this.Update(true);
			SimAndRenderScheduler.instance.Add(this, true);
		}

		// Token: 0x060099AC RID: 39340 RVA: 0x000C550D File Offset: 0x000C370D
		public void OnCleanUp()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x060099AD RID: 39341 RVA: 0x001085CC File Offset: 0x001067CC
		public void Sim4000ms(float dt)
		{
			this.Update(false);
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x060099AE RID: 39342 RVA: 0x001085D5 File Offset: 0x001067D5
		// (set) Token: 0x060099AF RID: 39343 RVA: 0x001085DD File Offset: 0x001067DD
		public GameObject gameObject { get; private set; }

		// Token: 0x060099B0 RID: 39344 RVA: 0x003C4A60 File Offset: 0x003C2C60
		public void Update(bool force = false)
		{
			if (!force && ClusterManager.Instance.activeWorldId != this.worldID)
			{
				return;
			}
			Color color = Color.white;
			global::Debug.Assert(this.diagnostic.LatestResult.opinion > ColonyDiagnostic.DiagnosticResult.Opinion.Unset, string.Format("{0} criteria returned no opinion. Make sure the DiagnosticResult parameters are used or an opinion result is otherwise set in all of its criteria", this.diagnostic));
			this.currentDisplayedResult = this.diagnostic.LatestResult.opinion;
			color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			if (this.diagnostic.tracker != null)
			{
				global::Tuple<float, float>[] data = this.diagnostic.tracker.ChartableData(600f);
				this.sparkLayer.RefreshLine(data, this.diagnostic.name);
				this.sparkLayer.SetColor(color);
			}
			this.indicator.color = this.diagnostic.colors[this.diagnostic.LatestResult.opinion];
			this.tooltip.SetSimpleTooltip((this.diagnostic.LatestResult.Message.IsNullOrWhiteSpace() ? UI.COLONY_DIAGNOSTICS.GENERIC_STATUS_NORMAL.text : this.diagnostic.LatestResult.Message) + "\n\n" + UI.COLONY_DIAGNOSTICS.MUTE_TUTORIAL.text);
			ColonyDiagnostic.PresentationSetting presentationSetting = this.diagnostic.presentationSetting;
			if (presentationSetting == ColonyDiagnostic.PresentationSetting.AverageValue || presentationSetting != ColonyDiagnostic.PresentationSetting.CurrentValue)
			{
				this.valueLabel.SetText(this.diagnostic.GetAverageValueString());
			}
			else
			{
				this.valueLabel.SetText(this.diagnostic.GetCurrentValueString());
			}
			if (!string.IsNullOrEmpty(this.diagnostic.icon))
			{
				this.image.sprite = Assets.GetSprite(this.diagnostic.icon);
			}
			if (color == Constants.NEUTRAL_COLOR)
			{
				color = Color.white;
			}
			this.titleLabel.color = color;
		}

		// Token: 0x060099B1 RID: 39345 RVA: 0x001085E6 File Offset: 0x001067E6
		public bool CheckAllowVisualNotification()
		{
			return this.timeOfLastNotification == 0f || GameClock.Instance.GetTime() >= this.timeOfLastNotification + 300f;
		}

		// Token: 0x060099B2 RID: 39346 RVA: 0x003C4C44 File Offset: 0x003C2E44
		public void TriggerVisualNotification()
		{
			if (DebugHandler.NotificationsDisabled)
			{
				return;
			}
			if (this.activeRoutine == null)
			{
				this.timeOfLastNotification = GameClock.Instance.GetTime();
				KFMOD.PlayUISound(GlobalAssets.GetSound(ColonyDiagnosticScreen.notificationSoundsActive[this.currentDisplayedResult], false));
				this.activeRoutine = this.gameObject.GetComponent<KMonoBehaviour>().StartCoroutine(this.VisualNotificationRoutine());
			}
		}

		// Token: 0x060099B3 RID: 39347 RVA: 0x00108612 File Offset: 0x00106812
		private IEnumerator VisualNotificationRoutine()
		{
			this.gameObject.GetComponentInChildren<NotificationAnimator>().Begin(false);
			RectTransform indicator = this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform;
			this.defaultIndicatorSizeDelta = Vector2.zero;
			indicator.sizeDelta = this.defaultIndicatorSizeDelta;
			float bounceDuration = 3f;
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			for (float i = 0f; i < bounceDuration; i += Time.unscaledDeltaTime)
			{
				indicator.sizeDelta = this.defaultIndicatorSizeDelta + Vector2.one * (float)Mathf.RoundToInt(Mathf.Sin(6f * (3.1415927f * (i / bounceDuration))));
				yield return 0;
			}
			this.ResolveNotificationRoutine();
			yield break;
		}

		// Token: 0x060099B4 RID: 39348 RVA: 0x003C4CA8 File Offset: 0x003C2EA8
		public void ResolveNotificationRoutine()
		{
			this.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("Indicator").rectTransform.sizeDelta = Vector2.zero;
			this.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("Content").localPosition = Vector2.zero;
			this.activeRoutine = null;
		}

		// Token: 0x04007788 RID: 30600
		private const float displayHistoryPeriod = 600f;

		// Token: 0x04007789 RID: 30601
		public ColonyDiagnostic diagnostic;

		// Token: 0x0400778A RID: 30602
		public SparkLayer sparkLayer;

		// Token: 0x0400778C RID: 30604
		public int worldID;

		// Token: 0x0400778D RID: 30605
		private LocText titleLabel;

		// Token: 0x0400778E RID: 30606
		private LocText valueLabel;

		// Token: 0x0400778F RID: 30607
		private Image indicator;

		// Token: 0x04007790 RID: 30608
		private ToolTip tooltip;

		// Token: 0x04007791 RID: 30609
		private MultiToggle button;

		// Token: 0x04007792 RID: 30610
		private Image image;

		// Token: 0x04007793 RID: 30611
		public ColonyDiagnostic.DiagnosticResult.Opinion currentDisplayedResult;

		// Token: 0x04007794 RID: 30612
		private Vector2 defaultIndicatorSizeDelta;

		// Token: 0x04007795 RID: 30613
		private float timeOfLastNotification;

		// Token: 0x04007796 RID: 30614
		private const float MIN_TIME_BETWEEN_NOTIFICATIONS = 300f;

		// Token: 0x04007797 RID: 30615
		private Coroutine activeRoutine;
	}
}
