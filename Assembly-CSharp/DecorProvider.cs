using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001249 RID: 4681
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/DecorProvider")]
public class DecorProvider : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06005F35 RID: 24373 RVA: 0x002B4360 File Offset: 0x002B2560
	private void AddDecor()
	{
		this.currDecor = 0f;
		if (this.decor != null)
		{
			this.currDecor = this.decor.GetTotalValue();
		}
		if (this.prefabId.HasTag(GameTags.Stored))
		{
			this.currDecor = 0f;
		}
		int num = Grid.PosToCell(base.gameObject);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (!Grid.Transparent[num] && Grid.Solid[num] && this.simCellOccupier == null)
		{
			this.currDecor = 0f;
		}
		if (this.currDecor == 0f)
		{
			return;
		}
		this.cellCount = 0;
		int num2 = 5;
		if (this.decorRadius != null)
		{
			num2 = (int)this.decorRadius.GetTotalValue();
		}
		Extents extents = this.occupyArea.GetExtents();
		extents.x = Mathf.Max(extents.x - num2, 0);
		extents.y = Mathf.Max(extents.y - num2, 0);
		extents.width = Mathf.Min(extents.width + num2 * 2, Grid.WidthInCells - 1);
		extents.height = Mathf.Min(extents.height + num2 * 2, Grid.HeightInCells - 1);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatCollectDecorProviders", base.gameObject, extents, GameScenePartitioner.Instance.decorProviderLayer, this.onCollectDecorProvidersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatSolidCheck", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, this.refreshPartionerCallback);
		int num3 = extents.x + extents.width;
		int num4 = extents.y + extents.height;
		int num5 = extents.x;
		int num6 = extents.y;
		int x;
		int y;
		Grid.CellToXY(num, out x, out y);
		num3 = Math.Min(num3, Grid.WidthInCells);
		num4 = Math.Min(num4, Grid.HeightInCells);
		num5 = Math.Max(0, num5);
		num6 = Math.Max(0, num6);
		int num7 = (num3 - num5) * (num4 - num6);
		if (this.cells == null || this.cells.Length != num7)
		{
			this.cells = new int[num7];
		}
		for (int i = num5; i < num3; i++)
		{
			for (int j = num6; j < num4; j++)
			{
				if (Grid.VisibilityTest(x, y, i, j, false))
				{
					int num8 = Grid.XYToCell(i, j);
					if (Grid.IsValidCell(num8))
					{
						Grid.Decor[num8] += this.currDecor;
						int[] array = this.cells;
						int num9 = this.cellCount;
						this.cellCount = num9 + 1;
						array[num9] = num8;
					}
				}
			}
		}
	}

	// Token: 0x06005F36 RID: 24374 RVA: 0x000E29CF File Offset: 0x000E0BCF
	public void Clear()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		this.RemoveDecor();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
	}

	// Token: 0x06005F37 RID: 24375 RVA: 0x002B45F8 File Offset: 0x002B27F8
	private void RemoveDecor()
	{
		if (this.currDecor == 0f)
		{
			return;
		}
		for (int i = 0; i < this.cellCount; i++)
		{
			int num = this.cells[i];
			if (Grid.IsValidCell(num))
			{
				Grid.Decor[num] -= this.currDecor;
			}
		}
	}

	// Token: 0x06005F38 RID: 24376 RVA: 0x002B464C File Offset: 0x002B284C
	public void Refresh()
	{
		this.Clear();
		this.AddDecor();
		bool flag = this.prefabId.HasTag(RoomConstraints.ConstraintTags.Decor20);
		bool flag2 = this.decor.GetTotalValue() >= 20f;
		if (flag != flag2)
		{
			if (flag2)
			{
				this.prefabId.AddTag(RoomConstraints.ConstraintTags.Decor20, false);
			}
			else
			{
				this.prefabId.RemoveTag(RoomConstraints.ConstraintTags.Decor20);
			}
			int cell = Grid.PosToCell(this);
			if (Grid.IsValidCell(cell))
			{
				Game.Instance.roomProber.SolidChangedEvent(cell, true);
			}
		}
	}

	// Token: 0x06005F39 RID: 24377 RVA: 0x002B46D4 File Offset: 0x002B28D4
	public float GetDecorForCell(int cell)
	{
		for (int i = 0; i < this.cellCount; i++)
		{
			if (this.cells[i] == cell)
			{
				return this.currDecor;
			}
		}
		return 0f;
	}

	// Token: 0x06005F3A RID: 24378 RVA: 0x000E2A05 File Offset: 0x000E0C05
	public void SetValues(EffectorValues values)
	{
		this.baseDecor = (float)values.amount;
		this.baseRadius = (float)values.radius;
		if (base.IsInitialized())
		{
			this.UpdateBaseDecorModifiers();
		}
	}

	// Token: 0x06005F3B RID: 24379 RVA: 0x002B470C File Offset: 0x002B290C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.decor = this.GetAttributes().Add(Db.Get().BuildingAttributes.Decor);
		this.decorRadius = this.GetAttributes().Add(Db.Get().BuildingAttributes.DecorRadius);
		this.UpdateBaseDecorModifiers();
	}

	// Token: 0x06005F3C RID: 24380 RVA: 0x002B4768 File Offset: 0x002B2968
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.refreshCallback = new System.Action(this.Refresh);
		this.refreshPartionerCallback = delegate(object data)
		{
			this.Refresh();
		};
		this.onCollectDecorProvidersCallback = new Action<object>(this.OnCollectDecorProviders);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "DecorProvider.OnSpawn");
		AttributeInstance attributeInstance = this.decor;
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, this.refreshCallback);
		AttributeInstance attributeInstance2 = this.decorRadius;
		attributeInstance2.OnDirty = (System.Action)Delegate.Combine(attributeInstance2.OnDirty, this.refreshCallback);
		this.Refresh();
	}

	// Token: 0x06005F3D RID: 24381 RVA: 0x002B481C File Offset: 0x002B2A1C
	private void UpdateBaseDecorModifiers()
	{
		Attributes attributes = this.GetAttributes();
		if (this.baseDecorModifier != null)
		{
			attributes.Remove(this.baseDecorModifier);
			attributes.Remove(this.baseDecorRadiusModifier);
			this.baseDecorModifier = null;
			this.baseDecorRadiusModifier = null;
		}
		if (this.baseDecor != 0f)
		{
			this.baseDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, this.baseDecor, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			this.baseDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, this.baseRadius, UI.TOOLTIPS.BASE_VALUE, false, false, true);
			attributes.Add(this.baseDecorModifier);
			attributes.Add(this.baseDecorRadiusModifier);
		}
	}

	// Token: 0x06005F3E RID: 24382 RVA: 0x000E2A2F File Offset: 0x000E0C2F
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06005F3F RID: 24383 RVA: 0x000E2A37 File Offset: 0x000E0C37
	private void OnCollectDecorProviders(object data)
	{
		((List<DecorProvider>)data).Add(this);
	}

	// Token: 0x06005F40 RID: 24384 RVA: 0x000E2A45 File Offset: 0x000E0C45
	public string GetName()
	{
		if (string.IsNullOrEmpty(this.overrideName))
		{
			return base.GetComponent<KSelectable>().GetName();
		}
		return this.overrideName;
	}

	// Token: 0x06005F41 RID: 24385 RVA: 0x002B48E8 File Offset: 0x002B2AE8
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (base.isSpawned)
		{
			AttributeInstance attributeInstance = this.decor;
			attributeInstance.OnDirty = (System.Action)Delegate.Remove(attributeInstance.OnDirty, this.refreshCallback);
			AttributeInstance attributeInstance2 = this.decorRadius;
			attributeInstance2.OnDirty = (System.Action)Delegate.Remove(attributeInstance2.OnDirty, this.refreshCallback);
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		}
		this.Clear();
	}

	// Token: 0x06005F42 RID: 24386 RVA: 0x002B4968 File Offset: 0x002B2B68
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.decor != null && this.decorRadius != null)
		{
			float totalValue = this.decor.GetTotalValue();
			float totalValue2 = this.decorRadius.GetTotalValue();
			string arg = (this.baseDecor > 0f) ? "produced" : "consumed";
			string text = (this.baseDecor > 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			text = text + "\n\n" + this.decor.GetAttributeValueTooltip();
			string text2 = GameUtil.AddPositiveSign(totalValue.ToString(), totalValue > 0f);
			Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg, text2, totalValue2), string.Format(text, text2, totalValue2), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		else if (this.baseDecor != 0f)
		{
			string arg2 = (this.baseDecor >= 0f) ? "produced" : "consumed";
			string format = (this.baseDecor >= 0f) ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED;
			string text3 = GameUtil.AddPositiveSign(this.baseDecor.ToString(), this.baseDecor > 0f);
			Descriptor item2 = new Descriptor(string.Format(UI.BUILDINGEFFECTS.DECORPROVIDED, arg2, text3, this.baseRadius), string.Format(format, text3, this.baseRadius), Descriptor.DescriptorType.Effect, false);
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x06005F43 RID: 24387 RVA: 0x000E2A66 File Offset: 0x000E0C66
	public static int GetLightDecorBonus(int cell)
	{
		if (Grid.LightIntensity[cell] > 0)
		{
			return DECOR.LIT_BONUS;
		}
		return 0;
	}

	// Token: 0x06005F44 RID: 24388 RVA: 0x000E2A7D File Offset: 0x000E0C7D
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x040043FB RID: 17403
	public const string ID = "DecorProvider";

	// Token: 0x040043FC RID: 17404
	public float baseRadius;

	// Token: 0x040043FD RID: 17405
	public float baseDecor;

	// Token: 0x040043FE RID: 17406
	public string overrideName;

	// Token: 0x040043FF RID: 17407
	public System.Action refreshCallback;

	// Token: 0x04004400 RID: 17408
	public Action<object> refreshPartionerCallback;

	// Token: 0x04004401 RID: 17409
	public Action<object> onCollectDecorProvidersCallback;

	// Token: 0x04004402 RID: 17410
	public AttributeInstance decor;

	// Token: 0x04004403 RID: 17411
	public AttributeInstance decorRadius;

	// Token: 0x04004404 RID: 17412
	private AttributeModifier baseDecorModifier;

	// Token: 0x04004405 RID: 17413
	private AttributeModifier baseDecorRadiusModifier;

	// Token: 0x04004406 RID: 17414
	[MyCmpReq]
	private KPrefabID prefabId;

	// Token: 0x04004407 RID: 17415
	[MyCmpReq]
	public OccupyArea occupyArea;

	// Token: 0x04004408 RID: 17416
	[MyCmpGet]
	public SimCellOccupier simCellOccupier;

	// Token: 0x04004409 RID: 17417
	private int[] cells;

	// Token: 0x0400440A RID: 17418
	private int cellCount;

	// Token: 0x0400440B RID: 17419
	public float currDecor;

	// Token: 0x0400440C RID: 17420
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400440D RID: 17421
	private HandleVector<int>.Handle solidChangedPartitionerEntry;
}
