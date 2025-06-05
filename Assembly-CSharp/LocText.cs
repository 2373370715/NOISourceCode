using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using STRINGS;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02001DCB RID: 7627
public class LocText : TextMeshProUGUI
{
	// Token: 0x06009F62 RID: 40802 RVA: 0x0010C1FE File Offset: 0x0010A3FE
	protected override void OnEnable()
	{
		base.OnEnable();
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06009F63 RID: 40803 RVA: 0x0010C206 File Offset: 0x0010A406
	// (set) Token: 0x06009F64 RID: 40804 RVA: 0x0010C20E File Offset: 0x0010A40E
	public bool AllowLinks
	{
		get
		{
			return this.allowLinksInternal;
		}
		set
		{
			this.allowLinksInternal = value;
			this.RefreshLinkHandler();
			this.raycastTarget = (this.raycastTarget || this.allowLinksInternal);
		}
	}

	// Token: 0x06009F65 RID: 40805 RVA: 0x003DF3B8 File Offset: 0x003DD5B8
	[ContextMenu("Apply Settings")]
	public void ApplySettings()
	{
		if (this.key != "" && Application.isPlaying)
		{
			StringKey key = new StringKey(this.key);
			this.text = Strings.Get(key);
		}
		if (this.textStyleSetting != null)
		{
			SetTextStyleSetting.ApplyStyle(this, this.textStyleSetting);
		}
	}

	// Token: 0x06009F66 RID: 40806 RVA: 0x003DF418 File Offset: 0x003DD618
	private new void Awake()
	{
		base.Awake();
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = Localization.IsRightToLeft;
		KInputManager.InputChange.AddListener(new UnityAction(this.RefreshText));
		SetTextStyleSetting setTextStyleSetting = base.gameObject.GetComponent<SetTextStyleSetting>();
		if (setTextStyleSetting == null)
		{
			setTextStyleSetting = base.gameObject.AddComponent<SetTextStyleSetting>();
		}
		if (!this.allowOverride)
		{
			setTextStyleSetting.SetStyle(this.textStyleSetting);
		}
		this.textLinkHandler = base.GetComponent<TextLinkHandler>();
	}

	// Token: 0x06009F67 RID: 40807 RVA: 0x0010C234 File Offset: 0x0010A434
	private new void Start()
	{
		base.Start();
		this.RefreshLinkHandler();
	}

	// Token: 0x06009F68 RID: 40808 RVA: 0x0010C242 File Offset: 0x0010A442
	private new void OnDestroy()
	{
		KInputManager.InputChange.RemoveListener(new UnityAction(this.RefreshText));
		base.OnDestroy();
	}

	// Token: 0x06009F69 RID: 40809 RVA: 0x0010C260 File Offset: 0x0010A460
	public override void SetLayoutDirty()
	{
		if (this.staticLayout)
		{
			return;
		}
		base.SetLayoutDirty();
	}

	// Token: 0x06009F6A RID: 40810 RVA: 0x0010C271 File Offset: 0x0010A471
	public void SetLinkOverrideAction(Func<string, bool> action)
	{
		this.RefreshLinkHandler();
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.overrideLinkAction = action;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06009F6B RID: 40811 RVA: 0x0010C293 File Offset: 0x0010A493
	// (set) Token: 0x06009F6C RID: 40812 RVA: 0x0010C29B File Offset: 0x0010A49B
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			base.text = this.FilterInput(value);
		}
	}

	// Token: 0x06009F6D RID: 40813 RVA: 0x0010C2AA File Offset: 0x0010A4AA
	public override void SetText(string text)
	{
		text = this.FilterInput(text);
		base.SetText(text);
	}

	// Token: 0x06009F6E RID: 40814 RVA: 0x0010C2BC File Offset: 0x0010A4BC
	private string FilterInput(string input)
	{
		if (input != null)
		{
			string text = LocText.ParseText(input);
			if (text != input)
			{
				this.originalString = input;
			}
			else
			{
				this.originalString = string.Empty;
			}
			input = text;
		}
		if (this.AllowLinks)
		{
			return LocText.ModifyLinkStrings(input);
		}
		return input;
	}

	// Token: 0x06009F6F RID: 40815 RVA: 0x003DF4D8 File Offset: 0x003DD6D8
	public static string ParseText(string input)
	{
		string pattern = "\\{Hotkey/(\\w+)\\}";
		string input2 = Regex.Replace(input, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			global::Action action;
			if (LocText.ActionLookup.TryGetValue(value, out action))
			{
				return GameUtil.GetHotkeyString(action);
			}
			return m.Value;
		});
		pattern = "\\(ClickType/(\\w+)\\)";
		return Regex.Replace(input2, pattern, delegate(Match m)
		{
			string value = m.Groups[1].Value;
			Pair<LocString, LocString> pair;
			if (!LocText.ClickLookup.TryGetValue(value, out pair))
			{
				return m.Value;
			}
			if (KInputManager.currentControllerIsGamepad)
			{
				return pair.first.ToString();
			}
			return pair.second.ToString();
		});
	}

	// Token: 0x06009F70 RID: 40816 RVA: 0x0010C2F6 File Offset: 0x0010A4F6
	private void RefreshText()
	{
		if (this.originalString != string.Empty)
		{
			this.SetText(this.originalString);
		}
	}

	// Token: 0x06009F71 RID: 40817 RVA: 0x0010C316 File Offset: 0x0010A516
	protected override void GenerateTextMesh()
	{
		base.GenerateTextMesh();
	}

	// Token: 0x06009F72 RID: 40818 RVA: 0x003DF53C File Offset: 0x003DD73C
	internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
	{
		base.font = font;
		if (this.key != "")
		{
			StringEntry stringEntry = Strings.Get(new StringKey(this.key));
			this.text = stringEntry.String;
		}
		this.text = Localization.Fixup(this.text);
		base.isRightToLeftText = isRightToLeft;
	}

	// Token: 0x06009F73 RID: 40819 RVA: 0x003DF598 File Offset: 0x003DD798
	private static string ModifyLinkStrings(string input)
	{
		if (input == null || input.IndexOf("<b><style=\"KLink\">") != -1)
		{
			return input;
		}
		StringBuilder stringBuilder = new StringBuilder(input);
		stringBuilder.Replace("<link=\"", LocText.combinedPrefix);
		stringBuilder.Replace("</link>", LocText.combinedSuffix);
		return stringBuilder.ToString();
	}

	// Token: 0x06009F74 RID: 40820 RVA: 0x003DF5E8 File Offset: 0x003DD7E8
	private void RefreshLinkHandler()
	{
		if (this.textLinkHandler == null && this.allowLinksInternal)
		{
			this.textLinkHandler = base.GetComponent<TextLinkHandler>();
			if (this.textLinkHandler == null)
			{
				this.textLinkHandler = base.gameObject.AddComponent<TextLinkHandler>();
			}
		}
		else if (!this.allowLinksInternal && this.textLinkHandler != null)
		{
			UnityEngine.Object.Destroy(this.textLinkHandler);
			this.textLinkHandler = null;
		}
		if (this.textLinkHandler != null)
		{
			this.textLinkHandler.CheckMouseOver();
		}
	}

	// Token: 0x04007D19 RID: 32025
	public string key;

	// Token: 0x04007D1A RID: 32026
	public TextStyleSetting textStyleSetting;

	// Token: 0x04007D1B RID: 32027
	public bool allowOverride;

	// Token: 0x04007D1C RID: 32028
	public bool staticLayout;

	// Token: 0x04007D1D RID: 32029
	private TextLinkHandler textLinkHandler;

	// Token: 0x04007D1E RID: 32030
	private string originalString = string.Empty;

	// Token: 0x04007D1F RID: 32031
	[SerializeField]
	private bool allowLinksInternal;

	// Token: 0x04007D20 RID: 32032
	private static readonly Dictionary<string, global::Action> ActionLookup = Enum.GetNames(typeof(global::Action)).ToDictionary((string x) => x, (string x) => (global::Action)Enum.Parse(typeof(global::Action), x), StringComparer.OrdinalIgnoreCase);

	// Token: 0x04007D21 RID: 32033
	private static readonly Dictionary<string, Pair<LocString, LocString>> ClickLookup = new Dictionary<string, Pair<LocString, LocString>>
	{
		{
			UI.ClickType.Click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESS, UI.CONTROLS.CLICK)
		},
		{
			UI.ClickType.Clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLE, UI.CONTROLS.CLICKABLE)
		},
		{
			UI.ClickType.Clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSED, UI.CONTROLS.CLICKED)
		},
		{
			UI.ClickType.Clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSING, UI.CONTROLS.CLICKING)
		},
		{
			UI.ClickType.Clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSES, UI.CONTROLS.CLICKS)
		},
		{
			UI.ClickType.click.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSLOWER, UI.CONTROLS.CLICKLOWER)
		},
		{
			UI.ClickType.clickable.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLELOWER, UI.CONTROLS.CLICKABLELOWER)
		},
		{
			UI.ClickType.clicked.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDLOWER, UI.CONTROLS.CLICKEDLOWER)
		},
		{
			UI.ClickType.clicking.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGLOWER, UI.CONTROLS.CLICKINGLOWER)
		},
		{
			UI.ClickType.clicks.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESLOWER, UI.CONTROLS.CLICKSLOWER)
		},
		{
			UI.ClickType.CLICK.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSUPPER, UI.CONTROLS.CLICKUPPER)
		},
		{
			UI.ClickType.CLICKABLE.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSABLEUPPER, UI.CONTROLS.CLICKABLEUPPER)
		},
		{
			UI.ClickType.CLICKED.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSEDUPPER, UI.CONTROLS.CLICKEDUPPER)
		},
		{
			UI.ClickType.CLICKING.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSINGUPPER, UI.CONTROLS.CLICKINGUPPER)
		},
		{
			UI.ClickType.CLICKS.ToString(),
			new Pair<LocString, LocString>(UI.CONTROLS.PRESSESUPPER, UI.CONTROLS.CLICKSUPPER)
		}
	};

	// Token: 0x04007D22 RID: 32034
	private const string linkPrefix_open = "<link=\"";

	// Token: 0x04007D23 RID: 32035
	private const string linkSuffix = "</link>";

	// Token: 0x04007D24 RID: 32036
	private const string linkColorPrefix = "<b><style=\"KLink\">";

	// Token: 0x04007D25 RID: 32037
	private const string linkColorSuffix = "</style></b>";

	// Token: 0x04007D26 RID: 32038
	private static readonly string combinedPrefix = "<b><style=\"KLink\"><link=\"";

	// Token: 0x04007D27 RID: 32039
	private static readonly string combinedSuffix = "</style></b></link>";
}
