using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200195D RID: 6493
internal class UpdateRocketSpeedParameter : LoopingSoundParameterUpdater
{
	// Token: 0x0600871C RID: 34588 RVA: 0x000FD13D File Offset: 0x000FB33D
	public UpdateRocketSpeedParameter() : base("rocketSpeed")
	{
	}

	// Token: 0x0600871D RID: 34589 RVA: 0x0035D094 File Offset: 0x0035B294
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketSpeedParameter.Entry item = new UpdateRocketSpeedParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600871E RID: 34590 RVA: 0x0035D0F0 File Offset: 0x0035B2F0
	public override void Update(float dt)
	{
		foreach (UpdateRocketSpeedParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						EventInstance ev = entry.ev;
						ev.setParameterByID(entry.parameterId, component.rocketSpeed, false);
					}
				}
			}
		}
	}

	// Token: 0x0600871F RID: 34591 RVA: 0x0035D188 File Offset: 0x0035B388
	public override void Remove(LoopingSoundParameterUpdater.Sound sound)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			if (this.entries[i].ev.handle == sound.ev.handle)
			{
				this.entries.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x0400666B RID: 26219
	private List<UpdateRocketSpeedParameter.Entry> entries = new List<UpdateRocketSpeedParameter.Entry>();

	// Token: 0x0200195E RID: 6494
	private struct Entry
	{
		// Token: 0x0400666C RID: 26220
		public RocketModule rocketModule;

		// Token: 0x0400666D RID: 26221
		public EventInstance ev;

		// Token: 0x0400666E RID: 26222
		public PARAMETER_ID parameterId;
	}
}
