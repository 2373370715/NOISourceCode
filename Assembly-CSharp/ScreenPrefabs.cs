﻿using System;
using UnityEngine;

[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	public static ScreenPrefabs Instance { get; private set; }

	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	public ControlsScreen ControlsScreen;

	public Hud HudScreen;

	public HoverTextScreen HoverTextScreen;

	public OverlayScreen OverlayScreen;

	public TileScreen TileScreen;

	public SpeedControlScreen SpeedControlScreen;

	public ManagementMenu ManagementMenu;

	public ToolTipScreen ToolTipScreen;

	public DebugPaintElementScreen DebugPaintElementScreen;

	public UserMenuScreen UserMenuScreen;

	public KButtonMenu OwnerScreen;

	public KButtonMenu ButtonGrid;

	public NameDisplayScreen NameDisplayScreen;

	public ConfirmDialogScreen ConfirmDialogScreen;

	public CustomizableDialogScreen CustomizableDialogScreen;

	public SpriteListDialogScreen SpriteListDialogScreen;

	public InfoDialogScreen InfoDialogScreen;

	public StoryMessageScreen StoryMessageScreen;

	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	public EventInfoScreen eventInfoScreen;

	public FileNameDialog FileNameDialog;

	public TagFilterScreen TagFilterScreen;

	public ResearchScreen ResearchScreen;

	public MessageDialogFrame MessageDialogFrame;

	public ResourceCategoryScreen ResourceCategoryScreen;

	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	public LanguageOptionsScreen languageOptionsScreen;

	public LargeImpactorSequenceUIReticle largeImpactorSequenceReticlePrefab;

	public ModsScreen modsMenu;

	public RailModUploadScreen RailModUploadMenu;

	public GameObject GameOverScreen;

	public GameObject VictoryScreen;

	public GameObject StatusItemIndicatorScreen;

	public GameObject CollapsableContentPanel;

	public GameObject DescriptionLabel;

	public LoadingOverlay loadingOverlay;

	public LoadScreen LoadScreen;

	public InspectSaveScreen InspectSaveScreen;

	public OptionsMenuScreen OptionsScreen;

	public WorldGenScreen WorldGenScreen;

	public ModeSelectScreen ModeSelectScreen;

	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	public VideoScreen VideoScreen;

	public ComicViewer ComicViewer;

	public GameObject OldVersionWarningScreen;

	public GameObject DLCBetaWarningScreen;

	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	public GameObject LockerMenuScreen;

	public GameObject LockerNavigator;

	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	public GameObject MainMenuForSpacedOut;

	public GameObject MainMenuIntroShort;

	public GameObject MainMenuHealthyGameMessage;
}
