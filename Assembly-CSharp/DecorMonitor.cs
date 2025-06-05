using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020015A3 RID: 5539
public class DecorMonitor : GameStateMachine<DecorMonitor, DecorMonitor.Instance>
{
	// Token: 0x06007325 RID: 29477 RVA: 0x0030E9D0 File Offset: 0x0030CBD0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleAttributeModifier("DecorSmoother", (DecorMonitor.Instance smi) => smi.GetDecorModifier(), (DecorMonitor.Instance smi) => true).Update("DecorSensing", delegate(DecorMonitor.Instance smi, float dt)
		{
			smi.Update(dt);
		}, UpdateRate.SIM_200ms, false).EventHandler(GameHashes.NewDay, (DecorMonitor.Instance smi) => GameClock.Instance, delegate(DecorMonitor.Instance smi)
		{
			smi.OnNewDay();
		});
	}

	// Token: 0x0400565C RID: 22108
	public static float MAXIMUM_DECOR_VALUE = 120f;

	// Token: 0x020015A4 RID: 5540
	public new class Instance : GameStateMachine<DecorMonitor, DecorMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007328 RID: 29480 RVA: 0x0030EAA8 File Offset: 0x0030CCA8
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.cycleTotalDecor = 2250f;
			this.amount = Db.Get().Amounts.Decor.Lookup(base.gameObject);
			this.modifier = new AttributeModifier(Db.Get().Amounts.Decor.deltaAttribute.Id, 1f, DUPLICANTS.NEEDS.DECOR.OBSERVED_DECOR, false, false, false);
		}

		// Token: 0x06007329 RID: 29481 RVA: 0x000EFE7D File Offset: 0x000EE07D
		public AttributeModifier GetDecorModifier()
		{
			return this.modifier;
		}

		// Token: 0x0600732A RID: 29482 RVA: 0x0030EBDC File Offset: 0x0030CDDC
		public void Update(float dt)
		{
			int cell = Grid.PosToCell(base.gameObject);
			if (!Grid.IsValidCell(cell))
			{
				return;
			}
			float decorAtCell = GameUtil.GetDecorAtCell(cell);
			this.cycleTotalDecor += decorAtCell * dt;
			float value = 0f;
			float num = 4.1666665f;
			if (Mathf.Abs(decorAtCell - this.amount.value) > 0.5f)
			{
				if (decorAtCell > this.amount.value)
				{
					value = 3f * num;
				}
				else if (decorAtCell < this.amount.value)
				{
					value = -num;
				}
			}
			else
			{
				this.amount.value = decorAtCell;
			}
			this.modifier.SetValue(value);
		}

		// Token: 0x0600732B RID: 29483 RVA: 0x0030EC80 File Offset: 0x0030CE80
		public void OnNewDay()
		{
			this.yesterdaysTotalDecor = this.cycleTotalDecor;
			this.cycleTotalDecor = 0f;
			float totalValue = base.gameObject.GetAttributes().Add(Db.Get().Attributes.DecorExpectation).GetTotalValue();
			float num = this.yesterdaysTotalDecor / 600f;
			num += totalValue;
			Effects component = base.gameObject.GetComponent<Effects>();
			foreach (KeyValuePair<float, string> keyValuePair in this.effectLookup)
			{
				if (num < keyValuePair.Key)
				{
					component.Add(keyValuePair.Value, true);
					break;
				}
			}
		}

		// Token: 0x0600732C RID: 29484 RVA: 0x000EFE85 File Offset: 0x000EE085
		public float GetTodaysAverageDecor()
		{
			return this.cycleTotalDecor / (GameClock.Instance.GetCurrentCycleAsPercentage() * 600f);
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x000EFE9E File Offset: 0x000EE09E
		public float GetYesterdaysAverageDecor()
		{
			return this.yesterdaysTotalDecor / 600f;
		}

		// Token: 0x0400565D RID: 22109
		[Serialize]
		private float cycleTotalDecor;

		// Token: 0x0400565E RID: 22110
		[Serialize]
		private float yesterdaysTotalDecor;

		// Token: 0x0400565F RID: 22111
		private AmountInstance amount;

		// Token: 0x04005660 RID: 22112
		private AttributeModifier modifier;

		// Token: 0x04005661 RID: 22113
		private List<KeyValuePair<float, string>> effectLookup = new List<KeyValuePair<float, string>>
		{
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * -0.25f, "DecorMinus1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0f, "Decor0"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.25f, "Decor1"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.5f, "Decor2"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE * 0.75f, "Decor3"),
			new KeyValuePair<float, string>(DecorMonitor.MAXIMUM_DECOR_VALUE, "Decor4"),
			new KeyValuePair<float, string>(float.MaxValue, "Decor5")
		};
	}
}
