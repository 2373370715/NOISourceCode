using System;
using UnityEngine;

// Token: 0x020017D4 RID: 6100
[ExecuteInEditMode]
public class Lighting : MonoBehaviour
{
	// Token: 0x06007D74 RID: 32116 RVA: 0x000F7252 File Offset: 0x000F5452
	private void Awake()
	{
		Lighting.Instance = this;
	}

	// Token: 0x06007D75 RID: 32117 RVA: 0x000F725A File Offset: 0x000F545A
	private void OnDestroy()
	{
		Lighting.Instance = null;
	}

	// Token: 0x06007D76 RID: 32118 RVA: 0x000F7262 File Offset: 0x000F5462
	private Color PremultiplyAlpha(Color c)
	{
		return c * c.a;
	}

	// Token: 0x06007D77 RID: 32119 RVA: 0x000F7270 File Offset: 0x000F5470
	private void Start()
	{
		this.UpdateLighting();
	}

	// Token: 0x06007D78 RID: 32120 RVA: 0x000F7270 File Offset: 0x000F5470
	private void Update()
	{
		this.UpdateLighting();
	}

	// Token: 0x06007D79 RID: 32121 RVA: 0x00331734 File Offset: 0x0032F934
	private void UpdateLighting()
	{
		Shader.SetGlobalInt(Lighting._liquidZ, -28);
		Shader.SetGlobalVector(Lighting._DigMapMapParameters, new Vector4(this.Settings.DigMapColour.r, this.Settings.DigMapColour.g, this.Settings.DigMapColour.b, this.Settings.DigMapScale));
		Shader.SetGlobalTexture(Lighting._DigDamageMap, this.Settings.DigDamageMap);
		Shader.SetGlobalTexture(Lighting._StateTransitionMap, this.Settings.StateTransitionMap);
		Shader.SetGlobalColor(Lighting._StateTransitionColor, this.Settings.StateTransitionColor);
		Shader.SetGlobalVector(Lighting._StateTransitionParameters, new Vector4(1f / this.Settings.StateTransitionUVScale, this.Settings.StateTransitionUVOffsetRate.x, this.Settings.StateTransitionUVOffsetRate.y, 0f));
		Shader.SetGlobalTexture(Lighting._FallingSolidMap, this.Settings.FallingSolidMap);
		Shader.SetGlobalColor(Lighting._FallingSolidColor, this.Settings.FallingSolidColor);
		Shader.SetGlobalVector(Lighting._FallingSolidParameters, new Vector4(1f / this.Settings.FallingSolidUVScale, this.Settings.FallingSolidUVOffsetRate.x, this.Settings.FallingSolidUVOffsetRate.y, 0f));
		Shader.SetGlobalColor(Lighting._WaterTrimColor, this.Settings.WaterTrimColor);
		Shader.SetGlobalVector(Lighting._WaterParameters2, new Vector4(this.Settings.WaterTrimSize, this.Settings.WaterAlphaTrimSize, 0f, this.Settings.WaterAlphaThreshold));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters, new Vector4(this.Settings.WaterWaveAmplitude, this.Settings.WaterWaveFrequency, this.Settings.WaterWaveSpeed, 0f));
		Shader.SetGlobalVector(Lighting._WaterWaveParameters2, new Vector4(this.Settings.WaterWaveAmplitude2, this.Settings.WaterWaveFrequency2, this.Settings.WaterWaveSpeed2, 0f));
		Shader.SetGlobalVector(Lighting._WaterDetailParameters, new Vector4(this.Settings.WaterCubeMapScale, this.Settings.WaterDetailTiling, this.Settings.WaterColorScale, this.Settings.WaterDetailTiling2));
		Shader.SetGlobalVector(Lighting._WaterDistortionParameters, new Vector4(this.Settings.WaterDistortionScaleStart, this.Settings.WaterDistortionScaleEnd, this.Settings.WaterDepthColorOpacityStart, this.Settings.WaterDepthColorOpacityEnd));
		Shader.SetGlobalVector(Lighting._BloomParameters, new Vector4(this.Settings.BloomScale, 0f, 0f, 0f));
		Shader.SetGlobalVector(Lighting._LiquidParameters2, new Vector4(this.Settings.LiquidMin, this.Settings.LiquidMax, this.Settings.LiquidCutoff, this.Settings.LiquidTransparency));
		Shader.SetGlobalVector(Lighting._GridParameters, new Vector4(this.Settings.GridLineWidth, this.Settings.GridSize, this.Settings.GridMinIntensity, this.Settings.GridMaxIntensity));
		Shader.SetGlobalColor(Lighting._GridColor, this.Settings.GridColor);
		Shader.SetGlobalVector(Lighting._EdgeGlowParameters, new Vector4(this.Settings.EdgeGlowCutoffStart, this.Settings.EdgeGlowCutoffEnd, this.Settings.EdgeGlowIntensity, 0f));
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, Vector4.one);
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, Vector4.one);
		}
		else
		{
			Shader.SetGlobalVector(Lighting._SubstanceParameters, new Vector4(this.Settings.substanceEdgeParameters.intensity, this.Settings.substanceEdgeParameters.edgeIntensity, this.Settings.substanceEdgeParameters.directSunlightScale, this.Settings.substanceEdgeParameters.power));
			Shader.SetGlobalVector(Lighting._TileEdgeParameters, new Vector4(this.Settings.tileEdgeParameters.intensity, this.Settings.tileEdgeParameters.edgeIntensity, this.Settings.tileEdgeParameters.directSunlightScale, this.Settings.tileEdgeParameters.power));
		}
		float w = (SimDebugView.Instance != null && SimDebugView.Instance.GetMode() == OverlayModes.Disease.ID) ? 1f : 0f;
		if (this.disableLighting)
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(1f, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		else
		{
			Shader.SetGlobalVector(Lighting._AnimParameters, new Vector4(this.Settings.AnimIntensity, this.Settings.WorldZoneAnimBlend, 0f, w));
		}
		Shader.SetGlobalVector(Lighting._GasOpacity, new Vector4(this.Settings.GasMinOpacity, this.Settings.GasMaxOpacity, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DarkenTintBackground, this.Settings.DarkenTints[0]);
		Shader.SetGlobalColor(Lighting._DarkenTintMidground, this.Settings.DarkenTints[1]);
		Shader.SetGlobalColor(Lighting._DarkenTintForeground, this.Settings.DarkenTints[2]);
		Shader.SetGlobalColor(Lighting._BrightenOverlay, this.Settings.BrightenOverlayColour);
		Shader.SetGlobalColor(Lighting._ColdFG, this.PremultiplyAlpha(this.Settings.ColdColours[2]));
		Shader.SetGlobalColor(Lighting._ColdMG, this.PremultiplyAlpha(this.Settings.ColdColours[1]));
		Shader.SetGlobalColor(Lighting._ColdBG, this.PremultiplyAlpha(this.Settings.ColdColours[0]));
		Shader.SetGlobalColor(Lighting._HotFG, this.PremultiplyAlpha(this.Settings.HotColours[2]));
		Shader.SetGlobalColor(Lighting._HotMG, this.PremultiplyAlpha(this.Settings.HotColours[1]));
		Shader.SetGlobalColor(Lighting._HotBG, this.PremultiplyAlpha(this.Settings.HotColours[0]));
		Shader.SetGlobalVector(Lighting._TemperatureParallax, this.Settings.TemperatureParallax);
		Shader.SetGlobalVector(Lighting._ColdUVOffset1, new Vector4(this.Settings.ColdBGUVOffset.x, this.Settings.ColdBGUVOffset.y, this.Settings.ColdMGUVOffset.x, this.Settings.ColdMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._ColdUVOffset2, new Vector4(this.Settings.ColdFGUVOffset.x, this.Settings.ColdFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalVector(Lighting._HotUVOffset1, new Vector4(this.Settings.HotBGUVOffset.x, this.Settings.HotBGUVOffset.y, this.Settings.HotMGUVOffset.x, this.Settings.HotMGUVOffset.y));
		Shader.SetGlobalVector(Lighting._HotUVOffset2, new Vector4(this.Settings.HotFGUVOffset.x, this.Settings.HotFGUVOffset.y, 0f, 0f));
		Shader.SetGlobalColor(Lighting._DustColour, this.PremultiplyAlpha(this.Settings.DustColour));
		Shader.SetGlobalVector(Lighting._DustInfo, new Vector4(this.Settings.DustScale, this.Settings.DustMovement.x, this.Settings.DustMovement.y, this.Settings.DustMovement.z));
		Shader.SetGlobalTexture(Lighting._DustTex, this.Settings.DustTex);
		Shader.SetGlobalVector(Lighting._DebugShowInfo, new Vector4(this.Settings.ShowDust, this.Settings.ShowGas, this.Settings.ShowShadow, this.Settings.ShowTemperature));
		Shader.SetGlobalVector(Lighting._HeatHazeParameters, this.Settings.HeatHazeParameters);
		Shader.SetGlobalTexture(Lighting._HeatHazeTexture, this.Settings.HeatHazeTexture);
		Shader.SetGlobalVector(Lighting._ShineParams, new Vector4(this.Settings.ShineCenter.x, this.Settings.ShineCenter.y, this.Settings.ShineRange.x, this.Settings.ShineRange.y));
		Shader.SetGlobalVector(Lighting._ShineParams2, new Vector4(this.Settings.ShineZoomSpeed, 0f, 0f, 0f));
		Shader.SetGlobalFloat(Lighting._WorldZoneGasBlend, this.Settings.WorldZoneGasBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneLiquidBlend, this.Settings.WorldZoneLiquidBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneForegroundBlend, this.Settings.WorldZoneForegroundBlend);
		Shader.SetGlobalFloat(Lighting._WorldZoneSimpleAnimBlend, this.Settings.WorldZoneSimpleAnimBlend);
		Shader.SetGlobalColor(Lighting._CharacterLitColour, this.Settings.characterLighting.litColour);
		Shader.SetGlobalColor(Lighting._CharacterUnlitColour, this.Settings.characterLighting.unlitColour);
		Shader.SetGlobalTexture(Lighting._BuildingDamagedTex, this.Settings.BuildingDamagedTex);
		Shader.SetGlobalVector(Lighting._BuildingDamagedUVParameters, this.Settings.BuildingDamagedUVParameters);
		Shader.SetGlobalTexture(Lighting._DiseaseOverlayTex, this.Settings.DiseaseOverlayTex);
		Shader.SetGlobalVector(Lighting._DiseaseOverlayTexInfo, this.Settings.DiseaseOverlayTexInfo);
		if (this.Settings.ShowRadiation)
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, this.PremultiplyAlpha(this.Settings.RadColor));
		}
		else
		{
			Shader.SetGlobalColor(Lighting._RadHazeColor, Color.clear);
		}
		Shader.SetGlobalVector(Lighting._RadUVOffset1, new Vector4(this.Settings.Rad1UVOffset.x, this.Settings.Rad1UVOffset.y, this.Settings.Rad2UVOffset.x, this.Settings.Rad2UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVOffset2, new Vector4(this.Settings.Rad3UVOffset.x, this.Settings.Rad3UVOffset.y, this.Settings.Rad4UVOffset.x, this.Settings.Rad4UVOffset.y));
		Shader.SetGlobalVector(Lighting._RadUVScales, new Vector4(1f / this.Settings.RadUVScales.x, 1f / this.Settings.RadUVScales.y, 1f / this.Settings.RadUVScales.z, 1f / this.Settings.RadUVScales.w));
		Shader.SetGlobalVector(Lighting._RadRange1, new Vector4(this.Settings.Rad1Range.x, this.Settings.Rad1Range.y, this.Settings.Rad2Range.x, this.Settings.Rad2Range.y));
		Shader.SetGlobalVector(Lighting._RadRange2, new Vector4(this.Settings.Rad3Range.x, this.Settings.Rad3Range.y, this.Settings.Rad4Range.x, this.Settings.Rad4Range.y));
		if (LightBuffer.Instance != null && LightBuffer.Instance.Texture != null)
		{
			Shader.SetGlobalTexture(Lighting._LightBufferTex, LightBuffer.Instance.Texture);
		}
	}

	// Token: 0x04005E7D RID: 24189
	public global::LightingSettings Settings;

	// Token: 0x04005E7E RID: 24190
	public static Lighting Instance;

	// Token: 0x04005E7F RID: 24191
	[NonSerialized]
	public bool disableLighting;

	// Token: 0x04005E80 RID: 24192
	private static int _liquidZ = Shader.PropertyToID("_LiquidZ");

	// Token: 0x04005E81 RID: 24193
	private static int _DigMapMapParameters = Shader.PropertyToID("_DigMapMapParameters");

	// Token: 0x04005E82 RID: 24194
	private static int _DigDamageMap = Shader.PropertyToID("_DigDamageMap");

	// Token: 0x04005E83 RID: 24195
	private static int _StateTransitionMap = Shader.PropertyToID("_StateTransitionMap");

	// Token: 0x04005E84 RID: 24196
	private static int _StateTransitionColor = Shader.PropertyToID("_StateTransitionColor");

	// Token: 0x04005E85 RID: 24197
	private static int _StateTransitionParameters = Shader.PropertyToID("_StateTransitionParameters");

	// Token: 0x04005E86 RID: 24198
	private static int _FallingSolidMap = Shader.PropertyToID("_FallingSolidMap");

	// Token: 0x04005E87 RID: 24199
	private static int _FallingSolidColor = Shader.PropertyToID("_FallingSolidColor");

	// Token: 0x04005E88 RID: 24200
	private static int _FallingSolidParameters = Shader.PropertyToID("_FallingSolidParameters");

	// Token: 0x04005E89 RID: 24201
	private static int _WaterTrimColor = Shader.PropertyToID("_WaterTrimColor");

	// Token: 0x04005E8A RID: 24202
	private static int _WaterParameters2 = Shader.PropertyToID("_WaterParameters2");

	// Token: 0x04005E8B RID: 24203
	private static int _WaterWaveParameters = Shader.PropertyToID("_WaterWaveParameters");

	// Token: 0x04005E8C RID: 24204
	private static int _WaterWaveParameters2 = Shader.PropertyToID("_WaterWaveParameters2");

	// Token: 0x04005E8D RID: 24205
	private static int _WaterDetailParameters = Shader.PropertyToID("_WaterDetailParameters");

	// Token: 0x04005E8E RID: 24206
	private static int _WaterDistortionParameters = Shader.PropertyToID("_WaterDistortionParameters");

	// Token: 0x04005E8F RID: 24207
	private static int _BloomParameters = Shader.PropertyToID("_BloomParameters");

	// Token: 0x04005E90 RID: 24208
	private static int _LiquidParameters2 = Shader.PropertyToID("_LiquidParameters2");

	// Token: 0x04005E91 RID: 24209
	private static int _GridParameters = Shader.PropertyToID("_GridParameters");

	// Token: 0x04005E92 RID: 24210
	private static int _GridColor = Shader.PropertyToID("_GridColor");

	// Token: 0x04005E93 RID: 24211
	private static int _EdgeGlowParameters = Shader.PropertyToID("_EdgeGlowParameters");

	// Token: 0x04005E94 RID: 24212
	private static int _SubstanceParameters = Shader.PropertyToID("_SubstanceParameters");

	// Token: 0x04005E95 RID: 24213
	private static int _TileEdgeParameters = Shader.PropertyToID("_TileEdgeParameters");

	// Token: 0x04005E96 RID: 24214
	private static int _AnimParameters = Shader.PropertyToID("_AnimParameters");

	// Token: 0x04005E97 RID: 24215
	private static int _GasOpacity = Shader.PropertyToID("_GasOpacity");

	// Token: 0x04005E98 RID: 24216
	private static int _DarkenTintBackground = Shader.PropertyToID("_DarkenTintBackground");

	// Token: 0x04005E99 RID: 24217
	private static int _DarkenTintMidground = Shader.PropertyToID("_DarkenTintMidground");

	// Token: 0x04005E9A RID: 24218
	private static int _DarkenTintForeground = Shader.PropertyToID("_DarkenTintForeground");

	// Token: 0x04005E9B RID: 24219
	private static int _BrightenOverlay = Shader.PropertyToID("_BrightenOverlay");

	// Token: 0x04005E9C RID: 24220
	private static int _ColdFG = Shader.PropertyToID("_ColdFG");

	// Token: 0x04005E9D RID: 24221
	private static int _ColdMG = Shader.PropertyToID("_ColdMG");

	// Token: 0x04005E9E RID: 24222
	private static int _ColdBG = Shader.PropertyToID("_ColdBG");

	// Token: 0x04005E9F RID: 24223
	private static int _HotFG = Shader.PropertyToID("_HotFG");

	// Token: 0x04005EA0 RID: 24224
	private static int _HotMG = Shader.PropertyToID("_HotMG");

	// Token: 0x04005EA1 RID: 24225
	private static int _HotBG = Shader.PropertyToID("_HotBG");

	// Token: 0x04005EA2 RID: 24226
	private static int _TemperatureParallax = Shader.PropertyToID("_TemperatureParallax");

	// Token: 0x04005EA3 RID: 24227
	private static int _ColdUVOffset1 = Shader.PropertyToID("_ColdUVOffset1");

	// Token: 0x04005EA4 RID: 24228
	private static int _ColdUVOffset2 = Shader.PropertyToID("_ColdUVOffset2");

	// Token: 0x04005EA5 RID: 24229
	private static int _HotUVOffset1 = Shader.PropertyToID("_HotUVOffset1");

	// Token: 0x04005EA6 RID: 24230
	private static int _HotUVOffset2 = Shader.PropertyToID("_HotUVOffset2");

	// Token: 0x04005EA7 RID: 24231
	private static int _DustColour = Shader.PropertyToID("_DustColour");

	// Token: 0x04005EA8 RID: 24232
	private static int _DustInfo = Shader.PropertyToID("_DustInfo");

	// Token: 0x04005EA9 RID: 24233
	private static int _DustTex = Shader.PropertyToID("_DustTex");

	// Token: 0x04005EAA RID: 24234
	private static int _DebugShowInfo = Shader.PropertyToID("_DebugShowInfo");

	// Token: 0x04005EAB RID: 24235
	private static int _HeatHazeParameters = Shader.PropertyToID("_HeatHazeParameters");

	// Token: 0x04005EAC RID: 24236
	private static int _HeatHazeTexture = Shader.PropertyToID("_HeatHazeTexture");

	// Token: 0x04005EAD RID: 24237
	private static int _ShineParams = Shader.PropertyToID("_ShineParams");

	// Token: 0x04005EAE RID: 24238
	private static int _ShineParams2 = Shader.PropertyToID("_ShineParams2");

	// Token: 0x04005EAF RID: 24239
	private static int _WorldZoneGasBlend = Shader.PropertyToID("_WorldZoneGasBlend");

	// Token: 0x04005EB0 RID: 24240
	private static int _WorldZoneLiquidBlend = Shader.PropertyToID("_WorldZoneLiquidBlend");

	// Token: 0x04005EB1 RID: 24241
	private static int _WorldZoneForegroundBlend = Shader.PropertyToID("_WorldZoneForegroundBlend");

	// Token: 0x04005EB2 RID: 24242
	private static int _WorldZoneSimpleAnimBlend = Shader.PropertyToID("_WorldZoneSimpleAnimBlend");

	// Token: 0x04005EB3 RID: 24243
	private static int _CharacterLitColour = Shader.PropertyToID("_CharacterLitColour");

	// Token: 0x04005EB4 RID: 24244
	private static int _CharacterUnlitColour = Shader.PropertyToID("_CharacterUnlitColour");

	// Token: 0x04005EB5 RID: 24245
	private static int _BuildingDamagedTex = Shader.PropertyToID("_BuildingDamagedTex");

	// Token: 0x04005EB6 RID: 24246
	private static int _BuildingDamagedUVParameters = Shader.PropertyToID("_BuildingDamagedUVParameters");

	// Token: 0x04005EB7 RID: 24247
	private static int _DiseaseOverlayTex = Shader.PropertyToID("_DiseaseOverlayTex");

	// Token: 0x04005EB8 RID: 24248
	private static int _DiseaseOverlayTexInfo = Shader.PropertyToID("_DiseaseOverlayTexInfo");

	// Token: 0x04005EB9 RID: 24249
	private static int _RadHazeColor = Shader.PropertyToID("_RadHazeColor");

	// Token: 0x04005EBA RID: 24250
	private static int _RadUVOffset1 = Shader.PropertyToID("_RadUVOffset1");

	// Token: 0x04005EBB RID: 24251
	private static int _RadUVOffset2 = Shader.PropertyToID("_RadUVOffset2");

	// Token: 0x04005EBC RID: 24252
	private static int _RadUVScales = Shader.PropertyToID("_RadUVScales");

	// Token: 0x04005EBD RID: 24253
	private static int _RadRange1 = Shader.PropertyToID("_RadRange1");

	// Token: 0x04005EBE RID: 24254
	private static int _RadRange2 = Shader.PropertyToID("_RadRange2");

	// Token: 0x04005EBF RID: 24255
	private static int _LightBufferTex = Shader.PropertyToID("_LightBufferTex");
}
