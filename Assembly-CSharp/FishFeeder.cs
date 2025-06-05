using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DA8 RID: 3496
public class FishFeeder : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>
{
	// Token: 0x060043FB RID: 17403 RVA: 0x00254B8C File Offset: 0x00252D8C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.notoperational;
		this.root.Enter(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.SetupFishFeederTopAndBot)).Exit(new StateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State.Callback(FishFeeder.CleanupFishFeederTopAndBot)).EventHandler(GameHashes.OnStorageChange, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnStorageChange)).EventHandler(GameHashes.RefreshUserMenu, new GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameEvent.Callback(FishFeeder.OnRefreshUserMenu));
		this.notoperational.TagTransition(GameTags.Operational, this.operational, false);
		this.operational.DefaultState(this.operational.on).TagTransition(GameTags.Operational, this.notoperational, true);
		this.operational.on.DoNothing();
		int num = 19;
		FishFeeder.ballSymbols = new HashedString[num];
		for (int i = 0; i < num; i++)
		{
			FishFeeder.ballSymbols[i] = "ball" + i.ToString();
		}
	}

	// Token: 0x060043FC RID: 17404 RVA: 0x00254C84 File Offset: 0x00252E84
	private static void SetupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		Storage storage = smi.Get<Storage>();
		smi.fishFeederTop = new FishFeeder.FishFeederTop(smi, FishFeeder.ballSymbols, storage.Capacity());
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot = new FishFeeder.FishFeederBot(smi, 10f, FishFeeder.ballSymbols);
		smi.fishFeederBot.RefreshStorage();
		smi.fishFeederTop.ToggleMutantSeedFetches(smi.ForbidMutantSeeds);
		smi.UpdateMutantSeedStatusItem();
	}

	// Token: 0x060043FD RID: 17405 RVA: 0x000D05D6 File Offset: 0x000CE7D6
	private static void CleanupFishFeederTopAndBot(FishFeeder.Instance smi)
	{
		smi.fishFeederTop.Cleanup();
	}

	// Token: 0x060043FE RID: 17406 RVA: 0x00254CF4 File Offset: 0x00252EF4
	private static void MoveStoredContentsToConsumeOffset(FishFeeder.Instance smi)
	{
		foreach (GameObject gameObject in smi.GetComponent<Storage>().items)
		{
			if (!(gameObject == null))
			{
				FishFeeder.OnStorageChange(smi, gameObject);
			}
		}
	}

	// Token: 0x060043FF RID: 17407 RVA: 0x000D05E3 File Offset: 0x000CE7E3
	private static void OnStorageChange(FishFeeder.Instance smi, object data)
	{
		if ((GameObject)data == null)
		{
			return;
		}
		smi.fishFeederTop.RefreshStorage();
		smi.fishFeederBot.RefreshStorage();
	}

	// Token: 0x06004400 RID: 17408 RVA: 0x00254D58 File Offset: 0x00252F58
	private static void OnRefreshUserMenu(FishFeeder.Instance smi, object data)
	{
		if (DlcManager.FeatureRadiationEnabled())
		{
			Game.Instance.userMenu.AddButton(smi.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", smi.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				smi.ForbidMutantSeeds = !smi.ForbidMutantSeeds;
				FishFeeder.OnRefreshUserMenu(smi, null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.FISH_FEEDER_TOOLTIP, true), 1f);
		}
	}

	// Token: 0x04002F15 RID: 12053
	public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State notoperational;

	// Token: 0x04002F16 RID: 12054
	public FishFeeder.OperationalState operational;

	// Token: 0x04002F17 RID: 12055
	public static HashedString[] ballSymbols;

	// Token: 0x02000DA9 RID: 3497
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000DAA RID: 3498
	public class OperationalState : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State
	{
		// Token: 0x04002F18 RID: 12056
		public GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.State on;
	}

	// Token: 0x02000DAB RID: 3499
	public new class Instance : GameStateMachine<FishFeeder, FishFeeder.Instance, IStateMachineTarget, FishFeeder.Def>.GameInstance
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06004404 RID: 17412 RVA: 0x000D061A File Offset: 0x000CE81A
		// (set) Token: 0x06004405 RID: 17413 RVA: 0x000D0622 File Offset: 0x000CE822
		public bool ForbidMutantSeeds
		{
			get
			{
				return this.forbidMutantSeeds;
			}
			set
			{
				this.forbidMutantSeeds = value;
				this.fishFeederTop.ToggleMutantSeedFetches(this.forbidMutantSeeds);
				this.UpdateMutantSeedStatusItem();
			}
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x00254DE0 File Offset: 0x00252FE0
		public Instance(IStateMachineTarget master, FishFeeder.Def def) : base(master, def)
		{
			this.mutantSeedStatusItem = new StatusItem("FISHFEEDERACCEPTSMUTANTSEEDS", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
			base.Subscribe(-905833192, new Action<object>(this.OnCopySettingsDelegate));
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x00254E38 File Offset: 0x00253038
		private void OnCopySettingsDelegate(object data)
		{
			GameObject gameObject = (GameObject)data;
			if (gameObject == null)
			{
				return;
			}
			FishFeeder.Instance smi = gameObject.GetSMI<FishFeeder.Instance>();
			if (smi == null)
			{
				return;
			}
			this.ForbidMutantSeeds = smi.ForbidMutantSeeds;
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x000D0642 File Offset: 0x000CE842
		public void UpdateMutantSeedStatusItem()
		{
			base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(this.mutantSeedStatusItem, Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && !this.forbidMutantSeeds, null);
		}

		// Token: 0x04002F19 RID: 12057
		private StatusItem mutantSeedStatusItem;

		// Token: 0x04002F1A RID: 12058
		public FishFeeder.FishFeederTop fishFeederTop;

		// Token: 0x04002F1B RID: 12059
		public FishFeeder.FishFeederBot fishFeederBot;

		// Token: 0x04002F1C RID: 12060
		[Serialize]
		private bool forbidMutantSeeds;
	}

	// Token: 0x02000DAC RID: 3500
	public class FishFeederTop : IRenderEveryTick
	{
		// Token: 0x06004409 RID: 17417 RVA: 0x000D0674 File Offset: 0x000CE874
		public FishFeederTop(FishFeeder.Instance smi, HashedString[] ball_symbols, float capacity)
		{
			this.smi = smi;
			this.ballSymbols = ball_symbols;
			this.massPerBall = capacity / (float)ball_symbols.Length;
			this.FillFeeder(this.mass);
			SimAndRenderScheduler.instance.Add(this, false);
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00254E70 File Offset: 0x00253070
		private void FillFeeder(float mass)
		{
			KBatchedAnimController component = this.smi.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < this.ballSymbols.Length; i++)
			{
				bool is_visible = mass > (float)(i + 1) * this.massPerBall;
				component.SetSymbolVisiblity(this.ballSymbols[i], is_visible);
			}
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00254EC4 File Offset: 0x002530C4
		public void RefreshStorage()
		{
			float num = 0f;
			foreach (GameObject gameObject in this.smi.GetComponent<Storage>().items)
			{
				if (!(gameObject == null))
				{
					num += gameObject.GetComponent<PrimaryElement>().Mass;
				}
			}
			this.targetMass = num;
			this.timeSinceLastBallAppeared = 0f;
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00254F4C File Offset: 0x0025314C
		public void RenderEveryTick(float dt)
		{
			this.timeSinceLastBallAppeared += dt;
			if (Mathf.Abs(this.targetMass - this.mass) > 1f && this.timeSinceLastBallAppeared > 0.025f)
			{
				float num = Mathf.Min(this.massPerBall, this.targetMass - this.mass);
				this.mass += num;
				this.FillFeeder(this.mass);
				this.timeSinceLastBallAppeared = 0f;
			}
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x000C550D File Offset: 0x000C370D
		public void Cleanup()
		{
			SimAndRenderScheduler.instance.Remove(this);
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x00254FCC File Offset: 0x002531CC
		public void ToggleMutantSeedFetches(bool allow)
		{
			StorageLocker component = this.smi.GetComponent<StorageLocker>();
			if (component != null)
			{
				component.UpdateForbiddenTag(GameTags.MutatedSeed, !allow);
			}
		}

		// Token: 0x04002F1D RID: 12061
		private FishFeeder.Instance smi;

		// Token: 0x04002F1E RID: 12062
		private float mass;

		// Token: 0x04002F1F RID: 12063
		private float targetMass;

		// Token: 0x04002F20 RID: 12064
		private HashedString[] ballSymbols;

		// Token: 0x04002F21 RID: 12065
		private float massPerBall;

		// Token: 0x04002F22 RID: 12066
		private float timeSinceLastBallAppeared;
	}

	// Token: 0x02000DAD RID: 3501
	public class FishFeederBot
	{
		// Token: 0x0600440F RID: 17423 RVA: 0x00255000 File Offset: 0x00253200
		public FishFeederBot(FishFeeder.Instance smi, float mass_per_ball, HashedString[] ball_symbols)
		{
			this.smi = smi;
			this.massPerBall = mass_per_ball;
			this.anim = GameUtil.KInstantiate(Assets.GetPrefab("FishFeederBot"), smi.transform.GetPosition(), Grid.SceneLayer.Front, null, 0).GetComponent<KBatchedAnimController>();
			this.anim.transform.SetParent(smi.transform);
			this.anim.gameObject.SetActive(true);
			this.anim.SetSceneLayer(Grid.SceneLayer.Building);
			this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			this.anim.Stop();
			foreach (HashedString hash in ball_symbols)
			{
				this.anim.SetSymbolVisiblity(hash, false);
			}
			foreach (Storage storage in smi.gameObject.GetComponents<Storage>())
			{
				if (storage.storageID == "FishFeederBot")
				{
					this.botStorage = storage;
				}
				else if (storage.storageID == "FishFeederTop")
				{
					this.topStorage = storage;
				}
			}
			if (!this.botStorage.IsEmpty())
			{
				this.SetBallSymbol(this.botStorage.items[0].gameObject);
				this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x00255184 File Offset: 0x00253384
		public void RefreshStorage()
		{
			if (this.refreshingStorage)
			{
				return;
			}
			this.refreshingStorage = true;
			foreach (GameObject gameObject in this.botStorage.items)
			{
				if (!(gameObject == null))
				{
					int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
					gameObject.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore));
				}
			}
			if (this.botStorage.IsEmpty())
			{
				float num = 0f;
				foreach (GameObject gameObject2 in this.topStorage.items)
				{
					if (!(gameObject2 == null))
					{
						num += gameObject2.GetComponent<PrimaryElement>().Mass;
					}
				}
				if (num > 0f)
				{
					Pickupable pickupable = this.topStorage.items[0].GetComponent<Pickupable>().Take(this.massPerBall);
					this.botStorage.Store(pickupable.gameObject, false, false, true, false);
					this.SetBallSymbol(pickupable.gameObject);
					this.anim.Play("ball", KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, false);
				}
			}
			this.refreshingStorage = false;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x00255320 File Offset: 0x00253520
		private void SetBallSymbol(GameObject stored_go)
		{
			if (stored_go == null)
			{
				return;
			}
			this.anim.SetSymbolVisiblity(FishFeeder.FishFeederBot.HASH_FEEDBALL, true);
			KAnim.Build build = stored_go.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
			KAnim.Build.Symbol symbol = stored_go.HasTag(GameTags.Seed) ? build.GetSymbol("object") : build.GetSymbol("algae");
			if (symbol != null)
			{
				this.anim.GetComponent<SymbolOverrideController>().AddSymbolOverride(FishFeeder.FishFeederBot.HASH_FEEDBALL, symbol, 0);
			}
			HashedString batchGroupOverride = new HashedString("FishFeeder" + stored_go.GetComponent<KPrefabID>().PrefabTag.Name);
			this.anim.SetBatchGroupOverride(batchGroupOverride);
			int cell = Grid.CellBelow(Grid.CellBelow(Grid.PosToCell(this.smi.transform.GetPosition())));
			stored_go.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingUse));
		}

		// Token: 0x04002F23 RID: 12067
		private KBatchedAnimController anim;

		// Token: 0x04002F24 RID: 12068
		private Storage topStorage;

		// Token: 0x04002F25 RID: 12069
		private Storage botStorage;

		// Token: 0x04002F26 RID: 12070
		private bool refreshingStorage;

		// Token: 0x04002F27 RID: 12071
		private FishFeeder.Instance smi;

		// Token: 0x04002F28 RID: 12072
		private float massPerBall;

		// Token: 0x04002F29 RID: 12073
		private static readonly HashedString HASH_FEEDBALL = "feedball";
	}
}
