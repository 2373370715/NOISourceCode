using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ED6 RID: 3798
public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
	// Token: 0x06004BF7 RID: 19447 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06004BF8 RID: 19448 RVA: 0x0026E43C File Offset: 0x0026C63C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, base.gameObject.GetMyWorldId());
		base.smi.StartSM();
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
		base.GetComponent<Activatable>().SetWorkTime(5f);
		base.smi.JournalDelivery.refillMass = 25f;
		base.smi.JournalDelivery.FillToCapacity = true;
	}

	// Token: 0x06004BF9 RID: 19449 RVA: 0x000D580B File Offset: 0x000D3A0B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(-1503271301);
	}

	// Token: 0x06004BFA RID: 19450 RVA: 0x0026E4CC File Offset: 0x0026C6CC
	private void OnBuildingSelect(object obj)
	{
		if (!(bool)obj)
		{
			return;
		}
		if (!this.introDisplayed)
		{
			this.introDisplayed = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, new System.Action(this.DoInitialUnlock)));
		}
		base.smi.ShowEventCompleteUI(null);
	}

	// Token: 0x06004BFB RID: 19451 RVA: 0x000D581E File Offset: 0x000D3A1E
	private void DoInitialUnlock()
	{
		Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial", true);
	}

	// Token: 0x0400352A RID: 13610
	[Serialize]
	private bool introDisplayed;

	// Token: 0x02000ED7 RID: 3799
	public class States : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank>
	{
		// Token: 0x06004BFD RID: 19453 RVA: 0x0026E53C File Offset: 0x0026C73C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.root;
			this.root.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				if (!StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
				{
					smi.GoTo(this.common.dormant);
					return;
				}
				if (smi.IsHungry)
				{
					smi.GoTo(this.common.idle);
					return;
				}
				smi.GoTo(this.common.active);
			});
			this.common.Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.IncrementMeter(dt);
				if (smi.UnitsFromLastStore != 0)
				{
					smi.ShelveJournals(dt);
				}
				bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, true);
				smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag, null);
			}, UpdateRate.SIM_33ms, false);
			this.common.dormant.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.SetBonusActive(false);
				smi.ElementConverter.SetAllConsumedActive(false);
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, false);
				smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, false);
				smi.master.GetComponent<Light2D>().enabled = false;
			}).Exit(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, true);
				smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, true);
				RequireInputs component = smi.GetComponent<RequireInputs>();
				component.requireConduitHasMass = true;
				component.visualizeRequirements = RequireInputs.Requirements.All;
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.ActivateBrains(dt);
			}, UpdateRate.SIM_33ms, false).OnSignal(this.storyTraitCompleted, this.common.active);
			this.common.idle.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(false);
			}).UpdateTransition(this.common.active, (MegaBrainTank.StatesInstance smi, float _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.common.active.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(true);
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.Digest(dt);
			}, UpdateRate.SIM_33ms, false).UpdateTransition(this.common.idle, (MegaBrainTank.StatesInstance smi, float _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.StatBonus = new Effect("MegaBrainTankBonus", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
			object[,] stat_BONUSES = MegaBrainTankConfig.STAT_BONUSES;
			int length = stat_BONUSES.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				string attribute_id = stat_BONUSES[i, 0] as string;
				float? num = (float?)stat_BONUSES[i, 1];
				Units? units = (Units?)stat_BONUSES[i, 2];
				this.StatBonus.Add(new AttributeModifier(attribute_id, ModifierSet.ConvertValue(num.Value, units.Value), DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, false, false, true));
			}
		}

		// Token: 0x0400352B RID: 13611
		public MegaBrainTank.States.CommonState common;

		// Token: 0x0400352C RID: 13612
		public StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.Signal storyTraitCompleted;

		// Token: 0x0400352D RID: 13613
		public Effect StatBonus;

		// Token: 0x02000ED8 RID: 3800
		public class CommonState : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State
		{
			// Token: 0x0400352E RID: 13614
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State dormant;

			// Token: 0x0400352F RID: 13615
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State idle;

			// Token: 0x04003530 RID: 13616
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State active;
		}
	}

	// Token: 0x02000EDA RID: 3802
	public class StatesInstance : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.GameInstance
	{
		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06004C0C RID: 19468 RVA: 0x000D58F0 File Offset: 0x000D3AF0
		public KBatchedAnimController BrainController
		{
			get
			{
				return this.controllers[0];
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06004C0D RID: 19469 RVA: 0x000D58FA File Offset: 0x000D3AFA
		public KBatchedAnimController ShelfController
		{
			get
			{
				return this.controllers[1];
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06004C0E RID: 19470 RVA: 0x000D5904 File Offset: 0x000D3B04
		// (set) Token: 0x06004C0F RID: 19471 RVA: 0x000D590C File Offset: 0x000D3B0C
		public Storage BrainStorage { get; private set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06004C10 RID: 19472 RVA: 0x000D5915 File Offset: 0x000D3B15
		// (set) Token: 0x06004C11 RID: 19473 RVA: 0x000D591D File Offset: 0x000D3B1D
		public KSelectable Selectable { get; private set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06004C12 RID: 19474 RVA: 0x000D5926 File Offset: 0x000D3B26
		// (set) Token: 0x06004C13 RID: 19475 RVA: 0x000D592E File Offset: 0x000D3B2E
		public Operational Operational { get; private set; }

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06004C14 RID: 19476 RVA: 0x000D5937 File Offset: 0x000D3B37
		// (set) Token: 0x06004C15 RID: 19477 RVA: 0x000D593F File Offset: 0x000D3B3F
		public ElementConverter ElementConverter { get; private set; }

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x000D5948 File Offset: 0x000D3B48
		// (set) Token: 0x06004C17 RID: 19479 RVA: 0x000D5950 File Offset: 0x000D3B50
		public ManualDeliveryKG JournalDelivery { get; private set; }

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06004C18 RID: 19480 RVA: 0x000D5959 File Offset: 0x000D3B59
		// (set) Token: 0x06004C19 RID: 19481 RVA: 0x000D5961 File Offset: 0x000D3B61
		public LoopingSounds BrainSounds { get; private set; }

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06004C1A RID: 19482 RVA: 0x000D596A File Offset: 0x000D3B6A
		public bool IsHungry
		{
			get
			{
				return !this.ElementConverter.HasEnoughMassToStartConverting(true);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06004C1B RID: 19483 RVA: 0x000D597B File Offset: 0x000D3B7B
		public int TimeTilDigested
		{
			get
			{
				return (int)this.timeTilDigested;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06004C1C RID: 19484 RVA: 0x000D5984 File Offset: 0x000D3B84
		public int ActivationProgress
		{
			get
			{
				return (int)(25f * this.meterFill);
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06004C1D RID: 19485 RVA: 0x000D5993 File Offset: 0x000D3B93
		public HashedString CurrentActivationAnim
		{
			get
			{
				return MegaBrainTankConfig.ACTIVATION_ANIMS[(int)(this.nextActiveBrain - 1)];
			}
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06004C1E RID: 19486 RVA: 0x0026E8EC File Offset: 0x0026CAEC
		private HashedString currentActivationLoop
		{
			get
			{
				int num = (int)(this.nextActiveBrain - 1 + 5);
				return MegaBrainTankConfig.ACTIVATION_ANIMS[num];
			}
		}

		// Token: 0x06004C1F RID: 19487 RVA: 0x0026E910 File Offset: 0x0026CB10
		public StatesInstance(MegaBrainTank master) : base(master)
		{
			this.BrainSounds = base.GetComponent<LoopingSounds>();
			this.BrainStorage = base.GetComponent<Storage>();
			this.ElementConverter = base.GetComponent<ElementConverter>();
			this.JournalDelivery = base.GetComponent<ManualDeliveryKG>();
			this.Operational = base.GetComponent<Operational>();
			this.Selectable = base.GetComponent<KSelectable>();
			this.notifier = base.GetComponent<Notifier>();
			this.controllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>();
			this.meter = new MeterController(this.BrainController, "meter_oxygen_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
			this.fxLink = new KAnimLink(this.BrainController, this.ShelfController);
		}

		// Token: 0x06004C20 RID: 19488 RVA: 0x0026E9D8 File Offset: 0x0026CBD8
		public override void StartSM()
		{
			this.InitializeEffectsList();
			base.StartSM();
			this.BrainController.onAnimComplete += this.OnAnimComplete;
			this.ShelfController.onAnimComplete += this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			this.brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP", false);
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			if (this.GetCurrentState() == base.sm.common.dormant)
			{
				this.meterFill = (this.targetProgress = unitsAvailable / 25f);
				this.meter.SetPositionPercent(this.meterFill);
				short num = (short)(5f * this.meterFill);
				if (num > 0)
				{
					this.nextActiveBrain = num;
					this.BrainSounds.StartSound(this.brainHum);
					this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)num);
					this.CompleteBrainActivation();
				}
				return;
			}
			this.timeTilDigested = unitsAvailable * 60f;
			this.meterFill = this.timeTilDigested - this.timeTilDigested % 0.04f;
			this.meterFill /= 1500f;
			this.meter.SetPositionPercent(this.meterFill);
			StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
			this.nextActiveBrain = 5;
			this.CompleteBrainActivation();
		}

		// Token: 0x06004C21 RID: 19489 RVA: 0x0026EB80 File Offset: 0x0026CD80
		public override void StopSM(string reason)
		{
			this.BrainController.onAnimComplete -= this.OnAnimComplete;
			this.ShelfController.onAnimComplete -= this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			base.StopSM(reason);
		}

		// Token: 0x06004C22 RID: 19490 RVA: 0x0026EBEC File Offset: 0x0026CDEC
		private void InitializeEffectsList()
		{
			Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
			liveMinionIdentities.OnAdd += this.OnLiveMinionIdAdded;
			liveMinionIdentities.OnRemove += this.OnLiveMinionIdRemoved;
			MegaBrainTank.StatesInstance.minionEffects = new List<Effects>((liveMinionIdentities.Count > 32) ? liveMinionIdentities.Count : 32);
			for (int i = 0; i < liveMinionIdentities.Count; i++)
			{
				this.OnLiveMinionIdAdded(liveMinionIdentities[i]);
			}
		}

		// Token: 0x06004C23 RID: 19491 RVA: 0x0026EC60 File Offset: 0x0026CE60
		private void OnLiveMinionIdAdded(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Add(component);
			if (this.GetCurrentState() == base.sm.common.active)
			{
				component.Add(base.sm.StatBonus, false);
			}
		}

		// Token: 0x06004C24 RID: 19492 RVA: 0x0026ECAC File Offset: 0x0026CEAC
		private void OnLiveMinionIdRemoved(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Remove(component);
		}

		// Token: 0x06004C25 RID: 19493 RVA: 0x0026ECCC File Offset: 0x0026CECC
		public void SetBonusActive(bool active)
		{
			for (int i = 0; i < MegaBrainTank.StatesInstance.minionEffects.Count; i++)
			{
				if (active)
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Add(base.sm.StatBonus, false);
				}
				else
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Remove(base.sm.StatBonus);
				}
			}
		}

		// Token: 0x06004C26 RID: 19494 RVA: 0x0026ED2C File Offset: 0x0026CF2C
		private void OnAnimComplete(HashedString anim)
		{
			if (anim == MegaBrainTankConfig.KACHUNK)
			{
				this.StoreJournals();
				return;
			}
			if ((anim == base.smi.CurrentActivationAnim || anim == MegaBrainTankConfig.ACTIVATE_ALL) && this.GetCurrentState() != base.sm.common.idle)
			{
				this.CompleteBrainActivation();
			}
		}

		// Token: 0x06004C27 RID: 19495 RVA: 0x0026ED8C File Offset: 0x0026CF8C
		private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (state != Workable.WorkableEvent.WorkStarted)
			{
				this.ShelfController.Play(MegaBrainTankConfig.KACHUNK, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			FetchAreaChore.StatesInstance smi = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
			if (smi.IsNullOrStopped())
			{
				return;
			}
			GameObject gameObject = smi.sm.deliveryObject.Get(smi);
			if (gameObject == null)
			{
				return;
			}
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.UnitsFromLastStore = (short)component.PrimaryElement.Units;
			float num = Mathf.Clamp01(component.PrimaryElement.Units / 5f);
			this.BrainStorage.SetWorkTime(num * this.BrainStorage.storageWorkTime);
		}

		// Token: 0x06004C28 RID: 19496 RVA: 0x0026EE38 File Offset: 0x0026D038
		public void ShelveJournals(float dt)
		{
			float num = this.lastRemainingTime - this.BrainStorage.WorkTimeRemaining;
			if (num <= 0f)
			{
				num = this.BrainStorage.storageWorkTime - this.BrainStorage.WorkTimeRemaining;
			}
			this.lastRemainingTime = this.BrainStorage.WorkTimeRemaining;
			if (this.BrainStorage.storageWorkTime / 5f - this.journalActivationTimer > 0.001f)
			{
				this.journalActivationTimer += num;
				return;
			}
			int num2 = -1;
			this.journalActivationTimer = 0f;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				byte b = (byte)(1 << i);
				bool flag = (this.activatedJournals & b) == 0;
				if (flag && num2 == -1)
				{
					num2 = i;
				}
				if (flag & UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					num2 = -1;
					this.activatedJournals |= b;
					this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], true);
					break;
				}
			}
			if (num2 != -1)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[num2], true);
			}
			this.UnitsFromLastStore -= 1;
		}

		// Token: 0x06004C29 RID: 19497 RVA: 0x0026EF6C File Offset: 0x0026D16C
		public void StoreJournals()
		{
			this.lastRemainingTime = 0f;
			this.activatedJournals = 0;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
			}
			this.ShelfController.PlayMode = KAnim.PlayMode.Paused;
			this.ShelfController.SetPositionPercent(0f);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.targetProgress = Mathf.Clamp01(unitsAvailable / 25f);
		}

		// Token: 0x06004C2A RID: 19498 RVA: 0x0026EFF8 File Offset: 0x0026D1F8
		public void ActivateBrains(float dt)
		{
			if (this.currentlyActivating)
			{
				return;
			}
			this.currentlyActivating = ((float)this.nextActiveBrain / 5f - this.meterFill <= 0.001f);
			if (!this.currentlyActivating)
			{
				return;
			}
			this.BrainController.QueueAndSyncTransition(this.CurrentActivationAnim, KAnim.PlayMode.Once, 1f, 0f);
			if (this.nextActiveBrain > 0)
			{
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
			}
		}

		// Token: 0x06004C2B RID: 19499 RVA: 0x0026F094 File Offset: 0x0026D294
		public void CompleteBrainActivation()
		{
			this.BrainController.Play(this.currentActivationLoop, KAnim.PlayMode.Loop, 1f, 0f);
			this.nextActiveBrain += 1;
			this.currentlyActivating = false;
			if (this.nextActiveBrain > 5)
			{
				float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
				this.timeTilDigested = unitsAvailable * 60f;
				this.CompleteEvent();
			}
		}

		// Token: 0x06004C2C RID: 19500 RVA: 0x0026F100 File Offset: 0x0026D300
		public void Digest(float dt)
		{
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (this.targetProgress - this.meterFill > Mathf.Epsilon)
			{
				return;
			}
			this.targetProgress = 0f;
			float num = this.meterFill - this.timeTilDigested / 1500f;
			if (num >= 0.04f)
			{
				this.meterFill -= num - num % 0.04f;
				this.meter.SetPositionPercent(this.meterFill);
			}
		}

		// Token: 0x06004C2D RID: 19501 RVA: 0x0026F190 File Offset: 0x0026D390
		public void CleanTank(bool active)
		{
			this.SetBonusActive(active);
			base.GetComponent<Light2D>().enabled = active;
			this.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, this);
			this.ElementConverter.SetAllConsumedActive(active);
			this.BrainController.ClearQueue();
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (active)
			{
				this.nextActiveBrain = 5;
				this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
				return;
			}
			if (this.timeTilDigested < 0.016666668f)
			{
				this.BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
				this.timeTilDigested = 0f;
				this.meterFill = 0f;
				this.meter.SetPositionPercent(this.meterFill);
			}
			this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
			this.BrainSounds.StopSound(this.brainHum);
		}

		// Token: 0x06004C2E RID: 19502 RVA: 0x0026F2CC File Offset: 0x0026D4CC
		public bool IncrementMeter(float dt)
		{
			if (this.targetProgress - this.meterFill <= Mathf.Epsilon)
			{
				return false;
			}
			this.meterFill += Mathf.Lerp(0f, 1f, 0.04f * dt);
			if (1f - this.meterFill <= 0.001f)
			{
				this.meterFill = 1f;
			}
			this.meter.SetPositionPercent(this.meterFill);
			return this.targetProgress - this.meterFill > 0.001f;
		}

		// Token: 0x06004C2F RID: 19503 RVA: 0x0026F358 File Offset: 0x0026D558
		public void CompleteEvent()
		{
			this.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress, false);
			this.Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			base.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
			this.eventComplete = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.ShowEventCompleteUI));
			this.notifier.Add(this.eventComplete, "");
		}

		// Token: 0x06004C30 RID: 19504 RVA: 0x0026F45C File Offset: 0x0026D65C
		public void ShowEventCompleteUI(object _ = null)
		{
			if (this.eventComplete == null)
			{
				return;
			}
			base.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			this.notifier.Remove(this.eventComplete);
			this.eventComplete = null;
			Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed", true);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnCompleteStorySequence),
				CanCompleteCB = null
			});
		}

		// Token: 0x06004C31 RID: 19505 RVA: 0x0026F564 File Offset: 0x0026D764
		private void OnCompleteStorySequence()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, keepsakeSpawnPosition);
			this.eventInfo = null;
			base.sm.storyTraitCompleted.Trigger(this);
		}

		// Token: 0x0400353B RID: 13627
		private static List<Effects> minionEffects;

		// Token: 0x04003542 RID: 13634
		public short UnitsFromLastStore;

		// Token: 0x04003543 RID: 13635
		private float meterFill = 0.04f;

		// Token: 0x04003544 RID: 13636
		private float targetProgress;

		// Token: 0x04003545 RID: 13637
		private float timeTilDigested;

		// Token: 0x04003546 RID: 13638
		private float journalActivationTimer;

		// Token: 0x04003547 RID: 13639
		private float lastRemainingTime;

		// Token: 0x04003548 RID: 13640
		private byte activatedJournals;

		// Token: 0x04003549 RID: 13641
		private bool currentlyActivating;

		// Token: 0x0400354A RID: 13642
		private short nextActiveBrain = 1;

		// Token: 0x0400354B RID: 13643
		private string brainHum;

		// Token: 0x0400354C RID: 13644
		private KBatchedAnimController[] controllers;

		// Token: 0x0400354D RID: 13645
		private KAnimLink fxLink;

		// Token: 0x0400354E RID: 13646
		private MeterController meter;

		// Token: 0x0400354F RID: 13647
		private EventInfoData eventInfo;

		// Token: 0x04003550 RID: 13648
		private Notification eventComplete;

		// Token: 0x04003551 RID: 13649
		private Notifier notifier;
	}
}
