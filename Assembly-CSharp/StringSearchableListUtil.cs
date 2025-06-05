using System;
using System.Linq;

// Token: 0x02000637 RID: 1591
public static class StringSearchableListUtil
{
	// Token: 0x06001C48 RID: 7240 RVA: 0x001B8710 File Offset: 0x001B6910
	public static bool DoAnyTagsMatchFilter(string[] lowercaseTags, in string filter)
	{
		string text = filter.Trim().ToLowerInvariant();
		string[] source = text.Split(' ', StringSplitOptions.None);
		for (int i = 0; i < lowercaseTags.Length; i++)
		{
			string tag = lowercaseTags[i];
			if (StringSearchableListUtil.DoesTagMatchFilter(tag, text))
			{
				return true;
			}
			if ((from f in source
			select StringSearchableListUtil.DoesTagMatchFilter(tag, f)).All((bool result) => result))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C49 RID: 7241 RVA: 0x000B705C File Offset: 0x000B525C
	public static bool DoesTagMatchFilter(string lowercaseTag, in string filter)
	{
		return string.IsNullOrWhiteSpace(filter) || lowercaseTag.Contains(filter);
	}

	// Token: 0x06001C4A RID: 7242 RVA: 0x000B7076 File Offset: 0x000B5276
	public static bool ShouldUseFilter(string filter)
	{
		return !string.IsNullOrWhiteSpace(filter);
	}
}
