using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
public class SparkleStreaker : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance>
{
	// Token: 0x060023AF RID: 9135 RVA: 0x001D2B84 File Offset: 0x001D0D84
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsSparkleStreaker").ToggleLoopingSound(this.soundPath, null, true, true, true).Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.sparkleStreakFX = Util.KInstantiate(EffectPrefabs.Instance.SparkleStreakFX, smi.master.transform.GetPosition() + this.offset);
			smi.sparkleStreakFX.transform.SetParent(smi.master.transform);
			smi.sparkleStreakFX.SetActive(true);
			smi.CreatePasserbyReactable();
		}).Exit(delegate(SparkleStreaker.Instance smi)
		{
			Util.KDestroyGameObject(smi.sparkleStreakFX);
			smi.ClearPasserbyReactable();
		});
		this.overjoyed.idle.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(0f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.moving, (SparkleStreaker.Instance smi) => smi.IsMoving());
		this.overjoyed.moving.Enter(delegate(SparkleStreaker.Instance smi)
		{
			smi.SetSparkleSoundParam(1f);
		}).EventTransition(GameHashes.ObjectMovementStateChanged, this.overjoyed.idle, (SparkleStreaker.Instance smi) => !smi.IsMoving());
	}

	// Token: 0x040017FB RID: 6139
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x040017FC RID: 6140
	public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017FD RID: 6141
	public SparkleStreaker.OverjoyedStates overjoyed;

	// Token: 0x040017FE RID: 6142
	public string soundPath = GlobalAssets.GetSound("SparkleStreaker_lp", false);

	// Token: 0x040017FF RID: 6143
	public HashedString SPARKLE_STREAKER_MOVING_PARAMETER = "sparkleStreaker_moving";

	// Token: 0x020007E5 RID: 2021
	public class OverjoyedStates : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04001800 RID: 6144
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04001801 RID: 6145
		public GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x020007E6 RID: 2022
	public new class Instance : GameStateMachine<SparkleStreaker, SparkleStreaker.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060023B3 RID: 9139 RVA: 0x000BBAC6 File Offset: 0x000B9CC6
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x001D2DC0 File Offset: 0x001D0FC0
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
				emoteReactable.SetEmote(clapCheer).SetThought(Db.Get().Thoughts.Happy).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000BBACF File Offset: 0x000B9CCF
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.GetComponent<Effects>().Add("SawSparkleStreaker", true);
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x000BB98A File Offset: 0x000B9B8A
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x000BBAE3 File Offset: 0x000B9CE3
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x000BBAFF File Offset: 0x000B9CFF
		public bool IsMoving()
		{
			return base.smi.master.GetComponent<Navigator>().IsMoving();
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000BBB16 File Offset: 0x000B9D16
		public void SetSparkleSoundParam(float val)
		{
			base.GetComponent<LoopingSounds>().SetParameter(GlobalAssets.GetSound("SparkleStreaker_lp", false), "sparkleStreaker_moving", val);
		}

		// Token: 0x04001802 RID: 6146
		private Reactable passerbyReactable;

		// Token: 0x04001803 RID: 6147
		public GameObject sparkleStreakFX;
	}
}
