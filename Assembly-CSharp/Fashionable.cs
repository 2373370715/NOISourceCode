using System;

// Token: 0x0200169C RID: 5788
[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
	// Token: 0x0600778B RID: 30603 RVA: 0x000F318F File Offset: 0x000F138F
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0600778C RID: 30604 RVA: 0x0031BE08 File Offset: 0x0031A008
	protected bool IsUncomfortable()
	{
		ClothingWearer component = base.GetComponent<ClothingWearer>();
		return component != null && component.currentClothing.decorMod <= 0;
	}

	// Token: 0x0200169D RID: 5789
	public class StatesInstance : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance
	{
		// Token: 0x0600778E RID: 30606 RVA: 0x000F31A4 File Offset: 0x000F13A4
		public StatesInstance(Fashionable master) : base(master)
		{
		}
	}

	// Token: 0x0200169E RID: 5790
	public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
	{
		// Token: 0x0600778F RID: 30607 RVA: 0x0031BE38 File Offset: 0x0031A038
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.satisfied;
			this.root.EventHandler(GameHashes.EquippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			}).EventHandler(GameHashes.UnequippedItemEquipper, delegate(Fashionable.StatesInstance smi)
			{
				if (smi.master.IsUncomfortable())
				{
					smi.GoTo(this.suffering);
					return;
				}
				smi.GoTo(this.satisfied);
			});
			this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable, null);
			this.satisfied.DoNothing();
		}

		// Token: 0x04005A08 RID: 23048
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;

		// Token: 0x04005A09 RID: 23049
		public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;
	}
}
