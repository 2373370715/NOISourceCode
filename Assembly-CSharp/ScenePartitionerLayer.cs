using System;

// Token: 0x02001896 RID: 6294
public class ScenePartitionerLayer
{
	// Token: 0x06008208 RID: 33288 RVA: 0x000FA0E2 File Offset: 0x000F82E2
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	// Token: 0x040062F5 RID: 25333
	public HashedString name;

	// Token: 0x040062F6 RID: 25334
	public int layer;

	// Token: 0x040062F7 RID: 25335
	public Action<int, object> OnEvent;
}
