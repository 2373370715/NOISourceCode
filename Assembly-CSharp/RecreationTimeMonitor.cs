using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001612 RID: 5650
public class RecreationTimeMonitor : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>
{
	// Token: 0x06007507 RID: 29959 RVA: 0x003140D0 File Offset: 0x003122D0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.idle;
		this.idle.EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
		this.bonusActive.ToggleEffect((RecreationTimeMonitor.Instance smi) => smi.moraleEffect).EventHandler(GameHashes.ScheduleBlocksTick, delegate(RecreationTimeMonitor.Instance smi)
		{
			smi.OnScheduleBlocksTick();
		}).Update(delegate(RecreationTimeMonitor.Instance smi, float dt)
		{
			smi.RefreshTimes();
		}, UpdateRate.SIM_200ms, false);
	}

	// Token: 0x040057E7 RID: 22503
	public const int MAX_BONUS = 5;

	// Token: 0x040057E8 RID: 22504
	public const float BONUS_DURATION_STANDARD = 600f;

	// Token: 0x040057E9 RID: 22505
	public const float BONUS_DURATION_BIONICS = 1800f;

	// Token: 0x040057EA RID: 22506
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State idle;

	// Token: 0x040057EB RID: 22507
	public GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.State bonusActive;

	// Token: 0x02001613 RID: 5651
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001614 RID: 5652
	public new class Instance : GameStateMachine<RecreationTimeMonitor, RecreationTimeMonitor.Instance, IStateMachineTarget, RecreationTimeMonitor.Def>.GameInstance
	{
		// Token: 0x0600750A RID: 29962 RVA: 0x003141BC File Offset: 0x003123BC
		public Instance(IStateMachineTarget master, RecreationTimeMonitor.Def def) : base(master, def)
		{
			this.bonus_duration = ((base.gameObject.PrefabID() == BionicMinionConfig.ID) ? 1800f : 600f);
			this.schedulable = master.GetComponent<Schedulable>();
			this.moraleModifier = new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, 0f, delegate()
			{
				if (Mathf.Clamp(this.moraleAddedTimes.Count - 1, 0, 5) == 5)
				{
					return DUPLICANTS.MODIFIERS.BREAK_BONUS.MAX_NAME;
				}
				return DUPLICANTS.MODIFIERS.BREAK_BONUS.NAME;
			}, false, false);
			this.moraleEffect.Add(this.moraleModifier);
			if ((SaveLoader.Instance.GameInfo.saveMajorVersion != 0 || SaveLoader.Instance.GameInfo.saveMinorVersion != 0) && SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 35))
			{
				this.RestoreFromSchedule();
			}
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x000F160B File Offset: 0x000EF80B
		public override void StartSM()
		{
			base.StartSM();
			this.RefreshTimes();
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x003142D4 File Offset: 0x003124D4
		public void RefreshTimes()
		{
			for (int i = this.moraleAddedTimes.Count - 1; i >= 0; i--)
			{
				if (GameClock.Instance.GetTime() - this.moraleAddedTimes[i] > this.bonus_duration)
				{
					this.moraleAddedTimes.RemoveAt(i);
				}
			}
			int num = Math.Clamp(this.moraleAddedTimes.Count - 1, 0, 5);
			this.moraleModifier.SetValue((float)num);
			if (num > 0)
			{
				if (base.smi.GetCurrentState() != base.smi.sm.bonusActive)
				{
					base.smi.GoTo(base.smi.sm.bonusActive);
					return;
				}
			}
			else if (base.smi.GetCurrentState() != base.smi.sm.idle)
			{
				base.smi.GoTo(base.smi.sm.idle);
			}
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x003143BC File Offset: 0x003125BC
		public void OnScheduleBlocksTick()
		{
			if (ScheduleManager.Instance.GetSchedule(this.schedulable).GetPreviousScheduleBlock().GroupId == Db.Get().ScheduleGroups.Recreation.Id)
			{
				this.moraleAddedTimes.Add(GameClock.Instance.GetTime());
			}
		}

		// Token: 0x0600750E RID: 29966 RVA: 0x00314414 File Offset: 0x00312614
		private void RestoreFromSchedule()
		{
			Effects component = base.GetComponent<Effects>();
			foreach (string effect_id in new string[]
			{
				"Break1",
				"Break2",
				"Break3",
				"Break4",
				"Break5"
			})
			{
				if (component.HasEffect(effect_id))
				{
					component.Remove(effect_id);
				}
			}
			Schedule schedule = ScheduleManager.Instance.GetSchedule(this.schedulable);
			List<ScheduleBlock> blocks = schedule.GetBlocks();
			int currentBlockIdx = schedule.GetCurrentBlockIdx();
			int num = 24;
			if (GameClock.Instance.GetTime() <= this.bonus_duration)
			{
				num = Math.Min(currentBlockIdx, Mathf.FloorToInt(GameClock.Instance.GetTime() / 25f));
			}
			for (int j = currentBlockIdx - num; j < currentBlockIdx; j++)
			{
				int k = j;
				global::Debug.Assert(blocks.Count > 0);
				while (k < 0)
				{
					k += blocks.Count;
				}
				if (blocks[k].GroupId == Db.Get().ScheduleGroups.Recreation.Id)
				{
					int num2;
					if (k > currentBlockIdx)
					{
						num2 = blocks.Count - k + currentBlockIdx - 1;
					}
					else
					{
						num2 = currentBlockIdx - k - 1;
					}
					float num3 = (float)num2 * 25f;
					float num4 = GameClock.Instance.GetTime() - num3;
					global::Debug.Assert(num4 > 0f);
					this.moraleAddedTimes.Add(num4);
				}
			}
		}

		// Token: 0x040057EC RID: 22508
		[Serialize]
		public List<float> moraleAddedTimes = new List<float>();

		// Token: 0x040057ED RID: 22509
		public Effect moraleEffect = new Effect("RecTimeEffect", "Rec Time Effect", "Rec Time Effect Description", 0f, false, false, false, null, -1f, 0f, null, "");

		// Token: 0x040057EE RID: 22510
		private Schedulable schedulable;

		// Token: 0x040057EF RID: 22511
		private AttributeModifier moraleModifier;

		// Token: 0x040057F0 RID: 22512
		private int shiftValue;

		// Token: 0x040057F1 RID: 22513
		private float bonus_duration;
	}
}
