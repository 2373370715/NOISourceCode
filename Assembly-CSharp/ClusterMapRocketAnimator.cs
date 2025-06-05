using System;
using UnityEngine;

// Token: 0x020019BA RID: 6586
public class ClusterMapRocketAnimator : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06008957 RID: 35159 RVA: 0x003664D8 File Offset: 0x003646D8
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.Transition(null, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.entityTarget.IsNull), UpdateRate.SIM_200ms).Target(this.entityTarget).EventHandlerTransition(GameHashes.RocketSelfDestructRequested, this.exploding, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.StartMining, this.utility.mining, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true).EventHandlerTransition(GameHashes.RocketLaunched, this.moving.takeoff, (ClusterMapRocketAnimator.StatesInstance smi, object data) => true);
		this.idle.Target(this.masterTarget).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("idle_loop", KAnim.PlayMode.Loop);
		}).Target(this.entityTarget).Transition(this.moving.traveling, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling), UpdateRate.SIM_200ms).Transition(this.grounded, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.moving.landing, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsLanding), UpdateRate.SIM_200ms).Transition(this.utility.mining, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsMining), UpdateRate.SIM_200ms);
		this.grounded.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(false, smi);
			smi.ToggleVisAnim(false);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
			smi.ToggleVisAnim(true);
		}).Target(this.entityTarget).EventTransition(GameHashes.RocketLaunched, this.moving.takeoff, null);
		this.moving.takeoff.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.landing.Transition(this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
			this.ToggleSelectable(false, smi);
		}).Exit(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			this.ToggleSelectable(true, smi);
		});
		this.moving.traveling.DefaultState(this.moving.traveling.regular).Target(this.entityTarget).EventTransition(GameHashes.ClusterLocationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)));
		this.moving.traveling.regular.Target(this.entityTarget).Transition(this.moving.traveling.boosted, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.moving.traveling.boosted.Target(this.entityTarget).Transition(this.moving.traveling.regular, GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsBoosted)), UpdateRate.SIM_200ms).Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("boosted", KAnim.PlayMode.Loop);
		});
		this.utility.Target(this.masterTarget).EventTransition(GameHashes.ClusterDestinationChanged, this.idle, new StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling));
		this.utility.mining.DefaultState(this.utility.mining.pre).Target(this.entityTarget).EventTransition(GameHashes.StopMining, this.utility.mining.pst, null);
		this.utility.mining.pre.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pre", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.utility.mining.loop);
			});
		});
		this.utility.mining.loop.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_loop", KAnim.PlayMode.Loop);
		});
		this.utility.mining.pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("mining_pst", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.idle);
			});
		});
		this.exploding.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().SwapAnims(new KAnimFile[]
			{
				Assets.GetAnim("rocket_self_destruct_kanim")
			});
			smi.PlayVisAnim("explode", KAnim.PlayMode.Once);
			smi.SubscribeOnVisAnimComplete(delegate(object data)
			{
				smi.GoTo(this.exploding_pst);
			});
		});
		this.exploding_pst.Enter(delegate(ClusterMapRocketAnimator.StatesInstance smi)
		{
			smi.GetComponent<ClusterMapVisualizer>().GetFirstAnimController().Stop();
			smi.entity.gameObject.Trigger(-1311384361, null);
		});
	}

	// Token: 0x06008958 RID: 35160 RVA: 0x003669A8 File Offset: 0x00364BA8
	private bool ClusterChangedAtMyLocation(ClusterMapRocketAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x06008959 RID: 35161 RVA: 0x000FE488 File Offset: 0x000FC688
	private bool IsTraveling(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling() && ((Clustercraft)smi.entity).HasResourcesToMove(1, Clustercraft.CombustionResource.All);
	}

	// Token: 0x0600895A RID: 35162 RVA: 0x000FE4B0 File Offset: 0x000FC6B0
	private bool IsBoosted(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).controlStationBuffTimeRemaining > 0f;
	}

	// Token: 0x0600895B RID: 35163 RVA: 0x000FE4C9 File Offset: 0x000FC6C9
	private bool IsGrounded(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x0600895C RID: 35164 RVA: 0x000FE4DE File Offset: 0x000FC6DE
	private bool IsLanding(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).Status == Clustercraft.CraftStatus.Landing;
	}

	// Token: 0x0600895D RID: 35165 RVA: 0x000FE4F3 File Offset: 0x000FC6F3
	private bool IsMining(ClusterMapRocketAnimator.StatesInstance smi)
	{
		return ((Clustercraft)smi.entity).HasTag(GameTags.POIHarvesting);
	}

	// Token: 0x0600895E RID: 35166 RVA: 0x003669EC File Offset: 0x00364BEC
	private bool IsSurfaceTransitioning(ClusterMapRocketAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x0600895F RID: 35167 RVA: 0x00366A24 File Offset: 0x00364C24
	private void ToggleSelectable(bool isSelectable, ClusterMapRocketAnimator.StatesInstance smi)
	{
		if (smi.entity.IsNullOrDestroyed())
		{
			return;
		}
		KSelectable component = smi.entity.GetComponent<KSelectable>();
		component.IsSelectable = isSelectable;
		if (!isSelectable && component.IsSelected && ClusterMapScreen.Instance.GetMode() != ClusterMapScreen.Mode.SelectDestination)
		{
			ClusterMapSelectTool.Instance.Select(null, true);
			SelectTool.Instance.Select(null, true);
		}
	}

	// Token: 0x040067E2 RID: 26594
	public StateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x040067E3 RID: 26595
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x040067E4 RID: 26596
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x040067E5 RID: 26597
	public ClusterMapRocketAnimator.MovingStates moving;

	// Token: 0x040067E6 RID: 26598
	public ClusterMapRocketAnimator.UtilityStates utility;

	// Token: 0x040067E7 RID: 26599
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding;

	// Token: 0x040067E8 RID: 26600
	public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State exploding_pst;

	// Token: 0x020019BB RID: 6587
	public class TravelingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040067E9 RID: 26601
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State regular;

		// Token: 0x040067EA RID: 26602
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State boosted;
	}

	// Token: 0x020019BC RID: 6588
	public class MovingStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040067EB RID: 26603
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State takeoff;

		// Token: 0x040067EC RID: 26604
		public ClusterMapRocketAnimator.TravelingStates traveling;

		// Token: 0x040067ED RID: 26605
		public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;
	}

	// Token: 0x020019BD RID: 6589
	public class UtilityStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x040067EE RID: 26606
		public ClusterMapRocketAnimator.UtilityStates.MiningStates mining;

		// Token: 0x020019BE RID: 6590
		public class MiningStates : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State
		{
			// Token: 0x040067EF RID: 26607
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pre;

			// Token: 0x040067F0 RID: 26608
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State loop;

			// Token: 0x040067F1 RID: 26609
			public GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.State pst;
		}
	}

	// Token: 0x020019BF RID: 6591
	public class StatesInstance : GameStateMachine<ClusterMapRocketAnimator, ClusterMapRocketAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x0600896E RID: 35182 RVA: 0x000FE572 File Offset: 0x000FC772
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x0600896F RID: 35183 RVA: 0x000FE377 File Offset: 0x000FC577
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x06008970 RID: 35184 RVA: 0x003663AC File Offset: 0x003645AC
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06008971 RID: 35185 RVA: 0x00366B94 File Offset: 0x00364D94
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x06008972 RID: 35186 RVA: 0x000FE59B File Offset: 0x000FC79B
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClusterMapRocketAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x06008973 RID: 35187 RVA: 0x000FE5D5 File Offset: 0x000FC7D5
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x040067F2 RID: 26610
		public ClusterGridEntity entity;

		// Token: 0x040067F3 RID: 26611
		private int animCompleteHandle = -1;

		// Token: 0x040067F4 RID: 26612
		private GameObject animCompleteSubscriber;
	}
}
