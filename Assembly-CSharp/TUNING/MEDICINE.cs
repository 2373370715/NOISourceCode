using System;

namespace TUNING
{
	// Token: 0x020022D3 RID: 8915
	public class MEDICINE
	{
		// Token: 0x04009C35 RID: 39989
		public const float DEFAULT_MASS = 1f;

		// Token: 0x04009C36 RID: 39990
		public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;

		// Token: 0x04009C37 RID: 39991
		public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;

		// Token: 0x04009C38 RID: 39992
		public const float WORK_TIME = 10f;

		// Token: 0x04009C39 RID: 39993
		public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("BasicBooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04009C3A RID: 39994
		public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("IntermediateBooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04009C3B RID: 39995
		public static readonly MedicineInfo BASICCURE = new MedicineInfo("BasicCure", null, MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"FoodSickness"
		});

		// Token: 0x04009C3C RID: 39996
		public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("Antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, null, new string[]
		{
			"Allergies"
		});

		// Token: 0x04009C3D RID: 39997
		public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("IntermediateCure", null, MedicineInfo.MedicineType.CureSpecific, "DoctorStation", new string[]
		{
			"SlimeSickness"
		});

		// Token: 0x04009C3E RID: 39998
		public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("AdvancedCure", null, MedicineInfo.MedicineType.CureSpecific, "AdvancedDoctorStation", new string[]
		{
			"ZombieSickness"
		});

		// Token: 0x04009C3F RID: 39999
		public static readonly MedicineInfo BASICRADPILL = new MedicineInfo("BasicRadPill", "Medicine_BasicRadPill", MedicineInfo.MedicineType.Booster, null, null);

		// Token: 0x04009C40 RID: 40000
		public static readonly MedicineInfo INTERMEDIATERADPILL = new MedicineInfo("IntermediateRadPill", "Medicine_IntermediateRadPill", MedicineInfo.MedicineType.Booster, "AdvancedDoctorStation", null);
	}
}
