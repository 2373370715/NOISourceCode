using System;
using System.Linq;
using Klei.AI;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F63 RID: 8035
[AddComponentMenu("KMonoBehaviour/scripts/ScheduleMinionWidget")]
public class ScheduleMinionWidget : KMonoBehaviour
{
	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x0600A996 RID: 43414 RVA: 0x001128AB File Offset: 0x00110AAB
	// (set) Token: 0x0600A997 RID: 43415 RVA: 0x001128B3 File Offset: 0x00110AB3
	public Schedulable schedulable { get; private set; }

	// Token: 0x0600A998 RID: 43416 RVA: 0x00411A60 File Offset: 0x0040FC60
	public void ChangeAssignment(Schedule targetSchedule, Schedulable schedulable)
	{
		DebugUtil.LogArgs(new object[]
		{
			"Assigning",
			schedulable,
			"from",
			ScheduleManager.Instance.GetSchedule(schedulable).name,
			"to",
			targetSchedule.name
		});
		ScheduleManager.Instance.GetSchedule(schedulable).Unassign(schedulable);
		targetSchedule.Assign(schedulable);
	}

	// Token: 0x0600A999 RID: 43417 RVA: 0x00411AC8 File Offset: 0x0040FCC8
	public void Setup(Schedulable schedulable)
	{
		this.schedulable = schedulable;
		IAssignableIdentity component = schedulable.GetComponent<IAssignableIdentity>();
		this.portrait.SetIdentityObject(component, true);
		this.label.text = component.GetProperName();
		MinionIdentity minionIdentity = component as MinionIdentity;
		StoredMinionIdentity storedMinionIdentity = component as StoredMinionIdentity;
		this.RefreshWidgetWorldData();
		if (minionIdentity != null)
		{
			Traits component2 = minionIdentity.GetComponent<Traits>();
			if (component2.HasTrait("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (component2.HasTrait("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		else if (storedMinionIdentity != null)
		{
			if (storedMinionIdentity.traitIDs.Contains("NightOwl"))
			{
				this.nightOwlIcon.SetActive(true);
			}
			else if (storedMinionIdentity.traitIDs.Contains("EarlyBird"))
			{
				this.earlyBirdIcon.SetActive(true);
			}
		}
		this.dropDown.Initialize(ScheduleManager.Instance.GetSchedules().Cast<IListableOption>(), new Action<IListableOption, object>(this.OnDropEntryClick), null, new Action<DropDownEntry, object>(this.DropEntryRefreshAction), false, schedulable);
	}

	// Token: 0x0600A99A RID: 43418 RVA: 0x00411BD8 File Offset: 0x0040FDD8
	public void RefreshWidgetWorldData()
	{
		this.worldContainer.SetActive(DlcManager.IsExpansion1Active());
		MinionIdentity minionIdentity = this.schedulable.GetComponent<IAssignableIdentity>() as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		if (DlcManager.IsExpansion1Active())
		{
			WorldContainer myWorld = minionIdentity.GetMyWorld();
			string text = myWorld.GetComponent<ClusterGridEntity>().Name;
			Image componentInChildren = this.worldContainer.GetComponentInChildren<Image>();
			componentInChildren.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
			componentInChildren.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
			if (ClusterManager.Instance.activeWorld != myWorld)
			{
				text = string.Concat(new string[]
				{
					"<color=",
					Constants.NEUTRAL_COLOR_STR,
					">",
					text,
					"</color>"
				});
			}
			this.worldContainer.GetComponentInChildren<LocText>().SetText(text);
		}
	}

	// Token: 0x0600A99B RID: 43419 RVA: 0x00411CC0 File Offset: 0x0040FEC0
	private void OnDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)option;
		this.ChangeAssignment(targetSchedule, this.schedulable);
	}

	// Token: 0x0600A99C RID: 43420 RVA: 0x00411CE4 File Offset: 0x0040FEE4
	private void DropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)entry.entryData;
		if (((Schedulable)obj).GetSchedule() == schedule)
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, schedule.name);
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = schedule.name;
			entry.button.isInteractable = true;
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(true);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(false);
	}

	// Token: 0x0600A99D RID: 43421 RVA: 0x00411DB8 File Offset: 0x0040FFB8
	public void SetupBlank(Schedule schedule)
	{
		this.label.text = UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_BLANK;
		this.dropDown.Initialize(Components.LiveMinionIdentities.Items.Cast<IListableOption>(), new Action<IListableOption, object>(this.OnBlankDropEntryClick), new Func<IListableOption, IListableOption, object, int>(this.BlankDropEntrySort), new Action<DropDownEntry, object>(this.BlankDropEntryRefreshAction), false, schedule);
		Components.LiveMinionIdentities.OnAdd += this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove += this.OnLivingMinionsChanged;
	}

	// Token: 0x0600A99E RID: 43422 RVA: 0x001128BC File Offset: 0x00110ABC
	private void OnLivingMinionsChanged(MinionIdentity minion)
	{
		this.dropDown.ChangeContent(Components.LiveMinionIdentities.Items.Cast<IListableOption>());
	}

	// Token: 0x0600A99F RID: 43423 RVA: 0x00411E48 File Offset: 0x00410048
	private void OnBlankDropEntryClick(IListableOption option, object obj)
	{
		Schedule targetSchedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)option;
		if (minionIdentity == null || minionIdentity.HasTag(GameTags.Dead))
		{
			return;
		}
		this.ChangeAssignment(targetSchedule, minionIdentity.GetComponent<Schedulable>());
	}

	// Token: 0x0600A9A0 RID: 43424 RVA: 0x00411E88 File Offset: 0x00410088
	private void BlankDropEntryRefreshAction(DropDownEntry entry, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)entry.entryData;
		WorldContainer myWorld = minionIdentity.GetMyWorld();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("worldContainer").gameObject.SetActive(DlcManager.IsExpansion1Active());
		Image reference = entry.gameObject.GetComponent<HierarchyReferences>().GetReference<Image>("worldIcon");
		reference.sprite = myWorld.GetComponent<ClusterGridEntity>().GetUISprite();
		reference.SetAlpha((ClusterManager.Instance.activeWorld == myWorld) ? 1f : 0.7f);
		string text = myWorld.GetComponent<ClusterGridEntity>().Name;
		if (ClusterManager.Instance.activeWorld != myWorld)
		{
			text = string.Concat(new string[]
			{
				"<color=",
				Constants.NEUTRAL_COLOR_STR,
				">",
				text,
				"</color>"
			});
		}
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<LocText>("worldLabel").SetText(text);
		if (schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>()))
		{
			entry.label.text = string.Format(UI.SCHEDULESCREEN.SCHEDULE_DROPDOWN_ASSIGNED, minionIdentity.GetProperName());
			entry.button.isInteractable = false;
		}
		else
		{
			entry.label.text = minionIdentity.GetProperName();
			entry.button.isInteractable = true;
		}
		Traits component = minionIdentity.GetComponent<Traits>();
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("NightOwlIcon").gameObject.SetActive(component.HasTrait("NightOwl"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("EarlyBirdIcon").gameObject.SetActive(component.HasTrait("EarlyBird"));
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("ScheduleIcon").gameObject.SetActive(false);
		entry.gameObject.GetComponent<HierarchyReferences>().GetReference<RectTransform>("PortraitContainer").gameObject.SetActive(true);
	}

	// Token: 0x0600A9A1 RID: 43425 RVA: 0x0041207C File Offset: 0x0041027C
	private int BlankDropEntrySort(IListableOption a, IListableOption b, object obj)
	{
		Schedule schedule = (Schedule)obj;
		MinionIdentity minionIdentity = (MinionIdentity)a;
		MinionIdentity minionIdentity2 = (MinionIdentity)b;
		bool flag = schedule.IsAssigned(minionIdentity.GetComponent<Schedulable>());
		bool flag2 = schedule.IsAssigned(minionIdentity2.GetComponent<Schedulable>());
		if (flag && !flag2)
		{
			return -1;
		}
		if (!flag && flag2)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600A9A2 RID: 43426 RVA: 0x001128D8 File Offset: 0x00110AD8
	protected override void OnCleanUp()
	{
		Components.LiveMinionIdentities.OnAdd -= this.OnLivingMinionsChanged;
		Components.LiveMinionIdentities.OnRemove -= this.OnLivingMinionsChanged;
	}

	// Token: 0x04008592 RID: 34194
	[SerializeField]
	private CrewPortrait portrait;

	// Token: 0x04008593 RID: 34195
	[SerializeField]
	private DropDown dropDown;

	// Token: 0x04008594 RID: 34196
	[SerializeField]
	private LocText label;

	// Token: 0x04008595 RID: 34197
	[SerializeField]
	private GameObject nightOwlIcon;

	// Token: 0x04008596 RID: 34198
	[SerializeField]
	private GameObject earlyBirdIcon;

	// Token: 0x04008597 RID: 34199
	[SerializeField]
	private GameObject worldContainer;
}
