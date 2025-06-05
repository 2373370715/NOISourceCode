using System;
using UnityEngine;

// Token: 0x02001570 RID: 5488
public class BlinkMonitor : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>
{
	// Token: 0x0600725F RID: 29279 RVA: 0x0030C9DC File Offset: 0x0030ABDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.CreateEyes)).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.DestroyEyes));
		this.satisfied.ScheduleGoTo(new Func<BlinkMonitor.Instance, float>(BlinkMonitor.GetRandomBlinkTime), this.blinking);
		this.blinking.EnterTransition(this.satisfied, GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Not(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.Transition.ConditionCallback(BlinkMonitor.CanBlink))).Enter(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.BeginBlinking)).Update(new Action<BlinkMonitor.Instance, float>(BlinkMonitor.UpdateBlinking), UpdateRate.RENDER_EVERY_TICK, false).Target(this.eyes).OnAnimQueueComplete(this.satisfied).Exit(new StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State.Callback(BlinkMonitor.EndBlinking));
	}

	// Token: 0x06007260 RID: 29280 RVA: 0x000EF60D File Offset: 0x000ED80D
	private static bool CanBlink(BlinkMonitor.Instance smi)
	{
		return SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject) && smi.Get<Navigator>().CurrentNavType != NavType.Ladder;
	}

	// Token: 0x06007261 RID: 29281 RVA: 0x000EF62F File Offset: 0x000ED82F
	private static float GetRandomBlinkTime(BlinkMonitor.Instance smi)
	{
		return UnityEngine.Random.Range(TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMin, TuningData<BlinkMonitor.Tuning>.Get().randomBlinkIntervalMax);
	}

	// Token: 0x06007262 RID: 29282 RVA: 0x0030CAA8 File Offset: 0x0030ACA8
	private static void CreateEyes(BlinkMonitor.Instance smi)
	{
		smi.eyes = Util.KInstantiate(Assets.GetPrefab(EyeAnimation.ID), null, null).GetComponent<KBatchedAnimController>();
		smi.eyes.gameObject.SetActive(true);
		smi.sm.eyes.Set(smi.eyes.gameObject, smi, false);
	}

	// Token: 0x06007263 RID: 29283 RVA: 0x000EF64A File Offset: 0x000ED84A
	private static void DestroyEyes(BlinkMonitor.Instance smi)
	{
		if (smi.eyes != null)
		{
			Util.KDestroyGameObject(smi.eyes);
			smi.eyes = null;
		}
	}

	// Token: 0x06007264 RID: 29284 RVA: 0x000EF66C File Offset: 0x000ED86C
	public static void BeginBlinking(BlinkMonitor.Instance smi)
	{
		smi.eyes.Play(smi.eye_anim, KAnim.PlayMode.Once, 1f, 0f);
		BlinkMonitor.UpdateBlinking(smi, 0f);
	}

	// Token: 0x06007265 RID: 29285 RVA: 0x000EF69A File Offset: 0x000ED89A
	public static void EndBlinking(BlinkMonitor.Instance smi)
	{
		smi.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, 3);
	}

	// Token: 0x06007266 RID: 29286 RVA: 0x0030CB08 File Offset: 0x0030AD08
	public static void UpdateBlinking(BlinkMonitor.Instance smi, float dt)
	{
		int currentFrameIndex = smi.eyes.GetCurrentFrameIndex();
		KAnimBatch batch = smi.eyes.GetBatch();
		if (currentFrameIndex == -1 || batch == null)
		{
			return;
		}
		KAnim.Anim.Frame frame;
		if (!smi.eyes.GetBatch().group.data.TryGetFrame(currentFrameIndex, out frame))
		{
			return;
		}
		HashedString hash = HashedString.Invalid;
		for (int i = 0; i < frame.numElements; i++)
		{
			int num = frame.firstElementIdx + i;
			if (num < batch.group.data.frameElements.Count)
			{
				KAnim.Anim.FrameElement frameElement = batch.group.data.frameElements[num];
				if (!(frameElement.symbol == HashedString.Invalid))
				{
					hash = frameElement.symbol;
					break;
				}
			}
		}
		smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(BlinkMonitor.HASH_SNAPTO_EYES, smi.eyes.AnimFiles[0].GetData().build.GetSymbol(hash), 3);
	}

	// Token: 0x040055C9 RID: 21961
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State satisfied;

	// Token: 0x040055CA RID: 21962
	public GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.State blinking;

	// Token: 0x040055CB RID: 21963
	public StateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.TargetParameter eyes;

	// Token: 0x040055CC RID: 21964
	private static HashedString HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x02001571 RID: 5489
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001572 RID: 5490
	public class Tuning : TuningData<BlinkMonitor.Tuning>
	{
		// Token: 0x040055CD RID: 21965
		public float randomBlinkIntervalMin;

		// Token: 0x040055CE RID: 21966
		public float randomBlinkIntervalMax;
	}

	// Token: 0x02001573 RID: 5491
	public new class Instance : GameStateMachine<BlinkMonitor, BlinkMonitor.Instance, IStateMachineTarget, BlinkMonitor.Def>.GameInstance
	{
		// Token: 0x0600726B RID: 29291 RVA: 0x000EF6CF File Offset: 0x000ED8CF
		public Instance(IStateMachineTarget master, BlinkMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x000EF6D9 File Offset: 0x000ED8D9
		public bool IsBlinking()
		{
			return base.IsInsideState(base.sm.blinking);
		}

		// Token: 0x0600726D RID: 29293 RVA: 0x000EF6EC File Offset: 0x000ED8EC
		public void Blink()
		{
			this.GoTo(base.sm.blinking);
		}

		// Token: 0x040055CF RID: 21967
		public KBatchedAnimController eyes;

		// Token: 0x040055D0 RID: 21968
		public string eye_anim;
	}
}
