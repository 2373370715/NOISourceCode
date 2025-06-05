using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x02001BB4 RID: 7092
public static class Sim
{
	// Token: 0x06009513 RID: 38163 RVA: 0x00105D58 File Offset: 0x00103F58
	public static bool IsRadiationEnabled()
	{
		return DlcManager.FeatureRadiationEnabled();
	}

	// Token: 0x06009514 RID: 38164 RVA: 0x00105D5F File Offset: 0x00103F5F
	public static bool IsValidHandle(int h)
	{
		return h != -1 && h != -2;
	}

	// Token: 0x06009515 RID: 38165 RVA: 0x00105D6F File Offset: 0x00103F6F
	public static int GetHandleIndex(int h)
	{
		return h & 16777215;
	}

	// Token: 0x06009516 RID: 38166
	[DllImport("SimDLL")]
	public static extern void SIM_Initialize(Sim.GAME_MessageHandler callback);

	// Token: 0x06009517 RID: 38167
	[DllImport("SimDLL")]
	public static extern void SIM_Shutdown();

	// Token: 0x06009518 RID: 38168
	[DllImport("SimDLL")]
	public unsafe static extern IntPtr SIM_HandleMessage(int sim_msg_id, int msg_length, byte* msg);

	// Token: 0x06009519 RID: 38169
	[DllImport("SimDLL")]
	private unsafe static extern byte* SIM_BeginSave(int* size, int x, int y);

	// Token: 0x0600951A RID: 38170
	[DllImport("SimDLL")]
	private static extern void SIM_EndSave();

	// Token: 0x0600951B RID: 38171
	[DllImport("SimDLL")]
	public static extern void SIM_DebugCrash();

	// Token: 0x0600951C RID: 38172 RVA: 0x003A34E8 File Offset: 0x003A16E8
	public unsafe static IntPtr HandleMessage(SimMessageHashes sim_msg_id, int msg_length, byte[] msg)
	{
		IntPtr result;
		fixed (byte[] array = msg)
		{
			byte* msg2;
			if (msg == null || array.Length == 0)
			{
				msg2 = null;
			}
			else
			{
				msg2 = &array[0];
			}
			result = Sim.SIM_HandleMessage((int)sim_msg_id, msg_length, msg2);
		}
		return result;
	}

	// Token: 0x0600951D RID: 38173 RVA: 0x003A3518 File Offset: 0x003A1718
	public unsafe static void Save(BinaryWriter writer, int x, int y)
	{
		int num;
		void* value = (void*)Sim.SIM_BeginSave(&num, x, y);
		byte[] array = new byte[num];
		Marshal.Copy((IntPtr)value, array, 0, num);
		Sim.SIM_EndSave();
		writer.Write(num);
		writer.Write(array);
	}

	// Token: 0x0600951E RID: 38174 RVA: 0x003A3558 File Offset: 0x003A1758
	public unsafe static int LoadWorld(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x0600951F RID: 38175 RVA: 0x003A35A8 File Offset: 0x003A17A8
	public static void AllocateCells(int width, int height, bool headless = false)
	{
		using (MemoryStream memoryStream = new MemoryStream(8))
		{
			using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
			{
				binaryWriter.Write(width);
				binaryWriter.Write(height);
				bool value = Sim.IsRadiationEnabled();
				binaryWriter.Write(value);
				binaryWriter.Write(headless);
				binaryWriter.Flush();
				Sim.HandleMessage(SimMessageHashes.AllocateCells, (int)memoryStream.Length, memoryStream.GetBuffer());
			}
		}
	}

	// Token: 0x06009520 RID: 38176 RVA: 0x003A3558 File Offset: 0x003A1758
	public unsafe static int Load(IReader reader)
	{
		int num = reader.ReadInt32();
		byte[] array;
		byte* msg;
		if ((array = reader.ReadBytes(num)) == null || array.Length == 0)
		{
			msg = null;
		}
		else
		{
			msg = &array[0];
		}
		IntPtr value = Sim.SIM_HandleMessage(-672538170, num, msg);
		array = null;
		if (value == IntPtr.Zero)
		{
			return -1;
		}
		return 0;
	}

	// Token: 0x06009521 RID: 38177 RVA: 0x003A3638 File Offset: 0x003A1838
	public unsafe static void Start()
	{
		Sim.GameDataUpdate* ptr = (Sim.GameDataUpdate*)((void*)Sim.SIM_HandleMessage(-931446686, 0, null));
		Grid.elementIdx = ptr->elementIdx;
		Grid.temperature = ptr->temperature;
		Grid.radiation = ptr->radiation;
		Grid.mass = ptr->mass;
		Grid.properties = ptr->properties;
		Grid.strengthInfo = ptr->strengthInfo;
		Grid.insulation = ptr->insulation;
		Grid.diseaseIdx = ptr->diseaseIdx;
		Grid.diseaseCount = ptr->diseaseCount;
		Grid.AccumulatedFlowValues = ptr->accumulatedFlow;
		PropertyTextures.externalFlowTex = ptr->propertyTextureFlow;
		PropertyTextures.externalLiquidTex = ptr->propertyTextureLiquid;
		PropertyTextures.externalLiquidDataTex = ptr->propertyTextureLiquidData;
		PropertyTextures.externalExposedToSunlight = ptr->propertyTextureExposedToSunlight;
		Grid.InitializeCells();
	}

	// Token: 0x06009522 RID: 38178 RVA: 0x00105D78 File Offset: 0x00103F78
	public static void Shutdown()
	{
		Sim.SIM_Shutdown();
		Grid.mass = null;
	}

	// Token: 0x06009523 RID: 38179
	[DllImport("SimDLL")]
	public unsafe static extern char* SYSINFO_Acquire();

	// Token: 0x06009524 RID: 38180
	[DllImport("SimDLL")]
	public static extern void SYSINFO_Release();

	// Token: 0x06009525 RID: 38181 RVA: 0x003A36F8 File Offset: 0x003A18F8
	public unsafe static int DLL_MessageHandler(int message_id, IntPtr data)
	{
		if (message_id == 0)
		{
			Sim.DLLExceptionHandlerMessage* ptr = (Sim.DLLExceptionHandlerMessage*)((void*)data);
			string stack_trace = Marshal.PtrToStringAnsi(ptr->callstack);
			string dmp_filename = Marshal.PtrToStringAnsi(ptr->dmpFilename);
			KCrashReporter.ReportSimDLLCrash("SimDLL Crash Dump", stack_trace, dmp_filename);
			return 0;
		}
		if (message_id == 1)
		{
			Sim.DLLReportMessageMessage* ptr2 = (Sim.DLLReportMessageMessage*)((void*)data);
			string msg = "SimMessage: " + Marshal.PtrToStringAnsi(ptr2->message);
			string stack_trace2;
			if (ptr2->callstack != IntPtr.Zero)
			{
				stack_trace2 = Marshal.PtrToStringAnsi(ptr2->callstack);
			}
			else
			{
				string str = Marshal.PtrToStringAnsi(ptr2->file);
				int line = ptr2->line;
				stack_trace2 = str + ":" + line.ToString();
			}
			KCrashReporter.ReportSimDLLCrash(msg, stack_trace2, null);
			return 0;
		}
		return -1;
	}

	// Token: 0x0400713E RID: 28990
	public const int InvalidHandle = -1;

	// Token: 0x0400713F RID: 28991
	public const int QueuedRegisterHandle = -2;

	// Token: 0x04007140 RID: 28992
	public const byte InvalidDiseaseIdx = 255;

	// Token: 0x04007141 RID: 28993
	public const ushort InvalidElementIdx = 65535;

	// Token: 0x04007142 RID: 28994
	public const byte SpaceZoneID = 255;

	// Token: 0x04007143 RID: 28995
	public const byte SolidZoneID = 0;

	// Token: 0x04007144 RID: 28996
	public const int ChunkEdgeSize = 32;

	// Token: 0x04007145 RID: 28997
	public const float StateTransitionEnergy = 3f;

	// Token: 0x04007146 RID: 28998
	public const float ZeroDegreesCentigrade = 273.15f;

	// Token: 0x04007147 RID: 28999
	public const float StandardTemperature = 293.15f;

	// Token: 0x04007148 RID: 29000
	public const float StandardMeltingPointOffset = 10f;

	// Token: 0x04007149 RID: 29001
	public const float StandardPressure = 101.3f;

	// Token: 0x0400714A RID: 29002
	public const float Epsilon = 0.0001f;

	// Token: 0x0400714B RID: 29003
	public const float MaxTemperature = 10000f;

	// Token: 0x0400714C RID: 29004
	public const float MinTemperature = 0f;

	// Token: 0x0400714D RID: 29005
	public const float MaxRadiation = 9000000f;

	// Token: 0x0400714E RID: 29006
	public const float MinRadiation = 0f;

	// Token: 0x0400714F RID: 29007
	public const float MaxMass = 10000f;

	// Token: 0x04007150 RID: 29008
	public const float MinMass = 1.0001f;

	// Token: 0x04007151 RID: 29009
	private const int PressureUpdateInterval = 1;

	// Token: 0x04007152 RID: 29010
	private const int TemperatureUpdateInterval = 1;

	// Token: 0x04007153 RID: 29011
	private const int LiquidUpdateInterval = 1;

	// Token: 0x04007154 RID: 29012
	private const int LifeUpdateInterval = 1;

	// Token: 0x04007155 RID: 29013
	public const byte ClearSkyGridValue = 253;

	// Token: 0x04007156 RID: 29014
	public const int PACKING_ALIGNMENT = 4;

	// Token: 0x02001BB5 RID: 7093
	// (Invoke) Token: 0x06009527 RID: 38183
	public delegate int GAME_MessageHandler(int message_id, IntPtr data);

	// Token: 0x02001BB6 RID: 7094
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLExceptionHandlerMessage
	{
		// Token: 0x04007157 RID: 29015
		public IntPtr callstack;

		// Token: 0x04007158 RID: 29016
		public IntPtr dmpFilename;
	}

	// Token: 0x02001BB7 RID: 7095
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DLLReportMessageMessage
	{
		// Token: 0x04007159 RID: 29017
		public IntPtr callstack;

		// Token: 0x0400715A RID: 29018
		public IntPtr message;

		// Token: 0x0400715B RID: 29019
		public IntPtr file;

		// Token: 0x0400715C RID: 29020
		public int line;
	}

	// Token: 0x02001BB8 RID: 7096
	private enum GameHandledMessages
	{
		// Token: 0x0400715E RID: 29022
		ExceptionHandler,
		// Token: 0x0400715F RID: 29023
		ReportMessage
	}

	// Token: 0x02001BB9 RID: 7097
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PhysicsData
	{
		// Token: 0x0600952A RID: 38186 RVA: 0x00105D86 File Offset: 0x00103F86
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.temperature);
			writer.Write(this.mass);
			writer.Write(this.pressure);
		}

		// Token: 0x04007160 RID: 29024
		public float temperature;

		// Token: 0x04007161 RID: 29025
		public float mass;

		// Token: 0x04007162 RID: 29026
		public float pressure;
	}

	// Token: 0x02001BBA RID: 7098
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Cell
	{
		// Token: 0x0600952B RID: 38187 RVA: 0x003A37B0 File Offset: 0x003A19B0
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.elementIdx);
			writer.Write(0);
			writer.Write(this.insulation);
			writer.Write(0);
			writer.Write(this.pad0);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.temperature);
			writer.Write(this.mass);
		}

		// Token: 0x0600952C RID: 38188 RVA: 0x00105DAF File Offset: 0x00103FAF
		public void SetValues(global::Element elem, List<global::Element> elements)
		{
			this.SetValues(elem, elem.defaultValues, elements);
		}

		// Token: 0x0600952D RID: 38189 RVA: 0x003A3824 File Offset: 0x003A1A24
		public void SetValues(global::Element elem, Sim.PhysicsData pd, List<global::Element> elements)
		{
			this.elementIdx = (ushort)elements.IndexOf(elem);
			this.temperature = pd.temperature;
			this.mass = pd.mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x0600952E RID: 38190 RVA: 0x003A388C File Offset: 0x003A1A8C
		public void SetValues(ushort new_elem_idx, float new_temperature, float new_mass)
		{
			this.elementIdx = new_elem_idx;
			this.temperature = new_temperature;
			this.mass = new_mass;
			this.insulation = byte.MaxValue;
			DebugUtil.Assert(this.temperature > 0f || this.mass == 0f, "A non-zero mass cannot have a <= 0 temperature");
		}

		// Token: 0x04007163 RID: 29027
		public ushort elementIdx;

		// Token: 0x04007164 RID: 29028
		public byte properties;

		// Token: 0x04007165 RID: 29029
		public byte insulation;

		// Token: 0x04007166 RID: 29030
		public byte strengthInfo;

		// Token: 0x04007167 RID: 29031
		public byte pad0;

		// Token: 0x04007168 RID: 29032
		public byte pad1;

		// Token: 0x04007169 RID: 29033
		public byte pad2;

		// Token: 0x0400716A RID: 29034
		public float temperature;

		// Token: 0x0400716B RID: 29035
		public float mass;

		// Token: 0x02001BBB RID: 7099
		public enum Properties
		{
			// Token: 0x0400716D RID: 29037
			GasImpermeable = 1,
			// Token: 0x0400716E RID: 29038
			LiquidImpermeable,
			// Token: 0x0400716F RID: 29039
			SolidImpermeable = 4,
			// Token: 0x04007170 RID: 29040
			Unbreakable = 8,
			// Token: 0x04007171 RID: 29041
			Transparent = 16,
			// Token: 0x04007172 RID: 29042
			Opaque = 32,
			// Token: 0x04007173 RID: 29043
			NotifyOnMelt = 64,
			// Token: 0x04007174 RID: 29044
			ConstructedTile = 128
		}
	}

	// Token: 0x02001BBC RID: 7100
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Element
	{
		// Token: 0x0600952F RID: 38191 RVA: 0x003A38E0 File Offset: 0x003A1AE0
		public Element(global::Element e, List<global::Element> elements)
		{
			this.id = e.id;
			this.state = (byte)e.state;
			if (e.HasTag(GameTags.Unstable))
			{
				this.state |= 8;
			}
			int num = elements.FindIndex((global::Element ele) => ele.id == e.lowTempTransitionTarget);
			int num2 = elements.FindIndex((global::Element ele) => ele.id == e.highTempTransitionTarget);
			this.lowTempTransitionIdx = (ushort)((num >= 0) ? num : 65535);
			this.highTempTransitionIdx = (ushort)((num2 >= 0) ? num2 : 65535);
			this.elementsTableIdx = (ushort)elements.IndexOf(e);
			this.specificHeatCapacity = e.specificHeatCapacity;
			this.thermalConductivity = e.thermalConductivity;
			this.solidSurfaceAreaMultiplier = e.solidSurfaceAreaMultiplier;
			this.liquidSurfaceAreaMultiplier = e.liquidSurfaceAreaMultiplier;
			this.gasSurfaceAreaMultiplier = e.gasSurfaceAreaMultiplier;
			this.molarMass = e.molarMass;
			this.strength = e.strength;
			this.flow = e.flow;
			this.viscosity = e.viscosity;
			this.minHorizontalFlow = e.minHorizontalFlow;
			this.minVerticalFlow = e.minVerticalFlow;
			this.maxMass = e.maxMass;
			this.lowTemp = e.lowTemp;
			this.highTemp = e.highTemp;
			this.highTempTransitionOreID = e.highTempTransitionOreID;
			this.highTempTransitionOreMassConversion = e.highTempTransitionOreMassConversion;
			this.lowTempTransitionOreID = e.lowTempTransitionOreID;
			this.lowTempTransitionOreMassConversion = e.lowTempTransitionOreMassConversion;
			this.sublimateIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.sublimateId);
			this.convertIndex = (ushort)elements.FindIndex((global::Element ele) => ele.id == e.convertId);
			this.pack0 = 0;
			if (e.substance == null)
			{
				this.colour = 0U;
			}
			else
			{
				Color32 color = e.substance.colour;
				this.colour = (uint)((int)color.a << 24 | (int)color.b << 16 | (int)color.g << 8 | (int)color.r);
			}
			this.sublimateFX = e.sublimateFX;
			this.sublimateRate = e.sublimateRate;
			this.sublimateEfficiency = e.sublimateEfficiency;
			this.sublimateProbability = e.sublimateProbability;
			this.offGasProbability = e.offGasPercentage;
			this.lightAbsorptionFactor = e.lightAbsorptionFactor;
			this.radiationAbsorptionFactor = e.radiationAbsorptionFactor;
			this.radiationPer1000Mass = e.radiationPer1000Mass;
			this.defaultValues = e.defaultValues;
		}

		// Token: 0x06009530 RID: 38192 RVA: 0x003A3BF0 File Offset: 0x003A1DF0
		public void Write(BinaryWriter writer)
		{
			writer.Write((int)this.id);
			writer.Write(this.lowTempTransitionIdx);
			writer.Write(this.highTempTransitionIdx);
			writer.Write(this.elementsTableIdx);
			writer.Write(this.state);
			writer.Write(this.pack0);
			writer.Write(this.specificHeatCapacity);
			writer.Write(this.thermalConductivity);
			writer.Write(this.molarMass);
			writer.Write(this.solidSurfaceAreaMultiplier);
			writer.Write(this.liquidSurfaceAreaMultiplier);
			writer.Write(this.gasSurfaceAreaMultiplier);
			writer.Write(this.flow);
			writer.Write(this.viscosity);
			writer.Write(this.minHorizontalFlow);
			writer.Write(this.minVerticalFlow);
			writer.Write(this.maxMass);
			writer.Write(this.lowTemp);
			writer.Write(this.highTemp);
			writer.Write(this.strength);
			writer.Write((int)this.lowTempTransitionOreID);
			writer.Write(this.lowTempTransitionOreMassConversion);
			writer.Write((int)this.highTempTransitionOreID);
			writer.Write(this.highTempTransitionOreMassConversion);
			writer.Write(this.sublimateIndex);
			writer.Write(this.convertIndex);
			writer.Write(this.colour);
			writer.Write((int)this.sublimateFX);
			writer.Write(this.sublimateRate);
			writer.Write(this.sublimateEfficiency);
			writer.Write(this.sublimateProbability);
			writer.Write(this.offGasProbability);
			writer.Write(this.lightAbsorptionFactor);
			writer.Write(this.radiationAbsorptionFactor);
			writer.Write(this.radiationPer1000Mass);
			this.defaultValues.Write(writer);
		}

		// Token: 0x04007175 RID: 29045
		public SimHashes id;

		// Token: 0x04007176 RID: 29046
		public ushort lowTempTransitionIdx;

		// Token: 0x04007177 RID: 29047
		public ushort highTempTransitionIdx;

		// Token: 0x04007178 RID: 29048
		public ushort elementsTableIdx;

		// Token: 0x04007179 RID: 29049
		public byte state;

		// Token: 0x0400717A RID: 29050
		public byte pack0;

		// Token: 0x0400717B RID: 29051
		public float specificHeatCapacity;

		// Token: 0x0400717C RID: 29052
		public float thermalConductivity;

		// Token: 0x0400717D RID: 29053
		public float molarMass;

		// Token: 0x0400717E RID: 29054
		public float solidSurfaceAreaMultiplier;

		// Token: 0x0400717F RID: 29055
		public float liquidSurfaceAreaMultiplier;

		// Token: 0x04007180 RID: 29056
		public float gasSurfaceAreaMultiplier;

		// Token: 0x04007181 RID: 29057
		public float flow;

		// Token: 0x04007182 RID: 29058
		public float viscosity;

		// Token: 0x04007183 RID: 29059
		public float minHorizontalFlow;

		// Token: 0x04007184 RID: 29060
		public float minVerticalFlow;

		// Token: 0x04007185 RID: 29061
		public float maxMass;

		// Token: 0x04007186 RID: 29062
		public float lowTemp;

		// Token: 0x04007187 RID: 29063
		public float highTemp;

		// Token: 0x04007188 RID: 29064
		public float strength;

		// Token: 0x04007189 RID: 29065
		public SimHashes lowTempTransitionOreID;

		// Token: 0x0400718A RID: 29066
		public float lowTempTransitionOreMassConversion;

		// Token: 0x0400718B RID: 29067
		public SimHashes highTempTransitionOreID;

		// Token: 0x0400718C RID: 29068
		public float highTempTransitionOreMassConversion;

		// Token: 0x0400718D RID: 29069
		public ushort sublimateIndex;

		// Token: 0x0400718E RID: 29070
		public ushort convertIndex;

		// Token: 0x0400718F RID: 29071
		public uint colour;

		// Token: 0x04007190 RID: 29072
		public SpawnFXHashes sublimateFX;

		// Token: 0x04007191 RID: 29073
		public float sublimateRate;

		// Token: 0x04007192 RID: 29074
		public float sublimateEfficiency;

		// Token: 0x04007193 RID: 29075
		public float sublimateProbability;

		// Token: 0x04007194 RID: 29076
		public float offGasProbability;

		// Token: 0x04007195 RID: 29077
		public float lightAbsorptionFactor;

		// Token: 0x04007196 RID: 29078
		public float radiationAbsorptionFactor;

		// Token: 0x04007197 RID: 29079
		public float radiationPer1000Mass;

		// Token: 0x04007198 RID: 29080
		public Sim.PhysicsData defaultValues;
	}

	// Token: 0x02001BBE RID: 7102
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseCell
	{
		// Token: 0x06009536 RID: 38198 RVA: 0x003A3DC4 File Offset: 0x003A1FC4
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.diseaseIdx);
			writer.Write(this.reservedInfestationTickCount);
			writer.Write(this.pad1);
			writer.Write(this.pad2);
			writer.Write(this.elementCount);
			writer.Write(this.reservedAccumulatedError);
		}

		// Token: 0x0400719A RID: 29082
		public byte diseaseIdx;

		// Token: 0x0400719B RID: 29083
		private byte reservedInfestationTickCount;

		// Token: 0x0400719C RID: 29084
		private byte pad1;

		// Token: 0x0400719D RID: 29085
		private byte pad2;

		// Token: 0x0400719E RID: 29086
		public int elementCount;

		// Token: 0x0400719F RID: 29087
		private float reservedAccumulatedError;

		// Token: 0x040071A0 RID: 29088
		public static readonly Sim.DiseaseCell Invalid = new Sim.DiseaseCell
		{
			diseaseIdx = byte.MaxValue,
			elementCount = 0
		};
	}

	// Token: 0x02001BBF RID: 7103
	// (Invoke) Token: 0x06009539 RID: 38201
	public delegate void GAME_Callback();

	// Token: 0x02001BC0 RID: 7104
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidInfo
	{
		// Token: 0x040071A1 RID: 29089
		public int cellIdx;

		// Token: 0x040071A2 RID: 29090
		public int isSolid;
	}

	// Token: 0x02001BC1 RID: 7105
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct LiquidChangeInfo
	{
		// Token: 0x040071A3 RID: 29091
		public int cellIdx;
	}

	// Token: 0x02001BC2 RID: 7106
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SolidSubstanceChangeInfo
	{
		// Token: 0x040071A4 RID: 29092
		public int cellIdx;
	}

	// Token: 0x02001BC3 RID: 7107
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SubstanceChangeInfo
	{
		// Token: 0x040071A5 RID: 29093
		public int cellIdx;

		// Token: 0x040071A6 RID: 29094
		public ushort oldElemIdx;

		// Token: 0x040071A7 RID: 29095
		public ushort newElemIdx;
	}

	// Token: 0x02001BC4 RID: 7108
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CallbackInfo
	{
		// Token: 0x040071A8 RID: 29096
		public int callbackIdx;
	}

	// Token: 0x02001BC5 RID: 7109
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GameDataUpdate
	{
		// Token: 0x040071A9 RID: 29097
		public int numFramesProcessed;

		// Token: 0x040071AA RID: 29098
		public unsafe ushort* elementIdx;

		// Token: 0x040071AB RID: 29099
		public unsafe float* temperature;

		// Token: 0x040071AC RID: 29100
		public unsafe float* mass;

		// Token: 0x040071AD RID: 29101
		public unsafe byte* properties;

		// Token: 0x040071AE RID: 29102
		public unsafe byte* insulation;

		// Token: 0x040071AF RID: 29103
		public unsafe byte* strengthInfo;

		// Token: 0x040071B0 RID: 29104
		public unsafe float* radiation;

		// Token: 0x040071B1 RID: 29105
		public unsafe byte* diseaseIdx;

		// Token: 0x040071B2 RID: 29106
		public unsafe int* diseaseCount;

		// Token: 0x040071B3 RID: 29107
		public int numSolidInfo;

		// Token: 0x040071B4 RID: 29108
		public unsafe Sim.SolidInfo* solidInfo;

		// Token: 0x040071B5 RID: 29109
		public int numLiquidChangeInfo;

		// Token: 0x040071B6 RID: 29110
		public unsafe Sim.LiquidChangeInfo* liquidChangeInfo;

		// Token: 0x040071B7 RID: 29111
		public int numSolidSubstanceChangeInfo;

		// Token: 0x040071B8 RID: 29112
		public unsafe Sim.SolidSubstanceChangeInfo* solidSubstanceChangeInfo;

		// Token: 0x040071B9 RID: 29113
		public int numSubstanceChangeInfo;

		// Token: 0x040071BA RID: 29114
		public unsafe Sim.SubstanceChangeInfo* substanceChangeInfo;

		// Token: 0x040071BB RID: 29115
		public int numCallbackInfo;

		// Token: 0x040071BC RID: 29116
		public unsafe Sim.CallbackInfo* callbackInfo;

		// Token: 0x040071BD RID: 29117
		public int numSpawnFallingLiquidInfo;

		// Token: 0x040071BE RID: 29118
		public unsafe Sim.SpawnFallingLiquidInfo* spawnFallingLiquidInfo;

		// Token: 0x040071BF RID: 29119
		public int numDigInfo;

		// Token: 0x040071C0 RID: 29120
		public unsafe Sim.SpawnOreInfo* digInfo;

		// Token: 0x040071C1 RID: 29121
		public int numSpawnOreInfo;

		// Token: 0x040071C2 RID: 29122
		public unsafe Sim.SpawnOreInfo* spawnOreInfo;

		// Token: 0x040071C3 RID: 29123
		public int numSpawnFXInfo;

		// Token: 0x040071C4 RID: 29124
		public unsafe Sim.SpawnFXInfo* spawnFXInfo;

		// Token: 0x040071C5 RID: 29125
		public int numUnstableCellInfo;

		// Token: 0x040071C6 RID: 29126
		public unsafe Sim.UnstableCellInfo* unstableCellInfo;

		// Token: 0x040071C7 RID: 29127
		public int numWorldDamageInfo;

		// Token: 0x040071C8 RID: 29128
		public unsafe Sim.WorldDamageInfo* worldDamageInfo;

		// Token: 0x040071C9 RID: 29129
		public int numBuildingTemperatures;

		// Token: 0x040071CA RID: 29130
		public unsafe Sim.BuildingTemperatureInfo* buildingTemperatures;

		// Token: 0x040071CB RID: 29131
		public int numMassConsumedCallbacks;

		// Token: 0x040071CC RID: 29132
		public unsafe Sim.MassConsumedCallback* massConsumedCallbacks;

		// Token: 0x040071CD RID: 29133
		public int numMassEmittedCallbacks;

		// Token: 0x040071CE RID: 29134
		public unsafe Sim.MassEmittedCallback* massEmittedCallbacks;

		// Token: 0x040071CF RID: 29135
		public int numDiseaseConsumptionCallbacks;

		// Token: 0x040071D0 RID: 29136
		public unsafe Sim.DiseaseConsumptionCallback* diseaseConsumptionCallbacks;

		// Token: 0x040071D1 RID: 29137
		public int numComponentStateChangedMessages;

		// Token: 0x040071D2 RID: 29138
		public unsafe Sim.ComponentStateChangedMessage* componentStateChangedMessages;

		// Token: 0x040071D3 RID: 29139
		public int numRemovedMassEntries;

		// Token: 0x040071D4 RID: 29140
		public unsafe Sim.ConsumedMassInfo* removedMassEntries;

		// Token: 0x040071D5 RID: 29141
		public int numEmittedMassEntries;

		// Token: 0x040071D6 RID: 29142
		public unsafe Sim.EmittedMassInfo* emittedMassEntries;

		// Token: 0x040071D7 RID: 29143
		public int numElementChunkInfos;

		// Token: 0x040071D8 RID: 29144
		public unsafe Sim.ElementChunkInfo* elementChunkInfos;

		// Token: 0x040071D9 RID: 29145
		public int numElementChunkMeltedInfos;

		// Token: 0x040071DA RID: 29146
		public unsafe Sim.MeltedInfo* elementChunkMeltedInfos;

		// Token: 0x040071DB RID: 29147
		public int numBuildingOverheatInfos;

		// Token: 0x040071DC RID: 29148
		public unsafe Sim.MeltedInfo* buildingOverheatInfos;

		// Token: 0x040071DD RID: 29149
		public int numBuildingNoLongerOverheatedInfos;

		// Token: 0x040071DE RID: 29150
		public unsafe Sim.MeltedInfo* buildingNoLongerOverheatedInfos;

		// Token: 0x040071DF RID: 29151
		public int numBuildingMeltedInfos;

		// Token: 0x040071E0 RID: 29152
		public unsafe Sim.MeltedInfo* buildingMeltedInfos;

		// Token: 0x040071E1 RID: 29153
		public int numCellMeltedInfos;

		// Token: 0x040071E2 RID: 29154
		public unsafe Sim.CellMeltedInfo* cellMeltedInfos;

		// Token: 0x040071E3 RID: 29155
		public int numDiseaseEmittedInfos;

		// Token: 0x040071E4 RID: 29156
		public unsafe Sim.DiseaseEmittedInfo* diseaseEmittedInfos;

		// Token: 0x040071E5 RID: 29157
		public int numDiseaseConsumedInfos;

		// Token: 0x040071E6 RID: 29158
		public unsafe Sim.DiseaseConsumedInfo* diseaseConsumedInfos;

		// Token: 0x040071E7 RID: 29159
		public int numRadiationConsumedCallbacks;

		// Token: 0x040071E8 RID: 29160
		public unsafe Sim.ConsumedRadiationCallback* radiationConsumedCallbacks;

		// Token: 0x040071E9 RID: 29161
		public unsafe float* accumulatedFlow;

		// Token: 0x040071EA RID: 29162
		public IntPtr propertyTextureFlow;

		// Token: 0x040071EB RID: 29163
		public IntPtr propertyTextureLiquid;

		// Token: 0x040071EC RID: 29164
		public IntPtr propertyTextureLiquidData;

		// Token: 0x040071ED RID: 29165
		public IntPtr propertyTextureExposedToSunlight;
	}

	// Token: 0x02001BC6 RID: 7110
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFallingLiquidInfo
	{
		// Token: 0x040071EE RID: 29166
		public int cellIdx;

		// Token: 0x040071EF RID: 29167
		public ushort elemIdx;

		// Token: 0x040071F0 RID: 29168
		public byte diseaseIdx;

		// Token: 0x040071F1 RID: 29169
		public byte pad0;

		// Token: 0x040071F2 RID: 29170
		public float mass;

		// Token: 0x040071F3 RID: 29171
		public float temperature;

		// Token: 0x040071F4 RID: 29172
		public int diseaseCount;
	}

	// Token: 0x02001BC7 RID: 7111
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnOreInfo
	{
		// Token: 0x040071F5 RID: 29173
		public int cellIdx;

		// Token: 0x040071F6 RID: 29174
		public ushort elemIdx;

		// Token: 0x040071F7 RID: 29175
		public byte diseaseIdx;

		// Token: 0x040071F8 RID: 29176
		private byte pad0;

		// Token: 0x040071F9 RID: 29177
		public float mass;

		// Token: 0x040071FA RID: 29178
		public float temperature;

		// Token: 0x040071FB RID: 29179
		public int diseaseCount;
	}

	// Token: 0x02001BC8 RID: 7112
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SpawnFXInfo
	{
		// Token: 0x040071FC RID: 29180
		public int cellIdx;

		// Token: 0x040071FD RID: 29181
		public int fxHash;

		// Token: 0x040071FE RID: 29182
		public float rotation;
	}

	// Token: 0x02001BC9 RID: 7113
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct UnstableCellInfo
	{
		// Token: 0x040071FF RID: 29183
		public int cellIdx;

		// Token: 0x04007200 RID: 29184
		public ushort elemIdx;

		// Token: 0x04007201 RID: 29185
		public byte fallingInfo;

		// Token: 0x04007202 RID: 29186
		public byte diseaseIdx;

		// Token: 0x04007203 RID: 29187
		public float mass;

		// Token: 0x04007204 RID: 29188
		public float temperature;

		// Token: 0x04007205 RID: 29189
		public int diseaseCount;

		// Token: 0x02001BCA RID: 7114
		public enum FallingInfo
		{
			// Token: 0x04007207 RID: 29191
			StartedFalling,
			// Token: 0x04007208 RID: 29192
			StoppedFalling
		}
	}

	// Token: 0x02001BCB RID: 7115
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct NewGameFrame
	{
		// Token: 0x04007209 RID: 29193
		public float elapsedSeconds;

		// Token: 0x0400720A RID: 29194
		public int minX;

		// Token: 0x0400720B RID: 29195
		public int minY;

		// Token: 0x0400720C RID: 29196
		public int maxX;

		// Token: 0x0400720D RID: 29197
		public int maxY;

		// Token: 0x0400720E RID: 29198
		public float currentSunlightIntensity;

		// Token: 0x0400720F RID: 29199
		public float currentCosmicRadiationIntensity;
	}

	// Token: 0x02001BCC RID: 7116
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct WorldDamageInfo
	{
		// Token: 0x04007210 RID: 29200
		public int gameCell;

		// Token: 0x04007211 RID: 29201
		public int damageSourceOffset;
	}

	// Token: 0x02001BCD RID: 7117
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct PipeTemperatureChange
	{
		// Token: 0x04007212 RID: 29202
		public int cellIdx;

		// Token: 0x04007213 RID: 29203
		public float temperature;
	}

	// Token: 0x02001BCE RID: 7118
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassConsumedCallback
	{
		// Token: 0x04007214 RID: 29204
		public int callbackIdx;

		// Token: 0x04007215 RID: 29205
		public ushort elemIdx;

		// Token: 0x04007216 RID: 29206
		public byte diseaseIdx;

		// Token: 0x04007217 RID: 29207
		private byte pad0;

		// Token: 0x04007218 RID: 29208
		public float mass;

		// Token: 0x04007219 RID: 29209
		public float temperature;

		// Token: 0x0400721A RID: 29210
		public int diseaseCount;
	}

	// Token: 0x02001BCF RID: 7119
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MassEmittedCallback
	{
		// Token: 0x0400721B RID: 29211
		public int callbackIdx;

		// Token: 0x0400721C RID: 29212
		public ushort elemIdx;

		// Token: 0x0400721D RID: 29213
		public byte suceeded;

		// Token: 0x0400721E RID: 29214
		public byte diseaseIdx;

		// Token: 0x0400721F RID: 29215
		public float mass;

		// Token: 0x04007220 RID: 29216
		public float temperature;

		// Token: 0x04007221 RID: 29217
		public int diseaseCount;
	}

	// Token: 0x02001BD0 RID: 7120
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumptionCallback
	{
		// Token: 0x04007222 RID: 29218
		public int callbackIdx;

		// Token: 0x04007223 RID: 29219
		public byte diseaseIdx;

		// Token: 0x04007224 RID: 29220
		private byte pad0;

		// Token: 0x04007225 RID: 29221
		private byte pad1;

		// Token: 0x04007226 RID: 29222
		private byte pad2;

		// Token: 0x04007227 RID: 29223
		public int diseaseCount;
	}

	// Token: 0x02001BD1 RID: 7121
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ComponentStateChangedMessage
	{
		// Token: 0x04007228 RID: 29224
		public int callbackIdx;

		// Token: 0x04007229 RID: 29225
		public int simHandle;
	}

	// Token: 0x02001BD2 RID: 7122
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DebugProperties
	{
		// Token: 0x0400722A RID: 29226
		public float buildingTemperatureScale;

		// Token: 0x0400722B RID: 29227
		public float buildingToBuildingTemperatureScale;

		// Token: 0x0400722C RID: 29228
		public float biomeTemperatureLerpRate;

		// Token: 0x0400722D RID: 29229
		public byte isDebugEditing;

		// Token: 0x0400722E RID: 29230
		public byte pad0;

		// Token: 0x0400722F RID: 29231
		public byte pad1;

		// Token: 0x04007230 RID: 29232
		public byte pad2;
	}

	// Token: 0x02001BD3 RID: 7123
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct EmittedMassInfo
	{
		// Token: 0x04007231 RID: 29233
		public ushort elemIdx;

		// Token: 0x04007232 RID: 29234
		public byte diseaseIdx;

		// Token: 0x04007233 RID: 29235
		public byte pad0;

		// Token: 0x04007234 RID: 29236
		public float mass;

		// Token: 0x04007235 RID: 29237
		public float temperature;

		// Token: 0x04007236 RID: 29238
		public int diseaseCount;
	}

	// Token: 0x02001BD4 RID: 7124
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedMassInfo
	{
		// Token: 0x04007237 RID: 29239
		public int simHandle;

		// Token: 0x04007238 RID: 29240
		public ushort removedElemIdx;

		// Token: 0x04007239 RID: 29241
		public byte diseaseIdx;

		// Token: 0x0400723A RID: 29242
		private byte pad0;

		// Token: 0x0400723B RID: 29243
		public float mass;

		// Token: 0x0400723C RID: 29244
		public float temperature;

		// Token: 0x0400723D RID: 29245
		public int diseaseCount;
	}

	// Token: 0x02001BD5 RID: 7125
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedDiseaseInfo
	{
		// Token: 0x0400723E RID: 29246
		public int simHandle;

		// Token: 0x0400723F RID: 29247
		public byte diseaseIdx;

		// Token: 0x04007240 RID: 29248
		private byte pad0;

		// Token: 0x04007241 RID: 29249
		private byte pad1;

		// Token: 0x04007242 RID: 29250
		private byte pad2;

		// Token: 0x04007243 RID: 29251
		public int diseaseCount;
	}

	// Token: 0x02001BD6 RID: 7126
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ElementChunkInfo
	{
		// Token: 0x04007244 RID: 29252
		public float temperature;

		// Token: 0x04007245 RID: 29253
		public float deltaKJ;
	}

	// Token: 0x02001BD7 RID: 7127
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct MeltedInfo
	{
		// Token: 0x04007246 RID: 29254
		public int handle;
	}

	// Token: 0x02001BD8 RID: 7128
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct CellMeltedInfo
	{
		// Token: 0x04007247 RID: 29255
		public int gameCell;
	}

	// Token: 0x02001BD9 RID: 7129
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingTemperatureInfo
	{
		// Token: 0x04007248 RID: 29256
		public int handle;

		// Token: 0x04007249 RID: 29257
		public float temperature;
	}

	// Token: 0x02001BDA RID: 7130
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct BuildingConductivityData
	{
		// Token: 0x0400724A RID: 29258
		public float temperature;

		// Token: 0x0400724B RID: 29259
		public float heatCapacity;

		// Token: 0x0400724C RID: 29260
		public float thermalConductivity;
	}

	// Token: 0x02001BDB RID: 7131
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseEmittedInfo
	{
		// Token: 0x0400724D RID: 29261
		public byte diseaseIdx;

		// Token: 0x0400724E RID: 29262
		private byte pad0;

		// Token: 0x0400724F RID: 29263
		private byte pad1;

		// Token: 0x04007250 RID: 29264
		private byte pad2;

		// Token: 0x04007251 RID: 29265
		public int count;
	}

	// Token: 0x02001BDC RID: 7132
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct DiseaseConsumedInfo
	{
		// Token: 0x04007252 RID: 29266
		public byte diseaseIdx;

		// Token: 0x04007253 RID: 29267
		private byte pad0;

		// Token: 0x04007254 RID: 29268
		private byte pad1;

		// Token: 0x04007255 RID: 29269
		private byte pad2;

		// Token: 0x04007256 RID: 29270
		public int count;
	}

	// Token: 0x02001BDD RID: 7133
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ConsumedRadiationCallback
	{
		// Token: 0x04007257 RID: 29271
		public int callbackIdx;

		// Token: 0x04007258 RID: 29272
		public int gameCell;

		// Token: 0x04007259 RID: 29273
		public float radiation;
	}
}
