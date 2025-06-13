using System;
using Klei;
using Klei.AI;
using UnityEngine;

public class OilChangerWorkableUse : Workable, IGameObjectEffectDescriptor
{
	private OilChangerWorkableUse()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.operational = base.GetComponent<Operational>();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.attributeConverter = Db.Get().AttributeConverters.ToiletSpeed;
		base.SetWorkTime(8.5f);
	}

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
			SimHashes lubricant = SimHashes.CrudeOil;
			foreach (SimHashes simHashes in BionicOilMonitor.LUBRICANT_TYPE_EFFECT.Keys)
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo;
				float num5;
				component2.ConsumeAndGetDisease(simHashes.CreateTag(), num2, out num4, out diseaseInfo, out num5);
				if (num4 > num3)
				{
					lubricant = simHashes;
					num3 = num4;
				}
				num2 -= num4;
			}
			base.GetComponent<Storage>().ConsumeIgnoringDisease(GameTags.LubricatingOil, num2);
			smi.RefillOil(num);
			BionicOilMonitor.ApplyLubricationEffects(worker.GetComponent<Effects>(), lubricant);
		}
		base.OnCompleteWork(worker);
	}

	private Operational operational;
}
