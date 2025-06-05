using System;
using UnityEngine;

// Token: 0x0200131B RID: 4891
public struct FallerComponent
{
	// Token: 0x06006423 RID: 25635 RVA: 0x002CAB60 File Offset: 0x002C8D60
	public FallerComponent(Transform transform, Vector2 initial_velocity)
	{
		this.transform = transform;
		this.transformInstanceId = transform.GetInstanceID();
		this.isFalling = false;
		this.initialVelocity = initial_velocity;
		this.partitionerEntry = default(HandleVector<int>.Handle);
		this.solidChangedCB = null;
		this.cellChangedCB = null;
		KCircleCollider2D component = transform.GetComponent<KCircleCollider2D>();
		if (component != null)
		{
			this.offset = component.radius;
			return;
		}
		KCollider2D component2 = transform.GetComponent<KCollider2D>();
		if (component2 != null)
		{
			this.offset = transform.GetPosition().y - component2.bounds.min.y;
			return;
		}
		this.offset = 0f;
	}

	// Token: 0x040047F3 RID: 18419
	public Transform transform;

	// Token: 0x040047F4 RID: 18420
	public int transformInstanceId;

	// Token: 0x040047F5 RID: 18421
	public bool isFalling;

	// Token: 0x040047F6 RID: 18422
	public float offset;

	// Token: 0x040047F7 RID: 18423
	public Vector2 initialVelocity;

	// Token: 0x040047F8 RID: 18424
	public HandleVector<int>.Handle partitionerEntry;

	// Token: 0x040047F9 RID: 18425
	public Action<object> solidChangedCB;

	// Token: 0x040047FA RID: 18426
	public System.Action cellChangedCB;
}
