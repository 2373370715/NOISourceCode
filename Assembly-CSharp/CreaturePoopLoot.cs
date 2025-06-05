using System;
using UnityEngine;

// Token: 0x02000A26 RID: 2598
public class CreaturePoopLoot : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>
{
	// Token: 0x06002F30 RID: 12080 RVA: 0x00205100 File Offset: 0x00203300
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Poop, this.roll, null);
		this.roll.Enter(new StateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State.Callback(CreaturePoopLoot.RollForLoot)).GoTo(this.idle);
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x00205158 File Offset: 0x00203358
	public static void RollForLoot(CreaturePoopLoot.Instance smi)
	{
		for (int i = 0; i < smi.def.Loot.Length; i++)
		{
			float value = UnityEngine.Random.value;
			CreaturePoopLoot.LootData lootData = smi.def.Loot[i];
			if (lootData.probability > 0f && value <= lootData.probability)
			{
				Tag tag = lootData.tag;
				Vector3 position = smi.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				Util.KInstantiate(Assets.GetPrefab(tag), position).SetActive(true);
			}
		}
	}

	// Token: 0x0400205C RID: 8284
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State idle;

	// Token: 0x0400205D RID: 8285
	public GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.State roll;

	// Token: 0x02000A27 RID: 2599
	public struct LootData
	{
		// Token: 0x0400205E RID: 8286
		public Tag tag;

		// Token: 0x0400205F RID: 8287
		public float probability;
	}

	// Token: 0x02000A28 RID: 2600
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04002060 RID: 8288
		public CreaturePoopLoot.LootData[] Loot;
	}

	// Token: 0x02000A29 RID: 2601
	public new class Instance : GameStateMachine<CreaturePoopLoot, CreaturePoopLoot.Instance, IStateMachineTarget, CreaturePoopLoot.Def>.GameInstance
	{
		// Token: 0x06002F34 RID: 12084 RVA: 0x000C30BF File Offset: 0x000C12BF
		public Instance(IStateMachineTarget master, CreaturePoopLoot.Def def) : base(master, def)
		{
		}
	}
}
