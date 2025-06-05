using System;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001015 RID: 4117
[AddComponentMenu("KMonoBehaviour/scripts/SweepBotStation")]
public class SweepBotStation : KMonoBehaviour
{
	// Token: 0x06005323 RID: 21283 RVA: 0x000DAA4A File Offset: 0x000D8C4A
	public void SetStorages(Storage botMaterialStorage, Storage sweepStorage)
	{
		this.botMaterialStorage = botMaterialStorage;
		this.sweepStorage = sweepStorage;
	}

	// Token: 0x06005324 RID: 21284 RVA: 0x000DAA5A File Offset: 0x000D8C5A
	protected override void OnPrefabInit()
	{
		this.Initialize(false);
		base.Subscribe<SweepBotStation>(-592767678, SweepBotStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x000DAA74 File Offset: 0x000D8C74
	protected void Initialize(bool use_logic_meter)
	{
		base.OnPrefabInit();
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, false);
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x00285C70 File Offset: 0x00283E70
	protected override void OnSpawn()
	{
		base.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		this.meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_frame",
			"meter_level"
		});
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		else
		{
			StorageUnloadMonitor.Instance smi = this.sweepBot.Get().GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			this.RefreshSweepBotSubscription();
		}
		this.UpdateMeter();
		this.UpdateNameDisplay();
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x00285D30 File Offset: 0x00283F30
	private void RequestNewSweepBot(object data = null)
	{
		if (this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS) == null)
		{
			FetchList2 fetchList = new FetchList2(this.botMaterialStorage, Db.Get().ChoreTypes.Fetch);
			fetchList.Add(base.GetComponent<PrimaryElement>().Element.tag, null, SweepBotConfig.MASS, Operational.State.None);
			fetchList.Submit(null, true);
			return;
		}
		this.MakeNewSweepBot(null);
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x00285DA0 File Offset: 0x00283FA0
	private void MakeNewSweepBot(object data = null)
	{
		if (this.newSweepyHandle.IsValid)
		{
			return;
		}
		if (this.botMaterialStorage.GetAmountAvailable(GameTags.RefinedMetal) < SweepBotConfig.MASS)
		{
			return;
		}
		PrimaryElement primaryElement = this.botMaterialStorage.FindFirstWithMass(GameTags.RefinedMetal, SweepBotConfig.MASS);
		if (primaryElement == null)
		{
			return;
		}
		SimHashes sweepBotMaterial = primaryElement.ElementID;
		float temperature;
		SimUtil.DiseaseInfo disease;
		float num;
		this.botMaterialStorage.ConsumeAndGetDisease(sweepBotMaterial.CreateTag(), SweepBotConfig.MASS, out num, out disease, out temperature);
		this.UpdateMeter();
		this.newSweepyHandle = GameScheduler.Instance.Schedule("MakeSweepy", 2f, delegate(object obj)
		{
			GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab("SweepBot"), Grid.CellToPos(Grid.CellRight(Grid.PosToCell(this.gameObject))), Grid.SceneLayer.Creatures, null, 0);
			gameObject.SetActive(true);
			this.sweepBot = new Ref<KSelectable>(gameObject.GetComponent<KSelectable>());
			if (!string.IsNullOrEmpty(this.storedName))
			{
				this.sweepBot.Get().GetComponent<UserNameable>().SetName(this.storedName);
			}
			this.UpdateNameDisplay();
			StorageUnloadMonitor.Instance smi = gameObject.GetSMI<StorageUnloadMonitor.Instance>();
			smi.sm.sweepLocker.Set(this.sweepStorage, smi, false);
			PrimaryElement component = this.sweepBot.Get().GetComponent<PrimaryElement>();
			component.ElementID = sweepBotMaterial;
			component.Temperature = temperature;
			if (disease.idx != 255)
			{
				component.AddDisease(disease.idx, disease.count, "Inherited from the material used for its creation");
			}
			this.RefreshSweepBotSubscription();
			this.newSweepyHandle.ClearScheduler();
		}, null, null);
		base.GetComponent<KBatchedAnimController>().Play("newsweepy", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06005329 RID: 21289 RVA: 0x00285E84 File Offset: 0x00284084
	private void RefreshSweepBotSubscription()
	{
		if (this.refreshSweepbotHandle != -1)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
			this.sweepBot.Get().Unsubscribe(this.sweepBotNameChangeHandle);
		}
		this.refreshSweepbotHandle = this.sweepBot.Get().Subscribe(1969584890, new Action<object>(this.RequestNewSweepBot));
		this.sweepBotNameChangeHandle = this.sweepBot.Get().Subscribe(1102426921, new Action<object>(this.UpdateStoredName));
	}

	// Token: 0x0600532A RID: 21290 RVA: 0x000DAA8D File Offset: 0x000D8C8D
	private void UpdateStoredName(object data)
	{
		this.storedName = (string)data;
		this.UpdateNameDisplay();
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x00285F14 File Offset: 0x00284114
	private void UpdateNameDisplay()
	{
		if (string.IsNullOrEmpty(this.storedName))
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, ROBOTS.MODELS.SWEEPBOT.NAME));
		}
		else
		{
			base.GetComponent<KSelectable>().SetName(string.Format(BUILDINGS.PREFABS.SWEEPBOTSTATION.NAMEDSTATION, this.storedName));
		}
		NameDisplayScreen.Instance.UpdateName(base.gameObject);
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x000DAAA1 File Offset: 0x000D8CA1
	public void DockRobot(bool docked)
	{
		base.GetComponent<Operational>().SetFlag(SweepBotStation.dockedRobot, docked);
	}

	// Token: 0x0600532D RID: 21293 RVA: 0x00285F80 File Offset: 0x00284180
	public void StartCharging()
	{
		base.GetComponent<KBatchedAnimController>().Queue("sleep_pre", KAnim.PlayMode.Once, 1f, 0f);
		base.GetComponent<KBatchedAnimController>().Queue("sleep_idle", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600532E RID: 21294 RVA: 0x000DAAB4 File Offset: 0x000D8CB4
	public void StopCharging()
	{
		base.GetComponent<KBatchedAnimController>().Play("sleep_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.UpdateNameDisplay();
	}

	// Token: 0x0600532F RID: 21295 RVA: 0x00285FD0 File Offset: 0x002841D0
	protected override void OnCleanUp()
	{
		if (this.newSweepyHandle.IsValid)
		{
			this.newSweepyHandle.ClearScheduler();
		}
		if (this.refreshSweepbotHandle != -1 && this.sweepBot.Get() != null)
		{
			this.sweepBot.Get().Unsubscribe(this.refreshSweepbotHandle);
		}
	}

	// Token: 0x06005330 RID: 21296 RVA: 0x00286028 File Offset: 0x00284228
	private void UpdateMeter()
	{
		float maxCapacityMinusStorageMargin = this.GetMaxCapacityMinusStorageMargin();
		float positionPercent = Mathf.Clamp01(this.GetAmountStored() / maxCapacityMinusStorageMargin);
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(positionPercent);
		}
	}

	// Token: 0x06005331 RID: 21297 RVA: 0x00286060 File Offset: 0x00284260
	private void OnStorageChanged(object data)
	{
		this.UpdateMeter();
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (component.currentFrame >= component.GetCurrentNumFrames())
		{
			base.GetComponent<KBatchedAnimController>().Play("remove", KAnim.PlayMode.Once, 1f, 0f);
		}
		for (int i = 0; i < this.sweepStorage.Count; i++)
		{
			this.sweepStorage[i].GetComponent<Clearable>().MarkForClear(false, true);
		}
	}

	// Token: 0x06005332 RID: 21298 RVA: 0x000DAADC File Offset: 0x000D8CDC
	private void OnOperationalChanged(object data)
	{
		Operational component = base.GetComponent<Operational>();
		component.SetActive(!component.Flags.ContainsValue(false), false);
		if (this.sweepBot == null || this.sweepBot.Get() == null)
		{
			this.RequestNewSweepBot(null);
		}
	}

	// Token: 0x06005333 RID: 21299 RVA: 0x000DAB1B File Offset: 0x000D8D1B
	private float GetMaxCapacityMinusStorageMargin()
	{
		return this.sweepStorage.Capacity() - this.sweepStorage.storageFullMargin;
	}

	// Token: 0x06005334 RID: 21300 RVA: 0x000DAB34 File Offset: 0x000D8D34
	private float GetAmountStored()
	{
		return this.sweepStorage.MassStored();
	}

	// Token: 0x04003AAB RID: 15019
	[Serialize]
	public Ref<KSelectable> sweepBot;

	// Token: 0x04003AAC RID: 15020
	[Serialize]
	public string storedName;

	// Token: 0x04003AAD RID: 15021
	private static readonly Operational.Flag dockedRobot = new Operational.Flag("dockedRobot", Operational.Flag.Type.Functional);

	// Token: 0x04003AAE RID: 15022
	private MeterController meter;

	// Token: 0x04003AAF RID: 15023
	[SerializeField]
	private Storage botMaterialStorage;

	// Token: 0x04003AB0 RID: 15024
	[SerializeField]
	private Storage sweepStorage;

	// Token: 0x04003AB1 RID: 15025
	private SchedulerHandle newSweepyHandle;

	// Token: 0x04003AB2 RID: 15026
	private static readonly EventSystem.IntraObjectHandler<SweepBotStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SweepBotStation>(delegate(SweepBotStation component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003AB3 RID: 15027
	private int refreshSweepbotHandle = -1;

	// Token: 0x04003AB4 RID: 15028
	private int sweepBotNameChangeHandle = -1;
}
