using System;
using System.Collections.Generic;

// Token: 0x020007AF RID: 1967
public class VoidChoreProvider : ChoreProvider
{
	// Token: 0x060022D9 RID: 8921 RVA: 0x000BB17A File Offset: 0x000B937A
	public static void DestroyInstance()
	{
		VoidChoreProvider.Instance = null;
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000BB182 File Offset: 0x000B9382
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		VoidChoreProvider.Instance = this;
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void AddChore(Chore chore)
	{
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void RemoveChore(Chore chore)
	{
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded, List<Chore.Precondition.Context> failed_contexts)
	{
	}

	// Token: 0x0400175E RID: 5982
	public static VoidChoreProvider Instance;
}
