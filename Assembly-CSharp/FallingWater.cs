using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using FMOD.Studio;
using FMODUnity;
using KSerialization;
using UnityEngine;

// Token: 0x02001320 RID: 4896
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/FallingWater")]
public class FallingWater : KMonoBehaviour, ISim200ms
{
	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x06006435 RID: 25653 RVA: 0x000E5DEA File Offset: 0x000E3FEA
	// (set) Token: 0x06006436 RID: 25654 RVA: 0x000AA038 File Offset: 0x000A8238
	public static FallingWater instance
	{
		get
		{
			return FallingWater._instance;
		}
		private set
		{
		}
	}

	// Token: 0x06006437 RID: 25655 RVA: 0x000E5DF1 File Offset: 0x000E3FF1
	public static void DestroyInstance()
	{
		FallingWater._instance = null;
	}

	// Token: 0x06006438 RID: 25656 RVA: 0x000E5DF9 File Offset: 0x000E3FF9
	protected override void OnPrefabInit()
	{
		FallingWater._instance = this;
		base.OnPrefabInit();
		this.mistEffect.SetActive(false);
		this.mistPool = new GameObjectPool(new Func<GameObject>(this.InstantiateMist), 16);
	}

	// Token: 0x06006439 RID: 25657 RVA: 0x002CB0BC File Offset: 0x002C92BC
	protected override void OnSpawn()
	{
		this.mesh = new Mesh();
		this.mesh.MarkDynamic();
		this.mesh.name = "FallingWater";
		this.lastSpawnTime = new float[Grid.WidthInCells * Grid.HeightInCells];
		for (int i = 0; i < this.lastSpawnTime.Length; i++)
		{
			this.lastSpawnTime[i] = 0f;
		}
		this.propertyBlock = new MaterialPropertyBlock();
		this.propertyBlock.SetTexture("_MainTex", this.texture);
		this.uvFrameSize = new Vector2(1f / (float)this.numFrames, 1f);
	}

	// Token: 0x0600643A RID: 25658 RVA: 0x000E5E2C File Offset: 0x000E402C
	protected override void OnCleanUp()
	{
		FallingWater.instance = null;
		base.OnCleanUp();
	}

	// Token: 0x0600643B RID: 25659 RVA: 0x000E5E3A File Offset: 0x000E403A
	private float GetTime()
	{
		return Time.time % 360f;
	}

	// Token: 0x0600643C RID: 25660 RVA: 0x002CB164 File Offset: 0x002C9364
	public void AddParticle(int cell, ushort elementIdx, float base_mass, float temperature, byte disease_idx, int base_disease_count, bool skip_sound = false, bool skip_decor = false, bool debug_track = false, bool disable_randomness = false)
	{
		Vector2 root_pos = Grid.CellToPos2D(cell);
		this.AddParticle(root_pos, elementIdx, base_mass, temperature, disease_idx, base_disease_count, skip_sound, skip_decor, debug_track, disable_randomness);
	}

	// Token: 0x0600643D RID: 25661 RVA: 0x002CB194 File Offset: 0x002C9394
	public void AddParticle(Vector2 root_pos, ushort elementIdx, float base_mass, float temperature, byte disease_idx, int base_disease_count, bool skip_sound = false, bool skip_decor = false, bool debug_track = false, bool disable_randomness = false)
	{
		int num = Grid.PosToCell(root_pos);
		if (!Grid.IsValidCell(num))
		{
			return;
		}
		if (temperature <= 0f || base_mass <= 0f)
		{
			global::Debug.LogError(string.Format("Unexpected water mass/temperature values added to the falling water manager T({0}) M({1})", temperature, base_mass));
		}
		float time = this.GetTime();
		if (!skip_sound)
		{
			FallingWater.SoundInfo soundInfo;
			if (!this.topSounds.TryGetValue(num, out soundInfo))
			{
				soundInfo = default(FallingWater.SoundInfo);
				soundInfo.handle = LoopingSoundManager.StartSound(this.liquid_top_loop, root_pos, true, true);
			}
			soundInfo.startTime = time;
			LoopingSoundManager.Get().UpdateSecondParameter(soundInfo.handle, FallingWater.HASH_LIQUIDVOLUME, SoundUtil.GetLiquidVolume(base_mass));
			this.topSounds[num] = soundInfo;
		}
		int num2 = base_disease_count;
		while (base_mass > 0f)
		{
			float num3 = UnityEngine.Random.value * 2f * this.particleMassVariation - this.particleMassVariation;
			float num4 = Mathf.Max(0f, Mathf.Min(base_mass, this.particleMassToSplit + num3));
			float num5 = num4 / base_mass;
			base_mass -= num4;
			int num6 = (int)(num5 * (float)base_disease_count);
			num6 = Mathf.Min(num2, num6);
			num2 = Mathf.Max(0, num2 - num6);
			int frame = UnityEngine.Random.Range(0, this.numFrames);
			Vector2 b = disable_randomness ? Vector2.zero : new Vector2(this.jitterStep * Mathf.Sin(this.offset), this.jitterStep * Mathf.Sin(this.offset + 17f));
			Vector2 b2 = disable_randomness ? Vector2.zero : new Vector2(UnityEngine.Random.Range(-this.multipleOffsetRange.x, this.multipleOffsetRange.x), UnityEngine.Random.Range(-this.multipleOffsetRange.y, this.multipleOffsetRange.y));
			Element element = ElementLoader.elements[(int)elementIdx];
			Vector2 vector = root_pos;
			bool flag = !skip_decor && this.SpawnLiquidTopDecor(time, Grid.CellLeft(num), false, element);
			bool flag2 = !skip_decor && this.SpawnLiquidTopDecor(time, Grid.CellRight(num), true, element);
			Vector2 vector2 = Vector2.ClampMagnitude(this.initialOffset + b + b2, 1f);
			if (flag || flag2)
			{
				if (flag && flag2)
				{
					vector += vector2;
					vector.x += 0.5f;
				}
				else if (flag)
				{
					vector += vector2;
				}
				else
				{
					vector.x += 1f - vector2.x;
					vector.y += vector2.y;
				}
			}
			else
			{
				vector += vector2;
				vector.x += 0.5f;
			}
			int num7 = Grid.PosToCell(vector);
			if ((Grid.Element[num7].state & Element.State.Solid) == Element.State.Solid || (Grid.Properties[num7] & 2) != 0)
			{
				vector.y = Mathf.Floor(vector.y + 1f);
			}
			this.physics.Add(new FallingWater.ParticlePhysics(vector, Vector2.zero, frame, elementIdx, (int)Grid.WorldIdx[num]));
			this.particleProperties.Add(new FallingWater.ParticleProperties(elementIdx, num4, temperature, disease_idx, num6, debug_track));
		}
	}

	// Token: 0x0600643E RID: 25662 RVA: 0x002CB4B4 File Offset: 0x002C96B4
	private bool SpawnLiquidTopDecor(float time, int cell, bool flip, Element element)
	{
		if (Grid.IsValidCell(cell) && Grid.Element[cell] == element)
		{
			Vector3 vector = Grid.CellToPosCBC(cell, Grid.SceneLayer.TileMain);
			if (CameraController.Instance.IsVisiblePos(vector))
			{
				Pair<int, bool> key = new Pair<int, bool>(cell, flip);
				FallingWater.MistInfo mistInfo;
				if (!this.mistAlive.TryGetValue(key, out mistInfo))
				{
					mistInfo = default(FallingWater.MistInfo);
					mistInfo.fx = this.SpawnMist();
					mistInfo.fx.TintColour = element.substance.colour;
					Vector3 position = vector + (flip ? (-Vector3.right) : Vector3.right) * 0.5f;
					mistInfo.fx.transform.SetPosition(position);
					mistInfo.fx.FlipX = flip;
				}
				mistInfo.deathTime = Time.time + this.mistEffectMinAliveTime;
				this.mistAlive[key] = mistInfo;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600643F RID: 25663 RVA: 0x002CB5A0 File Offset: 0x002C97A0
	public void SpawnLiquidSplash(float x, int cell, bool forceSplash = false)
	{
		float time = this.GetTime();
		float num = this.lastSpawnTime[cell];
		if (time - num >= this.minSpawnDelay || forceSplash)
		{
			this.lastSpawnTime[cell] = time;
			Vector2 a = Grid.CellToPos2D(cell);
			a.x = x - 0.5f;
			int num2 = UnityEngine.Random.Range(0, this.liquid_splash.names.Length);
			Vector2 vector = a + new Vector2(this.liquid_splash.offset.x, this.liquid_splash.offset.y);
			SpriteSheetAnimManager.instance.Play(this.liquid_splash.names[num2], new Vector3(vector.x, vector.y, this.renderOffset.z), new Vector2(this.liquid_splash.size.x, this.liquid_splash.size.y), Color.white);
		}
	}

	// Token: 0x06006440 RID: 25664 RVA: 0x002CB698 File Offset: 0x002C9898
	public void UpdateParticles(float dt)
	{
		if (dt <= 0f || this.simUpdateDelay >= 0)
		{
			return;
		}
		this.offset = (this.offset + dt) % 360f;
		int count = this.physics.Count;
		Vector2 b = Physics.gravity * dt * this.gravityScale;
		for (int i = 0; i < count; i++)
		{
			FallingWater.ParticlePhysics particlePhysics = this.physics[i];
			Vector3 vector = particlePhysics.position;
			int num;
			int num2;
			Grid.PosToXY(vector, out num, out num2);
			particlePhysics.velocity += b;
			Vector3 b2 = particlePhysics.velocity * dt;
			Vector3 v = vector + b2;
			particlePhysics.position = v;
			this.physics[i] = particlePhysics;
			int num3;
			int num4;
			Grid.PosToXY(particlePhysics.position, out num3, out num4);
			int num5 = (num2 > num4) ? num2 : num4;
			int num6 = (num2 > num4) ? num4 : num2;
			int j = num5;
			while (j >= num6)
			{
				int num7 = j * Grid.WidthInCells + num;
				int cell = (j + 1) * Grid.WidthInCells + num;
				if (Grid.IsValidCell(num7) && (int)Grid.WorldIdx[num7] != particlePhysics.worldIdx)
				{
					this.RemoveParticle(i, ref count);
					break;
				}
				if (Grid.IsValidCell(num7))
				{
					Element element = Grid.Element[num7];
					Element.State state = element.state & Element.State.Solid;
					bool flag = false;
					if (state == Element.State.Solid || (Grid.Properties[num7] & 2) != 0)
					{
						this.AddToSim(cell, i, ref count);
					}
					else
					{
						switch (state)
						{
						case Element.State.Vacuum:
							if (element.id == SimHashes.Vacuum)
							{
								flag = true;
							}
							else
							{
								this.RemoveParticle(i, ref count);
							}
							break;
						case Element.State.Gas:
							flag = true;
							break;
						case Element.State.Liquid:
						{
							FallingWater.ParticleProperties particleProperties = this.particleProperties[i];
							Element element2 = ElementLoader.elements[(int)particleProperties.elementIdx];
							if (element2.id == element.id)
							{
								if (Grid.Mass[num7] <= element.defaultValues.mass)
								{
									flag = true;
								}
								else
								{
									this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
									this.AddToSim(num7, i, ref count);
								}
							}
							else if (element2.molarMass > element.molarMass)
							{
								flag = true;
							}
							else
							{
								this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
								this.AddToSim(cell, i, ref count);
							}
							break;
						}
						}
					}
					if (!flag)
					{
						break;
					}
					j--;
				}
				else
				{
					if (Grid.IsValidCell(cell))
					{
						this.SpawnLiquidSplash(particlePhysics.position.x, cell, false);
						this.AddToSim(cell, i, ref count);
						break;
					}
					this.RemoveParticle(i, ref count);
					break;
				}
			}
		}
		float time = this.GetTime();
		this.UpdateSounds(time);
		this.UpdateMistFX(Time.time);
	}

	// Token: 0x06006441 RID: 25665 RVA: 0x002CB988 File Offset: 0x002C9B88
	private void UpdateMistFX(float t)
	{
		this.mistClearList.Clear();
		foreach (KeyValuePair<Pair<int, bool>, FallingWater.MistInfo> keyValuePair in this.mistAlive)
		{
			if (t > keyValuePair.Value.deathTime)
			{
				keyValuePair.Value.fx.Play("end", KAnim.PlayMode.Once, 1f, 0f);
				this.mistClearList.Add(keyValuePair.Key);
			}
		}
		foreach (Pair<int, bool> key in this.mistClearList)
		{
			this.mistAlive.Remove(key);
		}
		this.mistClearList.Clear();
	}

	// Token: 0x06006442 RID: 25666 RVA: 0x002CBA7C File Offset: 0x002C9C7C
	private void UpdateSounds(float t)
	{
		this.clearList.Clear();
		foreach (KeyValuePair<int, FallingWater.SoundInfo> keyValuePair in this.topSounds)
		{
			FallingWater.SoundInfo value = keyValuePair.Value;
			if (t - value.startTime >= this.stopTopLoopDelay)
			{
				if (value.handle != HandleVector<int>.InvalidHandle)
				{
					LoopingSoundManager.StopSound(value.handle);
				}
				this.clearList.Add(keyValuePair.Key);
			}
		}
		foreach (int key in this.clearList)
		{
			this.topSounds.Remove(key);
		}
		this.clearList.Clear();
		foreach (KeyValuePair<int, FallingWater.SoundInfo> keyValuePair2 in this.splashSounds)
		{
			FallingWater.SoundInfo value2 = keyValuePair2.Value;
			if (t - value2.startTime >= this.stopSplashLoopDelay)
			{
				if (value2.handle != HandleVector<int>.InvalidHandle)
				{
					LoopingSoundManager.StopSound(value2.handle);
				}
				this.clearList.Add(keyValuePair2.Key);
			}
		}
		foreach (int key2 in this.clearList)
		{
			this.splashSounds.Remove(key2);
		}
		this.clearList.Clear();
	}

	// Token: 0x06006443 RID: 25667 RVA: 0x002CBC4C File Offset: 0x002C9E4C
	public Dictionary<int, float> GetInfo(int cell)
	{
		Dictionary<int, float> dictionary = new Dictionary<int, float>();
		int count = this.physics.Count;
		for (int i = 0; i < count; i++)
		{
			if (Grid.PosToCell(this.physics[i].position) == cell)
			{
				FallingWater.ParticleProperties particleProperties = this.particleProperties[i];
				float num = 0f;
				dictionary.TryGetValue((int)particleProperties.elementIdx, out num);
				num += particleProperties.mass;
				dictionary[(int)particleProperties.elementIdx] = num;
			}
		}
		return dictionary;
	}

	// Token: 0x06006444 RID: 25668 RVA: 0x000E5E47 File Offset: 0x000E4047
	private float GetParticleVolume(float mass)
	{
		return Mathf.Clamp01((mass - (this.particleMassToSplit - this.particleMassVariation)) / (2f * this.particleMassVariation));
	}

	// Token: 0x06006445 RID: 25669 RVA: 0x002CBCD0 File Offset: 0x002C9ED0
	private void AddToSim(int cell, int particleIdx, ref int num_particles)
	{
		bool flag = false;
		for (;;)
		{
			if ((Grid.Element[cell].state & Element.State.Solid) == Element.State.Solid || (Grid.Properties[cell] & 2) != 0)
			{
				cell += Grid.WidthInCells;
				if (!Grid.IsValidCell(cell))
				{
					break;
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				goto Block_3;
			}
		}
		return;
		Block_3:
		FallingWater.ParticleProperties particleProperties = this.particleProperties[particleIdx];
		SimMessages.AddRemoveSubstance(cell, particleProperties.elementIdx, CellEventLogger.Instance.FallingWaterAddToSim, particleProperties.mass, particleProperties.temperature, particleProperties.diseaseIdx, particleProperties.diseaseCount, true, -1);
		this.RemoveParticle(particleIdx, ref num_particles);
		float time = this.GetTime();
		float num = this.lastSpawnTime[cell];
		if (time - num >= this.minSpawnDelay)
		{
			this.lastSpawnTime[cell] = time;
			Vector3 vector = Grid.CellToPosCCC(cell, Grid.SceneLayer.TileMain);
			vector.z = 0f;
			if (CameraController.Instance.IsAudibleSound(vector))
			{
				bool flag2 = true;
				FallingWater.SoundInfo soundInfo;
				if (this.splashSounds.TryGetValue(cell, out soundInfo))
				{
					soundInfo.splashCount++;
					if (soundInfo.splashCount > this.splashCountLoopThreshold)
					{
						if (soundInfo.handle == HandleVector<int>.InvalidHandle)
						{
							soundInfo.handle = LoopingSoundManager.StartSound(this.liquid_splash_loop, vector, true, true);
						}
						LoopingSoundManager.Get().UpdateFirstParameter(soundInfo.handle, FallingWater.HASH_LIQUIDDEPTH, SoundUtil.GetLiquidDepth(cell));
						LoopingSoundManager.Get().UpdateSecondParameter(soundInfo.handle, FallingWater.HASH_LIQUIDVOLUME, this.GetParticleVolume(particleProperties.mass));
						flag2 = false;
					}
				}
				else
				{
					soundInfo = default(FallingWater.SoundInfo);
					soundInfo.handle = HandleVector<int>.InvalidHandle;
				}
				soundInfo.startTime = time;
				this.splashSounds[cell] = soundInfo;
				if (flag2)
				{
					EventInstance instance = SoundEvent.BeginOneShot(this.liquid_splash_initial, vector, 1f, false);
					instance.setParameterByName("liquidDepth", SoundUtil.GetLiquidDepth(cell), false);
					instance.setParameterByName("liquidVolume", this.GetParticleVolume(particleProperties.mass), false);
					SoundEvent.EndOneShot(instance);
				}
			}
		}
	}

	// Token: 0x06006446 RID: 25670 RVA: 0x002CBEC8 File Offset: 0x002CA0C8
	private void RemoveParticle(int particleIdx, ref int num_particles)
	{
		num_particles--;
		this.physics[particleIdx] = this.physics[num_particles];
		this.particleProperties[particleIdx] = this.particleProperties[num_particles];
		this.physics.RemoveAt(num_particles);
		this.particleProperties.RemoveAt(num_particles);
	}

	// Token: 0x06006447 RID: 25671 RVA: 0x002CBF28 File Offset: 0x002CA128
	public void Render()
	{
		List<Vector3> vertices = MeshUtil.vertices;
		List<Color32> colours = MeshUtil.colours32;
		List<Vector2> uvs = MeshUtil.uvs;
		List<int> indices = MeshUtil.indices;
		uvs.Clear();
		vertices.Clear();
		indices.Clear();
		colours.Clear();
		float num = this.particleSize.x * 0.5f;
		float num2 = this.particleSize.y * 0.5f;
		Vector2 a = new Vector2(-num, -num2);
		Vector2 a2 = new Vector2(num, -num2);
		Vector2 a3 = new Vector2(num, num2);
		Vector2 a4 = new Vector2(-num, num2);
		float y = 1f;
		float y2 = 0f;
		int num3 = Mathf.Min(this.physics.Count, 16249);
		if (num3 < this.physics.Count)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"Too many water particles to render. Wanted",
				this.physics.Count,
				"but truncating to limit"
			});
		}
		for (int i = 0; i < num3; i++)
		{
			Vector2 position = this.physics[i].position;
			float d = Mathf.Lerp(0.25f, 1f, Mathf.Clamp01(this.particleProperties[i].mass / this.particleMassToSplit));
			vertices.Add(position + a * d);
			vertices.Add(position + a2 * d);
			vertices.Add(position + a3 * d);
			vertices.Add(position + a4 * d);
			int frame = this.physics[i].frame;
			float x = (float)frame * this.uvFrameSize.x;
			float x2 = (float)(frame + 1) * this.uvFrameSize.x;
			uvs.Add(new Vector2(x, y2));
			uvs.Add(new Vector2(x2, y2));
			uvs.Add(new Vector2(x2, y));
			uvs.Add(new Vector2(x, y));
			Color32 colour = this.physics[i].colour;
			colours.Add(colour);
			colours.Add(colour);
			colours.Add(colour);
			colours.Add(colour);
			int num4 = i * 4;
			indices.Add(num4);
			indices.Add(num4 + 1);
			indices.Add(num4 + 2);
			indices.Add(num4);
			indices.Add(num4 + 2);
			indices.Add(num4 + 3);
		}
		this.mesh.Clear();
		this.mesh.SetVertices(vertices);
		this.mesh.SetUVs(0, uvs);
		this.mesh.SetColors(colours);
		this.mesh.SetTriangles(indices, 0);
		int layer = LayerMask.NameToLayer("Water");
		Vector4 value = PropertyTextures.CalculateClusterWorldSize();
		this.material.SetVector("_ClusterWorldSizeInfo", value);
		Graphics.DrawMesh(this.mesh, this.renderOffset, Quaternion.identity, this.material, layer, null, 0, this.propertyBlock);
	}

	// Token: 0x06006448 RID: 25672 RVA: 0x000E5E6A File Offset: 0x000E406A
	private KBatchedAnimController SpawnMist()
	{
		GameObject instance = this.mistPool.GetInstance();
		instance.SetActive(true);
		KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
		component.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		return component;
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x000E5E9E File Offset: 0x000E409E
	private GameObject InstantiateMist()
	{
		GameObject gameObject = GameUtil.KInstantiate(this.mistEffect, Grid.SceneLayer.BuildingBack, null, 0);
		gameObject.SetActive(false);
		gameObject.GetComponent<KBatchedAnimController>().onDestroySelf = new Action<GameObject>(this.ReleaseMist);
		return gameObject;
	}

	// Token: 0x0600644A RID: 25674 RVA: 0x000E5ECD File Offset: 0x000E40CD
	private void ReleaseMist(GameObject go)
	{
		go.SetActive(false);
		this.mistPool.ReleaseInstance(go);
	}

	// Token: 0x0600644B RID: 25675 RVA: 0x000E5EE2 File Offset: 0x000E40E2
	public void Sim200ms(float dt)
	{
		if (this.simUpdateDelay >= 0)
		{
			this.simUpdateDelay--;
			return;
		}
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600644C RID: 25676 RVA: 0x002CC24C File Offset: 0x002CA44C
	[OnSerializing]
	private void OnSerializing()
	{
		List<Element> elements = ElementLoader.elements;
		Diseases diseases = Db.Get().Diseases;
		this.serializedParticleProperties = new List<FallingWater.SerializedParticleProperties>();
		foreach (FallingWater.ParticleProperties particleProperties in this.particleProperties)
		{
			FallingWater.SerializedParticleProperties item = default(FallingWater.SerializedParticleProperties);
			item.elementID = elements[(int)particleProperties.elementIdx].id;
			item.diseaseID = ((particleProperties.diseaseIdx != byte.MaxValue) ? diseases[(int)particleProperties.diseaseIdx].IdHash : HashedString.Invalid);
			item.mass = particleProperties.mass;
			item.temperature = particleProperties.temperature;
			item.diseaseCount = particleProperties.diseaseCount;
			this.serializedParticleProperties.Add(item);
		}
	}

	// Token: 0x0600644D RID: 25677 RVA: 0x000E5F07 File Offset: 0x000E4107
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedParticleProperties = null;
	}

	// Token: 0x0600644E RID: 25678 RVA: 0x002CC33C File Offset: 0x002CA53C
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 26))
		{
			for (int i = 0; i < this.physics.Count; i++)
			{
				int num = Grid.PosToCell(this.physics[i].position);
				if (Grid.IsValidCell(num))
				{
					FallingWater.ParticlePhysics value = this.physics[i];
					value.worldIdx = (int)Grid.WorldIdx[num];
					this.physics[i] = value;
				}
			}
		}
		if (this.serializedParticleProperties != null)
		{
			Diseases diseases = Db.Get().Diseases;
			this.particleProperties.Clear();
			using (List<FallingWater.SerializedParticleProperties>.Enumerator enumerator = this.serializedParticleProperties.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FallingWater.SerializedParticleProperties serializedParticleProperties = enumerator.Current;
					FallingWater.ParticleProperties item = default(FallingWater.ParticleProperties);
					item.elementIdx = ElementLoader.GetElementIndex(serializedParticleProperties.elementID);
					item.diseaseIdx = ((serializedParticleProperties.diseaseID != HashedString.Invalid) ? diseases.GetIndex(serializedParticleProperties.diseaseID) : byte.MaxValue);
					item.mass = serializedParticleProperties.mass;
					item.temperature = serializedParticleProperties.temperature;
					item.diseaseCount = serializedParticleProperties.diseaseCount;
					this.particleProperties.Add(item);
				}
				goto IL_15A;
			}
		}
		this.particleProperties = this.properties;
		IL_15A:
		this.properties = null;
	}

	// Token: 0x040047FF RID: 18431
	private const float STATE_TRANSITION_TEMPERATURE_BUFER = 3f;

	// Token: 0x04004800 RID: 18432
	private const byte FORCED_ALPHA = 191;

	// Token: 0x04004801 RID: 18433
	private int simUpdateDelay = 2;

	// Token: 0x04004802 RID: 18434
	[SerializeField]
	private Vector2 particleSize;

	// Token: 0x04004803 RID: 18435
	[SerializeField]
	private Vector2 initialOffset;

	// Token: 0x04004804 RID: 18436
	[SerializeField]
	private float jitterStep;

	// Token: 0x04004805 RID: 18437
	[SerializeField]
	private Vector3 renderOffset;

	// Token: 0x04004806 RID: 18438
	[SerializeField]
	private float minSpawnDelay;

	// Token: 0x04004807 RID: 18439
	[SerializeField]
	private float gravityScale = 0.05f;

	// Token: 0x04004808 RID: 18440
	[SerializeField]
	private float particleMassToSplit = 75f;

	// Token: 0x04004809 RID: 18441
	[SerializeField]
	private float particleMassVariation = 15f;

	// Token: 0x0400480A RID: 18442
	[SerializeField]
	private Vector2 multipleOffsetRange;

	// Token: 0x0400480B RID: 18443
	[SerializeField]
	private GameObject mistEffect;

	// Token: 0x0400480C RID: 18444
	[SerializeField]
	private float mistEffectMinAliveTime = 2f;

	// Token: 0x0400480D RID: 18445
	[SerializeField]
	private Material material;

	// Token: 0x0400480E RID: 18446
	[SerializeField]
	private Texture2D texture;

	// Token: 0x0400480F RID: 18447
	[SerializeField]
	private int numFrames;

	// Token: 0x04004810 RID: 18448
	[SerializeField]
	private FallingWater.DecorInfo liquid_splash;

	// Token: 0x04004811 RID: 18449
	[SerializeField]
	private EventReference liquid_top_loop;

	// Token: 0x04004812 RID: 18450
	[SerializeField]
	private EventReference liquid_splash_initial;

	// Token: 0x04004813 RID: 18451
	[SerializeField]
	private EventReference liquid_splash_loop;

	// Token: 0x04004814 RID: 18452
	[SerializeField]
	private float stopTopLoopDelay = 0.2f;

	// Token: 0x04004815 RID: 18453
	[SerializeField]
	private float stopSplashLoopDelay = 1f;

	// Token: 0x04004816 RID: 18454
	[SerializeField]
	private int splashCountLoopThreshold = 10;

	// Token: 0x04004817 RID: 18455
	[Serialize]
	private List<FallingWater.ParticlePhysics> physics = new List<FallingWater.ParticlePhysics>();

	// Token: 0x04004818 RID: 18456
	private List<FallingWater.ParticleProperties> particleProperties = new List<FallingWater.ParticleProperties>();

	// Token: 0x04004819 RID: 18457
	[Serialize]
	private List<FallingWater.SerializedParticleProperties> serializedParticleProperties;

	// Token: 0x0400481A RID: 18458
	[Serialize]
	private List<FallingWater.ParticleProperties> properties = new List<FallingWater.ParticleProperties>();

	// Token: 0x0400481B RID: 18459
	private Dictionary<int, FallingWater.SoundInfo> topSounds = new Dictionary<int, FallingWater.SoundInfo>();

	// Token: 0x0400481C RID: 18460
	private Dictionary<int, FallingWater.SoundInfo> splashSounds = new Dictionary<int, FallingWater.SoundInfo>();

	// Token: 0x0400481D RID: 18461
	private GameObjectPool mistPool;

	// Token: 0x0400481E RID: 18462
	private Mesh mesh;

	// Token: 0x0400481F RID: 18463
	private float offset;

	// Token: 0x04004820 RID: 18464
	private float[] lastSpawnTime;

	// Token: 0x04004821 RID: 18465
	private Dictionary<Pair<int, bool>, FallingWater.MistInfo> mistAlive = new Dictionary<Pair<int, bool>, FallingWater.MistInfo>();

	// Token: 0x04004822 RID: 18466
	private Vector2 uvFrameSize;

	// Token: 0x04004823 RID: 18467
	private MaterialPropertyBlock propertyBlock;

	// Token: 0x04004824 RID: 18468
	private static FallingWater _instance;

	// Token: 0x04004825 RID: 18469
	private List<int> clearList = new List<int>();

	// Token: 0x04004826 RID: 18470
	private List<Pair<int, bool>> mistClearList = new List<Pair<int, bool>>();

	// Token: 0x04004827 RID: 18471
	private static HashedString HASH_LIQUIDDEPTH = "liquidDepth";

	// Token: 0x04004828 RID: 18472
	private static HashedString HASH_LIQUIDVOLUME = "liquidVolume";

	// Token: 0x02001321 RID: 4897
	[Serializable]
	private struct DecorInfo
	{
		// Token: 0x04004829 RID: 18473
		public string[] names;

		// Token: 0x0400482A RID: 18474
		public Vector2 offset;

		// Token: 0x0400482B RID: 18475
		public Vector2 size;
	}

	// Token: 0x02001322 RID: 4898
	private struct SoundInfo
	{
		// Token: 0x0400482C RID: 18476
		public float startTime;

		// Token: 0x0400482D RID: 18477
		public int splashCount;

		// Token: 0x0400482E RID: 18478
		public HandleVector<int>.Handle handle;
	}

	// Token: 0x02001323 RID: 4899
	private struct MistInfo
	{
		// Token: 0x0400482F RID: 18479
		public KBatchedAnimController fx;

		// Token: 0x04004830 RID: 18480
		public float deathTime;
	}

	// Token: 0x02001324 RID: 4900
	private struct ParticlePhysics
	{
		// Token: 0x06006451 RID: 25681 RVA: 0x002CC578 File Offset: 0x002CA778
		public ParticlePhysics(Vector2 position, Vector2 velocity, int frame, ushort elementIdx, int worldIdx)
		{
			this.position = position;
			this.velocity = velocity;
			this.frame = frame;
			this.colour = ElementLoader.elements[(int)elementIdx].substance.colour;
			this.colour.a = 191;
			this.worldIdx = worldIdx;
		}

		// Token: 0x04004831 RID: 18481
		public Vector2 position;

		// Token: 0x04004832 RID: 18482
		public Vector2 velocity;

		// Token: 0x04004833 RID: 18483
		public int frame;

		// Token: 0x04004834 RID: 18484
		public Color32 colour;

		// Token: 0x04004835 RID: 18485
		public int worldIdx;
	}

	// Token: 0x02001325 RID: 4901
	private struct SerializedParticleProperties
	{
		// Token: 0x04004836 RID: 18486
		public SimHashes elementID;

		// Token: 0x04004837 RID: 18487
		public HashedString diseaseID;

		// Token: 0x04004838 RID: 18488
		public float mass;

		// Token: 0x04004839 RID: 18489
		public float temperature;

		// Token: 0x0400483A RID: 18490
		public int diseaseCount;
	}

	// Token: 0x02001326 RID: 4902
	private struct ParticleProperties
	{
		// Token: 0x06006452 RID: 25682 RVA: 0x000E5F30 File Offset: 0x000E4130
		public ParticleProperties(ushort elementIdx, float mass, float temperature, byte disease_idx, int disease_count, bool debug_track)
		{
			this.elementIdx = elementIdx;
			this.diseaseIdx = disease_idx;
			this.mass = mass;
			this.temperature = temperature;
			this.diseaseCount = disease_count;
		}

		// Token: 0x0400483B RID: 18491
		public ushort elementIdx;

		// Token: 0x0400483C RID: 18492
		public byte diseaseIdx;

		// Token: 0x0400483D RID: 18493
		public float mass;

		// Token: 0x0400483E RID: 18494
		public float temperature;

		// Token: 0x0400483F RID: 18495
		public int diseaseCount;
	}
}
