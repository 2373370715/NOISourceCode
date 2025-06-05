using System;
using STRINGS;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02002285 RID: 8837
	public class OVERLAY
	{
		// Token: 0x02002286 RID: 8838
		public class TEMPERATURE_LEGEND
		{
			// Token: 0x04009A3B RID: 39483
			public static readonly LegendEntry MAXHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.MAXHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009A3C RID: 39484
			public static readonly LegendEntry EXTREMEHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMEHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009A3D RID: 39485
			public static readonly LegendEntry VERYHOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYHOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(1f, 0f, 0f), null, null, true);

			// Token: 0x04009A3E RID: 39486
			public static readonly LegendEntry HOT = new LegendEntry(UI.OVERLAYS.TEMPERATURE.HOT, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 1f, 0f), null, null, true);

			// Token: 0x04009A3F RID: 39487
			public static readonly LegendEntry TEMPERATE = new LegendEntry(UI.OVERLAYS.TEMPERATURE.TEMPERATE, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009A40 RID: 39488
			public static readonly LegendEntry COLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.COLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x04009A41 RID: 39489
			public static readonly LegendEntry VERYCOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.VERYCOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 1f), null, null, true);

			// Token: 0x04009A42 RID: 39490
			public static readonly LegendEntry EXTREMECOLD = new LegendEntry(UI.OVERLAYS.TEMPERATURE.EXTREMECOLD, UI.OVERLAYS.TEMPERATURE.TOOLTIPS.TEMPERATURE, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02002287 RID: 8839
		public class HEATFLOW_LEGEND
		{
			// Token: 0x04009A43 RID: 39491
			public static readonly LegendEntry HEATING = new LegendEntry(UI.OVERLAYS.HEATFLOW.HEATING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.HEATING, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009A44 RID: 39492
			public static readonly LegendEntry NEUTRAL = new LegendEntry(UI.OVERLAYS.HEATFLOW.NEUTRAL, UI.OVERLAYS.HEATFLOW.TOOLTIPS.NEUTRAL, new Color(0f, 0f, 0f), null, null, true);

			// Token: 0x04009A45 RID: 39493
			public static readonly LegendEntry COOLING = new LegendEntry(UI.OVERLAYS.HEATFLOW.COOLING, UI.OVERLAYS.HEATFLOW.TOOLTIPS.COOLING, new Color(0f, 0f, 0f), null, null, true);
		}

		// Token: 0x02002288 RID: 8840
		public class POWER_LEGEND
		{
			// Token: 0x04009A46 RID: 39494
			public const float WATTAGE_WARNING_THRESHOLD = 0.75f;
		}
	}
}
