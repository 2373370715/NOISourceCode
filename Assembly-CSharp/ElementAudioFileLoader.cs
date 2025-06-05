using System;

// Token: 0x02000C61 RID: 3169
internal class ElementAudioFileLoader : AsyncCsvLoader<ElementAudioFileLoader, ElementsAudio.ElementAudioConfig>
{
	// Token: 0x06003BFF RID: 15359 RVA: 0x000CB2D2 File Offset: 0x000C94D2
	public ElementAudioFileLoader() : base(Assets.instance.elementAudio)
	{
	}

	// Token: 0x06003C00 RID: 15360 RVA: 0x000CB2E4 File Offset: 0x000C94E4
	public override void Run()
	{
		base.Run();
	}
}
