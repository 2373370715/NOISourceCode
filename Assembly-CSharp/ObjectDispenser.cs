using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000F1D RID: 3869
public class ObjectDispenser : Switch, IUserControlledCapacity
{
	// Token: 0x17000442 RID: 1090
	// (get) Token: 0x06004D85 RID: 19845 RVA: 0x000D6B4B File Offset: 0x000D4D4B
	// (set) Token: 0x06004D86 RID: 19846 RVA: 0x000D6B63 File Offset: 0x000D4D63
	public virtual float UserMaxCapacity
	{
		get
		{
			return Mathf.Min(this.userMaxCapacity, base.GetComponent<Storage>().capacityKg);
		}
		set
		{
			this.userMaxCapacity = value;
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06004D87 RID: 19847 RVA: 0x000D6B77 File Offset: 0x000D4D77
	public float AmountStored
	{
		get
		{
			return base.GetComponent<Storage>().MassStored();
		}
	}

	// Token: 0x17000444 RID: 1092
	// (get) Token: 0x06004D88 RID: 19848 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinCapacity
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000445 RID: 1093
	// (get) Token: 0x06004D89 RID: 19849 RVA: 0x000D6B84 File Offset: 0x000D4D84
	public float MaxCapacity
	{
		get
		{
			return base.GetComponent<Storage>().capacityKg;
		}
	}

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x06004D8A RID: 19850 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool WholeValues
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x06004D8B RID: 19851 RVA: 0x000CDA3B File Offset: 0x000CBC3B
	public LocString CapacityUnits
	{
		get
		{
			return GameUtil.GetCurrentMassUnit(false);
		}
	}

	// Token: 0x06004D8C RID: 19852 RVA: 0x000D6B91 File Offset: 0x000D4D91
	protected override void OnPrefabInit()
	{
		this.Initialize();
	}

	// Token: 0x06004D8D RID: 19853 RVA: 0x00274380 File Offset: 0x00272580
	protected void Initialize()
	{
		base.OnPrefabInit();
		this.log = new LoggerFS("ObjectDispenser", 35);
		this.filteredStorage = new FilteredStorage(this, null, this, false, Db.Get().ChoreTypes.StorageFetch);
		base.Subscribe<ObjectDispenser>(-905833192, ObjectDispenser.OnCopySettingsDelegate);
	}

	// Token: 0x06004D8E RID: 19854 RVA: 0x002743D4 File Offset: 0x002725D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new ObjectDispenser.Instance(this, base.IsSwitchedOn);
		this.smi.StartSM();
		if (ObjectDispenser.infoStatusItem == null)
		{
			ObjectDispenser.infoStatusItem = new StatusItem("ObjectDispenserAutomationInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			ObjectDispenser.infoStatusItem.resolveStringCallback = new Func<string, object, string>(ObjectDispenser.ResolveInfoStatusItemString);
		}
		this.filteredStorage.FilterChanged();
		base.GetComponent<KSelectable>().ToggleStatusItem(ObjectDispenser.infoStatusItem, true, this.smi);
	}

	// Token: 0x06004D8F RID: 19855 RVA: 0x000D6B99 File Offset: 0x000D4D99
	protected override void OnCleanUp()
	{
		this.filteredStorage.CleanUp();
		base.OnCleanUp();
	}

	// Token: 0x06004D90 RID: 19856 RVA: 0x0027446C File Offset: 0x0027266C
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ObjectDispenser component = gameObject.GetComponent<ObjectDispenser>();
		if (component == null)
		{
			return;
		}
		this.UserMaxCapacity = component.UserMaxCapacity;
	}

	// Token: 0x06004D91 RID: 19857 RVA: 0x002744A8 File Offset: 0x002726A8
	public void DropHeldItems()
	{
		while (this.storage.Count > 0)
		{
			GameObject gameObject = this.storage.Drop(this.storage.items[0], true);
			if (this.rotatable != null)
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.rotatable.GetRotatedCellOffset(this.dropOffset).ToVector3());
			}
			else
			{
				gameObject.transform.SetPosition(base.transform.GetPosition() + this.dropOffset.ToVector3());
			}
		}
		this.smi.GetMaster().GetComponent<Storage>().DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x06004D92 RID: 19858 RVA: 0x000D6BAC File Offset: 0x000D4DAC
	protected override void Toggle()
	{
		base.Toggle();
	}

	// Token: 0x06004D93 RID: 19859 RVA: 0x000D6BB4 File Offset: 0x000D4DB4
	protected override void OnRefreshUserMenu(object data)
	{
		if (!this.smi.IsAutomated())
		{
			base.OnRefreshUserMenu(data);
		}
	}

	// Token: 0x06004D94 RID: 19860 RVA: 0x00274578 File Offset: 0x00272778
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		ObjectDispenser.Instance instance = (ObjectDispenser.Instance)data;
		string format = instance.IsAutomated() ? BUILDING.STATUSITEMS.OBJECTDISPENSER.AUTOMATION_CONTROL : BUILDING.STATUSITEMS.OBJECTDISPENSER.MANUAL_CONTROL;
		string arg = instance.IsOpened ? BUILDING.STATUSITEMS.OBJECTDISPENSER.OPENED : BUILDING.STATUSITEMS.OBJECTDISPENSER.CLOSED;
		return string.Format(format, arg);
	}

	// Token: 0x04003669 RID: 13929
	public static readonly HashedString PORT_ID = "ObjectDispenser";

	// Token: 0x0400366A RID: 13930
	private LoggerFS log;

	// Token: 0x0400366B RID: 13931
	public CellOffset dropOffset;

	// Token: 0x0400366C RID: 13932
	[MyCmpReq]
	private Building building;

	// Token: 0x0400366D RID: 13933
	[MyCmpReq]
	private Storage storage;

	// Token: 0x0400366E RID: 13934
	[MyCmpGet]
	private Rotatable rotatable;

	// Token: 0x0400366F RID: 13935
	private ObjectDispenser.Instance smi;

	// Token: 0x04003670 RID: 13936
	private static StatusItem infoStatusItem;

	// Token: 0x04003671 RID: 13937
	[Serialize]
	private float userMaxCapacity = float.PositiveInfinity;

	// Token: 0x04003672 RID: 13938
	protected FilteredStorage filteredStorage;

	// Token: 0x04003673 RID: 13939
	private static readonly EventSystem.IntraObjectHandler<ObjectDispenser> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ObjectDispenser>(delegate(ObjectDispenser component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x02000F1E RID: 3870
	public class States : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser>
	{
		// Token: 0x06004D97 RID: 19863 RVA: 0x002745C8 File Offset: 0x002727C8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.idle.PlayAnim("on").EventHandler(GameHashes.OnStorageChange, delegate(ObjectDispenser.Instance smi)
			{
				smi.UpdateState();
			}).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p && !smi.master.GetComponent<Storage>().IsEmpty());
			this.load_item.PlayAnim("working_load").OnAnimQueueComplete(this.load_item_pst);
			this.load_item_pst.ParamTransition<bool>(this.should_open, this.idle, (ObjectDispenser.Instance smi, bool p) => !p).ParamTransition<bool>(this.should_open, this.drop_item, (ObjectDispenser.Instance smi, bool p) => p);
			this.drop_item.PlayAnim("working_dispense").OnAnimQueueComplete(this.idle).Exit(delegate(ObjectDispenser.Instance smi)
			{
				smi.master.DropHeldItems();
			});
		}

		// Token: 0x04003674 RID: 13940
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item;

		// Token: 0x04003675 RID: 13941
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State load_item_pst;

		// Token: 0x04003676 RID: 13942
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State drop_item;

		// Token: 0x04003677 RID: 13943
		public GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.State idle;

		// Token: 0x04003678 RID: 13944
		public StateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.BoolParameter should_open;
	}

	// Token: 0x02000F20 RID: 3872
	public class Instance : GameStateMachine<ObjectDispenser.States, ObjectDispenser.Instance, ObjectDispenser, object>.GameInstance
	{
		// Token: 0x06004DA0 RID: 19872 RVA: 0x00274714 File Offset: 0x00272914
		public Instance(ObjectDispenser master, bool manual_start_state) : base(master)
		{
			this.manual_on = manual_start_state;
			this.operational = base.GetComponent<Operational>();
			this.logic = base.GetComponent<LogicPorts>();
			base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
			base.Subscribe(-801688580, new Action<object>(this.OnLogicValueChanged));
			base.smi.sm.should_open.Set(true, base.smi, false);
		}

		// Token: 0x06004DA1 RID: 19873 RVA: 0x000D6C4B File Offset: 0x000D4E4B
		public void UpdateState()
		{
			base.smi.GoTo(base.sm.load_item);
		}

		// Token: 0x06004DA2 RID: 19874 RVA: 0x000D6C63 File Offset: 0x000D4E63
		public bool IsAutomated()
		{
			return this.logic.IsPortConnected(ObjectDispenser.PORT_ID);
		}

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06004DA3 RID: 19875 RVA: 0x000D6C75 File Offset: 0x000D4E75
		public bool IsOpened
		{
			get
			{
				if (!this.IsAutomated())
				{
					return this.manual_on;
				}
				return this.logic_on;
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x000D6C8C File Offset: 0x000D4E8C
		public void SetSwitchState(bool on)
		{
			this.manual_on = on;
			this.UpdateShouldOpen();
		}

		// Token: 0x06004DA5 RID: 19877 RVA: 0x000D6C9B File Offset: 0x000D4E9B
		public void SetActive(bool active)
		{
			this.operational.SetActive(active, false);
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x000D6CAA File Offset: 0x000D4EAA
		private void OnOperationalChanged(object data)
		{
			this.UpdateShouldOpen();
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0027479C File Offset: 0x0027299C
		private void OnLogicValueChanged(object data)
		{
			LogicValueChanged logicValueChanged = (LogicValueChanged)data;
			if (logicValueChanged.portID != ObjectDispenser.PORT_ID)
			{
				return;
			}
			this.logic_on = LogicCircuitNetwork.IsBitActive(0, logicValueChanged.newValue);
			this.UpdateShouldOpen();
		}

		// Token: 0x06004DA8 RID: 19880 RVA: 0x002747DC File Offset: 0x002729DC
		private void UpdateShouldOpen()
		{
			this.SetActive(this.operational.IsOperational);
			if (!this.operational.IsOperational)
			{
				return;
			}
			if (this.IsAutomated())
			{
				base.smi.sm.should_open.Set(this.logic_on, base.smi, false);
				return;
			}
			base.smi.sm.should_open.Set(this.manual_on, base.smi, false);
		}

		// Token: 0x0400367F RID: 13951
		private Operational operational;

		// Token: 0x04003680 RID: 13952
		public LogicPorts logic;

		// Token: 0x04003681 RID: 13953
		public bool logic_on = true;

		// Token: 0x04003682 RID: 13954
		private bool manual_on;
	}
}
