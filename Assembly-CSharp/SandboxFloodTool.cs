using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Klei.AI;
using UnityEngine;

// Token: 0x0200147F RID: 5247
public class SandboxFloodTool : FloodTool
{
	// Token: 0x06006CA7 RID: 27815 RVA: 0x000EBD52 File Offset: 0x000E9F52
	public static void DestroyInstance()
	{
		SandboxFloodTool.instance = null;
	}

	// Token: 0x06006CA8 RID: 27816 RVA: 0x000EBD5A File Offset: 0x000E9F5A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxFloodTool.instance = this;
		this.floodCriteria = ((int cell) => Grid.IsValidCell(cell) && Grid.Element[cell] == Grid.Element[this.mouseCell] && Grid.WorldIdx[cell] == Grid.WorldIdx[this.mouseCell]);
		this.paintArea = delegate(HashSet<int> cells)
		{
			foreach (int cell in cells)
			{
				this.PaintCell(cell);
			}
		};
	}

	// Token: 0x06006CA9 RID: 27817 RVA: 0x002F5210 File Offset: 0x002F3410
	private void PaintCell(int cell)
	{
		this.recentlyAffectedCells.Add(cell);
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
		}, false);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index = Db.Get().Diseases.GetIndex(disease.id);
		}
		int index2 = Game.Instance.callbackManager.Add(item).index;
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index2;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
	}

	// Token: 0x170006D6 RID: 1750
	// (get) Token: 0x06006CAA RID: 27818 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006CAB RID: 27819 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006CAC RID: 27820 RVA: 0x002F5344 File Offset: 0x002F3544
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
	}

	// Token: 0x06006CAD RID: 27821 RVA: 0x000EBD8C File Offset: 0x000E9F8C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.ev.release();
	}

	// Token: 0x06006CAE RID: 27822 RVA: 0x002F53DC File Offset: 0x002F35DC
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int cell in this.recentlyAffectedCells)
		{
			colors.Add(new ToolMenu.CellColorData(cell, this.recentlyAffectedCellColor));
		}
		foreach (int cell2 in this.cellsToAffect)
		{
			colors.Add(new ToolMenu.CellColorData(cell2, this.areaColour));
		}
	}

	// Token: 0x06006CAF RID: 27823 RVA: 0x000EBDB1 File Offset: 0x000E9FB1
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.cellsToAffect = base.Flood(Grid.PosToCell(cursorPos));
	}

	// Token: 0x06006CB0 RID: 27824 RVA: 0x002F5498 File Offset: 0x002F3698
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		base.OnLeftClickDown(cursor_pos);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		string sound;
		if (element.IsSolid)
		{
			sound = GlobalAssets.GetSound("Break_" + element.substance.GetMiningBreakSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Break_Rock", false);
			}
		}
		else if (element.IsGas)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Gas", false);
		}
		else if (element.IsLiquid)
		{
			sound = GlobalAssets.GetSound("SandboxTool_Bucket_Liquid", false);
		}
		else
		{
			sound = GlobalAssets.GetSound("Break_Rock", false);
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Bucket", false));
	}

	// Token: 0x06006CB1 RID: 27825 RVA: 0x002F5598 File Offset: 0x002F3798
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.SandboxCopyElement))
		{
			int cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			if (Grid.IsValidCell(cell))
			{
				SandboxSampleTool.Sample(cell);
			}
		}
		if (!e.Consumed)
		{
			base.OnKeyDown(e);
		}
	}

	// Token: 0x0400520F RID: 21007
	public static SandboxFloodTool instance;

	// Token: 0x04005210 RID: 21008
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04005211 RID: 21009
	protected HashSet<int> cellsToAffect = new HashSet<int>();

	// Token: 0x04005212 RID: 21010
	protected Color recentlyAffectedCellColor = new Color(1f, 1f, 1f, 0.1f);

	// Token: 0x04005213 RID: 21011
	private EventInstance ev;
}
