using System;
using Database;
using UnityEngine;

// Token: 0x020018D1 RID: 6353
public class SkyVisibilityMonitor : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>
{
	// Token: 0x0600835F RID: 33631 RVA: 0x000FAD9E File Offset: 0x000F8F9E
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Update(new Action<SkyVisibilityMonitor.Instance, float>(SkyVisibilityMonitor.CheckSkyVisibility), UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x06008360 RID: 33632 RVA: 0x0034EF34 File Offset: 0x0034D134
	public static void CheckSkyVisibility(SkyVisibilityMonitor.Instance smi, float dt)
	{
		bool hasSkyVisibility = smi.HasSkyVisibility;
		ValueTuple<bool, float> visibilityOf = smi.def.skyVisibilityInfo.GetVisibilityOf(smi.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		smi.Internal_SetPercentClearSky(item2);
		KSelectable component = smi.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, smi);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, smi);
		if (hasSkyVisibility == item)
		{
			return;
		}
		smi.TriggerVisibilityChange();
	}

	// Token: 0x020018D2 RID: 6354
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006413 RID: 25619
		public SkyVisibilityInfo skyVisibilityInfo;
	}

	// Token: 0x020018D3 RID: 6355
	public new class Instance : GameStateMachine<SkyVisibilityMonitor, SkyVisibilityMonitor.Instance, IStateMachineTarget, SkyVisibilityMonitor.Def>.GameInstance, BuildingStatusItems.ISkyVisInfo
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06008363 RID: 33635 RVA: 0x000FADCA File Offset: 0x000F8FCA
		public bool HasSkyVisibility
		{
			get
			{
				return this.PercentClearSky > 0f && !Mathf.Approximately(0f, this.PercentClearSky);
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06008364 RID: 33636 RVA: 0x000FADEE File Offset: 0x000F8FEE
		public float PercentClearSky
		{
			get
			{
				return this.percentClearSky01;
			}
		}

		// Token: 0x06008365 RID: 33637 RVA: 0x000FADF6 File Offset: 0x000F8FF6
		public void Internal_SetPercentClearSky(float percent01)
		{
			this.percentClearSky01 = percent01;
		}

		// Token: 0x06008366 RID: 33638 RVA: 0x000FADEE File Offset: 0x000F8FEE
		float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
		{
			return this.percentClearSky01;
		}

		// Token: 0x06008367 RID: 33639 RVA: 0x000FADFF File Offset: 0x000F8FFF
		public Instance(IStateMachineTarget master, SkyVisibilityMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008368 RID: 33640 RVA: 0x000FAE09 File Offset: 0x000F9009
		public override void StartSM()
		{
			base.StartSM();
			SkyVisibilityMonitor.CheckSkyVisibility(this, 0f);
			this.TriggerVisibilityChange();
		}

		// Token: 0x06008369 RID: 33641 RVA: 0x0034EFC0 File Offset: 0x0034D1C0
		public void TriggerVisibilityChange()
		{
			if (this.visibilityStatusItem != null)
			{
				base.smi.GetComponent<KSelectable>().ToggleStatusItem(this.visibilityStatusItem, !this.HasSkyVisibility, this);
			}
			base.smi.GetComponent<Operational>().SetFlag(SkyVisibilityMonitor.Instance.skyVisibilityFlag, this.HasSkyVisibility);
			if (this.SkyVisibilityChanged != null)
			{
				this.SkyVisibilityChanged();
			}
		}

		// Token: 0x04006414 RID: 25620
		private float percentClearSky01;

		// Token: 0x04006415 RID: 25621
		public System.Action SkyVisibilityChanged;

		// Token: 0x04006416 RID: 25622
		private StatusItem visibilityStatusItem;

		// Token: 0x04006417 RID: 25623
		private static readonly Operational.Flag skyVisibilityFlag = new Operational.Flag("sky visibility", Operational.Flag.Type.Requirement);
	}
}
