using System;

// Token: 0x02000BBC RID: 3004
public class Accessory : Resource
{
	// Token: 0x17000275 RID: 629
	// (get) Token: 0x060038BB RID: 14523 RVA: 0x000C9346 File Offset: 0x000C7546
	// (set) Token: 0x060038BC RID: 14524 RVA: 0x000C934E File Offset: 0x000C754E
	public KAnim.Build.Symbol symbol { get; private set; }

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x060038BD RID: 14525 RVA: 0x000C9357 File Offset: 0x000C7557
	// (set) Token: 0x060038BE RID: 14526 RVA: 0x000C935F File Offset: 0x000C755F
	public HashedString batchSource { get; private set; }

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x060038BF RID: 14527 RVA: 0x000C9368 File Offset: 0x000C7568
	// (set) Token: 0x060038C0 RID: 14528 RVA: 0x000C9370 File Offset: 0x000C7570
	public AccessorySlot slot { get; private set; }

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x060038C1 RID: 14529 RVA: 0x000C9379 File Offset: 0x000C7579
	// (set) Token: 0x060038C2 RID: 14530 RVA: 0x000C9381 File Offset: 0x000C7581
	public KAnimFile animFile { get; private set; }

	// Token: 0x060038C3 RID: 14531 RVA: 0x000C938A File Offset: 0x000C758A
	public Accessory(string id, ResourceSet parent, AccessorySlot slot, HashedString batchSource, KAnim.Build.Symbol symbol, KAnimFile animFile = null, KAnimFile defaultAnimFile = null) : base(id, parent, null)
	{
		this.slot = slot;
		this.symbol = symbol;
		this.batchSource = batchSource;
		this.animFile = animFile;
	}

	// Token: 0x060038C4 RID: 14532 RVA: 0x000C93B4 File Offset: 0x000C75B4
	public bool IsDefault()
	{
		return this.animFile == this.slot.defaultAnimFile;
	}
}
