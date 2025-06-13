using System;
using UnityEngine;

public class LogCatcher : ILogHandler
{
	public LogCatcher(ILogHandler old)
	{
		this.def = old;
	}

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

	void ILogHandler.LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		if (string.Format(format, args) == "False")
		{
			global::Debug.LogError("False only message!");
		}
		this.def.LogFormat(logType, context, format, args);
	}

	private ILogHandler def;
}
