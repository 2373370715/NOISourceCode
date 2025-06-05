using System;
using UnityEngine;

// Token: 0x02000AA2 RID: 2722
public class KCircleCollider2D : KCollider2D
{
	// Token: 0x170001FD RID: 509
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x000C4A2D File Offset: 0x000C2C2D
	// (set) Token: 0x06003196 RID: 12694 RVA: 0x000C4A35 File Offset: 0x000C2C35
	public float radius
	{
		get
		{
			return this._radius;
		}
		set
		{
			this._radius = value;
			base.MarkDirty(false);
		}
	}

	// Token: 0x06003197 RID: 12695 RVA: 0x0020D480 File Offset: 0x0020B680
	public override Extents GetExtents()
	{
		Vector3 vector = base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f);
		Vector2 vector2 = new Vector2(vector.x - this.radius, vector.y - this.radius);
		Vector2 vector3 = new Vector2(vector.x + this.radius, vector.y + this.radius);
		int width = (int)vector3.x - (int)vector2.x + 1;
		int height = (int)vector3.y - (int)vector2.y + 1;
		return new Extents((int)(vector.x - this._radius), (int)(vector.y - this._radius), width, height);
	}

	// Token: 0x170001FE RID: 510
	// (get) Token: 0x06003198 RID: 12696 RVA: 0x0020D544 File Offset: 0x0020B744
	public override Bounds bounds
	{
		get
		{
			return new Bounds(base.transform.GetPosition() + new Vector3(base.offset.x, base.offset.y, 0f), new Vector3(this._radius * 2f, this._radius * 2f, 0f));
		}
	}

	// Token: 0x06003199 RID: 12697 RVA: 0x0020D5A8 File Offset: 0x0020B7A8
	public override bool Intersects(Vector2 pos)
	{
		Vector3 position = base.transform.GetPosition();
		Vector2 b = new Vector2(position.x, position.y) + base.offset;
		return (pos - b).sqrMagnitude <= this._radius * this._radius;
	}

	// Token: 0x0600319A RID: 12698 RVA: 0x0020D600 File Offset: 0x0020B800
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.bounds.center, this.radius);
	}

	// Token: 0x04002209 RID: 8713
	[SerializeField]
	private float _radius;
}
