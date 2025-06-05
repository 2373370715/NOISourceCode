using System;

namespace ProcGenGame
{
	// Token: 0x02002115 RID: 8469
	public class WorldgenException : Exception
	{
		// Token: 0x0600B44F RID: 46159 RVA: 0x00119B48 File Offset: 0x00117D48
		public WorldgenException(string message, string userMessage) : base(message)
		{
			this.userMessage = userMessage;
		}

		// Token: 0x04008EB7 RID: 36535
		public readonly string userMessage;
	}
}
