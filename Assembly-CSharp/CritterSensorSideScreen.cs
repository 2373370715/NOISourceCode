using System;
using UnityEngine;

// Token: 0x02001FC1 RID: 8129
public class CritterSensorSideScreen : SideScreenContent
{
	// Token: 0x0600ABD5 RID: 43989 RVA: 0x001141E8 File Offset: 0x001123E8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.countCrittersToggle.onClick += this.ToggleCritters;
		this.countEggsToggle.onClick += this.ToggleEggs;
	}

	// Token: 0x0600ABD6 RID: 43990 RVA: 0x0011421E File Offset: 0x0011241E
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicCritterCountSensor>() != null;
	}

	// Token: 0x0600ABD7 RID: 43991 RVA: 0x0041B33C File Offset: 0x0041953C
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetSensor = target.GetComponent<LogicCritterCountSensor>();
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x0600ABD8 RID: 43992 RVA: 0x0011422C File Offset: 0x0011242C
	private void ToggleCritters()
	{
		this.targetSensor.countCritters = !this.targetSensor.countCritters;
		this.crittersCheckmark.enabled = this.targetSensor.countCritters;
	}

	// Token: 0x0600ABD9 RID: 43993 RVA: 0x0011425D File Offset: 0x0011245D
	private void ToggleEggs()
	{
		this.targetSensor.countEggs = !this.targetSensor.countEggs;
		this.eggsCheckmark.enabled = this.targetSensor.countEggs;
	}

	// Token: 0x04008751 RID: 34641
	public LogicCritterCountSensor targetSensor;

	// Token: 0x04008752 RID: 34642
	public KToggle countCrittersToggle;

	// Token: 0x04008753 RID: 34643
	public KToggle countEggsToggle;

	// Token: 0x04008754 RID: 34644
	public KImage crittersCheckmark;

	// Token: 0x04008755 RID: 34645
	public KImage eggsCheckmark;
}
