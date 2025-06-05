using System;
using UnityEngine;

// Token: 0x020013A9 RID: 5033
public class GassyMooComet : Comet
{
	// Token: 0x06006722 RID: 26402 RVA: 0x000E7BB3 File Offset: 0x000E5DB3
	public void SetCustomInitialFlip(bool state)
	{
		this.initialFlipState = new bool?(state);
	}

	// Token: 0x06006723 RID: 26403 RVA: 0x002E03E8 File Offset: 0x002DE5E8
	public override void RandomizeVelocity()
	{
		bool flag = false;
		byte id = Grid.WorldIdx[Grid.PosToCell(base.gameObject.transform.position)];
		WorldContainer world = ClusterManager.Instance.GetWorld((int)id);
		if (world == null)
		{
			return;
		}
		int num = world.WorldOffset.x + world.Width / 2;
		if (Grid.PosToXY(base.gameObject.transform.position).x > num)
		{
			flag = true;
		}
		if (this.initialFlipState != null)
		{
			flag = this.initialFlipState.Value;
		}
		float f = (flag ? -75f : -105f) * 3.1415927f / 180f;
		float num2 = UnityEngine.Random.Range(this.spawnVelocity.x, this.spawnVelocity.y);
		this.velocity = new Vector2(-Mathf.Cos(f) * num2, Mathf.Sin(f) * num2);
		base.GetComponent<KBatchedAnimController>().FlipX = flag;
	}

	// Token: 0x06006724 RID: 26404 RVA: 0x002E04DC File Offset: 0x002DE6DC
	protected override void SpawnCraterPrefabs()
	{
		KBatchedAnimController animController = base.GetComponent<KBatchedAnimController>();
		animController.Play("landing", KAnim.PlayMode.Once, 1f, 0f);
		animController.onAnimComplete += delegate(HashedString obj)
		{
			if (this.craterPrefabs != null && this.craterPrefabs.Length != 0)
			{
				byte world = Grid.WorldIdx[Grid.PosToCell(this.gameObject.transform.position)];
				float x = 0f;
				int num = Grid.PosToCell(this.transform.GetPosition());
				int num2 = Grid.OffsetCell(num, 0, 1);
				int num3 = Grid.OffsetCell(num, 0, -1);
				if (Grid.IsValidCellInWorld(num2, (int)world))
				{
					num = num2;
				}
				else
				{
					num = num3;
				}
				if (Grid.Solid[num])
				{
					bool flipX = animController.FlipX;
					int num4 = Grid.OffsetCell(num, -1, 0);
					int num5 = Grid.OffsetCell(num, 2, 0);
					if (!flipX && Grid.IsValidCell(num4) && !Grid.Solid[num4])
					{
						num = num4;
					}
					else if (flipX && Grid.IsValidCell(num5) && !Grid.Solid[num5])
					{
						num = num5;
					}
				}
				else
				{
					x = this.gameObject.transform.position.x - Mathf.Floor(this.gameObject.transform.position.x);
				}
				Vector3 position = Grid.CellToPos(num) + new Vector3(x, 0f, Grid.GetLayerZ(Grid.SceneLayer.Creatures));
				GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(this.craterPrefabs[UnityEngine.Random.Range(0, this.craterPrefabs.Length)]), position);
				Vector3 vector = gameObject.transform.position + this.mooSpawnImpactOffset;
				if (!Grid.Solid[Grid.PosToCell(vector)])
				{
					gameObject.transform.position = vector;
				}
				gameObject.GetComponent<KBatchedAnimController>().FlipX = animController.FlipX;
				gameObject.SetActive(true);
			}
			Util.KDestroyGameObject(this.gameObject);
		};
	}

	// Token: 0x04004DD9 RID: 19929
	public const float MOO_ANGLE = 15f;

	// Token: 0x04004DDA RID: 19930
	public Vector3 mooSpawnImpactOffset = new Vector3(-0.5f, 0f, 0f);

	// Token: 0x04004DDB RID: 19931
	private bool? initialFlipState;
}
