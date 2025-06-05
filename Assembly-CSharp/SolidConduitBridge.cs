using System;
using UnityEngine;

// Token: 0x02000FC5 RID: 4037
[AddComponentMenu("KMonoBehaviour/scripts/SolidConduitBridge")]
public class SolidConduitBridge : ConduitBridgeBase
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06005152 RID: 20818 RVA: 0x000D9726 File Offset: 0x000D7926
	public bool IsDispensing
	{
		get
		{
			return this.dispensing;
		}
	}

	// Token: 0x06005153 RID: 20819 RVA: 0x0027FC60 File Offset: 0x0027DE60
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Building component = base.GetComponent<Building>();
		this.inputCell = component.GetUtilityInputCell();
		this.outputCell = component.GetUtilityOutputCell();
		SolidConduit.GetFlowManager().AddConduitUpdater(new Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
	}

	// Token: 0x06005154 RID: 20820 RVA: 0x000D972E File Offset: 0x000D792E
	protected override void OnCleanUp()
	{
		SolidConduit.GetFlowManager().RemoveConduitUpdater(new Action<float>(this.ConduitUpdate));
		base.OnCleanUp();
	}

	// Token: 0x06005155 RID: 20821 RVA: 0x0027FCAC File Offset: 0x0027DEAC
	private void ConduitUpdate(float dt)
	{
		this.dispensing = false;
		float num = 0f;
		if (this.operational && !this.operational.IsOperational)
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		SolidConduitFlow flowManager = SolidConduit.GetFlowManager();
		if (!flowManager.HasConduit(this.inputCell) || !flowManager.HasConduit(this.outputCell))
		{
			base.SendEmptyOnMassTransfer();
			return;
		}
		if (flowManager.IsConduitFull(this.inputCell) && flowManager.IsConduitEmpty(this.outputCell))
		{
			Pickupable pickupable = flowManager.GetPickupable(flowManager.GetContents(this.inputCell).pickupableHandle);
			if (pickupable == null)
			{
				flowManager.RemovePickupable(this.inputCell);
				base.SendEmptyOnMassTransfer();
				return;
			}
			float num2 = pickupable.PrimaryElement.Mass;
			if (this.desiredMassTransfer != null)
			{
				num2 = this.desiredMassTransfer(dt, pickupable.PrimaryElement.Element.id, pickupable.PrimaryElement.Mass, pickupable.PrimaryElement.Temperature, pickupable.PrimaryElement.DiseaseIdx, pickupable.PrimaryElement.DiseaseCount, pickupable);
			}
			if (num2 == 0f)
			{
				base.SendEmptyOnMassTransfer();
				return;
			}
			if (num2 < pickupable.PrimaryElement.Mass)
			{
				Pickupable pickupable2 = pickupable.Take(num2);
				flowManager.AddPickupable(this.outputCell, pickupable2);
				this.dispensing = true;
				num = pickupable2.PrimaryElement.Mass;
				if (this.OnMassTransfer != null)
				{
					this.OnMassTransfer(pickupable2.PrimaryElement.ElementID, num, pickupable2.PrimaryElement.Temperature, pickupable2.PrimaryElement.DiseaseIdx, pickupable2.PrimaryElement.DiseaseCount, pickupable2);
				}
			}
			else
			{
				Pickupable pickupable3 = flowManager.RemovePickupable(this.inputCell);
				if (pickupable3)
				{
					flowManager.AddPickupable(this.outputCell, pickupable3);
					this.dispensing = true;
					num = pickupable3.PrimaryElement.Mass;
					if (this.OnMassTransfer != null)
					{
						this.OnMassTransfer(pickupable3.PrimaryElement.ElementID, num, pickupable3.PrimaryElement.Temperature, pickupable3.PrimaryElement.DiseaseIdx, pickupable3.PrimaryElement.DiseaseCount, pickupable3);
					}
				}
			}
		}
		if (num == 0f)
		{
			base.SendEmptyOnMassTransfer();
		}
	}

	// Token: 0x0400393A RID: 14650
	[MyCmpGet]
	private Operational operational;

	// Token: 0x0400393B RID: 14651
	private int inputCell;

	// Token: 0x0400393C RID: 14652
	private int outputCell;

	// Token: 0x0400393D RID: 14653
	private bool dispensing;
}
