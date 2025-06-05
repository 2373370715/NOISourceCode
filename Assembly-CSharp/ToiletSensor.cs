using System;

// Token: 0x02000854 RID: 2132
public class ToiletSensor : Sensor
{
	// Token: 0x060025A7 RID: 9639 RVA: 0x000BD243 File Offset: 0x000BB443
	public ToiletSensor(Sensors sensors) : base(sensors)
	{
		this.navigator = base.GetComponent<Navigator>();
	}

	// Token: 0x060025A8 RID: 9640 RVA: 0x001D9D08 File Offset: 0x001D7F08
	public override void Update()
	{
		IUsable usable = null;
		int num = int.MaxValue;
		bool flag = false;
		foreach (IUsable usable2 in Components.Toilets.Items)
		{
			if (usable2.IsUsable())
			{
				flag = true;
				int navigationCost = this.navigator.GetNavigationCost(Grid.PosToCell(usable2.transform.GetPosition()));
				if (navigationCost != -1 && navigationCost < num)
				{
					usable = usable2;
					num = navigationCost;
				}
			}
		}
		bool flag2 = Components.Toilets.Count > 0;
		if (usable != this.toilet || flag2 != this.areThereAnyToilets || this.areThereAnyUsableToilets != flag)
		{
			this.toilet = usable;
			this.areThereAnyToilets = flag2;
			this.areThereAnyUsableToilets = flag;
			base.Trigger(-752545459, null);
		}
	}

	// Token: 0x060025A9 RID: 9641 RVA: 0x000BD258 File Offset: 0x000BB458
	public bool AreThereAnyToilets()
	{
		return this.areThereAnyToilets;
	}

	// Token: 0x060025AA RID: 9642 RVA: 0x000BD260 File Offset: 0x000BB460
	public bool AreThereAnyUsableToilets()
	{
		return this.areThereAnyUsableToilets;
	}

	// Token: 0x060025AB RID: 9643 RVA: 0x000BD268 File Offset: 0x000BB468
	public IUsable GetNearestUsableToilet()
	{
		return this.toilet;
	}

	// Token: 0x040019EC RID: 6636
	private Navigator navigator;

	// Token: 0x040019ED RID: 6637
	private IUsable toilet;

	// Token: 0x040019EE RID: 6638
	private bool areThereAnyToilets;

	// Token: 0x040019EF RID: 6639
	private bool areThereAnyUsableToilets;
}
