using System;
using STRINGS;
using UnityEngine;

// Token: 0x020003E2 RID: 994
public class LogicGateBufferConfig : LogicGateBaseConfig
{
	// Token: 0x06001035 RID: 4149 RVA: 0x000B16D2 File Offset: 0x000AF8D2
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x06001036 RID: 4150 RVA: 0x000B16AA File Offset: 0x000AF8AA
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

	// Token: 0x17000053 RID: 83
	// (get) Token: 0x06001037 RID: 4151 RVA: 0x000B164D File Offset: 0x000AF84D
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

	// Token: 0x17000054 RID: 84
	// (get) Token: 0x06001038 RID: 4152 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001039 RID: 4153 RVA: 0x0018A15C File Offset: 0x0018835C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x000B16D5 File Offset: 0x000AF8D5
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x0018A1AC File Offset: 0x001883AC
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
		logicGateBuffer.op = this.GetLogicOp();
		logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
		logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
		logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000B8A RID: 2954
	public const string ID = "LogicGateBUFFER";
}
