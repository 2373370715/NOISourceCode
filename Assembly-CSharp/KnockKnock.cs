using System;

// Token: 0x02000AA8 RID: 2728
public class KnockKnock : Activatable
{
	// Token: 0x060031D1 RID: 12753 RVA: 0x000C4D4F File Offset: 0x000C2F4F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = false;
	}

	// Token: 0x060031D2 RID: 12754 RVA: 0x000C4D5E File Offset: 0x000C2F5E
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (!this.doorAnswered)
		{
			this.workTimeRemaining += dt;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x000C4D7E File Offset: 0x000C2F7E
	public void AnswerDoor()
	{
		this.doorAnswered = true;
		this.workTimeRemaining = 1f;
	}

	// Token: 0x04002219 RID: 8729
	private bool doorAnswered;
}
