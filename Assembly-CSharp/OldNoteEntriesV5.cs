using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x02000AE6 RID: 2790
public class OldNoteEntriesV5
{
	// Token: 0x06003368 RID: 13160 RVA: 0x002139E4 File Offset: 0x00211BE4
	public void Deserialize(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			OldNoteEntriesV5.NoteStorageBlock item = default(OldNoteEntriesV5.NoteStorageBlock);
			item.Deserialize(reader);
			this.storageBlocks.Add(item);
		}
	}

	// Token: 0x04002335 RID: 9013
	public List<OldNoteEntriesV5.NoteStorageBlock> storageBlocks = new List<OldNoteEntriesV5.NoteStorageBlock>();

	// Token: 0x02000AE7 RID: 2791
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntry
	{
		// Token: 0x04002336 RID: 9014
		[FieldOffset(0)]
		public int reportEntryId;

		// Token: 0x04002337 RID: 9015
		[FieldOffset(4)]
		public int noteHash;

		// Token: 0x04002338 RID: 9016
		[FieldOffset(8)]
		public float value;
	}

	// Token: 0x02000AE8 RID: 2792
	[StructLayout(LayoutKind.Explicit)]
	public struct NoteEntryArray
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600336A RID: 13162 RVA: 0x000C5FB4 File Offset: 0x000C41B4
		public int StructSizeInBytes
		{
			get
			{
				return Marshal.SizeOf(typeof(OldNoteEntriesV5.NoteEntry));
			}
		}

		// Token: 0x04002339 RID: 9017
		[FieldOffset(0)]
		public byte[] bytes;

		// Token: 0x0400233A RID: 9018
		[FieldOffset(0)]
		public OldNoteEntriesV5.NoteEntry[] structs;
	}

	// Token: 0x02000AE9 RID: 2793
	public struct NoteStorageBlock
	{
		// Token: 0x0600336B RID: 13163 RVA: 0x000C5FC5 File Offset: 0x000C41C5
		public void Deserialize(BinaryReader reader)
		{
			this.entryCount = reader.ReadInt32();
			this.entries.bytes = reader.ReadBytes(this.entries.StructSizeInBytes * this.entryCount);
		}

		// Token: 0x0400233B RID: 9019
		public int entryCount;

		// Token: 0x0400233C RID: 9020
		public OldNoteEntriesV5.NoteEntryArray entries;
	}
}
