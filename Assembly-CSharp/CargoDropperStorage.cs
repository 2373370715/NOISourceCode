using System;
using UnityEngine;

// Token: 0x02001098 RID: 4248
public class CargoDropperStorage : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>
{
	// Token: 0x06005646 RID: 22086 RVA: 0x000DCB38 File Offset: 0x000DAD38
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.JettisonCargo, delegate(CargoDropperStorage.StatesInstance smi, object data)
		{
			smi.JettisonCargo(data);
		});
	}

	// Token: 0x02001099 RID: 4249
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003D15 RID: 15637
		public Vector3 dropOffset;
	}

	// Token: 0x0200109A RID: 4250
	public class StatesInstance : GameStateMachine<CargoDropperStorage, CargoDropperStorage.StatesInstance, IStateMachineTarget, CargoDropperStorage.Def>.GameInstance
	{
		// Token: 0x06005649 RID: 22089 RVA: 0x000DCB7A File Offset: 0x000DAD7A
		public StatesInstance(IStateMachineTarget master, CargoDropperStorage.Def def) : base(master, def)
		{
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x0028F4BC File Offset: 0x0028D6BC
		public void JettisonCargo(object data)
		{
			Vector3 position = base.master.transform.GetPosition() + base.def.dropOffset;
			Storage component = base.GetComponent<Storage>();
			if (component != null)
			{
				GameObject gameObject = component.FindFirst("ScoutRover");
				if (gameObject != null)
				{
					component.Drop(gameObject, true);
					Vector3 position2 = base.master.transform.GetPosition();
					position2.z = Grid.GetLayerZ(Grid.SceneLayer.Creatures);
					gameObject.transform.SetPosition(position2);
					ChoreProvider component2 = gameObject.GetComponent<ChoreProvider>();
					if (component2 != null)
					{
						KBatchedAnimController component3 = gameObject.GetComponent<KBatchedAnimController>();
						if (component3 != null)
						{
							component3.Play("enter", KAnim.PlayMode.Once, 1f, 0f);
						}
						new EmoteChore(component2, Db.Get().ChoreTypes.EmoteHighPriority, null, new HashedString[]
						{
							"enter"
						}, KAnim.PlayMode.Once, false);
					}
					gameObject.GetMyWorld().SetRoverLanded();
				}
				component.DropAll(position, false, false, default(Vector3), true, null);
			}
		}
	}
}
