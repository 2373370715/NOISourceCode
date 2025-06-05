using System;
using System.Linq;
using TUNING;
using UnityEngine;

// Token: 0x02000FE9 RID: 4073
public class SpiceGrinderWorkable : Workable, IConfigurableConsumer
{
	// Token: 0x060051F6 RID: 20982 RVA: 0x00281774 File Offset: 0x0027F974
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanSpiceGrinder.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Spicing;
		this.attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_spice_grinder_kanim")
		};
		base.SetWorkTime(5f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060051F7 RID: 20983 RVA: 0x00281834 File Offset: 0x0027FA34
	protected override void OnStartWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood != null)
		{
			float num = this.Grinder.CurrentFood.Calories * 0.001f / 1000f;
			base.SetWorkTime(num * 5f);
		}
		else
		{
			global::Debug.LogWarning("SpiceGrider attempted to start spicing with no food");
			base.StopWork(worker, true);
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x060051F8 RID: 20984 RVA: 0x000D9D7A File Offset: 0x000D7F7A
	protected override void OnAbortWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.UpdateFoodSymbol();
	}

	// Token: 0x060051F9 RID: 20985 RVA: 0x000D9D9B File Offset: 0x000D7F9B
	protected override void OnCompleteWork(WorkerBase worker)
	{
		if (this.Grinder.CurrentFood == null)
		{
			return;
		}
		this.Grinder.SpiceFood();
	}

	// Token: 0x060051FA RID: 20986 RVA: 0x002818A0 File Offset: 0x0027FAA0
	public IConfigurableConsumerOption[] GetSettingOptions()
	{
		return SpiceGrinder.SettingOptions.Values.ToArray<SpiceGrinder.Option>();
	}

	// Token: 0x060051FB RID: 20987 RVA: 0x000D9DBC File Offset: 0x000D7FBC
	public IConfigurableConsumerOption GetSelectedOption()
	{
		return this.Grinder.SelectedOption;
	}

	// Token: 0x060051FC RID: 20988 RVA: 0x000D9DC9 File Offset: 0x000D7FC9
	public void SetSelectedOption(IConfigurableConsumerOption option)
	{
		this.Grinder.OnOptionSelected(option as SpiceGrinder.Option);
	}

	// Token: 0x040039CA RID: 14794
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x040039CB RID: 14795
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x040039CC RID: 14796
	public SpiceGrinder.StatesInstance Grinder;
}
