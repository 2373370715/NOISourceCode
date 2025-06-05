using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000784 RID: 1924
public abstract class Chore
{
	// Token: 0x170000DB RID: 219
	// (get) Token: 0x06002194 RID: 8596
	// (set) Token: 0x06002195 RID: 8597
	public abstract int id { get; protected set; }

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06002196 RID: 8598
	// (set) Token: 0x06002197 RID: 8599
	public abstract int priorityMod { get; protected set; }

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x06002198 RID: 8600
	// (set) Token: 0x06002199 RID: 8601
	public abstract ChoreType choreType { get; protected set; }

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x0600219A RID: 8602
	// (set) Token: 0x0600219B RID: 8603
	public abstract ChoreDriver driver { get; protected set; }

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x0600219C RID: 8604
	// (set) Token: 0x0600219D RID: 8605
	public abstract ChoreDriver lastDriver { get; protected set; }

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x0600219E RID: 8606
	public abstract bool isNull { get; }

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x0600219F RID: 8607
	public abstract GameObject gameObject { get; }

	// Token: 0x060021A0 RID: 8608
	public abstract bool SatisfiesUrge(Urge urge);

	// Token: 0x060021A1 RID: 8609
	public abstract bool IsValid();

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060021A2 RID: 8610
	// (set) Token: 0x060021A3 RID: 8611
	public abstract IStateMachineTarget target { get; protected set; }

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x060021A4 RID: 8612
	// (set) Token: 0x060021A5 RID: 8613
	public abstract bool isComplete { get; protected set; }

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x060021A6 RID: 8614
	// (set) Token: 0x060021A7 RID: 8615
	public abstract bool IsPreemptable { get; protected set; }

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x060021A8 RID: 8616
	// (set) Token: 0x060021A9 RID: 8617
	public abstract ChoreConsumer overrideTarget { get; protected set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x060021AA RID: 8618
	// (set) Token: 0x060021AB RID: 8619
	public abstract Prioritizable prioritizable { get; protected set; }

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x060021AC RID: 8620
	// (set) Token: 0x060021AD RID: 8621
	public abstract ChoreProvider provider { get; set; }

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x060021AE RID: 8622
	// (set) Token: 0x060021AF RID: 8623
	public abstract bool runUntilComplete { get; set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060021B0 RID: 8624
	// (set) Token: 0x060021B1 RID: 8625
	public abstract bool isExpanded { get; protected set; }

	// Token: 0x060021B2 RID: 8626
	public abstract List<Chore.PreconditionInstance> GetPreconditions();

	// Token: 0x060021B3 RID: 8627
	public abstract bool CanPreempt(Chore.Precondition.Context context);

	// Token: 0x060021B4 RID: 8628
	public abstract void PrepareChore(ref Chore.Precondition.Context context);

	// Token: 0x060021B5 RID: 8629
	public abstract void Cancel(string reason);

	// Token: 0x060021B6 RID: 8630
	public abstract ReportManager.ReportType GetReportType();

	// Token: 0x060021B7 RID: 8631
	public abstract string GetReportName(string context = null);

	// Token: 0x060021B8 RID: 8632
	public abstract void AddPrecondition(Chore.Precondition precondition, object data = null);

	// Token: 0x060021B9 RID: 8633
	public abstract void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> incomplete_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override);

	// Token: 0x060021BA RID: 8634 RVA: 0x000BA66B File Offset: 0x000B886B
	public void CollectChores(ChoreConsumerState consumer_state, List<Chore.Precondition.Context> succeeded_contexts, List<Chore.Precondition.Context> failed_contexts, bool is_attempting_override)
	{
		this.CollectChores(consumer_state, succeeded_contexts, null, failed_contexts, is_attempting_override);
	}

	// Token: 0x060021BB RID: 8635
	public abstract void Cleanup();

	// Token: 0x060021BC RID: 8636
	public abstract void Fail(string reason);

	// Token: 0x060021BD RID: 8637
	public abstract void Reserve(ChoreDriver reserver);

	// Token: 0x060021BE RID: 8638
	public abstract void Begin(Chore.Precondition.Context context);

	// Token: 0x060021BF RID: 8639
	public abstract bool InProgress();

	// Token: 0x060021C0 RID: 8640 RVA: 0x000B64D6 File Offset: 0x000B46D6
	public virtual string ResolveString(string str)
	{
		return str;
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x000BA679 File Offset: 0x000B8879
	public static int GetNextChoreID()
	{
		return ++Chore.nextId;
	}

	// Token: 0x0400169D RID: 5789
	public PrioritySetting masterPriority;

	// Token: 0x0400169E RID: 5790
	public bool showAvailabilityInHoverText = true;

	// Token: 0x0400169F RID: 5791
	public Action<Chore> onExit;

	// Token: 0x040016A0 RID: 5792
	public Action<Chore> onComplete;

	// Token: 0x040016A1 RID: 5793
	private static int nextId;

	// Token: 0x040016A2 RID: 5794
	public const int MAX_PLAYER_BASIC_PRIORITY = 9;

	// Token: 0x040016A3 RID: 5795
	public const int MIN_PLAYER_BASIC_PRIORITY = 1;

	// Token: 0x040016A4 RID: 5796
	public const int MAX_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x040016A5 RID: 5797
	public const int MIN_PLAYER_HIGH_PRIORITY = 0;

	// Token: 0x040016A6 RID: 5798
	public const int MAX_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x040016A7 RID: 5799
	public const int MIN_PLAYER_EMERGENCY_PRIORITY = 1;

	// Token: 0x040016A8 RID: 5800
	public const int DEFAULT_BASIC_PRIORITY = 5;

	// Token: 0x040016A9 RID: 5801
	public const int MAX_BASIC_PRIORITY = 10;

	// Token: 0x040016AA RID: 5802
	public const int MIN_BASIC_PRIORITY = 0;

	// Token: 0x040016AB RID: 5803
	public static bool ENABLE_PERSONAL_PRIORITIES = true;

	// Token: 0x040016AC RID: 5804
	public static PrioritySetting DefaultPrioritySetting = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);

	// Token: 0x02000785 RID: 1925
	// (Invoke) Token: 0x060021C5 RID: 8645
	public delegate bool PreconditionFn(ref Chore.Precondition.Context context, object data);

	// Token: 0x02000786 RID: 1926
	public struct PreconditionInstance
	{
		// Token: 0x040016AD RID: 5805
		public Chore.Precondition condition;

		// Token: 0x040016AE RID: 5806
		public object data;
	}

	// Token: 0x02000787 RID: 1927
	public struct Precondition
	{
		// Token: 0x040016AF RID: 5807
		public string id;

		// Token: 0x040016B0 RID: 5808
		public string description;

		// Token: 0x040016B1 RID: 5809
		public int sortOrder;

		// Token: 0x040016B2 RID: 5810
		public Chore.PreconditionFn fn;

		// Token: 0x040016B3 RID: 5811
		public bool canExecuteOnAnyThread;

		// Token: 0x02000788 RID: 1928
		[DebuggerDisplay("{chore.GetType()}, {chore.gameObject.name}")]
		public struct Context : IComparable<Chore.Precondition.Context>, IEquatable<Chore.Precondition.Context>
		{
			// Token: 0x060021C8 RID: 8648 RVA: 0x001CD9A4 File Offset: 0x001CBBA4
			public Context(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.personalPriority = consumer_state.consumer.GetPersonalPriority(chore.choreType);
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.skipMoreSatisfyingEarlyPrecondition = (RootMenu.Instance != null && RootMenu.Instance.IsBuildingChorePanelActive());
				this.SetPriority(chore);
			}

			// Token: 0x060021C9 RID: 8649 RVA: 0x001CDA5C File Offset: 0x001CBC5C
			public void Set(Chore chore, ChoreConsumerState consumer_state, bool is_attempting_override, object data = null)
			{
				this.masterPriority = chore.masterPriority;
				this.priority = 0;
				this.priorityMod = chore.priorityMod;
				this.consumerPriority = 0;
				this.interruptPriority = 0;
				this.cost = 0;
				this.chore = chore;
				this.consumerState = consumer_state;
				this.failedPreconditionId = -1;
				this.skippedPreconditions = false;
				this.isAttemptingOverride = is_attempting_override;
				this.data = data;
				this.choreTypeForPermission = chore.choreType;
				this.SetPriority(chore);
			}

			// Token: 0x060021CA RID: 8650 RVA: 0x001CDADC File Offset: 0x001CBCDC
			public void SetPriority(Chore chore)
			{
				this.priority = (Game.Instance.advancedPersonalPriorities ? chore.choreType.explicitPriority : chore.choreType.priority);
				this.priorityMod = chore.priorityMod;
				this.interruptPriority = chore.choreType.interruptPriority;
			}

			// Token: 0x060021CB RID: 8651 RVA: 0x000BA6AB File Offset: 0x000B88AB
			public bool IsSuccess()
			{
				return this.failedPreconditionId == -1 && !this.skippedPreconditions;
			}

			// Token: 0x060021CC RID: 8652 RVA: 0x000BA6C1 File Offset: 0x000B88C1
			public bool IsComplete()
			{
				return !this.skippedPreconditions;
			}

			// Token: 0x060021CD RID: 8653 RVA: 0x001CDB30 File Offset: 0x001CBD30
			public bool IsPotentialSuccess()
			{
				if (this.IsSuccess())
				{
					return true;
				}
				if (this.chore.driver == this.consumerState.choreDriver)
				{
					return true;
				}
				if (this.failedPreconditionId != -1)
				{
					if (this.failedPreconditionId >= 0 && this.failedPreconditionId < this.chore.GetPreconditions().Count)
					{
						return this.chore.GetPreconditions()[this.failedPreconditionId].condition.id == ChorePreconditions.instance.IsMoreSatisfyingLate.id;
					}
					DebugUtil.DevLogErrorFormat("failedPreconditionId out of range {0}/{1}", new object[]
					{
						this.failedPreconditionId,
						this.chore.GetPreconditions().Count
					});
				}
				return false;
			}

			// Token: 0x060021CE RID: 8654 RVA: 0x001CDC00 File Offset: 0x001CBE00
			private void DoPreconditions(bool mainThreadOnly)
			{
				bool flag = Game.IsOnMainThread();
				List<Chore.PreconditionInstance> preconditions = this.chore.GetPreconditions();
				this.skippedPreconditions = false;
				int i = 0;
				while (i < preconditions.Count)
				{
					Chore.PreconditionInstance preconditionInstance = preconditions[i];
					if (preconditionInstance.condition.canExecuteOnAnyThread)
					{
						if (!mainThreadOnly)
						{
							goto IL_43;
						}
					}
					else
					{
						if (flag)
						{
							goto IL_43;
						}
						this.skippedPreconditions = true;
					}
					IL_6B:
					i++;
					continue;
					IL_43:
					if (!preconditionInstance.condition.fn(ref this, preconditionInstance.data))
					{
						this.failedPreconditionId = i;
						this.skippedPreconditions = false;
						return;
					}
					goto IL_6B;
				}
			}

			// Token: 0x060021CF RID: 8655 RVA: 0x000BA6CC File Offset: 0x000B88CC
			public void RunPreconditions()
			{
				this.DoPreconditions(false);
			}

			// Token: 0x060021D0 RID: 8656 RVA: 0x000BA6D5 File Offset: 0x000B88D5
			public void FinishPreconditions()
			{
				this.DoPreconditions(true);
			}

			// Token: 0x060021D1 RID: 8657 RVA: 0x001CDC88 File Offset: 0x001CBE88
			public int CompareTo(Chore.Precondition.Context obj)
			{
				bool flag = this.failedPreconditionId != -1;
				bool flag2 = obj.failedPreconditionId != -1;
				if (flag == flag2)
				{
					int num = this.masterPriority.priority_class - obj.masterPriority.priority_class;
					if (num != 0)
					{
						return num;
					}
					int num2 = this.personalPriority - obj.personalPriority;
					if (num2 != 0)
					{
						return num2;
					}
					int num3 = this.masterPriority.priority_value - obj.masterPriority.priority_value;
					if (num3 != 0)
					{
						return num3;
					}
					int num4 = this.priority - obj.priority;
					if (num4 != 0)
					{
						return num4;
					}
					int num5 = this.priorityMod - obj.priorityMod;
					if (num5 != 0)
					{
						return num5;
					}
					int num6 = this.consumerPriority - obj.consumerPriority;
					if (num6 != 0)
					{
						return num6;
					}
					int num7 = obj.cost - this.cost;
					if (num7 != 0)
					{
						return num7;
					}
					if (this.chore == null && obj.chore == null)
					{
						return 0;
					}
					if (this.chore == null)
					{
						return -1;
					}
					if (obj.chore == null)
					{
						return 1;
					}
					return this.chore.id - obj.chore.id;
				}
				else
				{
					if (!flag)
					{
						return 1;
					}
					return -1;
				}
			}

			// Token: 0x060021D2 RID: 8658 RVA: 0x001CDDA4 File Offset: 0x001CBFA4
			public override bool Equals(object obj)
			{
				Chore.Precondition.Context obj2 = (Chore.Precondition.Context)obj;
				return this.CompareTo(obj2) == 0;
			}

			// Token: 0x060021D3 RID: 8659 RVA: 0x000BA6DE File Offset: 0x000B88DE
			public bool Equals(Chore.Precondition.Context other)
			{
				return this.CompareTo(other) == 0;
			}

			// Token: 0x060021D4 RID: 8660 RVA: 0x000BA6EA File Offset: 0x000B88EA
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060021D5 RID: 8661 RVA: 0x000BA6FC File Offset: 0x000B88FC
			public static bool operator ==(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) == 0;
			}

			// Token: 0x060021D6 RID: 8662 RVA: 0x000BA709 File Offset: 0x000B8909
			public static bool operator !=(Chore.Precondition.Context x, Chore.Precondition.Context y)
			{
				return x.CompareTo(y) != 0;
			}

			// Token: 0x060021D7 RID: 8663 RVA: 0x000BA716 File Offset: 0x000B8916
			public static bool ShouldFilter(string filter, string text)
			{
				return !string.IsNullOrEmpty(filter) && (string.IsNullOrEmpty(text) || text.ToLower().IndexOf(filter) < 0);
			}

			// Token: 0x040016B4 RID: 5812
			public PrioritySetting masterPriority;

			// Token: 0x040016B5 RID: 5813
			public int personalPriority;

			// Token: 0x040016B6 RID: 5814
			public int priority;

			// Token: 0x040016B7 RID: 5815
			public int priorityMod;

			// Token: 0x040016B8 RID: 5816
			public int interruptPriority;

			// Token: 0x040016B9 RID: 5817
			public int cost;

			// Token: 0x040016BA RID: 5818
			public int consumerPriority;

			// Token: 0x040016BB RID: 5819
			public Chore chore;

			// Token: 0x040016BC RID: 5820
			public ChoreConsumerState consumerState;

			// Token: 0x040016BD RID: 5821
			public int failedPreconditionId;

			// Token: 0x040016BE RID: 5822
			public bool skippedPreconditions;

			// Token: 0x040016BF RID: 5823
			public object data;

			// Token: 0x040016C0 RID: 5824
			public bool isAttemptingOverride;

			// Token: 0x040016C1 RID: 5825
			public ChoreType choreTypeForPermission;

			// Token: 0x040016C2 RID: 5826
			public bool skipMoreSatisfyingEarlyPrecondition;
		}
	}
}
