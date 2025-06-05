using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02001145 RID: 4421
[AddComponentMenu("KMonoBehaviour/scripts/ConversationManager")]
public class ConversationManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x06005A52 RID: 23122 RVA: 0x000DF343 File Offset: 0x000DD543
	protected override void OnPrefabInit()
	{
		this.activeSetups = new List<Conversation>();
		this.lastConvoTimeByMinion = new Dictionary<MinionIdentity, float>();
		this.simRenderLoadBalance = true;
	}

	// Token: 0x06005A53 RID: 23123 RVA: 0x002A2258 File Offset: 0x002A0458
	public void Sim200ms(float dt)
	{
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			Conversation conversation = this.activeSetups[i];
			for (int j = conversation.minions.Count - 1; j >= 0; j--)
			{
				if (!this.ValidMinionTags(conversation.minions[j]) || !this.MinionCloseEnoughToConvo(conversation.minions[j], conversation))
				{
					conversation.minions.RemoveAt(j);
				}
				else
				{
					this.setupsByMinion[conversation.minions[j]] = conversation;
				}
			}
			if (conversation.minions.Count <= 1)
			{
				this.activeSetups.RemoveAt(i);
			}
			else
			{
				bool flag = true;
				bool flag2 = conversation.minions.Find((MinionIdentity match) => !match.HasTag(GameTags.Partying)) == null;
				if ((conversation.numUtterances == 0 && flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().delayBeforeStart)
				{
					MinionIdentity minionIdentity = conversation.minions[UnityEngine.Random.Range(0, conversation.minions.Count)];
					conversation.conversationType.NewTarget(minionIdentity);
					flag = this.DoTalking(conversation, minionIdentity);
				}
				else if (conversation.numUtterances > 0 && conversation.numUtterances < TuningData<ConversationManager.Tuning>.Get().maxUtterances && ((flag2 && GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime / 4f) || GameClock.Instance.GetTime() > conversation.lastTalkedTime + TuningData<ConversationManager.Tuning>.Get().speakTime + TuningData<ConversationManager.Tuning>.Get().delayBetweenUtterances))
				{
					int index = (conversation.minions.IndexOf(conversation.lastTalked) + UnityEngine.Random.Range(1, conversation.minions.Count)) % conversation.minions.Count;
					MinionIdentity new_speaker = conversation.minions[index];
					flag = this.DoTalking(conversation, new_speaker);
				}
				else if (conversation.numUtterances >= TuningData<ConversationManager.Tuning>.Get().maxUtterances)
				{
					flag = false;
				}
				if (!flag)
				{
					this.activeSetups.RemoveAt(i);
				}
			}
		}
		foreach (MinionIdentity minionIdentity2 in Components.LiveMinionIdentities.Items)
		{
			if (this.ValidMinionTags(minionIdentity2) && !this.setupsByMinion.ContainsKey(minionIdentity2) && !this.MinionOnCooldown(minionIdentity2))
			{
				foreach (MinionIdentity minionIdentity3 in Components.LiveMinionIdentities.Items)
				{
					if (!(minionIdentity3 == minionIdentity2) && this.ValidMinionTags(minionIdentity3))
					{
						if (this.setupsByMinion.ContainsKey(minionIdentity3))
						{
							Conversation conversation2 = this.setupsByMinion[minionIdentity3];
							if (conversation2.minions.Count < TuningData<ConversationManager.Tuning>.Get().maxDupesPerConvo && (this.GetCentroid(conversation2) - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f)
							{
								conversation2.minions.Add(minionIdentity2);
								this.setupsByMinion[minionIdentity2] = conversation2;
								break;
							}
						}
						else if (!this.MinionOnCooldown(minionIdentity3) && (minionIdentity3.transform.GetPosition() - minionIdentity2.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance)
						{
							Conversation conversation3 = new Conversation();
							conversation3.minions.Add(minionIdentity2);
							conversation3.minions.Add(minionIdentity3);
							Type type = this.convoTypes[UnityEngine.Random.Range(0, this.convoTypes.Count)];
							conversation3.conversationType = (ConversationType)Activator.CreateInstance(type);
							conversation3.lastTalkedTime = GameClock.Instance.GetTime();
							this.activeSetups.Add(conversation3);
							this.setupsByMinion[minionIdentity2] = conversation3;
							this.setupsByMinion[minionIdentity3] = conversation3;
							break;
						}
					}
				}
			}
		}
		this.setupsByMinion.Clear();
	}

	// Token: 0x06005A54 RID: 23124 RVA: 0x002A2708 File Offset: 0x002A0908
	private bool DoTalking(Conversation setup, MinionIdentity new_speaker)
	{
		DebugUtil.Assert(setup != null, "setup was null");
		DebugUtil.Assert(new_speaker != null, "new_speaker was null");
		if (setup.lastTalked != null)
		{
			setup.lastTalked.Trigger(25860745, setup.lastTalked.gameObject);
		}
		DebugUtil.Assert(setup.conversationType != null, "setup.conversationType was null");
		Conversation.Topic nextTopic = setup.conversationType.GetNextTopic(new_speaker, setup.lastTopic);
		if (nextTopic == null || nextTopic.mode == Conversation.ModeType.End || nextTopic.mode == Conversation.ModeType.Segue)
		{
			return false;
		}
		Thought thoughtForTopic = this.GetThoughtForTopic(setup, nextTopic);
		if (thoughtForTopic == null)
		{
			return false;
		}
		ThoughtGraph.Instance smi = new_speaker.GetSMI<ThoughtGraph.Instance>();
		if (smi == null)
		{
			return false;
		}
		smi.AddThought(thoughtForTopic);
		setup.lastTopic = nextTopic;
		setup.lastTalked = new_speaker;
		setup.lastTalkedTime = GameClock.Instance.GetTime();
		DebugUtil.Assert(this.lastConvoTimeByMinion != null, "lastConvoTimeByMinion was null");
		this.lastConvoTimeByMinion[setup.lastTalked] = GameClock.Instance.GetTime();
		Effects component = setup.lastTalked.GetComponent<Effects>();
		DebugUtil.Assert(component != null, "effects was null");
		component.Add("GoodConversation", true);
		Conversation.Mode mode = Conversation.Topic.Modes[(int)nextTopic.mode];
		DebugUtil.Assert(mode != null, "mode was null");
		ConversationManager.StartedTalkingEvent data = new ConversationManager.StartedTalkingEvent
		{
			talker = new_speaker.gameObject,
			anim = mode.anim
		};
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!minionIdentity)
			{
				DebugUtil.DevAssert(false, "minion in setup.minions was null", null);
			}
			else
			{
				minionIdentity.Trigger(-594200555, data);
			}
		}
		setup.numUtterances++;
		return true;
	}

	// Token: 0x06005A55 RID: 23125 RVA: 0x000DF362 File Offset: 0x000DD562
	public bool TryGetConversation(MinionIdentity minion, out Conversation conversation)
	{
		return this.setupsByMinion.TryGetValue(minion, out conversation);
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x002A28E4 File Offset: 0x002A0AE4
	private Vector3 GetCentroid(Conversation setup)
	{
		Vector3 a = Vector3.zero;
		foreach (MinionIdentity minionIdentity in setup.minions)
		{
			if (!(minionIdentity == null))
			{
				a += minionIdentity.transform.GetPosition();
			}
		}
		return a / (float)setup.minions.Count;
	}

	// Token: 0x06005A57 RID: 23127 RVA: 0x002A2964 File Offset: 0x002A0B64
	private Thought GetThoughtForTopic(Conversation setup, Conversation.Topic topic)
	{
		if (string.IsNullOrEmpty(topic.topic))
		{
			DebugUtil.DevAssert(false, "topic.topic was null", null);
			return null;
		}
		Sprite sprite = setup.conversationType.GetSprite(topic.topic);
		if (sprite != null)
		{
			Conversation.Mode mode = Conversation.Topic.Modes[(int)topic.mode];
			return new Thought("Topic_" + topic.topic, null, sprite, mode.icon, mode.voice, "bubble_chatter", mode.mouth, DUPLICANTS.THOUGHTS.CONVERSATION.TOOLTIP, true, TuningData<ConversationManager.Tuning>.Get().speakTime);
		}
		return null;
	}

	// Token: 0x06005A58 RID: 23128 RVA: 0x000DF371 File Offset: 0x000DD571
	private bool ValidMinionTags(MinionIdentity minion)
	{
		return !(minion == null) && !minion.GetComponent<KPrefabID>().HasAnyTags(ConversationManager.invalidConvoTags);
	}

	// Token: 0x06005A59 RID: 23129 RVA: 0x002A29F8 File Offset: 0x002A0BF8
	private bool MinionCloseEnoughToConvo(MinionIdentity minion, Conversation setup)
	{
		return (this.GetCentroid(setup) - minion.transform.GetPosition()).magnitude < TuningData<ConversationManager.Tuning>.Get().maxDistance * 0.5f;
	}

	// Token: 0x06005A5A RID: 23130 RVA: 0x002A2A38 File Offset: 0x002A0C38
	private bool MinionOnCooldown(MinionIdentity minion)
	{
		return !minion.GetComponent<KPrefabID>().HasTag(GameTags.AlwaysConverse) && ((this.lastConvoTimeByMinion.ContainsKey(minion) && GameClock.Instance.GetTime() < this.lastConvoTimeByMinion[minion] + TuningData<ConversationManager.Tuning>.Get().minionCooldownTime) || GameClock.Instance.GetTime() / 600f < TuningData<ConversationManager.Tuning>.Get().cyclesBeforeFirstConversation);
	}

	// Token: 0x0400404D RID: 16461
	private List<Conversation> activeSetups;

	// Token: 0x0400404E RID: 16462
	private Dictionary<MinionIdentity, float> lastConvoTimeByMinion;

	// Token: 0x0400404F RID: 16463
	private Dictionary<MinionIdentity, Conversation> setupsByMinion = new Dictionary<MinionIdentity, Conversation>();

	// Token: 0x04004050 RID: 16464
	private List<Type> convoTypes = new List<Type>
	{
		typeof(RecentThingConversation),
		typeof(AmountStateConversation),
		typeof(CurrentJobConversation)
	};

	// Token: 0x04004051 RID: 16465
	private static readonly Tag[] invalidConvoTags = new Tag[]
	{
		GameTags.Asleep,
		GameTags.BionicBedTime,
		GameTags.HoldingBreath,
		GameTags.Dead
	};

	// Token: 0x02001146 RID: 4422
	public class Tuning : TuningData<ConversationManager.Tuning>
	{
		// Token: 0x04004052 RID: 16466
		public float cyclesBeforeFirstConversation;

		// Token: 0x04004053 RID: 16467
		public float maxDistance;

		// Token: 0x04004054 RID: 16468
		public int maxDupesPerConvo;

		// Token: 0x04004055 RID: 16469
		public float minionCooldownTime;

		// Token: 0x04004056 RID: 16470
		public float speakTime;

		// Token: 0x04004057 RID: 16471
		public float delayBetweenUtterances;

		// Token: 0x04004058 RID: 16472
		public float delayBeforeStart;

		// Token: 0x04004059 RID: 16473
		public int maxUtterances;
	}

	// Token: 0x02001147 RID: 4423
	public class StartedTalkingEvent
	{
		// Token: 0x0400405A RID: 16474
		public GameObject talker;

		// Token: 0x0400405B RID: 16475
		public string anim;
	}
}
