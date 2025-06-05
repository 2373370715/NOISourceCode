using System;

// Token: 0x02001117 RID: 4375
internal abstract class DivisibleTask<SharedData> : IWorkItem<SharedData>
{
	// Token: 0x0600597E RID: 22910 RVA: 0x000DEC23 File Offset: 0x000DCE23
	public void Run(SharedData sharedData, int threadIndex)
	{
		this.RunDivision(sharedData);
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x000DEC2C File Offset: 0x000DCE2C
	protected DivisibleTask(string name)
	{
		this.name = name;
	}

	// Token: 0x06005980 RID: 22912
	protected abstract void RunDivision(SharedData sharedData);

	// Token: 0x04003FAD RID: 16301
	public string name;

	// Token: 0x04003FAE RID: 16302
	public int start;

	// Token: 0x04003FAF RID: 16303
	public int end;
}
