using System;
using System.IO;
using STRINGS;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001AF3 RID: 6899
[AddComponentMenu("KMonoBehaviour/scripts/BaseNaming")]
public class BaseNaming : KMonoBehaviour
{
	// Token: 0x0600904A RID: 36938 RVA: 0x003863EC File Offset: 0x003845EC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.GenerateBaseName();
		this.shuffleBaseNameButton.onClick += this.GenerateBaseName;
		this.inputField.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
		this.inputField.onValueChanged.AddListener(new UnityAction<string>(this.OnEditing));
		this.minionSelectScreen = base.GetComponent<MinionSelectScreen>();
	}

	// Token: 0x0600904B RID: 36939 RVA: 0x00386460 File Offset: 0x00384660
	private bool CheckBaseName(string newName)
	{
		if (string.IsNullOrEmpty(newName))
		{
			return true;
		}
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		if (this.minionSelectScreen != null)
		{
			bool flag = false;
			try
			{
				bool flag2 = Directory.Exists(Path.Combine(savePrefixAndCreateFolder, newName));
				bool flag3 = cloudSavePrefix != null && Directory.Exists(Path.Combine(cloudSavePrefix, newName));
				flag = (flag2 || flag3);
			}
			catch (Exception arg)
			{
				flag = true;
				global::Debug.Log(string.Format("Base Naming / Warning / {0}", arg));
			}
			if (flag)
			{
				this.minionSelectScreen.SetProceedButtonActive(false, string.Format(UI.IMMIGRANTSCREEN.DUPLICATE_COLONY_NAME, newName));
				return false;
			}
			this.minionSelectScreen.SetProceedButtonActive(true, null);
		}
		return true;
	}

	// Token: 0x0600904C RID: 36940 RVA: 0x00102CAF File Offset: 0x00100EAF
	private void OnEditing(string newName)
	{
		Util.ScrubInputField(this.inputField, false, false);
		this.CheckBaseName(this.inputField.text);
	}

	// Token: 0x0600904D RID: 36941 RVA: 0x00386510 File Offset: 0x00384710
	private void OnEndEdit(string newName)
	{
		if (Localization.HasDirtyWords(newName))
		{
			this.inputField.text = this.GenerateBaseNameString();
			newName = this.inputField.text;
		}
		if (string.IsNullOrEmpty(newName))
		{
			return;
		}
		if (newName.EndsWith(" "))
		{
			newName = newName.TrimEnd(' ');
		}
		if (!this.CheckBaseName(newName))
		{
			return;
		}
		this.inputField.text = newName;
		SaveGame.Instance.SetBaseName(newName);
		string path = Path.ChangeExtension(newName, ".sav");
		string savePrefixAndCreateFolder = SaveLoader.GetSavePrefixAndCreateFolder();
		string cloudSavePrefix = SaveLoader.GetCloudSavePrefix();
		string path2 = savePrefixAndCreateFolder;
		if (SaveLoader.GetCloudSavesAvailable() && Game.Instance.SaveToCloudActive && cloudSavePrefix != null)
		{
			path2 = cloudSavePrefix;
		}
		SaveLoader.SetActiveSaveFilePath(Path.Combine(path2, newName, path));
	}

	// Token: 0x0600904E RID: 36942 RVA: 0x003865C4 File Offset: 0x003847C4
	private void GenerateBaseName()
	{
		string text = this.GenerateBaseNameString();
		((LocText)this.inputField.placeholder).text = text;
		this.inputField.text = text;
		this.OnEndEdit(text);
	}

	// Token: 0x0600904F RID: 36943 RVA: 0x00386604 File Offset: 0x00384804
	private string GenerateBaseNameString()
	{
		string fullString = LocString.GetStrings(typeof(NAMEGEN.COLONY.FORMATS)).GetRandom<string>();
		fullString = this.ReplaceStringWithRandom(fullString, "{noun}", LocString.GetStrings(typeof(NAMEGEN.COLONY.NOUN)));
		string[] strings = LocString.GetStrings(typeof(NAMEGEN.COLONY.ADJECTIVE));
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective2}", strings);
		fullString = this.ReplaceStringWithRandom(fullString, "{adjective3}", strings);
		return this.ReplaceStringWithRandom(fullString, "{adjective4}", strings);
	}

	// Token: 0x06009050 RID: 36944 RVA: 0x00102CD0 File Offset: 0x00100ED0
	private string ReplaceStringWithRandom(string fullString, string replacementKey, string[] replacementValues)
	{
		if (!fullString.Contains(replacementKey))
		{
			return fullString;
		}
		return fullString.Replace(replacementKey, replacementValues.GetRandom<string>());
	}

	// Token: 0x04006D0D RID: 27917
	[SerializeField]
	private KInputTextField inputField;

	// Token: 0x04006D0E RID: 27918
	[SerializeField]
	private KButton shuffleBaseNameButton;

	// Token: 0x04006D0F RID: 27919
	private MinionSelectScreen minionSelectScreen;
}
