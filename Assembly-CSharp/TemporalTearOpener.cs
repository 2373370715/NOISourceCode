using System;
using System.Collections.Generic;
using Database;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200102F RID: 4143
public class TemporalTearOpener : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>
{
	// Token: 0x060053E7 RID: 21479 RVA: 0x00287C4C File Offset: 0x00285E4C
	private static StatusItem CreateColoniesStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Insufficient_Colonies", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", string.Format("({0}/{1})", instance.CountColonies(), EstablishColonies.BASE_COUNT));
			return str;
		};
		return statusItem;
	}

	// Token: 0x060053E8 RID: 21480 RVA: 0x00287CA4 File Offset: 0x00285EA4
	private static StatusItem CreateProgressStatusItem()
	{
		StatusItem statusItem = new StatusItem("Temporal_Tear_Opener_Progress", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);
		statusItem.resolveStringCallback = delegate(string str, object data)
		{
			TemporalTearOpener.Instance instance = (TemporalTearOpener.Instance)data;
			str = str.Replace("{progress}", GameUtil.GetFormattedPercent(instance.GetPercentComplete(), GameUtil.TimeSlice.None));
			return str;
		};
		return statusItem;
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x00287CFC File Offset: 0x00285EFC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.UpdateMeter();
			if (ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearOpen())
			{
				smi.GoTo(this.opening_tear_finish);
				return;
			}
			smi.GoTo(this.check_requirements);
		}).PlayAnim("off");
		this.check_requirements.DefaultState(this.check_requirements.has_target).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = false;
			smi.GetComponent<KBatchedAnimController>().Play("port_close", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("off", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.check_requirements.has_target.ToggleStatusItem(TemporalTearOpener.s_noTargetStatus, null).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => ClusterManager.Instance.GetClusterPOIManager().IsTemporalTearRevealed(), UpdateRate.SIM_200ms, false);
		this.check_requirements.has_los.ToggleStatusItem(TemporalTearOpener.s_noLosStatus, null).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => smi.HasLineOfSight(), UpdateRate.SIM_200ms, false);
		this.check_requirements.enough_colonies.ToggleStatusItem(TemporalTearOpener.s_insufficient_colonies, null).UpdateTransition(this.charging, (TemporalTearOpener.Instance smi, float dt) => smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false);
		this.charging.DefaultState(this.charging.idle).ToggleStatusItem(TemporalTearOpener.s_progressStatus, (TemporalTearOpener.Instance smi) => smi).UpdateTransition(this.check_requirements.has_los, (TemporalTearOpener.Instance smi, float dt) => !smi.HasLineOfSight(), UpdateRate.SIM_200ms, false).UpdateTransition(this.check_requirements.enough_colonies, (TemporalTearOpener.Instance smi, float dt) => !smi.HasSufficientColonies(), UpdateRate.SIM_200ms, false).Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.GetComponent<HighEnergyParticleStorage>().receiverOpen = true;
			smi.GetComponent<KBatchedAnimController>().Play("port_open", KAnim.PlayMode.Once, 1f, 0f);
			smi.GetComponent<KBatchedAnimController>().Queue("inert", KAnim.PlayMode.Loop, 1f, 0f);
		});
		this.charging.idle.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.consuming, (TemporalTearOpener.Instance smi) => !smi.GetComponent<HighEnergyParticleStorage>().IsEmpty());
		this.charging.consuming.EventTransition(GameHashes.OnParticleStorageChanged, this.charging.idle, (TemporalTearOpener.Instance smi) => smi.GetComponent<HighEnergyParticleStorage>().IsEmpty()).UpdateTransition(this.ready, (TemporalTearOpener.Instance smi, float dt) => smi.ConsumeParticlesAndCheckComplete(dt), UpdateRate.SIM_200ms, false);
		this.ready.ToggleNotification((TemporalTearOpener.Instance smi) => new Notification(BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION, NotificationType.Good, (List<Notification> a, object b) => BUILDING.STATUSITEMS.TEMPORAL_TEAR_OPENER_READY.NOTIFICATION_TOOLTIP, null, false, 0f, null, null, null, true, false, false));
		this.opening_tear_beam_pre.PlayAnim("working_pre", KAnim.PlayMode.Once).OnAnimQueueComplete(this.opening_tear_beam);
		this.opening_tear_beam.Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.CreateBeamFX();
		}).PlayAnim("working_loop", KAnim.PlayMode.Loop).ScheduleGoTo(5f, this.opening_tear_finish);
		this.opening_tear_finish.PlayAnim("working_pst").Enter(delegate(TemporalTearOpener.Instance smi)
		{
			smi.OpenTemporalTear();
		});
	}

	// Token: 0x04003B1D RID: 15133
	private const float MIN_SUNLIGHT_EXPOSURE = 15f;

	// Token: 0x04003B1E RID: 15134
	private static StatusItem s_noLosStatus = new StatusItem("Temporal_Tear_Opener_No_Los", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04003B1F RID: 15135
	private static StatusItem s_insufficient_colonies = TemporalTearOpener.CreateColoniesStatusItem();

	// Token: 0x04003B20 RID: 15136
	private static StatusItem s_noTargetStatus = new StatusItem("Temporal_Tear_Opener_No_Target", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, false, 129022, null);

	// Token: 0x04003B21 RID: 15137
	private static StatusItem s_progressStatus = TemporalTearOpener.CreateProgressStatusItem();

	// Token: 0x04003B22 RID: 15138
	private TemporalTearOpener.CheckRequirementsState check_requirements;

	// Token: 0x04003B23 RID: 15139
	private TemporalTearOpener.ChargingState charging;

	// Token: 0x04003B24 RID: 15140
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam_pre;

	// Token: 0x04003B25 RID: 15141
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_beam;

	// Token: 0x04003B26 RID: 15142
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State opening_tear_finish;

	// Token: 0x04003B27 RID: 15143
	private GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State ready;

	// Token: 0x02001030 RID: 4144
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04003B28 RID: 15144
		public float consumeRate;

		// Token: 0x04003B29 RID: 15145
		public float numParticlesToOpen;
	}

	// Token: 0x02001031 RID: 4145
	private class ChargingState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04003B2A RID: 15146
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State idle;

		// Token: 0x04003B2B RID: 15147
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State consuming;
	}

	// Token: 0x02001032 RID: 4146
	private class CheckRequirementsState : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State
	{
		// Token: 0x04003B2C RID: 15148
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_target;

		// Token: 0x04003B2D RID: 15149
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State has_los;

		// Token: 0x04003B2E RID: 15150
		public GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.State enough_colonies;
	}

	// Token: 0x02001033 RID: 4147
	public new class Instance : GameStateMachine<TemporalTearOpener, TemporalTearOpener.Instance, IStateMachineTarget, TemporalTearOpener.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x060053F0 RID: 21488 RVA: 0x000DB0E5 File Offset: 0x000D92E5
		public Instance(IStateMachineTarget master, TemporalTearOpener.Def def) : base(master, def)
		{
			this.m_meter = new MeterController(base.gameObject.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Array.Empty<string>());
			EnterTemporalTearSequence.tearOpenerGameObject = base.gameObject;
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x000DB122 File Offset: 0x000D9322
		protected override void OnCleanUp()
		{
			if (EnterTemporalTearSequence.tearOpenerGameObject == base.gameObject)
			{
				EnterTemporalTearSequence.tearOpenerGameObject = null;
			}
			base.OnCleanUp();
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x002880EC File Offset: 0x002862EC
		public bool HasLineOfSight()
		{
			Extents extents = base.GetComponent<Building>().GetExtents();
			int x = extents.x;
			int num = extents.x + extents.width - 1;
			for (int i = x; i <= num; i++)
			{
				int i2 = Grid.XYToCell(i, extents.y);
				if ((float)Grid.ExposedToSunlight[i2] < 15f)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x000DB142 File Offset: 0x000D9342
		public bool HasSufficientColonies()
		{
			return this.CountColonies() >= EstablishColonies.BASE_COUNT;
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x0028814C File Offset: 0x0028634C
		public int CountColonies()
		{
			int num = 0;
			for (int i = 0; i < Components.Telepads.Count; i++)
			{
				Activatable component = Components.Telepads[i].GetComponent<Activatable>();
				if (component == null || component.IsActivated)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060053F5 RID: 21493 RVA: 0x00288198 File Offset: 0x00286398
		public bool ConsumeParticlesAndCheckComplete(float dt)
		{
			float amount = Mathf.Min(dt * base.def.consumeRate, base.def.numParticlesToOpen - this.m_particlesConsumed);
			float num = base.GetComponent<HighEnergyParticleStorage>().ConsumeAndGet(amount);
			this.m_particlesConsumed += num;
			this.UpdateMeter();
			return this.m_particlesConsumed >= base.def.numParticlesToOpen;
		}

		// Token: 0x060053F6 RID: 21494 RVA: 0x000DB154 File Offset: 0x000D9354
		public void UpdateMeter()
		{
			this.m_meter.SetPositionPercent(this.GetAmountComplete());
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x000DB167 File Offset: 0x000D9367
		private float GetAmountComplete()
		{
			return Mathf.Min(this.m_particlesConsumed / base.def.numParticlesToOpen, 1f);
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x000DB185 File Offset: 0x000D9385
		public float GetPercentComplete()
		{
			return this.GetAmountComplete() * 100f;
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x00288204 File Offset: 0x00286404
		public void CreateBeamFX()
		{
			Vector3 position = base.gameObject.transform.position;
			position.y += 3.25f;
			Quaternion rotation = Quaternion.Euler(-90f, 90f, 0f);
			Util.KInstantiate(EffectPrefabs.Instance.OpenTemporalTearBeam, position, rotation, base.gameObject, null, true, 0);
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x000DB193 File Offset: 0x000D9393
		public void OpenTemporalTear()
		{
			ClusterManager.Instance.GetClusterPOIManager().RevealTemporalTear();
			ClusterManager.Instance.GetClusterPOIManager().OpenTemporalTear(this.GetMyWorldId());
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060053FB RID: 21499 RVA: 0x000DB1B9 File Offset: 0x000D93B9
		public string SidescreenButtonText
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TEXT;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060053FC RID: 21500 RVA: 0x000DB1C5 File Offset: 0x000D93C5
		public string SidescreenButtonTooltip
		{
			get
			{
				return BUILDINGS.PREFABS.TEMPORALTEAROPENER.SIDESCREEN.TOOLTIP;
			}
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x000DB1D1 File Offset: 0x000D93D1
		public bool SidescreenEnabled()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x000DB1D1 File Offset: 0x000D93D1
		public bool SidescreenButtonInteractable()
		{
			return this.GetCurrentState() == base.sm.ready || DebugHandler.InstantBuildMode;
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x00288264 File Offset: 0x00286464
		public void OnSidescreenButtonPressed()
		{
			ConfirmDialogScreen component = GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay).GetComponent<ConfirmDialogScreen>();
			string text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_MESSAGE;
			System.Action on_confirm = delegate()
			{
				this.FireTemporalTearOpener(base.smi);
			};
			System.Action on_cancel = delegate()
			{
			};
			string configurable_text = null;
			System.Action on_configurable_clicked = null;
			string confirm_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CONFIRM;
			string cancel_text = UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_CANCEL;
			component.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, UI.UISIDESCREENS.TEMPORALTEARSIDESCREEN.CONFIRM_POPUP_TITLE, confirm_text, cancel_text, null);
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x000DB1ED File Offset: 0x000D93ED
		private void FireTemporalTearOpener(TemporalTearOpener.Instance smi)
		{
			smi.GoTo(base.sm.opening_tear_beam_pre);
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride text)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x000AFE89 File Offset: 0x000AE089
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x04003B2F RID: 15151
		[Serialize]
		private float m_particlesConsumed;

		// Token: 0x04003B30 RID: 15152
		private MeterController m_meter;
	}
}
