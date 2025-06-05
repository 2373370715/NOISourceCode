using System;
using UnityEngine;

// Token: 0x020017F9 RID: 6137
public class ResearchType
{
	// Token: 0x06007E3C RID: 32316 RVA: 0x000F7982 File Offset: 0x000F5B82
	public ResearchType(string id, string name, string description, Sprite sprite, Color color, Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription)
	{
		this._id = id;
		this._name = name;
		this._description = description;
		this._sprite = sprite;
		this._color = color;
		this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
	}

	// Token: 0x06007E3D RID: 32317 RVA: 0x003365E8 File Offset: 0x003347E8
	public GameObject CreatePrefab(Recipe.Ingredient[] fabricationIngredients, float fabricationTime, HashedString kAnim_ID, string[] fabricators, string recipeDescription, Color color)
	{
		GameObject gameObject = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, null, 293f);
		gameObject.AddOrGet<ResearchPointObject>().TypeID = this.id;
		this._recipe = new Recipe(this.id, 1f, (SimHashes)0, this.name, recipeDescription, 0);
		this._recipe.SetFabricators(fabricators, fabricationTime);
		this._recipe.SetIcon(Assets.GetSprite("research_type_icon"), color);
		if (fabricationIngredients != null)
		{
			foreach (Recipe.Ingredient ingredient in fabricationIngredients)
			{
				this._recipe.AddIngredient(ingredient);
			}
		}
		return gameObject;
	}

	// Token: 0x06007E3E RID: 32318 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06007E3F RID: 32319 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06007E40 RID: 32320 RVA: 0x000F79C2 File Offset: 0x000F5BC2
	public string id
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06007E41 RID: 32321 RVA: 0x000F79CA File Offset: 0x000F5BCA
	public string name
	{
		get
		{
			return this._name;
		}
	}

	// Token: 0x170007F7 RID: 2039
	// (get) Token: 0x06007E42 RID: 32322 RVA: 0x000F79D2 File Offset: 0x000F5BD2
	public string description
	{
		get
		{
			return this._description;
		}
	}

	// Token: 0x170007F8 RID: 2040
	// (get) Token: 0x06007E43 RID: 32323 RVA: 0x000F79DA File Offset: 0x000F5BDA
	public string recipe
	{
		get
		{
			return this.recipe;
		}
	}

	// Token: 0x170007F9 RID: 2041
	// (get) Token: 0x06007E44 RID: 32324 RVA: 0x000F79E2 File Offset: 0x000F5BE2
	public Color color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x170007FA RID: 2042
	// (get) Token: 0x06007E45 RID: 32325 RVA: 0x000F79EA File Offset: 0x000F5BEA
	public Sprite sprite
	{
		get
		{
			return this._sprite;
		}
	}

	// Token: 0x04005FEE RID: 24558
	private string _id;

	// Token: 0x04005FEF RID: 24559
	private string _name;

	// Token: 0x04005FF0 RID: 24560
	private string _description;

	// Token: 0x04005FF1 RID: 24561
	private Recipe _recipe;

	// Token: 0x04005FF2 RID: 24562
	private Sprite _sprite;

	// Token: 0x04005FF3 RID: 24563
	private Color _color;
}
