using System;
using System.Linq;
using UnityEngine;

// Token: 0x02001E71 RID: 7793
public readonly struct MinionBrowserScreenConfig
{
	// Token: 0x0600A342 RID: 41794 RVA: 0x0010E8C6 File Offset: 0x0010CAC6
	public MinionBrowserScreenConfig(MinionBrowserScreen.GridItem[] items, Option<MinionBrowserScreen.GridItem> defaultSelectedItem)
	{
		this.items = items;
		this.defaultSelectedItem = defaultSelectedItem;
		this.isValid = true;
	}

	// Token: 0x0600A343 RID: 41795 RVA: 0x003EF684 File Offset: 0x003ED884
	public static MinionBrowserScreenConfig Personalities(Option<Personality> defaultSelectedPersonality = default(Option<Personality>))
	{
		MinionBrowserScreen.GridItem.PersonalityTarget[] items = (from personality in Db.Get().Personalities.GetAll(true, false)
		select MinionBrowserScreen.GridItem.Of(personality)).ToArray<MinionBrowserScreen.GridItem.PersonalityTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedPersonality.AndThen<MinionBrowserScreen.GridItem>((Personality personality) => items.FirstOrDefault((MinionBrowserScreen.GridItem.PersonalityTarget item) => item.personality == personality));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x0600A344 RID: 41796 RVA: 0x003EF71C File Offset: 0x003ED91C
	public static MinionBrowserScreenConfig MinionInstances(Option<GameObject> defaultSelectedMinionInstance = default(Option<GameObject>))
	{
		MinionBrowserScreen.GridItem.MinionInstanceTarget[] items = (from minionIdentity in Components.MinionIdentities.Items
		select MinionBrowserScreen.GridItem.Of(minionIdentity.gameObject)).ToArray<MinionBrowserScreen.GridItem.MinionInstanceTarget>();
		Option<MinionBrowserScreen.GridItem> option = defaultSelectedMinionInstance.AndThen<MinionBrowserScreen.GridItem>((GameObject minionInstance) => items.FirstOrDefault((MinionBrowserScreen.GridItem.MinionInstanceTarget item) => item.minionInstance == minionInstance));
		if (option.IsNone() && items.Length != 0)
		{
			option = items[0];
		}
		MinionBrowserScreen.GridItem[] array = items;
		return new MinionBrowserScreenConfig(array, option);
	}

	// Token: 0x0600A345 RID: 41797 RVA: 0x0010E8DD File Offset: 0x0010CADD
	public void ApplyAndOpenScreen(System.Action onClose = null)
	{
		LockerNavigator.Instance.duplicantCatalogueScreen.GetComponent<MinionBrowserScreen>().Configure(this);
		LockerNavigator.Instance.PushScreen(LockerNavigator.Instance.duplicantCatalogueScreen, onClose);
	}

	// Token: 0x04007FA3 RID: 32675
	public readonly MinionBrowserScreen.GridItem[] items;

	// Token: 0x04007FA4 RID: 32676
	public readonly Option<MinionBrowserScreen.GridItem> defaultSelectedItem;

	// Token: 0x04007FA5 RID: 32677
	public readonly bool isValid;
}
