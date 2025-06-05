using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014F1 RID: 5361
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicPorts")]
public class LogicPorts : KMonoBehaviour, IGameObjectEffectDescriptor, IRenderEveryTick
{
	// Token: 0x06006F77 RID: 28535 RVA: 0x000C4A5D File Offset: 0x000C2C5D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.autoRegisterSimRender = false;
	}

	// Token: 0x06006F78 RID: 28536 RVA: 0x00301228 File Offset: 0x002FF428
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.isPhysical = (component == null || component is BuildingComplete);
		if (!this.isPhysical && !(component is BuildingUnderConstruction))
		{
			OverlayScreen instance = OverlayScreen.Instance;
			instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
			this.OnOverlayChanged(OverlayScreen.Instance.mode);
			this.CreateVisualizers();
			SimAndRenderScheduler.instance.Add(this, false);
			return;
		}
		if (this.isPhysical)
		{
			this.UpdateMissingWireIcon();
			this.CreatePhysicalPorts(false);
			return;
		}
		this.CreateVisualizers();
	}

	// Token: 0x06006F79 RID: 28537 RVA: 0x003012DC File Offset: 0x002FF4DC
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		this.DestroyVisualizers();
		if (this.isPhysical)
		{
			this.DestroyPhysicalPorts();
		}
		base.OnCleanUp();
	}

	// Token: 0x06006F7A RID: 28538 RVA: 0x000ED817 File Offset: 0x000EBA17
	public void RenderEveryTick(float dt)
	{
		this.CreateVisualizers();
	}

	// Token: 0x06006F7B RID: 28539 RVA: 0x000ED817 File Offset: 0x000EBA17
	public void HackRefreshVisualizers()
	{
		this.CreateVisualizers();
	}

	// Token: 0x06006F7C RID: 28540 RVA: 0x0030132C File Offset: 0x002FF52C
	private void CreateVisualizers()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		bool flag = num != this.cell;
		this.cell = num;
		if (!flag)
		{
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				Orientation orientation = component.GetOrientation();
				flag = (orientation != this.orientation);
				this.orientation = orientation;
			}
		}
		if (!flag)
		{
			return;
		}
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				LogicPortVisualizer logicPortVisualizer = new LogicPortVisualizer(this.GetActualCell(port.cellOffset), port.spriteType);
				this.outputPorts.Add(logicPortVisualizer);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer);
			}
		}
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				LogicPortVisualizer logicPortVisualizer2 = new LogicPortVisualizer(this.GetActualCell(port2.cellOffset), port2.spriteType);
				this.inputPorts.Add(logicPortVisualizer2);
				Game.Instance.logicCircuitManager.AddVisElem(logicPortVisualizer2);
			}
		}
	}

	// Token: 0x06006F7D RID: 28541 RVA: 0x0030147C File Offset: 0x002FF67C
	private void DestroyVisualizers()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement elem in this.outputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem);
			}
		}
		if (this.inputPorts != null)
		{
			foreach (ILogicUIElement elem2 in this.inputPorts)
			{
				Game.Instance.logicCircuitManager.RemoveVisElem(elem2);
			}
		}
	}

	// Token: 0x06006F7E RID: 28542 RVA: 0x00301534 File Offset: 0x002FF734
	private void CreatePhysicalPorts(bool forceCreate = false)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num == this.cell && !forceCreate)
		{
			return;
		}
		this.cell = num;
		this.DestroyVisualizers();
		if (this.outputPortInfo != null)
		{
			this.outputPorts = new List<ILogicUIElement>();
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port info = this.outputPortInfo[i];
				LogicEventSender logicEventSender = new LogicEventSender(info.id, this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.outputPorts.Add(logicEventSender);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventSender);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventSender.GetLogicUICell(), logicEventSender, true);
			}
			if (this.serializedOutputValues != null && this.serializedOutputValues.Length == this.outputPorts.Count)
			{
				for (int j = 0; j < this.outputPorts.Count; j++)
				{
					(this.outputPorts[j] as LogicEventSender).SetValue(this.serializedOutputValues[j]);
				}
			}
			else
			{
				for (int k = 0; k < this.outputPorts.Count; k++)
				{
					(this.outputPorts[k] as LogicEventSender).SetValue(0);
				}
			}
		}
		this.serializedOutputValues = null;
		if (this.inputPortInfo != null)
		{
			this.inputPorts = new List<ILogicUIElement>();
			for (int l = 0; l < this.inputPortInfo.Length; l++)
			{
				LogicPorts.Port info = this.inputPortInfo[l];
				LogicEventHandler logicEventHandler = new LogicEventHandler(this.GetActualCell(info.cellOffset), delegate(int new_value, int prev_value)
				{
					if (this != null)
					{
						this.OnLogicValueChanged(info.id, new_value, prev_value);
					}
				}, new Action<int, bool>(this.OnLogicNetworkConnectionChanged), info.spriteType);
				this.inputPorts.Add(logicEventHandler);
				Game.Instance.logicCircuitManager.AddVisElem(logicEventHandler);
				Game.Instance.logicCircuitSystem.AddToNetworks(logicEventHandler.GetLogicUICell(), logicEventHandler, true);
			}
		}
	}

	// Token: 0x06006F7F RID: 28543 RVA: 0x00301790 File Offset: 0x002FF990
	private bool ShowMissingWireIcon()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		if (this.outputPortInfo != null)
		{
			for (int i = 0; i < this.outputPortInfo.Length; i++)
			{
				LogicPorts.Port port = this.outputPortInfo[i];
				if (port.requiresConnection)
				{
					int portCell = this.GetPortCell(port.id);
					if (logicCircuitManager.GetNetworkForCell(portCell) == null)
					{
						return true;
					}
				}
			}
		}
		if (this.inputPortInfo != null)
		{
			for (int j = 0; j < this.inputPortInfo.Length; j++)
			{
				LogicPorts.Port port2 = this.inputPortInfo[j];
				if (port2.requiresConnection)
				{
					int portCell2 = this.GetPortCell(port2.id);
					if (logicCircuitManager.GetNetworkForCell(portCell2) == null)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06006F80 RID: 28544 RVA: 0x000ED81F File Offset: 0x000EBA1F
	public void OnMove()
	{
		this.DestroyPhysicalPorts();
		this.CreatePhysicalPorts(false);
	}

	// Token: 0x06006F81 RID: 28545 RVA: 0x000ED82E File Offset: 0x000EBA2E
	private void OnLogicNetworkConnectionChanged(int cell, bool connected)
	{
		this.UpdateMissingWireIcon();
	}

	// Token: 0x06006F82 RID: 28546 RVA: 0x000ED836 File Offset: 0x000EBA36
	private void UpdateMissingWireIcon()
	{
		LogicCircuitManager.ToggleNoWireConnected(this.ShowMissingWireIcon(), base.gameObject);
	}

	// Token: 0x06006F83 RID: 28547 RVA: 0x00301844 File Offset: 0x002FFA44
	private void DestroyPhysicalPorts()
	{
		if (this.outputPorts != null)
		{
			foreach (ILogicUIElement logicUIElement in this.outputPorts)
			{
				ILogicEventSender logicEventSender = (ILogicEventSender)logicUIElement;
				Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventSender.GetLogicCell(), logicEventSender, true);
			}
		}
		if (this.inputPorts != null)
		{
			for (int i = 0; i < this.inputPorts.Count; i++)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[i] as LogicEventHandler;
				if (logicEventHandler != null)
				{
					Game.Instance.logicCircuitSystem.RemoveFromNetworks(logicEventHandler.GetLogicCell(), logicEventHandler, true);
				}
			}
		}
	}

	// Token: 0x06006F84 RID: 28548 RVA: 0x00266118 File Offset: 0x00264318
	private void OnLogicValueChanged(HashedString port_id, int new_value, int prev_value)
	{
		if (base.gameObject != null)
		{
			base.gameObject.Trigger(-801688580, new LogicValueChanged
			{
				portID = port_id,
				newValue = new_value,
				prevValue = prev_value
			});
		}
	}

	// Token: 0x06006F85 RID: 28549 RVA: 0x00265674 File Offset: 0x00263874
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x06006F86 RID: 28550 RVA: 0x00301900 File Offset: 0x002FFB00
	public bool TryGetPortAtCell(int cell, out LogicPorts.Port port, out bool isInput)
	{
		foreach (LogicPorts.Port port2 in this.inputPortInfo)
		{
			if (this.GetActualCell(port2.cellOffset) == cell)
			{
				port = port2;
				isInput = true;
				return true;
			}
		}
		foreach (LogicPorts.Port port3 in this.outputPortInfo)
		{
			if (this.GetActualCell(port3.cellOffset) == cell)
			{
				port = port3;
				isInput = false;
				return true;
			}
		}
		port = default(LogicPorts.Port);
		isInput = false;
		return false;
	}

	// Token: 0x06006F87 RID: 28551 RVA: 0x00301988 File Offset: 0x002FFB88
	public void SendSignal(HashedString port_id, int new_value)
	{
		if (this.outputPortInfo != null && this.outputPorts == null)
		{
			this.CreatePhysicalPorts(true);
		}
		foreach (ILogicUIElement logicUIElement in this.outputPorts)
		{
			LogicEventSender logicEventSender = (LogicEventSender)logicUIElement;
			if (logicEventSender.ID == port_id)
			{
				logicEventSender.SetValue(new_value);
				break;
			}
		}
	}

	// Token: 0x06006F88 RID: 28552 RVA: 0x00301A08 File Offset: 0x002FFC08
	public int GetPortCell(HashedString port_id)
	{
		foreach (LogicPorts.Port port in this.inputPortInfo)
		{
			if (port.id == port_id)
			{
				return this.GetActualCell(port.cellOffset);
			}
		}
		foreach (LogicPorts.Port port2 in this.outputPortInfo)
		{
			if (port2.id == port_id)
			{
				return this.GetActualCell(port2.cellOffset);
			}
		}
		return -1;
	}

	// Token: 0x06006F89 RID: 28553 RVA: 0x00301A88 File Offset: 0x002FFC88
	public int GetInputValue(HashedString port_id)
	{
		int num = 0;
		while (num < this.inputPortInfo.Length && this.inputPorts != null)
		{
			if (this.inputPortInfo[num].id == port_id)
			{
				LogicEventHandler logicEventHandler = this.inputPorts[num] as LogicEventHandler;
				if (logicEventHandler == null)
				{
					return 0;
				}
				return logicEventHandler.Value;
			}
			else
			{
				num++;
			}
		}
		return 0;
	}

	// Token: 0x06006F8A RID: 28554 RVA: 0x00301AE8 File Offset: 0x002FFCE8
	public int GetOutputValue(HashedString port_id)
	{
		for (int i = 0; i < this.outputPorts.Count; i++)
		{
			LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
			if (logicEventSender == null)
			{
				return 0;
			}
			if (logicEventSender.ID == port_id)
			{
				return logicEventSender.GetLogicValue();
			}
		}
		return 0;
	}

	// Token: 0x06006F8B RID: 28555 RVA: 0x00301B38 File Offset: 0x002FFD38
	public bool IsPortConnected(HashedString port_id)
	{
		int portCell = this.GetPortCell(port_id);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell) != null;
	}

	// Token: 0x06006F8C RID: 28556 RVA: 0x000ED849 File Offset: 0x000EBA49
	private void OnOverlayChanged(HashedString mode)
	{
		if (mode == OverlayModes.Logic.ID)
		{
			base.enabled = true;
			this.CreateVisualizers();
			return;
		}
		base.enabled = false;
		this.DestroyVisualizers();
	}

	// Token: 0x06006F8D RID: 28557 RVA: 0x00301B60 File Offset: 0x002FFD60
	public LogicWire.BitDepth GetConnectedWireBitDepth(HashedString port_id)
	{
		LogicWire.BitDepth result = LogicWire.BitDepth.NumRatings;
		int portCell = this.GetPortCell(port_id);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component = gameObject.GetComponent<LogicWire>();
			if (component != null)
			{
				result = component.MaxBitDepth;
			}
		}
		return result;
	}

	// Token: 0x06006F8E RID: 28558 RVA: 0x00301BA8 File Offset: 0x002FFDA8
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		LogicPorts component = go.GetComponent<LogicPorts>();
		if (component != null)
		{
			if (component.inputPortInfo != null && component.inputPortInfo.Length != 0)
			{
				Descriptor item = new Descriptor(UI.LOGIC_PORTS.INPUT_PORTS, UI.LOGIC_PORTS.INPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item);
				foreach (LogicPorts.Port port in component.inputPortInfo)
				{
					string tooltip = string.Format(UI.LOGIC_PORTS.INPUT_PORT_TOOLTIP, port.activeDescription, port.inactiveDescription);
					item = new Descriptor(port.description, tooltip, Descriptor.DescriptorType.Effect, false);
					item.IncreaseIndent();
					list.Add(item);
				}
			}
			if (component.outputPortInfo != null && component.outputPortInfo.Length != 0)
			{
				Descriptor item2 = new Descriptor(UI.LOGIC_PORTS.OUTPUT_PORTS, UI.LOGIC_PORTS.OUTPUT_PORTS_TOOLTIP, Descriptor.DescriptorType.Effect, false);
				list.Add(item2);
				foreach (LogicPorts.Port port2 in component.outputPortInfo)
				{
					string tooltip2 = string.Format(UI.LOGIC_PORTS.OUTPUT_PORT_TOOLTIP, port2.activeDescription, port2.inactiveDescription);
					item2 = new Descriptor(port2.description, tooltip2, Descriptor.DescriptorType.Effect, false);
					item2.IncreaseIndent();
					list.Add(item2);
				}
			}
		}
		return list;
	}

	// Token: 0x06006F8F RID: 28559 RVA: 0x00301D10 File Offset: 0x002FFF10
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.isPhysical && this.outputPorts != null)
		{
			this.serializedOutputValues = new int[this.outputPorts.Count];
			for (int i = 0; i < this.outputPorts.Count; i++)
			{
				LogicEventSender logicEventSender = this.outputPorts[i] as LogicEventSender;
				this.serializedOutputValues[i] = logicEventSender.GetLogicValue();
			}
		}
	}

	// Token: 0x06006F90 RID: 28560 RVA: 0x000ED873 File Offset: 0x000EBA73
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedOutputValues = null;
	}

	// Token: 0x040053D3 RID: 21459
	[SerializeField]
	public LogicPorts.Port[] outputPortInfo;

	// Token: 0x040053D4 RID: 21460
	[SerializeField]
	public LogicPorts.Port[] inputPortInfo;

	// Token: 0x040053D5 RID: 21461
	public List<ILogicUIElement> outputPorts;

	// Token: 0x040053D6 RID: 21462
	public List<ILogicUIElement> inputPorts;

	// Token: 0x040053D7 RID: 21463
	private int cell = -1;

	// Token: 0x040053D8 RID: 21464
	private Orientation orientation = Orientation.NumRotations;

	// Token: 0x040053D9 RID: 21465
	[Serialize]
	private int[] serializedOutputValues;

	// Token: 0x040053DA RID: 21466
	private bool isPhysical;

	// Token: 0x020014F2 RID: 5362
	[Serializable]
	public struct Port
	{
		// Token: 0x06006F92 RID: 28562 RVA: 0x000ED892 File Offset: 0x000EBA92
		public Port(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon, LogicPortSpriteType sprite_type, bool display_custom_name = false)
		{
			this.id = id;
			this.cellOffset = cell_offset;
			this.description = description;
			this.activeDescription = activeDescription;
			this.inactiveDescription = inactiveDescription;
			this.requiresConnection = show_wire_missing_icon;
			this.spriteType = sprite_type;
			this.displayCustomName = display_custom_name;
		}

		// Token: 0x06006F93 RID: 28563 RVA: 0x000ED8D1 File Offset: 0x000EBAD1
		public static LogicPorts.Port InputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Input, display_custom_name);
		}

		// Token: 0x06006F94 RID: 28564 RVA: 0x000ED8E3 File Offset: 0x000EBAE3
		public static LogicPorts.Port OutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.Output, display_custom_name);
		}

		// Token: 0x06006F95 RID: 28565 RVA: 0x000ED8F5 File Offset: 0x000EBAF5
		public static LogicPorts.Port RibbonInputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonInput, display_custom_name);
		}

		// Token: 0x06006F96 RID: 28566 RVA: 0x000ED907 File Offset: 0x000EBB07
		public static LogicPorts.Port RibbonOutputPort(HashedString id, CellOffset cell_offset, string description, string activeDescription, string inactiveDescription, bool show_wire_missing_icon = false, bool display_custom_name = false)
		{
			return new LogicPorts.Port(id, cell_offset, description, activeDescription, inactiveDescription, show_wire_missing_icon, LogicPortSpriteType.RibbonOutput, display_custom_name);
		}

		// Token: 0x040053DB RID: 21467
		public HashedString id;

		// Token: 0x040053DC RID: 21468
		public CellOffset cellOffset;

		// Token: 0x040053DD RID: 21469
		public string description;

		// Token: 0x040053DE RID: 21470
		public string activeDescription;

		// Token: 0x040053DF RID: 21471
		public string inactiveDescription;

		// Token: 0x040053E0 RID: 21472
		public bool requiresConnection;

		// Token: 0x040053E1 RID: 21473
		public LogicPortSpriteType spriteType;

		// Token: 0x040053E2 RID: 21474
		public bool displayCustomName;
	}
}
