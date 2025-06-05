using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000CD4 RID: 3284
[AddComponentMenu("KMonoBehaviour/Workable/AstronautTrainingCenter")]
public class AstronautTrainingCenter : Workable
{
	// Token: 0x06003EAE RID: 16046 RVA: 0x000CD29D File Offset: 0x000CB49D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.chore = this.CreateChore();
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x00243788 File Offset: 0x00241988
	private Chore CreateChore()
	{
		return new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, this, null, true, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x000CD2B1 File Offset: 0x000CB4B1
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06003EB1 RID: 16049 RVA: 0x000CD2C7 File Offset: 0x000CB4C7
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		worker == null;
		return true;
	}

	// Token: 0x06003EB2 RID: 16050 RVA: 0x000CD2D2 File Offset: 0x000CB4D2
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		if (this.chore != null && !this.chore.isComplete)
		{
			this.chore.Cancel("completed but not complete??");
		}
		this.chore = this.CreateChore();
	}

	// Token: 0x06003EB3 RID: 16051 RVA: 0x000CD30C File Offset: 0x000CB50C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<Operational>().SetActive(false, false);
	}

	// Token: 0x06003EB4 RID: 16052 RVA: 0x000CD322 File Offset: 0x000CB522
	public override float GetPercentComplete()
	{
		base.worker == null;
		return 0f;
	}

	// Token: 0x06003EB5 RID: 16053 RVA: 0x002437BC File Offset: 0x002419BC
	public AstronautTrainingCenter()
	{
		Chore.Precondition isNotMarkedForDeconstruction = default(Chore.Precondition);
		isNotMarkedForDeconstruction.id = "IsNotMarkedForDeconstruction";
		isNotMarkedForDeconstruction.description = DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION;
		isNotMarkedForDeconstruction.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			Deconstructable deconstructable = data as Deconstructable;
			return deconstructable == null || !deconstructable.IsMarkedForDeconstruction();
		};
		this.IsNotMarkedForDeconstruction = isNotMarkedForDeconstruction;
		base..ctor();
	}

	// Token: 0x04002B5B RID: 11099
	public float daysToMasterRole;

	// Token: 0x04002B5C RID: 11100
	private Chore chore;

	// Token: 0x04002B5D RID: 11101
	public Chore.Precondition IsNotMarkedForDeconstruction;
}
