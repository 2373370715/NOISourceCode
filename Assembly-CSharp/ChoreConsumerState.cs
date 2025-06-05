using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class ChoreConsumerState
{
	// Token: 0x06002234 RID: 8756 RVA: 0x001CE6E8 File Offset: 0x001CC8E8
	public ChoreConsumerState(ChoreConsumer consumer)
	{
		this.consumer = consumer;
		this.navigator = consumer.GetComponent<Navigator>();
		this.prefabid = consumer.GetComponent<KPrefabID>();
		this.ownable = consumer.GetComponent<Ownable>();
		this.gameObject = consumer.gameObject;
		this.solidTransferArm = consumer.GetComponent<SolidTransferArm>();
		this.hasSolidTransferArm = (this.solidTransferArm != null);
		this.resume = consumer.GetComponent<MinionResume>();
		this.choreDriver = consumer.GetComponent<ChoreDriver>();
		this.schedulable = consumer.GetComponent<Schedulable>();
		this.traits = consumer.GetComponent<Traits>();
		this.choreProvider = consumer.GetComponent<ChoreProvider>();
		MinionIdentity component = consumer.GetComponent<MinionIdentity>();
		if (component != null)
		{
			if (component.assignableProxy == null)
			{
				component.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(component.assignableProxy, component);
			}
			this.assignables = component.GetSoleOwner();
			this.equipment = component.GetEquipment();
		}
		else
		{
			this.assignables = consumer.GetComponent<Assignables>();
			this.equipment = consumer.GetComponent<Equipment>();
		}
		this.storage = consumer.GetComponent<Storage>();
		this.consumableConsumer = consumer.GetComponent<ConsumableConsumer>();
		this.worker = consumer.GetComponent<WorkerBase>();
		this.selectable = consumer.GetComponent<KSelectable>();
		if (this.schedulable != null)
		{
			this.scheduleBlock = this.schedulable.GetSchedule().GetCurrentScheduleBlock();
		}
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x001CE83C File Offset: 0x001CCA3C
	public void Refresh()
	{
		if (this.schedulable != null)
		{
			Schedule schedule = this.schedulable.GetSchedule();
			if (schedule != null)
			{
				this.scheduleBlock = schedule.GetCurrentScheduleBlock();
			}
		}
	}

	// Token: 0x040016E3 RID: 5859
	public KPrefabID prefabid;

	// Token: 0x040016E4 RID: 5860
	public GameObject gameObject;

	// Token: 0x040016E5 RID: 5861
	public ChoreConsumer consumer;

	// Token: 0x040016E6 RID: 5862
	public ChoreProvider choreProvider;

	// Token: 0x040016E7 RID: 5863
	public Navigator navigator;

	// Token: 0x040016E8 RID: 5864
	public Ownable ownable;

	// Token: 0x040016E9 RID: 5865
	public Assignables assignables;

	// Token: 0x040016EA RID: 5866
	public MinionResume resume;

	// Token: 0x040016EB RID: 5867
	public ChoreDriver choreDriver;

	// Token: 0x040016EC RID: 5868
	public Schedulable schedulable;

	// Token: 0x040016ED RID: 5869
	public Traits traits;

	// Token: 0x040016EE RID: 5870
	public Equipment equipment;

	// Token: 0x040016EF RID: 5871
	public Storage storage;

	// Token: 0x040016F0 RID: 5872
	public ConsumableConsumer consumableConsumer;

	// Token: 0x040016F1 RID: 5873
	public KSelectable selectable;

	// Token: 0x040016F2 RID: 5874
	public WorkerBase worker;

	// Token: 0x040016F3 RID: 5875
	public SolidTransferArm solidTransferArm;

	// Token: 0x040016F4 RID: 5876
	public bool hasSolidTransferArm;

	// Token: 0x040016F5 RID: 5877
	public ScheduleBlock scheduleBlock;
}
