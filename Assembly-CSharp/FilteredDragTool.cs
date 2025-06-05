using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200146C RID: 5228
public class FilteredDragTool : DragTool
{
	// Token: 0x06006BE7 RID: 27623 RVA: 0x000EB487 File Offset: 0x000E9687
	public bool IsActiveLayer(string layer)
	{
		return this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On || (this.currentFilterTargets.ContainsKey(layer.ToUpper()) && this.currentFilterTargets[layer.ToUpper()] == ToolParameterMenu.ToggleState.On);
	}

	// Token: 0x06006BE8 RID: 27624 RVA: 0x002F2980 File Offset: 0x002F0B80
	public bool IsActiveLayer(ObjectLayer layer)
	{
		if (this.currentFilterTargets.ContainsKey(ToolParameterMenu.FILTERLAYERS.ALL) && this.currentFilterTargets[ToolParameterMenu.FILTERLAYERS.ALL] == ToolParameterMenu.ToggleState.On)
		{
			return true;
		}
		bool result = false;
		foreach (KeyValuePair<string, ToolParameterMenu.ToggleState> keyValuePair in this.currentFilterTargets)
		{
			if (keyValuePair.Value == ToolParameterMenu.ToggleState.On && this.GetObjectLayerFromFilterLayer(keyValuePair.Key) == layer)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006BE9 RID: 27625 RVA: 0x002F2A14 File Offset: 0x002F0C14
	protected virtual void GetDefaultFilters(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Add(ToolParameterMenu.FILTERLAYERS.ALL, ToolParameterMenu.ToggleState.On);
		filters.Add(ToolParameterMenu.FILTERLAYERS.WIRES, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.GASCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BUILDINGS, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.LOGIC, ToolParameterMenu.ToggleState.Off);
		filters.Add(ToolParameterMenu.FILTERLAYERS.BACKWALL, ToolParameterMenu.ToggleState.Off);
	}

	// Token: 0x06006BEA RID: 27626 RVA: 0x000EB4C6 File Offset: 0x000E96C6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06006BEB RID: 27627 RVA: 0x000EB4DA File Offset: 0x000E96DA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Combine(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
	}

	// Token: 0x06006BEC RID: 27628 RVA: 0x000EB508 File Offset: 0x000E9708
	protected override void OnCleanUp()
	{
		OverlayScreen instance = OverlayScreen.Instance;
		instance.OnOverlayChanged = (Action<HashedString>)Delegate.Remove(instance.OnOverlayChanged, new Action<HashedString>(this.OnOverlayChanged));
		base.OnCleanUp();
	}

	// Token: 0x06006BED RID: 27629 RVA: 0x000EB536 File Offset: 0x000E9736
	public void ResetFilter()
	{
		this.ResetFilter(this.filterTargets);
	}

	// Token: 0x06006BEE RID: 27630 RVA: 0x000EB544 File Offset: 0x000E9744
	protected void ResetFilter(Dictionary<string, ToolParameterMenu.ToggleState> filters)
	{
		filters.Clear();
		this.GetDefaultFilters(filters);
		this.currentFilterTargets = filters;
	}

	// Token: 0x06006BEF RID: 27631 RVA: 0x000EB55A File Offset: 0x000E975A
	protected override void OnActivateTool()
	{
		this.active = true;
		base.OnActivateTool();
		this.OnOverlayChanged(OverlayScreen.Instance.mode);
	}

	// Token: 0x06006BF0 RID: 27632 RVA: 0x000EB579 File Offset: 0x000E9779
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		this.active = false;
		ToolMenu.Instance.toolParameterMenu.ClearMenu();
		base.OnDeactivateTool(new_tool);
	}

	// Token: 0x06006BF1 RID: 27633 RVA: 0x002F2A84 File Offset: 0x002F0C84
	public virtual string GetFilterLayerFromGameObject(GameObject input)
	{
		BuildingComplete component = input.GetComponent<BuildingComplete>();
		BuildingUnderConstruction component2 = input.GetComponent<BuildingUnderConstruction>();
		if (component)
		{
			return this.GetFilterLayerFromObjectLayer(component.Def.ObjectLayer);
		}
		if (component2)
		{
			return this.GetFilterLayerFromObjectLayer(component2.Def.ObjectLayer);
		}
		if (input.GetComponent<Clearable>() != null || input.GetComponent<Moppable>() != null)
		{
			return "CleanAndClear";
		}
		if (input.GetComponent<Diggable>() != null)
		{
			return "DigPlacer";
		}
		return "Default";
	}

	// Token: 0x06006BF2 RID: 27634 RVA: 0x002F2B10 File Offset: 0x002F0D10
	public string GetFilterLayerFromObjectLayer(ObjectLayer gamer_layer)
	{
		if (gamer_layer > ObjectLayer.FoundationTile)
		{
			switch (gamer_layer)
			{
			case ObjectLayer.GasConduit:
			case ObjectLayer.GasConduitConnection:
				return "GasPipes";
			case ObjectLayer.GasConduitTile:
			case ObjectLayer.ReplacementGasConduit:
			case ObjectLayer.LiquidConduitTile:
			case ObjectLayer.ReplacementLiquidConduit:
				goto IL_AC;
			case ObjectLayer.LiquidConduit:
			case ObjectLayer.LiquidConduitConnection:
				return "LiquidPipes";
			case ObjectLayer.SolidConduit:
				break;
			default:
				switch (gamer_layer)
				{
				case ObjectLayer.SolidConduitConnection:
					break;
				case ObjectLayer.LadderTile:
				case ObjectLayer.ReplacementLadder:
				case ObjectLayer.WireTile:
				case ObjectLayer.ReplacementWire:
					goto IL_AC;
				case ObjectLayer.Wire:
				case ObjectLayer.WireConnectors:
					return "Wires";
				case ObjectLayer.LogicGate:
				case ObjectLayer.LogicWire:
					return "Logic";
				default:
					if (gamer_layer == ObjectLayer.Gantry)
					{
						goto IL_7C;
					}
					goto IL_AC;
				}
				break;
			}
			return "SolidConduits";
		}
		if (gamer_layer != ObjectLayer.Building)
		{
			if (gamer_layer == ObjectLayer.Backwall)
			{
				return "BackWall";
			}
			if (gamer_layer != ObjectLayer.FoundationTile)
			{
				goto IL_AC;
			}
			return "Tiles";
		}
		IL_7C:
		return "Buildings";
		IL_AC:
		return "Default";
	}

	// Token: 0x06006BF3 RID: 27635 RVA: 0x002F2BD0 File Offset: 0x002F0DD0
	private ObjectLayer GetObjectLayerFromFilterLayer(string filter_layer)
	{
		string text = filter_layer.ToLower();
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 2200975418U)
		{
			if (num <= 388608975U)
			{
				if (num != 25076977U)
				{
					if (num == 388608975U)
					{
						if (text == "solidconduits")
						{
							return ObjectLayer.SolidConduit;
						}
					}
				}
				else if (text == "wires")
				{
					return ObjectLayer.Wire;
				}
			}
			else if (num != 614364310U)
			{
				if (num == 2200975418U)
				{
					if (text == "backwall")
					{
						return ObjectLayer.Backwall;
					}
				}
			}
			else if (text == "liquidpipes")
			{
				return ObjectLayer.LiquidConduit;
			}
		}
		else if (num <= 2875565775U)
		{
			if (num != 2366751346U)
			{
				if (num == 2875565775U)
				{
					if (text == "gaspipes")
					{
						return ObjectLayer.GasConduit;
					}
				}
			}
			else if (text == "buildings")
			{
				return ObjectLayer.Building;
			}
		}
		else if (num != 3464443665U)
		{
			if (num == 4178729166U)
			{
				if (text == "tiles")
				{
					return ObjectLayer.FoundationTile;
				}
			}
		}
		else if (text == "logic")
		{
			return ObjectLayer.LogicWire;
		}
		throw new ArgumentException("Invalid filter layer: " + filter_layer);
	}

	// Token: 0x06006BF4 RID: 27636 RVA: 0x002F2D18 File Offset: 0x002F0F18
	private void OnOverlayChanged(HashedString overlay)
	{
		if (!this.active)
		{
			return;
		}
		string text = null;
		if (overlay == OverlayModes.Power.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.WIRES;
		}
		else if (overlay == OverlayModes.LiquidConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LIQUIDCONDUIT;
		}
		else if (overlay == OverlayModes.GasConduits.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.GASCONDUIT;
		}
		else if (overlay == OverlayModes.SolidConveyor.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.SOLIDCONDUIT;
		}
		else if (overlay == OverlayModes.Logic.ID)
		{
			text = ToolParameterMenu.FILTERLAYERS.LOGIC;
		}
		this.currentFilterTargets = this.filterTargets;
		if (text != null)
		{
			using (List<string>.Enumerator enumerator = new List<string>(this.filterTargets.Keys).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text2 = enumerator.Current;
					this.filterTargets[text2] = ToolParameterMenu.ToggleState.Disabled;
					if (text2 == text)
					{
						this.filterTargets[text2] = ToolParameterMenu.ToggleState.On;
					}
				}
				goto IL_102;
			}
		}
		if (this.overlayFilterTargets.Count == 0)
		{
			this.ResetFilter(this.overlayFilterTargets);
		}
		this.currentFilterTargets = this.overlayFilterTargets;
		IL_102:
		ToolMenu.Instance.toolParameterMenu.PopulateMenu(this.currentFilterTargets);
	}

	// Token: 0x040051BC RID: 20924
	private Dictionary<string, ToolParameterMenu.ToggleState> filterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x040051BD RID: 20925
	private Dictionary<string, ToolParameterMenu.ToggleState> overlayFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>();

	// Token: 0x040051BE RID: 20926
	private Dictionary<string, ToolParameterMenu.ToggleState> currentFilterTargets;

	// Token: 0x040051BF RID: 20927
	private bool active;
}
