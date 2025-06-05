using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001DAB RID: 7595
public class KleiPermitDioramaVis : KMonoBehaviour
{
	// Token: 0x06009EB0 RID: 40624 RVA: 0x0010BBC8 File Offset: 0x00109DC8
	protected override void OnPrefabInit()
	{
		this.Init();
	}

	// Token: 0x06009EB1 RID: 40625 RVA: 0x003DD368 File Offset: 0x003DB568
	private void Init()
	{
		if (this.initComplete)
		{
			return;
		}
		this.allVisList = ReflectionUtil.For<KleiPermitDioramaVis>(this).CollectValuesForFieldsThatInheritOrImplement<IKleiPermitDioramaVisTarget>(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.ConfigureSetup();
		}
		this.initComplete = true;
	}

	// Token: 0x06009EB2 RID: 40626 RVA: 0x003DD3D8 File Offset: 0x003DB5D8
	public void ConfigureWith(PermitResource permit)
	{
		if (!this.initComplete)
		{
			this.Init();
		}
		foreach (IKleiPermitDioramaVisTarget kleiPermitDioramaVisTarget in this.allVisList)
		{
			kleiPermitDioramaVisTarget.GetGameObject().SetActive(false);
		}
		KleiPermitVisUtil.ClearAnimation();
		IKleiPermitDioramaVisTarget permitVisTarget = this.GetPermitVisTarget(permit);
		permitVisTarget.GetGameObject().SetActive(true);
		permitVisTarget.ConfigureWith(permit);
		string dlcIdFrom = permit.GetDlcIdFrom();
		if (DlcManager.IsDlcId(dlcIdFrom))
		{
			this.dlcImage.gameObject.SetActive(true);
			this.dlcImage.sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(dlcIdFrom));
			return;
		}
		this.dlcImage.gameObject.SetActive(false);
	}

	// Token: 0x06009EB3 RID: 40627 RVA: 0x003DD4A4 File Offset: 0x003DB6A4
	private IKleiPermitDioramaVisTarget GetPermitVisTarget(PermitResource permit)
	{
		KleiPermitDioramaVis.lastRenderedPermit = permit;
		if (permit == null)
		{
			return this.fallbackVis.WithError(string.Format("Given invalid permit: {0}", permit));
		}
		if (permit.Category == PermitCategory.Equipment || permit.Category == PermitCategory.DupeTops || permit.Category == PermitCategory.DupeBottoms || permit.Category == PermitCategory.DupeGloves || permit.Category == PermitCategory.DupeShoes || permit.Category == PermitCategory.DupeHats || permit.Category == PermitCategory.DupeAccessories || permit.Category == PermitCategory.AtmoSuitHelmet || permit.Category == PermitCategory.AtmoSuitBody || permit.Category == PermitCategory.AtmoSuitGloves || permit.Category == PermitCategory.AtmoSuitBelt || permit.Category == PermitCategory.AtmoSuitShoes)
		{
			return this.equipmentVis;
		}
		if (permit.Category == PermitCategory.Building)
		{
			BuildLocationRule? buildLocationRule = KleiPermitVisUtil.GetBuildLocationRule(permit);
			BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
			if (!buildingDef.BuildingComplete.GetComponent<Bed>().IsNullOrDestroyed())
			{
				return this.buildingOnFloorVis;
			}
			BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
			if (buildingFacadeResource != null)
			{
				if (buildingFacadeResource.PrefabID.Contains("Wire") || buildingFacadeResource.PrefabID.Contains("Ribbon"))
				{
					return this.buildingWiresAndAutomationVis;
				}
				if (buildingFacadeResource.PrefabID.Contains("Logic"))
				{
					return this.buildingAutomationGatesVis;
				}
			}
			if (buildingDef.PrefabID == "RockCrusher" || buildingDef.PrefabID == "GasReservoir" || buildingDef.PrefabID == "ArcadeMachine" || buildingDef.PrefabID == "MicrobeMusher" || buildingDef.PrefabID == "FlushToilet" || buildingDef.PrefabID == "WashSink" || buildingDef.PrefabID == "Headquarters" || buildingDef.PrefabID == "GourmetCookingStation")
			{
				return this.buildingOnFloorBigVis;
			}
			if (!buildingDef.BuildingComplete.GetComponent<RocketModule>().IsNullOrDestroyed() || !buildingDef.BuildingComplete.GetComponent<RocketEngine>().IsNullOrDestroyed())
			{
				return this.buildingRocketVis;
			}
			if (buildingDef.PrefabID == "PlanterBox" || buildingDef.PrefabID == "FlowerVase")
			{
				return this.buildingOnFloorBotanicalVis;
			}
			if (buildingDef.PrefabID == "ExteriorWall")
			{
				return this.wallpaperVis;
			}
			if (buildingDef.PrefabID == "FlowerVaseHanging" || buildingDef.PrefabID == "FlowerVaseHangingFancy")
			{
				return this.buildingHangingHookBotanicalVis;
			}
			if (buildLocationRule != null)
			{
				BuildLocationRule valueOrDefault = buildLocationRule.GetValueOrDefault();
				switch (valueOrDefault)
				{
				case BuildLocationRule.OnFloor:
					break;
				case BuildLocationRule.OnFloorOverSpace:
					goto IL_2B9;
				case BuildLocationRule.OnCeiling:
					return this.buildingOnCeilingVis.WithAlignment(Alignment.Top());
				case BuildLocationRule.OnWall:
					return this.buildingOnWallVis.WithAlignment(Alignment.Left());
				case BuildLocationRule.InCorner:
					return this.buildingInCeilingCornerVis.WithAlignment(Alignment.TopLeft());
				default:
					if (valueOrDefault != BuildLocationRule.OnFoundationRotatable)
					{
						goto IL_2B9;
					}
					break;
				}
				return this.buildingOnFloorVis;
			}
			IL_2B9:
			return this.fallbackVis.WithError(string.Format("No visualization available for building with BuildLocationRule of {0}", buildLocationRule));
		}
		else if (permit.Category == PermitCategory.Artwork)
		{
			BuildingDef buildingDef2 = KleiPermitVisUtil.GetBuildingDef(permit);
			if (buildingDef2.IsNullOrDestroyed())
			{
				return this.fallbackVis.WithError("Couldn't find building def for Artable " + permit.Id);
			}
			if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<Sculpture>(buildingDef2))
			{
				if (buildingDef2.PrefabID == "WoodSculpture")
				{
					return this.artablePaintingVis;
				}
				return this.artableSculptureVis;
			}
			else
			{
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<Painting>(buildingDef2))
				{
					return this.artablePaintingVis;
				}
				if (KleiPermitDioramaVis.<GetPermitVisTarget>g__Has|24_0<MonumentPart>(buildingDef2))
				{
					return this.monumentPartVis;
				}
				return this.fallbackVis.WithError("No visualization available for Artable " + permit.Id);
			}
		}
		else
		{
			if (permit.Category != PermitCategory.JoyResponse)
			{
				return this.fallbackVis.WithError("No visualization has been defined for permit with id \"" + permit.Id + "\"");
			}
			if (permit is BalloonArtistFacadeResource)
			{
				return this.joyResponseBalloonVis;
			}
			return this.fallbackVis.WithError("No visualization available for JoyResponse " + permit.Id);
		}
	}

	// Token: 0x06009EB4 RID: 40628 RVA: 0x003DD87C File Offset: 0x003DBA7C
	public static Sprite GetDioramaBackground(PermitCategory permitCategory)
	{
		switch (permitCategory)
		{
		case PermitCategory.DupeTops:
		case PermitCategory.DupeBottoms:
		case PermitCategory.DupeGloves:
		case PermitCategory.DupeShoes:
		case PermitCategory.DupeHats:
		case PermitCategory.DupeAccessories:
			return Assets.GetSprite("screen_bg_clothing");
		case PermitCategory.AtmoSuitHelmet:
		case PermitCategory.AtmoSuitBody:
		case PermitCategory.AtmoSuitGloves:
		case PermitCategory.AtmoSuitBelt:
		case PermitCategory.AtmoSuitShoes:
			return Assets.GetSprite("screen_bg_atmosuit");
		case PermitCategory.Building:
			return Assets.GetSprite("screen_bg_buildings");
		case PermitCategory.Artwork:
			return Assets.GetSprite("screen_bg_art");
		case PermitCategory.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		}
		return null;
	}

	// Token: 0x06009EB5 RID: 40629 RVA: 0x003DD928 File Offset: 0x003DBB28
	public static Sprite GetDioramaBackground(ClothingOutfitUtility.OutfitType outfitType)
	{
		switch (outfitType)
		{
		case ClothingOutfitUtility.OutfitType.Clothing:
			return Assets.GetSprite("screen_bg_clothing");
		case ClothingOutfitUtility.OutfitType.JoyResponse:
			return Assets.GetSprite("screen_bg_joyresponse");
		case ClothingOutfitUtility.OutfitType.AtmoSuit:
			return Assets.GetSprite("screen_bg_atmosuit");
		default:
			return null;
		}
	}

	// Token: 0x06009EB7 RID: 40631 RVA: 0x0010B5BE File Offset: 0x001097BE
	[CompilerGenerated]
	internal static bool <GetPermitVisTarget>g__Has|24_0<T>(BuildingDef buildingDef) where T : Component
	{
		return !buildingDef.BuildingComplete.GetComponent<T>().IsNullOrDestroyed();
	}

	// Token: 0x04007C9E RID: 31902
	[SerializeField]
	private Image dlcImage;

	// Token: 0x04007C9F RID: 31903
	[SerializeField]
	private KleiPermitDioramaVis_Fallback fallbackVis;

	// Token: 0x04007CA0 RID: 31904
	[SerializeField]
	private KleiPermitDioramaVis_DupeEquipment equipmentVis;

	// Token: 0x04007CA1 RID: 31905
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorVis;

	// Token: 0x04007CA2 RID: 31906
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloorBig buildingOnFloorBigVis;

	// Token: 0x04007CA3 RID: 31907
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnWallVis;

	// Token: 0x04007CA4 RID: 31908
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingOnCeilingVis;

	// Token: 0x04007CA5 RID: 31909
	[SerializeField]
	private KleiPermitDioramaVis_BuildingPresentationStand buildingInCeilingCornerVis;

	// Token: 0x04007CA6 RID: 31910
	[SerializeField]
	private KleiPermitDioramaVis_BuildingRocket buildingRocketVis;

	// Token: 0x04007CA7 RID: 31911
	[SerializeField]
	private KleiPermitDioramaVis_BuildingOnFloor buildingOnFloorBotanicalVis;

	// Token: 0x04007CA8 RID: 31912
	[SerializeField]
	private KleiPermitDioramaVis_BuildingHangingHook buildingHangingHookBotanicalVis;

	// Token: 0x04007CA9 RID: 31913
	[SerializeField]
	private KleiPermitDioramaVis_WiresAndAutomation buildingWiresAndAutomationVis;

	// Token: 0x04007CAA RID: 31914
	[SerializeField]
	private KleiPermitDioramaVis_AutomationGates buildingAutomationGatesVis;

	// Token: 0x04007CAB RID: 31915
	[SerializeField]
	private KleiPermitDioramaVis_Wallpaper wallpaperVis;

	// Token: 0x04007CAC RID: 31916
	[SerializeField]
	private KleiPermitDioramaVis_ArtablePainting artablePaintingVis;

	// Token: 0x04007CAD RID: 31917
	[SerializeField]
	private KleiPermitDioramaVis_ArtableSculpture artableSculptureVis;

	// Token: 0x04007CAE RID: 31918
	[SerializeField]
	private KleiPermitDioramaVis_JoyResponseBalloon joyResponseBalloonVis;

	// Token: 0x04007CAF RID: 31919
	[SerializeField]
	private KleiPermitDioramaVis_MonumentPart monumentPartVis;

	// Token: 0x04007CB0 RID: 31920
	private bool initComplete;

	// Token: 0x04007CB1 RID: 31921
	private IReadOnlyList<IKleiPermitDioramaVisTarget> allVisList;

	// Token: 0x04007CB2 RID: 31922
	public static PermitResource lastRenderedPermit;
}
