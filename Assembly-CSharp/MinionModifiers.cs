using System;
using System.IO;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02001527 RID: 5415
[SerializationConfig(MemberSerialization.OptIn)]
public class MinionModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x06007097 RID: 28823 RVA: 0x00306414 File Offset: 0x00304614
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.addBaseTraits)
		{
			foreach (Klei.AI.Attribute attribute in Db.Get().Attributes.resources)
			{
				if (this.attributes.Get(attribute) == null)
				{
					this.attributes.Add(attribute);
				}
			}
			foreach (Disease disease in Db.Get().Diseases.resources)
			{
				AmountInstance amountInstance = this.AddAmount(disease.amount);
				this.attributes.Add(disease.cureSpeedBase);
				amountInstance.SetValue(0f);
			}
			ChoreConsumer component = base.GetComponent<ChoreConsumer>();
			if (component != null)
			{
				component.AddProvider(GlobalChoreProvider.Instance);
				base.gameObject.AddComponent<QualityOfLifeNeed>();
			}
		}
	}

	// Token: 0x06007098 RID: 28824 RVA: 0x0030652C File Offset: 0x0030472C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<MinionModifiers>(1623392196, MinionModifiers.OnDeathDelegate);
			base.Subscribe<MinionModifiers>(-1506069671, MinionModifiers.OnAttachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-485480405, MinionModifiers.OnDetachFollowCamDelegate);
			base.Subscribe<MinionModifiers>(-1988963660, MinionModifiers.OnBeginChoreDelegate);
			AmountInstance amountInstance = this.GetAmounts().Get("Calories");
			if (amountInstance != null)
			{
				AmountInstance amountInstance2 = amountInstance;
				amountInstance2.OnMaxValueReached = (System.Action)Delegate.Combine(amountInstance2.OnMaxValueReached, new System.Action(this.OnMaxCaloriesReached));
			}
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x06007099 RID: 28825 RVA: 0x0030662C File Offset: 0x0030482C
	private AmountInstance AddAmount(Amount amount)
	{
		AmountInstance instance = new AmountInstance(amount, base.gameObject);
		return this.amounts.Add(instance);
	}

	// Token: 0x0600709A RID: 28826 RVA: 0x00306654 File Offset: 0x00304854
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

	// Token: 0x0600709B RID: 28827 RVA: 0x003066E8 File Offset: 0x003048E8
	private void OnDeath(object data)
	{
		global::Debug.LogFormat("OnDeath {0} -- {1} has died!", new object[]
		{
			data,
			base.name
		});
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			minionIdentity.GetComponent<Effects>().Add("Mourning", true);
		}
	}

	// Token: 0x0600709C RID: 28828 RVA: 0x000EE1EA File Offset: 0x000EC3EA
	private void OnMaxCaloriesReached()
	{
		base.GetComponent<Effects>().Add("WellFed", true);
	}

	// Token: 0x0600709D RID: 28829 RVA: 0x002A3FE8 File Offset: 0x002A21E8
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x0600709E RID: 28830 RVA: 0x000EE1FE File Offset: 0x000EC3FE
	public override void OnSerialize(BinaryWriter writer)
	{
		base.OnSerialize(writer);
	}

	// Token: 0x0600709F RID: 28831 RVA: 0x000EE207 File Offset: 0x000EC407
	public override void OnDeserialize(IReader reader)
	{
		base.OnDeserialize(reader);
	}

	// Token: 0x060070A0 RID: 28832 RVA: 0x000EE210 File Offset: 0x000EC410
	private void OnAttachFollowCam(object data)
	{
		base.GetComponent<Effects>().Add("CenterOfAttention", false);
	}

	// Token: 0x060070A1 RID: 28833 RVA: 0x000EE224 File Offset: 0x000EC424
	private void OnDetachFollowCam(object data)
	{
		base.GetComponent<Effects>().Remove("CenterOfAttention");
	}

	// Token: 0x040054A0 RID: 21664
	public bool addBaseTraits = true;

	// Token: 0x040054A1 RID: 21665
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDeathDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040054A2 RID: 21666
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnAttachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnAttachFollowCam(data);
	});

	// Token: 0x040054A3 RID: 21667
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnDetachFollowCamDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnDetachFollowCam(data);
	});

	// Token: 0x040054A4 RID: 21668
	private static readonly EventSystem.IntraObjectHandler<MinionModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<MinionModifiers>(delegate(MinionModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
