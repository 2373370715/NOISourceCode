using System;

namespace TUNING
{
	// Token: 0x020022A0 RID: 8864
	public class STRESS
	{
		// Token: 0x04009AD0 RID: 39632
		public static float ACTING_OUT_RESET = 60f;

		// Token: 0x04009AD1 RID: 39633
		public static float VOMIT_AMOUNT = 0.90000004f;

		// Token: 0x04009AD2 RID: 39634
		public static float TEARS_RATE = 0.040000003f;

		// Token: 0x04009AD3 RID: 39635
		public static int BANSHEE_WAIL_RADIUS = 8;

		// Token: 0x020022A1 RID: 8865
		public class SHOCKER
		{
			// Token: 0x04009AD4 RID: 39636
			public static int SHOCK_RADIUS = 4;

			// Token: 0x04009AD5 RID: 39637
			public static float DAMAGE_RATE = 2.5f;

			// Token: 0x04009AD6 RID: 39638
			public static float POWER_CONSUMPTION_RATE = 2000f;

			// Token: 0x04009AD7 RID: 39639
			public static float FAKE_POWER_CONSUMPTION_RATE = STRESS.SHOCKER.POWER_CONSUMPTION_RATE * 0.25f;

			// Token: 0x04009AD8 RID: 39640
			public static float MAX_POWER_USE = 120000f;
		}
	}
}
