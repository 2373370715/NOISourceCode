using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	public class AttributeModifierSickness : Sickness.SicknessComponent
	{
		public AttributeModifierSickness(Tag minionModel, AttributeModifier[] attribute_modifiers)
		{
			this.GetAttributeModifierForMinionModel[minionModel] = attribute_modifiers;
			this.attributeModifiers = new AttributeModifier[0];
		}

		public AttributeModifierSickness(AttributeModifier[] attribute_modifiers)
		{
			this.attributeModifiers = attribute_modifiers;
		}

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

		public AttributeModifier[] Modifers
		{
			get
			{
				return this.attributeModifiers;
			}
		}

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

		private Dictionary<Tag, AttributeModifier[]> GetAttributeModifierForMinionModel = new Dictionary<Tag, AttributeModifier[]>();

		private AttributeModifier[] attributeModifiers;
	}
}
