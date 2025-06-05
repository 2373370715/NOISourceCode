using System;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class MinionConfig : IEntityConfig
{
	// Token: 0x060013F5 RID: 5109 RVA: 0x000B314C File Offset: 0x000B134C
	public static string[] GetAttributes()
	{
		return BaseMinionConfig.BaseMinionAttributes().Append(new string[]
		{
			Db.Get().Attributes.FoodExpectation.Id,
			Db.Get().Attributes.ToiletEfficiency.Id
		});
	}

	// Token: 0x060013F6 RID: 5110 RVA: 0x0019A654 File Offset: 0x00198854
	public static string[] GetAmounts()
	{
		return BaseMinionConfig.BaseMinionAmounts().Append(new string[]
		{
			Db.Get().Amounts.Bladder.Id,
			Db.Get().Amounts.Stamina.Id,
			Db.Get().Amounts.Calories.Id
		});
	}

	// Token: 0x060013F7 RID: 5111 RVA: 0x0019A6B8 File Offset: 0x001988B8
	public static AttributeModifier[] GetTraits()
	{
		return BaseMinionConfig.BaseMinionTraits(MinionConfig.MODEL).Append(new AttributeModifier[]
		{
			new AttributeModifier(Db.Get().Attributes.FoodExpectation.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.FOOD_QUALITY_EXPECTATION, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.MAX_CALORIES, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.CALORIES_BURNED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Stamina.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.STAMINA_USED_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Amounts.Bladder.deltaAttribute.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.BLADDER_INCREASE_PER_SECOND, MinionConfig.NAME, false, false, true),
			new AttributeModifier(Db.Get().Attributes.ToiletEfficiency.Id, DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL).BaseStats.TOILET_EFFICIENCY, MinionConfig.NAME, false, false, true)
		});
	}

	// Token: 0x060013F8 RID: 5112 RVA: 0x000B318C File Offset: 0x000B138C
	public GameObject CreatePrefab()
	{
		return BaseMinionConfig.BaseMinion(MinionConfig.MODEL, MinionConfig.GetAttributes(), MinionConfig.GetAmounts(), MinionConfig.GetTraits());
	}

	// Token: 0x060013F9 RID: 5113 RVA: 0x0019A840 File Offset: 0x00198A40
	public void OnPrefabInit(GameObject go)
	{
		BaseMinionConfig.BasePrefabInit(go, MinionConfig.MODEL);
		DUPLICANTSTATS statsFor = DUPLICANTSTATS.GetStatsFor(MinionConfig.MODEL);
		Db.Get().Amounts.Bladder.Lookup(go).value = UnityEngine.Random.Range(0f, 10f);
		AmountInstance amountInstance = Db.Get().Amounts.Calories.Lookup(go);
		amountInstance.value = (statsFor.BaseStats.HUNGRY_THRESHOLD + statsFor.BaseStats.SATISFIED_THRESHOLD) * 0.5f * amountInstance.GetMax();
		AmountInstance amountInstance2 = Db.Get().Amounts.Stamina.Lookup(go);
		amountInstance2.value = amountInstance2.GetMax();
	}

	// Token: 0x060013FA RID: 5114 RVA: 0x000B31A7 File Offset: 0x000B13A7
	public void OnSpawn(GameObject go)
	{
		Sensors component = go.GetComponent<Sensors>();
		component.Add(new ToiletSensor(component));
		BaseMinionConfig.BaseOnSpawn(go, MinionConfig.MODEL, this.RATIONAL_AI_STATE_MACHINES);
		go.GetComponent<OxygenBreather>().AddGasProvider(new GasBreatherFromWorldProvider());
		go.Trigger(1589886948, go);
	}

	// Token: 0x060013FB RID: 5115 RVA: 0x0019A8EC File Offset: 0x00198AEC
	public MinionConfig()
	{
		Func<RationalAi.Instance, StateMachine.Instance>[] array = BaseMinionConfig.BaseRationalAiStateMachines();
		Func<RationalAi.Instance, StateMachine.Instance>[] array2 = new Func<RationalAi.Instance, StateMachine.Instance>[9];
		array2[0] = ((RationalAi.Instance smi) => new BreathMonitor.Instance(smi.master));
		array2[1] = ((RationalAi.Instance smi) => new SteppedInMonitor.Instance(smi.master));
		array2[2] = ((RationalAi.Instance smi) => new Dreamer.Instance(smi.master));
		array2[3] = ((RationalAi.Instance smi) => new StaminaMonitor.Instance(smi.master));
		array2[4] = ((RationalAi.Instance smi) => new RationMonitor.Instance(smi.master));
		array2[5] = ((RationalAi.Instance smi) => new CalorieMonitor.Instance(smi.master));
		array2[6] = ((RationalAi.Instance smi) => new BladderMonitor.Instance(smi.master));
		array2[7] = ((RationalAi.Instance smi) => new HygieneMonitor.Instance(smi.master));
		array2[8] = ((RationalAi.Instance smi) => new TiredMonitor.Instance(smi.master));
		this.RATIONAL_AI_STATE_MACHINES = array.Append(array2);
		base..ctor();
	}

	// Token: 0x04000DBA RID: 3514
	public static Tag MODEL = GameTags.Minions.Models.Standard;

	// Token: 0x04000DBB RID: 3515
	public static string NAME = DUPLICANTS.MODEL.STANDARD.NAME;

	// Token: 0x04000DBC RID: 3516
	public static string ID = MinionConfig.MODEL.ToString();

	// Token: 0x04000DBD RID: 3517
	public Func<RationalAi.Instance, StateMachine.Instance>[] RATIONAL_AI_STATE_MACHINES;
}
