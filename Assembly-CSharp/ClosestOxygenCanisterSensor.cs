using System;
using System.Collections.Generic;

// Token: 0x0200084A RID: 2122
public class ClosestOxygenCanisterSensor : ClosestPickupableSensor<Pickupable>
{
	// Token: 0x06002571 RID: 9585 RVA: 0x001D95F8 File Offset: 0x001D77F8
	public ClosestOxygenCanisterSensor(Sensors sensors, bool shouldStartActive) : base(sensors, GameTags.Gas, shouldStartActive)
	{
		this.requiredTags = new Tag[]
		{
			GameTags.Breathable
		};
		this.BreathableGasses = ElementLoader.FindElements((Element element) => element.HasTag(GameTags.Breathable) && element.HasTag(GameTags.Gas));
	}

	// Token: 0x06002572 RID: 9586 RVA: 0x001D9654 File Offset: 0x001D7854
	public override HashSet<Tag> GetForbbidenTags()
	{
		if (this.consumableConsumer == null)
		{
			return new HashSet<Tag>(0);
		}
		HashSet<Tag> forbbidenTags = base.GetForbbidenTags();
		if (forbbidenTags == null || forbbidenTags.Count <= 0)
		{
			return forbbidenTags;
		}
		Tag[] array = new Tag[forbbidenTags.Count];
		base.GetForbbidenTags().CopyTo(array);
		HashSet<Tag> hashSet = new HashSet<Tag>();
		int i = 0;
		while (i < array.Length)
		{
			Tag tag = array[i];
			if (tag == ClosestOxygenCanisterSensor.GenericBreathableGassesTankTag)
			{
				using (List<Element>.Enumerator enumerator = this.BreathableGasses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Element element = enumerator.Current;
						hashSet.Add(element.id.ToString());
					}
					goto IL_BB;
				}
				goto IL_B2;
			}
			goto IL_B2;
			IL_BB:
			i++;
			continue;
			IL_B2:
			hashSet.Add(tag);
			goto IL_BB;
		}
		return hashSet;
	}

	// Token: 0x040019CA RID: 6602
	public static readonly Tag GenericBreathableGassesTankTag = new Tag("BreathableGasTank");

	// Token: 0x040019CB RID: 6603
	private List<Element> BreathableGasses;
}
