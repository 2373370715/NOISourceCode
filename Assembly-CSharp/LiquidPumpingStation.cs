using System;
using Klei;
using UnityEngine;

// Token: 0x02000E69 RID: 3689
[AddComponentMenu("KMonoBehaviour/Workable/LiquidPumpingStation")]
public class LiquidPumpingStation : Workable, ISim200ms
{
	// Token: 0x0600482C RID: 18476 RVA: 0x000D33B8 File Offset: 0x000D15B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.resetProgressOnStop = true;
		this.showProgressBar = false;
	}

	// Token: 0x0600482D RID: 18477 RVA: 0x00263090 File Offset: 0x00261290
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.infos = new LiquidPumpingStation.LiquidInfo[LiquidPumpingStation.liquidOffsets.Length * 2];
		this.RefreshStatusItem();
		this.Sim200ms(0f);
		base.SetWorkTime(10f);
		this.RefreshDepthAvailable();
		this.RegisterListenersToCellChanges();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_arrow",
			"meter_scale"
		});
		foreach (GameObject gameObject in base.GetComponent<Storage>().items)
		{
			if (!(gameObject == null) && gameObject != null)
			{
				gameObject.DeleteObject();
			}
		}
	}

	// Token: 0x0600482E RID: 18478 RVA: 0x00263178 File Offset: 0x00261378
	private void RegisterListenersToCellChanges()
	{
		int widthInCells = base.GetComponent<BuildingComplete>().Def.WidthInCells;
		CellOffset[] array = new CellOffset[widthInCells * 4];
		for (int i = 0; i < 4; i++)
		{
			int y = -(i + 1);
			for (int j = 0; j < widthInCells; j++)
			{
				array[i * widthInCells + j] = new CellOffset(j, y);
			}
		}
		Extents extents = new Extents(Grid.PosToCell(base.transform.GetPosition()), array);
		this.partitionerEntry_solids = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new Action<object>(this.OnLowerCellChanged));
		this.partitionerEntry_buildings = GameScenePartitioner.Instance.Add("LiquidPumpingStation", base.gameObject, extents, GameScenePartitioner.Instance.objectLayers[1], new Action<object>(this.OnLowerCellChanged));
	}

	// Token: 0x0600482F RID: 18479 RVA: 0x000D33CE File Offset: 0x000D15CE
	private void UnregisterListenersToCellChanges()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_solids);
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry_buildings);
	}

	// Token: 0x06004830 RID: 18480 RVA: 0x000D33F0 File Offset: 0x000D15F0
	private void OnLowerCellChanged(object o)
	{
		this.RefreshDepthAvailable();
	}

	// Token: 0x06004831 RID: 18481 RVA: 0x00263254 File Offset: 0x00261454
	private void RefreshDepthAvailable()
	{
		int num = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), base.gameObject);
		int num2 = 4;
		if (num != this.depthAvailable)
		{
			KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
			for (int i = 1; i <= num2; i++)
			{
				component.SetSymbolVisiblity("pipe" + i.ToString(), i <= num);
			}
			PumpingStationGuide.OccupyArea(base.gameObject, num);
			this.depthAvailable = num;
		}
	}

	// Token: 0x06004832 RID: 18482 RVA: 0x002632C8 File Offset: 0x002614C8
	public void Sim200ms(float dt)
	{
		if (this.session != null)
		{
			return;
		}
		int num = this.infoCount;
		for (int i = 0; i < this.infoCount; i++)
		{
			this.infos[i].amount = 0f;
		}
		if (base.GetComponent<Operational>().IsOperational)
		{
			int cell = Grid.PosToCell(this);
			for (int j = 0; j < LiquidPumpingStation.liquidOffsets.Length; j++)
			{
				if (this.depthAvailable >= Math.Abs(LiquidPumpingStation.liquidOffsets[j].y))
				{
					int num2 = Grid.OffsetCell(cell, LiquidPumpingStation.liquidOffsets[j]);
					bool flag = false;
					Element element = Grid.Element[num2];
					if (element.IsLiquid)
					{
						float num3 = Grid.Mass[num2];
						for (int k = 0; k < this.infoCount; k++)
						{
							if (this.infos[k].element == element)
							{
								LiquidPumpingStation.LiquidInfo[] array = this.infos;
								int num4 = k;
								array[num4].amount = array[num4].amount + num3;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.infos[this.infoCount].amount = num3;
							this.infos[this.infoCount].element = element;
							this.infoCount++;
						}
					}
				}
			}
		}
		int l = 0;
		while (l < this.infoCount)
		{
			LiquidPumpingStation.LiquidInfo liquidInfo = this.infos[l];
			if (liquidInfo.amount <= 1f)
			{
				if (liquidInfo.source != null)
				{
					liquidInfo.source.DeleteObject();
				}
				this.infos[l] = this.infos[this.infoCount - 1];
				this.infoCount--;
			}
			else
			{
				if (liquidInfo.source == null)
				{
					liquidInfo.source = base.GetComponent<Storage>().AddLiquid(liquidInfo.element.id, liquidInfo.amount, liquidInfo.element.defaultValues.temperature, byte.MaxValue, 0, false, true).GetComponent<SubstanceChunk>();
					Pickupable component = liquidInfo.source.GetComponent<Pickupable>();
					component.KPrefabID.AddTag(GameTags.LiquidSource, false);
					component.SetOffsets(new CellOffset[]
					{
						new CellOffset(0, 1)
					});
					component.targetWorkable = this;
					Pickupable pickupable = component;
					pickupable.OnReservationsChanged = (Action<Pickupable, bool, Pickupable.Reservation>)Delegate.Combine(pickupable.OnReservationsChanged, new Action<Pickupable, bool, Pickupable.Reservation>(this.OnReservationsChanged));
				}
				liquidInfo.source.GetComponent<Pickupable>().TotalAmount = liquidInfo.amount;
				this.infos[l] = liquidInfo;
				l++;
			}
		}
		if (num != this.infoCount)
		{
			this.RefreshStatusItem();
		}
	}

	// Token: 0x06004833 RID: 18483 RVA: 0x00263598 File Offset: 0x00261798
	private void RefreshStatusItem()
	{
		if (this.infoCount > 0)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.PumpingStation, this);
			return;
		}
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmptyPumpingStation, this);
	}

	// Token: 0x06004834 RID: 18484 RVA: 0x00263608 File Offset: 0x00261808
	public string ResolveString(string base_string)
	{
		string text = "";
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					this.infos[i].element.name,
					": ",
					GameUtil.GetFormattedMass(this.infos[i].amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")
				});
			}
		}
		return base_string.Replace("{Liquids}", text);
	}

	// Token: 0x06004835 RID: 18485 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public static bool IsLiquidAccessible(Element element)
	{
		return true;
	}

	// Token: 0x06004836 RID: 18486 RVA: 0x000D33F8 File Offset: 0x000D15F8
	public override float GetPercentComplete()
	{
		if (this.session != null)
		{
			return this.session.GetPercentComplete();
		}
		return 0f;
	}

	// Token: 0x06004837 RID: 18487 RVA: 0x002636AC File Offset: 0x002618AC
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		Pickupable.PickupableStartWorkInfo pickupableStartWorkInfo = (Pickupable.PickupableStartWorkInfo)worker.GetStartWorkInfo();
		float amount = pickupableStartWorkInfo.amount;
		Element element = pickupableStartWorkInfo.originalPickupable.PrimaryElement.Element;
		this.session = new LiquidPumpingStation.WorkSession(Grid.PosToCell(this), element.id, pickupableStartWorkInfo.originalPickupable.GetComponent<SubstanceChunk>(), amount, base.gameObject);
		this.meter.SetPositionPercent(0f);
		this.meter.SetSymbolTint(new KAnimHashedString("meter_target"), element.substance.colour);
	}

	// Token: 0x06004838 RID: 18488 RVA: 0x00263740 File Offset: 0x00261940
	protected override void OnStopWork(WorkerBase worker)
	{
		base.OnStopWork(worker);
		if (this.session != null)
		{
			Storage component = worker.GetComponent<Storage>();
			float consumedAmount = this.session.GetConsumedAmount();
			if (consumedAmount > 0f)
			{
				SubstanceChunk source = this.session.GetSource();
				SimUtil.DiseaseInfo diseaseInfo = (this.session != null) ? this.session.GetDiseaseInfo() : SimUtil.DiseaseInfo.Invalid;
				PrimaryElement component2 = source.GetComponent<PrimaryElement>();
				Pickupable component3 = LiquidSourceManager.Instance.CreateChunk(component2.Element, consumedAmount, this.session.GetTemperature(), diseaseInfo.idx, diseaseInfo.count, base.transform.GetPosition()).GetComponent<Pickupable>();
				component3.TotalAmount = consumedAmount;
				component3.Trigger(1335436905, source.GetComponent<Pickupable>());
				worker.SetWorkCompleteData(component3);
				this.Sim200ms(0f);
				if (component3 != null)
				{
					component.Store(component3.gameObject, false, false, true, false);
				}
			}
			this.session.Cleanup();
			this.session = null;
		}
		base.GetComponent<KAnimControllerBase>().Play("on", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06004839 RID: 18489 RVA: 0x00263864 File Offset: 0x00261A64
	private void OnReservationsChanged(Pickupable _ignore, bool _ignore2, Pickupable.Reservation _ignore3)
	{
		bool forceUnfetchable = false;
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null && this.infos[i].source.GetComponent<Pickupable>().ReservedAmount > 0f)
			{
				forceUnfetchable = true;
				break;
			}
		}
		for (int j = 0; j < this.infoCount; j++)
		{
			if (this.infos[j].source != null)
			{
				FetchableMonitor.Instance smi = this.infos[j].source.GetSMI<FetchableMonitor.Instance>();
				if (smi != null)
				{
					smi.SetForceUnfetchable(forceUnfetchable);
				}
			}
		}
	}

	// Token: 0x0600483A RID: 18490 RVA: 0x000D3413 File Offset: 0x000D1613
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (this.session != null)
		{
			this.meter.SetPositionPercent(this.session.GetPercentComplete());
			if (this.session.GetLastTickAmount() <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600483B RID: 18491 RVA: 0x00263910 File Offset: 0x00261B10
	protected override void OnCleanUp()
	{
		this.UnregisterListenersToCellChanges();
		base.OnCleanUp();
		if (this.session != null)
		{
			this.session.Cleanup();
			this.session = null;
		}
		for (int i = 0; i < this.infoCount; i++)
		{
			if (this.infos[i].source != null)
			{
				this.infos[i].source.DeleteObject();
			}
		}
	}

	// Token: 0x040032AA RID: 12970
	private static readonly CellOffset[] liquidOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(0, -1),
		new CellOffset(1, -1),
		new CellOffset(0, -2),
		new CellOffset(1, -2),
		new CellOffset(0, -3),
		new CellOffset(1, -3),
		new CellOffset(0, -4),
		new CellOffset(1, -4)
	};

	// Token: 0x040032AB RID: 12971
	private LiquidPumpingStation.LiquidInfo[] infos;

	// Token: 0x040032AC RID: 12972
	private int infoCount;

	// Token: 0x040032AD RID: 12973
	private int depthAvailable = -1;

	// Token: 0x040032AE RID: 12974
	private HandleVector<int>.Handle partitionerEntry_buildings;

	// Token: 0x040032AF RID: 12975
	private HandleVector<int>.Handle partitionerEntry_solids;

	// Token: 0x040032B0 RID: 12976
	private LiquidPumpingStation.WorkSession session;

	// Token: 0x040032B1 RID: 12977
	private MeterController meter;

	// Token: 0x02000E6A RID: 3690
	private class WorkSession
	{
		// Token: 0x0600483E RID: 18494 RVA: 0x00263A30 File Offset: 0x00261C30
		public WorkSession(int cell, SimHashes element, SubstanceChunk source, float amount_to_pickup, GameObject pump)
		{
			this.cell = cell;
			this.element = element;
			this.source = source;
			this.amountToPickup = amount_to_pickup;
			this.temperature = ElementLoader.FindElementByHash(element).defaultValues.temperature;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
			this.amountPerTick = 40f;
			this.pump = pump;
			this.lastTickAmount = this.amountPerTick;
			this.ConsumeMass();
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x000D3457 File Offset: 0x000D1657
		private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
		{
			((LiquidPumpingStation.WorkSession)data).OnSimConsume(mass_cb_info);
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x00263AA8 File Offset: 0x00261CA8
		private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
		{
			if (this.consumedAmount == 0f)
			{
				this.temperature = mass_cb_info.temperature;
			}
			else
			{
				this.temperature = GameUtil.GetFinalTemperature(this.temperature, this.consumedAmount, mass_cb_info.temperature, mass_cb_info.mass);
			}
			this.consumedAmount += mass_cb_info.mass;
			this.lastTickAmount = mass_cb_info.mass;
			this.diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(this.diseaseInfo.idx, this.diseaseInfo.count, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
			if (this.consumedAmount >= this.amountToPickup)
			{
				this.amountPerTick = 0f;
				this.lastTickAmount = 0f;
			}
			this.ConsumeMass();
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x00263B6C File Offset: 0x00261D6C
		private void ConsumeMass()
		{
			if (this.amountPerTick > 0f)
			{
				float num = Mathf.Min(this.amountPerTick, this.amountToPickup - this.consumedAmount);
				num = Mathf.Max(num, 1f);
				HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(LiquidPumpingStation.WorkSession.OnSimConsumeCallback), this, "LiquidPumpingStation");
				int depthAvailable = PumpingStationGuide.GetDepthAvailable(this.cell, this.pump);
				SimMessages.ConsumeMass(Grid.OffsetCell(this.cell, new CellOffset(0, -depthAvailable)), this.element, num, (byte)(depthAvailable + 1), handle.index);
			}
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x000D3465 File Offset: 0x000D1665
		public float GetPercentComplete()
		{
			return this.consumedAmount / this.amountToPickup;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x000D3474 File Offset: 0x000D1674
		public float GetLastTickAmount()
		{
			return this.lastTickAmount;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x000D347C File Offset: 0x000D167C
		public SimUtil.DiseaseInfo GetDiseaseInfo()
		{
			return this.diseaseInfo;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x000D3484 File Offset: 0x000D1684
		public SubstanceChunk GetSource()
		{
			return this.source;
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x000D348C File Offset: 0x000D168C
		public float GetConsumedAmount()
		{
			return this.consumedAmount;
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x000D3494 File Offset: 0x000D1694
		public float GetTemperature()
		{
			if (this.temperature <= 0f)
			{
				global::Debug.LogWarning("TODO(YOG): Fix bad temperature in liquid pumping station.");
				return ElementLoader.FindElementByHash(this.element).defaultValues.temperature;
			}
			return this.temperature;
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x000D34C9 File Offset: 0x000D16C9
		public void Cleanup()
		{
			this.amountPerTick = 0f;
			this.diseaseInfo = SimUtil.DiseaseInfo.Invalid;
		}

		// Token: 0x040032B2 RID: 12978
		private int cell;

		// Token: 0x040032B3 RID: 12979
		private float amountToPickup;

		// Token: 0x040032B4 RID: 12980
		private float consumedAmount;

		// Token: 0x040032B5 RID: 12981
		private float temperature;

		// Token: 0x040032B6 RID: 12982
		private float amountPerTick;

		// Token: 0x040032B7 RID: 12983
		private SimHashes element;

		// Token: 0x040032B8 RID: 12984
		private float lastTickAmount;

		// Token: 0x040032B9 RID: 12985
		private SubstanceChunk source;

		// Token: 0x040032BA RID: 12986
		private SimUtil.DiseaseInfo diseaseInfo;

		// Token: 0x040032BB RID: 12987
		private GameObject pump;
	}

	// Token: 0x02000E6B RID: 3691
	private struct LiquidInfo
	{
		// Token: 0x040032BC RID: 12988
		public float amount;

		// Token: 0x040032BD RID: 12989
		public Element element;

		// Token: 0x040032BE RID: 12990
		public SubstanceChunk source;
	}
}
