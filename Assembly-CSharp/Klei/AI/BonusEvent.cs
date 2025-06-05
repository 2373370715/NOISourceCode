using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C98 RID: 15512
	public class BonusEvent : GameplayEvent<BonusEvent.StatesInstance>
	{
		// Token: 0x0600EE10 RID: 60944 RVA: 0x004E5900 File Offset: 0x004E3B00
		public BonusEvent(string id, string overrideEffect = null, int numTimesAllowed = 1, bool preSelectMinion = false, int priority = 0) : base(id, priority, 0)
		{
			this.title = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".NAME");
			this.description = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".DESCRIPTION");
			this.effect = ((overrideEffect != null) ? overrideEffect : id);
			this.numTimesAllowed = numTimesAllowed;
			this.preSelectMinion = preSelectMinion;
			this.animFileName = id.ToLower() + "_kanim";
			base.AddPrecondition(GameplayEventPreconditions.Instance.LiveMinions(1));
		}

		// Token: 0x0600EE11 RID: 60945 RVA: 0x00144297 File Offset: 0x00142497
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new BonusEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0600EE12 RID: 60946 RVA: 0x001442A1 File Offset: 0x001424A1
		public BonusEvent TriggerOnNewBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.NewBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x0600EE13 RID: 60947 RVA: 0x001442D7 File Offset: 0x001424D7
		public BonusEvent TriggerOnUseBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.UseBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x0600EE14 RID: 60948 RVA: 0x0014430D File Offset: 0x0014250D
		public BonusEvent TriggerOnWorkableComplete(int triggerCount, params Type[] types)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.WorkableComplete;
			this.workableType = new HashSet<Type>(types);
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x0600EE15 RID: 60949 RVA: 0x0014433E File Offset: 0x0014253E
		public BonusEvent SetExtraCondition(BonusEvent.ConditionFn extraCondition)
		{
			this.extraCondition = extraCondition;
			return this;
		}

		// Token: 0x0600EE16 RID: 60950 RVA: 0x00144348 File Offset: 0x00142548
		public BonusEvent SetRoomConstraints(bool hasOwnableInRoom, params RoomType[] types)
		{
			this.roomHasOwnable = hasOwnableInRoom;
			this.roomRestrictions = ((types == null) ? null : new HashSet<RoomType>(types));
			return this;
		}

		// Token: 0x0600EE17 RID: 60951 RVA: 0x00144364 File Offset: 0x00142564
		public string GetEffectTooltip(Effect effect)
		{
			return effect.Name + "\n\n" + Effect.CreateTooltip(effect, true, "\n    • ", true);
		}

		// Token: 0x0600EE18 RID: 60952 RVA: 0x004E59AC File Offset: 0x004E3BAC
		public override Sprite GetDisplaySprite()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Assets.GetSprite(Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).uiFullColourSprite);
			}
			return null;
		}

		// Token: 0x0600EE19 RID: 60953 RVA: 0x004E5A10 File Offset: 0x004E3C10
		public override string GetDisplayString()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).Name;
			}
			return null;
		}

		// Token: 0x0400E9F0 RID: 59888
		public const int PRE_SELECT_MINION_TIMEOUT = 5;

		// Token: 0x0400E9F1 RID: 59889
		public string effect;

		// Token: 0x0400E9F2 RID: 59890
		public bool preSelectMinion;

		// Token: 0x0400E9F3 RID: 59891
		public int numTimesToTrigger;

		// Token: 0x0400E9F4 RID: 59892
		public BonusEvent.TriggerType triggerType;

		// Token: 0x0400E9F5 RID: 59893
		public HashSet<Tag> buildingTrigger;

		// Token: 0x0400E9F6 RID: 59894
		public HashSet<Type> workableType;

		// Token: 0x0400E9F7 RID: 59895
		public HashSet<RoomType> roomRestrictions;

		// Token: 0x0400E9F8 RID: 59896
		public BonusEvent.ConditionFn extraCondition;

		// Token: 0x0400E9F9 RID: 59897
		public bool roomHasOwnable;

		// Token: 0x02003C99 RID: 15513
		public enum TriggerType
		{
			// Token: 0x0400E9FB RID: 59899
			None,
			// Token: 0x0400E9FC RID: 59900
			NewBuilding,
			// Token: 0x0400E9FD RID: 59901
			UseBuilding,
			// Token: 0x0400E9FE RID: 59902
			WorkableComplete,
			// Token: 0x0400E9FF RID: 59903
			AchievementUnlocked
		}

		// Token: 0x02003C9A RID: 15514
		// (Invoke) Token: 0x0600EE1B RID: 60955
		public delegate bool ConditionFn(BonusEvent.GameplayEventData data);

		// Token: 0x02003C9B RID: 15515
		public class GameplayEventData
		{
			// Token: 0x0400EA00 RID: 59904
			public GameHashes eventTrigger;

			// Token: 0x0400EA01 RID: 59905
			public BuildingComplete building;

			// Token: 0x0400EA02 RID: 59906
			public Workable workable;

			// Token: 0x0400EA03 RID: 59907
			public WorkerBase worker;
		}

		// Token: 0x02003C9C RID: 15516
		public class States : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>
		{
			// Token: 0x0600EE1F RID: 60959 RVA: 0x004E5A68 File Offset: 0x004E3C68
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.load;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.load.Enter(new StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.AssignPreSelectedMinionIfNeeded)).Transition(this.waitNewBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.NewBuilding, UpdateRate.SIM_200ms).Transition(this.waitUseBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.UseBuilding, UpdateRate.SIM_200ms).Transition(this.waitforWorkables, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.WorkableComplete, UpdateRate.SIM_200ms).Transition(this.waitForAchievement, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.AchievementUnlocked, UpdateRate.SIM_200ms).Transition(this.immediate, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.None, UpdateRate.SIM_200ms);
				this.waitNewBuilding.EventHandlerTransition(GameHashes.NewBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitUseBuilding.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitforWorkables.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.WorkableEventTrigger));
				this.immediate.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					GameObject gameObject = smi.sm.chosen.Get(smi);
					if (gameObject == null)
					{
						gameObject = smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject;
						smi.sm.chosen.Set(gameObject, smi, false);
					}
				}).GoTo(this.active);
				this.active.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.sm.chosen.Get(smi).GetComponent<Effects>().Add(smi.gameplayEvent.effect, true);
				}).Enter(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStart(this.chosen, smi);
				}).Exit(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStop(this.chosen, smi);
				}).ScheduleGoTo(delegate(BonusEvent.StatesInstance smi)
				{
					Effect effect = this.GetEffect(smi);
					if (effect != null)
					{
						return effect.duration;
					}
					return 0f;
				}, this.ending).DefaultState(this.active.notify).OnTargetLost(this.chosen, this.ending).Target(this.chosen).TagTransition(GameTags.Dead, this.ending, false);
				this.active.notify.ToggleNotification((BonusEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.active.seenNotification.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.eventInstance.seenNotification = true;
				});
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600EE20 RID: 60960 RVA: 0x004E5D0C File Offset: 0x004E3F0C
			public override EventInfoData GenerateEventPopupData(BonusEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						"Minion not set for " + smi.gameplayEvent.Id
					});
					return null;
				}
				Effect effect = this.GetEffect(smi);
				if (effect == null)
				{
					return null;
				}
				eventInfoData.clickFocus = gameObject.transform;
				eventInfoData.minions = new GameObject[]
				{
					gameObject
				};
				eventInfoData.SetTextParameter("dupe", gameObject.GetProperName());
				if (smi.building != null)
				{
					eventInfoData.SetTextParameter("building", UI.FormatAsLink(smi.building.GetProperName(), smi.building.GetProperName().ToUpper()));
				}
				EventInfoData.Option option = eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.active.seenNotification);
				});
				GAMEPLAY_EVENTS.BONUS_EVENT_DESCRIPTION.Replace("{effects}", Effect.CreateTooltip(effect, false, " ", false)).Replace("{durration}", GameUtil.GetFormattedCycles(effect.duration, "F1", false));
				foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
				{
					Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
					string text = string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
					text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
					Sprite sprite = Assets.GetSprite(attribute.uiFullColourSprite);
					option.AddPositiveIcon(sprite, text, 1.75f);
				}
				return eventInfoData;
			}

			// Token: 0x0600EE21 RID: 60961 RVA: 0x004E5F54 File Offset: 0x004E4154
			private void AssignPreSelectedMinionIfNeeded(BonusEvent.StatesInstance smi)
			{
				if (smi.gameplayEvent.preSelectMinion && smi.sm.chosen.Get(smi) == null)
				{
					smi.sm.chosen.Set(smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject, smi, false);
					smi.timesTriggered = 0;
				}
			}

			// Token: 0x0600EE22 RID: 60962 RVA: 0x004E5FB4 File Offset: 0x004E41B4
			private bool IsCorrectMinion(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (!smi.gameplayEvent.preSelectMinion || !(smi.sm.chosen.Get(smi) != gameplayEventData.worker.gameObject))
				{
					return true;
				}
				if (GameUtil.GetCurrentTimeInCycles() - smi.lastTriggered > 5f && smi.PercentageUntilTriggered() < 0.5f)
				{
					smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
					smi.timesTriggered = 0;
					return true;
				}
				return false;
			}

			// Token: 0x0600EE23 RID: 60963 RVA: 0x004E603C File Offset: 0x004E423C
			private bool OtherConditionsAreSatisfied(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (smi.gameplayEvent.roomRestrictions != null)
				{
					Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameplayEventData.worker.gameObject);
					if (roomOfGameObject == null)
					{
						return false;
					}
					if (!smi.gameplayEvent.roomRestrictions.Contains(roomOfGameObject.roomType))
					{
						return false;
					}
					if (smi.gameplayEvent.roomHasOwnable)
					{
						bool flag = false;
						using (List<Ownables>.Enumerator enumerator = roomOfGameObject.GetOwners().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.gameObject == gameplayEventData.worker.gameObject)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				return smi.gameplayEvent.extraCondition == null || smi.gameplayEvent.extraCondition(gameplayEventData);
			}

			// Token: 0x0600EE24 RID: 60964 RVA: 0x004E6124 File Offset: 0x004E4324
			private bool IncrementAndTrigger(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				smi.timesTriggered++;
				smi.lastTriggered = GameUtil.GetCurrentTimeInCycles();
				if (smi.timesTriggered < smi.gameplayEvent.numTimesToTrigger)
				{
					return false;
				}
				smi.building = gameplayEventData.building;
				smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
				return true;
			}

			// Token: 0x0600EE25 RID: 60965 RVA: 0x004E618C File Offset: 0x004E438C
			private bool BuildingEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return !(gameplayEventData.building == null) && (smi.gameplayEvent.buildingTrigger.Count <= 0 || smi.gameplayEvent.buildingTrigger.Contains(gameplayEventData.building.prefabid.PrefabID())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600EE26 RID: 60966 RVA: 0x004E6214 File Offset: 0x004E4414
			private bool WorkableEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return (smi.gameplayEvent.workableType.Count <= 0 || smi.gameplayEvent.workableType.Contains(gameplayEventData.workable.GetType())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600EE27 RID: 60967 RVA: 0x00144383 File Offset: 0x00142583
			private bool ChosenMinionDied(BonusEvent.StatesInstance smi, object data)
			{
				return smi.sm.chosen.Get(smi) == data as GameObject;
			}

			// Token: 0x0600EE28 RID: 60968 RVA: 0x004E6288 File Offset: 0x004E4488
			private Effect GetEffect(BonusEvent.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					return null;
				}
				EffectInstance effectInstance = gameObject.GetComponent<Effects>().Get(smi.gameplayEvent.effect);
				if (effectInstance == null)
				{
					global::Debug.LogWarning(string.Format("Effect {0} not found on {1} in BonusEvent", smi.gameplayEvent.effect, gameObject));
					return null;
				}
				return effectInstance.effect;
			}

			// Token: 0x0400EA04 RID: 59908
			public StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.TargetParameter chosen;

			// Token: 0x0400EA05 RID: 59909
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State load;

			// Token: 0x0400EA06 RID: 59910
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitNewBuilding;

			// Token: 0x0400EA07 RID: 59911
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitUseBuilding;

			// Token: 0x0400EA08 RID: 59912
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitForAchievement;

			// Token: 0x0400EA09 RID: 59913
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitforWorkables;

			// Token: 0x0400EA0A RID: 59914
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State immediate;

			// Token: 0x0400EA0B RID: 59915
			public BonusEvent.States.ActiveStates active;

			// Token: 0x0400EA0C RID: 59916
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x02003C9D RID: 15517
			public class ActiveStates : GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400EA0D RID: 59917
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State notify;

				// Token: 0x0400EA0E RID: 59918
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State seenNotification;
			}
		}

		// Token: 0x02003CA0 RID: 15520
		public class StatesInstance : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EE3B RID: 60987 RVA: 0x00144494 File Offset: 0x00142694
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, BonusEvent bonusEvent) : base(master, eventInstance, bonusEvent)
			{
				this.lastTriggered = GameUtil.GetCurrentTimeInCycles();
			}

			// Token: 0x0600EE3C RID: 60988 RVA: 0x001444AA File Offset: 0x001426AA
			public float PercentageUntilTriggered()
			{
				return (float)this.timesTriggered / (float)base.smi.gameplayEvent.numTimesToTrigger;
			}

			// Token: 0x0400EA19 RID: 59929
			[Serialize]
			public int timesTriggered;

			// Token: 0x0400EA1A RID: 59930
			[Serialize]
			public float lastTriggered;

			// Token: 0x0400EA1B RID: 59931
			public BuildingComplete building;
		}
	}
}
