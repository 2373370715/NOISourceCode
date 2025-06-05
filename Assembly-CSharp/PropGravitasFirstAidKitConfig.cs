using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000530 RID: 1328
public class PropGravitasFirstAidKitConfig : IEntityConfig
{
	// Token: 0x060016CD RID: 5837 RVA: 0x001A3DF4 File Offset: 0x001A1FF4
	public GameObject CreatePrefab()
	{
		string id = "PropGravitasFirstAidKit";
		string name = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.NAME;
		string desc = STRINGS.BUILDINGS.PREFABS.PROPGRAVITASFIRSTAIDKIT.DESC;
		float mass = 50f;
		EffectorValues tier = TUNING.BUILDINGS.DECOR.BONUS.TIER0;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
		GameObject gameObject = EntityTemplates.CreatePlacedEntity(id, name, desc, mass, Assets.GetAnim("gravitas_first_aid_kit_kanim"), "off", Grid.SceneLayer.Building, 1, 1, tier, tier2, SimHashes.Creature, new List<Tag>
		{
			GameTags.Gravitas
		}, 293f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.SetElement(SimHashes.Granite, true);
		component.Temperature = 294.15f;
		Workable workable = gameObject.AddOrGet<Workable>();
		workable.synchronizeAnims = false;
		workable.resetProgressOnStop = true;
		SetLocker setLocker = gameObject.AddOrGet<SetLocker>();
		setLocker.overrideAnim = "anim_interacts_clothingfactory_kanim";
		setLocker.dropOffset = new Vector2I(0, 1);
		gameObject.AddOrGet<Demolishable>();
		return gameObject;
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x001A3EB8 File Offset: 0x001A20B8
	public static string[][] GetLockerBaseContents()
	{
		string text = DlcManager.FeatureRadiationEnabled() ? "BasicRadPill" : "IntermediateCure";
		return new string[][]
		{
			new string[]
			{
				"BasicCure",
				"BasicCure",
				"BasicCure"
			},
			new string[]
			{
				text,
				text
			}
		};
	}

	// Token: 0x060016CF RID: 5839 RVA: 0x000B42BF File Offset: 0x000B24BF
	public void OnPrefabInit(GameObject inst)
	{
		inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
		{
			ObjectLayer.Building
		};
		SetLocker component = inst.GetComponent<SetLocker>();
		component.possible_contents_ids = PropGravitasFirstAidKitConfig.GetLockerBaseContents();
		component.ChooseContents();
	}

	// Token: 0x060016D0 RID: 5840 RVA: 0x000AA038 File Offset: 0x000A8238
	public void OnSpawn(GameObject inst)
	{
	}
}
