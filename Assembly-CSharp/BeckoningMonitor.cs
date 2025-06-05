using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02001165 RID: 4453
public class BeckoningMonitor : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>
{
	// Token: 0x06005AD4 RID: 23252 RVA: 0x002A4978 File Offset: 0x002A2B78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.CaloriesConsumed, delegate(BeckoningMonitor.Instance smi, object data)
		{
			smi.OnCaloriesConsumed(data);
		}).ToggleBehaviour(GameTags.Creatures.WantsToBeckon, (BeckoningMonitor.Instance smi) => smi.IsReadyToBeckon(), null).Update(delegate(BeckoningMonitor.Instance smi, float dt)
		{
			smi.UpdateBlockedStatusItem();
		}, UpdateRate.SIM_1000ms, false);
	}

	// Token: 0x02001166 RID: 4454
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06005AD6 RID: 23254 RVA: 0x000DF8DD File Offset: 0x000DDADD
		public override void Configure(GameObject prefab)
		{
			prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Beckoning.Id);
		}

		// Token: 0x040040A6 RID: 16550
		public float caloriesPerCycle;

		// Token: 0x040040A7 RID: 16551
		public string effectId = "MooWellFed";
	}

	// Token: 0x02001167 RID: 4455
	public new class Instance : GameStateMachine<BeckoningMonitor, BeckoningMonitor.Instance, IStateMachineTarget, BeckoningMonitor.Def>.GameInstance
	{
		// Token: 0x06005AD8 RID: 23256 RVA: 0x000DF916 File Offset: 0x000DDB16
		public Instance(IStateMachineTarget master, BeckoningMonitor.Def def) : base(master, def)
		{
			this.beckoning = Db.Get().Amounts.Beckoning.Lookup(base.gameObject);
		}

		// Token: 0x06005AD9 RID: 23257 RVA: 0x002A4A10 File Offset: 0x002A2C10
		private bool IsSpaceVisible()
		{
			int num = Grid.PosToCell(this);
			return Grid.IsValidCell(num) && Grid.ExposedToSunlight[num] > 0;
		}

		// Token: 0x06005ADA RID: 23258 RVA: 0x000DF940 File Offset: 0x000DDB40
		private bool IsBeckoningAvailable()
		{
			return base.smi.beckoning.value >= base.smi.beckoning.GetMax();
		}

		// Token: 0x06005ADB RID: 23259 RVA: 0x000DF967 File Offset: 0x000DDB67
		public bool IsReadyToBeckon()
		{
			return this.IsBeckoningAvailable() && this.IsSpaceVisible();
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x002A4A3C File Offset: 0x002A2C3C
		public void UpdateBlockedStatusItem()
		{
			bool flag = this.IsSpaceVisible();
			if (!flag && this.IsBeckoningAvailable() && this.beckoningBlockedHandle == Guid.Empty)
			{
				this.beckoningBlockedHandle = this.kselectable.AddStatusItem(Db.Get().CreatureStatusItems.BeckoningBlocked, null);
				return;
			}
			if (flag)
			{
				this.beckoningBlockedHandle = this.kselectable.RemoveStatusItem(this.beckoningBlockedHandle, false);
			}
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x002A4AAC File Offset: 0x002A2CAC
		public void OnCaloriesConsumed(object data)
		{
			CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent)data;
			EffectInstance effectInstance = this.effects.Get(base.smi.def.effectId);
			if (effectInstance == null)
			{
				effectInstance = this.effects.Add(base.smi.def.effectId, true);
			}
			effectInstance.timeRemaining += caloriesConsumedEvent.calories / base.smi.def.caloriesPerCycle * 600f;
		}

		// Token: 0x040040A8 RID: 16552
		private AmountInstance beckoning;

		// Token: 0x040040A9 RID: 16553
		[MyCmpGet]
		private Effects effects;

		// Token: 0x040040AA RID: 16554
		[MyCmpGet]
		public KSelectable kselectable;

		// Token: 0x040040AB RID: 16555
		private Guid beckoningBlockedHandle;
	}
}
