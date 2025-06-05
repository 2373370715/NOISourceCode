using System;
using KSerialization;

// Token: 0x02000D42 RID: 3394
[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
	// Token: 0x060041D3 RID: 16851 RVA: 0x000CEFFE File Offset: 0x000CD1FE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filterable.onFilterChanged += this.OnFilterChanged;
		this.OnFilterChanged(this.filterable.SelectedTag);
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x0024D1A8 File Offset: 0x0024B3A8
	private void OnFilterChanged(Tag tag)
	{
		if (!tag.IsValid)
		{
			return;
		}
		bool on = tag == GameTags.Void;
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x060041D5 RID: 16853 RVA: 0x0024D1E8 File Offset: 0x0024B3E8
	protected override void ConduitUpdate(float dt)
	{
		Tag a;
		bool flag;
		this.GetContentsElement(out a, out flag);
		if (!base.IsSwitchedOn)
		{
			if (a == this.filterable.SelectedTag && flag)
			{
				this.Toggle();
				return;
			}
		}
		else if (a != this.filterable.SelectedTag || !flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x060041D6 RID: 16854 RVA: 0x0024D240 File Offset: 0x0024B440
	private void GetContentsElement(out Tag element, out bool hasMass)
	{
		int cell = Grid.PosToCell(this);
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(cell);
			element = contents.element.CreateTag();
			hasMass = (contents.mass > 0f);
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		SolidConduitFlow.ConduitContents contents2 = flowManager.GetContents(cell);
		Pickupable pickupable = flowManager.GetPickupable(contents2.pickupableHandle);
		KPrefabID kprefabID = (pickupable != null) ? pickupable.GetComponent<KPrefabID>() : null;
		if (kprefabID != null && pickupable.PrimaryElement.Mass > 0f)
		{
			element = kprefabID.PrefabTag;
			hasMass = true;
			return;
		}
		element = GameTags.Void;
		hasMass = false;
	}

	// Token: 0x04002D66 RID: 11622
	[MyCmpGet]
	private Filterable filterable;
}
