using System;

// Token: 0x0200116C RID: 4460
[SkipSaveFileSerialization]
public class BlightVulnerable : StateMachineComponent<BlightVulnerable.StatesInstance>
{
	// Token: 0x06005AEC RID: 23276 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06005AED RID: 23277 RVA: 0x000DFA45 File Offset: 0x000DDC45
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06005AEE RID: 23278 RVA: 0x000DFA58 File Offset: 0x000DDC58
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06005AEF RID: 23279 RVA: 0x000DFA60 File Offset: 0x000DDC60
	public void MakeBlighted()
	{
		Debug.Log("Blighting plant", this);
		base.smi.sm.isBlighted.Set(true, base.smi, false);
	}

	// Token: 0x040040B9 RID: 16569
	private SchedulerHandle handle;

	// Token: 0x040040BA RID: 16570
	public bool prefersDarkness;

	// Token: 0x0200116D RID: 4461
	public class StatesInstance : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.GameInstance
	{
		// Token: 0x06005AF1 RID: 23281 RVA: 0x000DFA93 File Offset: 0x000DDC93
		public StatesInstance(BlightVulnerable master) : base(master)
		{
		}
	}

	// Token: 0x0200116E RID: 4462
	public class States : GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable>
	{
		// Token: 0x06005AF2 RID: 23282 RVA: 0x002A4CC8 File Offset: 0x002A2EC8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.comfortable;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.comfortable.ParamTransition<bool>(this.isBlighted, this.blighted, GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.IsTrue);
			this.blighted.TriggerOnEnter(GameHashes.BlightChanged, (BlightVulnerable.StatesInstance smi) => true).Enter(delegate(BlightVulnerable.StatesInstance smi)
			{
				smi.GetComponent<SeedProducer>().seedInfo.seedId = RotPileConfig.ID;
			}).ToggleTag(GameTags.Blighted).Exit(delegate(BlightVulnerable.StatesInstance smi)
			{
				GameplayEventManager.Instance.Trigger(-1425542080, smi.gameObject);
			});
		}

		// Token: 0x040040BB RID: 16571
		public StateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.BoolParameter isBlighted;

		// Token: 0x040040BC RID: 16572
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State comfortable;

		// Token: 0x040040BD RID: 16573
		public GameStateMachine<BlightVulnerable.States, BlightVulnerable.StatesInstance, BlightVulnerable, object>.State blighted;
	}
}
