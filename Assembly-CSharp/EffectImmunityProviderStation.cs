using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
public class EffectImmunityProviderStation<StateMachineInstanceType> : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def> where StateMachineInstanceType : EffectImmunityProviderStation<StateMachineInstanceType>.BaseInstance
{
	// Token: 0x06003085 RID: 12421 RVA: 0x00209E44 File Offset: 0x00208044
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.inactive;
		this.inactive.EventTransition(GameHashes.ActiveChanged, this.active, (StateMachineInstanceType smi) => smi.GetComponent<Operational>().IsActive);
		this.active.EventTransition(GameHashes.ActiveChanged, this.inactive, (StateMachineInstanceType smi) => !smi.GetComponent<Operational>().IsActive);
	}

	// Token: 0x0400214F RID: 8527
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State inactive;

	// Token: 0x04002150 RID: 8528
	public GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.State active;

	// Token: 0x02000A70 RID: 2672
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06003087 RID: 12423 RVA: 0x000C3FB9 File Offset: 0x000C21B9
		public virtual string[] DefaultAnims()
		{
			return new string[]
			{
				"",
				"",
				""
			};
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000C21EF File Offset: 0x000C03EF
		public virtual string DefaultAnimFileName()
		{
			return "anim_warmup_kanim";
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000C3FD9 File Offset: 0x000C21D9
		public string[] GetAnimNames()
		{
			if (this.overrideAnims != null)
			{
				return this.overrideAnims;
			}
			return this.DefaultAnims();
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000C3FF0 File Offset: 0x000C21F0
		public string GetAnimFileName(GameObject entity)
		{
			if (this.overrideFileName != null)
			{
				return this.overrideFileName(entity);
			}
			return this.DefaultAnimFileName();
		}

		// Token: 0x04002151 RID: 8529
		public Action<GameObject, StateMachineInstanceType> onEffectApplied;

		// Token: 0x04002152 RID: 8530
		public Func<GameObject, bool> specialRequirements;

		// Token: 0x04002153 RID: 8531
		public Func<GameObject, string> overrideFileName;

		// Token: 0x04002154 RID: 8532
		public string[] overrideAnims;

		// Token: 0x04002155 RID: 8533
		public CellOffset[][] range;
	}

	// Token: 0x02000A71 RID: 2673
	public abstract class BaseInstance : GameStateMachine<EffectImmunityProviderStation<StateMachineInstanceType>, StateMachineInstanceType, IStateMachineTarget, EffectImmunityProviderStation<StateMachineInstanceType>.Def>.GameInstance
	{
		// Token: 0x0600308C RID: 12428 RVA: 0x000C400D File Offset: 0x000C220D
		public string GetAnimFileName(GameObject entity)
		{
			return base.def.GetAnimFileName(entity);
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600308D RID: 12429 RVA: 0x000C401B File Offset: 0x000C221B
		public string PreAnimName
		{
			get
			{
				return base.def.GetAnimNames()[0];
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x000C402A File Offset: 0x000C222A
		public string LoopAnimName
		{
			get
			{
				return base.def.GetAnimNames()[1];
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600308F RID: 12431 RVA: 0x000C4039 File Offset: 0x000C2239
		public string PstAnimName
		{
			get
			{
				return base.def.GetAnimNames()[2];
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06003090 RID: 12432 RVA: 0x000C4048 File Offset: 0x000C2248
		public bool CanBeUsed
		{
			get
			{
				return this.IsActive && (base.def.specialRequirements == null || base.def.specialRequirements(base.gameObject));
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x000C4079 File Offset: 0x000C2279
		protected bool IsActive
		{
			get
			{
				return base.IsInsideState(base.sm.active);
			}
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000C408C File Offset: 0x000C228C
		public BaseInstance(IStateMachineTarget master, EffectImmunityProviderStation<StateMachineInstanceType>.Def def) : base(master, def)
		{
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x00209ECC File Offset: 0x002080CC
		public int GetBestAvailableCell(Navigator dupeLooking, out int _cost)
		{
			_cost = int.MaxValue;
			if (!this.CanBeUsed)
			{
				return Grid.InvalidCell;
			}
			int num = Grid.PosToCell(this);
			int num2 = Grid.InvalidCell;
			if (base.def.range != null)
			{
				for (int i = 0; i < base.def.range.GetLength(0); i++)
				{
					int num3 = int.MaxValue;
					for (int j = 0; j < base.def.range[i].Length; j++)
					{
						int num4 = Grid.OffsetCell(num, base.def.range[i][j]);
						if (dupeLooking.CanReach(num4))
						{
							int navigationCost = dupeLooking.GetNavigationCost(num4);
							if (navigationCost < num3)
							{
								num3 = navigationCost;
								num2 = num4;
							}
						}
					}
					if (num2 != Grid.InvalidCell)
					{
						_cost = num3;
						break;
					}
				}
				return num2;
			}
			if (dupeLooking.CanReach(num))
			{
				_cost = dupeLooking.GetNavigationCost(num);
				return num;
			}
			return Grid.InvalidCell;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x00209FB0 File Offset: 0x002081B0
		public void ApplyImmunityEffect(GameObject target, bool triggerEvents = true)
		{
			Effects component = target.GetComponent<Effects>();
			if (component == null)
			{
				return;
			}
			this.ApplyImmunityEffect(component);
			if (triggerEvents)
			{
				Action<GameObject, StateMachineInstanceType> onEffectApplied = base.def.onEffectApplied;
				if (onEffectApplied == null)
				{
					return;
				}
				onEffectApplied(component.gameObject, (StateMachineInstanceType)((object)this));
			}
		}

		// Token: 0x06003095 RID: 12437
		protected abstract void ApplyImmunityEffect(Effects target);

		// Token: 0x06003096 RID: 12438 RVA: 0x000C4096 File Offset: 0x000C2296
		public override void StartSM()
		{
			Components.EffectImmunityProviderStations.Add(this);
			base.StartSM();
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000C40A9 File Offset: 0x000C22A9
		protected override void OnCleanUp()
		{
			Components.EffectImmunityProviderStations.Remove(this);
			base.OnCleanUp();
		}
	}
}
