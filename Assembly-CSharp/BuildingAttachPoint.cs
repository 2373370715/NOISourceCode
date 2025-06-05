using System;
using UnityEngine;

// Token: 0x02000CA5 RID: 3237
[AddComponentMenu("KMonoBehaviour/scripts/BuildingAttachPoint")]
public class BuildingAttachPoint : KMonoBehaviour
{
	// Token: 0x06003D90 RID: 15760 RVA: 0x000CC475 File Offset: 0x000CA675
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.BuildingAttachPoints.Add(this);
		this.TryAttachEmptyHardpoints();
	}

	// Token: 0x06003D91 RID: 15761 RVA: 0x000C474E File Offset: 0x000C294E
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x0023FBEC File Offset: 0x0023DDEC
	private void TryAttachEmptyHardpoints()
	{
		for (int i = 0; i < this.points.Length; i++)
		{
			if (!(this.points[i].attachedBuilding != null))
			{
				bool flag = false;
				int num = 0;
				while (num < Components.AttachableBuildings.Count && !flag)
				{
					if (Components.AttachableBuildings[num].attachableToTag == this.points[i].attachableType && Grid.OffsetCell(Grid.PosToCell(base.gameObject), this.points[i].position) == Grid.PosToCell(Components.AttachableBuildings[num]))
					{
						this.points[i].attachedBuilding = Components.AttachableBuildings[num];
						flag = true;
					}
					num++;
				}
			}
		}
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x0023FCC4 File Offset: 0x0023DEC4
	public bool AcceptsAttachment(Tag type, int cell)
	{
		int cell2 = Grid.PosToCell(base.gameObject);
		for (int i = 0; i < this.points.Length; i++)
		{
			if (Grid.OffsetCell(cell2, this.points[i].position) == cell && this.points[i].attachableType == type)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003D94 RID: 15764 RVA: 0x000CC48E File Offset: 0x000CA68E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.BuildingAttachPoints.Remove(this);
	}

	// Token: 0x04002A7D RID: 10877
	public BuildingAttachPoint.HardPoint[] points = new BuildingAttachPoint.HardPoint[0];

	// Token: 0x02000CA6 RID: 3238
	[Serializable]
	public struct HardPoint
	{
		// Token: 0x06003D96 RID: 15766 RVA: 0x000CC4B5 File Offset: 0x000CA6B5
		public HardPoint(CellOffset position, Tag attachableType, AttachableBuilding attachedBuilding)
		{
			this.position = position;
			this.attachableType = attachableType;
			this.attachedBuilding = attachedBuilding;
		}

		// Token: 0x04002A7E RID: 10878
		public CellOffset position;

		// Token: 0x04002A7F RID: 10879
		public Tag attachableType;

		// Token: 0x04002A80 RID: 10880
		public AttachableBuilding attachedBuilding;
	}
}
