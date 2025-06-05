using System;

// Token: 0x020000F5 RID: 245
internal class OilFloaterMovementSound : KMonoBehaviour
{
	// Token: 0x060003D7 RID: 983 RVA: 0x0015B548 File Offset: 0x00159748
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.sound = GlobalAssets.GetSound(this.sound, false);
		base.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged), "OilFloaterMovementSound");
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0015B5A0 File Offset: 0x001597A0
	private void OnObjectMovementStateChanged(object data)
	{
		GameHashes gameHashes = (GameHashes)data;
		this.isMoving = (gameHashes == GameHashes.ObjectMovementWakeUp);
		this.UpdateSound();
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x000AB4FD File Offset: 0x000A96FD
	private void OnCellChanged()
	{
		this.UpdateSound();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0015B5C8 File Offset: 0x001597C8
	private void UpdateSound()
	{
		bool flag = this.isMoving && base.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
		if (flag == this.isPlayingSound)
		{
			return;
		}
		LoopingSounds component = base.GetComponent<LoopingSounds>();
		if (flag)
		{
			component.StartSound(this.sound);
		}
		else
		{
			component.StopSound(this.sound);
		}
		this.isPlayingSound = flag;
	}

	// Token: 0x060003DB RID: 987 RVA: 0x000AB505 File Offset: 0x000A9705
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChanged));
	}

	// Token: 0x040002AE RID: 686
	public string sound;

	// Token: 0x040002AF RID: 687
	public bool isPlayingSound;

	// Token: 0x040002B0 RID: 688
	public bool isMoving;

	// Token: 0x040002B1 RID: 689
	private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>(delegate(OilFloaterMovementSound component, object data)
	{
		component.OnObjectMovementStateChanged(data);
	});
}
