using System;
using System.Collections.Generic;
using System.Linq;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02001C6E RID: 7278
public class ClusterMapPath : MonoBehaviour
{
	// Token: 0x06009753 RID: 38739 RVA: 0x00107021 File Offset: 0x00105221
	public void Init()
	{
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009754 RID: 38740 RVA: 0x00107040 File Offset: 0x00105240
	public void Init(List<Vector2> nodes, Color color)
	{
		this.m_nodes = nodes;
		this.m_color = color;
		this.lineRenderer = base.gameObject.GetComponentInChildren<UILineRenderer>();
		this.UpdateColor();
		this.UpdateRenderer();
		base.gameObject.SetActive(true);
	}

	// Token: 0x06009755 RID: 38741 RVA: 0x00107079 File Offset: 0x00105279
	public void SetColor(Color color)
	{
		this.m_color = color;
		this.UpdateColor();
	}

	// Token: 0x06009756 RID: 38742 RVA: 0x00107088 File Offset: 0x00105288
	private void UpdateColor()
	{
		this.lineRenderer.color = this.m_color;
		this.pathStart.color = this.m_color;
		this.pathEnd.color = this.m_color;
	}

	// Token: 0x06009757 RID: 38743 RVA: 0x001070BD File Offset: 0x001052BD
	public void SetPoints(List<Vector2> points)
	{
		this.m_nodes = points;
		this.UpdateRenderer();
	}

	// Token: 0x06009758 RID: 38744 RVA: 0x003B2E34 File Offset: 0x003B1034
	private void UpdateRenderer()
	{
		HashSet<Vector2> pointsOnCatmullRomSpline = ProcGen.Util.GetPointsOnCatmullRomSpline(this.m_nodes, 10);
		this.lineRenderer.Points = pointsOnCatmullRomSpline.ToArray<Vector2>();
		if (this.lineRenderer.Points.Length > 1)
		{
			this.pathStart.transform.localPosition = this.lineRenderer.Points[0];
			this.pathStart.gameObject.SetActive(true);
			Vector2 vector = this.lineRenderer.Points[this.lineRenderer.Points.Length - 1];
			Vector2 b = this.lineRenderer.Points[this.lineRenderer.Points.Length - 2];
			this.pathEnd.transform.localPosition = vector;
			Vector2 v = vector - b;
			this.pathEnd.transform.rotation = Quaternion.LookRotation(Vector3.forward, v);
			this.pathEnd.gameObject.SetActive(true);
			return;
		}
		this.pathStart.gameObject.SetActive(false);
		this.pathEnd.gameObject.SetActive(false);
	}

	// Token: 0x06009759 RID: 38745 RVA: 0x003B2F5C File Offset: 0x003B115C
	public float GetRotationForNextSegment()
	{
		if (this.m_nodes.Count > 1)
		{
			Vector2 b = this.m_nodes[0];
			Vector2 to = this.m_nodes[1] - b;
			return Vector2.SignedAngle(Vector2.up, to);
		}
		return 0f;
	}

	// Token: 0x040075D0 RID: 30160
	private List<Vector2> m_nodes;

	// Token: 0x040075D1 RID: 30161
	private Color m_color;

	// Token: 0x040075D2 RID: 30162
	public UILineRenderer lineRenderer;

	// Token: 0x040075D3 RID: 30163
	public Image pathStart;

	// Token: 0x040075D4 RID: 30164
	public Image pathEnd;
}
