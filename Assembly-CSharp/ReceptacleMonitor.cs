using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001203 RID: 4611
[SkipSaveFileSerialization]
public class ReceptacleMonitor : StateMachineComponent<ReceptacleMonitor.StatesInstance>, IGameObjectEffectDescriptor, IWiltCause, ISim1000ms
{
	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06005DAE RID: 23982 RVA: 0x000E18F1 File Offset: 0x000DFAF1
	public bool Replanted
	{
		get
		{
			return this.replanted;
		}
	}

	// Token: 0x06005DAF RID: 23983 RVA: 0x000E18F9 File Offset: 0x000DFAF9
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005DB0 RID: 23984 RVA: 0x000E190C File Offset: 0x000DFB0C
	public PlantablePlot GetReceptacle()
	{
		return (PlantablePlot)base.smi.sm.receptacle.Get(base.smi);
	}

	// Token: 0x06005DB1 RID: 23985 RVA: 0x002ADAB4 File Offset: 0x002ABCB4
	public void SetReceptacle(PlantablePlot plot = null)
	{
		if (plot == null)
		{
			base.smi.sm.receptacle.Set(null, base.smi, false);
			this.replanted = false;
		}
		else
		{
			base.smi.sm.receptacle.Set(plot, base.smi, false);
			this.replanted = true;
		}
		base.Trigger(-1636776682, null);
	}

	// Token: 0x06005DB2 RID: 23986 RVA: 0x002ADB24 File Offset: 0x002ABD24
	public void Sim1000ms(float dt)
	{
		if (base.smi.sm.receptacle.Get(base.smi) == null)
		{
			base.smi.GoTo(base.smi.sm.wild);
			return;
		}
		Operational component = base.smi.sm.receptacle.Get(base.smi).GetComponent<Operational>();
		if (component == null)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		if (component.IsOperational)
		{
			base.smi.GoTo(base.smi.sm.operational);
			return;
		}
		base.smi.GoTo(base.smi.sm.inoperational);
	}

	// Token: 0x17000590 RID: 1424
	// (get) Token: 0x06005DB3 RID: 23987 RVA: 0x000E192E File Offset: 0x000DFB2E
	WiltCondition.Condition[] IWiltCause.Conditions
	{
		get
		{
			return new WiltCondition.Condition[]
			{
				WiltCondition.Condition.Receptacle
			};
		}
	}

	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06005DB4 RID: 23988 RVA: 0x002ADBF8 File Offset: 0x002ABDF8
	public string WiltStateString
	{
		get
		{
			string text = "";
			if (base.smi.IsInsideState(base.smi.sm.inoperational))
			{
				text += CREATURES.STATUSITEMS.RECEPTACLEINOPERATIONAL.NAME;
			}
			return text;
		}
	}

	// Token: 0x06005DB5 RID: 23989 RVA: 0x000E193B File Offset: 0x000DFB3B
	public bool HasReceptacle()
	{
		return !base.smi.IsInsideState(base.smi.sm.wild);
	}

	// Token: 0x06005DB6 RID: 23990 RVA: 0x000E195B File Offset: 0x000DFB5B
	public bool HasOperationalReceptacle()
	{
		return base.smi.IsInsideState(base.smi.sm.operational);
	}

	// Token: 0x06005DB7 RID: 23991 RVA: 0x000E1978 File Offset: 0x000DFB78
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.GAMEOBJECTEFFECTS.REQUIRES_RECEPTACLE, UI.GAMEOBJECTEFFECTS.TOOLTIPS.REQUIRES_RECEPTACLE, Descriptor.DescriptorType.Requirement, false)
		};
	}

	// Token: 0x040042DB RID: 17115
	private bool replanted;

	// Token: 0x02001204 RID: 4612
	public class StatesInstance : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.GameInstance
	{
		// Token: 0x06005DB9 RID: 23993 RVA: 0x000E19A8 File Offset: 0x000DFBA8
		public StatesInstance(ReceptacleMonitor master) : base(master)
		{
		}
	}

	// Token: 0x02001205 RID: 4613
	public class States : GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor>
	{
		// Token: 0x06005DBA RID: 23994 RVA: 0x002ADC3C File Offset: 0x002ABE3C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.wild;
			base.serializable = StateMachine.SerializeType.Never;
			this.wild.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
			this.inoperational.TriggerOnEnter(GameHashes.ReceptacleInoperational, null);
			this.operational.TriggerOnEnter(GameHashes.ReceptacleOperational, null);
		}

		// Token: 0x040042DC RID: 17116
		public StateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.ObjectParameter<SingleEntityReceptacle> receptacle;

		// Token: 0x040042DD RID: 17117
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State wild;

		// Token: 0x040042DE RID: 17118
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State inoperational;

		// Token: 0x040042DF RID: 17119
		public GameStateMachine<ReceptacleMonitor.States, ReceptacleMonitor.StatesInstance, ReceptacleMonitor, object>.State operational;
	}
}
