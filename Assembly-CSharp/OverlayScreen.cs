using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02001B84 RID: 7044
[AddComponentMenu("KMonoBehaviour/scripts/OverlayScreen")]
public class OverlayScreen : KMonoBehaviour
{
	// Token: 0x170009A8 RID: 2472
	// (get) Token: 0x060093CC RID: 37836 RVA: 0x00104F4D File Offset: 0x0010314D
	public HashedString mode
	{
		get
		{
			return this.currentModeInfo.mode.ViewMode();
		}
	}

	// Token: 0x060093CD RID: 37837 RVA: 0x00104F5F File Offset: 0x0010315F
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(OverlayScreen.Instance == null);
		OverlayScreen.Instance = this;
		this.powerLabelParent = GameObject.Find("WorldSpaceCanvas").GetComponent<Canvas>();
	}

	// Token: 0x060093CE RID: 37838 RVA: 0x00104F8C File Offset: 0x0010318C
	protected override void OnLoadLevel()
	{
		this.harvestableNotificationPrefab = null;
		this.powerLabelParent = null;
		OverlayScreen.Instance = null;
		OverlayModes.Mode.Clear();
		this.modeInfos = null;
		this.currentModeInfo = default(OverlayScreen.ModeInfo);
		base.OnLoadLevel();
	}

	// Token: 0x060093CF RID: 37839 RVA: 0x0039B5F4 File Offset: 0x003997F4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.techViewSound = KFMOD.CreateInstance(this.techViewSoundPath);
		this.techViewSoundPlaying = false;
		Shader.SetGlobalVector("_OverlayParams", Vector4.zero);
		this.RegisterModes();
		this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
	}

	// Token: 0x060093D0 RID: 37840 RVA: 0x0039B64C File Offset: 0x0039984C
	private void RegisterModes()
	{
		this.modeInfos.Clear();
		OverlayModes.None mode = new OverlayModes.None();
		this.RegisterMode(mode);
		this.RegisterMode(new OverlayModes.Oxygen());
		this.RegisterMode(new OverlayModes.Power(this.powerLabelParent, this.powerLabelPrefab, this.batUIPrefab, this.powerLabelOffset, this.batteryUIOffset, this.batteryUITransformerOffset, this.batteryUISmallTransformerOffset));
		this.RegisterMode(new OverlayModes.Temperature());
		this.RegisterMode(new OverlayModes.ThermalConductivity());
		this.RegisterMode(new OverlayModes.Light());
		this.RegisterMode(new OverlayModes.LiquidConduits());
		this.RegisterMode(new OverlayModes.GasConduits());
		this.RegisterMode(new OverlayModes.Decor());
		this.RegisterMode(new OverlayModes.Disease(this.powerLabelParent, this.diseaseOverlayPrefab));
		this.RegisterMode(new OverlayModes.Crop(this.powerLabelParent, this.harvestableNotificationPrefab));
		this.RegisterMode(new OverlayModes.Harvest());
		this.RegisterMode(new OverlayModes.Priorities());
		this.RegisterMode(new OverlayModes.HeatFlow());
		this.RegisterMode(new OverlayModes.Rooms());
		this.RegisterMode(new OverlayModes.Suit(this.powerLabelParent, this.suitOverlayPrefab));
		this.RegisterMode(new OverlayModes.Logic(this.logicModeUIPrefab));
		this.RegisterMode(new OverlayModes.SolidConveyor());
		this.RegisterMode(new OverlayModes.TileMode());
		this.RegisterMode(new OverlayModes.Radiation());
	}

	// Token: 0x060093D1 RID: 37841 RVA: 0x0039B798 File Offset: 0x00399998
	private void RegisterMode(OverlayModes.Mode mode)
	{
		this.modeInfos[mode.ViewMode()] = new OverlayScreen.ModeInfo
		{
			mode = mode
		};
	}

	// Token: 0x060093D2 RID: 37842 RVA: 0x00104FC0 File Offset: 0x001031C0
	private void LateUpdate()
	{
		this.currentModeInfo.mode.Update();
	}

	// Token: 0x060093D3 RID: 37843 RVA: 0x00104FD2 File Offset: 0x001031D2
	public void RunPostProcessEffects(RenderTexture src, RenderTexture dest)
	{
		this.currentModeInfo.mode.OnRenderImage(src, dest);
	}

	// Token: 0x060093D4 RID: 37844 RVA: 0x0039B7C8 File Offset: 0x003999C8
	public void ToggleOverlay(HashedString newMode, bool allowSound = true)
	{
		bool flag = allowSound && !(this.currentModeInfo.mode.ViewMode() == newMode);
		if (newMode != OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		this.currentModeInfo.mode.Disable();
		if (newMode != this.currentModeInfo.mode.ViewMode() && newMode == OverlayModes.None.ID)
		{
			ManagementMenu.Instance.CloseAll();
		}
		SimDebugView.Instance.SetMode(newMode);
		if (!this.modeInfos.TryGetValue(newMode, out this.currentModeInfo))
		{
			this.currentModeInfo = this.modeInfos[OverlayModes.None.ID];
		}
		this.currentModeInfo.mode.Enable();
		if (flag)
		{
			this.UpdateOverlaySounds();
		}
		if (OverlayModes.None.ID == this.currentModeInfo.mode.ViewMode())
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().TechFilterOnMigrated, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicOverlayInactive();
			this.techViewSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.techViewSoundPlaying = false;
		}
		else if (!this.techViewSoundPlaying)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().TechFilterOnMigrated);
			MusicManager.instance.SetDynamicMusicOverlayActive();
			this.techViewSound.start();
			this.techViewSoundPlaying = true;
		}
		if (this.OnOverlayChanged != null)
		{
			this.OnOverlayChanged(this.currentModeInfo.mode.ViewMode());
		}
		this.ActivateLegend();
	}

	// Token: 0x060093D5 RID: 37845 RVA: 0x00104FE6 File Offset: 0x001031E6
	private void ActivateLegend()
	{
		if (OverlayLegend.Instance == null)
		{
			return;
		}
		OverlayLegend.Instance.SetLegend(this.currentModeInfo.mode, false);
	}

	// Token: 0x060093D6 RID: 37846 RVA: 0x0010500C File Offset: 0x0010320C
	public void Refresh()
	{
		this.LateUpdate();
	}

	// Token: 0x060093D7 RID: 37847 RVA: 0x00105014 File Offset: 0x00103214
	public HashedString GetMode()
	{
		if (this.currentModeInfo.mode == null)
		{
			return OverlayModes.None.ID;
		}
		return this.currentModeInfo.mode.ViewMode();
	}

	// Token: 0x060093D8 RID: 37848 RVA: 0x0039B950 File Offset: 0x00399B50
	private void UpdateOverlaySounds()
	{
		string text = this.currentModeInfo.mode.GetSoundName();
		if (text != "")
		{
			text = GlobalAssets.GetSound(text, false);
			KMonoBehaviour.PlaySound(text);
		}
	}

	// Token: 0x04007004 RID: 28676
	public static HashSet<Tag> WireIDs = new HashSet<Tag>();

	// Token: 0x04007005 RID: 28677
	public static HashSet<Tag> GasVentIDs = new HashSet<Tag>();

	// Token: 0x04007006 RID: 28678
	public static HashSet<Tag> LiquidVentIDs = new HashSet<Tag>();

	// Token: 0x04007007 RID: 28679
	public static HashSet<Tag> HarvestableIDs = new HashSet<Tag>();

	// Token: 0x04007008 RID: 28680
	public static HashSet<Tag> DiseaseIDs = new HashSet<Tag>();

	// Token: 0x04007009 RID: 28681
	public static HashSet<Tag> SuitIDs = new HashSet<Tag>();

	// Token: 0x0400700A RID: 28682
	public static HashSet<Tag> SolidConveyorIDs = new HashSet<Tag>();

	// Token: 0x0400700B RID: 28683
	public static HashSet<Tag> RadiationIDs = new HashSet<Tag>();

	// Token: 0x0400700C RID: 28684
	[SerializeField]
	public EventReference techViewSoundPath;

	// Token: 0x0400700D RID: 28685
	private EventInstance techViewSound;

	// Token: 0x0400700E RID: 28686
	private bool techViewSoundPlaying;

	// Token: 0x0400700F RID: 28687
	public static OverlayScreen Instance;

	// Token: 0x04007010 RID: 28688
	[Header("Power")]
	[SerializeField]
	private Canvas powerLabelParent;

	// Token: 0x04007011 RID: 28689
	[SerializeField]
	private LocText powerLabelPrefab;

	// Token: 0x04007012 RID: 28690
	[SerializeField]
	private BatteryUI batUIPrefab;

	// Token: 0x04007013 RID: 28691
	[SerializeField]
	private Vector3 powerLabelOffset;

	// Token: 0x04007014 RID: 28692
	[SerializeField]
	private Vector3 batteryUIOffset;

	// Token: 0x04007015 RID: 28693
	[SerializeField]
	private Vector3 batteryUITransformerOffset;

	// Token: 0x04007016 RID: 28694
	[SerializeField]
	private Vector3 batteryUISmallTransformerOffset;

	// Token: 0x04007017 RID: 28695
	[SerializeField]
	private Color consumerColour;

	// Token: 0x04007018 RID: 28696
	[SerializeField]
	private Color generatorColour;

	// Token: 0x04007019 RID: 28697
	[SerializeField]
	private Color buildingDisabledColour = Color.gray;

	// Token: 0x0400701A RID: 28698
	[Header("Circuits")]
	[SerializeField]
	private Color32 circuitUnpoweredColour;

	// Token: 0x0400701B RID: 28699
	[SerializeField]
	private Color32 circuitSafeColour;

	// Token: 0x0400701C RID: 28700
	[SerializeField]
	private Color32 circuitStrainingColour;

	// Token: 0x0400701D RID: 28701
	[SerializeField]
	private Color32 circuitOverloadingColour;

	// Token: 0x0400701E RID: 28702
	[Header("Crops")]
	[SerializeField]
	private GameObject harvestableNotificationPrefab;

	// Token: 0x0400701F RID: 28703
	[Header("Disease")]
	[SerializeField]
	private GameObject diseaseOverlayPrefab;

	// Token: 0x04007020 RID: 28704
	[Header("Suit")]
	[SerializeField]
	private GameObject suitOverlayPrefab;

	// Token: 0x04007021 RID: 28705
	[Header("ToolTip")]
	[SerializeField]
	private TextStyleSetting TooltipHeader;

	// Token: 0x04007022 RID: 28706
	[SerializeField]
	private TextStyleSetting TooltipDescription;

	// Token: 0x04007023 RID: 28707
	[Header("Logic")]
	[SerializeField]
	private LogicModeUI logicModeUIPrefab;

	// Token: 0x04007024 RID: 28708
	public Action<HashedString> OnOverlayChanged;

	// Token: 0x04007025 RID: 28709
	private OverlayScreen.ModeInfo currentModeInfo;

	// Token: 0x04007026 RID: 28710
	private Dictionary<HashedString, OverlayScreen.ModeInfo> modeInfos = new Dictionary<HashedString, OverlayScreen.ModeInfo>();

	// Token: 0x02001B85 RID: 7045
	private struct ModeInfo
	{
		// Token: 0x04007027 RID: 28711
		public OverlayModes.Mode mode;
	}
}
