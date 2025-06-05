using System;
using UnityEngine;

// Token: 0x02000CAA RID: 3242
[AddComponentMenu("KMonoBehaviour/scripts/BuildingConduitEndpoints")]
public class BuildingConduitEndpoints : KMonoBehaviour
{
	// Token: 0x06003DAB RID: 15787 RVA: 0x000CC602 File Offset: 0x000CA802
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.AddEndpoint();
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x000CC610 File Offset: 0x000CA810
	protected override void OnCleanUp()
	{
		this.RemoveEndPoint();
		base.OnCleanUp();
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x00240578 File Offset: 0x0023E778
	public void RemoveEndPoint()
	{
		if (this.itemInput != null)
		{
			if (this.itemInput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, this.itemInput, true);
			}
			this.itemInput = null;
		}
		if (this.itemOutput != null)
		{
			if (this.itemOutput.ConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			else
			{
				Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, this.itemOutput, true);
			}
			this.itemOutput = null;
		}
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x00240654 File Offset: 0x0023E854
	public void AddEndpoint()
	{
		Building component = base.GetComponent<Building>();
		BuildingDef def = component.Def;
		this.RemoveEndPoint();
		if (def.InputConduitType != ConduitType.None)
		{
			int utilityInputCell = component.GetUtilityInputCell();
			this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, base.gameObject);
			if (def.InputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, this.itemInput, true);
			}
			else
			{
				Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, this.itemInput, true);
			}
		}
		if (def.OutputConduitType != ConduitType.None)
		{
			int utilityOutputCell = component.GetUtilityOutputCell();
			this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, base.gameObject);
			if (def.OutputConduitType == ConduitType.Solid)
			{
				Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, this.itemOutput, true);
				return;
			}
			Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, this.itemOutput, true);
		}
	}

	// Token: 0x04002A8F RID: 10895
	private FlowUtilityNetwork.NetworkItem itemInput;

	// Token: 0x04002A90 RID: 10896
	private FlowUtilityNetwork.NetworkItem itemOutput;
}
