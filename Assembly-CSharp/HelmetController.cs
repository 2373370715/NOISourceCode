using System;
using UnityEngine;

// Token: 0x020012FA RID: 4858
[AddComponentMenu("KMonoBehaviour/scripts/HelmetController")]
public class HelmetController : KMonoBehaviour
{
	// Token: 0x0600639F RID: 25503 RVA: 0x000E5834 File Offset: 0x000E3A34
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<HelmetController>(-1617557748, HelmetController.OnEquippedDelegate);
		base.Subscribe<HelmetController>(-170173755, HelmetController.OnUnequippedDelegate);
	}

	// Token: 0x060063A0 RID: 25504 RVA: 0x002C8F10 File Offset: 0x002C7110
	private KBatchedAnimController GetAssigneeController()
	{
		Equippable component = base.GetComponent<Equippable>();
		if (component.assignee != null)
		{
			GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
			if (assigneeGameObject)
			{
				return assigneeGameObject.GetComponent<KBatchedAnimController>();
			}
		}
		return null;
	}

	// Token: 0x060063A1 RID: 25505 RVA: 0x002C8F4C File Offset: 0x002C714C
	private GameObject GetAssigneeGameObject(IAssignableIdentity ass_id)
	{
		GameObject result = null;
		MinionAssignablesProxy minionAssignablesProxy = ass_id as MinionAssignablesProxy;
		if (minionAssignablesProxy)
		{
			result = minionAssignablesProxy.GetTargetGameObject();
		}
		else
		{
			MinionIdentity minionIdentity = ass_id as MinionIdentity;
			if (minionIdentity)
			{
				result = minionIdentity.gameObject;
			}
		}
		return result;
	}

	// Token: 0x060063A2 RID: 25506 RVA: 0x002C8F8C File Offset: 0x002C718C
	private void OnEquipped(object data)
	{
		Equippable component = base.GetComponent<Equippable>();
		this.ShowHelmet();
		GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
		assigneeGameObject.Subscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
		assigneeGameObject.Subscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
		assigneeGameObject.Subscribe(1347184327, new Action<object>(this.OnPathAdvanced));
		this.in_tube = false;
		this.is_flying = false;
		this.owner_navigator = assigneeGameObject.GetComponent<Navigator>();
	}

	// Token: 0x060063A3 RID: 25507 RVA: 0x002C9018 File Offset: 0x002C7218
	private void OnUnequipped(object data)
	{
		this.owner_navigator = null;
		Equippable component = base.GetComponent<Equippable>();
		if (component != null)
		{
			this.HideHelmet();
			if (component.assignee != null)
			{
				GameObject assigneeGameObject = this.GetAssigneeGameObject(component.assignee);
				if (assigneeGameObject)
				{
					assigneeGameObject.Unsubscribe(961737054, new Action<object>(this.OnBeginRecoverBreath));
					assigneeGameObject.Unsubscribe(-2037519664, new Action<object>(this.OnEndRecoverBreath));
					assigneeGameObject.Unsubscribe(1347184327, new Action<object>(this.OnPathAdvanced));
				}
			}
		}
	}

	// Token: 0x060063A4 RID: 25508 RVA: 0x002C90A4 File Offset: 0x002C72A4
	private void ShowHelmet()
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = new KAnimHashedString("snapTo_neck");
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			KAnimFile anim = Assets.GetAnim(this.anim_file);
			assigneeController.GetComponent<SymbolOverrideController>().AddSymbolOverride(kanimHashedString, anim.GetData().build.GetSymbol(kanimHashedString), 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, true);
		this.is_shown = true;
		this.UpdateJets();
	}

	// Token: 0x060063A5 RID: 25509 RVA: 0x002C9128 File Offset: 0x002C7328
	private void HideHelmet()
	{
		this.is_shown = false;
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return;
		}
		KAnimHashedString kanimHashedString = "snapTo_neck";
		if (!string.IsNullOrEmpty(this.anim_file))
		{
			SymbolOverrideController component = assigneeController.GetComponent<SymbolOverrideController>();
			if (component == null)
			{
				return;
			}
			component.RemoveSymbolOverride(kanimHashedString, 6);
		}
		assigneeController.SetSymbolVisiblity(kanimHashedString, false);
		this.UpdateJets();
	}

	// Token: 0x060063A6 RID: 25510 RVA: 0x000E585E File Offset: 0x000E3A5E
	private void UpdateJets()
	{
		if (this.is_shown && this.is_flying)
		{
			this.EnableJets();
			return;
		}
		this.DisableJets();
	}

	// Token: 0x060063A7 RID: 25511 RVA: 0x002C9194 File Offset: 0x002C7394
	private void EnableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		if (this.jet_go)
		{
			return;
		}
		this.jet_go = this.AddTrackedAnim("jet", Assets.GetAnim("jetsuit_thruster_fx_kanim"), "loop", Grid.SceneLayer.Creatures, "snapTo_neck");
		this.glow_go = this.AddTrackedAnim("glow", Assets.GetAnim("jetsuit_thruster_glow_fx_kanim"), "loop", Grid.SceneLayer.Front, "snapTo_neck");
	}

	// Token: 0x060063A8 RID: 25512 RVA: 0x000E587D File Offset: 0x000E3A7D
	private void DisableJets()
	{
		if (!this.has_jets)
		{
			return;
		}
		UnityEngine.Object.Destroy(this.jet_go);
		this.jet_go = null;
		UnityEngine.Object.Destroy(this.glow_go);
		this.glow_go = null;
	}

	// Token: 0x060063A9 RID: 25513 RVA: 0x002C9210 File Offset: 0x002C7410
	private GameObject AddTrackedAnim(string name, KAnimFile tracked_anim_file, string anim_clip, Grid.SceneLayer layer, string symbol_name)
	{
		KBatchedAnimController assigneeController = this.GetAssigneeController();
		if (assigneeController == null)
		{
			return null;
		}
		string name2 = assigneeController.name + "." + name;
		GameObject gameObject = new GameObject(name2);
		gameObject.SetActive(false);
		gameObject.transform.parent = assigneeController.transform;
		gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name2);
		KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
		kbatchedAnimController.AnimFiles = new KAnimFile[]
		{
			tracked_anim_file
		};
		kbatchedAnimController.initialAnim = anim_clip;
		kbatchedAnimController.isMovable = true;
		kbatchedAnimController.sceneLayer = layer;
		gameObject.AddComponent<KBatchedAnimTracker>().symbol = symbol_name;
		bool flag;
		Vector3 position = assigneeController.GetSymbolTransform(symbol_name, out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(layer);
		gameObject.transform.SetPosition(position);
		gameObject.SetActive(true);
		kbatchedAnimController.Play(anim_clip, KAnim.PlayMode.Loop, 1f, 0f);
		return gameObject;
	}

	// Token: 0x060063AA RID: 25514 RVA: 0x000E58AC File Offset: 0x000E3AAC
	private void OnBeginRecoverBreath(object data)
	{
		this.HideHelmet();
	}

	// Token: 0x060063AB RID: 25515 RVA: 0x000E58B4 File Offset: 0x000E3AB4
	private void OnEndRecoverBreath(object data)
	{
		this.ShowHelmet();
	}

	// Token: 0x060063AC RID: 25516 RVA: 0x002C930C File Offset: 0x002C750C
	private void OnPathAdvanced(object data)
	{
		if (this.owner_navigator == null)
		{
			return;
		}
		bool flag = this.owner_navigator.CurrentNavType == NavType.Hover;
		bool flag2 = this.owner_navigator.CurrentNavType == NavType.Tube;
		if (flag2 != this.in_tube)
		{
			this.in_tube = flag2;
			if (this.in_tube)
			{
				this.HideHelmet();
			}
			else
			{
				this.ShowHelmet();
			}
		}
		if (flag != this.is_flying)
		{
			this.is_flying = flag;
			this.UpdateJets();
		}
	}

	// Token: 0x04004766 RID: 18278
	public string anim_file;

	// Token: 0x04004767 RID: 18279
	public bool has_jets;

	// Token: 0x04004768 RID: 18280
	private bool is_shown;

	// Token: 0x04004769 RID: 18281
	private bool in_tube;

	// Token: 0x0400476A RID: 18282
	private bool is_flying;

	// Token: 0x0400476B RID: 18283
	private Navigator owner_navigator;

	// Token: 0x0400476C RID: 18284
	private GameObject jet_go;

	// Token: 0x0400476D RID: 18285
	private GameObject glow_go;

	// Token: 0x0400476E RID: 18286
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnEquippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnEquipped(data);
	});

	// Token: 0x0400476F RID: 18287
	private static readonly EventSystem.IntraObjectHandler<HelmetController> OnUnequippedDelegate = new EventSystem.IntraObjectHandler<HelmetController>(delegate(HelmetController component, object data)
	{
		component.OnUnequipped(data);
	});
}
