using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000FEA RID: 4074
public class StaterpillarGenerator : Generator
{
	// Token: 0x060051FE RID: 20990 RVA: 0x002818C0 File Offset: 0x0027FAC0
	protected override void OnSpawn()
	{
		Staterpillar staterpillar = this.parent.Get();
		if (staterpillar == null || staterpillar.GetGenerator() != this)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		this.smi = new StaterpillarGenerator.StatesInstance(this);
		this.smi.StartSM();
		base.OnSpawn();
	}

	// Token: 0x060051FF RID: 20991 RVA: 0x0028191C File Offset: 0x0027FB1C
	public override void EnergySim200ms(float dt)
	{
		base.EnergySim200ms(dt);
		ushort circuitID = base.CircuitID;
		this.operational.SetFlag(Generator.wireConnectedFlag, circuitID != ushort.MaxValue);
		if (!this.operational.IsOperational)
		{
			return;
		}
		float num = base.GetComponent<Generator>().WattageRating;
		if (num > 0f)
		{
			num *= dt;
			num = Mathf.Max(num, 1f * dt);
			base.GenerateJoules(num, false);
		}
	}

	// Token: 0x040039CD RID: 14797
	private StaterpillarGenerator.StatesInstance smi;

	// Token: 0x040039CE RID: 14798
	[Serialize]
	public Ref<Staterpillar> parent = new Ref<Staterpillar>();

	// Token: 0x02000FEB RID: 4075
	public class StatesInstance : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.GameInstance
	{
		// Token: 0x06005201 RID: 20993 RVA: 0x000D9DEF File Offset: 0x000D7FEF
		public StatesInstance(StaterpillarGenerator master) : base(master)
		{
		}

		// Token: 0x040039CF RID: 14799
		private Attributes attributes;
	}

	// Token: 0x02000FEC RID: 4076
	public class States : GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator>
	{
		// Token: 0x06005202 RID: 20994 RVA: 0x00281990 File Offset: 0x0027FB90
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			this.root.EventTransition(GameHashes.OperationalChanged, this.idle, (StaterpillarGenerator.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.idle.EventTransition(GameHashes.OperationalChanged, this.root, (StaterpillarGenerator.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(StaterpillarGenerator.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
		}

		// Token: 0x040039D0 RID: 14800
		public GameStateMachine<StaterpillarGenerator.States, StaterpillarGenerator.StatesInstance, StaterpillarGenerator, object>.State idle;
	}
}
