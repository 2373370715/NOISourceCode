using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F10 RID: 7952
[AddComponentMenu("KMonoBehaviour/scripts/ProgressBar")]
public class ProgressBar : KMonoBehaviour
{
	// Token: 0x17000ABA RID: 2746
	// (get) Token: 0x0600A739 RID: 42809 RVA: 0x00110EE0 File Offset: 0x0010F0E0
	// (set) Token: 0x0600A73A RID: 42810 RVA: 0x00110EED File Offset: 0x0010F0ED
	public Color barColor
	{
		get
		{
			return this.bar.color;
		}
		set
		{
			this.bar.color = value;
		}
	}

	// Token: 0x17000ABB RID: 2747
	// (get) Token: 0x0600A73B RID: 42811 RVA: 0x00110EFB File Offset: 0x0010F0FB
	// (set) Token: 0x0600A73C RID: 42812 RVA: 0x00110F08 File Offset: 0x0010F108
	public float PercentFull
	{
		get
		{
			return this.bar.fillAmount;
		}
		set
		{
			this.bar.fillAmount = value;
		}
	}

	// Token: 0x0600A73D RID: 42813 RVA: 0x00110F16 File Offset: 0x0010F116
	public void SetVisibility(bool visible)
	{
		this.lastVisibilityValue = visible;
		this.RefreshVisibility();
	}

	// Token: 0x0600A73E RID: 42814 RVA: 0x004042AC File Offset: 0x004024AC
	private void RefreshVisibility()
	{
		int myWorldId = base.gameObject.GetMyWorldId();
		bool flag = this.lastVisibilityValue;
		flag &= (!this.hasBeenInitialize || myWorldId == ClusterManager.Instance.activeWorldId);
		flag &= (!this.autoHide || SimDebugView.Instance == null || SimDebugView.Instance.GetMode() == OverlayModes.None.ID);
		base.gameObject.SetActive(flag);
		if (this.updatePercentFull == null || this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x0600A73F RID: 42815 RVA: 0x00404348 File Offset: 0x00402548
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.hasBeenInitialize = true;
		if (this.autoHide)
		{
			this.overlayUpdateHandle = Game.Instance.Subscribe(1798162660, new Action<object>(this.OnOverlayChanged));
			if (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() != OverlayModes.None.ID)
			{
				base.gameObject.SetActive(false);
			}
		}
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		base.enabled = (this.updatePercentFull != null);
		this.RefreshVisibility();
	}

	// Token: 0x0600A740 RID: 42816 RVA: 0x004043FC File Offset: 0x004025FC
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		this.SetWorldActive(tuple.first);
	}

	// Token: 0x0600A741 RID: 42817 RVA: 0x00110F25 File Offset: 0x0010F125
	private void SetWorldActive(int worldId)
	{
		this.RefreshVisibility();
	}

	// Token: 0x0600A742 RID: 42818 RVA: 0x00110F2D File Offset: 0x0010F12D
	public void SetUpdateFunc(Func<float> func)
	{
		this.updatePercentFull = func;
		base.enabled = (this.updatePercentFull != null);
	}

	// Token: 0x0600A743 RID: 42819 RVA: 0x00110F45 File Offset: 0x0010F145
	public virtual void Update()
	{
		if (this.updatePercentFull != null && !this.updatePercentFull.Target.IsNullOrDestroyed())
		{
			this.PercentFull = this.updatePercentFull();
		}
	}

	// Token: 0x0600A744 RID: 42820 RVA: 0x00110F25 File Offset: 0x0010F125
	public virtual void OnOverlayChanged(object data = null)
	{
		this.RefreshVisibility();
	}

	// Token: 0x0600A745 RID: 42821 RVA: 0x0040441C File Offset: 0x0040261C
	public void Retarget(GameObject entity)
	{
		Vector3 vector = entity.transform.GetPosition() + Vector3.down * 0.5f;
		Building component = entity.GetComponent<Building>();
		if (component != null)
		{
			vector -= Vector3.right * 0.5f * (float)(component.Def.WidthInCells % 2);
		}
		else
		{
			vector -= Vector3.right * 0.5f;
		}
		base.transform.SetPosition(vector);
	}

	// Token: 0x0600A746 RID: 42822 RVA: 0x00110F72 File Offset: 0x0010F172
	protected override void OnCleanUp()
	{
		if (this.overlayUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.overlayUpdateHandle);
		}
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		base.OnCleanUp();
	}

	// Token: 0x0600A747 RID: 42823 RVA: 0x0010A5BA File Offset: 0x001087BA
	private void OnBecameInvisible()
	{
		base.enabled = false;
	}

	// Token: 0x0600A748 RID: 42824 RVA: 0x0010A5C3 File Offset: 0x001087C3
	private void OnBecameVisible()
	{
		base.enabled = true;
	}

	// Token: 0x0600A749 RID: 42825 RVA: 0x004044A8 File Offset: 0x004026A8
	public static ProgressBar CreateProgressBar(GameObject entity, Func<float> updateFunc)
	{
		ProgressBar progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, null, false);
		progressBar.SetUpdateFunc(updateFunc);
		progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
		progressBar.name = ((entity != null) ? (entity.name + "_") : "") + " ProgressBar";
		progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
		progressBar.Update();
		progressBar.Retarget(entity);
		return progressBar;
	}

	// Token: 0x0400832F RID: 33583
	public Image bar;

	// Token: 0x04008330 RID: 33584
	private Func<float> updatePercentFull;

	// Token: 0x04008331 RID: 33585
	private int overlayUpdateHandle = -1;

	// Token: 0x04008332 RID: 33586
	public bool autoHide = true;

	// Token: 0x04008333 RID: 33587
	private bool lastVisibilityValue = true;

	// Token: 0x04008334 RID: 33588
	private bool hasBeenInitialize;
}
