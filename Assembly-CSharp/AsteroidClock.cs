using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C53 RID: 7251
public class AsteroidClock : MonoBehaviour
{
	// Token: 0x060096A6 RID: 38566 RVA: 0x001068BF File Offset: 0x00104ABF
	private void Awake()
	{
		this.UpdateOverlay();
	}

	// Token: 0x060096A7 RID: 38567 RVA: 0x000AA038 File Offset: 0x000A8238
	private void Start()
	{
	}

	// Token: 0x060096A8 RID: 38568 RVA: 0x001068C7 File Offset: 0x00104AC7
	private void Update()
	{
		if (GameClock.Instance != null)
		{
			this.rotationTransform.rotation = Quaternion.Euler(0f, 0f, 360f * -GameClock.Instance.GetCurrentCycleAsPercentage());
		}
	}

	// Token: 0x060096A9 RID: 38569 RVA: 0x003AD2A4 File Offset: 0x003AB4A4
	private void UpdateOverlay()
	{
		float fillAmount = 0.125f;
		this.NightOverlay.fillAmount = fillAmount;
	}

	// Token: 0x0400750E RID: 29966
	public Transform rotationTransform;

	// Token: 0x0400750F RID: 29967
	public Image NightOverlay;
}
