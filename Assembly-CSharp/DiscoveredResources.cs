using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using UnityEngine;

// Token: 0x0200127A RID: 4730
[SerializationConfig(MemberSerialization.OptIn)]
public class DiscoveredResources : KMonoBehaviour, ISaveLoadable, ISim4000ms
{
	// Token: 0x0600607F RID: 24703 RVA: 0x000E34D8 File Offset: 0x000E16D8
	public static void DestroyInstance()
	{
		DiscoveredResources.Instance = null;
	}

	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06006080 RID: 24704 RVA: 0x002BC098 File Offset: 0x002BA298
	// (remove) Token: 0x06006081 RID: 24705 RVA: 0x002BC0D0 File Offset: 0x002BA2D0
	public event Action<Tag, Tag> OnDiscover;

	// Token: 0x06006082 RID: 24706 RVA: 0x002BC108 File Offset: 0x002BA308
	public void Discover(Tag tag, Tag categoryTag)
	{
		bool flag = this.Discovered.Add(tag);
		this.DiscoverCategory(categoryTag, tag);
		if (flag)
		{
			if (this.OnDiscover != null)
			{
				this.OnDiscover(categoryTag, tag);
			}
			if (!this.newDiscoveries.ContainsKey(tag))
			{
				this.newDiscoveries.Add(tag, (float)GameClock.Instance.GetCycle() + GameClock.Instance.GetCurrentCycleAsPercentage());
			}
		}
	}

	// Token: 0x06006083 RID: 24707 RVA: 0x000E34E0 File Offset: 0x000E16E0
	public void Discover(Tag tag)
	{
		this.Discover(tag, DiscoveredResources.GetCategoryForEntity(Assets.GetPrefab(tag).GetComponent<KPrefabID>()));
	}

	// Token: 0x06006084 RID: 24708 RVA: 0x000E34F9 File Offset: 0x000E16F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		DiscoveredResources.Instance = this;
	}

	// Token: 0x06006085 RID: 24709 RVA: 0x000E3507 File Offset: 0x000E1707
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.FilterDisabledContent();
	}

	// Token: 0x06006086 RID: 24710 RVA: 0x002BC170 File Offset: 0x002BA370
	private void FilterDisabledContent()
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		foreach (Tag tag in this.Discovered)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null && element.disabled)
			{
				hashSet.Add(tag);
			}
			else
			{
				GameObject gameObject = Assets.TryGetPrefab(tag);
				if (gameObject != null && gameObject.HasTag(GameTags.DeprecatedContent))
				{
					hashSet.Add(tag);
				}
				else if (gameObject == null)
				{
					hashSet.Add(tag);
				}
			}
		}
		foreach (Tag item in hashSet)
		{
			this.Discovered.Remove(item);
		}
		foreach (KeyValuePair<Tag, HashSet<Tag>> keyValuePair in this.DiscoveredCategories)
		{
			foreach (Tag item2 in hashSet)
			{
				if (keyValuePair.Value.Contains(item2))
				{
					keyValuePair.Value.Remove(item2);
				}
			}
		}
		foreach (string s in new List<string>
		{
			"Pacu",
			"PacuCleaner",
			"PacuTropical",
			"PacuBaby",
			"PacuCleanerBaby",
			"PacuTropicalBaby"
		})
		{
			if (this.DiscoveredCategories.ContainsKey(s))
			{
				List<Tag> list = this.DiscoveredCategories[s].ToList<Tag>();
				SolidConsumerMonitor.Def def = Assets.GetPrefab(s).GetDef<SolidConsumerMonitor.Def>();
				foreach (Tag tag2 in list)
				{
					if (def.diet.GetDietInfo(tag2) == null)
					{
						this.DiscoveredCategories[s].Remove(tag2);
					}
				}
			}
		}
	}

	// Token: 0x06006087 RID: 24711 RVA: 0x002BC418 File Offset: 0x002BA618
	public bool CheckAllDiscoveredAreNew()
	{
		foreach (Tag key in this.Discovered)
		{
			if (!this.newDiscoveries.ContainsKey(key))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06006088 RID: 24712 RVA: 0x002BC47C File Offset: 0x002BA67C
	private void DiscoverCategory(Tag category_tag, Tag item_tag)
	{
		HashSet<Tag> hashSet;
		if (!this.DiscoveredCategories.TryGetValue(category_tag, out hashSet))
		{
			hashSet = new HashSet<Tag>();
			this.DiscoveredCategories[category_tag] = hashSet;
		}
		hashSet.Add(item_tag);
	}

	// Token: 0x06006089 RID: 24713 RVA: 0x000E3515 File Offset: 0x000E1715
	public HashSet<Tag> GetDiscovered()
	{
		return this.Discovered;
	}

	// Token: 0x0600608A RID: 24714 RVA: 0x000E351D File Offset: 0x000E171D
	public bool IsDiscovered(Tag tag)
	{
		return this.Discovered.Contains(tag) || this.DiscoveredCategories.ContainsKey(tag);
	}

	// Token: 0x0600608B RID: 24715 RVA: 0x002BC4B4 File Offset: 0x002BA6B4
	public bool AnyDiscovered(ICollection<Tag> tags)
	{
		foreach (Tag tag in tags)
		{
			if (this.IsDiscovered(tag))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600608C RID: 24716 RVA: 0x000E353B File Offset: 0x000E173B
	public bool TryGetDiscoveredResourcesFromTag(Tag tag, out HashSet<Tag> resources)
	{
		return this.DiscoveredCategories.TryGetValue(tag, out resources);
	}

	// Token: 0x0600608D RID: 24717 RVA: 0x002BC508 File Offset: 0x002BA708
	public HashSet<Tag> GetDiscoveredResourcesFromTag(Tag tag)
	{
		HashSet<Tag> result;
		if (this.DiscoveredCategories.TryGetValue(tag, out result))
		{
			return result;
		}
		return new HashSet<Tag>();
	}

	// Token: 0x0600608E RID: 24718 RVA: 0x002BC52C File Offset: 0x002BA72C
	public Dictionary<Tag, HashSet<Tag>> GetDiscoveredResourcesFromTagSet(TagSet tagSet)
	{
		Dictionary<Tag, HashSet<Tag>> dictionary = new Dictionary<Tag, HashSet<Tag>>();
		foreach (Tag key in tagSet)
		{
			HashSet<Tag> value;
			if (this.DiscoveredCategories.TryGetValue(key, out value))
			{
				dictionary[key] = value;
			}
		}
		return dictionary;
	}

	// Token: 0x0600608F RID: 24719 RVA: 0x002BC58C File Offset: 0x002BA78C
	public static Tag GetCategoryForTags(HashSet<Tag> tags)
	{
		Tag result = Tag.Invalid;
		foreach (Tag tag in tags)
		{
			if (GameTags.AllCategories.Contains(tag) || GameTags.IgnoredMaterialCategories.Contains(tag))
			{
				result = tag;
				break;
			}
		}
		return result;
	}

	// Token: 0x06006090 RID: 24720 RVA: 0x002BC5F8 File Offset: 0x002BA7F8
	public static Tag GetCategoryForEntity(KPrefabID entity)
	{
		ElementChunk component = entity.GetComponent<ElementChunk>();
		if (component != null)
		{
			return component.GetComponent<PrimaryElement>().Element.materialCategory;
		}
		return DiscoveredResources.GetCategoryForTags(entity.Tags);
	}

	// Token: 0x06006091 RID: 24721 RVA: 0x002BC634 File Offset: 0x002BA834
	public void Sim4000ms(float dt)
	{
		float num = GameClock.Instance.GetTimeInCycles() + GameClock.Instance.GetCurrentCycleAsPercentage();
		List<Tag> list = new List<Tag>();
		foreach (KeyValuePair<Tag, float> keyValuePair in this.newDiscoveries)
		{
			if (num - keyValuePair.Value > 3f)
			{
				list.Add(keyValuePair.Key);
			}
		}
		foreach (Tag key in list)
		{
			this.newDiscoveries.Remove(key);
		}
	}

	// Token: 0x040044FE RID: 17662
	public static DiscoveredResources Instance;

	// Token: 0x040044FF RID: 17663
	[Serialize]
	private HashSet<Tag> Discovered = new HashSet<Tag>();

	// Token: 0x04004500 RID: 17664
	[Serialize]
	private Dictionary<Tag, HashSet<Tag>> DiscoveredCategories = new Dictionary<Tag, HashSet<Tag>>();

	// Token: 0x04004502 RID: 17666
	[Serialize]
	public Dictionary<Tag, float> newDiscoveries = new Dictionary<Tag, float>();
}
