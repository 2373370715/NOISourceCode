using System;
using System.Collections.Generic;

// Token: 0x0200150D RID: 5389
[Serializable]
public class MedicineInfo
{
	// Token: 0x0600701D RID: 28701 RVA: 0x003031A4 File Offset: 0x003013A4
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases = null)
	{
		Debug.Assert(!string.IsNullOrEmpty(effect) || (curedDiseases != null && curedDiseases.Length != 0), "Medicine should have an effect or cure diseases");
		this.id = id;
		this.effect = effect;
		this.medicineType = medicineType;
		this.doctorStationId = doctorStationId;
		if (curedDiseases != null)
		{
			this.curedSicknesses = new List<string>(curedDiseases);
			return;
		}
		this.curedSicknesses = new List<string>();
	}

	// Token: 0x0600701E RID: 28702 RVA: 0x000EDE6A File Offset: 0x000EC06A
	public Tag GetSupplyTag()
	{
		return MedicineInfo.GetSupplyTagForStation(this.doctorStationId);
	}

	// Token: 0x0600701F RID: 28703 RVA: 0x00303214 File Offset: 0x00301414
	public static Tag GetSupplyTagForStation(string stationID)
	{
		Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
		Assets.AddCountableTag(tag);
		return tag;
	}

	// Token: 0x0400542B RID: 21547
	public string id;

	// Token: 0x0400542C RID: 21548
	public string effect;

	// Token: 0x0400542D RID: 21549
	public MedicineInfo.MedicineType medicineType;

	// Token: 0x0400542E RID: 21550
	public List<string> curedSicknesses;

	// Token: 0x0400542F RID: 21551
	public string doctorStationId;

	// Token: 0x0200150E RID: 5390
	public enum MedicineType
	{
		// Token: 0x04005431 RID: 21553
		Booster,
		// Token: 0x04005432 RID: 21554
		CureAny,
		// Token: 0x04005433 RID: 21555
		CureSpecific
	}
}
