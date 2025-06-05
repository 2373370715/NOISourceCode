using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000EAC RID: 3756
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/LogicWire")]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IBitRating, IDisconnectable
{
	// Token: 0x06004AF9 RID: 19193 RVA: 0x000D4F3F File Offset: 0x000D313F
	public static int GetBitDepthAsInt(LogicWire.BitDepth rating)
	{
		if (rating == LogicWire.BitDepth.OneBit)
		{
			return 1;
		}
		if (rating != LogicWire.BitDepth.FourBit)
		{
			return 0;
		}
		return 4;
	}

	// Token: 0x06004AFA RID: 19194 RVA: 0x0026A794 File Offset: 0x00268994
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.logicCircuitSystem.AddToNetworks(cell, this, false);
		base.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
		base.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
		this.Connect();
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
	}

	// Token: 0x06004AFB RID: 19195 RVA: 0x0026A804 File Offset: 0x00268A04
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x06004AFC RID: 19196 RVA: 0x0026A890 File Offset: 0x00268A90
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.logicCircuitSystem.GetNetworkForCell(cell) is LogicCircuitNetwork;
		}
	}

	// Token: 0x06004AFD RID: 19197 RVA: 0x000D4F4F File Offset: 0x000D314F
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x06004AFE RID: 19198 RVA: 0x0026A8C8 File Offset: 0x00268AC8
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x06004AFF RID: 19199 RVA: 0x0026A910 File Offset: 0x00268B10
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
	}

	// Token: 0x06004B00 RID: 19200 RVA: 0x0026A960 File Offset: 0x00268B60
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.logicCircuitSystem.GetConnections(cell, true);
	}

	// Token: 0x06004B01 RID: 19201 RVA: 0x0026A990 File Offset: 0x00268B90
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.logicCircuitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x06004B02 RID: 19202 RVA: 0x000D4F57 File Offset: 0x000D3157
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06004B03 RID: 19203 RVA: 0x000D4F5F File Offset: 0x000D315F
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x06004B04 RID: 19204 RVA: 0x000D4F68 File Offset: 0x000D3168
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06004B05 RID: 19205 RVA: 0x000D4F7E File Offset: 0x000D317E
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06004B06 RID: 19206 RVA: 0x000D4F8D File Offset: 0x000D318D
	public LogicWire.BitDepth GetMaxBitRating()
	{
		return this.MaxBitDepth;
	}

	// Token: 0x06004B07 RID: 19207 RVA: 0x000D4F95 File Offset: 0x000D3195
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.logicCircuitSystem;
	}

	// Token: 0x06004B08 RID: 19208 RVA: 0x0026A9B4 File Offset: 0x00268BB4
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004B09 RID: 19209 RVA: 0x0026A9F0 File Offset: 0x00268BF0
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x06004B0A RID: 19210 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x0400348A RID: 13450
	[SerializeField]
	public LogicWire.BitDepth MaxBitDepth;

	// Token: 0x0400348B RID: 13451
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x0400348C RID: 13452
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x0400348D RID: 13453
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x0400348E RID: 13454
	private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>(delegate(LogicWire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x0400348F RID: 13455
	private System.Action firstFrameCallback;

	// Token: 0x02000EAD RID: 3757
	public enum BitDepth
	{
		// Token: 0x04003491 RID: 13457
		OneBit,
		// Token: 0x04003492 RID: 13458
		FourBit,
		// Token: 0x04003493 RID: 13459
		NumRatings
	}
}
