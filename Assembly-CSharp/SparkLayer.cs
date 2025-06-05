using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001D49 RID: 7497
public class SparkLayer : LineLayer
{
	// Token: 0x06009C90 RID: 40080 RVA: 0x003D2664 File Offset: 0x003D0864
	public void SetColor(ColonyDiagnostic.DiagnosticResult result)
	{
		switch (result.opinion)
		{
		case ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
			this.SetColor(Constants.NEGATIVE_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
			this.SetColor(Constants.WARNING_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial:
		case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
			this.SetColor(Constants.POSITIVE_COLOR);
			return;
		default:
			this.SetColor(Constants.NEUTRAL_COLOR);
			return;
		}
	}

	// Token: 0x06009C91 RID: 40081 RVA: 0x0010A43C File Offset: 0x0010863C
	public void SetColor(Color color)
	{
		this.line_formatting[0].color = color;
	}

	// Token: 0x06009C92 RID: 40082 RVA: 0x003D26E0 File Offset: 0x003D08E0
	public override GraphedLine NewLine(global::Tuple<float, float>[] points, string ID = "")
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		this.ScaleToData(points);
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
		return base.NewLine(points, ID);
	}

	// Token: 0x06009C93 RID: 40083 RVA: 0x0010A450 File Offset: 0x00108650
	public override void RefreshLine(global::Tuple<float, float>[] points, string ID)
	{
		this.SetColor(points);
		this.ScaleToData(points);
		base.RefreshLine(points, ID);
	}

	// Token: 0x06009C94 RID: 40084 RVA: 0x003D2868 File Offset: 0x003D0A68
	private void SetColor(global::Tuple<float, float>[] points)
	{
		Color positive_COLOR = Constants.POSITIVE_COLOR;
		Color neutral_COLOR = Constants.NEUTRAL_COLOR;
		Color negative_COLOR = Constants.NEGATIVE_COLOR;
		if (this.colorRules.setOwnColor)
		{
			if (points.Length > 2)
			{
				if (this.colorRules.zeroIsBad && points[points.Length - 1].second == 0f)
				{
					this.line_formatting[0].color = negative_COLOR;
				}
				else if (points[points.Length - 1].second > points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? positive_COLOR : negative_COLOR);
				}
				else if (points[points.Length - 1].second < points[points.Length - 2].second)
				{
					this.line_formatting[0].color = (this.colorRules.positiveIsGood ? negative_COLOR : positive_COLOR);
				}
				else
				{
					this.line_formatting[0].color = neutral_COLOR;
				}
			}
			else
			{
				this.line_formatting[0].color = neutral_COLOR;
			}
		}
		if (this.subZeroAreaFill != null)
		{
			this.subZeroAreaFill.color = new Color(this.line_formatting[0].color.r, this.line_formatting[0].color.g, this.line_formatting[0].color.b, this.fillAlphaMin);
		}
	}

	// Token: 0x06009C95 RID: 40085 RVA: 0x003D29E0 File Offset: 0x003D0BE0
	private void ScaleToData(global::Tuple<float, float>[] points)
	{
		if (this.scaleWidthToData || this.scaleHeightToData)
		{
			Vector2 vector = base.CalculateMin(points);
			Vector2 vector2 = base.CalculateMax(points);
			if (this.scaleHeightToData)
			{
				base.graph.ClearHorizontalGuides();
				base.graph.axis_y.max_value = vector2.y;
				base.graph.axis_y.min_value = vector.y;
				base.graph.RefreshHorizontalGuides();
			}
			if (this.scaleWidthToData)
			{
				base.graph.ClearVerticalGuides();
				base.graph.axis_x.max_value = vector2.x;
				base.graph.axis_x.min_value = vector.x;
				base.graph.RefreshVerticalGuides();
			}
		}
	}

	// Token: 0x04007A9A RID: 31386
	public Image subZeroAreaFill;

	// Token: 0x04007A9B RID: 31387
	public SparkLayer.ColorRules colorRules;

	// Token: 0x04007A9C RID: 31388
	public bool debugMark;

	// Token: 0x04007A9D RID: 31389
	public bool scaleHeightToData = true;

	// Token: 0x04007A9E RID: 31390
	public bool scaleWidthToData = true;

	// Token: 0x02001D4A RID: 7498
	[Serializable]
	public struct ColorRules
	{
		// Token: 0x04007A9F RID: 31391
		public bool setOwnColor;

		// Token: 0x04007AA0 RID: 31392
		public bool positiveIsGood;

		// Token: 0x04007AA1 RID: 31393
		public bool zeroIsBad;
	}
}
