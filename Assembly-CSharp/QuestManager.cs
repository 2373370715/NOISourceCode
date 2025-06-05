using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001778 RID: 6008
[SerializationConfig(MemberSerialization.OptIn)]
public class QuestManager : KMonoBehaviour
{
	// Token: 0x06007B9B RID: 31643 RVA: 0x000F5BE3 File Offset: 0x000F3DE3
	protected override void OnPrefabInit()
	{
		if (QuestManager.instance != null)
		{
			UnityEngine.Object.Destroy(QuestManager.instance);
			return;
		}
		QuestManager.instance = this;
		base.OnPrefabInit();
	}

	// Token: 0x06007B9C RID: 31644 RVA: 0x0032B304 File Offset: 0x00329504
	public static QuestInstance InitializeQuest(Tag ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.GetHash()][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06007B9D RID: 31645 RVA: 0x0032B358 File Offset: 0x00329558
	public static QuestInstance InitializeQuest(HashedString ownerId, Quest quest)
	{
		QuestInstance questInstance;
		if (!QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance))
		{
			questInstance = (QuestManager.instance.ownerToQuests[ownerId.HashValue][quest.IdHash] = new QuestInstance(quest));
		}
		questInstance.Initialize(quest);
		return questInstance;
	}

	// Token: 0x06007B9E RID: 31646 RVA: 0x0032B3AC File Offset: 0x003295AC
	public static QuestInstance GetInstance(Tag ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out result);
		return result;
	}

	// Token: 0x06007B9F RID: 31647 RVA: 0x0032B3CC File Offset: 0x003295CC
	public static QuestInstance GetInstance(HashedString ownerId, Quest quest)
	{
		QuestInstance result;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out result);
		return result;
	}

	// Token: 0x06007BA0 RID: 31648 RVA: 0x0032B3EC File Offset: 0x003295EC
	public static bool CheckState(HashedString ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.HashValue, quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06007BA1 RID: 31649 RVA: 0x0032B418 File Offset: 0x00329618
	public static bool CheckState(Tag ownerId, Quest quest, Quest.State state)
	{
		QuestInstance questInstance;
		QuestManager.TryGetQuest(ownerId.GetHash(), quest, out questInstance);
		return questInstance != null && questInstance.CurrentState == state;
	}

	// Token: 0x06007BA2 RID: 31650 RVA: 0x0032B444 File Offset: 0x00329644
	private static bool TryGetQuest(int ownerId, Quest quest, out QuestInstance qInst)
	{
		qInst = null;
		Dictionary<HashedString, QuestInstance> dictionary;
		if (!QuestManager.instance.ownerToQuests.TryGetValue(ownerId, out dictionary))
		{
			dictionary = (QuestManager.instance.ownerToQuests[ownerId] = new Dictionary<HashedString, QuestInstance>());
		}
		return dictionary.TryGetValue(quest.IdHash, out qInst);
	}

	// Token: 0x04005D2B RID: 23851
	private static QuestManager instance;

	// Token: 0x04005D2C RID: 23852
	[Serialize]
	private Dictionary<int, Dictionary<HashedString, QuestInstance>> ownerToQuests = new Dictionary<int, Dictionary<HashedString, QuestInstance>>();
}
