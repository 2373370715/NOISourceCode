using System;
using System.Collections.Generic;

// Token: 0x02000B85 RID: 2949
public class WorkingToiletTracker : WorldTracker
{
	// Token: 0x06003753 RID: 14163 RVA: 0x000C84F8 File Offset: 0x000C66F8
	public WorkingToiletTracker(int worldID) : base(worldID)
	{
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x00223BD4 File Offset: 0x00221DD4
	public override void UpdateData()
	{
		int num = 0;
		using (IEnumerator<IUsable> enumerator = Components.Toilets.WorldItemsEnumerate(base.WorldID, true).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsUsable())
				{
					num++;
				}
			}
		}
		base.AddPoint((float)num);
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x000C6C93 File Offset: 0x000C4E93
	public override string FormatValueString(float value)
	{
		return value.ToString();
	}
}
