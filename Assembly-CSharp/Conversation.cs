using System;
using System.Collections.Generic;

// Token: 0x02001149 RID: 4425
public class Conversation
{
	// Token: 0x0400405E RID: 16478
	public List<MinionIdentity> minions = new List<MinionIdentity>();

	// Token: 0x0400405F RID: 16479
	public MinionIdentity lastTalked;

	// Token: 0x04004060 RID: 16480
	public ConversationType conversationType;

	// Token: 0x04004061 RID: 16481
	public float lastTalkedTime;

	// Token: 0x04004062 RID: 16482
	public Conversation.Topic lastTopic;

	// Token: 0x04004063 RID: 16483
	public int numUtterances;

	// Token: 0x0200114A RID: 4426
	public enum ModeType
	{
		// Token: 0x04004065 RID: 16485
		Query,
		// Token: 0x04004066 RID: 16486
		Statement,
		// Token: 0x04004067 RID: 16487
		Agreement,
		// Token: 0x04004068 RID: 16488
		Disagreement,
		// Token: 0x04004069 RID: 16489
		Musing,
		// Token: 0x0400406A RID: 16490
		Satisfaction,
		// Token: 0x0400406B RID: 16491
		Nominal,
		// Token: 0x0400406C RID: 16492
		Dissatisfaction,
		// Token: 0x0400406D RID: 16493
		Stressing,
		// Token: 0x0400406E RID: 16494
		Segue,
		// Token: 0x0400406F RID: 16495
		End
	}

	// Token: 0x0200114B RID: 4427
	public class Mode
	{
		// Token: 0x06005A63 RID: 23139 RVA: 0x000DF405 File Offset: 0x000DD605
		public Mode(Conversation.ModeType type, string voice, string icon, string mouth, string anim, bool newTopic = false)
		{
			this.type = type;
			this.voice = voice;
			this.mouth = mouth;
			this.anim = anim;
			this.icon = icon;
			this.newTopic = newTopic;
		}

		// Token: 0x04004070 RID: 16496
		public Conversation.ModeType type;

		// Token: 0x04004071 RID: 16497
		public string voice;

		// Token: 0x04004072 RID: 16498
		public string mouth;

		// Token: 0x04004073 RID: 16499
		public string anim;

		// Token: 0x04004074 RID: 16500
		public string icon;

		// Token: 0x04004075 RID: 16501
		public bool newTopic;
	}

	// Token: 0x0200114C RID: 4428
	public class Topic
	{
		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x002A2B04 File Offset: 0x002A0D04
		public static Dictionary<int, Conversation.Mode> Modes
		{
			get
			{
				if (Conversation.Topic._modes == null)
				{
					Conversation.Topic._modes = new Dictionary<int, Conversation.Mode>();
					foreach (Conversation.Mode mode in Conversation.Topic.modeList)
					{
						Conversation.Topic._modes[(int)mode.type] = mode;
					}
				}
				return Conversation.Topic._modes;
			}
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x000DF43A File Offset: 0x000DD63A
		public Topic(string topic, Conversation.ModeType mode)
		{
			this.topic = topic;
			this.mode = mode;
		}

		// Token: 0x04004076 RID: 16502
		public static List<Conversation.Mode> modeList = new List<Conversation.Mode>
		{
			new Conversation.Mode(Conversation.ModeType.Query, "conversation_question", "mode_query", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Statement, "conversation_answer", "mode_statement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Agreement, "conversation_answer", "mode_agreement", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Disagreement, "conversation_answer", "mode_disagreement", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Musing, "conversation_short", "mode_musing", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Satisfaction, "conversation_short", "mode_satisfaction", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Nominal, "conversation_short", "mode_nominal", SpeechMonitor.PREFIX_HAPPY, "happy", false),
			new Conversation.Mode(Conversation.ModeType.Dissatisfaction, "conversation_short", "mode_dissatisfaction", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Stressing, "conversation_short", "mode_stressing", SpeechMonitor.PREFIX_SAD, "unhappy", false),
			new Conversation.Mode(Conversation.ModeType.Segue, "conversation_question", "mode_segue", SpeechMonitor.PREFIX_HAPPY, "happy", true)
		};

		// Token: 0x04004077 RID: 16503
		private static Dictionary<int, Conversation.Mode> _modes;

		// Token: 0x04004078 RID: 16504
		public string topic;

		// Token: 0x04004079 RID: 16505
		public Conversation.ModeType mode;
	}
}
