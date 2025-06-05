using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C81 RID: 15489
	public class SlimeSickness : Sickness
	{
		// Token: 0x0600EDAF RID: 60847 RVA: 0x004E3148 File Offset: 0x004E1348
		public SlimeSickness() : base("SlimeSickness", Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>
		{
			Sickness.InfectionVector.Inhalation
		}, 2220f, "SlimeSicknessRecovery")
		{
			base.AddSicknessComponent(new CommonSickEffectSickness());
			base.AddSicknessComponent(new AttributeModifierSickness(new AttributeModifier[]
			{
				new AttributeModifier("Athletics", -3f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AttributeModifierSickness(MinionConfig.ID, new AttributeModifier[]
			{
				new AttributeModifier("BreathDelta", DUPLICANTSTATS.STANDARD.Breath.BREATH_RATE * -1.25f, DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
			}));
			base.AddSicknessComponent(new AnimatedSickness(new HashedString[]
			{
				"anim_idle_sick_kanim"
			}, Db.Get().Expressions.Sick));
			base.AddSicknessComponent(new PeriodicEmoteSickness(Db.Get().Emotes.Minion.Sick, 50f));
			base.AddSicknessComponent(new SlimeSickness.SlimeLungComponent());
		}

		// Token: 0x0400E9B0 RID: 59824
		private const float COUGH_FREQUENCY = 20f;

		// Token: 0x0400E9B1 RID: 59825
		private const float COUGH_MASS = 0.1f;

		// Token: 0x0400E9B2 RID: 59826
		private const int DISEASE_AMOUNT = 1000;

		// Token: 0x0400E9B3 RID: 59827
		public const string ID = "SlimeSickness";

		// Token: 0x0400E9B4 RID: 59828
		public const string RECOVERY_ID = "SlimeSicknessRecovery";

		// Token: 0x02003C82 RID: 15490
		public class SlimeLungComponent : Sickness.SicknessComponent
		{
			// Token: 0x0600EDB0 RID: 60848 RVA: 0x00143F64 File Offset: 0x00142164
			public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
			{
				SlimeSickness.SlimeLungComponent.StatesInstance statesInstance = new SlimeSickness.SlimeLungComponent.StatesInstance(diseaseInstance);
				statesInstance.StartSM();
				return statesInstance;
			}

			// Token: 0x0600EDB1 RID: 60849 RVA: 0x00143F72 File Offset: 0x00142172
			public override void OnCure(GameObject go, object instance_data)
			{
				((SlimeSickness.SlimeLungComponent.StatesInstance)instance_data).StopSM("Cured");
			}

			// Token: 0x0600EDB2 RID: 60850 RVA: 0x00143F84 File Offset: 0x00142184
			public override List<Descriptor> GetSymptoms()
			{
				return new List<Descriptor>
				{
					new Descriptor(DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM, DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM_TOOLTIP, Descriptor.DescriptorType.SymptomAidable, false)
				};
			}

			// Token: 0x02003C83 RID: 15491
			public class StatesInstance : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.GameInstance
			{
				// Token: 0x0600EDB4 RID: 60852 RVA: 0x00143FB4 File Offset: 0x001421B4
				public StatesInstance(SicknessInstance master) : base(master)
				{
				}

				// Token: 0x0600EDB5 RID: 60853 RVA: 0x004E3264 File Offset: 0x004E1464
				public Reactable GetReactable()
				{
					Emote cough = Db.Get().Emotes.Minion.Cough;
					SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(base.master.gameObject, "SlimeLungCough", Db.Get().ChoreTypes.Cough, 0f, 0f, float.PositiveInfinity, 0f);
					selfEmoteReactable.SetEmote(cough);
					selfEmoteReactable.RegisterEmoteStepCallbacks("react", null, new Action<GameObject>(this.FinishedCoughing));
					return selfEmoteReactable;
				}

				// Token: 0x0600EDB6 RID: 60854 RVA: 0x004E32EC File Offset: 0x004E14EC
				private void ProduceSlime(GameObject cougher)
				{
					AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
					int gameCell = Grid.PosToCell(cougher);
					string id = Db.Get().Diseases.SlimeGerms.Id;
					Equippable equippable = base.master.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						equippable.GetComponent<Storage>().AddGasChunk(SimHashes.ContaminatedOxygen, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, false, true);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex(id), 1000, true, -1);
					}
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, base.master.modifier.Name, 1000), cougher.transform, 1.5f, false);
				}

				// Token: 0x0600EDB7 RID: 60855 RVA: 0x00143FBD File Offset: 0x001421BD
				private void FinishedCoughing(GameObject cougher)
				{
					this.ProduceSlime(cougher);
					base.sm.coughFinished.Trigger(this);
				}

				// Token: 0x0400E9B5 RID: 59829
				public float lastCoughTime;
			}

			// Token: 0x02003C84 RID: 15492
			public class States : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance>
			{
				// Token: 0x0600EDB8 RID: 60856 RVA: 0x004E340C File Offset: 0x004E160C
				public override void InitializeStates(out StateMachine.BaseState default_state)
				{
					default_state = this.breathing;
					this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false);
					this.breathing.normal.Enter("SetCoughTime", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi)
					{
						if (smi.lastCoughTime < Time.time)
						{
							smi.lastCoughTime = Time.time;
						}
					}).Update("Cough", delegate(SlimeSickness.SlimeLungComponent.StatesInstance smi, float dt)
					{
						if (!smi.master.IsDoctored && Time.time - smi.lastCoughTime > 20f)
						{
							smi.GoTo(this.breathing.cough);
						}
					}, UpdateRate.SIM_4000ms, false);
					this.breathing.cough.ToggleReactable((SlimeSickness.SlimeLungComponent.StatesInstance smi) => smi.GetReactable()).OnSignal(this.coughFinished, this.breathing.normal);
					this.notbreathing.TagTransition(new Tag[]
					{
						GameTags.NoOxygen
					}, this.breathing, true);
				}

				// Token: 0x0400E9B6 RID: 59830
				public StateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.Signal coughFinished;

				// Token: 0x0400E9B7 RID: 59831
				public SlimeSickness.SlimeLungComponent.States.BreathingStates breathing;

				// Token: 0x0400E9B8 RID: 59832
				public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State notbreathing;

				// Token: 0x02003C85 RID: 15493
				public class BreathingStates : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State
				{
					// Token: 0x0400E9B9 RID: 59833
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State normal;

					// Token: 0x0400E9BA RID: 59834
					public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State cough;
				}
			}
		}
	}
}
