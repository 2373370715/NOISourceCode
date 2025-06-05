using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009C9 RID: 2505
public class Bee : KMonoBehaviour
{
	// Token: 0x06002CFE RID: 11518 RVA: 0x001FB1F8 File Offset: 0x001F93F8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Bee>(-739654666, Bee.OnAttackDelegate);
		base.Subscribe<Bee>(-1283701846, Bee.OnSleepDelegate);
		base.Subscribe<Bee>(-2090444759, Bee.OnWakeUpDelegate);
		base.Subscribe<Bee>(1623392196, Bee.OnDeathDelegate);
		base.Subscribe<Bee>(49018834, Bee.OnSatisfiedDelegate);
		base.Subscribe<Bee>(-647798969, Bee.OnUnhappyDelegate);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("tag", false);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapto_tag", false);
		this.StopSleep();
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x001FB2A4 File Offset: 0x001F94A4
	private void OnDeath(object data)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Storage component2 = base.GetComponent<Storage>();
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
		component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH, false, true);
		component2.DropAll(base.transform.position, true, true, default(Vector3), true, null);
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x000C1933 File Offset: 0x000BFB33
	private void StartSleep()
	{
		this.RemoveRadiationMod(this.awakeRadiationModKey);
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000C194D File Offset: 0x000BFB4D
	private void StopSleep()
	{
		this.AddRadiationModifier(this.awakeRadiationModKey, this.awakeRadiationMod);
		base.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000C196D File Offset: 0x000BFB6D
	private void AddRadiationModifier(HashedString name, float mod)
	{
		this.radiationModifiers.Add(name, mod);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000C1982 File Offset: 0x000BFB82
	private void RemoveRadiationMod(HashedString name)
	{
		this.radiationModifiers.Remove(name);
		this.RefreshRadiationOutput();
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x001FB320 File Offset: 0x001F9520
	public void RefreshRadiationOutput()
	{
		float num = this.radiationOutputAmount;
		foreach (KeyValuePair<HashedString, float> keyValuePair in this.radiationModifiers)
		{
			num *= keyValuePair.Value;
		}
		RadiationEmitter component = base.GetComponent<RadiationEmitter>();
		component.SetEmitting(true);
		component.emitRads = num;
		component.Refresh();
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x000C1997 File Offset: 0x000BFB97
	private void OnAttack(object data)
	{
		if ((Tag)data == GameTags.Creatures.Attack)
		{
			base.GetComponent<Health>().Damage(base.GetComponent<Health>().hitPoints);
		}
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x001FB398 File Offset: 0x001F9598
	public KPrefabID FindHiveInRoom()
	{
		List<BeeHive.StatesInstance> list = new List<BeeHive.StatesInstance>();
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		foreach (BeeHive.StatesInstance statesInstance in Components.BeeHives.Items)
		{
			if (Game.Instance.roomProber.GetRoomOfGameObject(statesInstance.gameObject) == roomOfGameObject)
			{
				list.Add(statesInstance);
			}
		}
		int num = int.MaxValue;
		KPrefabID result = null;
		foreach (BeeHive.StatesInstance statesInstance2 in list)
		{
			int navigationCost = base.gameObject.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(statesInstance2.transform.GetLocalPosition()));
			if (navigationCost < num)
			{
				num = navigationCost;
				result = statesInstance2.GetComponent<KPrefabID>();
			}
		}
		return result;
	}

	// Token: 0x04001ED2 RID: 7890
	public float radiationOutputAmount;

	// Token: 0x04001ED3 RID: 7891
	private Dictionary<HashedString, float> radiationModifiers = new Dictionary<HashedString, float>();

	// Token: 0x04001ED4 RID: 7892
	private float unhappyRadiationMod = 0.1f;

	// Token: 0x04001ED5 RID: 7893
	private float awakeRadiationMod;

	// Token: 0x04001ED6 RID: 7894
	private HashedString unhappyRadiationModKey = "UNHAPPY";

	// Token: 0x04001ED7 RID: 7895
	private HashedString awakeRadiationModKey = "AWAKE";

	// Token: 0x04001ED8 RID: 7896
	private static readonly EventSystem.IntraObjectHandler<Bee> OnAttackDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnAttack(data);
	});

	// Token: 0x04001ED9 RID: 7897
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSleepDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StartSleep();
	});

	// Token: 0x04001EDA RID: 7898
	private static readonly EventSystem.IntraObjectHandler<Bee> OnWakeUpDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StopSleep();
	});

	// Token: 0x04001EDB RID: 7899
	private static readonly EventSystem.IntraObjectHandler<Bee> OnDeathDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04001EDC RID: 7900
	private static readonly EventSystem.IntraObjectHandler<Bee> OnUnhappyDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.AddRadiationModifier(component.unhappyRadiationModKey, component.unhappyRadiationMod);
	});

	// Token: 0x04001EDD RID: 7901
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSatisfiedDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.RemoveRadiationMod(component.unhappyRadiationModKey);
	});
}
