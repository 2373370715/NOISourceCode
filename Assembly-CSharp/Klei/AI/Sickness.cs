using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C74 RID: 15476
	[DebuggerDisplay("{base.Id}")]
	public abstract class Sickness : Resource
	{
		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x0600ED73 RID: 60787 RVA: 0x00143D71 File Offset: 0x00141F71
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x0600ED74 RID: 60788 RVA: 0x00143D83 File Offset: 0x00141F83
		public float SicknessDuration
		{
			get
			{
				return this.sicknessDuration;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x0600ED75 RID: 60789 RVA: 0x00143D8B File Offset: 0x00141F8B
		public StringKey DescriptiveSymptoms
		{
			get
			{
				return this.descriptiveSymptoms;
			}
		}

		// Token: 0x0600ED76 RID: 60790 RVA: 0x004E1F6C File Offset: 0x004E016C
		public Sickness(string id, Sickness.SicknessType type, Sickness.Severity severity, float immune_attack_strength, List<Sickness.InfectionVector> infection_vectors, float sickness_duration, string recovery_effect = null) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.sicknessType = type;
			this.severity = severity;
			this.infectionVectors = infection_vectors;
			this.sicknessDuration = sickness_duration;
			this.recoveryEffect = recovery_effect;
			this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
			this.cureSpeedBase.BaseValue = 1f;
			this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Db.Get().Attributes.Add(this.cureSpeedBase);
		}

		// Token: 0x0600ED77 RID: 60791 RVA: 0x004E2068 File Offset: 0x004E0268
		public object[] Infect(GameObject go, SicknessInstance diseaseInstance, SicknessExposureInfo exposure_info)
		{
			object[] array = new object[this.components.Count];
			for (int i = 0; i < this.components.Count; i++)
			{
				array[i] = this.components[i].OnInfect(go, diseaseInstance);
			}
			return array;
		}

		// Token: 0x0600ED78 RID: 60792 RVA: 0x004E20B4 File Offset: 0x004E02B4
		public void Cure(GameObject go, object[] componentData)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				this.components[i].OnCure(go, componentData[i]);
			}
		}

		// Token: 0x0600ED79 RID: 60793 RVA: 0x00143D93 File Offset: 0x00141F93
		public List<Descriptor> GetSymptoms()
		{
			return this.GetSymptoms(null);
		}

		// Token: 0x0600ED7A RID: 60794 RVA: 0x004E20EC File Offset: 0x004E02EC
		public List<Descriptor> GetSymptoms(GameObject victim)
		{
			List<Descriptor> list = new List<Descriptor>();
			for (int i = 0; i < this.components.Count; i++)
			{
				List<Descriptor> symptoms = this.components[i].GetSymptoms(victim);
				if (symptoms != null && symptoms.Count > 0)
				{
					list.AddRange(symptoms);
				}
			}
			if (this.fatalityDuration > 0f)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), Descriptor.DescriptorType.SymptomAidable, false));
			}
			return list;
		}

		// Token: 0x0600ED7B RID: 60795 RVA: 0x00143D9C File Offset: 0x00141F9C
		protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
		{
			this.components.Add(cmp);
		}

		// Token: 0x0600ED7C RID: 60796 RVA: 0x004E2198 File Offset: 0x004E0398
		public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] is T)
				{
					return this.components[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x0600ED7D RID: 60797 RVA: 0x000CE880 File Offset: 0x000CCA80
		public virtual List<Descriptor> GetSicknessSourceDescriptors()
		{
			return new List<Descriptor>();
		}

		// Token: 0x0600ED7E RID: 60798 RVA: 0x004E21F0 File Offset: 0x004E03F0
		public List<Descriptor> GetQualitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			using (List<Sickness.InfectionVector>.Enumerator enumerator = this.infectionVectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Sickness.InfectionVector.Contact:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Digestion:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Inhalation:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Exposure:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					}
				}
			}
			list.Add(new Descriptor(Strings.Get(this.descriptiveSymptoms), "", Descriptor.DescriptorType.Information, false));
			return list;
		}

		// Token: 0x0400E97D RID: 59773
		private StringKey name;

		// Token: 0x0400E97E RID: 59774
		private StringKey descriptiveSymptoms;

		// Token: 0x0400E97F RID: 59775
		private float sicknessDuration = 600f;

		// Token: 0x0400E980 RID: 59776
		public float fatalityDuration;

		// Token: 0x0400E981 RID: 59777
		public HashedString id;

		// Token: 0x0400E982 RID: 59778
		public Sickness.SicknessType sicknessType;

		// Token: 0x0400E983 RID: 59779
		public Sickness.Severity severity;

		// Token: 0x0400E984 RID: 59780
		public string recoveryEffect;

		// Token: 0x0400E985 RID: 59781
		public List<Sickness.InfectionVector> infectionVectors;

		// Token: 0x0400E986 RID: 59782
		private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();

		// Token: 0x0400E987 RID: 59783
		public Amount amount;

		// Token: 0x0400E988 RID: 59784
		public Attribute amountDeltaAttribute;

		// Token: 0x0400E989 RID: 59785
		public Attribute cureSpeedBase;

		// Token: 0x02003C75 RID: 15477
		public abstract class SicknessComponent
		{
			// Token: 0x0600ED7F RID: 60799
			public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

			// Token: 0x0600ED80 RID: 60800
			public abstract void OnCure(GameObject go, object instance_data);

			// Token: 0x0600ED81 RID: 60801 RVA: 0x000AA765 File Offset: 0x000A8965
			public virtual List<Descriptor> GetSymptoms()
			{
				return null;
			}

			// Token: 0x0600ED82 RID: 60802 RVA: 0x00143DAA File Offset: 0x00141FAA
			public virtual List<Descriptor> GetSymptoms(GameObject victim)
			{
				return this.GetSymptoms();
			}
		}

		// Token: 0x02003C76 RID: 15478
		public enum InfectionVector
		{
			// Token: 0x0400E98B RID: 59787
			Contact,
			// Token: 0x0400E98C RID: 59788
			Digestion,
			// Token: 0x0400E98D RID: 59789
			Inhalation,
			// Token: 0x0400E98E RID: 59790
			Exposure
		}

		// Token: 0x02003C77 RID: 15479
		public enum SicknessType
		{
			// Token: 0x0400E990 RID: 59792
			Pathogen,
			// Token: 0x0400E991 RID: 59793
			Ailment,
			// Token: 0x0400E992 RID: 59794
			Injury
		}

		// Token: 0x02003C78 RID: 15480
		public enum Severity
		{
			// Token: 0x0400E994 RID: 59796
			Benign,
			// Token: 0x0400E995 RID: 59797
			Minor,
			// Token: 0x0400E996 RID: 59798
			Major,
			// Token: 0x0400E997 RID: 59799
			Critical
		}
	}
}
