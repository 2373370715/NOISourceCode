using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020016F9 RID: 5881
public class PlantElementEmitter : StateMachineComponent<PlantElementEmitter.StatesInstance>, IGameObjectEffectDescriptor
{
	// Token: 0x0600793B RID: 31035 RVA: 0x000F421E File Offset: 0x000F241E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600793C RID: 31036 RVA: 0x000CE880 File Offset: 0x000CCA80
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>();
	}

	// Token: 0x04005B0C RID: 23308
	[MyCmpGet]
	private WiltCondition wiltCondition;

	// Token: 0x04005B0D RID: 23309
	[MyCmpReq]
	private KSelectable selectable;

	// Token: 0x04005B0E RID: 23310
	public SimHashes emittedElement;

	// Token: 0x04005B0F RID: 23311
	public float emitRate;

	// Token: 0x020016FA RID: 5882
	public class StatesInstance : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.GameInstance
	{
		// Token: 0x0600793E RID: 31038 RVA: 0x000F4239 File Offset: 0x000F2439
		public StatesInstance(PlantElementEmitter master) : base(master)
		{
		}

		// Token: 0x0600793F RID: 31039 RVA: 0x000F4242 File Offset: 0x000F2442
		public bool IsWilting()
		{
			return !(base.master.wiltCondition == null) && base.master.wiltCondition != null && base.master.wiltCondition.IsWilting();
		}
	}

	// Token: 0x020016FB RID: 5883
	public class States : GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter>
	{
		// Token: 0x06007940 RID: 31040 RVA: 0x00322A08 File Offset: 0x00320C08
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.healthy;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.healthy.EventTransition(GameHashes.Wilt, this.wilted, (PlantElementEmitter.StatesInstance smi) => smi.IsWilting()).Update("PlantEmit", delegate(PlantElementEmitter.StatesInstance smi, float dt)
			{
				SimMessages.EmitMass(Grid.PosToCell(smi.master.gameObject), ElementLoader.FindElementByHash(smi.master.emittedElement).idx, smi.master.emitRate * dt, ElementLoader.FindElementByHash(smi.master.emittedElement).defaultValues.temperature, byte.MaxValue, 0, -1);
			}, UpdateRate.SIM_4000ms, false);
			this.wilted.EventTransition(GameHashes.WiltRecover, this.healthy, null);
		}

		// Token: 0x04005B10 RID: 23312
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State wilted;

		// Token: 0x04005B11 RID: 23313
		public GameStateMachine<PlantElementEmitter.States, PlantElementEmitter.StatesInstance, PlantElementEmitter, object>.State healthy;
	}
}
