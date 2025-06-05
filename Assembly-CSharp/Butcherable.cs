using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200108E RID: 4238
[AddComponentMenu("KMonoBehaviour/Workable/Butcherable")]
public class Butcherable : Workable, ISaveLoadable
{
	// Token: 0x06005615 RID: 22037 RVA: 0x000DC8CA File Offset: 0x000DAACA
	public void SetDrops(string[] drops)
	{
		this.drops = drops;
	}

	// Token: 0x06005616 RID: 22038 RVA: 0x0028EAF4 File Offset: 0x0028CCF4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
		base.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
		this.workTime = 3f;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x06005617 RID: 22039 RVA: 0x000DC8D3 File Offset: 0x000DAAD3
	public void SetReadyToButcher(object param)
	{
		this.readyToButcher = true;
	}

	// Token: 0x06005618 RID: 22040 RVA: 0x000DC8DC File Offset: 0x000DAADC
	public void SetReadyToButcher(bool ready)
	{
		this.readyToButcher = ready;
	}

	// Token: 0x06005619 RID: 22041 RVA: 0x0028EB54 File Offset: 0x0028CD54
	public void ActivateChore(object param)
	{
		if (this.chore != null)
		{
			return;
		}
		this.chore = new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.OnRefreshUserMenu(null);
	}

	// Token: 0x0600561A RID: 22042 RVA: 0x000DC8E5 File Offset: 0x000DAAE5
	public void CancelChore(object param)
	{
		if (this.chore == null)
		{
			return;
		}
		this.chore.Cancel("User cancelled");
		this.chore = null;
	}

	// Token: 0x0600561B RID: 22043 RVA: 0x000DC907 File Offset: 0x000DAB07
	private void OnClickCancel()
	{
		this.CancelChore(null);
	}

	// Token: 0x0600561C RID: 22044 RVA: 0x000DC910 File Offset: 0x000DAB10
	private void OnClickButcher()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnButcherComplete();
			return;
		}
		this.ActivateChore(null);
	}

	// Token: 0x0600561D RID: 22045 RVA: 0x0028EBA0 File Offset: 0x0028CDA0
	private void OnRefreshUserMenu(object data)
	{
		if (!this.readyToButcher)
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = (this.chore != null) ? new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), global::Action.NumActions, null, null, null, "", true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), global::Action.NumActions, null, null, null, "", true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0600561E RID: 22046 RVA: 0x000DC927 File Offset: 0x000DAB27
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.OnButcherComplete();
	}

	// Token: 0x0600561F RID: 22047 RVA: 0x0028EC30 File Offset: 0x0028CE30
	public GameObject[] CreateDrops()
	{
		GameObject[] array = new GameObject[this.drops.Length];
		for (int i = 0; i < this.drops.Length; i++)
		{
			GameObject gameObject = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.drops[i], Grid.SceneLayer.Ore);
			gameObject.SetActive(true);
			Edible component = gameObject.GetComponent<Edible>();
			if (component)
			{
				ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component.Calories, StringFormatter.Replace(UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", gameObject.GetProperName()), UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
			}
			array[i] = gameObject;
		}
		return array;
	}

	// Token: 0x06005620 RID: 22048 RVA: 0x0028ECC8 File Offset: 0x0028CEC8
	public void OnButcherComplete()
	{
		if (this.butchered)
		{
			return;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		if (component && component.IsSelected)
		{
			SelectTool.Instance.Select(null, false);
		}
		Pickupable component2 = base.GetComponent<Pickupable>();
		Storage storage = (component2 != null) ? component2.storage : null;
		GameObject[] array = this.CreateDrops();
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (storage != null && storage.storeDropsFromButcherables)
				{
					storage.Store(array[i], false, false, true, false);
				}
			}
		}
		this.chore = null;
		this.butchered = true;
		this.readyToButcher = false;
		Game.Instance.userMenu.Refresh(base.gameObject);
		base.Trigger(395373363, array);
	}

	// Token: 0x06005621 RID: 22049 RVA: 0x0028ED90 File Offset: 0x0028CF90
	private int GetDropSpawnLocation()
	{
		int num = Grid.PosToCell(base.gameObject);
		int num2 = Grid.CellAbove(num);
		if (Grid.IsValidCell(num2) && !Grid.Solid[num2])
		{
			return num2;
		}
		return num;
	}

	// Token: 0x04003CEA RID: 15594
	[MyCmpGet]
	private KAnimControllerBase controller;

	// Token: 0x04003CEB RID: 15595
	[MyCmpGet]
	private Harvestable harvestable;

	// Token: 0x04003CEC RID: 15596
	private bool readyToButcher;

	// Token: 0x04003CED RID: 15597
	private bool butchered;

	// Token: 0x04003CEE RID: 15598
	public string[] drops;

	// Token: 0x04003CEF RID: 15599
	private Chore chore;

	// Token: 0x04003CF0 RID: 15600
	private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.SetReadyToButcher(data);
	});

	// Token: 0x04003CF1 RID: 15601
	private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>(delegate(Butcherable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
