﻿using System;
using Database;
using UnityEngine;

// Token: 0x02001DC0 RID: 7616
public static class KleiPermitVisUtil
{
	// Token: 0x06009F1C RID: 40732 RVA: 0x003DE504 File Offset: 0x003DC704
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingFacadeResource buildingPermit)
	{
		KAnimFile anim = Assets.GetAnim(buildingPermit.AnimFile);
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			anim
		});
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(anim), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009F1D RID: 40733 RVA: 0x003DE56C File Offset: 0x003DC76C
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, BuildingDef buildingDef)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(buildingDef.AnimFiles);
		buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(buildingDef.AnimFiles[0]), KAnim.PlayMode.Loop, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009F1E RID: 40734 RVA: 0x003DE5C4 File Offset: 0x003DC7C4
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, ArtableStage artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			Assets.GetAnim(artablePermit.animFile)
		});
		buildingKAnim.Play(artablePermit.anim, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009F1F RID: 40735 RVA: 0x003DE62C File Offset: 0x003DC82C
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, DbStickerBomb artablePermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			artablePermit.animFile
		});
		HashedString defaultStickerAnimHash = KleiPermitVisUtil.GetDefaultStickerAnimHash(artablePermit.animFile);
		if (defaultStickerAnimHash != null)
		{
			buildingKAnim.Play(defaultStickerAnimHash, KAnim.PlayMode.Once, 1f, 0f);
		}
		else
		{
			global::Debug.Assert(false, "Couldn't find default sticker for sticker " + artablePermit.Id);
			buildingKAnim.Play(KleiPermitVisUtil.GetFirstAnimHash(artablePermit.animFile), KAnim.PlayMode.Once, 1f, 0f);
		}
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009F20 RID: 40736 RVA: 0x003DE6D0 File Offset: 0x003DC8D0
	public static void ConfigureToRenderBuilding(KBatchedAnimController buildingKAnim, MonumentPartResource monumentPermit)
	{
		buildingKAnim.Stop();
		buildingKAnim.SwapAnims(new KAnimFile[]
		{
			monumentPermit.AnimFile
		});
		buildingKAnim.Play(monumentPermit.State, KAnim.PlayMode.Once, 1f, 0f);
		buildingKAnim.rectTransform().sizeDelta = 176f * Vector2.one;
	}

	// Token: 0x06009F21 RID: 40737 RVA: 0x003DE730 File Offset: 0x003DC930
	public static void ConfigureBuildingPosition(RectTransform transform, PrefabDefinedUIPosition anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		anchorPosition.SetOn(transform);
		transform.anchoredPosition += new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x06009F22 RID: 40738 RVA: 0x003DE78C File Offset: 0x003DC98C
	public static void ConfigureBuildingPosition(RectTransform transform, Vector2 anchorPosition, BuildingDef buildingDef, Alignment alignment)
	{
		transform.anchoredPosition = anchorPosition + new Vector2(176f * (float)buildingDef.WidthInCells * -(alignment.x - 0.5f), 176f * (float)buildingDef.HeightInCells * -alignment.y);
	}

	// Token: 0x06009F23 RID: 40739 RVA: 0x0010BF3D File Offset: 0x0010A13D
	public static void ClearAnimation()
	{
		if (!KleiPermitVisUtil.buildingAnimateIn.IsNullOrDestroyed())
		{
			UnityEngine.Object.Destroy(KleiPermitVisUtil.buildingAnimateIn.gameObject);
		}
	}

	// Token: 0x06009F24 RID: 40740 RVA: 0x0010BF5A File Offset: 0x0010A15A
	public static void AnimateIn(KBatchedAnimController buildingKAnim, Updater extraUpdater = default(Updater))
	{
		KleiPermitVisUtil.ClearAnimation();
		KleiPermitVisUtil.buildingAnimateIn = KleiPermitBuildingAnimateIn.MakeFor(buildingKAnim, extraUpdater);
	}

	// Token: 0x06009F25 RID: 40741 RVA: 0x0010BF6D File Offset: 0x0010A16D
	public static HashedString GetFirstAnimHash(KAnimFile animFile)
	{
		return animFile.GetData().GetAnim(0).hash;
	}

	// Token: 0x06009F26 RID: 40742 RVA: 0x003DE7DC File Offset: 0x003DC9DC
	public static HashedString GetDefaultStickerAnimHash(KAnimFile stickerAnimFile)
	{
		KAnimFileData data = stickerAnimFile.GetData();
		for (int i = 0; i < data.animCount; i++)
		{
			KAnim.Anim anim = data.GetAnim(i);
			if (anim.name.StartsWith("idle_sticker"))
			{
				return anim.hash;
			}
		}
		return null;
	}

	// Token: 0x06009F27 RID: 40743 RVA: 0x003DE828 File Offset: 0x003DCA28
	public static BuildLocationRule? GetBuildLocationRule(PermitResource permit)
	{
		BuildingDef buildingDef = KleiPermitVisUtil.GetBuildingDef(permit);
		if (buildingDef == null)
		{
			return null;
		}
		return new BuildLocationRule?(buildingDef.BuildLocationRule);
	}

	// Token: 0x06009F28 RID: 40744 RVA: 0x003DE85C File Offset: 0x003DCA5C
	public static BuildingDef GetBuildingDef(PermitResource permit)
	{
		BuildingFacadeResource buildingFacadeResource = permit as BuildingFacadeResource;
		if (buildingFacadeResource != null)
		{
			GameObject gameObject = Assets.TryGetPrefab(buildingFacadeResource.PrefabID);
			if (gameObject == null)
			{
				return null;
			}
			BuildingComplete component = gameObject.GetComponent<BuildingComplete>();
			if (component == null || !component)
			{
				return null;
			}
			return component.Def;
		}
		else
		{
			ArtableStage artableStage = permit as ArtableStage;
			if (artableStage != null)
			{
				BuildingComplete component2 = Assets.GetPrefab(artableStage.prefabId).GetComponent<BuildingComplete>();
				if (component2 == null || !component2)
				{
					return null;
				}
				return component2.Def;
			}
			else
			{
				if (!(permit is MonumentPartResource))
				{
					return null;
				}
				BuildingComplete component3 = Assets.GetPrefab("MonumentBottom").GetComponent<BuildingComplete>();
				if (component3 == null || !component3)
				{
					return null;
				}
				return component3.Def;
			}
		}
	}

	// Token: 0x04007CF0 RID: 31984
	public const float TILE_SIZE_UI = 176f;

	// Token: 0x04007CF1 RID: 31985
	public static KleiPermitBuildingAnimateIn buildingAnimateIn;
}
