using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000E46 RID: 3654
[AddComponentMenu("KMonoBehaviour/scripts/ItemPedestal")]
public class ItemPedestal : KMonoBehaviour
{
	// Token: 0x06004777 RID: 18295 RVA: 0x00260840 File Offset: 0x0025EA40
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
		if (this.receptacle.Occupant)
		{
			KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
			if (component)
			{
				component.enabled = true;
				component.sceneLayer = Grid.SceneLayer.Move;
			}
			this.OnOccupantChanged(this.receptacle.Occupant);
		}
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x002608B0 File Offset: 0x0025EAB0
	private void OnOccupantChanged(object data)
	{
		Attributes attributes = this.GetAttributes();
		if (this.decorModifier != null)
		{
			attributes.Remove(this.decorModifier);
			attributes.Remove(this.decorRadiusModifier);
			this.decorModifier = null;
			this.decorRadiusModifier = null;
		}
		if (data != null)
		{
			GameObject gameObject = (GameObject)data;
			UnityEngine.Object component = gameObject.GetComponent<DecorProvider>();
			float value = 5f;
			float value2 = 3f;
			if (component != null)
			{
				value = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(gameObject).GetTotalValue() * 2f, 5f);
				value2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(gameObject).GetTotalValue() + 2f;
			}
			string description = string.Format(BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, gameObject.GetComponent<KPrefabID>().PrefabTag.ProperName());
			this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, value, description, false, false, true);
			this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, value2, description, false, false, true);
			attributes.Add(this.decorModifier);
			attributes.Add(this.decorRadiusModifier);
		}
	}

	// Token: 0x04003202 RID: 12802
	[MyCmpReq]
	protected SingleEntityReceptacle receptacle;

	// Token: 0x04003203 RID: 12803
	[MyCmpReq]
	private DecorProvider decorProvider;

	// Token: 0x04003204 RID: 12804
	private const float MINIMUM_DECOR = 5f;

	// Token: 0x04003205 RID: 12805
	private const float STORED_DECOR_MODIFIER = 2f;

	// Token: 0x04003206 RID: 12806
	private const int RADIUS_BONUS = 2;

	// Token: 0x04003207 RID: 12807
	private AttributeModifier decorModifier;

	// Token: 0x04003208 RID: 12808
	private AttributeModifier decorRadiusModifier;

	// Token: 0x04003209 RID: 12809
	private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>(delegate(ItemPedestal component, object data)
	{
		component.OnOccupantChanged(data);
	});
}
