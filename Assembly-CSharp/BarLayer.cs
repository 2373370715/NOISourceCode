using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001D41 RID: 7489
public class BarLayer : GraphLayer
{
	// Token: 0x17000A4F RID: 2639
	// (get) Token: 0x06009C73 RID: 40051 RVA: 0x0010A35A File Offset: 0x0010855A
	public int bar_count
	{
		get
		{
			return this.bars.Count;
		}
	}

	// Token: 0x06009C74 RID: 40052 RVA: 0x003D1400 File Offset: 0x003CF600
	public void NewBar(int[] values, float x_position, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_bar, this.bar_container, true);
		if (ID == "")
		{
			ID = this.bars.Count.ToString();
		}
		gameObject.name = string.Format("bar_{0}", ID);
		GraphedBar component = gameObject.GetComponent<GraphedBar>();
		component.SetFormat(this.bar_formats[this.bars.Count % this.bar_formats.Length]);
		int[] array = new int[values.Length];
		for (int i = 0; i < values.Length; i++)
		{
			array[i] = (int)(base.graph.rectTransform().rect.height * base.graph.GetRelativeSize(new Vector2(0f, (float)values[i])).y);
		}
		component.SetValues(array, base.graph.GetRelativePosition(new Vector2(x_position, 0f)).x);
		this.bars.Add(component);
	}

	// Token: 0x06009C75 RID: 40053 RVA: 0x003D1500 File Offset: 0x003CF700
	public void ClearBars()
	{
		foreach (GraphedBar graphedBar in this.bars)
		{
			if (graphedBar != null && graphedBar.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedBar.gameObject);
			}
		}
		this.bars.Clear();
	}

	// Token: 0x04007A7A RID: 31354
	public GameObject bar_container;

	// Token: 0x04007A7B RID: 31355
	public GameObject prefab_bar;

	// Token: 0x04007A7C RID: 31356
	public GraphedBarFormatting[] bar_formats;

	// Token: 0x04007A7D RID: 31357
	private List<GraphedBar> bars = new List<GraphedBar>();
}
