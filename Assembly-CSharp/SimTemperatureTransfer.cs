using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020018CB RID: 6347
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/SimTemperatureTransfer")]
public class SimTemperatureTransfer : KMonoBehaviour
{
	// Token: 0x17000854 RID: 2132
	// (get) Token: 0x06008330 RID: 33584 RVA: 0x000FAC61 File Offset: 0x000F8E61
	// (set) Token: 0x06008331 RID: 33585 RVA: 0x000FAC69 File Offset: 0x000F8E69
	public float SurfaceArea
	{
		get
		{
			return this.surfaceArea;
		}
		set
		{
			this.surfaceArea = value;
		}
	}

	// Token: 0x17000855 RID: 2133
	// (get) Token: 0x06008332 RID: 33586 RVA: 0x000FAC72 File Offset: 0x000F8E72
	// (set) Token: 0x06008333 RID: 33587 RVA: 0x000FAC7A File Offset: 0x000F8E7A
	public float Thickness
	{
		get
		{
			return this.thickness;
		}
		set
		{
			this.thickness = value;
		}
	}

	// Token: 0x17000856 RID: 2134
	// (get) Token: 0x06008334 RID: 33588 RVA: 0x000FAC83 File Offset: 0x000F8E83
	// (set) Token: 0x06008335 RID: 33589 RVA: 0x000FAC8B File Offset: 0x000F8E8B
	public float GroundTransferScale
	{
		get
		{
			return this.groundTransferScale;
		}
		set
		{
			this.groundTransferScale = value;
		}
	}

	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06008336 RID: 33590 RVA: 0x000FAC94 File Offset: 0x000F8E94
	public int SimHandle
	{
		get
		{
			return this.simHandle;
		}
	}

	// Token: 0x06008337 RID: 33591 RVA: 0x000FAC9C File Offset: 0x000F8E9C
	public static void ClearInstanceMap()
	{
		SimTemperatureTransfer.handleInstanceMap.Clear();
	}

	// Token: 0x06008338 RID: 33592 RVA: 0x0034DFCC File Offset: 0x0034C1CC
	public static void DoOreMeltTransition(int sim_handle)
	{
		SimTemperatureTransfer simTemperatureTransfer = null;
		if (!SimTemperatureTransfer.handleInstanceMap.TryGetValue(sim_handle, out simTemperatureTransfer))
		{
			return;
		}
		if (simTemperatureTransfer == null)
		{
			return;
		}
		if (simTemperatureTransfer.HasTag(GameTags.Sealed))
		{
			return;
		}
		PrimaryElement primaryElement = simTemperatureTransfer.pe;
		Element element = primaryElement.Element;
		bool flag = primaryElement.Temperature >= element.highTemp;
		bool flag2 = primaryElement.Temperature <= element.lowTemp;
		if (!flag && !flag2)
		{
			return;
		}
		if (flag && element.highTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (flag2 && element.lowTempTransitionTarget == SimHashes.Unobtanium)
		{
			return;
		}
		if (primaryElement.Mass > 0f)
		{
			int gameCell = Grid.PosToCell(simTemperatureTransfer.transform.GetPosition());
			float num = primaryElement.Mass;
			int num2 = primaryElement.DiseaseCount;
			SimHashes new_element = flag ? element.highTempTransitionTarget : element.lowTempTransitionTarget;
			SimHashes simHashes = flag ? element.highTempTransitionOreID : element.lowTempTransitionOreID;
			float num3 = flag ? element.highTempTransitionOreMassConversion : element.lowTempTransitionOreMassConversion;
			if (simHashes != (SimHashes)0)
			{
				float num4 = num * num3;
				int num5 = (int)((float)num2 * num3);
				if (num4 > 0.001f)
				{
					num -= num4;
					num2 -= num5;
					Element element2 = ElementLoader.FindElementByHash(simHashes);
					if (element2.IsSolid)
					{
						GameObject obj = element2.substance.SpawnResource(simTemperatureTransfer.transform.GetPosition(), num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, false, true);
						element2.substance.ActivateSubstanceGameObject(obj, primaryElement.DiseaseIdx, num5);
					}
					else
					{
						SimMessages.AddRemoveSubstance(gameCell, element2.id, CellEventLogger.Instance.OreMelted, num4, primaryElement.Temperature, primaryElement.DiseaseIdx, num5, true, -1);
					}
				}
			}
			SimMessages.AddRemoveSubstance(gameCell, new_element, CellEventLogger.Instance.OreMelted, num, primaryElement.Temperature, primaryElement.DiseaseIdx, num2, true, -1);
		}
		simTemperatureTransfer.OnCleanUp();
		Util.KDestroyGameObject(simTemperatureTransfer.gameObject);
	}

	// Token: 0x06008339 RID: 33593 RVA: 0x0034E1B4 File Offset: 0x0034C3B4
	protected override void OnPrefabInit()
	{
		this.pe.sttOptimizationHook = this;
		this.pe.getTemperatureCallback = new PrimaryElement.GetTemperatureCallback(SimTemperatureTransfer.OnGetTemperature);
		this.pe.setTemperatureCallback = new PrimaryElement.SetTemperatureCallback(SimTemperatureTransfer.OnSetTemperature);
		PrimaryElement primaryElement = this.pe;
		primaryElement.onDataChanged = (Action<PrimaryElement>)Delegate.Combine(primaryElement.onDataChanged, new Action<PrimaryElement>(this.OnDataChanged));
	}

	// Token: 0x0600833A RID: 33594 RVA: 0x0034E224 File Offset: 0x0034C424
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Element element = this.pe.Element;
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "SimTemperatureTransfer.OnSpawn");
		if (!Grid.IsValidCell(Grid.PosToCell(this)) || this.pe.Element.HasTag(GameTags.Special) || element.specificHeatCapacity == 0f)
		{
			base.enabled = false;
		}
		this.SimRegister();
	}

	// Token: 0x0600833B RID: 33595 RVA: 0x000FACA8 File Offset: 0x000F8EA8
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimTemperatureTransfer.OnSetTemperature(this.pe, this.pe.Temperature);
		}
	}

	// Token: 0x0600833C RID: 33596 RVA: 0x0034E2A4 File Offset: 0x0034C4A4
	protected override void OnCmpDisable()
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float temperature = this.pe.Temperature;
			this.pe.InternalTemperature = this.pe.Temperature;
			SimMessages.SetElementChunkData(this.simHandle, temperature, 0f);
		}
		base.OnCmpDisable();
	}

	// Token: 0x0600833D RID: 33597 RVA: 0x0034E2F8 File Offset: 0x0034C4F8
	private void OnCellChanged()
	{
		int cell = Grid.PosToCell(this);
		if (!Grid.IsValidCell(cell))
		{
			base.enabled = false;
			return;
		}
		this.SimRegister();
		if (Sim.IsValidHandle(this.simHandle))
		{
			SimMessages.MoveElementChunk(this.simHandle, cell);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x0600833E RID: 33598 RVA: 0x000FACD9 File Offset: 0x000F8ED9
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
		this.SimUnregister();
		base.OnForcedCleanUp();
	}

	// Token: 0x0600833F RID: 33599 RVA: 0x0034E344 File Offset: 0x0034C544
	private unsafe static float OnGetTemperature(PrimaryElement primary_element)
	{
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		float result;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			result = Game.Instance.simData.elementChunks[handleIndex].temperature;
			sttOptimizationHook.deltaKJ = Game.Instance.simData.elementChunks[handleIndex].deltaKJ;
		}
		else
		{
			result = primary_element.InternalTemperature;
		}
		return result;
	}

	// Token: 0x06008340 RID: 33600 RVA: 0x0034E3C0 File Offset: 0x0034C5C0
	private unsafe static void OnSetTemperature(PrimaryElement primary_element, float temperature)
	{
		if (temperature <= 0f)
		{
			KCrashReporter.Assert(false, "STT.OnSetTemperature - Tried to set <= 0 degree temperature", null);
			temperature = 293f;
		}
		primary_element.InternalTemperature = temperature;
		SimTemperatureTransfer sttOptimizationHook = primary_element.sttOptimizationHook;
		if (Sim.IsValidHandle(sttOptimizationHook.simHandle))
		{
			float mass = primary_element.Mass;
			float heat_capacity = (mass >= 0.01f) ? (mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(sttOptimizationHook.simHandle, temperature, heat_capacity);
			int handleIndex = Sim.GetHandleIndex(sttOptimizationHook.simHandle);
			Game.Instance.simData.elementChunks[handleIndex].temperature = temperature;
		}
	}

	// Token: 0x06008341 RID: 33601 RVA: 0x0034E460 File Offset: 0x0034C660
	private void OnDataChanged(PrimaryElement primary_element)
	{
		if (Sim.IsValidHandle(this.simHandle))
		{
			float heat_capacity = (primary_element.Mass >= 0.01f) ? (primary_element.Mass * primary_element.Element.specificHeatCapacity) : 0f;
			SimMessages.SetElementChunkData(this.simHandle, primary_element.Temperature, heat_capacity);
			return;
		}
		this.forceDataSyncOnRegister = true;
	}

	// Token: 0x06008342 RID: 33602 RVA: 0x0034E4BC File Offset: 0x0034C6BC
	protected void SimRegister()
	{
		if (base.isSpawned && this.simHandle == -1 && base.enabled && this.pe.Mass > 0f && !this.pe.Element.IsTemperatureInsulated)
		{
			int gameCell = Grid.PosToCell(base.transform.GetPosition());
			this.simHandle = -2;
			HandleVector<Game.ComplexCallbackInfo<int>>.Handle handle = Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(SimTemperatureTransfer.OnSimRegisteredCallback), this, "SimTemperatureTransfer.SimRegister");
			float num = this.pe.InternalTemperature;
			if (num <= 0f)
			{
				this.pe.InternalTemperature = 293f;
				num = 293f;
			}
			this.forceDataSyncOnRegister = false;
			SimMessages.AddElementChunk(gameCell, this.pe.ElementID, this.pe.Mass, num, this.surfaceArea, this.thickness, this.groundTransferScale, handle.index);
		}
	}

	// Token: 0x06008343 RID: 33603 RVA: 0x0034E5B8 File Offset: 0x0034C7B8
	protected unsafe void SimUnregister()
	{
		if (this.simHandle != -1 && !KMonoBehaviour.isLoadingScene)
		{
			if (Sim.IsValidHandle(this.simHandle))
			{
				int handleIndex = Sim.GetHandleIndex(this.simHandle);
				this.pe.InternalTemperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
				SimMessages.RemoveElementChunk(this.simHandle, -1);
				SimTemperatureTransfer.handleInstanceMap.Remove(this.simHandle);
			}
			this.simHandle = -1;
		}
	}

	// Token: 0x06008344 RID: 33604 RVA: 0x000FAD03 File Offset: 0x000F8F03
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((SimTemperatureTransfer)data).OnSimRegistered(handle);
	}

	// Token: 0x06008345 RID: 33605 RVA: 0x0034E63C File Offset: 0x0034C83C
	private unsafe void OnSimRegistered(int handle)
	{
		if (this != null && this.simHandle == -2)
		{
			this.simHandle = handle;
			int handleIndex = Sim.GetHandleIndex(handle);
			float temperature = Game.Instance.simData.elementChunks[handleIndex].temperature;
			float internalTemperature = this.pe.InternalTemperature;
			if (temperature <= 0f)
			{
				KCrashReporter.Assert(false, "Bad temperature", null);
			}
			SimTemperatureTransfer.handleInstanceMap[this.simHandle] = this;
			if (this.forceDataSyncOnRegister || Mathf.Abs(temperature - internalTemperature) > 0.1f)
			{
				float heat_capacity = (this.pe.Mass >= 0.01f) ? (this.pe.Mass * this.pe.Element.specificHeatCapacity) : 0f;
				SimMessages.SetElementChunkData(this.simHandle, internalTemperature, heat_capacity);
				SimMessages.MoveElementChunk(this.simHandle, Grid.PosToCell(this));
				Game.Instance.simData.elementChunks[handleIndex].temperature = internalTemperature;
			}
			if (this.onSimRegistered != null)
			{
				this.onSimRegistered(this);
			}
			if (!base.enabled)
			{
				this.OnCmpDisable();
				return;
			}
		}
		else
		{
			SimMessages.RemoveElementChunk(handle, -1);
		}
	}

	// Token: 0x040063F1 RID: 25585
	[MyCmpReq]
	public PrimaryElement pe;

	// Token: 0x040063F2 RID: 25586
	private const float SIM_FREEZE_SPAWN_ORE_PERCENT = 0.8f;

	// Token: 0x040063F3 RID: 25587
	public const float MIN_MASS_FOR_TEMPERATURE_TRANSFER = 0.01f;

	// Token: 0x040063F4 RID: 25588
	public float deltaKJ;

	// Token: 0x040063F5 RID: 25589
	public Action<SimTemperatureTransfer> onSimRegistered;

	// Token: 0x040063F6 RID: 25590
	protected int simHandle = -1;

	// Token: 0x040063F7 RID: 25591
	protected bool forceDataSyncOnRegister;

	// Token: 0x040063F8 RID: 25592
	[SerializeField]
	protected float surfaceArea = 10f;

	// Token: 0x040063F9 RID: 25593
	[SerializeField]
	protected float thickness = 0.01f;

	// Token: 0x040063FA RID: 25594
	[SerializeField]
	protected float groundTransferScale = 0.0625f;

	// Token: 0x040063FB RID: 25595
	private static Dictionary<int, SimTemperatureTransfer> handleInstanceMap = new Dictionary<int, SimTemperatureTransfer>();
}
