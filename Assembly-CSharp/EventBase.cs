using System;

// Token: 0x0200130D RID: 4877
public class EventBase : Resource
{
	// Token: 0x060063EB RID: 25579 RVA: 0x000E5ABD File Offset: 0x000E3CBD
	public EventBase(string id) : base(id, id)
	{
		this.hash = Hash.SDBMLower(id);
	}

	// Token: 0x060063EC RID: 25580 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public virtual string GetDescription(EventInstanceBase ev)
	{
		return "";
	}

	// Token: 0x040047CC RID: 18380
	public int hash;
}
