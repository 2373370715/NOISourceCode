using System;
using TUNING;

// Token: 0x02000CBE RID: 3262
public class BuildingInternalConstructorWorkable : Workable
{
	// Token: 0x06003E35 RID: 15925 RVA: 0x00241EA0 File Offset: 0x002400A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.resetProgressOnStop = false;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x000CCC8D File Offset: 0x000CAE8D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.constructorInstance = this.GetSMI<BuildingInternalConstructor.Instance>();
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x000CCCA1 File Offset: 0x000CAEA1
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.constructorInstance.ConstructionComplete(false);
	}

	// Token: 0x04002AF0 RID: 10992
	private BuildingInternalConstructor.Instance constructorInstance;
}
