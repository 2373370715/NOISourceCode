using System;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
public class SplashTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x0600376F RID: 14191 RVA: 0x000C86BA File Offset: 0x000C68BA
	public SplashTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.lastSplashTime = Time.time;
	}

	// Token: 0x06003770 RID: 14192 RVA: 0x00224554 File Offset: 0x00222754
	private void RefreshSplashes(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (navigator == null)
		{
			return;
		}
		if (transition.end == NavType.Tube)
		{
			return;
		}
		Vector3 position = navigator.transform.GetPosition();
		if (this.lastSplashTime + 1f < Time.time && Grid.Element[Grid.PosToCell(position)].IsLiquid)
		{
			this.lastSplashTime = Time.time;
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("splash_step_kanim", position + new Vector3(0f, 0.75f, -0.1f), null, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx1", KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
	}

	// Token: 0x06003771 RID: 14193 RVA: 0x000C86CE File Offset: 0x000C68CE
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x06003772 RID: 14194 RVA: 0x000C86E0 File Offset: 0x000C68E0
	public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.UpdateTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x000C86F2 File Offset: 0x000C68F2
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x04002629 RID: 9769
	private float lastSplashTime;

	// Token: 0x0400262A RID: 9770
	private const float SPLASH_INTERVAL = 1f;
}
