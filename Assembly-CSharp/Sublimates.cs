using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A1D RID: 6685
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Sublimates")]
public class Sublimates : KMonoBehaviour, ISim200ms
{
	// Token: 0x1700091F RID: 2335
	// (get) Token: 0x06008B33 RID: 35635 RVA: 0x000FF92C File Offset: 0x000FDB2C
	public float Temperature
	{
		get
		{
			return this.primaryElement.Temperature;
		}
	}

	// Token: 0x06008B34 RID: 35636 RVA: 0x000FF939 File Offset: 0x000FDB39
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Sublimates>(-2064133523, Sublimates.OnAbsorbDelegate);
		base.Subscribe<Sublimates>(1335436905, Sublimates.OnSplitFromChunkDelegate);
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06008B35 RID: 35637 RVA: 0x000FF96A File Offset: 0x000FDB6A
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.flowAccumulator = Game.Instance.accumulators.Add("EmittedMass", this);
		this.RefreshStatusItem(Sublimates.EmitState.Emitting);
	}

	// Token: 0x06008B36 RID: 35638 RVA: 0x000FF994 File Offset: 0x000FDB94
	protected override void OnCleanUp()
	{
		this.flowAccumulator = Game.Instance.accumulators.Remove(this.flowAccumulator);
		base.OnCleanUp();
	}

	// Token: 0x06008B37 RID: 35639 RVA: 0x0036CF24 File Offset: 0x0036B124
	private void OnAbsorb(object data)
	{
		Pickupable pickupable = (Pickupable)data;
		if (pickupable != null)
		{
			Sublimates component = pickupable.GetComponent<Sublimates>();
			if (component != null)
			{
				this.sublimatedMass += component.sublimatedMass;
			}
		}
	}

	// Token: 0x06008B38 RID: 35640 RVA: 0x0036CF64 File Offset: 0x0036B164
	private void OnSplitFromChunk(object data)
	{
		Pickupable pickupable = data as Pickupable;
		PrimaryElement primaryElement = pickupable.PrimaryElement;
		Sublimates component = pickupable.GetComponent<Sublimates>();
		if (component == null)
		{
			return;
		}
		float mass = this.primaryElement.Mass;
		float mass2 = primaryElement.Mass;
		float num = mass / (mass2 + mass);
		this.sublimatedMass = component.sublimatedMass * num;
		float num2 = 1f - num;
		component.sublimatedMass *= num2;
	}

	// Token: 0x06008B39 RID: 35641 RVA: 0x0036CFD0 File Offset: 0x0036B1D0
	private unsafe bool SimMightOffcellOverpressure(int cell, SimHashes offgass)
	{
		SimHashes id = Grid.Element[cell].id;
		if (id == offgass || id == SimHashes.Vacuum)
		{
			return false;
		}
		IntPtr intPtr = stackalloc byte[(UIntPtr)12];
		*intPtr = Grid.CellLeft(cell);
		*(intPtr + 4) = Grid.CellRight(cell);
		*(intPtr + (IntPtr)2 * 4) = Grid.CellAbove(cell);
		ReadOnlySpan<int> readOnlySpan = new Span<int>(intPtr, 3);
		bool result = false;
		ReadOnlySpan<int> readOnlySpan2 = readOnlySpan;
		for (int i = 0; i < readOnlySpan2.Length; i++)
		{
			int num = *readOnlySpan2[i];
			if (Grid.IsValidCell(num))
			{
				if (Grid.Element[num].id == id)
				{
					return false;
				}
				if (Grid.Element[num].id == offgass)
				{
					result = true;
					if (Grid.Mass[num] < this.info.maxDestinationMass)
					{
						return false;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06008B3A RID: 35642 RVA: 0x0036D090 File Offset: 0x0036B290
	public void Sim200ms(float dt)
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		bool flag = this.HasTag(GameTags.Sealed);
		Pickupable component = base.GetComponent<Pickupable>();
		Storage storage = (component != null) ? component.storage : null;
		if (flag && !this.decayStorage)
		{
			return;
		}
		if (flag && storage != null && storage.HasTag(GameTags.CorrosionProof))
		{
			return;
		}
		Element element = ElementLoader.FindElementByHash(this.info.sublimatedElement);
		if (this.primaryElement.Temperature <= element.lowTemp)
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnTemperature);
			return;
		}
		float num2 = Grid.Mass[num];
		if (num2 < this.info.maxDestinationMass)
		{
			float num3 = this.primaryElement.Mass;
			if (num3 > 0f)
			{
				float num4 = Mathf.Pow(num3, this.info.massPower);
				float num5 = Mathf.Max(this.info.sublimationRate, this.info.sublimationRate * num4);
				num5 *= dt;
				num5 = Mathf.Min(num5, num3);
				this.sublimatedMass += num5;
				num3 -= num5;
				if (this.sublimatedMass > this.info.minSublimationAmount)
				{
					float num6 = this.sublimatedMass / this.primaryElement.Mass;
					byte diseaseIdx;
					int num7;
					if (this.info.diseaseIdx == 255)
					{
						diseaseIdx = this.primaryElement.DiseaseIdx;
						num7 = (int)((float)this.primaryElement.DiseaseCount * num6);
						this.primaryElement.ModifyDiseaseCount(-num7, "Sublimates.SimUpdate");
					}
					else
					{
						float num8 = this.sublimatedMass / this.info.sublimationRate;
						diseaseIdx = this.info.diseaseIdx;
						num7 = (int)((float)this.info.diseaseCount * num8);
					}
					float num9 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
					if (num9 <= 0f || this.SimMightOffcellOverpressure(num, element.id))
					{
						this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
						return;
					}
					this.Emit(num, num9, this.primaryElement.Temperature, diseaseIdx, num7);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num9);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num9);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					if (flag && this.decayStorage && storage != null)
					{
						storage.Trigger(-794517298, new BuildingHP.DamageSourceInfo
						{
							damage = 1,
							source = BUILDINGS.DAMAGESOURCES.CORROSIVE_ELEMENT,
							popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CORROSIVE_ELEMENT,
							fullDamageEffectName = "smoke_damage_kanim"
						});
						return;
					}
				}
			}
			else if (this.sublimatedMass > 0f)
			{
				float num10 = Mathf.Min(this.sublimatedMass, this.info.maxDestinationMass - num2);
				if (num10 > 0f && !this.SimMightOffcellOverpressure(num, element.id))
				{
					this.Emit(num, num10, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount);
					this.sublimatedMass = Mathf.Max(0f, this.sublimatedMass - num10);
					this.primaryElement.Mass = Mathf.Max(0f, this.primaryElement.Mass - num10);
					this.UpdateStorage();
					this.RefreshStatusItem(Sublimates.EmitState.Emitting);
					return;
				}
				this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
				return;
			}
			else if (!this.primaryElement.KeepZeroMassObject)
			{
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
		}
		else
		{
			this.RefreshStatusItem(Sublimates.EmitState.BlockedOnPressure);
		}
	}

	// Token: 0x06008B3B RID: 35643 RVA: 0x0036D468 File Offset: 0x0036B668
	private void UpdateStorage()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null && component.storage != null)
		{
			component.storage.Trigger(-1697596308, base.gameObject);
		}
	}

	// Token: 0x06008B3C RID: 35644 RVA: 0x0036D4AC File Offset: 0x0036B6AC
	private void Emit(int cell, float mass, float temperature, byte disease_idx, int disease_count)
	{
		SimMessages.AddRemoveSubstance(cell, this.info.sublimatedElement, CellEventLogger.Instance.SublimatesEmit, mass, temperature, disease_idx, disease_count, true, -1);
		Game.Instance.accumulators.Accumulate(this.flowAccumulator, mass);
		if (this.spawnFXHash != SpawnFXHashes.None)
		{
			base.transform.GetPosition().z = Grid.GetLayerZ(Grid.SceneLayer.Front);
			Game.Instance.SpawnFX(this.spawnFXHash, base.transform.GetPosition(), 0f);
		}
	}

	// Token: 0x06008B3D RID: 35645 RVA: 0x000FF9B7 File Offset: 0x000FDBB7
	public float AvgFlowRate()
	{
		return Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
	}

	// Token: 0x06008B3E RID: 35646 RVA: 0x0036D534 File Offset: 0x0036B734
	private void RefreshStatusItem(Sublimates.EmitState newEmitState)
	{
		if (newEmitState == this.lastEmitState)
		{
			return;
		}
		switch (newEmitState)
		{
		case Sublimates.EmitState.Emitting:
			if (this.info.sublimatedElement == SimHashes.Oxygen)
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingOxygenAvg, this);
			}
			else
			{
				this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingGasAvg, this);
			}
			break;
		case Sublimates.EmitState.BlockedOnPressure:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedHighPressure, this);
			break;
		case Sublimates.EmitState.BlockedOnTemperature:
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.EmittingBlockedLowTemperature, this);
			break;
		}
		this.lastEmitState = newEmitState;
	}

	// Token: 0x04006918 RID: 26904
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x04006919 RID: 26905
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x0400691A RID: 26906
	[SerializeField]
	public SpawnFXHashes spawnFXHash;

	// Token: 0x0400691B RID: 26907
	public bool decayStorage;

	// Token: 0x0400691C RID: 26908
	[SerializeField]
	public Sublimates.Info info;

	// Token: 0x0400691D RID: 26909
	[Serialize]
	private float sublimatedMass;

	// Token: 0x0400691E RID: 26910
	private HandleVector<int>.Handle flowAccumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x0400691F RID: 26911
	private Sublimates.EmitState lastEmitState = (Sublimates.EmitState)(-1);

	// Token: 0x04006920 RID: 26912
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnAbsorb(data);
	});

	// Token: 0x04006921 RID: 26913
	private static readonly EventSystem.IntraObjectHandler<Sublimates> OnSplitFromChunkDelegate = new EventSystem.IntraObjectHandler<Sublimates>(delegate(Sublimates component, object data)
	{
		component.OnSplitFromChunk(data);
	});

	// Token: 0x02001A1E RID: 6686
	[Serializable]
	public struct Info
	{
		// Token: 0x06008B41 RID: 35649 RVA: 0x000FFA1E File Offset: 0x000FDC1E
		public Info(float rate, float min_amount, float max_destination_mass, float mass_power, SimHashes element, byte disease_idx = 255, int disease_count = 0)
		{
			this.sublimationRate = rate;
			this.minSublimationAmount = min_amount;
			this.maxDestinationMass = max_destination_mass;
			this.massPower = mass_power;
			this.sublimatedElement = element;
			this.diseaseIdx = disease_idx;
			this.diseaseCount = disease_count;
		}

		// Token: 0x04006922 RID: 26914
		public float sublimationRate;

		// Token: 0x04006923 RID: 26915
		public float minSublimationAmount;

		// Token: 0x04006924 RID: 26916
		public float maxDestinationMass;

		// Token: 0x04006925 RID: 26917
		public float massPower;

		// Token: 0x04006926 RID: 26918
		public byte diseaseIdx;

		// Token: 0x04006927 RID: 26919
		public int diseaseCount;

		// Token: 0x04006928 RID: 26920
		[HashedEnum]
		public SimHashes sublimatedElement;
	}

	// Token: 0x02001A1F RID: 6687
	private enum EmitState
	{
		// Token: 0x0400692A RID: 26922
		Emitting,
		// Token: 0x0400692B RID: 26923
		BlockedOnPressure,
		// Token: 0x0400692C RID: 26924
		BlockedOnTemperature
	}
}
