using System;
using STRINGS;
using UnityEngine;

// Token: 0x02000747 RID: 1863
public class SeekAndInstallBionicUpgradeChore : Chore<SeekAndInstallBionicUpgradeChore.Instance>
{
	// Token: 0x060020BA RID: 8378 RVA: 0x001C95EC File Offset: 0x001C77EC
	public SeekAndInstallBionicUpgradeChore(IStateMachineTarget target)
	{
		Chore.Precondition canPickupAnyAssignedUpgrade = default(Chore.Precondition);
		canPickupAnyAssignedUpgrade.id = "CanPickupAnyAssignedUpgrade";
		canPickupAnyAssignedUpgrade.description = DUPLICANTS.CHORES.PRECONDITIONS.CANPICKUPANYASSIGNEDUPGRADE;
		canPickupAnyAssignedUpgrade.fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return ((BionicUpgradesMonitor.Instance)data).GetAnyReachableAssignedSlot() != null;
		};
		canPickupAnyAssignedUpgrade.canExecuteOnAnyThread = false;
		this.CanPickupAnyAssignedUpgrade = canPickupAnyAssignedUpgrade;
		base..ctor(Db.Get().ChoreTypes.SeekAndInstallUpgrade, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime);
		base.smi = new SeekAndInstallBionicUpgradeChore.Instance(this, target.gameObject);
		BionicUpgradesMonitor.Instance smi = target.gameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		this.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
		this.AddPrecondition(this.CanPickupAnyAssignedUpgrade, smi);
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x001C96B8 File Offset: 0x001C78B8
	public override void Begin(Chore.Precondition.Context context)
	{
		if (context.consumerState.consumer == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null context.consumer");
			return;
		}
		BionicUpgradesMonitor.Instance smi = context.consumerState.consumer.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null BionicUpgradesMonitor.Instance");
			return;
		}
		BionicUpgradesMonitor.UpgradeComponentSlot anyReachableAssignedSlot = smi.GetAnyReachableAssignedSlot();
		BionicUpgradeComponent bionicUpgradeComponent = (anyReachableAssignedSlot == null) ? null : anyReachableAssignedSlot.assignedUpgradeComponent;
		if (bionicUpgradeComponent == null)
		{
			global::Debug.LogError("SeekAndInstallBionicUpgradeChore null upgradeComponent.gameObject");
			return;
		}
		base.smi.sm.initialUpgradeComponent.Set(bionicUpgradeComponent.gameObject, base.smi, false);
		base.smi.sm.dupe.Set(context.consumerState.consumer, base.smi);
		base.Begin(context);
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x001C977C File Offset: 0x001C797C
	public static void SetOverrideAnimSymbol(SeekAndInstallBionicUpgradeChore.Instance smi, bool overriding)
	{
		string text = "booster";
		KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component2 = smi.gameObject.GetComponent<SymbolOverrideController>();
		GameObject gameObject = smi.sm.pickedUpgrade.Get(smi);
		if (gameObject != null)
		{
			gameObject.GetComponent<KBatchedAnimTracker>().enabled = !overriding;
			Storage.MakeItemInvisible(gameObject, overriding, false);
		}
		if (!overriding)
		{
			component2.RemoveSymbolOverride(text, 0);
			component.SetSymbolVisiblity(text, false);
			return;
		}
		string animStateName = BionicUpgradeComponentConfig.UpgradesData[gameObject.PrefabID()].animStateName;
		KAnim.Build.Symbol symbol = gameObject.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(animStateName);
		component2.AddSymbolOverride(text, symbol, 0);
		component.SetSymbolVisiblity(text, true);
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x001C984C File Offset: 0x001C7A4C
	public static bool IsBionicUpgradeAssignedTo(GameObject bionicUpgradeGameObject, GameObject ownerInQuestion)
	{
		if (bionicUpgradeGameObject == null)
		{
			return false;
		}
		Assignable component = bionicUpgradeGameObject.GetComponent<BionicUpgradeComponent>();
		IAssignableIdentity component2 = ownerInQuestion.GetComponent<IAssignableIdentity>();
		return component.IsAssignedTo(component2);
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x001C9878 File Offset: 0x001C7A78
	public static void InstallUpgrade(SeekAndInstallBionicUpgradeChore.Instance smi)
	{
		Storage storage = smi.gameObject.GetComponents<Storage>().FindFirst((Storage s) => s.storageID == GameTags.StoragesIds.DefaultStorage);
		GameObject gameObject = storage.FindFirst(GameTags.BionicUpgrade);
		if (gameObject != null)
		{
			BionicUpgradeComponent component = gameObject.GetComponent<BionicUpgradeComponent>();
			storage.Remove(component.gameObject, true);
			smi.upgradeMonitor.InstallUpgrade(component);
			if (PopFXManager.Instance != null)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, component.GetProperName(), smi.gameObject.transform, Vector3.up, 1.5f, true, false);
			}
		}
	}

	// Token: 0x040015CB RID: 5579
	private Chore.Precondition CanPickupAnyAssignedUpgrade;

	// Token: 0x02000748 RID: 1864
	public class States : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore>
	{
		// Token: 0x060020BF RID: 8383 RVA: 0x001C992C File Offset: 0x001C7B2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.fetch;
			base.Target(this.dupe);
			this.fetch.InitializeStates(this.dupe, this.initialUpgradeComponent, this.pickedUpgrade, this.amountRequested, this.actualunits, this.install, null).Target(this.initialUpgradeComponent).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.initialUpgradeComponent.Get(smi), smi.gameObject));
			this.install.Target(this.dupe).ToggleAnims("anim_bionic_booster_installation_kanim", 0f).PlayAnim("installation", KAnim.PlayMode.Once).Enter(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, true);
			}).Exit(delegate(SeekAndInstallBionicUpgradeChore.Instance smi)
			{
				SeekAndInstallBionicUpgradeChore.SetOverrideAnimSymbol(smi, false);
			}).OnAnimQueueComplete(this.complete).ScheduleGoTo(10f, this.complete).Target(this.pickedUpgrade).EventHandlerTransition(GameHashes.AssigneeChanged, null, (SeekAndInstallBionicUpgradeChore.Instance smi, object obj) => !SeekAndInstallBionicUpgradeChore.IsBionicUpgradeAssignedTo(smi.sm.pickedUpgrade.Get(smi), smi.gameObject));
			this.complete.Target(this.dupe).Enter(new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State.Callback(SeekAndInstallBionicUpgradeChore.InstallUpgrade)).ReturnSuccess();
		}

		// Token: 0x040015CC RID: 5580
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FetchSubState fetch;

		// Token: 0x040015CD RID: 5581
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State install;

		// Token: 0x040015CE RID: 5582
		public GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.State complete;

		// Token: 0x040015CF RID: 5583
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter dupe;

		// Token: 0x040015D0 RID: 5584
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter initialUpgradeComponent;

		// Token: 0x040015D1 RID: 5585
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.TargetParameter pickedUpgrade;

		// Token: 0x040015D2 RID: 5586
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter actualunits;

		// Token: 0x040015D3 RID: 5587
		public StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter amountRequested = new StateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.FloatParameter(1f);
	}

	// Token: 0x0200074A RID: 1866
	public class Instance : GameStateMachine<SeekAndInstallBionicUpgradeChore.States, SeekAndInstallBionicUpgradeChore.Instance, SeekAndInstallBionicUpgradeChore, object>.GameInstance
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060020C7 RID: 8391 RVA: 0x000B9EE0 File Offset: 0x000B80E0
		public BionicUpgradesMonitor.Instance upgradeMonitor
		{
			get
			{
				return base.sm.dupe.Get(this).GetSMI<BionicUpgradesMonitor.Instance>();
			}
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000B9EF8 File Offset: 0x000B80F8
		public Instance(SeekAndInstallBionicUpgradeChore master, GameObject duplicant) : base(master)
		{
		}
	}
}
