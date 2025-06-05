using System;
using UnityEngine;

// Token: 0x02000A3D RID: 2621
public class GasAndLiquidConsumerMonitor : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>
{
	// Token: 0x06002F67 RID: 12135 RVA: 0x00205A58 File Offset: 0x00203C58
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.cooldown;
		this.cooldown.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).ScheduleGoTo((GasAndLiquidConsumerMonitor.Instance smi) => UnityEngine.Random.Range(smi.def.minCooldown, smi.def.maxCooldown), this.satisfied);
		this.satisfied.Enter("ClearTargetCell", delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.ClearTargetCell();
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.looking, false);
		this.looking.ToggleBehaviour((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.behaviourTag, (GasAndLiquidConsumerMonitor.Instance smi) => smi.targetCell != -1, delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.GoTo(this.cooldown);
		}).TagTransition((GasAndLiquidConsumerMonitor.Instance smi) => smi.def.transitionTag, this.satisfied, true).PreBrainUpdate(delegate(GasAndLiquidConsumerMonitor.Instance smi)
		{
			smi.FindElement();
		});
	}

	// Token: 0x04002089 RID: 8329
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State cooldown;

	// Token: 0x0400208A RID: 8330
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State satisfied;

	// Token: 0x0400208B RID: 8331
	private GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.State looking;

	// Token: 0x02000A3E RID: 2622
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x0400208C RID: 8332
		public Tag[] transitionTag = new Tag[]
		{
			GameTags.Creatures.Hungry
		};

		// Token: 0x0400208D RID: 8333
		public Tag behaviourTag = GameTags.Creatures.WantsToEat;

		// Token: 0x0400208E RID: 8334
		public float minCooldown = 5f;

		// Token: 0x0400208F RID: 8335
		public float maxCooldown = 5f;

		// Token: 0x04002090 RID: 8336
		public Diet diet;

		// Token: 0x04002091 RID: 8337
		public float consumptionRate = 0.5f;

		// Token: 0x04002092 RID: 8338
		public Tag consumableElementTag = Tag.Invalid;
	}

	// Token: 0x02000A3F RID: 2623
	public new class Instance : GameStateMachine<GasAndLiquidConsumerMonitor, GasAndLiquidConsumerMonitor.Instance, IStateMachineTarget, GasAndLiquidConsumerMonitor.Def>.GameInstance
	{
		// Token: 0x06002F6B RID: 12139 RVA: 0x00205C2C File Offset: 0x00203E2C
		public Instance(IStateMachineTarget master, GasAndLiquidConsumerMonitor.Def def) : base(master, def)
		{
			this.navigator = base.smi.GetComponent<Navigator>();
			DebugUtil.Assert(base.smi.def.diet != null || this.storage != null, "GasAndLiquidConsumerMonitor needs either a diet or a storage");
		}

		// Token: 0x06002F6C RID: 12140 RVA: 0x000C33C7 File Offset: 0x000C15C7
		public void ClearTargetCell()
		{
			this.targetCell = -1;
			this.massUnavailableFrameCount = 0;
		}

		// Token: 0x06002F6D RID: 12141 RVA: 0x000C33D7 File Offset: 0x000C15D7
		public void FindElement()
		{
			this.targetCell = -1;
			this.FindTargetCell();
		}

		// Token: 0x06002F6E RID: 12142 RVA: 0x000C33E6 File Offset: 0x000C15E6
		public Element GetTargetElement()
		{
			return this.targetElement;
		}

		// Token: 0x06002F6F RID: 12143 RVA: 0x00205C84 File Offset: 0x00203E84
		public bool IsConsumableCell(int cell, out Element element)
		{
			element = Grid.Element[cell];
			bool flag = true;
			bool flag2 = true;
			if (base.smi.def.consumableElementTag != Tag.Invalid)
			{
				flag = element.HasTag(base.smi.def.consumableElementTag);
			}
			if (base.smi.def.diet != null)
			{
				flag2 = false;
				Diet.Info[] infos = base.smi.def.diet.infos;
				for (int i = 0; i < infos.Length; i++)
				{
					if (infos[i].IsMatch(element.tag))
					{
						flag2 = true;
						break;
					}
				}
			}
			return flag && flag2;
		}

		// Token: 0x06002F70 RID: 12144 RVA: 0x00205D24 File Offset: 0x00203F24
		public void FindTargetCell()
		{
			GasAndLiquidConsumerMonitor.ConsumableCellQuery consumableCellQuery = new GasAndLiquidConsumerMonitor.ConsumableCellQuery(base.smi, 25);
			this.navigator.RunQuery(consumableCellQuery);
			if (consumableCellQuery.success)
			{
				this.targetCell = consumableCellQuery.GetResultCell();
				this.targetElement = consumableCellQuery.targetElement;
			}
		}

		// Token: 0x06002F71 RID: 12145 RVA: 0x00205D6C File Offset: 0x00203F6C
		public void Consume(float dt)
		{
			int index = Game.Instance.massConsumedCallbackManager.Add(new Action<Sim.MassConsumedCallback, object>(GasAndLiquidConsumerMonitor.Instance.OnMassConsumedCallback), this, "GasAndLiquidConsumerMonitor").index;
			SimMessages.ConsumeMass(Grid.PosToCell(this), this.targetElement.id, base.def.consumptionRate * dt, 3, index);
		}

		// Token: 0x06002F72 RID: 12146 RVA: 0x000C33EE File Offset: 0x000C15EE
		private static void OnMassConsumedCallback(Sim.MassConsumedCallback mcd, object data)
		{
			((GasAndLiquidConsumerMonitor.Instance)data).OnMassConsumed(mcd);
		}

		// Token: 0x06002F73 RID: 12147 RVA: 0x00205DC8 File Offset: 0x00203FC8
		private void OnMassConsumed(Sim.MassConsumedCallback mcd)
		{
			if (!base.IsRunning())
			{
				return;
			}
			if (mcd.mass > 0f)
			{
				if (base.def.diet != null)
				{
					this.massUnavailableFrameCount = 0;
					Diet.Info dietInfo = base.def.diet.GetDietInfo(this.targetElement.tag);
					if (dietInfo == null)
					{
						return;
					}
					float calories = dietInfo.ConvertConsumptionMassToCalories(mcd.mass);
					CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = new CreatureCalorieMonitor.CaloriesConsumedEvent
					{
						tag = this.targetElement.tag,
						calories = calories
					};
					base.Trigger(-2038961714, caloriesConsumedEvent);
					return;
				}
				else if (this.storage != null)
				{
					this.storage.AddElement(this.targetElement.id, mcd.mass, mcd.temperature, mcd.diseaseIdx, mcd.diseaseCount, false, true);
					return;
				}
			}
			else
			{
				this.massUnavailableFrameCount++;
				if (this.massUnavailableFrameCount >= 2)
				{
					base.Trigger(801383139, null);
				}
			}
		}

		// Token: 0x04002093 RID: 8339
		public int targetCell = -1;

		// Token: 0x04002094 RID: 8340
		private Element targetElement;

		// Token: 0x04002095 RID: 8341
		private Navigator navigator;

		// Token: 0x04002096 RID: 8342
		private int massUnavailableFrameCount;

		// Token: 0x04002097 RID: 8343
		[MyCmpGet]
		private Storage storage;
	}

	// Token: 0x02000A40 RID: 2624
	public class ConsumableCellQuery : PathFinderQuery
	{
		// Token: 0x06002F74 RID: 12148 RVA: 0x000C33FC File Offset: 0x000C15FC
		public ConsumableCellQuery(GasAndLiquidConsumerMonitor.Instance smi, int maxIterations)
		{
			this.smi = smi;
			this.maxIterations = maxIterations;
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00205EC8 File Offset: 0x002040C8
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			int cell2 = Grid.CellAbove(cell);
			this.success = (this.smi.IsConsumableCell(cell, out this.targetElement) || (Grid.IsValidCell(cell2) && this.smi.IsConsumableCell(cell2, out this.targetElement)));
			if (!this.success)
			{
				int num = this.maxIterations - 1;
				this.maxIterations = num;
				return num <= 0;
			}
			return true;
		}

		// Token: 0x04002098 RID: 8344
		public bool success;

		// Token: 0x04002099 RID: 8345
		public Element targetElement;

		// Token: 0x0400209A RID: 8346
		private GasAndLiquidConsumerMonitor.Instance smi;

		// Token: 0x0400209B RID: 8347
		private int maxIterations;
	}
}
