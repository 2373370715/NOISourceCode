using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Database;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;

// Token: 0x02001BDE RID: 7134
public static class SimMessages
{
	// Token: 0x0600953C RID: 38204 RVA: 0x003A3E4C File Offset: 0x003A204C
	public unsafe static void AddElementConsumer(int gameCell, ElementConsumer.Configuration configuration, SimHashes element, byte radius, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.AddElementConsumerMessage* ptr = stackalloc SimMessages.AddElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementConsumerMessage))];
		ptr->cellIdx = gameCell;
		ptr->configuration = (byte)configuration;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(2024405073, sizeof(SimMessages.AddElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x0600953D RID: 38205 RVA: 0x003A3EB8 File Offset: 0x003A20B8
	public unsafe static void SetElementConsumerData(int sim_handle, int cell, float consumptionRate)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementConsumerDataMessage* ptr = stackalloc SimMessages.SetElementConsumerDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementConsumerDataMessage))];
		ptr->handle = sim_handle;
		ptr->cell = cell;
		ptr->consumptionRate = consumptionRate;
		Sim.SIM_HandleMessage(1575539738, sizeof(SimMessages.SetElementConsumerDataMessage), (byte*)ptr);
	}

	// Token: 0x0600953E RID: 38206 RVA: 0x003A3F04 File Offset: 0x003A2104
	public unsafe static void RemoveElementConsumer(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementConsumerMessage* ptr = stackalloc SimMessages.RemoveElementConsumerMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementConsumerMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(894417742, sizeof(SimMessages.RemoveElementConsumerMessage), (byte*)ptr);
	}

	// Token: 0x0600953F RID: 38207 RVA: 0x003A3F54 File Offset: 0x003A2154
	public unsafe static void AddElementEmitter(float max_pressure, int on_registered, int on_blocked = -1, int on_unblocked = -1)
	{
		SimMessages.AddElementEmitterMessage* ptr = stackalloc SimMessages.AddElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementEmitterMessage))];
		ptr->maxPressure = max_pressure;
		ptr->callbackIdx = on_registered;
		ptr->onBlockedCB = on_blocked;
		ptr->onUnblockedCB = on_unblocked;
		Sim.SIM_HandleMessage(-505471181, sizeof(SimMessages.AddElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009540 RID: 38208 RVA: 0x003A3F9C File Offset: 0x003A219C
	public unsafe static void ModifyElementEmitter(int sim_handle, int game_cell, int max_depth, SimHashes element, float emit_interval, float emit_mass, float emit_temperature, float max_pressure, byte disease_idx, int disease_count)
	{
		Debug.Assert(Grid.IsValidCell(game_cell));
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.ModifyElementEmitterMessage* ptr = stackalloc SimMessages.ModifyElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cellIdx = game_cell;
		ptr->emitInterval = emit_interval;
		ptr->emitMass = emit_mass;
		ptr->emitTemperature = emit_temperature;
		ptr->maxPressure = max_pressure;
		ptr->elementIdx = elementIndex;
		ptr->maxDepth = (byte)max_depth;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(403589164, sizeof(SimMessages.ModifyElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009541 RID: 38209 RVA: 0x003A4030 File Offset: 0x003A2230
	public unsafe static void RemoveElementEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementEmitterMessage* ptr = stackalloc SimMessages.RemoveElementEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-1524118282, sizeof(SimMessages.RemoveElementEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009542 RID: 38210 RVA: 0x003A4080 File Offset: 0x003A2280
	public unsafe static void AddRadiationEmitter(int on_registered, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		SimMessages.AddRadiationEmitterMessage* ptr = stackalloc SimMessages.AddRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddRadiationEmitterMessage))];
		ptr->callbackIdx = on_registered;
		ptr->cell = game_cell;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-1505895314, sizeof(SimMessages.AddRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009543 RID: 38211 RVA: 0x003A40F8 File Offset: 0x003A22F8
	public unsafe static void ModifyRadiationEmitter(int sim_handle, int game_cell, short emitRadiusX, short emitRadiusY, float emitRads, float emitRate, float emitSpeed, float emitDirection, float emitAngle, RadiationEmitter.RadiationEmitterType emitType)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ModifyRadiationEmitterMessage* ptr = stackalloc SimMessages.ModifyRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyRadiationEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->cell = game_cell;
		ptr->callbackIdx = -1;
		ptr->emitRadiusX = emitRadiusX;
		ptr->emitRadiusY = emitRadiusY;
		ptr->emitRads = emitRads;
		ptr->emitRate = emitRate;
		ptr->emitSpeed = emitSpeed;
		ptr->emitDirection = emitDirection;
		ptr->emitAngle = emitAngle;
		ptr->emitType = (int)emitType;
		Sim.SIM_HandleMessage(-503965465, sizeof(SimMessages.ModifyRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009544 RID: 38212 RVA: 0x003A4180 File Offset: 0x003A2380
	public unsafe static void RemoveRadiationEmitter(int cb_handle, int sim_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveRadiationEmitterMessage* ptr = stackalloc SimMessages.RemoveRadiationEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveRadiationEmitterMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-704259919, sizeof(SimMessages.RemoveRadiationEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009545 RID: 38213 RVA: 0x003A41D0 File Offset: 0x003A23D0
	public unsafe static void AddElementChunk(int gameCell, SimHashes element, float mass, float temperature, float surface_area, float thickness, float ground_transfer_scale, int cb_handle)
	{
		Debug.Assert(Grid.IsValidCell(gameCell));
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (mass * temperature > 0f)
		{
			ushort elementIndex = ElementLoader.GetElementIndex(element);
			SimMessages.AddElementChunkMessage* ptr = stackalloc SimMessages.AddElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddElementChunkMessage))];
			ptr->gameCell = gameCell;
			ptr->callbackIdx = cb_handle;
			ptr->mass = mass;
			ptr->temperature = temperature;
			ptr->surfaceArea = surface_area;
			ptr->thickness = thickness;
			ptr->groundTransferScale = ground_transfer_scale;
			ptr->elementIdx = elementIndex;
			Sim.SIM_HandleMessage(1445724082, sizeof(SimMessages.AddElementChunkMessage), (byte*)ptr);
		}
	}

	// Token: 0x06009546 RID: 38214 RVA: 0x003A425C File Offset: 0x003A245C
	public unsafe static void RemoveElementChunk(int sim_handle, int cb_handle)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.RemoveElementChunkMessage* ptr = stackalloc SimMessages.RemoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveElementChunkMessage))];
		ptr->callbackIdx = cb_handle;
		ptr->handle = sim_handle;
		Sim.SIM_HandleMessage(-912908555, sizeof(SimMessages.RemoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06009547 RID: 38215 RVA: 0x003A42AC File Offset: 0x003A24AC
	public unsafe static void SetElementChunkData(int sim_handle, float temperature, float heat_capacity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			return;
		}
		SimMessages.SetElementChunkDataMessage* ptr = stackalloc SimMessages.SetElementChunkDataMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetElementChunkDataMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		Sim.SIM_HandleMessage(-435115907, sizeof(SimMessages.SetElementChunkDataMessage), (byte*)ptr);
	}

	// Token: 0x06009548 RID: 38216 RVA: 0x003A42F8 File Offset: 0x003A24F8
	public unsafe static void MoveElementChunk(int sim_handle, int cell)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.MoveElementChunkMessage* ptr = stackalloc SimMessages.MoveElementChunkMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MoveElementChunkMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		Sim.SIM_HandleMessage(-374911358, sizeof(SimMessages.MoveElementChunkMessage), (byte*)ptr);
	}

	// Token: 0x06009549 RID: 38217 RVA: 0x003A4348 File Offset: 0x003A2548
	public unsafe static void ModifyElementChunkEnergy(int sim_handle, float delta_kj)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkEnergyMessage* ptr = stackalloc SimMessages.ModifyElementChunkEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkEnergyMessage))];
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		Sim.SIM_HandleMessage(1020555667, sizeof(SimMessages.ModifyElementChunkEnergyMessage), (byte*)ptr);
	}

	// Token: 0x0600954A RID: 38218 RVA: 0x003A4398 File Offset: 0x003A2598
	public unsafe static void ModifyElementChunkTemperatureAdjuster(int sim_handle, float temperature, float heat_capacity, float thermal_conductivity)
	{
		if (!Sim.IsValidHandle(sim_handle))
		{
			Debug.Assert(false, "Invalid handle");
			return;
		}
		SimMessages.ModifyElementChunkAdjusterMessage* ptr = stackalloc SimMessages.ModifyElementChunkAdjusterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyElementChunkAdjusterMessage))];
		ptr->handle = sim_handle;
		ptr->temperature = temperature;
		ptr->heatCapacity = heat_capacity;
		ptr->thermalConductivity = thermal_conductivity;
		Sim.SIM_HandleMessage(-1387601379, sizeof(SimMessages.ModifyElementChunkAdjusterMessage), (byte*)ptr);
	}

	// Token: 0x0600954B RID: 38219 RVA: 0x003A43F4 File Offset: 0x003A25F4
	public unsafe static void AddBuildingHeatExchange(Extents extents, float mass, float temperature, float thermal_conductivity, float operating_kw, ushort elem_idx, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(Grid.XYToCell(extents.x, extents.y)))
		{
			return;
		}
		int num = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		if (!Grid.IsValidCell(num))
		{
			Debug.LogErrorFormat("Invalid Cell [{0}] Extents [{1},{2}] [{3},{4}]", new object[]
			{
				num,
				extents.x,
				extents.y,
				extents.width,
				extents.height
			});
		}
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		SimMessages.AddBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callbackIdx;
		ptr->elemIdx = elem_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = float.MaxValue;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1739021608, sizeof(SimMessages.AddBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600954C RID: 38220 RVA: 0x003A4530 File Offset: 0x003A2730
	public unsafe static void ModifyBuildingHeatExchange(int sim_handle, Extents extents, float mass, float temperature, float thermal_conductivity, float overheat_temperature, float operating_kw, ushort element_idx)
	{
		int cell = Grid.XYToCell(extents.x, extents.y);
		Debug.Assert(Grid.IsValidCell(cell));
		if (!Grid.IsValidCell(cell))
		{
			return;
		}
		int cell2 = Grid.XYToCell(extents.x + extents.width, extents.y + extents.height);
		Debug.Assert(Grid.IsValidCell(cell2));
		if (!Grid.IsValidCell(cell2))
		{
			return;
		}
		SimMessages.ModifyBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.ModifyBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingHeatExchangeMessage))];
		ptr->callbackIdx = sim_handle;
		ptr->elemIdx = element_idx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->thermalConductivity = thermal_conductivity;
		ptr->overheatTemperature = overheat_temperature;
		ptr->operatingKilowatts = operating_kw;
		ptr->minX = extents.x;
		ptr->minY = extents.y;
		ptr->maxX = extents.x + extents.width;
		ptr->maxY = extents.y + extents.height;
		Sim.SIM_HandleMessage(1818001569, sizeof(SimMessages.ModifyBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600954D RID: 38221 RVA: 0x003A4624 File Offset: 0x003A2824
	public unsafe static void RemoveBuildingHeatExchange(int sim_handle, int callbackIdx = -1)
	{
		SimMessages.RemoveBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingHeatExchangeMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-456116629, sizeof(SimMessages.RemoveBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x0600954E RID: 38222 RVA: 0x003A4668 File Offset: 0x003A2868
	public unsafe static void ModifyBuildingEnergy(int sim_handle, float delta_kj, float min_temperature, float max_temperature)
	{
		SimMessages.ModifyBuildingEnergyMessage* ptr = stackalloc SimMessages.ModifyBuildingEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyBuildingEnergyMessage))];
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		ptr->handle = sim_handle;
		ptr->deltaKJ = delta_kj;
		ptr->minTemperature = min_temperature;
		ptr->maxTemperature = max_temperature;
		Sim.SIM_HandleMessage(-1348791658, sizeof(SimMessages.ModifyBuildingEnergyMessage), (byte*)ptr);
	}

	// Token: 0x0600954F RID: 38223 RVA: 0x003A46BC File Offset: 0x003A28BC
	public unsafe static void RegisterBuildingToBuildingHeatExchange(int structureTemperatureHandler, int callbackIdx = -1)
	{
		SimMessages.RegisterBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RegisterBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage))];
		ptr->structureTemperatureHandler = structureTemperatureHandler;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1338718217, sizeof(SimMessages.RegisterBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009550 RID: 38224 RVA: 0x003A46F8 File Offset: 0x003A28F8
	public unsafe static void AddBuildingToBuildingHeatExchange(int selfHandler, int buildingInContact, int cellsInContact)
	{
		SimMessages.AddBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.AddBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingInContactHandle = buildingInContact;
		ptr->cellsInContact = cellsInContact;
		Sim.SIM_HandleMessage(-1586724321, sizeof(SimMessages.AddBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009551 RID: 38225 RVA: 0x003A4738 File Offset: 0x003A2938
	public unsafe static void RemoveBuildingInContactFromBuildingToBuildingHeatExchange(int selfHandler, int buildingToRemove)
	{
		SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage))];
		ptr->selfHandler = selfHandler;
		ptr->buildingNoLongerInContactHandler = buildingToRemove;
		Sim.SIM_HandleMessage(-1993857213, sizeof(SimMessages.RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009552 RID: 38226 RVA: 0x003A4774 File Offset: 0x003A2974
	public unsafe static void RemoveBuildingToBuildingHeatExchange(int selfHandler, int callback = -1)
	{
		SimMessages.RemoveBuildingToBuildingHeatExchangeMessage* ptr = stackalloc SimMessages.RemoveBuildingToBuildingHeatExchangeMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage))];
		ptr->callbackIdx = callback;
		ptr->selfHandler = selfHandler;
		Sim.SIM_HandleMessage(697100730, sizeof(SimMessages.RemoveBuildingToBuildingHeatExchangeMessage), (byte*)ptr);
	}

	// Token: 0x06009553 RID: 38227 RVA: 0x003A47B0 File Offset: 0x003A29B0
	public unsafe static void AddDiseaseEmitter(int callbackIdx)
	{
		SimMessages.AddDiseaseEmitterMessage* ptr = stackalloc SimMessages.AddDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.AddDiseaseEmitterMessage))];
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(1486783027, sizeof(SimMessages.AddDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009554 RID: 38228 RVA: 0x003A47E4 File Offset: 0x003A29E4
	public unsafe static void ModifyDiseaseEmitter(int sim_handle, int cell, byte range, byte disease_idx, float emit_interval, int emit_count)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.ModifyDiseaseEmitterMessage* ptr = stackalloc SimMessages.ModifyDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->gameCell = cell;
		ptr->maxDepth = range;
		ptr->diseaseIdx = disease_idx;
		ptr->emitInterval = emit_interval;
		ptr->emitCount = emit_count;
		Sim.SIM_HandleMessage(-1899123924, sizeof(SimMessages.ModifyDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009555 RID: 38229 RVA: 0x003A4848 File Offset: 0x003A2A48
	public unsafe static void RemoveDiseaseEmitter(int cb_handle, int sim_handle)
	{
		Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveDiseaseEmitterMessage* ptr = stackalloc SimMessages.RemoveDiseaseEmitterMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RemoveDiseaseEmitterMessage))];
		ptr->handle = sim_handle;
		ptr->callbackIdx = cb_handle;
		Sim.SIM_HandleMessage(468135926, sizeof(SimMessages.RemoveDiseaseEmitterMessage), (byte*)ptr);
	}

	// Token: 0x06009556 RID: 38230 RVA: 0x003A488C File Offset: 0x003A2A8C
	public unsafe static void SetSavedOptionValue(SimMessages.SimSavedOptions option, int zero_or_one)
	{
		SimMessages.SetSavedOptionsMessage* ptr = stackalloc SimMessages.SetSavedOptionsMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetSavedOptionsMessage))];
		if (zero_or_one == 0)
		{
			SimMessages.SetSavedOptionsMessage* ptr2 = ptr;
			ptr2->clearBits = (ptr2->clearBits | (byte)option);
			ptr->setBits = 0;
		}
		else
		{
			ptr->clearBits = 0;
			SimMessages.SetSavedOptionsMessage* ptr3 = ptr;
			ptr3->setBits = (ptr3->setBits | (byte)option);
		}
		Sim.SIM_HandleMessage(1154135737, sizeof(SimMessages.SetSavedOptionsMessage), (byte*)ptr);
	}

	// Token: 0x06009557 RID: 38231 RVA: 0x003A48E4 File Offset: 0x003A2AE4
	private static void WriteKleiString(this BinaryWriter writer, string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		writer.Write(bytes.Length);
		if (bytes.Length != 0)
		{
			writer.Write(bytes);
		}
	}

	// Token: 0x06009558 RID: 38232 RVA: 0x003A4914 File Offset: 0x003A2B14
	public unsafe static void CreateSimElementsTable(List<Element> elements)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Element)) * elements.Count);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		Debug.Assert(elements.Count < 65535, "SimDLL internals assume there are fewer than 65535 elements");
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < elements.Count; i++)
		{
			Sim.Element element = new Sim.Element(elements[i], elements);
			element.Write(binaryWriter);
		}
		for (int j = 0; j < elements.Count; j++)
		{
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(elements[j].name));
		}
		byte[] buffer = memoryStream.GetBuffer();
		byte[] array;
		byte* msg;
		if ((array = buffer) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(1108437482, buffer.Length, msg);
		array = null;
	}

	// Token: 0x06009559 RID: 38233 RVA: 0x003A4A04 File Offset: 0x003A2C04
	public unsafe static void CreateDiseaseTable(Diseases diseases)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(diseases.Count);
		List<Element> elements = ElementLoader.elements;
		binaryWriter.Write(elements.Count);
		for (int i = 0; i < diseases.Count; i++)
		{
			Disease disease = diseases[i];
			binaryWriter.WriteKleiString(UI.StripLinkFormatting(disease.Name));
			binaryWriter.Write(disease.id.GetHashCode());
			binaryWriter.Write(disease.strength);
			disease.temperatureRange.Write(binaryWriter);
			disease.temperatureHalfLives.Write(binaryWriter);
			disease.pressureRange.Write(binaryWriter);
			disease.pressureHalfLives.Write(binaryWriter);
			binaryWriter.Write(disease.radiationKillRate);
			for (int j = 0; j < elements.Count; j++)
			{
				ElemGrowthInfo elemGrowthInfo = disease.elemGrowthInfo[j];
				elemGrowthInfo.Write(binaryWriter);
			}
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(825301935, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x0600955A RID: 38234 RVA: 0x003A4B40 File Offset: 0x003A2D40
	public unsafe static void DefineWorldOffsets(List<SimMessages.WorldOffsetData> worldOffsets)
	{
		MemoryStream memoryStream = new MemoryStream(1024);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(worldOffsets.Count);
		foreach (SimMessages.WorldOffsetData worldOffsetData in worldOffsets)
		{
			binaryWriter.Write(worldOffsetData.worldOffsetX);
			binaryWriter.Write(worldOffsetData.worldOffsetY);
			binaryWriter.Write(worldOffsetData.worldSizeX);
			binaryWriter.Write(worldOffsetData.worldSizeY);
		}
		byte[] array;
		byte* msg;
		if ((array = memoryStream.GetBuffer()) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		Sim.SIM_HandleMessage(-895846551, (int)memoryStream.Length, msg);
		array = null;
	}

	// Token: 0x0600955B RID: 38235 RVA: 0x003A4C10 File Offset: 0x003A2E10
	public static void SimDataInitializeFromCells(int width, int height, Sim.Cell[] cells, float[] bgTemp, Sim.DiseaseCell[] dc, bool headless)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(Sim.Cell)) * width * height + Marshal.SizeOf(typeof(float)) * width * height + Marshal.SizeOf(typeof(Sim.DiseaseCell)) * width * height);
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		bool value = Sim.IsRadiationEnabled();
		binaryWriter.Write(value);
		binaryWriter.Write(headless);
		int num = width * height;
		for (int i = 0; i < num; i++)
		{
			cells[i].Write(binaryWriter);
		}
		for (int j = 0; j < num; j++)
		{
			binaryWriter.Write(bgTemp[j]);
		}
		for (int k = 0; k < num; k++)
		{
			dc[k].Write(binaryWriter);
		}
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_InitializeFromCells, buffer.Length, buffer);
	}

	// Token: 0x0600955C RID: 38236 RVA: 0x003A4D1C File Offset: 0x003A2F1C
	public static void SimDataResizeGridAndInitializeVacuumCells(Vector2I grid_size, int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(grid_size.x);
		binaryWriter.Write(grid_size.y);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_ResizeAndInitializeVacuumCells, buffer.Length, buffer);
	}

	// Token: 0x0600955D RID: 38237 RVA: 0x003A4D9C File Offset: 0x003A2F9C
	public static void SimDataFreeCells(int width, int height, int x_offset, int y_offset)
	{
		MemoryStream memoryStream = new MemoryStream(Marshal.SizeOf(typeof(int)) + Marshal.SizeOf(typeof(int)));
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(width);
		binaryWriter.Write(height);
		binaryWriter.Write(x_offset);
		binaryWriter.Write(y_offset);
		byte[] buffer = memoryStream.GetBuffer();
		Sim.HandleMessage(SimMessageHashes.SimData_FreeCells, buffer.Length, buffer);
	}

	// Token: 0x0600955E RID: 38238 RVA: 0x003A4E04 File Offset: 0x003A3004
	public unsafe static void Dig(int gameCell, int callbackIdx = -1, bool skipEvent = false)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.DigMessage* ptr = stackalloc SimMessages.DigMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.DigMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->skipEvent = skipEvent;
		Sim.SIM_HandleMessage(833038498, sizeof(SimMessages.DigMessage), (byte*)ptr);
	}

	// Token: 0x0600955F RID: 38239 RVA: 0x003A4E50 File Offset: 0x003A3050
	public unsafe static void SetInsulation(int gameCell, float value)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		ptr->value = value;
		Sim.SIM_HandleMessage(-898773121, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x06009560 RID: 38240 RVA: 0x003A4E94 File Offset: 0x003A3094
	public unsafe static void SetStrength(int gameCell, int weight, float strengthMultiplier)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.SetCellFloatValueMessage* ptr = stackalloc SimMessages.SetCellFloatValueMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.SetCellFloatValueMessage))];
		ptr->cellIdx = gameCell;
		int num = (int)(strengthMultiplier * 4f) & 127;
		int num2 = (weight & 1) << 7 | num;
		ptr->value = (float)((byte)num2);
		Sim.SIM_HandleMessage(1593243982, sizeof(SimMessages.SetCellFloatValueMessage), (byte*)ptr);
	}

	// Token: 0x06009561 RID: 38241 RVA: 0x003A4EEC File Offset: 0x003A30EC
	public unsafe static void SetCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 1;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06009562 RID: 38242 RVA: 0x003A4F38 File Offset: 0x003A3138
	public unsafe static void ClearCellProperties(int gameCell, byte properties)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.CellPropertiesMessage* ptr = stackalloc SimMessages.CellPropertiesMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellPropertiesMessage))];
		ptr->cellIdx = gameCell;
		ptr->properties = properties;
		ptr->set = 0;
		Sim.SIM_HandleMessage(-469311643, sizeof(SimMessages.CellPropertiesMessage), (byte*)ptr);
	}

	// Token: 0x06009563 RID: 38243 RVA: 0x003A4F84 File Offset: 0x003A3184
	public unsafe static void ModifyCell(int gameCell, ushort elementIdx, float temperature, float mass, byte disease_idx, int disease_count, SimMessages.ReplaceType replace_type = SimMessages.ReplaceType.None, bool do_vertical_solid_displacement = false, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		if (element.maxMass == 0f && mass > element.maxMass)
		{
			Debug.LogWarningFormat("Invalid cell modification (mass greater than element maximum): Cell={0}, EIdx={1}, T={2}, M={3}, {4} max mass = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.maxMass
			});
			mass = element.maxMass;
		}
		if (temperature < 0f || temperature > 10000f)
		{
			Debug.LogWarningFormat("Invalid cell modification (temp out of bounds): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		if (temperature == 0f && mass > 0f)
		{
			Debug.LogWarningFormat("Invalid cell modification (zero temp with non-zero mass): Cell={0}, EIdx={1}, T={2}, M={3}, {4} default temp = {5}", new object[]
			{
				gameCell,
				elementIdx,
				temperature,
				mass,
				element.id,
				element.defaultValues.temperature
			});
			temperature = element.defaultValues.temperature;
		}
		SimMessages.ModifyCellMessage* ptr = stackalloc SimMessages.ModifyCellMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->temperature = temperature;
		ptr->mass = mass;
		ptr->elementIdx = elementIdx;
		ptr->replaceType = (byte)replace_type;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		ptr->addSubType = (do_vertical_solid_displacement ? 0 : 1);
		Sim.SIM_HandleMessage(-1252920804, sizeof(SimMessages.ModifyCellMessage), (byte*)ptr);
	}

	// Token: 0x06009564 RID: 38244 RVA: 0x003A5164 File Offset: 0x003A3364
	public unsafe static void ModifyDiseaseOnCell(int gameCell, byte disease_idx, int disease_delta)
	{
		SimMessages.CellDiseaseModification* ptr = stackalloc SimMessages.CellDiseaseModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellDiseaseModification))];
		ptr->cellIdx = gameCell;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_delta;
		Sim.SIM_HandleMessage(-1853671274, sizeof(SimMessages.CellDiseaseModification), (byte*)ptr);
	}

	// Token: 0x06009565 RID: 38245 RVA: 0x003A51A4 File Offset: 0x003A33A4
	public unsafe static void ModifyRadiationOnCell(int gameCell, float radiationDelta, int callbackIdx = -1)
	{
		SimMessages.CellRadiationModification* ptr = stackalloc SimMessages.CellRadiationModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellRadiationModification))];
		ptr->cellIdx = gameCell;
		ptr->radiationDelta = radiationDelta;
		ptr->callbackIdx = callbackIdx;
		Sim.SIM_HandleMessage(-1914877797, sizeof(SimMessages.CellRadiationModification), (byte*)ptr);
	}

	// Token: 0x06009566 RID: 38246 RVA: 0x003A51E4 File Offset: 0x003A33E4
	public unsafe static void ModifyRadiationParams(RadiationParams type, float value)
	{
		SimMessages.RadiationParamsModification* ptr = stackalloc SimMessages.RadiationParamsModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.RadiationParamsModification))];
		ptr->RadiationParamsType = (int)type;
		ptr->value = value;
		Sim.SIM_HandleMessage(377112707, sizeof(SimMessages.RadiationParamsModification), (byte*)ptr);
	}

	// Token: 0x06009567 RID: 38247 RVA: 0x00105E13 File Offset: 0x00104013
	public static ushort GetElementIndex(SimHashes element)
	{
		return ElementLoader.GetElementIndex(element);
	}

	// Token: 0x06009568 RID: 38248 RVA: 0x003A5220 File Offset: 0x003A3420
	public unsafe static void ConsumeMass(int gameCell, SimHashes element, float mass, byte radius, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		ushort elementIndex = ElementLoader.GetElementIndex(element);
		SimMessages.MassConsumptionMessage* ptr = stackalloc SimMessages.MassConsumptionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassConsumptionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->elementIdx = elementIndex;
		ptr->radius = radius;
		Sim.SIM_HandleMessage(1727657959, sizeof(SimMessages.MassConsumptionMessage), (byte*)ptr);
	}

	// Token: 0x06009569 RID: 38249 RVA: 0x003A5280 File Offset: 0x003A3480
	public unsafe static void EmitMass(int gameCell, ushort element_idx, float mass, float temperature, byte disease_idx, int disease_count, int callbackIdx = -1)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		SimMessages.MassEmissionMessage* ptr = stackalloc SimMessages.MassEmissionMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.MassEmissionMessage))];
		ptr->cellIdx = gameCell;
		ptr->callbackIdx = callbackIdx;
		ptr->mass = mass;
		ptr->temperature = temperature;
		ptr->elementIdx = element_idx;
		ptr->diseaseIdx = disease_idx;
		ptr->diseaseCount = disease_count;
		Sim.SIM_HandleMessage(797274363, sizeof(SimMessages.MassEmissionMessage), (byte*)ptr);
	}

	// Token: 0x0600956A RID: 38250 RVA: 0x003A52E8 File Offset: 0x003A34E8
	public unsafe static void ConsumeDisease(int game_cell, float percent_to_consume, int max_to_consume, int callback_idx)
	{
		if (!Grid.IsValidCell(game_cell))
		{
			return;
		}
		SimMessages.ConsumeDiseaseMessage* ptr = stackalloc SimMessages.ConsumeDiseaseMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ConsumeDiseaseMessage))];
		ptr->callbackIdx = callback_idx;
		ptr->gameCell = game_cell;
		ptr->percentToConsume = percent_to_consume;
		ptr->maxToConsume = max_to_consume;
		Sim.SIM_HandleMessage(-1019841536, sizeof(SimMessages.ConsumeDiseaseMessage), (byte*)ptr);
	}

	// Token: 0x0600956B RID: 38251 RVA: 0x003A5338 File Offset: 0x003A3538
	public static void AddRemoveSubstance(int gameCell, SimHashes new_element, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		SimMessages.AddRemoveSubstance(gameCell, elementIndex, ev, mass, temperature, disease_idx, disease_count, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x0600956C RID: 38252 RVA: 0x003A5360 File Offset: 0x003A3560
	public static void AddRemoveSubstance(int gameCell, ushort elementIdx, CellAddRemoveSubstanceEvent ev, float mass, float temperature, byte disease_idx, int disease_count, bool do_vertical_solid_displacement = true, int callbackIdx = -1)
	{
		if (elementIdx == 65535)
		{
			return;
		}
		Element element = ElementLoader.elements[(int)elementIdx];
		float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
		SimMessages.ModifyCell(gameCell, elementIdx, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, do_vertical_solid_displacement, callbackIdx);
	}

	// Token: 0x0600956D RID: 38253 RVA: 0x003A53B0 File Offset: 0x003A35B0
	public static void ReplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte diseaseIdx = 255, int diseaseCount = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, diseaseIdx, diseaseCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
		}
	}

	// Token: 0x0600956E RID: 38254 RVA: 0x003A5404 File Offset: 0x003A3604
	public static void ReplaceAndDisplaceElement(int gameCell, SimHashes new_element, CellElementEvent ev, float mass, float temperature = -1f, byte disease_idx = 255, int disease_count = 0, int callbackIdx = -1)
	{
		ushort elementIndex = SimMessages.GetElementIndex(new_element);
		if (elementIndex != 65535)
		{
			Element element = ElementLoader.elements[(int)elementIndex];
			float temperature2 = (temperature != -1f) ? temperature : element.defaultValues.temperature;
			SimMessages.ModifyCell(gameCell, elementIndex, temperature2, mass, disease_idx, disease_count, SimMessages.ReplaceType.ReplaceAndDisplace, false, callbackIdx);
		}
	}

	// Token: 0x0600956F RID: 38255 RVA: 0x003A5458 File Offset: 0x003A3658
	public unsafe static void ModifyEnergy(int gameCell, float kilojoules, float max_temperature, SimMessages.EnergySourceID id)
	{
		if (!Grid.IsValidCell(gameCell))
		{
			return;
		}
		if (max_temperature <= 0f)
		{
			Debug.LogError("invalid max temperature for cell energy modification");
			return;
		}
		SimMessages.ModifyCellEnergyMessage* ptr = stackalloc SimMessages.ModifyCellEnergyMessage[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.ModifyCellEnergyMessage))];
		ptr->cellIdx = gameCell;
		ptr->kilojoules = kilojoules;
		ptr->maxTemperature = max_temperature;
		ptr->id = (int)id;
		Sim.SIM_HandleMessage(818320644, sizeof(SimMessages.ModifyCellEnergyMessage), (byte*)ptr);
	}

	// Token: 0x06009570 RID: 38256 RVA: 0x003A54BC File Offset: 0x003A36BC
	public static void ModifyMass(int gameCell, float mass, byte disease_idx, int disease_count, CellModifyMassEvent ev, float temperature = -1f, SimHashes element = SimHashes.Vacuum)
	{
		if (element != SimHashes.Vacuum)
		{
			ushort elementIndex = SimMessages.GetElementIndex(element);
			if (elementIndex != 65535)
			{
				if (temperature == -1f)
				{
					temperature = ElementLoader.elements[(int)elementIndex].defaultValues.temperature;
				}
				SimMessages.ModifyCell(gameCell, elementIndex, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
				return;
			}
		}
		else
		{
			SimMessages.ModifyCell(gameCell, 0, temperature, mass, disease_idx, disease_count, SimMessages.ReplaceType.None, false, -1);
		}
	}

	// Token: 0x06009571 RID: 38257 RVA: 0x003A5524 File Offset: 0x003A3724
	public unsafe static void CreateElementInteractions(SimMessages.ElementInteraction[] interactions)
	{
		fixed (SimMessages.ElementInteraction[] array = interactions)
		{
			SimMessages.ElementInteraction* interactions2;
			if (interactions == null || array.Length == 0)
			{
				interactions2 = null;
			}
			else
			{
				interactions2 = &array[0];
			}
			SimMessages.CreateElementInteractionsMsg* ptr = stackalloc SimMessages.CreateElementInteractionsMsg[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CreateElementInteractionsMsg))];
			ptr->numInteractions = interactions.Length;
			ptr->interactions = interactions2;
			Sim.SIM_HandleMessage(-930289787, sizeof(SimMessages.CreateElementInteractionsMsg), (byte*)ptr);
		}
	}

	// Token: 0x06009572 RID: 38258 RVA: 0x003A557C File Offset: 0x003A377C
	public unsafe static void NewGameFrame(float elapsed_seconds, List<Game.SimActiveRegion> activeRegions)
	{
		Debug.Assert(activeRegions.Count > 0, "NewGameFrame cannot be called with zero activeRegions");
		Sim.NewGameFrame* ptr = stackalloc Sim.NewGameFrame[checked(unchecked((UIntPtr)activeRegions.Count) * (UIntPtr)sizeof(Sim.NewGameFrame))];
		Sim.NewGameFrame* ptr2 = ptr;
		foreach (Game.SimActiveRegion simActiveRegion in activeRegions)
		{
			Pair<Vector2I, Vector2I> region = simActiveRegion.region;
			region.first = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells - 1, simActiveRegion.region.first.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.first.y));
			region.second = new Vector2I(MathUtil.Clamp(0, Grid.WidthInCells, simActiveRegion.region.second.x), MathUtil.Clamp(0, Grid.HeightInCells - 1, simActiveRegion.region.second.y));
			ptr2->elapsedSeconds = elapsed_seconds;
			ptr2->minX = region.first.x;
			ptr2->minY = region.first.y;
			ptr2->maxX = region.second.x;
			ptr2->maxY = region.second.y;
			ptr2->currentSunlightIntensity = simActiveRegion.currentSunlightIntensity;
			ptr2->currentCosmicRadiationIntensity = simActiveRegion.currentCosmicRadiationIntensity;
			ptr2++;
		}
		Sim.SIM_HandleMessage(-775326397, sizeof(Sim.NewGameFrame) * activeRegions.Count, (byte*)ptr);
	}

	// Token: 0x06009573 RID: 38259 RVA: 0x003A5718 File Offset: 0x003A3918
	public unsafe static void SetDebugProperties(Sim.DebugProperties properties)
	{
		Sim.DebugProperties* ptr = stackalloc Sim.DebugProperties[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(Sim.DebugProperties))];
		*ptr = properties;
		ptr->buildingTemperatureScale = properties.buildingTemperatureScale;
		ptr->buildingToBuildingTemperatureScale = properties.buildingToBuildingTemperatureScale;
		Sim.SIM_HandleMessage(-1683118492, sizeof(Sim.DebugProperties), (byte*)ptr);
	}

	// Token: 0x06009574 RID: 38260 RVA: 0x003A5764 File Offset: 0x003A3964
	public unsafe static void ModifyCellWorldZone(int cell, byte zone_id)
	{
		SimMessages.CellWorldZoneModification* ptr = stackalloc SimMessages.CellWorldZoneModification[checked(unchecked((UIntPtr)1) * (UIntPtr)sizeof(SimMessages.CellWorldZoneModification))];
		ptr->cell = cell;
		ptr->zoneID = zone_id;
		Sim.SIM_HandleMessage(-449718014, sizeof(SimMessages.CellWorldZoneModification), (byte*)ptr);
	}

	// Token: 0x0400725A RID: 29274
	public const int InvalidCallback = -1;

	// Token: 0x0400725B RID: 29275
	public const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x02001BDF RID: 7135
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementConsumerMessage
	{
		// Token: 0x0400725C RID: 29276
		public int cellIdx;

		// Token: 0x0400725D RID: 29277
		public int callbackIdx;

		// Token: 0x0400725E RID: 29278
		public byte radius;

		// Token: 0x0400725F RID: 29279
		public byte configuration;

		// Token: 0x04007260 RID: 29280
		public ushort elementIdx;
	}

	// Token: 0x02001BE0 RID: 7136
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementConsumerDataMessage
	{
		// Token: 0x04007261 RID: 29281
		public int handle;

		// Token: 0x04007262 RID: 29282
		public int cell;

		// Token: 0x04007263 RID: 29283
		public float consumptionRate;
	}

	// Token: 0x02001BE1 RID: 7137
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementConsumerMessage
	{
		// Token: 0x04007264 RID: 29284
		public int handle;

		// Token: 0x04007265 RID: 29285
		public int callbackIdx;
	}

	// Token: 0x02001BE2 RID: 7138
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementEmitterMessage
	{
		// Token: 0x04007266 RID: 29286
		public float maxPressure;

		// Token: 0x04007267 RID: 29287
		public int callbackIdx;

		// Token: 0x04007268 RID: 29288
		public int onBlockedCB;

		// Token: 0x04007269 RID: 29289
		public int onUnblockedCB;
	}

	// Token: 0x02001BE3 RID: 7139
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementEmitterMessage
	{
		// Token: 0x0400726A RID: 29290
		public int handle;

		// Token: 0x0400726B RID: 29291
		public int cellIdx;

		// Token: 0x0400726C RID: 29292
		public float emitInterval;

		// Token: 0x0400726D RID: 29293
		public float emitMass;

		// Token: 0x0400726E RID: 29294
		public float emitTemperature;

		// Token: 0x0400726F RID: 29295
		public float maxPressure;

		// Token: 0x04007270 RID: 29296
		public int diseaseCount;

		// Token: 0x04007271 RID: 29297
		public ushort elementIdx;

		// Token: 0x04007272 RID: 29298
		public byte maxDepth;

		// Token: 0x04007273 RID: 29299
		public byte diseaseIdx;
	}

	// Token: 0x02001BE4 RID: 7140
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementEmitterMessage
	{
		// Token: 0x04007274 RID: 29300
		public int handle;

		// Token: 0x04007275 RID: 29301
		public int callbackIdx;
	}

	// Token: 0x02001BE5 RID: 7141
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddRadiationEmitterMessage
	{
		// Token: 0x04007276 RID: 29302
		public int callbackIdx;

		// Token: 0x04007277 RID: 29303
		public int cell;

		// Token: 0x04007278 RID: 29304
		public short emitRadiusX;

		// Token: 0x04007279 RID: 29305
		public short emitRadiusY;

		// Token: 0x0400727A RID: 29306
		public float emitRads;

		// Token: 0x0400727B RID: 29307
		public float emitRate;

		// Token: 0x0400727C RID: 29308
		public float emitSpeed;

		// Token: 0x0400727D RID: 29309
		public float emitDirection;

		// Token: 0x0400727E RID: 29310
		public float emitAngle;

		// Token: 0x0400727F RID: 29311
		public int emitType;
	}

	// Token: 0x02001BE6 RID: 7142
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyRadiationEmitterMessage
	{
		// Token: 0x04007280 RID: 29312
		public int handle;

		// Token: 0x04007281 RID: 29313
		public int cell;

		// Token: 0x04007282 RID: 29314
		public int callbackIdx;

		// Token: 0x04007283 RID: 29315
		public short emitRadiusX;

		// Token: 0x04007284 RID: 29316
		public short emitRadiusY;

		// Token: 0x04007285 RID: 29317
		public float emitRads;

		// Token: 0x04007286 RID: 29318
		public float emitRate;

		// Token: 0x04007287 RID: 29319
		public float emitSpeed;

		// Token: 0x04007288 RID: 29320
		public float emitDirection;

		// Token: 0x04007289 RID: 29321
		public float emitAngle;

		// Token: 0x0400728A RID: 29322
		public int emitType;
	}

	// Token: 0x02001BE7 RID: 7143
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveRadiationEmitterMessage
	{
		// Token: 0x0400728B RID: 29323
		public int handle;

		// Token: 0x0400728C RID: 29324
		public int callbackIdx;
	}

	// Token: 0x02001BE8 RID: 7144
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct AddElementChunkMessage
	{
		// Token: 0x0400728D RID: 29325
		public int gameCell;

		// Token: 0x0400728E RID: 29326
		public int callbackIdx;

		// Token: 0x0400728F RID: 29327
		public float mass;

		// Token: 0x04007290 RID: 29328
		public float temperature;

		// Token: 0x04007291 RID: 29329
		public float surfaceArea;

		// Token: 0x04007292 RID: 29330
		public float thickness;

		// Token: 0x04007293 RID: 29331
		public float groundTransferScale;

		// Token: 0x04007294 RID: 29332
		public ushort elementIdx;

		// Token: 0x04007295 RID: 29333
		public byte pad0;

		// Token: 0x04007296 RID: 29334
		public byte pad1;
	}

	// Token: 0x02001BE9 RID: 7145
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RemoveElementChunkMessage
	{
		// Token: 0x04007297 RID: 29335
		public int handle;

		// Token: 0x04007298 RID: 29336
		public int callbackIdx;
	}

	// Token: 0x02001BEA RID: 7146
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetElementChunkDataMessage
	{
		// Token: 0x04007299 RID: 29337
		public int handle;

		// Token: 0x0400729A RID: 29338
		public float temperature;

		// Token: 0x0400729B RID: 29339
		public float heatCapacity;
	}

	// Token: 0x02001BEB RID: 7147
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MoveElementChunkMessage
	{
		// Token: 0x0400729C RID: 29340
		public int handle;

		// Token: 0x0400729D RID: 29341
		public int gameCell;
	}

	// Token: 0x02001BEC RID: 7148
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkEnergyMessage
	{
		// Token: 0x0400729E RID: 29342
		public int handle;

		// Token: 0x0400729F RID: 29343
		public float deltaKJ;
	}

	// Token: 0x02001BED RID: 7149
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyElementChunkAdjusterMessage
	{
		// Token: 0x040072A0 RID: 29344
		public int handle;

		// Token: 0x040072A1 RID: 29345
		public float temperature;

		// Token: 0x040072A2 RID: 29346
		public float heatCapacity;

		// Token: 0x040072A3 RID: 29347
		public float thermalConductivity;
	}

	// Token: 0x02001BEE RID: 7150
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingHeatExchangeMessage
	{
		// Token: 0x040072A4 RID: 29348
		public int callbackIdx;

		// Token: 0x040072A5 RID: 29349
		public ushort elemIdx;

		// Token: 0x040072A6 RID: 29350
		public byte pad0;

		// Token: 0x040072A7 RID: 29351
		public byte pad1;

		// Token: 0x040072A8 RID: 29352
		public float mass;

		// Token: 0x040072A9 RID: 29353
		public float temperature;

		// Token: 0x040072AA RID: 29354
		public float thermalConductivity;

		// Token: 0x040072AB RID: 29355
		public float overheatTemperature;

		// Token: 0x040072AC RID: 29356
		public float operatingKilowatts;

		// Token: 0x040072AD RID: 29357
		public int minX;

		// Token: 0x040072AE RID: 29358
		public int minY;

		// Token: 0x040072AF RID: 29359
		public int maxX;

		// Token: 0x040072B0 RID: 29360
		public int maxY;
	}

	// Token: 0x02001BEF RID: 7151
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingHeatExchangeMessage
	{
		// Token: 0x040072B1 RID: 29361
		public int callbackIdx;

		// Token: 0x040072B2 RID: 29362
		public ushort elemIdx;

		// Token: 0x040072B3 RID: 29363
		public byte pad0;

		// Token: 0x040072B4 RID: 29364
		public byte pad1;

		// Token: 0x040072B5 RID: 29365
		public float mass;

		// Token: 0x040072B6 RID: 29366
		public float temperature;

		// Token: 0x040072B7 RID: 29367
		public float thermalConductivity;

		// Token: 0x040072B8 RID: 29368
		public float overheatTemperature;

		// Token: 0x040072B9 RID: 29369
		public float operatingKilowatts;

		// Token: 0x040072BA RID: 29370
		public int minX;

		// Token: 0x040072BB RID: 29371
		public int minY;

		// Token: 0x040072BC RID: 29372
		public int maxX;

		// Token: 0x040072BD RID: 29373
		public int maxY;
	}

	// Token: 0x02001BF0 RID: 7152
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyBuildingEnergyMessage
	{
		// Token: 0x040072BE RID: 29374
		public int handle;

		// Token: 0x040072BF RID: 29375
		public float deltaKJ;

		// Token: 0x040072C0 RID: 29376
		public float minTemperature;

		// Token: 0x040072C1 RID: 29377
		public float maxTemperature;
	}

	// Token: 0x02001BF1 RID: 7153
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingHeatExchangeMessage
	{
		// Token: 0x040072C2 RID: 29378
		public int handle;

		// Token: 0x040072C3 RID: 29379
		public int callbackIdx;
	}

	// Token: 0x02001BF2 RID: 7154
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RegisterBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x040072C4 RID: 29380
		public int callbackIdx;

		// Token: 0x040072C5 RID: 29381
		public int structureTemperatureHandler;
	}

	// Token: 0x02001BF3 RID: 7155
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x040072C6 RID: 29382
		public int selfHandler;

		// Token: 0x040072C7 RID: 29383
		public int buildingInContactHandle;

		// Token: 0x040072C8 RID: 29384
		public int cellsInContact;
	}

	// Token: 0x02001BF4 RID: 7156
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingInContactFromBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x040072C9 RID: 29385
		public int selfHandler;

		// Token: 0x040072CA RID: 29386
		public int buildingNoLongerInContactHandler;
	}

	// Token: 0x02001BF5 RID: 7157
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveBuildingToBuildingHeatExchangeMessage
	{
		// Token: 0x040072CB RID: 29387
		public int callbackIdx;

		// Token: 0x040072CC RID: 29388
		public int selfHandler;
	}

	// Token: 0x02001BF6 RID: 7158
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct AddDiseaseEmitterMessage
	{
		// Token: 0x040072CD RID: 29389
		public int callbackIdx;
	}

	// Token: 0x02001BF7 RID: 7159
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ModifyDiseaseEmitterMessage
	{
		// Token: 0x040072CE RID: 29390
		public int handle;

		// Token: 0x040072CF RID: 29391
		public int gameCell;

		// Token: 0x040072D0 RID: 29392
		public byte diseaseIdx;

		// Token: 0x040072D1 RID: 29393
		public byte maxDepth;

		// Token: 0x040072D2 RID: 29394
		private byte pad0;

		// Token: 0x040072D3 RID: 29395
		private byte pad1;

		// Token: 0x040072D4 RID: 29396
		public float emitInterval;

		// Token: 0x040072D5 RID: 29397
		public int emitCount;
	}

	// Token: 0x02001BF8 RID: 7160
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct RemoveDiseaseEmitterMessage
	{
		// Token: 0x040072D6 RID: 29398
		public int handle;

		// Token: 0x040072D7 RID: 29399
		public int callbackIdx;
	}

	// Token: 0x02001BF9 RID: 7161
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetSavedOptionsMessage
	{
		// Token: 0x040072D8 RID: 29400
		public byte clearBits;

		// Token: 0x040072D9 RID: 29401
		public byte setBits;
	}

	// Token: 0x02001BFA RID: 7162
	public enum SimSavedOptions : byte
	{
		// Token: 0x040072DB RID: 29403
		ENABLE_DIAGONAL_FALLING_SAND = 1
	}

	// Token: 0x02001BFB RID: 7163
	public struct WorldOffsetData
	{
		// Token: 0x040072DC RID: 29404
		public int worldOffsetX;

		// Token: 0x040072DD RID: 29405
		public int worldOffsetY;

		// Token: 0x040072DE RID: 29406
		public int worldSizeX;

		// Token: 0x040072DF RID: 29407
		public int worldSizeY;
	}

	// Token: 0x02001BFC RID: 7164
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct DigMessage
	{
		// Token: 0x040072E0 RID: 29408
		public int cellIdx;

		// Token: 0x040072E1 RID: 29409
		public int callbackIdx;

		// Token: 0x040072E2 RID: 29410
		public bool skipEvent;
	}

	// Token: 0x02001BFD RID: 7165
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetCellFloatValueMessage
	{
		// Token: 0x040072E3 RID: 29411
		public int cellIdx;

		// Token: 0x040072E4 RID: 29412
		public float value;
	}

	// Token: 0x02001BFE RID: 7166
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellPropertiesMessage
	{
		// Token: 0x040072E5 RID: 29413
		public int cellIdx;

		// Token: 0x040072E6 RID: 29414
		public byte properties;

		// Token: 0x040072E7 RID: 29415
		public byte set;

		// Token: 0x040072E8 RID: 29416
		public byte pad0;

		// Token: 0x040072E9 RID: 29417
		public byte pad1;
	}

	// Token: 0x02001BFF RID: 7167
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct SetInsulationValueMessage
	{
		// Token: 0x040072EA RID: 29418
		public int cellIdx;

		// Token: 0x040072EB RID: 29419
		public float value;
	}

	// Token: 0x02001C00 RID: 7168
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellMessage
	{
		// Token: 0x040072EC RID: 29420
		public int cellIdx;

		// Token: 0x040072ED RID: 29421
		public int callbackIdx;

		// Token: 0x040072EE RID: 29422
		public float temperature;

		// Token: 0x040072EF RID: 29423
		public float mass;

		// Token: 0x040072F0 RID: 29424
		public int diseaseCount;

		// Token: 0x040072F1 RID: 29425
		public ushort elementIdx;

		// Token: 0x040072F2 RID: 29426
		public byte replaceType;

		// Token: 0x040072F3 RID: 29427
		public byte diseaseIdx;

		// Token: 0x040072F4 RID: 29428
		public byte addSubType;
	}

	// Token: 0x02001C01 RID: 7169
	public enum ReplaceType
	{
		// Token: 0x040072F6 RID: 29430
		None,
		// Token: 0x040072F7 RID: 29431
		Replace,
		// Token: 0x040072F8 RID: 29432
		ReplaceAndDisplace
	}

	// Token: 0x02001C02 RID: 7170
	private enum AddSolidMassSubType
	{
		// Token: 0x040072FA RID: 29434
		DoVerticalDisplacement,
		// Token: 0x040072FB RID: 29435
		OnlyIfSameElement
	}

	// Token: 0x02001C03 RID: 7171
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellDiseaseModification
	{
		// Token: 0x040072FC RID: 29436
		public int cellIdx;

		// Token: 0x040072FD RID: 29437
		public byte diseaseIdx;

		// Token: 0x040072FE RID: 29438
		public byte pad0;

		// Token: 0x040072FF RID: 29439
		public byte pad1;

		// Token: 0x04007300 RID: 29440
		public byte pad2;

		// Token: 0x04007301 RID: 29441
		public int diseaseCount;
	}

	// Token: 0x02001C04 RID: 7172
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct RadiationParamsModification
	{
		// Token: 0x04007302 RID: 29442
		public int RadiationParamsType;

		// Token: 0x04007303 RID: 29443
		public float value;
	}

	// Token: 0x02001C05 RID: 7173
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CellRadiationModification
	{
		// Token: 0x04007304 RID: 29444
		public int cellIdx;

		// Token: 0x04007305 RID: 29445
		public float radiationDelta;

		// Token: 0x04007306 RID: 29446
		public int callbackIdx;
	}

	// Token: 0x02001C06 RID: 7174
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassConsumptionMessage
	{
		// Token: 0x04007307 RID: 29447
		public int cellIdx;

		// Token: 0x04007308 RID: 29448
		public int callbackIdx;

		// Token: 0x04007309 RID: 29449
		public float mass;

		// Token: 0x0400730A RID: 29450
		public ushort elementIdx;

		// Token: 0x0400730B RID: 29451
		public byte radius;
	}

	// Token: 0x02001C07 RID: 7175
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct MassEmissionMessage
	{
		// Token: 0x0400730C RID: 29452
		public int cellIdx;

		// Token: 0x0400730D RID: 29453
		public int callbackIdx;

		// Token: 0x0400730E RID: 29454
		public float mass;

		// Token: 0x0400730F RID: 29455
		public float temperature;

		// Token: 0x04007310 RID: 29456
		public int diseaseCount;

		// Token: 0x04007311 RID: 29457
		public ushort elementIdx;

		// Token: 0x04007312 RID: 29458
		public byte diseaseIdx;
	}

	// Token: 0x02001C08 RID: 7176
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ConsumeDiseaseMessage
	{
		// Token: 0x04007313 RID: 29459
		public int gameCell;

		// Token: 0x04007314 RID: 29460
		public int callbackIdx;

		// Token: 0x04007315 RID: 29461
		public float percentToConsume;

		// Token: 0x04007316 RID: 29462
		public int maxToConsume;
	}

	// Token: 0x02001C09 RID: 7177
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct ModifyCellEnergyMessage
	{
		// Token: 0x04007317 RID: 29463
		public int cellIdx;

		// Token: 0x04007318 RID: 29464
		public float kilojoules;

		// Token: 0x04007319 RID: 29465
		public float maxTemperature;

		// Token: 0x0400731A RID: 29466
		public int id;
	}

	// Token: 0x02001C0A RID: 7178
	public enum EnergySourceID
	{
		// Token: 0x0400731C RID: 29468
		DebugHeat = 1000,
		// Token: 0x0400731D RID: 29469
		DebugCool,
		// Token: 0x0400731E RID: 29470
		FierySkin,
		// Token: 0x0400731F RID: 29471
		Overheatable,
		// Token: 0x04007320 RID: 29472
		LiquidCooledFan,
		// Token: 0x04007321 RID: 29473
		ConduitTemperatureManager,
		// Token: 0x04007322 RID: 29474
		Excavator,
		// Token: 0x04007323 RID: 29475
		HeatBulb,
		// Token: 0x04007324 RID: 29476
		WarmBlooded,
		// Token: 0x04007325 RID: 29477
		StructureTemperature,
		// Token: 0x04007326 RID: 29478
		Burner,
		// Token: 0x04007327 RID: 29479
		VacuumRadiator
	}

	// Token: 0x02001C0B RID: 7179
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct VisibleCells
	{
		// Token: 0x04007328 RID: 29480
		public Vector2I min;

		// Token: 0x04007329 RID: 29481
		public Vector2I max;
	}

	// Token: 0x02001C0C RID: 7180
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct WakeCellMessage
	{
		// Token: 0x0400732A RID: 29482
		public int gameCell;
	}

	// Token: 0x02001C0D RID: 7181
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementInteraction
	{
		// Token: 0x0400732B RID: 29483
		public uint interactionType;

		// Token: 0x0400732C RID: 29484
		public ushort elemIdx1;

		// Token: 0x0400732D RID: 29485
		public ushort elemIdx2;

		// Token: 0x0400732E RID: 29486
		public ushort elemResultIdx;

		// Token: 0x0400732F RID: 29487
		public byte pad0;

		// Token: 0x04007330 RID: 29488
		public byte pad1;

		// Token: 0x04007331 RID: 29489
		public float minMass;

		// Token: 0x04007332 RID: 29490
		public float interactionProbability;

		// Token: 0x04007333 RID: 29491
		public float elem1MassDestructionPercent;

		// Token: 0x04007334 RID: 29492
		public float elem2MassRequiredMultiplier;

		// Token: 0x04007335 RID: 29493
		public float elemResultMassCreationMultiplier;
	}

	// Token: 0x02001C0E RID: 7182
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	private struct CreateElementInteractionsMsg
	{
		// Token: 0x04007336 RID: 29494
		public int numInteractions;

		// Token: 0x04007337 RID: 29495
		public unsafe SimMessages.ElementInteraction* interactions;
	}

	// Token: 0x02001C0F RID: 7183
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeChange
	{
		// Token: 0x04007338 RID: 29496
		public int cell;

		// Token: 0x04007339 RID: 29497
		public byte layer;

		// Token: 0x0400733A RID: 29498
		public byte pad0;

		// Token: 0x0400733B RID: 29499
		public byte pad1;

		// Token: 0x0400733C RID: 29500
		public byte pad2;

		// Token: 0x0400733D RID: 29501
		public float mass;

		// Token: 0x0400733E RID: 29502
		public float temperature;

		// Token: 0x0400733F RID: 29503
		public int elementHash;
	}

	// Token: 0x02001C10 RID: 7184
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellWorldZoneModification
	{
		// Token: 0x04007340 RID: 29504
		public int cell;

		// Token: 0x04007341 RID: 29505
		public byte zoneID;
	}
}
