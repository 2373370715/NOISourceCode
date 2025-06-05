using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02001A54 RID: 6740
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/UnstableGroundManager")]
public class UnstableGroundManager : KMonoBehaviour
{
	// Token: 0x06008C6F RID: 35951 RVA: 0x003724A0 File Offset: 0x003706A0
	protected override void OnPrefabInit()
	{
		this.fallingTileOffset = new Vector3(0.5f, 0f, 0f);
		UnstableGroundManager.EffectInfo[] array = this.effects;
		for (int i = 0; i < array.Length; i++)
		{
			UnstableGroundManager.EffectInfo effectInfo = array[i];
			GameObject prefab = effectInfo.prefab;
			prefab.SetActive(false);
			UnstableGroundManager.EffectRuntimeInfo value = default(UnstableGroundManager.EffectRuntimeInfo);
			GameObjectPool pool = new GameObjectPool(() => this.InstantiateObj(prefab), 16);
			value.pool = pool;
			value.releaseFunc = delegate(GameObject go)
			{
				this.ReleaseGO(go);
				pool.ReleaseInstance(go);
			};
			this.runtimeInfo[effectInfo.element] = value;
		}
	}

	// Token: 0x06008C70 RID: 35952 RVA: 0x001005EB File Offset: 0x000FE7EB
	private void ReleaseGO(GameObject go)
	{
		if (GameComps.Gravities.Has(go))
		{
			GameComps.Gravities.Remove(go);
		}
		go.SetActive(false);
	}

	// Token: 0x06008C71 RID: 35953 RVA: 0x0010060C File Offset: 0x000FE80C
	private GameObject InstantiateObj(GameObject prefab)
	{
		GameObject gameObject = GameUtil.KInstantiate(prefab, Grid.SceneLayer.BuildingBack, null, 0);
		gameObject.SetActive(false);
		gameObject.name = "UnstablePool";
		return gameObject;
	}

	// Token: 0x06008C72 RID: 35954 RVA: 0x00372564 File Offset: 0x00370764
	public void Spawn(int cell, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		Vector3 vector = Grid.CellToPosCCC(cell, Grid.SceneLayer.TileMain);
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			global::Debug.LogError("Tried to spawn unstable ground with NaN temperature");
			temperature = 293f;
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(vector, element, mass, temperature, disease_idx, disease_count);
		kbatchedAnimController.Play("start", KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		kbatchedAnimController.gameObject.name = "Falling " + element.name;
		GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero, null);
		this.fallingObjects.Add(kbatchedAnimController.gameObject);
		this.SpawnPuff(vector, element, mass, temperature, disease_idx, disease_count);
		Substance substance = element.substance;
		if (substance != null && !substance.fallingStartSound.IsNull && CameraController.Instance.IsAudibleSound(vector, substance.fallingStartSound))
		{
			SoundEvent.PlayOneShot(substance.fallingStartSound, vector, 1f);
		}
	}

	// Token: 0x06008C73 RID: 35955 RVA: 0x00372674 File Offset: 0x00370874
	private void SpawnOld(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (!element.IsUnstable)
		{
			global::Debug.LogError("Spawning falling ground with a stable element");
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
		GameComps.Gravities.Add(kbatchedAnimController.gameObject, Vector2.zero, null);
		kbatchedAnimController.Play("loop", KAnim.PlayMode.Loop, 1f, 0f);
		this.fallingObjects.Add(kbatchedAnimController.gameObject);
		kbatchedAnimController.gameObject.name = "SpawnOld " + element.name;
	}

	// Token: 0x06008C74 RID: 35956 RVA: 0x00372704 File Offset: 0x00370904
	private void SpawnPuff(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		if (!element.IsUnstable)
		{
			global::Debug.LogError("Spawning sand puff with a stable element");
		}
		KBatchedAnimController kbatchedAnimController = this.Spawn(pos, element, mass, temperature, disease_idx, disease_count);
		kbatchedAnimController.Play("sandPuff", KAnim.PlayMode.Once, 1f, 0f);
		kbatchedAnimController.gameObject.name = "Puff " + element.name;
		kbatchedAnimController.transform.SetPosition(kbatchedAnimController.transform.GetPosition() + this.spawnPuffOffset);
	}

	// Token: 0x06008C75 RID: 35957 RVA: 0x0037278C File Offset: 0x0037098C
	private KBatchedAnimController Spawn(Vector3 pos, Element element, float mass, float temperature, byte disease_idx, int disease_count)
	{
		UnstableGroundManager.EffectRuntimeInfo effectRuntimeInfo;
		if (!this.runtimeInfo.TryGetValue(element.id, out effectRuntimeInfo))
		{
			global::Debug.LogError(element.id.ToString() + " needs unstable ground info hookup!");
		}
		GameObject instance = effectRuntimeInfo.pool.GetInstance();
		instance.transform.SetPosition(pos);
		if (float.IsNaN(temperature) || float.IsInfinity(temperature))
		{
			global::Debug.LogError("Tried to spawn unstable ground with NaN temperature");
			temperature = 293f;
		}
		UnstableGround component = instance.GetComponent<UnstableGround>();
		component.element = element.id;
		component.mass = mass;
		component.temperature = temperature;
		component.diseaseIdx = disease_idx;
		component.diseaseCount = disease_count;
		instance.SetActive(true);
		KBatchedAnimController component2 = instance.GetComponent<KBatchedAnimController>();
		component2.onDestroySelf = effectRuntimeInfo.releaseFunc;
		component2.Stop();
		if (element.substance != null)
		{
			component2.TintColour = element.substance.colour;
		}
		return component2;
	}

	// Token: 0x06008C76 RID: 35958 RVA: 0x00372874 File Offset: 0x00370A74
	public List<int> GetCellsContainingFallingAbove(Vector2I cellXY)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.fallingObjects.Count; i++)
		{
			Vector2I vector2I;
			Grid.PosToXY(this.fallingObjects[i].transform.GetPosition(), out vector2I);
			if (vector2I.x == cellXY.x && vector2I.y >= cellXY.y)
			{
				int item = Grid.PosToCell(vector2I);
				list.Add(item);
			}
		}
		for (int j = 0; j < this.pendingCells.Count; j++)
		{
			Vector2I vector2I2 = Grid.CellToXY(this.pendingCells[j]);
			if (vector2I2.x == cellXY.x && vector2I2.y >= cellXY.y)
			{
				list.Add(this.pendingCells[j]);
			}
		}
		return list;
	}

	// Token: 0x06008C77 RID: 35959 RVA: 0x0010062A File Offset: 0x000FE82A
	private void RemoveFromPending(int cell)
	{
		this.pendingCells.Remove(cell);
	}

	// Token: 0x06008C78 RID: 35960 RVA: 0x0037294C File Offset: 0x00370B4C
	private void Update()
	{
		if (App.isLoading)
		{
			return;
		}
		int i = 0;
		while (i < this.fallingObjects.Count)
		{
			GameObject gameObject = this.fallingObjects[i];
			if (!(gameObject == null))
			{
				Vector3 position = gameObject.transform.GetPosition();
				int cell = Grid.PosToCell(position);
				Grid.CellRight(cell);
				Grid.CellLeft(cell);
				int num = Grid.CellBelow(cell);
				Grid.CellRight(num);
				Grid.CellLeft(num);
				int cell2 = cell;
				if (!Grid.IsValidCell(num) || Grid.Element[num].IsSolid || (Grid.Properties[num] & 4) != 0)
				{
					UnstableGround component = gameObject.GetComponent<UnstableGround>();
					this.pendingCells.Add(cell2);
					HandleVector<Game.CallbackInfo>.Handle handle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(delegate()
					{
						this.RemoveFromPending(cell);
					}, false));
					SimMessages.AddRemoveSubstance(cell2, component.element, CellEventLogger.Instance.UnstableGround, component.mass, component.temperature, component.diseaseIdx, component.diseaseCount, true, handle.index);
					ListPool<ScenePartitionerEntry, UnstableGroundManager>.PooledList pooledList = ListPool<ScenePartitionerEntry, UnstableGroundManager>.Allocate();
					Vector2I vector2I = Grid.CellToXY(cell);
					vector2I.x = Mathf.Max(0, vector2I.x - 1);
					vector2I.y = Mathf.Min(Grid.HeightInCells - 1, vector2I.y + 1);
					GameScenePartitioner.Instance.GatherEntries(vector2I.x, vector2I.y, 3, 3, GameScenePartitioner.Instance.collisionLayer, pooledList);
					foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
					{
						if (scenePartitionerEntry.obj is KCollider2D)
						{
							(scenePartitionerEntry.obj as KCollider2D).gameObject.Trigger(-975551167, null);
						}
					}
					pooledList.Recycle();
					Element element = ElementLoader.FindElementByHash(component.element);
					if (element != null && element.substance != null && !element.substance.fallingStopSound.IsNull && CameraController.Instance.IsAudibleSound(position, element.substance.fallingStopSound))
					{
						SoundEvent.PlayOneShot(element.substance.fallingStopSound, position, 1f);
					}
					GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.OreAbsorbId), position + this.landEffectOffset, Grid.SceneLayer.Front, null, 0).SetActive(true);
					this.fallingObjects[i] = this.fallingObjects[this.fallingObjects.Count - 1];
					this.fallingObjects.RemoveAt(this.fallingObjects.Count - 1);
					this.ReleaseGO(gameObject);
				}
				else
				{
					i++;
				}
			}
		}
	}

	// Token: 0x06008C79 RID: 35961 RVA: 0x00372C40 File Offset: 0x00370E40
	[OnSerializing]
	private void OnSerializing()
	{
		if (this.fallingObjects.Count > 0)
		{
			this.serializedInfo = new List<UnstableGroundManager.SerializedInfo>();
		}
		foreach (GameObject gameObject in this.fallingObjects)
		{
			UnstableGround component = gameObject.GetComponent<UnstableGround>();
			byte diseaseIdx = component.diseaseIdx;
			int diseaseID = (diseaseIdx != byte.MaxValue) ? Db.Get().Diseases[(int)diseaseIdx].id.HashValue : 0;
			this.serializedInfo.Add(new UnstableGroundManager.SerializedInfo
			{
				position = gameObject.transform.GetPosition(),
				element = component.element,
				mass = component.mass,
				temperature = component.temperature,
				diseaseID = diseaseID,
				diseaseCount = component.diseaseCount
			});
		}
	}

	// Token: 0x06008C7A RID: 35962 RVA: 0x00100639 File Offset: 0x000FE839
	[OnSerialized]
	private void OnSerialized()
	{
		this.serializedInfo = null;
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x00372D44 File Offset: 0x00370F44
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializedInfo == null)
		{
			return;
		}
		this.fallingObjects.Clear();
		HashedString id = default(HashedString);
		foreach (UnstableGroundManager.SerializedInfo serializedInfo in this.serializedInfo)
		{
			Element element = ElementLoader.FindElementByHash(serializedInfo.element);
			id.HashValue = serializedInfo.diseaseID;
			byte index = Db.Get().Diseases.GetIndex(id);
			int disease_count = serializedInfo.diseaseCount;
			if (index == 255)
			{
				disease_count = 0;
			}
			this.SpawnOld(serializedInfo.position, element, serializedInfo.mass, serializedInfo.temperature, index, disease_count);
		}
	}

	// Token: 0x04006A06 RID: 27142
	[SerializeField]
	private Vector3 spawnPuffOffset;

	// Token: 0x04006A07 RID: 27143
	[SerializeField]
	private Vector3 landEffectOffset;

	// Token: 0x04006A08 RID: 27144
	private Vector3 fallingTileOffset;

	// Token: 0x04006A09 RID: 27145
	[SerializeField]
	private UnstableGroundManager.EffectInfo[] effects;

	// Token: 0x04006A0A RID: 27146
	private List<GameObject> fallingObjects = new List<GameObject>();

	// Token: 0x04006A0B RID: 27147
	private List<int> pendingCells = new List<int>();

	// Token: 0x04006A0C RID: 27148
	private Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo> runtimeInfo = new Dictionary<SimHashes, UnstableGroundManager.EffectRuntimeInfo>();

	// Token: 0x04006A0D RID: 27149
	[Serialize]
	private List<UnstableGroundManager.SerializedInfo> serializedInfo;

	// Token: 0x02001A55 RID: 6741
	[Serializable]
	private struct EffectInfo
	{
		// Token: 0x04006A0E RID: 27150
		[HashedEnum]
		public SimHashes element;

		// Token: 0x04006A0F RID: 27151
		public GameObject prefab;
	}

	// Token: 0x02001A56 RID: 6742
	private struct EffectRuntimeInfo
	{
		// Token: 0x04006A10 RID: 27152
		public GameObjectPool pool;

		// Token: 0x04006A11 RID: 27153
		public Action<GameObject> releaseFunc;
	}

	// Token: 0x02001A57 RID: 6743
	private struct SerializedInfo
	{
		// Token: 0x04006A12 RID: 27154
		public Vector3 position;

		// Token: 0x04006A13 RID: 27155
		public SimHashes element;

		// Token: 0x04006A14 RID: 27156
		public float mass;

		// Token: 0x04006A15 RID: 27157
		public float temperature;

		// Token: 0x04006A16 RID: 27158
		public int diseaseID;

		// Token: 0x04006A17 RID: 27159
		public int diseaseCount;
	}
}
