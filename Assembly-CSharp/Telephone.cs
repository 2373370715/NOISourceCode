using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001A2E RID: 6702
public class Telephone : StateMachineComponent<Telephone.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06008BA2 RID: 35746 RVA: 0x0036E158 File Offset: 0x0036C358
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		Components.Telephones.Add(this);
		GameScheduler.Instance.Schedule("Scheduling Tutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Schedule, true);
		}, null, null);
	}

	// Token: 0x06008BA3 RID: 35747 RVA: 0x000FFED1 File Offset: 0x000FE0D1
	protected override void OnCleanUp()
	{
		Components.Telephones.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06008BA4 RID: 35748 RVA: 0x0036E1B8 File Offset: 0x0036C3B8
	public void AddModifierDescriptions(List<Descriptor> descs, string effect_id)
	{
		Effect effect = Db.Get().effects.Get(effect_id);
		string text;
		string text2;
		if (effect.Id == this.babbleEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_BABBLE_TOOLTIP;
		}
		else if (effect.Id == this.chatEffect)
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_CHAT_TOOLTIP;
		}
		else
		{
			text = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE;
			text2 = BUILDINGS.PREFABS.TELEPHONE.EFFECT_LONG_DISTANCE_TOOLTIP;
		}
		foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
		{
			Descriptor item = new Descriptor(text.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), text2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06008BA5 RID: 35749 RVA: 0x0036E324 File Offset: 0x0036C524
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
		list.Add(item);
		this.AddModifierDescriptions(list, this.babbleEffect);
		this.AddModifierDescriptions(list, this.chatEffect);
		this.AddModifierDescriptions(list, this.longDistanceEffect);
		return list;
	}

	// Token: 0x06008BA6 RID: 35750 RVA: 0x000FFEE4 File Offset: 0x000FE0E4
	public void HangUp()
	{
		this.isInUse = false;
		this.wasAnswered = false;
		this.RemoveTag(GameTags.LongDistanceCall);
	}

	// Token: 0x04006956 RID: 26966
	public string babbleEffect;

	// Token: 0x04006957 RID: 26967
	public string chatEffect;

	// Token: 0x04006958 RID: 26968
	public string longDistanceEffect;

	// Token: 0x04006959 RID: 26969
	public string trackingEffect;

	// Token: 0x0400695A RID: 26970
	public bool isInUse;

	// Token: 0x0400695B RID: 26971
	public bool wasAnswered;

	// Token: 0x02001A2F RID: 6703
	public class States : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone>
	{
		// Token: 0x06008BA8 RID: 35752 RVA: 0x0036E38C File Offset: 0x0036C58C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			Telephone.States.CreateStatusItems();
			default_state = this.unoperational;
			this.unoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false);
			this.ready.TagTransition(GameTags.Operational, this.unoperational, true).DefaultState(this.ready.idle).ToggleRecurringChore(new Func<Telephone.StatesInstance, Chore>(this.CreateChore), null).Enter(delegate(Telephone.StatesInstance smi)
			{
				using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.isInUse)
						{
							smi.GoTo(this.ready.speaker);
						}
					}
				}
			});
			this.ready.idle.WorkableStartTransition((Telephone.StatesInstance smi) => smi.master.GetComponent<TelephoneCallerWorkable>(), this.ready.calling.dial).TagTransition(GameTags.TelephoneRinging, this.ready.ringing, false).PlayAnim("off");
			this.ready.calling.ScheduleGoTo(15f, this.ready.talking.babbling);
			this.ready.calling.dial.PlayAnim("on_pre").OnAnimQueueComplete(this.ready.calling.animHack);
			this.ready.calling.animHack.ScheduleActionNextFrame("animHack_delay", delegate(Telephone.StatesInstance smi)
			{
				smi.GoTo(this.ready.calling.pre);
			});
			this.ready.calling.pre.PlayAnim("on").Enter(delegate(Telephone.StatesInstance smi)
			{
				this.RingAllTelephones(smi);
			}).OnAnimQueueComplete(this.ready.calling.wait);
			this.ready.calling.wait.PlayAnim("on", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_4000ms);
			this.ready.ringing.PlayAnim("on_receiving", KAnim.PlayMode.Loop).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms).TagTransition(GameTags.TelephoneRinging, this.ready.speaker, true).ScheduleGoTo(15f, this.ready.speaker).Exit(delegate(Telephone.StatesInstance smi)
			{
				smi.GetComponent<Telephone>().RemoveTag(GameTags.TelephoneRinging);
			});
			this.ready.answer.PlayAnim("on_pre_loop_receiving").OnAnimQueueComplete(this.ready.talking.chatting);
			this.ready.talking.ScheduleGoTo(25f, this.ready.hangup).Enter(delegate(Telephone.StatesInstance smi)
			{
				this.UpdatePartyLine(smi);
			});
			this.ready.talking.babbling.PlayAnim("on_loop", KAnim.PlayMode.Loop).Transition(this.ready.talking.chatting, (Telephone.StatesInstance smi) => smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.babbling, null);
			this.ready.talking.chatting.PlayAnim("on_loop_pre").QueueAnim("on_loop", true, null).Transition(this.ready.talking.babbling, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_33ms).ToggleStatusItem(Telephone.States.partyLine, null);
			this.ready.speaker.PlayAnim("on_loop_nobody", KAnim.PlayMode.Loop).Transition(this.ready, (Telephone.StatesInstance smi) => !smi.CallAnswered(), UpdateRate.SIM_4000ms).Transition(this.ready.answer, (Telephone.StatesInstance smi) => smi.GetComponent<Telephone>().isInUse, UpdateRate.SIM_33ms);
			this.ready.hangup.OnAnimQueueComplete(this.ready);
		}

		// Token: 0x06008BA9 RID: 35753 RVA: 0x0036E7C4 File Offset: 0x0036C9C4
		private Chore CreateChore(Telephone.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<TelephoneCallerWorkable>();
			WorkChore<TelephoneCallerWorkable> workChore = new WorkChore<TelephoneCallerWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x06008BAA RID: 35754 RVA: 0x0036E824 File Offset: 0x0036CA24
		public void UpdatePartyLine(Telephone.StatesInstance smi)
		{
			int myWorldId = smi.GetMyWorldId();
			bool flag = false;
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				telephone.RemoveTag(GameTags.TelephoneRinging);
				if (telephone.isInUse && myWorldId != telephone.GetMyWorldId())
				{
					flag = true;
					telephone.AddTag(GameTags.LongDistanceCall);
				}
			}
			Telephone component = smi.GetComponent<Telephone>();
			component.RemoveTag(GameTags.TelephoneRinging);
			if (flag)
			{
				component.AddTag(GameTags.LongDistanceCall);
			}
		}

		// Token: 0x06008BAB RID: 35755 RVA: 0x0036E8CC File Offset: 0x0036CACC
		public void RingAllTelephones(Telephone.StatesInstance smi)
		{
			Telephone component = smi.master.GetComponent<Telephone>();
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (component != telephone && telephone.GetComponent<Operational>().IsOperational)
				{
					TelephoneCallerWorkable component2 = telephone.GetComponent<TelephoneCallerWorkable>();
					if (component2 != null && component2.worker == null)
					{
						telephone.AddTag(GameTags.TelephoneRinging);
					}
				}
			}
		}

		// Token: 0x06008BAC RID: 35756 RVA: 0x0036E968 File Offset: 0x0036CB68
		private static void CreateStatusItems()
		{
			if (Telephone.States.partyLine == null)
			{
				Telephone.States.partyLine = new StatusItem("PartyLine", BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO, "", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.partyLine.resolveStringCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					int num = 0;
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							num++;
							if (num == 1)
							{
								str = str.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
								str = str.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							}
						}
					}
					if (num > 1)
					{
						str = string.Format(BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO_NUM, num);
					}
					return str;
				};
				Telephone.States.partyLine.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone component = ((Telephone.StatesInstance)obj).GetComponent<Telephone>();
					foreach (Telephone telephone in Components.Telephones.Items)
					{
						if (telephone.isInUse && telephone != component)
						{
							string text = BUILDING.STATUSITEMS.TELEPHONE.CONVERSATION.TALKING_TO;
							text = text.Replace("{Duplicant}", telephone.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
							text = text.Replace("{Asteroid}", telephone.GetMyWorld().GetProperName());
							str = str + text + "\n";
						}
					}
					return str;
				};
			}
			if (Telephone.States.babbling == null)
			{
				Telephone.States.babbling = new StatusItem("Babbling", BUILDING.STATUSITEMS.TELEPHONE.BABBLE.NAME, BUILDING.STATUSITEMS.TELEPHONE.BABBLE.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, 129022, true, null);
				Telephone.States.babbling.resolveTooltipCallback = delegate(string str, object obj)
				{
					Telephone.StatesInstance statesInstance = (Telephone.StatesInstance)obj;
					str = str.Replace("{Duplicant}", statesInstance.GetComponent<TelephoneCallerWorkable>().worker.GetProperName());
					return str;
				};
			}
		}

		// Token: 0x0400695C RID: 26972
		private GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State unoperational;

		// Token: 0x0400695D RID: 26973
		private Telephone.States.ReadyStates ready;

		// Token: 0x0400695E RID: 26974
		private static StatusItem partyLine;

		// Token: 0x0400695F RID: 26975
		private static StatusItem babbling;

		// Token: 0x02001A30 RID: 6704
		public class ReadyStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
		{
			// Token: 0x04006960 RID: 26976
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State idle;

			// Token: 0x04006961 RID: 26977
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State ringing;

			// Token: 0x04006962 RID: 26978
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State answer;

			// Token: 0x04006963 RID: 26979
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State speaker;

			// Token: 0x04006964 RID: 26980
			public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State hangup;

			// Token: 0x04006965 RID: 26981
			public Telephone.States.ReadyStates.CallingStates calling;

			// Token: 0x04006966 RID: 26982
			public Telephone.States.ReadyStates.TalkingStates talking;

			// Token: 0x02001A31 RID: 6705
			public class CallingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x04006967 RID: 26983
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State dial;

				// Token: 0x04006968 RID: 26984
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State animHack;

				// Token: 0x04006969 RID: 26985
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State pre;

				// Token: 0x0400696A RID: 26986
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State wait;
			}

			// Token: 0x02001A32 RID: 6706
			public class TalkingStates : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State
			{
				// Token: 0x0400696B RID: 26987
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State babbling;

				// Token: 0x0400696C RID: 26988
				public GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.State chatting;
			}
		}
	}

	// Token: 0x02001A34 RID: 6708
	public class StatesInstance : GameStateMachine<Telephone.States, Telephone.StatesInstance, Telephone, object>.GameInstance
	{
		// Token: 0x06008BC2 RID: 35778 RVA: 0x000FFF8C File Offset: 0x000FE18C
		public StatesInstance(Telephone smi) : base(smi)
		{
		}

		// Token: 0x06008BC3 RID: 35779 RVA: 0x0036EC8C File Offset: 0x0036CE8C
		public bool CallAnswered()
		{
			foreach (Telephone telephone in Components.Telephones.Items)
			{
				if (telephone.isInUse && telephone != base.smi.GetComponent<Telephone>())
				{
					telephone.wasAnswered = true;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008BC4 RID: 35780 RVA: 0x0036ED08 File Offset: 0x0036CF08
		public bool CallEnded()
		{
			using (List<Telephone>.Enumerator enumerator = Components.Telephones.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.isInUse)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
