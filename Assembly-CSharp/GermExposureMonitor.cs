using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020015C4 RID: 5572
public class GermExposureMonitor : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance>
{
	// Token: 0x060073A8 RID: 29608 RVA: 0x003100C0 File Offset: 0x0030E2C0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		base.serializable = StateMachine.SerializeType.Never;
		this.root.Update(delegate(GermExposureMonitor.Instance smi, float dt)
		{
			smi.OnInhaleExposureTick(dt);
		}, UpdateRate.SIM_1000ms, true).EventHandler(GameHashes.EatCompleteEater, delegate(GermExposureMonitor.Instance smi, object obj)
		{
			smi.OnEatComplete(obj);
		}).EventHandler(GameHashes.SicknessAdded, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessAdded(data);
		}).EventHandler(GameHashes.SicknessCured, delegate(GermExposureMonitor.Instance smi, object data)
		{
			smi.OnSicknessCured(data);
		}).EventHandler(GameHashes.SleepFinished, delegate(GermExposureMonitor.Instance smi)
		{
			smi.OnSleepFinished();
		});
	}

	// Token: 0x060073A9 RID: 29609 RVA: 0x000F0494 File Offset: 0x000EE694
	public static float GetContractionChance(float rating)
	{
		return 0.5f - 0.5f * (float)Math.Tanh(0.25 * (double)rating);
	}

	// Token: 0x020015C5 RID: 5573
	public enum ExposureState
	{
		// Token: 0x040056DC RID: 22236
		None,
		// Token: 0x040056DD RID: 22237
		Contact,
		// Token: 0x040056DE RID: 22238
		Exposed,
		// Token: 0x040056DF RID: 22239
		Contracted,
		// Token: 0x040056E0 RID: 22240
		Sick
	}

	// Token: 0x020015C6 RID: 5574
	public class ExposureStatusData
	{
		// Token: 0x040056E1 RID: 22241
		public ExposureType exposure_type;

		// Token: 0x040056E2 RID: 22242
		public GermExposureMonitor.Instance owner;
	}

	// Token: 0x020015C7 RID: 5575
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<GermExposureMonitor, GermExposureMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060073AC RID: 29612 RVA: 0x003101B0 File Offset: 0x0030E3B0
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
			this.primaryElement = master.GetComponent<PrimaryElement>();
			this.traits = master.GetComponent<Traits>();
			this.lastDiseaseSources = new Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo>();
			this.lastExposureTime = new Dictionary<HashedString, float>();
			this.inhaleExposureTick = new Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo>();
			GameClock.Instance.Subscribe(-722330267, new Action<object>(this.OnNightTime));
			base.gameObject.Subscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
			this.inateImmunities = DUPLICANTSTATS.GetStatsFor(base.gameObject).DiseaseImmunities.IMMUNITIES;
			OxygenBreather component = base.GetComponent<OxygenBreather>();
			if (component != null)
			{
				OxygenBreather oxygenBreather = component;
				oxygenBreather.onBreathableGasConsumed = (Action<SimHashes, float, float, byte, int>)Delegate.Combine(oxygenBreather.onBreathableGasConsumed, new Action<SimHashes, float, float, byte, int>(this.OnAirConsumed));
			}
		}

		// Token: 0x060073AD RID: 29613 RVA: 0x000F04BC File Offset: 0x000EE6BC
		protected override void OnCleanUp()
		{
			base.gameObject.Unsubscribe(-1582839653, new Action<object>(this.OnBionicTagsChanged));
			base.OnCleanUp();
		}

		// Token: 0x060073AE RID: 29614 RVA: 0x000F04E0 File Offset: 0x000EE6E0
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshStatusItems();
		}

		// Token: 0x060073AF RID: 29615 RVA: 0x003102C0 File Offset: 0x0030E4C0
		public override void StopSM(string reason)
		{
			GameClock.Instance.Unsubscribe(-722330267, new Action<object>(this.OnNightTime));
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
			}
			base.StopSM(reason);
		}

		// Token: 0x060073B0 RID: 29616 RVA: 0x0031032C File Offset: 0x0030E52C
		public void OnEatComplete(object obj)
		{
			Edible edible = (Edible)obj;
			HandleVector<int>.Handle handle = GameComps.DiseaseContainers.GetHandle(edible.gameObject);
			if (handle != HandleVector<int>.InvalidHandle)
			{
				DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle);
				if (header.diseaseIdx != 255)
				{
					Disease disease = Db.Get().Diseases[(int)header.diseaseIdx];
					float num = edible.unitsConsumed / (edible.unitsConsumed + edible.Units);
					int num2 = Mathf.CeilToInt((float)header.diseaseCount * num);
					GameComps.DiseaseContainers.ModifyDiseaseCount(handle, -num2);
					KPrefabID component = edible.GetComponent<KPrefabID>();
					this.InjectDisease(disease, num2, component.PrefabID(), Sickness.InfectionVector.Digestion);
				}
			}
		}

		// Token: 0x060073B1 RID: 29617 RVA: 0x003103DC File Offset: 0x0030E5DC
		public void OnAirConsumed(SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
		{
			if (disseaseIDX != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disseaseIDX];
				this.InjectDisease(disease, disseaseCount, ElementLoader.FindElementByHash(elementConsumed).tag, Sickness.InfectionVector.Inhalation);
			}
		}

		// Token: 0x060073B2 RID: 29618 RVA: 0x0031041C File Offset: 0x0030E61C
		public void OnInhaleExposureTick(float dt)
		{
			foreach (KeyValuePair<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> keyValuePair in this.inhaleExposureTick)
			{
				if (keyValuePair.Value.inhaled)
				{
					keyValuePair.Value.inhaled = false;
					keyValuePair.Value.ticks++;
				}
				else
				{
					keyValuePair.Value.ticks = Mathf.Max(0, keyValuePair.Value.ticks - 1);
				}
			}
		}

		// Token: 0x060073B3 RID: 29619 RVA: 0x003104BC File Offset: 0x0030E6BC
		public void TryInjectDisease(byte disease_idx, int count, Tag source, Sickness.InfectionVector vector)
		{
			if (disease_idx != 255)
			{
				Disease disease = Db.Get().Diseases[(int)disease_idx];
				this.InjectDisease(disease, count, source, vector);
			}
		}

		// Token: 0x060073B4 RID: 29620 RVA: 0x000F04EE File Offset: 0x000EE6EE
		public float GetGermResistance()
		{
			return Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
		}

		// Token: 0x060073B5 RID: 29621 RVA: 0x003104F0 File Offset: 0x0030E6F0
		public float GetResistanceToExposureType(ExposureType exposureType, float overrideExposureTier = -1f)
		{
			float num = overrideExposureTier;
			if (num == -1f)
			{
				num = this.GetExposureTier(exposureType.germ_id);
			}
			num = Mathf.Clamp(num, 1f, 3f);
			float num2 = GERM_EXPOSURE.EXPOSURE_TIER_RESISTANCE_BONUSES[(int)num - 1];
			float totalValue = Db.Get().Attributes.GermResistance.Lookup(base.gameObject).GetTotalValue();
			return (float)exposureType.base_resistance + totalValue + num2;
		}

		// Token: 0x060073B6 RID: 29622 RVA: 0x0031055C File Offset: 0x0030E75C
		public int AssessDigestedGerms(ExposureType exposure_type, int count)
		{
			int exposure_threshold = exposure_type.exposure_threshold;
			int val = count / exposure_threshold;
			return MathUtil.Clamp(1, 3, val);
		}

		// Token: 0x060073B7 RID: 29623 RVA: 0x0031057C File Offset: 0x0030E77C
		public bool AssessInhaledGerms(ExposureType exposure_type)
		{
			GermExposureMonitor.Instance.InhaleTickInfo inhaleTickInfo;
			this.inhaleExposureTick.TryGetValue(exposure_type.germ_id, out inhaleTickInfo);
			if (inhaleTickInfo == null)
			{
				inhaleTickInfo = new GermExposureMonitor.Instance.InhaleTickInfo();
				this.inhaleExposureTick[exposure_type.germ_id] = inhaleTickInfo;
			}
			if (!inhaleTickInfo.inhaled)
			{
				float exposureTier = this.GetExposureTier(exposure_type.germ_id);
				inhaleTickInfo.inhaled = true;
				return inhaleTickInfo.ticks >= GERM_EXPOSURE.INHALE_TICK_THRESHOLD[(int)exposureTier];
			}
			return false;
		}

		// Token: 0x060073B8 RID: 29624 RVA: 0x003105F4 File Offset: 0x0030E7F4
		public bool IsImmuneToDisease(string sicknessID)
		{
			if (this.inateImmunities == null)
			{
				return false;
			}
			for (int i = 0; i < this.inateImmunities.Length; i++)
			{
				if (sicknessID == this.inateImmunities[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060073B9 RID: 29625 RVA: 0x00310634 File Offset: 0x0030E834
		public void InjectDisease(Disease disease, int count, Tag source, Sickness.InfectionVector vector)
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (disease.id == exposureType.germ_id && !this.IsImmuneToDisease(exposureType.sickness_id) && count > exposureType.exposure_threshold && this.HasMinExposurePeriodElapsed(exposureType.germ_id) && this.IsExposureValidForTraits(exposureType))
				{
					Sickness sickness = (exposureType.sickness_id != null) ? Db.Get().Sicknesses.Get(exposureType.sickness_id) : null;
					if (sickness == null || sickness.infectionVectors.Contains(vector))
					{
						GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
						float exposureTier = this.GetExposureTier(exposureType.germ_id);
						if (exposureState == GermExposureMonitor.ExposureState.None || exposureState == GermExposureMonitor.ExposureState.Contact)
						{
							float contractionChance = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Contact);
							if (contractionChance > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance, base.transform.GetPosition());
								if (exposureType.infect_immediately)
								{
									this.InfectImmediately(exposureType);
								}
								else
								{
									bool flag = true;
									bool flag2 = vector == Sickness.InfectionVector.Inhalation;
									bool flag3 = vector == Sickness.InfectionVector.Digestion;
									int num = 1;
									if (flag2)
									{
										flag = this.AssessInhaledGerms(exposureType);
									}
									if (flag3)
									{
										num = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag)
									{
										if (flag2)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Exposed);
										this.SetExposureTier(exposureType.germ_id, (float)num);
										float amount = Mathf.Clamp01(contractionChance);
										GermExposureTracker.Instance.AddExposure(exposureType, amount);
									}
								}
							}
						}
						else if (exposureState == GermExposureMonitor.ExposureState.Exposed && exposureTier < 3f)
						{
							float contractionChance2 = GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f));
							if (contractionChance2 > 0f)
							{
								this.lastDiseaseSources[disease.id] = new GermExposureMonitor.Instance.DiseaseSourceInfo(source, vector, contractionChance2, base.transform.GetPosition());
								if (!exposureType.infect_immediately)
								{
									bool flag4 = true;
									bool flag5 = vector == Sickness.InfectionVector.Inhalation;
									bool flag6 = vector == Sickness.InfectionVector.Digestion;
									int num2 = 1;
									if (flag5)
									{
										flag4 = this.AssessInhaledGerms(exposureType);
									}
									if (flag6)
									{
										num2 = this.AssessDigestedGerms(exposureType, count);
									}
									if (flag4)
									{
										if (flag5)
										{
											this.inhaleExposureTick[exposureType.germ_id].ticks = 0;
										}
										this.SetExposureTier(exposureType.germ_id, this.GetExposureTier(exposureType.germ_id) + (float)num2);
										float amount2 = Mathf.Clamp01(GermExposureMonitor.GetContractionChance(this.GetResistanceToExposureType(exposureType, -1f)) - contractionChance2);
										GermExposureTracker.Instance.AddExposure(exposureType, amount2);
									}
								}
							}
						}
					}
				}
			}
			this.RefreshStatusItems();
		}

		// Token: 0x060073BA RID: 29626 RVA: 0x00310908 File Offset: 0x0030EB08
		public GermExposureMonitor.ExposureState GetExposureState(string germ_id)
		{
			GermExposureMonitor.ExposureState result;
			this.exposureStates.TryGetValue(germ_id, out result);
			return result;
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x00310928 File Offset: 0x0030EB28
		public float GetExposureTier(string germ_id)
		{
			float value = 1f;
			this.exposureTiers.TryGetValue(germ_id, out value);
			return Mathf.Clamp(value, 1f, 3f);
		}

		// Token: 0x060073BC RID: 29628 RVA: 0x000F050F File Offset: 0x000EE70F
		public void SetExposureState(string germ_id, GermExposureMonitor.ExposureState exposure_state)
		{
			this.exposureStates[germ_id] = exposure_state;
			this.RefreshStatusItems();
		}

		// Token: 0x060073BD RID: 29629 RVA: 0x000F0524 File Offset: 0x000EE724
		public void SetExposureTier(string germ_id, float tier)
		{
			tier = Mathf.Clamp(tier, 0f, 3f);
			this.exposureTiers[germ_id] = tier;
			this.RefreshStatusItems();
		}

		// Token: 0x060073BE RID: 29630 RVA: 0x000F054B File Offset: 0x000EE74B
		public void ContractGerms(string germ_id)
		{
			DebugUtil.DevAssert(this.GetExposureState(germ_id) == GermExposureMonitor.ExposureState.Exposed, "Duplicant is contracting a sickness but was never exposed to it!", null);
			this.SetExposureState(germ_id, GermExposureMonitor.ExposureState.Contracted);
		}

		// Token: 0x060073BF RID: 29631 RVA: 0x0031095C File Offset: 0x0030EB5C
		public void OnSicknessAdded(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
				}
			}
		}

		// Token: 0x060073C0 RID: 29632 RVA: 0x003109B0 File Offset: 0x0030EBB0
		public void OnSicknessCured(object sickness_instance_data)
		{
			SicknessInstance sicknessInstance = (SicknessInstance)sickness_instance_data;
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (exposureType.sickness_id == sicknessInstance.Sickness.Id)
				{
					this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
				}
			}
		}

		// Token: 0x060073C1 RID: 29633 RVA: 0x00310A04 File Offset: 0x0030EC04
		private bool IsExposureValidForTraits(ExposureType exposure_type)
		{
			if (exposure_type.required_traits != null && exposure_type.required_traits.Count > 0)
			{
				foreach (string trait_id in exposure_type.required_traits)
				{
					if (!this.traits.HasTrait(trait_id))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_traits != null && exposure_type.excluded_traits.Count > 0)
			{
				foreach (string trait_id2 in exposure_type.excluded_traits)
				{
					if (this.traits.HasTrait(trait_id2))
					{
						return false;
					}
				}
			}
			if (exposure_type.excluded_effects != null && exposure_type.excluded_effects.Count > 0)
			{
				Effects component = base.master.GetComponent<Effects>();
				foreach (string effect_id in exposure_type.excluded_effects)
				{
					if (component.HasEffect(effect_id))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060073C2 RID: 29634 RVA: 0x00310B50 File Offset: 0x0030ED50
		private bool HasMinExposurePeriodElapsed(string germ_id)
		{
			float num;
			this.lastExposureTime.TryGetValue(germ_id, out num);
			return num == 0f || GameClock.Instance.GetTime() - num > 540f;
		}

		// Token: 0x060073C3 RID: 29635 RVA: 0x00310B90 File Offset: 0x0030ED90
		private void RefreshStatusItems()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				Guid guid;
				this.contactStatusItemHandles.TryGetValue(exposureType.germ_id, out guid);
				Guid guid2;
				this.statusItemHandles.TryGetValue(exposureType.germ_id, out guid2);
				GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
				if (guid2 == Guid.Empty && (exposureState == GermExposureMonitor.ExposureState.Exposed || exposureState == GermExposureMonitor.ExposureState.Contracted) && !string.IsNullOrEmpty(exposureType.sickness_id))
				{
					guid2 = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ExposedToGerms, new GermExposureMonitor.ExposureStatusData
					{
						exposure_type = exposureType,
						owner = this
					});
				}
				else if (guid2 != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Exposed && exposureState != GermExposureMonitor.ExposureState.Contracted)
				{
					guid2 = base.GetComponent<KSelectable>().RemoveStatusItem(guid2, false);
				}
				this.statusItemHandles[exposureType.germ_id] = guid2;
				if (guid == Guid.Empty && exposureState == GermExposureMonitor.ExposureState.Contact)
				{
					if (!string.IsNullOrEmpty(exposureType.sickness_id))
					{
						guid = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.ContactWithGerms, new GermExposureMonitor.ExposureStatusData
						{
							exposure_type = exposureType,
							owner = this
						});
					}
				}
				else if (guid != Guid.Empty && exposureState != GermExposureMonitor.ExposureState.Contact)
				{
					guid = base.GetComponent<KSelectable>().RemoveStatusItem(guid, false);
				}
				this.contactStatusItemHandles[exposureType.germ_id] = guid;
			}
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x000F056A File Offset: 0x000EE76A
		private void OnNightTime(object data)
		{
			this.UpdateReports();
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x00310D04 File Offset: 0x0030EF04
		private void UpdateReports()
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseStatus, (float)this.primaryElement.DiseaseCount, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.GERMS, "{0}", base.master.name), base.master.gameObject.GetProperName());
		}

		// Token: 0x060073C6 RID: 29638 RVA: 0x00310D58 File Offset: 0x0030EF58
		public void InfectImmediately(ExposureType exposure_type)
		{
			if (exposure_type.infection_effect != null)
			{
				base.master.GetComponent<Effects>().Add(exposure_type.infection_effect, true);
			}
			if (exposure_type.sickness_id != null)
			{
				string lastDiseaseSource = this.GetLastDiseaseSource(exposure_type.germ_id);
				SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposure_type.sickness_id, lastDiseaseSource);
				this.sicknesses.Infect(exposure_info);
			}
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x00310DB4 File Offset: 0x0030EFB4
		private void OnBionicTagsChanged(object o)
		{
			if (o == null)
			{
				return;
			}
			TagChangedEventData tagChangedEventData = (TagChangedEventData)o;
			if (tagChangedEventData.tag == GameTags.BionicBedTime && !tagChangedEventData.added)
			{
				this.OnSleepFinished();
			}
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x00310DEC File Offset: 0x0030EFEC
		public void OnSleepFinished()
		{
			foreach (ExposureType exposureType in GERM_EXPOSURE.TYPES)
			{
				if (!exposureType.infect_immediately && exposureType.sickness_id != null)
				{
					GermExposureMonitor.ExposureState exposureState = this.GetExposureState(exposureType.germ_id);
					if (exposureState == GermExposureMonitor.ExposureState.Exposed)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.None);
					}
					if (exposureState == GermExposureMonitor.ExposureState.Contracted)
					{
						this.SetExposureState(exposureType.germ_id, GermExposureMonitor.ExposureState.Sick);
						string lastDiseaseSource = this.GetLastDiseaseSource(exposureType.germ_id);
						SicknessExposureInfo exposure_info = new SicknessExposureInfo(exposureType.sickness_id, lastDiseaseSource);
						this.sicknesses.Infect(exposure_info);
					}
					this.SetExposureTier(exposureType.germ_id, 0f);
				}
			}
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x00310E8C File Offset: 0x0030F08C
		public string GetLastDiseaseSource(string id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			string result;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				switch (diseaseSourceInfo.vector)
				{
				case Sickness.InfectionVector.Contact:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.SKIN;
					break;
				case Sickness.InfectionVector.Digestion:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.FOOD, diseaseSourceInfo.sourceObject.ProperName());
					break;
				case Sickness.InfectionVector.Inhalation:
					result = string.Format(DUPLICANTS.DISEASES.INFECTIONSOURCES.AIR, diseaseSourceInfo.sourceObject.ProperName());
					break;
				default:
					result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
					break;
				}
			}
			else
			{
				result = DUPLICANTS.DISEASES.INFECTIONSOURCES.UNKNOWN;
			}
			return result;
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x00310F28 File Offset: 0x0030F128
		public Vector3 GetLastExposurePosition(string germ_id)
		{
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(germ_id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.position;
			}
			return base.transform.GetPosition();
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x00310F5C File Offset: 0x0030F15C
		public float GetExposureWeight(string id)
		{
			float exposureTier = this.GetExposureTier(id);
			GermExposureMonitor.Instance.DiseaseSourceInfo diseaseSourceInfo;
			if (this.lastDiseaseSources.TryGetValue(id, out diseaseSourceInfo))
			{
				return diseaseSourceInfo.factor * exposureTier;
			}
			return 0f;
		}

		// Token: 0x040056E3 RID: 22243
		[Serialize]
		public Dictionary<HashedString, GermExposureMonitor.Instance.DiseaseSourceInfo> lastDiseaseSources;

		// Token: 0x040056E4 RID: 22244
		[Serialize]
		public Dictionary<HashedString, float> lastExposureTime;

		// Token: 0x040056E5 RID: 22245
		private Dictionary<HashedString, GermExposureMonitor.Instance.InhaleTickInfo> inhaleExposureTick;

		// Token: 0x040056E6 RID: 22246
		private string[] inateImmunities;

		// Token: 0x040056E7 RID: 22247
		private Sicknesses sicknesses;

		// Token: 0x040056E8 RID: 22248
		private PrimaryElement primaryElement;

		// Token: 0x040056E9 RID: 22249
		private Traits traits;

		// Token: 0x040056EA RID: 22250
		[Serialize]
		private Dictionary<string, GermExposureMonitor.ExposureState> exposureStates = new Dictionary<string, GermExposureMonitor.ExposureState>();

		// Token: 0x040056EB RID: 22251
		[Serialize]
		private Dictionary<string, float> exposureTiers = new Dictionary<string, float>();

		// Token: 0x040056EC RID: 22252
		private Dictionary<string, Guid> statusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x040056ED RID: 22253
		private Dictionary<string, Guid> contactStatusItemHandles = new Dictionary<string, Guid>();

		// Token: 0x020015C8 RID: 5576
		[Serializable]
		public class DiseaseSourceInfo
		{
			// Token: 0x060073CC RID: 29644 RVA: 0x000F0572 File Offset: 0x000EE772
			public DiseaseSourceInfo(Tag sourceObject, Sickness.InfectionVector vector, float factor, Vector3 position)
			{
				this.sourceObject = sourceObject;
				this.vector = vector;
				this.factor = factor;
				this.position = position;
			}

			// Token: 0x040056EE RID: 22254
			public Tag sourceObject;

			// Token: 0x040056EF RID: 22255
			public Sickness.InfectionVector vector;

			// Token: 0x040056F0 RID: 22256
			public float factor;

			// Token: 0x040056F1 RID: 22257
			public Vector3 position;
		}

		// Token: 0x020015C9 RID: 5577
		public class InhaleTickInfo
		{
			// Token: 0x040056F2 RID: 22258
			public bool inhaled;

			// Token: 0x040056F3 RID: 22259
			public int ticks;
		}
	}
}
