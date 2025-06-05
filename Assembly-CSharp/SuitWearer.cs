using System;
using System.Collections.Generic;

// Token: 0x02001652 RID: 5714
public class SuitWearer : GameStateMachine<SuitWearer, SuitWearer.Instance>
{
	// Token: 0x06007624 RID: 30244 RVA: 0x00317594 File Offset: 0x00315794
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, delegate(SuitWearer.Instance smi, object data)
		{
			smi.OnPathAdvanced(data);
		}).EventHandler(GameHashes.Died, delegate(SuitWearer.Instance smi, object data)
		{
			smi.UnreserveSuits();
		}).DoNothing();
		this.suit.DoNothing();
		this.nosuit.DoNothing();
	}

	// Token: 0x040058D7 RID: 22743
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State suit;

	// Token: 0x040058D8 RID: 22744
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State nosuit;

	// Token: 0x02001653 RID: 5715
	public new class Instance : GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007626 RID: 30246 RVA: 0x00317620 File Offset: 0x00315820
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.navigator.SetFlags(PathFinder.PotentialPath.Flags.PerformSuitChecks);
			this.prefabInstanceID = this.navigator.GetComponent<KPrefabID>().InstanceID;
		}

		// Token: 0x06007627 RID: 30247 RVA: 0x000F22ED File Offset: 0x000F04ED
		public void OnPathAdvanced(object data)
		{
			if (this.navigator.CurrentNavType == NavType.Hover && (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) <= PathFinder.PotentialPath.Flags.None)
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
			}
			this.UnreserveSuits();
			this.ReserveSuits();
		}

		// Token: 0x06007628 RID: 30248 RVA: 0x00317678 File Offset: 0x00315878
		public void ReserveSuits()
		{
			PathFinder.Path path = this.navigator.path;
			if (path.nodes == null)
			{
				return;
			}
			bool flag = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
			bool flag2 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
			bool flag3 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
			bool flag4 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				int cell = path.nodes[i].cell;
				Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
				PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
				if (Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2))
				{
					bool flag5 = (flags2 & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag6 = (flags2 & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
					bool flag7 = (flags2 & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
					bool flag8 = (flags2 & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag9 = flag2 || flag || flag3 || flag4;
					bool flag10 = flag5 == flag && flag6 == flag2 && flag7 == flag3 && flag8 == flag4;
					bool flag11 = SuitMarker.DoesTraversalDirectionRequireSuit(cell, path.nodes[i + 1].cell, flags);
					if (flag11 && !flag9)
					{
						if (Grid.ReserveSuit(cell, this.prefabInstanceID, true))
						{
							this.suitReservations.Add(cell);
							if (flag5)
							{
								flag = true;
							}
							if (flag6)
							{
								flag2 = true;
							}
							if (flag7)
							{
								flag3 = true;
							}
							if (flag8)
							{
								flag4 = true;
							}
						}
					}
					else if (!flag11 && flag10 && Grid.HasEmptyLocker(cell, this.prefabInstanceID) && Grid.ReserveEmptyLocker(cell, this.prefabInstanceID, true))
					{
						this.emptyLockerReservations.Add(cell);
						if (flag5)
						{
							flag = false;
						}
						if (flag6)
						{
							flag2 = false;
						}
						if (flag7)
						{
							flag3 = false;
						}
						if (flag8)
						{
							flag4 = false;
						}
					}
				}
			}
		}

		// Token: 0x06007629 RID: 30249 RVA: 0x00317824 File Offset: 0x00315A24
		public void UnreserveSuits()
		{
			foreach (int num in this.suitReservations)
			{
				if (Grid.HasSuitMarker[num])
				{
					Grid.ReserveSuit(num, this.prefabInstanceID, false);
				}
			}
			this.suitReservations.Clear();
			foreach (int num2 in this.emptyLockerReservations)
			{
				if (Grid.HasSuitMarker[num2])
				{
					Grid.ReserveEmptyLocker(num2, this.prefabInstanceID, false);
				}
			}
			this.emptyLockerReservations.Clear();
		}

		// Token: 0x0600762A RID: 30250 RVA: 0x000F2327 File Offset: 0x000F0527
		protected override void OnCleanUp()
		{
			this.UnreserveSuits();
		}

		// Token: 0x040058D9 RID: 22745
		private List<int> suitReservations = new List<int>();

		// Token: 0x040058DA RID: 22746
		private List<int> emptyLockerReservations = new List<int>();

		// Token: 0x040058DB RID: 22747
		private Navigator navigator;

		// Token: 0x040058DC RID: 22748
		private int prefabInstanceID;
	}
}
