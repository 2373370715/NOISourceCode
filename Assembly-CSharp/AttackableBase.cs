using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020009BB RID: 2491
[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
	// Token: 0x06002CA8 RID: 11432 RVA: 0x001FA4B4 File Offset: 0x001F86B4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		this.SetupScenePartitioner(null);
		base.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
		GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate, true);
		base.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
		base.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x001FA554 File Offset: 0x001F8754
	public float GetDamageMultiplier()
	{
		if (this.attributeConverter != null && base.worker != null)
		{
			AttributeConverterInstance attributeConverter = base.worker.GetAttributeConverter(this.attributeConverter.Id);
			return Mathf.Max(1f + attributeConverter.Evaluate(), 0.1f);
		}
		return 1f;
	}

	// Token: 0x06002CAA RID: 11434 RVA: 0x001FA5AC File Offset: 0x001F87AC
	private void SetupScenePartitioner(object data = null)
	{
		Extents extents = new Extents(Grid.PosToXY(base.transform.GetPosition()).x, Grid.PosToXY(base.transform.GetPosition()).y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, null);
	}

	// Token: 0x06002CAB RID: 11435 RVA: 0x000C15B8 File Offset: 0x000BF7B8
	private void OnDefeated(object data = null)
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000B95A1 File Offset: 0x000B77A1
	public override float GetEfficiencyMultiplier(WorkerBase worker)
	{
		return 1f;
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x001FA61C File Offset: 0x001F881C
	protected override void OnCleanUp()
	{
		base.Unsubscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate, false);
		GameUtil.UnsubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate);
		base.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate, false);
		base.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate, false);
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x04001E92 RID: 7826
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04001E93 RID: 7827
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04001E94 RID: 7828
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04001E95 RID: 7829
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.SetupScenePartitioner(data);
	});

	// Token: 0x04001E96 RID: 7830
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject));
	});
}
