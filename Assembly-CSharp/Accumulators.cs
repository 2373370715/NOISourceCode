using System;
using System.Collections.Generic;

// Token: 0x02000C3F RID: 3135
public class Accumulators
{
	// Token: 0x06003B40 RID: 15168 RVA: 0x000CAAFD File Offset: 0x000C8CFD
	public Accumulators()
	{
		this.elapsedTime = 0f;
		this.accumulated = new KCompactedVector<float>(0);
		this.average = new KCompactedVector<float>(0);
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x000CAB28 File Offset: 0x000C8D28
	public HandleVector<int>.Handle Add(string name, KMonoBehaviour owner)
	{
		HandleVector<int>.Handle result = this.accumulated.Allocate(0f);
		this.average.Allocate(0f);
		return result;
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x000CAB4B File Offset: 0x000C8D4B
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return HandleVector<int>.InvalidHandle;
		}
		this.accumulated.Free(handle);
		this.average.Free(handle);
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x00237F84 File Offset: 0x00236184
	public void Sim200ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 3f)
		{
			return;
		}
		this.elapsedTime -= 3f;
		List<float> dataList = this.accumulated.GetDataList();
		List<float> dataList2 = this.average.GetDataList();
		int count = dataList.Count;
		float num = 0.33333334f;
		for (int i = 0; i < count; i++)
		{
			dataList2[i] = dataList[i] * num;
			dataList[i] = 0f;
		}
	}

	// Token: 0x06003B44 RID: 15172 RVA: 0x000CAB7B File Offset: 0x000C8D7B
	public float GetAverageRate(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return 0f;
		}
		return this.average.GetData(handle);
	}

	// Token: 0x06003B45 RID: 15173 RVA: 0x00238014 File Offset: 0x00236214
	public void Accumulate(HandleVector<int>.Handle handle, float amount)
	{
		float data = this.accumulated.GetData(handle);
		this.accumulated.SetData(handle, data + amount);
	}

	// Token: 0x04002900 RID: 10496
	private const float TIME_WINDOW = 3f;

	// Token: 0x04002901 RID: 10497
	private float elapsedTime;

	// Token: 0x04002902 RID: 10498
	private KCompactedVector<float> accumulated;

	// Token: 0x04002903 RID: 10499
	private KCompactedVector<float> average;
}
