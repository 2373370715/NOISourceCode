using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200178F RID: 6031
[AddComponentMenu("KMonoBehaviour/Workable/RelaxationPoint")]
public class RelaxationPoint : Workable, IGameObjectEffectDescriptor
{
	// Token: 0x06007C10 RID: 31760 RVA: 0x000F6085 File Offset: 0x000F4285
	public RelaxationPoint()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
	}

	// Token: 0x06007C11 RID: 31761 RVA: 0x0032CB74 File Offset: 0x0032AD74
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.lightEfficiencyBonus = false;
		base.GetComponent<KPrefabID>().AddTag(TagManager.Create("RelaxationPoint", MISC.TAGS.RELAXATION_POINT), false);
		if (RelaxationPoint.stressReductionEffect == null)
		{
			RelaxationPoint.stressReductionEffect = this.CreateEffect();
			RelaxationPoint.roomStressReductionEffect = this.CreateRoomEffect();
		}
	}

	// Token: 0x06007C12 RID: 31762 RVA: 0x0032CBCC File Offset: 0x0032ADCC
	public Effect CreateEffect()
	{
		Effect effect = new Effect("StressReduction", DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.stressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06007C13 RID: 31763 RVA: 0x0032CC50 File Offset: 0x0032AE50
	public Effect CreateRoomEffect()
	{
		Effect effect = new Effect("RoomRelaxationEffect", DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.TOOLTIP, 0f, true, false, false, null, -1f, 0f, null, "");
		AttributeModifier modifier = new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, this.roomStressModificationValue / 600f, DUPLICANTS.MODIFIERS.STRESSREDUCTION_CLINIC.NAME, false, false, true);
		effect.Add(modifier);
		return effect;
	}

	// Token: 0x06007C14 RID: 31764 RVA: 0x000F609C File Offset: 0x000F429C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.smi = new RelaxationPoint.RelaxationPointSM.Instance(this);
		this.smi.StartSM();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x06007C15 RID: 31765 RVA: 0x0032CCD4 File Offset: 0x0032AED4
	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
		if (this.roomTracker != null && this.roomTracker.room != null && this.roomTracker.room.roomType == Db.Get().RoomTypes.MassageClinic)
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.roomStressReductionEffect, false);
		}
		else
		{
			worker.GetComponent<Effects>().Add(RelaxationPoint.stressReductionEffect, false);
		}
		base.GetComponent<Operational>().SetActive(true, false);
	}

	// Token: 0x06007C16 RID: 31766 RVA: 0x000F60C6 File Offset: 0x000F42C6
	protected override bool OnWorkTick(WorkerBase worker, float dt)
	{
		if (Db.Get().Amounts.Stress.Lookup(worker.gameObject).value <= this.stopStressingValue)
		{
			return true;
		}
		base.OnWorkTick(worker, dt);
		return false;
	}

	// Token: 0x06007C17 RID: 31767 RVA: 0x000F60FB File Offset: 0x000F42FB
	protected override void OnStopWork(WorkerBase worker)
	{
		worker.GetComponent<Effects>().Remove(RelaxationPoint.stressReductionEffect);
		worker.GetComponent<Effects>().Remove(RelaxationPoint.roomStressReductionEffect);
		base.GetComponent<Operational>().SetActive(false, false);
		base.OnStopWork(worker);
	}

	// Token: 0x06007C18 RID: 31768 RVA: 0x000D73E7 File Offset: 0x000D55E7
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
	}

	// Token: 0x06007C19 RID: 31769 RVA: 0x000B1628 File Offset: 0x000AF828
	public override bool InstantlyFinish(WorkerBase worker)
	{
		return false;
	}

	// Token: 0x06007C1A RID: 31770 RVA: 0x0032CD58 File Offset: 0x0032AF58
	protected virtual WorkChore<RelaxationPoint> CreateWorkChore()
	{
		return new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.Relax, this, null, false, null, null, null, false, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
	}

	// Token: 0x06007C1B RID: 31771 RVA: 0x0032CD8C File Offset: 0x0032AF8C
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(string.Format(UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, GameUtil.GetFormattedPercent(this.stressModificationValue / 600f * 60f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x04005D82 RID: 23938
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04005D83 RID: 23939
	[Serialize]
	protected float stopStressingValue;

	// Token: 0x04005D84 RID: 23940
	public float stressModificationValue;

	// Token: 0x04005D85 RID: 23941
	public float roomStressModificationValue;

	// Token: 0x04005D86 RID: 23942
	private RelaxationPoint.RelaxationPointSM.Instance smi;

	// Token: 0x04005D87 RID: 23943
	private static Effect stressReductionEffect;

	// Token: 0x04005D88 RID: 23944
	private static Effect roomStressReductionEffect;

	// Token: 0x02001790 RID: 6032
	public class RelaxationPointSM : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint>
	{
		// Token: 0x06007C1C RID: 31772 RVA: 0x0032CE08 File Offset: 0x0032B008
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.unoperational;
			this.unoperational.EventTransition(GameHashes.OperationalChanged, this.operational, (RelaxationPoint.RelaxationPointSM.Instance smi) => smi.GetComponent<Operational>().IsOperational).PlayAnim("off");
			this.operational.ToggleChore((RelaxationPoint.RelaxationPointSM.Instance smi) => smi.master.CreateWorkChore(), this.unoperational);
		}

		// Token: 0x04005D89 RID: 23945
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State unoperational;

		// Token: 0x04005D8A RID: 23946
		public GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.State operational;

		// Token: 0x02001791 RID: 6033
		public new class Instance : GameStateMachine<RelaxationPoint.RelaxationPointSM, RelaxationPoint.RelaxationPointSM.Instance, RelaxationPoint, object>.GameInstance
		{
			// Token: 0x06007C1E RID: 31774 RVA: 0x000F6139 File Offset: 0x000F4339
			public Instance(RelaxationPoint master) : base(master)
			{
			}
		}
	}
}
