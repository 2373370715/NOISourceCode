using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;

// Token: 0x02000929 RID: 2345
public class StateMachineSerializer
{
	// Token: 0x06002929 RID: 10537 RVA: 0x001E1800 File Offset: 0x001DFA00
	public void Serialize(List<StateMachine.Instance> state_machines, BinaryWriter writer)
	{
		writer.Write(StateMachineSerializer.SERIALIZER_VERSION);
		long position = writer.BaseStream.Position;
		writer.Write(0);
		long position2 = writer.BaseStream.Position;
		try
		{
			int num = (int)writer.BaseStream.Position;
			int num2 = 0;
			writer.Write(num2);
			using (List<StateMachine.Instance>.Enumerator enumerator = state_machines.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (StateMachineSerializer.Entry.TrySerialize(enumerator.Current, writer))
					{
						num2++;
					}
				}
			}
			int num3 = (int)writer.BaseStream.Position;
			writer.BaseStream.Position = (long)num;
			writer.Write(num2);
			writer.BaseStream.Position = (long)num3;
		}
		catch (Exception obj)
		{
			Debug.Log("StateMachines: ");
			foreach (StateMachine.Instance instance in state_machines)
			{
				Debug.Log(instance.ToString());
			}
			Debug.LogError(obj);
		}
		long position3 = writer.BaseStream.Position;
		long num4 = position3 - position2;
		writer.BaseStream.Position = position;
		writer.Write((int)num4);
		writer.BaseStream.Position = position3;
	}

	// Token: 0x0600292A RID: 10538 RVA: 0x001E1960 File Offset: 0x001DFB60
	public void Deserialize(IReader reader)
	{
		int num = reader.ReadInt32();
		int length = reader.ReadInt32();
		if (num < 10)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"State machine serializer version mismatch: ",
				num.ToString(),
				"!=",
				StateMachineSerializer.SERIALIZER_VERSION.ToString(),
				"\nDiscarding data."
			}));
			reader.SkipBytes(length);
			return;
		}
		if (num < 12)
		{
			this.entries = StateMachineSerializer.OldEntryV11.DeserializeOldEntries(reader, num);
			return;
		}
		int num2 = reader.ReadInt32();
		this.entries = new List<StateMachineSerializer.Entry>(num2);
		for (int i = 0; i < num2; i++)
		{
			StateMachineSerializer.Entry entry = StateMachineSerializer.Entry.Deserialize(reader, num);
			if (entry != null)
			{
				this.entries.Add(entry);
			}
		}
	}

	// Token: 0x0600292B RID: 10539 RVA: 0x001E1A14 File Offset: 0x001DFC14
	private static string TrimAssemblyInfo(string type_name)
	{
		int num = type_name.IndexOf("[[");
		if (num != -1)
		{
			return type_name.Substring(0, num);
		}
		return type_name;
	}

	// Token: 0x0600292C RID: 10540 RVA: 0x001E1A3C File Offset: 0x001DFC3C
	public bool Restore(StateMachine.Instance instance)
	{
		Type type = instance.GetType();
		for (int i = 0; i < this.entries.Count; i++)
		{
			StateMachineSerializer.Entry entry = this.entries[i];
			if (entry.type == type && instance.serializationSuffix == entry.typeSuffix)
			{
				this.entries.RemoveAt(i);
				return entry.Restore(instance);
			}
		}
		return false;
	}

	// Token: 0x0600292D RID: 10541 RVA: 0x000BF42B File Offset: 0x000BD62B
	private static bool DoesVersionHaveTypeSuffix(int version)
	{
		return version >= 20 || version == 11;
	}

	// Token: 0x04001BF9 RID: 7161
	public const int SERIALIZER_PRE_DLC1 = 10;

	// Token: 0x04001BFA RID: 7162
	public const int SERIALIZER_TYPE_SUFFIX = 11;

	// Token: 0x04001BFB RID: 7163
	public const int SERIALIZER_OPTIMIZE_BUFFERS = 12;

	// Token: 0x04001BFC RID: 7164
	public const int SERIALIZER_EXPANSION1 = 20;

	// Token: 0x04001BFD RID: 7165
	private static int SERIALIZER_VERSION = 20;

	// Token: 0x04001BFE RID: 7166
	private const string TargetParameterName = "TargetParameter";

	// Token: 0x04001BFF RID: 7167
	private List<StateMachineSerializer.Entry> entries = new List<StateMachineSerializer.Entry>();

	// Token: 0x0200092A RID: 2346
	private class Entry
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x001E1AAC File Offset: 0x001DFCAC
		public static bool TrySerialize(StateMachine.Instance smi, BinaryWriter writer)
		{
			if (!smi.IsRunning())
			{
				return false;
			}
			int num = (int)writer.BaseStream.Position;
			writer.Write(0);
			writer.WriteKleiString(smi.GetType().FullName);
			writer.WriteKleiString(smi.serializationSuffix);
			writer.WriteKleiString(smi.GetCurrentState().name);
			int num2 = (int)writer.BaseStream.Position;
			writer.Write(0);
			int num3 = (int)writer.BaseStream.Position;
			Serializer.SerializeTypeless(smi, writer);
			if (smi.GetStateMachine().serializable == StateMachine.SerializeType.ParamsOnly || smi.GetStateMachine().serializable == StateMachine.SerializeType.Both_DEPRECATED)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				writer.Write(parameterContexts.Length);
				foreach (StateMachine.Parameter.Context context in parameterContexts)
				{
					long position = (long)((int)writer.BaseStream.Position);
					writer.Write(0);
					long num4 = (long)((int)writer.BaseStream.Position);
					writer.WriteKleiString(context.GetType().FullName);
					writer.WriteKleiString(context.parameter.name);
					context.Serialize(writer);
					long num5 = (long)((int)writer.BaseStream.Position);
					writer.BaseStream.Position = position;
					long num6 = num5 - num4;
					writer.Write((int)num6);
					writer.BaseStream.Position = num5;
				}
			}
			int num7 = (int)writer.BaseStream.Position - num3;
			if (num7 > 0)
			{
				int num8 = (int)writer.BaseStream.Position;
				writer.BaseStream.Position = (long)num2;
				writer.Write(num7);
				writer.BaseStream.Position = (long)num8;
				return true;
			}
			writer.BaseStream.Position = (long)num;
			writer.BaseStream.SetLength((long)num);
			return false;
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x001E1C6C File Offset: 0x001DFE6C
		public static StateMachineSerializer.Entry Deserialize(IReader reader, int serializerVersion)
		{
			StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
			reader.ReadInt32();
			entry.version = serializerVersion;
			string typeName = reader.ReadKleiString();
			entry.type = Type.GetType(typeName);
			entry.typeSuffix = (StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null);
			entry.currentState = reader.ReadKleiString();
			int length = reader.ReadInt32();
			entry.entryData = new FastReader(reader.ReadBytes(length));
			if (entry.type == null)
			{
				return null;
			}
			return entry;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x001E1CF0 File Offset: 0x001DFEF0
		public bool Restore(StateMachine.Instance smi)
		{
			if (Manager.HasDeserializationMapping(smi.GetType()))
			{
				Deserializer.DeserializeTypeless(smi, this.entryData);
			}
			StateMachine.SerializeType serializable = smi.GetStateMachine().serializable;
			if (serializable == StateMachine.SerializeType.Never)
			{
				return false;
			}
			if ((serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.ParamsOnly) && !this.entryData.IsFinished)
			{
				StateMachine.Parameter.Context[] parameterContexts = smi.GetParameterContexts();
				int num = this.entryData.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int num2 = this.entryData.ReadInt32();
					int position = this.entryData.Position;
					string text = this.entryData.ReadKleiString();
					text = text.Replace("Version=2.0.0.0", "Version=4.0.0.0");
					string b = this.entryData.ReadKleiString();
					foreach (StateMachine.Parameter.Context context in parameterContexts)
					{
						if (context.parameter.name == b && (this.version > 10 || !(context.parameter.GetType().Name == "TargetParameter")) && context.GetType().FullName == text)
						{
							context.Deserialize(this.entryData, smi);
							break;
						}
					}
					this.entryData.SkipBytes(num2 - (this.entryData.Position - position));
				}
			}
			if (serializable == StateMachine.SerializeType.Both_DEPRECATED || serializable == StateMachine.SerializeType.CurrentStateOnly_DEPRECATED)
			{
				StateMachine.BaseState state = smi.GetStateMachine().GetState(this.currentState);
				if (state != null)
				{
					smi.OnParamsDeserialized();
					smi.GoTo(state);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001C00 RID: 7168
		public int version;

		// Token: 0x04001C01 RID: 7169
		public Type type;

		// Token: 0x04001C02 RID: 7170
		public string typeSuffix;

		// Token: 0x04001C03 RID: 7171
		public string currentState;

		// Token: 0x04001C04 RID: 7172
		public FastReader entryData;
	}

	// Token: 0x0200092B RID: 2347
	private class OldEntryV11
	{
		// Token: 0x06002934 RID: 10548 RVA: 0x000BF455 File Offset: 0x000BD655
		public OldEntryV11(int version, int dataPos, Type type, string typeSuffix, string currentState)
		{
			this.version = version;
			this.dataPos = dataPos;
			this.type = type;
			this.typeSuffix = typeSuffix;
			this.currentState = currentState;
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x001E1E7C File Offset: 0x001E007C
		public static List<StateMachineSerializer.Entry> DeserializeOldEntries(IReader reader, int serializerVersion)
		{
			Debug.Assert(serializerVersion < 12);
			List<StateMachineSerializer.OldEntryV11> list = StateMachineSerializer.OldEntryV11.ReadEntries(reader, serializerVersion);
			byte[] bytes = StateMachineSerializer.OldEntryV11.ReadEntryData(reader);
			List<StateMachineSerializer.Entry> list2 = new List<StateMachineSerializer.Entry>(list.Count);
			foreach (StateMachineSerializer.OldEntryV11 oldEntryV in list)
			{
				StateMachineSerializer.Entry entry = new StateMachineSerializer.Entry();
				entry.version = serializerVersion;
				entry.type = oldEntryV.type;
				entry.typeSuffix = oldEntryV.typeSuffix;
				entry.currentState = oldEntryV.currentState;
				entry.entryData = new FastReader(bytes);
				entry.entryData.SkipBytes(oldEntryV.dataPos);
				list2.Add(entry);
			}
			return list2;
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x001E1F44 File Offset: 0x001E0144
		private static StateMachineSerializer.OldEntryV11 Deserialize(IReader reader, int serializerVersion)
		{
			int num = reader.ReadInt32();
			int num2 = reader.ReadInt32();
			string typeName = reader.ReadKleiString();
			string text = StateMachineSerializer.DoesVersionHaveTypeSuffix(serializerVersion) ? reader.ReadKleiString() : null;
			string text2 = reader.ReadKleiString();
			Type left = Type.GetType(typeName);
			if (left == null)
			{
				return null;
			}
			return new StateMachineSerializer.OldEntryV11(num, num2, left, text, text2);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x001E1F9C File Offset: 0x001E019C
		private static List<StateMachineSerializer.OldEntryV11> ReadEntries(IReader reader, int serializerVersion)
		{
			List<StateMachineSerializer.OldEntryV11> list = new List<StateMachineSerializer.OldEntryV11>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				StateMachineSerializer.OldEntryV11 oldEntryV = StateMachineSerializer.OldEntryV11.Deserialize(reader, serializerVersion);
				if (oldEntryV != null)
				{
					list.Add(oldEntryV);
				}
			}
			return list;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x001E1FD8 File Offset: 0x001E01D8
		private static byte[] ReadEntryData(IReader reader)
		{
			int length = reader.ReadInt32();
			return reader.ReadBytes(length);
		}

		// Token: 0x04001C05 RID: 7173
		public int version;

		// Token: 0x04001C06 RID: 7174
		public int dataPos;

		// Token: 0x04001C07 RID: 7175
		public Type type;

		// Token: 0x04001C08 RID: 7176
		public string typeSuffix;

		// Token: 0x04001C09 RID: 7177
		public string currentState;
	}
}
