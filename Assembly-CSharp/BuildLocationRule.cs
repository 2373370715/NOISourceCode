using System;

// Token: 0x0200124E RID: 4686
public enum BuildLocationRule
{
	// Token: 0x04004422 RID: 17442
	Anywhere,
	// Token: 0x04004423 RID: 17443
	OnFloor,
	// Token: 0x04004424 RID: 17444
	OnFloorOverSpace,
	// Token: 0x04004425 RID: 17445
	OnCeiling,
	// Token: 0x04004426 RID: 17446
	OnWall,
	// Token: 0x04004427 RID: 17447
	InCorner,
	// Token: 0x04004428 RID: 17448
	Tile,
	// Token: 0x04004429 RID: 17449
	NotInTiles,
	// Token: 0x0400442A RID: 17450
	Conduit,
	// Token: 0x0400442B RID: 17451
	LogicBridge,
	// Token: 0x0400442C RID: 17452
	WireBridge,
	// Token: 0x0400442D RID: 17453
	HighWattBridgeTile,
	// Token: 0x0400442E RID: 17454
	BuildingAttachPoint,
	// Token: 0x0400442F RID: 17455
	OnFloorOrBuildingAttachPoint,
	// Token: 0x04004430 RID: 17456
	OnFoundationRotatable,
	// Token: 0x04004431 RID: 17457
	BelowRocketCeiling,
	// Token: 0x04004432 RID: 17458
	OnRocketEnvelope,
	// Token: 0x04004433 RID: 17459
	WallFloor,
	// Token: 0x04004434 RID: 17460
	NoLiquidConduitAtOrigin
}
