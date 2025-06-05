using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001739 RID: 5945
[SerializationConfig(MemberSerialization.OptIn)]
public class PlantSubSpeciesCatalog : KMonoBehaviour
{
	// Token: 0x06007A4B RID: 31307 RVA: 0x000F4F87 File Offset: 0x000F3187
	public static void DestroyInstance()
	{
		PlantSubSpeciesCatalog.Instance = null;
	}

	// Token: 0x170007A3 RID: 1955
	// (get) Token: 0x06007A4C RID: 31308 RVA: 0x00325C54 File Offset: 0x00323E54
	public bool AnyNonOriginalDiscovered
	{
		get
		{
			foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
			{
				if (keyValuePair.Value.Find((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !ss.IsOriginal).IsValid)
				{
					return true;
				}
			}
			return false;
		}
	}

	// Token: 0x06007A4D RID: 31309 RVA: 0x000F4F8F File Offset: 0x000F318F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		PlantSubSpeciesCatalog.Instance = this;
	}

	// Token: 0x06007A4E RID: 31310 RVA: 0x000F4F9D File Offset: 0x000F319D
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.EnsureOriginalSubSpecies();
	}

	// Token: 0x06007A4F RID: 31311 RVA: 0x000F4FAB File Offset: 0x000F31AB
	public List<Tag> GetAllDiscoveredSpecies()
	{
		return this.discoveredSubspeciesBySpecies.Keys.ToList<Tag>();
	}

	// Token: 0x06007A50 RID: 31312 RVA: 0x00325CDC File Offset: 0x00323EDC
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllSubSpeciesForSpecies(Tag speciesID)
	{
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> result;
		if (this.discoveredSubspeciesBySpecies.TryGetValue(speciesID, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x06007A51 RID: 31313 RVA: 0x00325CFC File Offset: 0x00323EFC
	public bool GetOriginalSubSpecies(Tag speciesID, out PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo)
	{
		if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
		{
			subSpeciesInfo = default(PlantSubSpeciesCatalog.SubSpeciesInfo);
			return false;
		}
		subSpeciesInfo = this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.IsOriginal);
		return true;
	}

	// Token: 0x06007A52 RID: 31314 RVA: 0x00325D58 File Offset: 0x00323F58
	public PlantSubSpeciesCatalog.SubSpeciesInfo GetSubSpecies(Tag speciesID, Tag subSpeciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].Find((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID);
	}

	// Token: 0x06007A53 RID: 31315 RVA: 0x00325D90 File Offset: 0x00323F90
	public PlantSubSpeciesCatalog.SubSpeciesInfo FindSubSpecies(Tag subSpeciesID)
	{
		Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> <>9__0;
		foreach (KeyValuePair<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> keyValuePair in this.discoveredSubspeciesBySpecies)
		{
			List<PlantSubSpeciesCatalog.SubSpeciesInfo> value = keyValuePair.Value;
			Predicate<PlantSubSpeciesCatalog.SubSpeciesInfo> match;
			if ((match = <>9__0) == null)
			{
				match = (<>9__0 = ((PlantSubSpeciesCatalog.SubSpeciesInfo i) => i.ID == subSpeciesID));
			}
			PlantSubSpeciesCatalog.SubSpeciesInfo result = value.Find(match);
			if (result.ID.IsValid)
			{
				return result;
			}
		}
		return default(PlantSubSpeciesCatalog.SubSpeciesInfo);
	}

	// Token: 0x06007A54 RID: 31316 RVA: 0x00325E38 File Offset: 0x00324038
	public void DiscoverSubSpecies(PlantSubSpeciesCatalog.SubSpeciesInfo newSubSpeciesInfo, MutantPlant source)
	{
		if (!this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Contains(newSubSpeciesInfo))
		{
			this.discoveredSubspeciesBySpecies[newSubSpeciesInfo.speciesID].Add(newSubSpeciesInfo);
			Notification notification = new Notification(MISC.NOTIFICATIONS.NEWMUTANTSEED.NAME, NotificationType.Good, new Func<List<Notification>, object, string>(this.NewSubspeciesTooltipCB), newSubSpeciesInfo, true, 0f, null, null, source.transform, true, false, false);
			base.gameObject.AddOrGet<Notifier>().Add(notification, "");
		}
	}

	// Token: 0x06007A55 RID: 31317 RVA: 0x00325EC0 File Offset: 0x003240C0
	private string NewSubspeciesTooltipCB(List<Notification> notifications, object data)
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = (PlantSubSpeciesCatalog.SubSpeciesInfo)data;
		return MISC.NOTIFICATIONS.NEWMUTANTSEED.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x06007A56 RID: 31318 RVA: 0x00325EF0 File Offset: 0x003240F0
	public void IdentifySubSpecies(Tag subSpeciesID)
	{
		if (this.identifiedSubSpecies.Add(subSpeciesID))
		{
			this.FindSubSpecies(subSpeciesID);
			foreach (object obj in Components.MutantPlants)
			{
				MutantPlant mutantPlant = (MutantPlant)obj;
				if (mutantPlant.HasTag(subSpeciesID))
				{
					mutantPlant.UpdateNameAndTags();
				}
			}
			GeneticAnalysisCompleteMessage message = new GeneticAnalysisCompleteMessage(subSpeciesID);
			Messenger.Instance.QueueMessage(message);
		}
	}

	// Token: 0x06007A57 RID: 31319 RVA: 0x000F4FBD File Offset: 0x000F31BD
	public bool IsSubSpeciesIdentified(Tag subSpeciesID)
	{
		return this.identifiedSubSpecies.Contains(subSpeciesID);
	}

	// Token: 0x06007A58 RID: 31320 RVA: 0x000F4FCB File Offset: 0x000F31CB
	public List<PlantSubSpeciesCatalog.SubSpeciesInfo> GetAllUnidentifiedSubSpecies(Tag speciesID)
	{
		return this.discoveredSubspeciesBySpecies[speciesID].FindAll((PlantSubSpeciesCatalog.SubSpeciesInfo ss) => !this.IsSubSpeciesIdentified(ss.ID));
	}

	// Token: 0x06007A59 RID: 31321 RVA: 0x00325F78 File Offset: 0x00324178
	public bool IsValidPlantableSeed(Tag seedID, Tag subspeciesID)
	{
		if (!seedID.IsValid)
		{
			return false;
		}
		MutantPlant component = Assets.GetPrefab(seedID).GetComponent<MutantPlant>();
		if (component == null)
		{
			return !subspeciesID.IsValid;
		}
		List<PlantSubSpeciesCatalog.SubSpeciesInfo> allSubSpeciesForSpecies = PlantSubSpeciesCatalog.Instance.GetAllSubSpeciesForSpecies(component.SpeciesID);
		return allSubSpeciesForSpecies != null && allSubSpeciesForSpecies.FindIndex((PlantSubSpeciesCatalog.SubSpeciesInfo s) => s.ID == subspeciesID) != -1 && PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(subspeciesID);
	}

	// Token: 0x06007A5A RID: 31322 RVA: 0x00325FFC File Offset: 0x003241FC
	private void EnsureOriginalSubSpecies()
	{
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<MutantPlant>())
		{
			MutantPlant component = gameObject.GetComponent<MutantPlant>();
			Tag speciesID = component.SpeciesID;
			if (!this.discoveredSubspeciesBySpecies.ContainsKey(speciesID))
			{
				this.discoveredSubspeciesBySpecies[speciesID] = new List<PlantSubSpeciesCatalog.SubSpeciesInfo>();
				this.discoveredSubspeciesBySpecies[speciesID].Add(component.GetSubSpeciesInfo());
			}
			this.identifiedSubSpecies.Add(component.SubSpeciesID);
		}
	}

	// Token: 0x04005C01 RID: 23553
	public static PlantSubSpeciesCatalog Instance;

	// Token: 0x04005C02 RID: 23554
	[Serialize]
	private Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>> discoveredSubspeciesBySpecies = new Dictionary<Tag, List<PlantSubSpeciesCatalog.SubSpeciesInfo>>();

	// Token: 0x04005C03 RID: 23555
	[Serialize]
	private HashSet<Tag> identifiedSubSpecies = new HashSet<Tag>();

	// Token: 0x0200173A RID: 5946
	[Serializable]
	public struct SubSpeciesInfo : IEquatable<PlantSubSpeciesCatalog.SubSpeciesInfo>
	{
		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06007A5D RID: 31325 RVA: 0x000F5019 File Offset: 0x000F3219
		public bool IsValid
		{
			get
			{
				return this.ID.IsValid;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06007A5E RID: 31326 RVA: 0x000F5026 File Offset: 0x000F3226
		public bool IsOriginal
		{
			get
			{
				return this.mutationIDs == null || this.mutationIDs.Count == 0;
			}
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x000F5040 File Offset: 0x000F3240
		public SubSpeciesInfo(Tag speciesID, List<string> mutationIDs)
		{
			this.speciesID = speciesID;
			this.mutationIDs = ((mutationIDs != null) ? new List<string>(mutationIDs) : new List<string>());
			this.ID = PlantSubSpeciesCatalog.SubSpeciesInfo.SubSpeciesIDFromMutations(speciesID, mutationIDs);
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x0032609C File Offset: 0x0032429C
		public static Tag SubSpeciesIDFromMutations(Tag speciesID, List<string> mutationIDs)
		{
			if (mutationIDs == null || mutationIDs.Count == 0)
			{
				Tag tag = speciesID;
				return tag.ToString() + "_Original";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(speciesID);
			foreach (string value in mutationIDs)
			{
				stringBuilder.Append("_");
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString().ToTag();
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x00326140 File Offset: 0x00324340
		public string GetMutationsNames()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.PLANT_MUTATIONS.NONE.NAME;
			}
			return string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs));
		}

		// Token: 0x06007A62 RID: 31330 RVA: 0x0032618C File Offset: 0x0032438C
		public string GetNameWithMutations(string properName, bool identified, bool cleanOriginal)
		{
			string result;
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				if (cleanOriginal)
				{
					result = properName;
				}
				else
				{
					result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.NONE.NAME);
				}
			}
			else if (!identified)
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", CREATURES.PLANT_MUTATIONS.UNIDENTIFIED);
			}
			else
			{
				result = CREATURES.PLANT_MUTATIONS.PLANT_NAME_FMT.Replace("{PlantName}", properName).Replace("{MutationList}", string.Join(", ", Db.Get().PlantMutations.GetNamesForMutations(this.mutationIDs)));
			}
			return result;
		}

		// Token: 0x06007A63 RID: 31331 RVA: 0x000F506C File Offset: 0x000F326C
		public static bool operator ==(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return obj1.Equals(obj2);
		}

		// Token: 0x06007A64 RID: 31332 RVA: 0x000F5076 File Offset: 0x000F3276
		public static bool operator !=(PlantSubSpeciesCatalog.SubSpeciesInfo obj1, PlantSubSpeciesCatalog.SubSpeciesInfo obj2)
		{
			return !(obj1 == obj2);
		}

		// Token: 0x06007A65 RID: 31333 RVA: 0x000F5082 File Offset: 0x000F3282
		public override bool Equals(object other)
		{
			return other is PlantSubSpeciesCatalog.SubSpeciesInfo && this == (PlantSubSpeciesCatalog.SubSpeciesInfo)other;
		}

		// Token: 0x06007A66 RID: 31334 RVA: 0x000F509F File Offset: 0x000F329F
		public bool Equals(PlantSubSpeciesCatalog.SubSpeciesInfo other)
		{
			return this.ID == other.ID;
		}

		// Token: 0x06007A67 RID: 31335 RVA: 0x000F50B2 File Offset: 0x000F32B2
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		// Token: 0x06007A68 RID: 31336 RVA: 0x00326244 File Offset: 0x00324444
		public string GetMutationsTooltip()
		{
			if (this.mutationIDs == null || this.mutationIDs.Count == 0)
			{
				return CREATURES.STATUSITEMS.ORIGINALPLANTMUTATION.TOOLTIP;
			}
			if (!PlantSubSpeciesCatalog.Instance.IsSubSpeciesIdentified(this.ID))
			{
				return CREATURES.STATUSITEMS.UNKNOWNMUTATION.TOOLTIP;
			}
			string id = this.mutationIDs[0];
			PlantMutation plantMutation = Db.Get().PlantMutations.Get(id);
			return CREATURES.STATUSITEMS.SPECIFICPLANTMUTATION.TOOLTIP.Replace("{MutationName}", plantMutation.Name) + "\n" + plantMutation.GetTooltip();
		}

		// Token: 0x04005C04 RID: 23556
		public Tag speciesID;

		// Token: 0x04005C05 RID: 23557
		public Tag ID;

		// Token: 0x04005C06 RID: 23558
		public List<string> mutationIDs;

		// Token: 0x04005C07 RID: 23559
		private const string ORIGINAL_SUFFIX = "_Original";
	}
}
