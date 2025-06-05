using System;
using System.Collections.Generic;

// Token: 0x020010A5 RID: 4261
public class Chatty : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x0600568B RID: 22155 RVA: 0x000DCD85 File Offset: 0x000DAF85
	protected override void OnPrefabInit()
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		base.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		this.identity = base.GetComponent<MinionIdentity>();
	}

	// Token: 0x0600568C RID: 22156 RVA: 0x00290918 File Offset: 0x0028EB18
	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.conversationPartners.Add(minionIdentity);
	}

	// Token: 0x0600568D RID: 22157 RVA: 0x00290944 File Offset: 0x0028EB44
	public void SimEveryTick(float dt)
	{
		if (this.conversationPartners.Count == 0)
		{
			return;
		}
		for (int i = this.conversationPartners.Count - 1; i >= 0; i--)
		{
			MinionIdentity minionIdentity = this.conversationPartners[i];
			this.conversationPartners.RemoveAt(i);
			if (!(minionIdentity == this.identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}

	// Token: 0x04003D51 RID: 15697
	private MinionIdentity identity;

	// Token: 0x04003D52 RID: 15698
	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();
}
