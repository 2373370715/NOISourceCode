using System;
using UnityEngine;

// Token: 0x02001DE3 RID: 7651
[AddComponentMenu("KMonoBehaviour/scripts/MainMenuIntroShort")]
public class MainMenuIntroShort : KMonoBehaviour
{
	// Token: 0x06009FFA RID: 40954 RVA: 0x003E18CC File Offset: 0x003DFACC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		string @string = KPlayerPrefs.GetString("PlayShortOnLaunch", "");
		if (!string.IsNullOrEmpty(MainMenu.Instance.IntroShortName) && @string != MainMenu.Instance.IntroShortName)
		{
			VideoScreen component = KScreenManager.AddChild(FrontEndManager.Instance.gameObject, ScreenPrefabs.Instance.VideoScreen.gameObject).GetComponent<VideoScreen>();
			component.PlayVideo(Assets.GetVideo(MainMenu.Instance.IntroShortName), false, AudioMixerSnapshots.Get().MainMenuVideoPlayingSnapshot, false, true);
			component.OnStop = (System.Action)Delegate.Combine(component.OnStop, new System.Action(delegate()
			{
				KPlayerPrefs.SetString("PlayShortOnLaunch", MainMenu.Instance.IntroShortName);
				base.gameObject.SetActive(false);
			}));
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x04007DA7 RID: 32167
	[SerializeField]
	private bool alwaysPlay;
}
