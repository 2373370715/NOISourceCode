using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E9C RID: 3740
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicRibbonReader")]
public class LogicRibbonReader : KMonoBehaviour, ILogicRibbonBitSelector, IRender200ms
{
	// Token: 0x06004A4A RID: 19018 RVA: 0x002695C0 File Offset: 0x002677C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicRibbonReader>(-801688580, LogicRibbonReader.OnLogicValueChangedDelegate);
		this.ports = base.GetComponent<LogicPorts>();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.kbac.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004A4B RID: 19019 RVA: 0x000D49AF File Offset: 0x000D2BAF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicRibbonReader>(-905833192, LogicRibbonReader.OnCopySettingsDelegate);
	}

	// Token: 0x06004A4C RID: 19020 RVA: 0x0026961C File Offset: 0x0026781C
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicRibbonReader.INPUT_PORT_ID)
		{
			return;
		}
		this.currentValue = logicValueChanged.newValue;
		this.UpdateLogicCircuit();
		this.UpdateVisuals();
	}

	// Token: 0x06004A4D RID: 19021 RVA: 0x0026965C File Offset: 0x0026785C
	private void OnCopySettings(object data)
	{
		LogicRibbonReader component = ((GameObject)data).GetComponent<LogicRibbonReader>();
		if (component != null)
		{
			this.SetBitSelection(component.selectedBit);
		}
	}

	// Token: 0x06004A4E RID: 19022 RVA: 0x0026968C File Offset: 0x0026788C
	private void UpdateLogicCircuit()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		LogicWire.BitDepth bitDepth = LogicWire.BitDepth.NumRatings;
		int portCell = component.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
		GameObject gameObject = Grid.Objects[portCell, 31];
		if (gameObject != null)
		{
			LogicWire component2 = gameObject.GetComponent<LogicWire>();
			if (component2 != null)
			{
				bitDepth = component2.MaxBitDepth;
			}
		}
		if (bitDepth != LogicWire.BitDepth.OneBit && bitDepth == LogicWire.BitDepth.FourBit)
		{
			int num = this.currentValue >> this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, num);
		}
		else
		{
			int num = this.currentValue & 1 << this.selectedBit;
			component.SendSignal(LogicRibbonReader.OUTPUT_PORT_ID, (num > 0) ? 1 : 0);
		}
		this.UpdateVisuals();
	}

	// Token: 0x06004A4F RID: 19023 RVA: 0x000D49C8 File Offset: 0x000D2BC8
	public void Render200ms(float dt)
	{
		this.UpdateVisuals();
	}

	// Token: 0x06004A50 RID: 19024 RVA: 0x000D49D0 File Offset: 0x000D2BD0
	public void SetBitSelection(int bit)
	{
		this.selectedBit = bit;
		this.UpdateLogicCircuit();
	}

	// Token: 0x06004A51 RID: 19025 RVA: 0x000D49DF File Offset: 0x000D2BDF
	public int GetBitSelection()
	{
		return this.selectedBit;
	}

	// Token: 0x06004A52 RID: 19026 RVA: 0x000D49E7 File Offset: 0x000D2BE7
	public int GetBitDepth()
	{
		return this.bitDepth;
	}

	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06004A53 RID: 19027 RVA: 0x000D49EF File Offset: 0x000D2BEF
	public string SideScreenTitle
	{
		get
		{
			return "STRINGS.UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_TITLE";
		}
	}

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06004A54 RID: 19028 RVA: 0x000D49F6 File Offset: 0x000D2BF6
	public string SideScreenDescription
	{
		get
		{
			return UI.UISIDESCREENS.LOGICBITSELECTORSIDESCREEN.RIBBON_READER_DESCRIPTION;
		}
	}

	// Token: 0x06004A55 RID: 19029 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool SideScreenDisplayWriterDescription()
	{
		return false;
	}

	// Token: 0x06004A56 RID: 19030 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SideScreenDisplayReaderDescription()
	{
		return true;
	}

	// Token: 0x06004A57 RID: 19031 RVA: 0x00269738 File Offset: 0x00267938
	public bool IsBitActive(int bit)
	{
		LogicCircuitNetwork logicCircuitNetwork = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			logicCircuitNetwork = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return logicCircuitNetwork != null && logicCircuitNetwork.IsBitActive(bit);
	}

	// Token: 0x06004A58 RID: 19032 RVA: 0x00269784 File Offset: 0x00267984
	public int GetInputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetInputValue(LogicRibbonReader.INPUT_PORT_ID);
	}

	// Token: 0x06004A59 RID: 19033 RVA: 0x002697B0 File Offset: 0x002679B0
	public int GetOutputValue()
	{
		LogicPorts component = base.GetComponent<LogicPorts>();
		if (!(component != null))
		{
			return 0;
		}
		return component.GetOutputValue(LogicRibbonReader.OUTPUT_PORT_ID);
	}

	// Token: 0x06004A5A RID: 19034 RVA: 0x002697DC File Offset: 0x002679DC
	private LogicCircuitNetwork GetInputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.INPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06004A5B RID: 19035 RVA: 0x0026981C File Offset: 0x00267A1C
	private LogicCircuitNetwork GetOutputNetwork()
	{
		LogicCircuitNetwork result = null;
		if (this.ports != null)
		{
			int portCell = this.ports.GetPortCell(LogicRibbonReader.OUTPUT_PORT_ID);
			result = Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
		}
		return result;
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0026985C File Offset: 0x00267A5C
	public void UpdateVisuals()
	{
		bool inputNetwork = this.GetInputNetwork() != null;
		LogicCircuitNetwork outputNetwork = this.GetOutputNetwork();
		this.GetInputValue();
		int num = 0;
		if (inputNetwork)
		{
			num += 4;
			this.kbac.SetSymbolTint(this.BIT_ONE_SYMBOL, this.IsBitActive(0) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_TWO_SYMBOL, this.IsBitActive(1) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_THREE_SYMBOL, this.IsBitActive(2) ? this.colorOn : this.colorOff);
			this.kbac.SetSymbolTint(this.BIT_FOUR_SYMBOL, this.IsBitActive(3) ? this.colorOn : this.colorOff);
		}
		if (outputNetwork != null)
		{
			num++;
			this.kbac.SetSymbolTint(this.OUTPUT_SYMBOL, LogicCircuitNetwork.IsBitActive(0, this.GetOutputValue()) ? this.colorOn : this.colorOff);
		}
		this.kbac.Play(num.ToString() + "_" + (this.GetBitSelection() + 1).ToString(), KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x04003436 RID: 13366
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicRibbonReaderInput");

	// Token: 0x04003437 RID: 13367
	public static readonly HashedString OUTPUT_PORT_ID = new HashedString("LogicRibbonReaderOutput");

	// Token: 0x04003438 RID: 13368
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04003439 RID: 13369
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x0400343A RID: 13370
	private static readonly EventSystem.IntraObjectHandler<LogicRibbonReader> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicRibbonReader>(delegate(LogicRibbonReader component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400343B RID: 13371
	private KAnimHashedString BIT_ONE_SYMBOL = "bit1_bloom";

	// Token: 0x0400343C RID: 13372
	private KAnimHashedString BIT_TWO_SYMBOL = "bit2_bloom";

	// Token: 0x0400343D RID: 13373
	private KAnimHashedString BIT_THREE_SYMBOL = "bit3_bloom";

	// Token: 0x0400343E RID: 13374
	private KAnimHashedString BIT_FOUR_SYMBOL = "bit4_bloom";

	// Token: 0x0400343F RID: 13375
	private KAnimHashedString OUTPUT_SYMBOL = "output_light_bloom";

	// Token: 0x04003440 RID: 13376
	private KBatchedAnimController kbac;

	// Token: 0x04003441 RID: 13377
	private Color colorOn = new Color(0.34117648f, 0.7254902f, 0.36862746f);

	// Token: 0x04003442 RID: 13378
	private Color colorOff = new Color(0.9529412f, 0.2901961f, 0.2784314f);

	// Token: 0x04003443 RID: 13379
	private LogicPorts ports;

	// Token: 0x04003444 RID: 13380
	public int bitDepth = 4;

	// Token: 0x04003445 RID: 13381
	[Serialize]
	public int selectedBit;

	// Token: 0x04003446 RID: 13382
	[Serialize]
	private int currentValue;
}
