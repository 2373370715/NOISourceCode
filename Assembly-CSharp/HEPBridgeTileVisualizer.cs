using System;

// Token: 0x02000E1A RID: 3610
public class HEPBridgeTileVisualizer : KMonoBehaviour, IHighEnergyParticleDirection
{
	// Token: 0x06004683 RID: 18051 RVA: 0x000D21A0 File Offset: 0x000D03A0
	protected override void OnSpawn()
	{
		base.Subscribe<HEPBridgeTileVisualizer>(-1643076535, HEPBridgeTileVisualizer.OnRotateDelegate);
		this.OnRotate();
	}

	// Token: 0x06004684 RID: 18052 RVA: 0x000D21B9 File Offset: 0x000D03B9
	public void OnRotate()
	{
		Game.Instance.ForceOverlayUpdate(true);
	}

	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06004685 RID: 18053 RVA: 0x0025DA20 File Offset: 0x0025BC20
	// (set) Token: 0x06004686 RID: 18054 RVA: 0x000AA038 File Offset: 0x000A8238
	public EightDirection Direction
	{
		get
		{
			EightDirection result = EightDirection.Right;
			Rotatable component = base.GetComponent<Rotatable>();
			if (component != null)
			{
				switch (component.Orientation)
				{
				case Orientation.Neutral:
					result = EightDirection.Left;
					break;
				case Orientation.R90:
					result = EightDirection.Up;
					break;
				case Orientation.R180:
					result = EightDirection.Right;
					break;
				case Orientation.R270:
					result = EightDirection.Down;
					break;
				}
			}
			return result;
		}
		set
		{
		}
	}

	// Token: 0x04003137 RID: 12599
	private static readonly EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer> OnRotateDelegate = new EventSystem.IntraObjectHandler<HEPBridgeTileVisualizer>(delegate(HEPBridgeTileVisualizer component, object data)
	{
		component.OnRotate();
	});
}
