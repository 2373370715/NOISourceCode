using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace KMod
{
	// Token: 0x0200223B RID: 8763
	public class LoadedModData
	{
		// Token: 0x040098B8 RID: 39096
		public Harmony harmony;

		// Token: 0x040098B9 RID: 39097
		public Dictionary<Assembly, UserMod2> userMod2Instances;

		// Token: 0x040098BA RID: 39098
		public ICollection<Assembly> dlls;

		// Token: 0x040098BB RID: 39099
		public ICollection<MethodBase> patched_methods;
	}
}
