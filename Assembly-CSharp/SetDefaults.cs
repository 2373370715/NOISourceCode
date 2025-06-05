using System;

// Token: 0x020018B3 RID: 6323
public class SetDefaults
{
	// Token: 0x06008298 RID: 33432 RVA: 0x0034B028 File Offset: 0x00349228
	public static void Initialize()
	{
		KSlider.DefaultSounds[0] = GlobalAssets.GetSound("Slider_Start", false);
		KSlider.DefaultSounds[1] = GlobalAssets.GetSound("Slider_Move", false);
		KSlider.DefaultSounds[2] = GlobalAssets.GetSound("Slider_End", false);
		KSlider.DefaultSounds[3] = GlobalAssets.GetSound("Slider_Boundary_Low", false);
		KSlider.DefaultSounds[4] = GlobalAssets.GetSound("Slider_Boundary_High", false);
		KScrollRect.DefaultSounds[KScrollRect.SoundType.OnMouseScroll] = GlobalAssets.GetSound("Mousewheel_Move", false);
		WidgetSoundPlayer.getSoundPath = new Func<string, string>(SetDefaults.GetSoundPath);
	}

	// Token: 0x06008299 RID: 33433 RVA: 0x000FA660 File Offset: 0x000F8860
	private static string GetSoundPath(string sound_name)
	{
		return GlobalAssets.GetSound(sound_name, false);
	}
}
