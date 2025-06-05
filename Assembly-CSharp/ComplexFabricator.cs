using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000D20 RID: 3360
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ComplexFabricator")]
public class ComplexFabricator : RemoteDockWorkTargetComponent, ISim200ms, ISim1000ms
{
	// Token: 0x17000313 RID: 787
	// (get) Token: 0x060040A2 RID: 16546 RVA: 0x000CE5A9 File Offset: 0x000CC7A9
	public ComplexFabricatorWorkable Workable
	{
		get
		{
			return this.workable;
		}
	}

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x060040A3 RID: 16547 RVA: 0x000CE5B1 File Offset: 0x000CC7B1
	// (set) Token: 0x060040A4 RID: 16548 RVA: 0x000CE5B9 File Offset: 0x000CC7B9
	public bool ForbidMutantSeeds
	{
		get
		{
			return this.forbidMutantSeeds;
		}
		set
		{
			this.forbidMutantSeeds = value;
			this.ToggleMutantSeedFetches();
			this.UpdateMutantSeedStatusItem();
		}
	}

	// Token: 0x17000315 RID: 789
	// (get) Token: 0x060040A5 RID: 16549 RVA: 0x000CE5CE File Offset: 0x000CC7CE
	public Tag[] ForbiddenTags
	{
		get
		{
			if (!this.forbidMutantSeeds)
			{
				return null;
			}
			return this.forbiddenMutantTags;
		}
	}

	// Token: 0x17000316 RID: 790
	// (get) Token: 0x060040A6 RID: 16550 RVA: 0x000CE5E0 File Offset: 0x000CC7E0
	public int CurrentOrderIdx
	{
		get
		{
			return this.nextOrderIdx;
		}
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x060040A7 RID: 16551 RVA: 0x000CE5E8 File Offset: 0x000CC7E8
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!this.HasWorkingOrder)
			{
				return null;
			}
			return this.recipe_list[this.workingOrderIdx];
		}
	}

	// Token: 0x17000318 RID: 792
	// (get) Token: 0x060040A8 RID: 16552 RVA: 0x000CE601 File Offset: 0x000CC801
	public ComplexRecipe NextOrder
	{
		get
		{
			if (!this.nextOrderIsWorkable)
			{
				return null;
			}
			return this.recipe_list[this.nextOrderIdx];
		}
	}

	// Token: 0x17000319 RID: 793
	// (get) Token: 0x060040A9 RID: 16553 RVA: 0x000CE61A File Offset: 0x000CC81A
	// (set) Token: 0x060040AA RID: 16554 RVA: 0x000CE622 File Offset: 0x000CC822
	public float OrderProgress
	{
		get
		{
			return this.orderProgress;
		}
		set
		{
			this.orderProgress = value;
		}
	}

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060040AB RID: 16555 RVA: 0x000CE62B File Offset: 0x000CC82B
	public bool HasAnyOrder
	{
		get
		{
			return this.HasWorkingOrder || this.hasOpenOrders;
		}
	}

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x060040AC RID: 16556 RVA: 0x000CE63D File Offset: 0x000CC83D
	public bool HasWorker
	{
		get
		{
			return !this.duplicantOperated || this.workable.worker != null;
		}
	}

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x060040AD RID: 16557 RVA: 0x000CE65A File Offset: 0x000CC85A
	public bool WaitingForWorker
	{
		get
		{
			return this.HasWorkingOrder && !this.HasWorker;
		}
	}

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060040AE RID: 16558 RVA: 0x000CE66F File Offset: 0x000CC86F
	private bool HasWorkingOrder
	{
		get
		{
			return this.workingOrderIdx > -1;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x060040AF RID: 16559 RVA: 0x000CE67A File Offset: 0x000CC87A
	public List<FetchList2> DebugFetchLists
	{
		get
		{
			return this.fetchListList;
		}
	}

	// Token: 0x060040B0 RID: 16560 RVA: 0x00249694 File Offset: 0x00247894
	[OnDeserialized]
	protected virtual void OnDeserializedMethod()
	{
		List<string> list = new List<string>();
		foreach (string text in this.recipeQueueCounts.Keys)
		{
			if (ComplexRecipeManager.Get().GetRecipe(text) == null)
			{
				list.Add(text);
			}
		}
		foreach (string text2 in list)
		{
			global::Debug.LogWarningFormat("{1} removing missing recipe from queue: {0}", new object[]
			{
				text2,
				base.name
			});
			this.recipeQueueCounts.Remove(text2);
		}
	}

	// Token: 0x060040B1 RID: 16561 RVA: 0x00249764 File Offset: 0x00247964
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.GetRecipes();
		this.simRenderLoadBalance = true;
		this.choreType = Db.Get().ChoreTypes.Fabricate;
		base.Subscribe<ComplexFabricator>(-1957399615, ComplexFabricator.OnDroppedAllDelegate);
		base.Subscribe<ComplexFabricator>(-592767678, ComplexFabricator.OnOperationalChangedDelegate);
		base.Subscribe<ComplexFabricator>(-905833192, ComplexFabricator.OnCopySettingsDelegate);
		base.Subscribe<ComplexFabricator>(-1697596308, ComplexFabricator.OnStorageChangeDelegate);
		base.Subscribe<ComplexFabricator>(-1837862626, ComplexFabricator.OnParticleStorageChangedDelegate);
		this.workable = base.GetComponent<ComplexFabricatorWorkable>();
		Components.ComplexFabricators.Add(this);
		base.Subscribe<ComplexFabricator>(493375141, ComplexFabricator.OnRefreshUserMenuDelegate);
	}

	// Token: 0x060040B2 RID: 16562 RVA: 0x00249818 File Offset: 0x00247A18
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.InitRecipeQueueCount();
		foreach (string key in this.recipeQueueCounts.Keys)
		{
			if (this.recipeQueueCounts[key] == 100)
			{
				this.recipeQueueCounts[key] = ComplexFabricator.QUEUE_INFINITE;
			}
		}
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.DropExcessIngredients(this.inStorage);
		int num = this.FindRecipeIndex(this.lastWorkingRecipe);
		if (num > -1)
		{
			this.nextOrderIdx = num;
		}
		this.UpdateMutantSeedStatusItem();
	}

	// Token: 0x060040B3 RID: 16563 RVA: 0x000CE682 File Offset: 0x000CC882
	protected override void OnCleanUp()
	{
		this.CancelAllOpenOrders();
		this.CancelChore();
		Components.ComplexFabricators.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060040B4 RID: 16564 RVA: 0x002498D4 File Offset: 0x00247AD4
	private void OnRefreshUserMenu(object data)
	{
		if (Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds())
		{
			Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_switch_toggle", this.ForbidMutantSeeds ? UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.ACCEPT : UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.REJECT, delegate()
			{
				this.ForbidMutantSeeds = !this.ForbidMutantSeeds;
				this.OnRefreshUserMenu(null);
			}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.ACCEPT_MUTANT_SEEDS.TOOLTIP, true), 1f);
		}
	}

	// Token: 0x060040B5 RID: 16565 RVA: 0x00249954 File Offset: 0x00247B54
	private bool HasRecipiesWithSeeds()
	{
		bool result = false;
		ComplexRecipe[] array = this.recipe_list;
		for (int i = 0; i < array.Length; i++)
		{
			ComplexRecipe.RecipeElement[] ingredients = array[i].ingredients;
			for (int j = 0; j < ingredients.Length; j++)
			{
				GameObject prefab = Assets.GetPrefab(ingredients[j].material);
				if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060040B6 RID: 16566 RVA: 0x002499C4 File Offset: 0x00247BC4
	private void UpdateMutantSeedStatusItem()
	{
		base.gameObject.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.FabricatorAcceptsMutantSeeds, Game.IsDlcActiveForCurrentSave("EXPANSION1_ID") && this.HasRecipiesWithSeeds() && !this.forbidMutantSeeds, null);
	}

	// Token: 0x060040B7 RID: 16567 RVA: 0x000CE6A1 File Offset: 0x000CC8A1
	private void OnOperationalChanged(object data)
	{
		if ((bool)data)
		{
			this.queueDirty = true;
		}
		else
		{
			this.CancelAllOpenOrders();
		}
		this.UpdateChore();
	}

	// Token: 0x060040B8 RID: 16568 RVA: 0x00249A18 File Offset: 0x00247C18
	public virtual void Sim1000ms(float dt)
	{
		this.RefreshAndStartNextOrder();
		if (this.materialNeedCache.Count > 0 && this.fetchListList.Count == 0)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} has material needs cached, but no open fetches. materialNeedCache={1}, fetchListList={2}", new object[]
			{
				base.gameObject,
				this.materialNeedCache.Count,
				this.fetchListList.Count
			});
			this.queueDirty = true;
		}
	}

	// Token: 0x060040B9 RID: 16569 RVA: 0x000CE6C0 File Offset: 0x000CC8C0
	protected virtual float ComputeWorkProgress(float dt, ComplexRecipe recipe)
	{
		return dt / recipe.time;
	}

	// Token: 0x060040BA RID: 16570 RVA: 0x00249A94 File Offset: 0x00247C94
	public void Sim200ms(float dt)
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		this.operational.SetActive(this.HasWorkingOrder && this.HasWorker, false);
		if (!this.duplicantOperated && this.HasWorkingOrder)
		{
			this.orderProgress += this.ComputeWorkProgress(dt, this.recipe_list[this.workingOrderIdx]);
			if (this.orderProgress >= 1f)
			{
				this.ShowProgressBar(false);
				this.CompleteWorkingOrder();
			}
		}
	}

	// Token: 0x060040BB RID: 16571 RVA: 0x00249B18 File Offset: 0x00247D18
	private void RefreshAndStartNextOrder()
	{
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.queueDirty)
		{
			this.RefreshQueue();
		}
		if (!this.HasWorkingOrder && this.nextOrderIsWorkable)
		{
			this.ShowProgressBar(true);
			this.StartWorkingOrder(this.nextOrderIdx);
		}
	}

	// Token: 0x060040BC RID: 16572 RVA: 0x000CE61A File Offset: 0x000CC81A
	public virtual float GetPercentComplete()
	{
		return this.orderProgress;
	}

	// Token: 0x060040BD RID: 16573 RVA: 0x00249B64 File Offset: 0x00247D64
	private void ShowProgressBar(bool show)
	{
		if (show && this.showProgressBar && !this.duplicantOperated)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.enabled = true;
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x060040BE RID: 16574 RVA: 0x000CE6CA File Offset: 0x000CC8CA
	public void SetQueueDirty()
	{
		this.queueDirty = true;
	}

	// Token: 0x060040BF RID: 16575 RVA: 0x000CE6D3 File Offset: 0x000CC8D3
	private void RefreshQueue()
	{
		this.queueDirty = false;
		this.ValidateWorkingOrder();
		this.ValidateNextOrder();
		this.UpdateOpenOrders();
		this.DropExcessIngredients(this.inStorage);
		base.Trigger(1721324763, this);
	}

	// Token: 0x060040C0 RID: 16576 RVA: 0x00249BF0 File Offset: 0x00247DF0
	private void StartWorkingOrder(int index)
	{
		global::Debug.Assert(!this.HasWorkingOrder, "machineOrderIdx already set");
		this.workingOrderIdx = index;
		if (this.recipe_list[this.workingOrderIdx].id != this.lastWorkingRecipe)
		{
			this.orderProgress = 0f;
			this.lastWorkingRecipe = this.recipe_list[this.workingOrderIdx].id;
		}
		this.TransferCurrentRecipeIngredientsForBuild();
		global::Debug.Assert(this.openOrderCounts[this.workingOrderIdx] > 0, "openOrderCount invalid");
		List<int> list = this.openOrderCounts;
		int index2 = this.workingOrderIdx;
		int num = list[index2];
		list[index2] = num - 1;
		this.UpdateChore();
		base.Trigger(2023536846, this.recipe_list[this.workingOrderIdx]);
		this.AdvanceNextOrder();
	}

	// Token: 0x060040C1 RID: 16577 RVA: 0x000CE706 File Offset: 0x000CC906
	private void CancelWorkingOrder()
	{
		global::Debug.Assert(this.HasWorkingOrder, "machineOrderIdx not set");
		this.buildStorage.Transfer(this.inStorage, true, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.UpdateChore();
	}

	// Token: 0x060040C2 RID: 16578 RVA: 0x00249CC0 File Offset: 0x00247EC0
	public virtual void CompleteWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			global::Debug.LogWarning("CompleteWorkingOrder called with no working order.", base.gameObject);
			return;
		}
		ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
		this.SpawnOrderProduct(complexRecipe);
		float num = this.buildStorage.MassStored();
		if (num != 0f)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} build storage contains mass {1} after order completion.", new object[]
			{
				base.gameObject,
				num
			});
			this.buildStorage.Transfer(this.inStorage, true, true);
		}
		this.DecrementRecipeQueueCountInternal(complexRecipe, true);
		this.workingOrderIdx = -1;
		this.orderProgress = 0f;
		this.CancelChore();
		base.Trigger(1355439576, complexRecipe);
		if (!this.cancelling)
		{
			this.RefreshAndStartNextOrder();
		}
	}

	// Token: 0x060040C3 RID: 16579 RVA: 0x00249D88 File Offset: 0x00247F88
	private void ValidateWorkingOrder()
	{
		if (!this.HasWorkingOrder)
		{
			return;
		}
		ComplexRecipe recipe = this.recipe_list[this.workingOrderIdx];
		if (!this.IsRecipeQueued(recipe))
		{
			this.CancelWorkingOrder();
		}
	}

	// Token: 0x060040C4 RID: 16580 RVA: 0x00249DBC File Offset: 0x00247FBC
	private void UpdateChore()
	{
		if (!this.duplicantOperated)
		{
			return;
		}
		bool flag = this.operational.IsOperational && this.HasWorkingOrder;
		if (flag && this.chore == null)
		{
			this.CreateChore();
			return;
		}
		if (!flag && this.chore != null)
		{
			this.CancelChore();
		}
	}

	// Token: 0x060040C5 RID: 16581 RVA: 0x00249E0C File Offset: 0x0024800C
	private void AdvanceNextOrder()
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			this.nextOrderIdx = (this.nextOrderIdx + 1) % this.recipe_list.Length;
			ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
			this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
			if (this.nextOrderIsWorkable)
			{
				break;
			}
		}
	}

	// Token: 0x060040C6 RID: 16582 RVA: 0x00249E7C File Offset: 0x0024807C
	private void ValidateNextOrder()
	{
		ComplexRecipe recipe = this.recipe_list[this.nextOrderIdx];
		this.nextOrderIsWorkable = (this.GetRemainingQueueCount(recipe) > 0 && this.HasIngredients(recipe, this.inStorage));
		if (!this.nextOrderIsWorkable)
		{
			this.AdvanceNextOrder();
		}
	}

	// Token: 0x060040C7 RID: 16583 RVA: 0x00249EC8 File Offset: 0x002480C8
	private void CancelAllOpenOrders()
	{
		for (int i = 0; i < this.openOrderCounts.Count; i++)
		{
			this.openOrderCounts[i] = 0;
		}
		this.ClearMaterialNeeds();
		this.CancelFetches();
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x00249F04 File Offset: 0x00248104
	private void UpdateOpenOrders()
	{
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != this.openOrderCounts.Count)
		{
			global::Debug.LogErrorFormat(base.gameObject, "Recipe count {0} doesn't match open order count {1}", new object[]
			{
				recipes.Length,
				this.openOrderCounts.Count
			});
		}
		bool flag = false;
		this.hasOpenOrders = false;
		for (int i = 0; i < recipes.Length; i++)
		{
			ComplexRecipe recipe = recipes[i];
			int recipePrefetchCount = this.GetRecipePrefetchCount(recipe);
			if (recipePrefetchCount > 0)
			{
				this.hasOpenOrders = true;
			}
			int num = this.openOrderCounts[i];
			if (num != recipePrefetchCount)
			{
				if (recipePrefetchCount < num)
				{
					flag = true;
				}
				this.openOrderCounts[i] = recipePrefetchCount;
			}
		}
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary2 = DictionaryPool<Tag, float, ComplexFabricator>.Allocate();
		for (int j = 0; j < this.openOrderCounts.Count; j++)
		{
			if (this.openOrderCounts[j] > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in this.recipe_list[j].ingredients)
				{
					pooledDictionary[recipeElement.material] = this.inStorage.GetAmountAvailable(recipeElement.material);
				}
			}
		}
		for (int l = 0; l < this.recipe_list.Length; l++)
		{
			int num2 = this.openOrderCounts[l];
			if (num2 > 0)
			{
				foreach (ComplexRecipe.RecipeElement recipeElement2 in this.recipe_list[l].ingredients)
				{
					float num3 = recipeElement2.amount * (float)num2;
					float num4 = num3 - pooledDictionary[recipeElement2.material];
					if (num4 > 0f)
					{
						float num5;
						pooledDictionary2.TryGetValue(recipeElement2.material, out num5);
						pooledDictionary2[recipeElement2.material] = num5 + num4;
						pooledDictionary[recipeElement2.material] = 0f;
					}
					else
					{
						DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary pooledDictionary3 = pooledDictionary;
						Tag material = recipeElement2.material;
						pooledDictionary3[material] -= num3;
					}
				}
			}
		}
		if (flag)
		{
			this.CancelFetches();
		}
		if (pooledDictionary2.Count > 0)
		{
			this.UpdateFetches(pooledDictionary2);
		}
		this.UpdateMaterialNeeds(pooledDictionary2);
		pooledDictionary2.Recycle();
		pooledDictionary.Recycle();
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x0024A150 File Offset: 0x00248350
	private void UpdateMaterialNeeds(Dictionary<Tag, float> missingAmounts)
	{
		this.ClearMaterialNeeds();
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, keyValuePair.Value, base.gameObject.GetMyWorldId());
			this.materialNeedCache.Add(keyValuePair.Key, keyValuePair.Value);
		}
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x0024A1D4 File Offset: 0x002483D4
	private void ClearMaterialNeeds()
	{
		foreach (KeyValuePair<Tag, float> keyValuePair in this.materialNeedCache)
		{
			MaterialNeeds.UpdateNeed(keyValuePair.Key, -keyValuePair.Value, base.gameObject.GetMyWorldId());
		}
		this.materialNeedCache.Clear();
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x0024A24C File Offset: 0x0024844C
	public int HighestHEPQueued()
	{
		int num = 0;
		foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
		{
			if (keyValuePair.Value > 0)
			{
				num = Math.Max(this.recipe_list[this.FindRecipeIndex(keyValuePair.Key)].consumedHEP, num);
			}
		}
		return num;
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x0024A2C8 File Offset: 0x002484C8
	private void OnFetchComplete()
	{
		for (int i = this.fetchListList.Count - 1; i >= 0; i--)
		{
			if (this.fetchListList[i].IsComplete)
			{
				this.fetchListList.RemoveAt(i);
				this.queueDirty = true;
			}
		}
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x000CE6CA File Offset: 0x000CC8CA
	private void OnStorageChange(object data)
	{
		this.queueDirty = true;
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x000CE743 File Offset: 0x000CC943
	private void OnDroppedAll(object data)
	{
		if (this.HasWorkingOrder)
		{
			this.CancelWorkingOrder();
		}
		this.CancelAllOpenOrders();
		this.RefreshQueue();
	}

	// Token: 0x060040CF RID: 16591 RVA: 0x0024A314 File Offset: 0x00248514
	private void DropExcessIngredients(Storage storage)
	{
		HashSet<Tag> hashSet = new HashSet<Tag>();
		if (this.keepAdditionalTag != Tag.Invalid)
		{
			hashSet.Add(this.keepAdditionalTag);
		}
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			ComplexRecipe complexRecipe = this.recipe_list[i];
			if (this.IsRecipeQueued(complexRecipe))
			{
				foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
				{
					hashSet.Add(recipeElement.material);
				}
			}
		}
		for (int k = storage.items.Count - 1; k >= 0; k--)
		{
			GameObject gameObject = storage.items[k];
			if (!(gameObject == null))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				if (!(component == null) && (!this.keepExcessLiquids || !component.Element.IsLiquid))
				{
					KPrefabID component2 = gameObject.GetComponent<KPrefabID>();
					if (component2 && !hashSet.Contains(component2.PrefabID()))
					{
						storage.Drop(gameObject, true);
					}
				}
			}
		}
	}

	// Token: 0x060040D0 RID: 16592 RVA: 0x0024A424 File Offset: 0x00248624
	private void OnCopySettings(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		ComplexFabricator component = gameObject.GetComponent<ComplexFabricator>();
		if (component == null)
		{
			return;
		}
		this.ForbidMutantSeeds = component.ForbidMutantSeeds;
		foreach (ComplexRecipe complexRecipe in this.recipe_list)
		{
			int count;
			if (!component.recipeQueueCounts.TryGetValue(complexRecipe.id, out count))
			{
				count = 0;
			}
			this.SetRecipeQueueCountInternal(complexRecipe, count);
		}
		this.RefreshQueue();
	}

	// Token: 0x060040D1 RID: 16593 RVA: 0x000CE75F File Offset: 0x000CC95F
	private int CompareRecipe(ComplexRecipe a, ComplexRecipe b)
	{
		if (a.sortOrder != b.sortOrder)
		{
			return a.sortOrder - b.sortOrder;
		}
		return StringComparer.InvariantCulture.Compare(a.id, b.id);
	}

	// Token: 0x060040D2 RID: 16594 RVA: 0x0024A4A4 File Offset: 0x002486A4
	public ComplexRecipe[] GetRecipes()
	{
		if (this.recipe_list == null)
		{
			Tag prefabTag = base.GetComponent<KPrefabID>().PrefabTag;
			List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
			List<ComplexRecipe> list = new List<ComplexRecipe>();
			foreach (ComplexRecipe complexRecipe in recipes)
			{
				using (List<Tag>.Enumerator enumerator2 = complexRecipe.fabricators.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == prefabTag && Game.IsCorrectDlcActiveForCurrentSave(complexRecipe))
						{
							list.Add(complexRecipe);
						}
					}
				}
			}
			this.recipe_list = list.ToArray();
			Array.Sort<ComplexRecipe>(this.recipe_list, new Comparison<ComplexRecipe>(this.CompareRecipe));
		}
		return this.recipe_list;
	}

	// Token: 0x060040D3 RID: 16595 RVA: 0x0024A58C File Offset: 0x0024878C
	private void InitRecipeQueueCount()
	{
		foreach (ComplexRecipe complexRecipe in this.GetRecipes())
		{
			bool flag = false;
			using (Dictionary<string, int>.KeyCollection.Enumerator enumerator = this.recipeQueueCounts.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == complexRecipe.id)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				this.recipeQueueCounts.Add(complexRecipe.id, 0);
			}
			this.openOrderCounts.Add(0);
		}
	}

	// Token: 0x060040D4 RID: 16596 RVA: 0x0024A62C File Offset: 0x0024882C
	private int FindRecipeIndex(string id)
	{
		for (int i = 0; i < this.recipe_list.Length; i++)
		{
			if (this.recipe_list[i].id == id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060040D5 RID: 16597 RVA: 0x000CE793 File Offset: 0x000CC993
	public int GetRecipeQueueCount(ComplexRecipe recipe)
	{
		return this.recipeQueueCounts[recipe.id];
	}

	// Token: 0x060040D6 RID: 16598 RVA: 0x0024A664 File Offset: 0x00248864
	public bool IsRecipeQueued(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		return num != 0;
	}

	// Token: 0x060040D7 RID: 16599 RVA: 0x0024A69C File Offset: 0x0024889C
	public int GetRecipePrefetchCount(ComplexRecipe recipe)
	{
		int remainingQueueCount = this.GetRemainingQueueCount(recipe);
		global::Debug.Assert(remainingQueueCount >= 0);
		return Mathf.Min(2, remainingQueueCount);
	}

	// Token: 0x060040D8 RID: 16600 RVA: 0x0024A6C4 File Offset: 0x002488C4
	private int GetRemainingQueueCount(ComplexRecipe recipe)
	{
		int num = this.recipeQueueCounts[recipe.id];
		global::Debug.Assert(num >= 0 || num == ComplexFabricator.QUEUE_INFINITE);
		if (num == ComplexFabricator.QUEUE_INFINITE)
		{
			return ComplexFabricator.MAX_QUEUE_SIZE;
		}
		if (num > 0)
		{
			if (this.IsCurrentRecipe(recipe))
			{
				num--;
			}
			return num;
		}
		return 0;
	}

	// Token: 0x060040D9 RID: 16601 RVA: 0x000CE7A6 File Offset: 0x000CC9A6
	private bool IsCurrentRecipe(ComplexRecipe recipe)
	{
		return this.workingOrderIdx >= 0 && this.recipe_list[this.workingOrderIdx].id == recipe.id;
	}

	// Token: 0x060040DA RID: 16602 RVA: 0x000CE7D0 File Offset: 0x000CC9D0
	public void SetRecipeQueueCount(ComplexRecipe recipe, int count)
	{
		this.SetRecipeQueueCountInternal(recipe, count);
		this.RefreshQueue();
	}

	// Token: 0x060040DB RID: 16603 RVA: 0x000CE7E0 File Offset: 0x000CC9E0
	private void SetRecipeQueueCountInternal(ComplexRecipe recipe, int count)
	{
		this.recipeQueueCounts[recipe.id] = count;
	}

	// Token: 0x060040DC RID: 16604 RVA: 0x0024A71C File Offset: 0x0024891C
	public void IncrementRecipeQueueCount(ComplexRecipe recipe)
	{
		if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
		{
			this.recipeQueueCounts[recipe.id] = 0;
		}
		else if (this.recipeQueueCounts[recipe.id] >= ComplexFabricator.MAX_QUEUE_SIZE)
		{
			this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
		}
		else
		{
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num + 1;
		}
		this.RefreshQueue();
	}

	// Token: 0x060040DD RID: 16605 RVA: 0x000CE7F4 File Offset: 0x000CC9F4
	public void DecrementRecipeQueueCount(ComplexRecipe recipe, bool respectInfinite = true)
	{
		this.DecrementRecipeQueueCountInternal(recipe, respectInfinite);
		this.RefreshQueue();
	}

	// Token: 0x060040DE RID: 16606 RVA: 0x0024A7AC File Offset: 0x002489AC
	private void DecrementRecipeQueueCountInternal(ComplexRecipe recipe, bool respectInfinite = true)
	{
		if (!respectInfinite || this.recipeQueueCounts[recipe.id] != ComplexFabricator.QUEUE_INFINITE)
		{
			if (this.recipeQueueCounts[recipe.id] == ComplexFabricator.QUEUE_INFINITE)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.MAX_QUEUE_SIZE;
				return;
			}
			if (this.recipeQueueCounts[recipe.id] == 0)
			{
				this.recipeQueueCounts[recipe.id] = ComplexFabricator.QUEUE_INFINITE;
				return;
			}
			Dictionary<string, int> dictionary = this.recipeQueueCounts;
			string id = recipe.id;
			int num = dictionary[id];
			dictionary[id] = num - 1;
		}
	}

	// Token: 0x060040DF RID: 16607 RVA: 0x000CE804 File Offset: 0x000CCA04
	private void CreateChore()
	{
		global::Debug.Assert(this.chore == null, "chore should be null");
		this.chore = this.workable.CreateWorkChore(this.choreType, this.orderProgress);
	}

	// Token: 0x1700031F RID: 799
	// (get) Token: 0x060040E0 RID: 16608 RVA: 0x000CE836 File Offset: 0x000CCA36
	public override Chore RemoteDockChore
	{
		get
		{
			if (!this.duplicantOperated)
			{
				return null;
			}
			return this.chore;
		}
	}

	// Token: 0x060040E1 RID: 16609 RVA: 0x000CE848 File Offset: 0x000CCA48
	private void CancelChore()
	{
		if (this.cancelling)
		{
			return;
		}
		this.cancelling = true;
		if (this.chore != null)
		{
			this.chore.Cancel("order cancelled");
			this.chore = null;
		}
		this.cancelling = false;
	}

	// Token: 0x060040E2 RID: 16610 RVA: 0x0024A84C File Offset: 0x00248A4C
	private void UpdateFetches(DictionaryPool<Tag, float, ComplexFabricator>.PooledDictionary missingAmounts)
	{
		ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
		foreach (KeyValuePair<Tag, float> keyValuePair in missingAmounts)
		{
			if (!this.allowManualFluidDelivery)
			{
				Element element = ElementLoader.GetElement(keyValuePair.Key);
				if (element != null && (element.IsLiquid || element.IsGas))
				{
					continue;
				}
			}
			if (keyValuePair.Value >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT && !this.HasPendingFetch(keyValuePair.Key))
			{
				FetchList2 fetchList = new FetchList2(this.inStorage, byHash);
				FetchList2 fetchList2 = fetchList;
				Tag key = keyValuePair.Key;
				float value = keyValuePair.Value;
				fetchList2.Add(key, this.ForbiddenTags, value, Operational.State.None);
				fetchList.ShowStatusItem = false;
				fetchList.Submit(new System.Action(this.OnFetchComplete), false);
				this.fetchListList.Add(fetchList);
			}
		}
	}

	// Token: 0x060040E3 RID: 16611 RVA: 0x0024A94C File Offset: 0x00248B4C
	private bool HasPendingFetch(Tag tag)
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			float num;
			fetchList.MinimumAmount.TryGetValue(tag, out num);
			if (num > 0f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060040E4 RID: 16612 RVA: 0x0024A9B4 File Offset: 0x00248BB4
	private void CancelFetches()
	{
		foreach (FetchList2 fetchList in this.fetchListList)
		{
			fetchList.Cancel("cancel all orders");
		}
		this.fetchListList.Clear();
	}

	// Token: 0x060040E5 RID: 16613 RVA: 0x0024AA14 File Offset: 0x00248C14
	protected virtual void TransferCurrentRecipeIngredientsForBuild()
	{
		ComplexRecipe.RecipeElement[] ingredients = this.recipe_list[this.workingOrderIdx].ingredients;
		int i = 0;
		while (i < ingredients.Length)
		{
			ComplexRecipe.RecipeElement recipeElement = ingredients[i];
			float num;
			for (;;)
			{
				num = recipeElement.amount - this.buildStorage.GetAmountAvailable(recipeElement.material);
				if (num <= 0f)
				{
					break;
				}
				if (this.inStorage.GetAmountAvailable(recipeElement.material) <= 0f)
				{
					goto Block_2;
				}
				this.inStorage.Transfer(this.buildStorage, recipeElement.material, num, false, true);
			}
			IL_9D:
			i++;
			continue;
			Block_2:
			global::Debug.LogWarningFormat("TransferCurrentRecipeIngredientsForBuild ran out of {0} but still needed {1} more.", new object[]
			{
				recipeElement.material,
				num
			});
			goto IL_9D;
		}
	}

	// Token: 0x060040E6 RID: 16614 RVA: 0x0024AACC File Offset: 0x00248CCC
	protected virtual bool HasIngredients(ComplexRecipe recipe, Storage storage)
	{
		ComplexRecipe.RecipeElement[] ingredients = recipe.ingredients;
		if (recipe.consumedHEP > 0)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			if (component == null || component.Particles < (float)recipe.consumedHEP)
			{
				return false;
			}
		}
		foreach (ComplexRecipe.RecipeElement recipeElement in ingredients)
		{
			float amountAvailable = storage.GetAmountAvailable(recipeElement.material);
			if (recipeElement.amount - amountAvailable >= PICKUPABLETUNING.MINIMUM_PICKABLE_AMOUNT)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060040E7 RID: 16615 RVA: 0x0024AB44 File Offset: 0x00248D44
	private void ToggleMutantSeedFetches()
	{
		if (this.HasAnyOrder)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreTypeIdHash);
			List<FetchList2> list = new List<FetchList2>();
			foreach (FetchList2 fetchList in this.fetchListList)
			{
				foreach (FetchOrder2 fetchOrder in fetchList.FetchOrders)
				{
					foreach (Tag tag in fetchOrder.Tags)
					{
						GameObject prefab = Assets.GetPrefab(tag);
						if (prefab != null && prefab.GetComponent<PlantableSeed>() != null)
						{
							fetchList.Cancel("MutantSeedTagChanged");
							list.Add(fetchList);
						}
					}
				}
			}
			foreach (FetchList2 fetchList2 in list)
			{
				this.fetchListList.Remove(fetchList2);
				foreach (FetchOrder2 fetchOrder2 in fetchList2.FetchOrders)
				{
					foreach (Tag tag2 in fetchOrder2.Tags)
					{
						FetchList2 fetchList3 = new FetchList2(this.inStorage, byHash);
						FetchList2 fetchList4 = fetchList3;
						Tag tag3 = tag2;
						float totalAmount = fetchOrder2.TotalAmount;
						fetchList4.Add(tag3, this.ForbiddenTags, totalAmount, Operational.State.None);
						fetchList3.ShowStatusItem = false;
						fetchList3.Submit(new System.Action(this.OnFetchComplete), false);
						this.fetchListList.Add(fetchList3);
					}
				}
			}
		}
	}

	// Token: 0x060040E8 RID: 16616 RVA: 0x0024AD84 File Offset: 0x00248F84
	protected virtual List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
	{
		List<GameObject> list = new List<GameObject>();
		SimUtil.DiseaseInfo diseaseInfo;
		diseaseInfo.count = 0;
		diseaseInfo.idx = 0;
		float num = 0f;
		float num2 = 0f;
		string text = null;
		foreach (ComplexRecipe.RecipeElement recipeElement in recipe.ingredients)
		{
			num2 += recipeElement.amount;
		}
		ComplexRecipe.RecipeElement recipeElement2 = null;
		Element element = null;
		foreach (ComplexRecipe.RecipeElement recipeElement3 in recipe.ingredients)
		{
			float num3 = recipeElement3.amount / num2;
			if (recipe.ProductHasFacade && text.IsNullOrWhiteSpace())
			{
				RepairableEquipment component = this.buildStorage.FindFirst(recipeElement3.material).GetComponent<RepairableEquipment>();
				if (component != null)
				{
					text = component.facadeID;
				}
			}
			if (recipeElement3.inheritElement)
			{
				recipeElement2 = recipeElement3;
				element = this.buildStorage.FindFirst(recipeElement3.material).GetComponent<PrimaryElement>().Element;
			}
			if (recipeElement3.Edible)
			{
				recipeElement2 = recipeElement3;
				this.buildStorage.TransferMass(this.outStorage, recipeElement3.material, recipeElement3.amount, true, true, true);
			}
			else
			{
				float num4;
				SimUtil.DiseaseInfo diseaseInfo2;
				float num5;
				this.buildStorage.ConsumeAndGetDisease(recipeElement3.material, recipeElement3.amount, out num4, out diseaseInfo2, out num5);
				if (diseaseInfo2.count > diseaseInfo.count)
				{
					diseaseInfo = diseaseInfo2;
				}
				num += num5 * num3;
			}
		}
		if (recipe.consumedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet((float)recipe.consumedHEP);
		}
		foreach (ComplexRecipe.RecipeElement recipeElement4 in recipe.results)
		{
			GameObject gameObject = this.buildStorage.FindFirst(recipeElement4.material);
			if (gameObject != null)
			{
				Edible component2 = gameObject.GetComponent<Edible>();
				if (component2)
				{
					ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component2.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.CRAFTED_USED, "{0}", component2.GetProperName()), UI.ENDOFDAYREPORT.NOTES.CRAFTED_CONTEXT);
				}
			}
			switch (recipeElement4.temperatureOperation)
			{
			case ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature:
			case ComplexRecipe.RecipeElement.TemperatureOperation.Heated:
			{
				GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
				int cell = Grid.PosToCell(this);
				gameObject2.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore) + this.outputOffset);
				PrimaryElement component3 = gameObject2.GetComponent<PrimaryElement>();
				component3.Units = recipeElement4.amount;
				component3.Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
				if (element != null)
				{
					component3.SetElement(element.id, false);
				}
				if (recipe.ProductHasFacade && !text.IsNullOrWhiteSpace())
				{
					Equippable component4 = gameObject2.GetComponent<Equippable>();
					if (component4 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component4, text);
					}
				}
				gameObject2.SetActive(true);
				float num6 = recipeElement4.amount / recipe.TotalResultUnits();
				component3.AddDisease(diseaseInfo.idx, Mathf.RoundToInt((float)diseaseInfo.count * num6), "ComplexFabricator.CompleteOrder");
				if (!recipeElement4.facadeID.IsNullOrWhiteSpace())
				{
					Equippable component5 = gameObject2.GetComponent<Equippable>();
					if (component5 != null)
					{
						EquippableFacade.AddFacadeToEquippable(component5, recipeElement4.facadeID);
					}
				}
				gameObject2.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
				list.Add(gameObject2);
				if (this.storeProduced || recipeElement4.storeElement)
				{
					this.outStorage.Store(gameObject2, false, false, true, false);
				}
				break;
			}
			case ComplexRecipe.RecipeElement.TemperatureOperation.Melted:
				if (this.storeProduced || recipeElement4.storeElement)
				{
					float temperature = ElementLoader.GetElement(recipeElement4.material).defaultValues.temperature;
					this.outStorage.AddLiquid(ElementLoader.GetElementID(recipeElement4.material), recipeElement4.amount, temperature, 0, 0, false, true);
				}
				break;
			case ComplexRecipe.RecipeElement.TemperatureOperation.Dehydrated:
				for (int j = 0; j < (int)recipeElement4.amount; j++)
				{
					GameObject gameObject3 = GameUtil.KInstantiate(Assets.GetPrefab(recipeElement4.material), Grid.SceneLayer.Ore, null, 0);
					int cell2 = Grid.PosToCell(this);
					gameObject3.transform.SetPosition(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Ore) + this.outputOffset);
					float amount = recipeElement2.amount / recipeElement4.amount;
					gameObject3.GetComponent<PrimaryElement>().Temperature = ((recipeElement4.temperatureOperation == ComplexRecipe.RecipeElement.TemperatureOperation.AverageTemperature) ? num : this.heatedTemperature);
					DehydratedFoodPackage component6 = gameObject3.GetComponent<DehydratedFoodPackage>();
					if (component6 != null)
					{
						Storage component7 = component6.GetComponent<Storage>();
						this.outStorage.TransferMass(component7, recipeElement2.material, amount, true, false, false);
					}
					gameObject3.SetActive(true);
					gameObject3.GetComponent<KMonoBehaviour>().Trigger(748399584, null);
					list.Add(gameObject3);
					if (this.storeProduced || recipeElement4.storeElement)
					{
						this.outStorage.Store(gameObject3, false, false, true, false);
					}
				}
				break;
			}
			if (list.Count > 0)
			{
				SymbolOverrideController component8 = base.GetComponent<SymbolOverrideController>();
				if (component8 != null)
				{
					KAnim.Build build = list[0].GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build;
					KAnim.Build.Symbol symbol = build.GetSymbol(build.name);
					if (symbol != null)
					{
						component8.TryRemoveSymbolOverride("output_tracker", 0);
						component8.AddSymbolOverride("output_tracker", symbol, 0);
					}
					else
					{
						global::Debug.LogWarning(component8.name + " is missing symbol " + build.name);
					}
				}
			}
		}
		if (recipe.producedHEP > 0)
		{
			base.GetComponent<HighEnergyParticleStorage>().Store((float)recipe.producedHEP);
		}
		return list;
	}

	// Token: 0x060040E9 RID: 16617 RVA: 0x0024B330 File Offset: 0x00249530
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		ComplexRecipe[] recipes = this.GetRecipes();
		if (recipes.Length != 0)
		{
			Descriptor item = default(Descriptor);
			item.SetupDescriptor(UI.BUILDINGEFFECTS.PROCESSES, UI.BUILDINGEFFECTS.TOOLTIPS.PROCESSES, Descriptor.DescriptorType.Effect);
			list.Add(item);
		}
		foreach (ComplexRecipe complexRecipe in recipes)
		{
			string text = "";
			string uiname = complexRecipe.GetUIName(false);
			foreach (ComplexRecipe.RecipeElement recipeElement in complexRecipe.ingredients)
			{
				text = text + "• " + string.Format(UI.BUILDINGEFFECTS.PROCESSEDITEM, recipeElement.material.ProperName(), recipeElement.amount) + "\n";
			}
			Descriptor item2 = new Descriptor(uiname, string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.FABRICATOR_INGREDIENTS, text), Descriptor.DescriptorType.Effect, false);
			item2.IncreaseIndent();
			list.Add(item2);
		}
		return list;
	}

	// Token: 0x060040EA RID: 16618 RVA: 0x000CE880 File Offset: 0x000CCA80
	public virtual List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
	{
		return new List<Descriptor>();
	}

	// Token: 0x060040EB RID: 16619 RVA: 0x0024B428 File Offset: 0x00249628
	public string GetConversationTopic()
	{
		if (this.HasWorkingOrder)
		{
			ComplexRecipe complexRecipe = this.recipe_list[this.workingOrderIdx];
			if (complexRecipe != null)
			{
				return complexRecipe.results[0].material.Name;
			}
		}
		return null;
	}

	// Token: 0x060040EC RID: 16620 RVA: 0x0024B464 File Offset: 0x00249664
	public bool NeedsMoreHEPForQueuedRecipe()
	{
		if (this.hasOpenOrders)
		{
			HighEnergyParticleStorage component = base.GetComponent<HighEnergyParticleStorage>();
			foreach (KeyValuePair<string, int> keyValuePair in this.recipeQueueCounts)
			{
				if (keyValuePair.Value > 0)
				{
					foreach (ComplexRecipe complexRecipe in this.GetRecipes())
					{
						if (complexRecipe.id == keyValuePair.Key && (float)complexRecipe.consumedHEP > component.Particles)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x04002CC0 RID: 11456
	private const int MaxPrefetchCount = 2;

	// Token: 0x04002CC1 RID: 11457
	public bool duplicantOperated = true;

	// Token: 0x04002CC2 RID: 11458
	protected ComplexFabricatorWorkable workable;

	// Token: 0x04002CC3 RID: 11459
	public string SideScreenSubtitleLabel = UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE;

	// Token: 0x04002CC4 RID: 11460
	public string SideScreenRecipeScreenTitle = UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS;

	// Token: 0x04002CC5 RID: 11461
	[SerializeField]
	public HashedString fetchChoreTypeIdHash = Db.Get().ChoreTypes.FabricateFetch.IdHash;

	// Token: 0x04002CC6 RID: 11462
	[SerializeField]
	public float heatedTemperature;

	// Token: 0x04002CC7 RID: 11463
	[SerializeField]
	public bool storeProduced;

	// Token: 0x04002CC8 RID: 11464
	[SerializeField]
	public bool allowManualFluidDelivery = true;

	// Token: 0x04002CC9 RID: 11465
	public ComplexFabricatorSideScreen.StyleSetting sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

	// Token: 0x04002CCA RID: 11466
	public bool labelByResult = true;

	// Token: 0x04002CCB RID: 11467
	public Vector3 outputOffset = Vector3.zero;

	// Token: 0x04002CCC RID: 11468
	public ChoreType choreType;

	// Token: 0x04002CCD RID: 11469
	public bool keepExcessLiquids;

	// Token: 0x04002CCE RID: 11470
	public Tag keepAdditionalTag = Tag.Invalid;

	// Token: 0x04002CCF RID: 11471
	public StatusItem workingStatusItem = Db.Get().BuildingStatusItems.ComplexFabricatorProducing;

	// Token: 0x04002CD0 RID: 11472
	public static int MAX_QUEUE_SIZE = 99;

	// Token: 0x04002CD1 RID: 11473
	public static int QUEUE_INFINITE = -1;

	// Token: 0x04002CD2 RID: 11474
	[Serialize]
	private Dictionary<string, int> recipeQueueCounts = new Dictionary<string, int>();

	// Token: 0x04002CD3 RID: 11475
	private int nextOrderIdx;

	// Token: 0x04002CD4 RID: 11476
	private bool nextOrderIsWorkable;

	// Token: 0x04002CD5 RID: 11477
	private int workingOrderIdx = -1;

	// Token: 0x04002CD6 RID: 11478
	[Serialize]
	private string lastWorkingRecipe;

	// Token: 0x04002CD7 RID: 11479
	[Serialize]
	private float orderProgress;

	// Token: 0x04002CD8 RID: 11480
	private List<int> openOrderCounts = new List<int>();

	// Token: 0x04002CD9 RID: 11481
	[Serialize]
	private bool forbidMutantSeeds;

	// Token: 0x04002CDA RID: 11482
	private Tag[] forbiddenMutantTags = new Tag[]
	{
		GameTags.MutatedSeed
	};

	// Token: 0x04002CDB RID: 11483
	private bool queueDirty = true;

	// Token: 0x04002CDC RID: 11484
	private bool hasOpenOrders;

	// Token: 0x04002CDD RID: 11485
	private List<FetchList2> fetchListList = new List<FetchList2>();

	// Token: 0x04002CDE RID: 11486
	private Chore chore;

	// Token: 0x04002CDF RID: 11487
	private bool cancelling;

	// Token: 0x04002CE0 RID: 11488
	private ComplexRecipe[] recipe_list;

	// Token: 0x04002CE1 RID: 11489
	private Dictionary<Tag, float> materialNeedCache = new Dictionary<Tag, float>();

	// Token: 0x04002CE2 RID: 11490
	[SerializeField]
	public Storage inStorage;

	// Token: 0x04002CE3 RID: 11491
	[SerializeField]
	public Storage buildStorage;

	// Token: 0x04002CE4 RID: 11492
	[SerializeField]
	public Storage outStorage;

	// Token: 0x04002CE5 RID: 11493
	[MyCmpAdd]
	private LoopingSounds loopingSounds;

	// Token: 0x04002CE6 RID: 11494
	[MyCmpReq]
	protected Operational operational;

	// Token: 0x04002CE7 RID: 11495
	[MyCmpAdd]
	protected ComplexFabricatorSM fabricatorSM;

	// Token: 0x04002CE8 RID: 11496
	private ProgressBar progressBar;

	// Token: 0x04002CE9 RID: 11497
	public bool showProgressBar;

	// Token: 0x04002CEA RID: 11498
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002CEB RID: 11499
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnParticleStorageChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04002CEC RID: 11500
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnDroppedAllDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnDroppedAll(data);
	});

	// Token: 0x04002CED RID: 11501
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04002CEE RID: 11502
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04002CEF RID: 11503
	private static readonly EventSystem.IntraObjectHandler<ComplexFabricator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<ComplexFabricator>(delegate(ComplexFabricator component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
