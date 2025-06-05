using System;
using UnityEngine;

// Token: 0x020020B2 RID: 8370
public class VictoryScreen : KModalScreen
{
	// Token: 0x0600B27C RID: 45692 RVA: 0x001188ED File Offset: 0x00116AED
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Init();
	}

	// Token: 0x0600B27D RID: 45693 RVA: 0x001188FB File Offset: 0x00116AFB
	private void Init()
	{
		if (this.DismissButton)
		{
			this.DismissButton.onClick += delegate()
			{
				this.Dismiss();
			};
		}
	}

	// Token: 0x0600B27E RID: 45694 RVA: 0x00118921 File Offset: 0x00116B21
	private void Retire()
	{
		if (RetireColonyUtility.SaveColonySummaryData())
		{
			this.Show(false);
		}
	}

	// Token: 0x0600B27F RID: 45695 RVA: 0x00103A4E File Offset: 0x00101C4E
	private void Dismiss()
	{
		this.Show(false);
	}

	// Token: 0x0600B280 RID: 45696 RVA: 0x0043D748 File Offset: 0x0043B948
	public void SetAchievements(string[] achievementIDs)
	{
		string text = "";
		for (int i = 0; i < achievementIDs.Length; i++)
		{
			if (i > 0)
			{
				text += "\n";
			}
			text += GameUtil.ApplyBoldString(Db.Get().ColonyAchievements.Get(achievementIDs[i]).Name);
			text = text + "\n" + Db.Get().ColonyAchievements.Get(achievementIDs[i]).description;
		}
		this.descriptionText.text = text;
	}

	// Token: 0x04008CE6 RID: 36070
	[SerializeField]
	private KButton DismissButton;

	// Token: 0x04008CE7 RID: 36071
	[SerializeField]
	private LocText descriptionText;
}
