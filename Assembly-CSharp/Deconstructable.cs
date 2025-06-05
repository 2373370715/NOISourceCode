using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A5C RID: 2652
[AddComponentMenu("KMonoBehaviour/Workable/Deconstructable")]
public class Deconstructable : Workable
{
	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x06002FDD RID: 12253 RVA: 0x00207094 File Offset: 0x00205294
	private CellOffset[] placementOffsets
	{
		get
		{
			Building component = base.GetComponent<Building>();
			if (component != null)
			{
				CellOffset[] array = component.Def.PlacementOffsets;
				Rotatable component2 = component.GetComponent<Rotatable>();
				if (component2 != null)
				{
					array = new CellOffset[component.Def.PlacementOffsets.Length];
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = component2.GetRotatedCellOffset(component.Def.PlacementOffsets[i]);
					}
				}
				return array;
			}
			OccupyArea component3 = base.GetComponent<OccupyArea>();
			if (component3 != null)
			{
				return component3.OccupiedCellsOffsets;
			}
			if (this.looseEntityDeconstructable)
			{
				return new CellOffset[]
				{
					new CellOffset(0, 0)
				};
			}
			global::Debug.Assert(false, "Ack! We put a Deconstructable on something that's neither a Building nor OccupyArea!", this);
			return null;
		}
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x00207158 File Offset: 0x00205358
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
		this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.minimumAttributeMultiplier = 0.75f;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.multitoolContext = "build";
		this.multitoolHitEffectTag = EffectConfigs.BuildSplashId;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		if (this.customWorkTime > 0f)
		{
			base.SetWorkTime(this.customWorkTime);
			return;
		}
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			base.SetWorkTime(component.Def.ConstructionTime * 0.5f);
			return;
		}
		base.SetWorkTime(30f);
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x00207268 File Offset: 0x00205468
	protected override void OnSpawn()
	{
		base.OnSpawn();
		CellOffset[] filter = null;
		CellOffset[][] table = OffsetGroups.InvertedStandardTable;
		Building component = base.GetComponent<Building>();
		if (component != null && component.Def.IsTilePiece)
		{
			table = OffsetGroups.InvertedStandardTableWithCorners;
			filter = component.Def.ConstructionOffsetFilter;
		}
		CellOffset[][] offsetTable = OffsetGroups.BuildReachabilityTable(this.placementOffsets, table, filter);
		base.SetOffsetTable(offsetTable);
		base.Subscribe<Deconstructable>(493375141, Deconstructable.OnRefreshUserMenuDelegate);
		base.Subscribe<Deconstructable>(-111137758, Deconstructable.OnRefreshUserMenuDelegate);
		base.Subscribe<Deconstructable>(2127324410, Deconstructable.OnCancelDelegate);
		base.Subscribe<Deconstructable>(-790448070, Deconstructable.OnDeconstructDelegate);
		if (this.constructionElements == null || this.constructionElements.Length == 0)
		{
			this.constructionElements = new Tag[1];
			this.constructionElements[0] = base.GetComponent<PrimaryElement>().Element.tag;
		}
		if (this.isMarkedForDeconstruction)
		{
			this.QueueDeconstruction(false);
		}
		this.reconstructable = base.GetComponent<Reconstructable>();
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x00207360 File Offset: 0x00205560
	protected override void OnStartWork(WorkerBase worker)
	{
		this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
		base.Trigger(1830962028, this);
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x002073B0 File Offset: 0x002055B0
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.Trigger(-702296337, this);
		if (this.reconstructable != null)
		{
			this.reconstructable.TryCommenceReconstruct();
		}
		Building component = base.GetComponent<Building>();
		SimCellOccupier component2 = base.GetComponent<SimCellOccupier>();
		if (DetailsScreen.Instance != null && DetailsScreen.Instance.CompareTargetWith(base.gameObject))
		{
			DetailsScreen.Instance.Show(false);
		}
		PrimaryElement component3 = base.GetComponent<PrimaryElement>();
		float temperature = component3.Temperature;
		byte disease_idx = component3.DiseaseIdx;
		int disease_count = component3.DiseaseCount;
		if (component2 != null)
		{
			if (component.Def.TileLayer != ObjectLayer.NumLayers)
			{
				int num = Grid.PosToCell(base.transform.GetPosition());
				if (Grid.Objects[num, (int)component.Def.TileLayer] == base.gameObject)
				{
					Grid.Objects[num, (int)component.Def.ObjectLayer] = null;
					Grid.Objects[num, (int)component.Def.TileLayer] = null;
					Grid.Foundation[num] = false;
					TileVisualizer.RefreshCell(num, component.Def.TileLayer, component.Def.ReplacementLayer);
				}
			}
			component2.DestroySelf(delegate
			{
				this.TriggerDestroy(temperature, disease_idx, disease_count, worker);
			});
		}
		else
		{
			this.TriggerDestroy(temperature, disease_idx, disease_count);
		}
		if (component == null || component.Def.PlayConstructionSounds)
		{
			string sound = GlobalAssets.GetSound("Finish_Deconstruction_" + ((!this.audioSize.IsNullOrWhiteSpace()) ? this.audioSize : component.Def.AudioSize), false);
			if (sound != null)
			{
				KMonoBehaviour.PlaySound3DAtLocation(sound, base.gameObject.transform.GetPosition());
			}
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06002FE2 RID: 12258 RVA: 0x000C3904 File Offset: 0x000C1B04
	public bool HasBeenDestroyed
	{
		get
		{
			return this.destroyed;
		}
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x00207598 File Offset: 0x00205798
	public List<GameObject> ForceDestroyAndGetMaterials()
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float temperature = component.Temperature;
		byte diseaseIdx = component.DiseaseIdx;
		int diseaseCount = component.DiseaseCount;
		return this.TriggerDestroy(temperature, diseaseIdx, diseaseCount);
	}

	// Token: 0x06002FE4 RID: 12260 RVA: 0x002075C8 File Offset: 0x002057C8
	private List<GameObject> TriggerDestroy(float temperature, byte disease_idx, int disease_count, WorkerBase tile_worker)
	{
		if (this == null || this.destroyed)
		{
			return null;
		}
		if (base.transform.parent != null)
		{
			Storage component = base.transform.parent.GetComponent<Storage>();
			if (component != null)
			{
				component.Remove(base.gameObject, true);
			}
		}
		List<GameObject> result = this.SpawnItemsFromConstruction(temperature, disease_idx, disease_count, tile_worker);
		this.destroyed = true;
		base.gameObject.DeleteObject();
		return result;
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000C390C File Offset: 0x000C1B0C
	private List<GameObject> TriggerDestroy(float temperature, byte disease_idx, int disease_count)
	{
		return this.TriggerDestroy(temperature, disease_idx, disease_count, base.worker);
	}

	// Token: 0x06002FE6 RID: 12262 RVA: 0x00207640 File Offset: 0x00205840
	public void QueueDeconstruction(bool userTriggered)
	{
		if (userTriggered && DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.chore == null)
		{
			BuildingComplete component = base.GetComponent<BuildingComplete>();
			if (component != null && component.Def.ReplacementLayer != ObjectLayer.NumLayers)
			{
				int cell = Grid.PosToCell(component);
				if (Grid.Objects[cell, (int)component.Def.ReplacementLayer] != null)
				{
					return;
				}
			}
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Deconstructable>(Db.Get().ChoreTypes.Deconstruct, this, null, true, null, null, null, true, null, false, false, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, this);
			this.isMarkedForDeconstruction = true;
			base.Trigger(2108245096, "Deconstruct");
		}
	}

	// Token: 0x06002FE7 RID: 12263 RVA: 0x000C391D File Offset: 0x000C1B1D
	private void QueueDeconstruction()
	{
		this.QueueDeconstruction(true);
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x000C3926 File Offset: 0x000C1B26
	private void OnDeconstruct()
	{
		if (this.chore == null)
		{
			this.QueueDeconstruction();
			return;
		}
		this.CancelDeconstruction();
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x000C393D File Offset: 0x000C1B3D
	public bool IsMarkedForDeconstruction()
	{
		return this.chore != null;
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x000C3948 File Offset: 0x000C1B48
	public void SetAllowDeconstruction(bool allow)
	{
		this.allowDeconstruction = allow;
		if (!this.allowDeconstruction)
		{
			this.CancelDeconstruction();
		}
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x0020771C File Offset: 0x0020591C
	public void SpawnItemsFromConstruction(WorkerBase chore_worker)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		float temperature = component.Temperature;
		byte diseaseIdx = component.DiseaseIdx;
		int diseaseCount = component.DiseaseCount;
		this.SpawnItemsFromConstruction(temperature, diseaseIdx, diseaseCount, chore_worker);
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x00207750 File Offset: 0x00205950
	private List<GameObject> SpawnItemsFromConstruction(float temperature, byte disease_idx, int disease_count, WorkerBase construction_worker)
	{
		List<GameObject> list = new List<GameObject>();
		if (!this.allowDeconstruction)
		{
			return list;
		}
		Building component = base.GetComponent<Building>();
		float[] array;
		if (component != null)
		{
			array = component.Def.Mass;
		}
		else
		{
			array = new float[]
			{
				base.GetComponent<PrimaryElement>().Mass
			};
		}
		int num = 0;
		while (num < this.constructionElements.Length && array.Length > num)
		{
			GameObject gameObject = this.SpawnItem(base.transform.GetPosition(), this.constructionElements[num], array[num], temperature, disease_idx, disease_count, construction_worker);
			int num2 = Grid.PosToCell(gameObject.transform.GetPosition());
			int num3 = Grid.CellAbove(num2);
			Vector2 zero;
			if ((Grid.IsValidCell(num2) && Grid.Solid[num2]) || (Grid.IsValidCell(num3) && Grid.Solid[num3]))
			{
				zero = Vector2.zero;
			}
			else
			{
				zero = new Vector2(UnityEngine.Random.Range(-1f, 1f) * Deconstructable.INITIAL_VELOCITY_RANGE.x, Deconstructable.INITIAL_VELOCITY_RANGE.y);
			}
			if (GameComps.Fallers.Has(gameObject))
			{
				GameComps.Fallers.Remove(gameObject);
			}
			GameComps.Fallers.Add(gameObject, zero);
			list.Add(gameObject);
			num++;
		}
		return list;
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x0020789C File Offset: 0x00205A9C
	public GameObject SpawnItem(Vector3 position, Tag src_element, float src_mass, float src_temperature, byte disease_idx, int disease_count, WorkerBase chore_worker)
	{
		GameObject gameObject = null;
		int cell = Grid.PosToCell(position);
		CellOffset[] placementOffsets = this.placementOffsets;
		Element element = ElementLoader.GetElement(src_element);
		if (element != null)
		{
			float num = src_mass;
			int num2 = 0;
			while ((float)num2 < src_mass / 400f)
			{
				int num3 = num2 % placementOffsets.Length;
				int cell2 = Grid.OffsetCell(cell, placementOffsets[num3]);
				float mass = num;
				if (num > 400f)
				{
					mass = 400f;
					num -= 400f;
				}
				gameObject = element.substance.SpawnResource(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), mass, src_temperature, disease_idx, disease_count, false, false, false);
				gameObject.Trigger(580035959, chore_worker);
				num2++;
			}
		}
		else
		{
			int num4 = 0;
			while ((float)num4 < src_mass)
			{
				int num5 = num4 % placementOffsets.Length;
				int cell3 = Grid.OffsetCell(cell, placementOffsets[num5]);
				gameObject = GameUtil.KInstantiate(Assets.GetPrefab(src_element), Grid.CellToPosCBC(cell3, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, null, 0);
				gameObject.SetActive(true);
				gameObject.Trigger(580035959, chore_worker);
				num4++;
			}
		}
		return gameObject;
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x0020799C File Offset: 0x00205B9C
	private void OnRefreshUserMenu(object data)
	{
		if (!this.allowDeconstruction)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore == null) ? new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DECONSTRUCT.NAME, new System.Action(this.OnDeconstruct), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.DECONSTRUCT.NAME_OFF, new System.Action(this.OnDeconstruct), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DECONSTRUCT.TOOLTIP_OFF, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 0f);
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x00207A40 File Offset: 0x00205C40
	public void CancelDeconstruction()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancelled deconstruction");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
			base.ShowProgressBar(false);
			this.isMarkedForDeconstruction = false;
			Prioritizable.RemoveRef(base.gameObject);
			Reconstructable component = base.GetComponent<Reconstructable>();
			if (component != null)
			{
				component.CancelReconstructOrder();
			}
		}
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x000C395F File Offset: 0x000C1B5F
	private void OnCancel(object data)
	{
		this.CancelDeconstruction();
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x000C3967 File Offset: 0x000C1B67
	private void OnDeconstruct(object data)
	{
		if (this.allowDeconstruction || DebugHandler.InstantBuildMode)
		{
			this.QueueDeconstruction();
		}
	}

	// Token: 0x040020F2 RID: 8434
	public Chore chore;

	// Token: 0x040020F3 RID: 8435
	public bool allowDeconstruction = true;

	// Token: 0x040020F4 RID: 8436
	public string audioSize;

	// Token: 0x040020F5 RID: 8437
	public float customWorkTime = -1f;

	// Token: 0x040020F6 RID: 8438
	private Reconstructable reconstructable;

	// Token: 0x040020F7 RID: 8439
	[Serialize]
	private bool isMarkedForDeconstruction;

	// Token: 0x040020F8 RID: 8440
	[Serialize]
	public Tag[] constructionElements;

	// Token: 0x040020F9 RID: 8441
	public bool looseEntityDeconstructable;

	// Token: 0x040020FA RID: 8442
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040020FB RID: 8443
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnCancel(data);
	});

	// Token: 0x040020FC RID: 8444
	private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Deconstructable>(delegate(Deconstructable component, object data)
	{
		component.OnDeconstruct(data);
	});

	// Token: 0x040020FD RID: 8445
	private static readonly Vector2 INITIAL_VELOCITY_RANGE = new Vector2(0.5f, 4f);

	// Token: 0x040020FE RID: 8446
	private bool destroyed;
}
