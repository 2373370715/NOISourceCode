﻿using System;
using STRINGS;
using UnityEngine;

public class PlaceSpaceAvailable : SelectModuleCondition
{
	public override bool EvaluateCondition(GameObject existingModule, BuildingDef selectedPart, SelectModuleCondition.SelectionContext selectionContext)
	{
		BuildingAttachPoint component = existingModule.GetComponent<BuildingAttachPoint>();
		switch (selectionContext)
		{
		case SelectModuleCondition.SelectionContext.AddModuleAbove:
		{
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule) && !component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(selectedPart.HeightInCells, null))
			{
				return false;
			}
			int cell = Grid.OffsetCell(Grid.PosToCell(existingModule), 0, existingModule.GetComponent<Building>().Def.HeightInCells);
			foreach (CellOffset offset in selectedPart.PlacementOffsets)
			{
				if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(cell, offset), existingModule))
				{
					return false;
				}
			}
			return true;
		}
		case SelectModuleCondition.SelectionContext.AddModuleBelow:
		{
			if (!existingModule.GetComponent<ReorderableBuilding>().CanMoveVertically(selectedPart.HeightInCells, null))
			{
				return false;
			}
			int cell2 = Grid.PosToCell(existingModule);
			foreach (CellOffset offset2 in selectedPart.PlacementOffsets)
			{
				if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(cell2, offset2), existingModule))
				{
					return false;
				}
			}
			return true;
		}
		case SelectModuleCondition.SelectionContext.ReplaceModule:
		{
			int moveAmount = selectedPart.HeightInCells - existingModule.GetComponent<Building>().Def.HeightInCells;
			if (component != null && component.points[0].attachedBuilding != null && component.points[0].attachedBuilding.HasTag(GameTags.RocketModule))
			{
				ReorderableBuilding component2 = existingModule.GetComponent<ReorderableBuilding>();
				if (!component.points[0].attachedBuilding.GetComponent<ReorderableBuilding>().CanMoveVertically(moveAmount, component2.gameObject))
				{
					return false;
				}
			}
			ReorderableBuilding component3 = existingModule.GetComponent<ReorderableBuilding>();
			foreach (CellOffset offset3 in selectedPart.PlacementOffsets)
			{
				if (!ReorderableBuilding.CheckCellClear(Grid.OffsetCell(Grid.PosToCell(component3), offset3), component3.gameObject))
				{
					return false;
				}
			}
			return true;
		}
		default:
			return true;
		}
	}

	public override string GetStatusTooltip(bool ready, GameObject moduleBase, BuildingDef selectedPart)
	{
		if (ready)
		{
			return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.SPACE_AVAILABLE.COMPLETE;
		}
		return UI.UISIDESCREENS.SELECTMODULESIDESCREEN.CONSTRAINTS.SPACE_AVAILABLE.FAILED;
	}
}
