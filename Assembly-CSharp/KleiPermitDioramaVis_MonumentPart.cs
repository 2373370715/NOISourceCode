﻿using System;
using Database;
using UnityEngine;

public class KleiPermitDioramaVis_MonumentPart : KMonoBehaviour, IKleiPermitDioramaVisTarget
{
	public GameObject GetGameObject()
	{
		return base.gameObject;
	}

	public void ConfigureSetup()
	{
	}

	public void ConfigureWith(PermitResource permit)
	{
		MonumentPartResource monumentPermit = (MonumentPartResource)permit;
		KleiPermitVisUtil.ConfigureToRenderBuilding(this.buildingKAnim, monumentPermit);
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		this.buildingKAnimPosition.SetOn(this.buildingKAnim);
		this.buildingKAnim.rectTransform().anchoredPosition += new Vector2(0f, -176f + (float)(buildingDef.HeightInCells * 6));
		this.buildingKAnim.rectTransform().localScale = Vector3.one * 0.55f;
		KleiPermitVisUtil.AnimateIn(this.buildingKAnim, default(Updater));
	}

	[SerializeField]
	private KBatchedAnimController buildingKAnim;

	private PrefabDefinedUIPosition buildingKAnimPosition = new PrefabDefinedUIPosition();
}
