using System;

// Token: 0x02001F5C RID: 8028
public class SaveActive : KScreen
{
	// Token: 0x0600A962 RID: 43362 RVA: 0x001126AA File Offset: 0x001108AA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
	}

	// Token: 0x0600A963 RID: 43363 RVA: 0x001126E0 File Offset: 0x001108E0
	private void DoCallBack(HashedString name)
	{
		this.controller.onAnimComplete -= this.DoCallBack;
		this.readyForSaveCallback();
		this.readyForSaveCallback = null;
	}

	// Token: 0x0600A964 RID: 43364 RVA: 0x0011270B File Offset: 0x0011090B
	private void ActiveateSaveIndicator(Game.CansaveCB cb)
	{
		this.readyForSaveCallback = cb;
		this.controller.onAnimComplete += this.DoCallBack;
		this.controller.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A965 RID: 43365 RVA: 0x0011274B File Offset: 0x0011094B
	private void SetActiveSaveIndicator()
	{
		this.controller.Play("working_loop", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A966 RID: 43366 RVA: 0x0011276D File Offset: 0x0011096D
	private void DeactivateSaveIndicator()
	{
		this.controller.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x0600A967 RID: 43367 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void OnKeyDown(KButtonEvent e)
	{
	}

	// Token: 0x0400856E RID: 34158
	[MyCmpGet]
	private KBatchedAnimController controller;

	// Token: 0x0400856F RID: 34159
	private Game.CansaveCB readyForSaveCallback;
}
