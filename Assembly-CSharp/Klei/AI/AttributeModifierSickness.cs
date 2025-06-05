using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C8B RID: 15499
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		// Token: 0x0600EDC7 RID: 60871 RVA: 0x00144048 File Offset: 0x00142248
		public AttributeModifierSickness(Tag minionModel, AttributeModifier[] attribute_modifiers)
		{
			this.GetAttributeModifierForMinionModel[minionModel] = attribute_modifiers;
			this.attributeModifiers = new AttributeModifier[0];
		}

		// Token: 0x0600EDC8 RID: 60872 RVA: 0x00144074 File Offset: 0x00142274
		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

		// Token: 0x0600EDC9 RID: 60873 RVA: 0x004E3E80 File Offset: 0x004E2080
		public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
		{
			Attributes attributes = go.GetAttributes();
			Tag key = go.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				for (int i = 0; i < this.GetAttributeModifierForMinionModel[key].Length; i++)
				{
					AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][i];
					attributes.Add(modifier);
				}
			}
			for (int j = 0; j < this.attributeModifiers.Length; j++)
			{
				AttributeModifier modifier2 = this.attributeModifiers[j];
				attributes.Add(modifier2);
			}
			return null;
		}

		// Token: 0x0600EDCA RID: 60874 RVA: 0x004E3F04 File Offset: 0x004E2104
		public override void OnCure(GameObject go, object instance_data)
		{
			Attributes attributes = go.GetAttributes();
			Tag key = go.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				for (int i = 0; i < this.GetAttributeModifierForMinionModel[key].Length; i++)
				{
					AttributeModifier modifier = this.GetAttributeModifierForMinionModel[key][i];
					attributes.Remove(modifier);
				}
			}
			for (int j = 0; j < this.attributeModifiers.Length; j++)
			{
				AttributeModifier modifier2 = this.attributeModifiers[j];
				attributes.Remove(modifier2);
			}
		}

		// Token: 0x17000C5D RID: 3165
		// (get) Token: 0x0600EDCB RID: 60875 RVA: 0x0014408E File Offset: 0x0014228E
		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

		// Token: 0x0600EDCC RID: 60876 RVA: 0x004E3F88 File Offset: 0x004E2188
		public override List<Descriptor> GetSymptoms(GameObject victim)
		{
			if (victim == null)
			{
				return this.GetSymptoms();
			}
			List<Descriptor> list = new List<Descriptor>();
			Tag key = victim.PrefabID();
			if (this.GetAttributeModifierForMinionModel.ContainsKey(key))
			{
				foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[key])
				{
					Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
					list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
				}
			}
			foreach (AttributeModifier attributeModifier2 in this.attributeModifiers)
			{
				Attribute attribute2 = Db.Get().Attributes.Get(attributeModifier2.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute2.Name, attributeModifier2.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute2.Name, attributeModifier2.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x0600EDCD RID: 60877 RVA: 0x004E40BC File Offset: 0x004E22BC
		public override List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			foreach (Tag tag in this.GetAttributeModifierForMinionModel.Keys)
			{
				string properName = Assets.GetPrefab(tag).GetProperName();
				foreach (AttributeModifier attributeModifier in this.GetAttributeModifierForMinionModel[tag])
				{
					Attribute attribute = Db.Get().Attributes.Get(attributeModifier.AttributeId);
					list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_BY_MODEL_MODIFIER_SYMPTOMS, properName, attribute.Name, attributeModifier.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute.Name, attributeModifier.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
				}
			}
			foreach (AttributeModifier attributeModifier2 in this.attributeModifiers)
			{
				Attribute attribute2 = Db.Get().Attributes.Get(attributeModifier2.AttributeId);
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS, attribute2.Name, attributeModifier2.GetFormattedString()), string.Format(DUPLICANTS.DISEASES.ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP, attribute2.Name, attributeModifier2.GetFormattedString()), Descriptor.DescriptorType.Symptom, false));
			}
			return list;
		}

		// Token: 0x0400E9C5 RID: 59845
		private Dictionary<Tag, AttributeModifier[]> GetAttributeModifierForMinionModel = new Dictionary<Tag, AttributeModifier[]>();

		// Token: 0x0400E9C6 RID: 59846
		private AttributeModifier[] attributeModifiers;
	}
}
