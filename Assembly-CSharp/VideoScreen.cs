using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x020020B4 RID: 8372
public class VideoScreen : KModalScreen
{
	// Token: 0x0600B285 RID: 45701 RVA: 0x0043D860 File Offset: 0x0043BA60
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.ConsumeMouseScroll = true;
		this.closeButton.onClick += delegate()
		{
			this.Stop();
		};
		this.proceedButton.onClick += delegate()
		{
			this.Stop();
		};
		this.videoPlayer.isLooping = false;
		this.videoPlayer.loopPointReached += delegate(VideoPlayer data)
		{
			if (this.victoryLoopQueued)
			{
				base.StartCoroutine(this.SwitchToVictoryLoop());
				return;
			}
			if (!this.videoPlayer.isLooping)
			{
				this.Stop();
			}
		};
		VideoScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x0600B286 RID: 45702 RVA: 0x00118939 File Offset: 0x00116B39
	protected override void OnForcedCleanUp()
	{
		VideoScreen.Instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600B287 RID: 45703 RVA: 0x00118947 File Offset: 0x00116B47
	protected override void OnShow(bool show)
	{
		base.transform.SetAsLastSibling();
		base.OnShow(show);
		this.screen = this.videoPlayer.gameObject.GetComponent<RawImage>();
	}

	// Token: 0x0600B288 RID: 45704 RVA: 0x00118971 File Offset: 0x00116B71
	public void DisableAllMedia()
	{
		this.overlayContainer.gameObject.SetActive(false);
		this.videoPlayer.gameObject.SetActive(false);
		this.slideshow.gameObject.SetActive(false);
	}

	// Token: 0x0600B289 RID: 45705 RVA: 0x0043D8D8 File Offset: 0x0043BAD8
	public void PlaySlideShow(Sprite[] sprites)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.preloadedSprites;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetSprites(sprites);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x0600B28A RID: 45706 RVA: 0x0043D928 File Offset: 0x0043BB28
	public void PlaySlideShow(string[] files)
	{
		this.Show(true);
		this.DisableAllMedia();
		this.slideshow.updateType = SlideshowUpdateType.loadOnDemand;
		this.slideshow.gameObject.SetActive(true);
		this.slideshow.SetFiles(files, 0);
		this.slideshow.SetPaused(false);
	}

	// Token: 0x0600B28B RID: 45707 RVA: 0x0043D978 File Offset: 0x0043BB78
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.IsAction(global::Action.Escape))
		{
			if (this.slideshow.gameObject.activeSelf && e.TryConsume(global::Action.Escape))
			{
				this.Stop();
				return;
			}
			if (e.TryConsume(global::Action.Escape))
			{
				if (this.videoSkippable)
				{
					this.Stop();
				}
				return;
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x0600B28C RID: 45708 RVA: 0x0043D9D0 File Offset: 0x0043BBD0
	public void PlayVideo(VideoClip clip, bool unskippable = false, EventReference overrideAudioSnapshot = default(EventReference), bool showProceedButton = false, bool syncAudio = true)
	{
		global::Debug.Assert(clip != null);
		for (int i = 0; i < this.overlayContainer.childCount; i++)
		{
			UnityEngine.Object.Destroy(this.overlayContainer.GetChild(i).gameObject);
		}
		this.Show(true);
		this.videoPlayer.isLooping = false;
		this.activeAudioSnapshot = (overrideAudioSnapshot.IsNull ? AudioMixerSnapshots.Get().TutorialVideoPlayingSnapshot : overrideAudioSnapshot);
		AudioMixer.instance.Start(this.activeAudioSnapshot);
		this.DisableAllMedia();
		this.videoPlayer.gameObject.SetActive(true);
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.screen.texture = this.renderTexture;
		this.videoPlayer.targetTexture = this.renderTexture;
		this.videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.videoPlayer.clip = clip;
		this.videoPlayer.timeReference = (syncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun);
		this.videoPlayer.Play();
		if (this.audioHandle.isValid())
		{
			KFMOD.EndOneShot(this.audioHandle);
			this.audioHandle.clearHandle();
		}
		this.audioHandle = KFMOD.BeginOneShot(GlobalAssets.GetSound("vid_" + clip.name, false), Vector3.zero, 1f);
		KFMOD.EndOneShot(this.audioHandle);
		this.videoSkippable = !unskippable;
		this.closeButton.gameObject.SetActive(this.videoSkippable);
		this.proceedButton.gameObject.SetActive(showProceedButton && this.videoSkippable);
	}

	// Token: 0x0600B28D RID: 45709 RVA: 0x0043DB80 File Offset: 0x0043BD80
	public void QueueVictoryVideoLoop(bool queue, string message = "", string victoryAchievement = "", string loopVideo = "", bool showAchievements = true, bool syncAudio = false)
	{
		this.victoryLoopQueued = queue;
		this.victoryLoopMessage = message;
		this.victoryLoopClip = loopVideo;
		this.victoryLoopSyncAudio = syncAudio;
		this.OnStop = (System.Action)Delegate.Combine(this.OnStop, new System.Action(delegate()
		{
			if (showAchievements)
			{
				RetireColonyUtility.SaveColonySummaryData();
				MainMenu.ActivateRetiredColoniesScreenFromData(this.transform.parent.gameObject, RetireColonyUtility.GetCurrentColonyRetiredColonyData());
			}
		}));
	}

	// Token: 0x0600B28E RID: 45710 RVA: 0x0043DBE4 File Offset: 0x0043BDE4
	public void SetOverlayText(string overlayTemplate, List<string> strings)
	{
		VideoOverlay videoOverlay = null;
		foreach (VideoOverlay videoOverlay2 in this.overlayPrefabs)
		{
			if (videoOverlay2.name == overlayTemplate)
			{
				videoOverlay = videoOverlay2;
				break;
			}
		}
		DebugUtil.Assert(videoOverlay != null, "Could not find a template named ", overlayTemplate);
		global::Util.KInstantiateUI<VideoOverlay>(videoOverlay.gameObject, this.overlayContainer.gameObject, true).SetText(strings);
		this.overlayContainer.gameObject.SetActive(true);
	}

	// Token: 0x0600B28F RID: 45711 RVA: 0x001189A6 File Offset: 0x00116BA6
	private IEnumerator SwitchToVictoryLoop()
	{
		this.victoryLoopQueued = false;
		Color color = this.fadeOverlay.color;
		for (float i = 0f; i < 1f; i += Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 1f);
		MusicManager.instance.PlaySong("Music_Victory_03_StoryAndSummary", false);
		MusicManager.instance.SetSongParameter("Music_Victory_03_StoryAndSummary", "songSection", 1f, true);
		this.closeButton.gameObject.SetActive(true);
		this.proceedButton.gameObject.SetActive(true);
		this.SetOverlayText("VictoryEnd", new List<string>
		{
			this.victoryLoopMessage
		});
		this.videoPlayer.clip = Assets.GetVideo(this.victoryLoopClip);
		this.videoPlayer.isLooping = true;
		this.videoPlayer.Play();
		this.proceedButton.gameObject.SetActive(true);
		this.videoPlayer.timeReference = (this.victoryLoopSyncAudio ? VideoTimeReference.ExternalTime : VideoTimeReference.Freerun);
		yield return SequenceUtil.WaitForSecondsRealtime(1f);
		for (float i = 1f; i >= 0f; i -= Time.unscaledDeltaTime)
		{
			this.fadeOverlay.color = new Color(color.r, color.g, color.b, i);
			yield return SequenceUtil.WaitForNextFrame;
		}
		this.fadeOverlay.color = new Color(color.r, color.g, color.b, 0f);
		yield break;
	}

	// Token: 0x0600B290 RID: 45712 RVA: 0x0043DC84 File Offset: 0x0043BE84
	public void Stop()
	{
		this.videoPlayer.Stop();
		this.screen.texture = null;
		this.videoPlayer.targetTexture = null;
		if (!this.activeAudioSnapshot.IsNull)
		{
			AudioMixer.instance.Stop(this.activeAudioSnapshot, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			this.audioHandle.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		}
		if (this.OnStop != null)
		{
			this.OnStop();
		}
		this.Show(false);
	}

	// Token: 0x0600B291 RID: 45713 RVA: 0x0043DCFC File Offset: 0x0043BEFC
	public override void ScreenUpdate(bool topLevel)
	{
		base.ScreenUpdate(topLevel);
		if (this.audioHandle.isValid())
		{
			int num;
			this.audioHandle.getTimelinePosition(out num);
			this.videoPlayer.externalReferenceTime = (double)((float)num / 1000f);
		}
	}

	// Token: 0x04008CE9 RID: 36073
	public static VideoScreen Instance;

	// Token: 0x04008CEA RID: 36074
	[SerializeField]
	private VideoPlayer videoPlayer;

	// Token: 0x04008CEB RID: 36075
	[SerializeField]
	private Slideshow slideshow;

	// Token: 0x04008CEC RID: 36076
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008CED RID: 36077
	[SerializeField]
	private KButton proceedButton;

	// Token: 0x04008CEE RID: 36078
	[SerializeField]
	private RectTransform overlayContainer;

	// Token: 0x04008CEF RID: 36079
	[SerializeField]
	private List<VideoOverlay> overlayPrefabs;

	// Token: 0x04008CF0 RID: 36080
	private RawImage screen;

	// Token: 0x04008CF1 RID: 36081
	private RenderTexture renderTexture;

	// Token: 0x04008CF2 RID: 36082
	private EventReference activeAudioSnapshot;

	// Token: 0x04008CF3 RID: 36083
	[SerializeField]
	private Image fadeOverlay;

	// Token: 0x04008CF4 RID: 36084
	private EventInstance audioHandle;

	// Token: 0x04008CF5 RID: 36085
	private bool victoryLoopQueued;

	// Token: 0x04008CF6 RID: 36086
	private string victoryLoopMessage = "";

	// Token: 0x04008CF7 RID: 36087
	private string victoryLoopClip = "";

	// Token: 0x04008CF8 RID: 36088
	private bool victoryLoopSyncAudio;

	// Token: 0x04008CF9 RID: 36089
	private bool videoSkippable = true;

	// Token: 0x04008CFA RID: 36090
	public System.Action OnStop;
}
