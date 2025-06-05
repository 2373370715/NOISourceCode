using System;
using STRINGS;

// Token: 0x020019D6 RID: 6614
public class ConditionHasAtmoSuit : ProcessCondition
{
	// Token: 0x060089DB RID: 35291 RVA: 0x00368364 File Offset: 0x00366564
	public ConditionHasAtmoSuit(CommandModule module)
	{
		this.module = module;
		ManualDeliveryKG manualDeliveryKG = this.module.FindOrAdd<ManualDeliveryKG>();
		manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
		manualDeliveryKG.SetStorage(module.storage);
		manualDeliveryKG.RequestedItemTag = GameTags.AtmoSuit;
		manualDeliveryKG.MinimumMass = 1f;
		manualDeliveryKG.refillMass = 0.1f;
		manualDeliveryKG.capacity = 1f;
	}

	// Token: 0x060089DC RID: 35292 RVA: 0x000FE95D File Offset: 0x000FCB5D
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.module.storage.GetAmountAvailable(GameTags.AtmoSuit) < 1f)
		{
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x060089DD RID: 35293 RVA: 0x000FE983 File Offset: 0x000FCB83
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.NAME;
		}
		return UI.STARMAP.NOSUIT.NAME;
	}

	// Token: 0x060089DE RID: 35294 RVA: 0x000FE99E File Offset: 0x000FCB9E
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return UI.STARMAP.HASSUIT.TOOLTIP;
		}
		return UI.STARMAP.NOSUIT.TOOLTIP;
	}

	// Token: 0x060089DF RID: 35295 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x0400683D RID: 26685
	private CommandModule module;
}
