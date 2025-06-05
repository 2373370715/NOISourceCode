using System;

// Token: 0x02000FF9 RID: 4089
public class StorageLockerSmart : StorageLocker
{
	// Token: 0x0600525D RID: 21085 RVA: 0x000DA130 File Offset: 0x000D8330
	protected override void OnPrefabInit()
	{
		base.Initialize(true);
	}

	// Token: 0x0600525E RID: 21086 RVA: 0x00282F80 File Offset: 0x00281180
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.ports = base.gameObject.GetComponent<LogicPorts>();
		base.Subscribe<StorageLockerSmart>(-1697596308, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		base.Subscribe<StorageLockerSmart>(-592767678, StorageLockerSmart.UpdateLogicCircuitCBDelegate);
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x0600525F RID: 21087 RVA: 0x000DA139 File Offset: 0x000D8339
	private void UpdateLogicCircuitCB(object data)
	{
		this.UpdateLogicAndActiveState();
	}

	// Token: 0x06005260 RID: 21088 RVA: 0x00282FCC File Offset: 0x002811CC
	private void UpdateLogicAndActiveState()
	{
		bool flag = this.filteredStorage.IsFull();
		bool isOperational = this.operational.IsOperational;
		bool flag2 = flag && isOperational;
		this.ports.SendSignal(FilteredStorage.FULL_PORT_ID, flag2 ? 1 : 0);
		this.filteredStorage.SetLogicMeter(flag2);
		this.operational.SetActive(isOperational, false);
	}

	// Token: 0x170004A4 RID: 1188
	// (get) Token: 0x06005261 RID: 21089 RVA: 0x000DA141 File Offset: 0x000D8341
	// (set) Token: 0x06005262 RID: 21090 RVA: 0x000DA149 File Offset: 0x000D8349
	public override float UserMaxCapacity
	{
		get
		{
			return base.UserMaxCapacity;
		}
		set
		{
			base.UserMaxCapacity = value;
			this.UpdateLogicAndActiveState();
		}
	}

	// Token: 0x04003A2B RID: 14891
	[MyCmpGet]
	private LogicPorts ports;

	// Token: 0x04003A2C RID: 14892
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04003A2D RID: 14893
	private static readonly EventSystem.IntraObjectHandler<StorageLockerSmart> UpdateLogicCircuitCBDelegate = new EventSystem.IntraObjectHandler<StorageLockerSmart>(delegate(StorageLockerSmart component, object data)
	{
		component.UpdateLogicCircuitCB(data);
	});
}
