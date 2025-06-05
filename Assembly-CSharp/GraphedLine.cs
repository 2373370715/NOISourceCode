using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02001D45 RID: 7493
[AddComponentMenu("KMonoBehaviour/scripts/GraphedLine")]
[Serializable]
public class GraphedLine : KMonoBehaviour
{
	// Token: 0x17000A51 RID: 2641
	// (get) Token: 0x06009C7D RID: 40061 RVA: 0x0010A3B8 File Offset: 0x001085B8
	public int PointCount
	{
		get
		{
			return this.points.Length;
		}
	}

	// Token: 0x06009C7E RID: 40062 RVA: 0x0010A3C2 File Offset: 0x001085C2
	public void SetPoints(Vector2[] points)
	{
		this.points = points;
		this.UpdatePoints();
	}

	// Token: 0x06009C7F RID: 40063 RVA: 0x003D16B8 File Offset: 0x003CF8B8
	private void UpdatePoints()
	{
		Vector2[] array = new Vector2[this.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = this.layer.graph.GetRelativePosition(this.points[i]);
		}
		this.line_renderer.Points = array;
	}

	// Token: 0x06009C80 RID: 40064 RVA: 0x003D1710 File Offset: 0x003CF910
	public Vector2 GetClosestDataToPointOnXAxis(Vector2 toPoint)
	{
		float num = toPoint.x / this.layer.graph.rectTransform().sizeDelta.x;
		float num2 = this.layer.graph.axis_x.min_value + this.layer.graph.axis_x.range * num;
		Vector2 vector = Vector2.zero;
		foreach (Vector2 vector2 in this.points)
		{
			if (Mathf.Abs(vector2.x - num2) < Mathf.Abs(vector.x - num2))
			{
				vector = vector2;
			}
		}
		return vector;
	}

	// Token: 0x06009C81 RID: 40065 RVA: 0x0010A3D1 File Offset: 0x001085D1
	public void HidePointHighlight()
	{
		if (this.highlightPoint != null)
		{
			this.highlightPoint.SetActive(false);
		}
	}

	// Token: 0x06009C82 RID: 40066 RVA: 0x003D17B8 File Offset: 0x003CF9B8
	public void SetPointHighlight(Vector2 point)
	{
		if (this.highlightPoint == null)
		{
			return;
		}
		this.highlightPoint.SetActive(true);
		Vector2 relativePosition = this.layer.graph.GetRelativePosition(point);
		this.highlightPoint.rectTransform().SetLocalPosition(new Vector2(relativePosition.x * this.layer.graph.rectTransform().sizeDelta.x - this.layer.graph.rectTransform().sizeDelta.x / 2f, relativePosition.y * this.layer.graph.rectTransform().sizeDelta.y - this.layer.graph.rectTransform().sizeDelta.y / 2f));
		ToolTip component = this.layer.graph.GetComponent<ToolTip>();
		component.ClearMultiStringTooltip();
		component.tooltipPositionOffset = new Vector2(this.highlightPoint.rectTransform().localPosition.x, this.layer.graph.rectTransform().rect.height / 2f - 12f);
		component.SetSimpleTooltip(string.Concat(new string[]
		{
			this.layer.graph.axis_x.name,
			" ",
			point.x.ToString(),
			", ",
			Mathf.RoundToInt(point.y).ToString(),
			" ",
			this.layer.graph.axis_y.name
		}));
		ToolTipScreen.Instance.SetToolTip(component);
	}

	// Token: 0x04007A85 RID: 31365
	public UILineRenderer line_renderer;

	// Token: 0x04007A86 RID: 31366
	public LineLayer layer;

	// Token: 0x04007A87 RID: 31367
	private Vector2[] points = new Vector2[0];

	// Token: 0x04007A88 RID: 31368
	[SerializeField]
	private GameObject highlightPoint;
}
