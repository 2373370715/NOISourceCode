using System;
using System.Collections.Generic;

// Token: 0x02001C18 RID: 7192
public class UIStringFormatter
{
	// Token: 0x04007466 RID: 29798
	private List<UIStringFormatter.Entry> entries = new List<UIStringFormatter.Entry>();

	// Token: 0x02001C19 RID: 7193
	private struct Entry
	{
		// Token: 0x04007467 RID: 29799
		public string format;

		// Token: 0x04007468 RID: 29800
		public string key;

		// Token: 0x04007469 RID: 29801
		public string value;

		// Token: 0x0400746A RID: 29802
		public string result;
	}
}
