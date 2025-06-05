using System;

// Token: 0x02000DE0 RID: 3552
public class GeoTunerSwitchGeyserWorkable : Workable
{
	// Token: 0x06004539 RID: 17721 RVA: 0x000D1352 File Offset: 0x000CF552
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_remote_kanim")
		};
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x000D1386 File Offset: 0x000CF586
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(3f);
	}

	// Token: 0x04003026 RID: 12326
	private const string animName = "anim_use_remote_kanim";
}
