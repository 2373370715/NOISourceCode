using System;
using FMOD.Studio;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DCD RID: 7629
public class LockerMenuScreen : KModalScreen
{
	// Token: 0x06009F7D RID: 40829 RVA: 0x0010C354 File Offset: 0x0010A554
	protected override void OnActivate()
	{
		LockerMenuScreen.Instance = this;
		this.Show(false);
	}

	// Token: 0x06009F7E RID: 40830 RVA: 0x0010C363 File Offset: 0x0010A563
	public override float GetSortKey()
	{
		return 40f;
	}

	// Token: 0x06009F7F RID: 40831 RVA: 0x0010C36A File Offset: 0x0010A56A
	public void ShowInventoryScreen()
	{
		if (!base.isActiveAndEnabled)
		{
			this.Show(true);
		}
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.kleiInventoryScreen, null);
		MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "inventory", true);
	}

	// Token: 0x06009F80 RID: 40832 RVA: 0x003DF998 File Offset: 0x003DDB98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MultiToggle multiToggle = this.buttonInventory;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			this.ShowInventoryScreen();
		}));
		MultiToggle multiToggle2 = this.buttonDuplicants;
		multiToggle2.onClick = (System.Action)Delegate.Combine(multiToggle2.onClick, new System.Action(delegate()
		{
			MinionBrowserScreenConfig.Personalities(default(Option<Personality>)).ApplyAndOpenScreen(null);
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "dupe", true);
		}));
		MultiToggle multiToggle3 = this.buttonOutfitBroswer;
		multiToggle3.onClick = (System.Action)Delegate.Combine(multiToggle3.onClick, new System.Action(delegate()
		{
			OutfitBrowserScreenConfig.Mannequin().ApplyAndOpenScreen();
			MusicManager.instance.SetSongParameter("Music_SupplyCloset", "SupplyClosetView", "wardrobe", true);
		}));
		this.closeButton.onClick += delegate()
		{
			this.Show(false);
		};
		this.ConfigureHoverForButton(this.buttonInventory, UI.LOCKER_MENU.BUTTON_INVENTORY_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonDuplicants, UI.LOCKER_MENU.BUTTON_DUPLICANTS_DESCRIPTION, true);
		this.ConfigureHoverForButton(this.buttonOutfitBroswer, UI.LOCKER_MENU.BUTTON_OUTFITS_DESCRIPTION, true);
		this.descriptionArea.text = UI.LOCKER_MENU.DEFAULT_DESCRIPTION;
	}

	// Token: 0x06009F81 RID: 40833 RVA: 0x003DFAB8 File Offset: 0x003DDCB8
	private void ConfigureHoverForButton(MultiToggle toggle, string desc, bool useHoverColor = true)
	{
		LockerMenuScreen.<>c__DisplayClass17_0 CS$<>8__locals1 = new LockerMenuScreen.<>c__DisplayClass17_0();
		CS$<>8__locals1.useHoverColor = useHoverColor;
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.defaultColor = new Color(0.30980393f, 0.34117648f, 0.38431373f, 1f);
		CS$<>8__locals1.hoverColor = new Color(0.7019608f, 0.3647059f, 0.53333336f, 1f);
		toggle.onEnter = null;
		toggle.onExit = null;
		toggle.onEnter = (System.Action)Delegate.Combine(toggle.onEnter, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverEnterFn|0(toggle, desc));
		toggle.onExit = (System.Action)Delegate.Combine(toggle.onExit, CS$<>8__locals1.<ConfigureHoverForButton>g__OnHoverExitFn|1(toggle));
	}

	// Token: 0x06009F82 RID: 40834 RVA: 0x003DFB60 File Offset: 0x003DDD60
	public override void Show(bool show = true)
	{
		base.Show(show);
		if (show)
		{
			AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot);
			MusicManager.instance.OnSupplyClosetMenu(true, 0.5f);
			MusicManager.instance.PlaySong("Music_SupplyCloset", false);
			ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.TriggerShouldRefreshClaimItems), false);
		}
		else
		{
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		this.RefreshClaimItemsButton();
	}

	// Token: 0x06009F83 RID: 40835 RVA: 0x0010C3AA File Offset: 0x0010A5AA
	private void TriggerShouldRefreshClaimItems()
	{
		this.refreshRequested = true;
	}

	// Token: 0x06009F84 RID: 40836 RVA: 0x00107377 File Offset: 0x00105577
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06009F85 RID: 40837 RVA: 0x0010C3B3 File Offset: 0x0010A5B3
	protected override void OnForcedCleanUp()
	{
		base.OnForcedCleanUp();
	}

	// Token: 0x06009F86 RID: 40838 RVA: 0x003DFC14 File Offset: 0x003DDE14
	private void RefreshClaimItemsButton()
	{
		this.noConnectionIcon.SetActive(!ThreadedHttps<KleiAccount>.Instance.HasValidTicket());
		this.refreshRequested = false;
		bool hasClaimable = PermitItems.HasUnopenedItem();
		this.dropsAvailableNotification.SetActive(hasClaimable);
		this.buttonClaimItems.ChangeState(hasClaimable ? 0 : 1);
		this.buttonClaimItems.GetComponent<HierarchyReferences>().GetReference<Image>("FGIcon").material = (hasClaimable ? null : this.desatUIMaterial);
		this.buttonClaimItems.onClick = null;
		MultiToggle multiToggle = this.buttonClaimItems;
		multiToggle.onClick = (System.Action)Delegate.Combine(multiToggle.onClick, new System.Action(delegate()
		{
			if (!hasClaimable)
			{
				return;
			}
			UnityEngine.Object.FindObjectOfType<KleiItemDropScreen>(true).Show(true);
			this.Show(false);
		}));
		this.ConfigureHoverForButton(this.buttonClaimItems, hasClaimable ? UI.LOCKER_MENU.BUTTON_CLAIM_DESCRIPTION : UI.LOCKER_MENU.BUTTON_CLAIM_NONE_DESCRIPTION, hasClaimable);
	}

	// Token: 0x06009F87 RID: 40839 RVA: 0x003DFD0C File Offset: 0x003DDF0C
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.Show(false);
			AudioMixer.instance.Stop(AudioMixerSnapshots.Get().FrontEndSupplyClosetSnapshot, STOP_MODE.ALLOWFADEOUT);
			MusicManager.instance.OnSupplyClosetMenu(false, 1f);
			if (MusicManager.instance.SongIsPlaying("Music_SupplyCloset"))
			{
				MusicManager.instance.StopSong("Music_SupplyCloset", true, STOP_MODE.ALLOWFADEOUT);
			}
		}
		base.OnKeyDown(e);
	}

	// Token: 0x06009F88 RID: 40840 RVA: 0x0010C3BB File Offset: 0x0010A5BB
	private void Update()
	{
		if (this.refreshRequested)
		{
			this.RefreshClaimItemsButton();
		}
	}

	// Token: 0x04007D2B RID: 32043
	public static LockerMenuScreen Instance;

	// Token: 0x04007D2C RID: 32044
	[SerializeField]
	private MultiToggle buttonInventory;

	// Token: 0x04007D2D RID: 32045
	[SerializeField]
	private MultiToggle buttonDuplicants;

	// Token: 0x04007D2E RID: 32046
	[SerializeField]
	private MultiToggle buttonOutfitBroswer;

	// Token: 0x04007D2F RID: 32047
	[SerializeField]
	private MultiToggle buttonClaimItems;

	// Token: 0x04007D30 RID: 32048
	[SerializeField]
	private LocText descriptionArea;

	// Token: 0x04007D31 RID: 32049
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007D32 RID: 32050
	[SerializeField]
	private GameObject dropsAvailableNotification;

	// Token: 0x04007D33 RID: 32051
	[SerializeField]
	private GameObject noConnectionIcon;

	// Token: 0x04007D34 RID: 32052
	private const string LOCKER_MENU_MUSIC = "Music_SupplyCloset";

	// Token: 0x04007D35 RID: 32053
	private const string MUSIC_PARAMETER = "SupplyClosetView";

	// Token: 0x04007D36 RID: 32054
	[SerializeField]
	private Material desatUIMaterial;

	// Token: 0x04007D37 RID: 32055
	private bool refreshRequested;
}
