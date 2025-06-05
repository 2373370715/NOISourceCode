using System;
using UnityEngine;

// Token: 0x02001310 RID: 4880
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Exhaust")]
public class Exhaust : KMonoBehaviour, ISim200ms
{
	// Token: 0x060063F5 RID: 25589 RVA: 0x000E5B68 File Offset: 0x000E3D68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Exhaust>(-592767678, Exhaust.OnConduitStateChangedDelegate);
		base.Subscribe<Exhaust>(-111137758, Exhaust.OnConduitStateChangedDelegate);
		base.GetComponent<RequireInputs>().visualizeRequirements = RequireInputs.Requirements.NoWire;
		this.simRenderLoadBalance = true;
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x000E5BA5 File Offset: 0x000E3DA5
	protected override void OnSpawn()
	{
		this.OnConduitStateChanged(null);
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x000E5BAE File Offset: 0x000E3DAE
	private void OnConduitStateChanged(object data)
	{
		this.operational.SetActive(this.operational.IsOperational && !this.vent.IsBlocked, false);
	}

	// Token: 0x060063F8 RID: 25592 RVA: 0x000E5BDA File Offset: 0x000E3DDA
	private void CalculateDiseaseTransfer(PrimaryElement item1, PrimaryElement item2, float transfer_rate, out int disease_to_item1, out int disease_to_item2)
	{
		disease_to_item1 = (int)((float)item2.DiseaseCount * transfer_rate);
		disease_to_item2 = (int)((float)item1.DiseaseCount * transfer_rate);
	}

	// Token: 0x060063F9 RID: 25593 RVA: 0x002CA4B0 File Offset: 0x002C86B0
	public void Sim200ms(float dt)
	{
		this.operational.SetFlag(Exhaust.canExhaust, !this.vent.IsBlocked);
		if (!this.operational.IsOperational)
		{
			if (this.isAnimating)
			{
				this.isAnimating = false;
				this.recentlyExhausted = false;
				base.Trigger(-793429877, null);
			}
			return;
		}
		this.UpdateEmission();
		this.elapsedSwitchTime -= dt;
		if (this.elapsedSwitchTime <= 0f)
		{
			this.elapsedSwitchTime = 1f;
			if (this.recentlyExhausted != this.isAnimating)
			{
				this.isAnimating = this.recentlyExhausted;
				base.Trigger(-793429877, null);
			}
			this.recentlyExhausted = false;
		}
	}

	// Token: 0x060063FA RID: 25594 RVA: 0x000E5BF6 File Offset: 0x000E3DF6
	public bool IsAnimating()
	{
		return this.isAnimating;
	}

	// Token: 0x060063FB RID: 25595 RVA: 0x002CA564 File Offset: 0x002C8764
	private void UpdateEmission()
	{
		if (this.consumer.ConsumptionRate == 0f)
		{
			return;
		}
		if (this.storage.items.Count == 0)
		{
			return;
		}
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (Grid.Solid[num])
		{
			return;
		}
		ConduitType typeOfConduit = this.consumer.TypeOfConduit;
		if (typeOfConduit != ConduitType.Gas)
		{
			if (typeOfConduit == ConduitType.Liquid)
			{
				this.EmitLiquid(num);
				return;
			}
		}
		else
		{
			this.EmitGas(num);
		}
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x002CA5DC File Offset: 0x002C87DC
	private bool EmitCommon(int cell, PrimaryElement primary_element, Exhaust.EmitDelegate emit)
	{
		if (primary_element.Mass <= 0f)
		{
			return false;
		}
		int num;
		int num2;
		this.CalculateDiseaseTransfer(this.exhaustPE, primary_element, 0.05f, out num, out num2);
		primary_element.ModifyDiseaseCount(-num, "Exhaust transfer");
		primary_element.AddDisease(this.exhaustPE.DiseaseIdx, num2, "Exhaust transfer");
		this.exhaustPE.ModifyDiseaseCount(-num2, "Exhaust transfer");
		this.exhaustPE.AddDisease(primary_element.DiseaseIdx, num, "Exhaust transfer");
		emit(cell, primary_element);
		if (this.vent != null)
		{
			this.vent.UpdateVentedMass(primary_element.ElementID, primary_element.Mass);
		}
		primary_element.KeepZeroMassObject = true;
		primary_element.Mass = 0f;
		primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
		if (this.lastElementEmmited != primary_element.ElementID)
		{
			this.lastElementEmmited = primary_element.ElementID;
			if (primary_element.Element != null && primary_element.Element.substance != null)
			{
				base.Trigger(-793429877, primary_element.Element.substance.colour);
			}
		}
		this.recentlyExhausted = true;
		return true;
	}

	// Token: 0x060063FD RID: 25597 RVA: 0x002CA704 File Offset: 0x002C8904
	private void EmitLiquid(int cell)
	{
		int num = Grid.CellBelow(cell);
		Exhaust.EmitDelegate emit = (Grid.IsValidCell(num) && !Grid.Solid[num]) ? Exhaust.emit_particle : Exhaust.emit_element;
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsLiquid && this.EmitCommon(cell, component, emit))
			{
				break;
			}
		}
	}

	// Token: 0x060063FE RID: 25598 RVA: 0x002CA7A0 File Offset: 0x002C89A0
	private void EmitGas(int cell)
	{
		foreach (GameObject gameObject in this.storage.items)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			if (component.Element.IsGas && this.EmitCommon(cell, component, Exhaust.emit_element))
			{
				break;
			}
		}
	}

	// Token: 0x040047D3 RID: 18387
	[MyCmpGet]
	private Vent vent;

	// Token: 0x040047D4 RID: 18388
	[MyCmpGet]
	private Storage storage;

	// Token: 0x040047D5 RID: 18389
	[MyCmpGet]
	private Operational operational;

	// Token: 0x040047D6 RID: 18390
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x040047D7 RID: 18391
	[MyCmpGet]
	private PrimaryElement exhaustPE;

	// Token: 0x040047D8 RID: 18392
	private static readonly Operational.Flag canExhaust = new Operational.Flag("canExhaust", Operational.Flag.Type.Requirement);

	// Token: 0x040047D9 RID: 18393
	private bool isAnimating;

	// Token: 0x040047DA RID: 18394
	private bool recentlyExhausted;

	// Token: 0x040047DB RID: 18395
	private const float MinSwitchTime = 1f;

	// Token: 0x040047DC RID: 18396
	private float elapsedSwitchTime;

	// Token: 0x040047DD RID: 18397
	private SimHashes lastElementEmmited;

	// Token: 0x040047DE RID: 18398
	private static readonly EventSystem.IntraObjectHandler<Exhaust> OnConduitStateChangedDelegate = new EventSystem.IntraObjectHandler<Exhaust>(delegate(Exhaust component, object data)
	{
		component.OnConduitStateChanged(data);
	});

	// Token: 0x040047DF RID: 18399
	private static Exhaust.EmitDelegate emit_element = delegate(int cell, PrimaryElement primary_element)
	{
		SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
	};

	// Token: 0x040047E0 RID: 18400
	private static Exhaust.EmitDelegate emit_particle = delegate(int cell, PrimaryElement primary_element)
	{
		FallingWater.instance.AddParticle(cell, primary_element.Element.idx, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
	};

	// Token: 0x02001311 RID: 4881
	// (Invoke) Token: 0x06006402 RID: 25602
	private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
}
