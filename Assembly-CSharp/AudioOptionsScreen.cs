using System;
using System.Collections.Generic;
using FMODUnity;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02001C58 RID: 7256
public class AudioOptionsScreen : KModalScreen
{
	// Token: 0x060096C0 RID: 38592 RVA: 0x003AE0DC File Offset: 0x003AC2DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.closeButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.doneButton.onClick += delegate()
		{
			this.OnClose(base.gameObject);
		};
		this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
		foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> keyValuePair in AudioMixer.instance.userVolumeSettings)
		{
			SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
			this.sliderBusMap.Add(newSlider.slider, keyValuePair.Key);
			newSlider.slider.value = keyValuePair.Value.busLevel;
			newSlider.nameLabel.text = keyValuePair.Value.labelString;
			newSlider.UpdateSliderLabel(keyValuePair.Value.busLevel);
			newSlider.slider.ClearReleaseHandleEvent();
			newSlider.slider.onValueChanged.AddListener(delegate(float value)
			{
				this.OnReleaseHandle(newSlider.slider);
			});
			if (keyValuePair.Key == "Master")
			{
				newSlider.transform.SetSiblingIndex(2);
				newSlider.slider.onValueChanged.AddListener(new UnityAction<float>(this.CheckMasterValue));
				this.CheckMasterValue(keyValuePair.Value.busLevel);
			}
		}
		HierarchyReferences component = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
		GameObject gameObject = component.GetReference("Button").gameObject;
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
		component.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayMusic();
		};
		component.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, 1);
		}
		HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
		GameObject gameObject2 = component2.GetReference("Button").gameObject;
		gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
		gameObject2.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleAlwaysPlayAutomation();
		};
		component2.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
		component2.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
		if (!KPlayerPrefs.HasKey(AudioOptionsScreen.MuteOnFocusLost))
		{
			KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, 0);
		}
		HierarchyReferences component3 = this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>();
		GameObject gameObject3 = component3.GetReference("Button").gameObject;
		gameObject3.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST_TOOLTIP);
		gameObject3.GetComponent<KButton>().onClick += delegate()
		{
			this.ToggleMuteOnFocusLost();
		};
		component3.GetReference<LocText>("Label").SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUTE_ON_FOCUS_LOST);
		component3.GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x060096C1 RID: 38593 RVA: 0x001069E4 File Offset: 0x00104BE4
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Deactivate();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x060096C2 RID: 38594 RVA: 0x00106A06 File Offset: 0x00104C06
	private void CheckMasterValue(float value)
	{
		this.jambell.enabled = (value == 0f);
	}

	// Token: 0x060096C3 RID: 38595 RVA: 0x00106A1B File Offset: 0x00104C1B
	private void OnReleaseHandle(KSlider slider)
	{
		AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
	}

	// Token: 0x060096C4 RID: 38596 RVA: 0x003AE454 File Offset: 0x003AC654
	private void ToggleAlwaysPlayMusic()
	{
		MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
		this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayMusicKey, MusicManager.instance.alwaysPlayMusic ? 1 : 0);
	}

	// Token: 0x060096C5 RID: 38597 RVA: 0x003AE4BC File Offset: 0x003AC6BC
	private void ToggleAlwaysPlayAutomation()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.AlwaysPlayAutomation, (KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1) ? 0 : 1);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) == 1);
	}

	// Token: 0x060096C6 RID: 38598 RVA: 0x003AE514 File Offset: 0x003AC714
	private void ToggleMuteOnFocusLost()
	{
		KPlayerPrefs.SetInt(AudioOptionsScreen.MuteOnFocusLost, (KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1) ? 0 : 1);
		this.muteOnFocusLostToggle.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(KPlayerPrefs.GetInt(AudioOptionsScreen.MuteOnFocusLost) == 1);
	}

	// Token: 0x060096C7 RID: 38599 RVA: 0x003AE56C File Offset: 0x003AC76C
	private void BuildAudioDeviceList()
	{
		this.audioDevices.Clear();
		this.audioDeviceOptions.Clear();
		int num;
		RuntimeManager.CoreSystem.getNumDrivers(out num);
		for (int i = 0; i < num; i++)
		{
			KFMOD.AudioDevice audioDevice = default(KFMOD.AudioDevice);
			string name;
			RuntimeManager.CoreSystem.getDriverInfo(i, out name, 64, out audioDevice.guid, out audioDevice.systemRate, out audioDevice.speakerMode, out audioDevice.speakerModeChannels);
			audioDevice.name = name;
			audioDevice.fmod_id = i;
			this.audioDevices.Add(audioDevice);
			this.audioDeviceOptions.Add(new Dropdown.OptionData(audioDevice.name));
		}
	}

	// Token: 0x060096C8 RID: 38600 RVA: 0x003AE618 File Offset: 0x003AC818
	private void OnAudioDeviceChanged(int idx)
	{
		RuntimeManager.CoreSystem.setDriver(idx);
		for (int i = 0; i < this.audioDevices.Count; i++)
		{
			if (idx == this.audioDevices[i].fmod_id)
			{
				KFMOD.currentDevice = this.audioDevices[i];
				KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
				return;
			}
		}
	}

	// Token: 0x060096C9 RID: 38601 RVA: 0x00106A39 File Offset: 0x00104C39
	private void OnClose(GameObject go)
	{
		this.alwaysPlayMusicMetric[AudioOptionsScreen.AlwaysPlayMusicKey] = MusicManager.instance.alwaysPlayMusic;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric, "AudioOptionsScreen");
		UnityEngine.Object.Destroy(go);
	}

	// Token: 0x04007530 RID: 30000
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007531 RID: 30001
	[SerializeField]
	private KButton doneButton;

	// Token: 0x04007532 RID: 30002
	[SerializeField]
	private SliderContainer sliderPrefab;

	// Token: 0x04007533 RID: 30003
	[SerializeField]
	private GameObject sliderGroup;

	// Token: 0x04007534 RID: 30004
	[SerializeField]
	private Image jambell;

	// Token: 0x04007535 RID: 30005
	[SerializeField]
	private GameObject alwaysPlayMusicButton;

	// Token: 0x04007536 RID: 30006
	[SerializeField]
	private GameObject alwaysPlayAutomationButton;

	// Token: 0x04007537 RID: 30007
	[SerializeField]
	private GameObject muteOnFocusLostToggle;

	// Token: 0x04007538 RID: 30008
	[SerializeField]
	private Dropdown deviceDropdown;

	// Token: 0x04007539 RID: 30009
	private UIPool<SliderContainer> sliderPool;

	// Token: 0x0400753A RID: 30010
	private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();

	// Token: 0x0400753B RID: 30011
	public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";

	// Token: 0x0400753C RID: 30012
	public static readonly string AlwaysPlayAutomation = "AlwaysPlayAutomation";

	// Token: 0x0400753D RID: 30013
	public static readonly string MuteOnFocusLost = "MuteOnFocusLost";

	// Token: 0x0400753E RID: 30014
	private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>
	{
		{
			AudioOptionsScreen.AlwaysPlayMusicKey,
			null
		}
	};

	// Token: 0x0400753F RID: 30015
	private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();

	// Token: 0x04007540 RID: 30016
	private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();
}
