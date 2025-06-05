using System;

namespace Database
{
	// Token: 0x02002174 RID: 8564
	public class Accessories : ResourceSet<Accessory>
	{
		// Token: 0x0600B672 RID: 46706 RVA: 0x0011AE4C File Offset: 0x0011904C
		public Accessories(ResourceSet parent) : base("Accessories", parent)
		{
		}

		// Token: 0x0600B673 RID: 46707 RVA: 0x00456180 File Offset: 0x00454380
		public void AddAccessories(string id, KAnimFile anim_file)
		{
			if (anim_file != null)
			{
				KAnim.Build build = anim_file.GetData().build;
				for (int i = 0; i < build.symbols.Length; i++)
				{
					string text = HashCache.Get().Get(build.symbols[i].hash);
					AccessorySlot accessorySlot = Db.Get().AccessorySlots.Find(text);
					if (accessorySlot != null)
					{
						Accessory accessory = new Accessory(id + text, this, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file, null);
						accessorySlot.accessories.Add(accessory);
						HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
					}
				}
			}
		}

		// Token: 0x0600B674 RID: 46708 RVA: 0x00456238 File Offset: 0x00454438
		public void AddCustomAccessories(KAnimFile anim_file, ResourceSet parent, AccessorySlots slots)
		{
			if (anim_file != null)
			{
				KAnim.Build build = anim_file.GetData().build;
				for (int i = 0; i < build.symbols.Length; i++)
				{
					string symbol_name = HashCache.Get().Get(build.symbols[i].hash);
					AccessorySlot accessorySlot = slots.resources.Find((AccessorySlot slot) => symbol_name.IndexOf(slot.Id, 0, StringComparison.OrdinalIgnoreCase) != -1);
					if (accessorySlot != null)
					{
						Accessory accessory = new Accessory(symbol_name, parent, accessorySlot, anim_file.batchTag, build.symbols[i], anim_file, null);
						accessorySlot.accessories.Add(accessory);
						HashCache.Get().Add(accessory.IdHash.HashValue, accessory.Id);
					}
				}
			}
		}
	}
}
