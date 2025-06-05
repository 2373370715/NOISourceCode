using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001510 RID: 5392
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
	// Token: 0x06007024 RID: 28708 RVA: 0x00303518 File Offset: 0x00301718
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

	// Token: 0x06007025 RID: 28709 RVA: 0x00303578 File Offset: 0x00301778
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = worker.GetComponent<Effects>();
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x06007026 RID: 28710 RVA: 0x000EDE80 File Offset: 0x000EC080
	private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

	// Token: 0x06007027 RID: 28711 RVA: 0x0030366C File Offset: 0x0030186C
	public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x1700072B RID: 1835
	// (get) Token: 0x06007028 RID: 28712 RVA: 0x002C3078 File Offset: 0x002C1278
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x1700072C RID: 1836
	// (get) Token: 0x06007029 RID: 28713 RVA: 0x000E446A File Offset: 0x000E266A
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x0600702A RID: 28714 RVA: 0x000EDE89 File Offset: 0x000EC089
	public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x0600702B RID: 28715 RVA: 0x000B1628 File Offset: 0x000AF828
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x0600702C RID: 28716 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04005435 RID: 21557
	public MedicinalPill pill;
}
