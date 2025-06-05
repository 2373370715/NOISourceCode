using System;
using System.Collections.Generic;

// Token: 0x02001C16 RID: 7190
public class UIFloatFormatter
{
	// Token: 0x06009595 RID: 38293 RVA: 0x00105F12 File Offset: 0x00104112
	public string Format(string format, float value)
	{
		return this.Replace(format, "{0}", value);
	}

	// Token: 0x06009596 RID: 38294 RVA: 0x003A6820 File Offset: 0x003A4A20
	private string Replace(string format, string key, float value)
	{
		UIFloatFormatter.Entry entry = default(UIFloatFormatter.Entry);
		if (this.activeStringCount >= this.entries.Count)
		{
			entry.format = format;
			entry.key = key;
			entry.value = value;
			entry.result = entry.format.Replace(key, value.ToString());
			this.entries.Add(entry);
		}
		else
		{
			entry = this.entries[this.activeStringCount];
			if (entry.format != format || entry.key != key || entry.value != value)
			{
				entry.format = format;
				entry.key = key;
				entry.value = value;
				entry.result = entry.format.Replace(key, value.ToString());
				this.entries[this.activeStringCount] = entry;
			}
		}
		this.activeStringCount++;
		return entry.result;
	}

	// Token: 0x06009597 RID: 38295 RVA: 0x00105F21 File Offset: 0x00104121
	public void BeginDrawing()
	{
		this.activeStringCount = 0;
	}

	// Token: 0x06009598 RID: 38296 RVA: 0x000AA038 File Offset: 0x000A8238
	public void EndDrawing()
	{
	}

	// Token: 0x04007460 RID: 29792
	private int activeStringCount;

	// Token: 0x04007461 RID: 29793
	private List<UIFloatFormatter.Entry> entries = new List<UIFloatFormatter.Entry>();

	// Token: 0x02001C17 RID: 7191
	private struct Entry
	{
		// Token: 0x04007462 RID: 29794
		public string format;

		// Token: 0x04007463 RID: 29795
		public string key;

		// Token: 0x04007464 RID: 29796
		public float value;

		// Token: 0x04007465 RID: 29797
		public string result;
	}
}
