using System;
using UnityEngine;

// Token: 0x0200114D RID: 4429
public class ConversationType
{
	// Token: 0x06005A67 RID: 23143 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void NewTarget(MinionIdentity speaker)
	{
	}

	// Token: 0x06005A68 RID: 23144 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		return null;
	}

	// Token: 0x06005A69 RID: 23145 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual Sprite GetSprite(string topic)
	{
		return null;
	}

	// Token: 0x0400407A RID: 16506
	public string id;

	// Token: 0x0400407B RID: 16507
	public string target;
}
