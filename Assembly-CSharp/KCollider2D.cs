using System;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
public abstract class KCollider2D : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x170001FF RID: 511
	// (get) Token: 0x0600319C RID: 12700 RVA: 0x000C4A45 File Offset: 0x000C2C45
	// (set) Token: 0x0600319D RID: 12701 RVA: 0x000C4A4D File Offset: 0x000C2C4D
	public Vector2 offset
	{
		get
		{
			return this._offset;
		}
		set
		{
			this._offset = value;
			this.MarkDirty(false);
		}
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x000C4A5D File Offset: 0x000C2C5D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x000C4A6C File Offset: 0x000C2C6C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		this.MarkDirty(true);
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x000C4A97 File Offset: 0x000C2C97
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(base.transform, new Action<Transform, bool>(KCollider2D.OnMovementStateChanged));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x0020D630 File Offset: 0x0020B830
	public void MarkDirty(bool force = false)
	{
		bool flag = force || this.partitionerEntry.IsValid();
		if (!flag)
		{
			return;
		}
		Extents extents = this.GetExtents();
		if (!force && this.cachedExtents.x == extents.x && this.cachedExtents.y == extents.y && this.cachedExtents.width == extents.width && this.cachedExtents.height == extents.height)
		{
			return;
		}
		this.cachedExtents = extents;
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (flag)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add(null, this, this.cachedExtents, GameScenePartitioner.Instance.collisionLayer, null);
		}
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x000C4ACB File Offset: 0x000C2CCB
	private void OnMovementStateChanged(bool is_moving)
	{
		if (is_moving)
		{
			this.MarkDirty(false);
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x000C4AEF File Offset: 0x000C2CEF
	private static void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		transform.GetComponent<KCollider2D>().OnMovementStateChanged(is_moving);
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x000C4AFD File Offset: 0x000C2CFD
	public void RenderEveryTick(float dt)
	{
		this.MarkDirty(false);
	}

	// Token: 0x060031A5 RID: 12709
	public abstract bool Intersects(Vector2 pos);

	// Token: 0x060031A6 RID: 12710
	public abstract Extents GetExtents();

	// Token: 0x17000200 RID: 512
	// (get) Token: 0x060031A7 RID: 12711
	public abstract Bounds bounds { get; }

	// Token: 0x0400220A RID: 8714
	[SerializeField]
	public Vector2 _offset;

	// Token: 0x0400220B RID: 8715
	private Extents cachedExtents;

	// Token: 0x0400220C RID: 8716
	private HandleVector<int>.Handle partitionerEntry;
}
