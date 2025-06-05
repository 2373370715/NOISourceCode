using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000E71 RID: 3697
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicClusterLocationSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06004876 RID: 18550 RVA: 0x000D3740 File Offset: 0x000D1940
	public bool ActiveInSpace
	{
		get
		{
			return this.activeInSpace;
		}
	}

	// Token: 0x06004877 RID: 18551 RVA: 0x000D3748 File Offset: 0x000D1948
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicClusterLocationSensor>(-905833192, LogicClusterLocationSensor.OnCopySettingsDelegate);
	}

	// Token: 0x06004878 RID: 18552 RVA: 0x002644B0 File Offset: 0x002626B0
	private void OnCopySettings(object data)
	{
		LogicClusterLocationSensor component = ((GameObject)data).GetComponent<LogicClusterLocationSensor>();
		if (component != null)
		{
			this.activeLocations.Clear();
			for (int i = 0; i < component.activeLocations.Count; i++)
			{
				this.SetLocationEnabled(component.activeLocations[i], true);
			}
			this.activeInSpace = component.activeInSpace;
		}
	}

	// Token: 0x06004879 RID: 18553 RVA: 0x000D3761 File Offset: 0x000D1961
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600487A RID: 18554 RVA: 0x000D3794 File Offset: 0x000D1994
	public void SetLocationEnabled(AxialI location, bool setting)
	{
		if (!setting)
		{
			this.activeLocations.Remove(location);
			return;
		}
		if (!this.activeLocations.Contains(location))
		{
			this.activeLocations.Add(location);
		}
	}

	// Token: 0x0600487B RID: 18555 RVA: 0x000D37C1 File Offset: 0x000D19C1
	public void SetSpaceEnabled(bool setting)
	{
		this.activeInSpace = setting;
	}

	// Token: 0x0600487C RID: 18556 RVA: 0x00264514 File Offset: 0x00262714
	public void Sim200ms(float dt)
	{
		bool state = this.CheckCurrentLocationSelected();
		this.SetState(state);
	}

	// Token: 0x0600487D RID: 18557 RVA: 0x00264530 File Offset: 0x00262730
	private bool CheckCurrentLocationSelected()
	{
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		return this.activeLocations.Contains(myWorldLocation) || (this.activeInSpace && this.CheckInEmptySpace());
	}

	// Token: 0x0600487E RID: 18558 RVA: 0x0026456C File Offset: 0x0026276C
	private bool CheckInEmptySpace()
	{
		bool result = true;
		AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
		foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
		{
			if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600487F RID: 18559 RVA: 0x000D37CA File Offset: 0x000D19CA
	public bool CheckLocationSelected(AxialI location)
	{
		return this.activeLocations.Contains(location);
	}

	// Token: 0x06004880 RID: 18560 RVA: 0x000D37D8 File Offset: 0x000D19D8
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x06004881 RID: 18561 RVA: 0x000CEE6A File Offset: 0x000CD06A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x06004882 RID: 18562 RVA: 0x002645E8 File Offset: 0x002627E8
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			AxialI myWorldLocation = base.gameObject.GetMyWorldLocation();
			bool flag = true;
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				if (!worldContainer.IsModuleInterior && worldContainer.GetMyWorldLocation() == myWorldLocation)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				component.Play(this.switchedOn ? "on_space_pre" : "on_space_pst", KAnim.PlayMode.Once, 1f, 0f);
				component.Queue(this.switchedOn ? "on_space" : "off_space", KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			component.Play(this.switchedOn ? "on_asteroid_pre" : "on_asteroid_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on_asteroid" : "off_asteroid", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x06004883 RID: 18563 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x040032E6 RID: 13030
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040032E7 RID: 13031
	[Serialize]
	private List<AxialI> activeLocations = new List<AxialI>();

	// Token: 0x040032E8 RID: 13032
	[Serialize]
	private bool activeInSpace = true;

	// Token: 0x040032E9 RID: 13033
	private bool wasOn;

	// Token: 0x040032EA RID: 13034
	private static readonly EventSystem.IntraObjectHandler<LogicClusterLocationSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicClusterLocationSensor>(delegate(LogicClusterLocationSensor component, object data)
	{
		component.OnCopySettings(data);
	});
}
