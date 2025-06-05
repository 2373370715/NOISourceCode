using System;
using STRINGS;
using UnityEngine;

// Token: 0x020014FD RID: 5373
public static class LoreBearerUtil
{
	// Token: 0x06006FCB RID: 28619 RVA: 0x000EDB32 File Offset: 0x000EBD32
	public static void AddLoreTo(GameObject prefabOrGameObject)
	{
		prefabOrGameObject.AddOrGet<LoreBearer>();
	}

	// Token: 0x06006FCC RID: 28620 RVA: 0x003020C0 File Offset: 0x003002C0
	public static void AddLoreTo(GameObject prefabOrGameObject, LoreBearerAction unlockLoreFn)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(unlockLoreFn);
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(unlockLoreFn);
		};
	}

	// Token: 0x06006FCD RID: 28621 RVA: 0x00302118 File Offset: 0x00300318
	public static void AddLoreTo(GameObject prefabOrGameObject, string[] collectionsToUnlockFrom)
	{
		KPrefabID component = prefabOrGameObject.GetComponent<KPrefabID>();
		if (component.IsInitialized())
		{
			prefabOrGameObject.AddOrGet<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
			return;
		}
		prefabOrGameObject.AddComponent<LoreBearer>();
		component.prefabInitFn += delegate(GameObject gameObject)
		{
			gameObject.GetComponent<LoreBearer>().Internal_SetContent(LoreBearerUtil.UnlockNextInCollections(collectionsToUnlockFrom));
		};
	}

	// Token: 0x06006FCE RID: 28622 RVA: 0x000EDB3B File Offset: 0x000EBD3B
	public static LoreBearerAction UnlockSpecificEntry(string unlockId, string searchDisplayText)
	{
		return delegate(InfoDialogScreen screen)
		{
			Game.Instance.unlocks.Unlock(unlockId, true);
			screen.AddPlainText(searchDisplayText);
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(unlockId, false), false);
		};
	}

	// Token: 0x06006FCF RID: 28623 RVA: 0x00302174 File Offset: 0x00300374
	public static void UnlockNextEmail(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("emails", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

	// Token: 0x06006FD0 RID: 28624 RVA: 0x00302220 File Offset: 0x00300420
	public static void UnlockNextResearchNote(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("researchnotes", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 3).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_TECHNOLOGY_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06006FD1 RID: 28625 RVA: 0x003022B8 File Offset: 0x003004B8
	public static void UnlockNextJournalEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("journals", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06006FD2 RID: 28626 RVA: 0x00302350 File Offset: 0x00300550
	public static void UnlockNextDimensionalLore(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("dimensionallore", true);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 6).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH1";
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str2));
	}

	// Token: 0x06006FD3 RID: 28627 RVA: 0x003023E8 File Offset: 0x003005E8
	public static void UnlockNextSpaceEntry(InfoDialogScreen screen)
	{
		string text = Game.Instance.unlocks.UnlockNext("space", false);
		if (text != null)
		{
			string str = "SEARCH" + UnityEngine.Random.Range(1, 7).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_SUCCESS." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 4).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_SPACEPOI_FAIL." + str2));
	}

	// Token: 0x06006FD4 RID: 28628 RVA: 0x00302494 File Offset: 0x00300694
	public static void UnlockNextDeskPodiumEntry(InfoDialogScreen screen)
	{
		if (!Game.Instance.unlocks.IsUnlocked("story_trait_critter_manipulator_parking"))
		{
			Game.Instance.unlocks.Unlock("story_trait_critter_manipulator_parking", true);
			string str = "SEARCH" + UnityEngine.Random.Range(1, 1).ToString();
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_PODIUM." + str));
			screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID("story_trait_critter_manipulator_parking", false), false);
			return;
		}
		string str2 = "SEARCH" + UnityEngine.Random.Range(1, 8).ToString();
		screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_COMPUTER_FAIL." + str2));
	}

	// Token: 0x06006FD5 RID: 28629 RVA: 0x000EDB5B File Offset: 0x000EBD5B
	public static LoreBearerAction UnlockNextInCollections(string[] collectionsToUnlockFrom)
	{
		return delegate(InfoDialogScreen screen)
		{
			foreach (string collectionID in collectionsToUnlockFrom)
			{
				string text = Game.Instance.unlocks.UnlockNext(collectionID, false);
				if (text != null)
				{
					screen.AddPlainText(UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_SUCCESS.SEARCH1);
					screen.AddOption(UI.USERMENUACTIONS.READLORE.GOTODATABASE, LoreBearerUtil.OpenCodexByLockKeyID(text, false), false);
					return;
				}
			}
			string str = "SEARCH1";
			screen.AddPlainText(Strings.Get("STRINGS.UI.USERMENUACTIONS.READLORE.SEARCH_OBJECT_FAIL." + str));
		};
	}

	// Token: 0x06006FD6 RID: 28630 RVA: 0x000EDB74 File Offset: 0x000EBD74
	public static void NerualVacillator(InfoDialogScreen screen)
	{
		Game.Instance.unlocks.Unlock("neuralvacillator", true);
		LoreBearerUtil.UnlockNextResearchNote(screen);
	}

	// Token: 0x06006FD7 RID: 28631 RVA: 0x000EDB91 File Offset: 0x000EBD91
	public static Action<InfoDialogScreen> OpenCodexByLockKeyID(string key, bool focusContent = false)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToLockId(key, focusContent);
		};
	}

	// Token: 0x06006FD8 RID: 28632 RVA: 0x000EDBB1 File Offset: 0x000EBDB1
	public static Action<InfoDialogScreen> OpenCodexByEntryID(string id)
	{
		return delegate(InfoDialogScreen dialog)
		{
			dialog.Deactivate();
			ManagementMenu.Instance.OpenCodexToEntry(id, null);
		};
	}
}
