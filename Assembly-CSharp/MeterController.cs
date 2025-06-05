using System;
using UnityEngine;

// Token: 0x02000EE6 RID: 3814
public class MeterController
{
	// Token: 0x06004C5C RID: 19548 RVA: 0x000BC493 File Offset: 0x000BA693
	public static float StandardLerp(float percentage, int frames)
	{
		return percentage;
	}

	// Token: 0x06004C5D RID: 19549 RVA: 0x000D5B6C File Offset: 0x000D3D6C
	public static float MinMaxStepLerp(float percentage, int frames)
	{
		if ((double)percentage <= 0.0 || frames <= 1)
		{
			return 0f;
		}
		if ((double)percentage >= 1.0 || frames == 2)
		{
			return 1f;
		}
		return (1f + percentage * (float)(frames - 2)) / (float)frames;
	}

	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06004C5E RID: 19550 RVA: 0x000D5BAB File Offset: 0x000D3DAB
	// (set) Token: 0x06004C5F RID: 19551 RVA: 0x000D5BB3 File Offset: 0x000D3DB3
	public KBatchedAnimController meterController { get; private set; }

	// Token: 0x06004C60 RID: 19552 RVA: 0x0026FE70 File Offset: 0x0026E070
	public MeterController(KMonoBehaviour target, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		string[] array = new string[symbols_to_hide.Length + 1];
		Array.Copy(symbols_to_hide, array, symbols_to_hide.Length);
		array[array.Length - 1] = "meter_target";
		KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
		this.Initialize(component, "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, array);
	}

	// Token: 0x06004C61 RID: 19553 RVA: 0x000D5BBC File Offset: 0x000D3DBC
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
	}

	// Token: 0x06004C62 RID: 19554 RVA: 0x000D5BEA File Offset: 0x000D3DEA
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
	}

	// Token: 0x06004C63 RID: 19555 RVA: 0x0026FEDC File Offset: 0x0026E0DC
	private void Initialize(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		if (building_controller.HasAnimation(meter_animation + "_cb") && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
		{
			meter_animation += "_cb";
		}
		string name = building_controller.name + "." + meter_animation;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(MeterConfig.ID));
		gameObject.name = name;
		gameObject.SetActive(false);
		gameObject.transform.parent = building_controller.transform;
		this.gameObject = gameObject;
		gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
		Vector3 position = building_controller.transform.GetPosition();
		switch (front_back)
		{
		case Meter.Offset.Infront:
			position.z -= 0.1f;
			break;
		case Meter.Offset.Behind:
			position.z += 0.1f;
			break;
		case Meter.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(user_specified_render_layer);
			break;
		}
		gameObject.transform.SetPosition(position);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.AnimFiles = new KAnimFile[]
		{
			building_controller.AnimFiles[0]
		};
		component.initialAnim = meter_animation;
		component.fgLayer = Grid.SceneLayer.NoLayer;
		component.initialMode = KAnim.PlayMode.Paused;
		component.isMovable = true;
		component.FlipX = building_controller.FlipX;
		component.FlipY = building_controller.FlipY;
		if (Meter.Offset.UserSpecified == front_back)
		{
			component.sceneLayer = user_specified_render_layer;
		}
		this.meterController = component;
		KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
		component2.offset = tracker_offset;
		component2.symbol = new HashedString(meter_target);
		gameObject.SetActive(true);
		building_controller.SetSymbolVisiblity(meter_target, false);
		if (symbols_to_hide != null)
		{
			for (int i = 0; i < symbols_to_hide.Length; i++)
			{
				building_controller.SetSymbolVisiblity(symbols_to_hide[i], false);
			}
		}
		this.link = new KAnimLink(building_controller, component);
	}

	// Token: 0x06004C64 RID: 19556 RVA: 0x002700AC File Offset: 0x0026E2AC
	public MeterController(KAnimControllerBase building_controller, KBatchedAnimController meter_controller, params string[] symbol_names)
	{
		if (meter_controller == null)
		{
			return;
		}
		this.meterController = meter_controller;
		this.link = new KAnimLink(building_controller, meter_controller);
		for (int i = 0; i < symbol_names.Length; i++)
		{
			building_controller.SetSymbolVisiblity(symbol_names[i], false);
		}
		this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
	}

	// Token: 0x06004C65 RID: 19557 RVA: 0x000D5C15 File Offset: 0x000D3E15
	public void SetPositionPercent(float percent_full)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.SetPositionPercent(this.interpolateFunction(percent_full, this.meterController.GetCurrentNumFrames()));
	}

	// Token: 0x06004C66 RID: 19558 RVA: 0x000D5C48 File Offset: 0x000D3E48
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.meterController != null)
		{
			this.meterController.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06004C67 RID: 19559 RVA: 0x000D5C6A File Offset: 0x000D3E6A
	public void SetRotation(float rot)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.Rotation = rot;
	}

	// Token: 0x06004C68 RID: 19560 RVA: 0x000D5C87 File Offset: 0x000D3E87
	public void Unlink()
	{
		if (this.link != null)
		{
			this.link.Unregister();
			this.link = null;
		}
	}

	// Token: 0x0400357C RID: 13692
	public GameObject gameObject;

	// Token: 0x0400357D RID: 13693
	public Func<float, int, float> interpolateFunction = new Func<float, int, float>(MeterController.MinMaxStepLerp);

	// Token: 0x0400357E RID: 13694
	private KAnimLink link;
}
