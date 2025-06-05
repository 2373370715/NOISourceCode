using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02002077 RID: 8311
public class SpeedLoopingSoundUpdater : LoopingSoundParameterUpdater
{
	// Token: 0x0600B0FB RID: 45307 RVA: 0x00117A03 File Offset: 0x00115C03
	public SpeedLoopingSoundUpdater() : base("Speed")
	{
	}

	// Token: 0x0600B0FC RID: 45308 RVA: 0x00434AF0 File Offset: 0x00432CF0
	public override void Add(LoopingSoundParameterUpdater.Sound sound)
	{
		SpeedLoopingSoundUpdater.Entry item = new SpeedLoopingSoundUpdater.Entry
		{
			ev = sound.ev,
			parameterId = sound.description.GetParameterId(base.parameter)
		};
		this.entries.Add(item);
	}

	// Token: 0x0600B0FD RID: 45309 RVA: 0x00434B3C File Offset: 0x00432D3C
	public override void Update(float dt)
	{
		float speedParameterValue = SpeedLoopingSoundUpdater.GetSpeedParameterValue();
		foreach (SpeedLoopingSoundUpdater.Entry entry in this.entries)
		{
			EventInstance ev = entry.ev;
			ev.setParameterByID(entry.parameterId, speedParameterValue, false);
		}
	}

	// Token: 0x0600B0FE RID: 45310 RVA: 0x00434BA8 File Offset: 0x00432DA8
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

	// Token: 0x0600B0FF RID: 45311 RVA: 0x00117A20 File Offset: 0x00115C20
	public static float GetSpeedParameterValue()
	{
		return Time.timeScale * 1f;
	}

	// Token: 0x04008B63 RID: 35683
	private List<SpeedLoopingSoundUpdater.Entry> entries = new List<SpeedLoopingSoundUpdater.Entry>();

	// Token: 0x02002078 RID: 8312
	private struct Entry
	{
		// Token: 0x04008B64 RID: 35684
		public EventInstance ev;

		// Token: 0x04008B65 RID: 35685
		public PARAMETER_ID parameterId;
	}
}
