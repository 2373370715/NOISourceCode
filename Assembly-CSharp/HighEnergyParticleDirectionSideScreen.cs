using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001FDB RID: 8155
public class HighEnergyParticleDirectionSideScreen : SideScreenContent
{
	// Token: 0x0600AC50 RID: 44112 RVA: 0x00114775 File Offset: 0x00112975
	public override string GetTitle()
	{
		return UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.TITLE;
	}

	// Token: 0x0600AC51 RID: 44113 RVA: 0x0041D3C4 File Offset: 0x0041B5C4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		for (int i = 0; i < this.Buttons.Count; i++)
		{
			KButton button = this.Buttons[i];
			button.onClick += delegate()
			{
				int num = this.Buttons.IndexOf(button);
				if (this.activeButton != null)
				{
					this.activeButton.isInteractable = true;
				}
				button.isInteractable = false;
				this.activeButton = button;
				if (this.target != null)
				{
					this.target.Direction = EightDirectionUtil.AngleToDirection(num * 45);
					Game.Instance.ForceOverlayUpdate(true);
					this.Refresh();
				}
			};
		}
	}

	// Token: 0x0600AC52 RID: 44114 RVA: 0x00114781 File Offset: 0x00112981
	public override int GetSideScreenSortOrder()
	{
		return 10;
	}

	// Token: 0x0600AC53 RID: 44115 RVA: 0x0041D424 File Offset: 0x0041B624
	public override bool IsValidForTarget(GameObject target)
	{
		HighEnergyParticleRedirector component = target.GetComponent<HighEnergyParticleRedirector>();
		bool flag = component != null;
		if (flag)
		{
			flag = (flag && component.directionControllable);
		}
		bool flag2 = target.GetComponent<HighEnergyParticleSpawner>() != null || target.GetComponent<ManualHighEnergyParticleSpawner>() != null || target.GetComponent<DevHEPSpawner>() != null;
		return (flag || flag2) && target.GetComponent<IHighEnergyParticleDirection>() != null;
	}

	// Token: 0x0600AC54 RID: 44116 RVA: 0x00114785 File Offset: 0x00112985
	public override void SetTarget(GameObject new_target)
	{
		if (new_target == null)
		{
			global::Debug.LogError("Invalid gameObject received");
			return;
		}
		this.target = new_target.GetComponent<IHighEnergyParticleDirection>();
		if (this.target == null)
		{
			global::Debug.LogError("The gameObject received does not contain IHighEnergyParticleDirection component");
			return;
		}
		this.Refresh();
	}

	// Token: 0x0600AC55 RID: 44117 RVA: 0x0041D48C File Offset: 0x0041B68C
	private void Refresh()
	{
		int directionIndex = EightDirectionUtil.GetDirectionIndex(this.target.Direction);
		if (directionIndex >= 0 && directionIndex < this.Buttons.Count)
		{
			this.Buttons[directionIndex].SignalClick(KKeyCode.Mouse0);
		}
		else
		{
			if (this.activeButton)
			{
				this.activeButton.isInteractable = true;
			}
			this.activeButton = null;
		}
		this.directionLabel.SetText(string.Format(UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.SELECTED_DIRECTION, this.directionStrings[directionIndex]));
	}

	// Token: 0x040087AC RID: 34732
	private IHighEnergyParticleDirection target;

	// Token: 0x040087AD RID: 34733
	public List<KButton> Buttons;

	// Token: 0x040087AE RID: 34734
	private KButton activeButton;

	// Token: 0x040087AF RID: 34735
	public LocText directionLabel;

	// Token: 0x040087B0 RID: 34736
	private string[] directionStrings = new string[]
	{
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_N,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_W,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SW,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_S,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_SE,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_E,
		UI.UISIDESCREENS.HIGHENERGYPARTICLEDIRECTIONSIDESCREEN.DIRECTION_NE
	};
}
