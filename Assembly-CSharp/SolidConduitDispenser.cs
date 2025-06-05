using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020018EC RID: 6380
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable, IConduitDispenser
{
	// Token: 0x1700086A RID: 2154
	// (get) Token: 0x060083F6 RID: 33782 RVA: 0x000FB47D File Offset: 0x000F967D
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x1700086B RID: 2155
	// (get) Token: 0x060083F7 RID: 33783 RVA: 0x000B1693 File Offset: 0x000AF893
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x1700086C RID: 2156
	// (get) Token: 0x060083F8 RID: 33784 RVA: 0x000FB485 File Offset: 0x000F9685
	public SolidConduitFlow.ConduitContents ConduitContents
	{
		get
		{
			return this.GetConduitFlow().GetContents(this.utilityCell);
		}
	}

	// Token: 0x1700086D RID: 2157
	// (get) Token: 0x060083F9 RID: 33785 RVA: 0x000FB498 File Offset: 0x000F9698
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x060083FA RID: 33786 RVA: 0x000D96D0 File Offset: 0x000D78D0
	public SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060083FB RID: 33787 RVA: 0x003507D4 File Offset: 0x0034E9D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetOutputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060083FC RID: 33788 RVA: 0x000FB4A0 File Offset: 0x000F96A0
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060083FD RID: 33789 RVA: 0x000FB4CF File Offset: 0x000F96CF
	private void OnConduitConnectionChanged(object data)
	{
		this.dispensing = (this.dispensing && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060083FE RID: 33790 RVA: 0x00350850 File Offset: 0x0034EA50
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
		if (this.operational.IsOperational || this.alwaysDispense)
		{
			SolidConduitFlow conduitFlow = this.GetConduitFlow();
			if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
			{
				Pickupable pickupable = this.FindSuitableItem();
				if (pickupable)
				{
					if (pickupable.PrimaryElement.Mass > 20f)
					{
						pickupable = pickupable.Take(20f);
					}
					conduitFlow.AddPickupable(this.utilityCell, pickupable);
					flag = true;
				}
			}
		}
		this.storage.storageNetworkID = this.GetConnectedNetworkID();
		this.dispensing = flag;
	}

	// Token: 0x060083FF RID: 33791 RVA: 0x00350904 File Offset: 0x0034EB04
	private bool isSolid(GameObject o)
	{
		PrimaryElement component = o.GetComponent<PrimaryElement>();
		return component != null && component.Element.IsSolid;
	}

	// Token: 0x06008400 RID: 33792 RVA: 0x00350930 File Offset: 0x0034EB30
	private Pickupable FindSuitableItem()
	{
		List<GameObject> items = this.storage.items;
		if (items.Count < 1)
		{
			return null;
		}
		this.round_robin_index %= items.Count;
		GameObject gameObject = items[this.round_robin_index];
		this.round_robin_index++;
		if (this.solidOnly && !this.isSolid(gameObject))
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < items.Count)
			{
				gameObject = items[(this.round_robin_index + num) % items.Count];
				if (this.isSolid(gameObject))
				{
					flag = true;
				}
				num++;
			}
			if (!flag)
			{
				return null;
			}
		}
		if (!gameObject)
		{
			return null;
		}
		return gameObject.GetComponent<Pickupable>();
	}

	// Token: 0x1700086E RID: 2158
	// (get) Token: 0x06008401 RID: 33793 RVA: 0x003509E0 File Offset: 0x0034EBE0
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x06008402 RID: 33794 RVA: 0x00350A18 File Offset: 0x0034EC18
	private int GetConnectedNetworkID()
	{
		GameObject gameObject = Grid.Objects[this.utilityCell, 20];
		SolidConduit solidConduit = (gameObject != null) ? gameObject.GetComponent<SolidConduit>() : null;
		UtilityNetwork utilityNetwork = (solidConduit != null) ? solidConduit.GetNetwork() : null;
		if (utilityNetwork == null)
		{
			return -1;
		}
		return utilityNetwork.id;
	}

	// Token: 0x06008403 RID: 33795 RVA: 0x00350A6C File Offset: 0x0034EC6C
	private int GetOutputCell()
	{
		Building component = base.GetComponent<Building>();
		if (this.useSecondaryOutput)
		{
			foreach (ISecondaryOutput secondaryOutput in base.GetComponents<ISecondaryOutput>())
			{
				if (secondaryOutput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(component.NaturalBuildingCell(), secondaryOutput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(component.NaturalBuildingCell(), CellOffset.none);
		}
		return component.GetUtilityOutputCell();
	}

	// Token: 0x0400647D RID: 25725
	[SerializeField]
	public SimHashes[] elementFilter;

	// Token: 0x0400647E RID: 25726
	[SerializeField]
	public bool invertElementFilter;

	// Token: 0x0400647F RID: 25727
	[SerializeField]
	public bool alwaysDispense;

	// Token: 0x04006480 RID: 25728
	[SerializeField]
	public bool useSecondaryOutput;

	// Token: 0x04006481 RID: 25729
	[SerializeField]
	public bool solidOnly;

	// Token: 0x04006482 RID: 25730
	private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);

	// Token: 0x04006483 RID: 25731
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04006484 RID: 25732
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04006485 RID: 25733
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04006486 RID: 25734
	private int utilityCell = -1;

	// Token: 0x04006487 RID: 25735
	private bool dispensing;

	// Token: 0x04006488 RID: 25736
	private int round_robin_index;
}
