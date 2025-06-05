using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000CE1 RID: 3297
public class BeeHive : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>
{
	// Token: 0x06003F2B RID: 16171 RVA: 0x00244AA4 File Offset: 0x00242CA4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.enabled.grownStates;
		this.root.DoTutorial(Tutorial.TutorialMessages.TM_Radiation).Enter(delegate(BeeHive.StatesInstance smi)
		{
			AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(smi.gameObject);
			if (amountInstance != null)
			{
				amountInstance.hide = true;
			}
		}).EventHandler(GameHashes.Died, delegate(BeeHive.StatesInstance smi)
		{
			PrimaryElement component = smi.GetComponent<PrimaryElement>();
			Storage component2 = smi.GetComponent<Storage>();
			byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
			component2.AddOre(SimHashes.NuclearWaste, BeeHiveTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeHiveTuning.GERMS_DROPPED_ON_DEATH, false, true);
			component2.DropAll(smi.master.transform.position, true, true, default(Vector3), true, null);
		});
		this.disabled.ToggleTag(GameTags.Creatures.Behaviours.DisableCreature).EventTransition(GameHashes.FoundationChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled()).EventTransition(GameHashes.EnteredBreathableArea, this.enabled, (BeeHive.StatesInstance smi) => !smi.IsDisabled());
		this.enabled.EventTransition(GameHashes.FoundationChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.EntombedChanged, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).EventTransition(GameHashes.Drowning, this.disabled, (BeeHive.StatesInstance smi) => smi.IsDisabled()).DefaultState(this.enabled.grownStates);
		this.enabled.growingStates.ParamTransition<float>(this.hiveGrowth, this.enabled.grownStates, (BeeHive.StatesInstance smi, float f) => f >= 1f).DefaultState(this.enabled.growingStates.idle);
		this.enabled.growingStates.idle.Update(delegate(BeeHive.StatesInstance smi, float dt)
		{
			smi.DeltaGrowth(dt / 600f / BeeHiveTuning.HIVE_GROWTH_TIME);
		}, UpdateRate.SIM_4000ms, false);
		this.enabled.grownStates.ParamTransition<float>(this.hiveGrowth, this.enabled.growingStates, (BeeHive.StatesInstance smi, float f) => f < 1f).DefaultState(this.enabled.grownStates.dayTime);
		this.enabled.grownStates.dayTime.EventTransition(GameHashes.Nighttime, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.nightTime, (BeeHive.StatesInstance smi) => GameClock.Instance.IsNighttime());
		this.enabled.grownStates.nightTime.EventTransition(GameHashes.NewDay, (BeeHive.StatesInstance smi) => GameClock.Instance, this.enabled.grownStates.dayTime, (BeeHive.StatesInstance smi) => GameClock.Instance.GetTimeSinceStartOfCycle() <= 1f).Exit(delegate(BeeHive.StatesInstance smi)
		{
			if (!GameClock.Instance.IsNighttime())
			{
				smi.SpawnNewLarvaFromHive();
			}
		});
	}

	// Token: 0x04002BAF RID: 11183
	public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State disabled;

	// Token: 0x04002BB0 RID: 11184
	public BeeHive.EnabledStates enabled;

	// Token: 0x04002BB1 RID: 11185
	public StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter hiveGrowth = new StateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.FloatParameter(1f);

	// Token: 0x02000CE2 RID: 3298
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002BB2 RID: 11186
		public string beePrefabID;

		// Token: 0x04002BB3 RID: 11187
		public string larvaPrefabID;
	}

	// Token: 0x02000CE3 RID: 3299
	public class GrowingStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04002BB4 RID: 11188
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State idle;
	}

	// Token: 0x02000CE4 RID: 3300
	public class GrownStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04002BB5 RID: 11189
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State dayTime;

		// Token: 0x04002BB6 RID: 11190
		public GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State nightTime;
	}

	// Token: 0x02000CE5 RID: 3301
	public class EnabledStates : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.State
	{
		// Token: 0x04002BB7 RID: 11191
		public BeeHive.GrowingStates growingStates;

		// Token: 0x04002BB8 RID: 11192
		public BeeHive.GrownStates grownStates;
	}

	// Token: 0x02000CE6 RID: 3302
	public class StatesInstance : GameStateMachine<BeeHive, BeeHive.StatesInstance, IStateMachineTarget, BeeHive.Def>.GameInstance
	{
		// Token: 0x06003F31 RID: 16177 RVA: 0x000CD8AF File Offset: 0x000CBAAF
		public StatesInstance(IStateMachineTarget master, BeeHive.Def def) : base(master, def)
		{
			base.Subscribe(1119167081, new Action<object>(this.OnNewGameSpawn));
			Components.BeeHives.Add(this);
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x000CD8DB File Offset: 0x000CBADB
		public void SetUpNewHive()
		{
			base.sm.hiveGrowth.Set(0f, this, false);
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x000CD8F5 File Offset: 0x000CBAF5
		protected override void OnCleanUp()
		{
			Components.BeeHives.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x000CD908 File Offset: 0x000CBB08
		private void OnNewGameSpawn(object data)
		{
			this.NewGamePopulateHive();
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00244E3C File Offset: 0x0024303C
		private void NewGamePopulateHive()
		{
			int num = 1;
			for (int i = 0; i < num; i++)
			{
				this.SpawnNewBeeFromHive();
			}
			num = 1;
			for (int j = 0; j < num; j++)
			{
				this.SpawnNewLarvaFromHive();
			}
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x000CD910 File Offset: 0x000CBB10
		public bool IsFullyGrown()
		{
			return base.sm.hiveGrowth.Get(this) >= 1f;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00244E74 File Offset: 0x00243074
		public void DeltaGrowth(float delta)
		{
			float num = base.sm.hiveGrowth.Get(this);
			num += delta;
			Mathf.Clamp01(num);
			base.sm.hiveGrowth.Set(num, this, false);
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x000CD92D File Offset: 0x000CBB2D
		public void SpawnNewLarvaFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.larvaPrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000CD95A File Offset: 0x000CBB5A
		public void SpawnNewBeeFromHive()
		{
			Util.KInstantiate(Assets.GetPrefab(base.def.beePrefabID), base.transform.GetPosition()).SetActive(true);
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x00244EB4 File Offset: 0x002430B4
		public bool IsDisabled()
		{
			KPrefabID component = base.GetComponent<KPrefabID>();
			return component.HasTag(GameTags.Creatures.HasNoFoundation) || component.HasTag(GameTags.Entombed) || component.HasTag(GameTags.Creatures.Drowning);
		}
	}
}
