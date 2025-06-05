using System;
using UnityEngine;

// Token: 0x020011B6 RID: 4534
[AddComponentMenu("KMonoBehaviour/scripts/EntityPreview")]
public class EntityPreview : KMonoBehaviour
{
	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06005C23 RID: 23587 RVA: 0x000E07BC File Offset: 0x000DE9BC
	// (set) Token: 0x06005C24 RID: 23588 RVA: 0x000E07C4 File Offset: 0x000DE9C4
	public bool Valid { get; private set; }

	// Token: 0x06005C25 RID: 23589 RVA: 0x002A82C8 File Offset: 0x002A64C8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.solidPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnAreaChanged));
		if (this.objectLayer != ObjectLayer.NumLayers)
		{
			this.objectPartitionerEntry = GameScenePartitioner.Instance.Add("EntityPreview", base.gameObject, this.occupyArea.GetExtents(), GameScenePartitioner.Instance.objectLayers[(int)this.objectLayer], new Action<object>(this.OnAreaChanged));
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "EntityPreview.OnSpawn");
		this.OnAreaChanged(null);
	}

	// Token: 0x06005C26 RID: 23590 RVA: 0x002A8390 File Offset: 0x002A6590
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.solidPartitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.objectPartitionerEntry);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x06005C27 RID: 23591 RVA: 0x000E07CD File Offset: 0x000DE9CD
	private void OnCellChange()
	{
		GameScenePartitioner.Instance.UpdatePosition(this.solidPartitionerEntry, this.occupyArea.GetExtents());
		GameScenePartitioner.Instance.UpdatePosition(this.objectPartitionerEntry, this.occupyArea.GetExtents());
		this.OnAreaChanged(null);
	}

	// Token: 0x06005C28 RID: 23592 RVA: 0x000E080C File Offset: 0x000DEA0C
	public void SetSolid()
	{
		this.occupyArea.ApplyToCells = true;
	}

	// Token: 0x06005C29 RID: 23593 RVA: 0x000E081A File Offset: 0x000DEA1A
	private void OnAreaChanged(object obj)
	{
		this.UpdateValidity();
	}

	// Token: 0x06005C2A RID: 23594 RVA: 0x002A83E0 File Offset: 0x002A65E0
	public void UpdateValidity()
	{
		bool valid = this.Valid;
		this.Valid = this.occupyArea.TestArea(Grid.PosToCell(this), this, EntityPreview.ValidTestDelegate);
		if (this.Valid)
		{
			this.animController.TintColour = Color.white;
		}
		else
		{
			this.animController.TintColour = Color.red;
		}
		if (valid != this.Valid)
		{
			base.Trigger(-1820564715, this.Valid);
		}
	}

	// Token: 0x06005C2B RID: 23595 RVA: 0x002A8464 File Offset: 0x002A6664
	private static bool ValidTest(int cell, object data)
	{
		EntityPreview entityPreview = (EntityPreview)data;
		return Grid.IsValidCell(cell) && !Grid.Solid[cell] && (entityPreview.objectLayer == ObjectLayer.NumLayers || Grid.Objects[cell, (int)entityPreview.objectLayer] == entityPreview.gameObject || Grid.Objects[cell, (int)entityPreview.objectLayer] == null);
	}

	// Token: 0x040041A0 RID: 16800
	[MyCmpReq]
	private OccupyArea occupyArea;

	// Token: 0x040041A1 RID: 16801
	[MyCmpReq]
	private KBatchedAnimController animController;

	// Token: 0x040041A2 RID: 16802
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040041A3 RID: 16803
	public ObjectLayer objectLayer = ObjectLayer.NumLayers;

	// Token: 0x040041A5 RID: 16805
	private HandleVector<int>.Handle solidPartitionerEntry;

	// Token: 0x040041A6 RID: 16806
	private HandleVector<int>.Handle objectPartitionerEntry;

	// Token: 0x040041A7 RID: 16807
	private static readonly Func<int, object, bool> ValidTestDelegate = (int cell, object data) => EntityPreview.ValidTest(cell, data);
}
