using System;
using UnityEngine;

// Token: 0x02000992 RID: 2450
public struct SoundCuller
{
	// Token: 0x06002BAB RID: 11179 RVA: 0x001ED53C File Offset: 0x001EB73C
	public static bool IsAudibleWorld(Vector2 pos)
	{
		bool result = false;
		int num = Grid.PosToCell(pos);
		if (Grid.IsValidCell(num) && (int)Grid.WorldIdx[num] == ClusterManager.Instance.activeWorldId)
		{
			result = true;
		}
		return result;
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x000C0E56 File Offset: 0x000BF056
	public bool IsAudible(Vector2 pos)
	{
		return SoundCuller.IsAudibleWorld(pos) && this.min.LessEqual(pos) && pos.LessEqual(this.max);
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x001ED570 File Offset: 0x001EB770
	public bool IsAudibleNoCameraScaling(Vector2 pos, float falloff_distance_sq)
	{
		return (pos.x - this.cameraPos.x) * (pos.x - this.cameraPos.x) + (pos.y - this.cameraPos.y) * (pos.y - this.cameraPos.y) < falloff_distance_sq;
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x000C0E7C File Offset: 0x000BF07C
	public bool IsAudible(Vector2 pos, float falloff_distance_sq)
	{
		if (!SoundCuller.IsAudibleWorld(pos))
		{
			return false;
		}
		pos = this.GetVerticallyScaledPosition(pos, false);
		return this.IsAudibleNoCameraScaling(pos, falloff_distance_sq);
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x000C0EA4 File Offset: 0x000BF0A4
	public bool IsAudible(Vector2 pos, HashedString sound_path)
	{
		return sound_path.IsValid && this.IsAudible(pos, KFMOD.GetSoundEventDescription(sound_path).falloffDistanceSq);
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x001ED5CC File Offset: 0x001EB7CC
	public Vector3 GetVerticallyScaledPosition(Vector3 pos, bool objectIsSelectedAndVisible = false)
	{
		float num = 1f;
		float num2;
		if (pos.y > this.max.y)
		{
			num2 = Mathf.Abs(pos.y - this.max.y);
		}
		else if (pos.y < this.min.y)
		{
			num2 = Mathf.Abs(pos.y - this.min.y);
			num = -1f;
		}
		else
		{
			num2 = 0f;
		}
		float extraYRange = TuningData<SoundCuller.Tuning>.Get().extraYRange;
		num2 = ((num2 < extraYRange) ? num2 : extraYRange);
		float num3 = num2 * num2 / (4f * this.zoomScaler);
		num3 *= num;
		Vector3 result = new Vector3(pos.x, pos.y + num3, 0f);
		if (objectIsSelectedAndVisible)
		{
			result.z = pos.z;
		}
		return result;
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x001ED6A0 File Offset: 0x001EB8A0
	public static SoundCuller CreateCuller()
	{
		SoundCuller result = default(SoundCuller);
		Camera main = Camera.main;
		Vector3 vector = main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 vector2 = main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		result.min = new Vector3(vector2.x, vector2.y, 0f);
		result.max = new Vector3(vector.x, vector.y, 0f);
		result.cameraPos = main.transform.GetPosition();
		Audio audio = Audio.Get();
		float num = CameraController.Instance.OrthographicSize / (audio.listenerReferenceZ - audio.listenerMinZ);
		if (num <= 0f)
		{
			num = 2f;
		}
		else
		{
			num = 1f;
		}
		result.zoomScaler = num;
		return result;
	}

	// Token: 0x04001DE9 RID: 7657
	private Vector2 min;

	// Token: 0x04001DEA RID: 7658
	private Vector2 max;

	// Token: 0x04001DEB RID: 7659
	private Vector2 cameraPos;

	// Token: 0x04001DEC RID: 7660
	private float zoomScaler;

	// Token: 0x02000993 RID: 2451
	public class Tuning : TuningData<SoundCuller.Tuning>
	{
		// Token: 0x04001DED RID: 7661
		public float extraYRange;
	}
}
