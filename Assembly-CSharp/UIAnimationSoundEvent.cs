using System;

// Token: 0x0200096B RID: 2411
public class UIAnimationSoundEvent : SoundEvent
{
	// Token: 0x06002B10 RID: 11024 RVA: 0x000BF67B File Offset: 0x000BD87B
	public UIAnimationSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06002B11 RID: 11025 RVA: 0x000C06BA File Offset: 0x000BE8BA
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06002B12 RID: 11026 RVA: 0x001E9D08 File Offset: 0x001E7F08
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", base.sound, behaviour.name)
				});
				return;
			}
		}
		else
		{
			try
			{
				if (SoundListenerController.Instance == null)
				{
					KFMOD.PlayUISound(base.sound);
				}
				else
				{
					KFMOD.PlayOneShot(base.sound, SoundListenerController.Instance.transform.GetPosition(), 1f);
				}
			}
			catch
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"AUDIOERROR: Missing [" + base.sound + "]"
				});
			}
		}
	}

	// Token: 0x06002B13 RID: 11027 RVA: 0x001E2BFC File Offset: 0x001E0DFC
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null)
			{
				component.StopSound(base.sound);
			}
		}
	}
}
