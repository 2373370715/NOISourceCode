using System;

// Token: 0x02000656 RID: 1622
public class CreatureBrain : Brain
{
	// Token: 0x06001CEA RID: 7402 RVA: 0x001B94EC File Offset: 0x001B76EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Navigator component = base.GetComponent<Navigator>();
		if (component != null)
		{
			component.SetAbilities(new CreaturePathFinderAbilities(component));
		}
	}

	// Token: 0x0400124D RID: 4685
	public string symbolPrefix;

	// Token: 0x0400124E RID: 4686
	public Tag species;
}
