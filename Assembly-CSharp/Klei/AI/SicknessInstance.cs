using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C79 RID: 15481
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SicknessInstance : ModifierInstance<Sickness>, ISaveLoadable
	{
		// Token: 0x17000C59 RID: 3161
		// (get) Token: 0x0600ED84 RID: 60804 RVA: 0x00143DB2 File Offset: 0x00141FB2
		public Sickness Sickness
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x17000C5A RID: 3162
		// (get) Token: 0x0600ED85 RID: 60805 RVA: 0x004E2310 File Offset: 0x004E0510
		public float TotalCureSpeedMultiplier
		{
			get
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.DiseaseCureSpeed.Lookup(this.smi.master.gameObject);
				AttributeInstance attributeInstance2 = this.modifier.cureSpeedBase.Lookup(this.smi.master.gameObject);
				float num = 1f;
				if (attributeInstance != null)
				{
					num *= attributeInstance.GetTotalValue();
				}
				if (attributeInstance2 != null)
				{
					num *= attributeInstance2.GetTotalValue();
				}
				return num;
			}
		}

		// Token: 0x17000C5B RID: 3163
		// (get) Token: 0x0600ED86 RID: 60806 RVA: 0x004E2384 File Offset: 0x004E0584
		public bool IsDoctored
		{
			get
			{
				if (base.gameObject == null)
				{
					return false;
				}
				AttributeInstance attributeInstance = Db.Get().Attributes.DoctoredLevel.Lookup(base.gameObject);
				return attributeInstance != null && attributeInstance.GetTotalValue() > 0f;
			}
		}

		// Token: 0x0600ED87 RID: 60807 RVA: 0x00143DBA File Offset: 0x00141FBA
		public SicknessInstance(GameObject game_object, Sickness disease) : base(game_object, disease)
		{
		}

		// Token: 0x0600ED88 RID: 60808 RVA: 0x00143DC4 File Offset: 0x00141FC4
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeAndStart();
		}

		// Token: 0x17000C5C RID: 3164
		// (get) Token: 0x0600ED89 RID: 60809 RVA: 0x00143DCC File Offset: 0x00141FCC
		// (set) Token: 0x0600ED8A RID: 60810 RVA: 0x00143DD4 File Offset: 0x00141FD4
		public SicknessExposureInfo ExposureInfo
		{
			get
			{
				return this.exposureInfo;
			}
			set
			{
				this.exposureInfo = value;
				this.InitializeAndStart();
			}
		}

		// Token: 0x0600ED8B RID: 60811 RVA: 0x004E23D0 File Offset: 0x004E05D0
		private void InitializeAndStart()
		{
			Sickness disease = this.modifier;
			Func<List<Notification>, object, string> tooltip = delegate(List<Notification> notificationList, object data)
			{
				string text = "";
				for (int i = 0; i < notificationList.Count; i++)
				{
					Notification notification = notificationList[i];
					string arg = (string)notification.tooltipData;
					text += string.Format(DUPLICANTS.DISEASES.NOTIFICATION_TOOLTIP, notification.NotifierName, disease.Name, arg);
					if (i < notificationList.Count - 1)
					{
						text += "\n";
					}
				}
				return text;
			};
			string name = disease.Name;
			string title = name;
			NotificationType type = (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad;
			object sourceInfo = this.exposureInfo.sourceInfo;
			this.notification = new Notification(title, type, tooltip, sourceInfo, true, 0f, null, null, null, true, false, false);
			this.statusItem = new StatusItem(disease.Id, disease.Name, DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.TEMPLATE, "", (disease.severity <= Sickness.Severity.Minor) ? StatusItem.IconType.Info : StatusItem.IconType.Exclamation, (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveString);
			if (this.smi != null)
			{
				this.smi.StopSM("refresh");
			}
			this.smi = new SicknessInstance.StatesInstance(this);
			this.smi.StartSM();
		}

		// Token: 0x0600ED8C RID: 60812 RVA: 0x004E24E8 File Offset: 0x004E06E8
		private string ResolveString(string str, object data)
		{
			if (this.smi == null)
			{
				global::Debug.LogWarning("Attempting to resolve string when smi is null");
				return str;
			}
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			str = str.Replace("{Descriptor}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DESCRIPTOR, Strings.Get("STRINGS.DUPLICANTS.DISEASES.SEVERITY." + this.modifier.severity.ToString().ToUpper()), Strings.Get("STRINGS.DUPLICANTS.DISEASES.TYPE." + this.modifier.sicknessType.ToString().ToUpper())));
			str = str.Replace("{Infectee}", component.GetProperName());
			str = str.Replace("{InfectionSource}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.INFECTION_SOURCE, this.exposureInfo.sourceInfo));
			if (this.modifier.severity <= Sickness.Severity.Minor)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
			}
			else if (this.modifier.severity == Sickness.Severity.Major)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
				if (!this.IsDoctored)
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.BEDREST);
				}
				else
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			else if (this.modifier.severity >= Sickness.Severity.Critical)
			{
				if (!this.IsDoctored)
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.FATALITY, GameUtil.GetFormattedCycles(this.GetFatalityTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTOR_REQUIRED);
				}
				else
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			List<Descriptor> symptoms = this.modifier.GetSymptoms(this.smi.gameObject);
			string text = "";
			foreach (Descriptor descriptor in symptoms)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text = text + "    • " + descriptor.text;
			}
			str = str.Replace("{Symptoms}", text);
			str = Regex.Replace(str, "{[^}]*}", "");
			return str;
		}

		// Token: 0x0600ED8D RID: 60813 RVA: 0x00143DE3 File Offset: 0x00141FE3
		public float GetInfectedTimeRemaining()
		{
			return this.modifier.SicknessDuration * (1f - this.smi.sm.percentRecovered.Get(this.smi)) / this.TotalCureSpeedMultiplier;
		}

		// Token: 0x0600ED8E RID: 60814 RVA: 0x00143E19 File Offset: 0x00142019
		public float GetFatalityTimeRemaining()
		{
			return this.modifier.fatalityDuration * (1f - this.smi.sm.percentDied.Get(this.smi));
		}

		// Token: 0x0600ED8F RID: 60815 RVA: 0x00143E48 File Offset: 0x00142048
		public float GetPercentCured()
		{
			if (this.smi == null)
			{
				return 0f;
			}
			return this.smi.sm.percentRecovered.Get(this.smi);
		}

		// Token: 0x0600ED90 RID: 60816 RVA: 0x00143E73 File Offset: 0x00142073
		public void SetPercentCured(float pct)
		{
			this.smi.sm.percentRecovered.Set(pct, this.smi, false);
		}

		// Token: 0x0600ED91 RID: 60817 RVA: 0x00143E93 File Offset: 0x00142093
		public void Cure()
		{
			this.smi.Cure();
		}

		// Token: 0x0600ED92 RID: 60818 RVA: 0x00143EA0 File Offset: 0x001420A0
		public override void OnCleanUp()
		{
			if (this.smi != null)
			{
				this.smi.StopSM("DiseaseInstance.OnCleanUp");
				this.smi = null;
			}
		}

		// Token: 0x0600ED93 RID: 60819 RVA: 0x00143EC1 File Offset: 0x001420C1
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x0600ED94 RID: 60820 RVA: 0x00143EC9 File Offset: 0x001420C9
		public List<Descriptor> GetDescriptors()
		{
			return this.modifier.GetSicknessSourceDescriptors();
		}

		// Token: 0x0400E998 RID: 59800
		[Serialize]
		private SicknessExposureInfo exposureInfo;

		// Token: 0x0400E999 RID: 59801
		private SicknessInstance.StatesInstance smi;

		// Token: 0x0400E99A RID: 59802
		private StatusItem statusItem;

		// Token: 0x0400E99B RID: 59803
		private Notification notification;

		// Token: 0x02003C7A RID: 15482
		private struct CureInfo
		{
			// Token: 0x0400E99C RID: 59804
			public string name;

			// Token: 0x0400E99D RID: 59805
			public float multiplier;
		}

		// Token: 0x02003C7B RID: 15483
		public class StatesInstance : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600ED95 RID: 60821 RVA: 0x00143ED6 File Offset: 0x001420D6
			public StatesInstance(SicknessInstance master) : base(master)
			{
			}

			// Token: 0x0600ED96 RID: 60822 RVA: 0x004E27BC File Offset: 0x004E09BC
			public void UpdateProgress(float dt)
			{
				float delta_value = dt * base.master.TotalCureSpeedMultiplier / base.master.modifier.SicknessDuration;
				base.sm.percentRecovered.Delta(delta_value, base.smi);
				if (base.master.modifier.fatalityDuration > 0f)
				{
					if (!base.master.IsDoctored)
					{
						float delta_value2 = dt / base.master.modifier.fatalityDuration;
						base.sm.percentDied.Delta(delta_value2, base.smi);
						return;
					}
					base.sm.percentDied.Set(0f, base.smi, false);
				}
			}

			// Token: 0x0600ED97 RID: 60823 RVA: 0x004E2870 File Offset: 0x004E0A70
			public void Infect()
			{
				Sickness modifier = base.master.modifier;
				this.componentData = modifier.Infect(base.gameObject, base.master, base.master.exposureInfo);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, string.Format(DUPLICANTS.DISEASES.INFECTED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
			}

			// Token: 0x0600ED98 RID: 60824 RVA: 0x004E28F4 File Offset: 0x004E0AF4
			public void Cure()
			{
				Sickness modifier = base.master.modifier;
				base.gameObject.GetComponent<Modifiers>().sicknesses.Cure(modifier);
				modifier.Cure(base.gameObject, this.componentData);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, string.Format(DUPLICANTS.DISEASES.CURED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
				if (!string.IsNullOrEmpty(modifier.recoveryEffect))
				{
					Effects component = base.gameObject.GetComponent<Effects>();
					if (component)
					{
						component.Add(modifier.recoveryEffect, true);
					}
				}
			}

			// Token: 0x0600ED99 RID: 60825 RVA: 0x00143EDF File Offset: 0x001420DF
			public SicknessExposureInfo GetExposureInfo()
			{
				return base.master.ExposureInfo;
			}

			// Token: 0x0400E99E RID: 59806
			private object[] componentData;
		}

		// Token: 0x02003C7C RID: 15484
		public class States : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600ED9A RID: 60826 RVA: 0x004E29B0 File Offset: 0x004E0BB0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.infected;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.infected.Enter("Infect", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.Infect();
				}).DoNotification((SicknessInstance.StatesInstance smi) => smi.master.notification).Update("UpdateProgress", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					smi.UpdateProgress(dt);
				}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SicknessInstance.StatesInstance smi) => smi.master.GetStatusItem(), (SicknessInstance.StatesInstance smi) => smi).ParamTransition<float>(this.percentRecovered, this.cured, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne).ParamTransition<float>(this.percentDied, this.fatality_pre, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne);
				this.cured.Enter("Cure", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.master.Cure();
				});
				this.fatality_pre.Update("DeathByDisease", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					DeathMonitor.Instance smi2 = smi.master.gameObject.GetSMI<DeathMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.Kill(Db.Get().Deaths.FatalDisease);
						smi.GoTo(this.fatality);
					}
				}, UpdateRate.SIM_200ms, false);
				this.fatality.DoNothing();
			}

			// Token: 0x0400E99F RID: 59807
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentRecovered;

			// Token: 0x0400E9A0 RID: 59808
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentDied;

			// Token: 0x0400E9A1 RID: 59809
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State infected;

			// Token: 0x0400E9A2 RID: 59810
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State cured;

			// Token: 0x0400E9A3 RID: 59811
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality_pre;

			// Token: 0x0400E9A4 RID: 59812
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality;
		}
	}
}
