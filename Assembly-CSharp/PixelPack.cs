using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000F4E RID: 3918
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/PixelPack")]
public class PixelPack : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06004E7D RID: 20093 RVA: 0x000D75EF File Offset: 0x000D57EF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<PixelPack>(-905833192, PixelPack.OnCopySettingsDelegate);
	}

	// Token: 0x06004E7E RID: 20094 RVA: 0x002767B0 File Offset: 0x002749B0
	private void OnCopySettings(object data)
	{
		PixelPack component = ((GameObject)data).GetComponent<PixelPack>();
		if (component != null)
		{
			for (int i = 0; i < component.colorSettings.Count; i++)
			{
				this.colorSettings[i] = component.colorSettings[i];
			}
		}
		this.UpdateColors();
	}

	// Token: 0x06004E7F RID: 20095 RVA: 0x00276808 File Offset: 0x00274A08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.Subscribe<PixelPack>(-801688580, PixelPack.OnLogicValueChangedDelegate);
		base.Subscribe<PixelPack>(-592767678, PixelPack.OnOperationalChangedDelegate);
		if (this.colorSettings == null)
		{
			PixelPack.ColorPair item = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item2 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item3 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			PixelPack.ColorPair item4 = new PixelPack.ColorPair
			{
				activeColor = this.defaultActive,
				standbyColor = this.defaultStandby
			};
			this.colorSettings = new List<PixelPack.ColorPair>();
			this.colorSettings.Add(item);
			this.colorSettings.Add(item2);
			this.colorSettings.Add(item3);
			this.colorSettings.Add(item4);
		}
	}

	// Token: 0x06004E80 RID: 20096 RVA: 0x00276924 File Offset: 0x00274B24
	private void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID == PixelPack.PORT_ID)
		{
			this.logicValue = logicValueChanged.newValue;
			this.UpdateColors();
		}
	}

	// Token: 0x06004E81 RID: 20097 RVA: 0x0027695C File Offset: 0x00274B5C
	private void OnOperationalChanged(object data)
	{
		if (this.operational.IsOperational)
		{
			this.UpdateColors();
			this.animController.Play(PixelPack.ON_ANIMS, KAnim.PlayMode.Once);
		}
		else
		{
			this.animController.Play(PixelPack.OFF_ANIMS, KAnim.PlayMode.Once);
		}
		this.operational.SetActive(this.operational.IsOperational, false);
	}

	// Token: 0x06004E82 RID: 20098 RVA: 0x002769B8 File Offset: 0x00274BB8
	public void UpdateColors()
	{
		if (this.operational.IsOperational)
		{
			LogicPorts component = base.GetComponent<LogicPorts>();
			if (component != null)
			{
				LogicWire.BitDepth connectedWireBitDepth = component.GetConnectedWireBitDepth(PixelPack.PORT_ID);
				if (connectedWireBitDepth == LogicWire.BitDepth.FourBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(1, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(2, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(3, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
					return;
				}
				if (connectedWireBitDepth == LogicWire.BitDepth.OneBit)
				{
					this.animController.SetSymbolTint(PixelPack.SYMBOL_ONE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[0].activeColor : this.colorSettings[0].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_TWO_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[1].activeColor : this.colorSettings[1].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_THREE_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[2].activeColor : this.colorSettings[2].standbyColor);
					this.animController.SetSymbolTint(PixelPack.SYMBOL_FOUR_NAME, LogicCircuitNetwork.IsBitActive(0, this.logicValue) ? this.colorSettings[3].activeColor : this.colorSettings[3].standbyColor);
				}
			}
		}
	}

	// Token: 0x04003715 RID: 14101
	protected KBatchedAnimController animController;

	// Token: 0x04003716 RID: 14102
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04003717 RID: 14103
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003718 RID: 14104
	public static readonly HashedString PORT_ID = new HashedString("PixelPackInput");

	// Token: 0x04003719 RID: 14105
	public static readonly HashedString SYMBOL_ONE_NAME = "screen1";

	// Token: 0x0400371A RID: 14106
	public static readonly HashedString SYMBOL_TWO_NAME = "screen2";

	// Token: 0x0400371B RID: 14107
	public static readonly HashedString SYMBOL_THREE_NAME = "screen3";

	// Token: 0x0400371C RID: 14108
	public static readonly HashedString SYMBOL_FOUR_NAME = "screen4";

	// Token: 0x0400371D RID: 14109
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400371E RID: 14110
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400371F RID: 14111
	private static readonly EventSystem.IntraObjectHandler<PixelPack> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PixelPack>(delegate(PixelPack component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04003720 RID: 14112
	public int logicValue;

	// Token: 0x04003721 RID: 14113
	[Serialize]
	public List<PixelPack.ColorPair> colorSettings;

	// Token: 0x04003722 RID: 14114
	private Color defaultActive = new Color(0.34509805f, 0.84705883f, 0.32941177f);

	// Token: 0x04003723 RID: 14115
	private Color defaultStandby = new Color(0.972549f, 0.47058824f, 0.34509805f);

	// Token: 0x04003724 RID: 14116
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04003725 RID: 14117
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"off_pre",
		"off"
	};

	// Token: 0x02000F4F RID: 3919
	public struct ColorPair
	{
		// Token: 0x04003726 RID: 14118
		public Color activeColor;

		// Token: 0x04003727 RID: 14119
		public Color standbyColor;
	}
}
