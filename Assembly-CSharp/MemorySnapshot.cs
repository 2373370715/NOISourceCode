using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

// Token: 0x02001511 RID: 5393
public class MemorySnapshot
{
	// Token: 0x0600702E RID: 28718 RVA: 0x00303754 File Offset: 0x00301954
	public static MemorySnapshot.TypeData GetTypeData(Type type, Dictionary<int, MemorySnapshot.TypeData> types)
	{
		int hashCode = type.GetHashCode();
		MemorySnapshot.TypeData typeData = null;
		if (!types.TryGetValue(hashCode, out typeData))
		{
			typeData = new MemorySnapshot.TypeData(type);
			types[hashCode] = typeData;
		}
		return typeData;
	}

	// Token: 0x0600702F RID: 28719 RVA: 0x00303788 File Offset: 0x00301988
	public static void IncrementFieldCount(Dictionary<int, MemorySnapshot.FieldCount> field_counts, string name)
	{
		int hashCode = name.GetHashCode();
		MemorySnapshot.FieldCount fieldCount = null;
		if (!field_counts.TryGetValue(hashCode, out fieldCount))
		{
			fieldCount = new MemorySnapshot.FieldCount();
			fieldCount.name = name;
			field_counts[hashCode] = fieldCount;
		}
		fieldCount.count++;
	}

	// Token: 0x06007030 RID: 28720 RVA: 0x003037CC File Offset: 0x003019CC
	private void CountReference(MemorySnapshot.ReferenceArgs refArgs)
	{
		if (MemorySnapshot.ShouldExclude(refArgs.reference_type))
		{
			return;
		}
		if (refArgs.reference_type == MemorySnapshot.detailType)
		{
			string text;
			if (refArgs.lineage.obj as UnityEngine.Object != null)
			{
				text = "\"" + ((UnityEngine.Object)refArgs.lineage.obj).name;
			}
			else
			{
				text = "\"" + MemorySnapshot.detailTypeStr;
			}
			if (refArgs.lineage.parent0 != null)
			{
				text += "\",\"";
				text += refArgs.lineage.parent0.ToString();
			}
			if (refArgs.lineage.parent1 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent1.ToString();
			}
			if (refArgs.lineage.parent2 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent2.ToString();
			}
			if (refArgs.lineage.parent3 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent3.ToString();
			}
			if (refArgs.lineage.parent4 != null)
			{
				text = text + "\",\"" + refArgs.lineage.parent4.ToString();
			}
			text += "\"\n";
			MemorySnapshot.DetailInfo value;
			this.detailTypeCount.TryGetValue(text, out value);
			value.count++;
			if (typeof(Array).IsAssignableFrom(refArgs.reference_type) && refArgs.lineage.obj != null)
			{
				Array array = refArgs.lineage.obj as Array;
				value.numArrayEntries += ((array != null) ? array.Length : 0);
			}
			this.detailTypeCount[text] = value;
		}
		if (refArgs.reference_type.IsClass)
		{
			MemorySnapshot.GetTypeData(refArgs.reference_type, this.types).refCount++;
			MemorySnapshot.IncrementFieldCount(this.fieldCounts, refArgs.field_name);
		}
		if (refArgs.lineage.obj == null)
		{
			return;
		}
		try
		{
			if (refArgs.lineage.obj.GetType().IsClass && !this.walked.Add(refArgs.lineage.obj))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		MemorySnapshot.TypeData typeData = MemorySnapshot.GetTypeData(refArgs.lineage.obj.GetType(), this.types);
		if (typeData.type.IsClass)
		{
			typeData.instanceCount++;
			if (typeof(Array).IsAssignableFrom(typeData.type))
			{
				Array array2 = refArgs.lineage.obj as Array;
				typeData.numArrayEntries += ((array2 != null) ? array2.Length : 0);
			}
			MemorySnapshot.HierarchyNode key = new MemorySnapshot.HierarchyNode(refArgs.lineage.parent0, refArgs.lineage.parent1, refArgs.lineage.parent2, refArgs.lineage.parent3, refArgs.lineage.parent4);
			int num = 0;
			typeData.hierarchies.TryGetValue(key, out num);
			typeData.hierarchies[key] = num + 1;
		}
		foreach (FieldInfo fieldInfo in typeData.fields)
		{
			this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, new MemorySnapshot.Lineage(refArgs.lineage.obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, fieldInfo.DeclaringType)));
		}
		ICollection collection = refArgs.lineage.obj as ICollection;
		if (collection != null)
		{
			Type type = typeof(object);
			if (collection.GetType().GetElementType() != null)
			{
				type = collection.GetType().GetElementType();
			}
			else if (collection.GetType().GetGenericArguments().Length != 0)
			{
				type = collection.GetType().GetGenericArguments()[0];
			}
			if (!MemorySnapshot.ShouldExclude(type))
			{
				foreach (object obj in collection)
				{
					this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(type, refArgs.field_name + ".Item", new MemorySnapshot.Lineage(obj, refArgs.lineage.parent3, refArgs.lineage.parent2, refArgs.lineage.parent1, refArgs.lineage.parent0, collection.GetType())));
				}
			}
		}
	}

	// Token: 0x06007031 RID: 28721 RVA: 0x00303CD4 File Offset: 0x00301ED4
	private void CountField(MemorySnapshot.FieldArgs fieldArgs)
	{
		if (MemorySnapshot.ShouldExclude(fieldArgs.field.FieldType))
		{
			return;
		}
		object obj = null;
		try
		{
			if (!fieldArgs.field.FieldType.Name.Contains("*"))
			{
				obj = fieldArgs.field.GetValue(fieldArgs.lineage.obj);
			}
		}
		catch
		{
			obj = null;
		}
		string field_name = fieldArgs.field.DeclaringType.ToString() + "." + fieldArgs.field.Name;
		this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(fieldArgs.field.FieldType, field_name, new MemorySnapshot.Lineage(obj, fieldArgs.lineage.parent3, fieldArgs.lineage.parent2, fieldArgs.lineage.parent1, fieldArgs.lineage.parent0, fieldArgs.field.DeclaringType)));
	}

	// Token: 0x06007032 RID: 28722 RVA: 0x000EDEA1 File Offset: 0x000EC0A1
	private static bool ShouldExclude(Type type)
	{
		return type.IsPrimitive || type.IsEnum || type == typeof(MemorySnapshot);
	}

	// Token: 0x06007033 RID: 28723 RVA: 0x00303DC0 File Offset: 0x00301FC0
	private void CountAll()
	{
		while (this.refsToProcess.Count > 0 || this.fieldsToProcess.Count > 0)
		{
			while (this.fieldsToProcess.Count > 0)
			{
				MemorySnapshot.FieldArgs fieldArgs = this.fieldsToProcess[this.fieldsToProcess.Count - 1];
				this.fieldsToProcess.RemoveAt(this.fieldsToProcess.Count - 1);
				this.CountField(fieldArgs);
			}
			while (this.refsToProcess.Count > 0)
			{
				MemorySnapshot.ReferenceArgs refArgs = this.refsToProcess[this.refsToProcess.Count - 1];
				this.refsToProcess.RemoveAt(this.refsToProcess.Count - 1);
				this.CountReference(refArgs);
			}
		}
	}

	// Token: 0x06007034 RID: 28724 RVA: 0x00303E7C File Offset: 0x0030207C
	public MemorySnapshot()
	{
		MemorySnapshot.Lineage lineage = new MemorySnapshot.Lineage(null, null, null, null, null, null);
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (fieldInfo.IsStatic)
				{
					this.statics.Add(fieldInfo);
					lineage.parent0 = fieldInfo.DeclaringType;
					this.fieldsToProcess.Add(new MemorySnapshot.FieldArgs(fieldInfo, lineage));
				}
			}
		}
		this.CountAll();
		foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)))
		{
			lineage.obj = @object;
			lineage.parent0 = @object.GetType();
			this.refsToProcess.Add(new MemorySnapshot.ReferenceArgs(@object.GetType(), "Object." + @object.name, lineage));
		}
		this.CountAll();
	}

	// Token: 0x06007035 RID: 28725 RVA: 0x00303FEC File Offset: 0x003021EC
	public void WriteTypeDetails(MemorySnapshot compare)
	{
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list = null;
		if (compare != null)
		{
			list = compare.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		}
		List<KeyValuePair<string, MemorySnapshot.DetailInfo>> list2 = this.detailTypeCount.ToList<KeyValuePair<string, MemorySnapshot.DetailInfo>>();
		list2.Sort((KeyValuePair<string, MemorySnapshot.DetailInfo> x, KeyValuePair<string, MemorySnapshot.DetailInfo> y) => y.Value.count - x.Value.count);
		using (StreamWriter streamWriter = new StreamWriter(GarbageProfiler.GetFileName("type_details_" + MemorySnapshot.detailTypeStr)))
		{
			streamWriter.WriteLine("Delta,Count,NumArrayEntries,Type");
			foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair in list2)
			{
				int num = keyValuePair.Value.count;
				if (list != null)
				{
					foreach (KeyValuePair<string, MemorySnapshot.DetailInfo> keyValuePair2 in list)
					{
						if (keyValuePair2.Key == keyValuePair.Key)
						{
							num -= keyValuePair2.Value.count;
							break;
						}
					}
				}
				TextWriter textWriter = streamWriter;
				string[] array = new string[7];
				array[0] = num.ToString();
				array[1] = ",";
				int num2 = 2;
				MemorySnapshot.DetailInfo value = keyValuePair.Value;
				array[num2] = value.count.ToString();
				array[3] = ",";
				int num3 = 4;
				value = keyValuePair.Value;
				array[num3] = value.numArrayEntries.ToString();
				array[5] = ",";
				array[6] = keyValuePair.Key;
				textWriter.Write(string.Concat(array));
			}
		}
	}

	// Token: 0x04005436 RID: 21558
	public Dictionary<int, MemorySnapshot.TypeData> types = new Dictionary<int, MemorySnapshot.TypeData>();

	// Token: 0x04005437 RID: 21559
	public Dictionary<int, MemorySnapshot.FieldCount> fieldCounts = new Dictionary<int, MemorySnapshot.FieldCount>();

	// Token: 0x04005438 RID: 21560
	public HashSet<object> walked = new HashSet<object>();

	// Token: 0x04005439 RID: 21561
	public List<FieldInfo> statics = new List<FieldInfo>();

	// Token: 0x0400543A RID: 21562
	public Dictionary<string, MemorySnapshot.DetailInfo> detailTypeCount = new Dictionary<string, MemorySnapshot.DetailInfo>();

	// Token: 0x0400543B RID: 21563
	private static readonly Type detailType = typeof(byte[]);

	// Token: 0x0400543C RID: 21564
	private static readonly string detailTypeStr = MemorySnapshot.detailType.ToString();

	// Token: 0x0400543D RID: 21565
	private List<MemorySnapshot.FieldArgs> fieldsToProcess = new List<MemorySnapshot.FieldArgs>();

	// Token: 0x0400543E RID: 21566
	private List<MemorySnapshot.ReferenceArgs> refsToProcess = new List<MemorySnapshot.ReferenceArgs>();

	// Token: 0x02001512 RID: 5394
	public struct HierarchyNode
	{
		// Token: 0x06007037 RID: 28727 RVA: 0x000EDEE5 File Offset: 0x000EC0E5
		public HierarchyNode(Type parent_0, Type parent_1, Type parent_2, Type parent_3, Type parent_4)
		{
			this.parent0 = parent_0;
			this.parent1 = parent_1;
			this.parent2 = parent_2;
			this.parent3 = parent_3;
			this.parent4 = parent_4;
		}

		// Token: 0x06007038 RID: 28728 RVA: 0x003041BC File Offset: 0x003023BC
		public bool Equals(MemorySnapshot.HierarchyNode a, MemorySnapshot.HierarchyNode b)
		{
			return a.parent0 == b.parent0 && a.parent1 == b.parent1 && a.parent2 == b.parent2 && a.parent3 == b.parent3 && a.parent4 == b.parent4;
		}

		// Token: 0x06007039 RID: 28729 RVA: 0x00304228 File Offset: 0x00302428
		public override int GetHashCode()
		{
			int num = 0;
			if (this.parent0 != null)
			{
				num += this.parent0.GetHashCode();
			}
			if (this.parent1 != null)
			{
				num += this.parent1.GetHashCode();
			}
			if (this.parent2 != null)
			{
				num += this.parent2.GetHashCode();
			}
			if (this.parent3 != null)
			{
				num += this.parent3.GetHashCode();
			}
			if (this.parent4 != null)
			{
				num += this.parent4.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600703A RID: 28730 RVA: 0x003042C4 File Offset: 0x003024C4
		public override string ToString()
		{
			if (this.parent4 != null)
			{
				return string.Concat(new string[]
				{
					this.parent4.ToString(),
					"--",
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent3 != null)
			{
				return string.Concat(new string[]
				{
					this.parent3.ToString(),
					"--",
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent2 != null)
			{
				return string.Concat(new string[]
				{
					this.parent2.ToString(),
					"--",
					this.parent1.ToString(),
					"--",
					this.parent0.ToString()
				});
			}
			if (this.parent1 != null)
			{
				return this.parent1.ToString() + "--" + this.parent0.ToString();
			}
			return this.parent0.ToString();
		}

		// Token: 0x0400543F RID: 21567
		public Type parent0;

		// Token: 0x04005440 RID: 21568
		public Type parent1;

		// Token: 0x04005441 RID: 21569
		public Type parent2;

		// Token: 0x04005442 RID: 21570
		public Type parent3;

		// Token: 0x04005443 RID: 21571
		public Type parent4;
	}

	// Token: 0x02001513 RID: 5395
	public class FieldCount
	{
		// Token: 0x04005444 RID: 21572
		public string name;

		// Token: 0x04005445 RID: 21573
		public int count;
	}

	// Token: 0x02001514 RID: 5396
	public class TypeData
	{
		// Token: 0x0600703C RID: 28732 RVA: 0x0030444C File Offset: 0x0030264C
		public TypeData(Type type)
		{
			this.type = type;
			this.fields = new List<FieldInfo>();
			this.instanceCount = 0;
			this.refCount = 0;
			this.numArrayEntries = 0;
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy))
			{
				if (!fieldInfo.IsStatic && !MemorySnapshot.ShouldExclude(fieldInfo.FieldType))
				{
					this.fields.Add(fieldInfo);
				}
			}
		}

		// Token: 0x04005446 RID: 21574
		public Dictionary<MemorySnapshot.HierarchyNode, int> hierarchies = new Dictionary<MemorySnapshot.HierarchyNode, int>();

		// Token: 0x04005447 RID: 21575
		public Type type;

		// Token: 0x04005448 RID: 21576
		public List<FieldInfo> fields;

		// Token: 0x04005449 RID: 21577
		public int instanceCount;

		// Token: 0x0400544A RID: 21578
		public int refCount;

		// Token: 0x0400544B RID: 21579
		public int numArrayEntries;
	}

	// Token: 0x02001515 RID: 5397
	public struct DetailInfo
	{
		// Token: 0x0400544C RID: 21580
		public int count;

		// Token: 0x0400544D RID: 21581
		public int numArrayEntries;
	}

	// Token: 0x02001516 RID: 5398
	private struct Lineage
	{
		// Token: 0x0600703D RID: 28733 RVA: 0x000EDF0C File Offset: 0x000EC10C
		public Lineage(object obj, Type parent4, Type parent3, Type parent2, Type parent1, Type parent0)
		{
			this.obj = obj;
			this.parent0 = parent0;
			this.parent1 = parent1;
			this.parent2 = parent2;
			this.parent3 = parent3;
			this.parent4 = parent4;
		}

		// Token: 0x0400544E RID: 21582
		public object obj;

		// Token: 0x0400544F RID: 21583
		public Type parent0;

		// Token: 0x04005450 RID: 21584
		public Type parent1;

		// Token: 0x04005451 RID: 21585
		public Type parent2;

		// Token: 0x04005452 RID: 21586
		public Type parent3;

		// Token: 0x04005453 RID: 21587
		public Type parent4;
	}

	// Token: 0x02001517 RID: 5399
	private struct ReferenceArgs
	{
		// Token: 0x0600703E RID: 28734 RVA: 0x000EDF3B File Offset: 0x000EC13B
		public ReferenceArgs(Type reference_type, string field_name, MemorySnapshot.Lineage lineage)
		{
			this.reference_type = reference_type;
			this.lineage = lineage;
			this.field_name = field_name;
		}

		// Token: 0x04005454 RID: 21588
		public Type reference_type;

		// Token: 0x04005455 RID: 21589
		public string field_name;

		// Token: 0x04005456 RID: 21590
		public MemorySnapshot.Lineage lineage;
	}

	// Token: 0x02001518 RID: 5400
	private struct FieldArgs
	{
		// Token: 0x0600703F RID: 28735 RVA: 0x000EDF52 File Offset: 0x000EC152
		public FieldArgs(FieldInfo field, MemorySnapshot.Lineage lineage)
		{
			this.field = field;
			this.lineage = lineage;
		}

		// Token: 0x04005457 RID: 21591
		public FieldInfo field;

		// Token: 0x04005458 RID: 21592
		public MemorySnapshot.Lineage lineage;
	}
}
