using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200114E RID: 4430
public class CurrentJobConversation : ConversationType
{
	// Token: 0x06005A6B RID: 23147 RVA: 0x000DF450 File Offset: 0x000DD650
	public CurrentJobConversation()
	{
		this.id = "CurrentJobConversation";
	}

	// Token: 0x06005A6C RID: 23148 RVA: 0x000DF463 File Offset: 0x000DD663
	public override void NewTarget(MinionIdentity speaker)
	{
		this.target = "hows_role";
	}

	// Token: 0x06005A6D RID: 23149 RVA: 0x002A2CDC File Offset: 0x002A0EDC
	public override Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		if (lastTopic == null)
		{
			return new Conversation.Topic(this.target, Conversation.ModeType.Query);
		}
		List<Conversation.ModeType> list = CurrentJobConversation.transitions[lastTopic.mode];
		Conversation.ModeType modeType = list[UnityEngine.Random.Range(0, list.Count)];
		if (modeType == Conversation.ModeType.Statement)
		{
			this.target = this.GetRoleForSpeaker(speaker);
			Conversation.ModeType modeForRole = this.GetModeForRole(speaker, this.target);
			return new Conversation.Topic(this.target, modeForRole);
		}
		return new Conversation.Topic(this.target, modeType);
	}

	// Token: 0x06005A6E RID: 23150 RVA: 0x002A2D58 File Offset: 0x002A0F58
	public override Sprite GetSprite(string topic)
	{
		if (topic == "hows_role")
		{
			return Assets.GetSprite("crew_state_role");
		}
		if (Db.Get().Skills.TryGet(topic) != null)
		{
			return Assets.GetSprite(Db.Get().Skills.Get(topic).hat);
		}
		return null;
	}

	// Token: 0x06005A6F RID: 23151 RVA: 0x000B17B4 File Offset: 0x000AF9B4
	private Conversation.ModeType GetModeForRole(MinionIdentity speaker, string roleId)
	{
		return Conversation.ModeType.Nominal;
	}

	// Token: 0x06005A70 RID: 23152 RVA: 0x000DF470 File Offset: 0x000DD670
	private string GetRoleForSpeaker(MinionIdentity speaker)
	{
		return speaker.GetComponent<MinionResume>().CurrentRole;
	}

	// Token: 0x0400407C RID: 16508
	public static Dictionary<Conversation.ModeType, List<Conversation.ModeType>> transitions = new Dictionary<Conversation.ModeType, List<Conversation.ModeType>>
	{
		{
			Conversation.ModeType.Query,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Statement
			}
		},
		{
			Conversation.ModeType.Satisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Agreement
			}
		},
		{
			Conversation.ModeType.Nominal,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Musing
			}
		},
		{
			Conversation.ModeType.Dissatisfaction,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Stressing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Disagreement
			}
		},
		{
			Conversation.ModeType.Agreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Disagreement,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		},
		{
			Conversation.ModeType.Musing,
			new List<Conversation.ModeType>
			{
				Conversation.ModeType.Query,
				Conversation.ModeType.End
			}
		}
	};
}
