using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000D4D RID: 3405
public class CritterCondo : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>
{
	// Token: 0x0600420A RID: 16906 RVA: 0x0024DFE4 File Offset: 0x0024C1E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.inoperational;
		this.inoperational.PlayAnim("off").EventTransition(GameHashes.UpdateRoom, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.operational, new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational));
		this.operational.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.UpdateRoom, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational))).EventTransition(GameHashes.OperationalChanged, this.inoperational, GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Not(new StateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.Transition.ConditionCallback(CritterCondo.IsOperational)));
	}

	// Token: 0x0600420B RID: 16907 RVA: 0x000CF28E File Offset: 0x000CD48E
	private static bool IsOperational(CritterCondo.Instance smi)
	{
		return smi.def.IsCritterCondoOperationalCb(smi);
	}

	// Token: 0x04002D8A RID: 11658
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State inoperational;

	// Token: 0x04002D8B RID: 11659
	public GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.State operational;

	// Token: 0x02000D4E RID: 3406
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600420D RID: 16909 RVA: 0x000CE880 File Offset: 0x000CCA80
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			return new List<Descriptor>();
		}

		// Token: 0x04002D8C RID: 11660
		public Func<CritterCondo.Instance, bool> IsCritterCondoOperationalCb;

		// Token: 0x04002D8D RID: 11661
		public StatusItem moveToStatusItem;

		// Token: 0x04002D8E RID: 11662
		public StatusItem interactStatusItem;

		// Token: 0x04002D8F RID: 11663
		public Tag condoTag = "CritterCondo";

		// Token: 0x04002D90 RID: 11664
		public string effectId;
	}

	// Token: 0x02000D4F RID: 3407
	public new class Instance : GameStateMachine<CritterCondo, CritterCondo.Instance, IStateMachineTarget, CritterCondo.Def>.GameInstance
	{
		// Token: 0x0600420F RID: 16911 RVA: 0x000CF2C1 File Offset: 0x000CD4C1
		public Instance(IStateMachineTarget master, CritterCondo.Def def) : base(master, def)
		{
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x000CF2CB File Offset: 0x000CD4CB
		public override void StartSM()
		{
			base.StartSM();
			Components.CritterCondos.Add(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x000CF2E9 File Offset: 0x000CD4E9
		protected override void OnCleanUp()
		{
			Components.CritterCondos.Remove(base.smi.GetMyWorldId(), this);
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x000CF301 File Offset: 0x000CD501
		public bool IsReserved()
		{
			return base.HasTag(GameTags.Creatures.ReservedByCreature);
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x0024E098 File Offset: 0x0024C298
		public void SetReserved(bool isReserved)
		{
			if (isReserved)
			{
				base.GetComponent<KPrefabID>().SetTag(GameTags.Creatures.ReservedByCreature, true);
				return;
			}
			if (base.HasTag(GameTags.Creatures.ReservedByCreature))
			{
				base.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.ReservedByCreature);
				return;
			}
			global::Debug.LogWarningFormat(base.smi.gameObject, "Tried to unreserve a condo that wasn't reserved", Array.Empty<object>());
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x000CF30E File Offset: 0x000CD50E
		public int GetInteractStartCell()
		{
			return Grid.PosToCell(this);
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x000CF316 File Offset: 0x000CD516
		public bool CanBeReserved()
		{
			return !this.IsReserved() && CritterCondo.IsOperational(this);
		}
	}
}
