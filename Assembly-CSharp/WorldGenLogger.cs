using System;

// Token: 0x020020E6 RID: 8422
public static class WorldGenLogger
{
	// Token: 0x0600B379 RID: 45945 RVA: 0x00119320 File Offset: 0x00117520
	public static void LogException(string message, string stack)
	{
		Debug.LogError(message + "\n" + stack);
	}
}
