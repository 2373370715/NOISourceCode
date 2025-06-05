using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x0200096E RID: 2414
[AddComponentMenu("KMonoBehaviour/scripts/AmbienceManager")]
public class AmbienceManager : KMonoBehaviour
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06002B1F RID: 11039 RVA: 0x000C074A File Offset: 0x000BE94A
	// (set) Token: 0x06002B1E RID: 11038 RVA: 0x000C0742 File Offset: 0x000BE942
	public static float BoilingTreshold { get; private set; } = 1f;

	// Token: 0x06002B20 RID: 11040 RVA: 0x001EA22C File Offset: 0x001E842C
	protected override void OnSpawn()
	{
		if (!RuntimeManager.IsInitialized)
		{
			base.enabled = false;
			return;
		}
		AmbienceManager.BoilingTreshold = this.LiquidMaterial.GetFloat("_BoilingTreshold");
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			this.quadrants[i] = new AmbienceManager.Quadrant(this.quadrantDefs[i]);
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x001EA288 File Offset: 0x001E8488
	protected override void OnForcedCleanUp()
	{
		AmbienceManager.Quadrant[] array = this.quadrants;
		for (int i = 0; i < array.Length; i++)
		{
			foreach (AmbienceManager.Layer layer in array[i].GetAllLayers())
			{
				layer.Stop();
			}
		}
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x001EA2F0 File Offset: 0x001E84F0
	private void LateUpdate()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = visibleArea.Min;
		Vector2I max = visibleArea.Max;
		Vector2I vector2I = min + (max - min) / 2;
		Vector3 a = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = vector + (a - vector) / 2f;
		Vector3 vector3 = a - vector;
		if (vector3.x > vector3.y)
		{
			vector3.y = vector3.x;
		}
		else
		{
			vector3.x = vector3.y;
		}
		a = vector2 + vector3 / 2f;
		vector = vector2 - vector3 / 2f;
		Vector3 vector4 = vector3 / 2f / 2f;
		this.quadrants[0].Update(new Vector2I(min.x, min.y), new Vector2I(vector2I.x, vector2I.y), new Vector3(vector.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[1].Update(new Vector2I(vector2I.x, min.y), new Vector2I(max.x, vector2I.y), new Vector3(vector2.x + vector4.x, vector.y + vector4.y, this.emitterZPosition));
		this.quadrants[2].Update(new Vector2I(min.x, vector2I.y), new Vector2I(vector2I.x, max.y), new Vector3(vector.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		this.quadrants[3].Update(new Vector2I(vector2I.x, vector2I.y), new Vector2I(max.x, max.y), new Vector3(vector2.x + vector4.x, vector2.y + vector4.y, this.emitterZPosition));
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (int i = 0; i < this.quadrants.Length; i++)
		{
			num += (float)this.quadrants[i].spaceLayer.tileCount;
			num2 += (float)this.quadrants[i].facilityLayer.tileCount;
			num3 += (float)this.quadrants[i].totalTileCount;
		}
		AudioMixer.instance.UpdateSpaceVisibleSnapshot(num / num3);
		AudioMixer.instance.UpdateFacilityVisibleSnapshot(num2 / num3);
	}

	// Token: 0x04001D39 RID: 7481
	public Material LiquidMaterial;

	// Token: 0x04001D3B RID: 7483
	private float emitterZPosition;

	// Token: 0x04001D3C RID: 7484
	public AmbienceManager.QuadrantDef[] quadrantDefs;

	// Token: 0x04001D3D RID: 7485
	public AmbienceManager.Quadrant[] quadrants = new AmbienceManager.Quadrant[4];

	// Token: 0x0200096F RID: 2415
	public class Tuning : TuningData<AmbienceManager.Tuning>
	{
		// Token: 0x04001D3E RID: 7486
		public int backwallTileValue = 1;

		// Token: 0x04001D3F RID: 7487
		public int foundationTileValue = 2;

		// Token: 0x04001D40 RID: 7488
		public int buildingTileValue = 3;
	}

	// Token: 0x02000970 RID: 2416
	public class LiquidLayer : AmbienceManager.Layer
	{
		// Token: 0x06002B26 RID: 11046 RVA: 0x000C078E File Offset: 0x000BE98E
		public LiquidLayer(EventReference sound, EventReference one_shot_sound = default(EventReference)) : base(sound, one_shot_sound)
		{
		}

		// Token: 0x06002B27 RID: 11047 RVA: 0x000C0798 File Offset: 0x000BE998
		public override void Reset()
		{
			base.Reset();
			this.boilingTileCount = 0;
			this.averageBoilIntensity = 0f;
		}

		// Token: 0x06002B28 RID: 11048 RVA: 0x000C07B2 File Offset: 0x000BE9B2
		public override void UpdatePercentage(int cell_count)
		{
			base.UpdatePercentage(cell_count);
			this.boilTilePercentage = (float)this.boilingTileCount / (float)cell_count;
		}

		// Token: 0x06002B29 RID: 11049 RVA: 0x000C07CB File Offset: 0x000BE9CB
		public override void UpdateParameters(Vector3 emitter_position)
		{
			base.UpdateParameters(emitter_position);
			this.soundEvent.setParameterByName("Boiling_Tile_Percentage", this.boilTilePercentage, false);
		}

		// Token: 0x06002B2A RID: 11050 RVA: 0x000C07EC File Offset: 0x000BE9EC
		public override void UpdateAverageTemperature()
		{
			base.UpdateAverageTemperature();
			this.UpdateAverageBoilIntensity();
		}

		// Token: 0x06002B2B RID: 11051 RVA: 0x000C07FA File Offset: 0x000BE9FA
		public void UpdateAverageBoilIntensity()
		{
			this.averageBoilIntensity = ((this.tileCount > 0) ? (this.averageBoilIntensity / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("Boiling_Intensity", this.averageBoilIntensity, false);
		}

		// Token: 0x04001D41 RID: 7489
		private const string BOILING_INTENSITY_ID = "Boiling_Intensity";

		// Token: 0x04001D42 RID: 7490
		private const string BOILING_TILE_PERCENTAGE_ID = "Boiling_Tile_Percentage";

		// Token: 0x04001D43 RID: 7491
		public int boilingTileCount;

		// Token: 0x04001D44 RID: 7492
		public float boilTilePercentage;

		// Token: 0x04001D45 RID: 7493
		public float averageBoilIntensity;
	}

	// Token: 0x02000971 RID: 2417
	public class Layer : IComparable<AmbienceManager.Layer>
	{
		// Token: 0x06002B2C RID: 11052 RVA: 0x000C0838 File Offset: 0x000BEA38
		public Layer(EventReference sound, EventReference one_shot_sound = default(EventReference))
		{
			this.sound = sound;
			this.oneShotSound = one_shot_sound;
		}

		// Token: 0x06002B2D RID: 11053 RVA: 0x000C084E File Offset: 0x000BEA4E
		public virtual void Reset()
		{
			this.tileCount = 0;
			this.averageTemperature = 0f;
			this.averageRadiation = 0f;
		}

		// Token: 0x06002B2E RID: 11054 RVA: 0x000C086D File Offset: 0x000BEA6D
		public virtual void UpdatePercentage(int cell_count)
		{
			this.tilePercentage = (float)this.tileCount / (float)cell_count;
		}

		// Token: 0x06002B2F RID: 11055 RVA: 0x000C087F File Offset: 0x000BEA7F
		public virtual void UpdateAverageTemperature()
		{
			this.averageTemperature /= (float)this.tileCount;
			this.soundEvent.setParameterByName("averageTemperature", this.averageTemperature, false);
		}

		// Token: 0x06002B30 RID: 11056 RVA: 0x000C08AD File Offset: 0x000BEAAD
		public void UpdateAverageRadiation()
		{
			this.averageRadiation = ((this.tileCount > 0) ? (this.averageRadiation / (float)this.tileCount) : 0f);
			this.soundEvent.setParameterByName("averageRadiation", this.averageRadiation, false);
		}

		// Token: 0x06002B31 RID: 11057 RVA: 0x001EA60C File Offset: 0x001E880C
		public virtual void UpdateParameters(Vector3 emitter_position)
		{
			if (!this.soundEvent.isValid())
			{
				return;
			}
			Vector3 pos = new Vector3(emitter_position.x, emitter_position.y, 0f);
			this.soundEvent.set3DAttributes(pos.To3DAttributes());
			this.soundEvent.setParameterByName("tilePercentage", this.tilePercentage, false);
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x000C08EB File Offset: 0x000BEAEB
		public void SetCustomParameter(string parameterName, float value)
		{
			this.soundEvent.setParameterByName(parameterName, value, false);
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x000C08FC File Offset: 0x000BEAFC
		public int CompareTo(AmbienceManager.Layer layer)
		{
			return layer.tileCount - this.tileCount;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x000C090B File Offset: 0x000BEB0B
		public void SetVolume(float volume)
		{
			if (this.volume != volume)
			{
				this.volume = volume;
				if (this.soundEvent.isValid())
				{
					this.soundEvent.setVolume(volume);
				}
			}
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x000C0937 File Offset: 0x000BEB37
		public void Stop()
		{
			if (this.soundEvent.isValid())
			{
				this.soundEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
				this.soundEvent.release();
			}
			this.isRunning = false;
		}

		// Token: 0x06002B36 RID: 11062 RVA: 0x001EA66C File Offset: 0x001E886C
		public void Start(Vector3 emitter_position)
		{
			if (!this.isRunning)
			{
				if (!this.oneShotSound.IsNull)
				{
					EventInstance eventInstance = KFMOD.CreateInstance(this.oneShotSound);
					if (!eventInstance.isValid())
					{
						string str = "Could not find event: ";
						EventReference eventReference = this.oneShotSound;
						global::Debug.LogWarning(str + eventReference.ToString());
						return;
					}
					ATTRIBUTES_3D attributes = new Vector3(emitter_position.x, emitter_position.y, 0f).To3DAttributes();
					eventInstance.set3DAttributes(attributes);
					eventInstance.setVolume(this.tilePercentage * 2f);
					eventInstance.start();
					eventInstance.release();
					return;
				}
				else
				{
					this.soundEvent = KFMOD.CreateInstance(this.sound);
					if (this.soundEvent.isValid())
					{
						this.soundEvent.start();
					}
					this.isRunning = true;
				}
			}
		}

		// Token: 0x04001D46 RID: 7494
		private const string TILE_PERCENTAGE_ID = "tilePercentage";

		// Token: 0x04001D47 RID: 7495
		private const string AVERAGE_TEMPERATURE_ID = "averageTemperature";

		// Token: 0x04001D48 RID: 7496
		private const string AVERAGE_RADIATION_ID = "averageRadiation";

		// Token: 0x04001D49 RID: 7497
		public EventReference sound;

		// Token: 0x04001D4A RID: 7498
		public EventReference oneShotSound;

		// Token: 0x04001D4B RID: 7499
		public int tileCount;

		// Token: 0x04001D4C RID: 7500
		public float tilePercentage;

		// Token: 0x04001D4D RID: 7501
		public float volume;

		// Token: 0x04001D4E RID: 7502
		public bool isRunning;

		// Token: 0x04001D4F RID: 7503
		protected EventInstance soundEvent;

		// Token: 0x04001D50 RID: 7504
		public float averageTemperature;

		// Token: 0x04001D51 RID: 7505
		public float averageRadiation;
	}

	// Token: 0x02000972 RID: 2418
	[Serializable]
	public class QuadrantDef
	{
		// Token: 0x04001D52 RID: 7506
		public string name;

		// Token: 0x04001D53 RID: 7507
		public EventReference[] liquidSounds;

		// Token: 0x04001D54 RID: 7508
		public EventReference[] gasSounds;

		// Token: 0x04001D55 RID: 7509
		public EventReference[] solidSounds;

		// Token: 0x04001D56 RID: 7510
		public EventReference fogSound;

		// Token: 0x04001D57 RID: 7511
		public EventReference spaceSound;

		// Token: 0x04001D58 RID: 7512
		public EventReference rocketInteriorSound;

		// Token: 0x04001D59 RID: 7513
		public EventReference facilitySound;

		// Token: 0x04001D5A RID: 7514
		public EventReference radiationSound;
	}

	// Token: 0x02000973 RID: 2419
	public class Quadrant
	{
		// Token: 0x06002B38 RID: 11064 RVA: 0x001EA748 File Offset: 0x001E8948
		public Quadrant(AmbienceManager.QuadrantDef def)
		{
			this.name = def.name;
			this.fogLayer = new AmbienceManager.Layer(def.fogSound, default(EventReference));
			this.allLayers.Add(this.fogLayer);
			this.loopingLayers.Add(this.fogLayer);
			this.spaceLayer = new AmbienceManager.Layer(def.spaceSound, default(EventReference));
			this.allLayers.Add(this.spaceLayer);
			this.loopingLayers.Add(this.spaceLayer);
			this.m_isClusterSpaceEnabled = DlcManager.FeatureClusterSpaceEnabled();
			if (this.m_isClusterSpaceEnabled)
			{
				this.rocketInteriorLayer = new AmbienceManager.Layer(def.rocketInteriorSound, default(EventReference));
				this.allLayers.Add(this.rocketInteriorLayer);
			}
			this.facilityLayer = new AmbienceManager.Layer(def.facilitySound, default(EventReference));
			this.allLayers.Add(this.facilityLayer);
			this.loopingLayers.Add(this.facilityLayer);
			this.m_isRadiationEnabled = Sim.IsRadiationEnabled();
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer = new AmbienceManager.Layer(def.radiationSound, default(EventReference));
				this.allLayers.Add(this.radiationLayer);
			}
			for (int i = 0; i < 4; i++)
			{
				this.gasLayers[i] = new AmbienceManager.Layer(def.gasSounds[i], default(EventReference));
				this.liquidLayers[i] = new AmbienceManager.LiquidLayer(def.liquidSounds[i], default(EventReference));
				this.allLayers.Add(this.gasLayers[i]);
				this.allLayers.Add(this.liquidLayers[i]);
				this.loopingLayers.Add(this.gasLayers[i]);
				this.loopingLayers.Add(this.liquidLayers[i]);
			}
			for (int j = 0; j < this.solidLayers.Length; j++)
			{
				if (j >= def.solidSounds.Length)
				{
					string str = "Missing solid layer: ";
					SolidAmbienceType solidAmbienceType = (SolidAmbienceType)j;
					global::Debug.LogError(str + solidAmbienceType.ToString());
				}
				this.solidLayers[j] = new AmbienceManager.Layer(default(EventReference), def.solidSounds[j]);
				this.allLayers.Add(this.solidLayers[j]);
				this.oneShotLayers.Add(this.solidLayers[j]);
			}
			this.solidTimers = new AmbienceManager.Quadrant.SolidTimer[AmbienceManager.Quadrant.activeSolidLayerCount];
			for (int k = 0; k < AmbienceManager.Quadrant.activeSolidLayerCount; k++)
			{
				this.solidTimers[k] = new AmbienceManager.Quadrant.SolidTimer();
			}
		}

		// Token: 0x06002B39 RID: 11065 RVA: 0x001EAA40 File Offset: 0x001E8C40
		public void Update(Vector2I min, Vector2I max, Vector3 emitter_position)
		{
			this.emitterPosition = emitter_position;
			this.totalTileCount = 0;
			for (int i = 0; i < this.allLayers.Count; i++)
			{
				this.allLayers[i].Reset();
			}
			float num = 1f - AmbienceManager.BoilingTreshold;
			for (int j = min.y; j < max.y; j++)
			{
				if (j % 2 != 1)
				{
					for (int k = min.x; k < max.x; k++)
					{
						if (k % 2 != 0)
						{
							int num2 = Grid.XYToCell(k, j);
							if (Grid.IsValidCell(num2))
							{
								this.totalTileCount++;
								if (Grid.IsVisible(num2))
								{
									if (Grid.GravitasFacility[num2])
									{
										this.facilityLayer.tileCount += 8;
									}
									else
									{
										Element element = Grid.Element[num2];
										if (element != null)
										{
											if (element.IsLiquid && Grid.IsSubstantialLiquid(num2, 0.35f))
											{
												AmbienceType ambience = element.substance.GetAmbience();
												if (ambience != AmbienceType.None)
												{
													this.liquidLayers[(int)ambience].tileCount++;
													this.liquidLayers[(int)ambience].averageTemperature += Grid.Temperature[num2];
													float num3 = Mathf.Clamp01(element.GetRelativeHeatLevel(Grid.Temperature[num2]) - AmbienceManager.BoilingTreshold) / num;
													this.liquidLayers[(int)ambience].boilingTileCount += ((num3 > 0f) ? 1 : 0);
													this.liquidLayers[(int)ambience].averageBoilIntensity += num3;
												}
											}
											else if (element.IsGas)
											{
												AmbienceType ambience2 = element.substance.GetAmbience();
												if (ambience2 != AmbienceType.None)
												{
													this.gasLayers[(int)ambience2].tileCount++;
													this.gasLayers[(int)ambience2].averageTemperature += Grid.Temperature[num2];
												}
											}
											else if (element.IsSolid)
											{
												SolidAmbienceType solidAmbienceType = element.substance.GetSolidAmbience();
												if (Grid.Foundation[num2])
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().foundationTileValue;
												}
												else if (Grid.Objects[num2, 2] != null)
												{
													solidAmbienceType = SolidAmbienceType.Tile;
													this.solidLayers[(int)solidAmbienceType].tileCount += TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().backwallTileValue;
												}
												else if (solidAmbienceType != SolidAmbienceType.None)
												{
													this.solidLayers[(int)solidAmbienceType].tileCount++;
												}
												else if (element.id == SimHashes.Regolith || element.id == SimHashes.MaficRock)
												{
													this.spaceLayer.tileCount++;
												}
											}
											else if (element.id == SimHashes.Vacuum && CellSelectionObject.IsExposedToSpace(num2))
											{
												if (Grid.Objects[num2, 1] != null)
												{
													this.spaceLayer.tileCount -= TuningData<AmbienceManager.Tuning>.Get().buildingTileValue;
												}
												this.spaceLayer.tileCount++;
											}
										}
									}
									if (Grid.Radiation[num2] > 0f)
									{
										this.radiationLayer.averageRadiation += Grid.Radiation[num2];
										this.radiationLayer.tileCount++;
									}
								}
								else
								{
									this.fogLayer.tileCount++;
								}
							}
						}
					}
				}
			}
			Vector2I vector2I = max - min;
			int cell_count = vector2I.x * vector2I.y;
			for (int l = 0; l < this.allLayers.Count; l++)
			{
				this.allLayers[l].UpdatePercentage(cell_count);
			}
			this.loopingLayers.Sort();
			this.topLayers.Clear();
			for (int m = 0; m < this.loopingLayers.Count; m++)
			{
				AmbienceManager.Layer layer = this.loopingLayers[m];
				if (m < 3 && layer.tilePercentage > 0f)
				{
					layer.Start(emitter_position);
					layer.UpdateAverageTemperature();
					layer.UpdateParameters(emitter_position);
					this.topLayers.Add(layer);
				}
				else
				{
					layer.Stop();
				}
			}
			if (this.m_isClusterSpaceEnabled)
			{
				float volume = 0f;
				if (ClusterManager.Instance != null && ClusterManager.Instance.activeWorld != null && ClusterManager.Instance.activeWorld.IsModuleInterior)
				{
					volume = 1f;
				}
				this.rocketInteriorLayer.Start(emitter_position);
				this.rocketInteriorLayer.SetCustomParameter("RocketState", (float)ClusterManager.RocketInteriorState);
				this.rocketInteriorLayer.SetVolume(volume);
			}
			if (this.m_isRadiationEnabled)
			{
				this.radiationLayer.Start(emitter_position);
				this.radiationLayer.UpdateAverageRadiation();
				this.radiationLayer.UpdateParameters(emitter_position);
			}
			this.oneShotLayers.Sort();
			for (int n = 0; n < AmbienceManager.Quadrant.activeSolidLayerCount; n++)
			{
				if (this.solidTimers[n].ShouldPlay() && this.oneShotLayers[n].tilePercentage > 0f)
				{
					this.oneShotLayers[n].Start(emitter_position);
				}
			}
		}

		// Token: 0x06002B3A RID: 11066 RVA: 0x000C0966 File Offset: 0x000BEB66
		public List<AmbienceManager.Layer> GetAllLayers()
		{
			return this.allLayers;
		}

		// Token: 0x04001D5B RID: 7515
		public string name;

		// Token: 0x04001D5C RID: 7516
		public Vector3 emitterPosition;

		// Token: 0x04001D5D RID: 7517
		public AmbienceManager.Layer[] gasLayers = new AmbienceManager.Layer[4];

		// Token: 0x04001D5E RID: 7518
		public AmbienceManager.LiquidLayer[] liquidLayers = new AmbienceManager.LiquidLayer[4];

		// Token: 0x04001D5F RID: 7519
		public AmbienceManager.Layer fogLayer;

		// Token: 0x04001D60 RID: 7520
		public AmbienceManager.Layer spaceLayer;

		// Token: 0x04001D61 RID: 7521
		public AmbienceManager.Layer rocketInteriorLayer;

		// Token: 0x04001D62 RID: 7522
		public AmbienceManager.Layer facilityLayer;

		// Token: 0x04001D63 RID: 7523
		public AmbienceManager.Layer radiationLayer;

		// Token: 0x04001D64 RID: 7524
		public AmbienceManager.Layer[] solidLayers = new AmbienceManager.Layer[20];

		// Token: 0x04001D65 RID: 7525
		private List<AmbienceManager.Layer> allLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001D66 RID: 7526
		private List<AmbienceManager.Layer> loopingLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001D67 RID: 7527
		private List<AmbienceManager.Layer> oneShotLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001D68 RID: 7528
		private List<AmbienceManager.Layer> topLayers = new List<AmbienceManager.Layer>();

		// Token: 0x04001D69 RID: 7529
		public static int activeSolidLayerCount = 2;

		// Token: 0x04001D6A RID: 7530
		public int totalTileCount;

		// Token: 0x04001D6B RID: 7531
		private bool m_isRadiationEnabled;

		// Token: 0x04001D6C RID: 7532
		private bool m_isClusterSpaceEnabled;

		// Token: 0x04001D6D RID: 7533
		private const string ROCKET_STATE_FOR_AMBIENCE = "RocketState";

		// Token: 0x04001D6E RID: 7534
		private AmbienceManager.Quadrant.SolidTimer[] solidTimers;

		// Token: 0x02000974 RID: 2420
		public class SolidTimer
		{
			// Token: 0x06002B3C RID: 11068 RVA: 0x000C0976 File Offset: 0x000BEB76
			public SolidTimer()
			{
				this.solidTargetTime = Time.unscaledTime + UnityEngine.Random.value * AmbienceManager.Quadrant.SolidTimer.solidMinTime;
			}

			// Token: 0x06002B3D RID: 11069 RVA: 0x000C0995 File Offset: 0x000BEB95
			public bool ShouldPlay()
			{
				if (Time.unscaledTime > this.solidTargetTime)
				{
					this.solidTargetTime = Time.unscaledTime + AmbienceManager.Quadrant.SolidTimer.solidMinTime + UnityEngine.Random.value * (AmbienceManager.Quadrant.SolidTimer.solidMaxTime - AmbienceManager.Quadrant.SolidTimer.solidMinTime);
					return true;
				}
				return false;
			}

			// Token: 0x04001D6F RID: 7535
			public static float solidMinTime = 9f;

			// Token: 0x04001D70 RID: 7536
			public static float solidMaxTime = 15f;

			// Token: 0x04001D71 RID: 7537
			public float solidTargetTime;
		}
	}
}
