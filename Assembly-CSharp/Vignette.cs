using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020B9 RID: 8377
public class Vignette : KMonoBehaviour
{
	// Token: 0x0600B2AA RID: 45738 RVA: 0x00118AD9 File Offset: 0x00116CD9
	public static void DestroyInstance()
	{
		Vignette.Instance = null;
	}

	// Token: 0x0600B2AB RID: 45739 RVA: 0x0043E12C File Offset: 0x0043C32C
	protected override void OnSpawn()
	{
		this.looping_sounds = base.GetComponent<LoopingSounds>();
		base.OnSpawn();
		Vignette.Instance = this;
		this.defaultColor = this.image.color;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(1585324898, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-1393151672, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-741654735, new Action<object>(this.Refresh));
		Game.Instance.Subscribe(-2062778933, new Action<object>(this.Refresh));
	}

	// Token: 0x0600B2AC RID: 45740 RVA: 0x00118AE1 File Offset: 0x00116CE1
	public void SetColor(Color color)
	{
		this.image.color = color;
	}

	// Token: 0x0600B2AD RID: 45741 RVA: 0x0043E1F0 File Offset: 0x0043C3F0
	public void Refresh(object data)
	{
		AlertStateManager.Instance alertManager = ClusterManager.Instance.activeWorld.AlertManager;
		if (alertManager == null)
		{
			return;
		}
		if (alertManager.IsYellowAlert())
		{
			this.SetColor(this.yellowAlertColor);
			if (!this.showingYellowAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("YellowAlert_LP", false), true, false, true);
				this.showingYellowAlert = true;
			}
		}
		else
		{
			this.showingYellowAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
		}
		if (alertManager.IsRedAlert())
		{
			this.SetColor(this.redAlertColor);
			if (!this.showingRedAlert)
			{
				this.looping_sounds.StartSound(GlobalAssets.GetSound("RedAlert_LP", false), true, false, true);
				this.showingRedAlert = true;
			}
		}
		else
		{
			this.showingRedAlert = false;
			this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		}
		if (!this.showingRedAlert && !this.showingYellowAlert)
		{
			this.Reset();
		}
	}

	// Token: 0x0600B2AE RID: 45742 RVA: 0x0043E2E0 File Offset: 0x0043C4E0
	public void Reset()
	{
		this.SetColor(this.defaultColor);
		this.showingRedAlert = false;
		this.showingYellowAlert = false;
		this.looping_sounds.StopSound(GlobalAssets.GetSound("RedAlert_LP", false));
		this.looping_sounds.StopSound(GlobalAssets.GetSound("YellowAlert_LP", false));
	}

	// Token: 0x04008D0C RID: 36108
	[SerializeField]
	private Image image;

	// Token: 0x04008D0D RID: 36109
	public Color defaultColor;

	// Token: 0x04008D0E RID: 36110
	public Color redAlertColor = new Color(1f, 0f, 0f, 0.3f);

	// Token: 0x04008D0F RID: 36111
	public Color yellowAlertColor = new Color(1f, 1f, 0f, 0.3f);

	// Token: 0x04008D10 RID: 36112
	public static Vignette Instance;

	// Token: 0x04008D11 RID: 36113
	private LoopingSounds looping_sounds;

	// Token: 0x04008D12 RID: 36114
	private bool showingRedAlert;

	// Token: 0x04008D13 RID: 36115
	private bool showingYellowAlert;
}
