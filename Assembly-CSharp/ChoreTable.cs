using System;
using System.Collections.Generic;

// Token: 0x0200079A RID: 1946
public class ChoreTable
{
	// Token: 0x06002294 RID: 8852 RVA: 0x000BAF7C File Offset: 0x000B917C
	public ChoreTable(ChoreTable.Entry[] entries)
	{
		this.entries = entries;
	}

	// Token: 0x06002295 RID: 8853 RVA: 0x001D00CC File Offset: 0x001CE2CC
	public ref ChoreTable.Entry GetEntry<T>()
	{
		ref ChoreTable.Entry result = ref ChoreTable.InvalidEntry;
		for (int i = 0; i < this.entries.Length; i++)
		{
			if (this.entries[i].stateMachineDef is T)
			{
				result = ref this.entries[i];
				break;
			}
		}
		return ref result;
	}

	// Token: 0x06002296 RID: 8854 RVA: 0x001D011C File Offset: 0x001CE31C
	public int GetChorePriority<StateMachineType>(ChoreConsumer chore_consumer)
	{
		for (int i = 0; i < this.entries.Length; i++)
		{
			ChoreTable.Entry entry = this.entries[i];
			if (entry.stateMachineDef.GetStateMachineType() == typeof(StateMachineType))
			{
				return entry.choreType.priority;
			}
		}
		Debug.LogError(chore_consumer.name + "'s chore table does not have an entry for: " + typeof(StateMachineType).Name);
		return -1;
	}

	// Token: 0x0400172E RID: 5934
	private ChoreTable.Entry[] entries;

	// Token: 0x0400172F RID: 5935
	public static ChoreTable.Entry InvalidEntry;

	// Token: 0x0200079B RID: 1947
	public class Builder
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x000BAF8B File Offset: 0x000B918B
		public ChoreTable.Builder PushInterruptGroup()
		{
			this.interruptGroupId++;
			return this;
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000BAF9C File Offset: 0x000B919C
		public ChoreTable.Builder PopInterruptGroup()
		{
			DebugUtil.Assert(this.interruptGroupId > 0);
			this.interruptGroupId--;
			return this;
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x001D0198 File Offset: 0x001CE398
		public ChoreTable.Builder Add(StateMachine.BaseDef def, bool condition = true, int forcePriority = -1)
		{
			if (condition)
			{
				ChoreTable.Builder.Info item = new ChoreTable.Builder.Info
				{
					interruptGroupId = this.interruptGroupId,
					forcePriority = forcePriority,
					def = def
				};
				this.infos.Add(item);
			}
			return this;
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x001D01DC File Offset: 0x001CE3DC
		public bool HasChoreType(Type choreType)
		{
			return this.infos.Exists((ChoreTable.Builder.Info info) => info.def.GetType() == choreType);
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x001D0210 File Offset: 0x001CE410
		public bool TryGetChoreDef<T>(out T def) where T : StateMachine.BaseDef
		{
			for (int i = 0; i < this.infos.Count; i++)
			{
				if (this.infos[i].def != null && typeof(T).IsAssignableFrom(this.infos[i].def.GetType()))
				{
					def = (T)((object)this.infos[i].def);
					return true;
				}
			}
			def = default(T);
			return false;
		}

		// Token: 0x0600229D RID: 8861 RVA: 0x001D0294 File Offset: 0x001CE494
		public ChoreTable CreateTable()
		{
			DebugUtil.Assert(this.interruptGroupId == 0);
			ChoreTable.Entry[] array = new ChoreTable.Entry[this.infos.Count];
			Stack<int> stack = new Stack<int>();
			int num = 10000;
			for (int i = 0; i < this.infos.Count; i++)
			{
				int num2 = (this.infos[i].forcePriority != -1) ? this.infos[i].forcePriority : (num - 100);
				num = num2;
				int num3 = 10000 - i * 100;
				int num4 = this.infos[i].interruptGroupId;
				if (num4 != 0)
				{
					if (stack.Count != num4)
					{
						stack.Push(num3);
					}
					else
					{
						num3 = stack.Peek();
					}
				}
				else if (stack.Count > 0)
				{
					stack.Pop();
				}
				array[i] = new ChoreTable.Entry(this.infos[i].def, num2, num3);
			}
			return new ChoreTable(array);
		}

		// Token: 0x04001730 RID: 5936
		private int interruptGroupId;

		// Token: 0x04001731 RID: 5937
		private List<ChoreTable.Builder.Info> infos = new List<ChoreTable.Builder.Info>();

		// Token: 0x04001732 RID: 5938
		private const int INVALID_PRIORITY = -1;

		// Token: 0x0200079C RID: 1948
		private struct Info
		{
			// Token: 0x04001733 RID: 5939
			public int interruptGroupId;

			// Token: 0x04001734 RID: 5940
			public int forcePriority;

			// Token: 0x04001735 RID: 5941
			public StateMachine.BaseDef def;
		}
	}

	// Token: 0x0200079E RID: 1950
	public class ChoreTableChore<StateMachineType, StateMachineInstanceType> : Chore<StateMachineInstanceType> where StateMachineInstanceType : StateMachine.Instance
	{
		// Token: 0x060022A1 RID: 8865 RVA: 0x001D0390 File Offset: 0x001CE590
		public ChoreTableChore(StateMachine.BaseDef state_machine_def, ChoreType chore_type, KPrefabID prefab_id) : base(chore_type, prefab_id, prefab_id.GetComponent<ChoreProvider>(), true, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
		{
			this.showAvailabilityInHoverText = false;
			base.smi = (state_machine_def.CreateSMI(this) as StateMachineInstanceType);
		}
	}

	// Token: 0x0200079F RID: 1951
	public struct Entry
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x001D03D8 File Offset: 0x001CE5D8
		public Entry(StateMachine.BaseDef state_machine_def, int priority, int interrupt_priority)
		{
			Type stateMachineInstanceType = Singleton<StateMachineManager>.Instance.CreateStateMachine(state_machine_def.GetStateMachineType()).GetStateMachineInstanceType();
			Type[] typeArguments = new Type[]
			{
				state_machine_def.GetStateMachineType(),
				stateMachineInstanceType
			};
			this.choreClassType = typeof(ChoreTable.ChoreTableChore<, >).MakeGenericType(typeArguments);
			this.choreType = new ChoreType(state_machine_def.ToString(), null, new string[0], "", "", "", "", new Tag[0], priority, priority);
			this.choreType.interruptPriority = interrupt_priority;
			this.stateMachineDef = state_machine_def;
		}

		// Token: 0x04001737 RID: 5943
		public Type choreClassType;

		// Token: 0x04001738 RID: 5944
		public ChoreType choreType;

		// Token: 0x04001739 RID: 5945
		public StateMachine.BaseDef stateMachineDef;
	}

	// Token: 0x020007A0 RID: 1952
	public class Instance
	{
		// Token: 0x060022A3 RID: 8867 RVA: 0x001D046C File Offset: 0x001CE66C
		public static void ResetParameters()
		{
			for (int i = 0; i < ChoreTable.Instance.parameters.Length; i++)
			{
				ChoreTable.Instance.parameters[i] = null;
			}
		}

		// Token: 0x060022A4 RID: 8868 RVA: 0x001D0494 File Offset: 0x001CE694
		public Instance(ChoreTable chore_table, KPrefabID prefab_id)
		{
			this.prefabId = prefab_id;
			this.entries = ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.Allocate();
			for (int i = 0; i < chore_table.entries.Length; i++)
			{
				this.entries.Add(new ChoreTable.Instance.Entry(chore_table.entries[i], prefab_id));
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x001D04EC File Offset: 0x001CE6EC
		~Instance()
		{
			this.OnCleanUp(this.prefabId);
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x001D0520 File Offset: 0x001CE720
		public void OnCleanUp(KPrefabID prefab_id)
		{
			if (this.entries == null)
			{
				return;
			}
			for (int i = 0; i < this.entries.Count; i++)
			{
				this.entries[i].OnCleanUp(prefab_id);
			}
			this.entries.Recycle();
			this.entries = null;
		}

		// Token: 0x0400173A RID: 5946
		private static object[] parameters = new object[3];

		// Token: 0x0400173B RID: 5947
		private KPrefabID prefabId;

		// Token: 0x0400173C RID: 5948
		private ListPool<ChoreTable.Instance.Entry, ChoreTable.Instance>.PooledList entries;

		// Token: 0x020007A1 RID: 1953
		private struct Entry
		{
			// Token: 0x060022A8 RID: 8872 RVA: 0x001D0574 File Offset: 0x001CE774
			public Entry(ChoreTable.Entry chore_table_entry, KPrefabID prefab_id)
			{
				ChoreTable.Instance.parameters[0] = chore_table_entry.stateMachineDef;
				ChoreTable.Instance.parameters[1] = chore_table_entry.choreType;
				ChoreTable.Instance.parameters[2] = prefab_id;
				this.chore = (Chore)Activator.CreateInstance(chore_table_entry.choreClassType, ChoreTable.Instance.parameters);
				ChoreTable.Instance.parameters[0] = null;
				ChoreTable.Instance.parameters[1] = null;
				ChoreTable.Instance.parameters[2] = null;
			}

			// Token: 0x060022A9 RID: 8873 RVA: 0x000BAFF3 File Offset: 0x000B91F3
			public void OnCleanUp(KPrefabID prefab_id)
			{
				if (this.chore != null)
				{
					this.chore.Cancel("ChoreTable.Instance.OnCleanUp");
					this.chore = null;
				}
			}

			// Token: 0x0400173D RID: 5949
			public Chore chore;
		}
	}
}
