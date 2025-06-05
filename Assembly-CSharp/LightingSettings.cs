using System;
using UnityEngine;

// Token: 0x020017D6 RID: 6102
public class LightingSettings : ScriptableObject
{
	// Token: 0x04005EC7 RID: 24263
	[Header("Global")]
	public bool UpdateLightSettings;

	// Token: 0x04005EC8 RID: 24264
	public float BloomScale;

	// Token: 0x04005EC9 RID: 24265
	public Color32 LightColour = Color.white;

	// Token: 0x04005ECA RID: 24266
	[Header("Digging")]
	public float DigMapScale;

	// Token: 0x04005ECB RID: 24267
	public Color DigMapColour;

	// Token: 0x04005ECC RID: 24268
	public Texture2D DigDamageMap;

	// Token: 0x04005ECD RID: 24269
	[Header("State Transition")]
	public Texture2D StateTransitionMap;

	// Token: 0x04005ECE RID: 24270
	public Color StateTransitionColor;

	// Token: 0x04005ECF RID: 24271
	public float StateTransitionUVScale;

	// Token: 0x04005ED0 RID: 24272
	public Vector2 StateTransitionUVOffsetRate;

	// Token: 0x04005ED1 RID: 24273
	[Header("Falling Solids")]
	public Texture2D FallingSolidMap;

	// Token: 0x04005ED2 RID: 24274
	public Color FallingSolidColor;

	// Token: 0x04005ED3 RID: 24275
	public float FallingSolidUVScale;

	// Token: 0x04005ED4 RID: 24276
	public Vector2 FallingSolidUVOffsetRate;

	// Token: 0x04005ED5 RID: 24277
	[Header("Metal Shine")]
	public Vector2 ShineCenter;

	// Token: 0x04005ED6 RID: 24278
	public Vector2 ShineRange;

	// Token: 0x04005ED7 RID: 24279
	public float ShineZoomSpeed;

	// Token: 0x04005ED8 RID: 24280
	[Header("Water")]
	public Color WaterTrimColor;

	// Token: 0x04005ED9 RID: 24281
	public float WaterTrimSize;

	// Token: 0x04005EDA RID: 24282
	public float WaterAlphaTrimSize;

	// Token: 0x04005EDB RID: 24283
	public float WaterAlphaThreshold;

	// Token: 0x04005EDC RID: 24284
	public float WaterCubesAlphaThreshold;

	// Token: 0x04005EDD RID: 24285
	public float WaterWaveAmplitude;

	// Token: 0x04005EDE RID: 24286
	public float WaterWaveFrequency;

	// Token: 0x04005EDF RID: 24287
	public float WaterWaveSpeed;

	// Token: 0x04005EE0 RID: 24288
	public float WaterDetailSpeed;

	// Token: 0x04005EE1 RID: 24289
	public float WaterDetailTiling;

	// Token: 0x04005EE2 RID: 24290
	public float WaterDetailTiling2;

	// Token: 0x04005EE3 RID: 24291
	public Vector2 WaterDetailDirection;

	// Token: 0x04005EE4 RID: 24292
	public float WaterWaveAmplitude2;

	// Token: 0x04005EE5 RID: 24293
	public float WaterWaveFrequency2;

	// Token: 0x04005EE6 RID: 24294
	public float WaterWaveSpeed2;

	// Token: 0x04005EE7 RID: 24295
	public float WaterCubeMapScale;

	// Token: 0x04005EE8 RID: 24296
	public float WaterColorScale;

	// Token: 0x04005EE9 RID: 24297
	public float WaterDistortionScaleStart;

	// Token: 0x04005EEA RID: 24298
	public float WaterDistortionScaleEnd;

	// Token: 0x04005EEB RID: 24299
	public float WaterDepthColorOpacityStart;

	// Token: 0x04005EEC RID: 24300
	public float WaterDepthColorOpacityEnd;

	// Token: 0x04005EED RID: 24301
	[Header("Liquid")]
	public float LiquidMin;

	// Token: 0x04005EEE RID: 24302
	public float LiquidMax;

	// Token: 0x04005EEF RID: 24303
	public float LiquidCutoff;

	// Token: 0x04005EF0 RID: 24304
	public float LiquidTransparency;

	// Token: 0x04005EF1 RID: 24305
	public float LiquidAmountOffset;

	// Token: 0x04005EF2 RID: 24306
	public float LiquidMaxMass;

	// Token: 0x04005EF3 RID: 24307
	[Header("Grid")]
	public float GridLineWidth;

	// Token: 0x04005EF4 RID: 24308
	public float GridSize;

	// Token: 0x04005EF5 RID: 24309
	public float GridMaxIntensity;

	// Token: 0x04005EF6 RID: 24310
	public float GridMinIntensity;

	// Token: 0x04005EF7 RID: 24311
	public Color GridColor;

	// Token: 0x04005EF8 RID: 24312
	[Header("Terrain")]
	public float EdgeGlowCutoffStart;

	// Token: 0x04005EF9 RID: 24313
	public float EdgeGlowCutoffEnd;

	// Token: 0x04005EFA RID: 24314
	public float EdgeGlowIntensity;

	// Token: 0x04005EFB RID: 24315
	public int BackgroundLayers;

	// Token: 0x04005EFC RID: 24316
	public float BackgroundBaseParallax;

	// Token: 0x04005EFD RID: 24317
	public float BackgroundLayerParallax;

	// Token: 0x04005EFE RID: 24318
	public float BackgroundDarkening;

	// Token: 0x04005EFF RID: 24319
	public float BackgroundClip;

	// Token: 0x04005F00 RID: 24320
	public float BackgroundUVScale;

	// Token: 0x04005F01 RID: 24321
	public global::LightingSettings.EdgeLighting substanceEdgeParameters;

	// Token: 0x04005F02 RID: 24322
	public global::LightingSettings.EdgeLighting tileEdgeParameters;

	// Token: 0x04005F03 RID: 24323
	public float AnimIntensity;

	// Token: 0x04005F04 RID: 24324
	public float GasMinOpacity;

	// Token: 0x04005F05 RID: 24325
	public float GasMaxOpacity;

	// Token: 0x04005F06 RID: 24326
	public Color[] DarkenTints;

	// Token: 0x04005F07 RID: 24327
	public global::LightingSettings.LightingColours characterLighting;

	// Token: 0x04005F08 RID: 24328
	public Color BrightenOverlayColour;

	// Token: 0x04005F09 RID: 24329
	public Color[] ColdColours;

	// Token: 0x04005F0A RID: 24330
	public Color[] HotColours;

	// Token: 0x04005F0B RID: 24331
	[Header("Temperature Overlay Effects")]
	public Vector4 TemperatureParallax;

	// Token: 0x04005F0C RID: 24332
	public Texture2D EmberTex;

	// Token: 0x04005F0D RID: 24333
	public Texture2D FrostTex;

	// Token: 0x04005F0E RID: 24334
	public Texture2D Thermal1Tex;

	// Token: 0x04005F0F RID: 24335
	public Texture2D Thermal2Tex;

	// Token: 0x04005F10 RID: 24336
	public Vector2 ColdFGUVOffset;

	// Token: 0x04005F11 RID: 24337
	public Vector2 ColdMGUVOffset;

	// Token: 0x04005F12 RID: 24338
	public Vector2 ColdBGUVOffset;

	// Token: 0x04005F13 RID: 24339
	public Vector2 HotFGUVOffset;

	// Token: 0x04005F14 RID: 24340
	public Vector2 HotMGUVOffset;

	// Token: 0x04005F15 RID: 24341
	public Vector2 HotBGUVOffset;

	// Token: 0x04005F16 RID: 24342
	public Texture2D DustTex;

	// Token: 0x04005F17 RID: 24343
	public Color DustColour;

	// Token: 0x04005F18 RID: 24344
	public float DustScale;

	// Token: 0x04005F19 RID: 24345
	public Vector3 DustMovement;

	// Token: 0x04005F1A RID: 24346
	public float ShowGas;

	// Token: 0x04005F1B RID: 24347
	public float ShowTemperature;

	// Token: 0x04005F1C RID: 24348
	public float ShowDust;

	// Token: 0x04005F1D RID: 24349
	public float ShowShadow;

	// Token: 0x04005F1E RID: 24350
	public Vector4 HeatHazeParameters;

	// Token: 0x04005F1F RID: 24351
	public Texture2D HeatHazeTexture;

	// Token: 0x04005F20 RID: 24352
	[Header("Biome")]
	public float WorldZoneGasBlend;

	// Token: 0x04005F21 RID: 24353
	public float WorldZoneLiquidBlend;

	// Token: 0x04005F22 RID: 24354
	public float WorldZoneForegroundBlend;

	// Token: 0x04005F23 RID: 24355
	public float WorldZoneSimpleAnimBlend;

	// Token: 0x04005F24 RID: 24356
	public float WorldZoneAnimBlend;

	// Token: 0x04005F25 RID: 24357
	[Header("FX")]
	public Color32 SmokeDamageTint;

	// Token: 0x04005F26 RID: 24358
	[Header("Building Damage")]
	public Texture2D BuildingDamagedTex;

	// Token: 0x04005F27 RID: 24359
	public Vector4 BuildingDamagedUVParameters;

	// Token: 0x04005F28 RID: 24360
	[Header("Disease")]
	public Texture2D DiseaseOverlayTex;

	// Token: 0x04005F29 RID: 24361
	public Vector4 DiseaseOverlayTexInfo;

	// Token: 0x04005F2A RID: 24362
	[Header("Conduits")]
	public ConduitFlowVisualizer.Tuning GasConduit;

	// Token: 0x04005F2B RID: 24363
	public ConduitFlowVisualizer.Tuning LiquidConduit;

	// Token: 0x04005F2C RID: 24364
	public SolidConduitFlowVisualizer.Tuning SolidConduit;

	// Token: 0x04005F2D RID: 24365
	[Header("Radiation Overlay")]
	public bool ShowRadiation;

	// Token: 0x04005F2E RID: 24366
	public Texture2D Radiation1Tex;

	// Token: 0x04005F2F RID: 24367
	public Texture2D Radiation2Tex;

	// Token: 0x04005F30 RID: 24368
	public Texture2D Radiation3Tex;

	// Token: 0x04005F31 RID: 24369
	public Texture2D Radiation4Tex;

	// Token: 0x04005F32 RID: 24370
	public Texture2D NoiseTex;

	// Token: 0x04005F33 RID: 24371
	public Color RadColor;

	// Token: 0x04005F34 RID: 24372
	public Vector2 Rad1UVOffset;

	// Token: 0x04005F35 RID: 24373
	public Vector2 Rad2UVOffset;

	// Token: 0x04005F36 RID: 24374
	public Vector2 Rad3UVOffset;

	// Token: 0x04005F37 RID: 24375
	public Vector2 Rad4UVOffset;

	// Token: 0x04005F38 RID: 24376
	public Vector4 RadUVScales;

	// Token: 0x04005F39 RID: 24377
	public Vector2 Rad1Range;

	// Token: 0x04005F3A RID: 24378
	public Vector2 Rad2Range;

	// Token: 0x04005F3B RID: 24379
	public Vector2 Rad3Range;

	// Token: 0x04005F3C RID: 24380
	public Vector2 Rad4Range;

	// Token: 0x020017D7 RID: 6103
	[Serializable]
	public struct EdgeLighting
	{
		// Token: 0x04005F3D RID: 24381
		public float intensity;

		// Token: 0x04005F3E RID: 24382
		public float edgeIntensity;

		// Token: 0x04005F3F RID: 24383
		public float directSunlightScale;

		// Token: 0x04005F40 RID: 24384
		public float power;
	}

	// Token: 0x020017D8 RID: 6104
	public enum TintLayers
	{
		// Token: 0x04005F42 RID: 24386
		Background,
		// Token: 0x04005F43 RID: 24387
		Midground,
		// Token: 0x04005F44 RID: 24388
		Foreground,
		// Token: 0x04005F45 RID: 24389
		NumLayers
	}

	// Token: 0x020017D9 RID: 6105
	[Serializable]
	public struct LightingColours
	{
		// Token: 0x04005F46 RID: 24390
		public Color32 litColour;

		// Token: 0x04005F47 RID: 24391
		public Color32 unlitColour;
	}
}
