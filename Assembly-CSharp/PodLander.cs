using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001970 RID: 6512
[SerializationConfig(MemberSerialization.OptIn)]
public class PodLander : StateMachineComponent<PodLander.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x060087A5 RID: 34725 RVA: 0x000FD588 File Offset: 0x000FB788
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x060087A6 RID: 34726 RVA: 0x0035F634 File Offset: 0x0035D834
	public void ReleaseAstronaut()
	{
		if (this.releasingAstronaut)
		{
			return;
		}
		this.releasingAstronaut = true;
		MinionStorage component = base.GetComponent<MinionStorage>();
		List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
		for (int i = storedMinionInfo.Count - 1; i >= 0; i--)
		{
			MinionStorage.Info info = storedMinionInfo[i];
			component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(base.smi.master.transform.GetPosition())));
		}
		this.releasingAstronaut = false;
	}

	// Token: 0x060087A7 RID: 34727 RVA: 0x000AA765 File Offset: 0x000A8965
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return null;
	}

	// Token: 0x040066BE RID: 26302
	[Serialize]
	private int landOffLocation;

	// Token: 0x040066BF RID: 26303
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x040066C0 RID: 26304
	private float rocketSpeed;

	// Token: 0x040066C1 RID: 26305
	public float exhaustEmitRate = 2f;

	// Token: 0x040066C2 RID: 26306
	public float exhaustTemperature = 1000f;

	// Token: 0x040066C3 RID: 26307
	public SimHashes exhaustElement = SimHashes.CarbonDioxide;

	// Token: 0x040066C4 RID: 26308
	private GameObject soundSpeakerObject;

	// Token: 0x040066C5 RID: 26309
	private bool releasingAstronaut;

	// Token: 0x02001971 RID: 6513
	public class StatesInstance : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.GameInstance
	{
		// Token: 0x060087A9 RID: 34729 RVA: 0x000FD5C4 File Offset: 0x000FB7C4
		public StatesInstance(PodLander master) : base(master)
		{
		}
	}

	// Token: 0x02001972 RID: 6514
	public class States : GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander>
	{
		// Token: 0x060087AA RID: 34730 RVA: 0x0035F6B0 File Offset: 0x0035D8B0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.landing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.landing.PlayAnim("launch_loop", KAnim.PlayMode.Loop).Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.flightAnimOffset = 50f;
			}).Update(delegate(PodLander.StatesInstance smi, float dt)
			{
				float num = 10f;
				smi.master.rocketSpeed = num - Mathf.Clamp(Mathf.Pow(smi.timeinstate / 3.5f, 4f), 0f, num - 2f);
				smi.master.flightAnimOffset -= dt * smi.master.rocketSpeed;
				KBatchedAnimController component = smi.master.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				Vector3 positionIncludingOffset = component.PositionIncludingOffset;
				int num2 = Grid.PosToCell(smi.master.gameObject.transform.GetPosition() + smi.master.GetComponent<KBatchedAnimController>().Offset);
				if (Grid.IsValidCell(num2))
				{
					SimMessages.EmitMass(num2, ElementLoader.GetElementIndex(smi.master.exhaustElement), dt * smi.master.exhaustEmitRate, smi.master.exhaustTemperature, 0, 0, -1);
				}
				if (component.Offset.y <= 0f)
				{
					smi.GoTo(this.crashed);
				}
			}, UpdateRate.SIM_33ms, false);
			this.crashed.PlayAnim("grounded").Enter(delegate(PodLander.StatesInstance smi)
			{
				smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
				smi.master.rocketSpeed = 0f;
				smi.master.ReleaseAstronaut();
			});
		}

		// Token: 0x040066C6 RID: 26310
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State landing;

		// Token: 0x040066C7 RID: 26311
		public GameStateMachine<PodLander.States, PodLander.StatesInstance, PodLander, object>.State crashed;
	}
}
