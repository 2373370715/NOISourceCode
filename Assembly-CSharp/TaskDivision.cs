using System;

// Token: 0x02001118 RID: 4376
internal class TaskDivision<Task, SharedData> where Task : DivisibleTask<SharedData>, new()
{
	// Token: 0x06005981 RID: 22913 RVA: 0x0029DBC4 File Offset: 0x0029BDC4
	public TaskDivision(int taskCount)
	{
		this.tasks = new Task[taskCount];
		for (int num = 0; num != this.tasks.Length; num++)
		{
			this.tasks[num] = Activator.CreateInstance<Task>();
		}
	}

	// Token: 0x06005982 RID: 22914 RVA: 0x000DEC3B File Offset: 0x000DCE3B
	public TaskDivision() : this(CPUBudget.coreCount)
	{
	}

	// Token: 0x06005983 RID: 22915 RVA: 0x0029DC08 File Offset: 0x0029BE08
	public void Initialize(int count)
	{
		int num = count / this.tasks.Length;
		for (int num2 = 0; num2 != this.tasks.Length; num2++)
		{
			this.tasks[num2].start = num2 * num;
			this.tasks[num2].end = this.tasks[num2].start + num;
		}
		DebugUtil.Assert(this.tasks[this.tasks.Length - 1].end + count % this.tasks.Length == count);
		this.tasks[this.tasks.Length - 1].end = count;
	}

	// Token: 0x06005984 RID: 22916 RVA: 0x0029DCCC File Offset: 0x0029BECC
	public void Run(SharedData sharedData, int threadIndex)
	{
		Task[] array = this.tasks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Run(sharedData, threadIndex);
		}
	}

	// Token: 0x04003FB0 RID: 16304
	public Task[] tasks;
}
