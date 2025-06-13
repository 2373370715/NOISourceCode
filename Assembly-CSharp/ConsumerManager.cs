using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/ConsumerManager")]
public class ConsumerManager : KMonoBehaviour, ISaveLoadable
{
	public static void DestroyInstance()
	{
		ConsumerManager.instance = null;
	}

add) Token: 0x06005A46 RID: 23110 RVA: 0x002A1DB8 File Offset: 0x0029FFB8
remove) Token: 0x06005A47 RID: 23111 RVA: 0x002A1DF0 File Offset: 0x0029FFF0
	public event Action<Tag> OnDiscover;

	public List<Tag> DefaultForbiddenTagsList
	{
		get
		{
			return this.defaultForbiddenTagsList;
		}
	}

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

	protected override void OnSpawn()
	{
		base.OnSpawn();
		ConsumerManager.instance = this;
		this.RefreshDiscovered(null);
		DiscoveredResources.Instance.OnDiscover += this.OnWorldInventoryDiscover;
		Game.Instance.Subscribe(-107300940, new Action<object>(this.RefreshDiscovered));
	}

	public bool isDiscovered(Tag id)
	{
		return !this.undiscoveredConsumableTags.Contains(id);
	}

	private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
	{
		if (this.undiscoveredConsumableTags.Contains(tag))
		{
			this.RefreshDiscovered(null);
		}
	}

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

	public static ConsumerManager instance;

	[Serialize]
	private List<Tag> undiscoveredConsumableTags = new List<Tag>();

	[Serialize]
	private List<Tag> defaultForbiddenTagsList = new List<Tag>();

	public static string OXYGEN_TANK_ID = ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag.ToString();
}
