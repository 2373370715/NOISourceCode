using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020014AB RID: 5291
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/JetSuitTank")]
public class JetSuitTank : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06006D89 RID: 28041 RVA: 0x000EC60F File Offset: 0x000EA80F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.amount = 25f;
		base.Subscribe<JetSuitTank>(-1617557748, JetSuitTank.OnEquippedDelegate);
		base.Subscribe<JetSuitTank>(-170173755, JetSuitTank.OnUnequippedDelegate);
	}

	// Token: 0x06006D8A RID: 28042 RVA: 0x000EC644 File Offset: 0x000EA844
	public float PercentFull()
	{
		return this.amount / 25f;
	}

	// Token: 0x06006D8B RID: 28043 RVA: 0x000EC652 File Offset: 0x000EA852
	public bool IsEmpty()
	{
		return this.amount <= 0f;
	}

	// Token: 0x06006D8C RID: 28044 RVA: 0x000EC664 File Offset: 0x000EA864
	public bool IsFull()
	{
		return this.PercentFull() >= 1f;
	}

	// Token: 0x06006D8D RID: 28045 RVA: 0x000EC676 File Offset: 0x000EA876
	public bool NeedsRecharging()
	{
		return this.PercentFull() < 0.25f;
	}

	// Token: 0x06006D8E RID: 28046 RVA: 0x002FA390 File Offset: 0x002F8590
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		string text = string.Format(UI.UISIDESCREENS.FABRICATORSIDESCREEN.EFFECTS.JETSUIT_TANK, GameUtil.GetFormattedMass(this.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
		list.Add(new Descriptor(text, text, Descriptor.DescriptorType.Effect, false));
		return list;
	}

	// Token: 0x06006D8F RID: 28047 RVA: 0x002FA3D4 File Offset: 0x002F85D4
	private void OnEquipped(object data)
	{
		Equipment equipment = (Equipment)data;
		NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), new Func<float>(this.PercentFull), true);
		this.jetSuitMonitor = new JetSuitMonitor.Instance(this, equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject());
		this.jetSuitMonitor.StartSM();
		if (this.IsEmpty())
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().AddTag(GameTags.JetSuitOutOfFuel);
		}
	}

	// Token: 0x06006D90 RID: 28048 RVA: 0x002FA44C File Offset: 0x002F864C
	private void OnUnequipped(object data)
	{
		Equipment equipment = (Equipment)data;
		if (!equipment.destroyed)
		{
			equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().RemoveTag(GameTags.JetSuitOutOfFuel);
			NameDisplayScreen.Instance.SetSuitFuelDisplay(equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject(), null, false);
			Navigator component = equipment.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Navigator>();
			if (component && component.CurrentNavType == NavType.Hover)
			{
				component.SetCurrentNavType(NavType.Floor);
			}
		}
		if (this.jetSuitMonitor != null)
		{
			this.jetSuitMonitor.StopSM("Removed jetsuit tank");
			this.jetSuitMonitor = null;
		}
	}

	// Token: 0x0400528D RID: 21133
	[MyCmpGet]
	private ElementEmitter elementConverter;

	// Token: 0x0400528E RID: 21134
	[Serialize]
	public float amount;

	// Token: 0x0400528F RID: 21135
	public const float FUEL_CAPACITY = 25f;

	// Token: 0x04005290 RID: 21136
	public const float FUEL_BURN_RATE = 0.1f;

	// Token: 0x04005291 RID: 21137
	public const float CO2_EMITTED_PER_FUEL_BURNED = 3f;

	// Token: 0x04005292 RID: 21138
	public const float EMIT_TEMPERATURE = 473.15f;

	// Token: 0x04005293 RID: 21139
	public const float REFILL_PERCENT = 0.25f;

	// Token: 0x04005294 RID: 21140
	private JetSuitMonitor.Instance jetSuitMonitor;

	// Token: 0x04005295 RID: 21141
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnEquippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x04005296 RID: 21142
	private static readonly EventSystem.IntraObjectHandler<JetSuitTank> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<JetSuitTank>(delegate(JetSuitTank component, object data)
	{
		component.OnUnequipped(data);
	});
}
