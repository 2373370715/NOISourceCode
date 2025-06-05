using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200142F RID: 5167
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x060069E2 RID: 27106 RVA: 0x000E9D8C File Offset: 0x000E7F8C
	public float Particles
	{
		get
		{
			return this.particles;
		}
	}

	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x060069E3 RID: 27107 RVA: 0x000E9D94 File Offset: 0x000E7F94
	// (set) Token: 0x060069E4 RID: 27108 RVA: 0x000E9D9C File Offset: 0x000E7F9C
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x060069E5 RID: 27109 RVA: 0x002EA87C File Offset: 0x002E8A7C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Combine(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
			component.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(component.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		this.SetupStorageStatusItems();
	}

	// Token: 0x060069E6 RID: 27110 RVA: 0x000E9DA5 File Offset: 0x000E7FA5
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateLogicPorts();
	}

	// Token: 0x060069E7 RID: 27111 RVA: 0x002EA8EC File Offset: 0x002E8AEC
	private void UpdateLogicPorts()
	{
		if (this._logicPorts != null)
		{
			bool value = this.IsFull();
			this._logicPorts.SendSignal(this.PORT_ID, Convert.ToInt32(value));
		}
	}

	// Token: 0x060069E8 RID: 27112 RVA: 0x000E9DB3 File Offset: 0x000E7FB3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Remove(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
		}
	}

	// Token: 0x060069E9 RID: 27113 RVA: 0x002EA92C File Offset: 0x002E8B2C
	private void OnParticleCapture(HighEnergyParticle particle)
	{
		float num = Mathf.Min(particle.payload, this.capacity - this.particles);
		this.Store(num);
		particle.payload -= num;
		if (particle.payload > 0f)
		{
			base.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
		}
	}

	// Token: 0x060069EA RID: 27114 RVA: 0x000E9DEF File Offset: 0x000E7FEF
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.particles < this.capacity && this.receiverOpen;
	}

	// Token: 0x060069EB RID: 27115 RVA: 0x002EA988 File Offset: 0x002E8B88
	private void DeltaParticles(float delta)
	{
		this.particles += delta;
		if (this.particles <= 0f)
		{
			base.Trigger(155636535, base.transform.gameObject);
		}
		base.Trigger(-1837862626, base.transform.gameObject);
		this.UpdateLogicPorts();
	}

	// Token: 0x060069EC RID: 27116 RVA: 0x002EA9E4 File Offset: 0x002E8BE4
	public float Store(float amount)
	{
		float num = Mathf.Min(amount, this.RemainingCapacity());
		this.DeltaParticles(num);
		return num;
	}

	// Token: 0x060069ED RID: 27117 RVA: 0x000E9E07 File Offset: 0x000E8007
	public float ConsumeAndGet(float amount)
	{
		amount = Mathf.Min(this.Particles, amount);
		this.DeltaParticles(-amount);
		return amount;
	}

	// Token: 0x060069EE RID: 27118 RVA: 0x000E9E20 File Offset: 0x000E8020
	[ContextMenu("Trigger Stored Event")]
	public void DEBUG_TriggerStorageEvent()
	{
		base.Trigger(-1837862626, base.transform.gameObject);
	}

	// Token: 0x060069EF RID: 27119 RVA: 0x000E9E38 File Offset: 0x000E8038
	[ContextMenu("Trigger Zero Event")]
	public void DEBUG_TriggerZeroEvent()
	{
		this.ConsumeAndGet(this.particles + 1f);
	}

	// Token: 0x060069F0 RID: 27120 RVA: 0x000E9E4D File Offset: 0x000E804D
	public float ConsumeAll()
	{
		return this.ConsumeAndGet(this.particles);
	}

	// Token: 0x060069F1 RID: 27121 RVA: 0x000E9E5B File Offset: 0x000E805B
	public bool HasRadiation()
	{
		return this.Particles > 0f;
	}

	// Token: 0x060069F2 RID: 27122 RVA: 0x000AA765 File Offset: 0x000A8965
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		return null;
	}

	// Token: 0x060069F3 RID: 27123 RVA: 0x000E9E6A File Offset: 0x000E806A
	public List<GameObject> GetItems()
	{
		return new List<GameObject>
		{
			base.gameObject
		};
	}

	// Token: 0x060069F4 RID: 27124 RVA: 0x000E9E7D File Offset: 0x000E807D
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x060069F5 RID: 27125 RVA: 0x000E9E8F File Offset: 0x000E808F
	public bool IsEmpty()
	{
		return this.Particles == 0f;
	}

	// Token: 0x060069F6 RID: 27126 RVA: 0x000E9E9E File Offset: 0x000E809E
	public float Capacity()
	{
		return this.capacity;
	}

	// Token: 0x060069F7 RID: 27127 RVA: 0x000E9EA6 File Offset: 0x000E80A6
	public float RemainingCapacity()
	{
		return Mathf.Max(this.capacity - this.Particles, 0f);
	}

	// Token: 0x060069F8 RID: 27128 RVA: 0x000E9EBF File Offset: 0x000E80BF
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x060069F9 RID: 27129 RVA: 0x000E9EC7 File Offset: 0x000E80C7
	public float GetAmountAvailable(Tag tag)
	{
		if (tag != GameTags.HighEnergyParticle)
		{
			return 0f;
		}
		return this.Particles;
	}

	// Token: 0x060069FA RID: 27130 RVA: 0x000E9EE2 File Offset: 0x000E80E2
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount", null);
		this.ConsumeAndGet(amount);
	}

	// Token: 0x060069FB RID: 27131 RVA: 0x002EAA08 File Offset: 0x002E8C08
	private void SetupStorageStatusItems()
	{
		if (HighEnergyParticleStorage.capacityStatusItem == null)
		{
			HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = (HighEnergyParticleStorage)data;
				string newValue = Util.FormatWholeNumber(highEnergyParticleStorage.particles);
				string newValue2 = Util.FormatWholeNumber(highEnergyParticleStorage.capacity);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				str = str.Replace("{Units}", UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, this);
		}
	}

	// Token: 0x0400504F RID: 20559
	[Serialize]
	[SerializeField]
	private float particles;

	// Token: 0x04005050 RID: 20560
	public float capacity = float.MaxValue;

	// Token: 0x04005051 RID: 20561
	public bool showInUI = true;

	// Token: 0x04005052 RID: 20562
	public bool showCapacityStatusItem;

	// Token: 0x04005053 RID: 20563
	public bool showCapacityAsMainStatus;

	// Token: 0x04005055 RID: 20565
	public bool autoStore;

	// Token: 0x04005056 RID: 20566
	[Serialize]
	public bool receiverOpen = true;

	// Token: 0x04005057 RID: 20567
	[MyCmpGet]
	private LogicPorts _logicPorts;

	// Token: 0x04005058 RID: 20568
	public string PORT_ID = "";

	// Token: 0x04005059 RID: 20569
	private static StatusItem capacityStatusItem;
}
