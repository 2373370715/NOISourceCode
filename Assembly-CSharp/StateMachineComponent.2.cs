using System;
using KSerialization;

// Token: 0x0200091F RID: 2335
[SerializationConfig(MemberSerialization.OptIn)]
public class StateMachineComponent<StateMachineInstanceType> : StateMachineComponent, ISaveLoadable where StateMachineInstanceType : StateMachine.Instance
{
	// Token: 0x17000135 RID: 309
	// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000BF007 File Offset: 0x000BD207
	public StateMachineInstanceType smi
	{
		get
		{
			if (this._smi == null)
			{
				this._smi = (StateMachineInstanceType)((object)Activator.CreateInstance(typeof(StateMachineInstanceType), new object[]
				{
					this
				}));
			}
			return this._smi;
		}
	}

	// Token: 0x060028E8 RID: 10472 RVA: 0x000BF040 File Offset: 0x000BD240
	public override StateMachine.Instance GetSMI()
	{
		return this._smi;
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x000BF04D File Offset: 0x000BD24D
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnCleanUp");
			this._smi = default(StateMachineInstanceType);
		}
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x000BF083 File Offset: 0x000BD283
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		if (base.isSpawned)
		{
			this.smi.StartSM();
		}
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x000BF0A3 File Offset: 0x000BD2A3
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this._smi != null)
		{
			this._smi.StopSM("StateMachineComponent.OnDisable");
		}
	}

	// Token: 0x04001BE3 RID: 7139
	private StateMachineInstanceType _smi;
}
