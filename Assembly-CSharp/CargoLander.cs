using System;
using KSerialization;
using UnityEngine;

// Token: 0x02001915 RID: 6421
[SerializationConfig(MemberSerialization.OptIn)]
public class CargoLander : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>
{
	// Token: 0x060084FD RID: 34045 RVA: 0x0035421C File Offset: 0x0035241C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.init;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.InitializeOperationalFlag(RocketModule.landedFlag, false).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).EventHandler(GameHashes.OnStorageChange, delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		});
		this.init.ParamTransition<bool>(this.isLanded, this.grounded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue).GoTo(this.stored);
		this.stored.TagTransition(GameTags.Stored, this.landing, true).EventHandler(GameHashes.JettisonedLander, delegate(CargoLander.StatesInstance smi)
		{
			smi.OnJettisoned();
		});
		this.landing.PlayAnim("landing", KAnim.PlayMode.Loop).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(true);
		}).Exit(delegate(CargoLander.StatesInstance smi)
		{
			smi.ShowLandingPreview(false);
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.ResetAnimPosition();
		}).Update(delegate(CargoLander.StatesInstance smi, float dt)
		{
			smi.LandingUpdate(dt);
		}, UpdateRate.SIM_EVERY_TICK, false).Transition(this.land, (CargoLander.StatesInstance smi) => smi.flightAnimOffset <= 0f, UpdateRate.SIM_200ms);
		this.land.PlayAnim("grounded_pre").OnAnimQueueComplete(this.grounded);
		this.grounded.DefaultState(this.grounded.loaded).ToggleOperationalFlag(RocketModule.landedFlag).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.CheckIfLoaded();
		}).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.sm.isLanded.Set(true, smi, false);
		});
		this.grounded.loaded.PlayAnim("grounded").ParamTransition<bool>(this.hasCargo, this.grounded.empty, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsFalse).OnSignal(this.emptyCargo, this.grounded.emptying).Enter(delegate(CargoLander.StatesInstance smi)
		{
			smi.DoLand();
		});
		this.grounded.emptying.PlayAnim("deploying").TriggerOnEnter(GameHashes.JettisonCargo, null).OnAnimQueueComplete(this.grounded.empty);
		this.grounded.empty.PlayAnim("deployed").ParamTransition<bool>(this.hasCargo, this.grounded.loaded, GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.IsTrue);
	}

	// Token: 0x04006531 RID: 25905
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter hasCargo;

	// Token: 0x04006532 RID: 25906
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.Signal emptyCargo;

	// Token: 0x04006533 RID: 25907
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State init;

	// Token: 0x04006534 RID: 25908
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State stored;

	// Token: 0x04006535 RID: 25909
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State landing;

	// Token: 0x04006536 RID: 25910
	public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State land;

	// Token: 0x04006537 RID: 25911
	public CargoLander.CrashedStates grounded;

	// Token: 0x04006538 RID: 25912
	public StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter isLanded = new StateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.BoolParameter(false);

	// Token: 0x02001916 RID: 6422
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04006539 RID: 25913
		public Tag previewTag;

		// Token: 0x0400653A RID: 25914
		public bool deployOnLanding = true;
	}

	// Token: 0x02001917 RID: 6423
	public class CrashedStates : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State
	{
		// Token: 0x0400653B RID: 25915
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State loaded;

		// Token: 0x0400653C RID: 25916
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State emptying;

		// Token: 0x0400653D RID: 25917
		public GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.State empty;
	}

	// Token: 0x02001918 RID: 6424
	public class StatesInstance : GameStateMachine<CargoLander, CargoLander.StatesInstance, IStateMachineTarget, CargoLander.Def>.GameInstance
	{
		// Token: 0x06008501 RID: 34049 RVA: 0x00354524 File Offset: 0x00352724
		public StatesInstance(IStateMachineTarget master, CargoLander.Def def) : base(master, def)
		{
		}

		// Token: 0x06008502 RID: 34050 RVA: 0x000FBE0F File Offset: 0x000FA00F
		public void ResetAnimPosition()
		{
			base.GetComponent<KBatchedAnimController>().Offset = Vector3.up * this.flightAnimOffset;
		}

		// Token: 0x06008503 RID: 34051 RVA: 0x000FBE2C File Offset: 0x000FA02C
		public void OnJettisoned()
		{
			this.flightAnimOffset = 50f;
		}

		// Token: 0x06008504 RID: 34052 RVA: 0x00354570 File Offset: 0x00352770
		public void ShowLandingPreview(bool show)
		{
			if (show)
			{
				this.landingPreview = Util.KInstantiate(Assets.GetPrefab(base.def.previewTag), base.transform.GetPosition(), Quaternion.identity, base.gameObject, null, true, 0);
				this.landingPreview.SetActive(true);
				return;
			}
			this.landingPreview.DeleteObject();
			this.landingPreview = null;
		}

		// Token: 0x06008505 RID: 34053 RVA: 0x003545D4 File Offset: 0x003527D4
		public void LandingUpdate(float dt)
		{
			this.flightAnimOffset = Mathf.Max(this.flightAnimOffset - dt * this.topSpeed, 0f);
			this.ResetAnimPosition();
			int num = Grid.PosToCell(base.gameObject.transform.GetPosition() + new Vector3(0f, this.flightAnimOffset, 0f));
			if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == base.gameObject.GetMyWorldId())
			{
				SimMessages.EmitMass(num, ElementLoader.GetElementIndex(this.exhaustElement), dt * this.exhaustEmitRate, this.exhaustTemperature, 0, 0, -1);
			}
		}

		// Token: 0x06008506 RID: 34054 RVA: 0x00354674 File Offset: 0x00352874
		public void DoLand()
		{
			base.smi.master.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
			OccupyArea component = base.smi.GetComponent<OccupyArea>();
			if (component != null)
			{
				component.ApplyToCells = true;
			}
			if (base.def.deployOnLanding && this.CheckIfLoaded())
			{
				base.sm.emptyCargo.Trigger(this);
			}
			base.smi.master.gameObject.Trigger(1591811118, this);
		}

		// Token: 0x06008507 RID: 34055 RVA: 0x003546F8 File Offset: 0x003528F8
		public bool CheckIfLoaded()
		{
			bool flag = false;
			MinionStorage component = base.GetComponent<MinionStorage>();
			if (component != null)
			{
				flag |= (component.GetStoredMinionInfo().Count > 0);
			}
			Storage component2 = base.GetComponent<Storage>();
			if (component2 != null && !component2.IsEmpty())
			{
				flag = true;
			}
			if (flag != base.sm.hasCargo.Get(this))
			{
				base.sm.hasCargo.Set(flag, this, false);
			}
			return flag;
		}

		// Token: 0x0400653E RID: 25918
		[Serialize]
		public float flightAnimOffset = 50f;

		// Token: 0x0400653F RID: 25919
		public float exhaustEmitRate = 2f;

		// Token: 0x04006540 RID: 25920
		public float exhaustTemperature = 1000f;

		// Token: 0x04006541 RID: 25921
		public SimHashes exhaustElement = SimHashes.CarbonDioxide;

		// Token: 0x04006542 RID: 25922
		public float topSpeed = 5f;

		// Token: 0x04006543 RID: 25923
		private GameObject landingPreview;
	}
}
