using System;

// Token: 0x020016EC RID: 5868
public class PassiveElementConsumer : ElementConsumer, IGameObjectEffectDescriptor
{
	// Token: 0x0600790B RID: 30987 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	protected override bool IsActive()
	{
		return true;
	}
}
