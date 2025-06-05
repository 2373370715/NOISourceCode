using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001EBA RID: 7866
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class OldVersionMessageScreen : KModalScreen
{
	// Token: 0x0600A510 RID: 42256 RVA: 0x003F8224 File Offset: 0x003F6424
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/topic/140474-previous-update-steam-branch-access/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.quitButton.onClick += delegate()
		{
			App.Quit();
		};
	}

	// Token: 0x0600A511 RID: 42257 RVA: 0x003F82A4 File Offset: 0x003F64A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.messageContainer.sizeDelta = new Vector2(Mathf.Max(384f, (float)Screen.width * 0.25f), this.messageContainer.sizeDelta.y);
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x0400810F RID: 33039
	public KButton forumButton;

	// Token: 0x04008110 RID: 33040
	public KButton confirmButton;

	// Token: 0x04008111 RID: 33041
	public KButton quitButton;

	// Token: 0x04008112 RID: 33042
	public LocText bodyText;

	// Token: 0x04008113 RID: 33043
	public bool previewInEditor;

	// Token: 0x04008114 RID: 33044
	public RectTransform messageContainer;
}
