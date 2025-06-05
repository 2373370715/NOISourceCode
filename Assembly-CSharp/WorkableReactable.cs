using System;
using UnityEngine;

// Token: 0x02000839 RID: 2105
public class WorkableReactable : Reactable
{
	// Token: 0x06002531 RID: 9521 RVA: 0x001D8CB4 File Offset: 0x001D6EB4
	public WorkableReactable(Workable workable, HashedString id, ChoreType chore_type, WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any) : base(workable.gameObject, id, chore_type, 1, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
	{
		this.workable = workable;
		this.allowedDirection = allowed_direction;
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x001D8CF8 File Offset: 0x001D6EF8
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.workable == null)
		{
			return false;
		}
		if (this.reactor != null)
		{
			return false;
		}
		Brain component = new_reactor.GetComponent<Brain>();
		if (component == null)
		{
			return false;
		}
		if (!component.IsRunning())
		{
			return false;
		}
		Navigator component2 = new_reactor.GetComponent<Navigator>();
		if (component2 == null)
		{
			return false;
		}
		if (!component2.IsMoving())
		{
			return false;
		}
		if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
		{
			return true;
		}
		Facing component3 = new_reactor.GetComponent<Facing>();
		if (component3 == null)
		{
			return false;
		}
		bool facing = component3.GetFacing();
		return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000BCAEE File Offset: 0x000BACEE
	protected override void InternalBegin()
	{
		this.worker = this.reactor.GetComponent<WorkerBase>();
		this.worker.StartWork(new WorkerBase.StartWorkInfo(this.workable));
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000BCB17 File Offset: 0x000BAD17
	public override void Update(float dt)
	{
		if (this.worker.GetWorkable() == null)
		{
			base.End();
			return;
		}
		if (this.worker.Work(dt) != WorkerBase.WorkResult.InProgress)
		{
			base.End();
		}
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000BCB48 File Offset: 0x000BAD48
	protected override void InternalEnd()
	{
		if (this.worker != null)
		{
			this.worker.StopWork();
		}
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void InternalCleanup()
	{
	}

	// Token: 0x040019A8 RID: 6568
	protected Workable workable;

	// Token: 0x040019A9 RID: 6569
	private WorkerBase worker;

	// Token: 0x040019AA RID: 6570
	public WorkableReactable.AllowedDirection allowedDirection;

	// Token: 0x0200083A RID: 2106
	public enum AllowedDirection
	{
		// Token: 0x040019AC RID: 6572
		Any,
		// Token: 0x040019AD RID: 6573
		Left,
		// Token: 0x040019AE RID: 6574
		Right
	}
}
