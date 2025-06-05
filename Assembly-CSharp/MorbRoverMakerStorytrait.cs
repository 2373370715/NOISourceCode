using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class MorbRoverMakerStorytrait : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>
{
	// Token: 0x06001528 RID: 5416 RVA: 0x000B3D3B File Offset: 0x000B1F3B
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.root;
	}

	// Token: 0x04000E89 RID: 3721
	public StateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, StateMachineController, MorbRoverMakerStorytrait.Def>.BoolParameter HasAnyBioBotBeenReleased;

	// Token: 0x020004D0 RID: 1232
	public class Def : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitDef
	{
		// Token: 0x0600152A RID: 5418 RVA: 0x0019D96C File Offset: 0x0019BB6C
		public override void Configure(GameObject prefab)
		{
			this.Story = Db.Get().Stories.MorbRoverMaker;
			this.CompletionData = new StoryCompleteData
			{
				KeepSakeSpawnOffset = new CellOffset(0, 2),
				CameraTargetOffset = new CellOffset(0, 3)
			};
			this.InitalLoreId = "story_trait_morbrover_initial";
			this.EventIntroInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.BEGIN.BUTTON,
				TextureName = "biobotdiscovered_kanim",
				DisplayImmediate = true,
				PopupType = EventInfoDataHelper.PopupType.BEGIN
			};
			this.EventMachineRevealedInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_CLOSE,
				extraButtons = new StoryManager.ExtraButtonInfo[]
				{
					new StoryManager.ExtraButtonInfo
					{
						ButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.REVEAL.BUTTON_READLORE,
						OnButtonClick = delegate()
						{
							System.Action normalPopupOpenCodexButtonPressed = this.NormalPopupOpenCodexButtonPressed;
							if (normalPopupOpenCodexButtonPressed != null)
							{
								normalPopupOpenCodexButtonPressed();
							}
							this.UnlockRevealEntries();
							string entryForLock = CodexCache.GetEntryForLock(this.MachineRevealedLoreId);
							if (entryForLock == null)
							{
								DebugUtil.DevLogError("Missing codex entry for lock: " + this.MachineRevealedLoreId);
								return;
							}
							ManagementMenu.Instance.OpenCodexToEntry(entryForLock, null);
						}
					}
				},
				TextureName = "BioBotCleanedUp_kanim",
				PopupType = EventInfoDataHelper.PopupType.NORMAL
			};
			this.CompleteLoreId = "story_trait_morbrover_complete";
			this.EventCompleteInfo = new StoryManager.PopupInfo
			{
				Title = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.NAME,
				Description = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.DESCRIPTION,
				CloseButtonText = CODEX.STORY_TRAITS.MORB_ROVER_MAKER.POPUPS.END.BUTTON,
				TextureName = "BioBotComplete_kanim",
				PopupType = EventInfoDataHelper.PopupType.COMPLETE
			};
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x000B3D54 File Offset: 0x000B1F54
		public void UnlockRevealEntries()
		{
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId, true);
			Game.Instance.unlocks.Unlock(this.MachineRevealedLoreId2, true);
		}

		// Token: 0x04000E8A RID: 3722
		public const string LORE_UNLOCK_PREFIX = "story_trait_morbrover_";

		// Token: 0x04000E8B RID: 3723
		public string MachineRevealedLoreId = "story_trait_morbrover_reveal";

		// Token: 0x04000E8C RID: 3724
		public string MachineRevealedLoreId2 = "story_trait_morbrover_reveal_lore";

		// Token: 0x04000E8D RID: 3725
		public string CompleteLoreId2 = "story_trait_morbrover_complete_lore";

		// Token: 0x04000E8E RID: 3726
		public string CompleteLoreId3 = "story_trait_morbrover_biobot";

		// Token: 0x04000E8F RID: 3727
		public System.Action NormalPopupOpenCodexButtonPressed;

		// Token: 0x04000E90 RID: 3728
		public StoryManager.PopupInfo EventMachineRevealedInfo;
	}

	// Token: 0x020004D1 RID: 1233
	public new class Instance : StoryTraitStateMachine<MorbRoverMakerStorytrait, MorbRoverMakerStorytrait.Instance, MorbRoverMakerStorytrait.Def>.TraitInstance
	{
		// Token: 0x0600152E RID: 5422 RVA: 0x000B3DB6 File Offset: 0x000B1FB6
		public Instance(StateMachineController master, MorbRoverMakerStorytrait.Def def) : base(master, def)
		{
			def.NormalPopupOpenCodexButtonPressed = (System.Action)Delegate.Combine(def.NormalPopupOpenCodexButtonPressed, new System.Action(this.OnNormalPopupOpenCodexButtonPressed));
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0019DB74 File Offset: 0x0019BD74
		public override void StartSM()
		{
			base.StartSM();
			this.machine = base.gameObject.GetSMI<MorbRoverMaker.Instance>();
			this.storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MorbRoverMaker.HashId);
			if (this.storyInstance == null)
			{
				return;
			}
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Combine(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Combine(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
				if (this.machine.HasBeenRevealed && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE && this.storyInstance.CurrentState != StoryInstance.State.IN_PROGRESS)
				{
					base.DisplayPopup(base.def.EventMachineRevealedInfo);
				}
				if (this.machine.HasBeenRevealed && base.sm.HasAnyBioBotBeenReleased.Get(this) && this.storyInstance.CurrentState != StoryInstance.State.COMPLETE)
				{
					this.CompleteEvent();
				}
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x000B3DE2 File Offset: 0x000B1FE2
		private void OnMachineUncovered()
		{
			if (this.storyInstance != null && !this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.DisplayPopup(base.def.EventMachineRevealedInfo);
			}
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x000B3E0B File Offset: 0x000B200B
		protected override void ShowEventNormalUI()
		{
			base.ShowEventNormalUI();
			if (this.storyInstance != null && this.storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
			{
				EventInfoScreen.ShowPopup(this.storyInstance.EventInfo);
			}
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0019DC8C File Offset: 0x0019BE8C
		public override void OnPopupClosed()
		{
			base.OnPopupClosed();
			if (this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId2, true);
				Game.Instance.unlocks.Unlock(base.def.CompleteLoreId3, true);
				return;
			}
			if (this.storyInstance != null && this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.NORMAL))
			{
				base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
				base.def.UnlockRevealEntries();
				return;
			}
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x000B3E3A File Offset: 0x000B203A
		private void OnNormalPopupOpenCodexButtonPressed()
		{
			base.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x000B3E43 File Offset: 0x000B2043
		private void OnRoverSpawned(GameObject rover)
		{
			base.smi.sm.HasAnyBioBotBeenReleased.Set(true, base.smi, false);
			if (!this.storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
			{
				this.CompleteEvent();
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0019DD10 File Offset: 0x0019BF10
		protected override void OnCleanUp()
		{
			if (this.machine != null)
			{
				MorbRoverMaker.Instance instance = this.machine;
				instance.OnUncovered = (System.Action)Delegate.Remove(instance.OnUncovered, new System.Action(this.OnMachineUncovered));
				MorbRoverMaker.Instance instance2 = this.machine;
				instance2.OnRoverSpawned = (Action<GameObject>)Delegate.Remove(instance2.OnRoverSpawned, new Action<GameObject>(this.OnRoverSpawned));
			}
			base.OnCleanUp();
		}

		// Token: 0x04000E91 RID: 3729
		private MorbRoverMaker.Instance machine;

		// Token: 0x04000E92 RID: 3730
		private StoryInstance storyInstance;
	}
}
