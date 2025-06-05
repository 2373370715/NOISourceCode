using System;
using Klei.CustomSettings;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02001295 RID: 4757
[AddComponentMenu("KMonoBehaviour/scripts/Durability")]
public class Durability : KMonoBehaviour
{
	// Token: 0x170005D1 RID: 1489
	// (get) Token: 0x06006123 RID: 24867 RVA: 0x000E3B0F File Offset: 0x000E1D0F
	// (set) Token: 0x06006124 RID: 24868 RVA: 0x000E3B17 File Offset: 0x000E1D17
	public float TimeEquipped
	{
		get
		{
			return this.timeEquipped;
		}
		set
		{
			this.timeEquipped = value;
		}
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x000E3B20 File Offset: 0x000E1D20
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Durability>(-1617557748, Durability.OnEquippedDelegate);
		base.Subscribe<Durability>(-170173755, Durability.OnUnequippedDelegate);
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x002BEA28 File Offset: 0x002BCC28
	protected override void OnSpawn()
	{
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.Durability, base.gameObject);
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Durability);
		if (currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (id == "Indestructible")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.INDESTRUCTIBLE_DURABILITY_MOD;
				return;
			}
			if (id == "Reinforced")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.REINFORCED_DURABILITY_MOD;
				return;
			}
			if (id == "Flimsy")
			{
				this.difficultySettingMod = EQUIPMENT.SUITS.FLIMSY_DURABILITY_MOD;
				return;
			}
			if (!(id == "Threadbare"))
			{
				return;
			}
			this.difficultySettingMod = EQUIPMENT.SUITS.THREADBARE_DURABILITY_MOD;
		}
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x000E3B4A File Offset: 0x000E1D4A
	private void OnEquipped()
	{
		if (!this.isEquipped)
		{
			this.isEquipped = true;
			this.timeEquipped = GameClock.Instance.GetTimeInCycles();
		}
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x002BEAD4 File Offset: 0x002BCCD4
	private void OnUnequipped()
	{
		if (this.isEquipped)
		{
			this.isEquipped = false;
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			this.DeltaDurability(num * this.durabilityLossPerCycle);
		}
	}

	// Token: 0x06006129 RID: 24873 RVA: 0x000E3B6B File Offset: 0x000E1D6B
	private void DeltaDurability(float delta)
	{
		delta *= this.difficultySettingMod;
		this.durability = Mathf.Clamp01(this.durability + delta);
	}

	// Token: 0x0600612A RID: 24874 RVA: 0x002BEB10 File Offset: 0x002BCD10
	public void ConvertToWornObject()
	{
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.wornEquipmentPrefabID), Grid.SceneLayer.Ore, null, 0);
		gameObject.transform.SetPosition(base.transform.GetPosition());
		gameObject.GetComponent<PrimaryElement>().SetElement(base.GetComponent<PrimaryElement>().ElementID, false);
		gameObject.SetActive(true);
		EquippableFacade component = base.GetComponent<EquippableFacade>();
		if (component != null)
		{
			gameObject.GetComponent<RepairableEquipment>().facadeID = component.FacadeID;
		}
		Storage component2 = base.gameObject.GetComponent<Storage>();
		if (component2)
		{
			JetSuitTank component3 = base.gameObject.GetComponent<JetSuitTank>();
			if (component3)
			{
				component2.AddLiquid(SimHashes.Petroleum, component3.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			}
			component2.DropAll(false, false, default(Vector3), true, null);
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x0600612B RID: 24875 RVA: 0x002BEBFC File Offset: 0x002BCDFC
	public float GetDurability()
	{
		if (this.isEquipped)
		{
			float num = GameClock.Instance.GetTimeInCycles() - this.timeEquipped;
			return this.durability - num * this.durabilityLossPerCycle;
		}
		return this.durability;
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x000E3B8A File Offset: 0x000E1D8A
	public bool IsWornOut()
	{
		return this.GetDurability() <= 0f;
	}

	// Token: 0x0400456D RID: 17773
	private static readonly EventSystem.IntraObjectHandler<Durability> OnEquippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnEquipped();
	});

	// Token: 0x0400456E RID: 17774
	private static readonly EventSystem.IntraObjectHandler<Durability> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<Durability>(delegate(Durability component, object data)
	{
		component.OnUnequipped();
	});

	// Token: 0x0400456F RID: 17775
	[Serialize]
	private bool isEquipped;

	// Token: 0x04004570 RID: 17776
	[Serialize]
	private float timeEquipped;

	// Token: 0x04004571 RID: 17777
	[Serialize]
	private float durability = 1f;

	// Token: 0x04004572 RID: 17778
	public float durabilityLossPerCycle = -0.1f;

	// Token: 0x04004573 RID: 17779
	public string wornEquipmentPrefabID;

	// Token: 0x04004574 RID: 17780
	private float difficultySettingMod = 1f;
}
