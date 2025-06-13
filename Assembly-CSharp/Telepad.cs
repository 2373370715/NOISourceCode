﻿using System;
using System.Collections;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

public class Telepad : StateMachineComponent<Telepad.StatesInstance>
{
	public void AddNewBaseMinion(GameObject minion, bool extra_power_banks)
	{
		Ref<MinionIdentity> item = new Ref<MinionIdentity>(minion.GetComponent<MinionIdentity>());
		this.aNewHopeEvents.Add(item);
		if (extra_power_banks)
		{
			this.extraPowerBanksEvents.Add(item);
		}
	}

	public void ScheduleNewBaseEvents()
	{
		this.aNewHopeEvents.RemoveAll((Ref<MinionIdentity> entry) => entry == null || entry.Get() == null);
		this.extraPowerBanksEvents.RemoveAll((Ref<MinionIdentity> entry) => entry == null || entry.Get() == null);
		Effect a_new_hope = Db.Get().effects.Get("AnewHope");
		Action<object> <>9__2;
		for (int i = 0; i < this.aNewHopeEvents.Count; i++)
		{
			GameObject gameObject = this.aNewHopeEvents[i].Get().gameObject;
			GameScheduler instance = GameScheduler.Instance;
			string name = "ANewHope";
			float time = 3f + 0.5f * (float)i;
			Action<object> callback;
			if ((callback = <>9__2) == null)
			{
				callback = (<>9__2 = delegate(object m)
				{
					GameObject gameObject3 = m as GameObject;
					if (gameObject3 == null)
					{
						return;
					}
					this.RemoveFromEvents(this.aNewHopeEvents, gameObject3);
					gameObject3.GetComponent<Effects>().Add(a_new_hope, true);
				});
			}
			instance.Schedule(name, time, callback, gameObject, null);
		}
		Action<object> <>9__3;
		for (int j = 0; j < this.extraPowerBanksEvents.Count; j++)
		{
			GameObject gameObject2 = this.extraPowerBanksEvents[j].Get().gameObject;
			GameScheduler instance2 = GameScheduler.Instance;
			string name2 = "ExtraPowerBanks";
			float time2 = 3f + 4.5f * (float)j;
			Action<object> callback2;
			if ((callback2 = <>9__3) == null)
			{
				callback2 = (<>9__3 = delegate(object m)
				{
					GameObject gameObject3 = m as GameObject;
					if (gameObject3 == null)
					{
						return;
					}
					this.RemoveFromEvents(this.extraPowerBanksEvents, gameObject3);
					GameUtil.GetTelepad(ClusterManager.Instance.GetStartWorld().id).Trigger(1982288670, null);
				});
			}
			instance2.Schedule(name2, time2, callback2, gameObject2, null);
		}
	}

	private void RemoveFromEvents(List<Ref<MinionIdentity>> listToRemove, GameObject go)
	{
		for (int i = listToRemove.Count - 1; i >= 0; i--)
		{
			if (listToRemove[i].Get() != null && listToRemove[i].Get() == go.GetComponent<MinionIdentity>())
			{
				listToRemove.RemoveAt(i);
				return;
			}
		}
	}

	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Deconstructable>().allowDeconstruction = false;
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(Grid.PosToCell(this), out num, out num2);
		if (num == 0)
		{
			global::Debug.LogError(string.Concat(new string[]
			{
				"Headquarters spawned at: (",
				num.ToString(),
				",",
				num2.ToString(),
				")"
			}));
		}
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Telepads.Add(this);
		this.meter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[]
		{
			"meter_target",
			"meter_fill",
			"meter_frame",
			"meter_OL"
		});
		this.meter.gameObject.GetComponent<KBatchedAnimController>().SetDirty();
		base.smi.StartSM();
		this.ScheduleNewBaseEvents();
	}

	protected override void OnCleanUp()
	{
		Components.Telepads.Remove(this);
		base.OnCleanUp();
	}

	public void Update()
	{
		if (base.smi.IsColonyLost())
		{
			return;
		}
		if (Immigration.Instance.ImmigrantsAvailable && base.GetComponent<Operational>().IsOperational)
		{
			base.smi.sm.openPortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.NewDuplicantsAvailable, this);
		}
		else
		{
			base.smi.sm.closePortal.Trigger(base.smi);
			this.selectable.SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Wattson, this);
		}
		if (this.GetTimeRemaining() < -120f)
		{
			Messenger.Instance.QueueMessage(new DuplicantsLeftMessage());
			Immigration.Instance.EndImmigration();
		}
	}

	public void RejectAll()
	{
		Immigration.Instance.EndImmigration();
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	public void OnAcceptDelivery(ITelepadDeliverable delivery)
	{
		int cell = Grid.PosToCell(this);
		Immigration.Instance.EndImmigration();
		GameObject gameObject = delivery.Deliver(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
		MinionIdentity component = gameObject.GetComponent<MinionIdentity>();
		if (component != null)
		{
			ReportManager.Instance.ReportValue(ReportManager.ReportType.PersonalTime, GameClock.Instance.GetTimeSinceStartOfReport(), string.Format(UI.ENDOFDAYREPORT.NOTES.PERSONAL_TIME, DUPLICANTS.CHORES.NOT_EXISTING_TASK), gameObject.GetProperName());
			foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.GetWorldItems(base.gameObject.GetComponent<KSelectable>().GetMyWorldId(), false))
			{
				minionIdentity.GetComponent<Effects>().Add("NewCrewArrival", true);
			}
			MinionResume component2 = component.GetComponent<MinionResume>();
			int num = 0;
			while ((float)num < this.startingSkillPoints)
			{
				component2.ForceAddSkillPoint();
				num++;
			}
			if (component.HasTag(GameTags.Minions.Models.Bionic))
			{
				GameScheduler.Instance.Schedule("BonusBatteryDelivery", 5f, delegate(object data)
				{
					base.Trigger(1982288670, null);
				}, null, null);
			}
		}
		base.smi.sm.closePortal.Trigger(base.smi);
	}

	public float GetTimeRemaining()
	{
		return Immigration.Instance.GetTimeRemaining();
	}

	[MyCmpReq]
	private KSelectable selectable;

	private MeterController meter;

	private const float MAX_IMMIGRATION_TIME = 120f;

	private const int NUM_METER_NOTCHES = 8;

	private List<MinionStartingStats> minionStats;

	public float startingSkillPoints;

	[Serialize]
	private List<Ref<MinionIdentity>> aNewHopeEvents = new List<Ref<MinionIdentity>>();

	[Serialize]
	private List<Ref<MinionIdentity>> extraPowerBanksEvents = new List<Ref<MinionIdentity>>();

	public static readonly HashedString[] PortalBirthAnim = new HashedString[]
	{
		"portalbirth"
	};

	public class StatesInstance : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.GameInstance
	{
		public StatesInstance(Telepad master) : base(master)
		{
		}

		public bool IsColonyLost()
		{
			return GameFlowManager.Instance != null && GameFlowManager.Instance.IsGameOver();
		}

		public void UpdateMeter()
		{
			float timeRemaining = Immigration.Instance.GetTimeRemaining();
			float totalWaitTime = Immigration.Instance.GetTotalWaitTime();
			float positionPercent = Mathf.Clamp01(1f - timeRemaining / totalWaitTime);
			base.master.meter.SetPositionPercent(positionPercent);
		}

		public IEnumerator SpawnExtraPowerBanks()
		{
			int cellTarget = Grid.OffsetCell(Grid.PosToCell(base.gameObject), 1, 2);
			int count = 5;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, MISC.POPFX.EXTRA_POWERBANKS_BIONIC, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
				KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("DisposableElectrobank_RawMetal"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
				gameObject.SetActive(true);
				Vector2 initial_velocity = new Vector2((-2.5f + 5f * ((float)i / 5f)) / 2f, 2f);
				if (GameComps.Fallers.Has(gameObject))
				{
					GameComps.Fallers.Remove(gameObject);
				}
				GameComps.Fallers.Add(gameObject, initial_velocity);
				yield return new WaitForSeconds(0.25f);
				num = i;
			}
			yield return new WaitForSeconds(0.35f);
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, ITEMS.LUBRICATIONSTICK.NAME, base.gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
			KMonoBehaviour.PlaySound(GlobalAssets.GetSound("SandboxTool_Spawner", false));
			GameObject gameObject2 = Util.KInstantiate(Assets.GetPrefab("LubricationStick"), Grid.CellToPosCBC(cellTarget, Grid.SceneLayer.Front) - Vector3.right / 2f);
			gameObject2.SetActive(true);
			Vector2 initial_velocity2 = new Vector2(3.75f, 2.5f);
			if (GameComps.Fallers.Has(gameObject2))
			{
				GameComps.Fallers.Remove(gameObject2);
			}
			GameComps.Fallers.Add(gameObject2, initial_velocity2);
			yield return 0;
			yield break;
		}
	}

	public class States : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad>
	{
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.OnSignal(this.idlePortal, this.resetToIdle).EventTransition(GameHashes.BonusTelepadDelivery, this.bonusDelivery.pre, null);
			this.resetToIdle.GoTo(this.idle);
			this.idle.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.UpdateMeter();
			}).Update("TelepadMeter", delegate(Telepad.StatesInstance smi, float dt)
			{
				smi.UpdateMeter();
			}, UpdateRate.SIM_4000ms, false).EventTransition(GameHashes.OperationalChanged, this.unoperational, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).PlayAnim("idle").OnSignal(this.openPortal, this.opening);
			this.unoperational.PlayAnim("idle").Enter("StopImmigration", delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).EventTransition(GameHashes.OperationalChanged, this.idle, (Telepad.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.opening.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_pre").OnAnimQueueComplete(this.open);
			this.open.OnSignal(this.closePortal, this.close).Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(1f);
			}).PlayAnim("working_loop", KAnim.PlayMode.Loop).Transition(this.close, (Telepad.StatesInstance smi) => smi.IsColonyLost(), UpdateRate.SIM_200ms).EventTransition(GameHashes.OperationalChanged, this.close, (Telepad.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.close.Enter(delegate(Telepad.StatesInstance smi)
			{
				smi.master.meter.SetPositionPercent(0f);
			}).PlayAnims((Telepad.StatesInstance smi) => Telepad.States.workingAnims, KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.bonusDelivery.pre.PlayAnim("bionic_working_pre").OnAnimQueueComplete(this.bonusDelivery.loop);
			this.bonusDelivery.loop.PlayAnim("bionic_working_loop", KAnim.PlayMode.Loop).ScheduleAction("SpawnBonusDelivery", 1f, delegate(Telepad.StatesInstance smi)
			{
				smi.master.StartCoroutine(smi.SpawnExtraPowerBanks());
			}).ScheduleGoTo(3f, this.bonusDelivery.pst);
			this.bonusDelivery.pst.PlayAnim("bionic_working_pst").OnAnimQueueComplete(this.idle);
		}

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal openPortal;

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal closePortal;

		public StateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.Signal idlePortal;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State idle;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State resetToIdle;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State opening;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State open;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State close;

		public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State unoperational;

		public Telepad.States.BonusDeliveryStates bonusDelivery;

		private static readonly HashedString[] workingAnims = new HashedString[]
		{
			"working_loop",
			"working_pst"
		};

		public class BonusDeliveryStates : GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State
		{
			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pre;

			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State loop;

			public GameStateMachine<Telepad.States, Telepad.StatesInstance, Telepad, object>.State pst;
		}
	}
}
