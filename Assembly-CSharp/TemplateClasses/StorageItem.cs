﻿using System;

namespace TemplateClasses
{
	[Serializable]
	public class StorageItem
	{
		public StorageItem()
		{
			this.rottable = new Rottable();
		}

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

		public string id { get; set; }

		public SimHashes element { get; set; }

		public float units { get; set; }

		public bool isOre { get; set; }

		public float temperature { get; set; }

		public string diseaseName { get; set; }

		public int diseaseCount { get; set; }

		public Rottable rottable { get; set; }

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
