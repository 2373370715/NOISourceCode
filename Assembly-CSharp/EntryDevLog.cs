using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02001D14 RID: 7444
public class EntryDevLog
{
	// Token: 0x06009B8D RID: 39821 RVA: 0x003CD0A0 File Offset: 0x003CB2A0
	[Conditional("UNITY_EDITOR")]
	public void AddModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Add(new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x06009B8E RID: 39822 RVA: 0x003CD0C8 File Offset: 0x003CB2C8
	[Conditional("UNITY_EDITOR")]
	public void InsertModificationRecord(int index, EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Insert(index, new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x06009B8F RID: 39823 RVA: 0x003CD0F4 File Offset: 0x003CB2F4
	private string TrimAuthor()
	{
		string text = "";
		string[] array = new string[]
		{
			"Invoke",
			"CreateInstance",
			"AwakeInternal",
			"Internal",
			"<>",
			"YamlDotNet",
			"Deserialize"
		};
		string[] array2 = new string[]
		{
			".ctor",
			"Trigger",
			"AddContentContainerRange",
			"AddContentContainer",
			"InsertContentContainer",
			"KInstantiateUI",
			"Start",
			"InitializeComponentAwake",
			"TrimAuthor",
			"InsertModificationRecord",
			"AddModificationRecord",
			"SetValue",
			"Write"
		};
		StackTrace stackTrace = new StackTrace();
		int i = 0;
		int num = 0;
		int num2 = 3;
		while (i < num2)
		{
			num++;
			if (stackTrace.FrameCount <= num)
			{
				break;
			}
			MethodBase method = stackTrace.GetFrame(num).GetMethod();
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = (flag || method.Name.Contains(array[j]));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				flag = (flag || method.Name.Contains(array2[k]));
			}
			if (!flag && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("set_") && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("Instantiate"))
			{
				if (i != 0)
				{
					text += " < ";
				}
				i++;
				text += stackTrace.GetFrame(num).GetMethod().Name;
			}
		}
		return text;
	}

	// Token: 0x0400799F RID: 31135
	[SerializeField]
	public List<EntryDevLog.ModificationRecord> modificationRecords = new List<EntryDevLog.ModificationRecord>();

	// Token: 0x02001D15 RID: 7445
	public class ModificationRecord
	{
		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06009B91 RID: 39825 RVA: 0x00109B50 File Offset: 0x00107D50
		// (set) Token: 0x06009B92 RID: 39826 RVA: 0x00109B58 File Offset: 0x00107D58
		public EntryDevLog.ModificationRecord.ActionType actionType { get; private set; }

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06009B93 RID: 39827 RVA: 0x00109B61 File Offset: 0x00107D61
		// (set) Token: 0x06009B94 RID: 39828 RVA: 0x00109B69 File Offset: 0x00107D69
		public string target { get; private set; }

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06009B95 RID: 39829 RVA: 0x00109B72 File Offset: 0x00107D72
		// (set) Token: 0x06009B96 RID: 39830 RVA: 0x00109B7A File Offset: 0x00107D7A
		public object newValue { get; private set; }

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06009B97 RID: 39831 RVA: 0x00109B83 File Offset: 0x00107D83
		// (set) Token: 0x06009B98 RID: 39832 RVA: 0x00109B8B File Offset: 0x00107D8B
		public string author { get; private set; }

		// Token: 0x06009B99 RID: 39833 RVA: 0x00109B94 File Offset: 0x00107D94
		public ModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue, string author)
		{
			this.target = target;
			this.newValue = newValue;
			this.author = author;
			this.actionType = actionType;
		}

		// Token: 0x02001D16 RID: 7446
		public enum ActionType
		{
			// Token: 0x040079A5 RID: 31141
			Created,
			// Token: 0x040079A6 RID: 31142
			ChangeSubEntry,
			// Token: 0x040079A7 RID: 31143
			ChangeContent,
			// Token: 0x040079A8 RID: 31144
			ValueChange,
			// Token: 0x040079A9 RID: 31145
			YAMLData
		}
	}
}
