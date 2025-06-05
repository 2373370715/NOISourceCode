using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020018CC RID: 6348
public class SimulatedTemperatureAdjuster
{
	// Token: 0x06008348 RID: 33608 RVA: 0x0034E774 File Offset: 0x0034C974
	public SimulatedTemperatureAdjuster(float simulated_temperature, float heat_capacity, float thermal_conductivity, Storage storage)
	{
		this.temperature = simulated_temperature;
		this.heatCapacity = heat_capacity;
		this.thermalConductivity = thermal_conductivity;
		this.storage = storage;
		storage.gameObject.Subscribe(824508782, new Action<object>(this.OnActivechanged));
		storage.gameObject.Subscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		Operational component = storage.gameObject.GetComponent<Operational>();
		this.OnActivechanged(component);
	}

	// Token: 0x06008349 RID: 33609 RVA: 0x000FAD4D File Offset: 0x000F8F4D
	public List<Descriptor> GetDescriptors()
	{
		return SimulatedTemperatureAdjuster.GetDescriptors(this.temperature);
	}

	// Token: 0x0600834A RID: 33610 RVA: 0x0034E7F4 File Offset: 0x0034C9F4
	public static List<Descriptor> GetDescriptors(float temperature)
	{
		List<Descriptor> list = new List<Descriptor>();
		string formattedTemperature = GameUtil.GetFormattedTemperature(temperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ITEM_TEMPERATURE_ADJUST, formattedTemperature), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x0600834B RID: 33611 RVA: 0x0034E844 File Offset: 0x0034CA44
	private void Register(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Combine(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			this.OnItemSimRegistered(stt);
		}
	}

	// Token: 0x0600834C RID: 33612 RVA: 0x0034E8AC File Offset: 0x0034CAAC
	private void Unregister(SimTemperatureTransfer stt)
	{
		stt.onSimRegistered = (Action<SimTemperatureTransfer>)Delegate.Remove(stt.onSimRegistered, new Action<SimTemperatureTransfer>(this.OnItemSimRegistered));
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, 0f, 0f, 0f);
		}
	}

	// Token: 0x0600834D RID: 33613 RVA: 0x0034E904 File Offset: 0x0034CB04
	private void OnItemSimRegistered(SimTemperatureTransfer stt)
	{
		if (stt == null)
		{
			return;
		}
		if (Sim.IsValidHandle(stt.SimHandle))
		{
			float num = this.temperature;
			float heat_capacity = this.heatCapacity;
			float thermal_conductivity = this.thermalConductivity;
			if (!this.active)
			{
				num = 0f;
				heat_capacity = 0f;
				thermal_conductivity = 0f;
			}
			SimMessages.ModifyElementChunkTemperatureAdjuster(stt.SimHandle, num, heat_capacity, thermal_conductivity);
		}
	}

	// Token: 0x0600834E RID: 33614 RVA: 0x0034E968 File Offset: 0x0034CB68
	private void OnActivechanged(object data)
	{
		Operational operational = (Operational)data;
		this.active = operational.IsActive;
		if (this.active)
		{
			using (List<GameObject>.Enumerator enumerator = this.storage.items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					if (gameObject != null)
					{
						SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
						this.OnItemSimRegistered(component);
					}
				}
				return;
			}
		}
		foreach (GameObject gameObject2 in this.storage.items)
		{
			if (gameObject2 != null)
			{
				SimTemperatureTransfer component2 = gameObject2.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component2);
			}
		}
	}

	// Token: 0x0600834F RID: 33615 RVA: 0x0034EA48 File Offset: 0x0034CC48
	public void CleanUp()
	{
		this.storage.gameObject.Unsubscribe(-1697596308, new Action<object>(this.OnStorageChanged));
		foreach (GameObject gameObject in this.storage.items)
		{
			if (gameObject != null)
			{
				SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
				this.Unregister(component);
			}
		}
	}

	// Token: 0x06008350 RID: 33616 RVA: 0x0034EAD4 File Offset: 0x0034CCD4
	private void OnStorageChanged(object data)
	{
		GameObject gameObject = (GameObject)data;
		SimTemperatureTransfer component = gameObject.GetComponent<SimTemperatureTransfer>();
		if (component == null)
		{
			return;
		}
		Pickupable component2 = gameObject.GetComponent<Pickupable>();
		if (component2 == null)
		{
			return;
		}
		if (this.active && component2.storage == this.storage)
		{
			this.Register(component);
			return;
		}
		this.Unregister(component);
	}

	// Token: 0x040063FC RID: 25596
	private float temperature;

	// Token: 0x040063FD RID: 25597
	private float heatCapacity;

	// Token: 0x040063FE RID: 25598
	private float thermalConductivity;

	// Token: 0x040063FF RID: 25599
	private bool active;

	// Token: 0x04006400 RID: 25600
	private Storage storage;
}
