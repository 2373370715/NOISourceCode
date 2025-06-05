using System;
using STRINGS;

// Token: 0x020003E5 RID: 997
public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x0600104E RID: 4174 RVA: 0x000B17B4 File Offset: 0x000AF9B4
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x1700005B RID: 91
	// (get) Token: 0x0600104F RID: 4175 RVA: 0x000B17B7 File Offset: 0x000AF9B7
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3)
			};
		}
	}

	// Token: 0x1700005C RID: 92
	// (get) Token: 0x06001050 RID: 4176 RVA: 0x000B17CD File Offset: 0x000AF9CD
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x1700005D RID: 93
	// (get) Token: 0x06001051 RID: 4177 RVA: 0x000B180D File Offset: 0x000AFA0D
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(0, 0)
			};
		}
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0018A0BC File Offset: 0x001882BC
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x000B1831 File Offset: 0x000AFA31
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
	}

	// Token: 0x04000B8D RID: 2957
	public const string ID = "LogicGateDemultiplexer";
}
