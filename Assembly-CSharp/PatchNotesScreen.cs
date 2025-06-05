using System;
using UnityEngine;

// Token: 0x02001B86 RID: 7046
public class PatchNotesScreen : KModalScreen
{
	// Token: 0x060093DB RID: 37851 RVA: 0x0039B9EC File Offset: 0x00399BEC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		this.closeButton.onClick += this.MarkAsReadAndClose;
		this.closeButton.soundPlayer.widget_sound_events()[0].OverrideAssetName = "HUD_Click_Close";
		this.okButton.onClick += this.MarkAsReadAndClose;
		this.previousVersion.onClick += delegate()
		{
			App.OpenWebURL("http://support.kleientertainment.com/customer/portal/articles/2776550");
		};
		this.fullPatchNotes.onClick += this.OnPatchNotesClick;
		PatchNotesScreen.instance = this;
	}

	// Token: 0x060093DC RID: 37852 RVA: 0x00105057 File Offset: 0x00103257
	protected override void OnCleanUp()
	{
		PatchNotesScreen.instance = null;
	}

	// Token: 0x060093DD RID: 37853 RVA: 0x000B1628 File Offset: 0x000AF828
	public static bool ShouldShowScreen()
	{
		return false;
	}

	// Token: 0x060093DE RID: 37854 RVA: 0x0010505F File Offset: 0x0010325F
	private void MarkAsReadAndClose()
	{
		KPlayerPrefs.SetInt("PatchNotesVersion", PatchNotesScreen.PatchNotesVersion);
		this.Deactivate();
	}

	// Token: 0x060093DF RID: 37855 RVA: 0x00105076 File Offset: 0x00103276
	public static void UpdatePatchNotes(string patchNotesSummary, string url)
	{
		PatchNotesScreen.m_patchNotesUrl = url;
		PatchNotesScreen.m_patchNotesText = patchNotesSummary;
		if (PatchNotesScreen.instance != null)
		{
			PatchNotesScreen.instance.changesLabel.text = PatchNotesScreen.m_patchNotesText;
		}
	}

	// Token: 0x060093E0 RID: 37856 RVA: 0x001050A5 File Offset: 0x001032A5
	private void OnPatchNotesClick()
	{
		App.OpenWebURL(PatchNotesScreen.m_patchNotesUrl);
	}

	// Token: 0x060093E1 RID: 37857 RVA: 0x001050B1 File Offset: 0x001032B1
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Escape) || e.TryConsume(global::Action.MouseRight))
		{
			this.MarkAsReadAndClose();
			return;
		}
		base.OnKeyDown(e);
	}

	// Token: 0x04007028 RID: 28712
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04007029 RID: 28713
	[SerializeField]
	private KButton okButton;

	// Token: 0x0400702A RID: 28714
	[SerializeField]
	private KButton fullPatchNotes;

	// Token: 0x0400702B RID: 28715
	[SerializeField]
	private KButton previousVersion;

	// Token: 0x0400702C RID: 28716
	[SerializeField]
	private LocText changesLabel;

	// Token: 0x0400702D RID: 28717
	private static string m_patchNotesUrl;

	// Token: 0x0400702E RID: 28718
	private static string m_patchNotesText;

	// Token: 0x0400702F RID: 28719
	private static int PatchNotesVersion = 9;

	// Token: 0x04007030 RID: 28720
	private static PatchNotesScreen instance;
}
