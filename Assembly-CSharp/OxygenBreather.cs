using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x020016DE RID: 5854
[RequireComponent(typeof(Health))]
[AddComponentMenu("KMonoBehaviour/scripts/OxygenBreather")]
public class OxygenBreather : KMonoBehaviour, ISim200ms
{
	// Token: 0x17000794 RID: 1940
	// (get) Token: 0x060078BA RID: 30906 RVA: 0x000F3D5C File Offset: 0x000F1F5C
	// (set) Token: 0x060078B9 RID: 30905 RVA: 0x000F3D53 File Offset: 0x000F1F53
	public KPrefabID prefabID { get; private set; }

	// Token: 0x17000795 RID: 1941
	// (get) Token: 0x060078BB RID: 30907 RVA: 0x000F3D64 File Offset: 0x000F1F64
	public float ConsumptionRate
	{
		get
		{
			if (this.airConsumptionRate != null)
			{
				return this.airConsumptionRate.GetTotalValue();
			}
			return 0f;
		}
	}

	// Token: 0x17000796 RID: 1942
	// (get) Token: 0x060078BC RID: 30908 RVA: 0x000F3D7F File Offset: 0x000F1F7F
	public float CO2EmitRate
	{
		get
		{
			return Game.Instance.accumulators.GetAverageRate(this.co2Accumulator);
		}
	}

	// Token: 0x17000797 RID: 1943
	// (get) Token: 0x060078BD RID: 30909 RVA: 0x000F3D96 File Offset: 0x000F1F96
	public HandleVector<int>.Handle O2Accumulator
	{
		get
		{
			return this.o2Accumulator;
		}
	}

	// Token: 0x060078BE RID: 30910 RVA: 0x00320CD0 File Offset: 0x0031EED0
	public OxygenBreather.IGasProvider GetCurrentGasProvider()
	{
		if (this.gasProviders.Count == 0)
		{
			return null;
		}
		OxygenBreather.IGasProvider result = null;
		for (int i = this.gasProviders.Count - 1; i >= 0; i--)
		{
			OxygenBreather.IGasProvider gasProvider = this.gasProviders[i];
			if (!gasProvider.IsBlocked())
			{
				result = gasProvider;
				if (gasProvider.HasOxygen())
				{
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060078BF RID: 30911 RVA: 0x00320D28 File Offset: 0x0031EF28
	public bool IsLowOxygen()
	{
		OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
		return currentGasProvider == null || currentGasProvider.IsLowOxygen();
	}

	// Token: 0x17000798 RID: 1944
	// (get) Token: 0x060078C0 RID: 30912 RVA: 0x000F3D9E File Offset: 0x000F1F9E
	public bool HasOxygen
	{
		get
		{
			return this.hasAir;
		}
	}

	// Token: 0x17000799 RID: 1945
	// (get) Token: 0x060078C1 RID: 30913 RVA: 0x000F3DA6 File Offset: 0x000F1FA6
	public bool IsOutOfOxygen
	{
		get
		{
			return !this.hasAir;
		}
	}

	// Token: 0x060078C2 RID: 30914 RVA: 0x000F3DB1 File Offset: 0x000F1FB1
	protected override void OnPrefabInit()
	{
		GameUtil.SubscribeToTags<OxygenBreather>(this, OxygenBreather.OnDeadTagAddedDelegate, true);
		this.prefabID = base.GetComponent<KPrefabID>();
	}

	// Token: 0x060078C3 RID: 30915 RVA: 0x00320D48 File Offset: 0x0031EF48
	protected override void OnSpawn()
	{
		this.airConsumptionRate = Db.Get().Attributes.AirConsumptionRate.Lookup(this);
		this.o2Accumulator = Game.Instance.accumulators.Add("O2", this);
		this.co2Accumulator = Game.Instance.accumulators.Add("CO2", this);
		bool flag = base.gameObject.PrefabID() == BionicMinionConfig.ID;
		KSelectable component = base.GetComponent<KSelectable>();
		this.o2StatusItem = component.AddStatusItem(flag ? Db.Get().DuplicantStatusItems.BreathingO2Bionic : Db.Get().DuplicantStatusItems.BreathingO2, this);
		this.cO2StatusItem = component.AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
		this.temperature = Db.Get().Amounts.Temperature.Lookup(this);
		NameDisplayScreen.Instance.RegisterComponent(base.gameObject, this, false);
	}

	// Token: 0x060078C4 RID: 30916 RVA: 0x00320E44 File Offset: 0x0031F044
	private void BreathableGasConsumed(SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
	{
		if (this.prefabID.HasTag(GameTags.Dead) || this.O2Accumulator == HandleVector<int>.Handle.InvalidHandle)
		{
			return;
		}
		if (elementConsumed == SimHashes.ContaminatedOxygen)
		{
			base.Trigger(-935848905, massConsumed);
		}
		Game.Instance.accumulators.Accumulate(this.O2Accumulator, massConsumed);
		float value = -massConsumed;
		ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, value, base.gameObject.GetProperName(), null);
		if (this.onBreathableGasConsumed != null)
		{
			this.onBreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
		}
	}

	// Token: 0x060078C5 RID: 30917 RVA: 0x000F3DCB File Offset: 0x000F1FCB
	public static void BreathableGasConsumed(OxygenBreather breather, SimHashes elementConsumed, float massConsumed, float temperature, byte disseaseIDX, int disseaseCount)
	{
		if (breather != null)
		{
			breather.BreathableGasConsumed(elementConsumed, massConsumed, temperature, disseaseIDX, disseaseCount);
		}
	}

	// Token: 0x060078C6 RID: 30918 RVA: 0x00320EDC File Offset: 0x0031F0DC
	public void Sim200ms(float dt)
	{
		if (!base.gameObject.HasTag(GameTags.Dead))
		{
			float num = this.airConsumptionRate.GetTotalValue() * dt;
			OxygenBreather.IGasProvider currentGasProvider = this.GetCurrentGasProvider();
			bool flag = currentGasProvider != null && currentGasProvider.ConsumeGas(this, num);
			if (flag)
			{
				if (currentGasProvider.ShouldEmitCO2())
				{
					if (this.cO2StatusItem != Guid.Empty)
					{
						this.cO2StatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
					}
					float num2 = num * this.O2toCO2conversion;
					Game.Instance.accumulators.Accumulate(this.co2Accumulator, num2);
					this.accumulatedCO2 += num2;
					if (this.accumulatedCO2 >= this.minCO2ToEmit)
					{
						this.accumulatedCO2 -= this.minCO2ToEmit;
						Vector3 position = base.transform.GetPosition();
						Vector3 vector = position;
						vector.x += (this.facing.GetFacing() ? (-this.mouthOffset.x) : this.mouthOffset.x);
						vector.y += this.mouthOffset.y;
						vector.z -= 0.5f;
						if (Mathf.FloorToInt(vector.x) != Mathf.FloorToInt(position.x))
						{
							vector.x = Mathf.Floor(position.x) + (this.facing.GetFacing() ? 0.01f : 0.99f);
						}
						CO2Manager.instance.SpawnBreath(vector, this.minCO2ToEmit, this.temperature.value, this.facing.GetFacing());
					}
				}
				else if (currentGasProvider.ShouldStoreCO2())
				{
					if (this.cO2StatusItem != Guid.Empty)
					{
						this.cO2StatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, this);
					}
					Equippable equippable = base.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
					if (equippable != null)
					{
						float num3 = num * this.O2toCO2conversion;
						Game.Instance.accumulators.Accumulate(this.co2Accumulator, num3);
						this.accumulatedCO2 += num3;
						if (this.accumulatedCO2 >= this.minCO2ToEmit)
						{
							this.accumulatedCO2 -= this.minCO2ToEmit;
							equippable.GetComponent<Storage>().AddGasChunk(SimHashes.CarbonDioxide, this.minCO2ToEmit, this.temperature.value, byte.MaxValue, 0, false, true);
						}
					}
				}
				else if (this.cO2StatusItem != Guid.Empty)
				{
					base.GetComponent<KSelectable>().RemoveStatusItem(this.cO2StatusItem, false);
					this.cO2StatusItem = Guid.Empty;
				}
			}
			if (flag != this.hasAir)
			{
				this.hasAirTimer.Start();
				if (this.hasAirTimer.TryStop(2f))
				{
					this.hasAir = flag;
					base.Trigger(-933153513, this.hasAir);
					return;
				}
			}
			else
			{
				this.hasAirTimer.Stop();
			}
		}
	}

	// Token: 0x060078C7 RID: 30919 RVA: 0x000F3DE3 File Offset: 0x000F1FE3
	public void AddGasProvider(OxygenBreather.IGasProvider gas_provider)
	{
		global::Debug.Assert(gas_provider != null, "Error at OxygenBreather.cs  adding gas provider, the gas provider param is null!");
		global::Debug.Assert(!this.gasProviders.Contains(gas_provider), "Error at OxygenBreather.cs adding gas provider, the gas provider was already added to the gas providers list!");
		this.gasProviders.Add(gas_provider);
		gas_provider.OnSetOxygenBreather(this);
	}

	// Token: 0x060078C8 RID: 30920 RVA: 0x003211E8 File Offset: 0x0031F3E8
	public bool RemoveGasProvider(OxygenBreather.IGasProvider provider)
	{
		if (this.gasProviders.Count > 0 && this.gasProviders.Contains(provider))
		{
			OxygenBreather.IGasProvider gasProvider = this.gasProviders[this.gasProviders.Count - 1];
			this.gasProviders.Remove(provider);
			provider.OnClearOxygenBreather(this);
			return true;
		}
		return false;
	}

	// Token: 0x060078C9 RID: 30921 RVA: 0x000F3E1F File Offset: 0x000F201F
	private void OnDeath(object data)
	{
		base.enabled = false;
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.BreathingO2, false);
		component.RemoveStatusItem(Db.Get().DuplicantStatusItems.EmittingCO2, false);
	}

	// Token: 0x060078CA RID: 30922 RVA: 0x00321244 File Offset: 0x0031F444
	protected override void OnCleanUp()
	{
		Game.Instance.accumulators.Remove(this.o2Accumulator);
		Game.Instance.accumulators.Remove(this.co2Accumulator);
		this.o2Accumulator = HandleVector<int>.InvalidHandle;
		this.co2Accumulator = HandleVector<int>.InvalidHandle;
		while (this.gasProviders.Count > 0)
		{
			OxygenBreather.IGasProvider provider = this.gasProviders[this.gasProviders.Count - 1];
			this.RemoveGasProvider(provider);
		}
		base.OnCleanUp();
	}

	// Token: 0x04005AA8 RID: 23208
	public float O2toCO2conversion = 0.5f;

	// Token: 0x04005AA9 RID: 23209
	public Vector2 mouthOffset;

	// Token: 0x04005AAA RID: 23210
	[Serialize]
	public float accumulatedCO2;

	// Token: 0x04005AAB RID: 23211
	[SerializeField]
	public float minCO2ToEmit = 0.3f;

	// Token: 0x04005AAC RID: 23212
	private bool hasAir = true;

	// Token: 0x04005AAD RID: 23213
	private Timer hasAirTimer = new Timer();

	// Token: 0x04005AAE RID: 23214
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04005AAF RID: 23215
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04005AB1 RID: 23217
	private HandleVector<int>.Handle o2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04005AB2 RID: 23218
	private HandleVector<int>.Handle co2Accumulator = HandleVector<int>.InvalidHandle;

	// Token: 0x04005AB3 RID: 23219
	private AmountInstance temperature;

	// Token: 0x04005AB4 RID: 23220
	public float lowOxygenThreshold;

	// Token: 0x04005AB5 RID: 23221
	public float noOxygenThreshold;

	// Token: 0x04005AB6 RID: 23222
	private AttributeInstance airConsumptionRate;

	// Token: 0x04005AB7 RID: 23223
	public Action<SimHashes, float, float, byte, int> onBreathableGasConsumed;

	// Token: 0x04005AB8 RID: 23224
	private static readonly EventSystem.IntraObjectHandler<OxygenBreather> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<OxygenBreather>(GameTags.Dead, delegate(OxygenBreather component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04005AB9 RID: 23225
	private List<OxygenBreather.IGasProvider> gasProviders = new List<OxygenBreather.IGasProvider>();

	// Token: 0x04005ABA RID: 23226
	private Guid o2StatusItem;

	// Token: 0x04005ABB RID: 23227
	private Guid cO2StatusItem;

	// Token: 0x020016DF RID: 5855
	public interface IGasProvider
	{
		// Token: 0x060078CD RID: 30925
		void OnSetOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x060078CE RID: 30926
		void OnClearOxygenBreather(OxygenBreather oxygen_breather);

		// Token: 0x060078CF RID: 30927
		bool ConsumeGas(OxygenBreather oxygen_breather, float amount);

		// Token: 0x060078D0 RID: 30928
		bool ShouldEmitCO2();

		// Token: 0x060078D1 RID: 30929
		bool ShouldStoreCO2();

		// Token: 0x060078D2 RID: 30930
		bool IsLowOxygen();

		// Token: 0x060078D3 RID: 30931
		bool HasOxygen();

		// Token: 0x060078D4 RID: 30932
		bool IsBlocked();
	}
}
