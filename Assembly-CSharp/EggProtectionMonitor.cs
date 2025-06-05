using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020011A6 RID: 4518
public class EggProtectionMonitor : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>
{
	// Token: 0x06005BE3 RID: 23523 RVA: 0x002A73D0 File Offset: 0x002A55D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.find_egg;
		this.find_egg.BatchUpdate(new UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.BatchUpdateDelegate(EggProtectionMonitor.Instance.FindEggToGuard), UpdateRate.SIM_200ms).ParamTransition<bool>(this.hasEggToGuard, this.guard.safe, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsTrue);
		this.guard.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, "_heat", 0);
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Hostile);
		}).Exit(delegate(EggProtectionMonitor.Instance smi)
		{
			if (!smi.def.animPrefix.IsNullOrWhiteSpace())
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim("pincher_kanim"), smi.def.animPrefix, null, 0);
			}
			else
			{
				smi.gameObject.AddOrGet<SymbolOverrideController>().RemoveBuildOverride(Assets.GetAnim("pincher_kanim").GetData(), 0);
			}
			smi.gameObject.AddOrGet<FactionAlignment>().SwitchAlignment(FactionManager.FactionID.Pest);
		}).Update("CanProtectEgg", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.CanProtectEgg();
		}, UpdateRate.SIM_1000ms, true).ParamTransition<bool>(this.hasEggToGuard, this.find_egg, GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.IsFalse);
		this.guard.safe.Enter(delegate(EggProtectionMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		}).Update("EggProtectionMonitor.safe", delegate(EggProtectionMonitor.Instance smi, float dt)
		{
			smi.RefreshThreat(null);
		}, UpdateRate.SIM_200ms, true).ToggleStatusItem(CREATURES.STATUSITEMS.PROTECTINGENTITY.NAME, CREATURES.STATUSITEMS.PROTECTINGENTITY.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, null);
		this.guard.threatened.ToggleBehaviour(GameTags.Creatures.Defend, (EggProtectionMonitor.Instance smi) => smi.threatMonitor.HasThreat(), delegate(EggProtectionMonitor.Instance smi)
		{
			smi.GoTo(this.guard.safe);
		}).Update("Threatened", new Action<EggProtectionMonitor.Instance, float>(EggProtectionMonitor.CritterUpdateThreats), UpdateRate.SIM_200ms, false);
	}

	// Token: 0x06005BE4 RID: 23524 RVA: 0x000E046C File Offset: 0x000DE66C
	private static void CritterUpdateThreats(EggProtectionMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.threatMonitor.HasThreat())
		{
			smi.GoTo(smi.sm.guard.safe);
		}
	}

	// Token: 0x04004162 RID: 16738
	public StateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.BoolParameter hasEggToGuard;

	// Token: 0x04004163 RID: 16739
	public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State find_egg;

	// Token: 0x04004164 RID: 16740
	public EggProtectionMonitor.GuardEggStates guard;

	// Token: 0x020011A7 RID: 4519
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04004165 RID: 16741
		public Tag[] allyTags;

		// Token: 0x04004166 RID: 16742
		public string animPrefix;
	}

	// Token: 0x020011A8 RID: 4520
	public class GuardEggStates : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State
	{
		// Token: 0x04004167 RID: 16743
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State safe;

		// Token: 0x04004168 RID: 16744
		public GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.State threatened;
	}

	// Token: 0x020011A9 RID: 4521
	public new class Instance : GameStateMachine<EggProtectionMonitor, EggProtectionMonitor.Instance, IStateMachineTarget, EggProtectionMonitor.Def>.GameInstance
	{
		// Token: 0x06005BE9 RID: 23529 RVA: 0x000E04BD File Offset: 0x000DE6BD
		public Instance(IStateMachineTarget master, EggProtectionMonitor.Def def) : base(master, def)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x002A7590 File Offset: 0x002A5790
		public void CanProtectEgg()
		{
			bool flag = true;
			if (this.eggToProtect == null)
			{
				flag = false;
			}
			if (flag)
			{
				int num = 150;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(this.eggToProtect));
				if (navigationCost == -1 || navigationCost >= num)
				{
					flag = false;
				}
			}
			if (!flag)
			{
				this.SetEggToGuard(null);
			}
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x002A75E4 File Offset: 0x002A57E4
		public static void FindEggToGuard(List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry> instances, float time_delta)
		{
			ListPool<KPrefabID, EggProtectionMonitor>.PooledList pooledList = ListPool<KPrefabID, EggProtectionMonitor>.Allocate();
			pooledList.Capacity = Mathf.Max(pooledList.Capacity, Components.IncubationMonitors.Count);
			foreach (object obj in Components.IncubationMonitors)
			{
				IncubationMonitor.Instance instance = (IncubationMonitor.Instance)obj;
				pooledList.Add(instance.gameObject.GetComponent<KPrefabID>());
			}
			ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.PooledList pooledList2 = ListPool<EggProtectionMonitor.Instance.Egg, EggProtectionMonitor>.Allocate();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(pooledList);
			for (int i = 0; i < pooledList.Count; i += 256)
			{
				EggProtectionMonitor.Instance.find_eggs_job.Add(new EggProtectionMonitor.Instance.FindEggsTask(i, Mathf.Min(i + 256, pooledList.Count)));
			}
			GlobalJobManager.Run(EggProtectionMonitor.Instance.find_eggs_job);
			for (int num = 0; num != EggProtectionMonitor.Instance.find_eggs_job.Count; num++)
			{
				EggProtectionMonitor.Instance.find_eggs_job.GetWorkItem(num).Finish(pooledList, pooledList2);
			}
			pooledList.Recycle();
			EggProtectionMonitor.Instance.find_eggs_job.Reset(null);
			foreach (UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry entry in new List<UpdateBucketWithUpdater<EggProtectionMonitor.Instance>.Entry>(instances))
			{
				GameObject eggToGuard = null;
				int num2 = 100;
				foreach (EggProtectionMonitor.Instance.Egg egg in pooledList2)
				{
					int navigationCost = entry.data.navigator.GetNavigationCost(egg.cell);
					if (navigationCost != -1 && navigationCost < num2)
					{
						eggToGuard = egg.game_object;
						num2 = navigationCost;
					}
				}
				entry.data.SetEggToGuard(eggToGuard);
			}
			pooledList2.Recycle();
		}

		// Token: 0x06005BEC RID: 23532 RVA: 0x000E04E5 File Offset: 0x000DE6E5
		public void SetEggToGuard(GameObject egg)
		{
			this.eggToProtect = egg;
			base.sm.hasEggToGuard.Set(egg != null, base.smi, false);
		}

		// Token: 0x06005BED RID: 23533 RVA: 0x000E050D File Offset: 0x000DE70D
		public void GoToThreatened()
		{
			base.smi.GoTo(base.sm.guard.threatened);
		}

		// Token: 0x06005BEE RID: 23534 RVA: 0x002A77C8 File Offset: 0x002A59C8
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning() || this.eggToProtect == null)
			{
				return;
			}
			if (base.smi.threatMonitor.HasThreat())
			{
				this.GoToThreatened();
				return;
			}
			if (base.smi.GetCurrentState() != base.sm.guard.safe)
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.guard.safe);
			}
		}

		// Token: 0x04004169 RID: 16745
		[MySmiReq]
		public ThreatMonitor.Instance threatMonitor;

		// Token: 0x0400416A RID: 16746
		public GameObject eggToProtect;

		// Token: 0x0400416B RID: 16747
		private Navigator navigator;

		// Token: 0x0400416C RID: 16748
		private Action<object> refreshThreatDelegate;

		// Token: 0x0400416D RID: 16749
		private static WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>> find_eggs_job = new WorkItemCollection<EggProtectionMonitor.Instance.FindEggsTask, List<KPrefabID>>();

		// Token: 0x020011AA RID: 4522
		private struct Egg
		{
			// Token: 0x0400416E RID: 16750
			public GameObject game_object;

			// Token: 0x0400416F RID: 16751
			public int cell;
		}

		// Token: 0x020011AB RID: 4523
		private struct FindEggsTask : IWorkItem<List<KPrefabID>>
		{
			// Token: 0x06005BF0 RID: 23536 RVA: 0x000E0536 File Offset: 0x000DE736
			public FindEggsTask(int start, int end)
			{
				this.start = start;
				this.end = end;
				this.eggs = ListPool<int, EggProtectionMonitor>.Allocate();
			}

			// Token: 0x06005BF1 RID: 23537 RVA: 0x002A784C File Offset: 0x002A5A4C
			public void Run(List<KPrefabID> prefab_ids, int threadIndex)
			{
				for (int num = this.start; num != this.end; num++)
				{
					if (EggProtectionMonitor.Instance.FindEggsTask.EGG_TAG.Contains(prefab_ids[num].PrefabTag))
					{
						this.eggs.Add(num);
					}
				}
			}

			// Token: 0x06005BF2 RID: 23538 RVA: 0x002A7894 File Offset: 0x002A5A94
			public void Finish(List<KPrefabID> prefab_ids, List<EggProtectionMonitor.Instance.Egg> eggs)
			{
				foreach (int index in this.eggs)
				{
					GameObject gameObject = prefab_ids[index].gameObject;
					eggs.Add(new EggProtectionMonitor.Instance.Egg
					{
						game_object = gameObject,
						cell = Grid.PosToCell(gameObject)
					});
				}
				this.eggs.Recycle();
			}

			// Token: 0x04004170 RID: 16752
			private static readonly List<Tag> EGG_TAG = new List<Tag>
			{
				"CrabEgg".ToTag(),
				"CrabWoodEgg".ToTag(),
				"CrabFreshWaterEgg".ToTag()
			};

			// Token: 0x04004171 RID: 16753
			private ListPool<int, EggProtectionMonitor>.PooledList eggs;

			// Token: 0x04004172 RID: 16754
			private int start;

			// Token: 0x04004173 RID: 16755
			private int end;
		}
	}
}
