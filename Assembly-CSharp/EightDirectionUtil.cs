using System;
using KSerialization;
using UnityEngine;

// Token: 0x020012B3 RID: 4787
[SerializationConfig(MemberSerialization.OptIn)]
public class EightDirectionUtil
{
	// Token: 0x060061C7 RID: 25031 RVA: 0x000BC493 File Offset: 0x000BA693
	public static int GetDirectionIndex(EightDirection direction)
	{
		return (int)direction;
	}

	// Token: 0x060061C8 RID: 25032 RVA: 0x000E42FD File Offset: 0x000E24FD
	public static EightDirection AngleToDirection(int angle)
	{
		return (EightDirection)Mathf.Floor((float)angle / 45f);
	}

	// Token: 0x060061C9 RID: 25033 RVA: 0x000E430D File Offset: 0x000E250D
	public static Vector3 GetNormal(EightDirection direction)
	{
		return EightDirectionUtil.normals[EightDirectionUtil.GetDirectionIndex(direction)];
	}

	// Token: 0x060061CA RID: 25034 RVA: 0x000E431F File Offset: 0x000E251F
	public static float GetAngle(EightDirection direction)
	{
		return (float)(45 * EightDirectionUtil.GetDirectionIndex(direction));
	}

	// Token: 0x040045EA RID: 17898
	public static readonly Vector3[] normals = new Vector3[]
	{
		Vector3.up,
		(Vector3.up + Vector3.left).normalized,
		Vector3.left,
		(Vector3.down + Vector3.left).normalized,
		Vector3.down,
		(Vector3.down + Vector3.right).normalized,
		Vector3.right,
		(Vector3.up + Vector3.right).normalized
	};
}
