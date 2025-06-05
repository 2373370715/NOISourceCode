using System;
using UnityEngine;

// Token: 0x02000E7C RID: 3708
[AddComponentMenu("KMonoBehaviour/scripts/LogicGateBase")]
public class LogicGateBase : KMonoBehaviour
{
	// Token: 0x060048FD RID: 18685 RVA: 0x00265674 File Offset: 0x00263874
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060048FE RID: 18686 RVA: 0x000D3C3F File Offset: 0x000D1E3F
	public int InputCellOne
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[0]);
		}
	}

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x000D3C53 File Offset: 0x000D1E53
	public int InputCellTwo
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[1]);
		}
	}

	// Token: 0x1700039A RID: 922
	// (get) Token: 0x06004900 RID: 18688 RVA: 0x000D3C67 File Offset: 0x000D1E67
	public int InputCellThree
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[2]);
		}
	}

	// Token: 0x1700039B RID: 923
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x000D3C7B File Offset: 0x000D1E7B
	public int InputCellFour
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[3]);
		}
	}

	// Token: 0x1700039C RID: 924
	// (get) Token: 0x06004902 RID: 18690 RVA: 0x000D3C8F File Offset: 0x000D1E8F
	public int OutputCellOne
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[0]);
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x000D3CA3 File Offset: 0x000D1EA3
	public int OutputCellTwo
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[1]);
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06004904 RID: 18692 RVA: 0x000D3CB7 File Offset: 0x000D1EB7
	public int OutputCellThree
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[2]);
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06004905 RID: 18693 RVA: 0x000D3CCB File Offset: 0x000D1ECB
	public int OutputCellFour
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[3]);
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06004906 RID: 18694 RVA: 0x000D3CDF File Offset: 0x000D1EDF
	public int ControlCellOne
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[0]);
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06004907 RID: 18695 RVA: 0x000D3CF3 File Offset: 0x000D1EF3
	public int ControlCellTwo
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[1]);
		}
	}

	// Token: 0x06004908 RID: 18696 RVA: 0x002656B0 File Offset: 0x002638B0
	public int PortCell(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.InputCellOne;
		case LogicGateBase.PortId.InputTwo:
			return this.InputCellTwo;
		case LogicGateBase.PortId.InputThree:
			return this.InputCellThree;
		case LogicGateBase.PortId.InputFour:
			return this.InputCellFour;
		case LogicGateBase.PortId.OutputOne:
			return this.OutputCellOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.OutputCellTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.OutputCellThree;
		case LogicGateBase.PortId.OutputFour:
			return this.OutputCellFour;
		case LogicGateBase.PortId.ControlOne:
			return this.ControlCellOne;
		case LogicGateBase.PortId.ControlTwo:
			return this.ControlCellTwo;
		default:
			return this.OutputCellOne;
		}
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x0026573C File Offset: 0x0026393C
	public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
	{
		if (cell == this.InputCellOne)
		{
			port = LogicGateBase.PortId.InputOne;
			return true;
		}
		if ((this.RequiresTwoInputs || this.RequiresFourInputs) && cell == this.InputCellTwo)
		{
			port = LogicGateBase.PortId.InputTwo;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellThree)
		{
			port = LogicGateBase.PortId.InputThree;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellFour)
		{
			port = LogicGateBase.PortId.InputFour;
			return true;
		}
		if (cell == this.OutputCellOne)
		{
			port = LogicGateBase.PortId.OutputOne;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellTwo)
		{
			port = LogicGateBase.PortId.OutputTwo;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellThree)
		{
			port = LogicGateBase.PortId.OutputThree;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellFour)
		{
			port = LogicGateBase.PortId.OutputFour;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellOne)
		{
			port = LogicGateBase.PortId.ControlOne;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellTwo)
		{
			port = LogicGateBase.PortId.ControlTwo;
			return true;
		}
		port = LogicGateBase.PortId.InputOne;
		return false;
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x0600490A RID: 18698 RVA: 0x000D3D07 File Offset: 0x000D1F07
	public bool RequiresTwoInputs
	{
		get
		{
			return LogicGateBase.OpRequiresTwoInputs(this.op);
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x0600490B RID: 18699 RVA: 0x000D3D14 File Offset: 0x000D1F14
	public bool RequiresFourInputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourInputs(this.op);
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x0600490C RID: 18700 RVA: 0x000D3D21 File Offset: 0x000D1F21
	public bool RequiresFourOutputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourOutputs(this.op);
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x0600490D RID: 18701 RVA: 0x000D3D2E File Offset: 0x000D1F2E
	public bool RequiresControlInputs
	{
		get
		{
			return LogicGateBase.OpRequiresControlInputs(this.op);
		}
	}

	// Token: 0x0600490E RID: 18702 RVA: 0x000D3D3B File Offset: 0x000D1F3B
	public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
	{
		return op != LogicGateBase.Op.Not && op - LogicGateBase.Op.CustomSingle > 2;
	}

	// Token: 0x0600490F RID: 18703 RVA: 0x000D3D4A File Offset: 0x000D1F4A
	public static bool OpRequiresFourInputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x06004910 RID: 18704 RVA: 0x000D3D53 File Offset: 0x000D1F53
	public static bool OpRequiresFourOutputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x06004911 RID: 18705 RVA: 0x000D3D5C File Offset: 0x000D1F5C
	public static bool OpRequiresControlInputs(LogicGateBase.Op op)
	{
		return op - LogicGateBase.Op.Multiplexer <= 1;
	}

	// Token: 0x0400332B RID: 13099
	public static LogicModeUI uiSrcData;

	// Token: 0x0400332C RID: 13100
	public static readonly HashedString OUTPUT_TWO_PORT_ID = new HashedString("LogicGateOutputTwo");

	// Token: 0x0400332D RID: 13101
	public static readonly HashedString OUTPUT_THREE_PORT_ID = new HashedString("LogicGateOutputThree");

	// Token: 0x0400332E RID: 13102
	public static readonly HashedString OUTPUT_FOUR_PORT_ID = new HashedString("LogicGateOutputFour");

	// Token: 0x0400332F RID: 13103
	[SerializeField]
	public LogicGateBase.Op op;

	// Token: 0x04003330 RID: 13104
	public static CellOffset[] portOffsets = new CellOffset[]
	{
		CellOffset.none,
		new CellOffset(0, 1),
		new CellOffset(1, 0)
	};

	// Token: 0x04003331 RID: 13105
	public CellOffset[] inputPortOffsets;

	// Token: 0x04003332 RID: 13106
	public CellOffset[] outputPortOffsets;

	// Token: 0x04003333 RID: 13107
	public CellOffset[] controlPortOffsets;

	// Token: 0x02000E7D RID: 3709
	public enum PortId
	{
		// Token: 0x04003335 RID: 13109
		InputOne,
		// Token: 0x04003336 RID: 13110
		InputTwo,
		// Token: 0x04003337 RID: 13111
		InputThree,
		// Token: 0x04003338 RID: 13112
		InputFour,
		// Token: 0x04003339 RID: 13113
		OutputOne,
		// Token: 0x0400333A RID: 13114
		OutputTwo,
		// Token: 0x0400333B RID: 13115
		OutputThree,
		// Token: 0x0400333C RID: 13116
		OutputFour,
		// Token: 0x0400333D RID: 13117
		ControlOne,
		// Token: 0x0400333E RID: 13118
		ControlTwo
	}

	// Token: 0x02000E7E RID: 3710
	public enum Op
	{
		// Token: 0x04003340 RID: 13120
		And,
		// Token: 0x04003341 RID: 13121
		Or,
		// Token: 0x04003342 RID: 13122
		Not,
		// Token: 0x04003343 RID: 13123
		Xor,
		// Token: 0x04003344 RID: 13124
		CustomSingle,
		// Token: 0x04003345 RID: 13125
		Multiplexer,
		// Token: 0x04003346 RID: 13126
		Demultiplexer
	}
}
