using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200063C RID: 1596
public readonly struct Updater : IEnumerator
{
	// Token: 0x06001C55 RID: 7253 RVA: 0x000B7102 File Offset: 0x000B5302
	public Updater(Func<float, UpdaterResult> fn)
	{
		this.fn = fn;
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x000B710B File Offset: 0x000B530B
	public UpdaterResult Internal_Update(float deltaTime)
	{
		return this.fn(deltaTime);
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x06001C57 RID: 7255 RVA: 0x000AA765 File Offset: 0x000A8965
	object IEnumerator.Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x000B7119 File Offset: 0x000B5319
	bool IEnumerator.MoveNext()
	{
		return this.fn(Updater.GetDeltaTime()) == UpdaterResult.NotComplete;
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x000AA038 File Offset: 0x000A8238
	void IEnumerator.Reset()
	{
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x000B712E File Offset: 0x000B532E
	public static implicit operator Updater(Promise promise)
	{
		return Updater.Until(() => promise.IsResolved);
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x000B714C File Offset: 0x000B534C
	public static Updater Until(Func<bool> fn)
	{
		return new Updater(delegate(float dt)
		{
			if (!fn())
			{
				return UpdaterResult.NotComplete;
			}
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x000B716A File Offset: 0x000B536A
	public static Updater While(Func<bool> isTrueFn)
	{
		return new Updater(delegate(float dt)
		{
			if (!isTrueFn())
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x000B7188 File Offset: 0x000B5388
	public static Updater While(Func<bool> isTrueFn, Func<Updater> getUpdaterWhileNotTrueFn)
	{
		Updater whileNotTrueUpdater = Updater.None();
		return new Updater(delegate(float dt)
		{
			if (whileNotTrueUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				if (!isTrueFn())
				{
					return UpdaterResult.Complete;
				}
				whileNotTrueUpdater = getUpdaterWhileNotTrueFn();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x000B71B8 File Offset: 0x000B53B8
	public static Updater None()
	{
		return new Updater((float dt) => UpdaterResult.Complete);
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x000B71DE File Offset: 0x000B53DE
	public static Updater WaitOneFrame()
	{
		return Updater.WaitFrames(1);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x000B71E6 File Offset: 0x000B53E6
	public static Updater WaitFrames(int framesToWait)
	{
		int frame = 0;
		return new Updater(delegate(float dt)
		{
			int frame = frame;
			frame++;
			if (framesToWait <= frame)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x000B720B File Offset: 0x000B540B
	public static Updater WaitForSeconds(float secondsToWait)
	{
		float currentSeconds = 0f;
		return new Updater(delegate(float dt)
		{
			currentSeconds += dt;
			if (secondsToWait <= currentSeconds)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x000B7234 File Offset: 0x000B5434
	public static Updater Ease(Action<float> fn, float from, float to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<float>(fn, new Func<float, float, float, float>(Mathf.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x000B724F File Offset: 0x000B544F
	public static Updater Ease(Action<Vector2> fn, Vector2 from, Vector2 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector2>(fn, new Func<Vector2, Vector2, float, Vector2>(Vector2.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x000B726A File Offset: 0x000B546A
	public static Updater Ease(Action<Vector3> fn, Vector3 from, Vector3 to, float duration, Easing.EasingFn easing = null, float delay = -1f)
	{
		return Updater.GenericEase<Vector3>(fn, new Func<Vector3, Vector3, float, Vector3>(Vector3.LerpUnclamped), easing, from, to, duration, delay);
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x001B889C File Offset: 0x001B6A9C
	public static Updater GenericEase<T>(Action<T> useFn, Func<T, T, float, T> interpolateFn, Easing.EasingFn easingFn, T from, T to, float duration, float delay)
	{
		Updater.<>c__DisplayClass18_0<T> CS$<>8__locals1 = new Updater.<>c__DisplayClass18_0<T>();
		CS$<>8__locals1.useFn = useFn;
		CS$<>8__locals1.interpolateFn = interpolateFn;
		CS$<>8__locals1.from = from;
		CS$<>8__locals1.to = to;
		CS$<>8__locals1.easingFn = easingFn;
		CS$<>8__locals1.duration = duration;
		if (CS$<>8__locals1.easingFn == null)
		{
			CS$<>8__locals1.easingFn = Easing.SmoothStep;
		}
		CS$<>8__locals1.currentSeconds = 0f;
		CS$<>8__locals1.<GenericEase>g__UseKeyframeAt|0(0f);
		Updater updater = new Updater(delegate(float dt)
		{
			CS$<>8__locals1.currentSeconds += dt;
			if (CS$<>8__locals1.currentSeconds < CS$<>8__locals1.duration)
			{
				base.<GenericEase>g__UseKeyframeAt|0(CS$<>8__locals1.currentSeconds / CS$<>8__locals1.duration);
				return UpdaterResult.NotComplete;
			}
			base.<GenericEase>g__UseKeyframeAt|0(1f);
			return UpdaterResult.Complete;
		});
		if (delay > 0f)
		{
			return Updater.Series(new Updater[]
			{
				Updater.WaitForSeconds(delay),
				updater
			});
		}
		return updater;
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x000B7285 File Offset: 0x000B5485
	public static Updater Do(System.Action fn)
	{
		return new Updater(delegate(float dt)
		{
			fn();
			return UpdaterResult.Complete;
		});
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x000B72A3 File Offset: 0x000B54A3
	public static Updater Do(Func<Updater> fn)
	{
		bool didInitalize = false;
		Updater target = default(Updater);
		return new Updater(delegate(float dt)
		{
			if (!didInitalize)
			{
				target = fn();
				didInitalize = true;
			}
			return target.Internal_Update(dt);
		});
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x000B72D4 File Offset: 0x000B54D4
	public static Updater Loop(params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(Option.None, makeUpdaterFns);
	}

	// Token: 0x06001C69 RID: 7273 RVA: 0x000B72E6 File Offset: 0x000B54E6
	public static Updater Loop(int loopCount, params Func<Updater>[] makeUpdaterFns)
	{
		return Updater.Internal_Loop(loopCount, makeUpdaterFns);
	}

	// Token: 0x06001C6A RID: 7274 RVA: 0x001B8944 File Offset: 0x001B6B44
	public static Updater Internal_Loop(Option<int> limitLoopCount, Func<Updater>[] makeUpdaterFns)
	{
		if (makeUpdaterFns == null || makeUpdaterFns.Length == 0)
		{
			return Updater.None();
		}
		int completedLoopCount = 0;
		int currentIndex = 0;
		Updater currentUpdater = makeUpdaterFns[currentIndex]();
		return new Updater(delegate(float dt)
		{
			if (currentUpdater.Internal_Update(dt) == UpdaterResult.Complete)
			{
				int num = currentIndex;
				currentIndex = num + 1;
				if (currentIndex >= makeUpdaterFns.Length)
				{
					currentIndex -= makeUpdaterFns.Length;
					num = completedLoopCount;
					completedLoopCount = num + 1;
					if (limitLoopCount.IsSome() && completedLoopCount >= limitLoopCount.Unwrap())
					{
						return UpdaterResult.Complete;
					}
				}
				currentUpdater = makeUpdaterFns[currentIndex]();
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C6B RID: 7275 RVA: 0x000B72F4 File Offset: 0x000B54F4
	public static Updater Parallel(params Updater[] updaters)
	{
		bool[] isCompleted = new bool[updaters.Length];
		return new Updater(delegate(float dt)
		{
			bool flag = false;
			for (int i = 0; i < updaters.Length; i++)
			{
				if (!isCompleted[i])
				{
					if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
					{
						isCompleted[i] = true;
					}
					else
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C6C RID: 7276 RVA: 0x000B7325 File Offset: 0x000B5525
	public static Updater Series(params Updater[] updaters)
	{
		int i = 0;
		return new Updater(delegate(float dt)
		{
			int i;
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			if (updaters[i].Internal_Update(dt) == UpdaterResult.Complete)
			{
				i = i;
				i++;
			}
			if (i == updaters.Length)
			{
				return UpdaterResult.Complete;
			}
			return UpdaterResult.NotComplete;
		});
	}

	// Token: 0x06001C6D RID: 7277 RVA: 0x001B89B4 File Offset: 0x001B6BB4
	public static Promise RunRoutine(MonoBehaviour monoBehaviour, IEnumerator coroutine)
	{
		Updater.<>c__DisplayClass26_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass26_0();
		CS$<>8__locals1.coroutine = coroutine;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<RunRoutine>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x000B734A File Offset: 0x000B554A
	public static Promise Run(MonoBehaviour monoBehaviour, params Updater[] updaters)
	{
		return Updater.Run(monoBehaviour, Updater.Series(updaters));
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x001B89EC File Offset: 0x001B6BEC
	public static Promise Run(MonoBehaviour monoBehaviour, Updater updater)
	{
		Updater.<>c__DisplayClass28_0 CS$<>8__locals1 = new Updater.<>c__DisplayClass28_0();
		CS$<>8__locals1.updater = updater;
		CS$<>8__locals1.willComplete = new Promise();
		monoBehaviour.StartCoroutine(CS$<>8__locals1.<Run>g__Routine|0());
		return CS$<>8__locals1.willComplete;
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x000B7358 File Offset: 0x000B5558
	public static float GetDeltaTime()
	{
		return Time.unscaledDeltaTime;
	}

	// Token: 0x040011FC RID: 4604
	public readonly Func<float, UpdaterResult> fn;
}
