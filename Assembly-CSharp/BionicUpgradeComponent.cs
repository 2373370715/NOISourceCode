using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000C7D RID: 3197
public class BionicUpgradeComponent : Assignable, IGameObjectEffectDescriptor
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06003CB3 RID: 15539 RVA: 0x000CBAAB File Offset: 0x000C9CAB
	// (set) Token: 0x06003CB2 RID: 15538 RVA: 0x000CBAA2 File Offset: 0x000C9CA2
	public BionicUpgradeComponent.IWattageController WattageController { get; private set; }

	// Token: 0x170002BC RID: 700
	// (get) Token: 0x06003CB4 RID: 15540 RVA: 0x000CBAB3 File Offset: 0x000C9CB3
	public float CurrentWattage
	{
		get
		{
			if (!this.HasWattageController)
			{
				return 0f;
			}
			return this.WattageController.GetCurrentWattageCost();
		}
	}

	// Token: 0x170002BD RID: 701
	// (get) Token: 0x06003CB5 RID: 15541 RVA: 0x000CBACE File Offset: 0x000C9CCE
	public string CurrentWattageName
	{
		get
		{
			if (!this.HasWattageController)
			{
				return string.Format(DUPLICANTS.MODIFIERS.BIONIC_WATTS.TOOLTIP.STANDARD_INACTIVE_TEMPLATE, this.GetProperName(), GameUtil.GetFormattedWattage(this.PotentialWattage, GameUtil.WattageFormatterUnit.Automatic, true));
			}
			return this.WattageController.GetCurrentWattageCostName();
		}
	}

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x000CBB06 File Offset: 0x000C9D06
	public bool HasWattageController
	{
		get
		{
			return this.WattageController != null;
		}
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x000CBB11 File Offset: 0x000C9D11
	public float PotentialWattage
	{
		get
		{
			return this.data.WattageCost;
		}
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x000CBB1E File Offset: 0x000C9D1E
	public BionicUpgradeComponentConfig.BoosterType Booster
	{
		get
		{
			return this.data.Booster;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x000CBB2B File Offset: 0x000C9D2B
	public Func<StateMachine.Instance, StateMachine.Instance> StateMachine
	{
		get
		{
			return this.data.stateMachine;
		}
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x0023D03C File Offset: 0x0023B23C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.data = BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()];
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_OnlyOnBionics));
		base.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.AssignablePrecondition_HasAvailableSlots));
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x0023D090 File Offset: 0x0023B290
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Game.Instance.assignmentManager.Remove(this);
		this.customAssignmentUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForBoosterAssignment);
		this.customAssignablesUITooltipFunc = new Func<Assignables, string>(this.GetTooltipForMinionAssigment);
		base.Subscribe(856640610, new Action<object>(this.RefreshStatusItem));
		this.RefreshStatusItem(null);
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x0023D0F8 File Offset: 0x0023B2F8
	private void RefreshStatusItem(object data = null)
	{
		if (this.assignee == null && !base.gameObject.HasTag(GameTags.Stored))
		{
			if (this.unassignedStatusItem == Guid.Empty)
			{
				this.unassignedStatusItem = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.UnassignedBionicBooster, null);
				return;
			}
		}
		else if (this.unassignedStatusItem != Guid.Empty)
		{
			this.unassignedStatusItem = base.GetComponent<KSelectable>().RemoveStatusItem(this.unassignedStatusItem, false);
		}
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x0023D180 File Offset: 0x0023B380
	public string GetTooltipForMinionAssigment(Assignables assignables)
	{
		MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
		if (component == null)
		{
			return "ERROR N/A";
		}
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject == null)
		{
			return "ERROR N/A";
		}
		BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			return "This Duplicant cannot install boosters";
		}
		int num = smi.CountBoosterAssignments(this.PrefabID());
		string text = (num == 0) ? string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num) : string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num);
		string text2 = string.Format((smi.AssignedSlotCount < smi.UnlockedSlotCount) ? UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.AVAILABLE_SLOTS : UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NO_AVAILABLE_SLOTS, targetGameObject.GetProperName(), smi.AssignedSlotCount, smi.UnlockedSlotCount);
		string text3 = "";
		List<AttributeInstance> list = new List<AttributeInstance>(targetGameObject.GetAttributes().AttributeTable).FindAll((AttributeInstance a) => a.Attribute.ShowInUI == Klei.AI.Attribute.Display.Skill);
		for (int i = 0; i < list.Count; i++)
		{
			string str = UIConstants.ColorPrefixWhite;
			if (list[i].GetTotalValue() > 0f)
			{
				str = UIConstants.ColorPrefixGreen;
			}
			else if (list[i].GetTotalValue() < 0f)
			{
				str = UIConstants.ColorPrefixRed;
			}
			text3 += string.Format("{0}: {1}", list[i].Name, str + list[i].GetFormattedValue() + UIConstants.ColorSuffix);
			if (i != list.Count - 1)
			{
				text3 += "\n";
			}
		}
		return string.Concat(new string[]
		{
			targetGameObject.GetProperName(),
			"\n\n",
			text,
			"\n\n",
			text2,
			"\n\n",
			text3
		});
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x0023D38C File Offset: 0x0023B58C
	public string GetTooltipForBoosterAssignment(Assignables assignables)
	{
		MinionAssignablesProxy component = assignables.GetComponent<MinionAssignablesProxy>();
		if (component == null)
		{
			return "ERROR N/A";
		}
		GameObject targetGameObject = component.GetTargetGameObject();
		if (targetGameObject == null)
		{
			return "ERROR N/A";
		}
		BionicUpgradesMonitor.Instance smi = targetGameObject.GetSMI<BionicUpgradesMonitor.Instance>();
		if (smi == null)
		{
			return "ERROR N/A";
		}
		int num = smi.CountBoosterAssignments(this.PrefabID());
		string str = (num == 0) ? string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.NOT_ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num) : string.Format(UI.UISIDESCREENS.BIONIC_SIDE_SCREEN.BOOSTER_ASSIGNMENT.ALREADY_ASSIGNED, smi.gameObject.GetProperName(), num);
		return BionicUpgradeComponentConfig.GenerateTooltipForBooster(this) + "\n\n" + str;
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x000CBB38 File Offset: 0x000C9D38
	public void InformOfWattageChanged()
	{
		System.Action onWattageCostChanged = this.OnWattageCostChanged;
		if (onWattageCostChanged == null)
		{
			return;
		}
		onWattageCostChanged();
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x000CBB4A File Offset: 0x000C9D4A
	public void SetWattageController(BionicUpgradeComponent.IWattageController wattageController)
	{
		this.WattageController = wattageController;
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x0023D43C File Offset: 0x0023B63C
	public override void Assign(IAssignableIdentity new_assignee)
	{
		AssignableSlotInstance specificSlotInstance = null;
		if (new_assignee == this.assignee)
		{
			return;
		}
		if (new_assignee != this.assignee && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
		{
			Ownables soleOwner = new_assignee.GetSoleOwner();
			if (soleOwner != null)
			{
				BionicUpgradesMonitor.Instance smi = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetSMI<BionicUpgradesMonitor.Instance>();
				if (smi != null)
				{
					BionicUpgradesMonitor.UpgradeComponentSlot firstEmptyAvailableSlot = smi.GetFirstEmptyAvailableSlot();
					if (firstEmptyAvailableSlot != null)
					{
						specificSlotInstance = firstEmptyAvailableSlot.GetAssignableSlotInstance();
					}
				}
			}
		}
		base.Assign(new_assignee, specificSlotInstance);
		base.Trigger(1980521255, null);
		this.RefreshStatusItem(null);
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x000CBB53 File Offset: 0x000C9D53
	public override void Unassign()
	{
		base.Unassign();
		base.Trigger(1980521255, null);
		this.RefreshStatusItem(null);
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x000CBB6E File Offset: 0x000C9D6E
	private bool AssignablePrecondition_OnlyOnBionics(MinionAssignablesProxy worker)
	{
		return worker.GetMinionModel() == BionicMinionConfig.MODEL;
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x0023D4C8 File Offset: 0x0023B6C8
	private bool AssignablePrecondition_HasAvailableSlots(MinionAssignablesProxy worker)
	{
		if (SelectTool.Instance.selected != null && SelectTool.Instance.selected.gameObject == worker.GetTargetGameObject())
		{
			return true;
		}
		MinionIdentity minionIdentity = worker.target as MinionIdentity;
		if (minionIdentity != null)
		{
			BionicUpgradesMonitor.Instance smi = minionIdentity.GetSMI<BionicUpgradesMonitor.Instance>();
			if (smi == null)
			{
				return true;
			}
			foreach (BionicUpgradesMonitor.UpgradeComponentSlot upgradeComponentSlot in smi.upgradeComponentSlots)
			{
				if (!upgradeComponentSlot.IsLocked && (upgradeComponentSlot.assignedUpgradeComponent == null || upgradeComponentSlot.assignedUpgradeComponent == this))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x000CBB80 File Offset: 0x000C9D80
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(BionicUpgradeComponentConfig.UpgradesData[base.gameObject.PrefabID()].stateMachineDescription, null, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x04002A1E RID: 10782
	private BionicUpgradeComponentConfig.BionicUpgradeData data;

	// Token: 0x04002A1F RID: 10783
	public System.Action OnWattageCostChanged;

	// Token: 0x04002A20 RID: 10784
	private Guid unassignedStatusItem = Guid.Empty;

	// Token: 0x02000C7E RID: 3198
	public interface IWattageController
	{
		// Token: 0x06003CC7 RID: 15559
		float GetCurrentWattageCost();

		// Token: 0x06003CC8 RID: 15560
		string GetCurrentWattageCostName();
	}
}
