using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02001638 RID: 5688
public class SpeechMonitor : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>
{
	// Token: 0x060075B0 RID: 30128 RVA: 0x00316054 File Offset: 0x00314254
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.CreateMouth)).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.DestroyMouth));
		this.satisfied.DoNothing();
		this.talking.Enter(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.BeginTalking)).Update(new Action<SpeechMonitor.Instance, float>(SpeechMonitor.UpdateTalking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.mouth).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State.Callback(SpeechMonitor.EndTalking));
	}

	// Token: 0x060075B1 RID: 30129 RVA: 0x003160F0 File Offset: 0x003142F0
	private static void CreateMouth(SpeechMonitor.Instance smi)
	{
		smi.mouth = global::Util.KInstantiate(Assets.GetPrefab(MouthAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.mouth.gameObject.SetActive(true);
		smi.sm.mouth.Set(smi.mouth.gameObject, smi, false);
		smi.SetMouthId();
	}

	// Token: 0x060075B2 RID: 30130 RVA: 0x000F1D33 File Offset: 0x000EFF33
	private static void DestroyMouth(SpeechMonitor.Instance smi)
	{
		if (smi.mouth != null)
		{
			global::Util.KDestroyGameObject(smi.mouth);
			smi.mouth = null;
		}
	}

	// Token: 0x060075B3 RID: 30131 RVA: 0x00316154 File Offset: 0x00314354
	private static string GetRandomSpeechAnim(SpeechMonitor.Instance smi)
	{
		return smi.speechPrefix + UnityEngine.Random.Range(1, TuningData<SpeechMonitor.Tuning>.Get().speechCount).ToString() + smi.mouthId;
	}

	// Token: 0x060075B4 RID: 30132 RVA: 0x0031618C File Offset: 0x0031438C
	public static bool IsAllowedToPlaySpeech(GameObject go)
	{
		KPrefabID component = go.GetComponent<KPrefabID>();
		if (component.HasTag(GameTags.Dead) || component.HasTag(GameTags.Incapacitated))
		{
			return false;
		}
		KBatchedAnimController component2 = go.GetComponent<KBatchedAnimController>();
		KAnim.Anim currentAnim = component2.GetCurrentAnim();
		return currentAnim == null || (GameAudioSheets.Get().IsAnimAllowedToPlaySpeech(currentAnim) && SpeechMonitor.CanOverrideHead(component2));
	}

	// Token: 0x060075B5 RID: 30133 RVA: 0x003161E4 File Offset: 0x003143E4
	private static bool CanOverrideHead(KBatchedAnimController kbac)
	{
		bool result = true;
		KAnim.Anim currentAnim = kbac.GetCurrentAnim();
		if (currentAnim == null)
		{
			result = false;
		}
		else if (currentAnim.animFile.name != SpeechMonitor.GENERIC_CONVO_ANIM_NAME)
		{
			int currentFrameIndex = kbac.GetCurrentFrameIndex();
			KAnim.Anim.Frame frame;
			if (currentFrameIndex <= 0)
			{
				result = false;
			}
			else if (KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag).TryGetFrame(currentFrameIndex, out frame) && frame.hasHead)
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x060075B6 RID: 30134 RVA: 0x00316258 File Offset: 0x00314458
	public static void BeginTalking(SpeechMonitor.Instance smi)
	{
		smi.ev.clearHandle();
		if (smi.voiceEvent != null)
		{
			smi.ev = VoiceSoundEvent.PlayVoice(smi.voiceEvent, smi.GetComponent<KBatchedAnimController>(), 0f, false, false);
		}
		if (smi.ev.isValid())
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			smi.mouth.Play(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
			smi.mouth.Queue(SpeechMonitor.GetRandomSpeechAnim(smi), KAnim.PlayMode.Once, 1f, 0f);
		}
		SpeechMonitor.UpdateTalking(smi, 0f);
	}

	// Token: 0x060075B7 RID: 30135 RVA: 0x000F1D55 File Offset: 0x000EFF55
	public static void EndTalking(SpeechMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, 3);
	}

	// Token: 0x060075B8 RID: 30136 RVA: 0x0031637C File Offset: 0x0031457C
	public static KAnim.Anim.FrameElement GetFirstFrameElement(KBatchedAnimController controller)
	{
		KAnim.Anim.FrameElement result = default(KAnim.Anim.FrameElement);
		result.symbol = HashedString.Invalid;
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		KAnimBatch batch = controller.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return result;
		}
		KAnim.Anim.Frame frame;
		if (!controller.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return result;
		}
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					result = frameElement;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060075B9 RID: 30137 RVA: 0x00316440 File Offset: 0x00314640
	public static void UpdateTalking(SpeechMonitor.Instance smi, float dt)
	{
		if (smi.ev.isValid())
		{
			PLAYBACK_STATE playback_STATE;
			smi.ev.getPlaybackState(out playback_STATE);
			if (playback_STATE == PLAYBACK_STATE.STOPPING || playback_STATE == PLAYBACK_STATE.STOPPED)
			{
				smi.GoTo(smi.sm.satisfied);
				smi.ev.clearHandle();
				return;
			}
		}
		KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(smi.mouth);
		if (firstFrameElement.symbol == HashedString.Invalid)
		{
			return;
		}
		smi.Get<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
	}

	// Token: 0x04005870 RID: 22640
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State satisfied;

	// Token: 0x04005871 RID: 22641
	public GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.State talking;

	// Token: 0x04005872 RID: 22642
	public static string PREFIX_SAD = "sad";

	// Token: 0x04005873 RID: 22643
	public static string PREFIX_HAPPY = "happy";

	// Token: 0x04005874 RID: 22644
	public static string PREFIX_SINGER = "sing";

	// Token: 0x04005875 RID: 22645
	public StateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.TargetParameter mouth;

	// Token: 0x04005876 RID: 22646
	private static HashedString HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04005877 RID: 22647
	private static HashedString GENERIC_CONVO_ANIM_NAME = new HashedString("anim_generic_convo_kanim");

	// Token: 0x02001639 RID: 5689
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200163A RID: 5690
	public class Tuning : TuningData<SpeechMonitor.Tuning>
	{
		// Token: 0x04005878 RID: 22648
		public float randomSpeechIntervalMin;

		// Token: 0x04005879 RID: 22649
		public float randomSpeechIntervalMax;

		// Token: 0x0400587A RID: 22650
		public int speechCount;
	}

	// Token: 0x0200163B RID: 5691
	public new class Instance : GameStateMachine<SpeechMonitor, SpeechMonitor.Instance, IStateMachineTarget, SpeechMonitor.Def>.GameInstance
	{
		// Token: 0x060075BE RID: 30142 RVA: 0x000F1DB7 File Offset: 0x000EFFB7
		public Instance(IStateMachineTarget master, SpeechMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x000F1DCC File Offset: 0x000EFFCC
		public bool IsPlayingSpeech()
		{
			return base.IsInsideState(base.sm.talking);
		}

		// Token: 0x060075C0 RID: 30144 RVA: 0x000F1DDF File Offset: 0x000EFFDF
		public void PlaySpeech(string speech_prefix, string voice_event)
		{
			this.speechPrefix = speech_prefix;
			this.voiceEvent = voice_event;
			this.GoTo(base.sm.talking);
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x003164E0 File Offset: 0x003146E0
		public void DrawMouth()
		{
			KAnim.Anim.FrameElement firstFrameElement = SpeechMonitor.GetFirstFrameElement(base.smi.mouth);
			if (firstFrameElement.symbol == HashedString.Invalid)
			{
				return;
			}
			KAnim.Build.Symbol symbol = base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			base.GetComponent<SymbolOverrideController>().AddSymbolOverride(SpeechMonitor.HASH_SNAPTO_MOUTH, base.smi.mouth.AnimFiles[0].GetData().build.GetSymbol(firstFrameElement.symbol), 3);
			KAnim.Build.Symbol symbol2 = KAnimBatchManager.Instance().GetBatchGroupData(component.batchGroupID).GetSymbol(SpeechMonitor.HASH_SNAPTO_MOUTH);
			KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(symbol.build.batchTag).symbolFrameInstances[symbol.firstFrameIdx + firstFrameElement.frame];
			symbolFrameInstance.buildImageIdx = base.GetComponent<SymbolOverrideController>().GetAtlasIdx(symbol.build.GetTexture(0));
			component.SetSymbolOverride(symbol2.firstFrameIdx, ref symbolFrameInstance);
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x003165F4 File Offset: 0x003147F4
		public void SetMouthId()
		{
			if (base.smi.Get<Accessorizer>().GetAccessory(Db.Get().AccessorySlots.Mouth).Id.Contains("006"))
			{
				base.smi.mouthId = "_006";
			}
		}

		// Token: 0x0400587B RID: 22651
		public KBatchedAnimController mouth;

		// Token: 0x0400587C RID: 22652
		public string speechPrefix = "happy";

		// Token: 0x0400587D RID: 22653
		public string voiceEvent;

		// Token: 0x0400587E RID: 22654
		public EventInstance ev;

		// Token: 0x0400587F RID: 22655
		public string mouthId;
	}
}
