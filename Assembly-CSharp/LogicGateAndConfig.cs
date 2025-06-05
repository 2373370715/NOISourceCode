using System;
using STRINGS;

// Token: 0x020003DE RID: 990
public class LogicGateAndConfig : LogicGateBaseConfig
{
	// Token: 0x06001019 RID: 4121 RVA: 0x000B1628 File Offset: 0x000AF828
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.And;
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x0600101A RID: 4122 RVA: 0x000B162B File Offset: 0x000AF82B
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

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x0600101B RID: 4123 RVA: 0x000B164D File Offset: 0x000AF84D
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

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600101C RID: 4124 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600101D RID: 4125 RVA: 0x0018A01C File Offset: 0x0018821C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x0600101E RID: 4126 RVA: 0x000B1663 File Offset: 0x000AF863
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
	}

	// Token: 0x04000B86 RID: 2950
	public const string ID = "LogicGateAND";
}
