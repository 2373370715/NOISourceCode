using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x02001E5A RID: 7770
[SerializationConfig(MemberSerialization.OptIn)]
public class SerializedList<ItemType>
{
	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x0600A2BB RID: 41659 RVA: 0x0010E3CF File Offset: 0x0010C5CF
	public int Count
	{
		get
		{
			return this.items.Count;
		}
	}

	// Token: 0x0600A2BC RID: 41660 RVA: 0x0010E3DC File Offset: 0x0010C5DC
	public IEnumerator<ItemType> GetEnumerator()
	{
		return this.items.GetEnumerator();
	}

	// Token: 0x17000A8A RID: 2698
	public ItemType this[int idx]
	{
		get
		{
			return this.items[idx];
		}
	}

	// Token: 0x0600A2BE RID: 41662 RVA: 0x0010E3FC File Offset: 0x0010C5FC
	public void Add(ItemType item)
	{
		this.items.Add(item);
	}

	// Token: 0x0600A2BF RID: 41663 RVA: 0x0010E40A File Offset: 0x0010C60A
	public void Remove(ItemType item)
	{
		this.items.Remove(item);
	}

	// Token: 0x0600A2C0 RID: 41664 RVA: 0x0010E419 File Offset: 0x0010C619
	public void RemoveAt(int idx)
	{
		this.items.RemoveAt(idx);
	}

	// Token: 0x0600A2C1 RID: 41665 RVA: 0x0010E427 File Offset: 0x0010C627
	public bool Contains(ItemType item)
	{
		return this.items.Contains(item);
	}

	// Token: 0x0600A2C2 RID: 41666 RVA: 0x0010E435 File Offset: 0x0010C635
	public void Clear()
	{
		this.items.Clear();
	}

	// Token: 0x0600A2C3 RID: 41667 RVA: 0x003EE198 File Offset: 0x003EC398
	[OnSerializing]
	private void OnSerializing()
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(this.items.Count);
		foreach (ItemType itemType in this.items)
		{
			binaryWriter.WriteKleiString(itemType.GetType().FullName);
			long position = binaryWriter.BaseStream.Position;
			binaryWriter.Write(0);
			long position2 = binaryWriter.BaseStream.Position;
			Serializer.SerializeTypeless(itemType, binaryWriter);
			long position3 = binaryWriter.BaseStream.Position;
			long num = position3 - position2;
			binaryWriter.BaseStream.Position = position;
			binaryWriter.Write((int)num);
			binaryWriter.BaseStream.Position = position3;
		}
		memoryStream.Flush();
		this.serializationBuffer = memoryStream.ToArray();
	}

	// Token: 0x0600A2C4 RID: 41668 RVA: 0x003EE298 File Offset: 0x003EC498
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.serializationBuffer == null)
		{
			return;
		}
		FastReader fastReader = new FastReader(this.serializationBuffer);
		int num = fastReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string text = fastReader.ReadKleiString();
			int num2 = fastReader.ReadInt32();
			int position = fastReader.Position;
			Type type = Type.GetType(text);
			if (type == null)
			{
				DebugUtil.LogWarningArgs(new object[]
				{
					"Type no longer exists: " + text
				});
				fastReader.SkipBytes(num2);
			}
			else
			{
				ItemType itemType;
				if (typeof(ItemType) != type)
				{
					itemType = (ItemType)((object)Activator.CreateInstance(type));
				}
				else
				{
					itemType = default(ItemType);
				}
				Deserializer.DeserializeTypeless(itemType, fastReader);
				if (fastReader.Position != position + num2)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Expected to be at offset",
						position + num2,
						"but was only at offset",
						fastReader.Position,
						". Skipping to catch up."
					});
					fastReader.SkipBytes(position + num2 - fastReader.Position);
				}
				this.items.Add(itemType);
			}
		}
	}

	// Token: 0x04007F5D RID: 32605
	[Serialize]
	private byte[] serializationBuffer;

	// Token: 0x04007F5E RID: 32606
	private List<ItemType> items = new List<ItemType>();
}
