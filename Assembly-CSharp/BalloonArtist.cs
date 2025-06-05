using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;

// Token: 0x020007D3 RID: 2003
public class BalloonArtist : GameStateMachine<BalloonArtist, BalloonArtist.Instance>
{
	// Token: 0x06002367 RID: 9063 RVA: 0x001D2180 File Offset: 0x001D0380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ParamTransition<int>(this.balloonsGivenOut, this.overjoyed.exitEarly, (BalloonArtist.Instance smi, int p) => p >= TRAITS.JOY_REACTIONS.BALLOON_ARTIST.NUM_BALLOONS_TO_GIVE).Exit(delegate(BalloonArtist.Instance smi)
		{
			smi.numBalloonsGiven = 0;
			this.balloonsGivenOut.Set(0, smi, false);
		});
		this.overjoyed.idle.Enter(delegate(BalloonArtist.Instance smi)
		{
			if (smi.IsRecTime())
			{
				smi.GoTo(this.overjoyed.balloon_stand);
			}
		}).ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistPlanning, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.balloon_stand, (BalloonArtist.Instance smi) => smi.IsRecTime());
		this.overjoyed.balloon_stand.ToggleStatusItem(Db.Get().DuplicantStatusItems.BalloonArtistHandingOut, null).EventTransition(GameHashes.ScheduleBlocksChanged, this.overjoyed.idle, (BalloonArtist.Instance smi) => !smi.IsRecTime()).ToggleChore((BalloonArtist.Instance smi) => new BalloonArtistChore(smi.master), this.overjoyed.idle);
		this.overjoyed.exitEarly.Enter(delegate(BalloonArtist.Instance smi)
		{
			smi.ExitJoyReactionEarly();
		});
	}

	// Token: 0x040017C1 RID: 6081
	public StateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.IntParameter balloonsGivenOut;

	// Token: 0x040017C2 RID: 6082
	public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017C3 RID: 6083
	public BalloonArtist.OverjoyedStates overjoyed;

	// Token: 0x020007D4 RID: 2004
	public class OverjoyedStates : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040017C4 RID: 6084
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040017C5 RID: 6085
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State balloon_stand;

		// Token: 0x040017C6 RID: 6086
		public GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.State exitEarly;
	}

	// Token: 0x020007D5 RID: 2005
	public new class Instance : GameStateMachine<BalloonArtist, BalloonArtist.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600236C RID: 9068 RVA: 0x000BB751 File Offset: 0x000B9951
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x000BB75A File Offset: 0x000B995A
		[OnDeserialized]
		private void OnDeserialized()
		{
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x001D2348 File Offset: 0x001D0548
		public void Internal_InitBalloons()
		{
			JoyResponseOutfitTarget joyResponseOutfitTarget = JoyResponseOutfitTarget.FromMinion(base.master.gameObject);
			if (!this.balloonSymbolIter.IsNullOrDestroyed())
			{
				if (this.balloonSymbolIter.facade.AndThen<string>((BalloonArtistFacadeResource f) => f.Id) == joyResponseOutfitTarget.ReadFacadeId())
				{
					return;
				}
			}
			this.balloonSymbolIter = joyResponseOutfitTarget.ReadFacadeId().AndThen<BalloonArtistFacadeResource>((string id) => Db.Get().Permits.BalloonArtistFacades.Get(id)).AndThen<BalloonOverrideSymbolIter>((BalloonArtistFacadeResource permit) => permit.GetSymbolIter()).UnwrapOr(new BalloonOverrideSymbolIter(Option.None), null);
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Current());
		}

		// Token: 0x0600236F RID: 9071 RVA: 0x000BB77F File Offset: 0x000B997F
		public bool IsRecTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation);
		}

		// Token: 0x06002370 RID: 9072 RVA: 0x001D2438 File Offset: 0x001D0638
		public void SetBalloonSymbolOverride(BalloonOverrideSymbol balloonOverrideSymbol)
		{
			if (balloonOverrideSymbol.animFile.IsNone())
			{
				base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", Assets.GetAnim("balloon_anim_kanim").GetData().build.GetSymbol("body"), 0);
				return;
			}
			base.master.GetComponent<SymbolOverrideController>().AddSymbolOverride("body", balloonOverrideSymbol.symbol.Unwrap(), 0);
		}

		// Token: 0x06002371 RID: 9073 RVA: 0x000BB7A0 File Offset: 0x000B99A0
		public BalloonOverrideSymbol GetCurrentBalloonSymbolOverride()
		{
			return this.balloonSymbolIter.Current();
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000BB7AD File Offset: 0x000B99AD
		public void ApplyNextBalloonSymbolOverride()
		{
			this.SetBalloonSymbolOverride(this.balloonSymbolIter.Next());
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000BB7C0 File Offset: 0x000B99C0
		public void GiveBalloon()
		{
			this.numBalloonsGiven++;
			base.smi.sm.balloonsGivenOut.Set(this.numBalloonsGiven, base.smi, false);
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x001D24C0 File Offset: 0x001D06C0
		public void ExitJoyReactionEarly()
		{
			JoyBehaviourMonitor.Instance smi = base.master.gameObject.GetSMI<JoyBehaviourMonitor.Instance>();
			smi.sm.exitEarly.Trigger(smi);
		}

		// Token: 0x040017C7 RID: 6087
		[Serialize]
		public int numBalloonsGiven;

		// Token: 0x040017C8 RID: 6088
		[NonSerialized]
		private BalloonOverrideSymbolIter balloonSymbolIter;

		// Token: 0x040017C9 RID: 6089
		private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

		// Token: 0x040017CA RID: 6090
		private const int TARGET_OVERRIDE_PRIORITY = 0;
	}
}
