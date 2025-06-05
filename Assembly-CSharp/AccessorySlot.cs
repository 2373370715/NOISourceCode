using System;
using System.Collections.Generic;

// Token: 0x02000BBD RID: 3005
public class AccessorySlot : Resource
{
	// Token: 0x17000279 RID: 633
	// (get) Token: 0x060038C5 RID: 14533 RVA: 0x000C93CC File Offset: 0x000C75CC
	// (set) Token: 0x060038C6 RID: 14534 RVA: 0x000C93D4 File Offset: 0x000C75D4
	public KAnimHashedString targetSymbolId { get; private set; }

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x060038C7 RID: 14535 RVA: 0x000C93DD File Offset: 0x000C75DD
	// (set) Token: 0x060038C8 RID: 14536 RVA: 0x000C93E5 File Offset: 0x000C75E5
	public List<Accessory> accessories { get; private set; }

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060038C9 RID: 14537 RVA: 0x000C93EE File Offset: 0x000C75EE
	public KAnimFile AnimFile
	{
		get
		{
			return this.file;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x060038CA RID: 14538 RVA: 0x000C93F6 File Offset: 0x000C75F6
	// (set) Token: 0x060038CB RID: 14539 RVA: 0x000C93FE File Offset: 0x000C75FE
	public KAnimFile defaultAnimFile { get; private set; }

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x060038CC RID: 14540 RVA: 0x000C9407 File Offset: 0x000C7607
	// (set) Token: 0x060038CD RID: 14541 RVA: 0x000C940F File Offset: 0x000C760F
	public int overrideLayer { get; private set; }

	// Token: 0x060038CE RID: 14542 RVA: 0x00229B78 File Offset: 0x00227D78
	public AccessorySlot(string id, ResourceSet parent, KAnimFile swap_build, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = new KAnimHashedString("snapTo_" + id.ToLower());
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.overrideLayer = overrideLayer;
		this.defaultAnimFile = swap_build;
	}

	// Token: 0x060038CF RID: 14543 RVA: 0x00229BE8 File Offset: 0x00227DE8
	public AccessorySlot(string id, ResourceSet parent, KAnimHashedString target_symbol_id, KAnimFile swap_build, KAnimFile defaultAnimFile = null, int overrideLayer = 0) : base(id, parent, null)
	{
		if (swap_build == null)
		{
			Debug.LogErrorFormat("AccessorySlot {0} missing swap_build", new object[]
			{
				id
			});
		}
		this.targetSymbolId = target_symbol_id;
		this.accessories = new List<Accessory>();
		this.file = swap_build;
		this.defaultAnimFile = ((defaultAnimFile != null) ? defaultAnimFile : swap_build);
		this.overrideLayer = overrideLayer;
	}

	// Token: 0x060038D0 RID: 14544 RVA: 0x00229C54 File Offset: 0x00227E54
	public void AddAccessories(KAnimFile default_build, ResourceSet parent)
	{
		KAnim.Build build = default_build.GetData().build;
		default_build.GetData().build.GetSymbol(this.targetSymbolId);
		string value = this.Id.ToLower();
		for (int i = 0; i < build.symbols.Length; i++)
		{
			string text = HashCache.Get().Get(build.symbols[i].hash);
			if (text.StartsWith(value))
			{
				Accessory accessory = new Accessory(text, parent, this, this.file.batchTag, build.symbols[i], default_build, null);
				this.accessories.Add(accessory);
				HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
			}
		}
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x000C9418 File Offset: 0x000C7618
	public Accessory Lookup(string id)
	{
		return this.Lookup(new HashedString(id));
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x00229D10 File Offset: 0x00227F10
	public Accessory Lookup(HashedString full_id)
	{
		if (!full_id.IsValid)
		{
			return null;
		}
		return this.accessories.Find((Accessory a) => a.IdHash == full_id);
	}

	// Token: 0x04002749 RID: 10057
	private KAnimFile file;
}
