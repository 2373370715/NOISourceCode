using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200114F RID: 4431
public class RecentThingConversation : ConversationType
{
	// Token: 0x06005A72 RID: 23154 RVA: 0x000DF47D File Offset: 0x000DD67D
	public RecentThingConversation()
	{
		this.id = "RecentThingConversation";
	}

	// Token: 0x06005A73 RID: 23155 RVA: 0x002A2E80 File Offset: 0x002A1080
	public override void NewTarget(MinionIdentity speaker)
	{
		ConversationMonitor.Instance smi = speaker.GetSMI<ConversationMonitor.Instance>();
		this.target = smi.GetATopic();
	}

	// Token: 0x06005A74 RID: 23156 RVA: 0x002A2EA0 File Offset: 0x002A10A0
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (string.IsNullOrEmpty(this.target))
		{
			return null;
		}
		List<Conversation.ModeType> list;
		if (lastTopic == null)
		{
			list = new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.Statement,
				Conversation.ModeType.Musing
			};
		}
		else
		{
			list = RecentThingConversation.transitions[lastTopic.mode];
		}
		Conversation.ModeType mode = list[UnityEngine.Random.Range(0, list.Count)];
		return new Conversation.Topic(this.target, mode);
	}

	// Token: 0x06005A75 RID: 23157 RVA: 0x002A2F0C File Offset: 0x002A110C
	public override Sprite GetSprite(string topic)
	{
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			return uisprite.first;
		}
		return null;
	}

	// Token: 0x0400407D RID: 16509
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Statement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement,
				Conversation.ModeType.Disagreement,
				Conversation.ModeType.Query,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Satisfaction
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Dissatisfaction
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.Statement,
				Conversation.ModeType.Segue
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Segue,
				Conversation.ModeType.End
			}
		}
	};
}
