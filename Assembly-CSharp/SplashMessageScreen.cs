using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200207A RID: 8314
[AddComponentMenu("KMonoBehaviour/scripts/SplashMessageScreen")]
public class SplashMessageScreen : KMonoBehaviour
{
	// Token: 0x0600B102 RID: 45314 RVA: 0x00434C00 File Offset: 0x00432E00
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.forumButton.onClick += delegate()
		{
			App.OpenWebURL("https://forums.kleientertainment.com/forums/forum/118-oxygen-not-included/");
		};
		this.confirmButton.onClick += delegate()
		{
			base.gameObject.SetActive(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
		};
		this.bodyText.text = UI.DEVELOPMENTBUILDS.ALPHA.LOADING.BODY;
	}

	// Token: 0x0600B103 RID: 45315 RVA: 0x00117A66 File Offset: 0x00115C66
	private void OnEnable()
	{
		this.confirmButton.GetComponent<LayoutElement>();
		this.confirmButton.GetComponentInChildren<LocText>();
	}

	// Token: 0x0600B104 RID: 45316 RVA: 0x00117A80 File Offset: 0x00115C80
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (!DlcManager.IsExpansion1Active())
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
	}

	// Token: 0x04008B66 RID: 35686
	public KButton forumButton;

	// Token: 0x04008B67 RID: 35687
	public KButton confirmButton;

	// Token: 0x04008B68 RID: 35688
	public LocText bodyText;

	// Token: 0x04008B69 RID: 35689
	public bool previewInEditor;
}
