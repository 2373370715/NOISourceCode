using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014CD RID: 5325
[SerializationConfig(MemberSerialization.OptIn)]
public class LeadSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06006E34 RID: 28212 RVA: 0x000ECBD4 File Offset: 0x000EADD4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LeadSuitTank>(-1617557748, LeadSuitTank.OnEquippedDelegate);
		base.Subscribe<LeadSuitTank>(-170173755, LeadSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06006E35 RID: 28213 RVA: 0x000ECBFE File Offset: 0x000EADFE
	public float PercentFull()
	{
		return this.batteryCharge;
	}

	// Token: 0x06006E36 RID: 28214 RVA: 0x000ECC06 File Offset: 0x000EAE06
	public bool IsEmpty()
	{
		return this.batteryCharge <= 0f;
	}

	// Token: 0x06006E37 RID: 28215 RVA: 0x000ECC18 File Offset: 0x000EAE18
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06006E38 RID: 28216 RVA: 0x000ECC2A File Offset: 0x000EAE2A
	public bool NeedsRecharging()
	{
		return this.PercentFull() <= 0.25f;
	}

	// Token: 0x06006E39 RID: 28217 RVA: 0x002FCEFC File Offset: 0x002FB0FC
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.LEADSUIT_BATTERY, GameUtil.GetFormattedPercent(this.PercentFull() * 100f, GameUtil.TimeSlice.None));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06006E3A RID: 28218 RVA: 0x002FCF40 File Offset: 0x002FB140
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.leadSuitMonitor = new LeadSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.leadSuitMonitor.StartSM();
		if (this.NeedsRecharging())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.SuitBatteryLow);
		}
	}

	// Token: 0x06006E3B RID: 28219 RVA: 0x002FCFB8 File Offset: 0x002FB1B8
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryLow);
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.SuitBatteryOut);
			NameDisplayScreen.Instance.SetSuitBatteryDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
		}
		if (this.leadSuitMonitor != null)
		{
			this.leadSuitMonitor.StopSM("Removed leadsuit tank");
			this.leadSuitMonitor = null;
		}
	}

	// Token: 0x0400531B RID: 21275
	[Serialize]
	public float batteryCharge = 1f;

	// Token: 0x0400531C RID: 21276
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x0400531D RID: 21277
	public float batteryDuration = 200f;

	// Token: 0x0400531E RID: 21278
	public float coolingOperationalTemperature = 333.15f;

	// Token: 0x0400531F RID: 21279
	public Tag coolantTag;

	// Token: 0x04005320 RID: 21280
	private LeadSuitMonitor.Instance leadSuitMonitor;

	// Token: 0x04005321 RID: 21281
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04005322 RID: 21282
	private static readonly EventSystem.IntraObjectHandler<LeadSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<LeadSuitTank>(delegate(LeadSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
