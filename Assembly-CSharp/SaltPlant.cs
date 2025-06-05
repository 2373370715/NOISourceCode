using System;

// Token: 0x02001746 RID: 5958
public class SaltPlant : StateMachineComponent<SaltPlant.StatesInstance>
{
	// Token: 0x06007A8B RID: 31371 RVA: 0x000F5280 File Offset: 0x000F3480
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SaltPlant>(-724860998, SaltPlant.OnWiltDelegate);
		base.Subscribe<SaltPlant>(712767498, SaltPlant.OnWiltRecoverDelegate);
	}

	// Token: 0x06007A8C RID: 31372 RVA: 0x000F52AA File Offset: 0x000F34AA
	private void OnWilt(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x06007A8D RID: 31373 RVA: 0x000F52BD File Offset: 0x000F34BD
	private void OnWiltRecover(object data = null)
	{
		base.gameObject.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x04005C24 RID: 23588
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWilt(data);
	});

	// Token: 0x04005C25 RID: 23589
	private static readonly EventSystem.IntraObjectHandler<SaltPlant> OnWiltRecoverDelegate = new EventSystem.IntraObjectHandler<SaltPlant>(delegate(SaltPlant component, object data)
	{
		component.OnWiltRecover(data);
	});

	// Token: 0x02001747 RID: 5959
	public class StatesInstance : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.GameInstance
	{
		// Token: 0x06007A90 RID: 31376 RVA: 0x000F530E File Offset: 0x000F350E
		public StatesInstance(SaltPlant master) : base(master)
		{
		}
	}

	// Token: 0x02001748 RID: 5960
	public class States : GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant>
	{
		// Token: 0x06007A91 RID: 31377 RVA: 0x000F5317 File Offset: 0x000F3517
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			default_state = this.alive;
			this.alive.DoNothing();
		}

		// Token: 0x04005C26 RID: 23590
		public GameStateMachine<SaltPlant.States, SaltPlant.StatesInstance, SaltPlant, object>.State alive;
	}
}
