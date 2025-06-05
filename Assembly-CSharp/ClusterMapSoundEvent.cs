using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000938 RID: 2360
public class ClusterMapSoundEvent : SoundEvent
{
	// Token: 0x06002970 RID: 10608 RVA: 0x000BF67B File Offset: 0x000BD87B
	public ClusterMapSoundEvent(string file_name, string sound_name, int frame, bool looping) : base(file_name, sound_name, frame, true, looping, (float)SoundEvent.IGNORE_INTERVAL, false)
	{
	}

	// Token: 0x06002971 RID: 10609 RVA: 0x000BF690 File Offset: 0x000BD890
	public override void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
		if (ClusterMapScreen.Instance.IsActive())
		{
			this.PlaySound(behaviour);
		}
	}

	// Token: 0x06002972 RID: 10610 RVA: 0x001E2AFC File Offset: 0x001E0CFC
	public override void PlaySound(AnimEventManager.EventPlayerData behaviour)
	{
		if (base.looping)
		{
			LoopingSounds component = behaviour.GetComponent<LoopingSounds>();
			if (component == null)
			{
				global::Debug.Log(behaviour.name + " (Cluster Map Object) is missing LoopingSounds component.");
				return;
			}
			if (!component.StartSound(base.sound, true, false, false))
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
			EventInstance instance = KFMOD.BeginOneShot(base.sound, Vector3.zero, 1f);
			instance.setParameterByName(ClusterMapSoundEvent.X_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().x / (float)Screen.width, false);
			instance.setParameterByName(ClusterMapSoundEvent.Y_POSITION_PARAMETER, behaviour.controller.transform.GetPosition().y / (float)Screen.height, false);
			instance.setParameterByName(ClusterMapSoundEvent.ZOOM_PARAMETER, ClusterMapScreen.Instance.CurrentZoomPercentage(), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x001E2BFC File Offset: 0x001E0DFC
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

	// Token: 0x04001C36 RID: 7222
	private static string X_POSITION_PARAMETER = "Starmap_Position_X";

	// Token: 0x04001C37 RID: 7223
	private static string Y_POSITION_PARAMETER = "Starmap_Position_Y";

	// Token: 0x04001C38 RID: 7224
	private static string ZOOM_PARAMETER = "Starmap_Zoom_Percentage";
}
