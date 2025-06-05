using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

// Token: 0x02001D3F RID: 7487
[AddComponentMenu("KMonoBehaviour/scripts/GraphBase")]
public class GraphBase : KMonoBehaviour
{
	// Token: 0x06009C69 RID: 40041 RVA: 0x003D0F48 File Offset: 0x003CF148
	public Vector2 GetRelativePosition(Vector2 absolute_point)
	{
		Vector2 zero = Vector2.zero;
		float num = Mathf.Max(1f, this.axis_x.max_value - this.axis_x.min_value);
		float num2 = absolute_point.x - this.axis_x.min_value;
		zero.x = num2 / num;
		float num3 = Mathf.Max(1f, this.axis_y.max_value - this.axis_y.min_value);
		float num4 = absolute_point.y - this.axis_y.min_value;
		zero.y = num4 / num3;
		return zero;
	}

	// Token: 0x06009C6A RID: 40042 RVA: 0x0010A302 File Offset: 0x00108502
	public Vector2 GetRelativeSize(Vector2 absolute_size)
	{
		return this.GetRelativePosition(absolute_size);
	}

	// Token: 0x06009C6B RID: 40043 RVA: 0x0010A30B File Offset: 0x0010850B
	public void ClearGuides()
	{
		this.ClearVerticalGuides();
		this.ClearHorizontalGuides();
	}

	// Token: 0x06009C6C RID: 40044 RVA: 0x003D0FDC File Offset: 0x003CF1DC
	public void ClearHorizontalGuides()
	{
		foreach (GameObject gameObject in this.horizontalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.horizontalGuides.Clear();
	}

	// Token: 0x06009C6D RID: 40045 RVA: 0x003D1044 File Offset: 0x003CF244
	public void ClearVerticalGuides()
	{
		foreach (GameObject gameObject in this.verticalGuides)
		{
			if (gameObject != null)
			{
				UnityEngine.Object.DestroyImmediate(gameObject);
			}
		}
		this.verticalGuides.Clear();
	}

	// Token: 0x06009C6E RID: 40046 RVA: 0x0010A319 File Offset: 0x00108519
	public void RefreshGuides()
	{
		this.ClearGuides();
		this.RefreshHorizontalGuides();
		this.RefreshVerticalGuides();
	}

	// Token: 0x06009C6F RID: 40047 RVA: 0x003D10AC File Offset: 0x003CF2AC
	public void RefreshHorizontalGuides()
	{
		if (this.prefab_guide_x != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_x, this.guides_x, true);
			gameObject.name = "guides_horizontal";
			Vector2[] array = new Vector2[2 * (int)(this.axis_y.range / this.axis_y.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2(this.axis_x.min_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2(this.axis_x.max_value, (float)i * (this.axis_y.guide_frequency / 2f));
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_horizontal_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_horizontal_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.MidlineLeft;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_y.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2(8f, (float)i * (base.gameObject.rectTransform().rect.height / (float)array.Length)) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.horizontalGuides.Add(gameObject);
		}
	}

	// Token: 0x06009C70 RID: 40048 RVA: 0x003D1254 File Offset: 0x003CF454
	public void RefreshVerticalGuides()
	{
		if (this.prefab_guide_y != null)
		{
			GameObject gameObject = Util.KInstantiateUI(this.prefab_guide_y, this.guides_y, true);
			gameObject.name = "guides_vertical";
			Vector2[] array = new Vector2[2 * (int)(this.axis_x.range / this.axis_x.guide_frequency)];
			for (int i = 0; i < array.Length; i += 2)
			{
				Vector2 absolute_point = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.min_value);
				array[i] = this.GetRelativePosition(absolute_point);
				Vector2 absolute_point2 = new Vector2((float)i * (this.axis_x.guide_frequency / 2f), this.axis_y.max_value);
				array[i + 1] = this.GetRelativePosition(absolute_point2);
				if (this.prefab_guide_vertical_label != null)
				{
					GameObject gameObject2 = Util.KInstantiateUI(this.prefab_guide_vertical_label, gameObject, true);
					gameObject2.GetComponent<LocText>().alignment = TextAlignmentOptions.Bottom;
					gameObject2.GetComponent<LocText>().text = ((int)this.axis_x.guide_frequency * (i / 2)).ToString();
					gameObject2.rectTransform().SetLocalPosition(new Vector2((float)i * (base.gameObject.rectTransform().rect.width / (float)array.Length), 4f) - base.gameObject.rectTransform().rect.size / 2f);
				}
			}
			gameObject.GetComponent<UILineRenderer>().Points = array;
			this.verticalGuides.Add(gameObject);
		}
	}

	// Token: 0x04007A67 RID: 31335
	[Header("Axis")]
	public GraphAxis axis_x;

	// Token: 0x04007A68 RID: 31336
	public GraphAxis axis_y;

	// Token: 0x04007A69 RID: 31337
	[Header("References")]
	public GameObject prefab_guide_x;

	// Token: 0x04007A6A RID: 31338
	public GameObject prefab_guide_y;

	// Token: 0x04007A6B RID: 31339
	public GameObject prefab_guide_horizontal_label;

	// Token: 0x04007A6C RID: 31340
	public GameObject prefab_guide_vertical_label;

	// Token: 0x04007A6D RID: 31341
	public GameObject guides_x;

	// Token: 0x04007A6E RID: 31342
	public GameObject guides_y;

	// Token: 0x04007A6F RID: 31343
	public LocText label_title;

	// Token: 0x04007A70 RID: 31344
	public LocText label_x;

	// Token: 0x04007A71 RID: 31345
	public LocText label_y;

	// Token: 0x04007A72 RID: 31346
	public string graphName;

	// Token: 0x04007A73 RID: 31347
	protected List<GameObject> horizontalGuides = new List<GameObject>();

	// Token: 0x04007A74 RID: 31348
	protected List<GameObject> verticalGuides = new List<GameObject>();

	// Token: 0x04007A75 RID: 31349
	private const int points_per_guide_line = 2;
}
