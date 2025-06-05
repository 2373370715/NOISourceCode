using System;

// Token: 0x02001666 RID: 5734
public class ToiletMonitor : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance>
{
	// Token: 0x06007688 RID: 30344 RVA: 0x00318988 File Offset: 0x00316B88
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.EventHandler(GameHashes.ToiletSensorChanged, delegate(ToiletMonitor.Instance smi)
		{
			smi.RefreshStatusItem();
		}).Exit("ClearStatusItem", delegate(ToiletMonitor.Instance smi)
		{
			smi.ClearStatusItem();
		});
	}

	// Token: 0x04005926 RID: 22822
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x04005927 RID: 22823
	public GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.State unsatisfied;

	// Token: 0x02001667 RID: 5735
	public new class Instance : GameStateMachine<ToiletMonitor, ToiletMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x0600768A RID: 30346 RVA: 0x000F275B File Offset: 0x000F095B
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.toiletSensor = base.GetComponent<Sensors>().GetSensor<ToiletSensor>();
		}

		// Token: 0x0600768B RID: 30347 RVA: 0x003189F8 File Offset: 0x00316BF8
		public void RefreshStatusItem()
		{
			StatusItem status_item = null;
			if (!this.toiletSensor.AreThereAnyToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoToilets;
			}
			else if (!this.toiletSensor.AreThereAnyUsableToilets())
			{
				status_item = Db.Get().DuplicantStatusItems.NoUsableToilets;
			}
			else if (this.toiletSensor.GetNearestUsableToilet() == null)
			{
				status_item = Db.Get().DuplicantStatusItems.ToiletUnreachable;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, status_item, null);
		}

		// Token: 0x0600768C RID: 30348 RVA: 0x000F2775 File Offset: 0x000F0975
		public void ClearStatusItem()
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Toilet, null, null);
		}

		// Token: 0x04005928 RID: 22824
		private ToiletSensor toiletSensor;
	}
}
