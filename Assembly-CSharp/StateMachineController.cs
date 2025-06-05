using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

// Token: 0x02000920 RID: 2336
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/StateMachineController")]
public class StateMachineController : KMonoBehaviour, ISaveLoadableDetails, IStateMachineControllerHack
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060028ED RID: 10477 RVA: 0x000BF0D5 File Offset: 0x000BD2D5
	public StateMachineController.CmpDef cmpdef
	{
		get
		{
			return this.defHandle.Get<StateMachineController.CmpDef>();
		}
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x000BF0E2 File Offset: 0x000BD2E2
	public IEnumerator<StateMachine.Instance> GetEnumerator()
	{
		return this.stateMachines.GetEnumerator();
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000BF0F4 File Offset: 0x000BD2F4
	public void AddStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!this.stateMachines.Contains(state_machine))
		{
			this.stateMachines.Add(state_machine);
			MyAttributes.OnAwake(state_machine, this);
		}
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000BF117 File Offset: 0x000BD317
	public void RemoveStateMachineInstance(StateMachine.Instance state_machine)
	{
		if (!state_machine.GetStateMachine().saveHistory && !state_machine.GetStateMachine().debugSettings.saveHistory)
		{
			this.stateMachines.Remove(state_machine);
		}
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000BF145 File Offset: 0x000BD345
	public bool HasStateMachineInstance(StateMachine.Instance state_machine)
	{
		return this.stateMachines.Contains(state_machine);
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000BF153 File Offset: 0x000BD353
	public void AddDef(StateMachine.BaseDef def)
	{
		this.cmpdef.defs.Add(def);
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000BF166 File Offset: 0x000BD366
	public LoggerFSSSS GetLog()
	{
		return this.log;
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000BF16E File Offset: 0x000BD36E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.log.SetName(base.name);
		base.Subscribe<StateMachineController>(1969584890, StateMachineController.OnTargetDestroyedDelegate);
		base.Subscribe<StateMachineController>(1502190696, StateMachineController.OnTargetDestroyedDelegate);
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x001E1140 File Offset: 0x001DF340
	private void OnTargetDestroyed(object data)
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x001E1184 File Offset: 0x001DF384
	protected override void OnLoadLevel()
	{
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.FreeResources();
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x001E11C4 File Offset: 0x001DF3C4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		while (this.stateMachines.Count > 0)
		{
			StateMachine.Instance instance = this.stateMachines[0];
			instance.StopSM("StateMachineController.OnCleanUp");
			this.stateMachines.Remove(instance);
		}
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x001E120C File Offset: 0x001DF40C
	public void CreateSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			baseDef.CreateSMI(this);
		}
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x001E1274 File Offset: 0x001DF474
	public void StartSMIS()
	{
		if (!this.defHandle.IsValid())
		{
			return;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (!baseDef.preventStartSMIOnSpawn)
			{
				StateMachine.Instance smi = this.GetSMI(Singleton<StateMachineManager>.Instance.CreateStateMachine(baseDef.GetStateMachineType()).GetStateMachineInstanceType());
				if (smi != null && !smi.IsRunning())
				{
					smi.StartSM();
				}
			}
		}
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000BF1A9 File Offset: 0x000BD3A9
	public void Serialize(BinaryWriter writer)
	{
		this.serializer.Serialize(this.stateMachines, writer);
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x000BF1BD File Offset: 0x000BD3BD
	public void Deserialize(IReader reader)
	{
		this.serializer.Deserialize(reader);
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000BF1CB File Offset: 0x000BD3CB
	public bool Restore(StateMachine.Instance smi)
	{
		return this.serializer.Restore(smi);
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x001E1308 File Offset: 0x001DF508
	public DefType GetDef<DefType>() where DefType : StateMachine.BaseDef
	{
		if (!this.defHandle.IsValid())
		{
			return default(DefType);
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				return defType;
			}
		}
		return default(DefType);
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x001E1394 File Offset: 0x001DF594
	public List<DefType> GetDefs<DefType>() where DefType : StateMachine.BaseDef
	{
		List<DefType> list = new List<DefType>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			DefType defType = baseDef as DefType;
			if (defType != null)
			{
				list.Add(defType);
			}
		}
		return list;
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x001E1414 File Offset: 0x001DF614
	public StateMachine.Instance GetSMI(Type type)
	{
		for (int i = 0; i < this.stateMachines.Count; i++)
		{
			StateMachine.Instance instance = this.stateMachines[i];
			if (type.IsAssignableFrom(instance.GetType()))
			{
				return instance;
			}
		}
		return null;
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000BF1D9 File Offset: 0x000BD3D9
	public StateMachineInstanceType GetSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		return this.GetSMI(typeof(StateMachineInstanceType)) as StateMachineInstanceType;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x001E1458 File Offset: 0x001DF658
	public List<StateMachineInstanceType> GetAllSMI<StateMachineInstanceType>() where StateMachineInstanceType : class
	{
		List<StateMachineInstanceType> list = new List<StateMachineInstanceType>();
		foreach (StateMachine.Instance instance in this.stateMachines)
		{
			StateMachineInstanceType stateMachineInstanceType = instance as StateMachineInstanceType;
			if (stateMachineInstanceType != null)
			{
				list.Add(stateMachineInstanceType);
			}
		}
		return list;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x001E14C4 File Offset: 0x001DF6C4
	public List<IGameObjectEffectDescriptor> GetDescriptors()
	{
		List<IGameObjectEffectDescriptor> list = new List<IGameObjectEffectDescriptor>();
		if (!this.defHandle.IsValid())
		{
			return list;
		}
		foreach (StateMachine.BaseDef baseDef in this.cmpdef.defs)
		{
			if (baseDef is IGameObjectEffectDescriptor)
			{
				list.Add(baseDef as IGameObjectEffectDescriptor);
			}
		}
		return list;
	}

	// Token: 0x04001BE4 RID: 7140
	public DefHandle defHandle;

	// Token: 0x04001BE5 RID: 7141
	private List<StateMachine.Instance> stateMachines = new List<StateMachine.Instance>();

	// Token: 0x04001BE6 RID: 7142
	private LoggerFSSSS log = new LoggerFSSSS("StateMachineController", 35);

	// Token: 0x04001BE7 RID: 7143
	private StateMachineSerializer serializer = new StateMachineSerializer();

	// Token: 0x04001BE8 RID: 7144
	private static readonly EventSystem.IntraObjectHandler<StateMachineController> OnTargetDestroyedDelegate = new EventSystem.IntraObjectHandler<StateMachineController>(delegate(StateMachineController component, object data)
	{
		component.OnTargetDestroyed(data);
	});

	// Token: 0x02000921 RID: 2337
	public class CmpDef
	{
		// Token: 0x04001BE9 RID: 7145
		public List<StateMachine.BaseDef> defs = new List<StateMachine.BaseDef>();
	}
}
