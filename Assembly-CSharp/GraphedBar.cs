using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D42 RID: 7490
[AddComponentMenu("KMonoBehaviour/scripts/GraphedBar")]
[Serializable]
public class GraphedBar : KMonoBehaviour
{
	// Token: 0x06009C77 RID: 40055 RVA: 0x0010A37A File Offset: 0x0010857A
	public void SetFormat(GraphedBarFormatting format)
	{
		this.format = format;
	}

	// Token: 0x06009C78 RID: 40056 RVA: 0x003D157C File Offset: 0x003CF77C
	public void SetValues(int[] values, float x_position)
	{
		this.ClearValues();
		base.gameObject.rectTransform().anchorMin = new Vector2(x_position, 0f);
		base.gameObject.rectTransform().anchorMax = new Vector2(x_position, 1f);
		base.gameObject.rectTransform().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)this.format.width);
		for (int i = 0; i < values.Length; i++)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_segment, this.segments_container, true);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.preferredHeight = (float)values[i];
			component.minWidth = (float)this.format.width;
			gameObject.GetComponent<Image>().color = this.format.colors[i % this.format.colors.Length];
			this.segments.Add(gameObject);
		}
	}

	// Token: 0x06009C79 RID: 40057 RVA: 0x003D165C File Offset: 0x003CF85C
	public void ClearValues()
	{
		foreach (GameObject obj in this.segments)
		{
			UnityEngine.Object.DestroyImmediate(obj);
		}
		this.segments.Clear();
	}

	// Token: 0x04007A7E RID: 31358
	public GameObject segments_container;

	// Token: 0x04007A7F RID: 31359
	public GameObject prefab_segment;

	// Token: 0x04007A80 RID: 31360
	private List<GameObject> segments = new List<GameObject>();

	// Token: 0x04007A81 RID: 31361
	private GraphedBarFormatting format;
}
