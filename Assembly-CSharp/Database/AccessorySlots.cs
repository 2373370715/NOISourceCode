using System;

namespace Database
{
	// Token: 0x02002176 RID: 8566
	public class AccessorySlots : ResourceSet<AccessorySlot>
	{
		// Token: 0x0600B677 RID: 46711 RVA: 0x00456300 File Offset: 0x00454500
		public AccessorySlots(ResourceSet parent) : base("AccessorySlots", parent)
		{
			parent = Db.Get().Accessories;
			KAnimFile anim = Assets.GetAnim("head_swap_kanim");
			KAnimFile anim2 = Assets.GetAnim("body_comp_default_kanim");
			KAnimFile anim3 = Assets.GetAnim("body_swap_kanim");
			KAnimFile anim4 = Assets.GetAnim("hair_swap_kanim");
			KAnimFile anim5 = Assets.GetAnim("hat_swap_kanim");
			this.Eyes = new AccessorySlot("Eyes", this, anim, 0);
			this.Hair = new AccessorySlot("Hair", this, anim4, 0);
			this.HeadShape = new AccessorySlot("HeadShape", this, anim, 0);
			this.Mouth = new AccessorySlot("Mouth", this, anim, 0);
			this.Hat = new AccessorySlot("Hat", this, anim5, 4);
			this.HatHair = new AccessorySlot("Hat_Hair", this, anim4, 0);
			this.HeadEffects = new AccessorySlot("HeadFX", this, anim, 0);
			this.Body = new AccessorySlot("Torso", this, new KAnimHashedString("torso"), anim3, null, 0);
			this.Arm = new AccessorySlot("Arm_Sleeve", this, new KAnimHashedString("arm_sleeve"), anim3, null, 0);
			this.ArmLower = new AccessorySlot("Arm_Lower_Sleeve", this, new KAnimHashedString("arm_lower_sleeve"), anim3, null, 0);
			this.Belt = new AccessorySlot("Belt", this, new KAnimHashedString("belt"), anim2, null, 0);
			this.Neck = new AccessorySlot("Neck", this, new KAnimHashedString("neck"), anim2, null, 0);
			this.Pelvis = new AccessorySlot("Pelvis", this, new KAnimHashedString("pelvis"), anim2, null, 0);
			this.Foot = new AccessorySlot("Foot", this, new KAnimHashedString("foot"), anim2, Assets.GetAnim("shoes_basic_black_kanim"), 0);
			this.Leg = new AccessorySlot("Leg", this, new KAnimHashedString("leg"), anim2, null, 0);
			this.Necklace = new AccessorySlot("Necklace", this, new KAnimHashedString("necklace"), anim2, null, 0);
			this.Cuff = new AccessorySlot("Cuff", this, new KAnimHashedString("cuff"), anim2, null, 0);
			this.Hand = new AccessorySlot("Hand", this, new KAnimHashedString("hand_paint"), anim2, null, 0);
			this.Skirt = new AccessorySlot("Skirt", this, new KAnimHashedString("skirt"), anim3, null, 0);
			this.ArmLowerSkin = new AccessorySlot("Arm_Lower", this, new KAnimHashedString("arm_lower"), anim3, null, 0);
			this.ArmUpperSkin = new AccessorySlot("Arm_Upper", this, new KAnimHashedString("arm_upper"), anim3, null, 0);
			this.LegSkin = new AccessorySlot("Leg_Skin", this, new KAnimHashedString("leg_skin"), anim3, null, 0);
			foreach (AccessorySlot accessorySlot in this.resources)
			{
				accessorySlot.AddAccessories(accessorySlot.AnimFile, parent);
			}
			Db.Get().Accessories.AddCustomAccessories(Assets.GetAnim("body_lonelyminion_kanim"), parent, this);
		}

		// Token: 0x0600B678 RID: 46712 RVA: 0x00456638 File Offset: 0x00454838
		public AccessorySlot Find(KAnimHashedString symbol_name)
		{
			foreach (AccessorySlot accessorySlot in Db.Get().AccessorySlots.resources)
			{
				if (symbol_name == accessorySlot.targetSymbolId)
				{
					return accessorySlot;
				}
			}
			return null;
		}

		// Token: 0x04009023 RID: 36899
		public AccessorySlot Eyes;

		// Token: 0x04009024 RID: 36900
		public AccessorySlot Hair;

		// Token: 0x04009025 RID: 36901
		public AccessorySlot HeadShape;

		// Token: 0x04009026 RID: 36902
		public AccessorySlot Mouth;

		// Token: 0x04009027 RID: 36903
		public AccessorySlot Body;

		// Token: 0x04009028 RID: 36904
		public AccessorySlot Arm;

		// Token: 0x04009029 RID: 36905
		public AccessorySlot ArmLower;

		// Token: 0x0400902A RID: 36906
		public AccessorySlot Hat;

		// Token: 0x0400902B RID: 36907
		public AccessorySlot HatHair;

		// Token: 0x0400902C RID: 36908
		public AccessorySlot HeadEffects;

		// Token: 0x0400902D RID: 36909
		public AccessorySlot Belt;

		// Token: 0x0400902E RID: 36910
		public AccessorySlot Neck;

		// Token: 0x0400902F RID: 36911
		public AccessorySlot Pelvis;

		// Token: 0x04009030 RID: 36912
		public AccessorySlot Leg;

		// Token: 0x04009031 RID: 36913
		public AccessorySlot Foot;

		// Token: 0x04009032 RID: 36914
		public AccessorySlot Skirt;

		// Token: 0x04009033 RID: 36915
		public AccessorySlot Necklace;

		// Token: 0x04009034 RID: 36916
		public AccessorySlot Cuff;

		// Token: 0x04009035 RID: 36917
		public AccessorySlot Hand;

		// Token: 0x04009036 RID: 36918
		public AccessorySlot ArmLowerSkin;

		// Token: 0x04009037 RID: 36919
		public AccessorySlot ArmUpperSkin;

		// Token: 0x04009038 RID: 36920
		public AccessorySlot LegSkin;
	}
}
