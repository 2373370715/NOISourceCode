using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02001A04 RID: 6660
[SkipSaveFileSerialization]
public class Stinky : StateMachineComponent<Stinky.StatesInstance>
{
	// Token: 0x06008AAE RID: 35502 RVA: 0x000FF2DC File Offset: 0x000FD4DC
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x06008AAF RID: 35503 RVA: 0x0036B0A8 File Offset: 0x003692A8
	private void Emit(object data)
	{
		GameObject gameObject = (GameObject)data;
		Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
		Vector2 a = gameObject.transform.GetPosition();
		for (int i = 0; i < liveMinionIdentities.Count; i++)
		{
			MinionIdentity minionIdentity = liveMinionIdentities[i];
			if (minionIdentity.gameObject != gameObject.gameObject)
			{
				Vector2 b = minionIdentity.transform.GetPosition();
				if (Vector2.SqrMagnitude(a - b) <= 2.25f)
				{
					minionIdentity.Trigger(508119890, Strings.Get("STRINGS.DUPLICANTS.DISEASES.PUTRIDODOUR.CRINGE_EFFECT").String);
					minionIdentity.GetComponent<Effects>().Add("SmelledStinky", true);
					minionIdentity.gameObject.GetSMI<ThoughtGraph.Instance>().AddThought(Db.Get().Thoughts.PutridOdour);
				}
			}
		}
		int gameCell = Grid.PosToCell(gameObject.transform.GetPosition());
		float value = Db.Get().Amounts.Temperature.Lookup(this).value;
		SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.ElementConsumerSimUpdate, 0.0025000002f, value, byte.MaxValue, 0, true, -1);
		GameObject gameObject2 = gameObject;
		bool flag = SoundEvent.ObjectIsSelectedAndVisible(gameObject2);
		Vector3 vector = gameObject2.transform.GetPosition();
		float volume = 1f;
		if (flag)
		{
			vector = SoundEvent.AudioHighlightListenerPosition(vector);
			volume = SoundEvent.GetVolume(flag);
		}
		else
		{
			vector.z = 0f;
		}
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Dupe_Flatulence", false), vector, volume);
	}

	// Token: 0x040068B1 RID: 26801
	private const float EmitMass = 0.0025000002f;

	// Token: 0x040068B2 RID: 26802
	private const SimHashes EmitElement = SimHashes.ContaminatedOxygen;

	// Token: 0x040068B3 RID: 26803
	private const float EmissionRadius = 1.5f;

	// Token: 0x040068B4 RID: 26804
	private const float MaxDistanceSq = 2.25f;

	// Token: 0x040068B5 RID: 26805
	private KBatchedAnimController stinkyController;

	// Token: 0x040068B6 RID: 26806
	private static readonly HashedString[] WorkLoopAnims = new HashedString[]
	{
		"working_pre",
		"working_loop",
		"working_pst"
	};

	// Token: 0x02001A05 RID: 6661
	public class StatesInstance : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.GameInstance
	{
		// Token: 0x06008AB2 RID: 35506 RVA: 0x000FF331 File Offset: 0x000FD531
		public StatesInstance(Stinky master) : base(master)
		{
		}
	}

	// Token: 0x02001A06 RID: 6662
	public class States : GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky>
	{
		// Token: 0x06008AB3 RID: 35507 RVA: 0x0036B224 File Offset: 0x00369424
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.TagTransition(GameTags.Dead, null, false).Enter(delegate(Stinky.StatesInstance smi)
			{
				KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("odor_fx_kanim", smi.master.gameObject.transform.GetPosition(), smi.master.gameObject.transform, true, Grid.SceneLayer.Front, false);
				kbatchedAnimController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				smi.master.stinkyController = kbatchedAnimController;
			}).Update("StinkyFX", delegate(Stinky.StatesInstance smi, float dt)
			{
				if (smi.master.stinkyController != null)
				{
					smi.master.stinkyController.Play(Stinky.WorkLoopAnims, KAnim.PlayMode.Once);
				}
			}, UpdateRate.SIM_4000ms, false);
			this.idle.Enter("ScheduleNextFart", delegate(Stinky.StatesInstance smi)
			{
				smi.ScheduleGoTo(this.GetNewInterval(), this.emit);
			});
			this.emit.Enter("Fart", delegate(Stinky.StatesInstance smi)
			{
				smi.master.Emit(smi.master.gameObject);
			}).ToggleExpression(Db.Get().Expressions.Relief, null).ScheduleGoTo(3f, this.idle);
		}

		// Token: 0x06008AB4 RID: 35508 RVA: 0x000FF33A File Offset: 0x000FD53A
		private float GetNewInterval()
		{
			return Mathf.Min(Mathf.Max(Util.GaussianRandom(TRAITS.STINKY_EMIT_INTERVAL_MAX - TRAITS.STINKY_EMIT_INTERVAL_MIN, 1f), TRAITS.STINKY_EMIT_INTERVAL_MIN), TRAITS.STINKY_EMIT_INTERVAL_MAX);
		}

		// Token: 0x040068B7 RID: 26807
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State idle;

		// Token: 0x040068B8 RID: 26808
		public GameStateMachine<Stinky.States, Stinky.StatesInstance, Stinky, object>.State emit;
	}
}
