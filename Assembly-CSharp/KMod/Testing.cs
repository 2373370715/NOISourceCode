using System;

namespace KMod
{
	// Token: 0x02002235 RID: 8757
	public static class Testing
	{
		// Token: 0x040098A1 RID: 39073
		public static Testing.DLLLoading dll_loading;

		// Token: 0x040098A2 RID: 39074
		public const Testing.SaveLoad SAVE_LOAD = Testing.SaveLoad.NoTesting;

		// Token: 0x040098A3 RID: 39075
		public const Testing.Install INSTALL = Testing.Install.NoTesting;

		// Token: 0x040098A4 RID: 39076
		public const Testing.Boot BOOT = Testing.Boot.NoTesting;

		// Token: 0x02002236 RID: 8758
		public enum DLLLoading
		{
			// Token: 0x040098A6 RID: 39078
			NoTesting,
			// Token: 0x040098A7 RID: 39079
			Fail,
			// Token: 0x040098A8 RID: 39080
			UseModLoaderDLLExclusively
		}

		// Token: 0x02002237 RID: 8759
		public enum SaveLoad
		{
			// Token: 0x040098AA RID: 39082
			NoTesting,
			// Token: 0x040098AB RID: 39083
			FailSave,
			// Token: 0x040098AC RID: 39084
			FailLoad
		}

		// Token: 0x02002238 RID: 8760
		public enum Install
		{
			// Token: 0x040098AE RID: 39086
			NoTesting,
			// Token: 0x040098AF RID: 39087
			ForceUninstall,
			// Token: 0x040098B0 RID: 39088
			ForceReinstall,
			// Token: 0x040098B1 RID: 39089
			ForceUpdate
		}

		// Token: 0x02002239 RID: 8761
		public enum Boot
		{
			// Token: 0x040098B3 RID: 39091
			NoTesting,
			// Token: 0x040098B4 RID: 39092
			Crash
		}
	}
}
