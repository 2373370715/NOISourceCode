using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020012B4 RID: 4788
public class Electrobank : KMonoBehaviour, ISim1000ms, ISim200ms, IConsumableUIItem, IGameObjectEffectDescriptor
{
	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x060061CE RID: 25038 RVA: 0x000E4334 File Offset: 0x000E2534
	// (set) Token: 0x060061CD RID: 25037 RVA: 0x000E432B File Offset: 0x000E252B
	public string ID { get; private set; }

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x060061CF RID: 25039 RVA: 0x000E433C File Offset: 0x000E253C
	public bool IsFullyCharged
	{
		get
		{
			return this.charge == Electrobank.capacity;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x060061D0 RID: 25040 RVA: 0x000E434B File Offset: 0x000E254B
	public float Charge
	{
		get
		{
			return this.charge;
		}
	}

	// Token: 0x060061D1 RID: 25041 RVA: 0x002C2900 File Offset: 0x002C0B00
	protected override void OnPrefabInit()
	{
		this.ID = base.gameObject.PrefabID().ToString();
		base.Subscribe(748399584, new Action<object>(this.OnCraft));
		base.OnPrefabInit();
	}

	// Token: 0x060061D2 RID: 25042 RVA: 0x002C294C File Offset: 0x002C0B4C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe(856640610, new Action<object>(this.ClearHealthBar));
		Components.Electrobanks.Add(base.gameObject.GetMyWorldId(), this);
		this.radiationEmitter = base.GetComponent<RadiationEmitter>();
		this.UpdateRadiationEmitter();
	}

	// Token: 0x060061D3 RID: 25043 RVA: 0x000E4353 File Offset: 0x000E2553
	private void OnCraft(object data)
	{
		WorldResourceAmountTracker<ElectrobankTracker>.Get().RegisterAmountProduced(this.Charge);
	}

	// Token: 0x060061D4 RID: 25044 RVA: 0x002C29A0 File Offset: 0x002C0BA0
	private void UpdateRadiationEmitter()
	{
		if (this.radiationEmitter == null)
		{
			return;
		}
		bool flag = this.timeSincePowerDrawn < 0.5f;
		this.radiationEmitter.emitRads = (flag ? this.radioactivityTuning : 0f);
		this.radiationEmitter.Refresh();
	}

	// Token: 0x060061D5 RID: 25045 RVA: 0x002C29F0 File Offset: 0x002C0BF0
	public static GameObject ReplaceEmptyWithCharged(GameObject EmptyElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = EmptyElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Electrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(EmptyElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = EmptyElectrobank.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Remove(EmptyElectrobank, true);
		}
		EmptyElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060061D6 RID: 25046 RVA: 0x002C2A80 File Offset: 0x002C0C80
	public static GameObject ReplaceChargedWithEmpty(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("EmptyElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Remove(ChargedElectrobank, true);
		}
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060061D7 RID: 25047 RVA: 0x002C2B10 File Offset: 0x002C0D10
	public static GameObject ReplaceEmptyWithGarbage(GameObject ChargedElectrobank, bool dropFromStorage = false)
	{
		Vector3 position = ChargedElectrobank.transform.GetPosition();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("GarbageElectrobank"), position);
		gameObject.GetComponent<PrimaryElement>().SetElement(ChargedElectrobank.GetComponent<PrimaryElement>().Element.id, true);
		gameObject.SetActive(true);
		Storage storage = ChargedElectrobank.GetComponent<Pickupable>().storage;
		if (storage != null)
		{
			storage.Remove(ChargedElectrobank, true);
		}
		ChargedElectrobank.DeleteObject();
		if (storage != null && !dropFromStorage)
		{
			storage.Store(gameObject, false, false, true, false);
		}
		return gameObject;
	}

	// Token: 0x060061D8 RID: 25048 RVA: 0x002C2BA0 File Offset: 0x002C0DA0
	public float AddPower(float joules)
	{
		if (joules < 0f)
		{
			joules = 0f;
		}
		float num = Mathf.Min(joules, Electrobank.capacity - this.charge);
		this.charge += num;
		return num;
	}

	// Token: 0x060061D9 RID: 25049 RVA: 0x002C2BE0 File Offset: 0x002C0DE0
	public float RemovePower(float joules, bool dropWhenEmpty)
	{
		float num = Mathf.Min(this.charge, joules);
		this.charge -= num;
		if (this.charge <= 0f)
		{
			this.OnEmpty(dropWhenEmpty);
		}
		if (num > 0f)
		{
			this.timeSincePowerDrawn = 0f;
		}
		return num;
	}

	// Token: 0x060061DA RID: 25050 RVA: 0x002C2C30 File Offset: 0x002C0E30
	protected virtual void OnEmpty(bool dropWhenEmpty)
	{
		if (this.rechargeable)
		{
			Electrobank.ReplaceChargedWithEmpty(base.gameObject, dropWhenEmpty);
			return;
		}
		if (!this.keepEmpty)
		{
			if (this.pickupable.storage != null)
			{
				this.pickupable.storage.Remove(base.gameObject, true);
			}
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x060061DB RID: 25051 RVA: 0x000E4365 File Offset: 0x000E2565
	public void FullyCharge()
	{
		this.charge = Electrobank.capacity;
	}

	// Token: 0x060061DC RID: 25052 RVA: 0x002C2C90 File Offset: 0x002C0E90
	public virtual void Explode()
	{
		int num = Grid.PosToCell(base.gameObject.transform.position);
		float num2 = Grid.Temperature[num];
		num2 += this.charge / (Grid.Mass[num] * Grid.Element[num].specificHeatCapacity);
		num2 = Mathf.Clamp(num2, 1f, 9999f);
		SimMessages.ReplaceElement(num, Grid.Element[num].id, CellEventLogger.Instance.SandBoxTool, Grid.Mass[num], num2, Grid.DiseaseIdx[num], Grid.DiseaseCount[num], -1);
		Game.Instance.SpawnFX(SpawnFXHashes.MeteorImpactMetal, base.gameObject.transform.position, 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_explode", false), base.gameObject.transform.position, 1f);
		if (this.rechargeable)
		{
			Electrobank.ReplaceEmptyWithGarbage(base.gameObject, false);
			return;
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x060061DD RID: 25053 RVA: 0x002C2D9C File Offset: 0x002C0F9C
	protected void LaunchNearbyStuff()
	{
		ListPool<ScenePartitionerEntry, Comet>.PooledList pooledList = ListPool<ScenePartitionerEntry, Comet>.Allocate();
		Vector3 position = base.transform.position;
		GameScenePartitioner.Instance.GatherEntries((int)position.x - 3, (int)position.y - 3, 6, 6, GameScenePartitioner.Instance.pickupablesLayer, pooledList);
		foreach (ScenePartitionerEntry scenePartitionerEntry in pooledList)
		{
			GameObject gameObject = (scenePartitionerEntry.obj as Pickupable).gameObject;
			if (!(gameObject.GetComponent<MinionIdentity>() != null) && !(gameObject.GetComponent<CreatureBrain>() != null) && gameObject.GetDef<RobotAi.Def>() == null)
			{
				Vector2 vector = gameObject.transform.GetPosition() - position;
				vector = vector.normalized;
				vector *= (float)UnityEngine.Random.Range(4, 6);
				vector.y += (float)UnityEngine.Random.Range(2, 4);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				if (GameComps.Gravities.Has(gameObject))
				{
					GameComps.Gravities.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, vector);
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x060061DE RID: 25054 RVA: 0x000E4372 File Offset: 0x000E2572
	public void Sim1000ms(float dt)
	{
		if (this.pickupable.KPrefabID.HasTag(GameTags.Stored))
		{
			return;
		}
		this.EvaluateWaterDamage(dt);
		this.UpdateHealthBar();
	}

	// Token: 0x060061DF RID: 25055 RVA: 0x000E4399 File Offset: 0x000E2599
	public virtual void Sim200ms(float dt)
	{
		this.UpdateRadiationEmitter();
		this.timeSincePowerDrawn = Mathf.Min(this.timeSincePowerDrawn + dt, 10f);
	}

	// Token: 0x060061E0 RID: 25056 RVA: 0x002C2EEC File Offset: 0x002C10EC
	private void EvaluateWaterDamage(float dt)
	{
		if (Grid.IsValidCell(this.pickupable.cachedCell) && Grid.Element[this.pickupable.cachedCell].HasTag(GameTags.AnyWater) && UnityEngine.Random.Range(1, 101) > 75)
		{
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.POWER_BANK_WATER_DAMAGE, base.transform, 1.5f, false);
			this.Damage(UnityEngine.Random.Range(0f, dt));
		}
	}

	// Token: 0x060061E1 RID: 25057 RVA: 0x002C2F70 File Offset: 0x002C1170
	public void Damage(float amount)
	{
		Game.Instance.SpawnFX(SpawnFXHashes.ElectrobankDamage, Grid.PosToCell(base.gameObject), 0f);
		KFMOD.PlayOneShot(GlobalAssets.GetSound("Battery_sparks_short", false), base.gameObject.transform.position, 1f);
		this.currentHealth -= amount;
		if (this.healthBar == null)
		{
			this.CreateHealthBar();
		}
		this.healthBar.Update();
		this.lastDamageTime = Time.time;
		if (this.currentHealth <= 0f)
		{
			this.Explode();
		}
	}

	// Token: 0x060061E2 RID: 25058 RVA: 0x000E43B9 File Offset: 0x000E25B9
	protected override void OnCleanUp()
	{
		this.ClearHealthBar(null);
		Components.Electrobanks.Remove(base.gameObject.GetMyWorldId(), this);
		base.OnCleanUp();
	}

	// Token: 0x060061E3 RID: 25059 RVA: 0x000E43DE File Offset: 0x000E25DE
	public void CreateHealthBar()
	{
		this.healthBar = ProgressBar.CreateProgressBar(base.gameObject, () => this.currentHealth / 10f);
		this.healthBar.SetVisibility(true);
		this.healthBar.barColor = Util.ColorFromHex("CC3333");
	}

	// Token: 0x060061E4 RID: 25060 RVA: 0x000E441E File Offset: 0x000E261E
	public void UpdateHealthBar()
	{
		if (this.healthBar != null && Time.time - this.lastDamageTime > 5f)
		{
			this.ClearHealthBar(null);
		}
	}

	// Token: 0x060061E5 RID: 25061 RVA: 0x000E4448 File Offset: 0x000E2648
	public void ClearHealthBar(object data = null)
	{
		if (this.healthBar != null)
		{
			Util.KDestroyGameObject(this.healthBar);
			this.healthBar = null;
		}
	}

	// Token: 0x060061E6 RID: 25062 RVA: 0x002C300C File Offset: 0x002C120C
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELECTROBANKS, GameUtil.GetFormattedJoules(this.Charge, "F1", GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		list.Add(item);
		return list;
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x060061E7 RID: 25063 RVA: 0x002C3078 File Offset: 0x002C1278
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x060061E8 RID: 25064 RVA: 0x000E446A File Offset: 0x000E266A
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x060061E9 RID: 25065 RVA: 0x000E4472 File Offset: 0x000E2672
	public int MajorOrder
	{
		get
		{
			return 500;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x060061EA RID: 25066 RVA: 0x000B1628 File Offset: 0x000AF828
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x060061EB RID: 25067 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x040045EC RID: 17900
	private static float capacity = 120000f;

	// Token: 0x040045ED RID: 17901
	[Serialize]
	private float charge = Electrobank.capacity;

	// Token: 0x040045EE RID: 17902
	private const float MAX_HEALTH = 10f;

	// Token: 0x040045EF RID: 17903
	[Serialize]
	private float currentHealth = 10f;

	// Token: 0x040045F0 RID: 17904
	[Serialize]
	private float timeSincePowerDrawn = 0.5f;

	// Token: 0x040045F1 RID: 17905
	private const float RADIATION_EMITTER_TIMEOUT = 0.5f;

	// Token: 0x040045F2 RID: 17906
	public float radioactivityTuning;

	// Token: 0x040045F3 RID: 17907
	private RadiationEmitter radiationEmitter;

	// Token: 0x040045F4 RID: 17908
	private float lastDamageTime;

	// Token: 0x040045F5 RID: 17909
	public ProgressBar healthBar;

	// Token: 0x040045F6 RID: 17910
	public bool rechargeable;

	// Token: 0x040045F7 RID: 17911
	public bool keepEmpty;

	// Token: 0x040045F8 RID: 17912
	[MyCmpGet]
	private Pickupable pickupable;
}
