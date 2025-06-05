using System;
using UnityEngine;

// Token: 0x02000CC1 RID: 3265
public class BuildingUnderConstruction : Building
{
	// Token: 0x06003E42 RID: 15938 RVA: 0x00242054 File Offset: 0x00240254
	protected override void OnPrefabInit()
	{
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(this.Def.SceneLayer);
		base.transform.SetPosition(position);
		base.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Construction"));
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		Rotatable component2 = base.GetComponent<Rotatable>();
		if (component != null && component2 == null)
		{
			component.Offset = this.Def.GetVisualizerOffset();
		}
		KBoxCollider2D component3 = base.GetComponent<KBoxCollider2D>();
		if (component3 != null)
		{
			Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
			component3.offset += new Vector2(visualizerOffset.x, visualizerOffset.y);
		}
		base.OnPrefabInit();
	}

	// Token: 0x06003E43 RID: 15939 RVA: 0x00242120 File Offset: 0x00240320
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.Def.IsTilePiece)
		{
			int cell = Grid.PosToCell(base.transform.GetPosition());
			this.Def.RunOnArea(cell, base.Orientation, delegate(int c)
			{
				TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer);
			});
		}
		base.RegisterBlockTileRenderer();
	}

	// Token: 0x06003E44 RID: 15940 RVA: 0x000CCD11 File Offset: 0x000CAF11
	protected override void OnCleanUp()
	{
		base.UnregisterBlockTileRenderer();
		base.OnCleanUp();
	}

	// Token: 0x04002AF3 RID: 10995
	[MyCmpAdd]
	private KSelectable selectable;

	// Token: 0x04002AF4 RID: 10996
	[MyCmpAdd]
	private SaveLoadRoot saveLoadRoot;

	// Token: 0x04002AF5 RID: 10997
	[MyCmpAdd]
	private KPrefabID kPrefabID;
}
