using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003C7F RID: 15487
	public class Sicknesses : Modifications<Sickness, SicknessInstance>
	{
		// Token: 0x0600EDA7 RID: 60839 RVA: 0x00143F38 File Offset: 0x00142138
		public Sicknesses(GameObject go) : base(go, Db.Get().Sicknesses)
		{
		}

		// Token: 0x0600EDA8 RID: 60840 RVA: 0x004E2BD4 File Offset: 0x004E0DD4
		public void Infect(SicknessExposureInfo exposure_info)
		{
			Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
			if (!base.Has(modifier))
			{
				this.CreateInstance(modifier).ExposureInfo = exposure_info;
			}
		}

		// Token: 0x0600EDA9 RID: 60841 RVA: 0x004E2C10 File Offset: 0x004E0E10
		public override SicknessInstance CreateInstance(Sickness sickness)
		{
			SicknessInstance sicknessInstance = new SicknessInstance(base.gameObject, sickness);
			this.Add(sicknessInstance);
			base.Trigger(GameHashes.SicknessAdded, sicknessInstance);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, base.gameObject.GetProperName(), null);
			return sicknessInstance;
		}

		// Token: 0x0600EDAA RID: 60842 RVA: 0x00143F4B File Offset: 0x0014214B
		public bool IsInfected()
		{
			return base.Count > 0;
		}

		// Token: 0x0600EDAB RID: 60843 RVA: 0x00143F56 File Offset: 0x00142156
		public bool Cure(Sickness sickness)
		{
			return this.Cure(sickness.Id);
		}

		// Token: 0x0600EDAC RID: 60844 RVA: 0x004E2C5C File Offset: 0x004E0E5C
		public bool Cure(string sickness_id)
		{
			SicknessInstance sicknessInstance = null;
			foreach (SicknessInstance sicknessInstance2 in this)
			{
				if (sicknessInstance2.modifier.Id == sickness_id)
				{
					sicknessInstance = sicknessInstance2;
					break;
				}
			}
			if (sicknessInstance != null)
			{
				this.Remove(sicknessInstance);
				base.Trigger(GameHashes.SicknessCured, sicknessInstance);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, base.gameObject.GetProperName(), null);
				return true;
			}
			return false;
		}
	}
}
