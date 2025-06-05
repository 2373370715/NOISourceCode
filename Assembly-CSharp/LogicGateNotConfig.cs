using System;
using STRINGS;

// Token: 0x020003E1 RID: 993
public class LogicGateNotConfig : LogicGateBaseConfig
{
	// Token: 0x0600102E RID: 4142 RVA: 0x000AA7FE File Offset: 0x000A89FE
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Not;
	}

	// Token: 0x1700004F RID: 79
	// (get) Token: 0x0600102F RID: 4143 RVA: 0x000B16AA File Offset: 0x000AF8AA
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000050 RID: 80
	// (get) Token: 0x06001030 RID: 4144 RVA: 0x000B164D File Offset: 0x000AF84D
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

	// Token: 0x17000051 RID: 81
	// (get) Token: 0x06001031 RID: 4145 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001032 RID: 4146 RVA: 0x0018A10C File Offset: 0x0018830C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001033 RID: 4147 RVA: 0x000B16BE File Offset: 0x000AF8BE
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
	}

	// Token: 0x04000B89 RID: 2953
	public const string ID = "LogicGateNOT";
}
