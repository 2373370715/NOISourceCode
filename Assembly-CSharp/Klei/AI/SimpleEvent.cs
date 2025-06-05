using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CD3 RID: 15571
	public class SimpleEvent : GameplayEvent<SimpleEvent.StatesInstance>
	{
		// Token: 0x0600EEF9 RID: 61177 RVA: 0x00144C5D File Offset: 0x00142E5D
		public SimpleEvent(string id, string title, string description, string animFileName, string buttonText = null, string buttonTooltip = null) : base(id, 0, 0)
		{
			this.title = title;
			this.description = description;
			this.buttonText = buttonText;
			this.buttonTooltip = buttonTooltip;
			this.animFileName = animFileName;
		}

		// Token: 0x0600EEFA RID: 61178 RVA: 0x00144C93 File Offset: 0x00142E93
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new SimpleEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x0400EABA RID: 60090
		private string buttonText;

		// Token: 0x0400EABB RID: 60091
		private string buttonTooltip;

		// Token: 0x02003CD4 RID: 15572
		public class States : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>
		{
			// Token: 0x0600EEFB RID: 61179 RVA: 0x00144C9D File Offset: 0x00142E9D
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600EEFC RID: 61180 RVA: 0x004E8A74 File Offset: 0x004E6C74
			public override EventInfoData GenerateEventPopupData(SimpleEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				eventInfoData.minions = smi.minions;
				eventInfoData.artifact = smi.artifact;
				EventInfoData.Option option = eventInfoData.AddOption(smi.gameplayEvent.buttonText, null);
				option.callback = delegate()
				{
					if (smi.callback != null)
					{
						smi.callback();
					}
					smi.StopSM("SimpleEvent Finished");
				};
				option.tooltip = smi.gameplayEvent.buttonTooltip;
				if (smi.textParameters != null)
				{
					foreach (global::Tuple<string, string> tuple in smi.textParameters)
					{
						eventInfoData.SetTextParameter(tuple.first, tuple.second);
					}
				}
				return eventInfoData;
			}

			// Token: 0x0400EABC RID: 60092
			public GameStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, object>.State ending;
		}

		// Token: 0x02003CD6 RID: 15574
		public class StatesInstance : GameplayEventStateMachine<SimpleEvent.States, SimpleEvent.StatesInstance, GameplayEventManager, SimpleEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600EF00 RID: 61184 RVA: 0x00144CEA File Offset: 0x00142EEA
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, SimpleEvent simpleEvent) : base(master, eventInstance, simpleEvent)
			{
			}

			// Token: 0x0600EF01 RID: 61185 RVA: 0x00144CF5 File Offset: 0x00142EF5
			public void SetTextParameter(string key, string value)
			{
				if (this.textParameters == null)
				{
					this.textParameters = new List<global::Tuple<string, string>>();
				}
				this.textParameters.Add(new global::Tuple<string, string>(key, value));
			}

			// Token: 0x0600EF02 RID: 61186 RVA: 0x00144D1C File Offset: 0x00142F1C
			public void ShowEventPopup()
			{
				EventInfoScreen.ShowPopup(base.smi.sm.GenerateEventPopupData(base.smi));
			}

			// Token: 0x0400EABE RID: 60094
			public GameObject[] minions;

			// Token: 0x0400EABF RID: 60095
			public GameObject artifact;

			// Token: 0x0400EAC0 RID: 60096
			public List<global::Tuple<string, string>> textParameters;

			// Token: 0x0400EAC1 RID: 60097
			public System.Action callback;
		}
	}
}
