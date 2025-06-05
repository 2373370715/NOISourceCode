using System;

// Token: 0x02001895 RID: 6293
public class ScenePartitionerEntry
{
	// Token: 0x06008204 RID: 33284 RVA: 0x00348F54 File Offset: 0x00347154
	public ScenePartitionerEntry(string name, object obj, int x, int y, int width, int height, ScenePartitionerLayer layer, ScenePartitioner partitioner, Action<object> event_callback)
	{
		if (x < 0 || y < 0 || width >= 0)
		{
		}
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.layer = layer.layer;
		this.partitioner = partitioner;
		this.eventCallback = event_callback;
		this.obj = obj;
	}

	// Token: 0x06008205 RID: 33285 RVA: 0x000FA0AD File Offset: 0x000F82AD
	public void UpdatePosition(int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, this);
	}

	// Token: 0x06008206 RID: 33286 RVA: 0x000FA0BD File Offset: 0x000F82BD
	public void UpdatePosition(Extents e)
	{
		this.partitioner.UpdatePosition(e, this);
	}

	// Token: 0x06008207 RID: 33287 RVA: 0x000FA0CC File Offset: 0x000F82CC
	public void Release()
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(this);
		}
	}

	// Token: 0x040062EC RID: 25324
	public int x;

	// Token: 0x040062ED RID: 25325
	public int y;

	// Token: 0x040062EE RID: 25326
	public int width;

	// Token: 0x040062EF RID: 25327
	public int height;

	// Token: 0x040062F0 RID: 25328
	public int layer;

	// Token: 0x040062F1 RID: 25329
	public int queryId;

	// Token: 0x040062F2 RID: 25330
	public ScenePartitioner partitioner;

	// Token: 0x040062F3 RID: 25331
	public Action<object> eventCallback;

	// Token: 0x040062F4 RID: 25332
	public object obj;
}
