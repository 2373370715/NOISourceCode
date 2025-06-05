using System;

// Token: 0x020007C3 RID: 1987
public class GameplayEventPrecondition
{
	// Token: 0x040017A2 RID: 6050
	public string description;

	// Token: 0x040017A3 RID: 6051
	public GameplayEventPrecondition.PreconditionFn condition;

	// Token: 0x040017A4 RID: 6052
	public bool required;

	// Token: 0x040017A5 RID: 6053
	public int priorityModifier;

	// Token: 0x020007C4 RID: 1988
	// (Invoke) Token: 0x06002337 RID: 9015
	public delegate bool PreconditionFn();
}
