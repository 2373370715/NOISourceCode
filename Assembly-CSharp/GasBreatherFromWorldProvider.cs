using System;

// Token: 0x020013A7 RID: 5031
public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
	// Token: 0x06006713 RID: 26387 RVA: 0x000E7B1A File Offset: 0x000E5D1A
	public GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAtCurrentLocation()
	{
		return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(Grid.PosToCell(this.oxygenBreather), GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS, this.oxygenBreather);
	}

	// Token: 0x06006714 RID: 26388 RVA: 0x002E017C File Offset: 0x002DE37C
	public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(int theSpecificCell, CellOffset[] breathRange, OxygenBreather breather)
	{
		float num;
		return GasBreatherFromWorldProvider.GetBestBreathableCellAroundSpecificCell(theSpecificCell, breathRange, breather, out num);
	}

	// Token: 0x06006715 RID: 26389 RVA: 0x002E0194 File Offset: 0x002DE394
	public static GasBreatherFromWorldProvider.BreathableCellData GetBestBreathableCellAroundSpecificCell(int theSpecificCell, CellOffset[] breathRange, OxygenBreather breather, out float totalBreathableMassAroundCell)
	{
		if (breathRange == null)
		{
			breathRange = GasBreatherFromWorldProvider.DEFAULT_BREATHABLE_OFFSETS;
		}
		float num = 0f;
		int cell = theSpecificCell;
		SimHashes simHashes = SimHashes.Vacuum;
		totalBreathableMassAroundCell = 0f;
		foreach (CellOffset offset in breathRange)
		{
			int num2 = Grid.OffsetCell(theSpecificCell, offset);
			SimHashes simHashes2;
			float breathableCellMass = GasBreatherFromWorldProvider.GetBreathableCellMass(num2, out simHashes2);
			totalBreathableMassAroundCell += breathableCellMass;
			if (breathableCellMass > num && breathableCellMass > breather.noOxygenThreshold)
			{
				num = breathableCellMass;
				cell = num2;
				simHashes = simHashes2;
			}
		}
		return new GasBreatherFromWorldProvider.BreathableCellData
		{
			Cell = cell,
			ElementID = simHashes,
			Mass = num,
			IsBreathable = (simHashes != SimHashes.Vacuum)
		};
	}

	// Token: 0x06006716 RID: 26390 RVA: 0x002E0248 File Offset: 0x002DE448
	private static float GetBreathableCellMass(int cell, out SimHashes elementID)
	{
		elementID = SimHashes.Vacuum;
		if (Grid.IsValidCell(cell))
		{
			Element element = Grid.Element[cell];
			if (element.HasTag(GameTags.Breathable))
			{
				elementID = element.id;
				return Grid.Mass[cell];
			}
		}
		return 0f;
	}

	// Token: 0x06006717 RID: 26391 RVA: 0x000E7B37 File Offset: 0x000E5D37
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
		this.oxygenBreather = oxygen_breather;
		this.nav = this.oxygenBreather.GetComponent<Navigator>();
	}

	// Token: 0x06006718 RID: 26392 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06006719 RID: 26393 RVA: 0x000E7B51 File Offset: 0x000E5D51
	public bool ShouldEmitCO2()
	{
		return this.nav.CurrentNavType != NavType.Tube;
	}

	// Token: 0x0600671A RID: 26394 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x0600671B RID: 26395 RVA: 0x002E0294 File Offset: 0x002DE494
	public bool IsLowOxygen()
	{
		GasBreatherFromWorldProvider.BreathableCellData bestBreathableCellAtCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
		return bestBreathableCellAtCurrentLocation.IsBreathable && bestBreathableCellAtCurrentLocation.Mass < this.oxygenBreather.lowOxygenThreshold;
	}

	// Token: 0x0600671C RID: 26396 RVA: 0x000E7B64 File Offset: 0x000E5D64
	public bool HasOxygen()
	{
		return this.oxygenBreather.prefabID.HasTag(GameTags.RecoveringBreath) || this.oxygenBreather.prefabID.HasTag(GameTags.InTransitTube) || this.GetBestBreathableCellAtCurrentLocation().IsBreathable;
	}

	// Token: 0x0600671D RID: 26397 RVA: 0x000E7BA1 File Offset: 0x000E5DA1
	public bool IsBlocked()
	{
		return this.oxygenBreather.HasTag(GameTags.HasSuitTank);
	}

	// Token: 0x0600671E RID: 26398 RVA: 0x002E02C8 File Offset: 0x002DE4C8
	public bool ConsumeGas(OxygenBreather oxygen_breather, float mass_to_consume)
	{
		if (this.nav.CurrentNavType != NavType.Tube)
		{
			GasBreatherFromWorldProvider.BreathableCellData bestBreathableCellAtCurrentLocation = this.GetBestBreathableCellAtCurrentLocation();
			if (!bestBreathableCellAtCurrentLocation.IsBreathable)
			{
				return false;
			}
			SimHashes elementID = bestBreathableCellAtCurrentLocation.ElementID;
			HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback), oxygen_breather, "GasBreatherFromWorldProvider");
			SimMessages.ConsumeMass(bestBreathableCellAtCurrentLocation.Cell, elementID, mass_to_consume, 3, handle.index);
		}
		return true;
	}

	// Token: 0x0600671F RID: 26399 RVA: 0x002E0334 File Offset: 0x002DE534
	private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
	{
		SimHashes id = ElementLoader.elements[(int)mass_cb_info.elemIdx].id;
		OxygenBreather.BreathableGasConsumed(data as OxygenBreather, id, mass_cb_info.mass, mass_cb_info.temperature, mass_cb_info.diseaseIdx, mass_cb_info.diseaseCount);
	}

	// Token: 0x04004DD2 RID: 19922
	public static CellOffset[] DEFAULT_BREATHABLE_OFFSETS = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(0, 1),
		new CellOffset(1, 1),
		new CellOffset(-1, 1),
		new CellOffset(1, 0),
		new CellOffset(-1, 0)
	};

	// Token: 0x04004DD3 RID: 19923
	private OxygenBreather oxygenBreather;

	// Token: 0x04004DD4 RID: 19924
	private Navigator nav;

	// Token: 0x020013A8 RID: 5032
	public struct BreathableCellData
	{
		// Token: 0x04004DD5 RID: 19925
		public int Cell;

		// Token: 0x04004DD6 RID: 19926
		public SimHashes ElementID;

		// Token: 0x04004DD7 RID: 19927
		public float Mass;

		// Token: 0x04004DD8 RID: 19928
		public bool IsBreathable;
	}
}
