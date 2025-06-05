using System;

namespace TemplateClasses
{
	// Token: 0x02002153 RID: 8531
	[Serializable]
	public class Cell
	{
		// Token: 0x0600B5D3 RID: 46547 RVA: 0x000AA024 File Offset: 0x000A8224
		public Cell()
		{
		}

		// Token: 0x0600B5D4 RID: 46548 RVA: 0x00455670 File Offset: 0x00453870
		public Cell(int loc_x, int loc_y, SimHashes _element, float _temperature, float _mass, string _diseaseName, int _diseaseCount, bool _preventFoWReveal = false)
		{
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.element = _element;
			this.temperature = _temperature;
			this.mass = _mass;
			this.diseaseName = _diseaseName;
			this.diseaseCount = _diseaseCount;
			this.preventFoWReveal = _preventFoWReveal;
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600B5D5 RID: 46549 RVA: 0x0011A7FC File Offset: 0x001189FC
		// (set) Token: 0x0600B5D6 RID: 46550 RVA: 0x0011A804 File Offset: 0x00118A04
		public SimHashes element { get; set; }

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x0600B5D7 RID: 46551 RVA: 0x0011A80D File Offset: 0x00118A0D
		// (set) Token: 0x0600B5D8 RID: 46552 RVA: 0x0011A815 File Offset: 0x00118A15
		public float mass { get; set; }

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x0600B5D9 RID: 46553 RVA: 0x0011A81E File Offset: 0x00118A1E
		// (set) Token: 0x0600B5DA RID: 46554 RVA: 0x0011A826 File Offset: 0x00118A26
		public float temperature { get; set; }

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x0600B5DB RID: 46555 RVA: 0x0011A82F File Offset: 0x00118A2F
		// (set) Token: 0x0600B5DC RID: 46556 RVA: 0x0011A837 File Offset: 0x00118A37
		public string diseaseName { get; set; }

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x0600B5DD RID: 46557 RVA: 0x0011A840 File Offset: 0x00118A40
		// (set) Token: 0x0600B5DE RID: 46558 RVA: 0x0011A848 File Offset: 0x00118A48
		public int diseaseCount { get; set; }

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x0600B5DF RID: 46559 RVA: 0x0011A851 File Offset: 0x00118A51
		// (set) Token: 0x0600B5E0 RID: 46560 RVA: 0x0011A859 File Offset: 0x00118A59
		public int location_x { get; set; }

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x0600B5E1 RID: 46561 RVA: 0x0011A862 File Offset: 0x00118A62
		// (set) Token: 0x0600B5E2 RID: 46562 RVA: 0x0011A86A File Offset: 0x00118A6A
		public int location_y { get; set; }

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x0600B5E3 RID: 46563 RVA: 0x0011A873 File Offset: 0x00118A73
		// (set) Token: 0x0600B5E4 RID: 46564 RVA: 0x0011A87B File Offset: 0x00118A7B
		public bool preventFoWReveal { get; set; }
	}
}
