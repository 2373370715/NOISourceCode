using System;
using UnityEngine;

// Token: 0x02001092 RID: 4242
public static class CameraSaveData
{
	// Token: 0x06005635 RID: 22069 RVA: 0x000DCA6A File Offset: 0x000DAC6A
	public static void Load(FastReader reader)
	{
		CameraSaveData.position = reader.ReadVector3();
		CameraSaveData.localScale = reader.ReadVector3();
		CameraSaveData.rotation = reader.ReadQuaternion();
		CameraSaveData.orthographicsSize = reader.ReadSingle();
		CameraSaveData.valid = true;
	}

	// Token: 0x04003D00 RID: 15616
	public static bool valid;

	// Token: 0x04003D01 RID: 15617
	public static Vector3 position;

	// Token: 0x04003D02 RID: 15618
	public static Vector3 localScale;

	// Token: 0x04003D03 RID: 15619
	public static Quaternion rotation;

	// Token: 0x04003D04 RID: 15620
	public static float orthographicsSize;
}
