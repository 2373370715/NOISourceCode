using System;
using Klei.AI;

// Token: 0x02000BB5 RID: 2997
public abstract class WorkerBase : KMonoBehaviour
{
	// Token: 0x06003898 RID: 14488
	public abstract bool UsesMultiTool();

	// Token: 0x06003899 RID: 14489
	public abstract bool IsFetchDrone();

	// Token: 0x0600389A RID: 14490
	public abstract KBatchedAnimController GetAnimController();

	// Token: 0x0600389B RID: 14491
	public abstract WorkerBase.State GetState();

	// Token: 0x0600389C RID: 14492
	public abstract WorkerBase.StartWorkInfo GetStartWorkInfo();

	// Token: 0x0600389D RID: 14493
	public abstract Workable GetWorkable();

	// Token: 0x0600389E RID: 14494
	public abstract Attributes GetAttributes();

	// Token: 0x0600389F RID: 14495
	public abstract AttributeConverterInstance GetAttributeConverter(string id);

	// Token: 0x060038A0 RID: 14496
	public abstract Guid OfferStatusItem(StatusItem item, object data = null);

	// Token: 0x060038A1 RID: 14497
	public abstract void RevokeStatusItem(Guid id);

	// Token: 0x060038A2 RID: 14498
	public abstract void StartWork(WorkerBase.StartWorkInfo start_work_info);

	// Token: 0x060038A3 RID: 14499
	public abstract void StopWork();

	// Token: 0x060038A4 RID: 14500
	public abstract bool InstantlyFinish();

	// Token: 0x060038A5 RID: 14501
	public abstract WorkerBase.WorkResult Work(float dt);

	// Token: 0x060038A6 RID: 14502
	public abstract void SetWorkCompleteData(object data);

	// Token: 0x02000BB6 RID: 2998
	public class StartWorkInfo
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x000C9264 File Offset: 0x000C7464
		// (set) Token: 0x060038A9 RID: 14505 RVA: 0x000C926C File Offset: 0x000C746C
		public Workable workable { get; set; }

		// Token: 0x060038AA RID: 14506 RVA: 0x000C9275 File Offset: 0x000C7475
		public StartWorkInfo(Workable workable)
		{
			this.workable = workable;
		}
	}

	// Token: 0x02000BB7 RID: 2999
	public enum State
	{
		// Token: 0x0400270C RID: 9996
		Idle,
		// Token: 0x0400270D RID: 9997
		Working,
		// Token: 0x0400270E RID: 9998
		PendingCompletion,
		// Token: 0x0400270F RID: 9999
		Completing
	}

	// Token: 0x02000BB8 RID: 3000
	public enum WorkResult
	{
		// Token: 0x04002711 RID: 10001
		Success,
		// Token: 0x04002712 RID: 10002
		InProgress,
		// Token: 0x04002713 RID: 10003
		Failed
	}
}
