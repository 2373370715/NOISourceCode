using System;
using KSerialization;
using STRINGS;

// Token: 0x02001E51 RID: 7761
public class GeneticAnalysisCompleteMessage : Message
{
	// Token: 0x0600A271 RID: 41585 RVA: 0x0010DE4C File Offset: 0x0010C04C
	public GeneticAnalysisCompleteMessage()
	{
	}

	// Token: 0x0600A272 RID: 41586 RVA: 0x0010E0AC File Offset: 0x0010C2AC
	public GeneticAnalysisCompleteMessage(Tag subSpeciesID)
	{
		this.subSpeciesID = subSpeciesID;
	}

	// Token: 0x0600A273 RID: 41587 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x0600A274 RID: 41588 RVA: 0x003EDB9C File Offset: 0x003EBD9C
	public override string GetMessageBody()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.MESSAGEBODY.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName()).Replace("{Subspecies}", subSpeciesInfo.GetNameWithMutations(subSpeciesInfo.speciesID.ProperName(), true, false)).Replace("{Info}", subSpeciesInfo.GetMutationsTooltip());
	}

	// Token: 0x0600A275 RID: 41589 RVA: 0x0010E0BB File Offset: 0x0010C2BB
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.NAME;
	}

	// Token: 0x0600A276 RID: 41590 RVA: 0x003EDC04 File Offset: 0x003EBE04
	public override string GetTooltip()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x0600A277 RID: 41591 RVA: 0x0010E0C7 File Offset: 0x0010C2C7
	public override bool IsValid()
	{
		return this.subSpeciesID.IsValid;
	}

	// Token: 0x04007F43 RID: 32579
	[Serialize]
	public Tag subSpeciesID;
}
