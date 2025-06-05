using System;
using System.Collections.Generic;
using FMOD.Studio;

// Token: 0x02001105 RID: 4357
internal class UpdateDistanceToImpactParameter : LoopingSoundParameterUpdater
{
	// Token: 0x06005902 RID: 22786 RVA: 0x000DE735 File Offset: 0x000DC935
	public UpdateDistanceToImpactParameter() : base("distanceToImpact")
	{
	}

	// Token: 0x06005903 RID: 22787 RVA: 0x0029C194 File Offset: 0x0029A394
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		UpdateDistanceToImpactParameter.Entry item = new UpdateDistanceToImpactParameter.Entry
		{
			comet = sound.transform.GetComponent<Comet>(),
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x06005904 RID: 22788 RVA: 0x0029C1F0 File Offset: 0x0029A3F0
	public override void Update(float dt)
	{
		foreach (UpdateDistanceToImpactParameter.Entry entry in this.entries)
		{
			if (!(entry.comet == null))
			{
				float soundDistance = entry.comet.GetSoundDistance();
				EventInstance ev = entry.ev;
				ev.setParameterByID(entry.parameterId, soundDistance, false);
			}
		}
	}

	// Token: 0x06005905 RID: 22789 RVA: 0x0029C270 File Offset: 0x0029A470
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

	// Token: 0x04003EE1 RID: 16097
	private List<UpdateDistanceToImpactParameter.Entry> entries = new List<UpdateDistanceToImpactParameter.Entry>();

	// Token: 0x02001106 RID: 4358
	private struct Entry
	{
		// Token: 0x04003EE2 RID: 16098
		public Comet comet;

		// Token: 0x04003EE3 RID: 16099
		public EventInstance ev;

		// Token: 0x04003EE4 RID: 16100
		public PARAMETER_ID parameterId;
	}
}
