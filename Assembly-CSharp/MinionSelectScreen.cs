using System;
using System.Collections;
using FMOD.Studio;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02001B42 RID: 6978
public class MinionSelectScreen : CharacterSelectionController
{
	// Token: 0x06009265 RID: 37477 RVA: 0x0039297C File Offset: 0x00390B7C
	protected override void OnPrefabInit()
	{
		base.IsStarterMinion = true;
		base.OnPrefabInit();
		if (MusicManager.instance.SongIsPlaying("Music_FrontEnd"))
		{
			MusicManager.instance.SetSongParameter("Music_FrontEnd", "songSection", 2f, true);
		}
		GameObject parent = GameObject.Find("ScreenSpaceOverlayCanvas");
		GameObject gameObject = global::Util.KInstantiateUI(this.wattsonMessagePrefab.gameObject, parent, false);
		gameObject.name = "WattsonMessage";
		gameObject.SetActive(false);
		Game.Instance.Subscribe(-1992507039, new Action<object>(this.OnBaseAlreadyCreated));
		this.backButton.onClick += delegate()
		{
			LoadScreen.ForceStopGame();
			App.LoadScene("frontend");
		};
		this.InitializeContainers();
		base.StartCoroutine(this.SetDefaultMinionsRoutine());
	}

	// Token: 0x06009266 RID: 37478 RVA: 0x00104437 File Offset: 0x00102637
	private IEnumerator SetDefaultMinionsRoutine()
	{
		yield return SequenceUtil.WaitForNextFrame;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ClusterLayout);
		if (SettingsCache.clusterLayouts.GetClusterData(currentQualitySetting.id).clusterTags.Contains("CeresCluster"))
		{
			((CharacterContainer)this.containers[2]).SetMinion(new MinionStartingStats(Db.Get().Personalities.Get("FREYJA"), null, null, false));
			((CharacterContainer)this.containers[1]).GenerateCharacter(true, null);
			((CharacterContainer)this.containers[0]).GenerateCharacter(true, null);
		}
		yield break;
	}

	// Token: 0x06009267 RID: 37479 RVA: 0x00392A48 File Offset: 0x00390C48
	public void SetProceedButtonActive(bool state, string tooltip = null)
	{
		if (state)
		{
			base.EnableProceedButton();
		}
		else
		{
			base.DisableProceedButton();
		}
		ToolTip component = this.proceedButton.GetComponent<ToolTip>();
		if (component != null)
		{
			if (tooltip != null)
			{
				component.toolTip = tooltip;
				return;
			}
			component.ClearMultiStringTooltip();
		}
	}

	// Token: 0x06009268 RID: 37480 RVA: 0x00392A8C File Offset: 0x00390C8C
	protected override void OnSpawn()
	{
		this.OnDeliverableAdded();
		base.EnableProceedButton();
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.EMBARK;
		this.containers.ForEach(delegate(ITelepadDeliverableContainer container)
		{
			CharacterContainer characterContainer = container as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.DisableSelectButton();
			}
		});
	}

	// Token: 0x06009269 RID: 37481 RVA: 0x00392AEC File Offset: 0x00390CEC
	protected override void OnProceed()
	{
		global::Util.KInstantiateUI(this.newBasePrefab.gameObject, GameScreenManager.Instance.ssOverlayCanvas, false);
		MusicManager.instance.StopSong("Music_FrontEnd", true, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().NewBaseSetupSnapshot);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWorldGenerationSnapshot, STOP_MODE.ALLOWFADEOUT);
		int num = 0;
		this.selectedDeliverables.Clear();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = (CharacterContainer)telepadDeliverableContainer;
			this.selectedDeliverables.Add(characterContainer.Stats);
			if (characterContainer.Stats.personality.model == BionicMinionConfig.MODEL)
			{
				num++;
			}
		}
		NewBaseScreen.Instance.Init(SaveLoader.Instance.Cluster, this.selectedDeliverables.ToArray());
		if (this.OnProceedEvent != null)
		{
			this.OnProceedEvent();
		}
		if (Game.IsDlcActiveForCurrentSave("DLC3_ID") && Components.RoleStations.Count > 0)
		{
			BuildingFacade component = Components.RoleStations[0].GetComponent<BuildingFacade>();
			bool flag = !component.IsOriginal;
			if (num == 3 || (!flag && num > 0))
			{
				component.ApplyBuildingFacade(Db.GetBuildingFacades().Get("permit_hqbase_cyberpunk"), false);
			}
		}
		Game.Instance.Trigger(-838649377, null);
		BuildWatermark.Instance.gameObject.SetActive(false);
		this.Deactivate();
	}

	// Token: 0x0600926A RID: 37482 RVA: 0x00104446 File Offset: 0x00102646
	private void OnBaseAlreadyCreated(object data)
	{
		Game.Instance.StopFE();
		Game.Instance.StartBE();
		Game.Instance.SetGameStarted();
		this.Deactivate();
	}

	// Token: 0x0600926B RID: 37483 RVA: 0x0010446C File Offset: 0x0010266C
	private void ReshuffleAll()
	{
		if (this.OnReshuffleEvent != null)
		{
			this.OnReshuffleEvent(base.IsStarterMinion);
		}
	}

	// Token: 0x0600926C RID: 37484 RVA: 0x00392C84 File Offset: 0x00390E84
	public override void OnPressBack()
	{
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.ForceStopEditingTitle();
			}
		}
	}

	// Token: 0x04006EF4 RID: 28404
	[SerializeField]
	private NewBaseScreen newBasePrefab;

	// Token: 0x04006EF5 RID: 28405
	[SerializeField]
	private WattsonMessage wattsonMessagePrefab;

	// Token: 0x04006EF6 RID: 28406
	public const string WattsonGameObjName = "WattsonMessage";

	// Token: 0x04006EF7 RID: 28407
	public KButton backButton;
}
