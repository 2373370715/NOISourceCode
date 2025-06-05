using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000853 RID: 2131
[AddComponentMenu("KMonoBehaviour/scripts/Sensors")]
public class Sensors : KMonoBehaviour
{
	// Token: 0x060025A0 RID: 9632 RVA: 0x000BD1ED File Offset: 0x000BB3ED
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.GetComponent<Brain>().onPreUpdate += this.OnBrainPreUpdate;
	}

	// Token: 0x060025A1 RID: 9633 RVA: 0x001D9BC4 File Offset: 0x001D7DC4
	public SensorType GetSensor<SensorType>() where SensorType : Sensor
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (typeof(SensorType).IsAssignableFrom(sensor.GetType()))
			{
				return (SensorType)((object)sensor);
			}
		}
		global::Debug.LogError("Missing sensor of type: " + typeof(SensorType).Name);
		return default(SensorType);
	}

	// Token: 0x060025A2 RID: 9634 RVA: 0x000BD20C File Offset: 0x000BB40C
	public void Add(Sensor sensor)
	{
		this.sensors.Add(sensor);
		if (sensor.IsEnabled)
		{
			sensor.Update();
		}
	}

	// Token: 0x060025A3 RID: 9635 RVA: 0x001D9C5C File Offset: 0x001D7E5C
	public void UpdateSensors()
	{
		foreach (Sensor sensor in this.sensors)
		{
			if (sensor.IsEnabled)
			{
				sensor.Update();
			}
		}
	}

	// Token: 0x060025A4 RID: 9636 RVA: 0x000BD228 File Offset: 0x000BB428
	private void OnBrainPreUpdate()
	{
		this.UpdateSensors();
	}

	// Token: 0x060025A5 RID: 9637 RVA: 0x001D9CB8 File Offset: 0x001D7EB8
	public void ShowEditor()
	{
		foreach (Sensor sensor in this.sensors)
		{
			sensor.ShowEditor();
		}
	}

	// Token: 0x040019EB RID: 6635
	public List<Sensor> sensors = new List<Sensor>();
}
