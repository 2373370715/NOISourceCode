﻿using System;

public class ScenePartitionerEntry
{
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

	public void UpdatePosition(int x, int y)
	{
		this.partitioner.UpdatePosition(x, y, this);
	}

	public void UpdatePosition(Extents e)
	{
		this.partitioner.UpdatePosition(e, this);
	}

	public void Release()
	{
		if (this.partitioner != null)
		{
			this.partitioner.Remove(this);
		}
	}

	public int x;

	public int y;

	public int width;

	public int height;

	public int layer;

	public int queryId;

	public ScenePartitioner partitioner;

	public Action<object> eventCallback;

	public object obj;
}
