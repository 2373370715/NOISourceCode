using System;
using System.Linq;
using Database;
using UnityEngine;

// Token: 0x02001DB9 RID: 7609
public class KleiPermitDioramaVis_JoyResponseBalloon : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	// Token: 0x06009EEE RID: 40686 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x06009EEF RID: 40687 RVA: 0x003DDFF4 File Offset: 0x003DC1F4
	public void ConfigureSetup()
	{
		this.minionUI.transform.localScale = Vector3.one * 0.7f;
		this.minionUI.transform.localPosition = new Vector3(this.minionUI.transform.localPosition.x - 73f, this.minionUI.transform.localPosition.y - 152f + 8f, this.minionUI.transform.localPosition.z);
	}

	// Token: 0x06009EF0 RID: 40688 RVA: 0x0010BCB3 File Offset: 0x00109EB3
	public void ConfigureWith(PermitResource permit)
	{
		this.ConfigureWith(Option.Some<BalloonArtistFacadeResource>((BalloonArtistFacadeResource)permit));
	}

	// Token: 0x06009EF1 RID: 40689 RVA: 0x003DE088 File Offset: 0x003DC288
	public void ConfigureWith(Option<BalloonArtistFacadeResource> permit)
	{
		KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0 CS$<>8__locals1 = new KleiPermitDioramaVis_JoyResponseBalloon.<>c__DisplayClass10_0();
		CS$<>8__locals1.permit = permit;
		KBatchedAnimController component = this.minionUI.SpawnedAvatar.GetComponent<KBatchedAnimController>();
		CS$<>8__locals1.minionSymbolOverrider = this.minionUI.SpawnedAvatar.GetComponent<SymbolOverrideController>();
		this.minionUI.SetMinion(this.specificPersonality.UnwrapOrElse(() => (from p in Db.Get().Personalities.GetAll(true, true)
		where p.joyTrait == "BalloonArtist"
		select p).GetRandom<Personality>(), null));
		if (!this.didAddAnims)
		{
			this.didAddAnims = true;
			component.AddAnimOverrides(Assets.GetAnim("anim_interacts_balloon_artist_kanim"), 0f);
		}
		component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		CS$<>8__locals1.<ConfigureWith>g__DisplayNextBalloon|3();
		Updater[] array = new Updater[2];
		array[0] = Updater.WaitForSeconds(1.3f);
		int num = 1;
		Func<Updater>[] array2 = new Func<Updater>[2];
		array2[0] = (() => Updater.WaitForSeconds(1.618f));
		array2[1] = (() => Updater.Do(new System.Action(base.<ConfigureWith>g__DisplayNextBalloon|3)));
		array[num] = Updater.Loop(array2);
		this.QueueUpdater(Updater.Series(array));
	}

	// Token: 0x06009EF2 RID: 40690 RVA: 0x0010BCC6 File Offset: 0x00109EC6
	public void SetMinion(Personality personality)
	{
		this.specificPersonality = personality;
		if (base.gameObject.activeInHierarchy)
		{
			this.minionUI.SetMinion(personality);
		}
	}

	// Token: 0x06009EF3 RID: 40691 RVA: 0x0010BCED File Offset: 0x00109EED
	private void QueueUpdater(Updater updater)
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.RunUpdater(updater);
			return;
		}
		this.updaterToRunOnStart = updater;
	}

	// Token: 0x06009EF4 RID: 40692 RVA: 0x0010BD10 File Offset: 0x00109F10
	private void RunUpdater(Updater updater)
	{
		if (this.updaterRoutine != null)
		{
			base.StopCoroutine(this.updaterRoutine);
			this.updaterRoutine = null;
		}
		this.updaterRoutine = base.StartCoroutine(updater);
	}

	// Token: 0x06009EF5 RID: 40693 RVA: 0x0010BD3F File Offset: 0x00109F3F
	private void OnEnable()
	{
		if (this.updaterToRunOnStart.IsSome())
		{
			this.RunUpdater(this.updaterToRunOnStart.Unwrap());
			this.updaterToRunOnStart = Option.None;
		}
	}

	// Token: 0x04007CD3 RID: 31955
	private const int FRAMES_TO_MAKE_BALLOON_IN_ANIM = 39;

	// Token: 0x04007CD4 RID: 31956
	private const float SECONDS_TO_MAKE_BALLOON_IN_ANIM = 1.3f;

	// Token: 0x04007CD5 RID: 31957
	private const float SECONDS_BETWEEN_BALLOONS = 1.618f;

	// Token: 0x04007CD6 RID: 31958
	[SerializeField]
	private UIMinion minionUI;

	// Token: 0x04007CD7 RID: 31959
	private bool didAddAnims;

	// Token: 0x04007CD8 RID: 31960
	private const string TARGET_SYMBOL_TO_OVERRIDE = "body";

	// Token: 0x04007CD9 RID: 31961
	private const int TARGET_OVERRIDE_PRIORITY = 0;

	// Token: 0x04007CDA RID: 31962
	private Option<Personality> specificPersonality;

	// Token: 0x04007CDB RID: 31963
	private Option<PermitResource> lastConfiguredPermit;

	// Token: 0x04007CDC RID: 31964
	private Option<Updater> updaterToRunOnStart;

	// Token: 0x04007CDD RID: 31965
	private Coroutine updaterRoutine;
}
