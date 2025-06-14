﻿using System;

public enum BuildLocationRule
{
	Anywhere,
	OnFloor,
	OnFloorOverSpace,
	OnCeiling,
	OnWall,
	InCorner,
	Tile,
	NotInTiles,
	Conduit,
	LogicBridge,
	WireBridge,
	HighWattBridgeTile,
	BuildingAttachPoint,
	OnFloorOrBuildingAttachPoint,
	OnFoundationRotatable,
	BelowRocketCeiling,
	OnRocketEnvelope,
	WallFloor,
	NoLiquidConduitAtOrigin
}
