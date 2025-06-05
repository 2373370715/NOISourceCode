using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001235 RID: 4661
public class CryoTank : StateMachineComponent<CryoTank.StatesInstance>, ISidescreenButtonControl
{
	// Token: 0x170005A3 RID: 1443
	// (get) Token: 0x06005E99 RID: 24217 RVA: 0x000E23C2 File Offset: 0x000E05C2
	public string SidescreenButtonText
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTON;
		}
	}

	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x06005E9A RID: 24218 RVA: 0x000E23CE File Offset: 0x000E05CE
	public string SidescreenButtonTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.CRYOTANK.DEFROSTBUTTONTOOLTIP;
		}
	}

	// Token: 0x06005E9B RID: 24219 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06005E9C RID: 24220 RVA: 0x000E23DA File Offset: 0x000E05DA
	public void OnSidescreenButtonPressed()
	{
		this.OnClickOpen();
	}

	// Token: 0x06005E9D RID: 24221 RVA: 0x000E23E2 File Offset: 0x000E05E2
	public bool SidescreenButtonInteractable()
	{
		return this.HasDefrostedFriend();
	}

	// Token: 0x06005E9E RID: 24222 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x06005E9F RID: 24223 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06005EA0 RID: 24224 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x06005EA1 RID: 24225 RVA: 0x002B0424 File Offset: 0x002AE624
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Demolishable component = base.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = !this.HasDefrostedFriend();
		}
	}

	// Token: 0x06005EA2 RID: 24226 RVA: 0x000E23EA File Offset: 0x000E05EA
	public bool HasDefrostedFriend()
	{
		return base.smi.IsInsideState(base.smi.sm.closed) && this.chore == null;
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x002B0464 File Offset: 0x002AE664
	public void DropContents()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(GameTags.Minions.Models.Standard, false, null, "AncientKnowledge", false);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPosCBC(Grid.OffsetCell(Grid.PosToCell(base.transform.position), this.dropOffset), Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
		gameObject.GetComponent<MinionIdentity>().arrivalTime = (float)UnityEngine.Random.Range(-2000, -1000);
		MinionResume component = gameObject.GetComponent<MinionResume>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			component.ForceAddSkillPoint();
		}
		base.smi.sm.defrostedDuplicant.Set(gameObject, base.smi, false);
		gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
		if (component2 != null)
		{
			base.smi.defrostAnimChore = new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, "anim_interacts_cryo_chamber_kanim", new HashedString[]
			{
				"defrost",
				"defrost_exit"
			}, KAnim.PlayMode.Once, false);
			Vector3 position2 = gameObject.transform.GetPosition();
			position2.z = Grid.GetLayerZ(Grid.SceneLayer.Gas);
			gameObject.transform.SetPosition(position2);
			gameObject.GetMyWorld().SetDupeVisited();
		}
		SaveGame.Instance.ColonyAchievementTracker.defrostedDuplicant = true;
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x002B0610 File Offset: 0x002AE810
	public void ShowEventPopup()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			SimpleEvent.StatesInstance statesInstance = GameplayEventManager.Instance.StartNewEvent(Db.Get().GameplayEvents.CryoFriend, -1, null).smi as SimpleEvent.StatesInstance;
			statesInstance.minions = new GameObject[]
			{
				gameObject,
				this.opener
			};
			statesInstance.SetTextParameter("dupe", this.opener.GetProperName());
			statesInstance.SetTextParameter("friend", gameObject.GetProperName());
			statesInstance.ShowEventPopup();
		}
	}

	// Token: 0x06005EA5 RID: 24229 RVA: 0x002B06BC File Offset: 0x002AE8BC
	public void Cheer()
	{
		GameObject gameObject = base.smi.sm.defrostedDuplicant.Get(base.smi);
		if (this.opener != null && gameObject != null)
		{
			Db db = Db.Get();
			this.opener.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(this.opener.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
			gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("CryoFriend"), true);
			new EmoteChore(gameObject.GetComponent<Effects>(), db.ChoreTypes.EmoteHighPriority, db.Emotes.Minion.Cheer, 1, null);
		}
	}

	// Token: 0x06005EA6 RID: 24230 RVA: 0x000E2414 File Offset: 0x000E0614
	private void OnClickOpen()
	{
		this.ActivateChore(null);
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x000E241D File Offset: 0x000E061D
	private void OnClickCancel()
	{
		this.CancelActivateChore(null);
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x002B07A8 File Offset: 0x002AE9A8
	public void ActivateChore(object param = null)
	{
		if (this.chore != null)
		{
			return;
		}
		base.GetComponent<Workable>().SetWorkTime(1.5f);
		this.chore = new WorkChore<Workable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, delegate(Chore o)
		{
			this.CompleteActivateChore();
		}, null, null, true, null, false, true, Assets.GetAnim(this.overrideAnim), false, true, true, PriorityScreen.PriorityClass.high, 5, false, true);
	}

	// Token: 0x06005EA9 RID: 24233 RVA: 0x000E2426 File Offset: 0x000E0626
	public void CancelActivateChore(object param = null)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x06005EAA RID: 24234 RVA: 0x002B0814 File Offset: 0x002AEA14
	private void CompleteActivateChore()
	{
		this.opener = this.chore.driver.gameObject;
		base.smi.GoTo(base.smi.sm.open);
		this.chore = null;
		Demolishable component = base.smi.GetComponent<Demolishable>();
		if (component != null)
		{
			component.allowDemolition = true;
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x040043A1 RID: 17313
	public string[][] possible_contents_ids;

	// Token: 0x040043A2 RID: 17314
	public string machineSound;

	// Token: 0x040043A3 RID: 17315
	public string overrideAnim;

	// Token: 0x040043A4 RID: 17316
	public CellOffset dropOffset = CellOffset.none;

	// Token: 0x040043A5 RID: 17317
	private GameObject opener;

	// Token: 0x040043A6 RID: 17318
	private Chore chore;

	// Token: 0x02001236 RID: 4662
	public class StatesInstance : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.GameInstance
	{
		// Token: 0x06005EAD RID: 24237 RVA: 0x000E2463 File Offset: 0x000E0663
		public StatesInstance(CryoTank master) : base(master)
		{
		}

		// Token: 0x040043A7 RID: 17319
		public Chore defrostAnimChore;
	}

	// Token: 0x02001237 RID: 4663
	public class States : GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank>
	{
		// Token: 0x06005EAE RID: 24238 RVA: 0x002B088C File Offset: 0x002AEA8C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.closed;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.closed.PlayAnim("on").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StartSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
			this.open.GoTo(this.defrost).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.DropContents();
			});
			this.defrost.PlayAnim("defrost").OnAnimQueueComplete(this.defrostExit).Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				smi.sm.defrostedDuplicant.Get(smi).GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				smi.master.ShowEventPopup();
			});
			this.defrostExit.PlayAnim("defrost_exit").Update(delegate(CryoTank.StatesInstance smi, float dt)
			{
				if (smi.defrostAnimChore == null || smi.defrostAnimChore.isComplete)
				{
					smi.GoTo(this.off);
				}
			}, UpdateRate.SIM_200ms, false).Exit(delegate(CryoTank.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.defrostedDuplicant.Get(smi);
				if (gameObject != null)
				{
					gameObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Move);
					smi.master.Cheer();
				}
			});
			this.off.PlayAnim("off").Enter(delegate(CryoTank.StatesInstance smi)
			{
				if (smi.master.machineSound != null)
				{
					LoopingSounds component = smi.master.GetComponent<LoopingSounds>();
					if (component != null)
					{
						component.StopSound(GlobalAssets.GetSound(smi.master.machineSound, false));
					}
				}
			});
		}

		// Token: 0x040043A8 RID: 17320
		public StateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.TargetParameter defrostedDuplicant;

		// Token: 0x040043A9 RID: 17321
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State closed;

		// Token: 0x040043AA RID: 17322
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State open;

		// Token: 0x040043AB RID: 17323
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrost;

		// Token: 0x040043AC RID: 17324
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State defrostExit;

		// Token: 0x040043AD RID: 17325
		public GameStateMachine<CryoTank.States, CryoTank.StatesInstance, CryoTank, object>.State off;
	}
}
