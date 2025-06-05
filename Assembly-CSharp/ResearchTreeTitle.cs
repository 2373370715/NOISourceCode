using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class ResearchTreeTitle : MonoBehaviour
{
	// Token: 0x06000016 RID: 22 RVA: 0x000A9F09 File Offset: 0x000A8109
	public void SetLabel(string txt)
	{
		this.treeLabel.text = txt;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x000A9F17 File Offset: 0x000A8117
	public void SetColor(int id)
	{
		this.BG.enabled = (id % 2 != 0);
	}

	// Token: 0x04000013 RID: 19
	[Header("References")]
	[SerializeField]
	private LocText treeLabel;

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private Image BG;
}
