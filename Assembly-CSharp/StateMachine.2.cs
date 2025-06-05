using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ImGuiNET;
using KSerialization;
using UnityEngine;

// Token: 0x020008FA RID: 2298
public class StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine where StateMachineInstanceType : StateMachine.Instance where MasterType : IStateMachineTarget
{
	// Token: 0x0600283D RID: 10301 RVA: 0x001DFB58 File Offset: 0x001DDD58
	public override string[] GetStateNames()
	{
		List<string> list = new List<string>();
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			list.Add(state.name);
		}
		return list.ToArray();
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x000BE9F5 File Offset: 0x000BCBF5
	public void Target(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter target)
	{
		this.stateTarget = target;
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x001DFBBC File Offset: 0x001DDDBC
	public void BindState(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, string state_name)
	{
		if (parent_state != null)
		{
			state_name = parent_state.name + "." + state_name;
		}
		state.name = state_name;
		state.longName = this.name + "." + state_name;
		state.debugPushName = "PuS: " + state.longName;
		state.debugPopName = "PoS: " + state.longName;
		state.debugExecuteName = "EA: " + state.longName;
		List<StateMachine.BaseState> list;
		if (parent_state != null)
		{
			list = new List<StateMachine.BaseState>(parent_state.branch);
		}
		else
		{
			list = new List<StateMachine.BaseState>();
		}
		list.Add(state);
		state.parent = parent_state;
		state.branch = list.ToArray();
		this.maxDepth = Math.Max(state.branch.Length, this.maxDepth);
		this.states.Add(state);
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x001DFC98 File Offset: 0x001DDE98
	public void BindStates(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State parent_state, object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)fieldInfo.GetValue(state_machine);
				if (state != parent_state)
				{
					string name = fieldInfo.Name;
					this.BindState(parent_state, state, name);
					this.BindStates(state, state);
				}
			}
		}
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x000BE9FE File Offset: 0x000BCBFE
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.InitializeStates(out default_state);
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x000BEA07 File Offset: 0x000BCC07
	public override void BindStates()
	{
		this.BindStates(null, this);
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x000BEA11 File Offset: 0x000BCC11
	public override Type GetStateMachineInstanceType()
	{
		return typeof(StateMachineInstanceType);
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x001DFD08 File Offset: 0x001DDF08
	public override StateMachine.BaseState GetState(string state_name)
	{
		foreach (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state in this.states)
		{
			if (state.name == state_name)
			{
				return state;
			}
		}
		return null;
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x001DFD6C File Offset: 0x001DDF6C
	public override void FreeResources()
	{
		for (int i = 0; i < this.states.Count; i++)
		{
			this.states[i].FreeResources();
		}
		this.states.Clear();
		base.FreeResources();
	}

	// Token: 0x04001BC7 RID: 7111
	private List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State> states = new List<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State>();

	// Token: 0x04001BC8 RID: 7112
	public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter masterTarget;

	// Token: 0x04001BC9 RID: 7113
	[StateMachine.DoNotAutoCreate]
	protected StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter stateTarget;

	// Token: 0x020008FB RID: 2299
	public class GenericInstance : StateMachine.Instance
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x000BEA30 File Offset: 0x000BCC30
		// (set) Token: 0x06002848 RID: 10312 RVA: 0x000BEA38 File Offset: 0x000BCC38
		public StateMachineType sm { get; private set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000BEA41 File Offset: 0x000BCC41
		protected StateMachineInstanceType smi
		{
			get
			{
				return (StateMachineInstanceType)((object)this);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600284A RID: 10314 RVA: 0x000BEA49 File Offset: 0x000BCC49
		// (set) Token: 0x0600284B RID: 10315 RVA: 0x000BEA51 File Offset: 0x000BCC51
		public MasterType master { get; private set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x000BEA5A File Offset: 0x000BCC5A
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x000BEA62 File Offset: 0x000BCC62
		public DefType def { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600284E RID: 10318 RVA: 0x000BEA6B File Offset: 0x000BCC6B
		public bool isMasterNull
		{
			get
			{
				return this.internalSm.masterTarget.IsNull((StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600284F RID: 10319 RVA: 0x000BEA83 File Offset: 0x000BCC83
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType> internalSm
		{
			get
			{
				return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>)((object)this.sm);
			}
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x000AA038 File Offset: 0x000A8238
		protected virtual void OnCleanUp()
		{
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06002851 RID: 10321 RVA: 0x000BEA95 File Offset: 0x000BCC95
		public override float timeinstate
		{
			get
			{
				return Time.time - this.stateEnterTime;
			}
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x001DFDB4 File Offset: 0x001DDFB4
		public override void FreeResources()
		{
			this.updateHandle.FreeResources();
			this.updateHandle = default(SchedulerHandle);
			this.controller = null;
			if (this.gotoStack != null)
			{
				this.gotoStack.Clear();
			}
			this.gotoStack = null;
			if (this.transitionStack != null)
			{
				this.transitionStack.Clear();
			}
			this.transitionStack = null;
			if (this.currentSchedulerGroup != null)
			{
				this.currentSchedulerGroup.FreeResources();
			}
			this.currentSchedulerGroup = null;
			if (this.stateStack != null)
			{
				for (int i = 0; i < this.stateStack.Length; i++)
				{
					if (this.stateStack[i].schedulerGroup != null)
					{
						this.stateStack[i].schedulerGroup.FreeResources();
					}
				}
			}
			this.stateStack = null;
			base.FreeResources();
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x001DFE80 File Offset: 0x001DE080
		public GenericInstance(MasterType master) : base((StateMachine)((object)Singleton<StateMachineManager>.Instance.CreateStateMachine<StateMachineType>()), master)
		{
			this.master = master;
			this.stateStack = new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[this.stateMachine.GetMaxDepth()];
			for (int i = 0; i < this.stateStack.Length; i++)
			{
				this.stateStack[i].schedulerGroup = Singleton<StateMachineManager>.Instance.CreateSchedulerGroup();
			}
			this.sm = (StateMachineType)((object)this.stateMachine);
			this.dataTable = new object[base.GetStateMachine().dataTableSize];
			this.updateTable = new StateMachine.Instance.UpdateTableEntry[base.GetStateMachine().updateTableSize];
			this.controller = master.GetComponent<StateMachineController>();
			if (this.controller == null)
			{
				this.controller = master.gameObject.AddComponent<StateMachineController>();
			}
			this.internalSm.masterTarget.Set(master.gameObject, this.smi, false);
			this.controller.AddStateMachineInstance(this);
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000BEAA3 File Offset: 0x000BCCA3
		public override IStateMachineTarget GetMaster()
		{
			return this.master;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x001DFFBC File Offset: 0x001DE1BC
		private void PushEvent(StateEvent evt)
		{
			StateEvent.Context item = evt.Subscribe(this);
			this.subscribedEvents.Push(item);
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x001DFFE0 File Offset: 0x001DE1E0
		private void PopEvent()
		{
			StateEvent.Context context = this.subscribedEvents.Pop();
			context.stateEvent.Unsubscribe(this, context);
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x001E0008 File Offset: 0x001DE208
		private bool TryEvaluateTransitions(StateMachine.BaseState state, int goto_id)
		{
			if (state.transitions == null)
			{
				return true;
			}
			bool result = true;
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition baseTransition = state.transitions[i];
				if (goto_id != this.gotoId)
				{
					result = false;
					break;
				}
				baseTransition.Evaluate(this.smi);
			}
			return result;
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x001E0064 File Offset: 0x001DE264
		private void PushTransitions(StateMachine.BaseState state)
		{
			if (state.transitions == null)
			{
				return;
			}
			for (int i = 0; i < state.transitions.Count; i++)
			{
				StateMachine.BaseTransition transition = state.transitions[i];
				this.PushTransition(transition);
			}
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x001E00A4 File Offset: 0x001DE2A4
		private void PushTransition(StateMachine.BaseTransition transition)
		{
			StateMachine.BaseTransition.Context item = transition.Register(this.smi);
			this.transitionStack.Push(item);
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x001E00D0 File Offset: 0x001DE2D0
		private void PopTransition(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state)
		{
			StateMachine.BaseTransition.Context context = this.transitionStack.Pop();
			state.transitions[context.idx].Unregister(this.smi, context);
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x001E010C File Offset: 0x001DE30C
		private void PushState(StateMachine.BaseState state)
		{
			int num = this.gotoId;
			this.currentActionIdx = -1;
			if (state.events != null)
			{
				foreach (StateEvent evt in state.events)
				{
					this.PushEvent(evt);
				}
			}
			this.PushTransitions(state);
			if (state.updateActions != null)
			{
				for (int i = 0; i < state.updateActions.Count; i++)
				{
					StateMachine.UpdateAction updateAction = state.updateActions[i];
					int updateTableIdx = updateAction.updateTableIdx;
					int nextBucketIdx = updateAction.nextBucketIdx;
					updateAction.nextBucketIdx = (updateAction.nextBucketIdx + 1) % updateAction.buckets.Length;
					UpdateBucketWithUpdater<StateMachineInstanceType> updateBucketWithUpdater = (UpdateBucketWithUpdater<StateMachineInstanceType>)updateAction.buckets[nextBucketIdx];
					this.smi.updateTable[updateTableIdx].bucket = updateBucketWithUpdater;
					this.smi.updateTable[updateTableIdx].handle = updateBucketWithUpdater.Add(this.smi, Singleton<StateMachineUpdater>.Instance.GetFrameTime(updateAction.updateRate, updateBucketWithUpdater.frame), (UpdateBucketWithUpdater<StateMachineInstanceType>.IUpdater)updateAction.updater);
					state.updateActions[i] = updateAction;
				}
			}
			this.stateEnterTime = Time.time;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int stackSize = this.stackSize;
			this.stackSize = stackSize + 1;
			array[stackSize].state = state;
			this.currentSchedulerGroup = this.stateStack[this.stackSize - 1].schedulerGroup;
			if (!this.TryEvaluateTransitions(state, num))
			{
				return;
			}
			if (num != this.gotoId)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
			int num2 = this.gotoId;
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x001E02E8 File Offset: 0x001DE4E8
		private void ExecuteActions(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, List<StateMachine.Action> actions)
		{
			if (actions == null)
			{
				return;
			}
			int num = this.gotoId;
			this.currentActionIdx++;
			while (this.currentActionIdx < actions.Count && num == this.gotoId)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback callback = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State.Callback)actions[this.currentActionIdx].callback;
				try
				{
					callback(this.smi);
				}
				catch (Exception e)
				{
					if (!StateMachine.Instance.error)
					{
						base.Error();
						string text = "(NULL).";
						IStateMachineTarget master = this.GetMaster();
						if (!master.isNull)
						{
							KPrefabID component = master.GetComponent<KPrefabID>();
							if (component != null)
							{
								text = "(" + component.PrefabTag.ToString() + ").";
							}
							else
							{
								text = "(" + base.gameObject.name + ").";
							}
						}
						string text2 = string.Concat(new string[]
						{
							"Exception in: ",
							text,
							this.stateMachine.ToString(),
							".",
							state.name,
							"."
						});
						if (this.currentActionIdx > 0 && this.currentActionIdx < actions.Count)
						{
							text2 += actions[this.currentActionIdx].name;
						}
						DebugUtil.LogException(this.controller, text2, e);
					}
				}
				this.currentActionIdx++;
			}
			this.currentActionIdx = 2147483646;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x001E047C File Offset: 0x001DE67C
		private void PopState()
		{
			this.currentActionIdx = -1;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] array = this.stateStack;
			int num = this.stackSize - 1;
			this.stackSize = num;
			StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry stackEntry = array[num];
			StateMachine.BaseState state = stackEntry.state;
			int num2 = 0;
			while (state.transitions != null && num2 < state.transitions.Count)
			{
				this.PopTransition((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state);
				num2++;
			}
			if (state.events != null)
			{
				for (int i = 0; i < state.events.Count; i++)
				{
					this.PopEvent();
				}
			}
			if (state.updateActions != null)
			{
				foreach (StateMachine.UpdateAction updateAction in state.updateActions)
				{
					int updateTableIdx = updateAction.updateTableIdx;
					StateMachineUpdater.BaseUpdateBucket baseUpdateBucket = (UpdateBucketWithUpdater<StateMachineInstanceType>)this.smi.updateTable[updateTableIdx].bucket;
					this.smi.updateTable[updateTableIdx].bucket = null;
					baseUpdateBucket.Remove(this.smi.updateTable[updateTableIdx].handle);
				}
			}
			stackEntry.schedulerGroup.Reset();
			this.currentSchedulerGroup = stackEntry.schedulerGroup;
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.exitActions);
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x001E05E0 File Offset: 0x001DE7E0
		public override SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.Schedule(name, time, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x001E0604 File Offset: 0x001DE804
		public override SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null)
		{
			string name = null;
			return Singleton<StateMachineManager>.Instance.ScheduleNextFrame(name, callback, callback_data, this.currentSchedulerGroup);
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x000BEAB0 File Offset: 0x000BCCB0
		public override void StartSM()
		{
			if (this.controller != null && !this.controller.HasStateMachineInstance(this))
			{
				this.controller.AddStateMachineInstance(this);
			}
			base.StartSM();
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x001E0628 File Offset: 0x001DE828
		public override void StopSM(string reason)
		{
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (!base.IsRunning())
			{
				return;
			}
			this.gotoId++;
			while (this.stackSize > 0)
			{
				this.PopState();
			}
			if (this.master != null && this.controller != null)
			{
				this.controller.RemoveStateMachineInstance(this);
			}
			if (this.status == StateMachine.Status.Running)
			{
				base.SetStatus(StateMachine.Status.Failed);
			}
			if (this.OnStop != null)
			{
				this.OnStop(reason, this.status);
			}
			for (int i = 0; i < this.parameterContexts.Length; i++)
			{
				this.parameterContexts[i].Cleanup();
			}
			this.OnCleanUp();
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x000BEAE0 File Offset: 0x000BCCE0
		private void FinishStateInProgress(StateMachine.BaseState state)
		{
			if (state.enterActions == null)
			{
				return;
			}
			this.ExecuteActions((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State)state, state.enterActions);
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x001E06F8 File Offset: 0x001DE8F8
		public override void GoTo(StateMachine.BaseState base_state)
		{
			if (App.IsExiting)
			{
				return;
			}
			if (StateMachine.Instance.error)
			{
				return;
			}
			if (this.isMasterNull)
			{
				return;
			}
			if (this.smi.IsNullOrDestroyed())
			{
				return;
			}
			try
			{
				if (base.IsBreakOnGoToEnabled())
				{
					Debugger.Break();
				}
				if (base_state != null)
				{
					while (base_state.defaultState != null)
					{
						base_state = base_state.defaultState;
					}
				}
				if (this.GetCurrentState() == null)
				{
					base.SetStatus(StateMachine.Status.Running);
				}
				if (this.gotoStack.Count > 100)
				{
					string text = "Potential infinite transition loop detected in state machine: " + this.ToString() + "\nGoto stack:\n";
					foreach (StateMachine.BaseState baseState in this.gotoStack)
					{
						text = text + "\n" + baseState.name;
					}
					global::Debug.LogError(text);
					base.Error();
				}
				else
				{
					this.gotoStack.Push(base_state);
					if (base_state == null)
					{
						this.StopSM("StateMachine.GoTo(null)");
						this.gotoStack.Pop();
					}
					else
					{
						int num = this.gotoId + 1;
						this.gotoId = num;
						int num2 = num;
						StateMachine.BaseState[] branch = (base_state as StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State).branch;
						int num3 = 0;
						while (num3 < this.stackSize && num3 < branch.Length && this.stateStack[num3].state == branch[num3])
						{
							num3++;
						}
						int num4 = this.stackSize - 1;
						if (num4 >= 0 && num4 == num3 - 1)
						{
							this.FinishStateInProgress(this.stateStack[num4].state);
						}
						while (this.stackSize > num3 && num2 == this.gotoId)
						{
							this.PopState();
						}
						int num5 = num3;
						while (num5 < branch.Length && num2 == this.gotoId)
						{
							this.PushState(branch[num5]);
							num5++;
						}
						this.gotoStack.Pop();
					}
				}
			}
			catch (Exception ex)
			{
				if (!StateMachine.Instance.error)
				{
					base.Error();
					string text2 = "(Stop)";
					if (base_state != null)
					{
						text2 = base_state.name;
					}
					string text3 = "(NULL).";
					if (!this.GetMaster().isNull)
					{
						text3 = "(" + base.gameObject.name + ").";
					}
					string str = string.Concat(new string[]
					{
						"Exception in: ",
						text3,
						this.stateMachine.ToString(),
						".GoTo(",
						text2,
						")"
					});
					DebugUtil.LogErrorArgs(this.controller, new object[]
					{
						str + "\n" + ex.ToString()
					});
				}
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x000BEAFD File Offset: 0x000BCCFD
		public override StateMachine.BaseState GetCurrentState()
		{
			if (this.stackSize > 0)
			{
				return this.stateStack[this.stackSize - 1].state;
			}
			return null;
		}

		// Token: 0x04001BCA RID: 7114
		private float stateEnterTime;

		// Token: 0x04001BCB RID: 7115
		private int gotoId;

		// Token: 0x04001BCC RID: 7116
		private int currentActionIdx = -1;

		// Token: 0x04001BCD RID: 7117
		private SchedulerHandle updateHandle;

		// Token: 0x04001BCE RID: 7118
		private Stack<StateMachine.BaseState> gotoStack = new Stack<StateMachine.BaseState>();

		// Token: 0x04001BCF RID: 7119
		protected Stack<StateMachine.BaseTransition.Context> transitionStack = new Stack<StateMachine.BaseTransition.Context>();

		// Token: 0x04001BD3 RID: 7123
		protected StateMachineController controller;

		// Token: 0x04001BD4 RID: 7124
		private SchedulerGroup currentSchedulerGroup;

		// Token: 0x04001BD5 RID: 7125
		private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.GenericInstance.StackEntry[] stateStack;

		// Token: 0x020008FC RID: 2300
		public struct StackEntry
		{
			// Token: 0x04001BD6 RID: 7126
			public StateMachine.BaseState state;

			// Token: 0x04001BD7 RID: 7127
			public SchedulerGroup schedulerGroup;
		}
	}

	// Token: 0x020008FD RID: 2301
	public class State : StateMachine.BaseState
	{
		// Token: 0x04001BD8 RID: 7128
		protected StateMachineType sm;

		// Token: 0x020008FE RID: 2302
		// (Invoke) Token: 0x06002867 RID: 10343
		public delegate void Callback(StateMachineInstanceType smi);
	}

	// Token: 0x020008FF RID: 2303
	public new abstract class ParameterTransition : StateMachine.ParameterTransition
	{
		// Token: 0x0600286A RID: 10346 RVA: 0x000BEB2A File Offset: 0x000BCD2A
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x02000900 RID: 2304
	public class Transition : StateMachine.BaseTransition
	{
		// Token: 0x0600286B RID: 10347 RVA: 0x000BEB37 File Offset: 0x000BCD37
		public Transition(string name, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State source_state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State target_state, int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition) : base(idx, name, source_state, target_state)
		{
			this.condition = condition;
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000BEB4C File Offset: 0x000BCD4C
		public override string ToString()
		{
			if (this.targetState != null)
			{
				return this.name + "->" + this.targetState.name;
			}
			return this.name + "->(Stop)";
		}

		// Token: 0x04001BD9 RID: 7129
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Transition.ConditionCallback condition;

		// Token: 0x02000901 RID: 2305
		// (Invoke) Token: 0x0600286E RID: 10350
		public delegate bool ConditionCallback(StateMachineInstanceType smi);
	}

	// Token: 0x02000902 RID: 2306
	public abstract class Parameter<ParameterType> : StateMachine.Parameter
	{
		// Token: 0x06002871 RID: 10353 RVA: 0x000BEB82 File Offset: 0x000BCD82
		public Parameter()
		{
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000BEB8A File Offset: 0x000BCD8A
		public Parameter(ParameterType default_value)
		{
			this.defaultValue = default_value;
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000BEB99 File Offset: 0x000BCD99
		public ParameterType Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).Set(value, smi, silenceEvents);
			return value;
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000BEBB5 File Offset: 0x000BCDB5
		public ParameterType Get(StateMachineInstanceType smi)
		{
			return ((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this)).value;
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000BEBCD File Offset: 0x000BCDCD
		public StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context GetContext(StateMachineInstanceType smi)
		{
			return (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this);
		}

		// Token: 0x04001BDA RID: 7130
		public ParameterType defaultValue;

		// Token: 0x04001BDB RID: 7131
		public bool isSignal;

		// Token: 0x02000903 RID: 2307
		// (Invoke) Token: 0x06002877 RID: 10359
		public delegate bool Callback(StateMachineInstanceType smi, ParameterType p);

		// Token: 0x02000904 RID: 2308
		public class Transition : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ParameterTransition
		{
			// Token: 0x0600287A RID: 10362 RVA: 0x000BEBE0 File Offset: 0x000BCDE0
			public Transition(int idx, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.State state, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback) : base(idx, parameter.name, null, state)
			{
				this.parameter = parameter;
				this.callback = callback;
			}

			// Token: 0x0600287B RID: 10363 RVA: 0x001E09C4 File Offset: 0x001DEBC4
			public override void Evaluate(StateMachine.Instance smi)
			{
				StateMachineInstanceType stateMachineInstanceType = smi as StateMachineInstanceType;
				global::Debug.Assert(stateMachineInstanceType != null);
				if (this.parameter.isSignal && this.callback == null)
				{
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)stateMachineInstanceType.GetParameterContext(this.parameter);
				if (this.callback(stateMachineInstanceType, context.value))
				{
					stateMachineInstanceType.GoTo(this.targetState);
				}
			}

			// Token: 0x0600287C RID: 10364 RVA: 0x000BD4D9 File Offset: 0x000BB6D9
			private void Trigger(StateMachineInstanceType smi)
			{
				smi.GoTo(this.targetState);
			}

			// Token: 0x0600287D RID: 10365 RVA: 0x001E0A40 File Offset: 0x001DEC40
			public override StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
				}
				else
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
					context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Combine(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
				}
				return new StateMachine.BaseTransition.Context(this);
			}

			// Token: 0x0600287E RID: 10366 RVA: 0x001E0AC4 File Offset: 0x001DECC4
			public override void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context transitionContext)
			{
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context = (StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context)smi.GetParameterContext(this.parameter);
				if (this.parameter.isSignal && this.callback == null)
				{
					StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context2 = context;
					context2.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context2.onDirty, new Action<StateMachineInstanceType>(this.Trigger));
					return;
				}
				StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Context context3 = context;
				context3.onDirty = (Action<StateMachineInstanceType>)Delegate.Remove(context3.onDirty, new Action<StateMachineInstanceType>(this.Evaluate));
			}

			// Token: 0x0600287F RID: 10367 RVA: 0x000BEC00 File Offset: 0x000BCE00
			public override string ToString()
			{
				if (this.targetState != null)
				{
					return this.parameter.name + "->" + this.targetState.name;
				}
				return this.parameter.name + "->(Stop)";
			}

			// Token: 0x04001BDC RID: 7132
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType> parameter;

			// Token: 0x04001BDD RID: 7133
			private StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ParameterType>.Callback callback;
		}

		// Token: 0x02000905 RID: 2309
		public new abstract class Context : StateMachine.Parameter.Context
		{
			// Token: 0x06002880 RID: 10368 RVA: 0x000BEC40 File Offset: 0x000BCE40
			public Context(StateMachine.Parameter parameter, ParameterType default_value) : base(parameter)
			{
				this.value = default_value;
			}

			// Token: 0x06002881 RID: 10369 RVA: 0x000BEC50 File Offset: 0x000BCE50
			public virtual void Set(ParameterType value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!EqualityComparer<ParameterType>.Default.Equals(value, this.value))
				{
					this.value = value;
					if (!silenceEvents && this.onDirty != null)
					{
						this.onDirty(smi);
					}
				}
			}

			// Token: 0x04001BDE RID: 7134
			public ParameterType value;

			// Token: 0x04001BDF RID: 7135
			public Action<StateMachineInstanceType> onDirty;
		}
	}

	// Token: 0x02000906 RID: 2310
	public class BoolParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x000BEC83 File Offset: 0x000BCE83
		public BoolParameter()
		{
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000BEC8B File Offset: 0x000BCE8B
		public BoolParameter(bool default_value) : base(default_value)
		{
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x000BEC94 File Offset: 0x000BCE94
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.BoolParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02000907 RID: 2311
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<bool>.Context
		{
			// Token: 0x06002885 RID: 10373 RVA: 0x000BECA2 File Offset: 0x000BCEA2
			public Context(StateMachine.Parameter parameter, bool default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002886 RID: 10374 RVA: 0x000BECAC File Offset: 0x000BCEAC
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value ? 1 : 0);
			}

			// Token: 0x06002887 RID: 10375 RVA: 0x000BECC1 File Offset: 0x000BCEC1
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (reader.ReadByte() > 0);
			}

			// Token: 0x06002888 RID: 10376 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002889 RID: 10377 RVA: 0x001E0B40 File Offset: 0x001DED40
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				bool value = this.value;
				if (ImGui.Checkbox(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x02000908 RID: 2312
	public class Vector3Parameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>
	{
		// Token: 0x0600288A RID: 10378 RVA: 0x000BECD2 File Offset: 0x000BCED2
		public Vector3Parameter()
		{
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000BECDA File Offset: 0x000BCEDA
		public Vector3Parameter(Vector3 default_value) : base(default_value)
		{
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000BECE3 File Offset: 0x000BCEE3
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Vector3Parameter.Context(this, this.defaultValue);
		}

		// Token: 0x02000909 RID: 2313
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Vector3>.Context
		{
			// Token: 0x0600288D RID: 10381 RVA: 0x000BECF1 File Offset: 0x000BCEF1
			public Context(StateMachine.Parameter parameter, Vector3 default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600288E RID: 10382 RVA: 0x000BECFB File Offset: 0x000BCEFB
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.x);
				writer.Write(this.value.y);
				writer.Write(this.value.z);
			}

			// Token: 0x0600288F RID: 10383 RVA: 0x000BED30 File Offset: 0x000BCF30
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value.x = reader.ReadSingle();
				this.value.y = reader.ReadSingle();
				this.value.z = reader.ReadSingle();
			}

			// Token: 0x06002890 RID: 10384 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002891 RID: 10385 RVA: 0x001E0B78 File Offset: 0x001DED78
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				Vector3 value = this.value;
				if (ImGui.InputFloat3(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x0200090A RID: 2314
	public class EnumParameter<EnumType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>
	{
		// Token: 0x06002892 RID: 10386 RVA: 0x000BED65 File Offset: 0x000BCF65
		public EnumParameter(EnumType default_value) : base(default_value)
		{
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000BED6E File Offset: 0x000BCF6E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.EnumParameter<EnumType>.Context(this, this.defaultValue);
		}

		// Token: 0x0200090B RID: 2315
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<EnumType>.Context
		{
			// Token: 0x06002894 RID: 10388 RVA: 0x000BED7C File Offset: 0x000BCF7C
			public Context(StateMachine.Parameter parameter, EnumType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x06002895 RID: 10389 RVA: 0x000BED86 File Offset: 0x000BCF86
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write((int)((object)this.value));
			}

			// Token: 0x06002896 RID: 10390 RVA: 0x000BED9E File Offset: 0x000BCF9E
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = (EnumType)((object)reader.ReadInt32());
			}

			// Token: 0x06002897 RID: 10391 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x06002898 RID: 10392 RVA: 0x001E0BB0 File Offset: 0x001DEDB0
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string[] names = Enum.GetNames(typeof(EnumType));
				Array values = Enum.GetValues(typeof(EnumType));
				int index = Array.IndexOf(values, this.value);
				if (ImGui.Combo(this.parameter.name, ref index, names, names.Length))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set((EnumType)((object)values.GetValue(index)), smi, false);
				}
			}
		}
	}

	// Token: 0x0200090C RID: 2316
	public class FloatParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>
	{
		// Token: 0x06002899 RID: 10393 RVA: 0x000BEDB6 File Offset: 0x000BCFB6
		public FloatParameter()
		{
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000BEDBE File Offset: 0x000BCFBE
		public FloatParameter(float default_value) : base(default_value)
		{
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x001E0C24 File Offset: 0x001DEE24
		public float Delta(float delta_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x001E0C48 File Offset: 0x001DEE48
		public float DeltaClamp(float delta_value, float min_value, float max_value, StateMachineInstanceType smi)
		{
			float num = base.Get(smi);
			num += delta_value;
			num = Mathf.Clamp(num, min_value, max_value);
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000BEDC7 File Offset: 0x000BCFC7
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.FloatParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200090D RID: 2317
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<float>.Context
		{
			// Token: 0x0600289E RID: 10398 RVA: 0x000BEDD5 File Offset: 0x000BCFD5
			public Context(StateMachine.Parameter parameter, float default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x0600289F RID: 10399 RVA: 0x000BEDDF File Offset: 0x000BCFDF
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x060028A0 RID: 10400 RVA: 0x000BEDED File Offset: 0x000BCFED
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadSingle();
			}

			// Token: 0x060028A1 RID: 10401 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028A2 RID: 10402 RVA: 0x001E0C78 File Offset: 0x001DEE78
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				float value = this.value;
				if (ImGui.InputFloat(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x0200090E RID: 2318
	public class IntParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>
	{
		// Token: 0x060028A3 RID: 10403 RVA: 0x000BEDFB File Offset: 0x000BCFFB
		public IntParameter()
		{
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000BEE03 File Offset: 0x000BD003
		public IntParameter(int default_value) : base(default_value)
		{
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x001E0CB0 File Offset: 0x001DEEB0
		public int Delta(int delta_value, StateMachineInstanceType smi)
		{
			int num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000BEE0C File Offset: 0x000BD00C
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.IntParameter.Context(this, this.defaultValue);
		}

		// Token: 0x0200090F RID: 2319
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<int>.Context
		{
			// Token: 0x060028A7 RID: 10407 RVA: 0x000BEE1A File Offset: 0x000BD01A
			public Context(StateMachine.Parameter parameter, int default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028A8 RID: 10408 RVA: 0x000BEE24 File Offset: 0x000BD024
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x060028A9 RID: 10409 RVA: 0x000BEE32 File Offset: 0x000BD032
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt32();
			}

			// Token: 0x060028AA RID: 10410 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028AB RID: 10411 RVA: 0x001E0CD4 File Offset: 0x001DEED4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				int value = this.value;
				if (ImGui.InputInt(this.parameter.name, ref value))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(value, smi, false);
				}
			}
		}
	}

	// Token: 0x02000910 RID: 2320
	public class LongParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>
	{
		// Token: 0x060028AC RID: 10412 RVA: 0x000BEE40 File Offset: 0x000BD040
		public LongParameter()
		{
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000BEE48 File Offset: 0x000BD048
		public LongParameter(long default_value) : base(default_value)
		{
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x001E0D0C File Offset: 0x001DEF0C
		public long Delta(long delta_value, StateMachineInstanceType smi)
		{
			long num = base.Get(smi);
			num += delta_value;
			base.Set(num, smi, false);
			return num;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000BEE51 File Offset: 0x000BD051
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.LongParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02000911 RID: 2321
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<long>.Context
		{
			// Token: 0x060028B0 RID: 10416 RVA: 0x000BEE5F File Offset: 0x000BD05F
			public Context(StateMachine.Parameter parameter, long default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028B1 RID: 10417 RVA: 0x000BEE69 File Offset: 0x000BD069
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value);
			}

			// Token: 0x060028B2 RID: 10418 RVA: 0x000BEE77 File Offset: 0x000BD077
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = reader.ReadInt64();
			}

			// Token: 0x060028B3 RID: 10419 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028B4 RID: 10420 RVA: 0x000BEE85 File Offset: 0x000BD085
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				long value = this.value;
			}
		}
	}

	// Token: 0x02000912 RID: 2322
	public class ResourceParameter<ResourceType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType> where ResourceType : Resource
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x001E0D30 File Offset: 0x001DEF30
		public ResourceParameter() : base(default(ResourceType))
		{
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000BEE8E File Offset: 0x000BD08E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ResourceParameter<ResourceType>.Context(this, this.defaultValue);
		}

		// Token: 0x02000913 RID: 2323
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ResourceType>.Context
		{
			// Token: 0x060028B7 RID: 10423 RVA: 0x000BED7C File Offset: 0x000BCF7C
			public Context(StateMachine.Parameter parameter, ResourceType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028B8 RID: 10424 RVA: 0x001E0D4C File Offset: 0x001DEF4C
			public override void Serialize(BinaryWriter writer)
			{
				string str = "";
				if (this.value != null)
				{
					if (this.value.Guid == null)
					{
						global::Debug.LogError("Cannot serialize resource with invalid guid: " + this.value.Id);
					}
					else
					{
						str = this.value.Guid.Guid;
					}
				}
				writer.WriteKleiString(str);
			}

			// Token: 0x060028B9 RID: 10425 RVA: 0x001E0DC4 File Offset: 0x001DEFC4
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				string text = reader.ReadKleiString();
				if (text != "")
				{
					ResourceGuid guid = new ResourceGuid(text, null);
					this.value = Db.Get().GetResource<ResourceType>(guid);
				}
			}

			// Token: 0x060028BA RID: 10426 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028BB RID: 10427 RVA: 0x001E0E00 File Offset: 0x001DF000
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x02000914 RID: 2324
	public class TagParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x000BEE9C File Offset: 0x000BD09C
		public TagParameter()
		{
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x000BEEA4 File Offset: 0x000BD0A4
		public TagParameter(Tag default_value) : base(default_value)
		{
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000BEEAD File Offset: 0x000BD0AD
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TagParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02000915 RID: 2325
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<Tag>.Context
		{
			// Token: 0x060028BF RID: 10431 RVA: 0x000BEEBB File Offset: 0x000BD0BB
			public Context(StateMachine.Parameter parameter, Tag default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028C0 RID: 10432 RVA: 0x000BEEC5 File Offset: 0x000BD0C5
			public override void Serialize(BinaryWriter writer)
			{
				writer.Write(this.value.GetHash());
			}

			// Token: 0x060028C1 RID: 10433 RVA: 0x000BEED8 File Offset: 0x000BD0D8
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				this.value = new Tag(reader.ReadInt32());
			}

			// Token: 0x060028C2 RID: 10434 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028C3 RID: 10435 RVA: 0x000BEEEB File Offset: 0x000BD0EB
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				ImGui.LabelText(this.parameter.name, this.value.ToString());
			}
		}
	}

	// Token: 0x02000916 RID: 2326
	public class ObjectParameter<ObjectType> : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType> where ObjectType : class
	{
		// Token: 0x060028C4 RID: 10436 RVA: 0x001E0D30 File Offset: 0x001DEF30
		public ObjectParameter() : base(default(ObjectType))
		{
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000BEF0E File Offset: 0x000BD10E
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.ObjectParameter<ObjectType>.Context(this, this.defaultValue);
		}

		// Token: 0x02000917 RID: 2327
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<ObjectType>.Context
		{
			// Token: 0x060028C6 RID: 10438 RVA: 0x000BED7C File Offset: 0x000BCF7C
			public Context(StateMachine.Parameter parameter, ObjectType default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028C7 RID: 10439 RVA: 0x000BEF1C File Offset: 0x000BD11C
			public override void Serialize(BinaryWriter writer)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x060028C8 RID: 10440 RVA: 0x000BEF1C File Offset: 0x000BD11C
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				DebugUtil.DevLogError("ObjectParameter cannot be serialized");
			}

			// Token: 0x060028C9 RID: 10441 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028CA RID: 10442 RVA: 0x001E0E00 File Offset: 0x001DF000
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				string fmt = "None";
				if (this.value != null)
				{
					fmt = this.value.ToString();
				}
				ImGui.LabelText(this.parameter.name, fmt);
			}
		}
	}

	// Token: 0x02000918 RID: 2328
	public class TargetParameter : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>
	{
		// Token: 0x060028CB RID: 10443 RVA: 0x000BEF28 File Offset: 0x000BD128
		public TargetParameter() : base(null)
		{
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x001E0E44 File Offset: 0x001DF044
		public SMT GetSMI<SMT>(StateMachineInstanceType smi) where SMT : StateMachine.Instance
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				SMT smi2 = gameObject.GetSMI<SMT>();
				if (smi2 != null)
				{
					return smi2;
				}
				global::Debug.LogError(gameObject.name + " does not have state machine " + typeof(StateMachineType).Name);
			}
			return default(SMT);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000BEF31 File Offset: 0x000BD131
		public bool IsNull(StateMachineInstanceType smi)
		{
			return base.Get(smi) == null;
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x001E0EA0 File Offset: 0x001DF0A0
		public ComponentType Get<ComponentType>(StateMachineInstanceType smi)
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType component = gameObject.GetComponent<ComponentType>();
				if (component != null)
				{
					return component;
				}
				global::Debug.LogError(gameObject.name + " does not have component " + typeof(ComponentType).Name);
			}
			return default(ComponentType);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x001E0EFC File Offset: 0x001DF0FC
		public ComponentType AddOrGet<ComponentType>(StateMachineInstanceType smi) where ComponentType : Component
		{
			GameObject gameObject = base.Get(smi);
			if (gameObject != null)
			{
				ComponentType componentType = gameObject.GetComponent<ComponentType>();
				if (componentType == null)
				{
					componentType = gameObject.AddComponent<ComponentType>();
				}
				return componentType;
			}
			return default(ComponentType);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x001E0F44 File Offset: 0x001DF144
		public void Set(KMonoBehaviour value, StateMachineInstanceType smi)
		{
			GameObject value2 = null;
			if (value != null)
			{
				value2 = value.gameObject;
			}
			base.Set(value2, smi, false);
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000BEF40 File Offset: 0x000BD140
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.TargetParameter.Context(this, this.defaultValue);
		}

		// Token: 0x02000919 RID: 2329
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<GameObject>.Context
		{
			// Token: 0x060028D2 RID: 10450 RVA: 0x000BEF4E File Offset: 0x000BD14E
			public Context(StateMachine.Parameter parameter, GameObject default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028D3 RID: 10451 RVA: 0x001E0F70 File Offset: 0x001DF170
			public override void Serialize(BinaryWriter writer)
			{
				if (this.value != null)
				{
					int instanceID = this.value.GetComponent<KPrefabID>().InstanceID;
					writer.Write(instanceID);
					return;
				}
				writer.Write(0);
			}

			// Token: 0x060028D4 RID: 10452 RVA: 0x001E0FAC File Offset: 0x001DF1AC
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
				try
				{
					int num = reader.ReadInt32();
					if (num != 0)
					{
						KPrefabID instance = KPrefabIDTracker.Get().GetInstance(num);
						if (instance != null)
						{
							this.value = instance.gameObject;
							this.objectDestroyedHandler = instance.Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
						}
						this.m_smi = (StateMachineInstanceType)((object)smi);
					}
				}
				catch (Exception ex)
				{
					if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 20))
					{
						global::Debug.LogWarning("Missing statemachine target params. " + ex.Message);
					}
				}
			}

			// Token: 0x060028D5 RID: 10453 RVA: 0x000BEF58 File Offset: 0x000BD158
			public override void Cleanup()
			{
				base.Cleanup();
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
			}

			// Token: 0x060028D6 RID: 10454 RVA: 0x001E1050 File Offset: 0x001DF250
			public override void Set(GameObject value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				this.m_smi = smi;
				if (this.value != null)
				{
					this.value.GetComponent<KMonoBehaviour>().Unsubscribe(this.objectDestroyedHandler);
					this.objectDestroyedHandler = 0;
				}
				if (value != null)
				{
					this.objectDestroyedHandler = value.GetComponent<KMonoBehaviour>().Subscribe(1969584890, new Action<object>(this.OnObjectDestroyed));
				}
				base.Set(value, smi, silenceEvents);
			}

			// Token: 0x060028D7 RID: 10455 RVA: 0x000BEF8B File Offset: 0x000BD18B
			private void OnObjectDestroyed(object data)
			{
				this.Set(null, this.m_smi, false);
			}

			// Token: 0x060028D8 RID: 10456 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028D9 RID: 10457 RVA: 0x001E10C4 File Offset: 0x001DF2C4
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (this.value != null)
				{
					ImGui.LabelText(this.parameter.name, this.value.name);
					return;
				}
				ImGui.LabelText(this.parameter.name, "null");
			}

			// Token: 0x04001BE0 RID: 7136
			private StateMachineInstanceType m_smi;

			// Token: 0x04001BE1 RID: 7137
			private int objectDestroyedHandler;
		}
	}

	// Token: 0x0200091A RID: 2330
	public class SignalParameter
	{
	}

	// Token: 0x0200091B RID: 2331
	public class Signal : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>
	{
		// Token: 0x060028DB RID: 10459 RVA: 0x000BEF9B File Offset: 0x000BD19B
		public Signal() : base(null)
		{
			this.isSignal = true;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000BEFAB File Offset: 0x000BD1AB
		public void Trigger(StateMachineInstanceType smi)
		{
			((StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context)smi.GetParameterContext(this)).Set(null, smi, false);
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000BEFC6 File Offset: 0x000BD1C6
		public override StateMachine.Parameter.Context CreateContext()
		{
			return new StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Signal.Context(this, this.defaultValue);
		}

		// Token: 0x0200091C RID: 2332
		public new class Context : StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.Parameter<StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter>.Context
		{
			// Token: 0x060028DE RID: 10462 RVA: 0x000BEFD4 File Offset: 0x000BD1D4
			public Context(StateMachine.Parameter parameter, StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter default_value) : base(parameter, default_value)
			{
			}

			// Token: 0x060028DF RID: 10463 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void Serialize(BinaryWriter writer)
			{
			}

			// Token: 0x060028E0 RID: 10464 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void Deserialize(IReader reader, StateMachine.Instance smi)
			{
			}

			// Token: 0x060028E1 RID: 10465 RVA: 0x000BEFDE File Offset: 0x000BD1DE
			public override void Set(StateMachine<StateMachineType, StateMachineInstanceType, MasterType, DefType>.SignalParameter value, StateMachineInstanceType smi, bool silenceEvents = false)
			{
				if (!silenceEvents && this.onDirty != null)
				{
					this.onDirty(smi);
				}
			}

			// Token: 0x060028E2 RID: 10466 RVA: 0x000AA038 File Offset: 0x000A8238
			public override void ShowEditor(StateMachine.Instance base_smi)
			{
			}

			// Token: 0x060028E3 RID: 10467 RVA: 0x001E1110 File Offset: 0x001DF310
			public override void ShowDevTool(StateMachine.Instance base_smi)
			{
				if (ImGui.Button(this.parameter.name))
				{
					StateMachineInstanceType smi = (StateMachineInstanceType)((object)base_smi);
					this.Set(null, smi, false);
				}
			}
		}
	}
}
