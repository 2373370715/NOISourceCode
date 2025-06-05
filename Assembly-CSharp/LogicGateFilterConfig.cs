using System;
using STRINGS;
using UnityEngine;

// Token: 0x020003E3 RID: 995
public class LogicGateFilterConfig : LogicGateBaseConfig
{
	// Token: 0x0600103E RID: 4158 RVA: 0x000B16D2 File Offset: 0x000AF8D2
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000055 RID: 85
	// (get) Token: 0x0600103F RID: 4159 RVA: 0x000B16AA File Offset: 0x000AF8AA
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

	// Token: 0x17000056 RID: 86
	// (get) Token: 0x06001040 RID: 4160 RVA: 0x000B164D File Offset: 0x000AF84D
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

	// Token: 0x17000057 RID: 87
	// (get) Token: 0x06001041 RID: 4161 RVA: 0x000AA765 File Offset: 0x000A8965
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001042 RID: 4162 RVA: 0x0018A218 File Offset: 0x00188418
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06001043 RID: 4163 RVA: 0x000B16FC File Offset: 0x000AF8FC
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", 2, 1);
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0018A268 File Offset: 0x00188468
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
		logicGateFilter.op = this.GetLogicOp();
		logicGateFilter.inputPortOffsets = this.InputPortOffsets;
		logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
		logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000B8B RID: 2955
	public const string ID = "LogicGateFILTER";
}
