using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000E4D RID: 3661
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Ladder")]
public class Ladder : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x0600479A RID: 18330 RVA: 0x00260FE8 File Offset: 0x0025F1E8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int i2 = Grid.OffsetCell(Grid.PosToCell(this), offset);
			Grid.HasPole[i2] = this.isPole;
			Grid.HasLadder[i2] = !this.isPole;
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.Ladders, false);
		Components.Ladders.Add(this);
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x000D2D57 File Offset: 0x000D0F57
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x00261088 File Offset: 0x0025F288
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Rotatable component = base.GetComponent<Rotatable>();
		foreach (CellOffset cellOffset in this.offsets)
		{
			CellOffset offset = cellOffset;
			if (component != null)
			{
				offset = component.GetRotatedCellOffset(cellOffset);
			}
			int num = Grid.OffsetCell(Grid.PosToCell(this), offset);
			if (Grid.Objects[num, 24] == null)
			{
				Grid.HasPole[num] = false;
				Grid.HasLadder[num] = false;
			}
		}
		Components.Ladders.Remove(this);
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x00261120 File Offset: 0x0025F320
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = null;
		if (this.upwardsMovementSpeedMultiplier != 1f)
		{
			list = new List<Descriptor>();
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, GameUtil.GetFormattedPercent(this.upwardsMovementSpeedMultiplier * 100f - 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x04003226 RID: 12838
	public float upwardsMovementSpeedMultiplier = 1f;

	// Token: 0x04003227 RID: 12839
	public float downwardsMovementSpeedMultiplier = 1f;

	// Token: 0x04003228 RID: 12840
	public bool isPole;

	// Token: 0x04003229 RID: 12841
	public CellOffset[] offsets = new CellOffset[]
	{
		CellOffset.none
	};
}
