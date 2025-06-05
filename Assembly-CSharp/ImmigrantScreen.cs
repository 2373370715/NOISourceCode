using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;

// Token: 0x02001B28 RID: 6952
public class ImmigrantScreen : CharacterSelectionController
{
	// Token: 0x06009198 RID: 37272 RVA: 0x00103A03 File Offset: 0x00101C03
	public static void DestroyInstance()
	{
		ImmigrantScreen.instance = null;
	}

	// Token: 0x170009A4 RID: 2468
	// (get) Token: 0x06009199 RID: 37273 RVA: 0x00103A0B File Offset: 0x00101C0B
	public Telepad Telepad
	{
		get
		{
			return this.telepad;
		}
	}

	// Token: 0x0600919A RID: 37274 RVA: 0x00103A13 File Offset: 0x00101C13
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600919B RID: 37275 RVA: 0x0038E0AC File Offset: 0x0038C2AC
	protected override void OnSpawn()
	{
		this.activateOnSpawn = false;
		base.ConsumeMouseScroll = false;
		base.OnSpawn();
		base.IsStarterMinion = false;
		this.rejectButton.onClick += this.OnRejectAll;
		this.confirmRejectionBtn.onClick += this.OnRejectionConfirmed;
		this.cancelRejectionBtn.onClick += this.OnRejectionCancelled;
		ImmigrantScreen.instance = this;
		this.title.text = UI.IMMIGRANTSCREEN.IMMIGRANTSCREENTITLE;
		this.proceedButton.GetComponentInChildren<LocText>().text = UI.IMMIGRANTSCREEN.PROCEEDBUTTON;
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.Show(false);
	}

	// Token: 0x0600919C RID: 37276 RVA: 0x0038E16C File Offset: 0x0038C36C
	protected override void OnShow(bool show)
	{
		if (show)
		{
			KFMOD.PlayUISound(GlobalAssets.GetSound("Dialog_Popup", false));
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot);
			MusicManager.instance.PlaySong("Music_SelectDuplicant", false);
			this.hasShown = true;
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
			if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
			{
				MusicManager.instance.StopSong("Music_SelectDuplicant", true, STOP_MODE.ALLOWFADEOUT);
			}
			if (Immigration.Instance.ImmigrantsAvailable && this.hasShown)
			{
				AudioMixer.instance.Start(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot);
			}
		}
		base.OnShow(show);
	}

	// Token: 0x0600919D RID: 37277 RVA: 0x00103A1B File Offset: 0x00101C1B
	public void DebugShuffleOptions()
	{
		this.OnRejectionConfirmed();
		Immigration.Instance.timeBeforeSpawn = 0f;
	}

	// Token: 0x0600919E RID: 37278 RVA: 0x00103A32 File Offset: 0x00101C32
	public override void OnPressBack()
	{
		if (this.rejectConfirmationScreen.activeSelf)
		{
			this.OnRejectionCancelled();
			return;
		}
		base.OnPressBack();
	}

	// Token: 0x0600919F RID: 37279 RVA: 0x00103A4E File Offset: 0x00101C4E
	public override void Deactivate()
	{
		this.Show(false);
	}

	// Token: 0x060091A0 RID: 37280 RVA: 0x00103A57 File Offset: 0x00101C57
	public static void InitializeImmigrantScreen(Telepad telepad)
	{
		ImmigrantScreen.instance.Initialize(telepad);
		ImmigrantScreen.instance.Show(true);
	}

	// Token: 0x060091A1 RID: 37281 RVA: 0x0038E224 File Offset: 0x0038C424
	private void Initialize(Telepad telepad)
	{
		this.InitializeContainers();
		foreach (ITelepadDeliverableContainer telepadDeliverableContainer in this.containers)
		{
			CharacterContainer characterContainer = telepadDeliverableContainer as CharacterContainer;
			if (characterContainer != null)
			{
				characterContainer.SetReshufflingState(false);
			}
		}
		this.telepad = telepad;
	}

	// Token: 0x060091A2 RID: 37282 RVA: 0x0038E294 File Offset: 0x0038C494
	protected override void OnProceed()
	{
		this.telepad.OnAcceptDelivery(this.selectedDeliverables[0]);
		this.Show(false);
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
		MusicManager.instance.PlaySong("Stinger_NewDuplicant", false);
	}

	// Token: 0x060091A3 RID: 37283 RVA: 0x00103A6F File Offset: 0x00101C6F
	private void OnRejectAll()
	{
		this.rejectConfirmationScreen.transform.SetAsLastSibling();
		this.rejectConfirmationScreen.SetActive(true);
	}

	// Token: 0x060091A4 RID: 37284 RVA: 0x00103A8D File Offset: 0x00101C8D
	private void OnRejectionCancelled()
	{
		this.rejectConfirmationScreen.SetActive(false);
	}

	// Token: 0x060091A5 RID: 37285 RVA: 0x0038E330 File Offset: 0x0038C530
	private void OnRejectionConfirmed()
	{
		this.telepad.RejectAll();
		this.containers.ForEach(delegate(ITelepadDeliverableContainer cc)
		{
			UnityEngine.Object.Destroy(cc.GetGameObject());
		});
		this.containers.Clear();
		this.rejectConfirmationScreen.SetActive(false);
		this.Show(false);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().MENUNewDuplicantSnapshot, STOP_MODE.ALLOWFADEOUT);
		AudioMixer.instance.Stop(AudioMixerSnapshots.Get().PortalLPDimmedSnapshot, STOP_MODE.ALLOWFADEOUT);
	}

	// Token: 0x04006E45 RID: 28229
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04006E46 RID: 28230
	[SerializeField]
	private KButton rejectButton;

	// Token: 0x04006E47 RID: 28231
	[SerializeField]
	private LocText title;

	// Token: 0x04006E48 RID: 28232
	[SerializeField]
	private GameObject rejectConfirmationScreen;

	// Token: 0x04006E49 RID: 28233
	[SerializeField]
	private KButton confirmRejectionBtn;

	// Token: 0x04006E4A RID: 28234
	[SerializeField]
	private KButton cancelRejectionBtn;

	// Token: 0x04006E4B RID: 28235
	public static ImmigrantScreen instance;

	// Token: 0x04006E4C RID: 28236
	private Telepad telepad;

	// Token: 0x04006E4D RID: 28237
	private bool hasShown;
}
