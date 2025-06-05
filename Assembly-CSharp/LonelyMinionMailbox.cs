using System;
using UnityEngine;

// Token: 0x02000EB6 RID: 3766
public class LonelyMinionMailbox : KMonoBehaviour
{
	// Token: 0x06004B4A RID: 19274 RVA: 0x0026C580 File Offset: 0x0026A780
	public void Initialize(LonelyMinionHouse.Instance house)
	{
		this.House = house;
		SingleEntityReceptacle component = base.GetComponent<SingleEntityReceptacle>();
		component.occupyingObjectRelativePosition = base.transform.InverseTransformPoint(house.GetParcelPosition());
		component.occupyingObjectRelativePosition.z = -1f;
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		StoryInstance storyInstance2 = storyInstance;
		storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
		this.OnStoryStateChanged(storyInstance.CurrentState);
	}

	// Token: 0x06004B4B RID: 19275 RVA: 0x000D5157 File Offset: 0x000D3357
	protected override void OnSpawn()
	{
		if (StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.LonelyMinion))
		{
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06004B4C RID: 19276 RVA: 0x0026C610 File Offset: 0x0026A810
	protected override void OnCleanUp()
	{
		StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
		storyInstance.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance.StoryStateChanged, new Action<StoryInstance.State>(this.OnStoryStateChanged));
	}

	// Token: 0x06004B4D RID: 19277 RVA: 0x0026C65C File Offset: 0x0026A85C
	private void OnStoryStateChanged(StoryInstance.State state)
	{
		QuestInstance quest = QuestManager.GetInstance(this.House.QuestOwnerId, Db.Get().Quests.LonelyMinionFoodQuest);
		if (state == StoryInstance.State.IN_PROGRESS)
		{
			base.Subscribe(-731304873, new Action<object>(this.OnStorageChanged));
			SingleEntityReceptacle singleEntityReceptacle = base.gameObject.AddOrGet<SingleEntityReceptacle>();
			singleEntityReceptacle.enabled = true;
			singleEntityReceptacle.AddAdditionalCriteria(delegate(GameObject candidate)
			{
				EdiblesManager.FoodInfo foodInfo = EdiblesManager.GetFoodInfo(candidate.GetComponent<KPrefabID>().PrefabTag.Name);
				int num = 0;
				return foodInfo != null && quest.DataSatisfiesCriteria(new Quest.ItemData
				{
					CriteriaId = LonelyMinionConfig.FoodCriteriaId,
					QualifyingTag = GameTags.Edible,
					CurrentValue = (float)foodInfo.Quality
				}, ref num);
			});
			RootMenu.Instance.Refresh();
			this.OnStorageChanged(singleEntityReceptacle.Occupant);
		}
		if (state == StoryInstance.State.COMPLETE)
		{
			base.Unsubscribe(-731304873, new Action<object>(this.OnStorageChanged));
			base.gameObject.AddOrGet<Deconstructable>().allowDeconstruction = true;
		}
	}

	// Token: 0x06004B4E RID: 19278 RVA: 0x000D5186 File Offset: 0x000D3386
	private void OnStorageChanged(object data)
	{
		this.House.MailboxContentChanged(data as GameObject);
	}

	// Token: 0x040034B1 RID: 13489
	public LonelyMinionHouse.Instance House;
}
