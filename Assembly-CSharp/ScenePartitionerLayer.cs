﻿using System;

public class ScenePartitionerLayer
{
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	public HashedString name;

	public int layer;

	public Action<int, object> OnEvent;
}
