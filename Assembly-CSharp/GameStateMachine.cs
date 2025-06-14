﻿using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

public abstract class GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> where StateMachineInstanceType : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameInstance where MasterType : IStateMachineTarget
{
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback And(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) && second_condition(smi);
	}

	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Or(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback first_condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback second_condition)
	{
		return (StateMachineInstanceType smi) => first_condition(smi) || second_condition(smi);
	}

	public static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback Not(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback transition_cb)
	{
		return (StateMachineInstanceType smi) => !transition_cb(smi);
	}

	public override void BindStates()
	{
		base.BindState(null, this.root, "root");
		base.BindStates(this.root, this);
	}

	public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State();

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsFalse = (StateMachineInstanceType smi, bool p) => !p;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Callback IsTrue = (StateMachineInstanceType smi, bool p) => p;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsZero = (StateMachineInstanceType smi, float p) => p == 0f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTZero = (StateMachineInstanceType smi, float p) => p < 0f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEZero = (StateMachineInstanceType smi, float p) => p <= 0f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTZero = (StateMachineInstanceType smi, float p) => p > 0f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEZero = (StateMachineInstanceType smi, float p) => p >= 0f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsOne = (StateMachineInstanceType smi, float p) => p == 1f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTOne = (StateMachineInstanceType smi, float p) => p < 1f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsLTEOne = (StateMachineInstanceType smi, float p) => p <= 1f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTOne = (StateMachineInstanceType smi, float p) => p > 1f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Callback IsGTEOne = (StateMachineInstanceType smi, float p) => p >= 1f;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNotNull = (StateMachineInstanceType smi, GameObject p) => p != null;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Callback IsNull = (StateMachineInstanceType smi, GameObject p) => p == null;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsZero_Int = (StateMachineInstanceType smi, int p) => p == 0;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsLTEOne_Int = (StateMachineInstanceType smi, int p) => p <= 1;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTOne_Int = (StateMachineInstanceType smi, int p) => p > 1;

	protected static StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Callback IsGTZero_Int = (StateMachineInstanceType smi, int p) => p > 0;

	public class PreLoopPostState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pre;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State loop;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pst;
	}

	public class WorkingState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State waiting;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pre;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_loop;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State working_pst;
	}

	public class GameInstance : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance
	{
		public void Queue(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Queue(anim, mode, 1f, 0f);
		}

		public void Play(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			base.smi.GetComponent<KBatchedAnimController>().Play(anim, mode, 1f, 0f);
		}

		public GameInstance(MasterType master, DefType def) : base(master)
		{
			base.def = def;
		}

		public GameInstance(MasterType master) : base(master)
		{
		}
	}

	public class TagTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		public TagTransitionData(string name, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, Tag[] tags, bool on_remove, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, Tag[]> tags_callback = null) : base(name, source_state, target_state, idx, null)
		{
			this.tags = tags;
			this.onRemove = on_remove;
			this.target = target;
			this.tags_callback = tags_callback;
		}

		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (!this.onRemove)
			{
				if (!this.HasAllTags(stateMachineInstanceType))
				{
					return;
				}
			}
			else if (this.HasAnyTags(stateMachineInstanceType))
			{
				return;
			}
			this.ExecuteTransition(stateMachineInstanceType);
		}

		private bool HasAllTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAllTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		private bool HasAnyTags(StateMachineInstanceType smi)
		{
			return this.target.Get(smi).GetComponent<KPrefabID>().HasAnyTags((this.tags_callback != null) ? this.tags_callback(smi) : this.tags);
		}

		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			if (this.is_executing)
			{
				return;
			}
			this.is_executing = true;
			smi.GoTo(this.targetState);
			this.is_executing = false;
		}

		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.target.Get(smi) == null)
			{
				return;
			}
			if (!this.onRemove)
			{
				if (!this.HasAllTags(smi))
				{
					return;
				}
			}
			else if (this.HasAnyTags(smi))
			{
				return;
			}
			this.ExecuteTransition(smi);
		}

		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType smi_internal = smi as StateMachineInstanceType;
			global::Debug.Assert(smi_internal != null);
			StateMachine.BaseTransition.Context result = base.Register(smi_internal);
			result.handlerId = this.target.Get(smi_internal).Subscribe(-1582839653, delegate(object data)
			{
				this.OnCallback(smi_internal);
			});
			return result;
		}

		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			if (this.target.Get(stateMachineInstanceType) != null)
			{
				this.target.Get(stateMachineInstanceType).Unsubscribe(context.handlerId);
			}
		}

		private Tag[] tags;

		private bool onRemove;

		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		private bool is_executing;

		private Func<StateMachineInstanceType, Tag[]> tags_callback;
	}

	public class EventTransitionData : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition
	{
		public EventTransitionData(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target) : base(evt.ToString(), source_state, target_state, idx, condition)
		{
			this.evtId = evt;
			this.target = target;
			this.globalEventSystemCallback = global_event_system_callback;
		}

		public override void Evaluate(StateMachine.Instance smi)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			if (this.condition != null && this.condition(stateMachineInstanceType))
			{
				this.ExecuteTransition(stateMachineInstanceType);
			}
		}

		private void ExecuteTransition(StateMachineInstanceType smi)
		{
			smi.GoTo(this.targetState);
		}

		private void OnCallback(StateMachineInstanceType smi)
		{
			if (this.condition == null || this.condition(smi))
			{
				this.ExecuteTransition(smi);
			}
		}

		public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			StateMachineInstanceType smi_internal = smi as StateMachineInstanceType;
			global::Debug.Assert(smi_internal != null);
			StateMachine.BaseTransition.Context result = base.Register(smi_internal);
			Action<object> handler = delegate(object d)
			{
				this.OnCallback(smi_internal);
			};
			GameObject gameObject;
			if (this.globalEventSystemCallback != null)
			{
				gameObject = this.globalEventSystemCallback(smi_internal).gameObject;
			}
			else
			{
				gameObject = this.target.Get(smi_internal);
				if (gameObject == null)
				{
					throw new InvalidOperationException("TargetParameter: " + this.target.name + " is null");
				}
			}
			result.handlerId = gameObject.Subscribe((int)this.evtId, handler);
			return result;
		}

		public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
			global::Debug.Assert(stateMachineInstanceType != null);
			base.Unregister(stateMachineInstanceType, context);
			GameObject gameObject = null;
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					gameObject = kmonoBehaviour.gameObject;
				}
			}
			else
			{
				gameObject = this.target.Get(stateMachineInstanceType);
			}
			if (gameObject != null)
			{
				gameObject.Unsubscribe(context.handlerId);
			}
		}

		private GameHashes evtId;

		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;
	}

	public new class State : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter GetStateTarget()
		{
			if (this.stateTarget != null)
			{
				return this.stateTarget;
			}
			if (this.parent != null)
			{
				return ((GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)this.parent).GetStateTarget();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter targetParameter = this.sm.stateTarget;
			if (targetParameter == null)
			{
				return this.sm.masterTarget;
			}
			return targetParameter;
		}

		public int CreateDataTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int dataTableSize = stateMachineType.dataTableSize;
			stateMachineType.dataTableSize = dataTableSize + 1;
			return dataTableSize;
		}

		public int CreateUpdateTableEntry()
		{
			StateMachineType stateMachineType = this.sm;
			int updateTableSize = stateMachineType.updateTableSize;
			stateMachineType.updateTableSize = updateTableSize + 1;
			return updateTableSize;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State root
		{
			get
			{
				return this;
			}
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNothing()
		{
			return this;
		}

		private static List<StateMachine.Action> AddAction(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback, List<StateMachine.Action> actions, bool add_to_end)
		{
			if (actions == null)
			{
				actions = new List<StateMachine.Action>();
			}
			StateMachine.Action item = new StateMachine.Action(name, callback);
			if (add_to_end)
			{
				actions.Add(item);
			}
			else
			{
				actions.Insert(0, item);
			}
			return actions;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State master
		{
			get
			{
				this.stateTarget = this.sm.masterTarget;
				return this;
			}
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
		{
			this.stateTarget = target;
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.Update(this.sm.name + "." + this.name, callback, update_rate, load_balance);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.BatchUpdate(this.sm.name + "." + this.name, batch_update, update_rate);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Enter("Enter", callback);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.Exit("Exit", callback);
		}

		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InternalUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater bucket_updater, UpdateRate update_rate, bool load_balance, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update = null)
		{
			int updateTableIdx = this.CreateUpdateTableEntry();
			if (this.updateActions == null)
			{
				this.updateActions = new List<StateMachine.UpdateAction>();
			}
			StateMachine.UpdateAction updateAction = default(StateMachine.UpdateAction);
			updateAction.updateTableIdx = updateTableIdx;
			updateAction.updateRate = update_rate;
			updateAction.updater = bucket_updater;
			int num = 1;
			if (load_balance)
			{
				num = Singleton<StateMachineUpdater>.Instance.GetFrameCount(update_rate);
			}
			updateAction.buckets = new StateMachineUpdater.BaseUpdateBucket[num];
			for (int i = 0; i < num; i++)
			{
				UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = new UpdateBucketWithUpdater<StateMachineInstanceType>(name);
				updateBucketWithUpdater.batch_update_delegate = batch_update;
				Singleton<StateMachineUpdater>.Instance.AddBucket(update_rate, updateBucketWithUpdater);
				updateAction.buckets[i] = updateBucketWithUpdater;
			}
			this.updateActions.Add(updateAction);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State UpdateTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State destination_state, Func<StateMachineInstanceType, float, bool> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			Action<StateMachineInstanceType, float> checkCallback = delegate(StateMachineInstanceType smi, float dt)
			{
				if (callback(smi, dt))
				{
					smi.GoTo(destination_state);
				}
			};
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				checkCallback(smi, 0f);
			});
			this.Update(checkCallback, update_rate, load_balance);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Update(string name, Action<StateMachineInstanceType, float> callback, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, new BucketUpdater<StateMachineInstanceType>(callback), update_rate, load_balance, null);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BatchUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.BatchUpdateDelegate batch_update, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			return this.InternalUpdate(name, null, update_rate, false, batch_update);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State FastUpdate(string name, UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater updater, UpdateRate update_rate = UpdateRate.SIM_200ms, bool load_balance = false)
		{
			return this.InternalUpdate(name, updater, update_rate, load_balance, null);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Enter(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.enterActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.enterActions, true);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Exit(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			this.exitActions = GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.AddAction(name, callback, this.exitActions, false);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Toggle(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback enter_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback exit_callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleEnter(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = GameStateMachineHelper.HasToggleEnteredFlag;
				enter_callback(smi);
			});
			this.Exit("ToggleExit(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					smi.dataTable[data_idx] = null;
					exit_callback(smi);
				}
			});
			return this;
		}

		private void Break(StateMachineInstanceType smi)
		{
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnEnter()
		{
			return this.Enter(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BreakOnExit()
		{
			return this.Exit(delegate(StateMachineInstanceType smi)
			{
				this.Break(smi);
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State AddEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, KAnimFile> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(kanimFile, 0f);
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				KAnimFile kanimFile = chooser_callback(smi);
				if (kanimFile == null)
				{
					return;
				}
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(kanimFile);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(Func<StateMachineInstanceType, HashedString> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableAnims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim == null)
					{
						string str = "Missing anims: ";
						HashedString hashedString2 = hashedString;
						global::Debug.LogWarning(str + hashedString2.ToString());
						return;
					}
					state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, 0f);
				}
			});
			this.Exit("Disableanims()", delegate(StateMachineInstanceType smi)
			{
				HashedString hashedString = chooser_callback(smi);
				if (hashedString == null)
				{
					return;
				}
				if (hashedString.IsValid)
				{
					KAnimFile anim = Assets.GetAnim(hashedString);
					if (anim != null)
					{
						state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
					}
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAnims(string anim_file, float priority = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Toggle("ToggleAnims(" + anim_file + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				if (anim == null)
				{
					global::Debug.LogError("Trying to add missing override anims:" + anim_file);
				}
				state_target.Get<KAnimControllerBase>(smi).AddAnimOverrides(anim, priority);
			}, delegate(StateMachineInstanceType smi)
			{
				KAnimFile anim = Assets.GetAnim(anim_file);
				state_target.Get<KAnimControllerBase>(smi).RemoveAnimOverrides(anim);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleAttributeModifier(string modifier_name, Func<StateMachineInstanceType, AttributeModifier> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					AttributeModifier attributeModifier = callback(smi);
					DebugUtil.Assert(smi.dataTable[data_idx] == null);
					smi.dataTable[data_idx] = attributeModifier;
					state_target.Get(smi).GetAttributes().Add(attributeModifier);
				}
			});
			this.Exit("RemoveAttributeModifier( " + modifier_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					AttributeModifier modifier = (AttributeModifier)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					GameObject gameObject = state_target.Get(smi);
					if (gameObject != null)
					{
						gameObject.GetAttributes().Remove(modifier);
					}
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string event_name, Func<StateMachineInstanceType, bool> condition = null, bool pause_on_game_pause = true, bool enable_culling = true, bool enable_camera_scaled_position = true)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(event_name, pause_on_game_pause, enable_culling, enable_camera_scaled_position);
				}
			});
			this.Exit("StopLoopingSound( " + event_name + " )", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<LoopingSounds>().StopSound(event_name);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleLoopingSound(string state_label, Func<StateMachineInstanceType, string> event_name_callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("StartLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					string text = event_name_callback(smi);
					smi.dataTable[data_idx] = text;
					state_target.Get(smi).GetComponent<LoopingSounds>().StartSound(text);
				}
			});
			this.Exit("StopLoopingSound( " + state_label + " )", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					state_target.Get(smi).GetComponent<LoopingSounds>().StopSound((string)smi.dataTable[data_idx]);
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RefreshUserMenuOnEnter()
		{
			this.Enter("RefreshUserMenuOnEnter()", delegate(StateMachineInstanceType smi)
			{
				UserMenu userMenu = Game.Instance.userMenu;
				MasterType master = smi.master;
				userMenu.Refresh(master.gameObject);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStartTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStarted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStartTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableStopTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkStopped)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableStopTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State WorkableCompleteTransition(Func<StateMachineInstanceType, Workable> get_workable_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("Enter WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable3 = get_workable_callback(smi);
				if (workable3 != null)
				{
					Action<Workable, Workable.WorkableEvent> action = delegate(Workable workable, Workable.WorkableEvent evt)
					{
						if (evt == Workable.WorkableEvent.WorkCompleted)
						{
							smi.GoTo(target_state);
						}
					};
					smi.dataTable[data_idx] = action;
					Workable workable2 = workable3;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(workable2.OnWorkableEventCB, action);
				}
			});
			this.Exit("Exit WorkableCompleteTransition(" + target_state.longName + ")", delegate(StateMachineInstanceType smi)
			{
				Workable workable = get_workable_callback(smi);
				if (workable != null)
				{
					Action<Workable, Workable.WorkableEvent> value = (Action<Workable, Workable.WorkableEvent>)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					Workable workable2 = workable;
					workable2.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(workable2.OnWorkableEventCB, value);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				smi.dataTable[data_idx] = gameObject;
				GameComps.Gravities.Add(gameObject, Vector2.zero, null);
			});
			this.Exit("RemoveComponent<Gravity>()", delegate(StateMachineInstanceType smi)
			{
				GameObject go = (GameObject)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				GameComps.Gravities.Remove(go);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleGravity(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State landed_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.EventTransition(GameHashes.Landed, landed_state, null);
			this.Toggle("GravityComponent", delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Add(state_target.Get(smi), Vector2.zero, null);
			}, delegate(StateMachineInstanceType smi)
			{
				GameComps.Gravities.Remove(state_target.Get(smi));
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
				}
			});
			if (condition_callback != null)
			{
				this.Update("ValidateThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition_callback(smi))
					{
						state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().AddThought(thought);
						return;
					}
					state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetSMI<ThoughtGraph.Instance>().RemoveThought(thought);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCreatureThought(Func<StateMachineInstanceType, Thought> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
			});
			this.Exit("DisableCreatureThought()", delegate(StateMachineInstanceType smi)
			{
				Thought thought = chooser_callback(smi);
				CreatureThoughtGraph.Instance smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveThought(thought);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCreatureThought(Thought thought, Func<StateMachineInstanceType, bool> condition_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition_callback == null || condition_callback(smi))
				{
					state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
				}
			});
			if (condition_callback != null)
			{
				this.Update("ValidateCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition_callback(smi))
					{
						state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().AddThought(thought);
						return;
					}
					state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>().RemoveThought(thought);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveCreatureThought(" + thought.Id + ")", delegate(StateMachineInstanceType smi)
			{
				CreatureThoughtGraph.Instance smi2 = state_target.Get(smi).GetSMI<CreatureThoughtGraph.Instance>();
				if (smi2 != null)
				{
					smi2.RemoveThought(thought);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Func<StateMachineInstanceType, Expression> chooser_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).AddExpression(chooser_callback(smi));
			});
			this.Exit("RemoveExpression", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<FaceGraph>(smi).RemoveExpression(chooser_callback(smi));
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleExpression(Expression expression, Func<StateMachineInstanceType, bool> condition = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					state_target.Get<FaceGraph>(smi).AddExpression(expression);
				}
			});
			if (condition != null)
			{
				this.Update("ValidateExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi, float dt)
				{
					if (condition(smi))
					{
						state_target.Get<FaceGraph>(smi).AddExpression(expression);
						return;
					}
					state_target.Get<FaceGraph>(smi).RemoveExpression(expression);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("RemoveExpression(" + expression.Id + ")", delegate(StateMachineInstanceType smi)
			{
				FaceGraph faceGraph = state_target.Get<FaceGraph>(smi);
				if (faceGraph != null)
				{
					faceGraph.RemoveExpression(expression);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, data);
			});
			this.Exit("RemoveMainStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleMainStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				object data = (callback != null) ? callback(smi) : smi;
				state_target.Get<KSelectable>(smi).SetStatusItem(Db.Get().StatusItemCategories.Main, status_item_cb(smi), data);
			});
			this.Exit("RemoveMainStatusItem(DynamicGeneration)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleCategoryStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"AddCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, (data != null) ? data : smi);
			});
			this.Exit(string.Concat(new string[]
			{
				"RemoveCategoryStatusItem(",
				category.Id,
				", ",
				status_item.Id,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null)
				{
					kselectable.SetStatusItem(category, null, null);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, object data = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				object obj = data;
				if (obj == null)
				{
					obj = smi;
				}
				Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, obj);
				smi.dataTable[data_idx] = guid;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleSnapOn(string snap_on)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("SnapOn(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<SnapOn>(smi).AttachSnapOnByName(snap_on);
			});
			this.Exit("SnapOff(" + snap_on + ")", delegate(StateMachineInstanceType smi)
			{
				SnapOn snapOn = state_target.Get<SnapOn>(smi);
				if (snapOn != null)
				{
					snapOn.DetachSnapOnByName(snap_on);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Tag tag)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(tag, false);
			});
			this.Exit("RemoveTag(" + tag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(tag);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleTag(Func<StateMachineInstanceType, Tag> behaviour_tag_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).AddTag(behaviour_tag_cb(smi), false);
			});
			this.Exit("RemoveTag(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<KPrefabID>(smi).RemoveTag(behaviour_tag_cb(smi));
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback)
		{
			return this.ToggleStatusItem(status_item, callback, null);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(StatusItem status_item, Func<StateMachineInstanceType, object> callback, StatusItemCategory category)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				if (category == null)
				{
					object data = (callback != null) ? callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(status_item, data);
					smi.dataTable[data_idx] = guid;
					return;
				}
				object data2 = (callback != null) ? callback(smi) : null;
				Guid guid2 = state_target.Get<KSelectable>(smi).SetStatusItem(category, status_item, data2);
				smi.dataTable[data_idx] = guid2;
			});
			this.Exit("RemoveStatusItem(" + status_item.Id + ")", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					if (category == null)
					{
						Guid guid = (Guid)smi.dataTable[data_idx];
						kselectable.RemoveStatusItem(guid, false);
					}
					else
					{
						kselectable.SetStatusItem(category, null, null);
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(Func<StateMachineInstanceType, StatusItem> status_item_cb, Func<StateMachineInstanceType, object> data_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("AddStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				StatusItem statusItem = status_item_cb(smi);
				if (statusItem != null)
				{
					object data = (data_callback != null) ? data_callback(smi) : null;
					Guid guid = state_target.Get<KSelectable>(smi).AddStatusItem(statusItem, data);
					smi.dataTable[data_idx] = guid;
				}
			});
			this.Exit("RemoveStatusItem(DynamicallyConstructed)", delegate(StateMachineInstanceType smi)
			{
				KSelectable kselectable = state_target.Get<KSelectable>(smi);
				if (kselectable != null && smi.dataTable[data_idx] != null)
				{
					Guid guid = (Guid)smi.dataTable[data_idx];
					kselectable.RemoveStatusItem(guid, false);
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFX(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				if (instance != null)
				{
					instance.StartSM();
					smi.dataTable[data_idx] = instance;
				}
			});
			this.Exit("DisableFX()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleFX.Exit");
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Func<StateMachineInstanceType, Tag> tag_cb, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete()", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag_cb(smi));
					smi.GoTo(null);
				});
			}
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State BehaviourComplete(Tag tag, bool on_exit = false)
		{
			if (on_exit)
			{
				this.Exit("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag);
					smi.GoTo(null);
				});
			}
			else
			{
				this.Enter("BehaviourComplete(" + tag.ToString() + ")", delegate(StateMachineInstanceType smi)
				{
					smi.Trigger(-739654666, tag);
					smi.GoTo(null);
				});
			}
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Tag behaviour_tag, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag, precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if ((Tag)data == behaviour_tag)
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBehaviour(Func<StateMachineInstanceType, Tag> behaviour_tag_cb, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback precondition, Action<StateMachineInstanceType> on_complete = null)
		{
			Func<object, bool> precondition_cb = (object obj) => precondition(obj as StateMachineInstanceType);
			this.Enter("AddPrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().AddBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.Exit("RemovePrecondition", delegate(StateMachineInstanceType smi)
			{
				if (smi.GetComponent<ChoreConsumer>() != null)
				{
					smi.GetComponent<ChoreConsumer>().RemoveBehaviourPrecondition(behaviour_tag_cb(smi), precondition_cb, smi);
				}
			});
			this.ToggleTag(behaviour_tag_cb);
			if (on_complete != null)
			{
				this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object data)
				{
					if ((Tag)data == behaviour_tag_cb(smi))
					{
						on_complete(smi);
					}
				});
			}
			return this;
		}

		public void ClearFetch(StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = (FetchList2)smi.dataTable[fetch_data_idx];
			if (fetchList != null)
			{
				smi.dataTable[fetch_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				fetchList.Cancel("ClearFetchListFromSM");
			}
		}

		public void SetupFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, StateMachineInstanceType smi, int fetch_data_idx, int callback_data_idx)
		{
			FetchList2 fetchList = create_fetchlist_callback(smi);
			System.Action action = delegate()
			{
				this.ClearFetch(smi, fetch_data_idx, callback_data_idx);
				smi.GoTo(target_state);
			};
			fetchList.Submit(action, true);
			smi.dataTable[fetch_data_idx] = fetchList;
			smi.dataTable[callback_data_idx] = action;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleFetch(Func<StateMachineInstanceType, FetchList2> create_fetchlist_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleFetchEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupFetch(create_fetchlist_callback, target_state, smi, data_idx, callback_data_idx);
			});
			this.Exit("ToggleFetchExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearFetch(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		private void ClearChore(StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx)
		{
			Chore chore = (Chore)smi.dataTable[chore_data_idx];
			if (chore != null)
			{
				Action<Chore> value = (Action<Chore>)smi.dataTable[callback_data_idx];
				smi.dataTable[chore_data_idx] = null;
				smi.dataTable[callback_data_idx] = null;
				Chore chore2 = chore;
				chore2.onExit = (Action<Chore>)Delegate.Remove(chore2.onExit, value);
				chore.Cancel("ClearGlobalChore");
			}
		}

		private Chore SetupChore(Func<StateMachineInstanceType, Chore> create_chore_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, StateMachineInstanceType smi, int chore_data_idx, int callback_data_idx, bool is_success_state_reentrant, bool is_failure_state_reentrant)
		{
			Chore chore = create_chore_callback(smi);
			DebugUtil.DevAssert(!chore.IsPreemptable, "ToggleChore can't be used with preemptable chores! :( (but it should...)", null);
			chore.runUntilComplete = false;
			Action<Chore> action = delegate(Chore chore_param)
			{
				bool isComplete = chore.isComplete;
				if ((isComplete & is_success_state_reentrant) || (is_failure_state_reentrant && !isComplete))
				{
					this.SetupChore(create_chore_callback, success_state, failure_state, smi, chore_data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
					return;
				}
				GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = success_state;
				if (!isComplete)
				{
					state = failure_state;
				}
				this.ClearChore(smi, chore_data_idx, callback_data_idx);
				smi.GoTo(state);
			};
			Chore chore2 = chore;
			chore2.onExit = (Action<Chore>)Delegate.Combine(chore2.onExit, action);
			smi.dataTable[chore_data_idx] = chore;
			smi.dataTable[callback_data_idx] = action;
			return chore;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback, Func<StateMachineInstanceType, bool> condition = null)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleRecurringChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					this.SetupChore(callback, this, this, smi, data_idx, callback_data_idx, true, true);
				}
			});
			this.Exit("ToggleRecurringChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleRecurringChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, Func<StateMachineInstanceType, bool> condition = null)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleRecurringChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				if (condition == null || condition(smi))
				{
					Chore arg = this.SetupChore(callback, this, this, smi, data_idx, callback_data_idx, true, true);
					processChore(smi, arg);
				}
			});
			this.Exit("ToggleRecurringChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				Chore arg = this.SetupChore(callback, target_state, target_state, smi, data_idx, callback_data_idx, false, false);
				processChore(smi, arg);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, target_state, target_state, smi, data_idx, callback_data_idx, false, false);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, Action<StateMachineInstanceType, Chore> processChore, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			bool is_success_state_reentrant = success_state == this;
			bool is_failure_state_reentrant = failure_state == this;
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				Chore arg = this.SetupChore(callback, success_state, failure_state, smi, data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
				processChore(smi, arg);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
				processChore(smi, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleChore(Func<StateMachineInstanceType, Chore> callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			int data_idx = this.CreateDataTableEntry();
			int callback_data_idx = this.CreateDataTableEntry();
			bool is_success_state_reentrant = success_state == this;
			bool is_failure_state_reentrant = failure_state == this;
			this.Enter("ToggleChoreEnter()", delegate(StateMachineInstanceType smi)
			{
				this.SetupChore(callback, success_state, failure_state, smi, data_idx, callback_data_idx, is_success_state_reentrant, is_failure_state_reentrant);
			});
			this.Exit("ToggleChoreExit()", delegate(StateMachineInstanceType smi)
			{
				this.ClearChore(smi, data_idx, callback_data_idx);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReactable(Func<StateMachineInstanceType, Reactable> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(delegate(StateMachineInstanceType smi)
			{
				smi.dataTable[data_idx] = callback(smi);
			});
			this.Exit(delegate(StateMachineInstanceType smi)
			{
				Reactable reactable = (Reactable)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (reactable != null)
				{
					reactable.Cleanup();
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State RemoveEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(string effect_name)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(effect_name, false);
			});
			this.Exit("RemoveEffect(" + effect_name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(effect_name);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, Effect> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleEffect(Func<StateMachineInstanceType, string> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Add(callback(smi), false);
			});
			this.Exit("RemoveEffect()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Effects>(smi).Remove(callback(smi));
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnExit(Func<StateMachineInstanceType, string> callback)
		{
			this.Enter("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State LogOnEnter(Func<StateMachineInstanceType, string> callback)
		{
			this.Exit("Log()", delegate(StateMachineInstanceType smi)
			{
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Urge urge)
		{
			return this.ToggleUrge((StateMachineInstanceType smi) => urge);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleUrge(Func<StateMachineInstanceType, Urge> urge_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("AddUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				state_target.Get<ChoreConsumer>(smi).AddUrge(urge);
			});
			this.Exit("RemoveUrge()", delegate(StateMachineInstanceType smi)
			{
				Urge urge = urge_callback(smi);
				ChoreConsumer choreConsumer = state_target.Get<ChoreConsumer>(smi);
				if (choreConsumer != null)
				{
					choreConsumer.RemoveUrge(urge);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnTargetLost(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state)
		{
			this.ParamTransition<GameObject>(parameter, target_state, (StateMachineInstanceType smi, GameObject p) => p == null);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleBrain(string reason)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StopBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Stop(reason);
			});
			this.Exit("ResetBrain(" + reason + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Brain>(smi).Reset(reason);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PreBrainUpdate(Action<StateMachineInstanceType> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action action = delegate()
				{
					callback(smi);
				};
				smi.dataTable[data_idx] = action;
				Brain brain = state_target.Get<Brain>(smi);
				DebugUtil.AssertArgs(brain != null, new object[]
				{
					"PreBrainUpdate cannot find a brain"
				});
				brain.onPreUpdate += action;
			});
			this.Exit("DisablePreBrainUpdate", delegate(StateMachineInstanceType smi)
			{
				System.Action value = (System.Action)smi.dataTable[data_idx];
				state_target.Get<Brain>(smi).onPreUpdate -= value;
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnEnter(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject go = state_target.Get(smi);
				object data = (callback != null) ? callback(smi) : null;
				go.Trigger((int)evt, data);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TriggerOnExit(GameHashes evt, Func<StateMachineInstanceType, object> callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Exit("Trigger(" + evt.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					object data = (callback != null) ? callback(smi) : null;
					gameObject.Trigger((int)evt, data);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachineList(Func<StateMachineInstanceType, Func<StateMachineInstanceType, StateMachine.Instance>[]> getListCallback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = new StateMachine.Instance[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					StateMachine.Instance instance = array[i](smi);
					instance.StartSM();
					array2[i] = instance;
				}
				smi.dataTable[data_idx] = array2;
			});
			this.Exit("DisableListOfStateMachines()", delegate(StateMachineInstanceType smi)
			{
				Func<StateMachineInstanceType, StateMachine.Instance>[] array = getListCallback(smi);
				StateMachine.Instance[] array2 = (StateMachine.Instance[])smi.dataTable[data_idx];
				for (int i = array.Length - 1; i >= 0; i--)
				{
					StateMachine.Instance instance = array2[i];
					if (instance != null)
					{
						instance.StopSM("ToggleListOfStateMachines.Exit");
					}
				}
				smi.dataTable[data_idx] = null;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStateMachine(Func<StateMachineInstanceType, StateMachine.Instance> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter("EnableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = callback(smi);
				smi.dataTable[data_idx] = instance;
				instance.StartSM();
			});
			this.Exit("DisableStateMachine()", delegate(StateMachineInstanceType smi)
			{
				StateMachine.Instance instance = (StateMachine.Instance)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				if (instance != null)
				{
					instance.StopSM("ToggleStateMachine.Exit");
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponentIfFound<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = !disable;
					}
				}
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				GameObject gameObject = state_target.Get(smi);
				if (gameObject != null)
				{
					ComponentType component = gameObject.GetComponent<ComponentType>();
					if (component != null)
					{
						component.enabled = disable;
					}
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleComponent<ComponentType>(bool disable = false) where ComponentType : MonoBehaviour
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = !disable;
			});
			this.Exit("DisableComponent(" + typeof(ComponentType).Name + ")", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<ComponentType>(smi).enabled = disable;
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeOperationalFlag(Operational.Flag flag, bool init_val = false)
		{
			this.Enter(string.Concat(new string[]
			{
				"InitOperationalFlag (",
				flag.Name,
				", ",
				init_val.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, init_val);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleOperationalFlag(Operational.Flag flag)
		{
			this.Enter("ToggleOperationalFlag True (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, true);
			});
			this.Exit("ToggleOperationalFlag False (" + flag.Name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GetComponent<Operational>().SetFlag(flag, false);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleReserve(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter reserver, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount)
		{
			int data_idx = this.CreateDataTableEntry();
			this.Enter(string.Concat(new string[]
			{
				"Reserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				GameObject gameObject = reserver.Get(smi);
				float val = requested_amount.Get(smi);
				float val2 = Mathf.Max(1f, Db.Get().Attributes.CarryAmount.Lookup(gameObject).GetTotalValue());
				float num = Math.Min(val, val2);
				num = Math.Min(num, pickupable.UnreservedFetchAmount);
				if (num <= 0f)
				{
					pickupable.PrintReservations();
					global::Debug.LogError(string.Concat(new string[]
					{
						val2.ToString(),
						", ",
						val.ToString(),
						", ",
						pickupable.UnreservedFetchAmount.ToString(),
						", ",
						num.ToString()
					}));
				}
				actual_amount.Set(num, smi, false);
				int num2 = pickupable.Reserve("ToggleReserve", gameObject.GetComponent<KPrefabID>().InstanceID, num);
				smi.dataTable[data_idx] = num2;
			});
			this.Exit(string.Concat(new string[]
			{
				"Unreserve(",
				pickup_target.name,
				", ",
				requested_amount.name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				int ticket = (int)smi.dataTable[data_idx];
				smi.dataTable[data_idx] = null;
				Pickupable pickupable = pickup_target.Get<Pickupable>(smi);
				if (pickupable != null)
				{
					pickupable.Unreserve("ToggleReserve", ticket);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork(string work_type, Action<StateMachineInstanceType> callback, Func<StateMachineInstanceType, bool> validate_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("StartWork(" + work_type + ")", delegate(StateMachineInstanceType smi)
			{
				if (validate_callback(smi))
				{
					callback(smi);
					return;
				}
				smi.GoTo(failure_state);
			});
			this.Update("Work(" + work_type + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				if (validate_callback(smi))
				{
					WorkerBase.WorkResult workResult = state_target.Get<WorkerBase>(smi).Work(dt);
					if (workResult == WorkerBase.WorkResult.Success)
					{
						smi.GoTo(success_state);
						return;
					}
					if (workResult == WorkerBase.WorkResult.Failed)
					{
						smi.GoTo(failure_state);
						return;
					}
				}
				else
				{
					smi.GoTo(failure_state);
				}
			}, UpdateRate.SIM_33ms, false);
			this.Exit("StopWork()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<WorkerBase>(smi).StopWork();
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleWork<WorkableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state, Func<StateMachineInstanceType, bool> is_valid_cb) where WorkableType : Workable
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork(typeof(WorkableType).Name, delegate(StateMachineInstanceType smi)
			{
				Workable workable = source_target.Get<WorkableType>(smi);
				state_target.Get<WorkerBase>(smi).StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => source_target.Get<WorkableType>(smi) != null && (is_valid_cb == null || is_valid_cb(smi)), success_state, failure_state);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoEat(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Eat", delegate(StateMachineInstanceType smi)
			{
				Edible workable = source_target.Get<Edible>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Edible.EdibleStartWorkInfo(workable, amount2));
			}, (StateMachineInstanceType smi) => source_target.Get<Edible>(smi) != null, success_state, failure_state);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoSleep(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter sleeper, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter bed, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Sleep", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				Sleepable workable = bed.Get<Sleepable>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => bed.Get<Sleepable>(smi) != null, success_state, failure_state);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoDelivery(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter worker_param, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter storage_param, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				WorkerBase workerBase = worker_param.Get<WorkerBase>(smi);
				Storage workable = storage_param.Get<Storage>(smi);
				workerBase.StartWork(new WorkerBase.StartWorkInfo(workable));
			}, (StateMachineInstanceType smi) => storage_param.Get<Storage>(smi) != null, success_state, failure_state);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoPickup(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter source_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter result_target, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.ToggleWork("Pickup", delegate(StateMachineInstanceType smi)
			{
				Pickupable pickupable = source_target.Get<Pickupable>(smi);
				WorkerBase workerBase = state_target.Get<WorkerBase>(smi);
				float amount2 = amount.Get(smi);
				workerBase.StartWork(new Pickupable.PickupableStartWorkInfo(pickupable, amount2, delegate(GameObject result)
				{
					result_target.Set(result, smi, false);
				}));
			}, (StateMachineInstanceType smi) => source_target.Get<Pickupable>(smi) != null || result_target.Get<Pickupable>(smi) != null, success_state, failure_state);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("EnableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				smi.dataTable[data_idx] = notification;
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			this.Exit("DisableNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = (Notification)smi.dataTable[data_idx];
				if (notification != null)
				{
					if (state_target != null)
					{
						Notifier notifier = state_target.Get<Notifier>(smi);
						if (notifier != null)
						{
							notifier.Remove(notification);
						}
					}
					smi.dataTable[data_idx] = null;
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoReport(ReportManager.ReportType reportType, Func<StateMachineInstanceType, float> callback, Func<StateMachineInstanceType, string> context_callback = null)
		{
			this.Enter("DoReport()", delegate(StateMachineInstanceType smi)
			{
				float value = callback(smi);
				string note = (context_callback != null) ? context_callback(smi) : null;
				ReportManager.Instance.ReportValue(reportType, value, note, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoNotification(Func<StateMachineInstanceType, Notification> callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("DoNotification()", delegate(StateMachineInstanceType smi)
			{
				Notification notification = callback(smi);
				state_target.AddOrGet<Notifier>(smi).Add(notification, "");
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DoTutorial(Tutorial.TutorialMessages msg)
		{
			this.Enter("DoTutorial()", delegate(StateMachineInstanceType smi)
			{
				Tutorial.Instance.TutorialMessage(msg, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleScheduleCallback(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> callback)
		{
			int data_idx = this.CreateDataTableEntry();
			Action<object> <>9__2;
			this.Enter("AddScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				GameScheduler instance = GameScheduler.Instance;
				string name2 = name;
				float time = time_cb(smi);
				Action<object> callback2;
				if ((callback2 = <>9__2) == null)
				{
					callback2 = (<>9__2 = delegate(object smi_data)
					{
						callback((StateMachineInstanceType)((object)smi_data));
					});
				}
				SchedulerHandle schedulerHandle = instance.Schedule(name2, time, callback2, smi, null);
				DebugUtil.Assert(smi.dataTable[data_idx] == null);
				smi.dataTable[data_idx] = schedulerHandle;
			});
			this.Exit("RemoveScheduledCallback(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				if (smi.dataTable[data_idx] != null)
				{
					SchedulerHandle schedulerHandle = (SchedulerHandle)smi.dataTable[data_idx];
					smi.dataTable[data_idx] = null;
					schedulerHandle.ClearScheduler();
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(Func<StateMachineInstanceType, float> time_cb, StateMachine.BaseState state)
		{
			this.Enter("ScheduleGoTo(" + state.name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time_cb(smi), state);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			string[] array = new string[5];
			array[0] = "ScheduleGoTo(";
			array[1] = time.ToString();
			array[2] = ", ";
			int num = 3;
			StateMachine.BaseState state2 = state;
			array[num] = ((state2 != null) ? state2.name : null);
			array[4] = ")";
			this.Enter(string.Concat(array), delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleGoTo(time, state);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, Func<StateMachineInstanceType, float> time_cb, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleAction(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time_cb(smi), delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleAction(string name, float time, Action<StateMachineInstanceType> action)
		{
			this.Enter(string.Concat(new string[]
			{
				"ScheduleAction(",
				time.ToString(),
				", ",
				name,
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				smi.Schedule(time, delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleActionNextFrame(string name, Action<StateMachineInstanceType> action)
		{
			this.Enter("ScheduleActionNextFrame(" + name + ")", delegate(StateMachineInstanceType smi)
			{
				smi.ScheduleNextFrame(delegate(object obj)
				{
					action(smi);
				}, null);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			if (this.events == null)
			{
				this.events = new List<StateEvent>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent(evt, callback, target, global_event_system_callback);
			this.events.Add(item);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				callback(smi);
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandler(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback)
		{
			this.EventHandler(evt, null, callback);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventHandlerTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, object, bool> callback)
		{
			return this.EventHandler(evt, global_event_system_callback, delegate(StateMachineInstanceType smi, object d)
			{
				if (callback(smi, d))
				{
					smi.GoTo(state);
				}
			});
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ParamTransition<ParameterType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition item = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Transition(this.transitions.Count, parameter, state, callback);
			this.transitions.Add(item);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, Func<StateMachineInstanceType, bool> callback)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, (StateMachineInstanceType smi, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter p) => callback(smi));
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnSignal(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal signal, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			this.ParamTransition<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>(signal, state, null);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EnterTransition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Transition(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, UpdateRate update_rate = UpdateRate.SIM_200ms)
		{
			string str = "(Stop)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("Transition(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				if (condition(smi))
				{
					smi.GoTo(state);
				}
			});
			this.FastUpdate("Transition(" + str + ")", new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.TransitionUpdater(condition, state), update_rate, false);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State DefaultState(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State default_state)
		{
			this.defaultState = default_state;
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EnterGoTo(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self", null);
			string str = "(null)";
			if (state != null)
			{
				str = state.name;
			}
			this.Enter("GoTo(" + str + ")", delegate(StateMachineInstanceType smi)
			{
				smi.GoTo(state);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State GoTo(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self", null);
			string str = "(null)";
			if (state != null)
			{
				str = state.name;
			}
			this.Update("GoTo(" + str + ")", delegate(StateMachineInstanceType smi, float dt)
			{
				smi.GoTo(state);
			}, UpdateRate.SIM_200ms, false);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State StopMoving()
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			this.Enter("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStationaryIdling()
		{
			this.GetStateTarget();
			this.ToggleTag(GameTags.StationaryIdling);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnBehaviourComplete(Tag behaviour, Action<StateMachineInstanceType> cb)
		{
			this.EventHandler(GameHashes.BehaviourTagComplete, delegate(StateMachineInstanceType smi, object d)
			{
				if ((Tag)d == behaviour)
				{
					cb(smi);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			return this.MoveTo(cell_callback, null, success_state, fail_state, update_cell);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo(Func<StateMachineInstanceType, int> cell_callback, Func<StateMachineInstanceType, CellOffset[]> cell_offsets_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state = null, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, bool update_cell = false)
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			CellOffset[] default_offset = new CellOffset[1];
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("MoveTo()", delegate(StateMachineInstanceType smi)
			{
				int cell = cell_callback(smi);
				Navigator navigator = state_target.Get<Navigator>(smi);
				CellOffset[] offsets = default_offset;
				if (cell_offsets_callback != null)
				{
					offsets = cell_offsets_callback(smi);
				}
				navigator.GoTo(cell, offsets);
			});
			if (update_cell)
			{
				this.Update("MoveTo()", delegate(StateMachineInstanceType smi, float dt)
				{
					int cell = cell_callback(smi);
					state_target.Get<Navigator>(smi).UpdateTarget(cell);
				}, UpdateRate.SIM_200ms, false);
			}
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get(smi).GetComponent<Navigator>().Stop(false, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets;
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, NavTactic tactic = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets(smi);
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State MoveTo<ApproachableType>(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_parameter, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, Func<StateMachineInstanceType, NavTactic> nav_tactic, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State fail_state = null, CellOffset[] override_offsets = null) where ApproachableType : IApproachable
		{
			this.EventTransition(GameHashes.DestinationReached, success_state, null);
			this.EventTransition(GameHashes.NavigationFailed, fail_state, null);
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			CellOffset[] offsets;
			this.Enter("MoveTo(" + move_parameter.name + ")", delegate(StateMachineInstanceType smi)
			{
				offsets = override_offsets;
				IApproachable approachable = move_parameter.Get<ApproachableType>(smi);
				KMonoBehaviour kmonoBehaviour = move_parameter.Get<KMonoBehaviour>(smi);
				if (kmonoBehaviour == null)
				{
					smi.GoTo(fail_state);
					return;
				}
				Navigator component = state_target.Get(smi).GetComponent<Navigator>();
				if (offsets == null)
				{
					offsets = approachable.GetOffsets();
				}
				NavTactic tactic = nav_tactic(smi);
				component.GoTo(kmonoBehaviour, offsets, tactic);
			});
			this.Exit("StopMoving()", delegate(StateMachineInstanceType smi)
			{
				state_target.Get<Navigator>(smi).Stop(false, true);
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State Face(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter face_target, float x_offset = 0f)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("Face", delegate(StateMachineInstanceType smi)
			{
				if (face_target != null)
				{
					IApproachable approachable = face_target.Get<IApproachable>(smi);
					if (approachable != null)
					{
						float target_x = approachable.transform.GetPosition().x + x_offset;
						state_target.Get<Facing>(smi).Face(target_x);
					}
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag[] tags, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData(tags.ToString(), this, state, this.transitions.Count, tags, on_remove, this.GetStateTarget(), null);
			this.transitions.Add(item);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Func<StateMachineInstanceType, Tag[]> tags_cb, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagTransitionData("DynamicTransition", this, state, this.transitions.Count, null, on_remove, this.GetStateTarget(), tags_cb);
			this.transitions.Add(item);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State TagTransition(Tag tag, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, bool on_remove = false)
		{
			return this.TagTransition(new Tag[]
			{
				tag
			}, state, on_remove);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			DebugUtil.DevAssert(state != this, "Can't transition to self!", null);
			if (this.transitions == null)
			{
				this.transitions = new List<StateMachine.BaseTransition>();
			}
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target = this.GetStateTarget();
			GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData item = new GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EventTransitionData(this, state, this.transitions.Count, evt, global_event_system_callback, condition, target);
			this.transitions.Add(item);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State EventTransition(GameHashes evt, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition = null)
		{
			return this.EventTransition(evt, null, state, condition);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ScheduleChange(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State targetState, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback callback)
		{
			return this.EventTransition(GameHashes.ScheduleBlocksChanged, targetState, callback).EventTransition(GameHashes.ScheduleChanged, targetState, callback).EventTransition(GameHashes.ScheduleBlocksTick, targetState, callback);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnSuccess()
		{
			this.Enter("ReturnSuccess()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Success);
				smi.StopSM("GameStateMachine.ReturnSuccess()");
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ReturnFailure()
		{
			this.Enter("ReturnFailure()", delegate(StateMachineInstanceType smi)
			{
				smi.SetStatus(StateMachine.Status.Failed);
				smi.StopSM("GameStateMachine.ReturnFailure()");
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State ToggleStatusItem(string name, string tooltip, string icon = "", StatusItem.IconType icon_type = StatusItem.IconType.Info, NotificationType notification_type = NotificationType.Neutral, bool allow_multiples = false, HashedString render_overlay = default(HashedString), int status_overlays = 129022, Func<string, StateMachineInstanceType, string> resolve_string_callback = null, Func<string, StateMachineInstanceType, string> resolve_tooltip_callback = null, StatusItemCategory category = null)
		{
			StatusItem statusItem = new StatusItem(this.longName, name, tooltip, icon, icon_type, notification_type, allow_multiples, render_overlay, status_overlays, true, null);
			if (resolve_string_callback != null)
			{
				statusItem.resolveStringCallback = ((string str, object obj) => resolve_string_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			if (resolve_tooltip_callback != null)
			{
				statusItem.resolveTooltipCallback = ((string str, object obj) => resolve_tooltip_callback(str, (StateMachineInstanceType)((object)obj)));
			}
			this.ToggleStatusItem(statusItem, (StateMachineInstanceType smi) => smi, category);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, string> anim_cb, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(Func<StateMachineInstanceType, string> anim_cb, Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnim(Dynamic)", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim_cb(smi), mode_cb(smi), 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim, mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnim(string anim, KAnim.PlayMode mode, Func<StateMachineInstanceType, string> suffix_callback)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter(string.Concat(new string[]
			{
				"PlayAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Play(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(Func<StateMachineInstanceType, string> anim_cb, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter("QueueAnim(" + mode.ToString() + ")", delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim_cb(smi) + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State QueueAnim(string anim, bool loop = false, Func<StateMachineInstanceType, string> suffix_callback = null)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			KAnim.PlayMode mode = KAnim.PlayMode.Once;
			if (loop)
			{
				mode = KAnim.PlayMode.Loop;
			}
			this.Enter(string.Concat(new string[]
			{
				"QueueAnim(",
				anim,
				", ",
				mode.ToString(),
				")"
			}), delegate(StateMachineInstanceType smi)
			{
				string str = "";
				if (suffix_callback != null)
				{
					str = suffix_callback(smi);
				}
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					kanimControllerBase.Queue(anim + str, mode, 1f, 0f);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, KAnim.PlayMode mode = KAnim.PlayMode.Once)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State PlayAnims(Func<StateMachineInstanceType, HashedString[]> anims_callback, Func<StateMachineInstanceType, KAnim.PlayMode> mode_cb)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("PlayAnims", delegate(StateMachineInstanceType smi)
			{
				KAnimControllerBase kanimControllerBase = state_target.Get<KAnimControllerBase>(smi);
				if (kanimControllerBase != null)
				{
					HashedString[] anim_names = anims_callback(smi);
					KAnim.PlayMode mode = mode_cb(smi);
					kanimControllerBase.Play(anim_names, mode);
				}
			});
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State OnAnimQueueComplete(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter state_target = this.GetStateTarget();
			this.Enter("CheckIfAnimQueueIsEmpty", delegate(StateMachineInstanceType smi)
			{
				if (state_target.Get<KBatchedAnimController>(smi).IsStopped())
				{
					smi.GoTo(state);
				}
			});
			return this.EventTransition(GameHashes.AnimQueueComplete, state, null);
		}

		internal void EventHandler()
		{
			throw new NotImplementedException();
		}

		[StateMachine.DoNotAutoCreate]
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

		private class TransitionUpdater : UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater
		{
			public TransitionUpdater(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
			{
				this.condition = condition;
				this.state = state;
			}

			public void Update(StateMachineInstanceType smi, float dt)
			{
				if (this.condition(smi))
				{
					smi.GoTo(this.state);
				}
			}

			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

			private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state;
		}
	}

	public class GameEvent : StateEvent
	{
		public GameEvent(GameHashes id, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, Func<StateMachineInstanceType, KMonoBehaviour> global_event_system_callback) : base(id.ToString())
		{
			this.id = id;
			this.target = target;
			this.callback = callback;
			this.globalEventSystemCallback = global_event_system_callback;
		}

		public override StateEvent.Context Subscribe(StateMachine.Instance smi)
		{
			StateEvent.Context result = base.Subscribe(smi);
			StateMachineInstanceType cast_smi = (StateMachineInstanceType)((object)smi);
			Action<object> handler = delegate(object d)
			{
				if (StateMachine.Instance.error)
				{
					return;
				}
				this.callback(cast_smi, d);
			};
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(cast_smi);
				result.data = kmonoBehaviour.Subscribe((int)this.id, handler);
			}
			else
			{
				result.data = this.target.Get(cast_smi).Subscribe((int)this.id, handler);
			}
			return result;
		}

		public override void Unsubscribe(StateMachine.Instance smi, StateEvent.Context context)
		{
			StateMachineInstanceType stateMachineInstanceType = (StateMachineInstanceType)((object)smi);
			if (this.globalEventSystemCallback != null)
			{
				KMonoBehaviour kmonoBehaviour = this.globalEventSystemCallback(stateMachineInstanceType);
				if (kmonoBehaviour != null)
				{
					kmonoBehaviour.Unsubscribe(context.data);
					return;
				}
			}
			else
			{
				GameObject gameObject = this.target.Get(stateMachineInstanceType);
				if (gameObject != null)
				{
					gameObject.Unsubscribe(context.data);
				}
			}
		}

		private GameHashes id;

		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target;

		private GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GameEvent.Callback callback;

		private Func<StateMachineInstanceType, KMonoBehaviour> globalEventSystemCallback;

		public delegate void Callback(StateMachineInstanceType smi, object callback_data);
	}

	public class ApproachSubState<ApproachableType> : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State where ApproachableType : IApproachable
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, CellOffset[] override_offsets = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, failure_state, override_offsets, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, Func<StateMachineInstanceType, CellOffset[]> override_offsets, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, NavTactic tactic = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, override_offsets, failure_state, (tactic == null) ? NavigationTactics.ReduceTravelDistance : tactic);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(Func<StateMachineInstanceType, NavTactic> navTactic, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter mover, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter move_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null, CellOffset[] override_offsets = null)
		{
			base.root.Target(mover).OnTargetLost(move_target, failure_state).MoveTo<ApproachableType>(move_target, success_state, navTactic, failure_state, override_offsets);
			return this;
		}
	}

	public class DebugGoToSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State exit_state)
		{
			base.root.Enter("GoToCursor", delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventHandler(GameHashes.DebugGoTo, (StateMachineInstanceType smi) => Game.Instance, delegate(StateMachineInstanceType smi)
			{
				this.GoToCursor(smi);
			}).EventTransition(GameHashes.DestinationReached, exit_state, null).EventTransition(GameHashes.NavigationFailed, exit_state, null);
			return this;
		}

		public void GoToCursor(StateMachineInstanceType smi)
		{
			smi.GetComponent<Navigator>().GoTo(Grid.PosToCell(DebugHandler.GetMousePos()), new CellOffset[1]);
		}
	}

	public class DropSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter carrier, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter item, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter drop_target, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.root.Target(carrier).Enter("Drop", delegate(StateMachineInstanceType smi)
			{
				Storage storage = carrier.Get<Storage>(smi);
				GameObject gameObject = item.Get(smi);
				storage.Drop(gameObject, true);
				int cell = Grid.CellAbove(Grid.PosToCell(drop_target.Get<Transform>(smi).GetPosition()));
				gameObject.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
				smi.GoTo(success_state);
			});
			return this;
		}
	}

	public class FetchSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter fetcher, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_source, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter pickup_chunk, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter requested_amount, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter actual_amount, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success_state, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State failure_state = null)
		{
			base.Target(fetcher);
			base.root.DefaultState(this.approach).ToggleReserve(fetcher, pickup_source, requested_amount, actual_amount);
			this.approach.InitializeStates(fetcher, pickup_source, this.pickup, null, null, NavigationTactics.ReduceTravelDistance).OnTargetLost(pickup_source, failure_state);
			this.pickup.DoPickup(pickup_source, pickup_chunk, actual_amount, success_state, failure_state).EventTransition(GameHashes.AbortWork, failure_state, null);
			return this;
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ApproachSubState<Pickupable> approach;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State pickup;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State success;
	}

	public class HungrySubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target, StatusItem status_item)
		{
			base.Target(target);
			base.root.DefaultState(this.satisfied);
			this.satisfied.EventTransition(GameHashes.AddUrge, this.hungry, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi));
			this.hungry.EventTransition(GameHashes.RemoveUrge, this.satisfied, (StateMachineInstanceType smi) => !GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.HungrySubState.IsHungry(smi)).ToggleStatusItem(status_item, null);
			return this;
		}

		private static bool IsHungry(StateMachineInstanceType smi)
		{
			return smi.GetComponent<ChoreConsumer>().HasUrge(Db.Get().Urges.Eat);
		}

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State satisfied;

		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State hungry;
	}

	public class PlantAliveSubState : GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State
	{
		public GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State InitializeStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter plant, GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State death_state = null)
		{
			base.root.Target(plant).TagTransition(GameTags.Uprooted, death_state, false).EventTransition(GameHashes.TooColdFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.TooHotFatal, death_state, (StateMachineInstanceType smi) => GameStateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.PlantAliveSubState.isLethalTemperature(plant.Get(smi))).EventTransition(GameHashes.Drowned, death_state, null);
			return this;
		}

		public bool ForceUpdateStatus(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			EntombVulnerable component2 = plant.GetComponent<EntombVulnerable>();
			PressureVulnerable component3 = plant.GetComponent<PressureVulnerable>();
			return (component == null || !component.IsLethal) && (component2 == null || !component2.GetEntombed) && (component3 == null || !component3.IsLethal);
		}

		private static bool isLethalTemperature(GameObject plant)
		{
			TemperatureVulnerable component = plant.GetComponent<TemperatureVulnerable>();
			return !(component == null) && (component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalCold || component.GetInternalTemperatureState == TemperatureVulnerable.TemperatureState.LethalHot);
		}
	}
}
