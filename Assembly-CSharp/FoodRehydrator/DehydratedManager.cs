using System;
using System.Collections.Generic;
using Klei;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace FoodRehydrator
{
	// Token: 0x02002158 RID: 8536
	public class DehydratedManager : KMonoBehaviour, FewOptionSideScreen.IFewOptionSideScreen
	{
		// Token: 0x0600B60A RID: 46602 RVA: 0x0011AA95 File Offset: 0x00118C95
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe<DehydratedManager>(-905833192, DehydratedManager.OnCopySettingsDelegate);
		}

		// Token: 0x17000BDB RID: 3035
		// (get) Token: 0x0600B60B RID: 46603 RVA: 0x0011AAAE File Offset: 0x00118CAE
		// (set) Token: 0x0600B60C RID: 46604 RVA: 0x00455808 File Offset: 0x00453A08
		public Tag ChosenContent
		{
			get
			{
				return this.chosenContent;
			}
			set
			{
				if (this.chosenContent != value)
				{
					base.GetComponent<ManualDeliveryKG>().RequestedItemTag = value;
					this.chosenContent = value;
					this.packages.DropUnlessHasTag(this.chosenContent);
					if (this.chosenContent != GameTags.Dehydrated)
					{
						AccessabilityManager component = base.GetComponent<AccessabilityManager>();
						if (component != null)
						{
							component.CancelActiveWorkable();
						}
					}
				}
			}
		}

		// Token: 0x0600B60D RID: 46605 RVA: 0x00455870 File Offset: 0x00453A70
		public void SetFabricatedFoodSymbol(Tag material)
		{
			this.foodKBAC.gameObject.SetActive(true);
			GameObject prefab = Assets.GetPrefab(material);
			this.foodKBAC.SwapAnims(prefab.GetComponent<KBatchedAnimController>().AnimFiles);
			this.foodKBAC.Play("object", KAnim.PlayMode.Loop, 1f, 0f);
		}

		// Token: 0x0600B60E RID: 46606 RVA: 0x004558CC File Offset: 0x00453ACC
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Storage[] components = base.GetComponents<Storage>();
			global::Debug.Assert(components.Length == 2);
			this.packages = components[0];
			this.water = components[1];
			this.packagesMeter = new MeterController(base.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, new string[]
			{
				"meter_target"
			});
			base.Subscribe(-1697596308, new Action<object>(this.StorageChangeHandler));
			this.SetupFoodSymbol();
			this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
		}

		// Token: 0x0600B60F RID: 46607 RVA: 0x00455974 File Offset: 0x00453B74
		public void ConsumeResourcesForRehydration(GameObject package, GameObject food)
		{
			global::Debug.Assert(this.packages.items.Contains(package));
			this.packages.ConsumeIgnoringDisease(package);
			float num;
			SimUtil.DiseaseInfo diseaseInfo;
			float num2;
			this.water.ConsumeAndGetDisease(FoodRehydratorConfig.REHYDRATION_TAG, 1f, out num, out diseaseInfo, out num2);
			PrimaryElement component = food.GetComponent<PrimaryElement>();
			if (component != null)
			{
				component.AddDisease(diseaseInfo.idx, diseaseInfo.count, "rehydrating");
				component.SetMassTemperature(component.Mass, component.Temperature * 0.125f + num2 * 0.875f);
			}
		}

		// Token: 0x0600B610 RID: 46608 RVA: 0x0011AAB6 File Offset: 0x00118CB6
		private void StorageChangeHandler(object obj)
		{
			if (((GameObject)obj).GetComponent<DehydratedFoodPackage>() != null)
			{
				this.packagesMeter.SetPositionPercent((float)this.packages.items.Count / 5f);
			}
		}

		// Token: 0x0600B611 RID: 46609 RVA: 0x00455A08 File Offset: 0x00453C08
		private void SetupFoodSymbol()
		{
			GameObject gameObject = Util.NewGameObject(base.gameObject, "food_symbol");
			gameObject.SetActive(false);
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			bool flag;
			Vector3 position = component.GetSymbolTransform(DehydratedManager.HASH_FOOD, out flag).GetColumn(3);
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			gameObject.transform.SetPosition(position);
			this.foodKBAC = gameObject.AddComponent<KBatchedAnimController>();
			this.foodKBAC.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("mushbar_kanim")
			};
			this.foodKBAC.initialAnim = "object";
			component.SetSymbolVisiblity(DehydratedManager.HASH_FOOD, false);
			this.foodKBAC.sceneLayer = Grid.SceneLayer.BuildingUse;
			KBatchedAnimTracker kbatchedAnimTracker = gameObject.AddComponent<KBatchedAnimTracker>();
			kbatchedAnimTracker.symbol = new HashedString("food");
			kbatchedAnimTracker.offset = Vector3.zero;
		}

		// Token: 0x0600B612 RID: 46610 RVA: 0x00455AF0 File Offset: 0x00453CF0
		public FewOptionSideScreen.IFewOptionSideScreen.Option[] GetOptions()
		{
			HashSet<Tag> discoveredResourcesFromTag = DiscoveredResources.Instance.GetDiscoveredResourcesFromTag(GameTags.Dehydrated);
			FewOptionSideScreen.IFewOptionSideScreen.Option[] array = new FewOptionSideScreen.IFewOptionSideScreen.Option[1 + discoveredResourcesFromTag.Count];
			array[0] = new FewOptionSideScreen.IFewOptionSideScreen.Option(GameTags.Dehydrated, UI.UISIDESCREENS.FILTERSIDESCREEN.DRIEDFOOD, Def.GetUISprite("icon_category_food", "ui", false), "");
			int num = 1;
			foreach (Tag tag in discoveredResourcesFromTag)
			{
				array[num] = new FewOptionSideScreen.IFewOptionSideScreen.Option(tag, tag.ProperName(), Def.GetUISprite(tag, "ui", false), "");
				num++;
			}
			return array;
		}

		// Token: 0x0600B613 RID: 46611 RVA: 0x0011AAED File Offset: 0x00118CED
		public void OnOptionSelected(FewOptionSideScreen.IFewOptionSideScreen.Option option)
		{
			this.ChosenContent = option.tag;
		}

		// Token: 0x0600B614 RID: 46612 RVA: 0x0011AAAE File Offset: 0x00118CAE
		public Tag GetSelectedOption()
		{
			return this.chosenContent;
		}

		// Token: 0x0600B615 RID: 46613 RVA: 0x00455BBC File Offset: 0x00453DBC
		protected void OnCopySettings(object data)
		{
			GameObject gameObject = data as GameObject;
			if (gameObject != null)
			{
				DehydratedManager component = gameObject.GetComponent<DehydratedManager>();
				if (component != null)
				{
					this.ChosenContent = component.ChosenContent;
				}
			}
		}

		// Token: 0x0400900F RID: 36879
		[MyCmpAdd]
		private CopyBuildingSettings copyBuildingSettings;

		// Token: 0x04009010 RID: 36880
		private Storage packages;

		// Token: 0x04009011 RID: 36881
		private Storage water;

		// Token: 0x04009012 RID: 36882
		private MeterController packagesMeter;

		// Token: 0x04009013 RID: 36883
		private static string HASH_FOOD = "food";

		// Token: 0x04009014 RID: 36884
		private KBatchedAnimController foodKBAC;

		// Token: 0x04009015 RID: 36885
		private static readonly EventSystem.IntraObjectHandler<DehydratedManager> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<DehydratedManager>(delegate(DehydratedManager component, object data)
		{
			component.OnCopySettings(data);
		});

		// Token: 0x04009016 RID: 36886
		[Serialize]
		private Tag chosenContent = GameTags.Dehydrated;
	}
}
