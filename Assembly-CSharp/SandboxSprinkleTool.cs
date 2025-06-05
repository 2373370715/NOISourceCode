using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001485 RID: 5253
public class SandboxSprinkleTool : BrushTool
{
	// Token: 0x06006CD6 RID: 27862 RVA: 0x000EC029 File Offset: 0x000EA229
	public static void DestroyInstance()
	{
		SandboxSprinkleTool.instance = null;
	}

	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06006CD7 RID: 27863 RVA: 0x000EBA47 File Offset: 0x000E9C47
	private SandboxSettings settings
	{
		get
		{
			return SandboxToolParameterMenu.instance.settings;
		}
	}

	// Token: 0x06006CD8 RID: 27864 RVA: 0x000EC031 File Offset: 0x000EA231
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		SandboxSprinkleTool.instance = this;
	}

	// Token: 0x06006CD9 RID: 27865 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006CDA RID: 27866 RVA: 0x002F5DE0 File Offset: 0x002F3FE0
	protected override void OnActivateTool()
	{
		base.OnActivateTool();
		SandboxToolParameterMenu.instance.gameObject.SetActive(true);
		SandboxToolParameterMenu.instance.DisableParameters();
		SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseScaleSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.noiseDensitySlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.massSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.temperatureSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.elementSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseSelector.row.SetActive(true);
		SandboxToolParameterMenu.instance.diseaseCountSlider.row.SetActive(true);
		SandboxToolParameterMenu.instance.brushRadiusSlider.SetValue((float)this.settings.GetIntSetting("SandboxTools.BrushSize"), true);
	}

	// Token: 0x06006CDB RID: 27867 RVA: 0x000EBB35 File Offset: 0x000E9D35
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x002F5ED8 File Offset: 0x002F40D8
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		foreach (int num in this.recentlyAffectedCells)
		{
			Color color = new Color(this.recentAffectedCellColor[num].r, this.recentAffectedCellColor[num].g, this.recentAffectedCellColor[num].b, MathUtil.ReRange(Mathf.Sin(Time.realtimeSinceStartup * 5f), -1f, 1f, 0.1f, 0.2f));
			colors.Add(new ToolMenu.CellColorData(num, color));
		}
		foreach (int num2 in this.cellsInRadius)
		{
			if (this.recentlyAffectedCells.Contains(num2))
			{
				Color radiusIndicatorColor = this.radiusIndicatorColor;
				Color color2 = this.recentAffectedCellColor[num2];
				color2.a = 0.2f;
				Color color3 = new Color((radiusIndicatorColor.r + color2.r) / 2f, (radiusIndicatorColor.g + color2.g) / 2f, (radiusIndicatorColor.b + color2.b) / 2f, radiusIndicatorColor.a + (1f - radiusIndicatorColor.a) * color2.a);
				colors.Add(new ToolMenu.CellColorData(num2, color3));
			}
			else
			{
				colors.Add(new ToolMenu.CellColorData(num2, this.radiusIndicatorColor));
			}
		}
	}

	// Token: 0x06006CDD RID: 27869 RVA: 0x002F60A0 File Offset: 0x002F42A0
	public override void SetBrushSize(int radius)
	{
		this.brushRadius = radius;
		this.brushOffsets.Clear();
		for (int i = 0; i < this.brushRadius * 2; i++)
		{
			for (int j = 0; j < this.brushRadius * 2; j++)
			{
				if (Vector2.Distance(new Vector2((float)i, (float)j), new Vector2((float)this.brushRadius, (float)this.brushRadius)) < (float)this.brushRadius - 0.8f)
				{
					Vector2 vector = Grid.CellToXY(Grid.OffsetCell(this.currentCell, i, j));
					float num = PerlinSimplexNoise.noise(vector.x / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), vector.y / this.settings.GetFloatSetting("SandboxTools.NoiseDensity"), Time.realtimeSinceStartup);
					if (this.settings.GetFloatSetting("SandboxTools.NoiseScale") <= num)
					{
						this.brushOffsets.Add(new Vector2((float)(i - this.brushRadius), (float)(j - this.brushRadius)));
					}
				}
			}
		}
	}

	// Token: 0x06006CDE RID: 27870 RVA: 0x000EC03F File Offset: 0x000EA23F
	private void Update()
	{
		this.OnMouseMove(Grid.CellToPos(this.currentCell));
	}

	// Token: 0x06006CDF RID: 27871 RVA: 0x000EC052 File Offset: 0x000EA252
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.SetBrushSize(this.settings.GetIntSetting("SandboxTools.BrushSize"));
	}

	// Token: 0x06006CE0 RID: 27872 RVA: 0x002F61AC File Offset: 0x002F43AC
	protected override void OnPaintCell(int cell, int distFromOrigin)
	{
		base.OnPaintCell(cell, distFromOrigin);
		this.recentlyAffectedCells.Add(cell);
		Element element = ElementLoader.elements[this.settings.GetIntSetting("SandboxTools.SelectedElement")];
		if (!this.recentAffectedCellColor.ContainsKey(cell))
		{
			this.recentAffectedCellColor.Add(cell, element.substance.uiColour);
		}
		else
		{
			this.recentAffectedCellColor[cell] = element.substance.uiColour;
		}
		Game.CallbackInfo item = new Game.CallbackInfo(delegate()
		{
			this.recentlyAffectedCells.Remove(cell);
			this.recentAffectedCellColor.Remove(cell);
		}, false);
		int index = Game.Instance.callbackManager.Add(item).index;
		byte index2 = Db.Get().Diseases.GetIndex(Db.Get().Diseases.Get("FoodPoisoning").id);
		Disease disease = Db.Get().Diseases.TryGet(this.settings.GetStringSetting("SandboxTools.SelectedDisease"));
		if (disease != null)
		{
			index2 = Db.Get().Diseases.GetIndex(disease.id);
		}
		int cell2 = cell;
		SimHashes id = element.id;
		CellElementEvent sandBoxTool = CellEventLogger.Instance.SandBoxTool;
		float floatSetting = this.settings.GetFloatSetting("SandboxTools.Mass");
		float floatSetting2 = this.settings.GetFloatSetting("SandbosTools.Temperature");
		int callbackIdx = index;
		SimMessages.ReplaceElement(cell2, id, sandBoxTool, floatSetting, floatSetting2, index2, this.settings.GetIntSetting("SandboxTools.DiseaseCount"), callbackIdx);
		this.SetBrushSize(this.brushRadius);
	}

	// Token: 0x06006CE1 RID: 27873 RVA: 0x002F4950 File Offset: 0x002F2B50
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

	// Token: 0x04005222 RID: 21026
	public static SandboxSprinkleTool instance;

	// Token: 0x04005223 RID: 21027
	protected HashSet<int> recentlyAffectedCells = new HashSet<int>();

	// Token: 0x04005224 RID: 21028
	private Dictionary<int, Color> recentAffectedCellColor = new Dictionary<int, Color>();
}
