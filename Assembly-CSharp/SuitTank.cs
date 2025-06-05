using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A2A RID: 6698
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SuitTank")]
public class SuitTank : KMonoBehaviour, IGameObjectEffectDescriptor, OxygenBreather.IGasProvider
{
	// Token: 0x06008B77 RID: 35703 RVA: 0x000FFCD4 File Offset: 0x000FDED4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<SuitTank>(-1617557748, SuitTank.OnEquippedDelegate);
		base.Subscribe<SuitTank>(-170173755, SuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06008B78 RID: 35704 RVA: 0x0036DCE4 File Offset: 0x0036BEE4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.amount != 0f)
		{
			this.storage.AddGasChunk(SimHashes.Oxygen, this.amount, base.GetComponent<PrimaryElement>().Temperature, byte.MaxValue, 0, false, true);
			this.amount = 0f;
		}
		this.equippable = base.GetComponent<Equippable>();
	}

	// Token: 0x06008B79 RID: 35705 RVA: 0x000FFCFE File Offset: 0x000FDEFE
	public float GetTankAmount()
	{
		if (this.storage == null)
		{
			this.storage = base.GetComponent<Storage>();
		}
		return this.storage.GetMassAvailable(this.elementTag);
	}

	// Token: 0x06008B7A RID: 35706 RVA: 0x000FFD2B File Offset: 0x000FDF2B
	public float PercentFull()
	{
		return this.GetTankAmount() / this.capacity;
	}

	// Token: 0x06008B7B RID: 35707 RVA: 0x000FFD3A File Offset: 0x000FDF3A
	public bool IsEmpty()
	{
		return this.GetTankAmount() <= 0f;
	}

	// Token: 0x06008B7C RID: 35708 RVA: 0x000FFD4C File Offset: 0x000FDF4C
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06008B7D RID: 35709 RVA: 0x000FFD5E File Offset: 0x000FDF5E
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06008B7E RID: 35710 RVA: 0x0036DD48 File Offset: 0x0036BF48
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.elementTag == GameTags.Breathable)
		{
			string text = this.underwaterSupport ? string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK_UNDERWATER, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")) : string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.OXYGEN_TANK, GameUtil.GetFormattedMass(this.GetTankAmount(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
			list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		}
		return list;
	}

	// Token: 0x06008B7F RID: 35711 RVA: 0x0036DDCC File Offset: 0x0036BFCC
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
		OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
		if (component != null)
		{
			component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().AddIgnoredFlagsSet("SuitTank", this.SafeCellFlagsToIgnoreOnEquipped);
			component.AddGasProvider(this);
		}
		targetGameObject.AddTag(GameTags.HasSuitTank);
	}

	// Token: 0x06008B80 RID: 35712 RVA: 0x0036DE4C File Offset: 0x0036C04C
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			NameDisplayScreen.Instance.SetSuitTankDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), false);
			GameObject targetGameObject = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
			OxygenBreather component = targetGameObject.GetComponent<OxygenBreather>();
			if (component != null)
			{
				component.GetComponent<Sensors>().GetSensor<SafeCellSensor>().RemoveIgnoredFlagsSet("SuitTank");
				component.RemoveGasProvider(this);
			}
			targetGameObject.RemoveTag(GameTags.HasSuitTank);
		}
	}

	// Token: 0x06008B81 RID: 35713 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06008B82 RID: 35714 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06008B83 RID: 35715 RVA: 0x0036DECC File Offset: 0x0036C0CC
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.IsEmpty())
		{
			return false;
		}
		float temperature = 0f;
		SimHashes elementConsumed = SimHashes.Vacuum;
		float massConsumed;
		SimUtil.DiseaseInfo diseaseInfo;
		this.storage.ConsumeAndGetDisease(this.elementTag, amount, out massConsumed, out diseaseInfo, out temperature, out elementConsumed);
		OxygenBreather.BreathableGasConsumed(oxygen_breather, elementConsumed, massConsumed, temperature, diseaseInfo.idx, diseaseInfo.count);
		base.Trigger(608245985, base.gameObject);
		return true;
	}

	// Token: 0x06008B84 RID: 35716 RVA: 0x0036DF30 File Offset: 0x0036C130
	public bool ShouldEmitCO2()
	{
		bool flag = base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
		if (flag)
		{
			return false;
		}
		bool flag2 = this.IsOwnerBionic();
		return !flag && !flag2;
	}

	// Token: 0x06008B85 RID: 35717 RVA: 0x0036DF64 File Offset: 0x0036C164
	public bool ShouldStoreCO2()
	{
		bool flag = base.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit);
		if (!flag)
		{
			return false;
		}
		bool flag2 = this.IsOwnerBionic();
		return flag && !flag2;
	}

	// Token: 0x06008B86 RID: 35718 RVA: 0x0036DF98 File Offset: 0x0036C198
	public bool IsOwnerBionic()
	{
		bool result = false;
		if (this.equippable != null && this.equippable.IsAssigned() && this.equippable.isEquipped)
		{
			Ownables soleOwner = this.equippable.assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
				if (targetGameObject)
				{
					result = (targetGameObject.PrefabID() == BionicMinionConfig.ID);
				}
			}
		}
		return result;
	}

	// Token: 0x06008B87 RID: 35719 RVA: 0x000FFD6D File Offset: 0x000FDF6D
	public bool IsLowOxygen()
	{
		return this.NeedsRecharging();
	}

	// Token: 0x06008B88 RID: 35720 RVA: 0x0036E014 File Offset: 0x0036C214
	[ContextMenu("SetToRefillAmount")]
	public void SetToRefillAmount()
	{
		float tankAmount = this.GetTankAmount();
		float num = 0.25f * this.capacity;
		if (tankAmount > num)
		{
			this.storage.ConsumeIgnoringDisease(this.elementTag, tankAmount - num);
		}
	}

	// Token: 0x06008B89 RID: 35721 RVA: 0x000FFD75 File Offset: 0x000FDF75
	[ContextMenu("Empty")]
	public void Empty()
	{
		this.storage.ConsumeIgnoringDisease(this.elementTag, this.GetTankAmount());
	}

	// Token: 0x06008B8A RID: 35722 RVA: 0x000FFD8E File Offset: 0x000FDF8E
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		this.Empty();
		this.storage.AddGasChunk(SimHashes.Oxygen, this.capacity, 15f, 0, 0, false, false);
	}

	// Token: 0x06008B8B RID: 35723 RVA: 0x000FFDB6 File Offset: 0x000FDFB6
	public bool HasOxygen()
	{
		return !this.IsEmpty();
	}

	// Token: 0x06008B8C RID: 35724 RVA: 0x000B1628 File Offset: 0x000AF828
	public bool IsBlocked()
	{
		return false;
	}

	// Token: 0x04006949 RID: 26953
	public SafeCellQuery.SafeFlags SafeCellFlagsToIgnoreOnEquipped = (SafeCellQuery.SafeFlags)464;

	// Token: 0x0400694A RID: 26954
	[Serialize]
	public string element;

	// Token: 0x0400694B RID: 26955
	[Serialize]
	public float amount;

	// Token: 0x0400694C RID: 26956
	public Tag elementTag;

	// Token: 0x0400694D RID: 26957
	[MyCmpReq]
	public Storage storage;

	// Token: 0x0400694E RID: 26958
	public float capacity;

	// Token: 0x0400694F RID: 26959
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04006950 RID: 26960
	public bool underwaterSupport;

	// Token: 0x04006951 RID: 26961
	private Equippable equippable;

	// Token: 0x04006952 RID: 26962
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04006953 RID: 26963
	private static readonly EventSystem.IntraObjectHandler<SuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<SuitTank>(delegate(SuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
