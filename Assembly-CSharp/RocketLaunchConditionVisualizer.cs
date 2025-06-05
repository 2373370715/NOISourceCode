using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B20 RID: 2848
public class RocketLaunchConditionVisualizer : KMonoBehaviour
{
	// Token: 0x060034D4 RID: 13524 RVA: 0x00218A24 File Offset: 0x00216C24
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			this.clusterModule = base.GetComponent<RocketModuleCluster>();
		}
		else
		{
			this.launchConditionManager = base.GetComponent<LaunchConditionManager>();
		}
		this.UpdateAllModuleData();
		base.Subscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x060034D5 RID: 13525 RVA: 0x000C6E9C File Offset: 0x000C509C
	protected override void OnCleanUp()
	{
		base.Unsubscribe(1512695988, new Action<object>(this.OnAnyRocketModuleChanged));
	}

	// Token: 0x060034D6 RID: 13526 RVA: 0x000C6EB5 File Offset: 0x000C50B5
	private void OnAnyRocketModuleChanged(object obj)
	{
		this.UpdateAllModuleData();
	}

	// Token: 0x060034D7 RID: 13527 RVA: 0x00218A78 File Offset: 0x00216C78
	private void UpdateAllModuleData()
	{
		if (this.moduleVisualizeData != null)
		{
			this.moduleVisualizeData = null;
		}
		bool flag = this.clusterModule != null;
		List<Ref<RocketModuleCluster>> list = null;
		List<RocketModule> list2 = null;
		if (flag)
		{
			list = new List<Ref<RocketModuleCluster>>(this.clusterModule.CraftInterface.ClusterModules);
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list.Count];
			list.Sort(delegate(Ref<RocketModuleCluster> a, Ref<RocketModuleCluster> b)
			{
				int y = Grid.PosToXY(a.Get().transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.Get().transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
		}
		else
		{
			list2 = new List<RocketModule>(this.launchConditionManager.rocketModules);
			list2.Sort(delegate(RocketModule a, RocketModule b)
			{
				int y = Grid.PosToXY(a.transform.GetPosition()).y;
				int y2 = Grid.PosToXY(b.transform.GetPosition()).y;
				return y.CompareTo(y2);
			});
			this.moduleVisualizeData = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData[list2.Count];
		}
		for (int i = 0; i < this.moduleVisualizeData.Length; i++)
		{
			RocketModule rocketModule = flag ? list[i].Get() : list2[i];
			Building component = rocketModule.GetComponent<Building>();
			this.moduleVisualizeData[i] = new RocketLaunchConditionVisualizer.RocketModuleVisualizeData
			{
				Module = rocketModule,
				RangeMax = Mathf.FloorToInt((float)component.Def.WidthInCells / 2f),
				RangeMin = -Mathf.FloorToInt((float)(component.Def.WidthInCells - 1) / 2f)
			};
		}
	}

	// Token: 0x0400244B RID: 9291
	public RocketLaunchConditionVisualizer.RocketModuleVisualizeData[] moduleVisualizeData;

	// Token: 0x0400244C RID: 9292
	private LaunchConditionManager launchConditionManager;

	// Token: 0x0400244D RID: 9293
	private RocketModuleCluster clusterModule;

	// Token: 0x02000B21 RID: 2849
	public struct RocketModuleVisualizeData
	{
		// Token: 0x0400244E RID: 9294
		public RocketModule Module;

		// Token: 0x0400244F RID: 9295
		public Vector2I OriginOffset;

		// Token: 0x04002450 RID: 9296
		public int RangeMin;

		// Token: 0x04002451 RID: 9297
		public int RangeMax;
	}
}
