using System;
using UnityEngine;

// Token: 0x02001341 RID: 4929
public class LogCatcher : ILogHandler
{
	// Token: 0x060064EF RID: 25839 RVA: 0x000E6508 File Offset: 0x000E4708
	public LogCatcher(ILogHandler old)
	{
		this.def = old;
	}

	// Token: 0x060064F0 RID: 25840 RVA: 0x002CF474 File Offset: 0x002CD674
	void ILogHandler.LogException(Exception exception, UnityEngine.Object context)
	{
		string a = exception.ToString();
		string a2 = (context != null) ? context.ToString() : null;
		if (a == "False" || a2 == "False")
		{
			global::Debug.LogError("False only message!");
		}
		this.def.LogException(exception, context);
	}

	// Token: 0x060064F1 RID: 25841 RVA: 0x000E6517 File Offset: 0x000E4717
	void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		if (string.Format(format, args) == "False")
		{
			global::Debug.LogError("False only message!");
		}
		this.def.LogFormat(logType, context, format, args);
	}

	// Token: 0x040048AA RID: 18602
	private ILogHandler def;
}
