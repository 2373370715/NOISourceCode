using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.Video;

// Token: 0x02001E60 RID: 7776
public class TutorialMessageDialog : MessageDialog
{
	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x0600A2DE RID: 41694 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool CanDontShowAgain
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600A2DF RID: 41695 RVA: 0x0010E567 File Offset: 0x0010C767
	public override bool CanDisplay(Message message)
	{
		return typeof(TutorialMessage).IsAssignableFrom(message.GetType());
	}

	// Token: 0x0600A2E0 RID: 41696 RVA: 0x003EE48C File Offset: 0x003EC68C
	public override void SetMessage(Message base_message)
	{
		this.message = (base_message as TutorialMessage);
		this.description.text = this.message.GetMessageBody();
		if (!string.IsNullOrEmpty(this.message.videoClipId))
		{
			VideoClip video = Assets.GetVideo(this.message.videoClipId);
			this.SetVideo(video, this.message.videoOverlayName, this.message.videoTitleText);
		}
	}

	// Token: 0x0600A2E1 RID: 41697 RVA: 0x003EE4FC File Offset: 0x003EC6FC
	public void SetVideo(VideoClip clip, string overlayName, string titleText)
	{
		if (this.videoWidget == null)
		{
			this.videoWidget = Util.KInstantiateUI(this.videoWidgetPrefab, base.transform.gameObject, true).GetComponent<VideoWidget>();
			this.videoWidget.transform.SetAsFirstSibling();
		}
		this.videoWidget.SetClip(clip, overlayName, new List<string>
		{
			titleText,
			VIDEOS.TUTORIAL_HEADER
		});
	}

	// Token: 0x0600A2E2 RID: 41698 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void OnClickAction()
	{
	}

	// Token: 0x0600A2E3 RID: 41699 RVA: 0x0010E57E File Offset: 0x0010C77E
	public override void OnDontShowAgain()
	{
		Tutorial.Instance.HideTutorialMessage(this.message.messageId);
	}

	// Token: 0x04007F6C RID: 32620
	[SerializeField]
	private LocText description;

	// Token: 0x04007F6D RID: 32621
	private TutorialMessage message;

	// Token: 0x04007F6E RID: 32622
	[SerializeField]
	private GameObject videoWidgetPrefab;

	// Token: 0x04007F6F RID: 32623
	private VideoWidget videoWidget;
}
