using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001CE5 RID: 7397
public class DLCBetaMessageScreen : KModalScreen
{
	// Token: 0x06009A43 RID: 39491 RVA: 0x003C6854 File Offset: 0x003C4A54
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
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

	// Token: 0x06009A44 RID: 39492 RVA: 0x003C68A8 File Offset: 0x003C4AA8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!this.betaIsLive || (Application.isEditor && this.skipInEditor) || !DlcManager.GetActiveDLCIds().Contains("DLC3_ID"))
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x06009A45 RID: 39493 RVA: 0x00108CF8 File Offset: 0x00106EF8
	private void Update()
	{
		this.logo.rectTransform().localPosition = new Vector3(0f, Mathf.Sin(Time.realtimeSinceStartup) * 7.5f);
	}

	// Token: 0x04007863 RID: 30819
	public RectTransform logo;

	// Token: 0x04007864 RID: 30820
	public KButton confirmButton;

	// Token: 0x04007865 RID: 30821
	public KButton quitButton;

	// Token: 0x04007866 RID: 30822
	public LocText bodyText;

	// Token: 0x04007867 RID: 30823
	public RectTransform messageContainer;

	// Token: 0x04007868 RID: 30824
	private bool betaIsLive;

	// Token: 0x04007869 RID: 30825
	private bool skipInEditor;
}
