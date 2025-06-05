using System;
using System.Collections;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000D31 RID: 3377
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Conduit")]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
	// Token: 0x06004147 RID: 16711 RVA: 0x000CEB6C File Offset: 0x000CCD6C
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x06004148 RID: 16712 RVA: 0x000CEB82 File Offset: 0x000CCD82
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

	// Token: 0x06004149 RID: 16713 RVA: 0x0024C0D4 File Offset: 0x0024A2D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
		base.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
		base.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
		base.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
	}

	// Token: 0x0600414A RID: 16714 RVA: 0x000CEB91 File Offset: 0x000CCD91
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
		base.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x0600414B RID: 16715 RVA: 0x0024C12C File Offset: 0x0024A32C
	protected virtual void OnStructureTemperatureRegistered(object data)
	{
		int cell = Grid.PosToCell(this);
		this.GetNetworkManager().AddToNetworks(cell, this, false);
		this.Connect();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, this);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
	}

	// Token: 0x0600414C RID: 16716 RVA: 0x0024C1C4 File Offset: 0x0024A3C4
	protected override void OnCleanUp()
	{
		base.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
		BuildingDef def = base.GetComponent<Building>().Def;
		if (def != null && def.ThermalConductivity != 1f)
		{
			this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(base.transform.GetPosition()), def.ThermalConductivity);
		}
		int cell = Grid.PosToCell(base.transform.GetPosition());
		this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
		BuildingComplete component = base.GetComponent<BuildingComplete>();
		if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)component.Def.ReplacementLayer] == null)
		{
			this.GetNetworkManager().RemoveFromNetworks(cell, this, false);
			this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
		}
		base.OnCleanUp();
	}

	// Token: 0x0600414D RID: 16717 RVA: 0x000CEBBB File Offset: 0x000CCDBB
	protected ConduitFlowVisualizer GetFlowVisualizer()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidFlowVisualizer;
		}
		return Game.Instance.gasFlowVisualizer;
	}

	// Token: 0x0600414E RID: 16718 RVA: 0x000CEBDB File Offset: 0x000CCDDB
	public IUtilityNetworkMgr GetNetworkManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x0600414F RID: 16719 RVA: 0x000CEBFB File Offset: 0x000CCDFB
	public ConduitFlow GetFlowManager()
	{
		if (this.type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06004150 RID: 16720 RVA: 0x000CEC1B File Offset: 0x000CCE1B
	public static ConduitFlow GetFlowManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitFlow;
		}
		return Game.Instance.gasConduitFlow;
	}

	// Token: 0x06004151 RID: 16721 RVA: 0x000CEC36 File Offset: 0x000CCE36
	public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
	{
		if (type != ConduitType.Gas)
		{
			return Game.Instance.liquidConduitSystem;
		}
		return Game.Instance.gasConduitSystem;
	}

	// Token: 0x06004152 RID: 16722 RVA: 0x0024C2B8 File Offset: 0x0024A4B8
	public virtual void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x06004153 RID: 16723 RVA: 0x0024C2E4 File Offset: 0x0024A4E4
	public virtual bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell(this));
		return networks.Contains(networkForCell);
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x000C1501 File Offset: 0x000BF701
	public virtual int GetNetworkCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x0024C30C File Offset: 0x0024A50C
	private void OnHighlighted(object data)
	{
		int highlightedCell = ((bool)data) ? Grid.PosToCell(base.transform.GetPosition()) : -1;
		this.GetFlowVisualizer().SetHighlightedCell(highlightedCell);
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x0024C344 File Offset: 0x0024A544
	private void OnConduitFrozen(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
			takeDamageEffect = ((this.ConduitType == ConduitType.Gas) ? SpawnFXHashes.BuildingLeakLiquid : SpawnFXHashes.BuildingFreeze),
			fullDamageEffectName = ((this.ConduitType == ConduitType.Gas) ? "water_damage_kanim" : "ice_damage_kanim")
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x0024C3E8 File Offset: 0x0024A5E8
	private void OnConduitBoiling(object data)
	{
		base.Trigger(-794517298, new BuildingHP.DamageSourceInfo
		{
			damage = 1,
			source = BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
			popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
			takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
			fullDamageEffectName = "gas_damage_kanim"
		});
		this.GetFlowManager().EmptyConduit(Grid.PosToCell(base.transform.GetPosition()));
	}

	// Token: 0x06004158 RID: 16728 RVA: 0x000CEC51 File Offset: 0x000CCE51
	private void OnBuildingBroken(object data)
	{
		this.Disconnect();
	}

	// Token: 0x06004159 RID: 16729 RVA: 0x000CEC59 File Offset: 0x000CCE59
	private void OnBuildingFullyRepaired(object data)
	{
		this.Connect();
	}

	// Token: 0x0600415A RID: 16730 RVA: 0x000CEC62 File Offset: 0x000CCE62
	public bool IsDisconnected()
	{
		return this.disconnected;
	}

	// Token: 0x0600415B RID: 16731 RVA: 0x0024C46C File Offset: 0x0024A66C
	public bool Connect()
	{
		BuildingHP component = base.GetComponent<BuildingHP>();
		if (component == null || component.HitPoints > 0)
		{
			this.disconnected = false;
			this.GetNetworkManager().ForceRebuildNetworks();
		}
		return !this.disconnected;
	}

	// Token: 0x0600415C RID: 16732 RVA: 0x000CEC6A File Offset: 0x000CCE6A
	public void Disconnect()
	{
		this.disconnected = true;
		this.GetNetworkManager().ForceRebuildNetworks();
	}

	// Token: 0x17000320 RID: 800
	// (set) Token: 0x0600415D RID: 16733 RVA: 0x000AA038 File Offset: 0x000A8238
	public FlowUtilityNetwork Network
	{
		set
		{
		}
	}

	// Token: 0x17000321 RID: 801
	// (get) Token: 0x0600415E RID: 16734 RVA: 0x000C1501 File Offset: 0x000BF701
	public int Cell
	{
		get
		{
			return Grid.PosToCell(this);
		}
	}

	// Token: 0x17000322 RID: 802
	// (get) Token: 0x0600415F RID: 16735 RVA: 0x000AA7FE File Offset: 0x000A89FE
	public Endpoint EndpointType
	{
		get
		{
			return Endpoint.Conduit;
		}
	}

	// Token: 0x17000323 RID: 803
	// (get) Token: 0x06004160 RID: 16736 RVA: 0x000CEC7E File Offset: 0x000CCE7E
	public ConduitType ConduitType
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06004161 RID: 16737 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x04002D34 RID: 11572
	[MyCmpReq]
	private KAnimGraphTileVisualizer graphTileDependency;

	// Token: 0x04002D35 RID: 11573
	[SerializeField]
	private bool disconnected = true;

	// Token: 0x04002D36 RID: 11574
	public ConduitType type;

	// Token: 0x04002D37 RID: 11575
	private System.Action firstFrameCallback;

	// Token: 0x04002D38 RID: 11576
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnHighlighted(data);
	});

	// Token: 0x04002D39 RID: 11577
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitFrozen(data);
	});

	// Token: 0x04002D3A RID: 11578
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnConduitBoiling(data);
	});

	// Token: 0x04002D3B RID: 11579
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnStructureTemperatureRegistered(data);
	});

	// Token: 0x04002D3C RID: 11580
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04002D3D RID: 11581
	protected static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>(delegate(Conduit component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
