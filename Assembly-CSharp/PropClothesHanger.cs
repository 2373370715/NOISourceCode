using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000516 RID: 1302
public class PropClothesHanger : IEntityConfig, IHasDlcRestrictions
{
	// Token: 0x06001657 RID: 5719 RVA: 0x000AA536 File Offset: 0x000A8736
	public string[] GetRequiredDlcIds()
	{
		return DlcManager.DLC2;
	}

	// Token: 0x06001658 RID: 5720 RVA: 0x000AA765 File Offset: 0x000A8965
	public string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x06001659 RID: 5721 RVA: 0x001A2B88 File Offset: 0x001A0D88
	public GameObject CreatePrefab()
	{
		string id = "PropClothesHanger";
		string name = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPCLOTHESHANGER.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("unlock_clothing_kanim"), "on", Grid.SceneLayer.Building, 1, 2, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas,
			GameTags.RoomProberBuilding
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Cinnabar, true);
		component.Temperature = 294.15f;
		gameObject.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		setLocker.dropOnDeconstruct = true;
		gameObject.AddOrGet<Deconstructable>().audioSize = "small";
		return gameObject;
	}

	// Token: 0x0600165A RID: 5722 RVA: 0x001A2C80 File Offset: 0x001A0E80
	public void OnPrefabInit(GameObject inst)
	{
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = new string[][]
		{
			new string[]
			{
				"Warm_Vest"
			}
		};
		component.ChooseContents();
	}

	// Token: 0x0600165B RID: 5723 RVA: 0x000B42AD File Offset: 0x000B24AD
	public void OnSpawn(GameObject inst)
	{
		inst.GetComponent<Deconstructable>().SetWorkTime(5f);
	}
}
