using System;
using STRINGS;

// Token: 0x020003E0 RID: 992
public class LogicGateXorConfig : LogicGateBaseConfig
{
	// Token: 0x06001027 RID: 4135 RVA: 0x000B1693 File Offset: 0x000AF893
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Xor;
	}

	// Token: 0x1700004C RID: 76
	// (get) Token: 0x06001028 RID: 4136 RVA: 0x000B162B File Offset: 0x000AF82B
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

	// Token: 0x1700004D RID: 77
	// (get) Token: 0x06001029 RID: 4137 RVA: 0x000B164D File Offset: 0x000AF84D
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

	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600102A RID: 4138 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600102B RID: 4139 RVA: 0x0018A0BC File Offset: 0x001882BC
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

	// Token: 0x0600102C RID: 4140 RVA: 0x000B1696 File Offset: 0x000AF896
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim", 2, 2);
	}

	// Token: 0x04000B88 RID: 2952
	public const string ID = "LogicGateXOR";
}
