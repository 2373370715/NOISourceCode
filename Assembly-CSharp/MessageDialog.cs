using System;

// Token: 0x02001E53 RID: 7763
public abstract class MessageDialog : KMonoBehaviour
{
	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x0600A284 RID: 41604 RVA: 0x000B1628 File Offset: 0x000AF828
	public virtual bool CanDontShowAgain
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600A285 RID: 41605
	public abstract bool CanDisplay(Message message);

	// Token: 0x0600A286 RID: 41606
	public abstract void SetMessage(Message message);

	// Token: 0x0600A287 RID: 41607
	public abstract void OnClickAction();

	// Token: 0x0600A288 RID: 41608 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnDontShowAgain()
	{
	}
}
