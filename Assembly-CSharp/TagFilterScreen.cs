using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001B9F RID: 7071
public class TagFilterScreen : SideScreenContent
{
	// Token: 0x0600948F RID: 38031 RVA: 0x00105753 File Offset: 0x00103953
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<TreeFilterable>() != null;
	}

	// Token: 0x06009490 RID: 38032 RVA: 0x003A0230 File Offset: 0x0039E430
	public override void SetTarget(GameObject target)
	{
		if (target == null)
		{
			global::Debug.LogError("The target object provided was null");
			return;
		}
		this.targetFilterable = target.GetComponent<TreeFilterable>();
		if (this.targetFilterable == null)
		{
			global::Debug.LogError("The target provided does not have a Tree Filterable component");
			return;
		}
		if (!this.targetFilterable.showUserMenu)
		{
			return;
		}
		this.Filter(this.targetFilterable.AcceptedTags);
		base.Activate();
	}

	// Token: 0x06009491 RID: 38033 RVA: 0x003A029C File Offset: 0x0039E49C
	protected override void OnActivate()
	{
		this.rootItem = this.BuildDisplay(this.rootTag);
		this.treeControl.SetUserItemRoot(this.rootItem);
		this.treeControl.root.opened = true;
		this.Filter(this.treeControl.root, this.acceptedTags, false);
	}

	// Token: 0x06009492 RID: 38034 RVA: 0x003A02F8 File Offset: 0x0039E4F8
	public static List<Tag> GetAllTags()
	{
		List<Tag> list = new List<Tag>();
		foreach (TagFilterScreen.TagEntry tagEntry in TagFilterScreen.defaultRootTag.children)
		{
			if (tagEntry.tag.IsValid)
			{
				list.Add(tagEntry.tag);
			}
		}
		return list;
	}

	// Token: 0x06009493 RID: 38035 RVA: 0x003A0344 File Offset: 0x0039E544
	private KTreeControl.UserItem BuildDisplay(TagFilterScreen.TagEntry root)
	{
		KTreeControl.UserItem userItem = null;
		if (root.name != null && root.name != "")
		{
			userItem = new KTreeControl.UserItem
			{
				text = root.name,
				userData = root.tag
			};
			List<KTreeControl.UserItem> list = new List<KTreeControl.UserItem>();
			if (root.children != null)
			{
				foreach (TagFilterScreen.TagEntry root2 in root.children)
				{
					list.Add(this.BuildDisplay(root2));
				}
			}
			userItem.children = list;
		}
		return userItem;
	}

	// Token: 0x06009494 RID: 38036 RVA: 0x003A03D0 File Offset: 0x0039E5D0
	private static KTreeControl.UserItem CreateTree(string tree_name, Tag tree_tag, IList<Element> items)
	{
		KTreeControl.UserItem userItem = new KTreeControl.UserItem
		{
			text = tree_name,
			userData = tree_tag,
			children = new List<KTreeControl.UserItem>()
		};
		foreach (Element element in items)
		{
			KTreeControl.UserItem item = new KTreeControl.UserItem
			{
				text = element.name,
				userData = GameTagExtensions.Create(element.id)
			};
			userItem.children.Add(item);
		}
		return userItem;
	}

	// Token: 0x06009495 RID: 38037 RVA: 0x00105761 File Offset: 0x00103961
	public void SetRootTag(TagFilterScreen.TagEntry root_tag)
	{
		this.rootTag = root_tag;
	}

	// Token: 0x06009496 RID: 38038 RVA: 0x0010576A File Offset: 0x0010396A
	public void Filter(HashSet<Tag> acceptedTags)
	{
		this.acceptedTags = acceptedTags;
	}

	// Token: 0x06009497 RID: 38039 RVA: 0x003A046C File Offset: 0x0039E66C
	private void Filter(KTreeItem root, HashSet<Tag> acceptedTags, bool parentEnabled)
	{
		root.checkboxChecked = (parentEnabled || (root.userData != null && acceptedTags.Contains((Tag)root.userData)));
		foreach (KTreeItem root2 in root.children)
		{
			this.Filter(root2, acceptedTags, root.checkboxChecked);
		}
		if (!root.checkboxChecked && root.children.Count > 0)
		{
			bool checkboxChecked = true;
			using (IEnumerator<KTreeItem> enumerator = root.children.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.checkboxChecked)
					{
						checkboxChecked = false;
						break;
					}
				}
			}
			root.checkboxChecked = checkboxChecked;
		}
	}

	// Token: 0x040070B4 RID: 28852
	[SerializeField]
	private KTreeControl treeControl;

	// Token: 0x040070B5 RID: 28853
	private KTreeControl.UserItem rootItem;

	// Token: 0x040070B6 RID: 28854
	private TagFilterScreen.TagEntry rootTag = TagFilterScreen.defaultRootTag;

	// Token: 0x040070B7 RID: 28855
	private HashSet<Tag> acceptedTags = new HashSet<Tag>();

	// Token: 0x040070B8 RID: 28856
	private TreeFilterable targetFilterable;

	// Token: 0x040070B9 RID: 28857
	public static TagFilterScreen.TagEntry defaultRootTag = new TagFilterScreen.TagEntry
	{
		name = "All",
		tag = default(Tag),
		children = new TagFilterScreen.TagEntry[0]
	};

	// Token: 0x02001BA0 RID: 7072
	public class TagEntry
	{
		// Token: 0x040070BA RID: 28858
		public string name;

		// Token: 0x040070BB RID: 28859
		public Tag tag;

		// Token: 0x040070BC RID: 28860
		public TagFilterScreen.TagEntry[] children;
	}
}
