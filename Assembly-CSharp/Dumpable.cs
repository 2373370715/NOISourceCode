using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200128F RID: 4751
[AddComponentMenu("KMonoBehaviour/Workable/Dumpable")]
public class Dumpable : Workable
{
	// Token: 0x0600610A RID: 24842 RVA: 0x000E3A10 File Offset: 0x000E1C10
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
	}

	// Token: 0x0600610B RID: 24843 RVA: 0x002BE2B8 File Offset: 0x002BC4B8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.isMarkedForDumping)
		{
			this.CreateChore();
		}
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_dumpable_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"working"
		};
		this.synchronizeAnims = false;
		base.SetWorkTime(1f);
	}

	// Token: 0x0600610C RID: 24844 RVA: 0x002BE328 File Offset: 0x002BC528
	public void ToggleDumping()
	{
		if (DebugHandler.InstantBuildMode)
		{
			this.OnCompleteWork(null);
			return;
		}
		if (this.isMarkedForDumping)
		{
			this.isMarkedForDumping = false;
			this.chore.Cancel("Cancel Dumping!");
			Prioritizable.RemoveRef(base.gameObject);
			this.chore = null;
			base.ShowProgressBar(false);
			return;
		}
		this.isMarkedForDumping = true;
		this.CreateChore();
	}

	// Token: 0x0600610D RID: 24845 RVA: 0x002BE38C File Offset: 0x002BC58C
	private void CreateChore()
	{
		if (this.chore == null)
		{
			Prioritizable.AddRef(base.gameObject);
			this.chore = new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x0600610E RID: 24846 RVA: 0x000E3A3E File Offset: 0x000E1C3E
	protected override void OnCompleteWork(WorkerBase worker)
	{
		this.isMarkedForDumping = false;
		this.chore = null;
		this.Dump();
		Prioritizable.RemoveRef(base.gameObject);
	}

	// Token: 0x0600610F RID: 24847 RVA: 0x000E3A5F File Offset: 0x000E1C5F
	public void Dump()
	{
		this.Dump(base.transform.GetPosition());
	}

	// Token: 0x06006110 RID: 24848 RVA: 0x002BE3D8 File Offset: 0x002BC5D8
	public void Dump(Vector3 pos)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			if (component.Element.IsLiquid)
			{
				FallingWater.instance.AddParticle(Grid.PosToCell(pos), component.Element.idx, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, false, false, false);
			}
			else
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
			}
		}
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06006111 RID: 24849 RVA: 0x002BE480 File Offset: 0x002BC680
	private void OnRefreshUserMenu(object data)
	{
		if (this.HasTag(GameTags.Stored))
		{
			return;
		}
		KIconButtonMenu.ButtonInfo button = this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.DUMP.TOOLTIP, true);
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x0400455D RID: 17757
	private Chore chore;

	// Token: 0x0400455E RID: 17758
	[Serialize]
	private bool isMarkedForDumping;

	// Token: 0x0400455F RID: 17759
	private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>(delegate(Dumpable component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
