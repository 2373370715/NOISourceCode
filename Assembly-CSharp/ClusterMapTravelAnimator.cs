using System;
using UnityEngine;

// Token: 0x020019C4 RID: 6596
public class ClusterMapTravelAnimator : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06008984 RID: 35204 RVA: 0x00366BD8 File Offset: 0x00364DD8
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.idle;
		this.root.OnTargetLost(this.entityTarget, null);
		this.idle.Target(this.entityTarget).Transition(this.grounded, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsGrounded), UpdateRate.SIM_200ms).Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling)).Target(this.masterTarget);
		this.grounded.Transition(this.surfaceTransitioning, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning), UpdateRate.SIM_200ms);
		this.surfaceTransitioning.Update(delegate(ClusterMapTravelAnimator.StatesInstance smi, float dt)
		{
			this.DoOrientToPath(smi);
		}, UpdateRate.SIM_200ms, false).Transition(this.repositioning, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsSurfaceTransitioning)), UpdateRate.SIM_200ms);
		this.repositioning.Transition(this.traveling.orientToIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoReposition), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.DefaultState(this.traveling.orientToPath);
		this.traveling.travelIdle.Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToIdle, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.IsTraveling))).EventTransition(GameHashes.ClusterDestinationChanged, this.traveling.orientToPath, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath))).EventTransition(GameHashes.ClusterLocationChanged, this.traveling.move, GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Not(new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove))).Target(this.masterTarget);
		this.traveling.orientToPath.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToPath), UpdateRate.RENDER_EVERY_TICK).Target(this.entityTarget).EventHandlerTransition(GameHashes.ClusterLocationChanged, (ClusterMapTravelAnimator.StatesInstance smi) => Game.Instance, this.repositioning, new Func<ClusterMapTravelAnimator.StatesInstance, object, bool>(this.ClusterChangedAtMyLocation)).Target(this.masterTarget);
		this.traveling.move.Transition(this.traveling.travelIdle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoMove), UpdateRate.RENDER_EVERY_TICK);
		this.traveling.orientToIdle.Transition(this.idle, new StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.Transition.ConditionCallback(this.DoOrientToIdle), UpdateRate.RENDER_EVERY_TICK);
	}

	// Token: 0x06008985 RID: 35205 RVA: 0x000FE6A1 File Offset: 0x000FC8A1
	private bool IsTraveling(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.entity.GetComponent<ClusterTraveler>().IsTraveling();
	}

	// Token: 0x06008986 RID: 35206 RVA: 0x00366ECC File Offset: 0x003650CC
	private bool IsSurfaceTransitioning(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && (clustercraft.Status == Clustercraft.CraftStatus.Landing || clustercraft.Status == Clustercraft.CraftStatus.Launching);
	}

	// Token: 0x06008987 RID: 35207 RVA: 0x00366F04 File Offset: 0x00365104
	private bool IsGrounded(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Clustercraft clustercraft = smi.entity as Clustercraft;
		return clustercraft != null && clustercraft.Status == Clustercraft.CraftStatus.Grounded;
	}

	// Token: 0x06008988 RID: 35208 RVA: 0x00366F34 File Offset: 0x00365134
	private bool DoReposition(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x06008989 RID: 35209 RVA: 0x00366F34 File Offset: 0x00365134
	private bool DoMove(ClusterMapTravelAnimator.StatesInstance smi)
	{
		Vector3 position = ClusterGrid.Instance.GetPosition(smi.entity);
		return smi.MoveTowards(position, Time.unscaledDeltaTime);
	}

	// Token: 0x0600898A RID: 35210 RVA: 0x00366F60 File Offset: 0x00365160
	private bool DoOrientToPath(ClusterMapTravelAnimator.StatesInstance smi)
	{
		float pathAngle = smi.GetComponent<ClusterMapVisualizer>().GetPathAngle();
		return smi.RotateTowards(pathAngle, Time.unscaledDeltaTime);
	}

	// Token: 0x0600898B RID: 35211 RVA: 0x000FE6B3 File Offset: 0x000FC8B3
	private bool DoOrientToIdle(ClusterMapTravelAnimator.StatesInstance smi)
	{
		return smi.keepRotationOnIdle || smi.RotateTowards(0f, Time.unscaledDeltaTime);
	}

	// Token: 0x0600898C RID: 35212 RVA: 0x00366F88 File Offset: 0x00365188
	private bool ClusterChangedAtMyLocation(ClusterMapTravelAnimator.StatesInstance smi, object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		return clusterLocationChangedEvent.oldLocation == smi.entity.Location || clusterLocationChangedEvent.newLocation == smi.entity.Location;
	}

	// Token: 0x04006804 RID: 26628
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State idle;

	// Token: 0x04006805 RID: 26629
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State grounded;

	// Token: 0x04006806 RID: 26630
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State repositioning;

	// Token: 0x04006807 RID: 26631
	public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State surfaceTransitioning;

	// Token: 0x04006808 RID: 26632
	public ClusterMapTravelAnimator.TravelingStates traveling;

	// Token: 0x04006809 RID: 26633
	public StateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x020019C5 RID: 6597
	private class Tuning : TuningData<ClusterMapTravelAnimator.Tuning>
	{
		// Token: 0x0400680A RID: 26634
		public float visualizerTransitionSpeed = 1f;

		// Token: 0x0400680B RID: 26635
		public float visualizerRotationSpeed = 1f;
	}

	// Token: 0x020019C6 RID: 6598
	public class TravelingStates : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State
	{
		// Token: 0x0400680C RID: 26636
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State travelIdle;

		// Token: 0x0400680D RID: 26637
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToPath;

		// Token: 0x0400680E RID: 26638
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State move;

		// Token: 0x0400680F RID: 26639
		public GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.State orientToIdle;
	}

	// Token: 0x020019C7 RID: 6599
	public class StatesInstance : GameStateMachine<ClusterMapTravelAnimator, ClusterMapTravelAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x06008991 RID: 35217 RVA: 0x000FE707 File Offset: 0x000FC907
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x06008992 RID: 35218 RVA: 0x00366FCC File Offset: 0x003651CC
		public bool MoveTowards(Vector3 targetPosition, float dt)
		{
			RectTransform component = base.GetComponent<RectTransform>();
			ClusterMapVisualizer component2 = base.GetComponent<ClusterMapVisualizer>();
			Vector3 localPosition = component.GetLocalPosition();
			Vector3 vector = targetPosition - localPosition;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			float num = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerTransitionSpeed * dt;
			if (num < magnitude)
			{
				Vector3 b = normalized * num;
				component.SetLocalPosition(localPosition + b);
				component2.RefreshPathDrawing();
				return false;
			}
			component.SetLocalPosition(targetPosition);
			component2.RefreshPathDrawing();
			return true;
		}

		// Token: 0x06008993 RID: 35219 RVA: 0x00367050 File Offset: 0x00365250
		public bool RotateTowards(float targetAngle, float dt)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			float num = targetAngle - this.simpleAngle;
			if (num > 180f)
			{
				num -= 360f;
			}
			else if (num < -180f)
			{
				num += 360f;
			}
			float num2 = TuningData<ClusterMapTravelAnimator.Tuning>.Get().visualizerRotationSpeed * dt;
			if (num > 0f && num2 < num)
			{
				this.simpleAngle += num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			if (num < 0f && -num2 > num)
			{
				this.simpleAngle -= num2;
				component.SetAnimRotation(this.simpleAngle);
				return false;
			}
			this.simpleAngle = targetAngle;
			component.SetAnimRotation(this.simpleAngle);
			return true;
		}

		// Token: 0x04006810 RID: 26640
		public ClusterGridEntity entity;

		// Token: 0x04006811 RID: 26641
		private float simpleAngle;

		// Token: 0x04006812 RID: 26642
		public bool keepRotationOnIdle;
	}
}
