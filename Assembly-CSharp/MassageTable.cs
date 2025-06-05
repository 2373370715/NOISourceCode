using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000ED0 RID: 3792
public class MassageTable : RelaxationPoint, IGameObjectEffectDescriptor, IActivationRangeTarget
{
	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06004BD1 RID: 19409 RVA: 0x000D56BC File Offset: 0x000D38BC
	public string ActivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x000D56C8 File Offset: 0x000D38C8
	public string DeactivateTooltip
	{
		get
		{
			return BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
		}
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x000D56D4 File Offset: 0x000D38D4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
	}

	// Token: 0x06004BD4 RID: 19412 RVA: 0x0026DF58 File Offset: 0x0026C158
	private void OnCopySettings(object data)
	{
		MassageTable component = ((GameObject)data).GetComponent<MassageTable>();
		if (component != null)
		{
			this.ActivateValue = component.ActivateValue;
			this.DeactivateValue = component.DeactivateValue;
		}
	}

	// Token: 0x06004BD5 RID: 19413 RVA: 0x0026DF94 File Offset: 0x0026C194
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		Effects component = worker.GetComponent<Effects>();
		for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
		{
			string effect_id = MassageTable.EffectsRemoved[i];
			component.Remove(effect_id);
		}
	}

	// Token: 0x06004BD6 RID: 19414 RVA: 0x0026DFD0 File Offset: 0x0026C1D0
	public new List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		if (MassageTable.EffectsRemoved.Length != 0)
		{
			Descriptor item2 = default(Descriptor);
			item2.SetupDescriptor(UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
			list.Add(item2);
			for (int i = 0; i < MassageTable.EffectsRemoved.Length; i++)
			{
				string text = MassageTable.EffectsRemoved[i];
				string arg = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".NAME");
				string arg2 = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + text.ToUpper() + ".CAUSE");
				Descriptor item3 = default(Descriptor);
				item3.IncreaseIndent();
				item3.SetupDescriptor("• " + string.Format(UI.BUILDINGEFFECTS.REMOVEDEFFECT, arg), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, arg2), Descriptor.DescriptorType.Effect);
				list.Add(item3);
			}
		}
		return list;
	}

	// Token: 0x06004BD7 RID: 19415 RVA: 0x0026E130 File Offset: 0x0026C330
	protected override WorkChore<RelaxationPoint> CreateWorkChore()
	{
		WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, this, null, true, null, null, null, false, null, true, true, null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
		workChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, this);
		return workChore;
	}

	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x000D56ED File Offset: 0x000D38ED
	// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x000D56F5 File Offset: 0x000D38F5
	public float ActivateValue
	{
		get
		{
			return this.activateValue;
		}
		set
		{
			this.activateValue = value;
		}
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x06004BDA RID: 19418 RVA: 0x000D56FE File Offset: 0x000D38FE
	// (set) Token: 0x06004BDB RID: 19419 RVA: 0x000D5706 File Offset: 0x000D3906
	public float DeactivateValue
	{
		get
		{
			return this.stopStressingValue;
		}
		set
		{
			this.stopStressingValue = value;
		}
	}

	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x06004BDC RID: 19420 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool UseWholeNumbers
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06004BDD RID: 19421 RVA: 0x000C18F8 File Offset: 0x000BFAF8
	public float MinValue
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06004BDE RID: 19422 RVA: 0x000CD7B4 File Offset: 0x000CB9B4
	public float MaxValue
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06004BDF RID: 19423 RVA: 0x000D570F File Offset: 0x000D390F
	public string ActivationRangeTitleText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
		}
	}

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x000D571B File Offset: 0x000D391B
	public string ActivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06004BE1 RID: 19425 RVA: 0x000D5727 File Offset: 0x000D3927
	public string DeactivateSliderLabelText
	{
		get
		{
			return UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
		}
	}

	// Token: 0x04003518 RID: 13592
	[Serialize]
	private float activateValue = 50f;

	// Token: 0x04003519 RID: 13593
	private static readonly string[] EffectsRemoved = new string[]
	{
		"SoreBack"
	};

	// Token: 0x0400351A RID: 13594
	private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>(delegate(MassageTable component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x0400351B RID: 13595
	private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition
	{
		id = "IsStressAboveActivationRange",
		description = DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget)data;
			return Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= activationRangeTarget.ActivateValue;
		}
	};
}
