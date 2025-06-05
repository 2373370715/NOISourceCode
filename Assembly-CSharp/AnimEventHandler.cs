using System;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
[AddComponentMenu("KMonoBehaviour/scripts/AnimEventHandler")]
public class AnimEventHandler : KMonoBehaviour
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06002C76 RID: 11382 RVA: 0x001F9C70 File Offset: 0x001F7E70
	// (remove) Token: 0x06002C77 RID: 11383 RVA: 0x001F9CA8 File Offset: 0x001F7EA8
	private event AnimEventHandler.SetPos onWorkTargetSet;

	// Token: 0x06002C78 RID: 11384 RVA: 0x000C1422 File Offset: 0x000BF622
	public int GetCachedCell()
	{
		return this.pickupable.cachedCell;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x001F9CE0 File Offset: 0x001F7EE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.cachedTransform = base.transform;
		this.pickupable = base.GetComponent<Pickupable>();
		foreach (KBatchedAnimTracker kbatchedAnimTracker in base.GetComponentsInChildren<KBatchedAnimTracker>(true))
		{
			if (kbatchedAnimTracker.useTargetPoint)
			{
				this.onWorkTargetSet += kbatchedAnimTracker.SetTarget;
			}
		}
		this.baseOffset = this.animCollider.offset;
		AnimEventHandlerManager.Instance.Add(this);
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000C142F File Offset: 0x000BF62F
	protected override void OnCleanUp()
	{
		AnimEventHandlerManager.Instance.Remove(this);
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000C143C File Offset: 0x000BF63C
	protected override void OnForcedCleanUp()
	{
		this.navigator = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000C144B File Offset: 0x000BF64B
	public HashedString GetContext()
	{
		return this.context;
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x000C1453 File Offset: 0x000BF653
	public void UpdateWorkTarget(Vector3 pos)
	{
		if (this.onWorkTargetSet != null)
		{
			this.onWorkTargetSet(pos);
		}
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x000C1469 File Offset: 0x000BF669
	public void SetContext(HashedString context)
	{
		this.context = context;
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x000C1472 File Offset: 0x000BF672
	public void SetTargetPos(Vector3 target_pos)
	{
		this.targetPos = target_pos;
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000C147B File Offset: 0x000BF67B
	public Vector3 GetTargetPos()
	{
		return this.targetPos;
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000C1483 File Offset: 0x000BF683
	public void ClearContext()
	{
		this.context = default(HashedString);
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x001F9D5C File Offset: 0x001F7F5C
	public void UpdateOffset()
	{
		Vector3 pivotSymbolPosition = this.controller.GetPivotSymbolPosition();
		Vector3 vector = this.navigator.NavGrid.GetNavTypeData(this.navigator.CurrentNavType).animControllerOffset;
		Vector3 position = this.cachedTransform.position;
		Vector2 vector2 = new Vector2(this.baseOffset.x + pivotSymbolPosition.x - position.x - vector.x, this.baseOffset.y + pivotSymbolPosition.y - position.y + vector.y);
		if (this.animCollider.offset != vector2)
		{
			this.animCollider.offset = vector2;
		}
	}

	// Token: 0x04001E79 RID: 7801
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x04001E7A RID: 7802
	[MyCmpGet]
	private KBoxCollider2D animCollider;

	// Token: 0x04001E7B RID: 7803
	[MyCmpGet]
	private Navigator navigator;

	// Token: 0x04001E7C RID: 7804
	private Pickupable pickupable;

	// Token: 0x04001E7D RID: 7805
	private Vector3 targetPos;

	// Token: 0x04001E7E RID: 7806
	public Transform cachedTransform;

	// Token: 0x04001E80 RID: 7808
	public Vector2 baseOffset;

	// Token: 0x04001E81 RID: 7809
	private HashedString context;

	// Token: 0x020009B3 RID: 2483
	// (Invoke) Token: 0x06002C85 RID: 11397
	private delegate void SetPos(Vector3 pos);
}
