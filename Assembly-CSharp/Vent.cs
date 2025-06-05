using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A72 RID: 6770
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Vent")]
public class Vent : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x06008D34 RID: 36148 RVA: 0x00100BF2 File Offset: 0x000FEDF2
	// (set) Token: 0x06008D35 RID: 36149 RVA: 0x00100BFA File Offset: 0x000FEDFA
	public int SortKey
	{
		get
		{
			return this.sortKey;
		}
		set
		{
			this.sortKey = value;
		}
	}

	// Token: 0x06008D36 RID: 36150 RVA: 0x00375628 File Offset: 0x00373828
	public void UpdateVentedMass(SimHashes element, float mass)
	{
		if (!this.lifeTimeVentMass.ContainsKey(element))
		{
			this.lifeTimeVentMass.Add(element, mass);
			return;
		}
		Dictionary<SimHashes, float> dictionary = this.lifeTimeVentMass;
		dictionary[element] += mass;
	}

	// Token: 0x06008D37 RID: 36151 RVA: 0x00100C03 File Offset: 0x000FEE03
	public float GetVentedMass(SimHashes element)
	{
		if (this.lifeTimeVentMass.ContainsKey(element))
		{
			return this.lifeTimeVentMass[element];
		}
		return 0f;
	}

	// Token: 0x06008D38 RID: 36152 RVA: 0x0037566C File Offset: 0x0037386C
	public bool Closed()
	{
		bool flag = false;
		return (this.operational.Flags.TryGetValue(LogicOperationalController.LogicOperationalFlag, out flag) && !flag) || (this.operational.Flags.TryGetValue(BuildingEnabledButton.EnabledFlag, out flag) && !flag);
	}

	// Token: 0x06008D39 RID: 36153 RVA: 0x003756B8 File Offset: 0x003738B8
	protected override void OnSpawn()
	{
		Building component = base.GetComponent<Building>();
		this.cell = component.GetUtilityOutputCell();
		this.smi = new Vent.StatesInstance(this);
		this.smi.StartSM();
	}

	// Token: 0x06008D3A RID: 36154 RVA: 0x003756F0 File Offset: 0x003738F0
	public Vent.State GetEndPointState()
	{
		Vent.State result = Vent.State.Invalid;
		Endpoint endpoint = this.endpointType;
		if (endpoint != Endpoint.Source)
		{
			if (endpoint == Endpoint.Sink)
			{
				result = Vent.State.Ready;
				int num = this.cell;
				if (!this.IsValidOutputCell(num))
				{
					result = (Grid.Solid[num] ? Vent.State.Blocked : Vent.State.OverPressure);
				}
			}
		}
		else
		{
			result = (this.IsConnected() ? Vent.State.Ready : Vent.State.Blocked);
		}
		return result;
	}

	// Token: 0x06008D3B RID: 36155 RVA: 0x00375744 File Offset: 0x00373944
	public bool IsConnected()
	{
		UtilityNetwork networkForCell = Conduit.GetNetworkManager(this.conduitType).GetNetworkForCell(this.cell);
		return networkForCell != null && (networkForCell as FlowUtilityNetwork).HasSinks;
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x06008D3C RID: 36156 RVA: 0x00100C25 File Offset: 0x000FEE25
	public bool IsBlocked
	{
		get
		{
			return this.GetEndPointState() != Vent.State.Ready;
		}
	}

	// Token: 0x06008D3D RID: 36157 RVA: 0x00375778 File Offset: 0x00373978
	private bool IsValidOutputCell(int output_cell)
	{
		bool result = false;
		if ((this.structure == null || !this.structure.IsEntombed() || !this.Closed()) && !Grid.Solid[output_cell])
		{
			result = (Grid.Mass[output_cell] < this.overpressureMass);
		}
		return result;
	}

	// Token: 0x06008D3E RID: 36158 RVA: 0x003757CC File Offset: 0x003739CC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		string formattedMass = GameUtil.GetFormattedMass(this.overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
		return new List<Descriptor>
		{
			new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04006A76 RID: 27254
	private int cell = -1;

	// Token: 0x04006A77 RID: 27255
	private int sortKey;

	// Token: 0x04006A78 RID: 27256
	[Serialize]
	public Dictionary<SimHashes, float> lifeTimeVentMass = new Dictionary<SimHashes, float>();

	// Token: 0x04006A79 RID: 27257
	private Vent.StatesInstance smi;

	// Token: 0x04006A7A RID: 27258
	[SerializeField]
	public ConduitType conduitType = ConduitType.Gas;

	// Token: 0x04006A7B RID: 27259
	[SerializeField]
	public Endpoint endpointType;

	// Token: 0x04006A7C RID: 27260
	[SerializeField]
	public float overpressureMass = 1f;

	// Token: 0x04006A7D RID: 27261
	[NonSerialized]
	public bool showConnectivityIcons = true;

	// Token: 0x04006A7E RID: 27262
	[MyCmpGet]
	[NonSerialized]
	public Structure structure;

	// Token: 0x04006A7F RID: 27263
	[MyCmpGet]
	[NonSerialized]
	public Operational operational;

	// Token: 0x02001A73 RID: 6771
	public enum State
	{
		// Token: 0x04006A81 RID: 27265
		Invalid,
		// Token: 0x04006A82 RID: 27266
		Ready,
		// Token: 0x04006A83 RID: 27267
		Blocked,
		// Token: 0x04006A84 RID: 27268
		OverPressure,
		// Token: 0x04006A85 RID: 27269
		Closed
	}

	// Token: 0x02001A74 RID: 6772
	public class StatesInstance : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.GameInstance
	{
		// Token: 0x06008D40 RID: 36160 RVA: 0x00100C66 File Offset: 0x000FEE66
		public StatesInstance(Vent master) : base(master)
		{
			this.exhaust = master.GetComponent<Exhaust>();
		}

		// Token: 0x06008D41 RID: 36161 RVA: 0x00100C7B File Offset: 0x000FEE7B
		public bool NeedsExhaust()
		{
			return this.exhaust != null && base.master.GetEndPointState() != Vent.State.Ready && base.master.endpointType == Endpoint.Source;
		}

		// Token: 0x06008D42 RID: 36162 RVA: 0x00100CA9 File Offset: 0x000FEEA9
		public bool Blocked()
		{
			return base.master.GetEndPointState() == Vent.State.Blocked && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x06008D43 RID: 36163 RVA: 0x00100CC9 File Offset: 0x000FEEC9
		public bool OverPressure()
		{
			return this.exhaust != null && base.master.GetEndPointState() == Vent.State.OverPressure && base.master.endpointType > Endpoint.Source;
		}

		// Token: 0x06008D44 RID: 36164 RVA: 0x00375820 File Offset: 0x00373A20
		public void CheckTransitions()
		{
			if (this.NeedsExhaust())
			{
				base.smi.GoTo(base.sm.needExhaust);
				return;
			}
			if (base.master.Closed())
			{
				base.smi.GoTo(base.sm.closed);
				return;
			}
			if (this.Blocked())
			{
				base.smi.GoTo(base.sm.open.blocked);
				return;
			}
			if (this.OverPressure())
			{
				base.smi.GoTo(base.sm.open.overPressure);
				return;
			}
			base.smi.GoTo(base.sm.open.idle);
		}

		// Token: 0x06008D45 RID: 36165 RVA: 0x00100CF7 File Offset: 0x000FEEF7
		public StatusItem SelectStatusItem(StatusItem gas_status_item, StatusItem liquid_status_item)
		{
			if (base.master.conduitType != ConduitType.Gas)
			{
				return liquid_status_item;
			}
			return gas_status_item;
		}

		// Token: 0x04006A86 RID: 27270
		private Exhaust exhaust;
	}

	// Token: 0x02001A75 RID: 6773
	public class States : GameStateMachine<Vent.States, Vent.StatesInstance, Vent>
	{
		// Token: 0x06008D46 RID: 36166 RVA: 0x003758D4 File Offset: 0x00373AD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.open.idle;
			this.root.Update("CheckTransitions", delegate(Vent.StatesInstance smi, float dt)
			{
				smi.CheckTransitions();
			}, UpdateRate.SIM_200ms, false);
			this.open.TriggerOnEnter(GameHashes.VentOpen, null);
			this.closed.TriggerOnEnter(GameHashes.VentClosed, null);
			this.open.blocked.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentObstructed, Db.Get().BuildingStatusItems.LiquidVentObstructed), null);
			this.open.overPressure.ToggleStatusItem((Vent.StatesInstance smi) => smi.SelectStatusItem(Db.Get().BuildingStatusItems.GasVentOverPressure, Db.Get().BuildingStatusItems.LiquidVentOverPressure), null);
		}

		// Token: 0x04006A87 RID: 27271
		public Vent.States.OpenState open;

		// Token: 0x04006A88 RID: 27272
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State closed;

		// Token: 0x04006A89 RID: 27273
		public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State needExhaust;

		// Token: 0x02001A76 RID: 6774
		public class OpenState : GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State
		{
			// Token: 0x04006A8A RID: 27274
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State idle;

			// Token: 0x04006A8B RID: 27275
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State blocked;

			// Token: 0x04006A8C RID: 27276
			public GameStateMachine<Vent.States, Vent.StatesInstance, Vent, object>.State overPressure;
		}
	}
}
