using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005BC RID: 1468
public class SpecialCargoBayClusterReceptacle : SingleEntityReceptacle, IBaggedStateAnimationInstructions
{
	// Token: 0x17000090 RID: 144
	// (get) Token: 0x0600196E RID: 6510 RVA: 0x000B522F File Offset: 0x000B342F
	public bool IsRocketOnGround
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketOnGround);
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x0600196F RID: 6511 RVA: 0x000B5241 File Offset: 0x000B3441
	public bool IsRocketInSpace
	{
		get
		{
			return base.gameObject.HasTag(GameTags.RocketInSpace);
		}
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x06001970 RID: 6512 RVA: 0x000B5253 File Offset: 0x000B3453
	private bool isDoorOpen
	{
		get
		{
			return this.capsule.sm.IsDoorOpen.Get(this.capsule);
		}
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000B5270 File Offset: 0x000B3470
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.choreType = Db.Get().ChoreTypes.CreatureFetch;
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x001AEB30 File Offset: 0x001ACD30
	protected override void OnSpawn()
	{
		this.capsule = base.gameObject.GetSMI<SpecialCargoBayCluster.Instance>();
		this.SetupLootSymbolObject();
		base.OnSpawn();
		this.SetTrappedCritterAnimations(base.Occupant);
		base.Subscribe(-1697596308, new Action<object>(this.OnCritterStorageChanged));
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-887025858, SpecialCargoBayClusterReceptacle.OnRocketLandedDelegate);
		base.Subscribe<SpecialCargoBayClusterReceptacle>(-1447108533, SpecialCargoBayClusterReceptacle.OnCargoBayRelocatedDelegate);
		base.Subscribe(-905833192, new Action<object>(this.OnCopySettings));
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x001AEBB8 File Offset: 0x001ACDB8
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			SpecialCargoBayClusterReceptacle component = gameObject.GetComponent<SpecialCargoBayClusterReceptacle>();
			if (component != null)
			{
				Tag tag = (component.Occupant != null) ? component.Occupant.PrefabID() : component.requestedEntityTag;
				if (base.Occupant != null && base.Occupant.PrefabID() != tag)
				{
					this.ClearOccupant();
				}
				if (tag != this.requestedEntityTag && this.fetchChore != null)
				{
					base.CancelActiveRequest();
				}
				if (tag != Tag.Invalid)
				{
					this.CreateOrder(tag, component.requestedEntityAdditionalFilterTag);
				}
			}
		}
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000B528D File Offset: 0x000B348D
	public override void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		base.CreateOrder(entityTag, additionalFilterTag);
		if (this.fetchChore != null)
		{
			this.fetchChore.AddPrecondition(ChorePreconditions.instance.IsNotARobot, null);
		}
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x001AEC68 File Offset: 0x001ACE68
	public void SetupLootSymbolObject()
	{
		Vector3 storePositionForDrops = this.capsule.GetStorePositionForDrops();
		storePositionForDrops.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
		GameObject gameObject = new GameObject();
		gameObject.name = "lootSymbol";
		gameObject.transform.SetParent(base.transform, true);
		gameObject.SetActive(false);
		gameObject.transform.SetPosition(storePositionForDrops);
		KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddOrGet<KBatchedAnimTracker>();
		kbatchedAnimTracker.symbol = "loot";
		kbatchedAnimTracker.forceAlwaysAlive = true;
		kbatchedAnimTracker.matchParentOffset = true;
		this.lootKBAC = gameObject.AddComponent<KBatchedAnimController>();
		this.lootKBAC.AnimFiles = new KAnimFile[]
		{
			Assets.GetAnim("mushbar_kanim")
		};
		this.lootKBAC.initialAnim = "object";
		this.buildingAnimCtr.SetSymbolVisiblity("loot", false);
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x001AED40 File Offset: 0x001ACF40
	protected override void ClearOccupant()
	{
		this.LastCritterDead = null;
		if (base.occupyingObject != null)
		{
			this.UnsubscribeFromOccupant();
		}
		this.originWorldID = -1;
		base.occupyingObject = null;
		base.UpdateActive();
		this.UpdateStatusItem();
		if (!this.isDoorOpen)
		{
			if (this.IsRocketOnGround)
			{
				this.SetLootSymbolImage(Tag.Invalid);
				this.capsule.OpenDoor();
			}
		}
		else
		{
			this.capsule.DropInventory();
		}
		base.Trigger(-731304873, base.occupyingObject);
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000B52B5 File Offset: 0x000B34B5
	private void OnCritterStorageChanged(object obj)
	{
		if (obj != null && this.storage.MassStored() == 0f && base.Occupant != null && base.Occupant == (GameObject)obj)
		{
			this.ClearOccupant();
		}
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x001AEDC8 File Offset: 0x001ACFC8
	protected override void SubscribeToOccupant()
	{
		base.SubscribeToOccupant();
		base.Subscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Subscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Subscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.SetupCritterTracker();
		for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
		{
			Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
			base.Occupant.AddTag(tag);
		}
		base.Occupant.GetComponent<Health>().UpdateHealthBar();
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x001AEE78 File Offset: 0x001AD078
	protected override void UnsubscribeFromOccupant()
	{
		base.UnsubscribeFromOccupant();
		base.Unsubscribe(base.Occupant, -1582839653, new Action<object>(this.OnTrappedCritterTagsChanged));
		base.Unsubscribe(base.Occupant, 395373363, new Action<object>(this.OnCreatureInStorageDied));
		base.Unsubscribe(base.Occupant, 663420073, new Action<object>(this.OnBabyInStorageGrows));
		this.RemoveCritterTracker();
		if (base.Occupant != null)
		{
			for (int i = 0; i < SpecialCargoBayClusterReceptacle.tagsForCritter.Length; i++)
			{
				Tag tag = SpecialCargoBayClusterReceptacle.tagsForCritter[i];
				base.occupyingObject.RemoveTag(tag);
			}
			base.occupyingObject.GetComponent<Health>().UpdateHealthBar();
		}
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x001AEF30 File Offset: 0x001AD130
	public void SetLootSymbolImage(Tag productTag)
	{
		bool flag = productTag != Tag.Invalid;
		this.lootKBAC.gameObject.SetActive(flag);
		if (flag)
		{
			GameObject prefab = Assets.GetPrefab(productTag.ToString());
			this.lootKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.lootKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x000B52F3 File Offset: 0x000B34F3
	private void SetupCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker kbatchedAnimTracker = base.Occupant.AddOrGet<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = "critter";
			kbatchedAnimTracker.forceAlwaysAlive = true;
			kbatchedAnimTracker.matchParentOffset = true;
		}
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x001AEFAC File Offset: 0x001AD1AC
	private void RemoveCritterTracker()
	{
		if (base.Occupant != null)
		{
			KBatchedAnimTracker component = base.Occupant.GetComponent<KBatchedAnimTracker>();
			if (component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000B532B File Offset: 0x000B352B
	protected override void ConfigureOccupyingObject(GameObject source)
	{
		this.originWorldID = source.GetMyWorldId();
		source.GetComponent<Baggable>().SetWrangled();
		this.SetTrappedCritterAnimations(source);
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x001AEFE4 File Offset: 0x001AD1E4
	private void OnBabyInStorageGrows(object obj)
	{
		int num = this.originWorldID;
		this.UnsubscribeFromOccupant();
		GameObject gameObject = (GameObject)obj;
		this.storage.Store(gameObject, false, false, true, false);
		base.occupyingObject = gameObject;
		this.ConfigureOccupyingObject(gameObject);
		this.originWorldID = num;
		this.PositionOccupyingObject();
		this.SubscribeToOccupant();
		this.UpdateStatusItem();
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x001AF040 File Offset: 0x001AD240
	private void OnTrappedCritterTagsChanged(object obj)
	{
		if (base.Occupant != null && base.Occupant.HasTag(GameTags.Creatures.Die) && this.LastCritterDead != base.Occupant)
		{
			this.capsule.PlayDeathCloud();
			this.LastCritterDead = base.Occupant;
			this.RemoveCritterTracker();
			base.Occupant.GetComponent<KBatchedAnimController>().SetVisiblity(false);
			Butcherable component = base.Occupant.GetComponent<Butcherable>();
			if (component != null && component.drops != null && component.drops.Length != 0)
			{
				this.SetLootSymbolImage(component.drops[0]);
			}
			else
			{
				this.SetLootSymbolImage(Tag.Invalid);
			}
			if (this.IsRocketInSpace)
			{
				DeathStates.Instance smi = base.Occupant.GetSMI<DeathStates.Instance>();
				smi.GoTo(smi.sm.pst);
			}
		}
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x001AF120 File Offset: 0x001AD320
	private void OnCreatureInStorageDied(object drops_obj)
	{
		GameObject[] array = drops_obj as GameObject[];
		if (array != null)
		{
			foreach (GameObject go in array)
			{
				this.sideProductStorage.Store(go, false, false, true, false);
			}
		}
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000B534B File Offset: 0x000B354B
	private void SetTrappedCritterAnimations(GameObject critter)
	{
		if (critter != null)
		{
			KBatchedAnimController component = critter.GetComponent<KBatchedAnimController>();
			component.FlipX = false;
			component.Play("rocket_biological", KAnim.PlayMode.Loop, 1f, 0f);
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000B538B File Offset: 0x000B358B
	protected override void PositionOccupyingObject()
	{
		if (base.Occupant != null)
		{
			base.Occupant.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
			this.SetupCritterTracker();
		}
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x001AF15C File Offset: 0x001AD35C
	protected override void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		bool flag = base.Occupant != null;
		if (component != null)
		{
			if (flag)
			{
				component.AddStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, this);
			}
			else
			{
				component.RemoveStatusItem(Db.Get().BuildingStatusItems.SpecialCargoBayClusterCritterStored, false);
			}
		}
		base.UpdateStatusItem();
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000B53B3 File Offset: 0x000B35B3
	private void OnCargoBayRelocated(object data)
	{
		if (base.Occupant != null)
		{
			KBatchedAnimController component = base.Occupant.GetComponent<KBatchedAnimController>();
			component.enabled = false;
			component.enabled = true;
		}
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x001AF1C0 File Offset: 0x001AD3C0
	private void OnRocketLanded(object data)
	{
		if (base.Occupant != null)
		{
			ClusterManager.Instance.MigrateCritter(base.Occupant, base.gameObject.GetMyWorldId(), this.originWorldID);
			this.originWorldID = base.Occupant.GetMyWorldId();
		}
		if (base.Occupant == null && !this.isDoorOpen)
		{
			this.SetLootSymbolImage(Tag.Invalid);
			if (this.sideProductStorage.MassStored() > 0f)
			{
				this.capsule.OpenDoor();
			}
		}
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x000B53DB File Offset: 0x000B35DB
	public string GetBaggedAnimationName()
	{
		return "rocket_biological";
	}

	// Token: 0x04001087 RID: 4231
	public const string TRAPPED_CRITTER_ANIM_NAME = "rocket_biological";

	// Token: 0x04001088 RID: 4232
	[MyCmpReq]
	private SymbolOverrideController symbolOverrideComponent;

	// Token: 0x04001089 RID: 4233
	[MyCmpGet]
	private KBatchedAnimController buildingAnimCtr;

	// Token: 0x0400108A RID: 4234
	private KBatchedAnimController lootKBAC;

	// Token: 0x0400108B RID: 4235
	public Storage sideProductStorage;

	// Token: 0x0400108C RID: 4236
	private SpecialCargoBayCluster.Instance capsule;

	// Token: 0x0400108D RID: 4237
	private GameObject LastCritterDead;

	// Token: 0x0400108E RID: 4238
	[Serialize]
	private int originWorldID;

	// Token: 0x0400108F RID: 4239
	private static Tag[] tagsForCritter = new Tag[]
	{
		GameTags.Creatures.TrappedInCargoBay,
		GameTags.Creatures.PausedHunger,
		GameTags.Creatures.PausedReproduction,
		GameTags.Creatures.PreventGrowAnimation,
		GameTags.HideHealthBar,
		GameTags.PreventDeadAnimation
	};

	// Token: 0x04001090 RID: 4240
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnRocketLanded(data);
	});

	// Token: 0x04001091 RID: 4241
	private static readonly EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle> OnCargoBayRelocatedDelegate = new EventSystem.IntraObjectHandler<SpecialCargoBayClusterReceptacle>(delegate(SpecialCargoBayClusterReceptacle component, object data)
	{
		component.OnCargoBayRelocated(data);
	});
}
