using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x020022F5 RID: 8949
	public class LIGHT2D
	{
		// Token: 0x04009D40 RID: 40256
		public const int SUNLIGHT_MAX_DEFAULT = 80000;

		// Token: 0x04009D41 RID: 40257
		public static readonly Color LIGHT_BLUE = new Color(0.38f, 0.61f, 1f, 1f);

		// Token: 0x04009D42 RID: 40258
		public static readonly Color LIGHT_PURPLE = new Color(0.9f, 0.4f, 0.74f, 1f);

		// Token: 0x04009D43 RID: 40259
		public static readonly Color LIGHT_PINK = new Color(0.9f, 0.4f, 0.6f, 1f);

		// Token: 0x04009D44 RID: 40260
		public static readonly Color LIGHT_YELLOW = new Color(0.57f, 0.55f, 0.44f, 1f);

		// Token: 0x04009D45 RID: 40261
		public static readonly Color LIGHT_OVERLAY = new Color(0.56f, 0.56f, 0.56f, 1f);

		// Token: 0x04009D46 RID: 40262
		public static readonly Vector2 DEFAULT_DIRECTION = new Vector2(0f, -1f);

		// Token: 0x04009D47 RID: 40263
		public const int FLOORLAMP_LUX = 1000;

		// Token: 0x04009D48 RID: 40264
		public const float FLOORLAMP_RANGE = 4f;

		// Token: 0x04009D49 RID: 40265
		public const float FLOORLAMP_ANGLE = 0f;

		// Token: 0x04009D4A RID: 40266
		public const global::LightShape FLOORLAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D4B RID: 40267
		public static readonly Color FLOORLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D4C RID: 40268
		public static readonly Color FLOORLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D4D RID: 40269
		public static readonly Vector2 FLOORLAMP_OFFSET = new Vector2(0.05f, 1.5f);

		// Token: 0x04009D4E RID: 40270
		public static readonly Vector2 FLOORLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D4F RID: 40271
		public const float CEILINGLIGHT_RANGE = 8f;

		// Token: 0x04009D50 RID: 40272
		public const float CEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04009D51 RID: 40273
		public const global::LightShape CEILINGLIGHT_SHAPE = global::LightShape.Cone;

		// Token: 0x04009D52 RID: 40274
		public static readonly Color CEILINGLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D53 RID: 40275
		public static readonly Color CEILINGLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D54 RID: 40276
		public static readonly Vector2 CEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04009D55 RID: 40277
		public static readonly Vector2 CEILINGLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D56 RID: 40278
		public const int CEILINGLIGHT_LUX = 1800;

		// Token: 0x04009D57 RID: 40279
		public static readonly int SUNLAMP_LUX = (int)((float)BeachChairConfig.TAN_LUX * 4f);

		// Token: 0x04009D58 RID: 40280
		public const float SUNLAMP_RANGE = 16f;

		// Token: 0x04009D59 RID: 40281
		public const float SUNLAMP_ANGLE = 5.2f;

		// Token: 0x04009D5A RID: 40282
		public const global::LightShape SUNLAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04009D5B RID: 40283
		public static readonly Color SUNLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D5C RID: 40284
		public static readonly Color SUNLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D5D RID: 40285
		public static readonly Vector2 SUNLAMP_OFFSET = new Vector2(0f, 3.5f);

		// Token: 0x04009D5E RID: 40286
		public static readonly Vector2 SUNLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D5F RID: 40287
		public const int MERCURYCEILINGLIGHT_LUX = 60000;

		// Token: 0x04009D60 RID: 40288
		public const float MERCURYCEILINGLIGHT_RANGE = 8f;

		// Token: 0x04009D61 RID: 40289
		public const float MERCURYCEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x04009D62 RID: 40290
		public const float MERCURYCEILINGLIGHT_FALLOFFRATE = 0.4f;

		// Token: 0x04009D63 RID: 40291
		public const int MERCURYCEILINGLIGHT_WIDTH = 3;

		// Token: 0x04009D64 RID: 40292
		public const global::LightShape MERCURYCEILINGLIGHT_SHAPE = global::LightShape.Quad;

		// Token: 0x04009D65 RID: 40293
		public static readonly Color MERCURYCEILINGLIGHT_LUX_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D66 RID: 40294
		public static readonly Color MERCURYCEILINGLIGHT_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04009D67 RID: 40295
		public static readonly Vector2 MERCURYCEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04009D68 RID: 40296
		public static readonly Vector2 MERCURYCEILINGLIGHT_DIRECTIONVECTOR = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D69 RID: 40297
		public const DiscreteShadowCaster.Direction MERCURYCEILINGLIGHT_DIRECTION = DiscreteShadowCaster.Direction.South;

		// Token: 0x04009D6A RID: 40298
		public static readonly Color LIGHT_PREVIEW_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D6B RID: 40299
		public const float HEADQUARTERS_RANGE = 5f;

		// Token: 0x04009D6C RID: 40300
		public const global::LightShape HEADQUARTERS_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D6D RID: 40301
		public static readonly Color HEADQUARTERS_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D6E RID: 40302
		public static readonly Color HEADQUARTERS_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D6F RID: 40303
		public static readonly Vector2 HEADQUARTERS_OFFSET = new Vector2(0.5f, 3f);

		// Token: 0x04009D70 RID: 40304
		public static readonly Vector2 EXOBASE_HEADQUARTERS_OFFSET = new Vector2(0f, 2.5f);

		// Token: 0x04009D71 RID: 40305
		public const float POI_TECH_UNLOCK_RANGE = 5f;

		// Token: 0x04009D72 RID: 40306
		public const float POI_TECH_UNLOCK_ANGLE = 2.6f;

		// Token: 0x04009D73 RID: 40307
		public const global::LightShape POI_TECH_UNLOCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04009D74 RID: 40308
		public static readonly Color POI_TECH_UNLOCK_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D75 RID: 40309
		public static readonly Color POI_TECH_UNLOCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D76 RID: 40310
		public static readonly Vector2 POI_TECH_UNLOCK_OFFSET = new Vector2(0f, 3.4f);

		// Token: 0x04009D77 RID: 40311
		public const int POI_TECH_UNLOCK_LUX = 1800;

		// Token: 0x04009D78 RID: 40312
		public static readonly Vector2 POI_TECH_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D79 RID: 40313
		public const float ENGINE_RANGE = 10f;

		// Token: 0x04009D7A RID: 40314
		public const global::LightShape ENGINE_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D7B RID: 40315
		public const int ENGINE_LUX = 80000;

		// Token: 0x04009D7C RID: 40316
		public const float WALLLIGHT_RANGE = 4f;

		// Token: 0x04009D7D RID: 40317
		public const float WALLLIGHT_ANGLE = 0f;

		// Token: 0x04009D7E RID: 40318
		public const global::LightShape WALLLIGHT_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D7F RID: 40319
		public static readonly Color WALLLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D80 RID: 40320
		public static readonly Color WALLLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D81 RID: 40321
		public static readonly Vector2 WALLLIGHT_OFFSET = new Vector2(0f, 0.5f);

		// Token: 0x04009D82 RID: 40322
		public static readonly Vector2 WALLLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D83 RID: 40323
		public const float LIGHTBUG_RANGE = 5f;

		// Token: 0x04009D84 RID: 40324
		public const float LIGHTBUG_ANGLE = 0f;

		// Token: 0x04009D85 RID: 40325
		public const global::LightShape LIGHTBUG_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D86 RID: 40326
		public const int LIGHTBUG_LUX = 1800;

		// Token: 0x04009D87 RID: 40327
		public static readonly Color LIGHTBUG_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009D88 RID: 40328
		public static readonly Color LIGHTBUG_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D89 RID: 40329
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(0.5686275f, 0.48235294f, 0.4392157f, 1f);

		// Token: 0x04009D8A RID: 40330
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(0.49019608f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04009D8B RID: 40331
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(0.5686275f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04009D8C RID: 40332
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(0.4392157f, 0.4862745f, 0.5686275f, 1f);

		// Token: 0x04009D8D RID: 40333
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(0.5137255f, 0.6666667f, 0.6666667f, 1f);

		// Token: 0x04009D8E RID: 40334
		public static readonly Color LIGHTBUG_COLOR_GREEN = new Color(0.43137255f, 1f, 0.53333336f, 1f);

		// Token: 0x04009D8F RID: 40335
		public const int MAJORFOSSILDIGSITE_LAMP_LUX = 1000;

		// Token: 0x04009D90 RID: 40336
		public const float MAJORFOSSILDIGSITE_LAMP_RANGE = 3f;

		// Token: 0x04009D91 RID: 40337
		public static readonly Vector2 MAJORFOSSILDIGSITE_LAMP_OFFSET = new Vector2(-0.15f, 2.35f);

		// Token: 0x04009D92 RID: 40338
		public static readonly Vector2 LIGHTBUG_OFFSET = new Vector2(0.05f, 0.25f);

		// Token: 0x04009D93 RID: 40339
		public static readonly Vector2 LIGHTBUG_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D94 RID: 40340
		public const int PLASMALAMP_LUX = 666;

		// Token: 0x04009D95 RID: 40341
		public const float PLASMALAMP_RANGE = 2f;

		// Token: 0x04009D96 RID: 40342
		public const float PLASMALAMP_ANGLE = 0f;

		// Token: 0x04009D97 RID: 40343
		public const global::LightShape PLASMALAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04009D98 RID: 40344
		public static readonly Color PLASMALAMP_COLOR = LIGHT2D.LIGHT_PURPLE;

		// Token: 0x04009D99 RID: 40345
		public static readonly Color PLASMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009D9A RID: 40346
		public static readonly Vector2 PLASMALAMP_OFFSET = new Vector2(0.05f, 0.5f);

		// Token: 0x04009D9B RID: 40347
		public static readonly Vector2 PLASMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009D9C RID: 40348
		public const int MAGMALAMP_LUX = 666;

		// Token: 0x04009D9D RID: 40349
		public const float MAGMALAMP_RANGE = 2f;

		// Token: 0x04009D9E RID: 40350
		public const float MAGMALAMP_ANGLE = 0f;

		// Token: 0x04009D9F RID: 40351
		public const global::LightShape MAGMALAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04009DA0 RID: 40352
		public static readonly Color MAGMALAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04009DA1 RID: 40353
		public static readonly Color MAGMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009DA2 RID: 40354
		public static readonly Vector2 MAGMALAMP_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009DA3 RID: 40355
		public static readonly Vector2 MAGMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009DA4 RID: 40356
		public const int BIOLUMROCK_LUX = 666;

		// Token: 0x04009DA5 RID: 40357
		public const float BIOLUMROCK_RANGE = 2f;

		// Token: 0x04009DA6 RID: 40358
		public const float BIOLUMROCK_ANGLE = 0f;

		// Token: 0x04009DA7 RID: 40359
		public const global::LightShape BIOLUMROCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04009DA8 RID: 40360
		public static readonly Color BIOLUMROCK_COLOR = LIGHT2D.LIGHT_BLUE;

		// Token: 0x04009DA9 RID: 40361
		public static readonly Color BIOLUMROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009DAA RID: 40362
		public static readonly Vector2 BIOLUMROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009DAB RID: 40363
		public static readonly Vector2 BIOLUMROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04009DAC RID: 40364
		public const float PINKROCK_RANGE = 2f;

		// Token: 0x04009DAD RID: 40365
		public const float PINKROCK_ANGLE = 0f;

		// Token: 0x04009DAE RID: 40366
		public const global::LightShape PINKROCK_SHAPE = global::LightShape.Circle;

		// Token: 0x04009DAF RID: 40367
		public static readonly Color PINKROCK_COLOR = LIGHT2D.LIGHT_PINK;

		// Token: 0x04009DB0 RID: 40368
		public static readonly Color PINKROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04009DB1 RID: 40369
		public static readonly Vector2 PINKROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04009DB2 RID: 40370
		public static readonly Vector2 PINKROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;
	}
}
