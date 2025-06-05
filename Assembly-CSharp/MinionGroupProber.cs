using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001526 RID: 5414
[AddComponentMenu("KMonoBehaviour/scripts/MinionGroupProber")]
public class MinionGroupProber : KMonoBehaviour, IGroupProber, ISim200ms
{
	// Token: 0x0600708B RID: 28811 RVA: 0x000EE19E File Offset: 0x000EC39E
	public static void DestroyInstance()
	{
		MinionGroupProber.Instance = null;
	}

	// Token: 0x0600708C RID: 28812 RVA: 0x000EE1A6 File Offset: 0x000EC3A6
	public static MinionGroupProber Get()
	{
		return MinionGroupProber.Instance;
	}

	// Token: 0x0600708D RID: 28813 RVA: 0x00306058 File Offset: 0x00304258
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		MinionGroupProber.Instance = this;
		this.cells = new Dictionary<object, short>[Grid.CellCount];
		for (int i = 0; i < Grid.CellCount; i++)
		{
			this.cells[i] = new Dictionary<object, short>();
		}
		this.cell_cleanup_index = 0;
		this.cell_checks_per_frame = Grid.CellCount / 500;
	}

	// Token: 0x0600708E RID: 28814 RVA: 0x003060B8 File Offset: 0x003042B8
	public bool IsReachable(int cell)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (KeyValuePair<object, short> keyValuePair in this.cells[cell])
		{
			object key = keyValuePair.Key;
			short value = keyValuePair.Value;
			KeyValuePair<short, short> keyValuePair2;
			if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (value == keyValuePair2.Key || value == keyValuePair2.Value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600708F RID: 28815 RVA: 0x00306150 File Offset: 0x00304350
	public bool IsReachable(int cell, CellOffset[] offsets)
	{
		if (!Grid.IsValidCell(cell))
		{
			return false;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007090 RID: 28816 RVA: 0x00306194 File Offset: 0x00304394
	public bool IsAllReachable(int cell, CellOffset[] offsets)
	{
		if (this.IsReachable(cell))
		{
			return true;
		}
		foreach (CellOffset offset in offsets)
		{
			if (this.IsReachable(Grid.OffsetCell(cell, offset)))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007091 RID: 28817 RVA: 0x000EE1AD File Offset: 0x000EC3AD
	public bool IsReachable(Workable workable)
	{
		return this.IsReachable(Grid.PosToCell(workable), workable.GetOffsets());
	}

	// Token: 0x06007092 RID: 28818 RVA: 0x003061D8 File Offset: 0x003043D8
	public void Occupy(object prober, short serial_no, IEnumerable<int> cells)
	{
		foreach (int num in cells)
		{
			Dictionary<object, short> obj = this.cells[num];
			lock (obj)
			{
				this.cells[num][prober] = serial_no;
			}
		}
	}

	// Token: 0x06007093 RID: 28819 RVA: 0x00306254 File Offset: 0x00304454
	public void SetValidSerialNos(object prober, short previous_serial_no, short serial_no)
	{
		object obj = this.access;
		lock (obj)
		{
			this.valid_serial_nos[prober] = new KeyValuePair<short, short>(previous_serial_no, serial_no);
		}
	}

	// Token: 0x06007094 RID: 28820 RVA: 0x003062A4 File Offset: 0x003044A4
	public bool ReleaseProber(object prober)
	{
		object obj = this.access;
		bool result;
		lock (obj)
		{
			result = this.valid_serial_nos.Remove(prober);
		}
		return result;
	}

	// Token: 0x06007095 RID: 28821 RVA: 0x003062EC File Offset: 0x003044EC
	public void Sim200ms(float dt)
	{
		int i = 0;
		while (i < this.cell_checks_per_frame)
		{
			this.pending_removals.Clear();
			foreach (KeyValuePair<object, short> keyValuePair in this.cells[this.cell_cleanup_index])
			{
				KeyValuePair<short, short> keyValuePair2;
				if (!this.valid_serial_nos.TryGetValue(keyValuePair.Key, out keyValuePair2) || (keyValuePair2.Key != keyValuePair.Value && keyValuePair2.Value != keyValuePair.Value))
				{
					this.pending_removals.Add(keyValuePair.Key);
				}
			}
			foreach (object key in this.pending_removals)
			{
				this.cells[this.cell_cleanup_index].Remove(key);
			}
			i++;
			this.cell_cleanup_index = (this.cell_cleanup_index + 1) % this.cells.Length;
		}
	}

	// Token: 0x04005499 RID: 21657
	private static MinionGroupProber Instance;

	// Token: 0x0400549A RID: 21658
	private Dictionary<object, short>[] cells;

	// Token: 0x0400549B RID: 21659
	private Dictionary<object, KeyValuePair<short, short>> valid_serial_nos = new Dictionary<object, KeyValuePair<short, short>>();

	// Token: 0x0400549C RID: 21660
	private List<object> pending_removals = new List<object>();

	// Token: 0x0400549D RID: 21661
	private int cell_cleanup_index;

	// Token: 0x0400549E RID: 21662
	private int cell_checks_per_frame;

	// Token: 0x0400549F RID: 21663
	private readonly object access = new object();
}
