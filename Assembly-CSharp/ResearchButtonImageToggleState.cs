using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001B8B RID: 7051
public class ResearchButtonImageToggleState : ImageToggleState
{
	// Token: 0x060093EE RID: 37870 RVA: 0x0039BE64 File Offset: 0x0039A064
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Research.Instance.Subscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Subscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		this.toggle = base.GetComponent<KToggle>();
	}

	// Token: 0x060093EF RID: 37871 RVA: 0x001050E8 File Offset: 0x001032E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateActiveResearch(null);
		this.RestartCoroutine();
	}

	// Token: 0x060093F0 RID: 37872 RVA: 0x0039BEBC File Offset: 0x0039A0BC
	protected override void OnCleanUp()
	{
		this.AbortCoroutine();
		Research.Instance.Unsubscribe(-1914338957, new Action<object>(this.UpdateActiveResearch));
		Research.Instance.Unsubscribe(-125623018, new Action<object>(this.RefreshProgressBar));
		base.OnCleanUp();
	}

	// Token: 0x060093F1 RID: 37873 RVA: 0x001050FD File Offset: 0x001032FD
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.RestartCoroutine();
	}

	// Token: 0x060093F2 RID: 37874 RVA: 0x0010510B File Offset: 0x0010330B
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		this.AbortCoroutine();
	}

	// Token: 0x060093F3 RID: 37875 RVA: 0x00105119 File Offset: 0x00103319
	private void AbortCoroutine()
	{
		if (this.scrollIconCoroutine != null)
		{
			base.StopCoroutine(this.scrollIconCoroutine);
		}
		this.scrollIconCoroutine = null;
	}

	// Token: 0x060093F4 RID: 37876 RVA: 0x00105136 File Offset: 0x00103336
	private void RestartCoroutine()
	{
		this.AbortCoroutine();
		if (base.gameObject.activeInHierarchy)
		{
			this.scrollIconCoroutine = base.StartCoroutine(this.ScrollIcon());
		}
	}

	// Token: 0x060093F5 RID: 37877 RVA: 0x0039BF0C File Offset: 0x0039A10C
	private void UpdateActiveResearch(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.currentResearchIcons = null;
		}
		else
		{
			this.currentResearchIcons = new Sprite[activeResearch.tech.unlockedItems.Count];
			for (int i = 0; i < activeResearch.tech.unlockedItems.Count; i++)
			{
				TechItem techItem = activeResearch.tech.unlockedItems[i];
				this.currentResearchIcons[i] = techItem.UISprite();
			}
		}
		this.ResetCoroutineTimers();
		this.RefreshProgressBar(o);
	}

	// Token: 0x060093F6 RID: 37878 RVA: 0x0039BF94 File Offset: 0x0039A194
	public void RefreshProgressBar(object o)
	{
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		if (activeResearch == null)
		{
			this.progressBar.fillAmount = 0f;
			return;
		}
		this.progressBar.fillAmount = activeResearch.GetTotalPercentageComplete();
	}

	// Token: 0x060093F7 RID: 37879 RVA: 0x0010515D File Offset: 0x0010335D
	public void SetProgressBarVisibility(bool viisble)
	{
		this.progressBar.enabled = viisble;
	}

	// Token: 0x060093F8 RID: 37880 RVA: 0x0010516B File Offset: 0x0010336B
	public override void SetActive()
	{
		base.SetActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x060093F9 RID: 37881 RVA: 0x0010517A File Offset: 0x0010337A
	public override void SetDisabledActive()
	{
		base.SetDisabledActive();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x060093FA RID: 37882 RVA: 0x00105189 File Offset: 0x00103389
	public override void SetDisabled()
	{
		base.SetDisabled();
		this.SetProgressBarVisibility(false);
	}

	// Token: 0x060093FB RID: 37883 RVA: 0x00105198 File Offset: 0x00103398
	public override void SetInactive()
	{
		base.SetInactive();
		this.SetProgressBarVisibility(true);
		this.RefreshProgressBar(null);
	}

	// Token: 0x060093FC RID: 37884 RVA: 0x001051AE File Offset: 0x001033AE
	private void ResetCoroutineTimers()
	{
		this.mainIconScreenTime = 0f;
		this.itemScreenTime = 0f;
		this.item_idx = -1;
	}

	// Token: 0x170009A9 RID: 2473
	// (get) Token: 0x060093FD RID: 37885 RVA: 0x001051CD File Offset: 0x001033CD
	private bool ReadyToDisplayIcons
	{
		get
		{
			return this.progressBar.enabled && this.currentResearchIcons != null && this.item_idx >= 0 && this.item_idx < this.currentResearchIcons.Length;
		}
	}

	// Token: 0x060093FE RID: 37886 RVA: 0x001051FF File Offset: 0x001033FF
	private IEnumerator ScrollIcon()
	{
		while (Application.isPlaying)
		{
			if (this.mainIconScreenTime < this.researchLogoDuration)
			{
				this.toggle.fgImage.Opacity(1f);
				if (this.toggle.fgImage.overrideSprite != null)
				{
					this.toggle.fgImage.overrideSprite = null;
				}
				this.item_idx = 0;
				this.itemScreenTime = 0f;
				this.mainIconScreenTime += Time.unscaledDeltaTime;
				if (this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons)
				{
					yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.progressBar.enabled && this.mainIconScreenTime >= this.researchLogoDuration && this.ReadyToDisplayIcons);
				}
				yield return null;
			}
			else if (this.ReadyToDisplayIcons)
			{
				if (this.toggle.fgImage.overrideSprite != this.currentResearchIcons[this.item_idx])
				{
					this.toggle.fgImage.overrideSprite = this.currentResearchIcons[this.item_idx];
				}
				yield return this.toggle.fgImage.FadeToVisible(this.fadingDuration, () => this.ReadyToDisplayIcons);
				while (this.itemScreenTime < this.durationPerResearchItemIcon && this.ReadyToDisplayIcons)
				{
					this.itemScreenTime += Time.unscaledDeltaTime;
					yield return null;
				}
				yield return this.toggle.fgImage.FadeAway(this.fadingDuration, () => this.ReadyToDisplayIcons);
				if (this.ReadyToDisplayIcons)
				{
					this.itemScreenTime = 0f;
					this.item_idx++;
				}
				yield return null;
			}
			else
			{
				this.mainIconScreenTime = 0f;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04007036 RID: 28726
	public Image progressBar;

	// Token: 0x04007037 RID: 28727
	private KToggle toggle;

	// Token: 0x04007038 RID: 28728
	[Header("Scroll Options")]
	public float researchLogoDuration = 5f;

	// Token: 0x04007039 RID: 28729
	public float durationPerResearchItemIcon = 0.6f;

	// Token: 0x0400703A RID: 28730
	public float fadingDuration = 0.2f;

	// Token: 0x0400703B RID: 28731
	private Coroutine scrollIconCoroutine;

	// Token: 0x0400703C RID: 28732
	private Sprite[] currentResearchIcons;

	// Token: 0x0400703D RID: 28733
	private float mainIconScreenTime;

	// Token: 0x0400703E RID: 28734
	private float itemScreenTime;

	// Token: 0x0400703F RID: 28735
	private int item_idx = -1;
}
