using System;
using Database;
using UnityEngine;

// Token: 0x02000E01 RID: 3585
[AddComponentMenu("KMonoBehaviour/Workable/GetBalloonWorkable")]
public class GetBalloonWorkable : Workable
{
	// Token: 0x06004606 RID: 17926 RVA: 0x0025BF08 File Offset: 0x0025A108
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = null;
		this.workingStatusItem = null;
		this.workAnims = GetBalloonWorkable.GET_BALLOON_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			GetBalloonWorkable.PST_ANIM
		};
	}

	// Token: 0x06004607 RID: 17927 RVA: 0x0025BF6C File Offset: 0x0025A16C
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		if (balloonOverride.animFile.IsNone())
		{
			worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
			return;
		}
		worker.gameObject.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverride.symbol.Unwrap(), 0);
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x0025C008 File Offset: 0x0025A208
	protected override void OnCompleteWork(WorkerBase worker)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EquippableBalloon"), worker.transform.GetPosition());
		gameObject.GetComponent<Equippable>().Assign(worker.GetComponent<MinionIdentity>());
		gameObject.GetComponent<Equippable>().isEquipped = true;
		gameObject.SetActive(true);
		base.OnCompleteWork(worker);
		BalloonOverrideSymbol balloonOverride = this.balloonArtist.GetBalloonOverride();
		this.balloonArtist.GiveBalloon(balloonOverride);
		gameObject.GetComponent<EquippableBalloon>().SetBalloonOverride(balloonOverride);
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x000D1BC5 File Offset: 0x000CFDC5
	public override Vector3 GetFacingTarget()
	{
		return this.balloonArtist.master.transform.GetPosition();
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x000D1BDC File Offset: 0x000CFDDC
	public void SetBalloonArtist(BalloonArtistChore.StatesInstance chore)
	{
		this.balloonArtist = chore;
	}

	// Token: 0x0600460B RID: 17931 RVA: 0x000D1BE5 File Offset: 0x000CFDE5
	public BalloonArtistChore.StatesInstance GetBalloonArtist()
	{
		return this.balloonArtist;
	}

	// Token: 0x040030DA RID: 12506
	private static readonly HashedString[] GET_BALLOON_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040030DB RID: 12507
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x040030DC RID: 12508
	private BalloonArtistChore.StatesInstance balloonArtist;

	// Token: 0x040030DD RID: 12509
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x040030DE RID: 12510
	private const int TARGET_OVERRIDE_PRIORITY = 0;
}
