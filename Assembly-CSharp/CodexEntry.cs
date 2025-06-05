using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001C7F RID: 7295
public class CodexEntry : IHasDlcRestrictions
{
	// Token: 0x060097E5 RID: 38885 RVA: 0x0010764C File Offset: 0x0010584C
	public CodexEntry()
	{
	}

	// Token: 0x060097E6 RID: 38886 RVA: 0x003B61E8 File Offset: 0x003B43E8
	public CodexEntry(string category, List<ContentContainer> contentContainers, string name)
	{
		this.category = category;
		this.name = name;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(name);
		}
	}

	// Token: 0x060097E7 RID: 38887 RVA: 0x003B6260 File Offset: 0x003B4460
	public CodexEntry(string category, string titleKey, List<ContentContainer> contentContainers)
	{
		this.category = category;
		this.title = titleKey;
		this.contentContainers = contentContainers;
		if (string.IsNullOrEmpty(this.sortString))
		{
			this.sortString = UI.StripLinkFormatting(this.title);
		}
	}

	// Token: 0x170009E3 RID: 2531
	// (get) Token: 0x060097E8 RID: 38888 RVA: 0x0010768B File Offset: 0x0010588B
	// (set) Token: 0x060097E9 RID: 38889 RVA: 0x00107693 File Offset: 0x00105893
	public List<ContentContainer> contentContainers
	{
		get
		{
			return this._contentContainers;
		}
		private set
		{
			this._contentContainers = value;
		}
	}

	// Token: 0x060097EA RID: 38890 RVA: 0x003B62E0 File Offset: 0x003B44E0
	public static List<string> ContentContainerDebug(List<ContentContainer> _contentContainers)
	{
		List<string> list = new List<string>();
		foreach (ContentContainer contentContainer in _contentContainers)
		{
			if (contentContainer != null)
			{
				string text = string.Concat(new string[]
				{
					"<b>",
					contentContainer.contentLayout.ToString(),
					" container: ",
					((contentContainer.content == null) ? 0 : contentContainer.content.Count).ToString(),
					" items</b>"
				});
				if (contentContainer.content != null)
				{
					text += "\n";
					for (int i = 0; i < contentContainer.content.Count; i++)
					{
						text = string.Concat(new string[]
						{
							text,
							"    • ",
							contentContainer.content[i].ToString(),
							": ",
							CodexEntry.GetContentWidgetDebugString(contentContainer.content[i]),
							"\n"
						});
					}
				}
				list.Add(text);
			}
			else
			{
				list.Add("null container");
			}
		}
		return list;
	}

	// Token: 0x060097EB RID: 38891 RVA: 0x003B6438 File Offset: 0x003B4638
	private static string GetContentWidgetDebugString(ICodexWidget widget)
	{
		CodexText codexText = widget as CodexText;
		if (codexText != null)
		{
			return codexText.text;
		}
		CodexLabelWithIcon codexLabelWithIcon = widget as CodexLabelWithIcon;
		if (codexLabelWithIcon != null)
		{
			return codexLabelWithIcon.label.text + " / " + codexLabelWithIcon.icon.spriteName;
		}
		CodexImage codexImage = widget as CodexImage;
		if (codexImage != null)
		{
			return codexImage.spriteName;
		}
		CodexVideo codexVideo = widget as CodexVideo;
		if (codexVideo != null)
		{
			return codexVideo.name;
		}
		CodexIndentedLabelWithIcon codexIndentedLabelWithIcon = widget as CodexIndentedLabelWithIcon;
		if (codexIndentedLabelWithIcon != null)
		{
			return codexIndentedLabelWithIcon.label.text + " / " + codexIndentedLabelWithIcon.icon.spriteName;
		}
		return "";
	}

	// Token: 0x060097EC RID: 38892 RVA: 0x0010769C File Offset: 0x0010589C
	public void CreateContentContainerCollection()
	{
		this.contentContainers = new List<ContentContainer>();
	}

	// Token: 0x060097ED RID: 38893 RVA: 0x001076A9 File Offset: 0x001058A9
	public void InsertContentContainer(int index, ContentContainer container)
	{
		this.contentContainers.Insert(index, container);
	}

	// Token: 0x060097EE RID: 38894 RVA: 0x001076B8 File Offset: 0x001058B8
	public void RemoveContentContainerAt(int index)
	{
		this.contentContainers.RemoveAt(index);
	}

	// Token: 0x060097EF RID: 38895 RVA: 0x001076C6 File Offset: 0x001058C6
	public void AddContentContainer(ContentContainer container)
	{
		this.contentContainers.Add(container);
	}

	// Token: 0x060097F0 RID: 38896 RVA: 0x001076D4 File Offset: 0x001058D4
	public void AddContentContainerRange(IEnumerable<ContentContainer> containers)
	{
		this.contentContainers.AddRange(containers);
	}

	// Token: 0x060097F1 RID: 38897 RVA: 0x001076E2 File Offset: 0x001058E2
	public void RemoveContentContainer(ContentContainer container)
	{
		this.contentContainers.Remove(container);
	}

	// Token: 0x060097F2 RID: 38898 RVA: 0x003B64D8 File Offset: 0x003B46D8
	public ICodexWidget GetFirstWidget()
	{
		for (int i = 0; i < this.contentContainers.Count; i++)
		{
			if (this.contentContainers[i].content != null)
			{
				for (int j = 0; j < this.contentContainers[i].content.Count; j++)
				{
					if (this.contentContainers[i].content[j] != null)
					{
						return this.contentContainers[i].content[j];
					}
				}
			}
		}
		return null;
	}

	// Token: 0x170009E4 RID: 2532
	// (get) Token: 0x060097F3 RID: 38899 RVA: 0x001076F1 File Offset: 0x001058F1
	// (set) Token: 0x060097F4 RID: 38900 RVA: 0x001076F9 File Offset: 0x001058F9
	public string[] requiredDlcIds { get; set; }

	// Token: 0x170009E5 RID: 2533
	// (get) Token: 0x060097F5 RID: 38901 RVA: 0x00107702 File Offset: 0x00105902
	// (set) Token: 0x060097F6 RID: 38902 RVA: 0x0010770A File Offset: 0x0010590A
	public string[] forbiddenDlcIds { get; set; }

	// Token: 0x060097F7 RID: 38903 RVA: 0x00107713 File Offset: 0x00105913
	public string[] GetRequiredDlcIds()
	{
		return this.requiredDlcIds;
	}

	// Token: 0x060097F8 RID: 38904 RVA: 0x0010771B File Offset: 0x0010591B
	public string[] GetForbiddenDlcIds()
	{
		return this.forbiddenDlcIds;
	}

	// Token: 0x170009E6 RID: 2534
	// (get) Token: 0x060097F9 RID: 38905 RVA: 0x00107723 File Offset: 0x00105923
	// (set) Token: 0x060097FA RID: 38906 RVA: 0x0010772B File Offset: 0x0010592B
	public string id
	{
		get
		{
			return this._id;
		}
		set
		{
			this._id = value;
		}
	}

	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x060097FB RID: 38907 RVA: 0x00107734 File Offset: 0x00105934
	// (set) Token: 0x060097FC RID: 38908 RVA: 0x0010773C File Offset: 0x0010593C
	public string parentId
	{
		get
		{
			return this._parentId;
		}
		set
		{
			this._parentId = value;
		}
	}

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x060097FD RID: 38909 RVA: 0x00107745 File Offset: 0x00105945
	// (set) Token: 0x060097FE RID: 38910 RVA: 0x0010774D File Offset: 0x0010594D
	public string category
	{
		get
		{
			return this._category;
		}
		set
		{
			this._category = value;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x060097FF RID: 38911 RVA: 0x00107756 File Offset: 0x00105956
	// (set) Token: 0x06009800 RID: 38912 RVA: 0x0010775E File Offset: 0x0010595E
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			this._title = value;
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06009801 RID: 38913 RVA: 0x00107767 File Offset: 0x00105967
	// (set) Token: 0x06009802 RID: 38914 RVA: 0x0010776F File Offset: 0x0010596F
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			this._name = value;
		}
	}

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06009803 RID: 38915 RVA: 0x00107778 File Offset: 0x00105978
	// (set) Token: 0x06009804 RID: 38916 RVA: 0x00107780 File Offset: 0x00105980
	public string subtitle
	{
		get
		{
			return this._subtitle;
		}
		set
		{
			this._subtitle = value;
		}
	}

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06009805 RID: 38917 RVA: 0x00107789 File Offset: 0x00105989
	// (set) Token: 0x06009806 RID: 38918 RVA: 0x00107791 File Offset: 0x00105991
	public List<SubEntry> subEntries
	{
		get
		{
			return this._subEntries;
		}
		set
		{
			this._subEntries = value;
		}
	}

	// Token: 0x170009ED RID: 2541
	// (get) Token: 0x06009807 RID: 38919 RVA: 0x0010779A File Offset: 0x0010599A
	// (set) Token: 0x06009808 RID: 38920 RVA: 0x001077A2 File Offset: 0x001059A2
	public List<CodexEntry_MadeAndUsed> contentMadeAndUsed
	{
		get
		{
			return this._contentMadeAndUsed;
		}
		set
		{
			this._contentMadeAndUsed = value;
		}
	}

	// Token: 0x170009EE RID: 2542
	// (get) Token: 0x06009809 RID: 38921 RVA: 0x001077AB File Offset: 0x001059AB
	// (set) Token: 0x0600980A RID: 38922 RVA: 0x001077B3 File Offset: 0x001059B3
	public Sprite icon
	{
		get
		{
			return this._icon;
		}
		set
		{
			this._icon = value;
		}
	}

	// Token: 0x170009EF RID: 2543
	// (get) Token: 0x0600980B RID: 38923 RVA: 0x001077BC File Offset: 0x001059BC
	// (set) Token: 0x0600980C RID: 38924 RVA: 0x001077C4 File Offset: 0x001059C4
	public Color iconColor
	{
		get
		{
			return this._iconColor;
		}
		set
		{
			this._iconColor = value;
		}
	}

	// Token: 0x170009F0 RID: 2544
	// (get) Token: 0x0600980D RID: 38925 RVA: 0x001077CD File Offset: 0x001059CD
	// (set) Token: 0x0600980E RID: 38926 RVA: 0x001077D5 File Offset: 0x001059D5
	public string iconPrefabID
	{
		get
		{
			return this._iconPrefabID;
		}
		set
		{
			this._iconPrefabID = value;
		}
	}

	// Token: 0x170009F1 RID: 2545
	// (get) Token: 0x0600980F RID: 38927 RVA: 0x001077DE File Offset: 0x001059DE
	// (set) Token: 0x06009810 RID: 38928 RVA: 0x001077E6 File Offset: 0x001059E6
	public string iconLockID
	{
		get
		{
			return this._iconLockID;
		}
		set
		{
			this._iconLockID = value;
		}
	}

	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x06009811 RID: 38929 RVA: 0x001077EF File Offset: 0x001059EF
	// (set) Token: 0x06009812 RID: 38930 RVA: 0x001077F7 File Offset: 0x001059F7
	public string iconAssetName
	{
		get
		{
			return this._iconAssetName;
		}
		set
		{
			this._iconAssetName = value;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x06009813 RID: 38931 RVA: 0x00107800 File Offset: 0x00105A00
	// (set) Token: 0x06009814 RID: 38932 RVA: 0x00107808 File Offset: 0x00105A08
	public bool disabled
	{
		get
		{
			return this._disabled;
		}
		set
		{
			this._disabled = value;
		}
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x06009815 RID: 38933 RVA: 0x00107811 File Offset: 0x00105A11
	// (set) Token: 0x06009816 RID: 38934 RVA: 0x00107819 File Offset: 0x00105A19
	public bool searchOnly
	{
		get
		{
			return this._searchOnly;
		}
		set
		{
			this._searchOnly = value;
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x06009817 RID: 38935 RVA: 0x00107822 File Offset: 0x00105A22
	// (set) Token: 0x06009818 RID: 38936 RVA: 0x0010782A File Offset: 0x00105A2A
	public int customContentLength
	{
		get
		{
			return this._customContentLength;
		}
		set
		{
			this._customContentLength = value;
		}
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x06009819 RID: 38937 RVA: 0x00107833 File Offset: 0x00105A33
	// (set) Token: 0x0600981A RID: 38938 RVA: 0x0010783B File Offset: 0x00105A3B
	public string sortString
	{
		get
		{
			return this._sortString;
		}
		set
		{
			this._sortString = value;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x0600981B RID: 38939 RVA: 0x00107844 File Offset: 0x00105A44
	// (set) Token: 0x0600981C RID: 38940 RVA: 0x0010784C File Offset: 0x00105A4C
	public bool showBeforeGeneratedCategoryLinks
	{
		get
		{
			return this._showBeforeGeneratedCategoryLinks;
		}
		set
		{
			this._showBeforeGeneratedCategoryLinks = value;
		}
	}

	// Token: 0x04007640 RID: 30272
	public EntryDevLog log = new EntryDevLog();

	// Token: 0x04007641 RID: 30273
	private List<ContentContainer> _contentContainers = new List<ContentContainer>();

	// Token: 0x04007644 RID: 30276
	private string _id;

	// Token: 0x04007645 RID: 30277
	private string _parentId;

	// Token: 0x04007646 RID: 30278
	private string _category;

	// Token: 0x04007647 RID: 30279
	private string _title;

	// Token: 0x04007648 RID: 30280
	private string _name;

	// Token: 0x04007649 RID: 30281
	private string _subtitle;

	// Token: 0x0400764A RID: 30282
	private List<SubEntry> _subEntries = new List<SubEntry>();

	// Token: 0x0400764B RID: 30283
	private List<CodexEntry_MadeAndUsed> _contentMadeAndUsed = new List<CodexEntry_MadeAndUsed>();

	// Token: 0x0400764C RID: 30284
	private Sprite _icon;

	// Token: 0x0400764D RID: 30285
	private Color _iconColor = Color.white;

	// Token: 0x0400764E RID: 30286
	private string _iconPrefabID;

	// Token: 0x0400764F RID: 30287
	private string _iconLockID;

	// Token: 0x04007650 RID: 30288
	private string _iconAssetName;

	// Token: 0x04007651 RID: 30289
	private bool _disabled;

	// Token: 0x04007652 RID: 30290
	private bool _searchOnly;

	// Token: 0x04007653 RID: 30291
	private int _customContentLength;

	// Token: 0x04007654 RID: 30292
	private string _sortString;

	// Token: 0x04007655 RID: 30293
	private bool _showBeforeGeneratedCategoryLinks;
}
