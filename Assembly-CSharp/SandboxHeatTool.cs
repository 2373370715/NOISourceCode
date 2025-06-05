using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001481 RID: 5249
public class SandboxHeatTool : BrushTool
{
	// Token: 0x06006CB7 RID: 27831 RVA: 0x000EBE58 File Offset: 0x000EA058
	public static void DestroyInstance()
	{
		SandboxHeatTool.instance = null;
	}

	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x06006CB8 RID: 27832 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006CB9 RID: 27833 RVA: 0x000EBE60 File Offset: 0x000EA060
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxHeatTool.instance = this;
		this.viewMode = OverlayModes.Temperature.ID;
	}

	// Token: 0x06006CBA RID: 27834 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006CBB RID: 27835 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006CBC RID: 27836 RVA: 0x002F5630 File Offset: 0x002F3830
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureAdditiveSlider.row.SetActive(true);
	}

	// Token: 0x06006CBD RID: 27837 RVA: 0x000EBB35 File Offset: 0x000E9D35
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006CBE RID: 27838 RVA: 0x002F5688 File Offset: 0x002F3888
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsInRadius)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.radiusIndicatorColor));
		}
	}

	// Token: 0x06006CBF RID: 27839 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006CC0 RID: 27840 RVA: 0x002F5740 File Offset: 0x002F3940
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		if (this.recentlyAffectedCells.Contains(cell))
		{
			return;
		}
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		float num = Grid.Temperature[cell];
		num += SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.TemperatureAdditive");
		GameUtil.TemperatureUnit temperatureUnit = GameUtil.temperatureUnit;
		if (temperatureUnit != GameUtil.TemperatureUnit.Celsius)
		{
			if (temperatureUnit == GameUtil.TemperatureUnit.Fahrenheit)
			{
				num -= 255.372f;
			}
		}
		else
		{
			num -= 273.15f;
		}
		num = Mathf.Clamp(num, 1f, 9999f);
		int cell2 = cell;
		SimHashes id = Grid.Element[cell].id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float mass = Grid.Mass[cell];
		float temperature = num;
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, mass, temperature, Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], callbackIdx);
		float currentValue = SandboxToolParameterMenu.instance.temperatureAdditiveSlider.inputField.currentValue;
		KFMOD.PlayUISoundWithLabeledParameter(GlobalAssets.GetSound("SandboxTool_HeatGun", false), "TemperatureSetting", (currentValue <= 0f) ? "Cooling" : "Heating");
	}

	// Token: 0x04005216 RID: 21014
	public static SandboxHeatTool instance;

	// Token: 0x04005217 RID: 21015
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04005218 RID: 21016
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);
}
