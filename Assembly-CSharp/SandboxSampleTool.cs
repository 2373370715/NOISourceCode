using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using STRINGS;
using UnityEngine;

// Token: 0x02001483 RID: 5251
public class SandboxSampleTool : InterfaceTool
{
	// Token: 0x06006CC4 RID: 27844 RVA: 0x000EBEC4 File Offset: 0x000EA0C4
	public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
	{
		colors = new HashSet<ToolMenu.CellColorData>();
		colors.Add(new ToolMenu.CellColorData(this.currentCell, this.radiusIndicatorColor));
	}

	// Token: 0x06006CC5 RID: 27845 RVA: 0x000EBEE6 File Offset: 0x000EA0E6
	public override void OnMouseMove(Vector3 cursorPos)
	{
		base.OnMouseMove(cursorPos);
		this.currentCell = Grid.PosToCell(cursorPos);
	}

	// Token: 0x06006CC6 RID: 27846 RVA: 0x002F58BC File Offset: 0x002F3ABC
	public override void OnLeftClickDown(Vector3 cursor_pos)
	{
		int cell = Grid.PosToCell(cursor_pos);
		if (!Grid.IsValidCell(cell))
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.DEBUG_TOOLS.INVALID_LOCATION, null, cursor_pos, 1.5f, false, true);
			return;
		}
		SandboxSampleTool.Sample(cell);
		KFMOD.PlayUISound(GlobalAssets.GetSound("SandboxTool_Click", false));
		this.PlaySound();
	}

	// Token: 0x06006CC7 RID: 27847 RVA: 0x002F5920 File Offset: 0x002F3B20
	public static void Sample(int cell)
	{
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.SelectedElement", (int)Grid.Element[cell].idx);
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandboxTools.Mass", Mathf.Round(Grid.Mass[cell] * 100f) / 100f);
		SandboxToolParameterMenu.instance.settings.SetFloatSetting("SandbosTools.Temperature", Mathf.Round(Grid.Temperature[cell] * 10f) / 10f);
		SandboxToolParameterMenu.instance.settings.SetIntSetting("SandboxTools.DiseaseCount", Grid.DiseaseCount[cell]);
		SandboxToolParameterMenu.instance.RefreshDisplay();
	}

	// Token: 0x06006CC8 RID: 27848 RVA: 0x002F5344 File Offset: 0x002F3544
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

	// Token: 0x06006CC9 RID: 27849 RVA: 0x000EBEFB File Offset: 0x000EA0FB
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		SandboxToolParameterMenu.instance.gameObject.SetActive(false);
		this.StopSound();
	}

	// Token: 0x06006CCA RID: 27850 RVA: 0x002F59D8 File Offset: 0x002F3BD8
	private void PlaySound()
	{
		Element element = ElementLoader.elements[SandboxToolParameterMenu.instance.settings.GetIntSetting("SandboxTools.SelectedElement")];
		float volume = 1f;
		float pitch = 1f;
		string sound = GlobalAssets.GetSound("Ore_bump_Rock", false);
		switch (element.state & Element.State.Solid)
		{
		case Element.State.Vacuum:
			sound = GlobalAssets.GetSound("ConduitBlob_Gas", false);
			break;
		case Element.State.Gas:
			sound = GlobalAssets.GetSound("ConduitBlob_Gas", false);
			break;
		case Element.State.Liquid:
			sound = GlobalAssets.GetSound("ConduitBlob_Liquid", false);
			break;
		case Element.State.Solid:
			sound = GlobalAssets.GetSound("Ore_bump_" + element.substance.GetMiningSound(), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("Ore_bump_Rock", false);
			}
			volume = 0.7f;
			pitch = 2f;
			break;
		}
		this.ev = KFMOD.CreateInstance(sound);
		ATTRIBUTES_3D attributes = SoundListenerController.Instance.transform.GetPosition().To3DAttributes();
		this.ev.set3DAttributes(attributes);
		this.ev.setVolume(volume);
		this.ev.setPitch(pitch);
		this.ev.setParameterByName("blobCount", (float)UnityEngine.Random.Range(0, 6), false);
		this.ev.setParameterByName("SandboxToggle", 1f, false);
		this.ev.start();
	}

	// Token: 0x06006CCB RID: 27851 RVA: 0x000EBF1A File Offset: 0x000EA11A
	private void StopSound()
	{
		this.ev.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		this.ev.release();
	}

	// Token: 0x0400521B RID: 21019
	protected Color radiusIndicatorColor = new Color(0.5f, 0.7f, 0.5f, 0.2f);

	// Token: 0x0400521C RID: 21020
	private int currentCell;

	// Token: 0x0400521D RID: 21021
	private EventInstance ev;
}
