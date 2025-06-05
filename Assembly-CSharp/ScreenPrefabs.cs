using System;
using UnityEngine;

// Token: 0x020018A0 RID: 6304
[AddComponentMenu("KMonoBehaviour/scripts/ScreenPrefabs")]
public class ScreenPrefabs : KMonoBehaviour
{
	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x06008257 RID: 33367 RVA: 0x000FA400 File Offset: 0x000F8600
	// (set) Token: 0x06008258 RID: 33368 RVA: 0x000FA407 File Offset: 0x000F8607
	public static ScreenPrefabs Instance { get; private set; }

	// Token: 0x06008259 RID: 33369 RVA: 0x000FA40F File Offset: 0x000F860F
	protected override void OnPrefabInit()
	{
		ScreenPrefabs.Instance = this;
	}

	// Token: 0x0600825A RID: 33370 RVA: 0x0034A1E4 File Offset: 0x003483E4
	public void ConfirmDoAction(string message, System.Action action, Transform parent)
	{
		((ConfirmDialogScreen)KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, delegate
		{
		}, null, null, null, null, null, null);
	}

	// Token: 0x04006319 RID: 25369
	public ControlsScreen ControlsScreen;

	// Token: 0x0400631A RID: 25370
	public Hud HudScreen;

	// Token: 0x0400631B RID: 25371
	public HoverTextScreen HoverTextScreen;

	// Token: 0x0400631C RID: 25372
	public OverlayScreen OverlayScreen;

	// Token: 0x0400631D RID: 25373
	public TileScreen TileScreen;

	// Token: 0x0400631E RID: 25374
	public SpeedControlScreen SpeedControlScreen;

	// Token: 0x0400631F RID: 25375
	public ManagementMenu ManagementMenu;

	// Token: 0x04006320 RID: 25376
	public ToolTipScreen ToolTipScreen;

	// Token: 0x04006321 RID: 25377
	public DebugPaintElementScreen DebugPaintElementScreen;

	// Token: 0x04006322 RID: 25378
	public UserMenuScreen UserMenuScreen;

	// Token: 0x04006323 RID: 25379
	public KButtonMenu OwnerScreen;

	// Token: 0x04006324 RID: 25380
	public KButtonMenu ButtonGrid;

	// Token: 0x04006325 RID: 25381
	public NameDisplayScreen NameDisplayScreen;

	// Token: 0x04006326 RID: 25382
	public ConfirmDialogScreen ConfirmDialogScreen;

	// Token: 0x04006327 RID: 25383
	public CustomizableDialogScreen CustomizableDialogScreen;

	// Token: 0x04006328 RID: 25384
	public SpriteListDialogScreen SpriteListDialogScreen;

	// Token: 0x04006329 RID: 25385
	public InfoDialogScreen InfoDialogScreen;

	// Token: 0x0400632A RID: 25386
	public StoryMessageScreen StoryMessageScreen;

	// Token: 0x0400632B RID: 25387
	public SubSpeciesInfoScreen SubSpeciesInfoScreen;

	// Token: 0x0400632C RID: 25388
	public EventInfoScreen eventInfoScreen;

	// Token: 0x0400632D RID: 25389
	public FileNameDialog FileNameDialog;

	// Token: 0x0400632E RID: 25390
	public TagFilterScreen TagFilterScreen;

	// Token: 0x0400632F RID: 25391
	public ResearchScreen ResearchScreen;

	// Token: 0x04006330 RID: 25392
	public MessageDialogFrame MessageDialogFrame;

	// Token: 0x04006331 RID: 25393
	public ResourceCategoryScreen ResourceCategoryScreen;

	// Token: 0x04006332 RID: 25394
	public ColonyDiagnosticScreen ColonyDiagnosticScreen;

	// Token: 0x04006333 RID: 25395
	public LanguageOptionsScreen languageOptionsScreen;

	// Token: 0x04006334 RID: 25396
	public ModsScreen modsMenu;

	// Token: 0x04006335 RID: 25397
	public RailModUploadScreen RailModUploadMenu;

	// Token: 0x04006336 RID: 25398
	public GameObject GameOverScreen;

	// Token: 0x04006337 RID: 25399
	public GameObject VictoryScreen;

	// Token: 0x04006338 RID: 25400
	public GameObject StatusItemIndicatorScreen;

	// Token: 0x04006339 RID: 25401
	public GameObject CollapsableContentPanel;

	// Token: 0x0400633A RID: 25402
	public GameObject DescriptionLabel;

	// Token: 0x0400633B RID: 25403
	public LoadingOverlay loadingOverlay;

	// Token: 0x0400633C RID: 25404
	public LoadScreen LoadScreen;

	// Token: 0x0400633D RID: 25405
	public InspectSaveScreen InspectSaveScreen;

	// Token: 0x0400633E RID: 25406
	public OptionsMenuScreen OptionsScreen;

	// Token: 0x0400633F RID: 25407
	public WorldGenScreen WorldGenScreen;

	// Token: 0x04006340 RID: 25408
	public ModeSelectScreen ModeSelectScreen;

	// Token: 0x04006341 RID: 25409
	public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;

	// Token: 0x04006342 RID: 25410
	public RetiredColonyInfoScreen RetiredColonyInfoScreen;

	// Token: 0x04006343 RID: 25411
	public VideoScreen VideoScreen;

	// Token: 0x04006344 RID: 25412
	public ComicViewer ComicViewer;

	// Token: 0x04006345 RID: 25413
	public GameObject OldVersionWarningScreen;

	// Token: 0x04006346 RID: 25414
	public GameObject DLCBetaWarningScreen;

	// Token: 0x04006347 RID: 25415
	[Header("Klei Items")]
	public GameObject KleiItemDropScreen;

	// Token: 0x04006348 RID: 25416
	public GameObject LockerMenuScreen;

	// Token: 0x04006349 RID: 25417
	public GameObject LockerNavigator;

	// Token: 0x0400634A RID: 25418
	[Header("Main Menu")]
	public GameObject MainMenuForVanilla;

	// Token: 0x0400634B RID: 25419
	public GameObject MainMenuForSpacedOut;

	// Token: 0x0400634C RID: 25420
	public GameObject MainMenuIntroShort;

	// Token: 0x0400634D RID: 25421
	public GameObject MainMenuHealthyGameMessage;
}
