using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000DF5 RID: 3573
public class GeothermalVent : StateMachineComponent<GeothermalVent.StatesInstance>, ISim200ms, ISaveLoadable
{
	// Token: 0x060045C5 RID: 17861 RVA: 0x000D185F File Offset: 0x000CFA5F
	public bool IsQuestEntombed()
	{
		return this.progress == GeothermalVent.QuestProgress.Entombed;
	}

	// Token: 0x060045C6 RID: 17862 RVA: 0x0025AD20 File Offset: 0x00258F20
	public void SetQuestComplete()
	{
		this.progress = GeothermalVent.QuestProgress.Complete;
		this.connectedToggler.showButton = true;
		base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.DESC;
		base.Trigger(-1514841199, null);
	}

	// Token: 0x060045C7 RID: 17863 RVA: 0x0025AD78 File Offset: 0x00258F78
	public static string GenerateName()
	{
		string text = "";
		for (int i = 0; i < 2; i++)
		{
			text += "0123456789"[UnityEngine.Random.Range(0, "0123456789".Length)].ToString();
		}
		return BUILDINGS.PREFABS.GEOTHERMALVENT.NAME_FMT.Replace("{ID}", text);
	}

	// Token: 0x060045C8 RID: 17864 RVA: 0x0025ADD0 File Offset: 0x00258FD0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.entombVulnerable.SetStatusItem(Db.Get().BuildingStatusItems.Entombed);
		base.GetComponent<PrimaryElement>().SetElement(SimHashes.Katairite, true);
		this.emitterInfo = default(GeothermalVent.EmitterInfo);
		this.emitterInfo.cell = Grid.PosToCell(base.gameObject) + Grid.WidthInCells * 3;
		this.emitterInfo.element = default(GeothermalVent.ElementInfo);
		this.emitterInfo.simHandle = -1;
		Components.GeothermalVents.Add(base.gameObject.GetMyWorldId(), this);
		if (this.progress == GeothermalVent.QuestProgress.Uninitialized)
		{
			if (Components.GeothermalVents.GetItems(base.gameObject.GetMyWorldId()).Count == 3)
			{
				this.progress = GeothermalVent.QuestProgress.Entombed;
			}
			else
			{
				this.progress = GeothermalVent.QuestProgress.Complete;
			}
		}
		if (this.progress == GeothermalVent.QuestProgress.Complete)
		{
			this.connectedToggler.showButton = true;
		}
		else
		{
			base.GetComponent<InfoDescription>().description = BUILDINGS.PREFABS.GEOTHERMALVENT.EFFECT + "\n\n" + BUILDINGS.PREFABS.GEOTHERMALVENT.BLOCKED_DESC;
			base.Trigger(-1514841199, null);
		}
		this.massMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.NoChange, Grid.SceneLayer.NoLayer, GeothermalVentConfig.BAROMETER_SYMBOLS);
		UserNameable component = base.GetComponent<UserNameable>();
		if (component.savedName == "" || component.savedName == BUILDINGS.PREFABS.GEOTHERMALVENT.NAME)
		{
			component.SetName(GeothermalVent.GenerateName());
		}
		this.SimRegister();
		base.smi.StartSM();
	}

	// Token: 0x060045C9 RID: 17865 RVA: 0x0025AF5C File Offset: 0x0025915C
	[OnDeserialized]
	internal void OnDeserializedMethod()
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
			Element element = ElementLoader.FindElementByHash(elementInfo.elementHash);
			if (element == null)
			{
				element = ElementLoader.FindElementByHash(SimHashes.Steam);
				elementInfo.elementHash = SimHashes.Steam;
				elementInfo.isSolid = false;
			}
			elementInfo.elementIdx = element.idx;
			this.availableMaterial[i] = elementInfo;
		}
		if (flag)
		{
			global::Debug.LogWarning("Invalid geothermal vent content in save was converted to steam on load.");
		}
	}

	// Token: 0x060045CA RID: 17866 RVA: 0x0025AFE4 File Offset: 0x002591E4
	protected void SimRegister()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimBlockedCallback), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnSimUnblockedCallback), true));
		SimMessages.AddElementEmitter(float.MaxValue, Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(GeothermalVent.OnSimRegisteredCallback), this, "GeothermalVentElementEmitter").index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x060045CB RID: 17867 RVA: 0x000D186A File Offset: 0x000CFA6A
	protected void OnSimBlockedCallback()
	{
		this.overpressure = true;
	}

	// Token: 0x060045CC RID: 17868 RVA: 0x000D1873 File Offset: 0x000CFA73
	protected void OnSimUnblockedCallback()
	{
		this.overpressure = false;
	}

	// Token: 0x060045CD RID: 17869 RVA: 0x000D187C File Offset: 0x000CFA7C
	protected static void OnSimRegisteredCallback(int handle, object data)
	{
		((GeothermalVent)data).OnSimRegisteredImpl(handle);
	}

	// Token: 0x060045CE RID: 17870 RVA: 0x000D188A File Offset: 0x000CFA8A
	protected void OnSimRegisteredImpl(int handle)
	{
		global::Debug.Assert(this.emitterInfo.simHandle == -1, "?! too many handles registered");
		this.emitterInfo.simHandle = handle;
	}

	// Token: 0x060045CF RID: 17871 RVA: 0x000D18B0 File Offset: 0x000CFAB0
	protected void SimUnregister()
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			SimMessages.RemoveElementEmitter(-1, this.emitterInfo.simHandle);
		}
		this.emitterInfo.simHandle = -1;
	}

	// Token: 0x060045D0 RID: 17872 RVA: 0x000D18E1 File Offset: 0x000CFAE1
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		Components.GeothermalVents.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x060045D1 RID: 17873 RVA: 0x0025B088 File Offset: 0x00259288
	protected void OnMassEmitted(ushort element, float mass)
	{
		bool flag = false;
		for (int i = 0; i < this.availableMaterial.Count; i++)
		{
			if (this.availableMaterial[i].elementIdx == element)
			{
				GeothermalVent.ElementInfo elementInfo = this.availableMaterial[i];
				elementInfo.mass -= mass;
				flag |= (elementInfo.mass <= 0f);
				this.availableMaterial[i] = elementInfo;
				break;
			}
		}
		if (flag)
		{
			this.RecomputeEmissions();
		}
	}

	// Token: 0x060045D2 RID: 17874 RVA: 0x0025B108 File Offset: 0x00259308
	public void SpawnKeepsake()
	{
		GameObject keepsakePrefab = Assets.GetPrefab("keepsake_geothermalplant");
		if (keepsakePrefab != null)
		{
			base.GetComponent<KBatchedAnimController>().Play("pooped", KAnim.PlayMode.Once, 1f, 0f);
			GameScheduler.Instance.Schedule("UncorkPoopAnim", 1.5f, delegate(object data)
			{
				this.GetComponent<KBatchedAnimController>().Play("uncork", KAnim.PlayMode.Once, 1f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("UncorkPoopFX", 2f, delegate(object data)
			{
				Game.Instance.SpawnFX(SpawnFXHashes.MissileExplosion, this.transform.GetPosition() + Vector3.up * 3f, 0f);
			}, null, null);
			GameScheduler.Instance.Schedule("SpawnGeothermalKeepsake", 3.75f, delegate(object data)
			{
				Vector3 position = this.transform.GetPosition();
				position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront);
				GameObject gameObject = Util.KInstantiate(keepsakePrefab, position);
				gameObject.SetActive(true);
				new UpgradeFX.Instance(gameObject.GetComponent<KMonoBehaviour>(), new Vector3(0f, -0.5f, -0.1f)).StartSM();
			}, null, null);
		}
	}

	// Token: 0x060045D3 RID: 17875 RVA: 0x000D191F File Offset: 0x000CFB1F
	public bool IsOverPressure()
	{
		return this.overpressure;
	}

	// Token: 0x060045D4 RID: 17876 RVA: 0x0025B1D4 File Offset: 0x002593D4
	protected void RecomputeEmissions()
	{
		this.availableMaterial.Sort();
		while (this.availableMaterial.Count > 0 && this.availableMaterial[this.availableMaterial.Count - 1].mass <= 0f)
		{
			this.availableMaterial.RemoveAt(this.availableMaterial.Count - 1);
		}
		int num = 0;
		using (List<GeothermalVent.ElementInfo>.Enumerator enumerator = this.availableMaterial.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.isSolid)
				{
					num++;
				}
			}
		}
		if (num > 0)
		{
			int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
			while (this.availableMaterial[num2].isSolid)
			{
				num2 = (num2 + 1) % this.availableMaterial.Count;
			}
			this.emitterInfo.element = this.availableMaterial[num2];
			this.emitterInfo.element.diseaseCount = (int)((float)this.availableMaterial[num2].diseaseCount * this.emitterInfo.element.mass / this.availableMaterial[num2].mass);
		}
		else
		{
			this.emitterInfo.element.elementIdx = 0;
			this.emitterInfo.element.mass = 0f;
		}
		this.emitterInfo.dirty = true;
	}

	// Token: 0x060045D5 RID: 17877 RVA: 0x000D1927 File Offset: 0x000CFB27
	public void addMaterial(GeothermalVent.ElementInfo info)
	{
		this.availableMaterial.Add(info);
		this.recentMass = this.MaterialAvailable();
	}

	// Token: 0x060045D6 RID: 17878 RVA: 0x0025B354 File Offset: 0x00259554
	public bool HasMaterial()
	{
		bool flag = this.availableMaterial.Count != 0;
		if (flag != this.logicPorts.GetOutputValue("GEOTHERMAL_VENT_STATUS_PORT") > 0)
		{
			this.logicPorts.SendSignal("GEOTHERMAL_VENT_STATUS_PORT", flag ? 1 : 0);
		}
		return flag;
	}

	// Token: 0x060045D7 RID: 17879 RVA: 0x0025B3A8 File Offset: 0x002595A8
	public float MaterialAvailable()
	{
		float num = 0f;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			num += elementInfo.mass;
		}
		return num;
	}

	// Token: 0x060045D8 RID: 17880 RVA: 0x000D1941 File Offset: 0x000CFB41
	public bool IsEntombed()
	{
		return this.entombVulnerable.GetEntombed;
	}

	// Token: 0x060045D9 RID: 17881 RVA: 0x000D194E File Offset: 0x000CFB4E
	public bool CanVent()
	{
		return !this.HasMaterial() && !this.IsEntombed();
	}

	// Token: 0x060045DA RID: 17882 RVA: 0x000D1963 File Offset: 0x000CFB63
	public bool IsVentConnected()
	{
		return !(this.connectedToggler == null) && this.connectedToggler.IsConnected;
	}

	// Token: 0x060045DB RID: 17883 RVA: 0x0025B404 File Offset: 0x00259604
	public void EmitSolidChunk()
	{
		int num = 0;
		foreach (GeothermalVent.ElementInfo elementInfo in this.availableMaterial)
		{
			if (elementInfo.isSolid && elementInfo.mass > 0f)
			{
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		int num2 = UnityEngine.Random.Range(0, this.availableMaterial.Count);
		while (!this.availableMaterial[num2].isSolid)
		{
			num2 = (num2 + 1) % this.availableMaterial.Count;
		}
		GeothermalVent.ElementInfo elementInfo2 = this.availableMaterial[num2];
		if (ElementLoader.elements[(int)this.availableMaterial[num2].elementIdx] == null)
		{
			return;
		}
		bool flag = UnityEngine.Random.value >= 0.5f;
		float f = GeothermalVentConfig.INITIAL_DEBRIS_ANGLE.Get() * 3.1415927f / 180f;
		Vector2 normalized = new Vector2(-Mathf.Cos(f), Mathf.Sin(f));
		if (flag)
		{
			normalized.x = -normalized.x;
		}
		normalized = normalized.normalized;
		normalized * GeothermalVentConfig.INITIAL_DEBRIS_VELOCIOTY.Get();
		float num3 = Math.Min(GeothermalVentConfig.DEBRIS_MASS_KG.Get(), elementInfo2.mass);
		if (elementInfo2.mass - num3 < GeothermalVentConfig.DEBRIS_MASS_KG.min)
		{
			num3 = elementInfo2.mass;
		}
		if (num3 < 0.01f)
		{
			elementInfo2.mass = 0f;
			this.availableMaterial[num2] = elementInfo2;
			return;
		}
		int num4 = (int)((float)elementInfo2.diseaseCount * num3 / elementInfo2.mass);
		Vector3 vector = Grid.CellToPos(this.emitterInfo.cell, CellAlignment.Top, Grid.SceneLayer.BuildingFront);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactDust, vector, 0f);
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(MiniCometConfig.ID), vector);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(ElementLoader.elements[(int)elementInfo2.elementIdx].id, true);
		component.Mass = num3;
		component.Temperature = elementInfo2.temperature;
		MiniComet component2 = gameObject.GetComponent<MiniComet>();
		component2.diseaseIdx = elementInfo2.diseaseIdx;
		component2.addDiseaseCount = num4;
		gameObject.SetActive(true);
		elementInfo2.diseaseCount -= num4;
		elementInfo2.mass -= num3;
		this.availableMaterial[num2] = elementInfo2;
	}

	// Token: 0x060045DC RID: 17884 RVA: 0x000D1980 File Offset: 0x000CFB80
	public void Sim200ms(float dt)
	{
		if (dt > 0f)
		{
			this.unsafeSim200ms(dt);
		}
	}

	// Token: 0x060045DD RID: 17885 RVA: 0x0025B66C File Offset: 0x0025986C
	private unsafe void unsafeSim200ms(float dt)
	{
		if (Sim.IsValidHandle(this.emitterInfo.simHandle))
		{
			if (this.emitterInfo.dirty)
			{
				SimMessages.ModifyElementEmitter(this.emitterInfo.simHandle, this.emitterInfo.cell, 1, ElementLoader.elements[(int)this.emitterInfo.element.elementIdx].id, 0.2f, Math.Min(3f, this.emitterInfo.element.mass), this.emitterInfo.element.temperature, 120f, this.emitterInfo.element.diseaseIdx, this.emitterInfo.element.diseaseCount);
				this.emitterInfo.dirty = false;
			}
			int handleIndex = Sim.GetHandleIndex(this.emitterInfo.simHandle);
			Sim.EmittedMassInfo emittedMassInfo = Game.Instance.simData.emittedMassEntries[handleIndex];
			if (emittedMassInfo.mass > 0f)
			{
				this.OnMassEmitted(emittedMassInfo.elemIdx, emittedMassInfo.mass);
			}
		}
		this.massMeter.SetPositionPercent(this.MaterialAvailable() / this.recentMass);
	}

	// Token: 0x060045DE RID: 17886 RVA: 0x000D1991 File Offset: 0x000CFB91
	protected static bool HasProblem(GeothermalVent.StatesInstance smi)
	{
		return smi.master.IsEntombed() || smi.master.IsOverPressure();
	}

	// Token: 0x04003094 RID: 12436
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003095 RID: 12437
	[MyCmpAdd]
	private ConnectionManager connectedToggler;

	// Token: 0x04003096 RID: 12438
	[MyCmpAdd]
	private EntombVulnerable entombVulnerable;

	// Token: 0x04003097 RID: 12439
	[MyCmpReq]
	private LogicPorts logicPorts;

	// Token: 0x04003098 RID: 12440
	[Serialize]
	private float recentMass = 1f;

	// Token: 0x04003099 RID: 12441
	private MeterController massMeter;

	// Token: 0x0400309A RID: 12442
	[Serialize]
	private GeothermalVent.QuestProgress progress;

	// Token: 0x0400309B RID: 12443
	protected GeothermalVent.EmitterInfo emitterInfo;

	// Token: 0x0400309C RID: 12444
	[Serialize]
	protected List<GeothermalVent.ElementInfo> availableMaterial = new List<GeothermalVent.ElementInfo>();

	// Token: 0x0400309D RID: 12445
	protected bool overpressure;

	// Token: 0x0400309E RID: 12446
	protected int debrisEmissionCell;

	// Token: 0x0400309F RID: 12447
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x040030A0 RID: 12448
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x02000DF6 RID: 3574
	private enum QuestProgress
	{
		// Token: 0x040030A2 RID: 12450
		Uninitialized,
		// Token: 0x040030A3 RID: 12451
		Entombed,
		// Token: 0x040030A4 RID: 12452
		Complete
	}

	// Token: 0x02000DF7 RID: 3575
	public struct ElementInfo : IComparable
	{
		// Token: 0x060045E0 RID: 17888 RVA: 0x000D19E1 File Offset: 0x000CFBE1
		public int CompareTo(object obj)
		{
			return -this.mass.CompareTo(((GeothermalVent.ElementInfo)obj).mass);
		}

		// Token: 0x040030A5 RID: 12453
		public bool isSolid;

		// Token: 0x040030A6 RID: 12454
		public SimHashes elementHash;

		// Token: 0x040030A7 RID: 12455
		public ushort elementIdx;

		// Token: 0x040030A8 RID: 12456
		public float mass;

		// Token: 0x040030A9 RID: 12457
		public float temperature;

		// Token: 0x040030AA RID: 12458
		public byte diseaseIdx;

		// Token: 0x040030AB RID: 12459
		public int diseaseCount;
	}

	// Token: 0x02000DF8 RID: 3576
	public struct EmitterInfo
	{
		// Token: 0x040030AC RID: 12460
		public int simHandle;

		// Token: 0x040030AD RID: 12461
		public int cell;

		// Token: 0x040030AE RID: 12462
		public GeothermalVent.ElementInfo element;

		// Token: 0x040030AF RID: 12463
		public bool dirty;
	}

	// Token: 0x02000DF9 RID: 3577
	public class States : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent>
	{
		// Token: 0x060045E1 RID: 17889 RVA: 0x0025B7A0 File Offset: 0x002599A0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EnterTransition(this.questEntombed, (GeothermalVent.StatesInstance smi) => smi.master.IsQuestEntombed()).EnterTransition(this.online, (GeothermalVent.StatesInstance smi) => !smi.master.IsQuestEntombed());
			this.questEntombed.PlayAnim("pooped").ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentQuestBlockage, (GeothermalVent.StatesInstance smi) => smi.master).Transition(this.online, (GeothermalVent.StatesInstance smi) => smi.master.progress == GeothermalVent.QuestProgress.Complete, UpdateRate.SIM_200ms);
			this.online.PlayAnim("on", KAnim.PlayMode.Once).defaultState = this.online.identify;
			this.online.identify.EnterTransition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem)).EnterTransition(this.online.active, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && smi.master.HasMaterial()).EnterTransition(this.online.ready, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && smi.master.IsVentConnected()).EnterTransition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi) && !smi.master.HasMaterial() && !smi.master.IsVentConnected());
			this.online.active.defaultState = this.online.active.preVent;
			this.online.active.preVent.PlayAnim("working_pre").OnAnimQueueComplete(this.online.active.loopVent);
			this.online.active.loopVent.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Exit(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.RecomputeEmissions();
			}).Transition(this.online.active.postVent, (GeothermalVent.StatesInstance smi) => !smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive.identify, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsVenting, (GeothermalVent.StatesInstance smi) => smi.master).Update(delegate(GeothermalVent.StatesInstance smi, float dt)
			{
				if (dt > 0f)
				{
					smi.master.RecomputeEmissions();
				}
			}, UpdateRate.SIM_4000ms, false).defaultState = this.online.active.loopVent.start;
			this.online.active.loopVent.start.PlayAnim("working1").OnAnimQueueComplete(this.online.active.loopVent.finish);
			this.online.active.loopVent.finish.Enter(delegate(GeothermalVent.StatesInstance smi)
			{
				smi.master.EmitSolidChunk();
			}).PlayAnim("working2").OnAnimQueueComplete(this.online.active.loopVent.start);
			this.online.active.postVent.QueueAnim("working_pst", false, null).OnAnimQueueComplete(this.online.ready);
			this.online.ready.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.disconnected, (GeothermalVent.StatesInstance smi) => !smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsReady, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.disconnected.PlayAnim("on", KAnim.PlayMode.Once).Transition(this.online.active, (GeothermalVent.StatesInstance smi) => smi.master.HasMaterial(), UpdateRate.SIM_200ms).Transition(this.online.inactive, new StateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.Transition.ConditionCallback(GeothermalVent.HasProblem), UpdateRate.SIM_200ms).Transition(this.online.ready, (GeothermalVent.StatesInstance smi) => smi.master.IsVentConnected(), UpdateRate.SIM_200ms).ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsDisconnected, (GeothermalVent.StatesInstance smi) => smi.master);
			this.online.inactive.PlayAnim("over_pressure", KAnim.PlayMode.Once).Transition(this.online.identify, (GeothermalVent.StatesInstance smi) => !GeothermalVent.HasProblem(smi), UpdateRate.SIM_200ms).defaultState = this.online.inactive.identify;
			this.online.inactive.identify.EnterTransition(this.online.inactive.entombed, (GeothermalVent.StatesInstance smi) => smi.master.IsEntombed()).EnterTransition(this.online.inactive.overpressure, (GeothermalVent.StatesInstance smi) => smi.master.IsOverPressure());
			this.online.inactive.entombed.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Entombed, null).Transition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsEntombed(), UpdateRate.SIM_200ms);
			this.online.inactive.overpressure.ToggleMainStatusItem(Db.Get().BuildingStatusItems.GeoVentsOverpressure, null).EnterTransition(this.online.inactive.identify, (GeothermalVent.StatesInstance smi) => !smi.master.IsOverPressure());
		}

		// Token: 0x040030B0 RID: 12464
		public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State questEntombed;

		// Token: 0x040030B1 RID: 12465
		public GeothermalVent.States.OnlineStates online;

		// Token: 0x02000DFA RID: 3578
		public class ActiveStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x040030B2 RID: 12466
			public GeothermalVent.States.ActiveStates.LoopStates loopVent;

			// Token: 0x040030B3 RID: 12467
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State preVent;

			// Token: 0x040030B4 RID: 12468
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State postVent;

			// Token: 0x02000DFB RID: 3579
			public class LoopStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
			{
				// Token: 0x040030B5 RID: 12469
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State start;

				// Token: 0x040030B6 RID: 12470
				public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State finish;
			}
		}

		// Token: 0x02000DFC RID: 3580
		public class ProblemStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x040030B7 RID: 12471
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x040030B8 RID: 12472
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State entombed;

			// Token: 0x040030B9 RID: 12473
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State overpressure;
		}

		// Token: 0x02000DFD RID: 3581
		public class OnlineStates : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State
		{
			// Token: 0x040030BA RID: 12474
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State identify;

			// Token: 0x040030BB RID: 12475
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State ready;

			// Token: 0x040030BC RID: 12476
			public GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.State disconnected;

			// Token: 0x040030BD RID: 12477
			public GeothermalVent.States.ActiveStates active;

			// Token: 0x040030BE RID: 12478
			public GeothermalVent.States.ProblemStates inactive;
		}
	}

	// Token: 0x02000DFF RID: 3583
	public class StatesInstance : GameStateMachine<GeothermalVent.States, GeothermalVent.StatesInstance, GeothermalVent, object>.GameInstance
	{
		// Token: 0x06004601 RID: 17921 RVA: 0x000D1B5B File Offset: 0x000CFD5B
		public StatesInstance(GeothermalVent smi) : base(smi)
		{
		}
	}
}
