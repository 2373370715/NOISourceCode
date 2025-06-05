using System;
using System.Collections.Generic;

namespace TemplateClasses
{
	// Token: 0x02002150 RID: 8528
	[Serializable]
	public class Prefab
	{
		// Token: 0x0600B5A9 RID: 46505 RVA: 0x0011A684 File Offset: 0x00118884
		public Prefab()
		{
			this.type = Prefab.Type.Other;
		}

		// Token: 0x0600B5AA RID: 46506 RVA: 0x004554CC File Offset: 0x004536CC
		public Prefab(string _id, Prefab.Type _type, int loc_x, int loc_y, SimHashes _element, float _temperature = -1f, float _units = 1f, string _disease = null, int _disease_count = 0, Orientation _rotation = Orientation.Neutral, Prefab.template_amount_value[] _amount_values = null, Prefab.template_amount_value[] _other_values = null, int _connections = 0, string facadeIdId = null)
		{
			this.id = _id;
			this.type = _type;
			this.location_x = loc_x;
			this.location_y = loc_y;
			this.connections = _connections;
			this.element = _element;
			this.temperature = _temperature;
			this.units = _units;
			this.diseaseName = _disease;
			this.diseaseCount = _disease_count;
			this.facadeId = facadeIdId;
			this.rotationOrientation = _rotation;
			if (_amount_values != null && _amount_values.Length != 0)
			{
				this.amounts = _amount_values;
			}
			if (_other_values != null && _other_values.Length != 0)
			{
				this.other_values = _other_values;
			}
		}

		// Token: 0x0600B5AB RID: 46507 RVA: 0x00455560 File Offset: 0x00453760
		public Prefab Clone(Vector2I offset)
		{
			Prefab prefab = new Prefab(this.id, this.type, offset.x + this.location_x, offset.y + this.location_y, this.element, this.temperature, this.units, this.diseaseName, this.diseaseCount, this.rotationOrientation, this.amounts, this.other_values, this.connections, this.facadeId);
			if (this.rottable != null)
			{
				prefab.rottable = new Rottable();
				prefab.rottable.rotAmount = this.rottable.rotAmount;
			}
			if (this.storage != null && this.storage.Count > 0)
			{
				prefab.storage = new List<StorageItem>();
				foreach (StorageItem storageItem in this.storage)
				{
					prefab.storage.Add(storageItem.Clone());
				}
			}
			return prefab;
		}

		// Token: 0x0600B5AC RID: 46508 RVA: 0x0011A693 File Offset: 0x00118893
		public void AssignStorage(StorageItem _storage)
		{
			if (this.storage == null)
			{
				this.storage = new List<StorageItem>();
			}
			this.storage.Add(_storage);
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x0600B5AD RID: 46509 RVA: 0x0011A6B4 File Offset: 0x001188B4
		// (set) Token: 0x0600B5AE RID: 46510 RVA: 0x0011A6BC File Offset: 0x001188BC
		public string id { get; set; }

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x0600B5AF RID: 46511 RVA: 0x0011A6C5 File Offset: 0x001188C5
		// (set) Token: 0x0600B5B0 RID: 46512 RVA: 0x0011A6CD File Offset: 0x001188CD
		public int location_x { get; set; }

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x0600B5B1 RID: 46513 RVA: 0x0011A6D6 File Offset: 0x001188D6
		// (set) Token: 0x0600B5B2 RID: 46514 RVA: 0x0011A6DE File Offset: 0x001188DE
		public int location_y { get; set; }

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x0600B5B3 RID: 46515 RVA: 0x0011A6E7 File Offset: 0x001188E7
		// (set) Token: 0x0600B5B4 RID: 46516 RVA: 0x0011A6EF File Offset: 0x001188EF
		public SimHashes element { get; set; }

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x0600B5B5 RID: 46517 RVA: 0x0011A6F8 File Offset: 0x001188F8
		// (set) Token: 0x0600B5B6 RID: 46518 RVA: 0x0011A700 File Offset: 0x00118900
		public float temperature { get; set; }

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x0600B5B7 RID: 46519 RVA: 0x0011A709 File Offset: 0x00118909
		// (set) Token: 0x0600B5B8 RID: 46520 RVA: 0x0011A711 File Offset: 0x00118911
		public float units { get; set; }

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x0600B5B9 RID: 46521 RVA: 0x0011A71A File Offset: 0x0011891A
		// (set) Token: 0x0600B5BA RID: 46522 RVA: 0x0011A722 File Offset: 0x00118922
		public string diseaseName { get; set; }

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x0600B5BB RID: 46523 RVA: 0x0011A72B File Offset: 0x0011892B
		// (set) Token: 0x0600B5BC RID: 46524 RVA: 0x0011A733 File Offset: 0x00118933
		public int diseaseCount { get; set; }

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x0600B5BD RID: 46525 RVA: 0x0011A73C File Offset: 0x0011893C
		// (set) Token: 0x0600B5BE RID: 46526 RVA: 0x0011A744 File Offset: 0x00118944
		public Orientation rotationOrientation { get; set; }

		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x0600B5BF RID: 46527 RVA: 0x0011A74D File Offset: 0x0011894D
		// (set) Token: 0x0600B5C0 RID: 46528 RVA: 0x0011A755 File Offset: 0x00118955
		public List<StorageItem> storage { get; set; }

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x0600B5C1 RID: 46529 RVA: 0x0011A75E File Offset: 0x0011895E
		// (set) Token: 0x0600B5C2 RID: 46530 RVA: 0x0011A766 File Offset: 0x00118966
		public Prefab.Type type { get; set; }

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x0600B5C3 RID: 46531 RVA: 0x0011A76F File Offset: 0x0011896F
		// (set) Token: 0x0600B5C4 RID: 46532 RVA: 0x0011A777 File Offset: 0x00118977
		public string facadeId { get; set; }

		// Token: 0x17000BC5 RID: 3013
		// (get) Token: 0x0600B5C5 RID: 46533 RVA: 0x0011A780 File Offset: 0x00118980
		// (set) Token: 0x0600B5C6 RID: 46534 RVA: 0x0011A788 File Offset: 0x00118988
		public int connections { get; set; }

		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x0600B5C7 RID: 46535 RVA: 0x0011A791 File Offset: 0x00118991
		// (set) Token: 0x0600B5C8 RID: 46536 RVA: 0x0011A799 File Offset: 0x00118999
		public Rottable rottable { get; set; }

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x0600B5C9 RID: 46537 RVA: 0x0011A7A2 File Offset: 0x001189A2
		// (set) Token: 0x0600B5CA RID: 46538 RVA: 0x0011A7AA File Offset: 0x001189AA
		public Prefab.template_amount_value[] amounts { get; set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x0600B5CB RID: 46539 RVA: 0x0011A7B3 File Offset: 0x001189B3
		// (set) Token: 0x0600B5CC RID: 46540 RVA: 0x0011A7BB File Offset: 0x001189BB
		public Prefab.template_amount_value[] other_values { get; set; }

		// Token: 0x02002151 RID: 8529
		public enum Type
		{
			// Token: 0x04008FF0 RID: 36848
			Building,
			// Token: 0x04008FF1 RID: 36849
			Ore,
			// Token: 0x04008FF2 RID: 36850
			Pickupable,
			// Token: 0x04008FF3 RID: 36851
			Other
		}

		// Token: 0x02002152 RID: 8530
		[Serializable]
		public class template_amount_value
		{
			// Token: 0x0600B5CD RID: 46541 RVA: 0x000AA024 File Offset: 0x000A8224
			public template_amount_value()
			{
			}

			// Token: 0x0600B5CE RID: 46542 RVA: 0x0011A7C4 File Offset: 0x001189C4
			public template_amount_value(string id, float value)
			{
				this.id = id;
				this.value = value;
			}

			// Token: 0x17000BC9 RID: 3017
			// (get) Token: 0x0600B5CF RID: 46543 RVA: 0x0011A7DA File Offset: 0x001189DA
			// (set) Token: 0x0600B5D0 RID: 46544 RVA: 0x0011A7E2 File Offset: 0x001189E2
			public string id { get; set; }

			// Token: 0x17000BCA RID: 3018
			// (get) Token: 0x0600B5D1 RID: 46545 RVA: 0x0011A7EB File Offset: 0x001189EB
			// (set) Token: 0x0600B5D2 RID: 46546 RVA: 0x0011A7F3 File Offset: 0x001189F3
			public float value { get; set; }
		}
	}
}
