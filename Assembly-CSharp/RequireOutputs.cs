using System;
using UnityEngine;

// Token: 0x020017EC RID: 6124
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireOutputs")]
public class RequireOutputs : KMonoBehaviour
{
	// Token: 0x06007DF8 RID: 32248 RVA: 0x00334E48 File Offset: 0x00333048
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ScenePartitionerLayer scenePartitionerLayer = null;
		Building component = base.GetComponent<Building>();
		this.utilityCell = component.GetUtilityOutputCell();
		this.conduitType = component.Def.OutputConduitType;
		switch (component.Def.OutputConduitType)
		{
		case ConduitType.Gas:
			scenePartitionerLayer = GameScenePartitioner.Instance.gasConduitsLayer;
			break;
		case ConduitType.Liquid:
			scenePartitionerLayer = GameScenePartitioner.Instance.liquidConduitsLayer;
			break;
		case ConduitType.Solid:
			scenePartitionerLayer = GameScenePartitioner.Instance.solidConduitsLayer;
			break;
		}
		this.UpdateConnectionState(true);
		this.UpdatePipeRoomState(true);
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("RequireOutputs", base.gameObject, this.utilityCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConnectionState(false);
			});
		}
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
	}

	// Token: 0x06007DF9 RID: 32249 RVA: 0x00334F20 File Offset: 0x00333120
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		IConduitFlow conduitFlow = this.GetConduitFlow();
		if (conduitFlow != null)
		{
			conduitFlow.RemoveConduitUpdater(new Action<float>(this.UpdatePipeState));
		}
		base.OnCleanUp();
	}

	// Token: 0x06007DFA RID: 32250 RVA: 0x00334F60 File Offset: 0x00333160
	private void UpdateConnectionState(bool force_update = false)
	{
		this.connected = this.IsConnected(this.utilityCell);
		if (this.connected != this.previouslyConnected || force_update)
		{
			this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
			this.previouslyConnected = this.connected;
			StatusItem status_item = null;
			switch (this.conduitType)
			{
			case ConduitType.Gas:
				status_item = Db.Get().BuildingStatusItems.NeedGasOut;
				break;
			case ConduitType.Liquid:
				status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
				break;
			case ConduitType.Solid:
				status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
				break;
			}
			this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, this);
		}
	}

	// Token: 0x06007DFB RID: 32251 RVA: 0x00335030 File Offset: 0x00333230
	private bool OutputPipeIsEmpty()
	{
		if (this.ignoreFullPipe)
		{
			return true;
		}
		bool result = true;
		if (this.connected)
		{
			result = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
		}
		return result;
	}

	// Token: 0x06007DFC RID: 32252 RVA: 0x000F7796 File Offset: 0x000F5996
	private void UpdatePipeState(float dt)
	{
		this.UpdatePipeRoomState(false);
	}

	// Token: 0x06007DFD RID: 32253 RVA: 0x00335064 File Offset: 0x00333264
	private void UpdatePipeRoomState(bool force_update = false)
	{
		bool flag = this.OutputPipeIsEmpty();
		if (flag != this.previouslyHadRoom || force_update)
		{
			this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
			this.previouslyHadRoom = flag;
			StatusItem status_item = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
			if (this.conduitType == ConduitType.Solid)
			{
				status_item = Db.Get().BuildingStatusItems.SolidConduitBlockedMultiples;
			}
			this.pipeBlockedGuid = this.selectable.ToggleStatusItem(status_item, this.pipeBlockedGuid, !flag, null);
		}
	}

	// Token: 0x06007DFE RID: 32254 RVA: 0x003350E8 File Offset: 0x003332E8
	private IConduitFlow GetConduitFlow()
	{
		switch (this.conduitType)
		{
		case ConduitType.Gas:
			return Game.Instance.gasConduitFlow;
		case ConduitType.Liquid:
			return Game.Instance.liquidConduitFlow;
		case ConduitType.Solid:
			return Game.Instance.solidConduitFlow;
		default:
			global::Debug.LogWarning("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString());
			return null;
		}
	}

	// Token: 0x06007DFF RID: 32255 RVA: 0x000F779F File Offset: 0x000F599F
	private bool IsConnected(int cell)
	{
		return RequireOutputs.IsConnected(cell, this.conduitType);
	}

	// Token: 0x06007E00 RID: 32256 RVA: 0x00335154 File Offset: 0x00333354
	public static bool IsConnected(int cell, ConduitType conduitType)
	{
		ObjectLayer layer = ObjectLayer.NumLayers;
		switch (conduitType)
		{
		case ConduitType.Gas:
			layer = ObjectLayer.GasConduit;
			break;
		case ConduitType.Liquid:
			layer = ObjectLayer.LiquidConduit;
			break;
		case ConduitType.Solid:
			layer = ObjectLayer.SolidConduit;
			break;
		}
		GameObject gameObject = Grid.Objects[cell, (int)layer];
		return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
	}

	// Token: 0x04005FBA RID: 24506
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005FBB RID: 24507
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04005FBC RID: 24508
	public bool ignoreFullPipe;

	// Token: 0x04005FBD RID: 24509
	private int utilityCell;

	// Token: 0x04005FBE RID: 24510
	private ConduitType conduitType;

	// Token: 0x04005FBF RID: 24511
	private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);

	// Token: 0x04005FC0 RID: 24512
	private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);

	// Token: 0x04005FC1 RID: 24513
	private bool previouslyConnected = true;

	// Token: 0x04005FC2 RID: 24514
	private bool previouslyHadRoom = true;

	// Token: 0x04005FC3 RID: 24515
	private bool connected;

	// Token: 0x04005FC4 RID: 24516
	private Guid hasPipeGuid;

	// Token: 0x04005FC5 RID: 24517
	private Guid pipeBlockedGuid;

	// Token: 0x04005FC6 RID: 24518
	private HandleVector<int>.Handle partitionerEntry;
}
