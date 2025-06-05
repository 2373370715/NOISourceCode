using System;

// Token: 0x020009A6 RID: 2470
public class Blueprints
{
	// Token: 0x06002C2D RID: 11309 RVA: 0x001EE2BC File Offset: 0x001EC4BC
	public static Blueprints Get()
	{
		if (Blueprints.instance == null)
		{
			Blueprints.instance = new Blueprints();
			Blueprints.instance.all.AddBlueprintsFrom<Blueprints_Default>(new Blueprints_Default());
			foreach (BlueprintProvider provider in Blueprints.instance.skinsReleaseProviders)
			{
				Blueprints.instance.skinsRelease.AddBlueprintsFrom<BlueprintProvider>(provider);
			}
			Blueprints.instance.all.AddBlueprintsFrom(Blueprints.instance.skinsRelease);
			Blueprints.instance.skinsRelease.PostProcess();
			Blueprints.instance.all.PostProcess();
		}
		return Blueprints.instance;
	}

	// Token: 0x04001E61 RID: 7777
	public BlueprintCollection all = new BlueprintCollection();

	// Token: 0x04001E62 RID: 7778
	public BlueprintCollection skinsRelease = new BlueprintCollection();

	// Token: 0x04001E63 RID: 7779
	public BlueprintProvider[] skinsReleaseProviders = new BlueprintProvider[]
	{
		new Blueprints_U51AndBefore(),
		new Blueprints_DlcPack2(),
		new Blueprints_U53(),
		new Blueprints_DlcPack3()
	};

	// Token: 0x04001E64 RID: 7780
	private static Blueprints instance;
}
