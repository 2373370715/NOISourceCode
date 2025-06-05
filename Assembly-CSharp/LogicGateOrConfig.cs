using System;
using STRINGS;

// Token: 0x020003DF RID: 991
public class LogicGateOrConfig : LogicGateBaseConfig
{
	// Token: 0x06001020 RID: 4128 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Or;
	}

	// Token: 0x17000049 RID: 73
	// (get) Token: 0x06001021 RID: 4129 RVA: 0x000B162B File Offset: 0x000AF82B
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}
	}

	// Token: 0x1700004A RID: 74
	// (get) Token: 0x06001022 RID: 4130 RVA: 0x000B164D File Offset: 0x000AF84D
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x1700004B RID: 75
	// (get) Token: 0x06001023 RID: 4131 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001024 RID: 4132 RVA: 0x0018A06C File Offset: 0x0018826C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001025 RID: 4133 RVA: 0x000B167F File Offset: 0x000AF87F
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateOR", "logic_or_kanim", 2, 2);
	}

	// Token: 0x04000B87 RID: 2951
	public const string ID = "LogicGateOR";
}
