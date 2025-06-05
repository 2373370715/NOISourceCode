using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D37 RID: 3383
[AddComponentMenu("KMonoBehaviour/scripts/ConduitBridge")]
public class ConduitBridge : ConduitBridgeBase, IBridgedNetworkItem
{
	// Token: 0x06004179 RID: 16761 RVA: 0x000CED6E File Offset: 0x000CCF6E
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.accumulator = Game.Instance.accumulators.Add("Flow", this);
	}

	// Token: 0x0600417A RID: 16762 RVA: 0x0024C5DC File Offset: 0x0024A7DC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		Conduit.GetFlowManager(this.type).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x0600417B RID: 16763 RVA: 0x000CED91 File Offset: 0x000CCF91
	protected override void OnCleanUp()
	{
		Conduit.GetFlowManager(this.type).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		Game.Instance.accumulators.Remove(this.accumulator);
		base.OnCleanUp();
	}

	// Token: 0x0600417C RID: 16764 RVA: 0x0024C62C File Offset: 0x0024A82C
	private void ConduitUpdate(float dt)
	{
		ConduitFlow flowManager = Conduit.GetFlowManager(this.type);
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
		float num = contents.mass;
		if (this.desiredMassTransfer != null)
		{
			num = this.desiredMassTransfer(dt, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount, null);
		}
		if (num > 0f)
		{
			int disease_count = (int)(num / contents.mass * (float)contents.diseaseCount);
			float num2 = flowManager.AddElement(this.outputCell, contents.element, num, contents.temperature, contents.diseaseIdx, disease_count);
			if (num2 <= 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			flowManager.RemoveElement(this.inputCell, num2);
			Game.Instance.accumulators.Accumulate(this.accumulator, contents.mass);
			if (this.OnMassTransfer != null)
			{
				this.OnMassTransfer(contents.element, num2, contents.temperature, contents.diseaseIdx, disease_count, null);
				return;
			}
		}
		else
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x0600417D RID: 16765 RVA: 0x0024C760 File Offset: 0x0024A960
	public void AddNetworks(ICollection<UtilityNetwork> networks)
	{
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		UtilityNetwork networkForCell = networkManager.GetNetworkForCell(this.inputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
		networkForCell = networkManager.GetNetworkForCell(this.outputCell);
		if (networkForCell != null)
		{
			networks.Add(networkForCell);
		}
	}

	// Token: 0x0600417E RID: 16766 RVA: 0x0024C7A8 File Offset: 0x0024A9A8
	public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
	{
		bool flag = false;
		IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.type);
		return flag || networks.Contains(networkManager.GetNetworkForCell(this.inputCell)) || networks.Contains(networkManager.GetNetworkForCell(this.outputCell));
	}

	// Token: 0x0600417F RID: 16767 RVA: 0x000CEDCB File Offset: 0x000CCFCB
	public int GetNetworkCell()
	{
		return this.inputCell;
	}

	// Token: 0x04002D46 RID: 11590
	[SerializeField]
	public ConduitType type;

	// Token: 0x04002D47 RID: 11591
	private int inputCell;

	// Token: 0x04002D48 RID: 11592
	private int outputCell;

	// Token: 0x04002D49 RID: 11593
	private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;
}
