using System;
using UnityEngine;

// Token: 0x02001889 RID: 6281
public class SceneInitializerLoader : MonoBehaviour
{
	// Token: 0x060081AA RID: 33194 RVA: 0x0034718C File Offset: 0x0034538C
	private void Awake()
	{
		Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
		KMonoBehaviour.isLoadingScene = false;
		Singleton<StateMachineManager>.Instance.Clear();
		Util.KInstantiate(this.sceneInitializer, null, null);
		if (SceneInitializerLoader.ReportDeferredError != null && SceneInitializerLoader.deferred_error.IsValid)
		{
			SceneInitializerLoader.ReportDeferredError(SceneInitializerLoader.deferred_error);
			SceneInitializerLoader.deferred_error = default(SceneInitializerLoader.DeferredError);
		}
	}

	// Token: 0x0400629F RID: 25247
	public SceneInitializer sceneInitializer;

	// Token: 0x040062A0 RID: 25248
	public static SceneInitializerLoader.DeferredError deferred_error;

	// Token: 0x040062A1 RID: 25249
	public static SceneInitializerLoader.DeferredErrorDelegate ReportDeferredError;

	// Token: 0x0200188A RID: 6282
	public struct DeferredError
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060081AC RID: 33196 RVA: 0x000F9D0E File Offset: 0x000F7F0E
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.msg);
			}
		}

		// Token: 0x040062A2 RID: 25250
		public string msg;

		// Token: 0x040062A3 RID: 25251
		public string stack_trace;
	}

	// Token: 0x0200188B RID: 6283
	// (Invoke) Token: 0x060081AE RID: 33198
	public delegate void DeferredErrorDelegate(SceneInitializerLoader.DeferredError deferred_error);
}
