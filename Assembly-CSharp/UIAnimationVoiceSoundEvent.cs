using System;
using UnityEngine;

// Token: 0x0200096C RID: 2412
public class UIAnimationVoiceSoundEvent : SoundEvent
{
	// Token: 0x06002B14 RID: 11028 RVA: 0x000C06C3 File Offset: 0x000BE8C3
	public UIAnimationVoiceSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, false, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
		this.actualSoundName = sound_name;
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000C06BA File Offset: 0x000BE8BA
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		this.PlaySound(behaviour);
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x001E9DF4 File Offset: 0x001E7FF4
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		string soundPath = MinionVoice.ByObject(behaviour.controller).UnwrapOr(MinionVoice.Random(), string.Format("Couldn't find MinionVoice on UI {0}, falling back to random voice", behaviour.controller)).GetSoundPath(this.actualSoundName);
		if (this.actualSoundName.Contains(":"))
		{
			float num = float.Parse(this.actualSoundName.Split(':', StringSplitOptions.None)[1]);
			if ((float)UnityEngine.Random.Range(0, 100) > num)
			{
				return;
			}
		}
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (UI Object) is missing LoopingSounds component.");
			}
			else if (!component.StartSound(soundPath, false, false, false))
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					string.Format("SoundEvent has invalid sound [{0}] on behaviour [{1}]", soundPath, behaviour.name)
				});
			}
			this.lastPlayedLoopingSoundPath = soundPath;
			return;
		}
		try
		{
			if (SoundListenerController.Instance == null)
			{
				KFMOD.PlayUISound(soundPath);
			}
			else
			{
				KFMOD.PlayOneShot(soundPath, SoundListenerController.Instance.transform.GetPosition(), 1f);
			}
		}
		catch
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"AUDIOERROR: Missing [" + soundPath + "]"
			});
		}
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x001E9F38 File Offset: 0x001E8138
	public override void Stop(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component != null && this.lastPlayedLoopingSoundPath != null)
			{
				component.StopSound(this.lastPlayedLoopingSoundPath);
			}
		}
		this.lastPlayedLoopingSoundPath = null;
	}

	// Token: 0x04001D34 RID: 7476
	private string actualSoundName;

	// Token: 0x04001D35 RID: 7477
	private string lastPlayedLoopingSoundPath;
}
