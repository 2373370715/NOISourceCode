using System;
using UnityEngine;

// Token: 0x02000D8B RID: 3467
public class EightDirectionController
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x0600435C RID: 17244 RVA: 0x000CFED8 File Offset: 0x000CE0D8
	// (set) Token: 0x0600435D RID: 17245 RVA: 0x000CFEE0 File Offset: 0x000CE0E0
	public KBatchedAnimController controller { get; private set; }

	// Token: 0x0600435E RID: 17246 RVA: 0x000CFEE9 File Offset: 0x000CE0E9
	public EightDirectionController(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBank)
	{
		this.Initialize(buildingController, targetSymbol, defaultAnim, frontBank, Grid.SceneLayer.NoLayer);
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00252A40 File Offset: 0x00250C40
	private void Initialize(KAnimControllerBase buildingController, string targetSymbol, string defaultAnim, EightDirectionController.Offset frontBack, Grid.SceneLayer userSpecifiedRenderLayer)
	{
		string name = buildingController.name + ".eight_direction";
		this.gameObject = new GameObject(name);
		this.gameObject.SetActive(false);
		this.gameObject.transform.parent = buildingController.transform;
		this.gameObject.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
		this.defaultAnim = defaultAnim;
		this.controller = this.gameObject.AddOrGet<KBatchedAnimController>();
		this.controller.AnimFiles = new KAnimFile[]
		{
			buildingController.AnimFiles[0]
		};
		this.controller.initialAnim = defaultAnim;
		this.controller.isMovable = true;
		this.controller.sceneLayer = Grid.SceneLayer.NoLayer;
		if (EightDirectionController.Offset.UserSpecified == frontBack)
		{
			this.controller.sceneLayer = userSpecifiedRenderLayer;
		}
		buildingController.SetSymbolVisiblity(targetSymbol, false);
		bool flag;
		Vector3 position = buildingController.GetSymbolTransform(new HashedString(targetSymbol), out flag).GetColumn(3);
		switch (frontBack)
		{
		case EightDirectionController.Offset.Infront:
			position.z = buildingController.transform.GetPosition().z - 0.1f;
			break;
		case EightDirectionController.Offset.Behind:
			position.z = buildingController.transform.GetPosition().z + 0.1f;
			break;
		case EightDirectionController.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(userSpecifiedRenderLayer);
			break;
		}
		this.gameObject.transform.SetPosition(position);
		this.gameObject.SetActive(true);
		this.link = new KAnimLink(buildingController, this.controller);
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x000CFEFE File Offset: 0x000CE0FE
	public void SetPositionPercent(float percent_full)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.SetPositionPercent(percent_full);
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x000CFF1B File Offset: 0x000CE11B
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.controller != null)
		{
			this.controller.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x000CFF3D File Offset: 0x000CE13D
	public void SetRotation(float rot)
	{
		if (this.controller == null)
		{
			return;
		}
		this.controller.Rotation = rot;
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x000CFF5A File Offset: 0x000CE15A
	public void PlayAnim(string anim, KAnim.PlayMode mode = KAnim.PlayMode.Once)
	{
		this.controller.Play(anim, mode, 1f, 0f);
	}

	// Token: 0x04002E92 RID: 11922
	public GameObject gameObject;

	// Token: 0x04002E93 RID: 11923
	private string defaultAnim;

	// Token: 0x04002E94 RID: 11924
	private KAnimLink link;

	// Token: 0x02000D8C RID: 3468
	public enum Offset
	{
		// Token: 0x04002E96 RID: 11926
		Infront,
		// Token: 0x04002E97 RID: 11927
		Behind,
		// Token: 0x04002E98 RID: 11928
		UserSpecified
	}
}
