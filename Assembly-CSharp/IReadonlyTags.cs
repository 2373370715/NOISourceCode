using System;

// Token: 0x02001A2C RID: 6700
public interface IReadonlyTags
{
	// Token: 0x06008B93 RID: 35731
	bool HasTag(string tag);

	// Token: 0x06008B94 RID: 35732
	bool HasTag(int hashtag);

	// Token: 0x06008B95 RID: 35733
	bool HasTags(int[] tags);
}
