using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000834 RID: 2100
public class ReactionMonitor : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>
{
	// Token: 0x0600250F RID: 9487 RVA: 0x001D86F4 File Offset: 0x001D68F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.EventHandler(GameHashes.DestinationReached, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		}).EventHandler(GameHashes.NavigationFailed, delegate(ReactionMonitor.Instance smi)
		{
			smi.ClearLastReaction();
		});
		this.idle.Enter("ClearReactable", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Set(null, smi, false);
		}).TagTransition(GameTags.Dead, this.dead, false);
		this.reacting.Enter("Reactable.Begin", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).Begin(smi.gameObject);
		}).Enter(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(-909573545, null);
		}).Enter("Reactable.AddChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
			}
		}).Update("Reactable.Update", delegate(ReactionMonitor.Instance smi, float dt)
		{
			this.reactable.Get(smi).Update(dt);
		}, UpdateRate.SIM_200ms, false).Exit(delegate(ReactionMonitor.Instance smi)
		{
			smi.master.Trigger(824899998, null);
		}).Exit("Reactable.End", delegate(ReactionMonitor.Instance smi)
		{
			this.reactable.Get(smi).End();
		}).Exit("Reactable.RemoveChorePreventionTag", delegate(ReactionMonitor.Instance smi)
		{
			if (this.reactable.Get(smi).preventChoreInterruption)
			{
				smi.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
			}
		}).EventTransition(GameHashes.NavigationFailed, this.idle, null).TagTransition(GameTags.Dying, this.dead, false).TagTransition(GameTags.Dead, this.dead, false);
		this.dead.DoNothing();
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000BC95F File Offset: 0x000BAB5F
	private static bool ShouldReact(ReactionMonitor.Instance smi)
	{
		return smi.ImmediateReactable != null;
	}

	// Token: 0x04001998 RID: 6552
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State idle;

	// Token: 0x04001999 RID: 6553
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State reacting;

	// Token: 0x0400199A RID: 6554
	public GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.State dead;

	// Token: 0x0400199B RID: 6555
	public StateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.ObjectParameter<Reactable> reactable;

	// Token: 0x02000835 RID: 2101
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400199C RID: 6556
		public ObjectLayer ReactionLayer;
	}

	// Token: 0x02000836 RID: 2102
	public new class Instance : GameStateMachine<ReactionMonitor, ReactionMonitor.Instance, IStateMachineTarget, ReactionMonitor.Def>.GameInstance
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06002519 RID: 9497 RVA: 0x000BCA0E File Offset: 0x000BAC0E
		// (set) Token: 0x0600251A RID: 9498 RVA: 0x000BCA16 File Offset: 0x000BAC16
		public Reactable ImmediateReactable { get; private set; }

		// Token: 0x0600251B RID: 9499 RVA: 0x000BCA1F File Offset: 0x000BAC1F
		public Instance(IStateMachineTarget master, ReactionMonitor.Def def) : base(master, def)
		{
			this.animController = base.GetComponent<KBatchedAnimController>();
			this.lastReactTimes = new Dictionary<HashedString, float>();
			this.oneshotReactables = new List<Reactable>();
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000BCA56 File Offset: 0x000BAC56
		public bool CanReact(Emote e)
		{
			return this.animController != null && e.IsValidForController(this.animController);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x001D8898 File Offset: 0x001D6A98
		public bool TryReact(Reactable reactable, float clockTime, Navigator.ActiveTransition transition = null)
		{
			if (reactable == null)
			{
				return false;
			}
			float num;
			if ((this.lastReactTimes.TryGetValue(reactable.id, out num) && num == this.lastReaction) || clockTime - num < reactable.localCooldown)
			{
				return false;
			}
			if (!reactable.CanBegin(base.gameObject, transition))
			{
				return false;
			}
			this.lastReactTimes[reactable.id] = clockTime;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.smi.GoTo(base.sm.reacting);
			return true;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x001D8928 File Offset: 0x001D6B28
		public void PollForReactables(Navigator.ActiveTransition transition)
		{
			if (this.IsReacting())
			{
				return;
			}
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (reactable.IsExpired())
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
			Vector2I vector2I = Grid.CellToXY(Grid.PosToCell(base.smi.gameObject));
			ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[(int)base.def.ReactionLayer];
			ListPool<ScenePartitionerEntry, ReactionMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ReactionMonitor>.Allocate();
			GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, layer, pooledList);
			float num = float.NaN;
			float time = GameClock.Instance.GetTime();
			for (int j = 0; j < pooledList.Count; j++)
			{
				Reactable reactable2 = pooledList[j].obj as Reactable;
				if (this.TryReact(reactable2, time, transition))
				{
					num = time;
					break;
				}
			}
			this.lastReaction = num;
			pooledList.Recycle();
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x000BCA74 File Offset: 0x000BAC74
		public void ClearLastReaction()
		{
			this.lastReaction = float.NaN;
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x001D8A30 File Offset: 0x001D6C30
		public void StopReaction()
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				if (base.sm.reactable.Get(base.smi) == this.oneshotReactables[i])
				{
					this.oneshotReactables[i].Cleanup();
					this.oneshotReactables.RemoveAt(i);
					break;
				}
			}
			base.smi.GoTo(base.sm.idle);
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x000BCA81 File Offset: 0x000BAC81
		public bool IsReacting()
		{
			return base.smi.IsInsideState(base.sm.reacting);
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x001D8AB0 File Offset: 0x001D6CB0
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, HashedString reactionId, Emote emote, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			if (!this.CanReact(emote))
			{
				return null;
			}
			SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(target, reactionId, choreType, globalCooldown, localCooldown, lifeSpan, maxInitialDelay);
			selfEmoteReactable.SetEmote(emote);
			int num = 0;
			while (emotePreconditions != null && num < emotePreconditions.Count)
			{
				selfEmoteReactable.AddPrecondition(emotePreconditions[num]);
				num++;
			}
			if (isOneShot)
			{
				this.AddOneshotReactable(selfEmoteReactable);
			}
			return selfEmoteReactable;
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x001D8B14 File Offset: 0x001D6D14
		public SelfEmoteReactable AddSelfEmoteReactable(GameObject target, string reactionId, string emoteAnim, bool isOneShot, ChoreType choreType, float globalCooldown = 0f, float localCooldown = 20f, float maxTriggerTime = float.NegativeInfinity, float maxInitialDelay = 0f, List<Reactable.ReactablePrecondition> emotePreconditions = null)
		{
			Emote emote = new Emote(null, reactionId, new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			}, emoteAnim);
			return this.AddSelfEmoteReactable(target, reactionId, emote, isOneShot, choreType, globalCooldown, localCooldown, maxTriggerTime, maxInitialDelay, emotePreconditions);
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x000BCA99 File Offset: 0x000BAC99
		public void AddOneshotReactable(SelfEmoteReactable reactable)
		{
			if (reactable == null)
			{
				return;
			}
			this.oneshotReactables.Add(reactable);
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x001D8B64 File Offset: 0x001D6D64
		public void CancelOneShotReactable(SelfEmoteReactable cancel_target)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				Reactable reactable = this.oneshotReactables[i];
				if (cancel_target == reactable)
				{
					reactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x001D8BB0 File Offset: 0x001D6DB0
		public void CancelOneShotReactables(Emote reactionEmote)
		{
			for (int i = this.oneshotReactables.Count - 1; i >= 0; i--)
			{
				EmoteReactable emoteReactable = this.oneshotReactables[i] as EmoteReactable;
				if (emoteReactable != null && emoteReactable.emote == reactionEmote)
				{
					emoteReactable.Cleanup();
					this.oneshotReactables.RemoveAt(i);
				}
			}
		}

		// Token: 0x0400199E RID: 6558
		private KBatchedAnimController animController;

		// Token: 0x0400199F RID: 6559
		private float lastReaction = float.NaN;

		// Token: 0x040019A0 RID: 6560
		private Dictionary<HashedString, float> lastReactTimes;

		// Token: 0x040019A1 RID: 6561
		private List<Reactable> oneshotReactables;
	}
}
