using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x0200223A RID: 8762
	public class UserMod2
	{
		// Token: 0x17000BFC RID: 3068
		// (get) Token: 0x0600BA24 RID: 47652 RVA: 0x0011C70D File Offset: 0x0011A90D
		// (set) Token: 0x0600BA25 RID: 47653 RVA: 0x0011C715 File Offset: 0x0011A915
		public Assembly assembly { get; set; }

		// Token: 0x17000BFD RID: 3069
		// (get) Token: 0x0600BA26 RID: 47654 RVA: 0x0011C71E File Offset: 0x0011A91E
		// (set) Token: 0x0600BA27 RID: 47655 RVA: 0x0011C726 File Offset: 0x0011A926
		public string path { get; set; }

		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x0600BA28 RID: 47656 RVA: 0x0011C72F File Offset: 0x0011A92F
		// (set) Token: 0x0600BA29 RID: 47657 RVA: 0x0011C737 File Offset: 0x0011A937
		public Mod mod { get; set; }

		// Token: 0x0600BA2A RID: 47658 RVA: 0x0011C740 File Offset: 0x0011A940
		public virtual void OnLoad(Harmony harmony)
		{
			harmony.PatchAll(this.assembly);
		}

		// Token: 0x0600BA2B RID: 47659 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
		{
		}
	}
}
