using System;
using System.Collections.Generic;
using Klei;
using Rendering;
using UnityEngine;

// Token: 0x02001A9E RID: 6814
[AddComponentMenu("KMonoBehaviour/scripts/World")]
public class World : KMonoBehaviour
{
	// Token: 0x1700094A RID: 2378
	// (get) Token: 0x06008E2F RID: 36399 RVA: 0x0010159E File Offset: 0x000FF79E
	// (set) Token: 0x06008E30 RID: 36400 RVA: 0x001015A5 File Offset: 0x000FF7A5
	public static World Instance { get; private set; }

	// Token: 0x1700094B RID: 2379
	// (get) Token: 0x06008E31 RID: 36401 RVA: 0x001015AD File Offset: 0x000FF7AD
	// (set) Token: 0x06008E32 RID: 36402 RVA: 0x001015B5 File Offset: 0x000FF7B5
	public SubworldZoneRenderData zoneRenderData { get; private set; }

	// Token: 0x06008E33 RID: 36403 RVA: 0x001015BE File Offset: 0x000FF7BE
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(World.Instance == null);
		World.Instance = this;
		this.blockTileRenderer = base.GetComponent<BlockTileRenderer>();
	}

	// Token: 0x06008E34 RID: 36404 RVA: 0x00378564 File Offset: 0x00376764
	protected override void OnSpawn()
	{
		base.GetComponent<SimDebugView>().OnReset();
		base.GetComponent<PropertyTextures>().OnReset(null);
		this.zoneRenderData = base.GetComponent<SubworldZoneRenderData>();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x06008E35 RID: 36405 RVA: 0x003785B4 File Offset: 0x003767B4
	protected override void OnLoadLevel()
	{
		World.Instance = null;
		if (this.blockTileRenderer != null)
		{
			this.blockTileRenderer.FreeResources();
		}
		this.blockTileRenderer = null;
		if (this.groundRenderer != null)
		{
			this.groundRenderer.FreeResources();
		}
		this.groundRenderer = null;
		this.revealedCells.Clear();
		this.revealedCells = null;
		base.OnLoadLevel();
	}

	// Token: 0x06008E36 RID: 36406 RVA: 0x00378620 File Offset: 0x00376820
	public unsafe void UpdateCellInfo(List<SolidInfo> solidInfo, List<CallbackInfo> callbackInfo, int num_solid_substance_change_info, Sim.SolidSubstanceChangeInfo* solid_substance_change_info, int num_liquid_change_info, Sim.LiquidChangeInfo* liquid_change_info)
	{
		int count = solidInfo.Count;
		this.changedCells.Clear();
		for (int i = 0; i < count; i++)
		{
			int cellIdx = solidInfo[i].cellIdx;
			if (!this.changedCells.Contains(cellIdx))
			{
				this.changedCells.Add(cellIdx);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(cellIdx);
			WorldDamage.Instance.OnSolidStateChanged(cellIdx);
			if (this.OnSolidChanged != null)
			{
				this.OnSolidChanged(cellIdx);
			}
		}
		if (this.changedCells.Count != 0)
		{
			SaveGame.Instance.entombedItemManager.OnSolidChanged(this.changedCells);
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
		int count2 = callbackInfo.Count;
		for (int j = 0; j < count2; j++)
		{
			callbackInfo[j].Release();
		}
		for (int k = 0; k < num_solid_substance_change_info; k++)
		{
			int cellIdx2 = solid_substance_change_info[k].cellIdx;
			if (!Grid.IsValidCell(cellIdx2))
			{
				global::Debug.LogError(cellIdx2);
			}
			else
			{
				Grid.RenderedByWorld[cellIdx2] = (Grid.Element[cellIdx2].substance.renderedByWorld && Grid.Objects[cellIdx2, 9] == null);
				this.groundRenderer.MarkDirty(cellIdx2);
			}
		}
		GameScenePartitioner instance = GameScenePartitioner.Instance;
		this.changedCells.Clear();
		for (int l = 0; l < num_liquid_change_info; l++)
		{
			int cellIdx3 = liquid_change_info[l].cellIdx;
			this.changedCells.Add(cellIdx3);
			if (this.OnLiquidChanged != null)
			{
				this.OnLiquidChanged(cellIdx3);
			}
		}
		instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.liquidChangedLayer, null);
	}

	// Token: 0x06008E37 RID: 36407 RVA: 0x001015E2 File Offset: 0x000FF7E2
	private void OnReveal(int cell)
	{
		this.revealedCells.Add(cell);
	}

	// Token: 0x06008E38 RID: 36408 RVA: 0x003787F8 File Offset: 0x003769F8
	private void LateUpdate()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (GameUtil.IsCapturingTimeLapse())
		{
			this.groundRenderer.RenderAll();
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		else
		{
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			this.groundRenderer.Render(visibleArea.Min, visibleArea.Max, false);
			Vector2I vis_chunk_min;
			Vector2I vis_chunk_max;
			Singleton<KBatchedAnimUpdater>.Instance.GetVisibleArea(out vis_chunk_min, out vis_chunk_max);
			KAnimBatchManager.Instance().UpdateActiveArea(vis_chunk_min, vis_chunk_max);
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		if (Camera.main != null)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -Camera.main.transform.GetPosition().z));
			Shader.SetGlobalVector("_CursorPos", new Vector4(vector.x, vector.y, vector.z, 0f));
		}
		FallingWater.instance.UpdateParticles(Time.deltaTime);
		FallingWater.instance.Render();
		if (this.revealedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.revealedCells, GameScenePartitioner.Instance.fogOfWarChangedLayer, null);
			this.revealedCells.Clear();
		}
	}

	// Token: 0x04006B43 RID: 27459
	public Action<int> OnSolidChanged;

	// Token: 0x04006B44 RID: 27460
	public Action<int> OnLiquidChanged;

	// Token: 0x04006B46 RID: 27462
	public BlockTileRenderer blockTileRenderer;

	// Token: 0x04006B47 RID: 27463
	[MyCmpGet]
	[NonSerialized]
	public GroundRenderer groundRenderer;

	// Token: 0x04006B48 RID: 27464
	private List<int> revealedCells = new List<int>();

	// Token: 0x04006B49 RID: 27465
	public static int DebugCellID = -1;

	// Token: 0x04006B4A RID: 27466
	private List<int> changedCells = new List<int>();
}
