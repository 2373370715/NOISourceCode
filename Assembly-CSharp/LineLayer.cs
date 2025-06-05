using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D46 RID: 7494
public class LineLayer : GraphLayer
{
	// Token: 0x06009C84 RID: 40068 RVA: 0x003D197C File Offset: 0x003CFB7C
	private void InitAreaTexture()
	{
		if (this.areaTexture != null)
		{
			return;
		}
		this.areaTexture = new Texture2D(96, 32);
		this.areaFill.sprite = Sprite.Create(this.areaTexture, new Rect(0f, 0f, (float)this.areaTexture.width, (float)this.areaTexture.height), new Vector2(0.5f, 0.5f), 100f);
	}

	// Token: 0x06009C85 RID: 40069 RVA: 0x003D19F8 File Offset: 0x003CFBF8
	public virtual GraphedLine NewLine(global::Tuple<float, float>[] points, string ID = "")
	{
		Vector2[] array = new Vector2[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			array[i] = new Vector2(points[i].first, points[i].second);
		}
		if (this.fillAreaUnderLine)
		{
			this.InitAreaTexture();
			Vector2 vector = this.CalculateMin(points);
			Vector2 vector2 = this.CalculateMax(points) - vector;
			this.areaTexture.filterMode = FilterMode.Point;
			for (int j = 0; j < this.areaTexture.width; j++)
			{
				float num = vector.x + vector2.x * ((float)j / (float)this.areaTexture.width);
				if (points.Length > 1)
				{
					int num2 = 1;
					for (int k = 1; k < points.Length; k++)
					{
						if (points[k].first >= num)
						{
							num2 = k;
							break;
						}
					}
					Vector2 vector3 = new Vector2(points[num2].first - points[num2 - 1].first, points[num2].second - points[num2 - 1].second);
					float num3 = (num - points[num2 - 1].first) / vector3.x;
					bool flag = false;
					int num4 = -1;
					for (int l = this.areaTexture.height - 1; l >= 0; l--)
					{
						if (!flag && vector.y + vector2.y * ((float)l / (float)this.areaTexture.height) < points[num2 - 1].second + vector3.y * num3)
						{
							flag = true;
							num4 = l;
						}
						Color color = flag ? new Color(1f, 1f, 1f, Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float)(num4 - l) / this.fillFadePixels, 0f, 1f))) : Color.clear;
						this.areaTexture.SetPixel(j, l, color);
					}
				}
			}
			this.areaTexture.Apply();
			this.areaFill.color = this.line_formatting[0].color;
		}
		return this.NewLine(array, ID);
	}

	// Token: 0x06009C86 RID: 40070 RVA: 0x003D1C1C File Offset: 0x003CFE1C
	private GraphedLine FindLine(string ID)
	{
		string text = string.Format("line_{0}", ID);
		foreach (GraphedLine graphedLine in this.lines)
		{
			if (graphedLine.name == text)
			{
				return graphedLine.GetComponent<GraphedLine>();
			}
		}
		GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
		gameObject.name = text;
		GraphedLine component = gameObject.GetComponent<GraphedLine>();
		this.lines.Add(component);
		return component;
	}

	// Token: 0x06009C87 RID: 40071 RVA: 0x003D1CBC File Offset: 0x003CFEBC
	public virtual void RefreshLine(global::Tuple<float, float>[] data, string ID)
	{
		this.FillArea(data);
		Vector2[] array2;
		if (data.Length > this.compressDataToPointCount)
		{
			Vector2[] array = new Vector2[this.compressDataToPointCount];
			if (this.compressType == LineLayer.DataScalingType.DropValues)
			{
				float num = (float)(data.Length - this.compressDataToPointCount + 1);
				float num2 = (float)data.Length / num;
				int num3 = 0;
				float num4 = 0f;
				for (int i = 0; i < data.Length; i++)
				{
					num4 += 1f;
					if (num4 >= num2)
					{
						num4 -= num2;
					}
					else
					{
						array[num3] = new Vector2(data[i].first, data[i].second);
						num3++;
					}
				}
				if (array[this.compressDataToPointCount - 1] == Vector2.zero)
				{
					array[this.compressDataToPointCount - 1] = array[this.compressDataToPointCount - 2];
				}
			}
			else
			{
				int num5 = data.Length / this.compressDataToPointCount;
				for (int j = 0; j < this.compressDataToPointCount; j++)
				{
					if (j > 0)
					{
						float num6 = 0f;
						LineLayer.DataScalingType dataScalingType = this.compressType;
						if (dataScalingType != LineLayer.DataScalingType.Average)
						{
							if (dataScalingType == LineLayer.DataScalingType.Max)
							{
								for (int k = 0; k < num5; k++)
								{
									num6 = Mathf.Max(num6, data[j * num5 - k].second);
								}
							}
						}
						else
						{
							for (int l = 0; l < num5; l++)
							{
								num6 += data[j * num5 - l].second;
							}
							num6 /= (float)num5;
						}
						array[j] = new Vector2(data[j * num5].first, num6);
					}
				}
			}
			array2 = array;
		}
		else
		{
			array2 = new Vector2[data.Length];
			for (int m = 0; m < data.Length; m++)
			{
				array2[m] = new Vector2(data[m].first, data[m].second);
			}
		}
		GraphedLine graphedLine = this.FindLine(ID);
		graphedLine.SetPoints(array2);
		graphedLine.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
		graphedLine.line_renderer.LineThickness = (float)this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
	}

	// Token: 0x06009C88 RID: 40072 RVA: 0x003D1F08 File Offset: 0x003D0108
	private void FillArea(global::Tuple<float, float>[] points)
	{
		if (this.fillAreaUnderLine)
		{
			this.InitAreaTexture();
			Vector2 vector;
			Vector2 a;
			this.CalculateMinMax(points, out vector, out a);
			Vector2 vector2 = a - vector;
			this.areaTexture.filterMode = FilterMode.Point;
			Vector2 vector3 = default(Vector2);
			for (int i = 0; i < this.areaTexture.width; i++)
			{
				float num = vector.x + vector2.x * ((float)i / (float)this.areaTexture.width);
				if (points.Length > 1)
				{
					int num2 = 1;
					for (int j = 1; j < points.Length; j++)
					{
						if (points[j].first >= num)
						{
							num2 = j;
							break;
						}
					}
					vector3.x = points[num2].first - points[num2 - 1].first;
					vector3.y = points[num2].second - points[num2 - 1].second;
					float num3 = (num - points[num2 - 1].first) / vector3.x;
					bool flag = false;
					int num4 = -1;
					for (int k = this.areaTexture.height - 1; k >= 0; k--)
					{
						if (!flag && vector.y + vector2.y * ((float)k / (float)this.areaTexture.height) < points[num2 - 1].second + vector3.y * num3)
						{
							flag = true;
							num4 = k;
						}
						Color color = flag ? new Color(1f, 1f, 1f, Mathf.Lerp(1f, this.fillAlphaMin, Mathf.Clamp((float)(num4 - k) / this.fillFadePixels, 0f, 1f))) : Color.clear;
						this.areaTexture.SetPixel(i, k, color);
					}
				}
			}
			this.areaTexture.Apply();
			this.areaFill.color = this.line_formatting[0].color;
		}
	}

	// Token: 0x06009C89 RID: 40073 RVA: 0x003D20FC File Offset: 0x003D02FC
	private void CalculateMinMax(global::Tuple<float, float>[] points, out Vector2 min, out Vector2 max)
	{
		max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		min = new Vector2(float.PositiveInfinity, 0f);
		for (int i = 0; i < points.Length; i++)
		{
			max = new Vector2(Mathf.Max(points[i].first, max.x), Mathf.Max(points[i].second, max.y));
			min = new Vector2(Mathf.Min(points[i].first, min.x), Mathf.Min(points[i].second, min.y));
		}
	}

	// Token: 0x06009C8A RID: 40074 RVA: 0x003D21A4 File Offset: 0x003D03A4
	protected Vector2 CalculateMax(global::Tuple<float, float>[] points)
	{
		Vector2 vector = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
		for (int i = 0; i < points.Length; i++)
		{
			vector = new Vector2(Mathf.Max(points[i].first, vector.x), Mathf.Max(points[i].second, vector.y));
		}
		return vector;
	}

	// Token: 0x06009C8B RID: 40075 RVA: 0x003D2200 File Offset: 0x003D0400
	protected Vector2 CalculateMin(global::Tuple<float, float>[] points)
	{
		Vector2 vector = new Vector2(float.PositiveInfinity, 0f);
		for (int i = 0; i < points.Length; i++)
		{
			vector = new Vector2(Mathf.Min(points[i].first, vector.x), Mathf.Min(points[i].second, vector.y));
		}
		return vector;
	}

	// Token: 0x06009C8C RID: 40076 RVA: 0x003D225C File Offset: 0x003D045C
	public GraphedLine NewLine(Vector2[] points, string ID = "")
	{
		GameObject gameObject = Util.KInstantiateUI(this.prefab_line, this.line_container, true);
		if (ID == "")
		{
			ID = this.lines.Count.ToString();
		}
		gameObject.name = string.Format("line_{0}", ID);
		GraphedLine component = gameObject.GetComponent<GraphedLine>();
		if (points.Length > this.compressDataToPointCount)
		{
			Vector2[] array = new Vector2[this.compressDataToPointCount];
			if (this.compressType == LineLayer.DataScalingType.DropValues)
			{
				float num = (float)(points.Length - this.compressDataToPointCount + 1);
				float num2 = (float)points.Length / num;
				int num3 = 0;
				float num4 = 0f;
				for (int i = 0; i < points.Length; i++)
				{
					num4 += 1f;
					if (num4 >= num2)
					{
						num4 -= num2;
					}
					else
					{
						array[num3] = points[i];
						num3++;
					}
				}
				if (array[this.compressDataToPointCount - 1] == Vector2.zero)
				{
					array[this.compressDataToPointCount - 1] = array[this.compressDataToPointCount - 2];
				}
			}
			else
			{
				int num5 = points.Length / this.compressDataToPointCount;
				for (int j = 0; j < this.compressDataToPointCount; j++)
				{
					if (j > 0)
					{
						float num6 = 0f;
						LineLayer.DataScalingType dataScalingType = this.compressType;
						if (dataScalingType != LineLayer.DataScalingType.Average)
						{
							if (dataScalingType == LineLayer.DataScalingType.Max)
							{
								for (int k = 0; k < num5; k++)
								{
									num6 = Mathf.Max(num6, points[j * num5 - k].y);
								}
							}
						}
						else
						{
							for (int l = 0; l < num5; l++)
							{
								num6 += points[j * num5 - l].y;
							}
							num6 /= (float)num5;
						}
						array[j] = new Vector2(points[j * num5].x, num6);
					}
				}
			}
			points = array;
		}
		component.SetPoints(points);
		component.line_renderer.color = this.line_formatting[this.lines.Count % this.line_formatting.Length].color;
		component.line_renderer.LineThickness = (float)this.line_formatting[this.lines.Count % this.line_formatting.Length].thickness;
		this.lines.Add(component);
		return component;
	}

	// Token: 0x06009C8D RID: 40077 RVA: 0x003D24B8 File Offset: 0x003D06B8
	public void ClearLines()
	{
		foreach (GraphedLine graphedLine in this.lines)
		{
			if (graphedLine != null && graphedLine.gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(graphedLine.gameObject);
			}
		}
		this.lines.Clear();
	}

	// Token: 0x06009C8E RID: 40078 RVA: 0x003D2534 File Offset: 0x003D0734
	private void Update()
	{
		RectTransform component = base.gameObject.GetComponent<RectTransform>();
		if (!RectTransformUtility.RectangleContainsScreenPoint(component, Input.mousePosition))
		{
			for (int i = 0; i < this.lines.Count; i++)
			{
				this.lines[i].HidePointHighlight();
			}
			return;
		}
		Vector2 vector = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(base.gameObject.GetComponent<RectTransform>(), Input.mousePosition, null, out vector);
		vector += component.sizeDelta / 2f;
		for (int j = 0; j < this.lines.Count; j++)
		{
			if (this.lines[j].PointCount != 0)
			{
				Vector2 closestDataToPointOnXAxis = this.lines[j].GetClosestDataToPointOnXAxis(vector);
				if (!float.IsInfinity(closestDataToPointOnXAxis.x) && !float.IsNaN(closestDataToPointOnXAxis.x) && !float.IsInfinity(closestDataToPointOnXAxis.y) && !float.IsNaN(closestDataToPointOnXAxis.y))
				{
					this.lines[j].SetPointHighlight(closestDataToPointOnXAxis);
				}
				else
				{
					this.lines[j].HidePointHighlight();
				}
			}
		}
	}

	// Token: 0x04007A89 RID: 31369
	[Header("Lines")]
	public LineLayer.LineFormat[] line_formatting;

	// Token: 0x04007A8A RID: 31370
	public Image areaFill;

	// Token: 0x04007A8B RID: 31371
	public GameObject prefab_line;

	// Token: 0x04007A8C RID: 31372
	public GameObject line_container;

	// Token: 0x04007A8D RID: 31373
	private List<GraphedLine> lines = new List<GraphedLine>();

	// Token: 0x04007A8E RID: 31374
	protected float fillAlphaMin = 0.33f;

	// Token: 0x04007A8F RID: 31375
	protected float fillFadePixels = 15f;

	// Token: 0x04007A90 RID: 31376
	public bool fillAreaUnderLine;

	// Token: 0x04007A91 RID: 31377
	private Texture2D areaTexture;

	// Token: 0x04007A92 RID: 31378
	private int compressDataToPointCount = 256;

	// Token: 0x04007A93 RID: 31379
	private LineLayer.DataScalingType compressType = LineLayer.DataScalingType.DropValues;

	// Token: 0x02001D47 RID: 7495
	[Serializable]
	public struct LineFormat
	{
		// Token: 0x04007A94 RID: 31380
		public Color color;

		// Token: 0x04007A95 RID: 31381
		public int thickness;
	}

	// Token: 0x02001D48 RID: 7496
	public enum DataScalingType
	{
		// Token: 0x04007A97 RID: 31383
		Average,
		// Token: 0x04007A98 RID: 31384
		Max,
		// Token: 0x04007A99 RID: 31385
		DropValues
	}
}
