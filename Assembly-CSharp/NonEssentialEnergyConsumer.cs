using System;

// Token: 0x020016B8 RID: 5816
public class NonEssentialEnergyConsumer : EnergyConsumer
{
	// Token: 0x1700078D RID: 1933
	// (get) Token: 0x060077F9 RID: 30713 RVA: 0x000F36F4 File Offset: 0x000F18F4
	// (set) Token: 0x060077FA RID: 30714 RVA: 0x000F36FC File Offset: 0x000F18FC
	public override bool IsPowered
	{
		get
		{
			return this.isPowered;
		}
		protected set
		{
			if (value == this.isPowered)
			{
				return;
			}
			this.isPowered = value;
			Action<bool> poweredStateChanged = this.PoweredStateChanged;
			if (poweredStateChanged == null)
			{
				return;
			}
			poweredStateChanged(this.isPowered);
		}
	}

	// Token: 0x04005A32 RID: 23090
	public Action<bool> PoweredStateChanged;

	// Token: 0x04005A33 RID: 23091
	private bool isPowered;
}
