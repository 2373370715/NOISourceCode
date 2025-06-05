using System;
using Database;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C96 RID: 3222
public class BionicUpgrade_SkilledWorker : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>
{
	// Token: 0x06003D21 RID: 15649 RVA: 0x0023DEA0 File Offset: 0x0023C0A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.Inactive;
		this.root.Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplySkillPerks)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveSkillPerks)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyModifiers)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveModifiers)).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ApplyHats)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.RemoveHats));
		this.Inactive.EventTransition(GameHashes.ScheduleBlocksChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.ScheduleChanged, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.BionicOnline, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).EventTransition(GameHashes.StartWork, this.Active, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SkilledWorker.IsMinionWorkingOnlineAndNotInBatterySaveMode)).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null);
		this.Active.EventTransition(GameHashes.ScheduleBlocksChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.ScheduleChanged, this.Inactive, new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.Transition.ConditionCallback(BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore)).EventTransition(GameHashes.BionicOffline, this.Inactive, null).EventTransition(GameHashes.StopWork, this.Inactive, null).TriggerOnEnter(GameHashes.BionicUpgradeWattageChanged, null).Enter(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.CreateFX)).Exit(new StateMachine<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance, IStateMachineTarget, BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def>.State.Callback(BionicUpgrade_SkilledWorker.ClearFX));
	}

	// Token: 0x06003D22 RID: 15650 RVA: 0x000CBFFE File Offset: 0x000CA1FE
	public static void ApplySkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.resume.ApplyAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def)smi.def).SkillPerksIds);
	}

	// Token: 0x06003D23 RID: 15651 RVA: 0x000CC01B File Offset: 0x000CA21B
	public static void RemoveSkillPerks(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.resume.RemoveAdditionalSkillPerks(((BionicUpgrade_SkilledWorker.Def)smi.def).SkillPerksIds);
	}

	// Token: 0x06003D24 RID: 15652 RVA: 0x000CC038 File Offset: 0x000CA238
	public static void ApplyModifiers(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplyModifiers();
	}

	// Token: 0x06003D25 RID: 15653 RVA: 0x000CC040 File Offset: 0x000CA240
	public static void RemoveModifiers(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveModifiers();
	}

	// Token: 0x06003D26 RID: 15654 RVA: 0x000CC048 File Offset: 0x000CA248
	public static void ApplyHats(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.ApplyHats();
	}

	// Token: 0x06003D27 RID: 15655 RVA: 0x000CC050 File Offset: 0x000CA250
	public static void RemoveHats(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.RemoveHats();
	}

	// Token: 0x06003D28 RID: 15656 RVA: 0x000CC058 File Offset: 0x000CA258
	public static bool IsMinionWorkingOnlineAndNotInBatterySaveMode(BionicUpgrade_SkilledWorker.Instance smi)
	{
		return BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsOnline(smi) && !BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.IsInBedTimeChore(smi) && BionicUpgrade_SkilledWorker.IsMinionWorkingWithAttribute(smi);
	}

	// Token: 0x06003D29 RID: 15657 RVA: 0x0023E03C File Offset: 0x0023C23C
	public static bool IsMinionWorkingWithAttribute(BionicUpgrade_SkilledWorker.Instance smi)
	{
		Workable workable = smi.worker.GetWorkable();
		return workable != null && smi.worker.GetState() == WorkerBase.State.Working && workable.GetWorkAttribute() != null && workable.GetWorkAttribute().Id == ((BionicUpgrade_SkilledWorker.Def)smi.def).AttributeId;
	}

	// Token: 0x06003D2A RID: 15658 RVA: 0x000CC072 File Offset: 0x000CA272
	public static void CreateFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		BionicUpgrade_SkilledWorker.CreateAndReturnFX(smi);
	}

	// Token: 0x06003D2B RID: 15659 RVA: 0x0023E098 File Offset: 0x0023C298
	public static BionicAttributeUseFx.Instance CreateAndReturnFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		if (!smi.isMasterNull)
		{
			smi.fx = new BionicAttributeUseFx.Instance(smi.GetComponent<KMonoBehaviour>(), new Vector3(0f, 0f, Grid.GetLayerZ(Grid.SceneLayer.FXFront)));
			smi.fx.StartSM();
			return smi.fx;
		}
		return null;
	}

	// Token: 0x06003D2C RID: 15660 RVA: 0x000CC07B File Offset: 0x000CA27B
	public static void ClearFX(BionicUpgrade_SkilledWorker.Instance smi)
	{
		smi.fx.sm.destroyFX.Trigger(smi.fx);
		smi.fx = null;
	}

	// Token: 0x02000C97 RID: 3223
	public new class Def : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.Def
	{
		// Token: 0x06003D2E RID: 15662 RVA: 0x000CC0A7 File Offset: 0x000CA2A7
		public Def(string upgradeID, string attributeID, AttributeModifier[] modifiers = null, SkillPerk[] skillPerks = null, string[] hats = null) : base(upgradeID)
		{
			this.AttributeId = attributeID;
			this.modifiers = modifiers;
			this.SkillPerksIds = skillPerks;
			this.hats = hats;
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x0023E0E8 File Offset: 0x0023C2E8
		public override string GetDescription()
		{
			string text = "";
			if (this.SkillPerksIds.Length != 0)
			{
				text += UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_PERKS;
				for (int i = 0; i < this.SkillPerksIds.Length; i++)
				{
					text += "\n";
					text += SkillPerk.GetDescription(this.SkillPerksIds[i].Id);
				}
				if (this.modifiers.Length != 0)
				{
					text += "\n\n";
				}
			}
			if (this.modifiers.Length != 0)
			{
				text += UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.HEADER_ATTRIBUTES;
				for (int j = 0; j < this.modifiers.Length; j++)
				{
					text += "\n";
					text = text + this.modifiers[j].GetName() + ": " + this.modifiers[j].GetFormattedString();
				}
			}
			return text;
		}

		// Token: 0x04002A42 RID: 10818
		public SkillPerk[] SkillPerksIds;

		// Token: 0x04002A43 RID: 10819
		public string AttributeId;

		// Token: 0x04002A44 RID: 10820
		public AttributeModifier[] modifiers;

		// Token: 0x04002A45 RID: 10821
		public string[] hats;
	}

	// Token: 0x02000C98 RID: 3224
	public new class Instance : BionicUpgrade_SM<BionicUpgrade_SkilledWorker, BionicUpgrade_SkilledWorker.Instance>.BaseInstance
	{
		// Token: 0x06003D30 RID: 15664 RVA: 0x000CC0CE File Offset: 0x000CA2CE
		public Instance(IStateMachineTarget master, BionicUpgrade_SkilledWorker.Def def) : base(master, def)
		{
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x000CC0D8 File Offset: 0x000CA2D8
		public override float GetCurrentWattageCost()
		{
			if (base.IsInsideState(base.sm.Active))
			{
				return base.Data.WattageCost;
			}
			return 0f;
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x0023E1C4 File Offset: 0x0023C3C4
		public override string GetCurrentWattageCostName()
		{
			float currentWattageCost = this.GetCurrentWattageCost();
			if (base.IsInsideState(base.sm.Active))
			{
				string str = "<b>" + ((currentWattageCost >= 0f) ? "+" : "-") + "</b>";
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_ACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), str + GameUtil.GetFormattedWattage(currentWattageCost, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.upgradeComponent.GetProperName(), GameUtil.GetFormattedWattage(this.upgradeComponent.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x0023E264 File Offset: 0x0023C464
		public void ApplyModifiers()
		{
			Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
			foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)base.smi.def).modifiers)
			{
				attributes.Add(modifier);
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x0023E2B4 File Offset: 0x0023C4B4
		public void RemoveModifiers()
		{
			Klei.AI.Attributes attributes = this.resume.GetIdentity.GetAttributes();
			foreach (AttributeModifier modifier in ((BionicUpgrade_SkilledWorker.Def)base.smi.def).modifiers)
			{
				attributes.Remove(modifier);
			}
		}

		// Token: 0x06003D35 RID: 15669 RVA: 0x0023E304 File Offset: 0x0023C504
		public void ApplyHats()
		{
			string[] hats = ((BionicUpgrade_SkilledWorker.Def)base.smi.def).hats;
			if (hats == null)
			{
				return;
			}
			MinionResume component = base.GetComponent<MinionResume>();
			string properName = Assets.GetPrefab(base.smi.def.UpgradeID).GetProperName();
			foreach (string hat in hats)
			{
				component.AddAdditionalHat(properName, hat);
			}
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x0023E378 File Offset: 0x0023C578
		public void RemoveHats()
		{
			string[] hats = ((BionicUpgrade_SkilledWorker.Def)base.smi.def).hats;
			if (hats == null)
			{
				return;
			}
			MinionResume component = base.GetComponent<MinionResume>();
			string properName = Assets.GetPrefab(base.smi.def.UpgradeID).GetProperName();
			foreach (string hat in hats)
			{
				component.RemoveAdditionalHat(properName, hat);
			}
		}

		// Token: 0x04002A46 RID: 10822
		[MyCmpGet]
		public WorkerBase worker;

		// Token: 0x04002A47 RID: 10823
		[MyCmpGet]
		public MinionResume resume;

		// Token: 0x04002A48 RID: 10824
		public BionicAttributeUseFx.Instance fx;
	}
}
