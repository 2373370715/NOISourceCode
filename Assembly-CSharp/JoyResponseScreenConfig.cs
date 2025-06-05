using System;
using Database;
using UnityEngine;

// Token: 0x02001D65 RID: 7525
public readonly struct JoyResponseScreenConfig
{
	// Token: 0x06009D19 RID: 40217 RVA: 0x0010AB06 File Offset: 0x00108D06
	private JoyResponseScreenConfig(JoyResponseOutfitTarget target, Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem)
	{
		this.target = target;
		this.initalSelectedItem = initalSelectedItem;
		this.isValid = true;
	}

	// Token: 0x06009D1A RID: 40218 RVA: 0x0010AB1D File Offset: 0x00108D1D
	public JoyResponseScreenConfig WithInitialSelection(Option<BalloonArtistFacadeResource> initialSelectedItem)
	{
		return new JoyResponseScreenConfig(this.target, JoyResponseDesignerScreen.GalleryItem.Of(initialSelectedItem));
	}

	// Token: 0x06009D1B RID: 40219 RVA: 0x0010AB35 File Offset: 0x00108D35
	public static JoyResponseScreenConfig Minion(GameObject minionInstance)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromMinion(minionInstance), Option.None);
	}

	// Token: 0x06009D1C RID: 40220 RVA: 0x0010AB4C File Offset: 0x00108D4C
	public static JoyResponseScreenConfig Personality(Personality personality)
	{
		return new JoyResponseScreenConfig(JoyResponseOutfitTarget.FromPersonality(personality), Option.None);
	}

	// Token: 0x06009D1D RID: 40221 RVA: 0x003D6C70 File Offset: 0x003D4E70
	public static JoyResponseScreenConfig From(MinionBrowserScreen.GridItem item)
	{
		MinionBrowserScreen.GridItem.PersonalityTarget personalityTarget = item as MinionBrowserScreen.GridItem.PersonalityTarget;
		if (personalityTarget != null)
		{
			return JoyResponseScreenConfig.Personality(personalityTarget.personality);
		}
		MinionBrowserScreen.GridItem.MinionInstanceTarget minionInstanceTarget = item as MinionBrowserScreen.GridItem.MinionInstanceTarget;
		if (minionInstanceTarget != null)
		{
			return JoyResponseScreenConfig.Minion(minionInstanceTarget.minionInstance);
		}
		throw new NotImplementedException();
	}

	// Token: 0x06009D1E RID: 40222 RVA: 0x0010AB63 File Offset: 0x00108D63
	public void ApplyAndOpenScreen()
	{
		LockerNavigator.Instance.joyResponseDesignerScreen.GetComponent<JoyResponseDesignerScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.joyResponseDesignerScreen, null);
	}

	// Token: 0x04007B58 RID: 31576
	public readonly JoyResponseOutfitTarget target;

	// Token: 0x04007B59 RID: 31577
	public readonly Option<JoyResponseDesignerScreen.GalleryItem> initalSelectedItem;

	// Token: 0x04007B5A RID: 31578
	public readonly bool isValid;
}
