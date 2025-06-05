using System;
using System.Runtime.InteropServices;

// Token: 0x02001141 RID: 4417
public class ConduitTemperatureManager
{
	// Token: 0x06005A36 RID: 23094 RVA: 0x000DF2A5 File Offset: 0x000DD4A5
	public ConduitTemperatureManager()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Initialize();
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x000DF2CA File Offset: 0x000DD4CA
	public void Shutdown()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Shutdown();
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x002A1B14 File Offset: 0x0029FD14
	public HandleVector<int>.Handle Allocate(ConduitType conduit_type, int conduit_idx, HandleVector<int>.Handle conduit_structure_temperature_handle, ref ConduitFlow.ConduitContents contents)
	{
		StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(conduit_structure_temperature_handle);
		Element element = payload.primaryElement.Element;
		BuildingDef def = payload.building.Def;
		float conduit_heat_capacity = def.MassForTemperatureModification * element.specificHeatCapacity;
		float conduit_thermal_conductivity = element.thermalConductivity * def.ThermalConductivity;
		int num = ConduitTemperatureManager.ConduitTemperatureManager_Add(contents.temperature, contents.mass, (int)contents.element, payload.simHandleCopy, conduit_heat_capacity, conduit_thermal_conductivity, def.ThermalConductivity < 1f);
		HandleVector<int>.Handle result = default(HandleVector<int>.Handle);
		result.index = num;
		int handleIndex = Sim.GetHandleIndex(num);
		if (handleIndex + 1 > this.temperatures.Length)
		{
			Array.Resize<float>(ref this.temperatures, (handleIndex + 1) * 2);
			Array.Resize<ConduitTemperatureManager.ConduitInfo>(ref this.conduitInfo, (handleIndex + 1) * 2);
		}
		this.temperatures[handleIndex] = contents.temperature;
		this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
		{
			type = conduit_type,
			idx = conduit_idx
		};
		return result;
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x002A1C18 File Offset: 0x0029FE18
	public void SetData(HandleVector<int>.Handle handle, ref ConduitFlow.ConduitContents contents)
	{
		if (!handle.IsValid())
		{
			return;
		}
		this.temperatures[Sim.GetHandleIndex(handle.index)] = contents.temperature;
		ConduitTemperatureManager.ConduitTemperatureManager_Set(handle.index, contents.temperature, contents.mass, (int)contents.element);
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x002A1C68 File Offset: 0x0029FE68
	public void Free(HandleVector<int>.Handle handle)
	{
		if (handle.IsValid())
		{
			int handleIndex = Sim.GetHandleIndex(handle.index);
			this.temperatures[handleIndex] = -1f;
			this.conduitInfo[handleIndex] = new ConduitTemperatureManager.ConduitInfo
			{
				type = ConduitType.None,
				idx = -1
			};
			ConduitTemperatureManager.ConduitTemperatureManager_Remove(handle.index);
		}
	}

	// Token: 0x06005A3B RID: 23099 RVA: 0x000DF2D1 File Offset: 0x000DD4D1
	public void Clear()
	{
		ConduitTemperatureManager.ConduitTemperatureManager_Clear();
	}

	// Token: 0x06005A3C RID: 23100 RVA: 0x002A1CCC File Offset: 0x0029FECC
	public unsafe void Sim200ms(float dt)
	{
		ConduitTemperatureManager.ConduitTemperatureUpdateData* ptr = (ConduitTemperatureManager.ConduitTemperatureUpdateData*)((void*)ConduitTemperatureManager.ConduitTemperatureManager_Update(dt, (IntPtr)((void*)Game.Instance.simData.buildingTemperatures)));
		int numEntries = ptr->numEntries;
		if (numEntries > 0)
		{
			Marshal.Copy((IntPtr)((void*)ptr->temperatures), this.temperatures, 0, numEntries);
		}
		for (int i = 0; i < ptr->numFrozenHandles; i++)
		{
			int handleIndex = Sim.GetHandleIndex(ptr->frozenHandles[i]);
			ConduitTemperatureManager.ConduitInfo conduitInfo = this.conduitInfo[handleIndex];
			Conduit.GetFlowManager(conduitInfo.type).FreezeConduitContents(conduitInfo.idx);
		}
		for (int j = 0; j < ptr->numMeltedHandles; j++)
		{
			int handleIndex2 = Sim.GetHandleIndex(ptr->meltedHandles[j]);
			ConduitTemperatureManager.ConduitInfo conduitInfo2 = this.conduitInfo[handleIndex2];
			Conduit.GetFlowManager(conduitInfo2.type).MeltConduitContents(conduitInfo2.idx);
		}
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x000DF2D8 File Offset: 0x000DD4D8
	public float GetTemperature(HandleVector<int>.Handle handle)
	{
		return this.temperatures[Sim.GetHandleIndex(handle.index)];
	}

	// Token: 0x06005A3E RID: 23102
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Initialize();

	// Token: 0x06005A3F RID: 23103
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Shutdown();

	// Token: 0x06005A40 RID: 23104
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Add(float contents_temperature, float contents_mass, int contents_element_hash, int conduit_structure_temperature_handle, float conduit_heat_capacity, float conduit_thermal_conductivity, bool conduit_insulated);

	// Token: 0x06005A41 RID: 23105
	[DllImport("SimDLL")]
	private static extern int ConduitTemperatureManager_Set(int handle, float contents_temperature, float contents_mass, int contents_element_hash);

	// Token: 0x06005A42 RID: 23106
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Remove(int handle);

	// Token: 0x06005A43 RID: 23107
	[DllImport("SimDLL")]
	private static extern IntPtr ConduitTemperatureManager_Update(float dt, IntPtr building_conductivity_data);

	// Token: 0x06005A44 RID: 23108
	[DllImport("SimDLL")]
	private static extern void ConduitTemperatureManager_Clear();

	// Token: 0x0400403E RID: 16446
	private float[] temperatures = new float[0];

	// Token: 0x0400403F RID: 16447
	private ConduitTemperatureManager.ConduitInfo[] conduitInfo = new ConduitTemperatureManager.ConduitInfo[0];

	// Token: 0x02001142 RID: 4418
	private struct ConduitInfo
	{
		// Token: 0x04004040 RID: 16448
		public ConduitType type;

		// Token: 0x04004041 RID: 16449
		public int idx;
	}

	// Token: 0x02001143 RID: 4419
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConduitTemperatureUpdateData
	{
		// Token: 0x04004042 RID: 16450
		public int numEntries;

		// Token: 0x04004043 RID: 16451
		public unsafe float* temperatures;

		// Token: 0x04004044 RID: 16452
		public int numFrozenHandles;

		// Token: 0x04004045 RID: 16453
		public unsafe int* frozenHandles;

		// Token: 0x04004046 RID: 16454
		public int numMeltedHandles;

		// Token: 0x04004047 RID: 16455
		public unsafe int* meltedHandles;
	}
}
