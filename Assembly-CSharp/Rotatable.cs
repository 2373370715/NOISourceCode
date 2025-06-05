using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000B28 RID: 2856
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Rotatable")]
public class Rotatable : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x060034EA RID: 13546 RVA: 0x000C6F32 File Offset: 0x000C5132
	public Orientation Orientation
	{
		get
		{
			return this.orientation;
		}
	}

	// Token: 0x060034EB RID: 13547 RVA: 0x00218F60 File Offset: 0x00217160
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			this.SetSize(component.Def.WidthInCells, component.Def.HeightInCells);
		}
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060034EC RID: 13548 RVA: 0x00218FB8 File Offset: 0x002171B8
	public void SetSize(int width, int height)
	{
		this.width = width;
		this.height = height;
		if (width % 2 == 0)
		{
			this.pivot = new Vector3(-0.5f, 0.5f, 0f);
			this.visualizerOffset = new Vector3(0.5f, 0f, 0f);
			return;
		}
		this.pivot = new Vector3(0f, 0.5f, 0f);
		this.visualizerOffset = Vector3.zero;
	}

	// Token: 0x060034ED RID: 13549 RVA: 0x00219038 File Offset: 0x00217238
	public Orientation Rotate()
	{
		switch (this.permittedRotations)
		{
		case PermittedRotations.R90:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.R90 : Orientation.Neutral);
			break;
		case PermittedRotations.R360:
			this.orientation = (this.orientation + 1) % Orientation.NumRotations;
			break;
		case PermittedRotations.FlipH:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipH : Orientation.Neutral);
			break;
		case PermittedRotations.FlipV:
			this.orientation = ((this.orientation == Orientation.Neutral) ? Orientation.FlipV : Orientation.Neutral);
			break;
		}
		this.OrientVisualizer(this.orientation);
		return this.orientation;
	}

	// Token: 0x060034EE RID: 13550 RVA: 0x000C6F3A File Offset: 0x000C513A
	public void SetOrientation(Orientation new_orientation)
	{
		this.orientation = new_orientation;
		this.OrientVisualizer(new_orientation);
		this.OrientCollider(new_orientation);
	}

	// Token: 0x060034EF RID: 13551 RVA: 0x002190C4 File Offset: 0x002172C4
	public void Match(Rotatable other)
	{
		this.pivot = other.pivot;
		this.visualizerOffset = other.visualizerOffset;
		this.permittedRotations = other.permittedRotations;
		this.orientation = other.orientation;
		this.OrientVisualizer(this.orientation);
		this.OrientCollider(this.orientation);
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x0021911C File Offset: 0x0021731C
	public float GetVisualizerRotation()
	{
		PermittedRotations permittedRotations = this.permittedRotations;
		if (permittedRotations - PermittedRotations.R90 <= 1)
		{
			return -90f * (float)this.orientation;
		}
		return 0f;
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x000C6F51 File Offset: 0x000C5151
	public bool GetVisualizerFlipX()
	{
		return this.orientation == Orientation.FlipH;
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x000C6F5C File Offset: 0x000C515C
	public bool GetVisualizerFlipY()
	{
		return this.orientation == Orientation.FlipV;
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x0021914C File Offset: 0x0021734C
	public Vector3 GetVisualizerPivot()
	{
		Vector3 result = this.pivot;
		Orientation orientation = this.orientation;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
			}
		}
		else
		{
			result.x = -this.pivot.x;
		}
		return result;
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x00219188 File Offset: 0x00217388
	private Vector3 GetVisualizerOffset()
	{
		Orientation orientation = this.orientation;
		Vector3 result;
		if (orientation != Orientation.FlipH)
		{
			if (orientation != Orientation.FlipV)
			{
				result = this.visualizerOffset;
			}
			else
			{
				result = new Vector3(this.visualizerOffset.x, 1f, this.visualizerOffset.z);
			}
		}
		else
		{
			result = new Vector3(-this.visualizerOffset.x, this.visualizerOffset.y, this.visualizerOffset.z);
		}
		return result;
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x002191FC File Offset: 0x002173FC
	private void OrientVisualizer(Orientation orientation)
	{
		float visualizerRotation = this.GetVisualizerRotation();
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Pivot = this.GetVisualizerPivot();
		component.Rotation = visualizerRotation;
		component.Offset = this.GetVisualizerOffset();
		component.FlipX = this.GetVisualizerFlipX();
		component.FlipY = this.GetVisualizerFlipY();
		base.Trigger(-1643076535, this);
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x00219258 File Offset: 0x00217458
	private void OrientCollider(Orientation orientation)
	{
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component == null)
		{
			return;
		}
		float num = 0.5f * (float)((this.width + 1) % 2);
		float num2 = 0f;
		switch (orientation)
		{
		case Orientation.R90:
			num2 = -90f;
			goto IL_11B;
		case Orientation.R180:
			num2 = -180f;
			goto IL_11B;
		case Orientation.R270:
			num2 = -270f;
			goto IL_11B;
		case Orientation.FlipH:
			component.offset = new Vector2(num + (float)(this.width % 2) - 1f, 0.5f * (float)this.height);
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		case Orientation.FlipV:
			component.offset = new Vector2(num, -0.5f * (float)(this.height - 2));
			component.size = new Vector2((float)this.width, (float)this.height);
			goto IL_11B;
		}
		component.offset = new Vector2(num, 0.5f * (float)this.height);
		component.size = new Vector2((float)this.width, (float)this.height);
		IL_11B:
		if (num2 != 0f)
		{
			Matrix2x3 n = Matrix2x3.Translate(-this.pivot);
			Matrix2x3 n2 = Matrix2x3.Rotate(num2 * 0.017453292f);
			Matrix2x3 matrix2x = Matrix2x3.Translate(this.pivot + new Vector3(num, 0f, 0f)) * n2 * n;
			Vector2 vector = new Vector2(-0.5f * (float)this.width, 0f);
			Vector2 vector2 = new Vector2(0.5f * (float)this.width, (float)this.height);
			Vector2 vector3 = new Vector2(0f, 0.5f * (float)this.height);
			vector = matrix2x.MultiplyPoint(vector);
			vector2 = matrix2x.MultiplyPoint(vector2);
			vector3 = matrix2x.MultiplyPoint(vector3);
			float num3 = Mathf.Min(vector.x, vector2.x);
			float num4 = Mathf.Max(vector.x, vector2.x);
			float num5 = Mathf.Min(vector.y, vector2.y);
			float num6 = Mathf.Max(vector.y, vector2.y);
			component.offset = vector3;
			component.size = new Vector2(num4 - num3, num6 - num5);
		}
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x000C6F67 File Offset: 0x000C5167
	public CellOffset GetRotatedCellOffset(CellOffset offset)
	{
		return Rotatable.GetRotatedCellOffset(offset, this.orientation);
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x002194E0 File Offset: 0x002176E0
	public static CellOffset GetRotatedCellOffset(CellOffset offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new CellOffset(offset.y, -offset.x);
		case Orientation.R180:
			return new CellOffset(-offset.x, -offset.y);
		case Orientation.R270:
			return new CellOffset(-offset.y, offset.x);
		case Orientation.FlipH:
			return new CellOffset(-offset.x, offset.y);
		case Orientation.FlipV:
			return new CellOffset(offset.x, -offset.y);
		}
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x000C6F75 File Offset: 0x000C5175
	public static CellOffset GetRotatedCellOffset(int x, int y, Orientation orientation)
	{
		return Rotatable.GetRotatedCellOffset(new CellOffset(x, y), orientation);
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x000C6F84 File Offset: 0x000C5184
	public Vector3 GetRotatedOffset(Vector3 offset)
	{
		return Rotatable.GetRotatedOffset(offset, this.orientation);
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x00219570 File Offset: 0x00217770
	public static Vector3 GetRotatedOffset(Vector3 offset, Orientation orientation)
	{
		switch (orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector3(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector3(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector3(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector3(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector3(offset.x, -offset.y);
		}
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x00219600 File Offset: 0x00217800
	public Vector2I GetRotatedOffset(Vector2I offset)
	{
		switch (this.orientation)
		{
		default:
			return offset;
		case Orientation.R90:
			return new Vector2I(offset.y, -offset.x);
		case Orientation.R180:
			return new Vector2I(-offset.x, -offset.y);
		case Orientation.R270:
			return new Vector2I(-offset.y, offset.x);
		case Orientation.FlipH:
			return new Vector2I(-offset.x, offset.y);
		case Orientation.FlipV:
			return new Vector2I(offset.x, -offset.y);
		}
	}

	// Token: 0x060034FD RID: 13565 RVA: 0x000C6F32 File Offset: 0x000C5132
	public Orientation GetOrientation()
	{
		return this.orientation;
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x060034FE RID: 13566 RVA: 0x000C6F92 File Offset: 0x000C5192
	public bool IsRotated
	{
		get
		{
			return this.orientation > Orientation.Neutral;
		}
	}

	// Token: 0x04002470 RID: 9328
	[Serialize]
	[SerializeField]
	private Orientation orientation;

	// Token: 0x04002471 RID: 9329
	[SerializeField]
	private Vector3 pivot = Vector3.zero;

	// Token: 0x04002472 RID: 9330
	[SerializeField]
	private Vector3 visualizerOffset = Vector3.zero;

	// Token: 0x04002473 RID: 9331
	public PermittedRotations permittedRotations;

	// Token: 0x04002474 RID: 9332
	[SerializeField]
	private int width;

	// Token: 0x04002475 RID: 9333
	[SerializeField]
	private int height;
}
