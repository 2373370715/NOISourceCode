using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000E93 RID: 3731
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicMemory")]
public class LogicMemory : KMonoBehaviour
{
	// Token: 0x060049F2 RID: 18930 RVA: 0x00268DB8 File Offset: 0x00266FB8
	protected override void OnSpawn()
	{
		if (LogicMemory.infoStatusItem == null)
		{
			LogicMemory.infoStatusItem = new StatusItem("StoredValue", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicMemory.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicMemory.ResolveInfoStatusItemString);
		}
		base.Subscribe<LogicMemory>(-801688580, LogicMemory.OnLogicValueChangedDelegate);
	}

	// Token: 0x060049F3 RID: 18931 RVA: 0x00268E1C File Offset: 0x0026701C
	public void OnLogicValueChanged(object data)
	{
		if (this.ports == null || base.gameObject == null || this == null)
		{
			return;
		}
		if (((LogicValueChanged)data).portID != LogicMemory.READ_PORT_ID)
		{
			int inputValue = this.ports.GetInputValue(LogicMemory.SET_PORT_ID);
			int inputValue2 = this.ports.GetInputValue(LogicMemory.RESET_PORT_ID);
			int num = this.value;
			if (LogicCircuitNetwork.IsBitActive(0, inputValue2))
			{
				num = 0;
			}
			else if (LogicCircuitNetwork.IsBitActive(0, inputValue))
			{
				num = 1;
			}
			if (num != this.value)
			{
				this.value = num;
				this.ports.SendSignal(LogicMemory.READ_PORT_ID, this.value);
				KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
				if (component != null)
				{
					component.Play(LogicCircuitNetwork.IsBitActive(0, this.value) ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
				}
			}
		}
	}

	// Token: 0x060049F4 RID: 18932 RVA: 0x00268F10 File Offset: 0x00267110
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		int outputValue = ((LogicMemory)data).ports.GetOutputValue(LogicMemory.READ_PORT_ID);
		return string.Format(BUILDINGS.PREFABS.LOGICMEMORY.STATUS_ITEM_VALUE, outputValue);
	}

	// Token: 0x0400340E RID: 13326
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x0400340F RID: 13327
	[Serialize]
	private int value;

	// Token: 0x04003410 RID: 13328
	private static StatusItem infoStatusItem;

	// Token: 0x04003411 RID: 13329
	public static readonly HashedString READ_PORT_ID = new HashedString("LogicMemoryRead");

	// Token: 0x04003412 RID: 13330
	public static readonly HashedString SET_PORT_ID = new HashedString("LogicMemorySet");

	// Token: 0x04003413 RID: 13331
	public static readonly HashedString RESET_PORT_ID = new HashedString("LogicMemoryReset");

	// Token: 0x04003414 RID: 13332
	private static readonly EventSystem.IntraObjectHandler<LogicMemory> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicMemory>(delegate(LogicMemory component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
