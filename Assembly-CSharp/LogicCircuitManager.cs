using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020014EA RID: 5354
public class LogicCircuitManager
{
	// Token: 0x06006F39 RID: 28473 RVA: 0x00300474 File Offset: 0x002FE674
	public LogicCircuitManager(UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduit_system)
	{
		this.conduitSystem = conduit_system;
		this.timeSinceBridgeRefresh = 0f;
		this.elapsedTime = 0f;
		for (int i = 0; i < 2; i++)
		{
			this.bridgeGroups[i] = new List<LogicUtilityNetworkLink>();
		}
	}

	// Token: 0x06006F3A RID: 28474 RVA: 0x000ED525 File Offset: 0x000EB725
	public void RenderEveryTick(float dt)
	{
		this.Refresh(dt);
	}

	// Token: 0x06006F3B RID: 28475 RVA: 0x003004D4 File Offset: 0x002FE6D4
	private void Refresh(float dt)
	{
		if (this.conduitSystem.IsDirty)
		{
			this.conduitSystem.Update();
			LogicCircuitNetwork.logicSoundRegister.Clear();
			this.PropagateSignals(true);
			this.elapsedTime = 0f;
		}
		else if (SpeedControlScreen.Instance != null && !SpeedControlScreen.Instance.IsPaused)
		{
			this.elapsedTime += dt;
			this.timeSinceBridgeRefresh += dt;
			while (this.elapsedTime > LogicCircuitManager.ClockTickInterval)
			{
				this.elapsedTime -= LogicCircuitManager.ClockTickInterval;
				this.PropagateSignals(false);
				if (this.onLogicTick != null)
				{
					this.onLogicTick();
				}
			}
			if (this.timeSinceBridgeRefresh > LogicCircuitManager.BridgeRefreshInterval)
			{
				this.UpdateCircuitBridgeLists();
				this.timeSinceBridgeRefresh = 0f;
			}
		}
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			this.CheckCircuitOverloaded(dt, logicCircuitNetwork.id, logicCircuitNetwork.GetBitsUsed());
		}
	}

	// Token: 0x06006F3C RID: 28476 RVA: 0x00300608 File Offset: 0x002FE808
	private void PropagateSignals(bool force_send_events)
	{
		IList<UtilityNetwork> networks = Game.Instance.logicCircuitSystem.GetNetworks();
		foreach (UtilityNetwork utilityNetwork in networks)
		{
			((LogicCircuitNetwork)utilityNetwork).UpdateLogicValue();
		}
		foreach (UtilityNetwork utilityNetwork2 in networks)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork2;
			logicCircuitNetwork.SendLogicEvents(force_send_events, logicCircuitNetwork.id);
		}
	}

	// Token: 0x06006F3D RID: 28477 RVA: 0x000ED52E File Offset: 0x000EB72E
	public LogicCircuitNetwork GetNetworkForCell(int cell)
	{
		return this.conduitSystem.GetNetworkForCell(cell) as LogicCircuitNetwork;
	}

	// Token: 0x06006F3E RID: 28478 RVA: 0x000ED541 File Offset: 0x000EB741
	public void AddVisElem(ILogicUIElement elem)
	{
		this.uiVisElements.Add(elem);
		if (this.onElemAdded != null)
		{
			this.onElemAdded(elem);
		}
	}

	// Token: 0x06006F3F RID: 28479 RVA: 0x000ED563 File Offset: 0x000EB763
	public void RemoveVisElem(ILogicUIElement elem)
	{
		if (this.onElemRemoved != null)
		{
			this.onElemRemoved(elem);
		}
		this.uiVisElements.Remove(elem);
	}

	// Token: 0x06006F40 RID: 28480 RVA: 0x000ED586 File Offset: 0x000EB786
	public List<ILogicUIElement> GetVisElements()
	{
		return this.uiVisElements;
	}

	// Token: 0x06006F41 RID: 28481 RVA: 0x000ED58E File Offset: 0x000EB78E
	public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
	{
		go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, null);
	}

	// Token: 0x06006F42 RID: 28482 RVA: 0x003006A4 File Offset: 0x002FE8A4
	private void CheckCircuitOverloaded(float dt, int id, int bits_used)
	{
		UtilityNetwork networkByID = Game.Instance.logicCircuitSystem.GetNetworkByID(id);
		if (networkByID != null)
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)networkByID;
			if (logicCircuitNetwork != null)
			{
				logicCircuitNetwork.UpdateOverloadTime(dt, bits_used);
			}
		}
	}

	// Token: 0x06006F43 RID: 28483 RVA: 0x000ED5AD File Offset: 0x000EB7AD
	public void Connect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Add(bridge);
	}

	// Token: 0x06006F44 RID: 28484 RVA: 0x000ED5C2 File Offset: 0x000EB7C2
	public void Disconnect(LogicUtilityNetworkLink bridge)
	{
		this.bridgeGroups[(int)bridge.bitDepth].Remove(bridge);
	}

	// Token: 0x06006F45 RID: 28485 RVA: 0x003006D8 File Offset: 0x002FE8D8
	private void UpdateCircuitBridgeLists()
	{
		foreach (UtilityNetwork utilityNetwork in Game.Instance.logicCircuitSystem.GetNetworks())
		{
			LogicCircuitNetwork logicCircuitNetwork = (LogicCircuitNetwork)utilityNetwork;
			if (this.updateEvenBridgeGroups)
			{
				if (logicCircuitNetwork.id % 2 == 0)
				{
					logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
				}
			}
			else if (logicCircuitNetwork.id % 2 == 1)
			{
				logicCircuitNetwork.UpdateRelevantBridges(this.bridgeGroups);
			}
		}
		this.updateEvenBridgeGroups = !this.updateEvenBridgeGroups;
	}

	// Token: 0x040053A5 RID: 21413
	public static float ClockTickInterval = 0.1f;

	// Token: 0x040053A6 RID: 21414
	private float elapsedTime;

	// Token: 0x040053A7 RID: 21415
	private UtilityNetworkManager<LogicCircuitNetwork, LogicWire> conduitSystem;

	// Token: 0x040053A8 RID: 21416
	private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();

	// Token: 0x040053A9 RID: 21417
	public static float BridgeRefreshInterval = 1f;

	// Token: 0x040053AA RID: 21418
	private List<LogicUtilityNetworkLink>[] bridgeGroups = new List<LogicUtilityNetworkLink>[2];

	// Token: 0x040053AB RID: 21419
	private bool updateEvenBridgeGroups;

	// Token: 0x040053AC RID: 21420
	private float timeSinceBridgeRefresh;

	// Token: 0x040053AD RID: 21421
	public System.Action onLogicTick;

	// Token: 0x040053AE RID: 21422
	public Action<ILogicUIElement> onElemAdded;

	// Token: 0x040053AF RID: 21423
	public Action<ILogicUIElement> onElemRemoved;

	// Token: 0x020014EB RID: 5355
	private struct Signal
	{
		// Token: 0x06006F47 RID: 28487 RVA: 0x000ED5EE File Offset: 0x000EB7EE
		public Signal(int cell, int value)
		{
			this.cell = cell;
			this.value = value;
		}

		// Token: 0x040053B0 RID: 21424
		public int cell;

		// Token: 0x040053B1 RID: 21425
		public int value;
	}
}
