using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200165B RID: 5723
public class ThreatMonitor : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>
{
	// Token: 0x0600764F RID: 30287 RVA: 0x00317DF0 File Offset: 0x00315FF0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.safe;
		this.root.EventHandler(GameHashes.SafeFromThreats, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnSafe(d);
		}).EventHandler(GameHashes.Attacked, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.OnAttacked(d);
		}).EventHandler(GameHashes.ObjectDestroyed, delegate(ThreatMonitor.Instance smi, object d)
		{
			smi.Cleanup(d);
		});
		this.safe.Enter(delegate(ThreatMonitor.Instance smi)
		{
			smi.revengeThreat.Clear();
		}).Enter(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats)).EventHandler(GameHashes.FactionChanged, new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State.Callback(ThreatMonitor.SeekThreats));
		this.safe.passive.DoNothing();
		this.safe.seeking.PreBrainUpdate(delegate(ThreatMonitor.Instance smi)
		{
			smi.RefreshThreat(null);
		});
		this.threatened.duplicant.Transition(this.safe, GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Not(new StateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.Transition.ConditionCallback(ThreatMonitor.DupeHasValidTarget)), UpdateRate.SIM_200ms);
		this.threatened.duplicant.ShouldFight.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateAttackChore), this.safe).Update("DupeUpdateTarget", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.DupeUpdateTarget), UpdateRate.SIM_200ms, false);
		this.threatened.duplicant.ShoudFlee.ToggleChore(new Func<ThreatMonitor.Instance, Chore>(this.CreateFleeChore), this.safe);
		this.threatened.creature.ToggleBehaviour(GameTags.Creatures.Flee, (ThreatMonitor.Instance smi) => !smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).ToggleBehaviour(GameTags.Creatures.Attack, (ThreatMonitor.Instance smi) => smi.WillFight(), delegate(ThreatMonitor.Instance smi)
		{
			smi.GoTo(this.safe);
		}).Update("CritterCalmUpdate", new Action<ThreatMonitor.Instance, float>(ThreatMonitor.CritterCalmUpdate), UpdateRate.SIM_200ms, false).PreBrainUpdate(new Action<ThreatMonitor.Instance>(ThreatMonitor.CritterUpdateThreats));
	}

	// Token: 0x06007650 RID: 30288 RVA: 0x0031804C File Offset: 0x0031624C
	private static void SeekThreats(ThreatMonitor.Instance smi)
	{
		Faction faction = FactionManager.Instance.GetFaction(smi.alignment.Alignment);
		if (smi.IAmADuplicant || faction.CanAttack)
		{
			smi.GoTo(smi.sm.safe.seeking);
			return;
		}
		smi.GoTo(smi.sm.safe.passive);
	}

	// Token: 0x06007651 RID: 30289 RVA: 0x003180AC File Offset: 0x003162AC
	private static bool DupeHasValidTarget(ThreatMonitor.Instance smi)
	{
		bool result = false;
		if (smi.MainThreat != null && smi.MainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
		{
			IApproachable component = smi.MainThreat.GetComponent<RangedAttackable>();
			if (component != null)
			{
				result = (smi.navigator.GetNavigationCost(component) != -1);
			}
		}
		return result;
	}

	// Token: 0x06007652 RID: 30290 RVA: 0x000F246E File Offset: 0x000F066E
	private static void DupeUpdateTarget(ThreatMonitor.Instance smi, float dt)
	{
		if (!ThreatMonitor.DupeHasValidTarget(smi))
		{
			smi.Trigger(2144432245, null);
		}
	}

	// Token: 0x06007653 RID: 30291 RVA: 0x000F2484 File Offset: 0x000F0684
	private static void CritterCalmUpdate(ThreatMonitor.Instance smi, float dt)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (smi.revengeThreat.target != null && smi.revengeThreat.Calm(dt, smi.alignment))
		{
			smi.Trigger(-21431934, null);
		}
	}

	// Token: 0x06007654 RID: 30292 RVA: 0x000F24C2 File Offset: 0x000F06C2
	private static void CritterUpdateThreats(ThreatMonitor.Instance smi)
	{
		if (smi.isMasterNull)
		{
			return;
		}
		if (!smi.CheckForThreats() && !ThreatMonitor.IsInSafeState(smi))
		{
			smi.GoTo(smi.sm.safe);
		}
	}

	// Token: 0x06007655 RID: 30293 RVA: 0x000F24EE File Offset: 0x000F06EE
	private static bool IsInSafeState(ThreatMonitor.Instance smi)
	{
		return smi.GetCurrentState() == smi.sm.safe.passive || smi.GetCurrentState() == smi.sm.safe.seeking;
	}

	// Token: 0x06007656 RID: 30294 RVA: 0x000F2522 File Offset: 0x000F0722
	private Chore CreateAttackChore(ThreatMonitor.Instance smi)
	{
		return new AttackChore(smi.master, smi.MainThreat);
	}

	// Token: 0x06007657 RID: 30295 RVA: 0x000F2535 File Offset: 0x000F0735
	private Chore CreateFleeChore(ThreatMonitor.Instance smi)
	{
		return new FleeChore(smi.master, smi.MainThreat);
	}

	// Token: 0x040058FE RID: 22782
	public ThreatMonitor.SafeStates safe;

	// Token: 0x040058FF RID: 22783
	public ThreatMonitor.ThreatenedStates threatened;

	// Token: 0x0200165C RID: 5724
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005900 RID: 22784
		public Health.HealthState fleethresholdState = Health.HealthState.Injured;

		// Token: 0x04005901 RID: 22785
		public Tag[] friendlyCreatureTags;

		// Token: 0x04005902 RID: 22786
		public int maxSearchEntities = 50;

		// Token: 0x04005903 RID: 22787
		public int maxSearchDistance = 20;

		// Token: 0x04005904 RID: 22788
		public CellOffset[] offsets = OffsetGroups.Use;
	}

	// Token: 0x0200165D RID: 5725
	public class SafeStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04005905 RID: 22789
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State passive;

		// Token: 0x04005906 RID: 22790
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State seeking;
	}

	// Token: 0x0200165E RID: 5726
	public class ThreatenedStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04005907 RID: 22791
		public ThreatMonitor.ThreatenedDuplicantStates duplicant;

		// Token: 0x04005908 RID: 22792
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State creature;
	}

	// Token: 0x0200165F RID: 5727
	public class ThreatenedDuplicantStates : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State
	{
		// Token: 0x04005909 RID: 22793
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShoudFlee;

		// Token: 0x0400590A RID: 22794
		public GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.State ShouldFight;
	}

	// Token: 0x02001660 RID: 5728
	public struct Grudge
	{
		// Token: 0x0600765F RID: 30303 RVA: 0x00318100 File Offset: 0x00316300
		public void Reset(FactionAlignment revengeTarget)
		{
			this.target = revengeTarget;
			float num = 10f;
			this.grudgeTime = num;
		}

		// Token: 0x06007660 RID: 30304 RVA: 0x00318124 File Offset: 0x00316324
		public bool Calm(float dt, FactionAlignment self)
		{
			if (this.grudgeTime <= 0f)
			{
				return true;
			}
			this.grudgeTime = Mathf.Max(0f, this.grudgeTime - dt);
			if (this.grudgeTime == 0f)
			{
				if (FactionManager.Instance.GetDisposition(self.Alignment, this.target.Alignment) != FactionManager.Disposition.Attack)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.GAMEOBJECTEFFECTS.FORGAVEATTACKER, self.transform, 2f, true);
				}
				this.Clear();
				return true;
			}
			return false;
		}

		// Token: 0x06007661 RID: 30305 RVA: 0x000F2590 File Offset: 0x000F0790
		public void Clear()
		{
			this.grudgeTime = 0f;
			this.target = null;
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x003181B8 File Offset: 0x003163B8
		public bool IsValidRevengeTarget(bool isDuplicant)
		{
			return this.target != null && this.target.IsAlignmentActive() && (this.target.health == null || !this.target.health.IsDefeated()) && (!isDuplicant || !this.target.IsPlayerTargeted());
		}

		// Token: 0x0400590B RID: 22795
		public FactionAlignment target;

		// Token: 0x0400590C RID: 22796
		public float grudgeTime;
	}

	// Token: 0x02001661 RID: 5729
	public new class Instance : GameStateMachine<ThreatMonitor, ThreatMonitor.Instance, IStateMachineTarget, ThreatMonitor.Def>.GameInstance
	{
		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06007663 RID: 30307 RVA: 0x000F25A4 File Offset: 0x000F07A4
		public GameObject MainThreat
		{
			get
			{
				return this.mainThreat;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06007664 RID: 30308 RVA: 0x000F25AC File Offset: 0x000F07AC
		public bool IAmADuplicant
		{
			get
			{
				return this.alignment.Alignment == FactionManager.FactionID.Duplicant;
			}
		}

		// Token: 0x06007665 RID: 30309 RVA: 0x0031821C File Offset: 0x0031641C
		public Instance(IStateMachineTarget master, ThreatMonitor.Def def) : base(master, def)
		{
			this.alignment = master.GetComponent<FactionAlignment>();
			this.navigator = master.GetComponent<Navigator>();
			this.choreDriver = master.GetComponent<ChoreDriver>();
			this.health = master.GetComponent<Health>();
			this.choreConsumer = master.GetComponent<ChoreConsumer>();
			this.refreshThreatDelegate = new Action<object>(this.RefreshThreat);
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x000F25BC File Offset: 0x000F07BC
		public void ClearMainThreat()
		{
			this.SetMainThreat(null);
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x0031828C File Offset: 0x0031648C
		public void SetMainThreat(GameObject threat)
		{
			if (threat == this.mainThreat)
			{
				return;
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
				if (threat == null)
				{
					base.Trigger(2144432245, null);
				}
			}
			if (this.mainThreat != null)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
			this.mainThreat = threat;
			if (this.mainThreat != null)
			{
				this.mainThreat.Subscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Subscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x000F25C5 File Offset: 0x000F07C5
		public bool HasThreat()
		{
			return this.MainThreat != null;
		}

		// Token: 0x06007669 RID: 30313 RVA: 0x000F25D3 File Offset: 0x000F07D3
		public void OnSafe(object data)
		{
			if (this.revengeThreat.target != null)
			{
				if (!this.revengeThreat.target.GetComponent<FactionAlignment>().IsAlignmentActive())
				{
					this.revengeThreat.Clear();
				}
				this.ClearMainThreat();
			}
		}

		// Token: 0x0600766A RID: 30314 RVA: 0x00318374 File Offset: 0x00316574
		public void OnAttacked(object data)
		{
			FactionAlignment factionAlignment = (FactionAlignment)data;
			this.revengeThreat.Reset(factionAlignment);
			if (this.mainThreat == null)
			{
				this.SetMainThreat(factionAlignment.gameObject);
				this.GoToThreatened();
			}
			else if (!this.WillFight())
			{
				this.GoToThreatened();
			}
			if (factionAlignment.GetComponent<Bee>())
			{
				Chore chore = (this.choreDriver != null) ? this.choreDriver.GetCurrentChore() : null;
				if (chore != null && chore.gameObject.GetComponent<HiveWorkableEmpty>() != null)
				{
					chore.gameObject.GetComponent<HiveWorkableEmpty>().wasStung = true;
				}
			}
		}

		// Token: 0x0600766B RID: 30315 RVA: 0x00318418 File Offset: 0x00316618
		public bool WillFight()
		{
			if (this.choreConsumer != null)
			{
				if (!this.choreConsumer.IsPermittedByUser(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
				if (!this.choreConsumer.IsPermittedByTraits(Db.Get().ChoreGroups.Combat))
				{
					return false;
				}
			}
			return this.health.State < base.smi.def.fleethresholdState;
		}

		// Token: 0x0600766C RID: 30316 RVA: 0x00318494 File Offset: 0x00316694
		private void GotoThreatResponse()
		{
			Chore currentChore = base.smi.master.GetComponent<ChoreDriver>().GetCurrentChore();
			if (this.WillFight() && this.mainThreat.GetComponent<FactionAlignment>().IsPlayerTargeted())
			{
				base.smi.GoTo(base.smi.sm.threatened.duplicant.ShouldFight);
				return;
			}
			if (currentChore != null && currentChore.target != null && currentChore.target != base.master && currentChore.target.GetComponent<Pickupable>() != null)
			{
				return;
			}
			base.smi.GoTo(base.smi.sm.threatened.duplicant.ShoudFlee);
		}

		// Token: 0x0600766D RID: 30317 RVA: 0x000F2610 File Offset: 0x000F0810
		public void GoToThreatened()
		{
			if (this.IAmADuplicant)
			{
				this.GotoThreatResponse();
				return;
			}
			base.smi.GoTo(base.sm.threatened.creature);
		}

		// Token: 0x0600766E RID: 30318 RVA: 0x000F263C File Offset: 0x000F083C
		public void Cleanup(object data)
		{
			if (this.mainThreat)
			{
				this.mainThreat.Unsubscribe(1623392196, this.refreshThreatDelegate);
				this.mainThreat.Unsubscribe(1969584890, this.refreshThreatDelegate);
			}
		}

		// Token: 0x0600766F RID: 30319 RVA: 0x0031854C File Offset: 0x0031674C
		public void RefreshThreat(object data)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (base.smi.CheckForThreats())
			{
				this.GoToThreatened();
				return;
			}
			if (!ThreatMonitor.IsInSafeState(base.smi))
			{
				base.Trigger(-21431934, null);
				base.smi.GoTo(base.sm.safe);
			}
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x003185A8 File Offset: 0x003167A8
		public bool CheckForThreats()
		{
			if (base.isMasterNull)
			{
				return false;
			}
			GameObject x;
			if (this.revengeThreat.IsValidRevengeTarget(this.IAmADuplicant))
			{
				x = this.revengeThreat.target.gameObject;
			}
			else if (this.IAmADuplicant)
			{
				x = this.FindThreatDuplicant();
			}
			else
			{
				x = this.FindThreatOther();
			}
			this.SetMainThreat(x);
			return x != null;
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x0031860C File Offset: 0x0031680C
		private GameObject FindThreatDuplicant()
		{
			this.threats.Clear();
			if (this.WillFight())
			{
				foreach (object obj in Components.PlayerTargeted)
				{
					FactionAlignment factionAlignment = (FactionAlignment)obj;
					if (!factionAlignment.IsNullOrDestroyed() && factionAlignment.IsPlayerTargeted() && !factionAlignment.health.IsDefeated() && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
					{
						this.threats.Add(factionAlignment);
					}
				}
			}
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x000F2677 File Offset: 0x000F0877
		private GameObject FindThreatOther()
		{
			this.threats.Clear();
			this.GatherThreats();
			return this.PickBestTarget(this.threats);
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x003186D0 File Offset: 0x003168D0
		private void GatherThreats()
		{
			ListPool<ScenePartitionerEntry, ThreatMonitor>.PooledList pooledList = ListPool<ScenePartitionerEntry, ThreatMonitor>.Allocate();
			Extents extents = new Extents(Grid.PosToCell(base.gameObject), base.def.maxSearchDistance);
			GameScenePartitioner.Instance.GatherEntries(extents, GameScenePartitioner.Instance.attackableEntitiesLayer, pooledList);
			int count = pooledList.Count;
			int num = Mathf.Min(count, base.def.maxSearchEntities);
			for (int i = 0; i < num; i++)
			{
				if (this.currentUpdateIndex >= count)
				{
					this.currentUpdateIndex = 0;
				}
				ScenePartitionerEntry scenePartitionerEntry = pooledList[this.currentUpdateIndex];
				this.currentUpdateIndex++;
				FactionAlignment factionAlignment = scenePartitionerEntry.obj as FactionAlignment;
				if (!(factionAlignment.transform == null) && !(factionAlignment == this.alignment) && (base.def.friendlyCreatureTags == null || !factionAlignment.kprefabID.HasAnyTags(base.def.friendlyCreatureTags)) && factionAlignment.IsAlignmentActive() && FactionManager.Instance.GetDisposition(this.alignment.Alignment, factionAlignment.Alignment) == FactionManager.Disposition.Attack && this.navigator.CanReach(factionAlignment.attackable.GetCell(), base.smi.def.offsets))
				{
					this.threats.Add(factionAlignment);
				}
			}
			pooledList.Recycle();
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x0031882C File Offset: 0x00316A2C
		public GameObject PickBestTarget(List<FactionAlignment> threats)
		{
			float num = 1f;
			Vector2 a = base.gameObject.transform.GetPosition();
			GameObject result = null;
			float num2 = float.PositiveInfinity;
			for (int i = threats.Count - 1; i >= 0; i--)
			{
				FactionAlignment factionAlignment = threats[i];
				float num3 = Vector2.Distance(a, factionAlignment.transform.GetPosition()) / num;
				if (num3 < num2)
				{
					num2 = num3;
					result = factionAlignment.gameObject;
				}
			}
			return result;
		}

		// Token: 0x0400590D RID: 22797
		public FactionAlignment alignment;

		// Token: 0x0400590E RID: 22798
		public Navigator navigator;

		// Token: 0x0400590F RID: 22799
		public ChoreDriver choreDriver;

		// Token: 0x04005910 RID: 22800
		private Health health;

		// Token: 0x04005911 RID: 22801
		private ChoreConsumer choreConsumer;

		// Token: 0x04005912 RID: 22802
		public ThreatMonitor.Grudge revengeThreat;

		// Token: 0x04005913 RID: 22803
		public int currentUpdateIndex;

		// Token: 0x04005914 RID: 22804
		private GameObject mainThreat;

		// Token: 0x04005915 RID: 22805
		private List<FactionAlignment> threats = new List<FactionAlignment>();

		// Token: 0x04005916 RID: 22806
		private Action<object> refreshThreatDelegate;
	}
}
