using System;

// Token: 0x0200197A RID: 6522
public class RoboPilotModule : KMonoBehaviour
{
	// Token: 0x060087D2 RID: 34770 RVA: 0x0036016C File Offset: 0x0035E36C
	protected override void OnSpawn()
	{
		this.databankStorage = base.GetComponent<Storage>();
		this.manualDeliveryChore = base.GetComponent<ManualDeliveryKG>();
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimTracker>().matchParentOffset = true;
		this.UpdateMeter(null);
		this.databankStorage.SetOffsets(RoboPilotModule.dataDeliveryOffsets);
		base.Subscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Subscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
		base.Subscribe(-887025858, new Action<object>(this.OnRocketLanded));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			component.CraftInterface.Subscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
			component.CraftInterface.Subscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
		}
		else
		{
			base.Subscribe(705820818, new Action<object>(this.OnRocketLaunched));
			base.GetComponent<RocketModule>().FindLaunchConditionManager().Subscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
		}
		this.RequestDataBanksForDestination(null);
	}

	// Token: 0x060087D3 RID: 34771 RVA: 0x003602D0 File Offset: 0x0035E4D0
	private void RequestDataBanksForDestination(object data = null)
	{
		int num = -1;
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			ClusterTraveler component2 = component.CraftInterface.GetComponent<ClusterTraveler>();
			if (component2 != null && component2.CurrentPath != null)
			{
				num = component2.RemainingTravelNodes() * 2;
			}
		}
		else
		{
			LaunchConditionManager launchConditionManager = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			if (launchConditionManager != null)
			{
				SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(launchConditionManager);
				if (spacecraftDestination != null)
				{
					num = spacecraftDestination.OneBasedDistance * 2;
				}
			}
		}
		if (num > 0 && !this.HasResourcesToMove(num))
		{
			this.manualDeliveryChore.refillMass = MathF.Min(this.ResourcesRequiredToMove(num), this.databankStorage.Capacity() - this.databankStorage.UnitsStored());
		}
	}

	// Token: 0x060087D4 RID: 34772 RVA: 0x00360384 File Offset: 0x0035E584
	protected override void OnCleanUp()
	{
		base.Unsubscribe(-1697596308, new Action<object>(this.UpdateMeter));
		base.Unsubscribe(-887025858, new Action<object>(this.OnRocketLanded));
		base.Unsubscribe(-778359855, new Action<object>(this.PlayDeliveryAnimation));
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null)
		{
			component.CraftInterface.Unsubscribe(1655598572, new Action<object>(this.OnLaunchConditionChanged));
			component.CraftInterface.Unsubscribe(543433792, new Action<object>(this.RequestDataBanksForDestination));
		}
		else
		{
			base.Unsubscribe(705820818, new Action<object>(this.OnRocketLaunched));
			base.GetComponent<RocketModule>().FindLaunchConditionManager().Unsubscribe(929158128, new Action<object>(this.RequestDataBanksForDestination));
		}
		base.OnCleanUp();
	}

	// Token: 0x060087D5 RID: 34773 RVA: 0x00360460 File Offset: 0x0035E660
	private void OnLaunchConditionChanged(object data)
	{
		RocketModuleCluster component = base.GetComponent<RocketModuleCluster>();
		if (component != null && component.CraftInterface.IsLaunchRequested())
		{
			component.CraftInterface.GetComponent<Clustercraft>().Launch(false);
		}
	}

	// Token: 0x060087D6 RID: 34774 RVA: 0x0036049C File Offset: 0x0035E69C
	private void OnRocketLanded(object o)
	{
		if (this.consumeDataBanksOnLand)
		{
			LaunchConditionManager lcm = base.GetComponent<RocketModule>().FindLaunchConditionManager();
			Spacecraft spacecraftFromLaunchConditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(lcm);
			float amount = Math.Min((float)(SpacecraftManager.instance.GetSpacecraftDestination(spacecraftFromLaunchConditionManager.id).OneBasedDistance * this.dataBankConsumption * 2), this.databankStorage.MassStored());
			this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, amount);
		}
		this.RequestDataBanksForDestination(null);
	}

	// Token: 0x060087D7 RID: 34775 RVA: 0x00360514 File Offset: 0x0035E714
	private void OnRocketLaunched(object o)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.Play("launch_pre", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("launch", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue("launch_pst", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060087D8 RID: 34776 RVA: 0x000FD78B File Offset: 0x000FB98B
	public void ConsumeDataBanksInFlight()
	{
		if (this.databankStorage != null)
		{
			this.databankStorage.ConsumeIgnoringDisease(DatabankHelper.TAG, (float)this.dataBankConsumption);
		}
	}

	// Token: 0x060087D9 RID: 34777 RVA: 0x00360578 File Offset: 0x0035E778
	private void PlayDeliveryAnimation(object data = null)
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		HashedString currentAnim = component.currentAnim;
		component.Play("databank_delivery_reaction", KAnim.PlayMode.Once, 1f, 0f);
		component.Queue(currentAnim, KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060087DA RID: 34778 RVA: 0x000FD7B2 File Offset: 0x000FB9B2
	private void UpdateMeter(object data = null)
	{
		this.meter.SetPositionPercent(this.databankStorage.MassStored() / this.databankStorage.Capacity());
	}

	// Token: 0x060087DB RID: 34779 RVA: 0x000FD7D6 File Offset: 0x000FB9D6
	public bool HasResourcesToMove(int distance)
	{
		return this.databankStorage.UnitsStored() >= (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x060087DC RID: 34780 RVA: 0x000FD7F1 File Offset: 0x000FB9F1
	public float ResourcesRequiredToMove(int distance)
	{
		return (float)(distance * this.dataBankConsumption);
	}

	// Token: 0x060087DD RID: 34781 RVA: 0x000FD7FC File Offset: 0x000FB9FC
	public bool IsFull()
	{
		return this.databankStorage.MassStored() >= this.databankStorage.Capacity();
	}

	// Token: 0x060087DE RID: 34782 RVA: 0x000FD819 File Offset: 0x000FBA19
	public float GetDataBanksStored()
	{
		if (!(this.databankStorage != null))
		{
			return 0f;
		}
		return this.databankStorage.UnitsStored();
	}

	// Token: 0x060087DF RID: 34783 RVA: 0x003605C0 File Offset: 0x0035E7C0
	public float GetDataBankRange()
	{
		if (this.databankStorage == null)
		{
			return 0f;
		}
		if (this.consumeDataBanksOnLand)
		{
			return this.databankStorage.UnitsStored() / (float)this.dataBankConsumption * RoboPilotCommandModuleConfig.DATABANKRANGE;
		}
		return this.databankStorage.UnitsStored() / (float)this.dataBankConsumption * 600f;
	}

	// Token: 0x040066DF RID: 26335
	private MeterController meter;

	// Token: 0x040066E0 RID: 26336
	private Storage databankStorage;

	// Token: 0x040066E1 RID: 26337
	private ManualDeliveryKG manualDeliveryChore;

	// Token: 0x040066E2 RID: 26338
	public int dataBankConsumption = 2;

	// Token: 0x040066E3 RID: 26339
	public bool consumeDataBanksOnLand;

	// Token: 0x040066E4 RID: 26340
	private static CellOffset[] dataDeliveryOffsets = new CellOffset[]
	{
		new CellOffset(0, 0),
		new CellOffset(1, 0),
		new CellOffset(2, 0),
		new CellOffset(3, 0),
		new CellOffset(-1, 0),
		new CellOffset(-2, 0),
		new CellOffset(-3, 0)
	};
}
