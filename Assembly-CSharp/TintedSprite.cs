using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C5B RID: 3163
[DebuggerDisplay("{name}")]
[Serializable]
public class TintedSprite : ISerializationCallbackReceiver
{
	// Token: 0x06003BC5 RID: 15301 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnAfterDeserialize()
	{
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x000CB12D File Offset: 0x000C932D
	public void OnBeforeSerialize()
	{
		if (this.sprite != null)
		{
			this.name = this.sprite.name;
		}
	}

	// Token: 0x0400296E RID: 10606
	[ReadOnly]
	public string name;

	// Token: 0x0400296F RID: 10607
	public Sprite sprite;

	// Token: 0x04002970 RID: 10608
	public Color color;
}
