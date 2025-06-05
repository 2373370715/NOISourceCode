using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/MinimumOperatingTemperature")]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
	// Token: 0x06003250 RID: 12880 RVA: 0x000C5179 File Offset: 0x000C3379
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.TestTemperature(true);
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x000C5188 File Offset: 0x000C3388
	public void Sim200ms(float dt)
	{
		this.TestTemperature(false);
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x00210474 File Offset: 0x0020E674
	private void TestTemperature(bool force)
	{
		bool flag;
		if (this.primaryElement.Temperature < this.minimumTemperature)
		{
			flag = false;
		}
		else
		{
			flag = true;
			for (int i = 0; i < this.building.PlacementCells.Length; i++)
			{
				int i2 = this.building.PlacementCells[i];
				float num = Grid.Temperature[i2];
				float num2 = Grid.Mass[i2];
				if ((num != 0f || num2 != 0f) && num < this.minimumTemperature)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			this.lastOffTime = Time.time;
		}
		if ((flag != this.isWarm && !flag) || (flag != this.isWarm && flag && Time.time > this.lastOffTime + 5f) || force)
		{
			this.isWarm = flag;
			this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, this);
		}
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x000C5191 File Offset: 0x000C3391
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x00210580 File Offset: 0x0020E780
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false);
		list.Add(item);
		return list;
	}

	// Token: 0x0400226C RID: 8812
	[MyCmpReq]
	private Building building;

	// Token: 0x0400226D RID: 8813
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400226E RID: 8814
	[MyCmpReq]
	private PrimaryElement primaryElement;

	// Token: 0x0400226F RID: 8815
	public float minimumTemperature = 275.15f;

	// Token: 0x04002270 RID: 8816
	private const float TURN_ON_DELAY = 5f;

	// Token: 0x04002271 RID: 8817
	private float lastOffTime;

	// Token: 0x04002272 RID: 8818
	public static readonly Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);

	// Token: 0x04002273 RID: 8819
	private bool isWarm;

	// Token: 0x04002274 RID: 8820
	private HandleVector<int>.Handle partitionerEntry;
}
