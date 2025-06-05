using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x02001CC9 RID: 7369
public class ColorSet : ScriptableObject
{
	// Token: 0x060099C1 RID: 39361 RVA: 0x003C5040 File Offset: 0x003C3240
	private void Init()
	{
		if (this.namedLookup == null)
		{
			this.namedLookup = new Dictionary<string, Color32>();
			foreach (FieldInfo fieldInfo in typeof(ColorSet).GetFields())
			{
				if (fieldInfo.FieldType == typeof(Color32))
				{
					this.namedLookup[fieldInfo.Name] = (Color32)fieldInfo.GetValue(this);
				}
			}
		}
	}

	// Token: 0x060099C2 RID: 39362 RVA: 0x00108661 File Offset: 0x00106861
	public Color32 GetColorByName(string name)
	{
		this.Init();
		return this.namedLookup[name];
	}

	// Token: 0x060099C3 RID: 39363 RVA: 0x00108675 File Offset: 0x00106875
	public void RefreshLookup()
	{
		this.namedLookup = null;
		this.Init();
	}

	// Token: 0x060099C4 RID: 39364 RVA: 0x00108684 File Offset: 0x00106884
	public bool IsDefaultColorSet()
	{
		return Array.IndexOf<ColorSet>(GlobalAssets.Instance.colorSetOptions, this) == 0;
	}

	// Token: 0x040077A2 RID: 30626
	public string settingName;

	// Token: 0x040077A3 RID: 30627
	[Header("Logic")]
	public Color32 logicOn;

	// Token: 0x040077A4 RID: 30628
	public Color32 logicOff;

	// Token: 0x040077A5 RID: 30629
	public Color32 logicDisconnected;

	// Token: 0x040077A6 RID: 30630
	public Color32 logicOnText;

	// Token: 0x040077A7 RID: 30631
	public Color32 logicOffText;

	// Token: 0x040077A8 RID: 30632
	public Color32 logicOnSidescreen;

	// Token: 0x040077A9 RID: 30633
	public Color32 logicOffSidescreen;

	// Token: 0x040077AA RID: 30634
	[Header("Decor")]
	public Color32 decorPositive;

	// Token: 0x040077AB RID: 30635
	public Color32 decorNegative;

	// Token: 0x040077AC RID: 30636
	public Color32 decorBaseline;

	// Token: 0x040077AD RID: 30637
	public Color32 decorHighlightPositive;

	// Token: 0x040077AE RID: 30638
	public Color32 decorHighlightNegative;

	// Token: 0x040077AF RID: 30639
	[Header("Crop Overlay")]
	public Color32 cropHalted;

	// Token: 0x040077B0 RID: 30640
	public Color32 cropGrowing;

	// Token: 0x040077B1 RID: 30641
	public Color32 cropGrown;

	// Token: 0x040077B2 RID: 30642
	[Header("Harvest Overlay")]
	public Color32 harvestEnabled;

	// Token: 0x040077B3 RID: 30643
	public Color32 harvestDisabled;

	// Token: 0x040077B4 RID: 30644
	[Header("Gameplay Events")]
	public Color32 eventPositive;

	// Token: 0x040077B5 RID: 30645
	public Color32 eventNegative;

	// Token: 0x040077B6 RID: 30646
	public Color32 eventNeutral;

	// Token: 0x040077B7 RID: 30647
	[Header("Notifications")]
	public Color32 NotificationNormal;

	// Token: 0x040077B8 RID: 30648
	public Color32 NotificationNormalBG;

	// Token: 0x040077B9 RID: 30649
	public Color32 NotificationBad;

	// Token: 0x040077BA RID: 30650
	public Color32 NotificationBadBG;

	// Token: 0x040077BB RID: 30651
	public Color32 NotificationEvent;

	// Token: 0x040077BC RID: 30652
	public Color32 NotificationEventBG;

	// Token: 0x040077BD RID: 30653
	public Color32 NotificationMessage;

	// Token: 0x040077BE RID: 30654
	public Color32 NotificationMessageBG;

	// Token: 0x040077BF RID: 30655
	public Color32 NotificationMessageImportant;

	// Token: 0x040077C0 RID: 30656
	public Color32 NotificationMessageImportantBG;

	// Token: 0x040077C1 RID: 30657
	public Color32 NotificationTutorial;

	// Token: 0x040077C2 RID: 30658
	public Color32 NotificationTutorialBG;

	// Token: 0x040077C3 RID: 30659
	[Header("PrioritiesScreen")]
	public Color32 PrioritiesNeutralColor;

	// Token: 0x040077C4 RID: 30660
	public Color32 PrioritiesLowColor;

	// Token: 0x040077C5 RID: 30661
	public Color32 PrioritiesHighColor;

	// Token: 0x040077C6 RID: 30662
	[Header("Info Screen Status Items")]
	public Color32 statusItemBad;

	// Token: 0x040077C7 RID: 30663
	public Color32 statusItemEvent;

	// Token: 0x040077C8 RID: 30664
	public Color32 statusItemMessageImportant;

	// Token: 0x040077C9 RID: 30665
	[Header("Germ Overlay")]
	public Color32 germFoodPoisoning;

	// Token: 0x040077CA RID: 30666
	public Color32 germPollenGerms;

	// Token: 0x040077CB RID: 30667
	public Color32 germSlimeLung;

	// Token: 0x040077CC RID: 30668
	public Color32 germZombieSpores;

	// Token: 0x040077CD RID: 30669
	public Color32 germRadiationSickness;

	// Token: 0x040077CE RID: 30670
	[Header("Room Overlay")]
	public Color32 roomNone;

	// Token: 0x040077CF RID: 30671
	public Color32 roomFood;

	// Token: 0x040077D0 RID: 30672
	public Color32 roomSleep;

	// Token: 0x040077D1 RID: 30673
	public Color32 roomRecreation;

	// Token: 0x040077D2 RID: 30674
	public Color32 roomBathroom;

	// Token: 0x040077D3 RID: 30675
	public Color32 roomHospital;

	// Token: 0x040077D4 RID: 30676
	public Color32 roomIndustrial;

	// Token: 0x040077D5 RID: 30677
	public Color32 roomAgricultural;

	// Token: 0x040077D6 RID: 30678
	public Color32 roomScience;

	// Token: 0x040077D7 RID: 30679
	public Color32 roomBionic;

	// Token: 0x040077D8 RID: 30680
	public Color32 roomPark;

	// Token: 0x040077D9 RID: 30681
	[Header("Power Overlay")]
	public Color32 powerConsumer;

	// Token: 0x040077DA RID: 30682
	public Color32 powerGenerator;

	// Token: 0x040077DB RID: 30683
	public Color32 powerBuildingDisabled;

	// Token: 0x040077DC RID: 30684
	public Color32 powerCircuitUnpowered;

	// Token: 0x040077DD RID: 30685
	public Color32 powerCircuitSafe;

	// Token: 0x040077DE RID: 30686
	public Color32 powerCircuitStraining;

	// Token: 0x040077DF RID: 30687
	public Color32 powerCircuitOverloading;

	// Token: 0x040077E0 RID: 30688
	[Header("Light Overlay")]
	public Color32 lightOverlay;

	// Token: 0x040077E1 RID: 30689
	[Header("Conduit Overlay")]
	public Color32 conduitNormal;

	// Token: 0x040077E2 RID: 30690
	public Color32 conduitInsulated;

	// Token: 0x040077E3 RID: 30691
	public Color32 conduitRadiant;

	// Token: 0x040077E4 RID: 30692
	[Header("Temperature Overlay")]
	public Color32 temperatureThreshold0;

	// Token: 0x040077E5 RID: 30693
	public Color32 temperatureThreshold1;

	// Token: 0x040077E6 RID: 30694
	public Color32 temperatureThreshold2;

	// Token: 0x040077E7 RID: 30695
	public Color32 temperatureThreshold3;

	// Token: 0x040077E8 RID: 30696
	public Color32 temperatureThreshold4;

	// Token: 0x040077E9 RID: 30697
	public Color32 temperatureThreshold5;

	// Token: 0x040077EA RID: 30698
	public Color32 temperatureThreshold6;

	// Token: 0x040077EB RID: 30699
	public Color32 temperatureThreshold7;

	// Token: 0x040077EC RID: 30700
	public Color32 heatflowThreshold0;

	// Token: 0x040077ED RID: 30701
	public Color32 heatflowThreshold1;

	// Token: 0x040077EE RID: 30702
	public Color32 heatflowThreshold2;

	// Token: 0x040077EF RID: 30703
	private Dictionary<string, Color32> namedLookup;
}
