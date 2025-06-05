using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

// Token: 0x020020B7 RID: 8375
[AddComponentMenu("KMonoBehaviour/scripts/VideoWidget")]
public class VideoWidget : KMonoBehaviour
{
	// Token: 0x0600B29E RID: 45726 RVA: 0x00118A53 File Offset: 0x00116C53
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.Clicked;
		this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
	}

	// Token: 0x0600B29F RID: 45727 RVA: 0x0043DFD8 File Offset: 0x0043C1D8
	private void Clicked()
	{
		VideoScreen.Instance.PlayVideo(this.clip, false, default(EventReference), false, true);
		if (!string.IsNullOrEmpty(this.overlayName))
		{
			VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
		}
	}

	// Token: 0x0600B2A0 RID: 45728 RVA: 0x0043E024 File Offset: 0x0043C224
	public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
	{
		if (clip == null)
		{
			global::Debug.LogWarning("Tried to assign null video clip to VideoWidget");
			return;
		}
		this.clip = clip;
		this.overlayName = overlayName;
		this.texts = texts;
		this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
		this.thumbnailPlayer.targetTexture = this.renderTexture;
		this.rawImage.texture = this.renderTexture;
		base.StartCoroutine(this.ConfigureThumbnail());
	}

	// Token: 0x0600B2A1 RID: 45729 RVA: 0x00118A83 File Offset: 0x00116C83
	private IEnumerator ConfigureThumbnail()
	{
		this.thumbnailPlayer.audioOutputMode = VideoAudioOutputMode.None;
		this.thumbnailPlayer.clip = this.clip;
		this.thumbnailPlayer.time = 0.0;
		this.thumbnailPlayer.Play();
		yield return null;
		yield break;
	}

	// Token: 0x0600B2A2 RID: 45730 RVA: 0x00118A92 File Offset: 0x00116C92
	private void Update()
	{
		if (this.thumbnailPlayer.isPlaying && this.thumbnailPlayer.time > 2.0)
		{
			this.thumbnailPlayer.Pause();
		}
	}

	// Token: 0x04008D02 RID: 36098
	[SerializeField]
	private VideoClip clip;

	// Token: 0x04008D03 RID: 36099
	[SerializeField]
	private VideoPlayer thumbnailPlayer;

	// Token: 0x04008D04 RID: 36100
	[SerializeField]
	private KButton button;

	// Token: 0x04008D05 RID: 36101
	[SerializeField]
	private string overlayName;

	// Token: 0x04008D06 RID: 36102
	[SerializeField]
	private List<string> texts;

	// Token: 0x04008D07 RID: 36103
	private RenderTexture renderTexture;

	// Token: 0x04008D08 RID: 36104
	private RawImage rawImage;
}
