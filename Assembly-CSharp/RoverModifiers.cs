using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001155 RID: 4437
[SerializationConfig(MemberSerialization.OptIn)]
public class RoverModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x06005A98 RID: 23192 RVA: 0x002A3E5C File Offset: 0x002A205C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributes.Add(Db.Get().Attributes.Construction);
		this.attributes.Add(Db.Get().Attributes.Digging);
		this.attributes.Add(Db.Get().Attributes.Strength);
	}

	// Token: 0x06005A99 RID: 23193 RVA: 0x002A3EC0 File Offset: 0x002A20C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<RoverModifiers>(-1988963660, RoverModifiers.OnBeginChoreDelegate);
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x002A3F54 File Offset: 0x002A2154
	private void SetupDependentAttribute(Klei.AI.Attribute targetAttribute, AttributeConverter attributeConverter)
	{
		Klei.AI.Attribute attribute = attributeConverter.attribute;
		AttributeInstance attributeInstance = attribute.Lookup(this);
		AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup(this).Evaluate(), attribute.Name, false, false, false);
		this.GetAttributes().Add(target_modifier);
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(delegate()
		{
			target_modifier.SetValue(attributeConverter.Lookup(this).Evaluate());
		}));
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x002A3FE8 File Offset: 0x002A21E8
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x04004085 RID: 16517
	private static readonly EventSystem.IntraObjectHandler<RoverModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<RoverModifiers>(delegate(RoverModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
