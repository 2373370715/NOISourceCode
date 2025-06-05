using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C8E RID: 15502
	public class PeriodicEmoteSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600EDD4 RID: 60884 RVA: 0x001440BE File Offset: 0x001422BE
		public PeriodicEmoteSickness(Emote emote, float cooldown)
		{
			this.emote = emote;
			this.cooldown = cooldown;
		}

		// Token: 0x0600EDD5 RID: 60885 RVA: 0x001440D4 File Offset: 0x001422D4
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
			statesInstance.StartSM();
			return statesInstance;
		}

		// Token: 0x0600EDD6 RID: 60886 RVA: 0x001440E3 File Offset: 0x001422E3
		public override void OnCure(GameObject go, object instance_data)
		{
			((PeriodicEmoteSickness.StatesInstance)instance_data).StopSM("Cured");
		}

		// Token: 0x0400E9C9 RID: 59849
		private Emote emote;

		// Token: 0x0400E9CA RID: 59850
		private float cooldown;

		// Token: 0x02003C8F RID: 15503
		public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600EDD7 RID: 60887 RVA: 0x001440F5 File Offset: 0x001422F5
			public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness) : base(master)
			{
				this.periodicEmoteSickness = periodicEmoteSickness;
			}

			// Token: 0x0600EDD8 RID: 60888 RVA: 0x004E42F4 File Offset: 0x004E24F4
			public Reactable GetReactable()
			{
				return new SelfEmoteReactable(base.master.gameObject, "PeriodicEmoteSickness", Db.Get().ChoreTypes.Emote, 0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity, 0f).SetEmote(this.periodicEmoteSickness.emote).SetOverideAnimSet("anim_sneeze_kanim");
			}

			// Token: 0x0400E9CB RID: 59851
			public PeriodicEmoteSickness periodicEmoteSickness;
		}

		// Token: 0x02003C90 RID: 15504
		public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600EDD9 RID: 60889 RVA: 0x00144105 File Offset: 0x00142305
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.root;
				this.root.ToggleReactable((PeriodicEmoteSickness.StatesInstance smi) => smi.GetReactable());
			}
		}
	}
}
