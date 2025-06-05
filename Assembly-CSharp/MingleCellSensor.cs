using System;
using UnityEngine;

// Token: 0x0200084E RID: 2126
public class MingleCellSensor : Sensor
{
	// Token: 0x06002581 RID: 9601 RVA: 0x000BCFAC File Offset: 0x000BB1AC
	public MingleCellSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
		this.brain = base.GetComponent<MinionBrain>();
	}

	// Token: 0x06002582 RID: 9602 RVA: 0x001D9954 File Offset: 0x001D7B54
	public override void Update()
	{
		this.cell = Grid.InvalidCell;
		int num = int.MaxValue;
		ListPool<int, MingleCellSensor>.PooledList pooledList = ListPool<int, MingleCellSensor>.Allocate();
		int num2 = 50;
		foreach (int num3 in Game.Instance.mingleCellTracker.mingleCells)
		{
			if (this.brain.IsCellClear(num3))
			{
				int navigationCost = this.navigator.GetNavigationCost(num3);
				if (navigationCost != -1)
				{
					if (num3 == Grid.InvalidCell || navigationCost < num)
					{
						this.cell = num3;
						num = navigationCost;
					}
					if (navigationCost < num2)
					{
						pooledList.Add(num3);
					}
				}
			}
		}
		if (pooledList.Count > 0)
		{
			this.cell = pooledList[UnityEngine.Random.Range(0, pooledList.Count)];
		}
		pooledList.Recycle();
	}

	// Token: 0x06002583 RID: 9603 RVA: 0x000BCFCD File Offset: 0x000BB1CD
	public int GetCell()
	{
		return this.cell;
	}

	// Token: 0x040019DC RID: 6620
	private MinionBrain brain;

	// Token: 0x040019DD RID: 6621
	private Navigator navigator;

	// Token: 0x040019DE RID: 6622
	private int cell;
}
