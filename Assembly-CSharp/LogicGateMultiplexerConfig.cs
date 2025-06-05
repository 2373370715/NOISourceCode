using System;
using STRINGS;

// Token: 0x020003E4 RID: 996
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06001047 RID: 4167 RVA: 0x000B1723 File Offset: 0x000AF923
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x17000058 RID: 88
	// (get) Token: 0x06001048 RID: 4168 RVA: 0x000B1726 File Offset: 0x000AF926
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3),
				new CellOffset(-1, 2),
				new CellOffset(-1, 1),
				new CellOffset(-1, 0)
			};
		}
	}

	// Token: 0x17000059 RID: 89
	// (get) Token: 0x06001049 RID: 4169 RVA: 0x000B1766 File Offset: 0x000AF966
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3)
			};
		}
	}

	// Token: 0x1700005A RID: 90
	// (get) Token: 0x0600104A RID: 4170 RVA: 0x000B177C File Offset: 0x000AF97C
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0018A2D4 File Offset: 0x001884D4
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x000B17A0 File Offset: 0x000AF9A0
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
	}

	// Token: 0x04000B8C RID: 2956
	public const string ID = "LogicGateMultiplexer";
}
