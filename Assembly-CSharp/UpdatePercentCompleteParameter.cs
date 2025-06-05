using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000BB3 RID: 2995
internal class UpdatePercentCompleteParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06003894 RID: 14484 RVA: 0x000C9247 File Offset: 0x000C7447
	public UpdatePercentCompleteParameter() : base("percentComplete")
	{
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0022952C File Offset: 0x0022772C
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdatePercentCompleteParameter.Entry item = new UpdatePercentCompleteParameter.Entry
		{
			worker = sound.transform.GetComponent<WorkerBase>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x00229588 File Offset: 0x00227788
	public override void Update(float dt)
	{
		foreach (UpdatePercentCompleteParameter.Entry entry in this.entries)
		{
			if (!(entry.worker == null))
			{
				Workable workable = entry.worker.GetWorkable();
				if (!(workable == null))
				{
					float percentComplete = workable.GetPercentComplete();
					EventInstance ev = entry.ev;
					ev.setParameterByID(entry.parameterId, percentComplete, false);
				}
			}
		}
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x00229618 File Offset: 0x00227818
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

	// Token: 0x04002706 RID: 9990
	private List<UpdatePercentCompleteParameter.Entry> entries = new List<UpdatePercentCompleteParameter.Entry>();

	// Token: 0x02000BB4 RID: 2996
	private struct Entry
	{
		// Token: 0x04002707 RID: 9991
		public WorkerBase worker;

		// Token: 0x04002708 RID: 9992
		public EventInstance ev;

		// Token: 0x04002709 RID: 9993
		public PARAMETER_ID parameterId;
	}
}
