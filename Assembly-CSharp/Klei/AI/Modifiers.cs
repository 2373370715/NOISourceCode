using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE0 RID: 15584
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Modifiers")]
	public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
	{
		// Token: 0x0600EF34 RID: 61236 RVA: 0x004E8FF8 File Offset: 0x004E71F8
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			this.amounts = new Amounts(base.gameObject);
			this.sicknesses = new Sicknesses(base.gameObject);
			this.attributes = new Attributes(base.gameObject);
			foreach (string id in this.initialAmounts)
			{
				this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(id), base.gameObject));
			}
			foreach (string text in this.initialAttributes)
			{
				Attribute attribute = Db.Get().CritterAttributes.TryGet(text);
				if (attribute == null)
				{
					attribute = Db.Get().PlantAttributes.TryGet(text);
				}
				if (attribute == null)
				{
					attribute = Db.Get().Attributes.TryGet(text);
				}
				DebugUtil.Assert(attribute != null, "Couldn't find an attribute for id", text);
				this.attributes.Add(attribute);
			}
			Traits component = base.GetComponent<Traits>();
			if (this.initialTraits != null)
			{
				foreach (string id2 in this.initialTraits)
				{
					Trait trait = Db.Get().traits.Get(id2);
					component.Add(trait);
				}
			}
		}

		// Token: 0x0600EF35 RID: 61237 RVA: 0x00144FE8 File Offset: 0x001431E8
		public float GetPreModifiedAttributeValue(Attribute attribute)
		{
			return AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
		}

		// Token: 0x0600EF36 RID: 61238 RVA: 0x004E91A4 File Offset: 0x004E73A4
		public string GetPreModifiedAttributeFormattedValue(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return attribute.formatter.GetFormattedValue(totalValue, attribute.formatter.DeltaTimeSlice);
		}

		// Token: 0x0600EF37 RID: 61239 RVA: 0x004E91D8 File Offset: 0x004E73D8
		public string GetPreModifiedAttributeDescription(Attribute attribute)
		{
			float totalValue = AttributeInstance.GetTotalValue(attribute, this.GetPreModifiers(attribute));
			return string.Format(DUPLICANTS.ATTRIBUTES.VALUE, attribute.Name, attribute.formatter.GetFormattedValue(totalValue, GameUtil.TimeSlice.None));
		}

		// Token: 0x0600EF38 RID: 61240 RVA: 0x00144FF7 File Offset: 0x001431F7
		public string GetPreModifiedAttributeToolTip(Attribute attribute)
		{
			return attribute.formatter.GetTooltip(attribute, this.GetPreModifiers(attribute), null);
		}

		// Token: 0x0600EF39 RID: 61241 RVA: 0x004E9218 File Offset: 0x004E7418
		public List<AttributeModifier> GetPreModifiers(Attribute attribute)
		{
			List<AttributeModifier> list = new List<AttributeModifier>();
			foreach (string id in this.initialTraits)
			{
				foreach (AttributeModifier attributeModifier in Db.Get().traits.Get(id).SelfModifiers)
				{
					if (attributeModifier.AttributeId == attribute.Id)
					{
						list.Add(attributeModifier);
					}
				}
			}
			MutantPlant component = base.GetComponent<MutantPlant>();
			if (component != null && component.MutationIDs != null)
			{
				foreach (string id2 in component.MutationIDs)
				{
					foreach (AttributeModifier attributeModifier2 in Db.Get().PlantMutations.Get(id2).SelfModifiers)
					{
						if (attributeModifier2.AttributeId == attribute.Id)
						{
							list.Add(attributeModifier2);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600EF3A RID: 61242 RVA: 0x0014500D File Offset: 0x0014320D
		public void Serialize(BinaryWriter writer)
		{
			this.OnSerialize(writer);
		}

		// Token: 0x0600EF3B RID: 61243 RVA: 0x00145016 File Offset: 0x00143216
		public void Deserialize(IReader reader)
		{
			this.OnDeserialize(reader);
		}

		// Token: 0x0600EF3C RID: 61244 RVA: 0x0014501F File Offset: 0x0014321F
		public virtual void OnSerialize(BinaryWriter writer)
		{
			this.amounts.Serialize(writer);
			this.sicknesses.Serialize(writer);
		}

		// Token: 0x0600EF3D RID: 61245 RVA: 0x00145039 File Offset: 0x00143239
		public virtual void OnDeserialize(IReader reader)
		{
			this.amounts.Deserialize(reader);
			this.sicknesses.Deserialize(reader);
		}

		// Token: 0x0600EF3E RID: 61246 RVA: 0x00145053 File Offset: 0x00143253
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.amounts != null)
			{
				this.amounts.Cleanup();
			}
		}

		// Token: 0x0400EAD3 RID: 60115
		public Amounts amounts;

		// Token: 0x0400EAD4 RID: 60116
		public Attributes attributes;

		// Token: 0x0400EAD5 RID: 60117
		public Sicknesses sicknesses;

		// Token: 0x0400EAD6 RID: 60118
		public List<string> initialTraits = new List<string>();

		// Token: 0x0400EAD7 RID: 60119
		public List<string> initialAmounts = new List<string>();

		// Token: 0x0400EAD8 RID: 60120
		public List<string> initialAttributes = new List<string>();
	}
}
