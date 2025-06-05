using System;
using System.Collections.Generic;

// Token: 0x0200061B RID: 1563
public class HashMapObjectPool<PoolKey, PoolValue>
{
	// Token: 0x06001BC3 RID: 7107 RVA: 0x000B68F3 File Offset: 0x000B4AF3
	public HashMapObjectPool(Func<PoolKey, PoolValue> instantiator, int initialCount = 0)
	{
		this.initialCount = initialCount;
		this.instantiator = instantiator;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x001B7F5C File Offset: 0x001B615C
	public HashMapObjectPool(HashMapObjectPool<PoolKey, PoolValue>.IPoolDescriptor[] descriptors, int initialCount = 0)
	{
		this.initialCount = initialCount;
		for (int i = 0; i < descriptors.Length; i++)
		{
			if (this.objectPoolMap.ContainsKey(descriptors[i].PoolId))
			{
				Debug.LogWarning(string.Format("HshMapObjectPool alaready contains key of {0}! Skipping!", descriptors[i].PoolId));
			}
			else
			{
				this.objectPoolMap[descriptors[i].PoolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(descriptors[i].GetInstance), initialCount);
			}
		}
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x001B7FEC File Offset: 0x001B61EC
	public PoolValue GetInstance(PoolKey poolId)
	{
		ObjectPool<PoolValue> objectPool;
		if (!this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			objectPool = (this.objectPoolMap[poolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(this.PoolInstantiator), this.initialCount));
		}
		this.currentPoolId = poolId;
		return objectPool.GetInstance();
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x001B8040 File Offset: 0x001B6240
	public void ReleaseInstance(PoolKey poolId, PoolValue inst)
	{
		ObjectPool<PoolValue> objectPool;
		if (inst == null || !this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			return;
		}
		objectPool.ReleaseInstance(inst);
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x001B8070 File Offset: 0x001B6270
	private PoolValue PoolInstantiator()
	{
		if (this.instantiator == null)
		{
			return default(PoolValue);
		}
		return this.instantiator(this.currentPoolId);
	}

	// Token: 0x040011C8 RID: 4552
	private Dictionary<PoolKey, ObjectPool<PoolValue>> objectPoolMap = new Dictionary<PoolKey, ObjectPool<PoolValue>>();

	// Token: 0x040011C9 RID: 4553
	private int initialCount;

	// Token: 0x040011CA RID: 4554
	private PoolKey currentPoolId;

	// Token: 0x040011CB RID: 4555
	private Func<PoolKey, PoolValue> instantiator;

	// Token: 0x0200061C RID: 1564
	public interface IPoolDescriptor
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06001BC8 RID: 7112
		PoolKey PoolId { get; }

		// Token: 0x06001BC9 RID: 7113
		PoolValue GetInstance();
	}
}
