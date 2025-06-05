using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000924 RID: 2340
public class StateMachineDebuggerSettings : ScriptableObject
{
	// Token: 0x06002910 RID: 10512 RVA: 0x000BF29D File Offset: 0x000BD49D
	public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
	{
		return this.entries.GetEnumerator();
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x000BF2AF File Offset: 0x000BD4AF
	public static StateMachineDebuggerSettings Get()
	{
		if (StateMachineDebuggerSettings._Instance == null)
		{
			StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>("StateMachineDebuggerSettings");
			StateMachineDebuggerSettings._Instance.Initialize();
		}
		return StateMachineDebuggerSettings._Instance;
	}

	// Token: 0x06002912 RID: 10514 RVA: 0x001E15CC File Offset: 0x001DF7CC
	private void Initialize()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (typeof(StateMachine).IsAssignableFrom(type))
			{
				this.CreateEntry(type);
			}
		}
		this.entries.RemoveAll((StateMachineDebuggerSettings.Entry x) => x.type == null);
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x001E165C File Offset: 0x001DF85C
	public StateMachineDebuggerSettings.Entry CreateEntry(Type type)
	{
		foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
		{
			if (type.FullName == entry.typeName)
			{
				entry.type = type;
				return entry;
			}
		}
		StateMachineDebuggerSettings.Entry entry2 = new StateMachineDebuggerSettings.Entry(type);
		this.entries.Add(entry2);
		return entry2;
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000BF2DC File Offset: 0x000BD4DC
	public void Clear()
	{
		this.entries.Clear();
		this.Initialize();
	}

	// Token: 0x04001BEB RID: 7147
	public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();

	// Token: 0x04001BEC RID: 7148
	private static StateMachineDebuggerSettings _Instance;

	// Token: 0x02000925 RID: 2341
	[Serializable]
	public class Entry
	{
		// Token: 0x06002916 RID: 10518 RVA: 0x000BF302 File Offset: 0x000BD502
		public Entry(Type type)
		{
			this.typeName = type.FullName;
			this.type = type;
		}

		// Token: 0x04001BED RID: 7149
		public Type type;

		// Token: 0x04001BEE RID: 7150
		public string typeName;

		// Token: 0x04001BEF RID: 7151
		public bool breakOnGoTo;

		// Token: 0x04001BF0 RID: 7152
		public bool enableConsoleLogging;

		// Token: 0x04001BF1 RID: 7153
		public bool saveHistory;
	}
}
