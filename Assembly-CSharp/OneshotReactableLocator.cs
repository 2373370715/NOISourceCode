using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class OneshotReactableLocator : IEntityConfig
{
	// Token: 0x060013D2 RID: 5074 RVA: 0x0019A0CC File Offset: 0x001982CC
	public static EmoteReactable CreateOneshotReactable(GameObject source, float lifetime, string id, ChoreType chore_type, int range_width = 15, int range_height = 15, float min_reactor_time = 20f)
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OneshotReactableLocator.ID), source.transform.GetPosition());
		EmoteReactable emoteReactable = new EmoteReactable(gameObject, id, chore_type, range_width, range_height, 100000f, min_reactor_time, float.PositiveInfinity, 0f);
		emoteReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
		OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
		component.lifetime = lifetime;
		component.SetReactable(emoteReactable);
		gameObject.SetActive(true);
		return emoteReactable;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x000B3077 File Offset: 0x000B1277
	private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
	{
		return (GameObject reactor, Navigator.ActiveTransition transition) => reactor != source;
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x000B3090 File Offset: 0x000B1290
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
		gameObject.AddTag(GameTags.NotConversationTopic);
		gameObject.AddOrGet<OneshotReactableHost>();
		return gameObject;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject go)
	{
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject go)
	{
	}

	// Token: 0x04000D97 RID: 3479
	public static readonly string ID = "OneshotReactableLocator";
}
