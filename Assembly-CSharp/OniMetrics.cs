using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001ABC RID: 6844
public class OniMetrics : MonoBehaviour
{
	// Token: 0x06008F29 RID: 36649 RVA: 0x00380DF4 File Offset: 0x0037EFF4
	private static void EnsureMetrics()
	{
		if (OniMetrics.Metrics != null)
		{
			return;
		}
		OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
		for (int i = 0; i < 2; i++)
		{
			OniMetrics.Metrics.Add(null);
		}
	}

	// Token: 0x06008F2A RID: 36650 RVA: 0x00101EB8 File Offset: 0x001000B8
	public static void LogEvent(OniMetrics.Event eventType, string key, object data)
	{
		OniMetrics.EnsureMetrics();
		if (OniMetrics.Metrics[(int)eventType] == null)
		{
			OniMetrics.Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		OniMetrics.Metrics[(int)eventType][key] = data;
	}

	// Token: 0x06008F2B RID: 36651 RVA: 0x00380E2C File Offset: 0x0037F02C
	public static void SendEvent(OniMetrics.Event eventType, string debugName)
	{
		if (OniMetrics.Metrics[(int)eventType] == null || OniMetrics.Metrics[(int)eventType].Count == 0)
		{
			return;
		}
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int)eventType], debugName);
		OniMetrics.Metrics[(int)eventType].Clear();
	}

	// Token: 0x04006BDE RID: 27614
	private static List<Dictionary<string, object>> Metrics;

	// Token: 0x02001ABD RID: 6845
	public enum Event : short
	{
		// Token: 0x04006BE0 RID: 27616
		NewSave,
		// Token: 0x04006BE1 RID: 27617
		EndOfCycle,
		// Token: 0x04006BE2 RID: 27618
		NumEvents
	}
}
