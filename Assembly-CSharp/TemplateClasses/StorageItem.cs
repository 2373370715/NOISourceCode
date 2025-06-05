using System;

namespace TemplateClasses
{
	// Token: 0x02002154 RID: 8532
	[Serializable]
	public class StorageItem
	{
		// Token: 0x0600B5E5 RID: 46565 RVA: 0x0011A884 File Offset: 0x00118A84
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

		// Token: 0x0600B5E6 RID: 46566 RVA: 0x004556C0 File Offset: 0x004538C0
		public StorageItem(string _id, float _units, float _temp, SimHashes _element, string _disease, int _disease_count, bool _isOre)
		{
			this.rottable = new Rottable();
			this.id = _id;
			this.element = _element;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.isOre = _isOre;
			this.temperature = _temp;
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x0600B5E7 RID: 46567 RVA: 0x0011A897 File Offset: 0x00118A97
		// (set) Token: 0x0600B5E8 RID: 46568 RVA: 0x0011A89F File Offset: 0x00118A9F
		public string id { get; set; }

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x0600B5E9 RID: 46569 RVA: 0x0011A8A8 File Offset: 0x00118AA8
		// (set) Token: 0x0600B5EA RID: 46570 RVA: 0x0011A8B0 File Offset: 0x00118AB0
		public SimHashes element { get; set; }

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x0600B5EB RID: 46571 RVA: 0x0011A8B9 File Offset: 0x00118AB9
		// (set) Token: 0x0600B5EC RID: 46572 RVA: 0x0011A8C1 File Offset: 0x00118AC1
		public float units { get; set; }

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600B5ED RID: 46573 RVA: 0x0011A8CA File Offset: 0x00118ACA
		// (set) Token: 0x0600B5EE RID: 46574 RVA: 0x0011A8D2 File Offset: 0x00118AD2
		public bool isOre { get; set; }

		// Token: 0x17000BD7 RID: 3031
		// (get) Token: 0x0600B5EF RID: 46575 RVA: 0x0011A8DB File Offset: 0x00118ADB
		// (set) Token: 0x0600B5F0 RID: 46576 RVA: 0x0011A8E3 File Offset: 0x00118AE3
		public float temperature { get; set; }

		// Token: 0x17000BD8 RID: 3032
		// (get) Token: 0x0600B5F1 RID: 46577 RVA: 0x0011A8EC File Offset: 0x00118AEC
		// (set) Token: 0x0600B5F2 RID: 46578 RVA: 0x0011A8F4 File Offset: 0x00118AF4
		public string diseaseName { get; set; }

		// Token: 0x17000BD9 RID: 3033
		// (get) Token: 0x0600B5F3 RID: 46579 RVA: 0x0011A8FD File Offset: 0x00118AFD
		// (set) Token: 0x0600B5F4 RID: 46580 RVA: 0x0011A905 File Offset: 0x00118B05
		public int diseaseCount { get; set; }

		// Token: 0x17000BDA RID: 3034
		// (get) Token: 0x0600B5F5 RID: 46581 RVA: 0x0011A90E File Offset: 0x00118B0E
		// (set) Token: 0x0600B5F6 RID: 46582 RVA: 0x0011A916 File Offset: 0x00118B16
		public Rottable rottable { get; set; }

		// Token: 0x0600B5F7 RID: 46583 RVA: 0x00455714 File Offset: 0x00453914
		public StorageItem Clone()
		{
			return new StorageItem(this.id, this.units, this.temperature, this.element, this.diseaseName, this.diseaseCount, this.isOre)
			{
				rottable = 
				{
					rotAmount = this.rottable.rotAmount
				}
			};
		}
	}
}
