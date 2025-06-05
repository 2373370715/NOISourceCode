using System;
using KSerialization;
using STRINGS;

// Token: 0x02000337 RID: 823
public class FossilBits : FossilExcavationWorkable, ISidescreenButtonControl
{
	// Token: 0x06000CE4 RID: 3300 RVA: 0x000AFDF1 File Offset: 0x000ADFF1
	protected override bool IsMarkedForExcavation()
	{
		return this.MarkedForDig;
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x000AFDF9 File Offset: 0x000ADFF9
	public void SetEntombStatusItemVisibility(bool visible)
	{
		this.entombComponent.SetShowStatusItemOnEntombed(visible);
	}

	// Token: 0x06000CE6 RID: 3302 RVA: 0x0017B8F4 File Offset: 0x00179AF4
	public void CreateWorkableChore()
	{
		if (this.chore == null && this.operational.IsOperational)
		{
			this.chore = new WorkChore<FossilBits>(Db.Get().ChoreTypes.ExcavateFossil, this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}
	}

	// Token: 0x06000CE7 RID: 3303 RVA: 0x000AFE07 File Offset: 0x000AE007
	public void CancelWorkChore()
	{
		if (this.chore != null)
		{
			this.chore.Cancel("FossilBits.CancelChore");
			this.chore = null;
		}
	}

	// Token: 0x06000CE8 RID: 3304 RVA: 0x0017B944 File Offset: 0x00179B44
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sculpture_kanim")
		};
		base.Subscribe(-592767678, new Action<object>(this.OnOperationalChanged));
		base.SetWorkTime(30f);
	}

	// Token: 0x06000CE9 RID: 3305 RVA: 0x000AFE28 File Offset: 0x000AE028
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		base.SetShouldShowSkillPerkStatusItem(this.IsMarkedForExcavation());
	}

	// Token: 0x06000CEA RID: 3306 RVA: 0x000AFE48 File Offset: 0x000AE048
	private void OnOperationalChanged(object state)
	{
		if ((bool)state)
		{
			if (this.MarkedForDig)
			{
				this.CreateWorkableChore();
				return;
			}
		}
		else if (this.MarkedForDig)
		{
			this.CancelWorkChore();
		}
	}

	// Token: 0x06000CEB RID: 3307 RVA: 0x0017B998 File Offset: 0x00179B98
	private void DropLoot()
	{
		PrimaryElement component = base.gameObject.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(base.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000CEC RID: 3308 RVA: 0x000AFE6F File Offset: 0x000AE06F
	protected override void OnCompleteWork(WorkerBase worker)
	{
		base.OnCompleteWork(worker);
		this.DropLoot();
		Util.KDestroyGameObject(base.gameObject);
	}

	// Token: 0x06000CED RID: 3309 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x06000CEE RID: 3310 RVA: 0x000AFE8C File Offset: 0x000AE08C
	public string SidescreenButtonText
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000CEF RID: 3311 RVA: 0x000AFEAB File Offset: 0x000AE0AB
	public string SidescreenButtonTooltip
	{
		get
		{
			if (!this.MarkedForDig)
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP;
			}
			return CODEX.STORY_TRAITS.FOSSILHUNT.UISIDESCREENS.FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP;
		}
	}

	// Token: 0x06000CF0 RID: 3312 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000CF1 RID: 3313 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x06000CF2 RID: 3314 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenButtonInteractable()
	{
		return true;
	}

	// Token: 0x06000CF3 RID: 3315 RVA: 0x0017BA50 File Offset: 0x00179C50
	public void OnSidescreenButtonPressed()
	{
		this.MarkedForDig = !this.MarkedForDig;
		base.SetShouldShowSkillPerkStatusItem(this.MarkedForDig);
		this.SetEntombStatusItemVisibility(this.MarkedForDig);
		if (this.MarkedForDig)
		{
			this.CreateWorkableChore();
		}
		else
		{
			this.CancelWorkChore();
		}
		this.UpdateStatusItem(null);
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x040009AE RID: 2478
	[Serialize]
	public bool MarkedForDig;

	// Token: 0x040009AF RID: 2479
	private Chore chore;

	// Token: 0x040009B0 RID: 2480
	[MyCmpGet]
	private EntombVulnerable entombComponent;

	// Token: 0x040009B1 RID: 2481
	[MyCmpGet]
	private Operational operational;
}
