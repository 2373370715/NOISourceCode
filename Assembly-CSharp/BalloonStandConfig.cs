using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class BalloonStandConfig : IEntityConfig
{
	// Token: 0x06001223 RID: 4643 RVA: 0x0019219C File Offset: 0x0019039C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(BalloonStandConfig.ID, BalloonStandConfig.ID, false);
		KAnimFile[] overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_balloon_receiver_kanim")
		};
		GetBalloonWorkable getBalloonWorkable = gameObject.AddOrGet<GetBalloonWorkable>();
		getBalloonWorkable.workTime = 2f;
		getBalloonWorkable.workLayer = Grid.SceneLayer.BuildingFront;
		getBalloonWorkable.overrideAnims = overrideAnims;
		getBalloonWorkable.synchronizeAnims = false;
		return gameObject;
	}

	// Token: 0x06001224 RID: 4644 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06001225 RID: 4645 RVA: 0x001921FC File Offset: 0x001903FC
	public void OnSpawn(GameObject inst)
	{
		GetBalloonWorkable component = inst.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x0019227C File Offset: 0x0019047C
	private void MakeNewBalloonChore(Chore chore)
	{
		GetBalloonWorkable component = chore.target.GetComponent<GetBalloonWorkable>();
		WorkChore<GetBalloonWorkable> workChore = new WorkChore<GetBalloonWorkable>(Db.Get().ChoreTypes.JoyReaction, component, null, true, new Action<Chore>(this.MakeNewBalloonChore), null, null, true, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, true, PriorityScreen.PriorityClass.high, 5, true, true);
		workChore.AddPrecondition(this.HasNoBalloon, workChore);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		component.GetBalloonArtist().NextBalloonOverride();
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x00192300 File Offset: 0x00190500
	public BalloonStandConfig()
	{
		Chore.Precondition hasNoBalloon = default(Chore.Precondition);
		hasNoBalloon.id = "HasNoBalloon";
		hasNoBalloon.description = "__ Duplicant doesn't have a balloon already";
		hasNoBalloon.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return !(context.consumerState.consumer == null) && !context.consumerState.gameObject.GetComponent<Effects>().HasEffect("HasBalloon");
		};
		this.HasNoBalloon = hasNoBalloon;
		base..ctor();
	}

	// Token: 0x04000CA3 RID: 3235
	public static readonly string ID = "BalloonStand";

	// Token: 0x04000CA4 RID: 3236
	private Chore.Precondition HasNoBalloon;
}
