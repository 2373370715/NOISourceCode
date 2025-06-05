using System;
using Rendering;
using UnityEngine;

// Token: 0x02000A9F RID: 2719
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimGridTileVisualizer")]
public class KAnimGridTileVisualizer : KMonoBehaviour, IBlockTileInfo
{
	// Token: 0x06003183 RID: 12675 RVA: 0x000C4987 File Offset: 0x000C2B87
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<KAnimGridTileVisualizer>(-1503271301, KAnimGridTileVisualizer.OnSelectionChangedDelegate);
		base.Subscribe<KAnimGridTileVisualizer>(-1201923725, KAnimGridTileVisualizer.OnHighlightChangedDelegate);
	}

	// Token: 0x06003184 RID: 12676 RVA: 0x0020D0F8 File Offset: 0x0020B2F8
	protected override void OnCleanUp()
	{
		Building component = base.GetComponent<Building>();
		if (component != null)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			ObjectLayer tileLayer = component.Def.TileLayer;
			if (Grid.Objects[cell, (int)tileLayer] == base.gameObject)
			{
				Grid.Objects[cell, (int)tileLayer] = null;
			}
			TileVisualizer.RefreshCell(cell, tileLayer, component.Def.ReplacementLayer);
		}
		base.OnCleanUp();
	}

	// Token: 0x06003185 RID: 12677 RVA: 0x0020D170 File Offset: 0x0020B370
	private void OnSelectionChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.SelectCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x06003186 RID: 12678 RVA: 0x0020D1A4 File Offset: 0x0020B3A4
	private void OnHighlightChanged(object data)
	{
		bool enabled = (bool)data;
		World.Instance.blockTileRenderer.HighlightCell(Grid.PosToCell(base.transform.GetPosition()), enabled);
	}

	// Token: 0x06003187 RID: 12679 RVA: 0x000C49B1 File Offset: 0x000C2BB1
	public int GetBlockTileConnectorID()
	{
		return this.blockTileConnectorID;
	}

	// Token: 0x04002204 RID: 8708
	[SerializeField]
	public int blockTileConnectorID;

	// Token: 0x04002205 RID: 8709
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnSelectionChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnSelectionChanged(data);
	});

	// Token: 0x04002206 RID: 8710
	private static readonly EventSystem.IntraObjectHandler<KAnimGridTileVisualizer> OnHighlightChangedDelegate = new EventSystem.IntraObjectHandler<KAnimGridTileVisualizer>(delegate(KAnimGridTileVisualizer component, object data)
	{
		component.OnHighlightChanged(data);
	});
}
