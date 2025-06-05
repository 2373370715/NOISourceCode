using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001279 RID: 4729
[AddComponentMenu("KMonoBehaviour/scripts/DietManager")]
public class DietManager : KMonoBehaviour
{
	// Token: 0x06006076 RID: 24694 RVA: 0x000E34AD File Offset: 0x000E16AD
	public static void DestroyInstance()
	{
		DietManager.Instance = null;
	}

	// Token: 0x06006077 RID: 24695 RVA: 0x000E34B5 File Offset: 0x000E16B5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.diets = DietManager.CollectSaveDiets(null);
		DietManager.Instance = this;
	}

	// Token: 0x06006078 RID: 24696 RVA: 0x002BBD4C File Offset: 0x002B9F4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		foreach (Tag tag in DiscoveredResources.Instance.GetDiscovered())
		{
			this.Discover(tag);
		}
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			Diet.Info[] infos = keyValuePair.Value.infos;
			for (int i = 0; i < infos.Length; i++)
			{
				foreach (Tag tag2 in infos[i].consumedTags)
				{
					if (Assets.GetPrefab(tag2) == null)
					{
						global::Debug.LogError(string.Format("Could not find prefab {0}, required by diet for {1}", tag2, keyValuePair.Key));
					}
				}
			}
		}
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
	}

	// Token: 0x06006079 RID: 24697 RVA: 0x002BBE94 File Offset: 0x002BA094
	private void Discover(Tag tag)
	{
		foreach (KeyValuePair<Tag, Diet> keyValuePair in this.diets)
		{
			if (keyValuePair.Value.GetDietInfo(tag) != null)
			{
				DiscoveredResources.Instance.Discover(tag, keyValuePair.Key);
			}
		}
	}

	// Token: 0x0600607A RID: 24698 RVA: 0x000E34CF File Offset: 0x000E16CF
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		this.Discover(tag);
	}

	// Token: 0x0600607B RID: 24699 RVA: 0x002BBF04 File Offset: 0x002BA104
	public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = diet;
			}
		}
		return dictionary;
	}

	// Token: 0x0600607C RID: 24700 RVA: 0x002BBFAC File Offset: 0x002BA1AC
	public static Dictionary<Tag, Diet> CollectSaveDiets(Tag[] target_species)
	{
		Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
		foreach (KPrefabID kprefabID in Assets.Prefabs)
		{
			CreatureCalorieMonitor.Def def = kprefabID.GetDef<CreatureCalorieMonitor.Def>();
			BeehiveCalorieMonitor.Def def2 = kprefabID.GetDef<BeehiveCalorieMonitor.Def>();
			Diet diet = null;
			if (def != null)
			{
				diet = def.diet;
			}
			else if (def2 != null)
			{
				diet = def2.diet;
			}
			if (diet != null && (target_species == null || Array.IndexOf<Tag>(target_species, kprefabID.GetComponent<CreatureBrain>().species) >= 0))
			{
				dictionary[kprefabID.PrefabTag] = new Diet(diet);
				dictionary[kprefabID.PrefabTag].FilterDLC();
			}
		}
		return dictionary;
	}

	// Token: 0x0600607D RID: 24701 RVA: 0x002BC06C File Offset: 0x002BA26C
	public Diet GetPrefabDiet(GameObject owner)
	{
		Diet result;
		if (this.diets.TryGetValue(owner.GetComponent<KPrefabID>().PrefabTag, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x040044FC RID: 17660
	private Dictionary<Tag, Diet> diets;

	// Token: 0x040044FD RID: 17661
	public static DietManager Instance;
}
