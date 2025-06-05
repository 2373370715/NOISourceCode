using System;
using UnityEngine;

// Token: 0x020012C8 RID: 4808
public class ElementEmitter : SimComponent
{
	// Token: 0x17000609 RID: 1545
	// (get) Token: 0x06006268 RID: 25192 RVA: 0x000E4A23 File Offset: 0x000E2C23
	// (set) Token: 0x06006269 RID: 25193 RVA: 0x000E4A2B File Offset: 0x000E2C2B
	public bool isEmitterBlocked { get; private set; }

	// Token: 0x0600626A RID: 25194 RVA: 0x002C5128 File Offset: 0x002C3328
	protected override void OnSpawn()
	{
		this.onBlockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterBlocked), true));
		this.onUnblockedHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnEmitterUnblocked), true));
		base.OnSpawn();
	}

	// Token: 0x0600626B RID: 25195 RVA: 0x000E4A34 File Offset: 0x000E2C34
	protected override void OnCleanUp()
	{
		Game.Instance.ManualReleaseHandle(this.onBlockedHandle);
		Game.Instance.ManualReleaseHandle(this.onUnblockedHandle);
		base.OnCleanUp();
	}

	// Token: 0x0600626C RID: 25196 RVA: 0x000E4A5C File Offset: 0x000E2C5C
	public void SetEmitting(bool emitting)
	{
		base.SetSimActive(emitting);
	}

	// Token: 0x0600626D RID: 25197 RVA: 0x002C518C File Offset: 0x002C338C
	protected override void OnSimActivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		if (this.outputElement.elementHash != (SimHashes)0 && this.outputElement.massGenerationRate > 0f && this.emissionFrequency > 0f)
		{
			float emit_temperature = (this.outputElement.minOutputTemperature == 0f) ? base.GetComponent<PrimaryElement>().Temperature : this.outputElement.minOutputTemperature;
			SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, this.outputElement.elementHash, this.emissionFrequency, this.outputElement.massGenerationRate, emit_temperature, this.maxPressure, this.outputElement.addedDiseaseIdx, this.outputElement.addedDiseaseCount);
		}
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().ReplaceStatusItem(this.statusHandle, Db.Get().BuildingStatusItems.ElementEmitterOutput, this);
		}
	}

	// Token: 0x0600626E RID: 25198 RVA: 0x002C52A8 File Offset: 0x002C34A8
	protected override void OnSimDeactivate()
	{
		int game_cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		SimMessages.ModifyElementEmitter(this.simHandle, game_cell, (int)this.emitRange, SimHashes.Vacuum, 0f, 0f, 0f, 0f, byte.MaxValue, 0);
		if (this.showDescriptor)
		{
			this.statusHandle = base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x0600626F RID: 25199 RVA: 0x002C5340 File Offset: 0x002C3540
	public void ForceEmit(float mass, byte disease_idx, int disease_count, float temperature = -1f)
	{
		if (mass <= 0f)
		{
			return;
		}
		float temperature2 = (temperature > 0f) ? temperature : this.outputElement.minOutputTemperature;
		Element element = ElementLoader.FindElementByHash(this.outputElement.elementHash);
		if (element.IsGas || element.IsLiquid)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(base.transform.GetPosition()), this.outputElement.elementHash, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature2, disease_idx, disease_count, true, -1);
		}
		else if (element.IsSolid)
		{
			element.substance.SpawnResource(base.transform.GetPosition() + new Vector3(0f, 0.5f, 0f), mass, temperature2, disease_idx, disease_count, false, true, false);
		}
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(this.outputElement.elementHash).name, base.gameObject.transform, 1.5f, false);
	}

	// Token: 0x06006270 RID: 25200 RVA: 0x000E4A65 File Offset: 0x000E2C65
	private void OnEmitterBlocked()
	{
		this.isEmitterBlocked = true;
		base.Trigger(1615168894, this);
	}

	// Token: 0x06006271 RID: 25201 RVA: 0x000E4A7A File Offset: 0x000E2C7A
	private void OnEmitterUnblocked()
	{
		this.isEmitterBlocked = false;
		base.Trigger(-657992955, this);
	}

	// Token: 0x06006272 RID: 25202 RVA: 0x000E4A8F File Offset: 0x000E2C8F
	protected override void OnSimRegister(HandleVector<Game.ComplexCallbackInfo<int>>.Handle cb_handle)
	{
		Game.Instance.simComponentCallbackManager.GetItem(cb_handle);
		SimMessages.AddElementEmitter(this.maxPressure, cb_handle.index, this.onBlockedHandle.index, this.onUnblockedHandle.index);
	}

	// Token: 0x06006273 RID: 25203 RVA: 0x000E4ACA File Offset: 0x000E2CCA
	protected override void OnSimUnregister()
	{
		ElementEmitter.StaticUnregister(this.simHandle);
	}

	// Token: 0x06006274 RID: 25204 RVA: 0x000E4AD7 File Offset: 0x000E2CD7
	private static void StaticUnregister(int sim_handle)
	{
		global::Debug.Assert(Sim.IsValidHandle(sim_handle));
		SimMessages.RemoveElementEmitter(-1, sim_handle);
	}

	// Token: 0x06006275 RID: 25205 RVA: 0x002C543C File Offset: 0x002C363C
	private void OnDrawGizmosSelected()
	{
		int cell = Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), (int)this.outputElement.outputElementOffset.x, (int)this.outputElement.outputElementOffset.y);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(Grid.CellToPos(cell) + Vector3.right / 2f + Vector3.up / 2f, 0.2f);
	}

	// Token: 0x06006276 RID: 25206 RVA: 0x000E4AEB File Offset: 0x000E2CEB
	protected override Action<int> GetStaticUnregister()
	{
		return new Action<int>(ElementEmitter.StaticUnregister);
	}

	// Token: 0x040046A3 RID: 18083
	[SerializeField]
	public ElementConverter.OutputElement outputElement;

	// Token: 0x040046A4 RID: 18084
	[SerializeField]
	public float emissionFrequency = 1f;

	// Token: 0x040046A5 RID: 18085
	[SerializeField]
	public byte emitRange = 1;

	// Token: 0x040046A6 RID: 18086
	[SerializeField]
	public float maxPressure = 1f;

	// Token: 0x040046A7 RID: 18087
	private Guid statusHandle = Guid.Empty;

	// Token: 0x040046A8 RID: 18088
	public bool showDescriptor = true;

	// Token: 0x040046A9 RID: 18089
	private HandleVector<Game.CallbackInfo>.Handle onBlockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;

	// Token: 0x040046AA RID: 18090
	private HandleVector<Game.CallbackInfo>.Handle onUnblockedHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
}
