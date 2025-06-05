using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E9E RID: 3742
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonWriter")]
public class LogicRibbonWriter : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06004A63 RID: 19043 RVA: 0x000D4A20 File Offset: 0x000D2C20
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonWriter>(-905833192, LogicRibbonWriter.OnCopySettingsDelegate);
	}

	// Token: 0x06004A64 RID: 19044 RVA: 0x00269A98 File Offset: 0x00267C98
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonWriter>(-801688580, LogicRibbonWriter.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004A65 RID: 19045 RVA: 0x00269AF4 File Offset: 0x00267CF4
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonWriter.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06004A66 RID: 19046 RVA: 0x00269B34 File Offset: 0x00267D34
	private void OnCopySettings(object data)
	{
		LogicRibbonWriter component = ((GameObject)data).GetComponent<LogicRibbonWriter>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06004A67 RID: 19047 RVA: 0x00269B64 File Offset: 0x00267D64
	private void UpdateLogicCircuit()
	{
		int new_value = this.currentValue << this.selectedBit;
		base.GetComponent<LogicPorts>().SendSignal(LogicRibbonWriter.OUTPUT_PORT_ID, new_value);
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x000D4A39 File Offset: 0x000D2C39
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06004A69 RID: 19049 RVA: 0x00269B94 File Offset: 0x00267D94
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06004A6A RID: 19050 RVA: 0x00269BD4 File Offset: 0x00267DD4
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x000D4A41 File Offset: 0x000D2C41
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A6C RID: 19052 RVA: 0x000D4A50 File Offset: 0x000D2C50
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x000D4A58 File Offset: 0x000D2C58
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170003EC RID: 1004
	// (get) Token: 0x06004A6E RID: 19054 RVA: 0x000D4A60 File Offset: 0x000D2C60
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_TITLE";
		}
	}

	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x06004A6F RID: 19055 RVA: 0x000D4A67 File Offset: 0x000D2C67
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_WRITER_DESCRIPTION;
		}
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SideScreenDisplayWriterDescription()
	{
		return true;
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool SideScreenDisplayReaderDescription()
	{
		return false;
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x00269C14 File Offset: 0x00267E14
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonWriter.OUTPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x00269C60 File Offset: 0x00267E60
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonWriter.INPUT_PORT_ID);
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x00269C8C File Offset: 0x00267E8C
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonWriter.OUTPUT_PORT_ID);
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x00269CB8 File Offset: 0x00267EB8
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		int num = 0;
		if (inputNetwork)
		{
			num++;
			this.kbac.SetSymbolTint(LogicRibbonWriter.INPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetInputValue()) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num += 4;
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(LogicRibbonWriter.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003448 RID: 13384
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonWriterInput");

	// Token: 0x04003449 RID: 13385
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonWriterOutput");

	// Token: 0x0400344A RID: 13386
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400344B RID: 13387
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400344C RID: 13388
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonWriter> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonWriter>(delegate(LogicRibbonWriter component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400344D RID: 13389
	private LogicPorts ports;

	// Token: 0x0400344E RID: 13390
	public int bitDepth = 4;

	// Token: 0x0400344F RID: 13391
	[Serialize]
	public int selectedBit;

	// Token: 0x04003450 RID: 13392
	[Serialize]
	private int currentValue;

	// Token: 0x04003451 RID: 13393
	private KBatchedAnimController kbac;

	// Token: 0x04003452 RID: 13394
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04003453 RID: 13395
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04003454 RID: 13396
	private static KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x04003455 RID: 13397
	private static KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x04003456 RID: 13398
	private static KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x04003457 RID: 13399
	private static KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x04003458 RID: 13400
	private static KAnimHashedString INPUT_SYMBOL = "input_light_bloom";
}
