using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200150F RID: 5391
[AddComponentMenu("KMonoBehaviour/game/MedicinalPill")]
public class MedicinalPill : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06007020 RID: 28704 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06007021 RID: 28705 RVA: 0x00303240 File Offset: 0x00301440
	public List<Descriptor> EffectDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (string.IsNullOrEmpty(this.info.doctorStationId))
		{
			if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_BOOSTER_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.SELF_ADMINISTERED_CURE_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
		}
		else
		{
			string properName = Assets.GetPrefab(this.info.doctorStationId).GetProperName();
			if (this.info.medicineType == MedicineInfo.MedicineType.Booster)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER.Replace("{Station}", properName), Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP.Replace("{Station}", properName), Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
			else
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE.Replace("{Station}", properName), Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.DOCTOR_ADMINISTERED_CURE_TOOLTIP.Replace("{Station}", properName), Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			}
		}
		switch (this.info.medicineType)
		{
		case MedicineInfo.MedicineType.CureAny:
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_ANY, Array.Empty<object>()), string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_ANY_TOOLTIP, Array.Empty<object>()), Descriptor.DescriptorType.Effect, false));
			break;
		case MedicineInfo.MedicineType.CureSpecific:
		{
			List<string> list2 = new List<string>();
			foreach (string text in this.info.curedSicknesses)
			{
				list2.Add(Strings.Get("STRINGS.DUPLICANTS.DISEASES." + text.ToUpper() + ".NAME"));
			}
			string arg = string.Join(",", list2.ToArray());
			list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES, arg), string.Format(DUPLICANTS.DISEASES.MEDICINE.CURES_TOOLTIP, arg), Descriptor.DescriptorType.Effect, false));
			break;
		}
		}
		if (!string.IsNullOrEmpty(this.info.effect))
		{
			Effect effect = Db.Get().effects.Get(this.info.effect);
			list.Add(new Descriptor(string.Format(DUPLICANTS.MODIFIERS.MEDICINE_GENERICPILL.EFFECT_DESC, effect.Name), string.Format("{0}\n{1}", effect.description, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06007022 RID: 28706 RVA: 0x000EDE77 File Offset: 0x000EC077
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.EffectDescriptors(go);
	}

	// Token: 0x04005434 RID: 21556
	public MedicineInfo info;
}
