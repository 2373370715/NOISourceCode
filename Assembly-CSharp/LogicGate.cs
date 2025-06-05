using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E7F RID: 3711
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicGate : LogicGateBase, ILogicEventSender, ILogicNetworkConnection
{
	// Token: 0x06004914 RID: 18708 RVA: 0x00265894 File Offset: 0x00263A94
	protected override void OnSpawn()
	{
		this.inputOne = new LogicEventHandler(base.InputCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		if (base.RequiresTwoInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		else if (base.RequiresFourInputs)
		{
			this.inputTwo = new LogicEventHandler(base.InputCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputThree = new LogicEventHandler(base.InputCellThree, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
			this.inputFour = new LogicEventHandler(base.InputCellFour, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.Input);
		}
		if (base.RequiresControlInputs)
		{
			this.controlOne = new LogicEventHandler(base.ControlCellOne, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
			this.controlTwo = new LogicEventHandler(base.ControlCellTwo, new Action<int, int>(this.UpdateState), null, LogicPortSpriteType.ControlInput);
		}
		if (base.RequiresFourOutputs)
		{
			this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
			this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
			this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
			this.outputTwoSender = new LogicEventSender(LogicGateBase.OUTPUT_TWO_PORT_ID, base.OutputCellTwo, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_TWO_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputThreeSender = new LogicEventSender(LogicGateBase.OUTPUT_THREE_PORT_ID, base.OutputCellThree, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_THREE_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
			this.outputFourSender = new LogicEventSender(LogicGateBase.OUTPUT_FOUR_PORT_ID, base.OutputCellFour, delegate(int new_value, int prev_value)
			{
				if (this != null)
				{
					this.OnAdditionalOutputsLogicValueChanged(LogicGateBase.OUTPUT_FOUR_PORT_ID, new_value, prev_value);
				}
			}, null, LogicPortSpriteType.Output);
		}
		base.Subscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate);
		base.Subscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate);
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || !component.IsBroken)
		{
			this.Connect();
		}
	}

	// Token: 0x06004915 RID: 18709 RVA: 0x000D3D67 File Offset: 0x000D1F67
	protected override void OnCleanUp()
	{
		this.cleaningUp = true;
		this.Disconnect();
		base.Unsubscribe<LogicGate>(774203113, LogicGate.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicGate>(-1735440190, LogicGate.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x06004916 RID: 18710 RVA: 0x000D3D9E File Offset: 0x000D1F9E
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06004917 RID: 18711 RVA: 0x000D3DA6 File Offset: 0x000D1FA6
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06004918 RID: 18712 RVA: 0x00265A84 File Offset: 0x00263C84
	private void Connect()
	{
		if (!this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = true;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.AddToNetworks(outputCellOne, this, true);
			this.outputOne = new LogicPortVisualizer(outputCellOne, LogicPortSpriteType.Output);
			logicCircuitManager.AddVisElem(this.outputOne);
			if (base.RequiresFourOutputs)
			{
				this.outputTwo = new LogicPortVisualizer(base.OutputCellTwo, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.AddVisElem(this.outputTwo);
				this.outputThree = new LogicPortVisualizer(base.OutputCellThree, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.AddVisElem(this.outputThree);
				this.outputFour = new LogicPortVisualizer(base.OutputCellFour, LogicPortSpriteType.Output);
				logicCircuitSystem.AddToNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.AddVisElem(this.outputFour);
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.AddToNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.AddVisElem(this.inputOne);
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.AddToNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.AddToNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.AddVisElem(this.inputTwo);
				logicCircuitSystem.AddToNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.AddVisElem(this.inputThree);
				logicCircuitSystem.AddToNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.AddVisElem(this.inputFour);
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.AddToNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.AddVisElem(this.controlOne);
				logicCircuitSystem.AddToNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.AddVisElem(this.controlTwo);
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x06004919 RID: 18713 RVA: 0x00265C80 File Offset: 0x00263E80
	private void Disconnect()
	{
		if (this.connected)
		{
			LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
			UtilityNetworkManager<LogicCircuitNetwork, LogicWire> logicCircuitSystem = Game.Instance.logicCircuitSystem;
			this.connected = false;
			int outputCellOne = base.OutputCellOne;
			logicCircuitSystem.RemoveFromNetworks(outputCellOne, this, true);
			logicCircuitManager.RemoveVisElem(this.outputOne);
			this.outputOne = null;
			if (base.RequiresFourOutputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellTwo, this.outputTwoSender, true);
				logicCircuitManager.RemoveVisElem(this.outputTwo);
				this.outputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellThree, this.outputThreeSender, true);
				logicCircuitManager.RemoveVisElem(this.outputThree);
				this.outputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.OutputCellFour, this.outputFourSender, true);
				logicCircuitManager.RemoveVisElem(this.outputFour);
				this.outputFour = null;
			}
			int inputCellOne = base.InputCellOne;
			logicCircuitSystem.RemoveFromNetworks(inputCellOne, this.inputOne, true);
			logicCircuitManager.RemoveVisElem(this.inputOne);
			this.inputOne = null;
			if (base.RequiresTwoInputs)
			{
				int inputCellTwo = base.InputCellTwo;
				logicCircuitSystem.RemoveFromNetworks(inputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
			}
			else if (base.RequiresFourInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.InputCellTwo, this.inputTwo, true);
				logicCircuitManager.RemoveVisElem(this.inputTwo);
				this.inputTwo = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellThree, this.inputThree, true);
				logicCircuitManager.RemoveVisElem(this.inputThree);
				this.inputThree = null;
				logicCircuitSystem.RemoveFromNetworks(base.InputCellFour, this.inputFour, true);
				logicCircuitManager.RemoveVisElem(this.inputFour);
				this.inputFour = null;
			}
			if (base.RequiresControlInputs)
			{
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellOne, this.controlOne, true);
				logicCircuitManager.RemoveVisElem(this.controlOne);
				this.controlOne = null;
				logicCircuitSystem.RemoveFromNetworks(base.ControlCellTwo, this.controlTwo, true);
				logicCircuitManager.RemoveVisElem(this.controlTwo);
				this.controlTwo = null;
			}
			this.RefreshAnimation();
		}
	}

	// Token: 0x0600491A RID: 18714 RVA: 0x00265E84 File Offset: 0x00264084
	private void UpdateState(int new_value, int prev_value)
	{
		if (this.cleaningUp)
		{
			return;
		}
		int value = this.inputOne.Value;
		int num = (this.inputTwo != null) ? this.inputTwo.Value : 0;
		int num2 = (this.inputThree != null) ? this.inputThree.Value : 0;
		int num3 = (this.inputFour != null) ? this.inputFour.Value : 0;
		int value2 = (this.controlOne != null) ? this.controlOne.Value : 0;
		int value3 = (this.controlTwo != null) ? this.controlTwo.Value : 0;
		if (base.RequiresFourInputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			if (this.op == LogicGateBase.Op.Multiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value2))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueOne = num;
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					this.outputValueOne = num2;
				}
				else
				{
					this.outputValueOne = num3;
				}
			}
		}
		if (base.RequiresFourOutputs && base.RequiresControlInputs)
		{
			this.outputValueOne = 0;
			this.outputValueTwo = 0;
			this.outputTwoSender.SetValue(0);
			this.outputValueThree = 0;
			this.outputThreeSender.SetValue(0);
			this.outputValueFour = 0;
			this.outputFourSender.SetValue(0);
			if (this.op == LogicGateBase.Op.Demultiplexer)
			{
				if (!LogicCircuitNetwork.IsBitActive(0, value2))
				{
					if (!LogicCircuitNetwork.IsBitActive(0, value3))
					{
						this.outputValueOne = value;
					}
					else
					{
						this.outputValueTwo = value;
						this.outputTwoSender.SetValue(value);
					}
				}
				else if (!LogicCircuitNetwork.IsBitActive(0, value3))
				{
					this.outputValueThree = value;
					this.outputThreeSender.SetValue(value);
				}
				else
				{
					this.outputValueFour = value;
					this.outputFourSender.SetValue(value);
				}
			}
		}
		switch (this.op)
		{
		case LogicGateBase.Op.And:
			this.outputValueOne = (value & num);
			break;
		case LogicGateBase.Op.Or:
			this.outputValueOne = (value | num);
			break;
		case LogicGateBase.Op.Not:
		{
			LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
			int inputCellOne = base.InputCellOne;
			GameObject gameObject = Grid.Objects[inputCellOne, 31];
			if (gameObject != null)
			{
				LogicWire component = gameObject.GetComponent<LogicWire>();
				if (component != null)
				{
					bitDepth = component.MaxBitDepth;
				}
			}
			if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
			{
				uint num4 = (uint)value;
				num4 = ~num4;
				num4 &= 15U;
				this.outputValueOne = (int)num4;
			}
			else
			{
				this.outputValueOne = ((value == 0) ? 1 : 0);
			}
			break;
		}
		case LogicGateBase.Op.Xor:
			this.outputValueOne = (value ^ num);
			break;
		case LogicGateBase.Op.CustomSingle:
			this.outputValueOne = this.GetCustomValue(value, num);
			break;
		}
		this.RefreshAnimation();
	}

	// Token: 0x0600491B RID: 18715 RVA: 0x00266118 File Offset: 0x00264318
	private void OnAdditionalOutputsLogicValueChanged(HashedString port_id, int new_value, int prev_value)
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

	// Token: 0x0600491C RID: 18716 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void LogicTick()
	{
	}

	// Token: 0x0600491D RID: 18717 RVA: 0x000B64D6 File Offset: 0x000B46D6
	protected virtual int GetCustomValue(int val1, int val2)
	{
		return val1;
	}

	// Token: 0x0600491E RID: 18718 RVA: 0x0026616C File Offset: 0x0026436C
	public int GetPortValue(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.inputOne.Value;
		case LogicGateBase.PortId.InputTwo:
			if (base.RequiresTwoInputs || base.RequiresFourInputs)
			{
				return this.inputTwo.Value;
			}
			return 0;
		case LogicGateBase.PortId.InputThree:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputThree.Value;
		case LogicGateBase.PortId.InputFour:
			if (!base.RequiresFourInputs)
			{
				return 0;
			}
			return this.inputFour.Value;
		case LogicGateBase.PortId.OutputOne:
			return this.outputValueOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.outputValueTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.outputValueThree;
		case LogicGateBase.PortId.OutputFour:
			return this.outputValueFour;
		case LogicGateBase.PortId.ControlOne:
			return this.controlOne.Value;
		case LogicGateBase.PortId.ControlTwo:
			return this.controlTwo.Value;
		default:
			return this.outputValueOne;
		}
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x0026623C File Offset: 0x0026443C
	public bool GetPortConnected(LogicGateBase.PortId port)
	{
		if ((port == LogicGateBase.PortId.InputTwo && !base.RequiresTwoInputs && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputThree && !base.RequiresFourInputs) || (port == LogicGateBase.PortId.InputFour && !base.RequiresFourInputs))
		{
			return false;
		}
		int cell = base.PortCell(port);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(cell) != null;
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x000D3DAE File Offset: 0x000D1FAE
	public void SetPortDescriptions(LogicGate.LogicGateDescriptions descriptions)
	{
		this.descriptions = descriptions;
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x00266294 File Offset: 0x00264494
	public LogicGate.LogicGateDescriptions.Description GetPortDescription(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresTwoInputs && !base.RequiresFourInputs)
			{
				return LogicGate.INPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.INPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.InputTwo:
			if (this.descriptions.inputTwo == null)
			{
				return LogicGate.INPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.inputTwo;
		case LogicGateBase.PortId.InputThree:
			if (this.descriptions.inputThree == null)
			{
				return LogicGate.INPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.inputThree;
		case LogicGateBase.PortId.InputFour:
			if (this.descriptions.inputFour == null)
			{
				return LogicGate.INPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.inputFour;
		case LogicGateBase.PortId.OutputOne:
			if (this.descriptions.inputOne != null)
			{
				return this.descriptions.inputOne;
			}
			if (!base.RequiresFourOutputs)
			{
				return LogicGate.OUTPUT_ONE_SINGLE_DESCRIPTION;
			}
			return LogicGate.OUTPUT_ONE_MULTI_DESCRIPTION;
		case LogicGateBase.PortId.OutputTwo:
			if (this.descriptions.outputTwo == null)
			{
				return LogicGate.OUTPUT_TWO_DESCRIPTION;
			}
			return this.descriptions.outputTwo;
		case LogicGateBase.PortId.OutputThree:
			if (this.descriptions.outputThree == null)
			{
				return LogicGate.OUTPUT_THREE_DESCRIPTION;
			}
			return this.descriptions.outputThree;
		case LogicGateBase.PortId.OutputFour:
			if (this.descriptions.outputFour == null)
			{
				return LogicGate.OUTPUT_FOUR_DESCRIPTION;
			}
			return this.descriptions.outputFour;
		case LogicGateBase.PortId.ControlOne:
			if (this.descriptions.controlOne == null)
			{
				return LogicGate.CONTROL_ONE_DESCRIPTION;
			}
			return this.descriptions.controlOne;
		case LogicGateBase.PortId.ControlTwo:
			if (this.descriptions.controlTwo == null)
			{
				return LogicGate.CONTROL_TWO_DESCRIPTION;
			}
			return this.descriptions.controlTwo;
		default:
			return this.descriptions.outputOne;
		}
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x000D3DB7 File Offset: 0x000D1FB7
	public int GetLogicValue()
	{
		return this.outputValueOne;
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x000D3DBF File Offset: 0x000D1FBF
	public int GetLogicCell()
	{
		return this.GetLogicUICell();
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x000D3DC7 File Offset: 0x000D1FC7
	public int GetLogicUICell()
	{
		return base.OutputCellOne;
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsLogicInput()
	{
		return false;
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x000D3DCF File Offset: 0x000D1FCF
	private LogicEventHandler GetInputFromControlValue(int val)
	{
		switch (val)
		{
		case 1:
			return this.inputTwo;
		case 2:
			return this.inputThree;
		case 3:
			return this.inputFour;
		}
		return this.inputOne;
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x000D3E04 File Offset: 0x000D2004
	private void ShowSymbolConditionally(bool showAnything, bool active, KBatchedAnimController kbac, KAnimHashedString ifTrue, KAnimHashedString ifFalse)
	{
		if (!showAnything)
		{
			kbac.SetSymbolVisiblity(ifTrue, false);
			kbac.SetSymbolVisiblity(ifFalse, false);
			return;
		}
		kbac.SetSymbolVisiblity(ifTrue, active);
		kbac.SetSymbolVisiblity(ifFalse, !active);
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x000D3E31 File Offset: 0x000D2031
	private void TintSymbolConditionally(bool tintAnything, bool condition, KBatchedAnimController kbac, KAnimHashedString symbol, Color ifTrue, Color ifFalse)
	{
		if (tintAnything)
		{
			kbac.SetSymbolTint(symbol, condition ? ifTrue : ifFalse);
			return;
		}
		kbac.SetSymbolTint(symbol, Color.white);
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x000D3E55 File Offset: 0x000D2055
	private void SetBloomSymbolShowing(bool showing, KBatchedAnimController kbac, KAnimHashedString symbol, KAnimHashedString bloomSymbol)
	{
		kbac.SetSymbolVisiblity(bloomSymbol, showing);
		kbac.SetSymbolVisiblity(symbol, !showing);
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x0026643C File Offset: 0x0026463C
	protected void RefreshAnimation()
	{
		if (this.cleaningUp)
		{
			return;
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.op == LogicGateBase.Op.Multiplexer)
		{
			int num = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value) * 2;
			if (this.lastAnimState != num)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num;
			LogicEventHandler inputFromControlValue = this.GetInputFromControlValue(num);
			KAnimHashedString[] array = LogicGate.multiplexerSymbolPaths[num];
			LogicCircuitNetwork logicCircuitNetwork = Game.Instance.logicCircuitSystem.GetNetworkForCell(inputFromControlValue.GetLogicCell()) as LogicCircuitNetwork;
			UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellOne);
			UtilityNetwork networkForCell2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellTwo);
			UtilityNetwork networkForCell3 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellThree);
			UtilityNetwork networkForCell4 = Game.Instance.logicCircuitSystem.GetNetworkForCell(base.InputCellFour);
			this.ShowSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL_BLM_RED, LogicGate.INPUT2_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL_BLM_RED, LogicGate.INPUT3_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL_BLM_RED, LogicGate.INPUT4_SYMBOL_BLM_GRN);
			this.ShowSymbolConditionally(logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL_BLM_RED, LogicGate.OUTPUT1_SYMBOL_BLM_GRN);
			this.TintSymbolConditionally(networkForCell != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell2 != null, this.inputTwo.Value == 0, component, LogicGate.INPUT2_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell3 != null, this.inputThree.Value == 0, component, LogicGate.INPUT3_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(networkForCell4 != null, this.inputFour.Value == 0, component, LogicGate.INPUT4_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			this.TintSymbolConditionally(Game.Instance.logicCircuitSystem.GetNetworkForCell(base.OutputCellOne) != null && logicCircuitNetwork != null, inputFromControlValue.Value == 0, component, LogicGate.OUTPUT1_SYMBOL, this.inactiveTintColor, this.activeTintColor);
			for (int i = 0; i < LogicGate.multiplexerSymbols.Length; i++)
			{
				KAnimHashedString symbol = LogicGate.multiplexerSymbols[i];
				KAnimHashedString kanimHashedString = LogicGate.multiplexerBloomSymbols[i];
				bool flag = Array.IndexOf<KAnimHashedString>(array, kanimHashedString) != -1 && logicCircuitNetwork != null;
				this.SetBloomSymbolShowing(flag, component, symbol, kanimHashedString);
				if (flag)
				{
					component.SetSymbolTint(kanimHashedString, (inputFromControlValue.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			return;
		}
		if (this.op == LogicGateBase.Op.Demultiplexer)
		{
			int num2 = LogicCircuitNetwork.GetBitValue(0, this.controlOne.Value) * 2 + LogicCircuitNetwork.GetBitValue(0, this.controlTwo.Value);
			if (this.lastAnimState != num2)
			{
				if (this.lastAnimState == -1)
				{
					component.Play(num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
				else
				{
					component.Play(this.lastAnimState.ToString() + "_" + num2.ToString(), KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			this.lastAnimState = num2;
			KAnimHashedString[] array2 = LogicGate.demultiplexerSymbolPaths[num2];
			LogicCircuitNetwork logicCircuitNetwork2 = Game.Instance.logicCircuitSystem.GetNetworkForCell(this.inputOne.GetLogicCell()) as LogicCircuitNetwork;
			for (int j = 0; j < LogicGate.demultiplexerSymbols.Length; j++)
			{
				KAnimHashedString symbol2 = LogicGate.demultiplexerSymbols[j];
				KAnimHashedString kanimHashedString2 = LogicGate.demultiplexerBloomSymbols[j];
				bool flag2 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString2) != -1 && logicCircuitNetwork2 != null;
				this.SetBloomSymbolShowing(flag2, component, symbol2, kanimHashedString2);
				if (flag2)
				{
					component.SetSymbolTint(kanimHashedString2, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
				}
			}
			this.ShowSymbolConditionally(logicCircuitNetwork2 != null, this.inputOne.Value == 0, component, LogicGate.INPUT1_SYMBOL_BLM_RED, LogicGate.INPUT1_SYMBOL_BLM_GRN);
			if (logicCircuitNetwork2 != null)
			{
				component.SetSymbolTint(LogicGate.INPUT1_SYMBOL_BLOOM, (this.inputOne.Value == 0) ? this.inactiveTintColor : this.activeTintColor);
			}
			int[] array3 = new int[]
			{
				base.OutputCellOne,
				base.OutputCellTwo,
				base.OutputCellThree,
				base.OutputCellFour
			};
			for (int k = 0; k < LogicGate.demultiplexerOutputSymbols.Length; k++)
			{
				KAnimHashedString kanimHashedString3 = LogicGate.demultiplexerOutputSymbols[k];
				bool flag3 = Array.IndexOf<KAnimHashedString>(array2, kanimHashedString3) == -1 || this.inputOne.Value == 0;
				UtilityNetwork networkForCell5 = Game.Instance.logicCircuitSystem.GetNetworkForCell(array3[k]);
				this.TintSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, kanimHashedString3, this.inactiveTintColor, this.activeTintColor);
				this.ShowSymbolConditionally(logicCircuitNetwork2 != null && networkForCell5 != null, flag3, component, LogicGate.demultiplexerOutputRedSymbols[k], LogicGate.demultiplexerOutputGreenSymbols[k]);
			}
			return;
		}
		if (this.op == LogicGateBase.Op.And || this.op == LogicGateBase.Op.Xor || this.op == LogicGateBase.Op.Not || this.op == LogicGateBase.Op.Or)
		{
			int outputCellOne = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				int num3 = this.inputOne.Value * 2 + this.inputTwo.Value;
				if (this.lastAnimState != num3)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + num3.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = num3;
					return;
				}
			}
			else
			{
				int value = this.inputOne.Value;
				if (this.lastAnimState != value)
				{
					if (this.lastAnimState == -1)
					{
						component.Play(value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					else
					{
						component.Play(this.lastAnimState.ToString() + "_" + value.ToString(), KAnim.PlayMode.Once, 1f, 0f);
					}
					this.lastAnimState = value;
					return;
				}
			}
		}
		else
		{
			int outputCellOne2 = base.OutputCellOne;
			if (!(Game.Instance.logicCircuitSystem.GetNetworkForCell(outputCellOne2) is LogicCircuitNetwork))
			{
				component.Play("off", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			if (base.RequiresTwoInputs)
			{
				component.Play("on_" + (this.inputOne.Value + this.inputTwo.Value * 2 + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play("on_" + (this.inputOne.Value + this.outputValueOne * 4).ToString(), KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnLogicNetworkConnectionChanged(bool connected)
	{
	}

	// Token: 0x04003347 RID: 13127
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_INPUT_ONE_INACTIVE
	};

	// Token: 0x04003348 RID: 13128
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_ONE_INACTIVE
	};

	// Token: 0x04003349 RID: 13129
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_TWO_INACTIVE
	};

	// Token: 0x0400334A RID: 13130
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_THREE_INACTIVE
	};

	// Token: 0x0400334B RID: 13131
	private static readonly LogicGate.LogicGateDescriptions.Description INPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_INPUT_FOUR_INACTIVE
	};

	// Token: 0x0400334C RID: 13132
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_SINGLE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_SINGLE_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x0400334D RID: 13133
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_ONE_MULTI_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_ONE_INACTIVE
	};

	// Token: 0x0400334E RID: 13134
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_TWO_INACTIVE
	};

	// Token: 0x0400334F RID: 13135
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_THREE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_THREE_INACTIVE
	};

	// Token: 0x04003350 RID: 13136
	private static readonly LogicGate.LogicGateDescriptions.Description OUTPUT_FOUR_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTI_OUTPUT_FOUR_INACTIVE
	};

	// Token: 0x04003351 RID: 13137
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_ONE_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_ONE_INACTIVE
	};

	// Token: 0x04003352 RID: 13138
	private static readonly LogicGate.LogicGateDescriptions.Description CONTROL_TWO_DESCRIPTION = new LogicGate.LogicGateDescriptions.Description
	{
		name = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_NAME,
		active = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_ACTIVE,
		inactive = UI.LOGIC_PORTS.GATE_MULTIPLEXER_CONTROL_TWO_INACTIVE
	};

	// Token: 0x04003353 RID: 13139
	private LogicGate.LogicGateDescriptions descriptions;

	// Token: 0x04003354 RID: 13140
	private LogicEventSender[] additionalOutputs;

	// Token: 0x04003355 RID: 13141
	private const bool IS_CIRCUIT_ENDPOINT = true;

	// Token: 0x04003356 RID: 13142
	private bool connected;

	// Token: 0x04003357 RID: 13143
	protected bool cleaningUp;

	// Token: 0x04003358 RID: 13144
	private int lastAnimState = -1;

	// Token: 0x04003359 RID: 13145
	[Serialize]
	protected int outputValueOne;

	// Token: 0x0400335A RID: 13146
	[Serialize]
	protected int outputValueTwo;

	// Token: 0x0400335B RID: 13147
	[Serialize]
	protected int outputValueThree;

	// Token: 0x0400335C RID: 13148
	[Serialize]
	protected int outputValueFour;

	// Token: 0x0400335D RID: 13149
	private LogicEventHandler inputOne;

	// Token: 0x0400335E RID: 13150
	private LogicEventHandler inputTwo;

	// Token: 0x0400335F RID: 13151
	private LogicEventHandler inputThree;

	// Token: 0x04003360 RID: 13152
	private LogicEventHandler inputFour;

	// Token: 0x04003361 RID: 13153
	private LogicPortVisualizer outputOne;

	// Token: 0x04003362 RID: 13154
	private LogicPortVisualizer outputTwo;

	// Token: 0x04003363 RID: 13155
	private LogicPortVisualizer outputThree;

	// Token: 0x04003364 RID: 13156
	private LogicPortVisualizer outputFour;

	// Token: 0x04003365 RID: 13157
	private LogicEventSender outputTwoSender;

	// Token: 0x04003366 RID: 13158
	private LogicEventSender outputThreeSender;

	// Token: 0x04003367 RID: 13159
	private LogicEventSender outputFourSender;

	// Token: 0x04003368 RID: 13160
	private LogicEventHandler controlOne;

	// Token: 0x04003369 RID: 13161
	private LogicEventHandler controlTwo;

	// Token: 0x0400336A RID: 13162
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x0400336B RID: 13163
	private static readonly EventSystem.IntraObjectHandler<LogicGate> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicGate>(delegate(LogicGate component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x0400336C RID: 13164
	private static KAnimHashedString INPUT1_SYMBOL = "input1";

	// Token: 0x0400336D RID: 13165
	private static KAnimHashedString INPUT2_SYMBOL = "input2";

	// Token: 0x0400336E RID: 13166
	private static KAnimHashedString INPUT3_SYMBOL = "input3";

	// Token: 0x0400336F RID: 13167
	private static KAnimHashedString INPUT4_SYMBOL = "input4";

	// Token: 0x04003370 RID: 13168
	private static KAnimHashedString OUTPUT1_SYMBOL = "output1";

	// Token: 0x04003371 RID: 13169
	private static KAnimHashedString OUTPUT2_SYMBOL = "output2";

	// Token: 0x04003372 RID: 13170
	private static KAnimHashedString OUTPUT3_SYMBOL = "output3";

	// Token: 0x04003373 RID: 13171
	private static KAnimHashedString OUTPUT4_SYMBOL = "output4";

	// Token: 0x04003374 RID: 13172
	private static KAnimHashedString INPUT1_SYMBOL_BLM_RED = "input1_red_bloom";

	// Token: 0x04003375 RID: 13173
	private static KAnimHashedString INPUT1_SYMBOL_BLM_GRN = "input1_green_bloom";

	// Token: 0x04003376 RID: 13174
	private static KAnimHashedString INPUT2_SYMBOL_BLM_RED = "input2_red_bloom";

	// Token: 0x04003377 RID: 13175
	private static KAnimHashedString INPUT2_SYMBOL_BLM_GRN = "input2_green_bloom";

	// Token: 0x04003378 RID: 13176
	private static KAnimHashedString INPUT3_SYMBOL_BLM_RED = "input3_red_bloom";

	// Token: 0x04003379 RID: 13177
	private static KAnimHashedString INPUT3_SYMBOL_BLM_GRN = "input3_green_bloom";

	// Token: 0x0400337A RID: 13178
	private static KAnimHashedString INPUT4_SYMBOL_BLM_RED = "input4_red_bloom";

	// Token: 0x0400337B RID: 13179
	private static KAnimHashedString INPUT4_SYMBOL_BLM_GRN = "input4_green_bloom";

	// Token: 0x0400337C RID: 13180
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_RED = "output1_red_bloom";

	// Token: 0x0400337D RID: 13181
	private static KAnimHashedString OUTPUT1_SYMBOL_BLM_GRN = "output1_green_bloom";

	// Token: 0x0400337E RID: 13182
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_RED = "output2_red_bloom";

	// Token: 0x0400337F RID: 13183
	private static KAnimHashedString OUTPUT2_SYMBOL_BLM_GRN = "output2_green_bloom";

	// Token: 0x04003380 RID: 13184
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_RED = "output3_red_bloom";

	// Token: 0x04003381 RID: 13185
	private static KAnimHashedString OUTPUT3_SYMBOL_BLM_GRN = "output3_green_bloom";

	// Token: 0x04003382 RID: 13186
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_RED = "output4_red_bloom";

	// Token: 0x04003383 RID: 13187
	private static KAnimHashedString OUTPUT4_SYMBOL_BLM_GRN = "output4_green_bloom";

	// Token: 0x04003384 RID: 13188
	private static KAnimHashedString LINE_LEFT_1_SYMBOL = "line_left_1";

	// Token: 0x04003385 RID: 13189
	private static KAnimHashedString LINE_LEFT_2_SYMBOL = "line_left_2";

	// Token: 0x04003386 RID: 13190
	private static KAnimHashedString LINE_LEFT_3_SYMBOL = "line_left_3";

	// Token: 0x04003387 RID: 13191
	private static KAnimHashedString LINE_LEFT_4_SYMBOL = "line_left_4";

	// Token: 0x04003388 RID: 13192
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL = "line_right_1";

	// Token: 0x04003389 RID: 13193
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL = "line_right_2";

	// Token: 0x0400338A RID: 13194
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL = "line_right_3";

	// Token: 0x0400338B RID: 13195
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL = "line_right_4";

	// Token: 0x0400338C RID: 13196
	private static KAnimHashedString FLIPPER_1_SYMBOL = "flipper1";

	// Token: 0x0400338D RID: 13197
	private static KAnimHashedString FLIPPER_2_SYMBOL = "flipper2";

	// Token: 0x0400338E RID: 13198
	private static KAnimHashedString FLIPPER_3_SYMBOL = "flipper3";

	// Token: 0x0400338F RID: 13199
	private static KAnimHashedString INPUT_SYMBOL = "input";

	// Token: 0x04003390 RID: 13200
	private static KAnimHashedString OUTPUT_SYMBOL = "output";

	// Token: 0x04003391 RID: 13201
	private static KAnimHashedString INPUT1_SYMBOL_BLOOM = "input1_bloom";

	// Token: 0x04003392 RID: 13202
	private static KAnimHashedString INPUT2_SYMBOL_BLOOM = "input2_bloom";

	// Token: 0x04003393 RID: 13203
	private static KAnimHashedString INPUT3_SYMBOL_BLOOM = "input3_bloom";

	// Token: 0x04003394 RID: 13204
	private static KAnimHashedString INPUT4_SYMBOL_BLOOM = "input4_bloom";

	// Token: 0x04003395 RID: 13205
	private static KAnimHashedString OUTPUT1_SYMBOL_BLOOM = "output1_bloom";

	// Token: 0x04003396 RID: 13206
	private static KAnimHashedString OUTPUT2_SYMBOL_BLOOM = "output2_bloom";

	// Token: 0x04003397 RID: 13207
	private static KAnimHashedString OUTPUT3_SYMBOL_BLOOM = "output3_bloom";

	// Token: 0x04003398 RID: 13208
	private static KAnimHashedString OUTPUT4_SYMBOL_BLOOM = "output4_bloom";

	// Token: 0x04003399 RID: 13209
	private static KAnimHashedString LINE_LEFT_1_SYMBOL_BLOOM = "line_left_1_bloom";

	// Token: 0x0400339A RID: 13210
	private static KAnimHashedString LINE_LEFT_2_SYMBOL_BLOOM = "line_left_2_bloom";

	// Token: 0x0400339B RID: 13211
	private static KAnimHashedString LINE_LEFT_3_SYMBOL_BLOOM = "line_left_3_bloom";

	// Token: 0x0400339C RID: 13212
	private static KAnimHashedString LINE_LEFT_4_SYMBOL_BLOOM = "line_left_4_bloom";

	// Token: 0x0400339D RID: 13213
	private static KAnimHashedString LINE_RIGHT_1_SYMBOL_BLOOM = "line_right_1_bloom";

	// Token: 0x0400339E RID: 13214
	private static KAnimHashedString LINE_RIGHT_2_SYMBOL_BLOOM = "line_right_2_bloom";

	// Token: 0x0400339F RID: 13215
	private static KAnimHashedString LINE_RIGHT_3_SYMBOL_BLOOM = "line_right_3_bloom";

	// Token: 0x040033A0 RID: 13216
	private static KAnimHashedString LINE_RIGHT_4_SYMBOL_BLOOM = "line_right_4_bloom";

	// Token: 0x040033A1 RID: 13217
	private static KAnimHashedString FLIPPER_1_SYMBOL_BLOOM = "flipper1_bloom";

	// Token: 0x040033A2 RID: 13218
	private static KAnimHashedString FLIPPER_2_SYMBOL_BLOOM = "flipper2_bloom";

	// Token: 0x040033A3 RID: 13219
	private static KAnimHashedString FLIPPER_3_SYMBOL_BLOOM = "flipper3_bloom";

	// Token: 0x040033A4 RID: 13220
	private static KAnimHashedString INPUT_SYMBOL_BLOOM = "input_bloom";

	// Token: 0x040033A5 RID: 13221
	private static KAnimHashedString OUTPUT_SYMBOL_BLOOM = "output_bloom";

	// Token: 0x040033A6 RID: 13222
	private static KAnimHashedString[][] multiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		},
		new KAnimHashedString[]
		{
			LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
			LogicGate.FLIPPER_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.FLIPPER_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT_SYMBOL_BLOOM
		}
	};

	// Token: 0x040033A7 RID: 13223
	private static KAnimHashedString[] multiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_LEFT_3_SYMBOL,
		LogicGate.LINE_LEFT_4_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.FLIPPER_1_SYMBOL,
		LogicGate.FLIPPER_2_SYMBOL,
		LogicGate.FLIPPER_3_SYMBOL,
		LogicGate.OUTPUT_SYMBOL
	};

	// Token: 0x040033A8 RID: 13224
	private static KAnimHashedString[] multiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_3_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_4_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_1_SYMBOL_BLOOM,
		LogicGate.FLIPPER_2_SYMBOL_BLOOM,
		LogicGate.FLIPPER_3_SYMBOL_BLOOM,
		LogicGate.OUTPUT_SYMBOL_BLOOM
	};

	// Token: 0x040033A9 RID: 13225
	private static KAnimHashedString[][] demultiplexerSymbolPaths = new KAnimHashedString[][]
	{
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
			LogicGate.OUTPUT1_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
			LogicGate.OUTPUT2_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
			LogicGate.OUTPUT3_SYMBOL
		},
		new KAnimHashedString[]
		{
			LogicGate.INPUT_SYMBOL_BLOOM,
			LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
			LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM,
			LogicGate.OUTPUT4_SYMBOL
		}
	};

	// Token: 0x040033AA RID: 13226
	private static KAnimHashedString[] demultiplexerSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL,
		LogicGate.LINE_LEFT_1_SYMBOL,
		LogicGate.LINE_LEFT_2_SYMBOL,
		LogicGate.LINE_RIGHT_1_SYMBOL,
		LogicGate.LINE_RIGHT_2_SYMBOL,
		LogicGate.LINE_RIGHT_3_SYMBOL,
		LogicGate.LINE_RIGHT_4_SYMBOL
	};

	// Token: 0x040033AB RID: 13227
	private static KAnimHashedString[] demultiplexerBloomSymbols = new KAnimHashedString[]
	{
		LogicGate.INPUT_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_1_SYMBOL_BLOOM,
		LogicGate.LINE_LEFT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_1_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_2_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_3_SYMBOL_BLOOM,
		LogicGate.LINE_RIGHT_4_SYMBOL_BLOOM
	};

	// Token: 0x040033AC RID: 13228
	private static KAnimHashedString[] demultiplexerOutputSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL,
		LogicGate.OUTPUT2_SYMBOL,
		LogicGate.OUTPUT3_SYMBOL,
		LogicGate.OUTPUT4_SYMBOL
	};

	// Token: 0x040033AD RID: 13229
	private static KAnimHashedString[] demultiplexerOutputRedSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_RED,
		LogicGate.OUTPUT2_SYMBOL_BLM_RED,
		LogicGate.OUTPUT3_SYMBOL_BLM_RED,
		LogicGate.OUTPUT4_SYMBOL_BLM_RED
	};

	// Token: 0x040033AE RID: 13230
	private static KAnimHashedString[] demultiplexerOutputGreenSymbols = new KAnimHashedString[]
	{
		LogicGate.OUTPUT1_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT2_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT3_SYMBOL_BLM_GRN,
		LogicGate.OUTPUT4_SYMBOL_BLM_GRN
	};

	// Token: 0x040033AF RID: 13231
	private Color activeTintColor = new Color(0.5411765f, 0.9882353f, 0.29803923f);

	// Token: 0x040033B0 RID: 13232
	private Color inactiveTintColor = Color.red;

	// Token: 0x02000E80 RID: 3712
	public class LogicGateDescriptions
	{
		// Token: 0x040033B1 RID: 13233
		public LogicGate.LogicGateDescriptions.Description inputOne;

		// Token: 0x040033B2 RID: 13234
		public LogicGate.LogicGateDescriptions.Description inputTwo;

		// Token: 0x040033B3 RID: 13235
		public LogicGate.LogicGateDescriptions.Description inputThree;

		// Token: 0x040033B4 RID: 13236
		public LogicGate.LogicGateDescriptions.Description inputFour;

		// Token: 0x040033B5 RID: 13237
		public LogicGate.LogicGateDescriptions.Description outputOne;

		// Token: 0x040033B6 RID: 13238
		public LogicGate.LogicGateDescriptions.Description outputTwo;

		// Token: 0x040033B7 RID: 13239
		public LogicGate.LogicGateDescriptions.Description outputThree;

		// Token: 0x040033B8 RID: 13240
		public LogicGate.LogicGateDescriptions.Description outputFour;

		// Token: 0x040033B9 RID: 13241
		public LogicGate.LogicGateDescriptions.Description controlOne;

		// Token: 0x040033BA RID: 13242
		public LogicGate.LogicGateDescriptions.Description controlTwo;

		// Token: 0x02000E81 RID: 3713
		public class Description
		{
			// Token: 0x040033BB RID: 13243
			public string name;

			// Token: 0x040033BC RID: 13244
			public string active;

			// Token: 0x040033BD RID: 13245
			public string inactive;
		}
	}
}
