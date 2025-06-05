using System;
using System.Collections.Generic;
using ImGuiNET;
using STRINGS;

// Token: 0x02000C1C RID: 3100
public class DevTool_StoryTrait_CritterManipulator : DevTool
{
	// Token: 0x06003AC1 RID: 15041 RVA: 0x00236204 File Offset: 0x00234404
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.CollapsingHeader("Debug species lore unlock popup", ImGuiTreeNodeFlags.DefaultOpen))
		{
			this.Button_OpenSpecies(Tag.Invalid, "Unknown Species");
			ImGui.Separator();
			foreach (Tag species in this.GetCritterSpeciesTags())
			{
				this.Button_OpenSpecies(species, GravitasCreatureManipulatorConfig.GetNameForSpeciesTag(species).Unwrap());
			}
		}
	}

	// Token: 0x06003AC2 RID: 15042 RVA: 0x000CA6A3 File Offset: 0x000C88A3
	public void Button_OpenSpecies(Tag species, string speciesName = null)
	{
		if (speciesName == null)
		{
			speciesName = species.Name;
		}
		else
		{
			speciesName = string.Format("\"{0}\" (ID: {1})", UI.StripLinkFormatting(speciesName), species);
		}
		if (ImGui.Button("Show popup for: " + speciesName))
		{
			GravitasCreatureManipulator.Instance.ShowLoreUnlockedPopup(species);
		}
	}

	// Token: 0x06003AC3 RID: 15043 RVA: 0x000CA6E3 File Offset: 0x000C88E3
	public IEnumerable<Tag> GetCritterSpeciesTags()
	{
		yield return GameTags.Creatures.Species.HatchSpecies;
		yield return GameTags.Creatures.Species.LightBugSpecies;
		yield return GameTags.Creatures.Species.OilFloaterSpecies;
		yield return GameTags.Creatures.Species.DreckoSpecies;
		yield return GameTags.Creatures.Species.GlomSpecies;
		yield return GameTags.Creatures.Species.PuftSpecies;
		yield return GameTags.Creatures.Species.PacuSpecies;
		yield return GameTags.Creatures.Species.MooSpecies;
		yield return GameTags.Creatures.Species.MoleSpecies;
		yield return GameTags.Creatures.Species.SquirrelSpecies;
		yield return GameTags.Creatures.Species.CrabSpecies;
		yield return GameTags.Creatures.Species.DivergentSpecies;
		yield return GameTags.Creatures.Species.StaterpillarSpecies;
		yield return GameTags.Creatures.Species.BeetaSpecies;
		yield break;
	}
}
