using System;

// Token: 0x02000B91 RID: 2961
public class NavTeleportTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06003786 RID: 14214 RVA: 0x000C8704 File Offset: 0x000C6904
	public NavTeleportTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06003787 RID: 14215 RVA: 0x00224B84 File Offset: 0x00222D84
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		if (transition.start == NavType.Teleport)
		{
			int num = Grid.PosToCell(navigator);
			int num2;
			int num3;
			Grid.CellToXY(num, out num2, out num3);
			int num4 = navigator.NavGrid.teleportTransitions[num];
			int num5;
			int num6;
			Grid.CellToXY(navigator.NavGrid.teleportTransitions[num], out num5, out num6);
			transition.x = num5 - num2;
			transition.y = num6 - num3;
		}
	}
}
