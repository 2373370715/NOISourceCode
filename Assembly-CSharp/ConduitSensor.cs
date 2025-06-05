using System;

// Token: 0x02000D3D RID: 3389
public abstract class ConduitSensor : Switch
{
	// Token: 0x06004197 RID: 16791
	protected abstract void ConduitUpdate(float dt);

	// Token: 0x06004198 RID: 16792 RVA: 0x0024CBA4 File Offset: 0x0024ADA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
			return;
		}
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0024CC38 File Offset: 0x0024AE38
	protected override void OnCleanUp()
	{
		if (this.conduitType == ConduitType.Liquid || this.conduitType == ConduitType.Gas)
		{
			Conduit.GetFlowManager(this.conduitType).RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		else
		{
			SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		}
		base.OnCleanUp();
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x000CEE5B File Offset: 0x000CD05B
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x0024CC94 File Offset: 0x0024AE94
	protected virtual void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			if (this.switchedOn)
			{
				this.animController.Play(ConduitSensor.ON_ANIMS, KAnim.PlayMode.Loop);
				return;
			}
			this.animController.Play(ConduitSensor.OFF_ANIMS, KAnim.PlayMode.Once);
		}
	}

	// Token: 0x04002D54 RID: 11604
	public ConduitType conduitType;

	// Token: 0x04002D55 RID: 11605
	protected bool wasOn;

	// Token: 0x04002D56 RID: 11606
	protected KBatchedAnimController animController;

	// Token: 0x04002D57 RID: 11607
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on"
	};

	// Token: 0x04002D58 RID: 11608
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
