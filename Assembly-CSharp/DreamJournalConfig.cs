using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000397 RID: 919
public class DreamJournalConfig : IEntityConfig
{
	// Token: 0x06000EC8 RID: 3784 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00184CE8 File Offset: 0x00182EE8
	public GameObject CreatePrefab()
	{
		KAnimFile anim = Assets.GetAnim("dream_journal_kanim");
		GameObject gameObject = EntityTemplates.CreateLooseEntity(DreamJournalConfig.ID.Name, ITEMS.DREAMJOURNAL.NAME, ITEMS.DREAMJOURNAL.DESC, 1f, true, anim, "object", Grid.SceneLayer.BuildingFront, EntityTemplates.CollisionShape.RECTANGLE, 0.6f, 0.7f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.StoryTraitResource
		});
		gameObject.AddOrGet<EntitySplitter>().maxStackSize = 25f;
		return gameObject;
	}

	// Token: 0x04000AF0 RID: 2800
	public static Tag ID = new Tag("DreamJournal");

	// Token: 0x04000AF1 RID: 2801
	public const float MASS = 1f;

	// Token: 0x04000AF2 RID: 2802
	public const int FABRICATION_TIME_SECONDS = 300;

	// Token: 0x04000AF3 RID: 2803
	private const string ANIM_FILE = "dream_journal_kanim";

	// Token: 0x04000AF4 RID: 2804
	private const string INITIAL_ANIM = "object";

	// Token: 0x04000AF5 RID: 2805
	public const int MAX_STACK_SIZE = 25;
}
