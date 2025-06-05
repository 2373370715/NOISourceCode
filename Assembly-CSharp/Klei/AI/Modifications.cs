using System;
using System.Collections.Generic;
using System.IO;
using KSerialization;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CDC RID: 15580
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Modifications<ModifierType, InstanceType> : ISaveLoadableDetails where ModifierType : Resource where InstanceType : ModifierInstance<ModifierType>
	{
		// Token: 0x17000C61 RID: 3169
		// (get) Token: 0x0600EF0F RID: 61199 RVA: 0x00144E11 File Offset: 0x00143011
		public int Count
		{
			get
			{
				return this.ModifierList.Count;
			}
		}

		// Token: 0x0600EF10 RID: 61200 RVA: 0x00144E1E File Offset: 0x0014301E
		public IEnumerator<InstanceType> GetEnumerator()
		{
			return this.ModifierList.GetEnumerator();
		}

		// Token: 0x17000C62 RID: 3170
		// (get) Token: 0x0600EF11 RID: 61201 RVA: 0x00144E30 File Offset: 0x00143030
		// (set) Token: 0x0600EF12 RID: 61202 RVA: 0x00144E38 File Offset: 0x00143038
		public GameObject gameObject { get; private set; }

		// Token: 0x17000C63 RID: 3171
		public InstanceType this[int idx]
		{
			get
			{
				return this.ModifierList[idx];
			}
		}

		// Token: 0x0600EF14 RID: 61204 RVA: 0x00144E4F File Offset: 0x0014304F
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x0600EF15 RID: 61205 RVA: 0x00144E5C File Offset: 0x0014305C
		public void Trigger(GameHashes hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger((int)hash, data);
		}

		// Token: 0x0600EF16 RID: 61206 RVA: 0x004E8BDC File Offset: 0x004E6DDC
		public virtual InstanceType CreateInstance(ModifierType modifier)
		{
			return default(InstanceType);
		}

		// Token: 0x0600EF17 RID: 61207 RVA: 0x00144E70 File Offset: 0x00143070
		public Modifications(GameObject go, ResourceSet<ModifierType> resources = null)
		{
			this.resources = resources;
			this.gameObject = go;
		}

		// Token: 0x0600EF18 RID: 61208 RVA: 0x00144E91 File Offset: 0x00143091
		public virtual InstanceType Add(InstanceType instance)
		{
			this.ModifierList.Add(instance);
			return instance;
		}

		// Token: 0x0600EF19 RID: 61209 RVA: 0x004E8BF4 File Offset: 0x004E6DF4
		public virtual void Remove(InstanceType instance)
		{
			for (int i = 0; i < this.ModifierList.Count; i++)
			{
				if (this.ModifierList[i] == instance)
				{
					this.ModifierList.RemoveAt(i);
					instance.OnCleanUp();
					return;
				}
			}
		}

		// Token: 0x0600EF1A RID: 61210 RVA: 0x00144EA0 File Offset: 0x001430A0
		public bool Has(ModifierType modifier)
		{
			return this.Get(modifier) != null;
		}

		// Token: 0x0600EF1B RID: 61211 RVA: 0x004E8C48 File Offset: 0x004E6E48
		public InstanceType Get(ModifierType modifier)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier == modifier)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x0600EF1C RID: 61212 RVA: 0x004E8CBC File Offset: 0x004E6EBC
		public InstanceType Get(string id)
		{
			foreach (InstanceType instanceType in this.ModifierList)
			{
				if (instanceType.modifier.Id == id)
				{
					return instanceType;
				}
			}
			return default(InstanceType);
		}

		// Token: 0x0600EF1D RID: 61213 RVA: 0x004E8D34 File Offset: 0x004E6F34
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.ModifierList.Count);
			foreach (InstanceType instanceType in this.ModifierList)
			{
				writer.WriteKleiString(instanceType.modifier.Id);
				long position = writer.BaseStream.Position;
				writer.Write(0);
				long position2 = writer.BaseStream.Position;
				Serializer.SerializeTypeless(instanceType, writer);
				long position3 = writer.BaseStream.Position;
				long num = position3 - position2;
				writer.BaseStream.Position = position;
				writer.Write((int)num);
				writer.BaseStream.Position = position3;
			}
		}

		// Token: 0x0600EF1E RID: 61214 RVA: 0x004E8E14 File Offset: 0x004E7014
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string text = reader.ReadKleiString();
				int num2 = reader.ReadInt32();
				int position = reader.Position;
				InstanceType instanceType = this.Get(text);
				if (instanceType == null && this.resources != null)
				{
					ModifierType modifierType = this.resources.TryGet(text);
					if (modifierType != null)
					{
						instanceType = this.CreateInstance(modifierType);
					}
				}
				if (instanceType == null)
				{
					if (text != "Condition")
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							this.gameObject.name,
							"Missing modifier: " + text
						});
					}
					reader.SkipBytes(num2);
				}
				else if (!(instanceType is ISaveLoadable))
				{
					reader.SkipBytes(num2);
				}
				else
				{
					Deserializer.DeserializeTypeless(instanceType, reader);
					if (reader.Position != position + num2)
					{
						DebugUtil.LogWarningArgs(new object[]
						{
							"Expected to be at offset",
							position + num2,
							"but was only at offset",
							reader.Position,
							". Skipping to catch up."
						});
						reader.SkipBytes(position + num2 - reader.Position);
					}
				}
			}
		}

		// Token: 0x0400EACB RID: 60107
		public List<InstanceType> ModifierList = new List<InstanceType>();

		// Token: 0x0400EACD RID: 60109
		private ResourceSet<ModifierType> resources;
	}
}
