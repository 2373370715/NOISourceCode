using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using KSerialization;
using UnityEngine;

// Token: 0x020008EA RID: 2282
public abstract class StateMachine
{
	// Token: 0x060027EE RID: 10222 RVA: 0x000BE6B8 File Offset: 0x000BC8B8
	public StateMachine()
	{
		this.name = base.GetType().FullName;
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000BE6DD File Offset: 0x000BC8DD
	public virtual void FreeResources()
	{
		this.name = null;
		if (this.defaultState != null)
		{
			this.defaultState.FreeResources();
		}
		this.defaultState = null;
		this.parameters = null;
	}

	// Token: 0x060027F0 RID: 10224
	public abstract string[] GetStateNames();

	// Token: 0x060027F1 RID: 10225
	public abstract StateMachine.BaseState GetState(string name);

	// Token: 0x060027F2 RID: 10226
	public abstract void BindStates();

	// Token: 0x060027F3 RID: 10227
	public abstract Type GetStateMachineInstanceType();

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000BE707 File Offset: 0x000BC907
	// (set) Token: 0x060027F5 RID: 10229 RVA: 0x000BE70F File Offset: 0x000BC90F
	public int version { get; protected set; }

	// Token: 0x1700012A RID: 298
	// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000BE718 File Offset: 0x000BC918
	// (set) Token: 0x060027F7 RID: 10231 RVA: 0x000BE720 File Offset: 0x000BC920
	public StateMachine.SerializeType serializable { get; protected set; }

	// Token: 0x060027F8 RID: 10232 RVA: 0x000BE729 File Offset: 0x000BC929
	public virtual void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = null;
	}

	// Token: 0x060027F9 RID: 10233 RVA: 0x001DF790 File Offset: 0x001DD990
	public void InitializeStateMachine()
	{
		this.debugSettings = StateMachineDebuggerSettings.Get().CreateEntry(base.GetType());
		StateMachine.BaseState baseState = null;
		this.InitializeStates(out baseState);
		DebugUtil.Assert(baseState != null);
		this.defaultState = baseState;
	}

	// Token: 0x060027FA RID: 10234 RVA: 0x001DF7D0 File Offset: 0x001DD9D0
	public void CreateStates(object state_machine)
	{
		foreach (FieldInfo fieldInfo in state_machine.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
		{
			bool flag = false;
			object[] customAttributes = fieldInfo.GetCustomAttributes(false);
			for (int j = 0; j < customAttributes.Length; j++)
			{
				if (customAttributes[j].GetType() == typeof(StateMachine.DoNotAutoCreate))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.BaseState)))
				{
					StateMachine.BaseState baseState = (StateMachine.BaseState)Activator.CreateInstance(fieldInfo.FieldType);
					this.CreateStates(baseState);
					fieldInfo.SetValue(state_machine, baseState);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine.Parameter)))
				{
					StateMachine.Parameter parameter = (StateMachine.Parameter)fieldInfo.GetValue(state_machine);
					if (parameter == null)
					{
						parameter = (StateMachine.Parameter)Activator.CreateInstance(fieldInfo.FieldType);
						fieldInfo.SetValue(state_machine, parameter);
					}
					parameter.name = fieldInfo.Name;
					parameter.idx = this.parameters.Length;
					this.parameters = this.parameters.Append(parameter);
				}
				else if (fieldInfo.FieldType.IsSubclassOf(typeof(StateMachine)))
				{
					fieldInfo.SetValue(state_machine, this);
				}
			}
		}
	}

	// Token: 0x060027FB RID: 10235 RVA: 0x000BE72E File Offset: 0x000BC92E
	public StateMachine.BaseState GetDefaultState()
	{
		return this.defaultState;
	}

	// Token: 0x060027FC RID: 10236 RVA: 0x000BE736 File Offset: 0x000BC936
	public int GetMaxDepth()
	{
		return this.maxDepth;
	}

	// Token: 0x060027FD RID: 10237 RVA: 0x000BE73E File Offset: 0x000BC93E
	public override string ToString()
	{
		return this.name;
	}

	// Token: 0x04001B84 RID: 7044
	protected string name;

	// Token: 0x04001B85 RID: 7045
	protected int maxDepth;

	// Token: 0x04001B86 RID: 7046
	protected StateMachine.BaseState defaultState;

	// Token: 0x04001B87 RID: 7047
	protected StateMachine.Parameter[] parameters = new StateMachine.Parameter[0];

	// Token: 0x04001B88 RID: 7048
	public int dataTableSize;

	// Token: 0x04001B89 RID: 7049
	public int updateTableSize;

	// Token: 0x04001B8C RID: 7052
	public StateMachineDebuggerSettings.Entry debugSettings;

	// Token: 0x04001B8D RID: 7053
	public bool saveHistory;

	// Token: 0x020008EB RID: 2283
	public sealed class DoNotAutoCreate : Attribute
	{
	}

	// Token: 0x020008EC RID: 2284
	public enum Status
	{
		// Token: 0x04001B8F RID: 7055
		Initialized,
		// Token: 0x04001B90 RID: 7056
		Running,
		// Token: 0x04001B91 RID: 7057
		Failed,
		// Token: 0x04001B92 RID: 7058
		Success
	}

	// Token: 0x020008ED RID: 2285
	public class BaseDef
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x000BE746 File Offset: 0x000BC946
		public StateMachine.Instance CreateSMI(IStateMachineTarget master)
		{
			return Singleton<StateMachineManager>.Instance.CreateSMIFromDef(master, this);
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000BE754 File Offset: 0x000BC954
		public Type GetStateMachineType()
		{
			return base.GetType().DeclaringType;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Configure(GameObject prefab)
		{
		}

		// Token: 0x04001B93 RID: 7059
		public bool preventStartSMIOnSpawn;
	}

	// Token: 0x020008EE RID: 2286
	public class Category : Resource
	{
		// Token: 0x06002803 RID: 10243 RVA: 0x000BB146 File Offset: 0x000B9346
		public Category(string id) : base(id, null, null)
		{
		}
	}

	// Token: 0x020008EF RID: 2287
	[SerializationConfig(MemberSerialization.OptIn)]
	public abstract class Instance
	{
		// Token: 0x06002804 RID: 10244
		public abstract StateMachine.BaseState GetCurrentState();

		// Token: 0x06002805 RID: 10245
		public abstract void GoTo(StateMachine.BaseState state);

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06002806 RID: 10246
		public abstract float timeinstate { get; }

		// Token: 0x06002807 RID: 10247
		public abstract IStateMachineTarget GetMaster();

		// Token: 0x06002808 RID: 10248
		public abstract void StopSM(string reason);

		// Token: 0x06002809 RID: 10249
		public abstract SchedulerHandle Schedule(float time, Action<object> callback, object callback_data = null);

		// Token: 0x0600280A RID: 10250
		public abstract SchedulerHandle ScheduleNextFrame(Action<object> callback, object callback_data = null);

		// Token: 0x0600280B RID: 10251 RVA: 0x000BE761 File Offset: 0x000BC961
		public virtual void FreeResources()
		{
			this.stateMachine = null;
			if (this.subscribedEvents != null)
			{
				this.subscribedEvents.Clear();
			}
			this.subscribedEvents = null;
			this.parameterContexts = null;
			this.dataTable = null;
			this.updateTable = null;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000BE799 File Offset: 0x000BC999
		public Instance(StateMachine state_machine, IStateMachineTarget master)
		{
			this.stateMachine = state_machine;
			this.CreateParameterContexts();
			this.log = new LoggerFSSSS(this.stateMachine.name, 35);
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void OnParamsDeserialized()
		{
		}

		// Token: 0x0600280E RID: 10254 RVA: 0x000BE7D1 File Offset: 0x000BC9D1
		public bool IsRunning()
		{
			return this.GetCurrentState() != null;
		}

		// Token: 0x0600280F RID: 10255 RVA: 0x001DF91C File Offset: 0x001DDB1C
		public void GoTo(string state_name)
		{
			DebugUtil.DevAssert(!KMonoBehaviour.isLoadingScene, "Using Goto while scene was loaded", null);
			StateMachine.BaseState state = this.stateMachine.GetState(state_name);
			this.GoTo(state);
		}

		// Token: 0x06002810 RID: 10256 RVA: 0x000BE7DC File Offset: 0x000BC9DC
		public int GetStackSize()
		{
			return this.stackSize;
		}

		// Token: 0x06002811 RID: 10257 RVA: 0x000BE7E4 File Offset: 0x000BC9E4
		public StateMachine GetStateMachine()
		{
			return this.stateMachine;
		}

		// Token: 0x06002812 RID: 10258 RVA: 0x000AA038 File Offset: 0x000A8238
		[Conditional("UNITY_EDITOR")]
		public void Log(string a, string b = "", string c = "", string d = "")
		{
		}

		// Token: 0x06002813 RID: 10259 RVA: 0x000BE7EC File Offset: 0x000BC9EC
		public bool IsConsoleLoggingEnabled()
		{
			return this.enableConsoleLogging || this.stateMachine.debugSettings.enableConsoleLogging;
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x000BE808 File Offset: 0x000BCA08
		public bool IsBreakOnGoToEnabled()
		{
			return this.breakOnGoTo || this.stateMachine.debugSettings.breakOnGoTo;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000BE824 File Offset: 0x000BCA24
		public LoggerFSSSS GetLog()
		{
			return this.log;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000BE82C File Offset: 0x000BCA2C
		public StateMachine.Parameter.Context[] GetParameterContexts()
		{
			return this.parameterContexts;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000BE834 File Offset: 0x000BCA34
		public StateMachine.Parameter.Context GetParameterContext(StateMachine.Parameter parameter)
		{
			return this.parameterContexts[parameter.idx];
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000BE843 File Offset: 0x000BCA43
		public StateMachine.Status GetStatus()
		{
			return this.status;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000BE84B File Offset: 0x000BCA4B
		public void SetStatus(StateMachine.Status status)
		{
			this.status = status;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000BE854 File Offset: 0x000BCA54
		public void Error()
		{
			if (!StateMachine.Instance.error)
			{
				this.isCrashed = true;
				StateMachine.Instance.error = true;
				RestartWarning.ShouldWarn = true;
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x001DF950 File Offset: 0x001DDB50
		public override string ToString()
		{
			string str = "";
			if (this.GetCurrentState() != null)
			{
				str = this.GetCurrentState().name;
			}
			else if (this.GetStatus() != StateMachine.Status.Initialized)
			{
				str = this.GetStatus().ToString();
			}
			return this.stateMachine.ToString() + "(" + str + ")";
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x001DF9B4 File Offset: 0x001DDBB4
		public virtual void StartSM()
		{
			if (!this.IsRunning())
			{
				StateMachineController component = this.GetComponent<StateMachineController>();
				MyAttributes.OnStart(this, component);
				StateMachine.BaseState defaultState = this.stateMachine.GetDefaultState();
				DebugUtil.Assert(defaultState != null);
				if (!component.Restore(this))
				{
					this.OnParamsDeserialized();
					this.GoTo(defaultState);
				}
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000BE870 File Offset: 0x000BCA70
		public bool HasTag(Tag tag)
		{
			return this.GetComponent<KPrefabID>().HasTag(tag);
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x001DFA04 File Offset: 0x001DDC04
		public bool IsInsideState(StateMachine.BaseState state)
		{
			StateMachine.BaseState currentState = this.GetCurrentState();
			if (currentState == null)
			{
				return false;
			}
			bool flag = state == currentState;
			int num = 0;
			while (!flag && num < currentState.branch.Length && !(flag = (state == currentState.branch[num])))
			{
				num++;
			}
			return flag;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000BE87E File Offset: 0x000BCA7E
		public void ScheduleGoTo(float time, StateMachine.BaseState state)
		{
			if (this.scheduleGoToCallback == null)
			{
				this.scheduleGoToCallback = delegate(object d)
				{
					this.GoTo((StateMachine.BaseState)d);
				};
			}
			this.Schedule(time, this.scheduleGoToCallback, state);
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000BE8A9 File Offset: 0x000BCAA9
		public void Subscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Subscribe(hash, handler);
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000BE8B9 File Offset: 0x000BCAB9
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.GetMaster().Unsubscribe(hash, handler);
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000BE8C8 File Offset: 0x000BCAC8
		public void Trigger(int hash, object data = null)
		{
			this.GetMaster().GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000BE8DC File Offset: 0x000BCADC
		public ComponentType Get<ComponentType>()
		{
			return this.GetComponent<ComponentType>();
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000BE8E4 File Offset: 0x000BCAE4
		public ComponentType GetComponent<ComponentType>()
		{
			return this.GetMaster().GetComponent<ComponentType>();
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x001DFA48 File Offset: 0x001DDC48
		private void CreateParameterContexts()
		{
			this.parameterContexts = new StateMachine.Parameter.Context[this.stateMachine.parameters.Length];
			for (int i = 0; i < this.stateMachine.parameters.Length; i++)
			{
				this.parameterContexts[i] = this.stateMachine.parameters[i].CreateContext();
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06002826 RID: 10278 RVA: 0x000BE8F1 File Offset: 0x000BCAF1
		public GameObject gameObject
		{
			get
			{
				return this.GetMaster().gameObject;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x000BE8FE File Offset: 0x000BCAFE
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x04001B94 RID: 7060
		public string serializationSuffix;

		// Token: 0x04001B95 RID: 7061
		protected LoggerFSSSS log;

		// Token: 0x04001B96 RID: 7062
		protected StateMachine.Status status;

		// Token: 0x04001B97 RID: 7063
		protected StateMachine stateMachine;

		// Token: 0x04001B98 RID: 7064
		protected Stack<StateEvent.Context> subscribedEvents = new Stack<StateEvent.Context>();

		// Token: 0x04001B99 RID: 7065
		protected int stackSize;

		// Token: 0x04001B9A RID: 7066
		protected StateMachine.Parameter.Context[] parameterContexts;

		// Token: 0x04001B9B RID: 7067
		public object[] dataTable;

		// Token: 0x04001B9C RID: 7068
		public StateMachine.Instance.UpdateTableEntry[] updateTable;

		// Token: 0x04001B9D RID: 7069
		private Action<object> scheduleGoToCallback;

		// Token: 0x04001B9E RID: 7070
		public Action<string, StateMachine.Status> OnStop;

		// Token: 0x04001B9F RID: 7071
		public bool breakOnGoTo;

		// Token: 0x04001BA0 RID: 7072
		public bool enableConsoleLogging;

		// Token: 0x04001BA1 RID: 7073
		public bool isCrashed;

		// Token: 0x04001BA2 RID: 7074
		public static bool error;

		// Token: 0x020008F0 RID: 2288
		public struct UpdateTableEntry
		{
			// Token: 0x04001BA3 RID: 7075
			public HandleVector<int>.Handle handle;

			// Token: 0x04001BA4 RID: 7076
			public StateMachineUpdater.BaseUpdateBucket bucket;
		}
	}

	// Token: 0x020008F1 RID: 2289
	[DebuggerDisplay("{longName}")]
	public class BaseState
	{
		// Token: 0x06002829 RID: 10281 RVA: 0x000BE919 File Offset: 0x000BCB19
		public BaseState()
		{
			this.branch = new StateMachine.BaseState[1];
			this.branch[0] = this;
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x001DFAA0 File Offset: 0x001DDCA0
		public void FreeResources()
		{
			if (this.name == null)
			{
				return;
			}
			this.name = null;
			if (this.defaultState != null)
			{
				this.defaultState.FreeResources();
			}
			this.defaultState = null;
			this.events = null;
			int num = 0;
			while (this.transitions != null && num < this.transitions.Count)
			{
				this.transitions[num].Clear();
				num++;
			}
			this.transitions = null;
			this.enterActions = null;
			this.exitActions = null;
			if (this.branch != null)
			{
				for (int i = 0; i < this.branch.Length; i++)
				{
					this.branch[i].FreeResources();
				}
			}
			this.branch = null;
			this.parent = null;
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000BE936 File Offset: 0x000BCB36
		public int GetStateCount()
		{
			return this.branch.Length;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000BE940 File Offset: 0x000BCB40
		public StateMachine.BaseState GetState(int idx)
		{
			return this.branch[idx];
		}

		// Token: 0x04001BA5 RID: 7077
		public string name;

		// Token: 0x04001BA6 RID: 7078
		public string longName;

		// Token: 0x04001BA7 RID: 7079
		public string debugPushName;

		// Token: 0x04001BA8 RID: 7080
		public string debugPopName;

		// Token: 0x04001BA9 RID: 7081
		public string debugExecuteName;

		// Token: 0x04001BAA RID: 7082
		public StateMachine.BaseState defaultState;

		// Token: 0x04001BAB RID: 7083
		public List<StateEvent> events;

		// Token: 0x04001BAC RID: 7084
		public List<StateMachine.BaseTransition> transitions;

		// Token: 0x04001BAD RID: 7085
		public List<StateMachine.UpdateAction> updateActions;

		// Token: 0x04001BAE RID: 7086
		public List<StateMachine.Action> enterActions;

		// Token: 0x04001BAF RID: 7087
		public List<StateMachine.Action> exitActions;

		// Token: 0x04001BB0 RID: 7088
		public StateMachine.BaseState[] branch;

		// Token: 0x04001BB1 RID: 7089
		public StateMachine.BaseState parent;
	}

	// Token: 0x020008F2 RID: 2290
	public class BaseTransition
	{
		// Token: 0x0600282D RID: 10285 RVA: 0x000BE94A File Offset: 0x000BCB4A
		public BaseTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state)
		{
			this.idx = idx;
			this.name = name;
			this.sourceState = source_state;
			this.targetState = target_state;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Evaluate(StateMachine.Instance smi)
		{
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000BE96F File Offset: 0x000BCB6F
		public virtual StateMachine.BaseTransition.Context Register(StateMachine.Instance smi)
		{
			return new StateMachine.BaseTransition.Context(this);
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void Unregister(StateMachine.Instance smi, StateMachine.BaseTransition.Context context)
		{
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000BE977 File Offset: 0x000BCB77
		public void Clear()
		{
			this.name = null;
			if (this.sourceState != null)
			{
				this.sourceState.FreeResources();
			}
			this.sourceState = null;
			if (this.targetState != null)
			{
				this.targetState.FreeResources();
			}
			this.targetState = null;
		}

		// Token: 0x04001BB2 RID: 7090
		public int idx;

		// Token: 0x04001BB3 RID: 7091
		public string name;

		// Token: 0x04001BB4 RID: 7092
		public StateMachine.BaseState sourceState;

		// Token: 0x04001BB5 RID: 7093
		public StateMachine.BaseState targetState;

		// Token: 0x020008F3 RID: 2291
		public struct Context
		{
			// Token: 0x06002832 RID: 10290 RVA: 0x000BE9B4 File Offset: 0x000BCBB4
			public Context(StateMachine.BaseTransition transition)
			{
				this.idx = transition.idx;
				this.handlerId = 0;
			}

			// Token: 0x04001BB6 RID: 7094
			public int idx;

			// Token: 0x04001BB7 RID: 7095
			public int handlerId;
		}
	}

	// Token: 0x020008F4 RID: 2292
	public struct UpdateAction
	{
		// Token: 0x04001BB8 RID: 7096
		public int updateTableIdx;

		// Token: 0x04001BB9 RID: 7097
		public UpdateRate updateRate;

		// Token: 0x04001BBA RID: 7098
		public int nextBucketIdx;

		// Token: 0x04001BBB RID: 7099
		public StateMachineUpdater.BaseUpdateBucket[] buckets;

		// Token: 0x04001BBC RID: 7100
		public object updater;
	}

	// Token: 0x020008F5 RID: 2293
	public struct Action
	{
		// Token: 0x06002833 RID: 10291 RVA: 0x000BE9C9 File Offset: 0x000BCBC9
		public Action(string name, object callback)
		{
			this.name = name;
			this.callback = callback;
		}

		// Token: 0x04001BBD RID: 7101
		public string name;

		// Token: 0x04001BBE RID: 7102
		public object callback;
	}

	// Token: 0x020008F6 RID: 2294
	public class ParameterTransition : StateMachine.BaseTransition
	{
		// Token: 0x06002834 RID: 10292 RVA: 0x000BE9D9 File Offset: 0x000BCBD9
		public ParameterTransition(int idx, string name, StateMachine.BaseState source_state, StateMachine.BaseState target_state) : base(idx, name, source_state, target_state)
		{
		}
	}

	// Token: 0x020008F7 RID: 2295
	public abstract class Parameter
	{
		// Token: 0x06002835 RID: 10293
		public abstract StateMachine.Parameter.Context CreateContext();

		// Token: 0x04001BBF RID: 7103
		public string name;

		// Token: 0x04001BC0 RID: 7104
		public int idx;

		// Token: 0x020008F8 RID: 2296
		public abstract class Context
		{
			// Token: 0x06002837 RID: 10295 RVA: 0x000BE9E6 File Offset: 0x000BCBE6
			public Context(StateMachine.Parameter parameter)
			{
				this.parameter = parameter;
			}

			// Token: 0x06002838 RID: 10296
			public abstract void Serialize(BinaryWriter writer);

			// Token: 0x06002839 RID: 10297
			public abstract void Deserialize(IReader reader, StateMachine.Instance smi);

			// Token: 0x0600283A RID: 10298 RVA: 0x000AA038 File Offset: 0x000A8238
			public virtual void Cleanup()
			{
			}

			// Token: 0x0600283B RID: 10299
			public abstract void ShowEditor(StateMachine.Instance base_smi);

			// Token: 0x0600283C RID: 10300
			public abstract void ShowDevTool(StateMachine.Instance base_smi);

			// Token: 0x04001BC1 RID: 7105
			public StateMachine.Parameter parameter;
		}
	}

	// Token: 0x020008F9 RID: 2297
	public enum SerializeType
	{
		// Token: 0x04001BC3 RID: 7107
		Never,
		// Token: 0x04001BC4 RID: 7108
		ParamsOnly,
		// Token: 0x04001BC5 RID: 7109
		CurrentStateOnly_DEPRECATED,
		// Token: 0x04001BC6 RID: 7110
		Both_DEPRECATED
	}
}
