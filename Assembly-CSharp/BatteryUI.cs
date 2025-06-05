using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001C5C RID: 7260
[AddComponentMenu("KMonoBehaviour/scripts/BatteryUI")]
public class BatteryUI : KMonoBehaviour
{
	// Token: 0x060096E1 RID: 38625 RVA: 0x003AEB7C File Offset: 0x003ACD7C
	private void Initialize()
	{
		if (this.unitLabel == null)
		{
			this.unitLabel = this.currentKJLabel.gameObject.GetComponentInChildrenOnly<LocText>();
		}
		if (this.sizeMap == null || this.sizeMap.Count == 0)
		{
			this.sizeMap = new Dictionary<float, float>();
			this.sizeMap.Add(20000f, 10f);
			this.sizeMap.Add(40000f, 25f);
			this.sizeMap.Add(60000f, 40f);
		}
	}

	// Token: 0x060096E2 RID: 38626 RVA: 0x003AEC0C File Offset: 0x003ACE0C
	public void SetContent(Battery bat)
	{
		if (bat == null || bat.GetMyWorldId() != ClusterManager.Instance.activeWorldId)
		{
			if (base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(false);
			}
			return;
		}
		base.gameObject.SetActive(true);
		this.Initialize();
		RectTransform component = this.batteryBG.GetComponent<RectTransform>();
		float num = 0f;
		foreach (KeyValuePair<float, float> keyValuePair in this.sizeMap)
		{
			if (bat.Capacity <= keyValuePair.Key)
			{
				num = keyValuePair.Value;
				break;
			}
		}
		this.batteryBG.sprite = ((bat.Capacity >= 40000f) ? this.bigBatteryBG : this.regularBatteryBG);
		float y = 25f;
		component.sizeDelta = new Vector2(num, y);
		BuildingEnabledButton component2 = bat.GetComponent<BuildingEnabledButton>();
		Color color;
		if (component2 != null && !component2.IsEnabled)
		{
			color = Color.gray;
		}
		else
		{
			color = ((bat.PercentFull >= bat.PreviousPercentFull) ? this.energyIncreaseColor : this.energyDecreaseColor);
		}
		this.batteryMeter.color = color;
		this.batteryBG.color = color;
		float num2 = this.batteryBG.GetComponent<RectTransform>().rect.height * bat.PercentFull;
		this.batteryMeter.GetComponent<RectTransform>().sizeDelta = new Vector2(num - 5.5f, num2 - 5.5f);
		color.a = 1f;
		if (this.currentKJLabel.color != color)
		{
			this.currentKJLabel.color = color;
			this.unitLabel.color = color;
		}
		this.currentKJLabel.text = bat.JoulesAvailable.ToString("F0");
	}

	// Token: 0x0400755D RID: 30045
	[SerializeField]
	private LocText currentKJLabel;

	// Token: 0x0400755E RID: 30046
	[SerializeField]
	private Image batteryBG;

	// Token: 0x0400755F RID: 30047
	[SerializeField]
	private Image batteryMeter;

	// Token: 0x04007560 RID: 30048
	[SerializeField]
	private Sprite regularBatteryBG;

	// Token: 0x04007561 RID: 30049
	[SerializeField]
	private Sprite bigBatteryBG;

	// Token: 0x04007562 RID: 30050
	[SerializeField]
	private Color energyIncreaseColor = Color.green;

	// Token: 0x04007563 RID: 30051
	[SerializeField]
	private Color energyDecreaseColor = Color.red;

	// Token: 0x04007564 RID: 30052
	private LocText unitLabel;

	// Token: 0x04007565 RID: 30053
	private const float UIUnit = 10f;

	// Token: 0x04007566 RID: 30054
	private Dictionary<float, float> sizeMap;
}
