using System;
using UnityEngine;

// Token: 0x02000987 RID: 2439
[Serializable]
public class AudioSheet
{
	// Token: 0x04001DAC RID: 7596
	public TextAsset asset;

	// Token: 0x04001DAD RID: 7597
	public string defaultType;

	// Token: 0x04001DAE RID: 7598
	public AudioSheet.SoundInfo[] soundInfos;

	// Token: 0x02000988 RID: 2440
	public class SoundInfo : Resource
	{
		// Token: 0x04001DAF RID: 7599
		public string File;

		// Token: 0x04001DB0 RID: 7600
		public string Anim;

		// Token: 0x04001DB1 RID: 7601
		public string Type;

		// Token: 0x04001DB2 RID: 7602
		public string RequiredDlcId;

		// Token: 0x04001DB3 RID: 7603
		public float MinInterval;

		// Token: 0x04001DB4 RID: 7604
		public string Name0;

		// Token: 0x04001DB5 RID: 7605
		public int Frame0;

		// Token: 0x04001DB6 RID: 7606
		public string Name1;

		// Token: 0x04001DB7 RID: 7607
		public int Frame1;

		// Token: 0x04001DB8 RID: 7608
		public string Name2;

		// Token: 0x04001DB9 RID: 7609
		public int Frame2;

		// Token: 0x04001DBA RID: 7610
		public string Name3;

		// Token: 0x04001DBB RID: 7611
		public int Frame3;

		// Token: 0x04001DBC RID: 7612
		public string Name4;

		// Token: 0x04001DBD RID: 7613
		public int Frame4;

		// Token: 0x04001DBE RID: 7614
		public string Name5;

		// Token: 0x04001DBF RID: 7615
		public int Frame5;

		// Token: 0x04001DC0 RID: 7616
		public string Name6;

		// Token: 0x04001DC1 RID: 7617
		public int Frame6;

		// Token: 0x04001DC2 RID: 7618
		public string Name7;

		// Token: 0x04001DC3 RID: 7619
		public int Frame7;

		// Token: 0x04001DC4 RID: 7620
		public string Name8;

		// Token: 0x04001DC5 RID: 7621
		public int Frame8;

		// Token: 0x04001DC6 RID: 7622
		public string Name9;

		// Token: 0x04001DC7 RID: 7623
		public int Frame9;

		// Token: 0x04001DC8 RID: 7624
		public string Name10;

		// Token: 0x04001DC9 RID: 7625
		public int Frame10;

		// Token: 0x04001DCA RID: 7626
		public string Name11;

		// Token: 0x04001DCB RID: 7627
		public int Frame11;
	}
}
