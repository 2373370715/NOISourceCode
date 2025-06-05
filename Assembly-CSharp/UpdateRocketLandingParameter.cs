using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x0200195B RID: 6491
internal class UpdateRocketLandingParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06008718 RID: 34584 RVA: 0x000FD120 File Offset: 0x000FB320
	public UpdateRocketLandingParameter() : base("rocketLanding")
	{
	}

	// Token: 0x06008719 RID: 34585 RVA: 0x0035CF24 File Offset: 0x0035B124
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateRocketLandingParameter.Entry item = new UpdateRocketLandingParameter.Entry
		{
			rocketModule = sound.transform.GetComponent<RocketModule>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600871A RID: 34586 RVA: 0x0035CF80 File Offset: 0x0035B180
	public override void Update(float dt)
	{
		foreach (UpdateRocketLandingParameter.Entry entry in this.entries)
		{
			if (!(entry.rocketModule == null))
			{
				LaunchConditionManager conditionManager = entry.rocketModule.conditionManager;
				if (!(conditionManager == null))
				{
					ILaunchableRocket component = conditionManager.GetComponent<ILaunchableRocket>();
					if (component != null)
					{
						if (component.isLanding)
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 1f, false);
						}
						else
						{
							EventInstance ev = entry.ev;
							ev.setParameterByID(entry.parameterId, 0f, false);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600871B RID: 34587 RVA: 0x0035D03C File Offset: 0x0035B23C
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

	// Token: 0x04006667 RID: 26215
	private List<UpdateRocketLandingParameter.Entry> entries = new List<UpdateRocketLandingParameter.Entry>();

	// Token: 0x0200195C RID: 6492
	private struct Entry
	{
		// Token: 0x04006668 RID: 26216
		public RocketModule rocketModule;

		// Token: 0x04006669 RID: 26217
		public EventInstance ev;

		// Token: 0x0400666A RID: 26218
		public PARAMETER_ID parameterId;
	}
}
