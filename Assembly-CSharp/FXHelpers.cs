using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C2A RID: 3114
public static class FXHelpers
{
	// Token: 0x06003AF8 RID: 15096 RVA: 0x00236D1C File Offset: 0x00234F1C
	public static KBatchedAnimController CreateEffect(string anim_file_name, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_name);
		component.name = anim_file_name;
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		KAnimFile anim = Assets.GetAnim(anim_file_name);
		if (anim == null)
		{
			global::Debug.LogWarning("Missing effect anim: " + anim_file_name);
		}
		else
		{
			component.AnimFiles = new KAnimFile[]
			{
				anim
			};
		}
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}

	// Token: 0x06003AF9 RID: 15097 RVA: 0x00236DDC File Offset: 0x00234FDC
	public static KBatchedAnimController CreateEffect(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in (from name in anim_file_names
		select new ValueTuple<string, KAnimFile>(name, Assets.GetAnim(name))).Where(delegate([TupleElementNames(new string[]
		{
			"name",
			"anim"
		})] ValueTuple<string, KAnimFile> e)
		{
			if (e.Item2 == null)
			{
				global::Debug.LogWarning("Missing effect anim: " + e.Item1);
				return false;
			}
			return true;
		})
		select e.Item2).ToArray<KAnimFile>();
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}

	// Token: 0x06003AFA RID: 15098 RVA: 0x00236EE0 File Offset: 0x002350E0
	public static KBatchedAnimController CreateEffectOverride(string[] anim_file_names, Vector3 position, Transform parent = null, bool update_looping_sounds_position = false, Grid.SceneLayer layer = Grid.SceneLayer.Front, bool set_inactive = false)
	{
		KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.EffectTemplateOverrideId), position, layer, null, 0).GetComponent<KBatchedAnimController>();
		component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_names[0]);
		component.name = anim_file_names[0];
		if (parent != null)
		{
			component.transform.SetParent(parent, false);
		}
		component.transform.SetPosition(position);
		if (update_looping_sounds_position)
		{
			component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
		}
		component.AnimFiles = (from e in (from name in anim_file_names
		select new ValueTuple<string, KAnimFile>(name, Assets.GetAnim(name))).Where(delegate([TupleElementNames(new string[]
		{
			"name",
			"anim"
		})] ValueTuple<string, KAnimFile> e)
		{
			if (e.Item2 == null)
			{
				global::Debug.LogWarning("Missing effect anim: " + e.Item1);
				return false;
			}
			return true;
		})
		select e.Item2).ToArray<KAnimFile>();
		if (!set_inactive)
		{
			component.gameObject.SetActive(true);
		}
		return component;
	}
}
