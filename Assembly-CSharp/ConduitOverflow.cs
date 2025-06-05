using System;
using UnityEngine;

// Token: 0x02000D3B RID: 3387
[AddComponentMenu("KMonoBehaviour/scripts/ConduitOverflow")]
public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput
{
	// Token: 0x0600418B RID: 16779 RVA: 0x0024C7F0 File Offset: 0x0024A9F0
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
		this.secondaryOutput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell2, base.gameObject);
		networkManager.AddToNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x0024C8B4 File Offset: 0x0024AAB4
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryOutput.Cell, this.secondaryOutput, true);
		Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x0024C910 File Offset: 0x0024AB10
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
		if (!flowManager.HasConduit(this.inputCell))
		{
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		if (contents.mass <= 0f)
		{
			return;
		}
		int cell = this.outputCell;
		ConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		if (contents2.mass > 0f)
		{
			cell = this.secondaryOutput.Cell;
			contents2 = flowManager.GetContents(cell);
		}
		if (contents2.mass <= 0f)
		{
			float num = flowManager.AddElement(cell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
			if (num > 0f)
			{
				flowManager.RemoveElement(this.inputCell, num);
			}
		}
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x000CEE0D File Offset: 0x000CD00D
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x000CEE1D File Offset: 0x000CD01D
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x04002D4C RID: 11596
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04002D4D RID: 11597
	private int inputCell;

	// Token: 0x04002D4E RID: 11598
	private int outputCell;

	// Token: 0x04002D4F RID: 11599
	private FlowUtilityNetwork.NetworkItem secondaryOutput;
}
