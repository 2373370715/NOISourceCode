using System;
using System.Collections.Generic;
using System.Threading;
using Klei.CustomSettings;
using ProcGenGame;
using STRINGS;
using TMPro;
using UnityEngine;

// Token: 0x020020E1 RID: 8417
[AddComponentMenu("KMonoBehaviour/scripts/OfflineWorldGen")]
public class OfflineWorldGen : KMonoBehaviour
{
	// Token: 0x0600B366 RID: 45926 RVA: 0x0011925F File Offset: 0x0011745F
	private void TrackProgress(string text)
	{
		if (this.trackProgress)
		{
			global::Debug.Log(text);
		}
	}

	// Token: 0x0600B367 RID: 45927 RVA: 0x00442020 File Offset: 0x00440220
	public static bool CanLoadSave()
	{
		bool flag = WorldGen.CanLoad(SaveLoader.GetActiveSaveFilePath());
		if (!flag)
		{
			SaveLoader.SetActiveSaveFilePath(null);
			flag = WorldGen.CanLoad(WorldGen.WORLDGEN_SAVE_FILENAME);
		}
		return flag;
	}

	// Token: 0x0600B368 RID: 45928 RVA: 0x00442050 File Offset: 0x00440250
	public void Generate()
	{
		this.doWorldGen = !OfflineWorldGen.CanLoadSave();
		this.updateText.gameObject.SetActive(false);
		this.percentText.gameObject.SetActive(false);
		this.doWorldGen |= this.debug;
		if (this.doWorldGen)
		{
			this.seedText.text = string.Format(UI.WORLDGEN.USING_PLAYER_SEED, this.seed);
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.TITLE.ToString();
			this.mainText.text = UI.WORLDGEN.CHOOSEWORLDSIZE.ToString();
			for (int i = 0; i < this.validDimensions.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
				gameObject.SetActive(true);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.SetParent(this.buttonRoot);
				component.localScale = Vector3.one;
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<LocText>();
				OfflineWorldGen.ValidDimensions validDimensions = this.validDimensions[i];
				componentInChildren.text = validDimensions.name.ToString();
				int idx = i;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.DoWorldGen(idx);
					this.ToggleGenerationUI();
				};
			}
			if (this.validDimensions.Length == 1)
			{
				this.DoWorldGen(0);
				this.ToggleGenerationUI();
			}
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
			this.OnResize();
		}
		else
		{
			this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.LOADINGGAME.ToString();
			this.mainText.gameObject.SetActive(false);
			this.currentConvertedCurrentStage = UI.WORLDGEN.COMPLETE.key;
			this.currentPercent = 1f;
			this.updateText.gameObject.SetActive(false);
			this.percentText.gameObject.SetActive(false);
			this.RemoveButtons();
		}
		this.buttonPrefab.SetActive(false);
	}

	// Token: 0x0600B369 RID: 45929 RVA: 0x00442250 File Offset: 0x00440450
	private void OnResize()
	{
		float canvasScale = base.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
		if (this.asteriodAnim != null)
		{
			this.asteriodAnim.animScale = 0.005f * (1f / canvasScale);
		}
	}

	// Token: 0x0600B36A RID: 45930 RVA: 0x00442298 File Offset: 0x00440498
	private void ToggleGenerationUI()
	{
		this.percentText.gameObject.SetActive(false);
		this.updateText.gameObject.SetActive(true);
		this.titleText.text = UI.FRONTEND.WORLDGENSCREEN.GENERATINGWORLD.ToString();
		if (this.titleText != null && this.titleText.gameObject != null)
		{
			this.titleText.gameObject.SetActive(false);
		}
		if (this.buttonRoot != null && this.buttonRoot.gameObject != null)
		{
			this.buttonRoot.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600B36B RID: 45931 RVA: 0x00442340 File Offset: 0x00440540
	private bool UpdateProgress(StringKey stringKeyRoot, float completePercent, WorldGenProgressStages.Stages stage)
	{
		if (this.currentStage != stage)
		{
			this.currentStage = stage;
		}
		if (this.currentStringKeyRoot.Hash != stringKeyRoot.Hash)
		{
			this.currentConvertedCurrentStage = stringKeyRoot;
			this.currentStringKeyRoot = stringKeyRoot;
		}
		else
		{
			int num = (int)completePercent * 10;
			LocString locString = this.convertList.Find((LocString s) => s.key.Hash == stringKeyRoot.Hash);
			if (num != 0 && locString != null)
			{
				this.currentConvertedCurrentStage = new StringKey(locString.key.String + num.ToString());
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = WorldGenProgressStages.StageWeights[(int)stage].Value * completePercent;
		for (int i = 0; i < WorldGenProgressStages.StageWeights.Length; i++)
		{
			num3 += WorldGenProgressStages.StageWeights[i].Value;
			if (i < (int)this.currentStage)
			{
				num2 += WorldGenProgressStages.StageWeights[i].Value;
			}
		}
		float num5 = (num2 + num4) / num3;
		this.currentPercent = num5;
		return !this.shouldStop;
	}

	// Token: 0x0600B36C RID: 45932 RVA: 0x00442468 File Offset: 0x00440668
	private void Update()
	{
		if (this.loadTriggered)
		{
			return;
		}
		if (this.currentConvertedCurrentStage.String == null)
		{
			return;
		}
		this.errorMutex.WaitOne();
		int count = this.errors.Count;
		this.errorMutex.ReleaseMutex();
		if (count > 0)
		{
			this.DoExitFlow();
			return;
		}
		this.updateText.text = Strings.Get(this.currentConvertedCurrentStage.String);
		if (!this.debug && this.currentConvertedCurrentStage.Hash == UI.WORLDGEN.COMPLETE.key.Hash && this.currentPercent >= 1f && this.cluster.IsGenerationComplete)
		{
			if (KCrashReporter.terminateOnError && KCrashReporter.hasCrash)
			{
				return;
			}
			this.percentText.text = "";
			this.loadTriggered = true;
			App.LoadScene(this.mainGameLevel);
			return;
		}
		else
		{
			if (this.currentPercent < 0f)
			{
				this.DoExitFlow();
				return;
			}
			if (this.currentPercent > 0f && !this.percentText.gameObject.activeSelf)
			{
				this.percentText.gameObject.SetActive(false);
			}
			this.percentText.text = GameUtil.GetFormattedPercent(this.currentPercent * 100f, GameUtil.TimeSlice.None);
			this.meterAnim.SetPositionPercent(this.currentPercent);
			return;
		}
	}

	// Token: 0x0600B36D RID: 45933 RVA: 0x004425BC File Offset: 0x004407BC
	private void DisplayErrors()
	{
		this.errorMutex.WaitOne();
		if (this.errors.Count > 0)
		{
			foreach (OfflineWorldGen.ErrorInfo errorInfo in this.errors)
			{
				Util.KInstantiateUI<ConfirmDialogScreen>(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, FrontEndManager.Instance.gameObject, true).PopupConfirmDialog(errorInfo.errorDesc, new System.Action(this.OnConfirmExit), null, null, null, null, null, null, null);
			}
		}
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x0600B36E RID: 45934 RVA: 0x0011926F File Offset: 0x0011746F
	private void DoExitFlow()
	{
		if (this.startedExitFlow)
		{
			return;
		}
		this.startedExitFlow = true;
		this.percentText.text = UI.WORLDGEN.RESTARTING.ToString();
		this.loadTriggered = true;
		Sim.Shutdown();
		this.DisplayErrors();
	}

	// Token: 0x0600B36F RID: 45935 RVA: 0x001192A8 File Offset: 0x001174A8
	private void OnConfirmExit()
	{
		App.LoadScene(this.frontendGameLevel);
	}

	// Token: 0x0600B370 RID: 45936 RVA: 0x0044266C File Offset: 0x0044086C
	private void RemoveButtons()
	{
		for (int i = this.buttonRoot.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.buttonRoot.GetChild(i).gameObject);
		}
	}

	// Token: 0x0600B371 RID: 45937 RVA: 0x001192B5 File Offset: 0x001174B5
	private void DoWorldGen(int selectedDimension)
	{
		this.RemoveButtons();
		this.DoWorldGenInitialize();
	}

	// Token: 0x0600B372 RID: 45938 RVA: 0x004426A8 File Offset: 0x004408A8
	private void DoWorldGenInitialize()
	{
		string clusterName = "";
		Func<int, WorldGen, bool> shouldSkipWorldCallback = null;
		this.seed = CustomGameSettings.Instance.GetCurrentWorldgenSeed();
		clusterName = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout).id;
		List<string> list = new List<string>();
		foreach (string id in CustomGameSettings.Instance.GetCurrentStories())
		{
			list.Add(Db.Get().Stories.Get(id).worldgenStoryTraitKey);
		}
		this.cluster = new Cluster(clusterName, this.seed, list, true, false, false);
		this.cluster.ShouldSkipWorldCallback = shouldSkipWorldCallback;
		this.cluster.Generate(new WorldGen.OfflineCallbackFunction(this.UpdateProgress), new Action<OfflineWorldGen.ErrorInfo>(this.OnError), this.seed, this.seed, this.seed, this.seed, true, false, false);
	}

	// Token: 0x0600B373 RID: 45939 RVA: 0x001192C3 File Offset: 0x001174C3
	private void OnError(OfflineWorldGen.ErrorInfo error)
	{
		this.errorMutex.WaitOne();
		this.errors.Add(error);
		this.errorMutex.ReleaseMutex();
	}

	// Token: 0x04008DF5 RID: 36341
	[SerializeField]
	private RectTransform buttonRoot;

	// Token: 0x04008DF6 RID: 36342
	[SerializeField]
	private GameObject buttonPrefab;

	// Token: 0x04008DF7 RID: 36343
	[SerializeField]
	private RectTransform chooseLocationPanel;

	// Token: 0x04008DF8 RID: 36344
	[SerializeField]
	private GameObject locationButtonPrefab;

	// Token: 0x04008DF9 RID: 36345
	private const float baseScale = 0.005f;

	// Token: 0x04008DFA RID: 36346
	private Mutex errorMutex = new Mutex();

	// Token: 0x04008DFB RID: 36347
	private List<OfflineWorldGen.ErrorInfo> errors = new List<OfflineWorldGen.ErrorInfo>();

	// Token: 0x04008DFC RID: 36348
	private OfflineWorldGen.ValidDimensions[] validDimensions = new OfflineWorldGen.ValidDimensions[]
	{
		new OfflineWorldGen.ValidDimensions
		{
			width = 256,
			height = 384,
			name = UI.FRONTEND.WORLDGENSCREEN.SIZES.STANDARD.key
		}
	};

	// Token: 0x04008DFD RID: 36349
	public string frontendGameLevel = "frontend";

	// Token: 0x04008DFE RID: 36350
	public string mainGameLevel = "backend";

	// Token: 0x04008DFF RID: 36351
	private bool shouldStop;

	// Token: 0x04008E00 RID: 36352
	private StringKey currentConvertedCurrentStage;

	// Token: 0x04008E01 RID: 36353
	private float currentPercent;

	// Token: 0x04008E02 RID: 36354
	public bool debug;

	// Token: 0x04008E03 RID: 36355
	private bool trackProgress = true;

	// Token: 0x04008E04 RID: 36356
	private bool doWorldGen;

	// Token: 0x04008E05 RID: 36357
	[SerializeField]
	private LocText titleText;

	// Token: 0x04008E06 RID: 36358
	[SerializeField]
	private LocText mainText;

	// Token: 0x04008E07 RID: 36359
	[SerializeField]
	private LocText updateText;

	// Token: 0x04008E08 RID: 36360
	[SerializeField]
	private LocText percentText;

	// Token: 0x04008E09 RID: 36361
	[SerializeField]
	private LocText seedText;

	// Token: 0x04008E0A RID: 36362
	[SerializeField]
	private KBatchedAnimController meterAnim;

	// Token: 0x04008E0B RID: 36363
	[SerializeField]
	private KBatchedAnimController asteriodAnim;

	// Token: 0x04008E0C RID: 36364
	private Cluster cluster;

	// Token: 0x04008E0D RID: 36365
	private StringKey currentStringKeyRoot;

	// Token: 0x04008E0E RID: 36366
	private List<LocString> convertList = new List<LocString>
	{
		UI.WORLDGEN.SETTLESIM,
		UI.WORLDGEN.BORDERS,
		UI.WORLDGEN.PROCESSING,
		UI.WORLDGEN.COMPLETELAYOUT,
		UI.WORLDGEN.WORLDLAYOUT,
		UI.WORLDGEN.GENERATENOISE,
		UI.WORLDGEN.BUILDNOISESOURCE,
		UI.WORLDGEN.GENERATESOLARSYSTEM
	};

	// Token: 0x04008E0F RID: 36367
	private WorldGenProgressStages.Stages currentStage;

	// Token: 0x04008E10 RID: 36368
	private bool loadTriggered;

	// Token: 0x04008E11 RID: 36369
	private bool startedExitFlow;

	// Token: 0x04008E12 RID: 36370
	private int seed;

	// Token: 0x020020E2 RID: 8418
	public struct ErrorInfo
	{
		// Token: 0x04008E13 RID: 36371
		public string errorDesc;

		// Token: 0x04008E14 RID: 36372
		public Exception exception;
	}

	// Token: 0x020020E3 RID: 8419
	[Serializable]
	private struct ValidDimensions
	{
		// Token: 0x04008E15 RID: 36373
		public int width;

		// Token: 0x04008E16 RID: 36374
		public int height;

		// Token: 0x04008E17 RID: 36375
		public StringKey name;
	}
}
