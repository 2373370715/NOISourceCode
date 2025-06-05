using System;
using UnityEngine;

// Token: 0x02001F97 RID: 8087
public class AutoPlumberSideScreen : SideScreenContent
{
	// Token: 0x0600AAD3 RID: 43731 RVA: 0x00416FE0 File Offset: 0x004151E0
	protected override void OnSpawn()
	{
		this.activateButton.onClick += delegate()
		{
			DevAutoPlumber.AutoPlumbBuilding(this.building);
		};
		this.powerButton.onClick += delegate()
		{
			DevAutoPlumber.DoElectricalPlumbing(this.building);
		};
		this.pipesButton.onClick += delegate()
		{
			DevAutoPlumber.DoLiquidAndGasPlumbing(this.building);
		};
		this.solidsButton.onClick += delegate()
		{
			DevAutoPlumber.SetupSolidOreDelivery(this.building);
		};
		this.minionButton.onClick += delegate()
		{
			this.SpawnMinion();
		};
	}

	// Token: 0x0600AAD4 RID: 43732 RVA: 0x00417060 File Offset: 0x00415260
	private void SpawnMinion()
	{
		MinionStartingStats minionStartingStats = new MinionStartingStats(false, null, null, true);
		GameObject prefab = Assets.GetPrefab(BaseMinionConfig.GetMinionIDForModel(minionStartingStats.personality.model));
		GameObject gameObject = Util.KInstantiate(prefab, null, null);
		gameObject.name = prefab.name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
		Vector3 position = Grid.CellToPos(Grid.PosToCell(this.building), CellAlignment.Bottom, Grid.SceneLayer.Move);
		gameObject.transform.SetLocalPosition(position);
		gameObject.SetActive(true);
		minionStartingStats.Apply(gameObject);
	}

	// Token: 0x0600AAD5 RID: 43733 RVA: 0x0011378A File Offset: 0x0011198A
	public override int GetSideScreenSortOrder()
	{
		return -150;
	}

	// Token: 0x0600AAD6 RID: 43734 RVA: 0x00113791 File Offset: 0x00111991
	public override bool IsValidForTarget(GameObject target)
	{
		return DebugHandler.InstantBuildMode && target.GetComponent<Building>() != null;
	}

	// Token: 0x0600AAD7 RID: 43735 RVA: 0x001137A8 File Offset: 0x001119A8
	public override void SetTarget(GameObject target)
	{
		this.building = target.GetComponent<Building>();
	}

	// Token: 0x0600AAD8 RID: 43736 RVA: 0x000AA038 File Offset: 0x000A8238
	public override void ClearTarget()
	{
	}

	// Token: 0x0400867A RID: 34426
	public KButton activateButton;

	// Token: 0x0400867B RID: 34427
	public KButton powerButton;

	// Token: 0x0400867C RID: 34428
	public KButton pipesButton;

	// Token: 0x0400867D RID: 34429
	public KButton solidsButton;

	// Token: 0x0400867E RID: 34430
	public KButton minionButton;

	// Token: 0x0400867F RID: 34431
	private Building building;
}
