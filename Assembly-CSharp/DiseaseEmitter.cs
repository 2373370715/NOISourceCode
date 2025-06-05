using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x0200127E RID: 4734
[AddComponentMenu("KMonoBehaviour/scripts/DiseaseEmitter")]
public class DiseaseEmitter : KMonoBehaviour
{
	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x060060A1 RID: 24737 RVA: 0x000E3584 File Offset: 0x000E1784
	public float EmitRate
	{
		get
		{
			return this.emitRate;
		}
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x002BCFCC File Offset: 0x002BB1CC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.emitDiseases != null)
		{
			this.simHandles = new int[this.emitDiseases.Length];
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				this.simHandles[i] = -1;
			}
		}
		this.SimRegister();
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x000E358C File Offset: 0x000E178C
	protected override void OnCleanUp()
	{
		this.SimUnregister();
		base.OnCleanUp();
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x000E359A File Offset: 0x000E179A
	public void SetEnable(bool enable)
	{
		if (this.enableEmitter == enable)
		{
			return;
		}
		this.enableEmitter = enable;
		if (this.enableEmitter)
		{
			this.SimRegister();
			return;
		}
		this.SimUnregister();
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x002BD01C File Offset: 0x002BB21C
	private void OnCellChanged()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		int cell = Grid.PosToCell(this);
		if (Grid.IsValidCell(cell))
		{
			for (int i = 0; i < this.emitDiseases.Length; i++)
			{
				if (Sim.IsValidHandle(this.simHandles[i]))
				{
					SimMessages.ModifyDiseaseEmitter(this.simHandles[i], cell, this.emitRange, this.emitDiseases[i], this.emitRate, this.emitCount);
				}
			}
		}
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x002BD094 File Offset: 0x002BB294
	private void SimRegister()
	{
		if (this.simHandles == null || !this.enableEmitter)
		{
			return;
		}
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "DiseaseEmitter.Modify");
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (this.simHandles[i] == -1)
			{
				this.simHandles[i] = -2;
				SimMessages.AddDiseaseEmitter(Game.Instance.simComponentCallbackManager.Add(new Action<int, object>(DiseaseEmitter.OnSimRegisteredCallback), this, "DiseaseEmitter").index);
			}
		}
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x002BD12C File Offset: 0x002BB32C
	private void SimUnregister()
	{
		if (this.simHandles == null)
		{
			return;
		}
		for (int i = 0; i < this.simHandles.Length; i++)
		{
			if (Sim.IsValidHandle(this.simHandles[i]))
			{
				SimMessages.RemoveDiseaseEmitter(-1, this.simHandles[i]);
			}
			this.simHandles[i] = -1;
		}
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x000E35C2 File Offset: 0x000E17C2
	private static void OnSimRegisteredCallback(int handle, object data)
	{
		((DiseaseEmitter)data).OnSimRegistered(handle);
	}

	// Token: 0x060060A9 RID: 24745 RVA: 0x002BD198 File Offset: 0x002BB398
	private void OnSimRegistered(int handle)
	{
		bool flag = false;
		if (this != null)
		{
			for (int i = 0; i < this.simHandles.Length; i++)
			{
				if (this.simHandles[i] == -2)
				{
					this.simHandles[i] = handle;
					flag = true;
					break;
				}
			}
			this.OnCellChanged();
		}
		if (!flag)
		{
			SimMessages.RemoveDiseaseEmitter(-1, handle);
		}
	}

	// Token: 0x060060AA RID: 24746 RVA: 0x002BD1EC File Offset: 0x002BB3EC
	public void SetDiseases(List<Disease> diseases)
	{
		this.emitDiseases = new byte[diseases.Count];
		for (int i = 0; i < diseases.Count; i++)
		{
			this.emitDiseases[i] = Db.Get().Diseases.GetIndex(diseases[i].id);
		}
	}

	// Token: 0x0400450F RID: 17679
	[Serialize]
	public float emitRate = 1f;

	// Token: 0x04004510 RID: 17680
	[Serialize]
	public byte emitRange;

	// Token: 0x04004511 RID: 17681
	[Serialize]
	public int emitCount;

	// Token: 0x04004512 RID: 17682
	[Serialize]
	public byte[] emitDiseases;

	// Token: 0x04004513 RID: 17683
	public int[] simHandles;

	// Token: 0x04004514 RID: 17684
	[Serialize]
	private bool enableEmitter;
}
