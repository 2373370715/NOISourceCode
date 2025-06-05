using System;

// Token: 0x0200123E RID: 4670
public class Death : Resource
{
	// Token: 0x06005F07 RID: 24327 RVA: 0x000E281E File Offset: 0x000E0A1E
	public Death(string id, ResourceSet parent, string name, string description, string pre_anim, string loop_anim) : base(id, parent, name)
	{
		this.preAnim = pre_anim;
		this.loopAnim = loop_anim;
		this.description = description;
	}

	// Token: 0x040043D4 RID: 17364
	public string preAnim;

	// Token: 0x040043D5 RID: 17365
	public string loopAnim;

	// Token: 0x040043D6 RID: 17366
	public string sound;

	// Token: 0x040043D7 RID: 17367
	public string description;
}
