using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D9B RID: 3483
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ElementFilter")]
public class ElementFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
{
	// Token: 0x060043AD RID: 17325 RVA: 0x000D0330 File Offset: 0x000CE530
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitializeStatusItems();
	}

	// Token: 0x060043AE RID: 17326 RVA: 0x00253760 File Offset: 0x00251960
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.inputCell = this.building.GetUtilityInputCell();
		this.outputCell = this.building.GetUtilityOutputCell();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		CellOffset rotatedOffset = this.building.GetRotatedOffset(this.portInfo.offset);
		this.filteredCell = Grid.OffsetCell(cell, rotatedOffset);
		IUtilityNetworkMgr utilityNetworkMgr = (this.portInfo.conduitType == ConduitType.Solid) ? SolidConduit.GetFlowManager().networkMgr : Conduit.GetNetworkManager(this.portInfo.conduitType);
		this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, base.gameObject);
		utilityNetworkMgr.AddToNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
		{
			base.GetComponent<ConduitConsumer>().isConsuming = false;
		}
		this.OnFilterChanged(this.filterable.SelectedTag);
		this.filterable.onFilterChanged += this.OnFilterChanged;
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, this);
		this.UpdateConduitExistsStatus();
		this.UpdateConduitBlockedStatus();
		ScenePartitionerLayer scenePartitionerLayer = null;
		switch (this.portInfo.conduitType)
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
		if (scenePartitionerLayer != null)
		{
			this.partitionerEntry = GameScenePartitioner.Instance.Add("ElementFilterConduitExists", base.gameObject, this.filteredCell, scenePartitionerLayer, delegate(object data)
			{
				this.UpdateConduitExistsStatus();
			});
		}
	}

	// Token: 0x060043AF RID: 17327 RVA: 0x0025396C File Offset: 0x00251B6C
	protected override void OnCleanUp()
	{
		Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, this.itemFilter, true);
		if (this.portInfo.conduitType == ConduitType.Solid)
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		else
		{
			Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new Action<float>(this.OnConduitTick));
		}
		if (this.partitionerEntry.IsValid() && GameScenePartitioner.Instance != null)
		{
			GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		}
		base.OnCleanUp();
	}

	// Token: 0x060043B0 RID: 17328 RVA: 0x00253A14 File Offset: 0x00251C14
	private void OnConduitTick(float dt)
	{
		bool value = false;
		this.UpdateConduitBlockedStatus();
		if (this.operational.IsOperational)
		{
			if (this.portInfo.conduitType == ConduitType.Gas || this.portInfo.conduitType == ConduitType.Liquid)
			{
				ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
				ConduitFlow.ConduitContents contents = flowManager.GetContents(this.inputCell);
				int num = (contents.element.CreateTag() == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
				ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
				if (contents.mass > 0f && contents2.mass <= 0f)
				{
					value = true;
					float num2 = flowManager.AddElement(num, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
					if (num2 > 0f)
					{
						flowManager.RemoveElement(this.inputCell, num2);
					}
				}
			}
			else
			{
				SolidConduitFlow flowManager2 = SolidConduit.GetFlowManager();
				SolidConduitFlow.ConduitContents contents3 = flowManager2.GetContents(this.inputCell);
				Pickupable pickupable = flowManager2.GetPickupable(contents3.pickupableHandle);
				if (pickupable != null)
				{
					int num3 = (pickupable.GetComponent<KPrefabID>().PrefabTag == this.filterable.SelectedTag) ? this.filteredCell : this.outputCell;
					SolidConduitFlow.ConduitContents contents4 = flowManager2.GetContents(num3);
					Pickupable pickupable2 = flowManager2.GetPickupable(contents4.pickupableHandle);
					PrimaryElement primaryElement = null;
					if (pickupable2 != null)
					{
						primaryElement = pickupable2.PrimaryElement;
					}
					if (pickupable.PrimaryElement.Mass > 0f && (pickupable2 == null || primaryElement.Mass <= 0f))
					{
						value = true;
						Pickupable pickupable3 = flowManager2.RemovePickupable(this.inputCell);
						if (pickupable3 != null)
						{
							flowManager2.AddPickupable(num3, pickupable3);
						}
					}
				}
				else
				{
					flowManager2.RemovePickupable(this.inputCell);
				}
			}
		}
		this.operational.SetActive(value, false);
	}

	// Token: 0x060043B1 RID: 17329 RVA: 0x00253C18 File Offset: 0x00251E18
	private void UpdateConduitExistsStatus()
	{
		bool flag = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
		StatusItem status_item;
		switch (this.portInfo.conduitType)
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
		default:
			throw new ArgumentOutOfRangeException();
		}
		bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x060043B2 RID: 17330 RVA: 0x00253CCC File Offset: 0x00251ECC
	private void UpdateConduitBlockedStatus()
	{
		bool flag = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
		StatusItem conduitBlockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
		bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
		if (flag == flag2)
		{
			this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(conduitBlockedMultiples, this.conduitBlockedStatusItemGuid, !flag, null);
		}
	}

	// Token: 0x060043B3 RID: 17331 RVA: 0x00253D38 File Offset: 0x00251F38
	private void OnFilterChanged(Tag tag)
	{
		bool on = !tag.IsValid || tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x060043B4 RID: 17332 RVA: 0x00253D7C File Offset: 0x00251F7C
	private void InitializeStatusItems()
	{
		if (ElementFilter.filterStatusItem == null)
		{
			ElementFilter.filterStatusItem = new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022, null);
			ElementFilter.filterStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				ElementFilter elementFilter = (ElementFilter)data;
				if (!elementFilter.filterable.SelectedTag.IsValid || elementFilter.filterable.SelectedTag == GameTags.Void)
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
				}
				else
				{
					str = string.Format(BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, elementFilter.filterable.SelectedTag.ProperName());
				}
				return str;
			};
			ElementFilter.filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(this.ShowInUtilityOverlay);
		}
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x00253DF8 File Offset: 0x00251FF8
	private bool ShowInUtilityOverlay(HashedString mode, object data)
	{
		bool result = false;
		switch (((ElementFilter)data).portInfo.conduitType)
		{
		case ConduitType.Gas:
			result = (mode == OverlayModes.GasConduits.ID);
			break;
		case ConduitType.Liquid:
			result = (mode == OverlayModes.LiquidConduits.ID);
			break;
		case ConduitType.Solid:
			result = (mode == OverlayModes.SolidConveyor.ID);
			break;
		}
		return result;
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x000D033E File Offset: 0x000CE53E
	public bool HasSecondaryConduitType(ConduitType type)
	{
		return this.portInfo.conduitType == type;
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x000D034E File Offset: 0x000CE54E
	public CellOffset GetSecondaryConduitOffset(ConduitType type)
	{
		return this.portInfo.offset;
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x000D035B File Offset: 0x000CE55B
	public int GetFilteredCell()
	{
		return this.filteredCell;
	}

	// Token: 0x04002EDA RID: 11994
	[SerializeField]
	public ConduitPortInfo portInfo;

	// Token: 0x04002EDB RID: 11995
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002EDC RID: 11996
	[MyCmpReq]
	private Building building;

	// Token: 0x04002EDD RID: 11997
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04002EDE RID: 11998
	[MyCmpReq]
	private Filterable filterable;

	// Token: 0x04002EDF RID: 11999
	private Guid needsConduitStatusItemGuid;

	// Token: 0x04002EE0 RID: 12000
	private Guid conduitBlockedStatusItemGuid;

	// Token: 0x04002EE1 RID: 12001
	private int inputCell = -1;

	// Token: 0x04002EE2 RID: 12002
	private int outputCell = -1;

	// Token: 0x04002EE3 RID: 12003
	private int filteredCell = -1;

	// Token: 0x04002EE4 RID: 12004
	private FlowUtilityNetwork.NetworkItem itemFilter;

	// Token: 0x04002EE5 RID: 12005
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04002EE6 RID: 12006
	private static StatusItem filterStatusItem;
}
