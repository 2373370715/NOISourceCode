using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001956 RID: 6486
[SerializationConfig(MemberSerialization.OptIn)]
public class LaunchableRocket : StateMachineComponent<LaunchableRocket.StatesInstance>, ILaunchableRocket
{
	// Token: 0x170008D4 RID: 2260
	// (get) Token: 0x060086F7 RID: 34551 RVA: 0x000B1628 File Offset: 0x000AF828
	public LaunchableRocketRegisterType registerType
	{
		get
		{
			return LaunchableRocketRegisterType.Spacecraft;
		}
	}

	// Token: 0x170008D5 RID: 2261
	// (get) Token: 0x060086F8 RID: 34552 RVA: 0x000CEC86 File Offset: 0x000CCE86
	public GameObject LaunchableGameObject
	{
		get
		{
			return base.gameObject;
		}
	}

	// Token: 0x170008D6 RID: 2262
	// (get) Token: 0x060086F9 RID: 34553 RVA: 0x000FD032 File Offset: 0x000FB232
	// (set) Token: 0x060086FA RID: 34554 RVA: 0x000FD03A File Offset: 0x000FB23A
	public float rocketSpeed { get; private set; }

	// Token: 0x170008D7 RID: 2263
	// (get) Token: 0x060086FB RID: 34555 RVA: 0x000FD043 File Offset: 0x000FB243
	// (set) Token: 0x060086FC RID: 34556 RVA: 0x000FD04B File Offset: 0x000FB24B
	public bool isLanding { get; private set; }

	// Token: 0x060086FD RID: 34557 RVA: 0x0035C008 File Offset: 0x0035A208
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.master.parts = AttachableBuilding.GetAttachedNetwork(base.smi.master.GetComponent<AttachableBuilding>());
		if (SpacecraftManager.instance.GetSpacecraftID(this) == -1)
		{
			Spacecraft spacecraft = new Spacecraft(base.GetComponent<LaunchConditionManager>());
			spacecraft.GenerateName();
			SpacecraftManager.instance.RegisterSpacecraft(spacecraft);
			base.gameObject.AddOrGet<RocketLaunchConditionVisualizerEffect>();
		}
		base.smi.StartSM();
	}

	// Token: 0x060086FE RID: 34558 RVA: 0x0035C084 File Offset: 0x0035A284
	public List<GameObject> GetEngines()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject gameObject in this.parts)
		{
			if (gameObject.GetComponent<RocketEngine>())
			{
				list.Add(gameObject);
			}
		}
		return list;
	}

	// Token: 0x060086FF RID: 34559 RVA: 0x000FD054 File Offset: 0x000FB254
	protected override void OnCleanUp()
	{
		SpacecraftManager.instance.UnregisterSpacecraft(base.GetComponent<LaunchConditionManager>());
		base.OnCleanUp();
	}

	// Token: 0x0400664E RID: 26190
	public List<GameObject> parts = new List<GameObject>();

	// Token: 0x0400664F RID: 26191
	[Serialize]
	private int takeOffLocation;

	// Token: 0x04006650 RID: 26192
	[Serialize]
	private float flightAnimOffset;

	// Token: 0x04006651 RID: 26193
	private GameObject soundSpeakerObject;

	// Token: 0x02001957 RID: 6487
	public class StatesInstance : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.GameInstance
	{
		// Token: 0x06008701 RID: 34561 RVA: 0x000FD07F File Offset: 0x000FB27F
		public StatesInstance(LaunchableRocket master) : base(master)
		{
		}

		// Token: 0x06008702 RID: 34562 RVA: 0x000FD088 File Offset: 0x000FB288
		public bool IsMissionState(Spacecraft.MissionState state)
		{
			return SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).state == state;
		}

		// Token: 0x06008703 RID: 34563 RVA: 0x000FD0A7 File Offset: 0x000FB2A7
		public void SetMissionState(Spacecraft.MissionState state)
		{
			SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(base.master.GetComponent<LaunchConditionManager>()).SetState(state);
		}
	}

	// Token: 0x02001958 RID: 6488
	public class States : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket>
	{
		// Token: 0x06008704 RID: 34564 RVA: 0x0035C0EC File Offset: 0x0035A2EC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.grounded;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.grounded.ToggleTag(GameTags.RocketOnGround).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.AddTag(GameTags.RocketOnGround);
					}
				}
			}).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.RemoveTag(GameTags.RocketOnGround);
					}
				}
			}).EventTransition(GameHashes.DoLaunchRocket, this.not_grounded.launch_pre, null).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.rocketSpeed = 0f;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.zero;
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Grounded);
			});
			this.not_grounded.ToggleTag(GameTags.RocketNotOnGround).Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.AddTag(GameTags.RocketNotOnGround);
					}
				}
			}).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.RemoveTag(GameTags.RocketNotOnGround);
					}
				}
			});
			this.not_grounded.launch_pre.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = false;
				smi.master.rocketSpeed = 0f;
				smi.master.parts = AttachableBuilding.GetAttachedNetwork(smi.master.GetComponent<AttachableBuilding>());
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				foreach (GameObject go in smi.master.GetEngines())
				{
					go.Trigger(-1358394196, null);
				}
				Game.Instance.Trigger(-1277991738, smi.gameObject);
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						smi.master.takeOffLocation = Grid.PosToCell(smi.master.gameObject);
						gameObject.Trigger(-1277991738, null);
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Launching);
			}).ScheduleGoTo(5f, this.not_grounded.launch_loop);
			this.not_grounded.launch_loop.EventTransition(GameHashes.DoReturnRocket, this.not_grounded.returning, null).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.isLanding = false;
				bool flag = true;
				float num = Mathf.Clamp(Mathf.Pow(smi.timeinstate / 5f, 4f), 0f, 10f);
				smi.master.rocketSpeed = num;
				smi.master.flightAnimOffset += dt * num;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
						component.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component.PositionIncludingOffset;
						if (smi.master.soundSpeakerObject == null)
						{
							smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
							smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
						}
						smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
						if (Grid.PosToXY(positionIncludingOffset).y > Singleton<KBatchedAnimUpdater>.Instance.GetVisibleSize().y + 20)
						{
							gameObject.GetComponent<KBatchedAnimController>().enabled = false;
						}
						else
						{
							flag = false;
							LaunchableRocket.States.DoWorldDamage(gameObject, positionIncludingOffset);
						}
					}
				}
				if (flag)
				{
					smi.GoTo(this.not_grounded.space);
				}
			}, UpdateRate.SIM_33ms, false).Exit(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.gameObject.GetMyWorld().RevealSurface();
			});
			this.not_grounded.space.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.rocketSpeed = 0f;
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
						gameObject.GetComponent<KBatchedAnimController>().enabled = false;
					}
				}
				smi.SetMissionState(Spacecraft.MissionState.Underway);
			}).EventTransition(GameHashes.DoReturnRocket, this.not_grounded.returning, (LaunchableRocket.StatesInstance smi) => smi.IsMissionState(Spacecraft.MissionState.WaitingToLand));
			this.not_grounded.returning.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = true;
				smi.master.rocketSpeed = 0f;
				smi.SetMissionState(Spacecraft.MissionState.Landing);
			}).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.isLanding = true;
				KBatchedAnimController component = smi.master.gameObject.GetComponent<KBatchedAnimController>();
				component.Offset = Vector3.up * smi.master.flightAnimOffset;
				float num = Mathf.Abs(smi.master.gameObject.transform.position.y + component.Offset.y - (Grid.CellToPos(smi.master.takeOffLocation) + Vector3.down * (Grid.CellSizeInMeters / 2f)).y);
				float num2 = Mathf.Clamp(0.5f * num, 0f, 10f) * dt;
				smi.master.rocketSpeed = num2;
				smi.master.flightAnimOffset -= num2;
				bool flag = true;
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
				foreach (GameObject gameObject in smi.master.parts)
				{
					if (!(gameObject == null))
					{
						KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
						component2.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component2.PositionIncludingOffset;
						if (Grid.IsValidCell(Grid.PosToCell(gameObject)))
						{
							gameObject.GetComponent<KBatchedAnimController>().enabled = true;
						}
						else
						{
							flag = false;
						}
						LaunchableRocket.States.DoWorldDamage(gameObject, positionIncludingOffset);
					}
				}
				if (flag)
				{
					smi.GoTo(this.not_grounded.landing_loop);
				}
			}, UpdateRate.SIM_33ms, false);
			this.not_grounded.landing_loop.Enter(delegate(LaunchableRocket.StatesInstance smi)
			{
				smi.master.isLanding = true;
				int num = -1;
				for (int i = 0; i < smi.master.parts.Count; i++)
				{
					GameObject gameObject = smi.master.parts[i];
					if (!(gameObject == null) && gameObject != smi.master.gameObject && gameObject.GetComponent<RocketEngine>() != null)
					{
						num = i;
					}
				}
				if (num != -1)
				{
					smi.master.parts[num].Trigger(-1358394196, null);
				}
			}).Update(delegate(LaunchableRocket.StatesInstance smi, float dt)
			{
				smi.master.gameObject.GetComponent<KBatchedAnimController>().Offset = Vector3.up * smi.master.flightAnimOffset;
				float flightAnimOffset = smi.master.flightAnimOffset;
				float num = Mathf.Clamp(0.5f * flightAnimOffset, 0f, 10f);
				smi.master.rocketSpeed = num;
				smi.master.flightAnimOffset -= num * dt;
				if (smi.master.soundSpeakerObject == null)
				{
					smi.master.soundSpeakerObject = new GameObject("rocketSpeaker");
					smi.master.soundSpeakerObject.transform.SetParent(smi.master.gameObject.transform);
				}
				smi.master.soundSpeakerObject.transform.SetLocalPosition(smi.master.flightAnimOffset * Vector3.up);
				if (num <= 0.0025f && dt != 0f)
				{
					smi.master.GetComponent<KSelectable>().IsSelectable = true;
					Game.Instance.Trigger(-887025858, smi.gameObject);
					foreach (GameObject gameObject in smi.master.parts)
					{
						if (!(gameObject == null))
						{
							gameObject.Trigger(-887025858, null);
						}
					}
					smi.GoTo(this.grounded);
					return;
				}
				foreach (GameObject gameObject2 in smi.master.parts)
				{
					if (!(gameObject2 == null))
					{
						KBatchedAnimController component = gameObject2.GetComponent<KBatchedAnimController>();
						component.Offset = Vector3.up * smi.master.flightAnimOffset;
						Vector3 positionIncludingOffset = component.PositionIncludingOffset;
						LaunchableRocket.States.DoWorldDamage(gameObject2, positionIncludingOffset);
					}
				}
			}, UpdateRate.SIM_33ms, false);
		}

		// Token: 0x06008705 RID: 34565 RVA: 0x0035C37C File Offset: 0x0035A57C
		private static void DoWorldDamage(GameObject part, Vector3 apparentPosition)
		{
			OccupyArea component = part.GetComponent<OccupyArea>();
			component.UpdateOccupiedArea();
			foreach (CellOffset offset in component.OccupiedCellsOffsets)
			{
				int num = Grid.OffsetCell(Grid.PosToCell(apparentPosition), offset);
				if (Grid.IsValidCell(num))
				{
					if (Grid.Solid[num])
					{
						WorldDamage.Instance.ApplyDamage(num, 10000f, num, BUILDINGS.DAMAGESOURCES.ROCKET, UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET);
					}
					else if (Grid.FakeFloor[num])
					{
						GameObject gameObject = Grid.Objects[num, 39];
						if (gameObject != null)
						{
							BuildingHP component2 = gameObject.GetComponent<BuildingHP>();
							if (component2 != null)
							{
								gameObject.Trigger(-794517298, new BuildingHP.DamageSourceInfo
								{
									damage = component2.MaxHitPoints,
									source = BUILDINGS.DAMAGESOURCES.ROCKET,
									popString = UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.ROCKET
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x04006654 RID: 26196
		public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State grounded;

		// Token: 0x04006655 RID: 26197
		public LaunchableRocket.States.NotGroundedStates not_grounded;

		// Token: 0x02001959 RID: 6489
		public class NotGroundedStates : GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State
		{
			// Token: 0x04006656 RID: 26198
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_pre;

			// Token: 0x04006657 RID: 26199
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State space;

			// Token: 0x04006658 RID: 26200
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State launch_loop;

			// Token: 0x04006659 RID: 26201
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State returning;

			// Token: 0x0400665A RID: 26202
			public GameStateMachine<LaunchableRocket.States, LaunchableRocket.StatesInstance, LaunchableRocket, object>.State landing_loop;
		}
	}
}
