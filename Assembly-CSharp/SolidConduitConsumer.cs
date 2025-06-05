using System;
using UnityEngine;

// Token: 0x020018EB RID: 6379
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitConsumer")]
public class SolidConduitConsumer : KMonoBehaviour, IConduitConsumer
{
	// Token: 0x17000866 RID: 2150
	// (get) Token: 0x060083EA RID: 33770 RVA: 0x000FB3EA File Offset: 0x000F95EA
	public Storage Storage
	{
		get
		{
			return this.storage;
		}
	}

	// Token: 0x17000867 RID: 2151
	// (get) Token: 0x060083EB RID: 33771 RVA: 0x000B1693 File Offset: 0x000AF893
	public ConduitType ConduitType
	{
		get
		{
			return ConduitType.Solid;
		}
	}

	// Token: 0x17000868 RID: 2152
	// (get) Token: 0x060083EC RID: 33772 RVA: 0x000FB3F2 File Offset: 0x000F95F2
	public bool IsConsuming
	{
		get
		{
			return this.consuming;
		}
	}

	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x060083ED RID: 33773 RVA: 0x0035051C File Offset: 0x0034E71C
	public bool IsConnected
	{
		get
		{
			GameObject gameObject = Grid.Objects[this.utilityCell, 20];
			return gameObject != null && gameObject.GetComponent<BuildingComplete>() != null;
		}
	}

	// Token: 0x060083EE RID: 33774 RVA: 0x000D96D0 File Offset: 0x000D78D0
	private SolidConduitFlow GetConduitFlow()
	{
		return Game.Instance.solidConduitFlow;
	}

	// Token: 0x060083EF RID: 33775 RVA: 0x00350554 File Offset: 0x0034E754
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.utilityCell = this.GetInputCell();
		ScenePartitionerLayer layer = GameScenePartitioner.Instance.objectLayers[20];
		this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", base.gameObject, this.utilityCell, layer, new Action<object>(this.OnConduitConnectionChanged));
		this.GetConduitFlow().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
		this.OnConduitConnectionChanged(null);
	}

	// Token: 0x060083F0 RID: 33776 RVA: 0x000FB3FA File Offset: 0x000F95FA
	protected override void OnCleanUp()
	{
		this.GetConduitFlow().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x060083F1 RID: 33777 RVA: 0x000FB429 File Offset: 0x000F9629
	private void OnConduitConnectionChanged(object data)
	{
		this.consuming = (this.consuming && this.IsConnected);
		base.Trigger(-2094018600, this.IsConnected);
	}

	// Token: 0x060083F2 RID: 33778 RVA: 0x003505D0 File Offset: 0x0034E7D0
	private void ConduitUpdate(float dt)
	{
		bool flag = false;
		SolidConduitFlow conduitFlow = this.GetConduitFlow();
		if (this.IsConnected)
		{
			SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
			if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
			{
				float num = (this.capacityTag != GameTags.Any) ? this.storage.GetMassAvailable(this.capacityTag) : this.storage.MassStored();
				float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
				float num3 = Mathf.Max(0f, num2 - num);
				if (num3 > 0f)
				{
					Pickupable pickupable = conduitFlow.GetPickupable(contents.pickupableHandle);
					if (pickupable.PrimaryElement.Mass <= num3 || pickupable.PrimaryElement.Mass > num2)
					{
						Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
						if (pickupable2)
						{
							this.storage.Store(pickupable2.gameObject, true, false, true, false);
							flag = true;
						}
					}
				}
			}
		}
		if (this.storage != null)
		{
			this.storage.storageNetworkID = this.GetConnectedNetworkID();
		}
		this.consuming = flag;
	}

	// Token: 0x060083F3 RID: 33779 RVA: 0x00350710 File Offset: 0x0034E910
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

	// Token: 0x060083F4 RID: 33780 RVA: 0x00350764 File Offset: 0x0034E964
	private int GetInputCell()
	{
		if (this.useSecondaryInput)
		{
			foreach (ISecondaryInput secondaryInput in base.GetComponents<ISecondaryInput>())
			{
				if (secondaryInput.HasSecondaryConduitType(ConduitType.Solid))
				{
					return Grid.OffsetCell(this.building.NaturalBuildingCell(), secondaryInput.GetSecondaryConduitOffset(ConduitType.Solid));
				}
			}
			return Grid.OffsetCell(this.building.NaturalBuildingCell(), CellOffset.none);
		}
		return this.building.GetUtilityInputCell();
	}

	// Token: 0x04006473 RID: 25715
	[SerializeField]
	public Tag capacityTag = GameTags.Any;

	// Token: 0x04006474 RID: 25716
	[SerializeField]
	public float capacityKG = float.PositiveInfinity;

	// Token: 0x04006475 RID: 25717
	[SerializeField]
	public bool alwaysConsume;

	// Token: 0x04006476 RID: 25718
	[SerializeField]
	public bool useSecondaryInput;

	// Token: 0x04006477 RID: 25719
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04006478 RID: 25720
	[MyCmpReq]
	private Building building;

	// Token: 0x04006479 RID: 25721
	[MyCmpGet]
	public Storage storage;

	// Token: 0x0400647A RID: 25722
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x0400647B RID: 25723
	private int utilityCell = -1;

	// Token: 0x0400647C RID: 25724
	private bool consuming;
}
