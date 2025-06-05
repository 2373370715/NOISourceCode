using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x020018D6 RID: 6358
public abstract class SlicedUpdaterSim1000ms<T> : KMonoBehaviour, ISim200ms where T : KMonoBehaviour, ISlicedSim1000ms
{
	// Token: 0x06008378 RID: 33656 RVA: 0x000FAEE5 File Offset: 0x000F90E5
	protected override void OnPrefabInit()
	{
		this.InitializeSlices();
		base.OnPrefabInit();
		SlicedUpdaterSim1000ms<T>.instance = this;
	}

	// Token: 0x06008379 RID: 33657 RVA: 0x000FAEF9 File Offset: 0x000F90F9
	protected override void OnForcedCleanUp()
	{
		SlicedUpdaterSim1000ms<T>.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x0600837A RID: 33658 RVA: 0x0034F424 File Offset: 0x0034D624
	private void InitializeSlices()
	{
		int num = SlicedUpdaterSim1000ms<T>.NUM_200MS_BUCKETS * this.numSlicesPer200ms;
		this.m_slices = new List<SlicedUpdaterSim1000ms<T>.Slice>();
		for (int i = 0; i < num; i++)
		{
			this.m_slices.Add(new SlicedUpdaterSim1000ms<T>.Slice());
		}
		this.m_nextSliceIdx = 0;
	}

	// Token: 0x0600837B RID: 33659 RVA: 0x000FAF07 File Offset: 0x000F9107
	private int GetSliceIdx(T toBeUpdated)
	{
		return toBeUpdated.GetComponent<KPrefabID>().InstanceID % this.m_slices.Count;
	}

	// Token: 0x0600837C RID: 33660 RVA: 0x0034F46C File Offset: 0x0034D66C
	public void RegisterUpdate1000ms(T toBeUpdated)
	{
		SlicedUpdaterSim1000ms<T>.Slice slice = this.m_slices[this.GetSliceIdx(toBeUpdated)];
		slice.Register(toBeUpdated);
		DebugUtil.DevAssert(slice.Count < this.maxUpdatesPer200ms, string.Format("The SlicedUpdaterSim1000ms for {0} wants to update no more than {1} instances per 200ms tick, but a slice has grown more than the SlicedUpdaterSim1000ms can support.", typeof(T).Name, this.maxUpdatesPer200ms), null);
	}

	// Token: 0x0600837D RID: 33661 RVA: 0x000FAF25 File Offset: 0x000F9125
	public void UnregisterUpdate1000ms(T toBeUpdated)
	{
		this.m_slices[this.GetSliceIdx(toBeUpdated)].Unregister(toBeUpdated);
	}

	// Token: 0x0600837E RID: 33662 RVA: 0x0034F4CC File Offset: 0x0034D6CC
	public void Sim200ms(float dt)
	{
		foreach (SlicedUpdaterSim1000ms<T>.Slice slice in this.m_slices)
		{
			slice.IncrementDt(dt);
		}
		int num = 0;
		int i = 0;
		while (i < this.numSlicesPer200ms)
		{
			SlicedUpdaterSim1000ms<T>.Slice slice2 = this.m_slices[this.m_nextSliceIdx];
			num += slice2.Count;
			if (num > this.maxUpdatesPer200ms && i > 0)
			{
				break;
			}
			slice2.Update();
			i++;
			this.m_nextSliceIdx = (this.m_nextSliceIdx + 1) % this.m_slices.Count;
		}
	}

	// Token: 0x04006427 RID: 25639
	private static int NUM_200MS_BUCKETS = 5;

	// Token: 0x04006428 RID: 25640
	public static SlicedUpdaterSim1000ms<T> instance;

	// Token: 0x04006429 RID: 25641
	[Serialize]
	public int maxUpdatesPer200ms = 300;

	// Token: 0x0400642A RID: 25642
	[Serialize]
	public int numSlicesPer200ms = 3;

	// Token: 0x0400642B RID: 25643
	private List<SlicedUpdaterSim1000ms<T>.Slice> m_slices;

	// Token: 0x0400642C RID: 25644
	private int m_nextSliceIdx;

	// Token: 0x020018D7 RID: 6359
	private class Slice
	{
		// Token: 0x06008381 RID: 33665 RVA: 0x000FAF61 File Offset: 0x000F9161
		public void Register(T toBeUpdated)
		{
			if (this.m_timeSinceLastUpdate == 0f)
			{
				this.m_updateList.Add(toBeUpdated);
				return;
			}
			this.m_recentlyAdded[toBeUpdated] = 0f;
		}

		// Token: 0x06008382 RID: 33666 RVA: 0x000FAF8E File Offset: 0x000F918E
		public void Unregister(T toBeUpdated)
		{
			if (!this.m_updateList.Remove(toBeUpdated))
			{
				this.m_recentlyAdded.Remove(toBeUpdated);
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06008383 RID: 33667 RVA: 0x000FAFAB File Offset: 0x000F91AB
		public int Count
		{
			get
			{
				return this.m_updateList.Count + this.m_recentlyAdded.Count;
			}
		}

		// Token: 0x06008384 RID: 33668 RVA: 0x000FAFC4 File Offset: 0x000F91C4
		public List<T> GetUpdateList()
		{
			List<T> list = new List<T>();
			list.AddRange(this.m_updateList);
			list.AddRange(this.m_recentlyAdded.Keys);
			return list;
		}

		// Token: 0x06008385 RID: 33669 RVA: 0x0034F578 File Offset: 0x0034D778
		public void Update()
		{
			foreach (T t in this.m_updateList)
			{
				t.SlicedSim1000ms(this.m_timeSinceLastUpdate);
			}
			foreach (KeyValuePair<T, float> keyValuePair in this.m_recentlyAdded)
			{
				keyValuePair.Key.SlicedSim1000ms(keyValuePair.Value);
				this.m_updateList.Add(keyValuePair.Key);
			}
			this.m_recentlyAdded.Clear();
			this.m_timeSinceLastUpdate = 0f;
		}

		// Token: 0x06008386 RID: 33670 RVA: 0x0034F650 File Offset: 0x0034D850
		public void IncrementDt(float dt)
		{
			this.m_timeSinceLastUpdate += dt;
			if (this.m_recentlyAdded.Count > 0)
			{
				foreach (T t in new List<T>(this.m_recentlyAdded.Keys))
				{
					Dictionary<T, float> recentlyAdded = this.m_recentlyAdded;
					T key = t;
					recentlyAdded[key] += dt;
				}
			}
		}

		// Token: 0x0400642D RID: 25645
		private float m_timeSinceLastUpdate;

		// Token: 0x0400642E RID: 25646
		private List<T> m_updateList = new List<T>();

		// Token: 0x0400642F RID: 25647
		private Dictionary<T, float> m_recentlyAdded = new Dictionary<T, float>();
	}
}
