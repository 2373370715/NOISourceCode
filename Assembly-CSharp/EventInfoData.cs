using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
public class EventInfoData
{
	// Token: 0x060022DF RID: 8927 RVA: 0x001D11B0 File Offset: 0x001CF3B0
	public EventInfoData(string title, string description, HashedString animFileName)
	{
		this.title = title;
		this.description = description;
		this.animFileName = animFileName;
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000BB198 File Offset: 0x000B9398
	public List<EventInfoData.Option> GetOptions()
	{
		this.FinalizeText();
		return this.options;
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x001D1200 File Offset: 0x001CF400
	public EventInfoData.Option AddOption(string mainText, string description = null)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			description = description
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x001D1238 File Offset: 0x001CF438
	public EventInfoData.Option SimpleOption(string mainText, System.Action callback)
	{
		EventInfoData.Option option = new EventInfoData.Option
		{
			mainText = mainText,
			callback = callback
		};
		this.options.Add(option);
		this.dirty = true;
		return option;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000BB1A6 File Offset: 0x000B93A6
	public EventInfoData.Option AddDefaultOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_NAME, callback);
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000BB1B9 File Offset: 0x000B93B9
	public EventInfoData.Option AddDefaultConsiderLaterOption(System.Action callback = null)
	{
		return this.SimpleOption(GAMEPLAY_EVENTS.DEFAULT_OPTION_CONSIDER_NAME, callback);
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000BB1CC File Offset: 0x000B93CC
	public void SetTextParameter(string key, string value)
	{
		this.textParameters[key] = value;
		this.dirty = true;
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x001D1270 File Offset: 0x001CF470
	public void FinalizeText()
	{
		if (!this.dirty)
		{
			return;
		}
		this.dirty = false;
		foreach (KeyValuePair<string, string> keyValuePair in this.textParameters)
		{
			string oldValue = "{" + keyValuePair.Key + "}";
			if (this.title != null)
			{
				this.title = this.title.Replace(oldValue, keyValuePair.Value);
			}
			if (this.description != null)
			{
				this.description = this.description.Replace(oldValue, keyValuePair.Value);
			}
			if (this.location != null)
			{
				this.location = this.location.Replace(oldValue, keyValuePair.Value);
			}
			if (this.whenDescription != null)
			{
				this.whenDescription = this.whenDescription.Replace(oldValue, keyValuePair.Value);
			}
			foreach (EventInfoData.Option option in this.options)
			{
				if (option.mainText != null)
				{
					option.mainText = option.mainText.Replace(oldValue, keyValuePair.Value);
				}
				if (option.description != null)
				{
					option.description = option.description.Replace(oldValue, keyValuePair.Value);
				}
				if (option.tooltip != null)
				{
					option.tooltip = option.tooltip.Replace(oldValue, keyValuePair.Value);
				}
				foreach (EventInfoData.OptionIcon optionIcon in option.informationIcons)
				{
					if (optionIcon.tooltip != null)
					{
						optionIcon.tooltip = optionIcon.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
				foreach (EventInfoData.OptionIcon optionIcon2 in option.consequenceIcons)
				{
					if (optionIcon2.tooltip != null)
					{
						optionIcon2.tooltip = optionIcon2.tooltip.Replace(oldValue, keyValuePair.Value);
					}
				}
			}
		}
	}

	// Token: 0x0400175F RID: 5983
	public string title;

	// Token: 0x04001760 RID: 5984
	public string description;

	// Token: 0x04001761 RID: 5985
	public string location;

	// Token: 0x04001762 RID: 5986
	public string whenDescription;

	// Token: 0x04001763 RID: 5987
	public Transform clickFocus;

	// Token: 0x04001764 RID: 5988
	public GameObject[] minions;

	// Token: 0x04001765 RID: 5989
	public GameObject artifact;

	// Token: 0x04001766 RID: 5990
	public HashedString animFileName;

	// Token: 0x04001767 RID: 5991
	public HashedString mainAnim = "event";

	// Token: 0x04001768 RID: 5992
	public Dictionary<string, string> textParameters = new Dictionary<string, string>();

	// Token: 0x04001769 RID: 5993
	public List<EventInfoData.Option> options = new List<EventInfoData.Option>();

	// Token: 0x0400176A RID: 5994
	public System.Action showCallback;

	// Token: 0x0400176B RID: 5995
	private bool dirty;

	// Token: 0x020007B1 RID: 1969
	public class OptionIcon
	{
		// Token: 0x060022E7 RID: 8935 RVA: 0x000BB1E2 File Offset: 0x000B93E2
		public OptionIcon(Sprite sprite, EventInfoData.OptionIcon.ContainerType containerType, string tooltip, float scale = 1f)
		{
			this.sprite = sprite;
			this.containerType = containerType;
			this.tooltip = tooltip;
			this.scale = scale;
		}

		// Token: 0x0400176C RID: 5996
		public EventInfoData.OptionIcon.ContainerType containerType;

		// Token: 0x0400176D RID: 5997
		public Sprite sprite;

		// Token: 0x0400176E RID: 5998
		public string tooltip;

		// Token: 0x0400176F RID: 5999
		public float scale;

		// Token: 0x020007B2 RID: 1970
		public enum ContainerType
		{
			// Token: 0x04001771 RID: 6001
			Neutral,
			// Token: 0x04001772 RID: 6002
			Positive,
			// Token: 0x04001773 RID: 6003
			Negative,
			// Token: 0x04001774 RID: 6004
			Information
		}
	}

	// Token: 0x020007B3 RID: 1971
	public class Option
	{
		// Token: 0x060022E8 RID: 8936 RVA: 0x000BB207 File Offset: 0x000B9407
		public void AddInformationIcon(string tooltip, float scale = 1f)
		{
			this.informationIcons.Add(new EventInfoData.OptionIcon(null, EventInfoData.OptionIcon.ContainerType.Information, tooltip, scale));
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000BB21D File Offset: 0x000B941D
		public void AddPositiveIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Positive, tooltip, scale));
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000BB233 File Offset: 0x000B9433
		public void AddNeutralIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Neutral, tooltip, scale));
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000BB249 File Offset: 0x000B9449
		public void AddNegativeIcon(Sprite sprite, string tooltip, float scale = 1f)
		{
			this.consequenceIcons.Add(new EventInfoData.OptionIcon(sprite, EventInfoData.OptionIcon.ContainerType.Negative, tooltip, scale));
		}

		// Token: 0x04001775 RID: 6005
		public string mainText;

		// Token: 0x04001776 RID: 6006
		public string description;

		// Token: 0x04001777 RID: 6007
		public string tooltip;

		// Token: 0x04001778 RID: 6008
		public System.Action callback;

		// Token: 0x04001779 RID: 6009
		public List<EventInfoData.OptionIcon> informationIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x0400177A RID: 6010
		public List<EventInfoData.OptionIcon> consequenceIcons = new List<EventInfoData.OptionIcon>();

		// Token: 0x0400177B RID: 6011
		public bool allowed = true;
	}
}
