using System;
using UnityEngine;

// Token: 0x020017EA RID: 6122
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/RequireInputs")]
public class RequireInputs : KMonoBehaviour, ISim200ms
{
	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06007DEC RID: 32236 RVA: 0x000F76F3 File Offset: 0x000F58F3
	public bool RequiresPower
	{
		get
		{
			return this.requirePower;
		}
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06007DED RID: 32237 RVA: 0x000F76FB File Offset: 0x000F58FB
	public bool RequiresInputConduit
	{
		get
		{
			return this.requireConduit;
		}
	}

	// Token: 0x06007DEE RID: 32238 RVA: 0x000F7703 File Offset: 0x000F5903
	public void SetRequirements(bool power, bool conduit)
	{
		this.requirePower = power;
		this.requireConduit = conduit;
	}

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06007DEF RID: 32239 RVA: 0x000F7713 File Offset: 0x000F5913
	public bool RequirementsMet
	{
		get
		{
			return this.requirementsMet;
		}
	}

	// Token: 0x06007DF0 RID: 32240 RVA: 0x000F771B File Offset: 0x000F591B
	protected override void OnPrefabInit()
	{
		this.Bind();
	}

	// Token: 0x06007DF1 RID: 32241 RVA: 0x000F7723 File Offset: 0x000F5923
	protected override void OnSpawn()
	{
		this.CheckRequirements(true);
		this.Bind();
	}

	// Token: 0x06007DF2 RID: 32242 RVA: 0x00334B5C File Offset: 0x00332D5C
	[ContextMenu("Bind")]
	private void Bind()
	{
		if (this.requirePower)
		{
			this.energy = base.GetComponent<IEnergyConsumer>();
			this.button = base.GetComponent<BuildingEnabledButton>();
		}
		if (this.requireConduit && !this.conduitConsumer)
		{
			this.conduitConsumer = base.GetComponent<ConduitConsumer>();
		}
	}

	// Token: 0x06007DF3 RID: 32243 RVA: 0x000F7732 File Offset: 0x000F5932
	public void Sim200ms(float dt)
	{
		this.CheckRequirements(false);
	}

	// Token: 0x06007DF4 RID: 32244 RVA: 0x00334BAC File Offset: 0x00332DAC
	private void CheckRequirements(bool forceEvent)
	{
		bool flag = true;
		bool flag2 = false;
		if (this.requirePower)
		{
			bool isConnected = this.energy.IsConnected;
			bool isPowered = this.energy.IsPowered;
			flag = (flag && isPowered && isConnected);
			bool show = this.VisualizeRequirement(RequireInputs.Requirements.NeedPower) && isConnected && !isPowered && (this.button == null || this.button.IsEnabled);
			bool show2 = this.VisualizeRequirement(RequireInputs.Requirements.NoWire) && !isConnected;
			this.needPowerStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NeedPower, this.needPowerStatusGuid, show, this);
			this.noWireStatusGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, this.noWireStatusGuid, show2, this);
			flag2 = (flag != this.RequirementsMet && base.GetComponent<Light2D>() != null);
		}
		if (this.requireConduit)
		{
			bool flag3 = !this.conduitConsumer.enabled || this.conduitConsumer.IsConnected;
			bool flag4 = !this.conduitConsumer.enabled || this.conduitConsumer.IsSatisfied;
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitConnected) && this.previouslyConnected != flag3)
			{
				this.previouslyConnected = flag3;
				StatusItem statusItem = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem = Db.Get().BuildingStatusItems.NeedLiquidIn;
					}
				}
				else
				{
					statusItem = Db.Get().BuildingStatusItems.NeedGasIn;
				}
				if (statusItem != null)
				{
					this.selectable.ToggleStatusItem(statusItem, !flag3, new global::Tuple<ConduitType, Tag>(this.conduitConsumer.TypeOfConduit, this.conduitConsumer.capacityTag));
				}
				this.operational.SetFlag(RequireInputs.inputConnectedFlag, flag3);
			}
			flag = (flag && flag3);
			if (this.VisualizeRequirement(RequireInputs.Requirements.ConduitEmpty) && this.previouslySatisfied != flag4)
			{
				this.previouslySatisfied = flag4;
				StatusItem statusItem2 = null;
				ConduitType typeOfConduit = this.conduitConsumer.TypeOfConduit;
				if (typeOfConduit != ConduitType.Gas)
				{
					if (typeOfConduit == ConduitType.Liquid)
					{
						statusItem2 = Db.Get().BuildingStatusItems.LiquidPipeEmpty;
					}
				}
				else
				{
					statusItem2 = Db.Get().BuildingStatusItems.GasPipeEmpty;
				}
				if (this.requireConduitHasMass)
				{
					if (statusItem2 != null)
					{
						this.selectable.ToggleStatusItem(statusItem2, !flag4, this);
					}
					this.operational.SetFlag(RequireInputs.pipesHaveMass, flag4);
				}
			}
		}
		this.requirementsMet = flag;
		if (flag2)
		{
			Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
			if (roomOfGameObject != null)
			{
				Game.Instance.roomProber.UpdateRoom(roomOfGameObject.cavity);
			}
		}
	}

	// Token: 0x06007DF5 RID: 32245 RVA: 0x000F773B File Offset: 0x000F593B
	public bool VisualizeRequirement(RequireInputs.Requirements r)
	{
		return (this.visualizeRequirements & r) == r;
	}

	// Token: 0x04005FA1 RID: 24481
	[SerializeField]
	private bool requirePower = true;

	// Token: 0x04005FA2 RID: 24482
	[SerializeField]
	private bool requireConduit;

	// Token: 0x04005FA3 RID: 24483
	public bool requireConduitHasMass = true;

	// Token: 0x04005FA4 RID: 24484
	public RequireInputs.Requirements visualizeRequirements = RequireInputs.Requirements.All;

	// Token: 0x04005FA5 RID: 24485
	private static readonly Operational.Flag inputConnectedFlag = new Operational.Flag("inputConnected", Operational.Flag.Type.Requirement);

	// Token: 0x04005FA6 RID: 24486
	private static readonly Operational.Flag pipesHaveMass = new Operational.Flag("pipesHaveMass", Operational.Flag.Type.Requirement);

	// Token: 0x04005FA7 RID: 24487
	private Guid noWireStatusGuid;

	// Token: 0x04005FA8 RID: 24488
	private Guid needPowerStatusGuid;

	// Token: 0x04005FA9 RID: 24489
	private bool requirementsMet;

	// Token: 0x04005FAA RID: 24490
	private BuildingEnabledButton button;

	// Token: 0x04005FAB RID: 24491
	private IEnergyConsumer energy;

	// Token: 0x04005FAC RID: 24492
	public ConduitConsumer conduitConsumer;

	// Token: 0x04005FAD RID: 24493
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005FAE RID: 24494
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04005FAF RID: 24495
	private bool previouslyConnected = true;

	// Token: 0x04005FB0 RID: 24496
	private bool previouslySatisfied = true;

	// Token: 0x020017EB RID: 6123
	[Flags]
	public enum Requirements
	{
		// Token: 0x04005FB2 RID: 24498
		None = 0,
		// Token: 0x04005FB3 RID: 24499
		NoWire = 1,
		// Token: 0x04005FB4 RID: 24500
		NeedPower = 2,
		// Token: 0x04005FB5 RID: 24501
		ConduitConnected = 4,
		// Token: 0x04005FB6 RID: 24502
		ConduitEmpty = 8,
		// Token: 0x04005FB7 RID: 24503
		AllPower = 3,
		// Token: 0x04005FB8 RID: 24504
		AllConduit = 12,
		// Token: 0x04005FB9 RID: 24505
		All = 15
	}
}
