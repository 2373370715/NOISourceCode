using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C1F RID: 7199
public class AlertVignette : KMonoBehaviour
{
	// Token: 0x060095BA RID: 38330 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060095BB RID: 38331 RVA: 0x003A87E8 File Offset: 0x003A69E8
	private void Update()
	{
		Color color = this.image.color;
		if (ClusterManager.Instance.GetWorld(this.worldID) == null)
		{
			color = Color.clear;
			this.image.color = color;
			return;
		}
		if (ClusterManager.Instance.GetWorld(this.worldID).IsRedAlert())
		{
			if (color.r != Vignette.Instance.redAlertColor.r || color.g != Vignette.Instance.redAlertColor.g || color.b != Vignette.Instance.redAlertColor.b)
			{
				color = Vignette.Instance.redAlertColor;
			}
		}
		else if (ClusterManager.Instance.GetWorld(this.worldID).IsYellowAlert())
		{
			if (color.r != Vignette.Instance.yellowAlertColor.r || color.g != Vignette.Instance.yellowAlertColor.g || color.b != Vignette.Instance.yellowAlertColor.b)
			{
				color = Vignette.Instance.yellowAlertColor;
			}
		}
		else
		{
			color = Color.clear;
		}
		if (color != Color.clear)
		{
			color.a = 0.2f + (0.5f + Mathf.Sin(Time.unscaledTime * 4f - 1f) / 2f) * 0.5f;
		}
		if (this.image.color != color)
		{
			this.image.color = color;
		}
	}

	// Token: 0x04007483 RID: 29827
	public Image image;

	// Token: 0x04007484 RID: 29828
	public int worldID;
}
