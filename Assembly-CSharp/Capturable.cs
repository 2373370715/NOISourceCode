using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02001176 RID: 4470
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Capturable")]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06005B0D RID: 23309 RVA: 0x000DFC4A File Offset: 0x000DDE4A
	public bool IsMarkedForCapture
	{
		get
		{
			return this.markedForCapture;
		}
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x002A4E54 File Offset: 0x002A3054
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.Capturables.Add(this);
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.attributeConverter = Db.Get().AttributeConverters.CapturableSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Ranching.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		this.multitoolContext = "capture";
		this.multitoolHitEffectTag = "fx_capture_splash";
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x002A4F14 File Offset: 0x002A3114
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
		base.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
		base.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
		if (this.markedForCapture)
		{
			Prioritizable.AddRef(base.gameObject);
		}
		this.UpdateStatusItem();
		this.UpdateChore();
		base.SetWorkTime(10f);
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x000DFC52 File Offset: 0x000DDE52
	protected override void OnCleanUp()
	{
		Components.Capturables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x002A4F84 File Offset: 0x002A3184
	public override Vector3 GetTargetPoint()
	{
		Vector3 result = base.transform.GetPosition();
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			result = component.bounds.center;
		}
		result.z = 0f;
		return result;
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x000DFC65 File Offset: 0x000DDE65
	private void OnDeath(object data)
	{
		this.allowCapture = false;
		this.markedForCapture = false;
		this.UpdateChore();
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x000DFC7B File Offset: 0x000DDE7B
	private void OnTagsChanged(object data)
	{
		this.MarkForCapture(this.markedForCapture);
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x002A4FCC File Offset: 0x002A31CC
	public void MarkForCapture(bool mark)
	{
		PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
		this.MarkForCapture(mark, priority, false);
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x002A4FEC File Offset: 0x002A31EC
	public void MarkForCapture(bool mark, PrioritySetting priority, bool updateMarkedPriority = false)
	{
		mark = (mark && this.IsCapturable());
		if (this.markedForCapture && !mark)
		{
			Prioritizable.RemoveRef(base.gameObject);
		}
		else if (!this.markedForCapture && mark)
		{
			Prioritizable.AddRef(base.gameObject);
			Prioritizable component = base.GetComponent<Prioritizable>();
			if (component)
			{
				component.SetMasterPriority(priority);
			}
		}
		else if (updateMarkedPriority && this.markedForCapture && mark)
		{
			Prioritizable component2 = base.GetComponent<Prioritizable>();
			if (component2)
			{
				component2.SetMasterPriority(priority);
			}
		}
		this.markedForCapture = mark;
		this.UpdateStatusItem();
		this.UpdateChore();
	}

	// Token: 0x06005B16 RID: 23318 RVA: 0x002A5088 File Offset: 0x002A3288
	public bool IsCapturable()
	{
		return this.allowCapture && !base.gameObject.HasTag(GameTags.Trapped) && !base.gameObject.HasTag(GameTags.Stored) && !base.gameObject.HasTag(GameTags.Creatures.Bagged);
	}

	// Token: 0x06005B17 RID: 23319 RVA: 0x002A50DC File Offset: 0x002A32DC
	private void OnRefreshUserMenu(object data)
	{
		if (!this.IsCapturable())
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (!this.markedForCapture) ? new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CAPTURE.NAME, delegate()
		{
			this.MarkForCapture(true);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", UI.USERMENUACTIONS.CANCELCAPTURE.NAME, delegate()
		{
			this.MarkForCapture(false);
		}, global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x06005B18 RID: 23320 RVA: 0x002A5180 File Offset: 0x002A3380
	private void UpdateStatusItem()
	{
		this.shouldShowSkillPerkStatusItem = this.markedForCapture;
		base.UpdateStatusItem(null);
		if (this.markedForCapture)
		{
			base.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, this);
			return;
		}
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
	}

	// Token: 0x06005B19 RID: 23321 RVA: 0x002A51E4 File Offset: 0x002A33E4
	private void UpdateChore()
	{
		if (this.markedForCapture && this.chore == null)
		{
			this.chore = new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, this, null, true, null, new Action<Chore>(this.OnChoreBegins), new Action<Chore>(this.OnChoreEnds), true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			return;
		}
		if (!this.markedForCapture && this.chore != null)
		{
			this.chore.Cancel("not marked for capture");
			this.chore = null;
		}
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x002A526C File Offset: 0x002A346C
	private void OnChoreBegins(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.root);
			smi.GetComponent<Navigator>().Stop(false, true);
		}
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x002A52A8 File Offset: 0x002A34A8
	private void OnChoreEnds(Chore chore)
	{
		IdleStates.Instance smi = base.gameObject.GetSMI<IdleStates.Instance>();
		if (smi != null)
		{
			smi.GoTo(smi.sm.GetDefaultState());
		}
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x000DFC89 File Offset: 0x000DDE89
	protected override void OnStartWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned, false);
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x000DFC9C File Offset: 0x000DDE9C
	protected override void OnStopWork(WorkerBase worker)
	{
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);
	}

	// Token: 0x06005B1E RID: 23326 RVA: 0x002A52D8 File Offset: 0x002A34D8
	protected override void OnCompleteWork(WorkerBase worker)
	{
		int num = this.NaturalBuildingCell();
		if (Grid.Solid[num])
		{
			int num2 = Grid.CellAbove(num);
			if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
			{
				num = num2;
			}
		}
		this.MarkForCapture(false);
		this.baggable.SetWrangled();
		this.baggable.transform.SetPosition(Grid.CellToPosCCC(num, Grid.SceneLayer.Ore));
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x002A5344 File Offset: 0x002A3544
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		if (this.allowCapture)
		{
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
		}
		return descriptors;
	}

	// Token: 0x040040CE RID: 16590
	[MyCmpAdd]
	private Baggable baggable;

	// Token: 0x040040CF RID: 16591
	[MyCmpAdd]
	private Prioritizable prioritizable;

	// Token: 0x040040D0 RID: 16592
	public bool allowCapture = true;

	// Token: 0x040040D1 RID: 16593
	[Serialize]
	private bool markedForCapture;

	// Token: 0x040040D2 RID: 16594
	private Chore chore;

	// Token: 0x040040D3 RID: 16595
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x040040D4 RID: 16596
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});

	// Token: 0x040040D5 RID: 16597
	private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>(delegate(Capturable component, object data)
	{
		component.OnTagsChanged(data);
	});
}
