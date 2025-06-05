using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02000A20 RID: 2592
internal class UpdateConsumedMassParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06002F21 RID: 12065 RVA: 0x000C3013 File Offset: 0x000C1213
	public UpdateConsumedMassParameter() : base("consumedMass")
	{
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x00204D84 File Offset: 0x00202F84
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateConsumedMassParameter.Entry item = new UpdateConsumedMassParameter.Entry
		{
			creatureCalorieMonitor = sound.transform.GetSMI<CreatureCalorieMonitor.Instance>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x00204DE0 File Offset: 0x00202FE0
	public override void Update(float dt)
	{
		foreach (UpdateConsumedMassParameter.Entry entry in this.entries)
		{
			if (!entry.creatureCalorieMonitor.IsNullOrStopped())
			{
				float fullness = entry.creatureCalorieMonitor.stomach.GetFullness();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, fullness, false);
			}
		}
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x00204E64 File Offset: 0x00203064
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

	// Token: 0x0400204D RID: 8269
	private List<UpdateConsumedMassParameter.Entry> entries = new List<UpdateConsumedMassParameter.Entry>();

	// Token: 0x02000A21 RID: 2593
	private struct Entry
	{
		// Token: 0x0400204E RID: 8270
		public CreatureCalorieMonitor.Instance creatureCalorieMonitor;

		// Token: 0x0400204F RID: 8271
		public EventInstance ev;

		// Token: 0x04002050 RID: 8272
		public PARAMETER_ID parameterId;
	}
}
