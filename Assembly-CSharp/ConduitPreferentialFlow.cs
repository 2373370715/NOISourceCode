using System;
using UnityEngine;

// Token: 0x02000D3C RID: 3388
[AddComponentMenu("KMonoBehaviour/scripts/ConduitPreferentialFlow")]
public class ConduitPreferentialFlow : KMonoBehaviour, ISecondaryInput
{
	// Token: 0x06004191 RID: 16785 RVA: 0x0024C9D8 File Offset: 0x0024ABD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = component.GetRotatedOffset(this.portInfo.offset);
		int cell2 = Grid.OffsetCell(cell, rotatedOffset);
		Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.secondaryInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x0024CA9C File Offset: 0x0024AC9C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInput.Cell, this.secondaryInput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x0024CAF8 File Offset: 0x0024ACF8
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.outputCell))
		{
			return;
		}
		int cell = this.inputCell;
		ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
		if (contents.mass <= 0f)
		{
			cell = this.secondaryInput.Cell;
			contents = flowManager.GetContents(cell);
		}
		if (contents.mass > 0f)
		{
			float num = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(cell, num);
			}
		}
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x000CEE2A File Offset: 0x000CD02A
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x000CEE3A File Offset: 0x000CD03A
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		if (this.portInfo.conduitType == type)
		{
			return this.portInfo.offset;
		}
		return CellOffset.none;
	}

	// Token: 0x04002D50 RID: 11600
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04002D51 RID: 11601
	private int inputCell;

	// Token: 0x04002D52 RID: 11602
	private int outputCell;

	// Token: 0x04002D53 RID: 11603
	private FlowUtilityNetwork.NetworkItem secondaryInput;
}
