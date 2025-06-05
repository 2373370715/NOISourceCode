using System;
using Klei;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CF2 RID: 7410
public class DemoTimer : MonoBehaviour
{
	// Token: 0x06009AA3 RID: 39587 RVA: 0x001091CD File Offset: 0x001073CD
	public static void DestroyInstance()
	{
		DemoTimer.Instance = null;
	}

	// Token: 0x06009AA4 RID: 39588 RVA: 0x003C8788 File Offset: 0x003C6988
	private void Start()
	{
		DemoTimer.Instance = this;
		if (GenericGameSettings.instance != null)
		{
			if (GenericGameSettings.instance.demoMode)
			{
				this.duration = (float)GenericGameSettings.instance.demoTime;
				this.labelText.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
				this.clockImage.gameObject.SetActive(GenericGameSettings.instance.showDemoTimer);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}
		else
		{
			base.gameObject.SetActive(false);
		}
		this.duration = (float)GenericGameSettings.instance.demoTime;
		this.fadeOutScreen = Util.KInstantiateUI(this.Prefab_FadeOutScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
		Image component = this.fadeOutScreen.GetComponent<Image>();
		component.raycastTarget = false;
		this.fadeOutColor = component.color;
		this.fadeOutColor.a = 0f;
		this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
	}

	// Token: 0x06009AA5 RID: 39589 RVA: 0x003C8888 File Offset: 0x003C6A88
	private void Update()
	{
		if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.BackQuote))
		{
			this.CountdownActive = !this.CountdownActive;
			this.UpdateLabel();
		}
		if (this.demoOver || !this.CountdownActive)
		{
			return;
		}
		if (this.beginTime == -1f)
		{
			this.beginTime = Time.unscaledTime;
		}
		this.elapsed = Mathf.Clamp(0f, Time.unscaledTime - this.beginTime, this.duration);
		if (this.elapsed + 5f >= this.duration)
		{
			float f = (this.duration - this.elapsed) / 5f;
			this.fadeOutColor.a = Mathf.Min(1f, 1f - Mathf.Sqrt(f));
			this.fadeOutScreen.GetComponent<Image>().color = this.fadeOutColor;
		}
		if (this.elapsed >= this.duration)
		{
			this.EndDemo();
		}
		this.UpdateLabel();
	}

	// Token: 0x06009AA6 RID: 39590 RVA: 0x003C8990 File Offset: 0x003C6B90
	private void UpdateLabel()
	{
		int num = Mathf.RoundToInt(this.duration - this.elapsed);
		int num2 = Mathf.FloorToInt((float)(num / 60));
		int num3 = num % 60;
		this.labelText.text = string.Concat(new string[]
		{
			UI.DEMOOVERSCREEN.TIMEREMAINING,
			" ",
			num2.ToString("00"),
			":",
			num3.ToString("00")
		});
		if (!this.CountdownActive)
		{
			this.labelText.text = UI.DEMOOVERSCREEN.TIMERINACTIVE;
		}
	}

	// Token: 0x06009AA7 RID: 39591 RVA: 0x001091D5 File Offset: 0x001073D5
	public void EndDemo()
	{
		if (this.demoOver)
		{
			return;
		}
		this.demoOver = true;
		Util.KInstantiateUI(this.Prefab_DemoOverScreen, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false).GetComponent<DemoOverScreen>().Show(true);
	}

	// Token: 0x040078B9 RID: 30905
	public static DemoTimer Instance;

	// Token: 0x040078BA RID: 30906
	public LocText labelText;

	// Token: 0x040078BB RID: 30907
	public Image clockImage;

	// Token: 0x040078BC RID: 30908
	public GameObject Prefab_DemoOverScreen;

	// Token: 0x040078BD RID: 30909
	public GameObject Prefab_FadeOutScreen;

	// Token: 0x040078BE RID: 30910
	private float duration;

	// Token: 0x040078BF RID: 30911
	private float elapsed;

	// Token: 0x040078C0 RID: 30912
	private bool demoOver;

	// Token: 0x040078C1 RID: 30913
	private float beginTime = -1f;

	// Token: 0x040078C2 RID: 30914
	public bool CountdownActive;

	// Token: 0x040078C3 RID: 30915
	private GameObject fadeOutScreen;

	// Token: 0x040078C4 RID: 30916
	private Color fadeOutColor;
}
