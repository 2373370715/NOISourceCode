using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DD8 RID: 7640
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonDisplayUI")]
public class LogicControlInputUI : KMonoBehaviour
{
	// Token: 0x06009FB0 RID: 40880 RVA: 0x003E02A4 File Offset: 0x003DE4A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.colourOn = GlobalAssets.Instance.colorSet.logicOn;
		this.colourOff = GlobalAssets.Instance.colorSet.logicOff;
		this.colourOn.a = (this.colourOff.a = byte.MaxValue);
		this.colourDisconnected = GlobalAssets.Instance.colorSet.logicDisconnected;
		this.icon.raycastTarget = false;
		this.border.raycastTarget = false;
	}

	// Token: 0x06009FB1 RID: 40881 RVA: 0x003E032C File Offset: 0x003DE52C
	public void SetContent(LogicCircuitNetwork network)
	{
		Color32 c = (network == null) ? GlobalAssets.Instance.colorSet.logicDisconnected : (network.IsBitActive(0) ? this.colourOn : this.colourOff);
		this.icon.color = c;
	}

	// Token: 0x04007D5E RID: 32094
	[SerializeField]
	private Image icon;

	// Token: 0x04007D5F RID: 32095
	[SerializeField]
	private Image border;

	// Token: 0x04007D60 RID: 32096
	[SerializeField]
	private LogicModeUI uiAsset;

	// Token: 0x04007D61 RID: 32097
	private Color32 colourOn;

	// Token: 0x04007D62 RID: 32098
	private Color32 colourOff;

	// Token: 0x04007D63 RID: 32099
	private Color32 colourDisconnected;
}
