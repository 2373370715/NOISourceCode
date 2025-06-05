using System;
using KSerialization;

// Token: 0x0200130E RID: 4878
[SerializationConfig(MemberSerialization.OptIn)]
public class EventInstanceBase : ISaveLoadable
{
	// Token: 0x060063ED RID: 25581 RVA: 0x000E5AD3 File Offset: 0x000E3CD3
	public EventInstanceBase(EventBase ev)
	{
		this.frame = GameClock.Instance.GetFrame();
		this.eventHash = ev.hash;
		this.ev = ev;
	}

	// Token: 0x060063EE RID: 25582 RVA: 0x002CA330 File Offset: 0x002C8530
	public override string ToString()
	{
		string str = "[" + this.frame.ToString() + "] ";
		if (this.ev != null)
		{
			return str + this.ev.GetDescription(this);
		}
		return str + "Unknown event";
	}

	// Token: 0x040047CD RID: 18381
	[Serialize]
	public int frame;

	// Token: 0x040047CE RID: 18382
	[Serialize]
	public int eventHash;

	// Token: 0x040047CF RID: 18383
	public EventBase ev;
}
