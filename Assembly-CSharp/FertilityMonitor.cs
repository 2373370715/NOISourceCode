using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020011BA RID: 4538
public class FertilityMonitor : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>
{
	// Token: 0x06005C42 RID: 23618 RVA: 0x002A86E0 File Offset: 0x002A68E0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.fertile;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.root.DefaultState(this.fertile);
		this.fertile.ToggleBehaviour(GameTags.Creatures.Fertile, (FertilityMonitor.Instance smi) => smi.IsReadyToLayEgg(), null).ToggleEffect((FertilityMonitor.Instance smi) => smi.fertileEffect).Transition(this.infertile, GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Not(new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile)), UpdateRate.SIM_1000ms);
		this.infertile.Transition(this.fertile, new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile), UpdateRate.SIM_1000ms);
	}

	// Token: 0x06005C43 RID: 23619 RVA: 0x000E096E File Offset: 0x000DEB6E
	public static bool IsFertile(FertilityMonitor.Instance smi)
	{
		return !smi.HasTag(GameTags.Creatures.PausedReproduction) && !smi.HasTag(GameTags.Creatures.Confined) && !smi.HasTag(GameTags.Creatures.Expecting);
	}

	// Token: 0x06005C44 RID: 23620 RVA: 0x002A87A0 File Offset: 0x002A69A0
	public static Tag EggBreedingRoll(List<FertilityMonitor.BreedingChance> breedingChances, bool excludeOriginalCreature = false)
	{
		float num = UnityEngine.Random.value;
		if (excludeOriginalCreature)
		{
			num *= 1f - breedingChances[0].weight;
		}
		foreach (FertilityMonitor.BreedingChance breedingChance in breedingChances)
		{
			if (excludeOriginalCreature)
			{
				excludeOriginalCreature = false;
			}
			else
			{
				num -= breedingChance.weight;
				if (num <= 0f)
				{
					return breedingChance.egg;
				}
			}
		}
		return Tag.Invalid;
	}

	// Token: 0x040041B4 RID: 16820
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile;

	// Token: 0x040041B5 RID: 16821
	private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile;

	// Token: 0x020011BB RID: 4539
	[Serializable]
	public class BreedingChance
	{
		// Token: 0x040041B6 RID: 16822
		public Tag egg;

		// Token: 0x040041B7 RID: 16823
		public float weight;
	}

	// Token: 0x020011BC RID: 4540
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005C47 RID: 23623 RVA: 0x000E09A6 File Offset: 0x000DEBA6
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
		}

		// Token: 0x040041B8 RID: 16824
		public Tag eggPrefab;

		// Token: 0x040041B9 RID: 16825
		public List<FertilityMonitor.BreedingChance> initialBreedingWeights;

		// Token: 0x040041BA RID: 16826
		public float baseFertileCycles;
	}

	// Token: 0x020011BD RID: 4541
	public new class Instance : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.GameInstance
	{
		// Token: 0x06005C49 RID: 23625 RVA: 0x002A8830 File Offset: 0x002A6A30
		public Instance(IStateMachineTarget master, FertilityMonitor.Def def) : base(master, def)
		{
			this.fertility = Db.Get().Amounts.Fertility.Lookup(base.gameObject);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				this.fertility.deltaAttribute.Add(new AttributeModifier(this.fertility.deltaAttribute.Id, 33.333332f, "Accelerated Lifecycle", false, false, true));
			}
			float value = 100f / (def.baseFertileCycles * 600f);
			this.fertileEffect = new Effect("Fertile", CREATURES.MODIFIERS.BASE_FERTILITY.NAME, CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP, 0f, false, false, false, null, -1f, 0f, null, "");
			this.fertileEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, value, CREATURES.MODIFIERS.BASE_FERTILITY.NAME, false, false, true));
			this.InitializeBreedingChances();
		}

		// Token: 0x06005C4A RID: 23626 RVA: 0x002A8930 File Offset: 0x002A6B30
		[OnDeserialized]
		private void OnDeserialized()
		{
			int num = (base.def.initialBreedingWeights != null) ? base.def.initialBreedingWeights.Count : 0;
			if (this.breedingChances.Count != num)
			{
				this.InitializeBreedingChances();
			}
		}

		// Token: 0x06005C4B RID: 23627 RVA: 0x002A8974 File Offset: 0x002A6B74
		private void InitializeBreedingChances()
		{
			this.breedingChances = new List<FertilityMonitor.BreedingChance>();
			if (base.def.initialBreedingWeights != null)
			{
				foreach (FertilityMonitor.BreedingChance breedingChance in base.def.initialBreedingWeights)
				{
					this.breedingChances.Add(new FertilityMonitor.BreedingChance
					{
						egg = breedingChance.egg,
						weight = breedingChance.weight
					});
					foreach (FertilityModifier fertilityModifier in Db.Get().FertilityModifiers.GetForTag(breedingChance.egg))
					{
						fertilityModifier.ApplyFunction(this, breedingChance.egg);
					}
				}
				this.NormalizeBreedingChances();
			}
		}

		// Token: 0x06005C4C RID: 23628 RVA: 0x002A8A6C File Offset: 0x002A6C6C
		public void ShowEgg()
		{
			if (this.egg != null)
			{
				bool flag;
				Vector3 vector = base.GetComponent<KBatchedAnimController>().GetSymbolTransform(FertilityMonitor.Instance.targetEggSymbol, out flag).MultiplyPoint3x4(Vector3.zero);
				if (flag)
				{
					vector.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
					int num = Grid.PosToCell(vector);
					if (Grid.IsValidCell(num) && !Grid.Solid[num])
					{
						this.egg.transform.SetPosition(vector);
					}
				}
				this.egg.SetActive(true);
				Db.Get().Amounts.Wildness.Copy(this.egg, base.gameObject);
				this.egg = null;
			}
		}

		// Token: 0x06005C4D RID: 23629 RVA: 0x002A8B1C File Offset: 0x002A6D1C
		public void LayEgg()
		{
			this.fertility.value = 0f;
			Vector3 position = base.smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
			Tag tag = FertilityMonitor.EggBreedingRoll(this.breedingChances, false);
			if (GenericGameSettings.instance.acceleratedLifecycle)
			{
				float num = 0f;
				foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
				{
					if (breedingChance.weight > num)
					{
						num = breedingChance.weight;
						tag = breedingChance.egg;
					}
				}
			}
			global::Debug.Assert(tag != Tag.Invalid, "Didn't pick an egg to lay. Weights weren't normalized?");
			GameObject prefab = Assets.GetPrefab(tag);
			GameObject gameObject = Util.KInstantiate(prefab, position);
			this.egg = gameObject;
			SymbolOverrideController component = base.GetComponent<SymbolOverrideController>();
			string str = "egg01";
			CreatureBrain component2 = Assets.GetPrefab(prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>();
			if (!string.IsNullOrEmpty(component2.symbolPrefix))
			{
				str = component2.symbolPrefix + "egg01";
			}
			KAnim.Build.Symbol symbol = this.egg.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol(str);
			if (symbol != null)
			{
				component.AddSymbolOverride(FertilityMonitor.Instance.targetEggSymbol, symbol, 0);
			}
			base.Trigger(1193600993, this.egg);
		}

		// Token: 0x06005C4E RID: 23630 RVA: 0x000E09CC File Offset: 0x000DEBCC
		public bool IsReadyToLayEgg()
		{
			return base.smi.fertility.value >= base.smi.fertility.GetMax();
		}

		// Token: 0x06005C4F RID: 23631 RVA: 0x002A8C94 File Offset: 0x002A6E94
		public void AddBreedingChance(Tag type, float addedPercentChance)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					float num = Mathf.Min(1f - breedingChance.weight, Mathf.Max(0f - breedingChance.weight, addedPercentChance));
					breedingChance.weight += num;
				}
			}
			this.NormalizeBreedingChances();
			base.master.Trigger(1059811075, this.breedingChances);
		}

		// Token: 0x06005C50 RID: 23632 RVA: 0x002A8D3C File Offset: 0x002A6F3C
		public float GetBreedingChance(Tag type)
		{
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				if (breedingChance.egg == type)
				{
					return breedingChance.weight;
				}
			}
			return -1f;
		}

		// Token: 0x06005C51 RID: 23633 RVA: 0x002A8DA8 File Offset: 0x002A6FA8
		public void NormalizeBreedingChances()
		{
			float num = 0f;
			foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
			{
				num += breedingChance.weight;
			}
			foreach (FertilityMonitor.BreedingChance breedingChance2 in this.breedingChances)
			{
				breedingChance2.weight /= num;
			}
		}

		// Token: 0x06005C52 RID: 23634 RVA: 0x000E09F3 File Offset: 0x000DEBF3
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			if (this.egg != null)
			{
				UnityEngine.Object.Destroy(this.egg);
				this.egg = null;
			}
		}

		// Token: 0x040041BB RID: 16827
		public AmountInstance fertility;

		// Token: 0x040041BC RID: 16828
		private GameObject egg;

		// Token: 0x040041BD RID: 16829
		[Serialize]
		public List<FertilityMonitor.BreedingChance> breedingChances;

		// Token: 0x040041BE RID: 16830
		public Effect fertileEffect;

		// Token: 0x040041BF RID: 16831
		private static HashedString targetEggSymbol = "snapto_egg";
	}
}
