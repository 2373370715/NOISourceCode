using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200148D RID: 5261
public class SandboxStressTool : BrushTool
{
	// Token: 0x06006CFA RID: 27898 RVA: 0x000EC114 File Offset: 0x000EA314
	public static void DestroyInstance()
	{
		SandboxStressTool.instance = null;
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x06006CFB RID: 27899 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006CFC RID: 27900 RVA: 0x000EC11C File Offset: 0x000EA31C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxStressTool.instance = this;
	}

	// Token: 0x06006CFD RID: 27901 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	protected override string GetDragSound()
	{
		return "";
	}

	// Token: 0x06006CFE RID: 27902 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006CFF RID: 27903 RVA: 0x002F6B78 File Offset: 0x002F4D78
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.stressAdditiveSlider.SetValue(5f, true);
		SandboxToolParameterMenu.instance.moraleSlider.SetValue(0f, true);
		if (DebugHandler.InstantBuildMode)
		{
			SandboxToolParameterMenu.instance.moraleSlider.row.SetActive(true);
		}
	}

	// Token: 0x06006D00 RID: 27904 RVA: 0x000EC12A File Offset: 0x000EA32A
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x06006D01 RID: 27905 RVA: 0x002F6C18 File Offset: 0x002F4E18
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

	// Token: 0x06006D02 RID: 27906 RVA: 0x000EBB4E File Offset: 0x000E9D4E
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
	}

	// Token: 0x06006D03 RID: 27907 RVA: 0x000EBA86 File Offset: 0x000E9C86
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
	}

	// Token: 0x06006D04 RID: 27908 RVA: 0x002F6CD0 File Offset: 0x002F4ED0
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		for (int i = 0; i < Components.LiveMinionIdentities.Count; i++)
		{
			GameObject gameObject = Components.LiveMinionIdentities[i].gameObject;
			if (Grid.PosToCell(gameObject) == cell)
			{
				float num = -1f * SandboxToolParameterMenu.instance.settings.GetFloatSetting("SandbosTools.StressAdditive");
				Db.Get().Amounts.Stress.Lookup(Components.LiveMinionIdentities[i].gameObject).ApplyDelta(num);
				if (num >= 0f)
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_angry"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				else
				{
					PopFXManager.Instance.SpawnFX(Assets.GetSprite("crew_state_happy"), GameUtil.GetFormattedPercent(num, GameUtil.TimeSlice.None), gameObject.transform, 1.5f, false);
				}
				this.PlaySound(num, gameObject.transform.GetPosition());
				int intSetting = SandboxToolParameterMenu.instance.settings.GetIntSetting("SandbosTools.MoraleAdjustment");
				AttributeInstance attributeInstance = gameObject.GetAttributes().Get(Db.Get().Attributes.QualityOfLife);
				MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
				if (this.moraleAdjustments.ContainsKey(component))
				{
					attributeInstance.Remove(this.moraleAdjustments[component]);
					this.moraleAdjustments.Remove(component);
				}
				if (intSetting != 0)
				{
					AttributeModifier attributeModifier = new AttributeModifier(attributeInstance.Id, (float)intSetting, () => DUPLICANTS.MODIFIERS.SANDBOXMORALEADJUSTMENT.NAME, false, false);
					attributeModifier.SetValue((float)intSetting);
					attributeInstance.Add(attributeModifier);
					this.moraleAdjustments.Add(component, attributeModifier);
				}
			}
		}
	}

	// Token: 0x06006D05 RID: 27909 RVA: 0x002F6E98 File Offset: 0x002F5098
	private void PlaySound(float sliderValue, Vector3 position)
	{
		this.ev = KFMOD.CreateInstance(this.UISoundPath);
		ATTRIBUTES_3D attributes = position.To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByNameWithLabel("SanboxTool_Effect", (sliderValue >= 0f) ? "Decrease" : "Increase", false);
		this.ev.start();
	}

	// Token: 0x06006D06 RID: 27910 RVA: 0x000EC149 File Offset: 0x000EA349
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x04005237 RID: 21047
	public static SandboxStressTool instance;

	// Token: 0x04005238 RID: 21048
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04005239 RID: 21049
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x0400523A RID: 21050
	private string UISoundPath = GlobalAssets.GetSound("SandboxTool_Happy", false);

	// Token: 0x0400523B RID: 21051
	private EventInstance ev;

	// Token: 0x0400523C RID: 21052
	private Dictionary<MinionIdentity, AttributeModifier> moraleAdjustments = new Dictionary<MinionIdentity, AttributeModifier>();
}
