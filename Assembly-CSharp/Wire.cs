using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001088 RID: 4232
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Wire")]
public class Wire : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IWattageRating, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
	// Token: 0x060055EB RID: 21995 RVA: 0x0028E244 File Offset: 0x0028C444
	public static float GetMaxWattageAsFloat(Wire.WattageRating rating)
	{
		switch (rating)
		{
		case Wire.WattageRating.Max500:
			return 500f;
		case Wire.WattageRating.Max1000:
			return 1000f;
		case Wire.WattageRating.Max2000:
			return 2000f;
		case Wire.WattageRating.Max20000:
			return 20000f;
		case Wire.WattageRating.Max50000:
			return 50000f;
		default:
			return 0f;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x060055EC RID: 21996 RVA: 0x0028E290 File Offset: 0x0028C490
	public bool IsConnected
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			return Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) is ElectricalUtilityNetwork;
		}
	}

	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x060055ED RID: 21997 RVA: 0x0028E2C8 File Offset: 0x0028C4C8
	public ushort NetworkID
	{
		get
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ElectricalUtilityNetwork electricalUtilityNetwork = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell) as ElectricalUtilityNetwork;
			if (electricalUtilityNetwork == null)
			{
				return ushort.MaxValue;
			}
			return (ushort)electricalUtilityNetwork.id;
		}
	}

	// Token: 0x060055EE RID: 21998 RVA: 0x0028E30C File Offset: 0x0028C50C
	protected override void OnSpawn()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Game.Instance.electricalConduitSystem.AddToNetworks(cell, this, false);
		this.InitializeSwitchState();
		base.Subscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate);
		base.Subscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireCircuitStatus, this);
		base.GetComponent<KSelectable>().AddStatusItem(Wire.WireMaxWattageStatus, this);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(Wire.OutlineSymbol, false);
	}

	// Token: 0x060055EF RID: 21999 RVA: 0x0028E39C File Offset: 0x0028C59C
	protected override void OnCleanUp()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			Game.Instance.electricalConduitSystem.RemoveFromNetworks(cell, this, false);
		}
		base.Unsubscribe<Wire>(774203113, Wire.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Wire>(-1735440190, Wire.OnBuildingFullyRepairedDelegate, false);
		base.OnCleanUp();
	}

	// Token: 0x060055F0 RID: 22000 RVA: 0x0028E428 File Offset: 0x0028C628
	private void InitializeSwitchState()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		bool flag = false;
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			CircuitSwitch component = gameObject.GetComponent<CircuitSwitch>();
			if (component != null)
			{
				flag = true;
				component.AttachWire(this);
			}
		}
		if (!flag)
		{
			this.Connect();
		}
	}

	// Token: 0x060055F1 RID: 22001 RVA: 0x0028E484 File Offset: 0x0028C684
	public UtilityConnections GetWireConnections()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		return Game.Instance.electricalConduitSystem.GetConnections(cell, true);
	}

	// Token: 0x060055F2 RID: 22002 RVA: 0x0028E4B4 File Offset: 0x0028C6B4
	public string GetWireConnectionsString()
	{
		UtilityConnections wireConnections = this.GetWireConnections();
		return Game.Instance.electricalConduitSystem.GetVisualizerString(wireConnections);
	}

	// Token: 0x060055F3 RID: 22003 RVA: 0x000DC813 File Offset: 0x000DAA13
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x060055F4 RID: 22004 RVA: 0x000DC81B File Offset: 0x000DAA1B
	private void OnBuildingFullyRepaired(object data)
	{
		this.InitializeSwitchState();
	}

	// Token: 0x060055F5 RID: 22005 RVA: 0x0028E4D8 File Offset: 0x0028C6D8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<KPrefabID>().AddTag(GameTags.Wires, false);
		if (Wire.WireCircuitStatus == null)
		{
			Wire.WireCircuitStatus = new StatusItem("WireCircuitStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsUsedByCircuit = circuitManager.GetWattsUsedByCircuit(circuitID);
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				string wireLoadColor = GameUtil.GetWireLoadColor(wattsUsedByCircuit, maxWattageAsFloat, wattsNeededWhenActive);
				string text = (wattsUsedByCircuit < 0f) ? "?" : GameUtil.GetFormattedWattage(wattsUsedByCircuit, unit, true);
				str = str.Replace("{CurrentLoadAndColor}", (wireLoadColor == Color.white.ToHexString()) ? text : string.Concat(new string[]
				{
					"<color=#",
					wireLoadColor,
					">",
					text,
					"</color>"
				}));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				str = str.Replace("{WireType}", this.GetProperName());
				return str;
			});
		}
		if (Wire.WireMaxWattageStatus == null)
		{
			Wire.WireMaxWattageStatus = new StatusItem("WireMaxWattageStatus", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null).SetResolveStringCallback(delegate(string str, object data)
			{
				Wire wire = (Wire)data;
				GameUtil.WattageFormatterUnit unit = GameUtil.WattageFormatterUnit.Watts;
				if (wire.MaxWattageRating >= Wire.WattageRating.Max20000)
				{
					unit = GameUtil.WattageFormatterUnit.Kilowatts;
				}
				int cell = Grid.PosToCell(wire.transform.GetPosition());
				CircuitManager circuitManager = Game.Instance.circuitManager;
				ushort circuitID = circuitManager.GetCircuitID(cell);
				float wattsNeededWhenActive = circuitManager.GetWattsNeededWhenActive(circuitID);
				float maxWattageAsFloat = Wire.GetMaxWattageAsFloat(wire.MaxWattageRating);
				str = str.Replace("{TotalPotentialLoadAndColor}", (wattsNeededWhenActive > maxWattageAsFloat) ? string.Concat(new string[]
				{
					"<color=#",
					new Color(0.9843137f, 0.6901961f, 0.23137255f).ToHexString(),
					">",
					GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true),
					"</color>"
				}) : GameUtil.GetFormattedWattage(wattsNeededWhenActive, unit, true));
				str = str.Replace("{MaxLoad}", GameUtil.GetFormattedWattage(maxWattageAsFloat, unit, true));
				return str;
			});
		}
	}

	// Token: 0x060055F6 RID: 22006 RVA: 0x000DC823 File Offset: 0x000DAA23
	public Wire.WattageRating GetMaxWattageRating()
	{
		return this.MaxWattageRating;
	}

	// Token: 0x060055F7 RID: 22007 RVA: 0x000DC82B File Offset: 0x000DAA2B
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x060055F8 RID: 22008 RVA: 0x0028E590 File Offset: 0x0028C790
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x060055F9 RID: 22009 RVA: 0x0028E5D8 File Offset: 0x0028C7D8
	public void Disconnect()
	{
		this.disconnected = true;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, null);
		Game.Instance.electricalConduitSystem.ForceRebuildNetworks();
	}

	// Token: 0x060055FA RID: 22010 RVA: 0x000DC833 File Offset: 0x000DAA33
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060055FB RID: 22011 RVA: 0x000DC849 File Offset: 0x000DAA49
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

	// Token: 0x060055FC RID: 22012 RVA: 0x000DC858 File Offset: 0x000DAA58
	public IUtilityNetworkMgr GetNetworkManager()
	{
		return Game.Instance.electricalConduitSystem;
	}

	// Token: 0x060055FD RID: 22013 RVA: 0x0028E628 File Offset: 0x0028C828
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x060055FE RID: 22014 RVA: 0x0028E664 File Offset: 0x0028C864
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		UtilityNetwork networkForCell = Game.Instance.electricalConduitSystem.GetNetworkForCell(cell);
		return networks.Contains(networkForCell);
	}

	// Token: 0x060055FF RID: 22015 RVA: 0x000C1501 File Offset: 0x000BF701
	public int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x04003CD0 RID: 15568
	[SerializeField]
	public Wire.WattageRating MaxWattageRating;

	// Token: 0x04003CD1 RID: 15569
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04003CD2 RID: 15570
	public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");

	// Token: 0x04003CD3 RID: 15571
	public float circuitOverloadTime;

	// Token: 0x04003CD4 RID: 15572
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04003CD5 RID: 15573
	private static readonly EventSystem.IntraObjectHandler<Wire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Wire>(delegate(Wire component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});

	// Token: 0x04003CD6 RID: 15574
	private static StatusItem WireCircuitStatus = null;

	// Token: 0x04003CD7 RID: 15575
	private static StatusItem WireMaxWattageStatus = null;

	// Token: 0x04003CD8 RID: 15576
	private System.Action firstFrameCallback;

	// Token: 0x02001089 RID: 4233
	public enum WattageRating
	{
		// Token: 0x04003CDA RID: 15578
		Max500,
		// Token: 0x04003CDB RID: 15579
		Max1000,
		// Token: 0x04003CDC RID: 15580
		Max2000,
		// Token: 0x04003CDD RID: 15581
		Max20000,
		// Token: 0x04003CDE RID: 15582
		Max50000,
		// Token: 0x04003CDF RID: 15583
		NumRatings
	}
}
