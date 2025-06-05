using System;
using Database;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE6 RID: 15590
	public abstract class StoryTraitStateMachine<TStateMachine, TInstance, TDef> : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef> where TStateMachine : StoryTraitStateMachine<TStateMachine, TInstance, TDef> where TInstance : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitInstance where TDef : StoryTraitStateMachine<TStateMachine, TInstance, TDef>.TraitDef
	{
		// Token: 0x02003CE7 RID: 15591
		public class TraitDef : StateMachine.BaseDef
		{
			// Token: 0x0400EAE3 RID: 60131
			public string InitalLoreId;

			// Token: 0x0400EAE4 RID: 60132
			public string CompleteLoreId;

			// Token: 0x0400EAE5 RID: 60133
			public Story Story;

			// Token: 0x0400EAE6 RID: 60134
			public StoryCompleteData CompletionData;

			// Token: 0x0400EAE7 RID: 60135
			public StoryManager.PopupInfo EventIntroInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};

			// Token: 0x0400EAE8 RID: 60136
			public StoryManager.PopupInfo EventCompleteInfo = new StoryManager.PopupInfo
			{
				PopupType = EventInfoDataHelper.PopupType.NONE
			};
		}

		// Token: 0x02003CE8 RID: 15592
		public class TraitInstance : GameStateMachine<TStateMachine, TInstance, StateMachineController, TDef>.GameInstance
		{
			// Token: 0x0600EF5D RID: 61277 RVA: 0x004E96F8 File Offset: 0x004E78F8
			public TraitInstance(StateMachineController master) : base(master)
			{
				StoryManager.Instance.ForceCreateStory(base.def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600EF5E RID: 61278 RVA: 0x004E9758 File Offset: 0x004E7958
			public TraitInstance(StateMachineController master, TDef def) : base(master, def)
			{
				StoryManager.Instance.ForceCreateStory(def.Story, base.gameObject.GetMyWorldId());
				this.buildingActivatedHandle = master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
			}

			// Token: 0x0600EF5F RID: 61279 RVA: 0x004E97B4 File Offset: 0x004E79B4
			public override void StartSM()
			{
				this.selectable = base.GetComponent<KSelectable>();
				this.notifier = base.gameObject.AddOrGet<Notifier>();
				base.StartSM();
				base.Subscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				if (this.buildingActivatedHandle == -1)
				{
					this.buildingActivatedHandle = base.master.Subscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				}
				this.TriggerStoryEvent(StoryInstance.State.DISCOVERED);
			}

			// Token: 0x0600EF60 RID: 61280 RVA: 0x00145221 File Offset: 0x00143421
			public override void StopSM(string reason)
			{
				base.StopSM(reason);
				base.Unsubscribe(-1503271301, new Action<object>(this.OnObjectSelect));
				base.Unsubscribe(-1909216579, new Action<object>(this.OnBuildingActivated));
				this.buildingActivatedHandle = -1;
			}

			// Token: 0x0600EF61 RID: 61281 RVA: 0x004E9830 File Offset: 0x004E7A30
			public void TriggerStoryEvent(StoryInstance.State storyEvent)
			{
				switch (storyEvent)
				{
				case StoryInstance.State.RETROFITTED:
				case StoryInstance.State.NOT_STARTED:
					return;
				case StoryInstance.State.DISCOVERED:
					StoryManager.Instance.DiscoverStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.IN_PROGRESS:
					StoryManager.Instance.BeginStoryEvent(base.def.Story);
					return;
				case StoryInstance.State.COMPLETE:
				{
					Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.KeepSakeSpawnOffset), Grid.SceneLayer.Ore);
					StoryManager.Instance.CompleteStoryEvent(base.def.Story, keepsakeSpawnPosition);
					return;
				}
				default:
					throw new NotImplementedException(storyEvent.ToString());
				}
			}

			// Token: 0x0600EF62 RID: 61282 RVA: 0x00145261 File Offset: 0x00143461
			protected virtual void OnBuildingActivated(object activated)
			{
				if (!(bool)activated)
				{
					return;
				}
				this.TriggerStoryEvent(StoryInstance.State.IN_PROGRESS);
			}

			// Token: 0x0600EF63 RID: 61283 RVA: 0x004E98F0 File Offset: 0x004E7AF0
			protected virtual void OnObjectSelect(object clicked)
			{
				if (!(bool)clicked)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance != null && storyInstance.PendingType != EventInfoDataHelper.PopupType.NONE)
				{
					this.OnNotificationClicked(null);
					return;
				}
				if (!StoryManager.Instance.HasDisplayedPopup(base.def.Story, EventInfoDataHelper.PopupType.BEGIN))
				{
					this.DisplayPopup(base.def.EventIntroInfo);
				}
			}

			// Token: 0x0600EF64 RID: 61284 RVA: 0x004E9970 File Offset: 0x004E7B70
			public virtual void CompleteEvent()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
				{
					return;
				}
				this.DisplayPopup(base.def.EventCompleteInfo);
			}

			// Token: 0x0600EF65 RID: 61285 RVA: 0x00145273 File Offset: 0x00143473
			public virtual void OnCompleteStorySequence()
			{
				this.TriggerStoryEvent(StoryInstance.State.COMPLETE);
			}

			// Token: 0x0600EF66 RID: 61286 RVA: 0x004E99C0 File Offset: 0x004E7BC0
			protected void DisplayPopup(StoryManager.PopupInfo info)
			{
				if (info.PopupType == EventInfoDataHelper.PopupType.NONE)
				{
					return;
				}
				StoryInstance storyInstance = StoryManager.Instance.DisplayPopup(base.def.Story, info, new System.Action(this.OnPopupClosed), new Notification.ClickCallback(this.OnNotificationClicked));
				if (storyInstance != null && !info.DisplayImmediate)
				{
					this.selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
					this.notifier.Add(storyInstance.Notification, "");
				}
			}

			// Token: 0x0600EF67 RID: 61287 RVA: 0x004E9A54 File Offset: 0x004E7C54
			public void OnNotificationClicked(object data = null)
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				this.selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
				this.notifier.Remove(storyInstance.Notification);
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.COMPLETE)
				{
					this.ShowEventCompleteUI();
					return;
				}
				if (storyInstance.PendingType == EventInfoDataHelper.PopupType.NORMAL)
				{
					this.ShowEventNormalUI();
					return;
				}
				this.ShowEventBeginUI();
			}

			// Token: 0x0600EF68 RID: 61288 RVA: 0x004E9AD8 File Offset: 0x004E7CD8
			public virtual void OnPopupClosed()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				if (storyInstance.HasDisplayedPopup(EventInfoDataHelper.PopupType.COMPLETE))
				{
					Game.Instance.unlocks.Unlock(base.def.CompleteLoreId, true);
					return;
				}
				Game.Instance.unlocks.Unlock(base.def.InitalLoreId, true);
			}

			// Token: 0x0600EF69 RID: 61289 RVA: 0x000AA038 File Offset: 0x000A8238
			protected virtual void ShowEventBeginUI()
			{
			}

			// Token: 0x0600EF6A RID: 61290 RVA: 0x000AA038 File Offset: 0x000A8238
			protected virtual void ShowEventNormalUI()
			{
			}

			// Token: 0x0600EF6B RID: 61291 RVA: 0x004E9B54 File Offset: 0x004E7D54
			protected virtual void ShowEventCompleteUI()
			{
				StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(base.def.Story.HashId);
				if (storyInstance == null)
				{
					return;
				}
				Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), base.def.CompletionData.CameraTargetOffset), Grid.SceneLayer.Ore);
				StoryManager.Instance.CompleteStoryEvent(base.def.Story, base.master, new FocusTargetSequence.Data
				{
					WorldId = base.master.GetMyWorldId(),
					OrthographicSize = 6f,
					TargetSize = 6f,
					Target = target,
					PopupData = storyInstance.EventInfo,
					CompleteCB = new System.Action(this.OnCompleteStorySequence),
					CanCompleteCB = null
				});
			}

			// Token: 0x0400EAE9 RID: 60137
			protected int buildingActivatedHandle = -1;

			// Token: 0x0400EAEA RID: 60138
			protected Notifier notifier;

			// Token: 0x0400EAEB RID: 60139
			protected KSelectable selectable;
		}
	}
}
