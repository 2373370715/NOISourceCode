using System;
using STRINGS;

// Token: 0x02000D59 RID: 3417
public class DetectorNetwork : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>
{
	// Token: 0x06004252 RID: 16978 RVA: 0x0024ED60 File Offset: 0x0024CF60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (DetectorNetwork.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.operational.InitializeStates(this).EventTransition(GameHashes.OperationalChanged, this.inoperational, (DetectorNetwork.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
	}

	// Token: 0x04002DC5 RID: 11717
	public StateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.FloatParameter networkQuality;

	// Token: 0x04002DC6 RID: 11718
	public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State inoperational;

	// Token: 0x04002DC7 RID: 11719
	public DetectorNetwork.NetworkStates operational;

	// Token: 0x02000D5A RID: 3418
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000D5B RID: 3419
	public class NetworkStates : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State
	{
		// Token: 0x06004255 RID: 16981 RVA: 0x0024EDE8 File Offset: 0x0024CFE8
		public DetectorNetwork.NetworkStates InitializeStates(DetectorNetwork parent)
		{
			base.DefaultState(this.poor);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state = this.poor;
			string name = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon = "";
			StatusItem.IconType icon_type = StatusItem.IconType.Exclamation;
			NotificationType notification_type = NotificationType.BadMinor;
			bool allow_multiples = false;
			Func<string, DetectorNetwork.Instance, string> resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.good, (DetectorNetwork.Instance smi, float p) => (double)p >= 0.8);
			GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State state2 = this.good;
			string name2 = BUILDING.STATUSITEMS.NETWORKQUALITY.NAME;
			string tooltip2 = BUILDING.STATUSITEMS.NETWORKQUALITY.TOOLTIP;
			string icon2 = "";
			StatusItem.IconType icon_type2 = StatusItem.IconType.Info;
			NotificationType notification_type2 = NotificationType.Neutral;
			bool allow_multiples2 = false;
			resolve_string_callback = new Func<string, DetectorNetwork.Instance, string>(this.StringCallback);
			state2.ToggleStatusItem(name2, tooltip2, icon2, icon_type2, notification_type2, allow_multiples2, default(HashedString), 129022, resolve_string_callback, null, null).ParamTransition<float>(parent.networkQuality, this.poor, (DetectorNetwork.Instance smi, float p) => (double)p < 0.8);
			return this;
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0024EEF0 File Offset: 0x0024D0F0
		private string StringCallback(string str, DetectorNetwork.Instance smi)
		{
			MathUtil.MinMax detectTimeRangeForWorld = Game.Instance.spaceScannerNetworkManager.GetDetectTimeRangeForWorld(smi.GetMyWorldId());
			float num = Game.Instance.spaceScannerNetworkManager.GetQualityForWorld(smi.GetMyWorldId());
			num = num.Remap(new ValueTuple<float, float>(0f, 1f), new ValueTuple<float, float>(0f, 0.5f));
			return str.Replace("{TotalQuality}", GameUtil.GetFormattedPercent(smi.GetNetworkQuality01() * 100f, GameUtil.TimeSlice.None)).Replace("{WorstTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.min, "F0")).Replace("{BestTime}", GameUtil.GetFormattedTime(detectTimeRangeForWorld.max, "F0")).Replace("{Coverage}", GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None));
		}

		// Token: 0x04002DC8 RID: 11720
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State poor;

		// Token: 0x04002DC9 RID: 11721
		public GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.State good;
	}

	// Token: 0x02000D5D RID: 3421
	public new class Instance : GameStateMachine<DetectorNetwork, DetectorNetwork.Instance, IStateMachineTarget, DetectorNetwork.Def>.GameInstance
	{
		// Token: 0x0600425C RID: 16988 RVA: 0x000CF609 File Offset: 0x000CD809
		public Instance(IStateMachineTarget master, DetectorNetwork.Def def) : base(master, def)
		{
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x000CF613 File Offset: 0x000CD813
		public override void StartSM()
		{
			this.worldId = base.master.gameObject.GetMyWorldId();
			Components.DetectorNetworks.Add(this.worldId, this);
			base.StartSM();
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x000CF642 File Offset: 0x000CD842
		public override void StopSM(string reason)
		{
			base.StopSM(reason);
			Components.DetectorNetworks.Remove(this.worldId, this);
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x000CF65C File Offset: 0x000CD85C
		public void Internal_SetNetworkQuality(float quality01)
		{
			base.sm.networkQuality.Set(quality01, base.smi, false);
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x000CF677 File Offset: 0x000CD877
		public float GetNetworkQuality01()
		{
			return base.sm.networkQuality.Get(base.smi);
		}

		// Token: 0x04002DCD RID: 11725
		[NonSerialized]
		private int worldId;
	}
}
