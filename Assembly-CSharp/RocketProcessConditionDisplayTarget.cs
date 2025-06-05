using System;
using System.Collections.Generic;

// Token: 0x020019E9 RID: 6633
public class RocketProcessConditionDisplayTarget : KMonoBehaviour, IProcessConditionSet, ISim1000ms
{
	// Token: 0x06008A40 RID: 35392 RVA: 0x000FEE31 File Offset: 0x000FD031
	public List<ProcessCondition> GetConditionSet(ProcessCondition.ProcessConditionType conditionType)
	{
		if (this.craftModuleInterface == null)
		{
			this.craftModuleInterface = base.GetComponent<RocketModuleCluster>().CraftInterface;
		}
		return this.craftModuleInterface.GetConditionSet(conditionType);
	}

	// Token: 0x06008A41 RID: 35393 RVA: 0x003696EC File Offset: 0x003678EC
	public void Sim1000ms(float dt)
	{
		bool flag = false;
		using (List<ProcessCondition>.Enumerator enumerator = this.GetConditionSet(ProcessCondition.ProcessConditionType.All).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.EvaluateCondition() == ProcessCondition.Status.Failure)
				{
					flag = true;
					if (this.statusHandle == Guid.Empty)
					{
						this.statusHandle = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.RocketChecklistIncomplete, null);
						break;
					}
					break;
				}
			}
		}
		if (!flag && this.statusHandle != Guid.Empty)
		{
			base.GetComponent<KSelectable>().RemoveStatusItem(this.statusHandle, false);
		}
	}

	// Token: 0x04006858 RID: 26712
	private CraftModuleInterface craftModuleInterface;

	// Token: 0x04006859 RID: 26713
	private Guid statusHandle = Guid.Empty;
}
