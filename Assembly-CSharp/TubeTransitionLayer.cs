using System;
using UnityEngine;

// Token: 0x02000B8E RID: 2958
public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x0600377D RID: 14205 RVA: 0x000C8733 File Offset: 0x000C6933
	public TubeTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
		if (this.tube_traveller != null && navigator.CurrentNavType == NavType.Tube && !this.tube_traveller.inTube)
		{
			this.tube_traveller.OnTubeTransition(true);
		}
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x002248F4 File Offset: 0x00222AF4
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.tube_traveller.OnPathAdvanced(null);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube)
		{
			int cell = Grid.PosToCell(navigator);
			this.entrance = this.GetEntrance(cell);
			return;
		}
		this.entrance = null;
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x00224944 File Offset: 0x00222B44
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		if (transition.start != NavType.Tube && transition.end == NavType.Tube && this.entrance)
		{
			this.entrance.ConsumeCharge(navigator.gameObject);
			this.entrance = null;
		}
		this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x002249A4 File Offset: 0x00222BA4
	private TravelTubeEntrance GetEntrance(int cell)
	{
		if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
		{
			return null;
		}
		GameObject gameObject = Grid.Objects[cell, 1];
		if (gameObject != null)
		{
			TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
			if (component != null && component.isSpawned)
			{
				return component;
			}
		}
		return null;
	}

	// Token: 0x0400262C RID: 9772
	private TubeTraveller.Instance tube_traveller;

	// Token: 0x0400262D RID: 9773
	private TravelTubeEntrance entrance;
}
