using System;

namespace Database
{
	// Token: 0x020021ED RID: 8685
	public abstract class ColonyAchievementRequirement
	{
		// Token: 0x0600B8F6 RID: 47350
		public abstract bool Success();

		// Token: 0x0600B8F7 RID: 47351 RVA: 0x000B1628 File Offset: 0x000AF828
		public virtual bool Fail()
		{
			return false;
		}

		// Token: 0x0600B8F8 RID: 47352 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
		public virtual string GetProgress(bool complete)
		{
			return "";
		}
	}
}
