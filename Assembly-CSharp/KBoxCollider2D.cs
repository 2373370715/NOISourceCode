using System;
using UnityEngine;

// Token: 0x02000AA1 RID: 2721
public class KBoxCollider2D : KCollider2D
{
	// Token: 0x170001FB RID: 507
	// (get) Token: 0x0600318E RID: 12686 RVA: 0x000C4A0D File Offset: 0x000C2C0D
	// (set) Token: 0x0600318F RID: 12687 RVA: 0x000C4A15 File Offset: 0x000C2C15
	public Vector2 size
	{
		get
		{
			return this._size;
		}
		set
		{
			this._size = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06003190 RID: 12688 RVA: 0x0020D1D8 File Offset: 0x0020B3D8
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = this.size * 0.9999f;
		Vector2 vector3 = new Vector2(vector.x - vector2.x * 0.5f, vector.y - vector2.y * 0.5f);
		Vector2 vector4 = new Vector2(vector.x + vector2.x * 0.5f, vector.y + vector2.y * 0.5f);
		Vector2I vector2I = new Vector2I((int)vector3.x, (int)vector3.y);
		Vector2I vector2I2 = new Vector2I((int)vector4.x, (int)vector4.y);
		int width = vector2I2.x - vector2I.x + 1;
		int height = vector2I2.y - vector2I.y + 1;
		return new Extents(vector2I.x, vector2I.y, width, height);
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x0020D2E4 File Offset: 0x0020B4E4
	public override bool Intersects(Vector2 intersect_pos)
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.size.x * 0.5f, vector.y - this.size.y * 0.5f);
		Vector2 vector3 = new Vector2(vector.x + this.size.x * 0.5f, vector.y + this.size.y * 0.5f);
		return intersect_pos.x >= vector2.x && intersect_pos.x <= vector3.x && intersect_pos.y >= vector2.y && intersect_pos.y <= vector3.y;
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06003192 RID: 12690 RVA: 0x0020D3D0 File Offset: 0x0020B5D0
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._size.x, this._size.y, 0f));
		}
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x0020D434 File Offset: 0x0020B634
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.bounds.center, new Vector3(this._size.x, this._size.y, 0f));
	}

	// Token: 0x04002208 RID: 8712
	[SerializeField]
	private Vector2 _size;
}
