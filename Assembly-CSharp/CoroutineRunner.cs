using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class CoroutineRunner : MonoBehaviour
{
	// Token: 0x06001B61 RID: 7009 RVA: 0x000B6422 File Offset: 0x000B4622
	public Promise Run(IEnumerator routine)
	{
		return new Promise(delegate(System.Action resolve)
		{
			this.StartCoroutine(this.RunRoutine(routine, resolve));
		});
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x001B7390 File Offset: 0x001B5590
	public ValueTuple<Promise, System.Action> RunCancellable(IEnumerator routine)
	{
		Promise promise = new Promise();
		Coroutine coroutine = base.StartCoroutine(this.RunRoutine(routine, new System.Action(promise.Resolve)));
		System.Action item = delegate()
		{
			this.StopCoroutine(coroutine);
		};
		return new ValueTuple<Promise, System.Action>(promise, item);
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000B6447 File Offset: 0x000B4647
	private IEnumerator RunRoutine(IEnumerator routine, System.Action completedCallback)
	{
		yield return routine;
		completedCallback();
		yield break;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000B645D File Offset: 0x000B465D
	public static CoroutineRunner Create()
	{
		return new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x001B73E4 File Offset: 0x001B55E4
	public static Promise RunOne(IEnumerator routine)
	{
		CoroutineRunner runner = CoroutineRunner.Create();
		return runner.Run(routine).Then(delegate
		{
			UnityEngine.Object.Destroy(runner.gameObject);
		});
	}
}
