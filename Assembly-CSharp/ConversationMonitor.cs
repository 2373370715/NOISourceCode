using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001585 RID: 5509
public class ConversationMonitor : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>
{
	// Token: 0x060072BC RID: 29372 RVA: 0x0030D7A0 File Offset: 0x0030B9A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.TopicDiscussed, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscussed(obj);
		}).EventHandler(GameHashes.TopicDiscovered, delegate(ConversationMonitor.Instance smi, object obj)
		{
			smi.OnTopicDiscovered(obj);
		});
	}

	// Token: 0x04005607 RID: 22023
	private const int MAX_RECENT_TOPICS = 5;

	// Token: 0x04005608 RID: 22024
	private const int MAX_FAVOURITE_TOPICS = 5;

	// Token: 0x04005609 RID: 22025
	private const float FAVOURITE_CHANCE = 0.033333335f;

	// Token: 0x0400560A RID: 22026
	private const float LEARN_CHANCE = 0.33333334f;

	// Token: 0x02001586 RID: 5510
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001587 RID: 5511
	[SerializationConfig(MemberSerialization.OptIn)]
	public new class Instance : GameStateMachine<ConversationMonitor, ConversationMonitor.Instance, IStateMachineTarget, ConversationMonitor.Def>.GameInstance
	{
		// Token: 0x060072BF RID: 29375 RVA: 0x0030D810 File Offset: 0x0030BA10
		public Instance(IStateMachineTarget master, ConversationMonitor.Def def) : base(master, def)
		{
			this.recentTopics = new Queue<string>();
			this.favouriteTopics = new List<string>
			{
				ConversationMonitor.Instance.randomTopics[UnityEngine.Random.Range(0, ConversationMonitor.Instance.randomTopics.Count)]
			};
			this.personalTopics = new List<string>();
		}

		// Token: 0x060072C0 RID: 29376 RVA: 0x0030D868 File Offset: 0x0030BA68
		public string GetATopic()
		{
			int maxExclusive = this.recentTopics.Count + this.favouriteTopics.Count * 2 + this.personalTopics.Count;
			int num = UnityEngine.Random.Range(0, maxExclusive);
			if (num < this.recentTopics.Count)
			{
				return this.recentTopics.Dequeue();
			}
			num -= this.recentTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.favouriteTopics.Count)
			{
				return this.favouriteTopics[num];
			}
			num -= this.favouriteTopics.Count;
			if (num < this.personalTopics.Count)
			{
				return this.personalTopics[num];
			}
			return "";
		}

		// Token: 0x060072C1 RID: 29377 RVA: 0x0030D940 File Offset: 0x0030BB40
		public void OnTopicDiscovered(object data)
		{
			string item = (string)data;
			if (!this.recentTopics.Contains(item))
			{
				this.recentTopics.Enqueue(item);
				if (this.recentTopics.Count > 5)
				{
					string topic = this.recentTopics.Dequeue();
					this.TryMakeFavouriteTopic(topic);
				}
			}
		}

		// Token: 0x060072C2 RID: 29378 RVA: 0x0030D990 File Offset: 0x0030BB90
		public void OnTopicDiscussed(object data)
		{
			string data2 = (string)data;
			if (UnityEngine.Random.value < 0.33333334f)
			{
				this.OnTopicDiscovered(data2);
			}
		}

		// Token: 0x060072C3 RID: 29379 RVA: 0x0030D9B8 File Offset: 0x0030BBB8
		private void TryMakeFavouriteTopic(string topic)
		{
			if (UnityEngine.Random.value < 0.033333335f)
			{
				if (this.favouriteTopics.Count < 5)
				{
					this.favouriteTopics.Add(topic);
					return;
				}
				this.favouriteTopics[UnityEngine.Random.Range(0, this.favouriteTopics.Count)] = topic;
			}
		}

		// Token: 0x0400560B RID: 22027
		[Serialize]
		private Queue<string> recentTopics;

		// Token: 0x0400560C RID: 22028
		[Serialize]
		private List<string> favouriteTopics;

		// Token: 0x0400560D RID: 22029
		private List<string> personalTopics;

		// Token: 0x0400560E RID: 22030
		private static readonly List<string> randomTopics = new List<string>
		{
			"Headquarters"
		};
	}
}
