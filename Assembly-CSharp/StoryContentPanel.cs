using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.CustomSettings;
using ProcGen;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002087 RID: 8327
public class StoryContentPanel : KMonoBehaviour
{
	// Token: 0x0600B16C RID: 45420 RVA: 0x00438D04 File Offset: 0x00436F04
	public List<string> GetActiveStories()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x0600B16D RID: 45421 RVA: 0x00117D42 File Offset: 0x00115F42
	public void Init()
	{
		this.SpawnRows();
		this.RefreshRows();
		this.RefreshDescriptionPanel();
		this.SelectDefault();
		CustomGameSettings.Instance.OnStorySettingChanged += this.OnStorySettingChanged;
	}

	// Token: 0x0600B16E RID: 45422 RVA: 0x00117D72 File Offset: 0x00115F72
	public void Cleanup()
	{
		CustomGameSettings.Instance.OnStorySettingChanged -= this.OnStorySettingChanged;
	}

	// Token: 0x0600B16F RID: 45423 RVA: 0x00117D8A File Offset: 0x00115F8A
	private void OnStorySettingChanged(SettingConfig config, SettingLevel level)
	{
		this.storyStates[config.id] = ((level.id == "Guaranteed") ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		this.RefreshStoryDisplay(config.id);
	}

	// Token: 0x0600B170 RID: 45424 RVA: 0x00438D70 File Offset: 0x00436F70
	private void SpawnRows()
	{
		using (List<Story>.Enumerator enumerator = Db.Get().Stories.resources.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Story story = enumerator.Current;
				GameObject gameObject = global::Util.KInstantiateUI(this.storyRowPrefab, this.storyRowContainer, true);
				HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
				component.GetReference<LocText>("Label").SetText(Strings.Get(story.StoryTrait.name));
				MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
				component2.onClick = (System.Action)Delegate.Combine(component2.onClick, new System.Action(delegate()
				{
					this.SelectRow(story.Id);
				}));
				this.storyRows.Add(story.Id, gameObject);
				component.GetReference<Image>("Icon").sprite = Assets.GetSprite(story.StoryTrait.icon);
				MultiToggle reference = component.GetReference<MultiToggle>("checkbox");
				reference.onClick = (System.Action)Delegate.Combine(reference.onClick, new System.Action(delegate()
				{
					this.IncrementStorySetting(story.Id, true);
					this.RefreshStoryDisplay(story.Id);
				}));
				this.storyStates.Add(story.Id, this._defaultStoryState);
			}
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshStoryLabel();
	}

	// Token: 0x0600B171 RID: 45425 RVA: 0x00117DBF File Offset: 0x00115FBF
	private void SelectRow(string id)
	{
		this.selectedStoryId = id;
		this.RefreshRows();
		this.RefreshDescriptionPanel();
	}

	// Token: 0x0600B172 RID: 45426 RVA: 0x00438EEC File Offset: 0x004370EC
	public void SelectDefault()
	{
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				this.SelectRow(keyValuePair.Key);
				return;
			}
		}
		using (Dictionary<string, StoryContentPanel.StoryState>.Enumerator enumerator = this.storyStates.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 = enumerator.Current;
				this.SelectRow(keyValuePair2.Key);
			}
		}
	}

	// Token: 0x0600B173 RID: 45427 RVA: 0x00438F9C File Offset: 0x0043719C
	private void IncrementStorySetting(string storyId, bool forward = true)
	{
		int num = (int)this.storyStates[storyId];
		num += (forward ? 1 : -1);
		if (num < 0)
		{
			num += 2;
		}
		num %= 2;
		this.SetStoryState(storyId, (StoryContentPanel.StoryState)num);
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x0600B174 RID: 45428 RVA: 0x00438FE0 File Offset: 0x004371E0
	private void SetStoryState(string storyId, StoryContentPanel.StoryState state)
	{
		this.storyStates[storyId] = state;
		SettingConfig config = CustomGameSettings.Instance.StorySettings[storyId];
		CustomGameSettings.Instance.SetStorySetting(config, this.storyStates[storyId] == StoryContentPanel.StoryState.Guaranteed);
	}

	// Token: 0x0600B175 RID: 45429 RVA: 0x00439028 File Offset: 0x00437228
	public void SelectRandomStories(int min = 5, int max = 5, bool useBias = false)
	{
		int num = UnityEngine.Random.Range(min, max);
		List<Story> list = new List<Story>(Db.Get().Stories.resources);
		List<Story> list2 = new List<Story>();
		list.Shuffle<Story>();
		int num2 = 0;
		while (num2 < num && list.Count - 1 >= num2)
		{
			list2.Add(list[num2]);
			num2++;
		}
		float num3 = 0.7f;
		int num4 = list2.Count((Story x) => x.IsNew());
		if (useBias && num4 == 0 && UnityEngine.Random.value < num3)
		{
			List<Story> list3 = (from x in Db.Get().Stories.resources
			where x.IsNew()
			select x).ToList<Story>();
			list3.Shuffle<Story>();
			if (list3.Count > 0)
			{
				list2.RemoveAt(0);
				list2.Add(list3[0]);
			}
		}
		foreach (Story story in list)
		{
			this.SetStoryState(story.Id, list2.Contains(story) ? StoryContentPanel.StoryState.Guaranteed : StoryContentPanel.StoryState.Forbidden);
		}
		this.RefreshAllStoryStates();
		this.mainScreen.RefreshRowsAndDescriptions();
	}

	// Token: 0x0600B176 RID: 45430 RVA: 0x0043918C File Offset: 0x0043738C
	private void RefreshAllStoryStates()
	{
		foreach (string id in this.storyRows.Keys)
		{
			this.RefreshStoryDisplay(id);
		}
	}

	// Token: 0x0600B177 RID: 45431 RVA: 0x004391E4 File Offset: 0x004373E4
	private void RefreshStoryDisplay(string id)
	{
		MultiToggle reference = this.storyRows[id].GetComponent<HierarchyReferences>().GetReference<MultiToggle>("checkbox");
		StoryContentPanel.StoryState storyState = this.storyStates[id];
		if (storyState == StoryContentPanel.StoryState.Forbidden)
		{
			reference.ChangeState(0);
			return;
		}
		if (storyState != StoryContentPanel.StoryState.Guaranteed)
		{
			return;
		}
		reference.ChangeState(1);
	}

	// Token: 0x0600B178 RID: 45432 RVA: 0x00439234 File Offset: 0x00437434
	private void RefreshRows()
	{
		foreach (KeyValuePair<string, GameObject> keyValuePair in this.storyRows)
		{
			keyValuePair.Value.GetComponent<MultiToggle>().ChangeState((keyValuePair.Key == this.selectedStoryId) ? 1 : 0);
		}
	}

	// Token: 0x0600B179 RID: 45433 RVA: 0x004392AC File Offset: 0x004374AC
	private void RefreshDescriptionPanel()
	{
		if (this.selectedStoryId.IsNullOrWhiteSpace())
		{
			this.selectedStoryTitleLabel.SetText("");
			this.selectedStoryDescriptionLabel.SetText("");
			return;
		}
		WorldTrait storyTrait = Db.Get().Stories.GetStoryTrait(this.selectedStoryId, true);
		this.selectedStoryTitleLabel.SetText(Strings.Get(storyTrait.name));
		this.selectedStoryDescriptionLabel.SetText(Strings.Get(storyTrait.description));
		string s = storyTrait.icon.Replace("_icon", "_image");
		this.selectedStoryImage.sprite = Assets.GetSprite(s);
	}

	// Token: 0x0600B17A RID: 45434 RVA: 0x00439360 File Offset: 0x00437560
	public string GetTraitsString(bool tooltip = false)
	{
		int num = 0;
		int num2 = 5;
		foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair in this.storyStates)
		{
			if (keyValuePair.Value == StoryContentPanel.StoryState.Guaranteed)
			{
				num++;
			}
		}
		string text = UI.FRONTEND.COLONYDESTINATIONSCREEN.STORY_TRAITS_HEADER;
		string str;
		if (num != 0)
		{
			if (num != 1)
			{
				str = string.Format(UI.FRONTEND.COLONYDESTINATIONSCREEN.TRAIT_COUNT, num);
			}
			else
			{
				str = UI.FRONTEND.COLONYDESTINATIONSCREEN.SINGLE_TRAIT;
			}
		}
		else
		{
			str = UI.FRONTEND.COLONYDESTINATIONSCREEN.NO_TRAITS;
		}
		text = text + ": " + str;
		if (num > num2)
		{
			text = text + " " + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING;
		}
		if (tooltip)
		{
			foreach (KeyValuePair<string, StoryContentPanel.StoryState> keyValuePair2 in this.storyStates)
			{
				if (keyValuePair2.Value == StoryContentPanel.StoryState.Guaranteed)
				{
					WorldTrait storyTrait = Db.Get().Stories.Get(keyValuePair2.Key).StoryTrait;
					text = string.Concat(new string[]
					{
						text,
						"\n\n<b>",
						Strings.Get(storyTrait.name).String,
						"</b>\n",
						Strings.Get(storyTrait.description).String
					});
				}
			}
			if (num > num2)
			{
				text = text + "\n\n" + UI.FRONTEND.COLONYDESTINATIONSCREEN.TOO_MANY_TRAITS_WARNING_TOOLTIP;
			}
		}
		return text;
	}

	// Token: 0x04008BE5 RID: 35813
	[SerializeField]
	private GameObject storyRowPrefab;

	// Token: 0x04008BE6 RID: 35814
	[SerializeField]
	private GameObject storyRowContainer;

	// Token: 0x04008BE7 RID: 35815
	private Dictionary<string, GameObject> storyRows = new Dictionary<string, GameObject>();

	// Token: 0x04008BE8 RID: 35816
	public const int DEFAULT_RANDOMIZE_STORY_COUNT = 5;

	// Token: 0x04008BE9 RID: 35817
	private Dictionary<string, StoryContentPanel.StoryState> storyStates = new Dictionary<string, StoryContentPanel.StoryState>();

	// Token: 0x04008BEA RID: 35818
	private string selectedStoryId = "";

	// Token: 0x04008BEB RID: 35819
	[SerializeField]
	private ColonyDestinationSelectScreen mainScreen;

	// Token: 0x04008BEC RID: 35820
	[Header("Trait Count")]
	[Header("SelectedStory")]
	[SerializeField]
	private Image selectedStoryImage;

	// Token: 0x04008BED RID: 35821
	[SerializeField]
	private LocText selectedStoryTitleLabel;

	// Token: 0x04008BEE RID: 35822
	[SerializeField]
	private LocText selectedStoryDescriptionLabel;

	// Token: 0x04008BEF RID: 35823
	[SerializeField]
	private Sprite spriteForbidden;

	// Token: 0x04008BF0 RID: 35824
	[SerializeField]
	private Sprite spritePossible;

	// Token: 0x04008BF1 RID: 35825
	[SerializeField]
	private Sprite spriteGuaranteed;

	// Token: 0x04008BF2 RID: 35826
	private StoryContentPanel.StoryState _defaultStoryState;

	// Token: 0x04008BF3 RID: 35827
	private List<string> storyTraitSettings = new List<string>
	{
		"None",
		"Few",
		"Lots"
	};

	// Token: 0x02002088 RID: 8328
	private enum StoryState
	{
		// Token: 0x04008BF5 RID: 35829
		Forbidden,
		// Token: 0x04008BF6 RID: 35830
		Guaranteed,
		// Token: 0x04008BF7 RID: 35831
		LENGTH
	}
}
