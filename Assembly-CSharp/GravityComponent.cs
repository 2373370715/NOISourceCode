using System;
using UnityEngine;

// Token: 0x020013D6 RID: 5078
public struct GravityComponent
{
	// Token: 0x0600683C RID: 26684 RVA: 0x002E5EBC File Offset: 0x002E40BC
	public GravityComponent(Transform transform, System.Action on_landed, Vector2 initial_velocity, bool land_on_fake_floors, bool mayLeaveWorld)
	{
		this.transform = transform;
		this.elapsedTime = 0f;
		this.velocity = initial_velocity;
		this.onLanded = on_landed;
		this.landOnFakeFloors = land_on_fake_floors;
		this.mayLeaveWorld = mayLeaveWorld;
		this.collider2D = transform.GetComponent<KCollider2D>();
		this.extents = GravityComponent.GetExtents(this.collider2D);
	}

	// Token: 0x0600683D RID: 26685 RVA: 0x002E5F18 File Offset: 0x002E4118
	public static float GetGroundOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents.y - collider.offset.y;
		}
		return 0f;
	}

	// Token: 0x0600683E RID: 26686 RVA: 0x000E89BA File Offset: 0x000E6BBA
	public static float GetGroundOffset(GravityComponent gravityComponent)
	{
		if (gravityComponent.collider2D != null)
		{
			return gravityComponent.extents.y - gravityComponent.collider2D.offset.y;
		}
		return 0f;
	}

	// Token: 0x0600683F RID: 26687 RVA: 0x002E5F54 File Offset: 0x002E4154
	public static Vector2 GetExtents(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.bounds.extents;
		}
		return Vector2.zero;
	}

	// Token: 0x06006840 RID: 26688 RVA: 0x000E89EC File Offset: 0x000E6BEC
	public static Vector2 GetOffset(KCollider2D collider)
	{
		if (collider != null)
		{
			return collider.offset;
		}
		return Vector2.zero;
	}

	// Token: 0x04004EC0 RID: 20160
	public Transform transform;

	// Token: 0x04004EC1 RID: 20161
	public Vector2 velocity;

	// Token: 0x04004EC2 RID: 20162
	public float elapsedTime;

	// Token: 0x04004EC3 RID: 20163
	public System.Action onLanded;

	// Token: 0x04004EC4 RID: 20164
	public bool landOnFakeFloors;

	// Token: 0x04004EC5 RID: 20165
	public bool mayLeaveWorld;

	// Token: 0x04004EC6 RID: 20166
	public Vector2 extents;

	// Token: 0x04004EC7 RID: 20167
	public KCollider2D collider2D;
}
