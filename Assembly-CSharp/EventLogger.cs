using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x0200130F RID: 4879
[SerializationConfig(MemberSerialization.OptIn)]
public class EventLogger<EventInstanceType, EventType> : KMonoBehaviour, ISaveLoadable where EventInstanceType : EventInstanceBase where EventType : EventBase
{
	// Token: 0x060063EF RID: 25583 RVA: 0x000E5AFE File Offset: 0x000E3CFE
	public IEnumerator<EventInstanceType> GetEnumerator()
	{
		return this.EventInstances.GetEnumerator();
	}

	// Token: 0x060063F0 RID: 25584 RVA: 0x002CA380 File Offset: 0x002C8580
	public EventType AddEvent(EventType ev)
	{
		for (int i = 0; i < this.Events.Count; i++)
		{
			if (this.Events[i].hash == ev.hash)
			{
				this.Events[i] = ev;
				return this.Events[i];
			}
		}
		this.Events.Add(ev);
		return ev;
	}

	// Token: 0x060063F1 RID: 25585 RVA: 0x000E5B10 File Offset: 0x000E3D10
	public EventInstanceType Add(EventInstanceType ev)
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveAt(0);
		}
		this.EventInstances.Add(ev);
		return ev;
	}

	// Token: 0x060063F2 RID: 25586 RVA: 0x002CA3F0 File Offset: 0x002C85F0
	[OnDeserialized]
	protected internal void OnDeserialized()
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveRange(0, this.EventInstances.Count - 10000);
		}
		for (int i = 0; i < this.EventInstances.Count; i++)
		{
			for (int j = 0; j < this.Events.Count; j++)
			{
				if (this.Events[j].hash == this.EventInstances[i].eventHash)
				{
					this.EventInstances[i].ev = this.Events[j];
					break;
				}
			}
		}
	}

	// Token: 0x060063F3 RID: 25587 RVA: 0x000E5B3D File Offset: 0x000E3D3D
	public void Clear()
	{
		this.EventInstances.Clear();
	}

	// Token: 0x040047D0 RID: 18384
	private const int MAX_NUM_EVENTS = 10000;

	// Token: 0x040047D1 RID: 18385
	private List<EventType> Events = new List<EventType>();

	// Token: 0x040047D2 RID: 18386
	[Serialize]
	private List<EventInstanceType> EventInstances = new List<EventInstanceType>();
}
