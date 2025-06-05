using System;
using System.Collections.Generic;
using FMOD.Studio;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001103 RID: 4355
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Comet")]
public class Comet : KMonoBehaviour, ISim33ms
{
	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x060058E2 RID: 22754 RVA: 0x000DE61D File Offset: 0x000DC81D
	public float ExplosionMass
	{
		get
		{
			return this.explosionMass;
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x060058E3 RID: 22755 RVA: 0x000DE625 File Offset: 0x000DC825
	public float AddTileMass
	{
		get
		{
			return this.addTileMass;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x060058E4 RID: 22756 RVA: 0x000DE62D File Offset: 0x000DC82D
	public Vector3 TargetPosition
	{
		get
		{
			return this.anim.PositionIncludingOffset;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x060058E5 RID: 22757 RVA: 0x000DE63A File Offset: 0x000DC83A
	// (set) Token: 0x060058E6 RID: 22758 RVA: 0x000DE642 File Offset: 0x000DC842
	public Vector2 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x060058E7 RID: 22759 RVA: 0x0029AB80 File Offset: 0x00298D80
	private float GetVolume(GameObject gameObject)
	{
		float result = 1f;
		if (gameObject != null && this.selectable != null && this.selectable.IsSelected)
		{
			result = 1f;
		}
		return result;
	}

	// Token: 0x060058E8 RID: 22760 RVA: 0x000DE64B File Offset: 0x000DC84B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.remainingTileDamage = this.totalTileDamage;
		this.loopingSounds = base.gameObject.GetComponent<LoopingSounds>();
		this.flyingSound = GlobalAssets.GetSound("Meteor_LP", false);
		this.RandomizeVelocity();
	}

	// Token: 0x060058E9 RID: 22761 RVA: 0x0029ABC0 File Offset: 0x00298DC0
	protected override void OnSpawn()
	{
		this.anim.Offset = this.offsetPosition;
		if (this.spawnWithOffset)
		{
			this.SetupOffset();
		}
		base.OnSpawn();
		this.RandomizeMassAndTemperature();
		this.StartLoopingSound();
		bool flag = this.offsetPosition.x != 0f || this.offsetPosition.y != 0f;
		this.selectable.enabled = !flag;
		this.typeID = base.GetComponent<KPrefabID>().PrefabTag;
		Components.Meteors.Add(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060058EA RID: 22762 RVA: 0x000DE687 File Offset: 0x000DC887
	protected override void OnCleanUp()
	{
		Components.Meteors.Remove(base.gameObject.GetMyWorldId(), this);
	}

	// Token: 0x060058EB RID: 22763 RVA: 0x0029AC60 File Offset: 0x00298E60
	protected void SetupOffset()
	{
		Vector3 position = base.transform.GetPosition();
		Vector3 position2 = base.transform.GetPosition();
		position2.z = 0f;
		Vector3 vector = new Vector3(this.velocity.x, this.velocity.y, 0f);
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		float num = (float)(myWorld.WorldOffset.y + myWorld.Height + MissileLauncher.Def.launchRange.y) * Grid.CellSizeInMeters - position2.y;
		float f = Vector3.Angle(Vector3.up, -vector) * 0.017453292f;
		float d = Mathf.Abs(num / Mathf.Cos(f));
		Vector3 vector2 = position2 - vector.normalized * d;
		float num2 = (float)(myWorld.WorldOffset.x + myWorld.Width) * Grid.CellSizeInMeters;
		if (vector2.x < (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters || vector2.x > num2)
		{
			float num3 = (vector.x < 0f) ? (num2 - position2.x) : (position2.x - (float)myWorld.WorldOffset.x * Grid.CellSizeInMeters);
			f = Vector3.Angle((vector.x < 0f) ? Vector3.right : Vector3.left, -vector) * 0.017453292f;
			d = Mathf.Abs(num3 / Mathf.Cos(f));
		}
		Vector3 b = -vector.normalized * d;
		(position2 + b).z = position.z;
		this.offsetPosition = b;
		this.anim.Offset = this.offsetPosition;
	}

	// Token: 0x060058EC RID: 22764 RVA: 0x0029AE24 File Offset: 0x00299024
	public virtual void RandomizeVelocity()
	{
		float num = UnityEngine.Random.Range(this.spawnAngle.x, this.spawnAngle.y);
		float f = num * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().Rotation = -num - 90f;
	}

	// Token: 0x060058ED RID: 22765 RVA: 0x0029AEA8 File Offset: 0x002990A8
	public void RandomizeMassAndTemperature()
	{
		float num = UnityEngine.Random.Range(this.massRange.x, this.massRange.y) * this.GetMassMultiplier();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		component.Mass = num;
		component.Temperature = UnityEngine.Random.Range(this.temperatureRange.x, this.temperatureRange.y);
		if (this.addTiles > 0)
		{
			float num2 = UnityEngine.Random.Range(0.95f, 0.98f);
			this.explosionMass = num * (1f - num2);
			this.addTileMass = num * num2;
			return;
		}
		this.explosionMass = num;
		this.addTileMass = 0f;
	}

	// Token: 0x060058EE RID: 22766 RVA: 0x0029AF4C File Offset: 0x0029914C
	public float GetMassMultiplier()
	{
		float num = 1f;
		SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.MeteorShowers);
		if (this.affectedByDifficulty && currentQualitySetting != null)
		{
			string id = currentQualitySetting.id;
			if (!(id == "Infrequent"))
			{
				if (!(id == "Intense"))
				{
					if (id == "Doomed")
					{
						num *= 0.5f;
					}
				}
				else
				{
					num *= 0.8f;
				}
			}
			else
			{
				num *= 1f;
			}
		}
		return num;
	}

	// Token: 0x060058EF RID: 22767 RVA: 0x000DE69F File Offset: 0x000DC89F
	public int GetRandomNumOres()
	{
		return UnityEngine.Random.Range(this.explosionOreCount.x, this.explosionOreCount.y + 1);
	}

	// Token: 0x060058F0 RID: 22768 RVA: 0x000DE6BE File Offset: 0x000DC8BE
	public float GetRandomTemperatureForOres()
	{
		return UnityEngine.Random.Range(this.explosionTemperatureRange.x, this.explosionTemperatureRange.y);
	}

	// Token: 0x060058F1 RID: 22769 RVA: 0x0029AFC8 File Offset: 0x002991C8
	[ContextMenu("Explode")]
	private void Explode(Vector3 pos, int cell, int prev_cell, Element element)
	{
		int world = (int)Grid.WorldIdx[cell];
		this.PlayImpactSound(pos);
		Vector3 pos2 = pos;
		pos2.z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2);
		if (this.explosionEffectHash != SpawnFXHashes.None)
		{
			Game.Instance.SpawnFX(this.explosionEffectHash, pos2, 0f);
		}
		Substance substance = element.substance;
		int randomNumOres = this.GetRandomNumOres();
		Vector2 vector = -this.velocity.normalized;
		Vector2 a = new Vector2(vector.y, -vector.x);
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x - 3, (int)pos.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 vector2 = (gameObject.transform.GetPosition() - pos).normalized;
				vector2 += new Vector2(0f, 0.55f);
				vector2 *= 0.5f * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, vector2);
			}
		}
		pooledList.Recycle();
		int num = this.splashRadius + 1;
		for (int i = -num; i <= num; i++)
		{
			for (int j = -num; j <= num; j++)
			{
				int num2 = Grid.OffsetCell(cell, j, i);
				if (Grid.IsValidCellInWorld(num2, world) && !this.destroyedCells.Contains(num2))
				{
					float num3 = (1f - (float)Mathf.Abs(j) / (float)num) * (1f - (float)Mathf.Abs(i) / (float)num);
					if (num3 > 0f)
					{
						this.DamageTiles(num2, prev_cell, num3 * this.totalTileDamage * 0.5f);
					}
				}
			}
		}
		float mass = (randomNumOres > 0) ? (this.explosionMass / (float)randomNumOres) : 1f;
		float randomTemperatureForOres = this.GetRandomTemperatureForOres();
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		for (int k = 0; k < randomNumOres; k++)
		{
			Vector2 normalized = (vector + a * UnityEngine.Random.Range(-1f, 1f)).normalized;
			Vector3 v = normalized * UnityEngine.Random.Range(this.explosionSpeedRange.x, this.explosionSpeedRange.y);
			Vector3 vector3 = normalized.normalized * 0.75f;
			vector3 += new Vector3(0f, 0.55f, 0f);
			vector3 += pos;
			GameObject go = substance.SpawnResource(vector3, mass, randomTemperatureForOres, component.DiseaseIdx, component.DiseaseCount / (randomNumOres + this.addTiles), false, false, false);
			if (GameComps.Fallers.Has(go))
			{
				GameComps.Fallers.Remove(go);
			}
			GameComps.Fallers.Add(go, v);
		}
		if (this.addTiles > 0)
		{
			this.DepositTiles(cell, element, world, prev_cell, randomTemperatureForOres);
		}
		this.SpawnCraterPrefabs();
		if (this.OnImpact != null)
		{
			this.OnImpact();
		}
	}

	// Token: 0x060058F2 RID: 22770 RVA: 0x0029B3AC File Offset: 0x002995AC
	protected virtual void DepositTiles(int cell, Element element, int world, int prev_cell, float temperature)
	{
		float depthOfElement = (float)this.GetDepthOfElement(cell3, element, world);
		float num = 1f;
		float num2 = (depthOfElement - (float)this.addTilesMinHeight) / (float)(this.addTilesMaxHeight - this.addTilesMinHeight);
		if (!float.IsNaN(num2))
		{
			num -= num2;
		}
		int num3 = Mathf.Min(this.addTiles, Mathf.Clamp(Mathf.RoundToInt((float)this.addTiles * num), 1, this.addTiles));
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet = HashSetPool<int, Comet>.Allocate();
		HashSetPool<int, Comet>.PooledHashSet pooledHashSet2 = HashSetPool<int, Comet>.Allocate();
		QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
		int num4 = -1;
		int num5 = 1;
		if (this.velocity.x < 0f)
		{
			num4 *= -1;
			num5 *= -1;
		}
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = prev_cell,
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num4, 0)),
			depth = 0
		});
		pooledQueue.Enqueue(new GameUtil.FloodFillInfo
		{
			cell = Grid.OffsetCell(prev_cell, new CellOffset(num5, 0)),
			depth = 0
		});
		Func<int, bool> condition = (int cell) => Grid.IsValidCellInWorld(cell, world) && !Grid.Solid[cell];
		GameUtil.FloodFillConditional(pooledQueue, condition, pooledHashSet2, pooledHashSet, 10);
		float mass = (num3 > 0) ? (this.addTileMass / (float)this.addTiles) : 1f;
		int disease_count = this.addDiseaseCount / num3;
		if (element.HasTag(GameTags.Unstable))
		{
			UnstableGroundManager component = World.Instance.GetComponent<UnstableGroundManager>();
			using (HashSet<int>.Enumerator enumerator = pooledHashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int cell2 = enumerator.Current;
					if (num3 <= 0)
					{
						break;
					}
					component.Spawn(cell2, element, mass, temperature, byte.MaxValue, 0);
					num3--;
				}
				goto IL_229;
			}
		}
		foreach (int gameCell in pooledHashSet)
		{
			if (num3 <= 0)
			{
				break;
			}
			SimMessages.AddRemoveSubstance(gameCell, element.id, CellEventLogger.Instance.ElementEmitted, mass, temperature, this.diseaseIdx, disease_count, true, -1);
			num3--;
		}
		IL_229:
		pooledHashSet.Recycle();
		pooledHashSet2.Recycle();
		pooledQueue.Recycle();
	}

	// Token: 0x060058F3 RID: 22771 RVA: 0x0029B614 File Offset: 0x00299814
	protected virtual void SpawnCraterPrefabs()
	{
		if (this.craterPrefabs != null && this.craterPrefabs.Length != 0)
		{
			GameObject gameObject = global::Util.KInstantiate(Assets.GetPrefab(this.craterPrefabs[UnityEngine.Random.Range(0, this.craterPrefabs.Length)]), Grid.CellToPos(Grid.PosToCell(this)));
			gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -19.5f);
			gameObject.SetActive(true);
		}
	}

	// Token: 0x060058F4 RID: 22772 RVA: 0x0029B6A0 File Offset: 0x002998A0
	protected int GetDepthOfElement(int cell, Element element, int world)
	{
		int num = 0;
		int num2 = Grid.CellBelow(cell);
		while (Grid.IsValidCellInWorld(num2, world) && Grid.Element[num2] == element)
		{
			num++;
			num2 = Grid.CellBelow(num2);
		}
		return num;
	}

	// Token: 0x060058F5 RID: 22773 RVA: 0x0029B6D8 File Offset: 0x002998D8
	[ContextMenu("DamageTiles")]
	private float DamageTiles(int cell, int prev_cell, float input_damage)
	{
		GameObject gameObject = Grid.Objects[cell, 9];
		float num = 1f;
		bool flag = false;
		if (gameObject != null)
		{
			if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Window))
			{
				num = this.windowDamageMultiplier;
			}
			else if (gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker))
			{
				num = this.bunkerDamageMultiplier;
				if (gameObject.GetComponent<Door>() != null)
				{
					Game.Instance.savedInfo.blockedCometWithBunkerDoor = true;
				}
			}
			SimCellOccupier component = gameObject.GetComponent<SimCellOccupier>();
			if (component != null && !component.doReplaceElement)
			{
				flag = true;
			}
		}
		Element element;
		if (flag)
		{
			element = gameObject.GetComponent<PrimaryElement>().Element;
		}
		else
		{
			element = Grid.Element[cell];
		}
		if (element.strength == 0f)
		{
			return 0f;
		}
		float num2 = input_damage * num / element.strength;
		this.PlayTileDamageSound(element, Grid.CellToPos(cell), gameObject);
		if (num2 == 0f)
		{
			return 0f;
		}
		float num3;
		if (flag)
		{
			BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
			float a = (float)component2.HitPoints / (float)component2.MaxHitPoints;
			float f = num2 * (float)component2.MaxHitPoints;
			component2.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
			{
				damage = Mathf.RoundToInt(f),
				source = BUILDINGS.DAMAGESOURCES.COMET,
				popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
			});
			num3 = Mathf.Min(a, num2);
		}
		else
		{
			num3 = WorldDamage.Instance.ApplyDamage(cell, num2, prev_cell, BUILDINGS.DAMAGESOURCES.COMET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET);
		}
		this.destroyedCells.Add(cell);
		float num4 = num3 / num2;
		return input_damage * (1f - num4);
	}

	// Token: 0x060058F6 RID: 22774 RVA: 0x0029B890 File Offset: 0x00299A90
	private void DamageThings(Vector3 pos, int cell, int damage, GameObject ignoreObject = null)
	{
		if (damage == 0 || !Grid.IsValidCell(cell))
		{
			return;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null && gameObject != ignoreObject)
		{
			BuildingHP component = gameObject.GetComponent<BuildingHP>();
			Building component2 = gameObject.GetComponent<Building>();
			if (component != null && !this.damagedEntities.Contains(gameObject))
			{
				float f = gameObject.GetComponent<KPrefabID>().HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				if (component2 != null && component2.Def != null)
				{
					this.PlayBuildingDamageSound(component2.Def, Grid.CellToPos(cell), gameObject);
				}
				component.gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
				{
					damage = Mathf.RoundToInt(f),
					source = BUILDINGS.DAMAGESOURCES.COMET,
					popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.COMET
				});
				this.damagedEntities.Add(gameObject);
			}
		}
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		GameScenePartitioner.Instance.GatherEntries((int)pos.x, (int)pos.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			Pickupable pickupable = scenePartitionerEntry.obj as Pickupable;
			Health component3 = pickupable.GetComponent<Health>();
			if (component3 != null && !this.damagedEntities.Contains(pickupable.gameObject))
			{
				float amount = pickupable.KPrefabID.HasTag(GameTags.Bunker) ? ((float)damage * this.bunkerDamageMultiplier) : ((float)damage);
				component3.Damage(amount);
				this.damagedEntities.Add(pickupable.gameObject);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060058F7 RID: 22775 RVA: 0x0029BA7C File Offset: 0x00299C7C
	public float GetDistanceFromImpact()
	{
		float num = this.velocity.x / this.velocity.y;
		Vector3 position = base.transform.GetPosition();
		float num2 = 0f;
		while (num2 > -6f)
		{
			num2 -= 1f;
			num2 = Mathf.Ceil(position.y + num2) - 0.2f - position.y;
			float x = num2 * num;
			Vector3 b = new Vector3(x, num2, 0f);
			int num3 = Grid.PosToCell(position + b);
			if (Grid.IsValidCell(num3) && Grid.Solid[num3])
			{
				return b.magnitude;
			}
		}
		return 6f;
	}

	// Token: 0x060058F8 RID: 22776 RVA: 0x000DE6DB File Offset: 0x000DC8DB
	public float GetSoundDistance()
	{
		return this.GetDistanceFromImpact();
	}

	// Token: 0x060058F9 RID: 22777 RVA: 0x0029BB28 File Offset: 0x00299D28
	private void PlayTileDamageSound(Element element, Vector3 pos, GameObject tile_go)
	{
		string text = element.substance.GetMiningBreakSound();
		if (text == null)
		{
			if (element.HasTag(GameTags.RefinedMetal))
			{
				text = "RefinedMetal";
			}
			else if (element.HasTag(GameTags.Metal))
			{
				text = "RawMetal";
			}
			else
			{
				text = "Rock";
			}
		}
		text = "MeteorDamage_" + text;
		text = GlobalAssets.GetSound(text, false);
		if (CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, text))
		{
			float volume = this.GetVolume(tile_go);
			KFMOD.PlayOneShot(text, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
		}
	}

	// Token: 0x060058FA RID: 22778 RVA: 0x0029BBC4 File Offset: 0x00299DC4
	private void PlayBuildingDamageSound(BuildingDef def, Vector3 pos, GameObject building_go)
	{
		if (def != null)
		{
			string sound = GlobalAssets.GetSound(StringFormatter.Combine("MeteorDamage_Building_", def.AudioCategory), false);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound("MeteorDamage_Building_Metal", false);
			}
			if (sound != null && CameraController.Instance && CameraController.Instance.IsAudibleSound(pos, sound))
			{
				float volume = this.GetVolume(building_go);
				KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition(pos, false), volume);
			}
		}
	}

	// Token: 0x060058FB RID: 22779 RVA: 0x0029BC40 File Offset: 0x00299E40
	public void Sim33ms(float dt)
	{
		if (this.hasExploded)
		{
			return;
		}
		if (this.offsetPosition.y > 0f)
		{
			Vector3 b = new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			Vector3 vector = this.offsetPosition + b;
			this.offsetPosition = vector;
			this.anim.Offset = this.offsetPosition;
		}
		else
		{
			if (this.anim.Offset != Vector3.zero)
			{
				this.anim.Offset = Vector3.zero;
			}
			if (!this.selectable.enabled)
			{
				this.selectable.enabled = true;
			}
			Vector2 vector2 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * -0.1f;
			Vector2 vector3 = new Vector2((float)Grid.WidthInCells, (float)Grid.HeightInCells) * 1.1f;
			Vector3 position = base.transform.GetPosition();
			Vector3 vector4 = position + new Vector3(this.velocity.x * dt, this.velocity.y * dt, 0f);
			int num = Grid.PosToCell(vector4);
			this.loopingSounds.UpdateVelocity(this.flyingSound, vector4 - position);
			Element element = ElementLoader.FindElementByHash(this.EXHAUST_ELEMENT);
			if (this.EXHAUST_ELEMENT != SimHashes.Void && Grid.IsValidCell(num) && !Grid.Solid[num])
			{
				SimMessages.EmitMass(num, element.idx, dt * this.EXHAUST_RATE, element.defaultValues.temperature, this.diseaseIdx, Mathf.RoundToInt((float)this.addDiseaseCount * dt), -1);
			}
			if (vector4.x < vector2.x || vector3.x < vector4.x || vector4.y < vector2.y)
			{
				global::Util.KDestroyGameObject(base.gameObject);
			}
			int num2 = Grid.PosToCell(this);
			int num3 = Grid.PosToCell(this.previousPosition);
			if (num2 != num3)
			{
				if (Grid.IsValidCell(num2) && Grid.Solid[num2])
				{
					PrimaryElement component = base.GetComponent<PrimaryElement>();
					this.remainingTileDamage = this.DamageTiles(num2, num3, this.remainingTileDamage);
					if (this.remainingTileDamage <= 0f)
					{
						this.Explode(position, num2, num3, component.Element);
						this.hasExploded = true;
						if (this.destroyOnExplode)
						{
							global::Util.KDestroyGameObject(base.gameObject);
						}
						return;
					}
				}
				else
				{
					GameObject ignoreObject = (this.ignoreObstacleForDamage.Get() == null) ? null : this.ignoreObstacleForDamage.Get().gameObject;
					this.DamageThings(position, num2, this.entityDamage, ignoreObject);
				}
			}
			if (this.canHitDuplicants && this.age > 0.25f && Grid.Objects[Grid.PosToCell(position), 0] != null)
			{
				base.transform.position = Grid.CellToPos(Grid.PosToCell(position));
				this.Explode(position, num2, num3, base.GetComponent<PrimaryElement>().Element);
				if (this.destroyOnExplode)
				{
					global::Util.KDestroyGameObject(base.gameObject);
				}
				return;
			}
			this.previousPosition = position;
			base.transform.SetPosition(vector4);
		}
		this.age += dt;
	}

	// Token: 0x060058FC RID: 22780 RVA: 0x0029BF90 File Offset: 0x0029A190
	private void PlayImpactSound(Vector3 pos)
	{
		if (this.impactSound == null)
		{
			this.impactSound = "Meteor_Large_Impact";
		}
		this.loopingSounds.StopSound(this.flyingSound);
		string sound = GlobalAssets.GetSound(this.impactSound, false);
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			float volume = this.GetVolume(base.gameObject);
			pos.z = 0f;
			EventInstance instance = KFMOD.BeginOneShot(sound, pos, volume);
			instance.setParameterByName("userVolume_SFX", KPlayerPrefs.GetFloat("Volume_SFX"), false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x060058FD RID: 22781 RVA: 0x000DE6E3 File Offset: 0x000DC8E3
	private void StartLoopingSound()
	{
		this.loopingSounds.StartSound(this.flyingSound);
		this.loopingSounds.UpdateFirstParameter(this.flyingSound, this.FLYING_SOUND_ID_PARAMETER, (float)this.flyingSoundID);
	}

	// Token: 0x060058FE RID: 22782 RVA: 0x0029C034 File Offset: 0x0029A234
	public void Explode()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Vector3 position = base.transform.GetPosition();
		int num = Grid.PosToCell(position);
		this.Explode(position, num, num, component.Element);
		this.hasExploded = true;
		if (this.destroyOnExplode)
		{
			global::Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x04003EAF RID: 16047
	public SimHashes EXHAUST_ELEMENT = SimHashes.CarbonDioxide;

	// Token: 0x04003EB0 RID: 16048
	public float EXHAUST_RATE = 50f;

	// Token: 0x04003EB1 RID: 16049
	public Vector2 spawnVelocity = new Vector2(12f, 15f);

	// Token: 0x04003EB2 RID: 16050
	public Vector2 spawnAngle = new Vector2(-100f, -80f);

	// Token: 0x04003EB3 RID: 16051
	public Vector2 massRange;

	// Token: 0x04003EB4 RID: 16052
	public Vector2 temperatureRange;

	// Token: 0x04003EB5 RID: 16053
	public SpawnFXHashes explosionEffectHash;

	// Token: 0x04003EB6 RID: 16054
	public int splashRadius = 1;

	// Token: 0x04003EB7 RID: 16055
	public int addTiles;

	// Token: 0x04003EB8 RID: 16056
	public int addTilesMinHeight;

	// Token: 0x04003EB9 RID: 16057
	public int addTilesMaxHeight;

	// Token: 0x04003EBA RID: 16058
	public int entityDamage = 1;

	// Token: 0x04003EBB RID: 16059
	public float totalTileDamage = 0.2f;

	// Token: 0x04003EBC RID: 16060
	protected float addTileMass;

	// Token: 0x04003EBD RID: 16061
	public int addDiseaseCount;

	// Token: 0x04003EBE RID: 16062
	public byte diseaseIdx = byte.MaxValue;

	// Token: 0x04003EBF RID: 16063
	public Vector2 elementReplaceTileTemperatureRange = new Vector2(800f, 1000f);

	// Token: 0x04003EC0 RID: 16064
	public Vector2I explosionOreCount = new Vector2I(0, 0);

	// Token: 0x04003EC1 RID: 16065
	private float explosionMass;

	// Token: 0x04003EC2 RID: 16066
	public Vector2 explosionTemperatureRange = new Vector2(500f, 700f);

	// Token: 0x04003EC3 RID: 16067
	public Vector2 explosionSpeedRange = new Vector2(8f, 14f);

	// Token: 0x04003EC4 RID: 16068
	public float windowDamageMultiplier = 5f;

	// Token: 0x04003EC5 RID: 16069
	public float bunkerDamageMultiplier;

	// Token: 0x04003EC6 RID: 16070
	public string impactSound;

	// Token: 0x04003EC7 RID: 16071
	public string flyingSound;

	// Token: 0x04003EC8 RID: 16072
	public int flyingSoundID;

	// Token: 0x04003EC9 RID: 16073
	private HashedString FLYING_SOUND_ID_PARAMETER = "meteorType";

	// Token: 0x04003ECA RID: 16074
	public bool affectedByDifficulty = true;

	// Token: 0x04003ECB RID: 16075
	public bool Targeted;

	// Token: 0x04003ECC RID: 16076
	[Serialize]
	protected Vector3 offsetPosition;

	// Token: 0x04003ECD RID: 16077
	[Serialize]
	protected Vector2 velocity;

	// Token: 0x04003ECE RID: 16078
	[Serialize]
	private float remainingTileDamage;

	// Token: 0x04003ECF RID: 16079
	private Vector3 previousPosition;

	// Token: 0x04003ED0 RID: 16080
	private bool hasExploded;

	// Token: 0x04003ED1 RID: 16081
	public bool canHitDuplicants;

	// Token: 0x04003ED2 RID: 16082
	public string[] craterPrefabs;

	// Token: 0x04003ED3 RID: 16083
	public string[] lootOnDestroyedByMissile;

	// Token: 0x04003ED4 RID: 16084
	public bool destroyOnExplode = true;

	// Token: 0x04003ED5 RID: 16085
	public bool spawnWithOffset;

	// Token: 0x04003ED6 RID: 16086
	private float age;

	// Token: 0x04003ED7 RID: 16087
	public System.Action OnImpact;

	// Token: 0x04003ED8 RID: 16088
	public Ref<KPrefabID> ignoreObstacleForDamage = new Ref<KPrefabID>();

	// Token: 0x04003ED9 RID: 16089
	[MyCmpGet]
	private KBatchedAnimController anim;

	// Token: 0x04003EDA RID: 16090
	[MyCmpGet]
	private KSelectable selectable;

	// Token: 0x04003EDB RID: 16091
	public Tag typeID;

	// Token: 0x04003EDC RID: 16092
	private LoopingSounds loopingSounds;

	// Token: 0x04003EDD RID: 16093
	private List<GameObject> damagedEntities = new List<GameObject>();

	// Token: 0x04003EDE RID: 16094
	private List<int> destroyedCells = new List<int>();

	// Token: 0x04003EDF RID: 16095
	private const float MAX_DISTANCE_TEST = 6f;
}
