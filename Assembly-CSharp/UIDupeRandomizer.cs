using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x020020A2 RID: 8354
public class UIDupeRandomizer : MonoBehaviour
{
	// Token: 0x0600B222 RID: 45602 RVA: 0x0043BF28 File Offset: 0x0043A128
	protected virtual void Start()
	{
		this.slots = Db.Get().AccessorySlots;
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.anims[i].curBody = null;
			this.GetNewBody(i);
		}
	}

	// Token: 0x0600B223 RID: 45603 RVA: 0x0043BF74 File Offset: 0x0043A174
	protected void GetNewBody(int minion_idx)
	{
		Personality random = Db.Get().Personalities.GetRandom(true, false);
		foreach (KBatchedAnimController dupe in this.anims[minion_idx].minions)
		{
			this.Apply(dupe, random);
		}
	}

	// Token: 0x0600B224 RID: 45604 RVA: 0x0043BFE8 File Offset: 0x0043A1E8
	private void Apply(KBatchedAnimController dupe, Personality personality)
	{
		KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
		SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
		component.RemoveAllSymbolOverrides(0);
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLower.Lookup(bodyData.armslower));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Belt.Lookup(bodyData.belt));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Hand.Lookup(bodyData.hand));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Neck.Lookup(bodyData.neck));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Cuff.Lookup(bodyData.cuff));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Pelvis.Lookup(bodyData.pelvis));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Leg.Lookup(bodyData.legs));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.Foot.Lookup(bodyData.foot));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmLowerSkin.Lookup(bodyData.armLowerSkin));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.ArmUpperSkin.Lookup(bodyData.armUpperSkin));
		UIDupeRandomizer.AddAccessory(dupe, this.slots.LegSkin.Lookup(bodyData.legSkin));
		if (this.applySuit && UnityEngine.Random.value < 0.15f)
		{
			component.AddBuildOverride(Assets.GetAnim("body_oxygen_kanim").GetData(), 6);
			dupe.SetSymbolVisiblity("snapto_neck", true);
			dupe.SetSymbolVisiblity("belt", false);
		}
		else
		{
			dupe.SetSymbolVisiblity("snapto_neck", false);
		}
		if (this.applyHat && UnityEngine.Random.value < 0.5f)
		{
			List<string> list = new List<string>();
			foreach (Skill skill in Db.Get().Skills.resources)
			{
				if (skill.requiredDuplicantModel.IsNullOrWhiteSpace() || skill.requiredDuplicantModel == personality.model)
				{
					list.Add(skill.hat);
				}
			}
			string id = list[UnityEngine.Random.Range(0, list.Count)];
			UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
		}
		else
		{
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
			dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
		}
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Skirt.targetSymbolId, false);
		dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Necklace.targetSymbolId, false);
	}

	// Token: 0x0600B225 RID: 45605 RVA: 0x0043C414 File Offset: 0x0043A614
	public static KAnimHashedString AddAccessory(KBatchedAnimController minion, Accessory accessory)
	{
		if (accessory != null)
		{
			SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
			DebugUtil.Assert(component != null, minion.name + " is missing symbol override controller");
			component.TryRemoveSymbolOverride(accessory.slot.targetSymbolId, 0);
			component.AddSymbolOverride(accessory.slot.targetSymbolId, accessory.symbol, 0);
			minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
			return accessory.slot.targetSymbolId;
		}
		return HashedString.Invalid;
	}

	// Token: 0x0600B226 RID: 45606 RVA: 0x0043C4A4 File Offset: 0x0043A6A4
	public KAnimHashedString AddRandomAccessory(KBatchedAnimController minion, List<Accessory> choices)
	{
		Accessory accessory = choices[UnityEngine.Random.Range(1, choices.Count)];
		return UIDupeRandomizer.AddAccessory(minion, accessory);
	}

	// Token: 0x0600B227 RID: 45607 RVA: 0x0043C4CC File Offset: 0x0043A6CC
	public void Randomize()
	{
		if (this.slots == null)
		{
			return;
		}
		for (int i = 0; i < this.anims.Length; i++)
		{
			this.GetNewBody(i);
		}
	}

	// Token: 0x0600B228 RID: 45608 RVA: 0x000AA038 File Offset: 0x000A8238
	protected virtual void Update()
	{
	}

	// Token: 0x04008CA6 RID: 36006
	[Tooltip("Enable this to allow for a chance for skill hats to appear")]
	public bool applyHat = true;

	// Token: 0x04008CA7 RID: 36007
	[Tooltip("Enable this to allow for a chance for suit helmets to appear (ie. atmosuit and leadsuit)")]
	public bool applySuit = true;

	// Token: 0x04008CA8 RID: 36008
	public UIDupeRandomizer.AnimChoice[] anims;

	// Token: 0x04008CA9 RID: 36009
	private AccessorySlots slots;

	// Token: 0x020020A3 RID: 8355
	[Serializable]
	public struct AnimChoice
	{
		// Token: 0x04008CAA RID: 36010
		public string anim_name;

		// Token: 0x04008CAB RID: 36011
		public List<KBatchedAnimController> minions;

		// Token: 0x04008CAC RID: 36012
		public float minSecondsBetweenAction;

		// Token: 0x04008CAD RID: 36013
		public float maxSecondsBetweenAction;

		// Token: 0x04008CAE RID: 36014
		public float lastWaitTime;

		// Token: 0x04008CAF RID: 36015
		public KAnimFile curBody;
	}
}
