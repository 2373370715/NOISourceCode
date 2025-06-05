using System;
using UnityEngine;

// Token: 0x02001DA3 RID: 7587
public class KleiItemDropScreen_PermitVis_DupeEquipment : KMonoBehaviour
{
	// Token: 0x06009E85 RID: 40581 RVA: 0x003DC96C File Offset: 0x003DAB6C
	public void ConfigureWith(DropScreenPresentationInfo info)
	{
		this.dupeKAnim.GetComponent<UIDupeRandomizer>().Randomize();
		KAnimFile anim = Assets.GetAnim(info.BuildOverride);
		this.dupeKAnim.AddAnimOverrides(anim, 0f);
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapto_neck");
		KAnim.Build.Symbol symbol = anim.GetData().build.GetSymbol(kanimHashedString);
		if (symbol != null)
		{
			this.dupeKAnim.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, symbol, 6);
			this.dupeKAnim.SetSymbolVisiblity(kanimHashedString, true);
		}
		else
		{
			this.dupeKAnim.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(kanimHashedString, 6);
			this.dupeKAnim.SetSymbolVisiblity(kanimHashedString, false);
		}
		this.dupeKAnim.Play("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
		this.dupeKAnim.Queue("cheer_pre", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("cheer_loop", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("cheer_pst", KAnim.PlayMode.Once, 1f, 0f);
		this.dupeKAnim.Queue("idle_default", KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x04007C90 RID: 31888
	[SerializeField]
	private KBatchedAnimController droppedItemKAnim;

	// Token: 0x04007C91 RID: 31889
	[SerializeField]
	private KBatchedAnimController dupeKAnim;
}
