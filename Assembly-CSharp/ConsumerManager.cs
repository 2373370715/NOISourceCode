using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001144 RID: 4420
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06005A45 RID: 23109 RVA: 0x000DF2ED File Offset: 0x000DD4ED
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

	// Token: 0x14000017 RID: 23
	// (add) Token: 0x06005A46 RID: 23110 RVA: 0x002A1DB8 File Offset: 0x0029FFB8
	// (remove) Token: 0x06005A47 RID: 23111 RVA: 0x002A1DF0 File Offset: 0x0029FFF0
	public event Action<Tag> OnDiscover;

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06005A48 RID: 23112 RVA: 0x000DF2F5 File Offset: 0x000DD4F5
	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06005A49 RID: 23113 RVA: 0x002A1E28 File Offset: 0x002A0028
	public List<Tag> StandardDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.ChargedPortableBattery))
			{
				list.Add(go.PrefabID());
			}
			list.Add(ConsumerManager.OXYGEN_TANK_ID);
			return list;
		}
	}

	// Token: 0x17000569 RID: 1385
	// (get) Token: 0x06005A4A RID: 23114 RVA: 0x002A1E9C File Offset: 0x002A009C
	public List<Tag> BionicDuplicantDietaryRestrictions
	{
		get
		{
			List<Tag> list = new List<Tag>();
			foreach (GameObject go in Assets.GetPrefabsWithTag(GameTags.Edible))
			{
				list.Add(go.PrefabID());
			}
			Tag[] array = new Tag[GameTags.BionicIncompatibleBatteries.Count];
			GameTags.BionicIncompatibleBatteries.CopyTo(array, 0);
			foreach (Tag item in array)
			{
				list.Add(item);
			}
			return list;
		}
	}

	// Token: 0x06005A4B RID: 23115 RVA: 0x002A1F44 File Offset: 0x002A0144
	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	// Token: 0x06005A4C RID: 23116 RVA: 0x000DF2FD File Offset: 0x000DD4FD
	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	// Token: 0x06005A4D RID: 23117 RVA: 0x000DF30E File Offset: 0x000DD50E
	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

	// Token: 0x06005A4E RID: 23118 RVA: 0x002A1F98 File Offset: 0x002A0198
	public void RefreshDiscovered(object data = null)
	{
		foreach (EdiblesManager.FoodInfo foodInfo in EdiblesManager.GetAllFoodTypes())
		{
			if (!this.ShouldBeDiscovered(foodInfo.Id.ToTag()) && !this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Add(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover("UndiscoveredSomething".ToTag());
				}
			}
			else if (this.undiscoveredConsumableTags.Contains(foodInfo.Id.ToTag()) && this.ShouldBeDiscovered(foodInfo.Id.ToTag()))
			{
				this.undiscoveredConsumableTags.Remove(foodInfo.Id.ToTag());
				if (this.OnDiscover != null)
				{
					this.OnDiscover(foodInfo.Id.ToTag());
				}
				if (!DiscoveredResources.Instance.IsDiscovered(foodInfo.Id.ToTag()))
				{
					if (foodInfo.CaloriesPerUnit == 0f)
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.CookingIngredient);
					}
					else
					{
						DiscoveredResources.Instance.Discover(foodInfo.Id.ToTag(), GameTags.Edible);
					}
				}
			}
		}
	}

	// Token: 0x06005A4F RID: 23119 RVA: 0x002A211C File Offset: 0x002A031C
	private bool ShouldBeDiscovered(Tag food_id)
	{
		if (DiscoveredResources.Instance.IsDiscovered(food_id))
		{
			return true;
		}
		foreach (Recipe recipe in RecipeManager.Get().recipes)
		{
			if (recipe.Result == food_id)
			{
				foreach (string id in recipe.fabricators)
				{
					if (Db.Get().TechItems.IsTechItemComplete(id))
					{
						return true;
					}
				}
			}
		}
		foreach (Crop crop in Components.Crops.Items)
		{
			if (Grid.IsVisible(Grid.PosToCell(crop.gameObject)) && crop.cropId == food_id.Name)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04004048 RID: 16456
	public static ConsumerManager instance;

	// Token: 0x0400404A RID: 16458
	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	// Token: 0x0400404B RID: 16459
	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	// Token: 0x0400404C RID: 16460
	public static string OXYGEN_TANK_ID = ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag.ToString();
}
