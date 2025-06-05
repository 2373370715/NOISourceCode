using System;
using Klei;
using UnityEngine;

// Token: 0x02000D02 RID: 3330
public class Chlorinator : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>
{
	// Token: 0x06003FE4 RID: 16356 RVA: 0x002470A4 File Offset: 0x002452A4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.TagTransition(GameTags.Operational, this.ready, false);
		this.ready.TagTransition(GameTags.Operational, this.inoperational, true).DefaultState(this.ready.idle);
		this.ready.idle.EventTransition(GameHashes.OnStorageChange, this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).EnterTransition(this.ready.wait, (Chlorinator.StatesInstance smi) => smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.wait.ScheduleGoTo(new Func<Chlorinator.StatesInstance, float>(Chlorinator.GetPoppingDelay), this.ready.popPre).EnterTransition(this.ready.idle, (Chlorinator.StatesInstance smi) => !smi.CanEmit()).Target(this.hopper).PlayAnim("hopper_idle_loop");
		this.ready.popPre.Target(this.hopper).PlayAnim("meter_hopper_pre").OnAnimQueueComplete(this.ready.pop);
		this.ready.pop.Enter(delegate(Chlorinator.StatesInstance smi)
		{
			smi.TryEmit();
		}).Target(this.hopper).PlayAnim("meter_hopper_loop").OnAnimQueueComplete(this.ready.popPst);
		this.ready.popPst.Target(this.hopper).PlayAnim("meter_hopper_pst").OnAnimQueueComplete(this.ready.wait);
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x000CDF46 File Offset: 0x000CC146
	public static float GetPoppingDelay(Chlorinator.StatesInstance smi)
	{
		return smi.def.popWaitRange.Get();
	}

	// Token: 0x04002C23 RID: 11299
	private GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State inoperational;

	// Token: 0x04002C24 RID: 11300
	private Chlorinator.ReadyStates ready;

	// Token: 0x04002C25 RID: 11301
	public StateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.TargetParameter hopper;

	// Token: 0x02000D03 RID: 3331
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002C26 RID: 11302
		public MathUtil.MinMax popWaitRange = new MathUtil.MinMax(0.2f, 0.8f);

		// Token: 0x04002C27 RID: 11303
		public Tag primaryOreTag;

		// Token: 0x04002C28 RID: 11304
		public float primaryOreMassPerOre;

		// Token: 0x04002C29 RID: 11305
		public MathUtil.MinMaxInt primaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04002C2A RID: 11306
		public Tag secondaryOreTag;

		// Token: 0x04002C2B RID: 11307
		public float secondaryOreMassPerOre;

		// Token: 0x04002C2C RID: 11308
		public MathUtil.MinMaxInt secondaryOreCount = new MathUtil.MinMaxInt(1, 1);

		// Token: 0x04002C2D RID: 11309
		public Vector3 offset = Vector3.zero;

		// Token: 0x04002C2E RID: 11310
		public MathUtil.MinMax initialVelocity = new MathUtil.MinMax(1f, 3f);

		// Token: 0x04002C2F RID: 11311
		public MathUtil.MinMax initialDirectionHalfAngleDegreesRange = new MathUtil.MinMax(160f, 20f);
	}

	// Token: 0x02000D04 RID: 3332
	public class ReadyStates : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State
	{
		// Token: 0x04002C30 RID: 11312
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State idle;

		// Token: 0x04002C31 RID: 11313
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State wait;

		// Token: 0x04002C32 RID: 11314
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPre;

		// Token: 0x04002C33 RID: 11315
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State pop;

		// Token: 0x04002C34 RID: 11316
		public GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.State popPst;
	}

	// Token: 0x02000D05 RID: 3333
	public class StatesInstance : GameStateMachine<Chlorinator, Chlorinator.StatesInstance, IStateMachineTarget, Chlorinator.Def>.GameInstance
	{
		// Token: 0x06003FE9 RID: 16361 RVA: 0x00247318 File Offset: 0x00245518
		public StatesInstance(IStateMachineTarget master, Chlorinator.Def def) : base(master, def)
		{
			this.storage = base.GetComponent<ComplexFabricator>().outStorage;
			KAnimControllerBase component = master.GetComponent<KAnimControllerBase>();
			this.hopperMeter = new MeterController(component, "meter_target", "meter_hopper_pre", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
			{
				"meter_target"
			});
			base.sm.hopper.Set(this.hopperMeter.gameObject, this, false);
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x000CDF68 File Offset: 0x000CC168
		public bool CanEmit()
		{
			return !this.storage.IsEmpty();
		}

		// Token: 0x06003FEB RID: 16363 RVA: 0x0024738C File Offset: 0x0024558C
		public void TryEmit()
		{
			this.TryEmit(base.smi.def.primaryOreCount.Get(), base.def.primaryOreTag, base.def.primaryOreMassPerOre);
			this.TryEmit(base.smi.def.secondaryOreCount.Get(), base.def.secondaryOreTag, base.def.secondaryOreMassPerOre);
		}

		// Token: 0x06003FEC RID: 16364 RVA: 0x002473FC File Offset: 0x002455FC
		private void TryEmit(int oreSpawnCount, Tag emitTag, float amount)
		{
			GameObject gameObject = this.storage.FindFirst(emitTag);
			if (gameObject == null)
			{
				return;
			}
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			Substance substance = component.Element.substance;
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float temperature;
			this.storage.ConsumeAndGetDisease(emitTag, amount, out num, out diseaseInfo, out temperature);
			if (num <= 0f)
			{
				return;
			}
			float mass = num * component.MassPerUnit / (float)oreSpawnCount;
			Vector3 vector = base.smi.gameObject.transform.position;
			vector += base.def.offset;
			bool flag = UnityEngine.Random.value >= 0.5f;
			for (int i = 0; i < oreSpawnCount; i++)
			{
				float f = base.def.initialDirectionHalfAngleDegreesRange.Get() * 3.1415927f / 180f;
				Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
				if (flag)
				{
					normalized.x = -normalized.x;
				}
				flag = !flag;
				normalized = normalized.normalized;
				Vector3 v = normalized * base.def.initialVelocity.Get();
				Vector3 vector2 = vector;
				vector2 += normalized * 0.1f;
				GameObject go = substance.SpawnResource(vector2, mass, temperature, diseaseInfo.idx, diseaseInfo.count / oreSpawnCount, false, false, false);
				KFMOD.PlayOneShot(GlobalAssets.GetSound("Chlorinator_popping", false), CameraController.Instance.GetVerticallyScaledPosition(vector2, false), 1f);
				if (GameComps.Fallers.Has(go))
				{
					GameComps.Fallers.Remove(go);
				}
				GameComps.Fallers.Add(go, v);
			}
		}

		// Token: 0x04002C35 RID: 11317
		public Storage storage;

		// Token: 0x04002C36 RID: 11318
		public MeterController hopperMeter;
	}
}
