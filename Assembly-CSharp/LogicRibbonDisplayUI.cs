using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DD9 RID: 7641
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicRibbonDisplayUI : KMonoBehaviour
{
	// Token: 0x06009FB3 RID: 40883 RVA: 0x003E0378 File Offset: 0x003DE578
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.wire1.raycastTarget = false;
		this.wire2.raycastTarget = false;
		this.wire3.raycastTarget = false;
		this.wire4.raycastTarget = false;
	}

	// Token: 0x06009FB4 RID: 40884 RVA: 0x003E0404 File Offset: 0x003DE604
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 color = this.colourDisconnected;
		List<Color32> list = new List<Color32>();
		for (int i = 0; i < this.bitDepth; i++)
		{
			list.Add((network == null) ? color : (network.IsBitActive(i) ? this.colourOn : this.colourOff));
		}
		if (this.wire1.color != list[0])
		{
			this.wire1.color = list[0];
		}
		if (this.wire2.color != list[1])
		{
			this.wire2.color = list[1];
		}
		if (this.wire3.color != list[2])
		{
			this.wire3.color = list[2];
		}
		if (this.wire4.color != list[3])
		{
			this.wire4.color = list[3];
		}
	}

	// Token: 0x04007D64 RID: 32100
	[SerializeField]
	private Image wire1;

	// Token: 0x04007D65 RID: 32101
	[SerializeField]
	private Image wire2;

	// Token: 0x04007D66 RID: 32102
	[SerializeField]
	private Image wire3;

	// Token: 0x04007D67 RID: 32103
	[SerializeField]
	private Image wire4;

	// Token: 0x04007D68 RID: 32104
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04007D69 RID: 32105
	private Color32 colourOn;

	// Token: 0x04007D6A RID: 32106
	private Color32 colourOff;

	// Token: 0x04007D6B RID: 32107
	private Color32 colourDisconnected = new Color(255f, 255f, 255f, 255f);

	// Token: 0x04007D6C RID: 32108
	private int bitDepth = 4;
}
