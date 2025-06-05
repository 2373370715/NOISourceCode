using System;
using UnityEngine;

// Token: 0x0200177E RID: 6014
public class RadiationEmitter : SimComponent
{
	// Token: 0x06007BAE RID: 31662 RVA: 0x000F5C87 File Offset: 0x000F3E87
	protected override void OnSpawn()
	{
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "RadiationEmitter.OnSpawn");
		base.OnSpawn();
	}

	// Token: 0x06007BAF RID: 31663 RVA: 0x000F5CB1 File Offset: 0x000F3EB1
	protected override void OnCleanUp()
	{
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		base.OnCleanUp();
	}

	// Token: 0x06007BB0 RID: 31664 RVA: 0x000E4A5C File Offset: 0x000E2C5C
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x06007BB1 RID: 31665 RVA: 0x000F5CD5 File Offset: 0x000F3ED5
	public int GetEmissionCell()
	{
		return Grid.PosToCell(base.transform.GetPosition() + this.emissionOffset);
	}

	// Token: 0x06007BB2 RID: 31666 RVA: 0x0032B56C File Offset: 0x0032976C
	public void Refresh()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06007BB3 RID: 31667 RVA: 0x000F5CF2 File Offset: 0x000F3EF2
	private void OnCellChange()
	{
		this.Refresh();
	}

	// Token: 0x06007BB4 RID: 31668 RVA: 0x0032B5CC File Offset: 0x003297CC
	private void SetRadiusProportionalToRads()
	{
		this.emitRadiusX = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
		this.emitRadiusY = (short)Mathf.Clamp(Mathf.RoundToInt(this.emitRads * 1f), 1, 128);
	}

	// Token: 0x06007BB5 RID: 31669 RVA: 0x0032B56C File Offset: 0x0032976C
	protected override void OnSimActivate()
	{
		int emissionCell = this.GetEmissionCell();
		if (this.radiusProportionalToRads)
		{
			this.SetRadiusProportionalToRads();
		}
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, this.emitRadiusX, this.emitRadiusY, this.emitRads, this.emitRate, this.emitSpeed, this.emitDirection, this.emitAngle, this.emitType);
	}

	// Token: 0x06007BB6 RID: 31670 RVA: 0x0032B620 File Offset: 0x00329820
	protected override void OnSimDeactivate()
	{
		int emissionCell = this.GetEmissionCell();
		SimMessages.ModifyRadiationEmitter(this.simHandle, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06007BB7 RID: 31671 RVA: 0x0032B664 File Offset: 0x00329864
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		int emissionCell = this.GetEmissionCell();
		SimMessages.AddRadiationEmitter(cb_handle.index, emissionCell, 0, 0, 0f, 0f, 0f, 0f, 0f, this.emitType);
	}

	// Token: 0x06007BB8 RID: 31672 RVA: 0x000F5CFA File Offset: 0x000F3EFA
	protected override void OnSimUnregister()
	{
		RadiationEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06007BB9 RID: 31673 RVA: 0x000F5D07 File Offset: 0x000F3F07
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveRadiationEmitter(-1, sim_handle);
	}

	// Token: 0x06007BBA RID: 31674 RVA: 0x0032B6B8 File Offset: 0x003298B8
	private void OnDrawGizmosSelected()
	{
		int emissionCell = this.GetEmissionCell();
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(emissionCell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06007BBB RID: 31675 RVA: 0x000F5D1B File Offset: 0x000F3F1B
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(RadiationEmitter.StaticUnregister);
	}

	// Token: 0x04005D37 RID: 23863
	public bool radiusProportionalToRads;

	// Token: 0x04005D38 RID: 23864
	[SerializeField]
	public short emitRadiusX = 4;

	// Token: 0x04005D39 RID: 23865
	[SerializeField]
	public short emitRadiusY = 4;

	// Token: 0x04005D3A RID: 23866
	[SerializeField]
	public float emitRads = 10f;

	// Token: 0x04005D3B RID: 23867
	[SerializeField]
	public float emitRate = 1f;

	// Token: 0x04005D3C RID: 23868
	[SerializeField]
	public float emitSpeed = 1f;

	// Token: 0x04005D3D RID: 23869
	[SerializeField]
	public float emitDirection;

	// Token: 0x04005D3E RID: 23870
	[SerializeField]
	public float emitAngle = 360f;

	// Token: 0x04005D3F RID: 23871
	[SerializeField]
	public RadiationEmitter.RadiationEmitterType emitType;

	// Token: 0x04005D40 RID: 23872
	[SerializeField]
	public Vector3 emissionOffset = Vector3.zero;

	// Token: 0x0200177F RID: 6015
	public enum RadiationEmitterType
	{
		// Token: 0x04005D42 RID: 23874
		Constant,
		// Token: 0x04005D43 RID: 23875
		Pulsing,
		// Token: 0x04005D44 RID: 23876
		PulsingAveraged,
		// Token: 0x04005D45 RID: 23877
		SimplePulse,
		// Token: 0x04005D46 RID: 23878
		RadialBeams,
		// Token: 0x04005D47 RID: 23879
		Attractor
	}
}
