using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020012AC RID: 4780
[AddComponentMenu("KMonoBehaviour/scripts/EdiblesManager")]
public class EdiblesManager : KMonoBehaviour
{
	// Token: 0x060061AA RID: 25002 RVA: 0x000E417B File Offset: 0x000E237B
	public static List<EdiblesManager.FoodInfo> GetAllLoadedFoodTypes()
	{
		return EdiblesManager.s_allFoodTypes.Where(new Func<EdiblesManager.FoodInfo, bool>(DlcManager.IsCorrectDlcSubscribed)).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x060061AB RID: 25003 RVA: 0x000E4198 File Offset: 0x000E2398
	public static List<EdiblesManager.FoodInfo> GetAllFoodTypes()
	{
		global::Debug.Assert(SaveLoader.Instance != null, "Call GetAllLoadedFoodTypes from the frontend");
		return EdiblesManager.s_allFoodTypes.Where(new Func<EdiblesManager.FoodInfo, bool>(Game.IsCorrectDlcActiveForCurrentSave)).ToList<EdiblesManager.FoodInfo>();
	}

	// Token: 0x060061AC RID: 25004 RVA: 0x002C262C File Offset: 0x002C082C
	public static EdiblesManager.FoodInfo GetFoodInfo(string foodID)
	{
		string key = foodID.Replace("Compost", "");
		EdiblesManager.FoodInfo result = null;
		EdiblesManager.s_allFoodMap.TryGetValue(key, out result);
		return result;
	}

	// Token: 0x060061AD RID: 25005 RVA: 0x000E41CA File Offset: 0x000E23CA
	public static bool TryGetFoodInfo(string foodID, out EdiblesManager.FoodInfo info)
	{
		info = null;
		if (string.IsNullOrEmpty(foodID))
		{
			return false;
		}
		info = EdiblesManager.GetFoodInfo(foodID);
		return info != null;
	}

	// Token: 0x040045BF RID: 17855
	private static List<EdiblesManager.FoodInfo> s_allFoodTypes = new List<EdiblesManager.FoodInfo>();

	// Token: 0x040045C0 RID: 17856
	private static Dictionary<string, EdiblesManager.FoodInfo> s_allFoodMap = new Dictionary<string, EdiblesManager.FoodInfo>();

	// Token: 0x020012AD RID: 4781
	public class FoodInfo : IConsumableUIItem, IHasDlcRestrictions
	{
		// Token: 0x060061B0 RID: 25008 RVA: 0x000E41FC File Offset: 0x000E23FC
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x000E4204 File Offset: 0x000E2404
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x002C265C File Offset: 0x002C085C
		[Obsolete("Use constructor with required/forbidden instead")]
		public FoodInfo(string id, string dlcId, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot) : this(id, caloriesPerUnit, quality, preserveTemperatue, rotTemperature, spoilTime, can_rot, null, null)
		{
			if (dlcId != "")
			{
				this.requiredDlcIds = new string[]
				{
					dlcId
				};
			}
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x002C269C File Offset: 0x002C089C
		public FoodInfo(string id, float caloriesPerUnit, int quality, float preserveTemperatue, float rotTemperature, float spoilTime, bool can_rot, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			this.Id = id;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.CaloriesPerUnit = caloriesPerUnit;
			this.Quality = quality;
			this.PreserveTemperature = preserveTemperatue;
			this.RotTemperature = rotTemperature;
			this.StaleTime = spoilTime / 2f;
			this.SpoilTime = spoilTime;
			this.CanRot = can_rot;
			this.Name = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".NAME");
			this.Description = Strings.Get("STRINGS.ITEMS.FOOD." + id.ToUpper() + ".DESC");
			this.Effects = new List<string>();
			EdiblesManager.s_allFoodTypes.Add(this);
			EdiblesManager.s_allFoodMap[this.Id] = this;
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x000E420C File Offset: 0x000E240C
		public EdiblesManager.FoodInfo AddEffects(List<string> effects, string[] requiredDlcIds = null, string[] forbiddenDlcIds = null)
		{
			if (DlcManager.IsCorrectDlcSubscribed(requiredDlcIds, forbiddenDlcIds))
			{
				this.Effects.AddRange(effects);
			}
			return this;
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x060061B5 RID: 25013 RVA: 0x000E4224 File Offset: 0x000E2424
		public string ConsumableId
		{
			get
			{
				return this.Id;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x060061B6 RID: 25014 RVA: 0x000E422C File Offset: 0x000E242C
		public string ConsumableName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x060061B7 RID: 25015 RVA: 0x000E4234 File Offset: 0x000E2434
		public int MajorOrder
		{
			get
			{
				return this.Quality;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x060061B8 RID: 25016 RVA: 0x000E423C File Offset: 0x000E243C
		public int MinorOrder
		{
			get
			{
				return (int)this.CaloriesPerUnit;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x060061B9 RID: 25017 RVA: 0x000E4245 File Offset: 0x000E2445
		public bool Display
		{
			get
			{
				return this.CaloriesPerUnit != 0f;
			}
		}

		// Token: 0x040045C1 RID: 17857
		public string Id;

		// Token: 0x040045C2 RID: 17858
		public string Name;

		// Token: 0x040045C3 RID: 17859
		public string Description;

		// Token: 0x040045C4 RID: 17860
		public float CaloriesPerUnit;

		// Token: 0x040045C5 RID: 17861
		public float PreserveTemperature;

		// Token: 0x040045C6 RID: 17862
		public float RotTemperature;

		// Token: 0x040045C7 RID: 17863
		public float StaleTime;

		// Token: 0x040045C8 RID: 17864
		public float SpoilTime;

		// Token: 0x040045C9 RID: 17865
		public bool CanRot;

		// Token: 0x040045CA RID: 17866
		public int Quality;

		// Token: 0x040045CB RID: 17867
		public List<string> Effects;

		// Token: 0x040045CC RID: 17868
		private string[] requiredDlcIds;

		// Token: 0x040045CD RID: 17869
		private string[] forbiddenDlcIds;
	}
}
