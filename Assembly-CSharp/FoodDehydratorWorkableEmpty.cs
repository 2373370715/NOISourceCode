using System;

// Token: 0x02000DBE RID: 3518
public class FoodDehydratorWorkableEmpty : Workable
{
	// Token: 0x06004481 RID: 17537 RVA: 0x000D0B77 File Offset: 0x000CED77
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.workAnims = FoodDehydratorWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = FoodDehydratorWorkableEmpty.WORK_ANIMS_PST;
		this.workingPstFailed = FoodDehydratorWorkableEmpty.WORK_ANIMS_FAIL_PST;
	}

	// Token: 0x04002F89 RID: 12169
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"empty_pre",
		"empty_loop"
	};

	// Token: 0x04002F8A RID: 12170
	private static readonly HashedString[] WORK_ANIMS_PST = new HashedString[]
	{
		"empty_pst"
	};

	// Token: 0x04002F8B RID: 12171
	private static readonly HashedString[] WORK_ANIMS_FAIL_PST = new HashedString[]
	{
		""
	};
}
