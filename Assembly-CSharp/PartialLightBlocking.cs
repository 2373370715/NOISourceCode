using System;
using KSerialization;

// Token: 0x02000F46 RID: 3910
[SerializationConfig(MemberSerialization.OptIn)]
public class PartialLightBlocking : KMonoBehaviour
{
	// Token: 0x06004E69 RID: 20073 RVA: 0x000D7526 File Offset: 0x000D5726
	protected override void OnSpawn()
	{
		this.SetLightBlocking();
		base.OnSpawn();
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x000D7534 File Offset: 0x000D5734
	protected override void OnCleanUp()
	{
		this.ClearLightBlocking();
		base.OnCleanUp();
	}

	// Token: 0x06004E6B RID: 20075 RVA: 0x00276474 File Offset: 0x00274674
	public void SetLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.SetCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x002764A8 File Offset: 0x002746A8
	public void ClearLightBlocking()
	{
		int[] placementCells = base.GetComponent<Building>().PlacementCells;
		for (int i = 0; i < placementCells.Length; i++)
		{
			SimMessages.ClearCellProperties(placementCells[i], 48);
		}
	}

	// Token: 0x04003706 RID: 14086
	private const byte PartialLightBlockingProperties = 48;
}
