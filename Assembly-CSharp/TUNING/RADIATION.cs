using System;

namespace TUNING
{
	// Token: 0x020022F1 RID: 8945
	public class RADIATION
	{
		// Token: 0x04009D29 RID: 40233
		public const float GERM_RAD_SCALE = 0.01f;

		// Token: 0x04009D2A RID: 40234
		public const float STANDARD_DAILY_RECOVERY = 100f;

		// Token: 0x04009D2B RID: 40235
		public const float EXTRA_VOMIT_RECOVERY = 20f;

		// Token: 0x04009D2C RID: 40236
		public const float REACT_THRESHOLD = 133f;

		// Token: 0x020022F2 RID: 8946
		public class STANDARD_EMITTER
		{
			// Token: 0x04009D2D RID: 40237
			public const float STEADY_PULSE_RATE = 0.2f;

			// Token: 0x04009D2E RID: 40238
			public const float DOUBLE_SPEED_PULSE_RATE = 0.1f;

			// Token: 0x04009D2F RID: 40239
			public const float RADIUS_SCALE = 1f;
		}

		// Token: 0x020022F3 RID: 8947
		public class RADIATION_PER_SECOND
		{
			// Token: 0x04009D30 RID: 40240
			public const float TRIVIAL = 60f;

			// Token: 0x04009D31 RID: 40241
			public const float VERY_LOW = 120f;

			// Token: 0x04009D32 RID: 40242
			public const float LOW = 240f;

			// Token: 0x04009D33 RID: 40243
			public const float MODERATE = 600f;

			// Token: 0x04009D34 RID: 40244
			public const float HIGH = 1800f;

			// Token: 0x04009D35 RID: 40245
			public const float VERY_HIGH = 4800f;

			// Token: 0x04009D36 RID: 40246
			public const int EXTREME = 9600;
		}

		// Token: 0x020022F4 RID: 8948
		public class RADIATION_CONSTANT_RADS_PER_CYCLE
		{
			// Token: 0x04009D37 RID: 40247
			public const float LESS_THAN_TRIVIAL = 60f;

			// Token: 0x04009D38 RID: 40248
			public const float TRIVIAL = 120f;

			// Token: 0x04009D39 RID: 40249
			public const float VERY_LOW = 240f;

			// Token: 0x04009D3A RID: 40250
			public const float LOW = 480f;

			// Token: 0x04009D3B RID: 40251
			public const float MODERATE = 1200f;

			// Token: 0x04009D3C RID: 40252
			public const float MODERATE_PLUS = 2400f;

			// Token: 0x04009D3D RID: 40253
			public const float HIGH = 3600f;

			// Token: 0x04009D3E RID: 40254
			public const float VERY_HIGH = 8400f;

			// Token: 0x04009D3F RID: 40255
			public const int EXTREME = 16800;
		}
	}
}
