using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C73 RID: 3187
public class BeachChair : StateMachineComponent<BeachChair.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x06003C89 RID: 15497 RVA: 0x000CB87D File Offset: 0x000C9A7D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x000CB890 File Offset: 0x000C9A90
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x0023C53C File Offset: 0x0023A73C
	public static void AddModifierDescriptions(List<Descriptor> descs, string effect_id, bool high_lux)
	{
		Klei.AI.Modifier modifier = Db.Get().effects.Get(effect_id);
		LocString locString = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW;
		LocString locString2 = high_lux ? BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_HIGH_TOOLTIP : BUILDINGS.PREFABS.BEACHCHAIR.LIGHTEFFECT_LOW_TOOLTIP;
		foreach (AttributeModifier attributeModifier in modifier.SelfModifiers)
		{
			Descriptor item = new Descriptor(locString.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), locString2.Replace("{attrib}", Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + attributeModifier.AttributeId.ToUpper() + ".NAME")).Replace("{amount}", attributeModifier.GetFormattedString()).Replace("{lux}", GameUtil.GetFormattedLux(BeachChairConfig.TAN_LUX)), Descriptor.DescriptorType.Effect, false);
			item.IncreaseIndent();
			descs.Add(item);
		}
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x0023C67C File Offset: 0x0023A87C
	List<Descriptor> IGameObjectEffectDescriptor.GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		list.Add(new Descriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect, false));
		BeachChair.AddModifierDescriptions(list, this.specificEffectLit, true);
		BeachChair.AddModifierDescriptions(list, this.specificEffectUnlit, false);
		return list;
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x000CB898 File Offset: 0x000C9A98
	public void SetLit(bool v)
	{
		base.smi.sm.lit.Set(v, base.smi, false);
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x000CB8B8 File Offset: 0x000C9AB8
	public void SetWorker(WorkerBase worker)
	{
		base.smi.sm.worker.Set(worker, base.smi);
	}

	// Token: 0x040029FB RID: 10747
	public string specificEffectUnlit;

	// Token: 0x040029FC RID: 10748
	public string specificEffectLit;

	// Token: 0x040029FD RID: 10749
	public string trackingEffect;

	// Token: 0x040029FE RID: 10750
	public const float LIT_RATIO_FOR_POSITIVE_EFFECT = 0.75f;

	// Token: 0x02000C74 RID: 3188
	public class States : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair>
	{
		// Token: 0x06003C90 RID: 15504 RVA: 0x0023C6CC File Offset: 0x0023A8CC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.inoperational;
			this.inoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.ready, false).ToggleMainStatusItem(Db.Get().BuildingStatusItems.MissingRequirements, null);
			this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle).ToggleChore(new Func<BeachChair.StatesInstance, Chore>(this.CreateChore), this.inoperational).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Working, null);
			this.ready.idle.PlayAnim("on", KAnim.PlayMode.Loop).WorkableStartTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.working_pre);
			this.ready.working_pre.PlayAnim("working_pre").QueueAnim("working_loop", true, null).Target(this.worker).PlayAnim("working_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit);
			});
			this.ready.working_unlit.DefaultState(this.ready.working_unlit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.UNLIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightInsufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_unlit_pre");
			this.ready.working_unlit.working.ParamTransition<bool>(this.lit, this.ready.working_unlit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsTrue).Target(this.worker).QueueAnim("working_unlit_loop", true, null);
			this.ready.working_unlit.post.Target(this.worker).PlayAnim("working_unlit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_lit);
					return;
				}
				smi.GoTo(this.ready.working_unlit.working);
			});
			this.ready.working_lit.DefaultState(this.ready.working_lit.working).Enter(delegate(BeachChair.StatesInstance smi)
			{
				BeachChairWorkable component = smi.master.GetComponent<BeachChairWorkable>();
				component.workingPstComplete = (component.workingPstFailed = this.LIT_PST_ANIMS);
			}).ToggleStatusItem(Db.Get().BuildingStatusItems.TanningLightSufficient, null).WorkableStopTransition((BeachChair.StatesInstance smi) => smi.master.GetComponent<BeachChairWorkable>(), this.ready.post).Target(this.worker).PlayAnim("working_lit_pre");
			this.ready.working_lit.working.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).QueueAnim("working_lit_loop", true, null).ScheduleGoTo((BeachChair.StatesInstance smi) => UnityEngine.Random.Range(5f, 15f), this.ready.working_lit.silly);
			this.ready.working_lit.silly.ParamTransition<bool>(this.lit, this.ready.working_lit.post, GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.IsFalse).Target(this.worker).PlayAnim((BeachChair.StatesInstance smi) => this.SILLY_ANIMS[UnityEngine.Random.Range(0, this.SILLY_ANIMS.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.ready.working_lit.working);
			this.ready.working_lit.post.Target(this.worker).PlayAnim("working_lit_pst").EventHandler(GameHashes.AnimQueueComplete, delegate(BeachChair.StatesInstance smi)
			{
				if (!this.lit.Get(smi))
				{
					smi.GoTo(this.ready.working_unlit);
					return;
				}
				smi.GoTo(this.ready.working_lit.working);
			});
			this.ready.post.PlayAnim("working_pst").Exit(delegate(BeachChair.StatesInstance smi)
			{
				this.worker.Set(null, smi);
			}).OnAnimQueueComplete(this.ready);
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0023CAE8 File Offset: 0x0023ACE8
		private Chore CreateChore(BeachChair.StatesInstance smi)
		{
			Workable component = smi.master.GetComponent<BeachChairWorkable>();
			WorkChore<BeachChairWorkable> workChore = new WorkChore<BeachChairWorkable>(Db.Get().ChoreTypes.Relax, component, null, true, null, null, null, false, Db.Get().ScheduleBlockTypes.Recreation, false, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
			workChore.AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, component);
			return workChore;
		}

		// Token: 0x040029FF RID: 10751
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.BoolParameter lit;

		// Token: 0x04002A00 RID: 10752
		public StateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.TargetParameter worker;

		// Token: 0x04002A01 RID: 10753
		private GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State inoperational;

		// Token: 0x04002A02 RID: 10754
		private BeachChair.States.ReadyStates ready;

		// Token: 0x04002A03 RID: 10755
		private HashedString[] UNLIT_PST_ANIMS = new HashedString[]
		{
			"working_unlit_pst",
			"working_pst"
		};

		// Token: 0x04002A04 RID: 10756
		private HashedString[] LIT_PST_ANIMS = new HashedString[]
		{
			"working_lit_pst",
			"working_pst"
		};

		// Token: 0x04002A05 RID: 10757
		private string[] SILLY_ANIMS = new string[]
		{
			"working_lit_loop1",
			"working_lit_loop2",
			"working_lit_loop3"
		};

		// Token: 0x02000C75 RID: 3189
		public class LitWorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x04002A06 RID: 10758
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x04002A07 RID: 10759
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State silly;

			// Token: 0x04002A08 RID: 10760
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x02000C76 RID: 3190
		public class WorkingStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x04002A09 RID: 10761
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working;

			// Token: 0x04002A0A RID: 10762
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}

		// Token: 0x02000C77 RID: 3191
		public class ReadyStates : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State
		{
			// Token: 0x04002A0B RID: 10763
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State idle;

			// Token: 0x04002A0C RID: 10764
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State working_pre;

			// Token: 0x04002A0D RID: 10765
			public BeachChair.States.WorkingStates working_unlit;

			// Token: 0x04002A0E RID: 10766
			public BeachChair.States.LitWorkingStates working_lit;

			// Token: 0x04002A0F RID: 10767
			public GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.State post;
		}
	}

	// Token: 0x02000C79 RID: 3193
	public class StatesInstance : GameStateMachine<BeachChair.States, BeachChair.StatesInstance, BeachChair, object>.GameInstance
	{
		// Token: 0x06003CA3 RID: 15523 RVA: 0x000CB9D9 File Offset: 0x000C9BD9
		public StatesInstance(BeachChair smi) : base(smi)
		{
		}
	}
}
