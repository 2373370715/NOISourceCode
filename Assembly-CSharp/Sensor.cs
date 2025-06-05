using System;
using UnityEngine;

// Token: 0x02000852 RID: 2130
public class Sensor
{
	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06002594 RID: 9620 RVA: 0x000BD140 File Offset: 0x000BB340
	// (set) Token: 0x06002593 RID: 9619 RVA: 0x000BD137 File Offset: 0x000BB337
	public bool IsEnabled { get; private set; } = true;

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06002595 RID: 9621 RVA: 0x000BD148 File Offset: 0x000BB348
	// (set) Token: 0x06002596 RID: 9622 RVA: 0x000BD150 File Offset: 0x000BB350
	public string Name { get; private set; }

	// Token: 0x06002597 RID: 9623 RVA: 0x000BD159 File Offset: 0x000BB359
	public Sensor(Sensors sensors, bool active)
	{
		this.sensors = sensors;
		this.SetActive(active);
		this.Name = base.GetType().Name;
	}

	// Token: 0x06002598 RID: 9624 RVA: 0x000BD187 File Offset: 0x000BB387
	public Sensor(Sensors sensors)
	{
		this.sensors = sensors;
		this.Name = base.GetType().Name;
	}

	// Token: 0x06002599 RID: 9625 RVA: 0x000BD1AE File Offset: 0x000BB3AE
	public ComponentType GetComponent<ComponentType>()
	{
		return this.sensors.GetComponent<ComponentType>();
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x0600259A RID: 9626 RVA: 0x000BD1BB File Offset: 0x000BB3BB
	public GameObject gameObject
	{
		get
		{
			return this.sensors.gameObject;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x0600259B RID: 9627 RVA: 0x000BD1C8 File Offset: 0x000BB3C8
	public Transform transform
	{
		get
		{
			return this.gameObject.transform;
		}
	}

	// Token: 0x0600259C RID: 9628 RVA: 0x000BD1D5 File Offset: 0x000BB3D5
	public virtual void SetActive(bool enabled)
	{
		this.IsEnabled = enabled;
	}

	// Token: 0x0600259D RID: 9629 RVA: 0x000BD1DE File Offset: 0x000BB3DE
	public void Trigger(int hash, object data = null)
	{
		this.sensors.Trigger(hash, data);
	}

	// Token: 0x0600259E RID: 9630 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Update()
	{
	}

	// Token: 0x0600259F RID: 9631 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void ShowEditor()
	{
	}

	// Token: 0x040019E9 RID: 6633
	protected Sensors sensors;
}
