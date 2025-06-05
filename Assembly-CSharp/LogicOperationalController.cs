using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E95 RID: 3733
[AddComponentMenu("KMonoBehaviour/scripts/LogicOperationalController")]
public class LogicOperationalController : KMonoBehaviour
{
	// Token: 0x060049FA RID: 18938 RVA: 0x00268F9C File Offset: 0x0026719C
	public static List<LogicPorts.Port> CreateSingleInputPortList(CellOffset offset)
	{
		return new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, offset, UI.LOGIC_PORTS.CONTROL_OPERATIONAL, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_ACTIVE, UI.LOGIC_PORTS.CONTROL_OPERATIONAL_INACTIVE, false, false)
		};
	}

	// Token: 0x060049FB RID: 18939 RVA: 0x00268FE0 File Offset: 0x002671E0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<LogicOperationalController>(-801688580, LogicOperationalController.OnLogicValueChangedDelegate);
		if (LogicOperationalController.infoStatusItem == null)
		{
			LogicOperationalController.infoStatusItem = new StatusItem("LogicOperationalInfo", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			LogicOperationalController.infoStatusItem.resolveStringCallback = new Func<string, object, string>(LogicOperationalController.ResolveInfoStatusItemString);
		}
		this.CheckWireState();
	}

	// Token: 0x060049FC RID: 18940 RVA: 0x00269050 File Offset: 0x00267250
	private LogicCircuitNetwork GetNetwork()
	{
		int portCell = base.GetComponent<LogicPorts>().GetPortCell(LogicOperationalController.PORT_ID);
		return Game.Instance.logicCircuitManager.GetNetworkForCell(portCell);
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x00269080 File Offset: 0x00267280
	private LogicCircuitNetwork CheckWireState()
	{
		LogicCircuitNetwork network = this.GetNetwork();
		int value = (network != null) ? network.OutputValue : this.unNetworkedValue;
		this.operational.SetFlag(LogicOperationalController.LogicOperationalFlag, LogicCircuitNetwork.IsBitActive(0, value));
		return network;
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x000D4654 File Offset: 0x000D2854
	private static string ResolveInfoStatusItemString(string format_str, object data)
	{
		return ((LogicOperationalController)data).operational.GetFlag(LogicOperationalController.LogicOperationalFlag) ? BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_ENABLED : BUILDING.STATUSITEMS.LOGIC.LOGIC_CONTROLLED_DISABLED;
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x002690C0 File Offset: 0x002672C0
	private void OnLogicValueChanged(object data)
	{
		if (((LogicValueChanged)data).portID == LogicOperationalController.PORT_ID)
		{
			LogicCircuitNetwork logicCircuitNetwork = this.CheckWireState();
			base.GetComponent<KSelectable>().ToggleStatusItem(LogicOperationalController.infoStatusItem, logicCircuitNetwork != null, this);
		}
	}

	// Token: 0x04003416 RID: 13334
	public static readonly HashedString PORT_ID = "LogicOperational";

	// Token: 0x04003417 RID: 13335
	public int unNetworkedValue = 1;

	// Token: 0x04003418 RID: 13336
	public static readonly Operational.Flag LogicOperationalFlag = new Operational.Flag("LogicOperational", Operational.Flag.Type.Requirement);

	// Token: 0x04003419 RID: 13337
	private static StatusItem infoStatusItem;

	// Token: 0x0400341A RID: 13338
	[MyCmpGet]
	public Operational operational;

	// Token: 0x0400341B RID: 13339
	private static readonly EventSystem.IntraObjectHandler<LogicOperationalController> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicOperationalController>(delegate(LogicOperationalController component, object data)
	{
		component.OnLogicValueChanged(data);
	});
}
