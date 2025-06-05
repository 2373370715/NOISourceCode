using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000F59 RID: 3929
[SerializationConfig(MemberSerialization.OptIn)]
public class Polymerizer : StateMachineComponent<Polymerizer.StatesInstance>
{
	// Token: 0x06004EBC RID: 20156 RVA: 0x00277600 File Offset: 0x00275800
	protected override void OnSpawn()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		this.plasticMeter = new MeterController(component, "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		this.oilMeter = new MeterController(component, "meter2_target", "meter2", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new Vector3(0f, 0f, 0f), null);
		component.SetSymbolVisiblity("meter_target", true);
		this.UpdateOilMeter();
		base.smi.StartSM();
		base.Subscribe<Polymerizer>(-1697596308, Polymerizer.OnStorageChangedDelegate);
	}

	// Token: 0x06004EBD RID: 20157 RVA: 0x002776A4 File Offset: 0x002758A4
	private void TryEmit()
	{
		GameObject gameObject = this.storage.FindFirst(this.emitTag);
		if (gameObject != null)
		{
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			this.UpdatePercentDone(component);
			this.TryEmit(component);
		}
	}

	// Token: 0x06004EBE RID: 20158 RVA: 0x002776E4 File Offset: 0x002758E4
	private void TryEmit(PrimaryElement primary_elem)
	{
		if (primary_elem.Mass >= this.emitMass)
		{
			this.plasticMeter.SetPositionPercent(0f);
			GameObject gameObject = this.storage.Drop(primary_elem.gameObject, true);
			Rotatable component = base.GetComponent<Rotatable>();
			Vector3 vector = component.transform.GetPosition() + component.GetRotatedOffset(this.emitOffset);
			int i = Grid.PosToCell(vector);
			if (Grid.Solid[i])
			{
				vector += component.GetRotatedOffset(Vector3.left);
			}
			gameObject.transform.SetPosition(vector);
			PrimaryElement primaryElement = this.storage.FindPrimaryElement(this.exhaustElement);
			if (primaryElement != null)
			{
				SimMessages.AddRemoveSubstance(Grid.PosToCell(vector), primaryElement.ElementID, null, primaryElement.Mass, primaryElement.Temperature, primaryElement.DiseaseIdx, primaryElement.DiseaseCount, true, -1);
				primaryElement.Mass = 0f;
				primaryElement.ModifyDiseaseCount(int.MinValue, "Polymerizer.Exhaust");
			}
		}
	}

	// Token: 0x06004EBF RID: 20159 RVA: 0x002777DC File Offset: 0x002759DC
	private void UpdatePercentDone(PrimaryElement primary_elem)
	{
		float positionPercent = Mathf.Clamp01(primary_elem.Mass / this.emitMass);
		this.plasticMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x06004EC0 RID: 20160 RVA: 0x00277808 File Offset: 0x00275A08
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		if (gameObject == null)
		{
			return;
		}
		if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
		{
			this.UpdateOilMeter();
		}
	}

	// Token: 0x06004EC1 RID: 20161 RVA: 0x0027783C File Offset: 0x00275A3C
	private void UpdateOilMeter()
	{
		float num = 0f;
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject.HasTag(PolymerizerConfig.INPUT_ELEMENT_TAG))
			{
				PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
				num += component.Mass;
			}
		}
		float positionPercent = Mathf.Clamp01(num / this.consumer.capacityKG);
		this.oilMeter.SetPositionPercent(positionPercent);
	}

	// Token: 0x0400373F RID: 14143
	[SerializeField]
	public float maxMass = 2.5f;

	// Token: 0x04003740 RID: 14144
	[SerializeField]
	public float emitMass = 1f;

	// Token: 0x04003741 RID: 14145
	[SerializeField]
	public Tag emitTag;

	// Token: 0x04003742 RID: 14146
	[SerializeField]
	public Vector3 emitOffset = Vector3.zero;

	// Token: 0x04003743 RID: 14147
	[SerializeField]
	public SimHashes exhaustElement = SimHashes.Vacuum;

	// Token: 0x04003744 RID: 14148
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04003745 RID: 14149
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04003746 RID: 14150
	[MyCmpGet]
	private ConduitConsumer consumer;

	// Token: 0x04003747 RID: 14151
	[MyCmpGet]
	private ElementConverter converter;

	// Token: 0x04003748 RID: 14152
	private MeterController plasticMeter;

	// Token: 0x04003749 RID: 14153
	private MeterController oilMeter;

	// Token: 0x0400374A RID: 14154
	private static readonly EventSystem.IntraObjectHandler<Polymerizer> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<Polymerizer>(delegate(Polymerizer component, object data)
	{
		component.OnStorageChanged(data);
	});

	// Token: 0x02000F5A RID: 3930
	public class StatesInstance : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.GameInstance
	{
		// Token: 0x06004EC4 RID: 20164 RVA: 0x000D79A8 File Offset: 0x000D5BA8
		public StatesInstance(Polymerizer smi) : base(smi)
		{
		}
	}

	// Token: 0x02000F5B RID: 3931
	public class States : GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer>
	{
		// Token: 0x06004EC5 RID: 20165 RVA: 0x002778D4 File Offset: 0x00275AD4
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.EventTransition(GameHashes.OperationalChanged, this.off, (Polymerizer.StatesInstance smi) => !smi.master.operational.IsOperational);
			this.off.EventTransition(GameHashes.OperationalChanged, this.on, (Polymerizer.StatesInstance smi) => smi.master.operational.IsOperational);
			this.on.EventTransition(GameHashes.OnStorageChange, this.converting, (Polymerizer.StatesInstance smi) => smi.master.converter.CanConvertAtAll());
			this.converting.Enter("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(true, false);
			}).EventHandler(GameHashes.OnStorageChange, delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.TryEmit();
			}).EventTransition(GameHashes.OnStorageChange, this.on, (Polymerizer.StatesInstance smi) => !smi.master.converter.CanConvertAtAll()).Exit("Ready", delegate(Polymerizer.StatesInstance smi)
			{
				smi.master.operational.SetActive(false, false);
			});
		}

		// Token: 0x0400374B RID: 14155
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State off;

		// Token: 0x0400374C RID: 14156
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State on;

		// Token: 0x0400374D RID: 14157
		public GameStateMachine<Polymerizer.States, Polymerizer.StatesInstance, Polymerizer, object>.State converting;
	}
}
