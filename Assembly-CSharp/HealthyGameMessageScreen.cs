using System;
using UnityEngine;

// Token: 0x02001D4E RID: 7502
[AddComponentMenu("KMonoBehaviour/scripts/HealthyGameMessageScreen")]
public class HealthyGameMessageScreen : KMonoBehaviour
{
	// Token: 0x06009CAB RID: 40107 RVA: 0x0010A5EA File Offset: 0x001087EA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.confirmButton.onClick += delegate()
		{
			this.PlayIntroShort();
		};
		this.confirmButton.gameObject.SetActive(false);
	}

	// Token: 0x06009CAC RID: 40108 RVA: 0x003D319C File Offset: 0x003D139C
	private void PlayIntroShort()
	{
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false, true);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				if (base.gameObject != null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}));
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06009CAD RID: 40109 RVA: 0x0010A61A File Offset: 0x0010881A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06009CAE RID: 40110 RVA: 0x003D3250 File Offset: 0x003D1450
	private void Update()
	{
		if (!DistributionPlatform.Inst.IsDLCStatusReady())
		{
			return;
		}
		if (this.isFirstUpdate)
		{
			this.isFirstUpdate = false;
			this.spawnTime = Time.unscaledTime;
			return;
		}
		float num = Mathf.Min(Time.unscaledDeltaTime, 0.033333335f);
		float num2 = Time.unscaledTime - this.spawnTime;
		if (num2 < this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha + num * (1f / this.fadeTime);
			return;
		}
		if (num2 >= this.totalTime + 0.75f)
		{
			this.canvasGroup.alpha = 1f;
			this.confirmButton.gameObject.SetActive(true);
			return;
		}
		if (num2 >= this.totalTime - this.fadeTime)
		{
			this.canvasGroup.alpha = this.canvasGroup.alpha - num * (1f / this.fadeTime);
		}
	}

	// Token: 0x04007AC0 RID: 31424
	public KButton confirmButton;

	// Token: 0x04007AC1 RID: 31425
	public CanvasGroup canvasGroup;

	// Token: 0x04007AC2 RID: 31426
	private float spawnTime;

	// Token: 0x04007AC3 RID: 31427
	private float totalTime = 10f;

	// Token: 0x04007AC4 RID: 31428
	private float fadeTime = 1.5f;

	// Token: 0x04007AC5 RID: 31429
	private bool isFirstUpdate = true;
}
