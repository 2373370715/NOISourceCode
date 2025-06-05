using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009B1 RID: 2481
[AddComponentMenu("KMonoBehaviour/Workable/Activatable")]
public class Activatable : Workable, ISidescreenButtonControl
{
	// Token: 0x17000197 RID: 407
	// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000C132B File Offset: 0x000BF52B
	public bool IsActivated
	{
		get
		{
			return this.activated;
		}
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x000C1333 File Offset: 0x000BF533
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x000C133B File Offset: 0x000BF53B
	protected override void OnSpawn()
	{
		this.UpdateFlag();
		if (this.awaitingActivation && this.activateChore == null)
		{
			this.CreateChore();
		}
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x000C1359 File Offset: 0x000BF559
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.activated = true;
		if (this.onActivate != null)
		{
			this.onActivate();
		}
		this.awaitingActivation = false;
		this.UpdateFlag();
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x001F9AD0 File Offset: 0x001F7CD0
	private void UpdateFlag()
	{
		base.GetComponent<Operational>().SetFlag(this.Required ? Activatable.activatedFlagRequirement : Activatable.activatedFlagFunctional, this.activated);
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.DuplicantActivationRequired, !this.activated, null);
		base.Trigger(-1909216579, this.IsActivated);
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x001F9B40 File Offset: 0x001F7D40
	private void CreateChore()
	{
		if (this.activateChore != null)
		{
			return;
		}
		Prioritizable.AddRef(base.gameObject);
		this.activateChore = new WorkChore<Activatable>(Db.Get().ChoreTypes.Toggle, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		if (!string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			this.shouldShowSkillPerkStatusItem = true;
			this.requireMinionToWork = true;
			this.UpdateStatusItem(null);
		}
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000C1394 File Offset: 0x000BF594
	private void CancelChore()
	{
		if (this.activateChore == null)
		{
			return;
		}
		this.activateChore.Cancel("User cancelled");
		this.activateChore = null;
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000198 RID: 408
	// (get) Token: 0x06002C6D RID: 11373 RVA: 0x001F9BB0 File Offset: 0x001F7DB0
	public string SidescreenButtonText
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelText : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.Text : UI.USERMENUACTIONS.ACTIVATEBUILDING.ACTIVATE;
		}
	}

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x06002C6E RID: 11374 RVA: 0x001F9C10 File Offset: 0x001F7E10
	public string SidescreenButtonTooltip
	{
		get
		{
			if (this.activateChore != null)
			{
				return this.textOverride.IsValid ? this.textOverride.CancelToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_CANCEL;
			}
			return this.textOverride.IsValid ? this.textOverride.ToolTip : UI.USERMENUACTIONS.ACTIVATEBUILDING.TOOLTIP_ACTIVATE;
		}
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000C13B6 File Offset: 0x000BF5B6
	public bool SidescreenEnabled()
	{
		return !this.activated;
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000C13C1 File Offset: 0x000BF5C1
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		this.textOverride = text;
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000C13CA File Offset: 0x000BF5CA
	public void OnSidescreenButtonPressed()
	{
		if (this.activateChore == null)
		{
			this.CreateChore();
		}
		else
		{
			this.CancelChore();
		}
		this.awaitingActivation = (this.activateChore != null);
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000C13B6 File Offset: 0x000BF5B6
	public bool SidescreenButtonInteractable()
	{
		return !this.activated;
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x04001E70 RID: 7792
	public bool Required = true;

	// Token: 0x04001E71 RID: 7793
	private static readonly Operational.Flag activatedFlagRequirement = new Operational.Flag("activated", Operational.Flag.Type.Requirement);

	// Token: 0x04001E72 RID: 7794
	private static readonly Operational.Flag activatedFlagFunctional = new Operational.Flag("activated", Operational.Flag.Type.Functional);

	// Token: 0x04001E73 RID: 7795
	[Serialize]
	private bool activated;

	// Token: 0x04001E74 RID: 7796
	[Serialize]
	private bool awaitingActivation;

	// Token: 0x04001E75 RID: 7797
	private Guid statusItem;

	// Token: 0x04001E76 RID: 7798
	private Chore activateChore;

	// Token: 0x04001E77 RID: 7799
	public System.Action onActivate;

	// Token: 0x04001E78 RID: 7800
	[SerializeField]
	private ButtonMenuTextOverride textOverride;
}
