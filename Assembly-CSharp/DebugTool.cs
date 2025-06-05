using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001460 RID: 5216
public class DebugTool : DragTool
{
	// Token: 0x06006B87 RID: 27527 RVA: 0x000EB0F7 File Offset: 0x000E92F7
	public static void DestroyInstance()
	{
		DebugTool.Instance = null;
	}

	// Token: 0x06006B88 RID: 27528 RVA: 0x000EB0FF File Offset: 0x000E92FF
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DebugTool.Instance = this;
	}

	// Token: 0x06006B89 RID: 27529 RVA: 0x000EAFAB File Offset: 0x000E91AB
	public void Activate()
	{
		PlayerController.Instance.ActivateTool(this);
	}

	// Token: 0x06006B8A RID: 27530 RVA: 0x000EB10D File Offset: 0x000E930D
	public void Activate(DebugTool.Type type)
	{
		this.type = type;
		this.Activate();
	}

	// Token: 0x06006B8B RID: 27531 RVA: 0x000EB11C File Offset: 0x000E931C
	protected override void OnDeactivateTool(InterfaceTool new_tool)
	{
		base.OnDeactivateTool(new_tool);
		PlayerController.Instance.ToolDeactivated(this);
	}

	// Token: 0x06006B8C RID: 27532 RVA: 0x002F0EA4 File Offset: 0x002EF0A4
	protected override void OnDragTool(int cell, int distFromOrigin)
	{
		if (Grid.IsValidCell(cell))
		{
			switch (this.type)
			{
			case DebugTool.Type.ReplaceSubstance:
				this.DoReplaceSubstance(cell);
				return;
			case DebugTool.Type.FillReplaceSubstance:
			{
				GameUtil.FloodFillNext.Value.Clear();
				GameUtil.FloodFillVisited.Value.Clear();
				SimHashes elem_hash = Grid.Element[cell].id;
				GameUtil.FloodFillConditional(cell, delegate(int check_cell)
				{
					bool result = false;
					if (Grid.Element[check_cell].id == elem_hash)
					{
						result = true;
						this.DoReplaceSubstance(check_cell);
					}
					return result;
				}, GameUtil.FloodFillVisited.Value, null);
				return;
			}
			case DebugTool.Type.Clear:
				this.ClearCell(cell);
				return;
			case DebugTool.Type.AddSelection:
				DebugBaseTemplateButton.Instance.AddToSelection(cell);
				return;
			case DebugTool.Type.RemoveSelection:
				DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
				return;
			case DebugTool.Type.Deconstruct:
				this.DeconstructCell(cell);
				return;
			case DebugTool.Type.Destroy:
				this.DestroyCell(cell);
				return;
			case DebugTool.Type.Sample:
				DebugPaintElementScreen.Instance.SampleCell(cell);
				return;
			case DebugTool.Type.StoreSubstance:
				this.DoStoreSubstance(cell);
				return;
			case DebugTool.Type.Dig:
				SimMessages.Dig(cell, -1, false);
				return;
			case DebugTool.Type.Heat:
				SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
				return;
			case DebugTool.Type.Cool:
				SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
				return;
			case DebugTool.Type.AddPressure:
				SimMessages.ModifyMass(cell, 10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 293f, SimHashes.Oxygen);
				return;
			case DebugTool.Type.RemovePressure:
				SimMessages.ModifyMass(cell, -10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 0f, SimHashes.Oxygen);
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06006B8D RID: 27533 RVA: 0x002F102C File Offset: 0x002EF22C
	public void DoReplaceSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte b = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : Grid.Temperature[cell];
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : Grid.Mass[cell];
		int num3 = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : Grid.DiseaseCount[cell];
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		if (DebugPaintElementScreen.Instance.affectCells.isOn)
		{
			SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, num2, num, b, num3, -1);
			if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal)
			{
				Grid.Visible[cell] = 0;
				Grid.PreventFogOfWarReveal[cell] = true;
			}
			else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = false;
			}
		}
		if (DebugPaintElementScreen.Instance.affectBuildings.isOn)
		{
			foreach (GameObject gameObject in new List<GameObject>
			{
				Grid.Objects[cell, 1],
				Grid.Objects[cell, 2],
				Grid.Objects[cell, 9],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 12],
				Grid.Objects[cell, 16],
				Grid.Objects[cell, 26]
			})
			{
				if (gameObject != null)
				{
					PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
					if (num > 0f)
					{
						component.Temperature = num;
					}
					if (num3 > 0 && b != 255)
					{
						component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
						component.AddDisease(b, num3, "DebugTool.DoReplaceSubstance");
					}
				}
			}
		}
	}

	// Token: 0x06006B8E RID: 27534 RVA: 0x000EB130 File Offset: 0x000E9330
	public void DeconstructCell(int cell)
	{
		bool instantBuildMode = DebugHandler.InstantBuildMode;
		DebugHandler.InstantBuildMode = true;
		DeconstructTool.Instance.DeconstructCell(cell);
		if (!instantBuildMode)
		{
			DebugHandler.InstantBuildMode = false;
		}
	}

	// Token: 0x06006B8F RID: 27535 RVA: 0x002F12F0 File Offset: 0x002EF4F0
	public void DestroyCell(int cell)
	{
		foreach (GameObject gameObject in new List<GameObject>
		{
			Grid.Objects[cell, 2],
			Grid.Objects[cell, 1],
			Grid.Objects[cell, 12],
			Grid.Objects[cell, 16],
			Grid.Objects[cell, 20],
			Grid.Objects[cell, 0],
			Grid.Objects[cell, 26],
			Grid.Objects[cell, 31],
			Grid.Objects[cell, 30]
		})
		{
			if (gameObject != null)
			{
				Util.KDestroyGameObject(gameObject);
			}
		}
		this.ClearCell(cell);
		if (ElementLoader.elements[(int)Grid.ElementIdx[cell]].id == SimHashes.Void)
		{
			SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
			return;
		}
		SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0f, 0f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06006B90 RID: 27536 RVA: 0x002F1468 File Offset: 0x002EF668
	public void ClearCell(int cell)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		ListPool<ScenePartitionerEntry, DebugTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
		GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		for (int i = 0; i < pooledList.Count; i++)
		{
			Pickupable pickupable = pooledList[i].obj as Pickupable;
			if (pickupable != null && pickupable.GetComponent<MinionBrain>() == null)
			{
				Util.KDestroyGameObject(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x06006B91 RID: 27537 RVA: 0x002F14F0 File Offset: 0x002EF6F0
	public void DoStoreSubstance(int cell)
	{
		if (!Grid.IsValidBuildingCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject == null)
		{
			return;
		}
		Storage component = gameObject.GetComponent<Storage>();
		if (component == null)
		{
			return;
		}
		Element element = DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element) : ElementLoader.elements[(int)Grid.ElementIdx[cell]];
		if (element == null)
		{
			element = ElementLoader.FindElementByHash(SimHashes.Vacuum);
		}
		byte disease_idx = DebugPaintElementScreen.Instance.paintDisease.isOn ? DebugPaintElementScreen.Instance.diseaseIdx : Grid.DiseaseIdx[cell];
		float num = DebugPaintElementScreen.Instance.paintTemperature.isOn ? DebugPaintElementScreen.Instance.temperature : element.defaultValues.temperature;
		float num2 = DebugPaintElementScreen.Instance.paintMass.isOn ? DebugPaintElementScreen.Instance.mass : element.defaultValues.mass;
		if (num == -1f)
		{
			num = element.defaultValues.temperature;
		}
		if (num2 == -1f)
		{
			num2 = element.defaultValues.mass;
		}
		int disease_count = DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? DebugPaintElementScreen.Instance.diseaseCount : 0;
		if (element.IsGas)
		{
			component.AddGasChunk(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsLiquid)
		{
			component.AddLiquid(element.id, num2, num, disease_idx, disease_count, false, true);
			return;
		}
		if (element.IsSolid)
		{
			component.AddOre(element.id, num2, num, disease_idx, disease_count, false, true);
		}
	}

	// Token: 0x04005180 RID: 20864
	public static DebugTool Instance;

	// Token: 0x04005181 RID: 20865
	public DebugTool.Type type;

	// Token: 0x02001461 RID: 5217
	public enum Type
	{
		// Token: 0x04005183 RID: 20867
		ReplaceSubstance,
		// Token: 0x04005184 RID: 20868
		FillReplaceSubstance,
		// Token: 0x04005185 RID: 20869
		Clear,
		// Token: 0x04005186 RID: 20870
		AddSelection,
		// Token: 0x04005187 RID: 20871
		RemoveSelection,
		// Token: 0x04005188 RID: 20872
		Deconstruct,
		// Token: 0x04005189 RID: 20873
		Destroy,
		// Token: 0x0400518A RID: 20874
		Sample,
		// Token: 0x0400518B RID: 20875
		StoreSubstance,
		// Token: 0x0400518C RID: 20876
		Dig,
		// Token: 0x0400518D RID: 20877
		Heat,
		// Token: 0x0400518E RID: 20878
		Cool,
		// Token: 0x0400518F RID: 20879
		AddPressure,
		// Token: 0x04005190 RID: 20880
		RemovePressure,
		// Token: 0x04005191 RID: 20881
		PaintPlant
	}
}
