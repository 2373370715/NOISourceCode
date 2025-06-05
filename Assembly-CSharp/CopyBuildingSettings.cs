using System;
using STRINGS;
using UnityEngine;

// Token: 0x02001151 RID: 4433
[AddComponentMenu("KMonoBehaviour/scripts/CopyBuildingSettings")]
public class CopyBuildingSettings : KMonoBehaviour
{
	// Token: 0x06005A7D RID: 23165 RVA: 0x000DF4C5 File Offset: 0x000DD6C5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CopyBuildingSettings>(493375141, CopyBuildingSettings.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x002A3478 File Offset: 0x002A1678
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_mirror", UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME, new System.Action(this.ActivateCopyTool), global::Action.BuildingUtility1, null, null, null, UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP, true), 1f);
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x000DF4DE File Offset: 0x000DD6DE
	private void ActivateCopyTool()
	{
		CopySettingsTool.Instance.SetSourceObject(base.gameObject);
		PlayerController.Instance.ActivateTool(CopySettingsTool.Instance);
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x002A34D4 File Offset: 0x002A16D4
	public static bool ApplyCopy(int targetCell, GameObject sourceGameObject)
	{
		ObjectLayer layer = ObjectLayer.Building;
		if (sourceGameObject.GetComponent<MoverLayerOccupier>() != null)
		{
			layer = ObjectLayer.Mover;
		}
		Building component = sourceGameObject.GetComponent<BuildingComplete>();
		if (component != null)
		{
			layer = component.Def.ObjectLayer;
		}
		GameObject gameObject = Grid.Objects[targetCell, (int)layer];
		if (gameObject == null)
		{
			return false;
		}
		if (gameObject == sourceGameObject)
		{
			return false;
		}
		KPrefabID component2 = sourceGameObject.GetComponent<KPrefabID>();
		if (component2 == null)
		{
			return false;
		}
		KPrefabID component3 = gameObject.GetComponent<KPrefabID>();
		if (component3 == null)
		{
			return false;
		}
		CopyBuildingSettings component4 = sourceGameObject.GetComponent<CopyBuildingSettings>();
		if (component4 == null)
		{
			return false;
		}
		CopyBuildingSettings component5 = gameObject.GetComponent<CopyBuildingSettings>();
		if (component5 == null)
		{
			return false;
		}
		if (component4.copyGroupTag != Tag.Invalid)
		{
			if (component4.copyGroupTag != component5.copyGroupTag)
			{
				return false;
			}
		}
		else if (component3.PrefabID() != component2.PrefabID())
		{
			return false;
		}
		component3.Trigger(-905833192, sourceGameObject);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, UI.COPIED_SETTINGS, gameObject.transform, new Vector3(0f, 0.5f, 0f), 1.5f, false, false);
		return true;
	}

	// Token: 0x04004080 RID: 16512
	[MyCmpReq]
	private KPrefabID id;

	// Token: 0x04004081 RID: 16513
	public Tag copyGroupTag;

	// Token: 0x04004082 RID: 16514
	private static readonly EventSystem.IntraObjectHandler<CopyBuildingSettings> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CopyBuildingSettings>(delegate(CopyBuildingSettings component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
