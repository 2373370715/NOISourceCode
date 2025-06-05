using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000A42 RID: 2626
public class HugMonitor : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>
{
	// Token: 0x06002F80 RID: 12160 RVA: 0x00205F38 File Offset: 0x00204138
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.normal;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.root.Update(new Action<HugMonitor.Instance, float>(this.UpdateHugEggCooldownTimer), UpdateRate.SIM_1000ms, false).ToggleBehaviour(GameTags.Creatures.WantsToTendEgg, (HugMonitor.Instance smi) => smi.UpdateHasTarget(), delegate(HugMonitor.Instance smi)
		{
			smi.hugTarget = null;
		});
		this.normal.DefaultState(this.normal.idle).ParamTransition<float>(this.hugFrenzyTimer, this.hugFrenzy, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsGTZero);
		this.normal.idle.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		this.normal.hugReady.ToggleReactable(new Func<HugMonitor.Instance, Reactable>(this.GetHugReactable));
		GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State state = this.normal.hugReady.passiveHug.ParamTransition<float>(this.wantsHugCooldownTimer, this.normal.hugReady.seekingHug, GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.IsLTEZero).Update(new Action<HugMonitor.Instance, float>(this.UpdateWantsHugCooldownTimer), UpdateRate.SIM_1000ms, false);
		string name = CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME;
		string tooltip = CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP;
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		state.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, null, null, main);
		this.normal.hugReady.seekingHug.ToggleBehaviour(GameTags.Creatures.WantsAHug, (HugMonitor.Instance smi) => true, delegate(HugMonitor.Instance smi)
		{
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldownFailed, smi, false);
			smi.GoTo(this.normal.hugReady.passiveHug);
		});
		this.hugFrenzy.ParamTransition<float>(this.hugFrenzyTimer, this.normal, (HugMonitor.Instance smi, float p) => p <= 0f && !smi.IsHugging()).Update(new Action<HugMonitor.Instance, float>(this.UpdateHugFrenzyTimer), UpdateRate.SIM_1000ms, false).ToggleEffect((HugMonitor.Instance smi) => smi.frenzyEffect).ToggleLoopingSound(HugMonitor.soundPath, null, true, true, true).Enter(delegate(HugMonitor.Instance smi)
		{
			smi.hugParticleFx = Util.KInstantiate(EffectPrefabs.Instance.HugFrenzyFX, smi.master.transform.GetPosition() + smi.hugParticleOffset);
			smi.hugParticleFx.transform.SetParent(smi.master.transform);
			smi.hugParticleFx.SetActive(true);
		}).Exit(delegate(HugMonitor.Instance smi)
		{
			Util.KDestroyGameObject(smi.hugParticleFx);
			this.wantsHugCooldownTimer.Set(smi.def.hugFrenzyCooldown, smi, false);
		});
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x000C3473 File Offset: 0x000C1673
	private Reactable GetHugReactable(HugMonitor.Instance smi)
	{
		return new HugMinionReactable(smi.gameObject);
	}

	// Token: 0x06002F82 RID: 12162 RVA: 0x000C3480 File Offset: 0x000C1680
	private void UpdateWantsHugCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.wantsHugCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x000C349B File Offset: 0x000C169B
	private void UpdateHugEggCooldownTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugEggCooldownTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x000C34B6 File Offset: 0x000C16B6
	private void UpdateHugFrenzyTimer(HugMonitor.Instance smi, float dt)
	{
		this.hugFrenzyTimer.DeltaClamp(-dt, 0f, float.MaxValue, smi);
	}

	// Token: 0x040020A5 RID: 8357
	private static string soundPath = GlobalAssets.GetSound("Squirrel_hug_frenzyFX", false);

	// Token: 0x040020A6 RID: 8358
	private static Effect hugEffect;

	// Token: 0x040020A7 RID: 8359
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugFrenzyTimer;

	// Token: 0x040020A8 RID: 8360
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter wantsHugCooldownTimer;

	// Token: 0x040020A9 RID: 8361
	private StateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.FloatParameter hugEggCooldownTimer;

	// Token: 0x040020AA RID: 8362
	public HugMonitor.NormalStates normal;

	// Token: 0x040020AB RID: 8363
	public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State hugFrenzy;

	// Token: 0x02000A43 RID: 2627
	public class HUGTUNING
	{
		// Token: 0x040020AC RID: 8364
		public const float HUG_EGG_TIME = 15f;

		// Token: 0x040020AD RID: 8365
		public const float HUG_DUPE_WAIT = 60f;

		// Token: 0x040020AE RID: 8366
		public const float FRENZY_EGGS_PER_CYCLE = 6f;

		// Token: 0x040020AF RID: 8367
		public const float FRENZY_EGG_TRAVEL_TIME_BUFFER = 5f;

		// Token: 0x040020B0 RID: 8368
		public const float HUG_FRENZY_DURATION = 120f;
	}

	// Token: 0x02000A44 RID: 2628
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040020B1 RID: 8369
		public float hugsPerCycle = 2f;

		// Token: 0x040020B2 RID: 8370
		public float scanningInterval = 30f;

		// Token: 0x040020B3 RID: 8371
		public float hugFrenzyDuration = 120f;

		// Token: 0x040020B4 RID: 8372
		public float hugFrenzyCooldown = 480f;

		// Token: 0x040020B5 RID: 8373
		public float hugFrenzyCooldownFailed = 120f;

		// Token: 0x040020B6 RID: 8374
		public float scanningIntervalFrenzy = 15f;

		// Token: 0x040020B7 RID: 8375
		public int maxSearchCost = 30;
	}

	// Token: 0x02000A45 RID: 2629
	public class HugReadyStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x040020B8 RID: 8376
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State passiveHug;

		// Token: 0x040020B9 RID: 8377
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State seekingHug;
	}

	// Token: 0x02000A46 RID: 2630
	public class NormalStates : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State
	{
		// Token: 0x040020BA RID: 8378
		public GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.State idle;

		// Token: 0x040020BB RID: 8379
		public HugMonitor.HugReadyStates hugReady;
	}

	// Token: 0x02000A47 RID: 2631
	public new class Instance : GameStateMachine<HugMonitor, HugMonitor.Instance, IStateMachineTarget, HugMonitor.Def>.GameInstance
	{
		// Token: 0x06002F8D RID: 12173 RVA: 0x0020621C File Offset: 0x0020441C
		public Instance(IStateMachineTarget master, HugMonitor.Def def) : base(master, def)
		{
			this.frenzyEffect = Db.Get().effects.Get("HuggingFrenzy");
			this.RefreshSearchTime();
			if (HugMonitor.hugEffect == null)
			{
				HugMonitor.hugEffect = Db.Get().effects.Get("EggHug");
			}
			base.smi.sm.wantsHugCooldownTimer.Set(UnityEngine.Random.Range(base.smi.def.hugFrenzyCooldownFailed, base.smi.def.hugFrenzyCooldown), base.smi, false);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x002062B4 File Offset: 0x002044B4
		private void RefreshSearchTime()
		{
			if (this.hugTarget == null)
			{
				base.smi.sm.hugEggCooldownTimer.Set(this.GetScanningInterval(), base.smi, false);
				return;
			}
			base.smi.sm.hugEggCooldownTimer.Set(this.GetHugInterval(), base.smi, false);
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000C354A File Offset: 0x000C174A
		private float GetScanningInterval()
		{
			if (!this.IsHuggingFrenzy())
			{
				return base.def.scanningInterval;
			}
			return base.def.scanningIntervalFrenzy;
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000C356B File Offset: 0x000C176B
		private float GetHugInterval()
		{
			if (this.IsHuggingFrenzy())
			{
				return 0f;
			}
			return 600f / base.def.hugsPerCycle;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000C358C File Offset: 0x000C178C
		public bool IsHuggingFrenzy()
		{
			return base.smi.GetCurrentState() == base.smi.sm.hugFrenzy;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000C35AB File Offset: 0x000C17AB
		public bool IsHugging()
		{
			return base.smi.GetSMI<AnimInterruptMonitor.Instance>().anims != null;
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00206318 File Offset: 0x00204518
		public bool UpdateHasTarget()
		{
			if (this.hugTarget == null)
			{
				if (base.smi.sm.hugEggCooldownTimer.Get(base.smi) > 0f)
				{
					return false;
				}
				this.FindEgg();
				this.RefreshSearchTime();
			}
			return this.hugTarget != null;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x00206370 File Offset: 0x00204570
		public void EnterHuggingFrenzy()
		{
			base.smi.sm.hugFrenzyTimer.Set(base.smi.def.hugFrenzyDuration, base.smi, false);
			base.smi.sm.hugEggCooldownTimer.Set(0f, base.smi, false);
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x002063CC File Offset: 0x002045CC
		private void FindEgg()
		{
			int cell = Grid.PosToCell(base.gameObject);
			CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
			int num = base.def.maxSearchCost;
			this.hugTarget = null;
			if (cavityForCell != null)
			{
				foreach (KPrefabID kprefabID in cavityForCell.eggs)
				{
					if (!kprefabID.HasTag(GameTags.Creatures.ReservedByCreature) && !kprefabID.GetComponent<Effects>().HasEffect(HugMonitor.hugEffect))
					{
						int num2 = Grid.PosToCell(kprefabID);
						if (kprefabID.HasTag(GameTags.Stored))
						{
							GameObject gameObject;
							KPrefabID kprefabID2;
							if (!Grid.ObjectLayers[1].TryGetValue(num2, out gameObject) || !gameObject.TryGetComponent<KPrefabID>(out kprefabID2) || !kprefabID2.IsPrefabID("EggIncubator"))
							{
								continue;
							}
							num2 = Grid.PosToCell(gameObject);
							kprefabID = kprefabID2;
						}
						int navigationCost = this.navigator.GetNavigationCost(num2);
						if (navigationCost != -1 && navigationCost < num)
						{
							this.hugTarget = kprefabID;
							num = navigationCost;
						}
					}
				}
			}
		}

		// Token: 0x040020BC RID: 8380
		public GameObject hugParticleFx;

		// Token: 0x040020BD RID: 8381
		public Vector3 hugParticleOffset;

		// Token: 0x040020BE RID: 8382
		public Effect frenzyEffect;

		// Token: 0x040020BF RID: 8383
		public KPrefabID hugTarget;

		// Token: 0x040020C0 RID: 8384
		[MyCmpGet]
		private Navigator navigator;
	}
}
