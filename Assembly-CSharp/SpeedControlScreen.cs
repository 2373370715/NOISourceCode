using System;
using System.Collections;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02002075 RID: 8309
public class SpeedControlScreen : KScreen
{
	// Token: 0x17000B54 RID: 2900
	// (get) Token: 0x0600B0D9 RID: 45273 RVA: 0x001178B3 File Offset: 0x00115AB3
	// (set) Token: 0x0600B0DA RID: 45274 RVA: 0x001178BA File Offset: 0x00115ABA
	public static SpeedControlScreen Instance { get; private set; }

	// Token: 0x0600B0DB RID: 45275 RVA: 0x001178C2 File Offset: 0x00115AC2
	public static void DestroyInstance()
	{
		SpeedControlScreen.Instance = null;
	}

	// Token: 0x17000B55 RID: 2901
	// (get) Token: 0x0600B0DC RID: 45276 RVA: 0x001178CA File Offset: 0x00115ACA
	public bool IsPaused
	{
		get
		{
			return this.pauseCount > 0;
		}
	}

	// Token: 0x0600B0DD RID: 45277 RVA: 0x00434360 File Offset: 0x00432560
	protected override void OnPrefabInit()
	{
		SpeedControlScreen.Instance = this;
		this.pauseButton = this.pauseButtonWidget.GetComponent<KToggle>();
		this.slowButton = this.speedButtonWidget_slow.GetComponent<KToggle>();
		this.mediumButton = this.speedButtonWidget_medium.GetComponent<KToggle>();
		this.fastButton = this.speedButtonWidget_fast.GetComponent<KToggle>();
		KToggle[] array = new KToggle[]
		{
			this.pauseButton,
			this.slowButton,
			this.mediumButton,
			this.fastButton
		};
		for (int i = 0; i < array.Length; i++)
		{
			array[i].soundPlayer.Enabled = false;
		}
		this.slowButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(1f);
			this.SetSpeed(0);
		};
		this.mediumButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(2f);
			this.SetSpeed(1);
		};
		this.fastButton.onClick += delegate()
		{
			this.PlaySpeedChangeSound(3f);
			this.SetSpeed(2);
		};
		this.pauseButton.onClick += delegate()
		{
			this.TogglePause(true);
		};
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.playButtonWidget.GetComponent<KButton>().onClick += delegate()
		{
			this.TogglePause(true);
		};
		KInputManager.InputChange.AddListener(new UnityAction(this.ResetToolTip));
	}

	// Token: 0x0600B0DE RID: 45278 RVA: 0x001178D5 File Offset: 0x00115AD5
	protected override void OnSpawn()
	{
		if (SaveGame.Instance != null)
		{
			this.speed = SaveGame.Instance.GetSpeed();
			this.SetSpeed(this.speed);
		}
		base.OnSpawn();
		this.OnChanged();
	}

	// Token: 0x0600B0DF RID: 45279 RVA: 0x0011790C File Offset: 0x00115B0C
	protected override void OnForcedCleanUp()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.ResetToolTip));
		base.OnForcedCleanUp();
	}

	// Token: 0x0600B0E0 RID: 45280 RVA: 0x0011792A File Offset: 0x00115B2A
	public int GetSpeed()
	{
		return this.speed;
	}

	// Token: 0x0600B0E1 RID: 45281 RVA: 0x00434504 File Offset: 0x00432704
	public void SetSpeed(int Speed)
	{
		this.speed = Speed % 3;
		switch (this.speed)
		{
		case 0:
			this.slowButton.Select();
			this.slowButton.isOn = true;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = false;
			break;
		case 1:
			this.mediumButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = true;
			this.fastButton.isOn = false;
			break;
		case 2:
			this.fastButton.Select();
			this.slowButton.isOn = false;
			this.mediumButton.isOn = false;
			this.fastButton.isOn = true;
			break;
		}
		this.OnSpeedChange();
	}

	// Token: 0x0600B0E2 RID: 45282 RVA: 0x00117932 File Offset: 0x00115B32
	public void ToggleRidiculousSpeed()
	{
		if (this.ultraSpeed == 3f)
		{
			this.ultraSpeed = 10f;
		}
		else
		{
			this.ultraSpeed = 3f;
		}
		this.speed = 2;
		this.OnChanged();
	}

	// Token: 0x0600B0E3 RID: 45283 RVA: 0x00117966 File Offset: 0x00115B66
	public void TogglePause(bool playsound = true)
	{
		if (this.IsPaused)
		{
			this.Unpause(playsound);
			return;
		}
		this.Pause(playsound, false);
	}

	// Token: 0x0600B0E4 RID: 45284 RVA: 0x004345D0 File Offset: 0x004327D0
	public void ResetToolTip()
	{
		this.speedButtonWidget_slow.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_medium.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_fast.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.speedButtonWidget_slow.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_SLOW, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_medium.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_MEDIUM, global::Action.CycleSpeed), this.TooltipTextStyle);
		this.speedButtonWidget_fast.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.SPEEDBUTTON_FAST, global::Action.CycleSpeed), this.TooltipTextStyle);
		if (this.pauseButton.isOn)
		{
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			return;
		}
		this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
		this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
	}

	// Token: 0x0600B0E5 RID: 45285 RVA: 0x00434700 File Offset: 0x00432900
	public void Pause(bool playSound = true, bool isCrashed = false)
	{
		this.pauseCount++;
		if (this.pauseCount == 1)
		{
			if (playSound)
			{
				if (isCrashed)
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Crash_Screen", false));
				}
				else
				{
					KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Pause", false));
				}
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(0f);
				}
			}
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().SpeedPausedMigrated);
			MusicManager.instance.SetDynamicMusicPaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.UNPAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = true;
			this.OnPause();
		}
	}

	// Token: 0x0600B0E6 RID: 45286 RVA: 0x004347D4 File Offset: 0x004329D4
	public void Unpause(bool playSound = true)
	{
		this.pauseCount = Mathf.Max(0, this.pauseCount - 1);
		if (this.pauseCount == 0)
		{
			if (playSound)
			{
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Speed_Unpause", false));
				if (SoundListenerController.Instance != null)
				{
					SoundListenerController.Instance.SetLoopingVolume(1f);
				}
			}
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().SpeedPausedMigrated, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.SetDynamicMusicUnpaused();
			this.pauseButtonWidget.GetComponent<ToolTip>().ClearMultiStringTooltip();
			this.pauseButtonWidget.GetComponent<ToolTip>().AddMultiStringTooltip(GameUtil.ReplaceHotkeyString(UI.TOOLTIPS.PAUSE, global::Action.TogglePause), this.TooltipTextStyle);
			this.pauseButton.isOn = false;
			this.SetSpeed(this.speed);
			this.OnPlay();
		}
	}

	// Token: 0x0600B0E7 RID: 45287 RVA: 0x00117980 File Offset: 0x00115B80
	private void OnPause()
	{
		this.OnChanged();
	}

	// Token: 0x0600B0E8 RID: 45288 RVA: 0x00117980 File Offset: 0x00115B80
	private void OnPlay()
	{
		this.OnChanged();
	}

	// Token: 0x0600B0E9 RID: 45289 RVA: 0x00117988 File Offset: 0x00115B88
	public void OnSpeedChange()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		this.OnChanged();
	}

	// Token: 0x0600B0EA RID: 45290 RVA: 0x004348A4 File Offset: 0x00432AA4
	private void OnChanged()
	{
		if (this.IsPaused)
		{
			Time.timeScale = 0f;
			return;
		}
		if (this.speed == 0)
		{
			Time.timeScale = this.normalSpeed;
			return;
		}
		if (this.speed == 1)
		{
			Time.timeScale = this.fastSpeed;
			return;
		}
		if (this.speed == 2)
		{
			Time.timeScale = this.ultraSpeed;
		}
	}

	// Token: 0x0600B0EB RID: 45291 RVA: 0x00434904 File Offset: 0x00432B04
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.TogglePause))
		{
			this.TogglePause(true);
			return;
		}
		if (e.TryConsume(global::Action.CycleSpeed))
		{
			this.PlaySpeedChangeSound((float)((this.speed + 1) % 3 + 1));
			this.SetSpeed(this.speed + 1);
			this.OnSpeedChange();
			return;
		}
		if (e.TryConsume(global::Action.SpeedUp))
		{
			this.speed++;
			this.speed = Math.Min(this.speed, 2);
			this.SetSpeed(this.speed);
			return;
		}
		if (e.TryConsume(global::Action.SlowDown))
		{
			this.speed--;
			this.speed = Math.Max(this.speed, 0);
			this.SetSpeed(this.speed);
		}
	}

	// Token: 0x0600B0EC RID: 45292 RVA: 0x004349C4 File Offset: 0x00432BC4
	private void PlaySpeedChangeSound(float speed)
	{
		string sound = GlobalAssets.GetSound("Speed_Change", false);
		if (sound != null)
		{
			EventInstance instance = SoundEvent.BeginOneShot(sound, Vector3.zero, 1f, false);
			instance.setParameterByName("Speed", speed, false);
			SoundEvent.EndOneShot(instance);
		}
	}

	// Token: 0x0600B0ED RID: 45293 RVA: 0x00434A0C File Offset: 0x00432C0C
	public void DebugStepFrame()
	{
		DebugUtil.LogArgs(new object[]
		{
			string.Format("Stepping one frame {0} ({1})", GameClock.Instance.GetTime(), GameClock.Instance.GetTime() / 600f)
		});
		this.stepTime = Time.time;
		this.Unpause(false);
		base.StartCoroutine(this.DebugStepFrameDelay());
	}

	// Token: 0x0600B0EE RID: 45294 RVA: 0x00117998 File Offset: 0x00115B98
	private IEnumerator DebugStepFrameDelay()
	{
		yield return null;
		DebugUtil.LogArgs(new object[]
		{
			"Stepped one frame",
			Time.time - this.stepTime,
			"seconds"
		});
		this.Pause(false, false);
		yield break;
	}

	// Token: 0x04008B4C RID: 35660
	public GameObject playButtonWidget;

	// Token: 0x04008B4D RID: 35661
	public GameObject pauseButtonWidget;

	// Token: 0x04008B4E RID: 35662
	public Image playIcon;

	// Token: 0x04008B4F RID: 35663
	public Image pauseIcon;

	// Token: 0x04008B50 RID: 35664
	[SerializeField]
	private TextStyleSetting TooltipTextStyle;

	// Token: 0x04008B51 RID: 35665
	public GameObject speedButtonWidget_slow;

	// Token: 0x04008B52 RID: 35666
	public GameObject speedButtonWidget_medium;

	// Token: 0x04008B53 RID: 35667
	public GameObject speedButtonWidget_fast;

	// Token: 0x04008B54 RID: 35668
	public GameObject mainMenuWidget;

	// Token: 0x04008B55 RID: 35669
	public float normalSpeed;

	// Token: 0x04008B56 RID: 35670
	public float fastSpeed;

	// Token: 0x04008B57 RID: 35671
	public float ultraSpeed;

	// Token: 0x04008B58 RID: 35672
	private KToggle pauseButton;

	// Token: 0x04008B59 RID: 35673
	private KToggle slowButton;

	// Token: 0x04008B5A RID: 35674
	private KToggle mediumButton;

	// Token: 0x04008B5B RID: 35675
	private KToggle fastButton;

	// Token: 0x04008B5C RID: 35676
	private int speed;

	// Token: 0x04008B5D RID: 35677
	private int pauseCount;

	// Token: 0x04008B5F RID: 35679
	private float stepTime;
}
