﻿using System;
using System.Diagnostics;

public class CellElementEvent : CellEvent
{
	public CellElementEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, SimHashes element, int callback_id)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, (int)element, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	public override string GetDescription(EventInstanceBase ev)
	{
		SimHashes data = (SimHashes)(ev as CellEventInstance).data;
		return string.Concat(new string[]
		{
			base.GetMessagePrefix(),
			"Element=",
			data.ToString(),
			" (",
			this.reason,
			")"
		});
	}
}
