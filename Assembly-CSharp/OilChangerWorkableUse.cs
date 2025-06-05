using System;
using Klei;
using Klei.AI;
using UnityEngine;

// Token: 0x02000F26 RID: 3878
public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06004DBF RID: 19903 RVA: 0x000B09A4 File Offset: 0x000AEBA4
	private OilChangerWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x00274AD8 File Offset: 0x00272CD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x00274B28 File Offset: 0x00272D28
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			worker.transform.SetPosition(position);
		}
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		if (roomOfGameObject != null)
		{
			roomOfGameObject.roomType.TriggerRoomEffects(base.GetComponent<KPrefabID>(), worker.GetComponent<Effects>());
		}
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x00274BAC File Offset: 0x00272DAC
	protected override void OnStopWork(WorkerBase worker)
	{
		if (worker != null)
		{
			Vector3 position = worker.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			worker.transform.SetPosition(position);
		}
		this.operational.SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x00274C00 File Offset: 0x00272E00
	protected override void OnCompleteWork(WorkerBase worker)
	{
		Storage component = base.GetComponent<Storage>();
		BionicOilMonitor.Instance smi = worker.GetSMI<BionicOilMonitor.Instance>();
		if (smi != null)
		{
			float b = 200f - smi.CurrentOilMass;
			float num = Mathf.Min(component.GetMassAvailable(GameTags.LubricatingOil), b);
			float num2 = num;
			float num3 = 0f;
			Storage component2 = base.GetComponent<Storage>();
			SimHashes simHashes = SimHashes.CrudeOil;
			foreach (SimHashes simHashes2 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo;
				float num5;
				component2.ConsumeAndGetDisease(simHashes2.CreateTag(), num2, out num4, out diseaseInfo, out num5);
				if (num4 > num3)
				{
					simHashes = simHashes2;
					num3 = num4;
				}
				num2 -= num4;
			}
			base.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
			smi.RefillOil(num);
			Effects component3 = worker.GetComponent<Effects>();
			foreach (SimHashes simHashes3 in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				Effect effect = BionicOilMonitor.LUBRICANT_TYPE_EFFECT[simHashes3];
				if (simHashes == simHashes3)
				{
					component3.Add(effect, true);
				}
				else
				{
					component3.Remove(effect);
				}
			}
		}
		base.OnCompleteWork(worker);
	}

	// Token: 0x04003693 RID: 13971
	private Operational operational;
}
