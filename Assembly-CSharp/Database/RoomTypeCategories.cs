using System;
using STRINGS;

namespace Database
{
	// Token: 0x020021CB RID: 8651
	public class RoomTypeCategories : ResourceSet<RoomTypeCategory>
	{
		// Token: 0x0600B886 RID: 47238 RVA: 0x0046F734 File Offset: 0x0046D934
		private RoomTypeCategory Add(string id, string name, string colorName, string icon)
		{
			RoomTypeCategory roomTypeCategory = new RoomTypeCategory(id, name, colorName, icon);
			base.Add(roomTypeCategory);
			return roomTypeCategory;
		}

		// Token: 0x0600B887 RID: 47239 RVA: 0x0046F758 File Offset: 0x0046D958
		public RoomTypeCategories(ResourceSet parent) : base("RoomTypeCategories", parent)
		{
			base.Initialize();
			this.None = this.Add("None", ROOMS.CATEGORY.NONE.NAME, "roomNone", "unknown");
			this.Food = this.Add("Food", ROOMS.CATEGORY.FOOD.NAME, "roomFood", "ui_room_food");
			this.Sleep = this.Add("Sleep", ROOMS.CATEGORY.SLEEP.NAME, "roomSleep", "ui_room_sleep");
			this.Recreation = this.Add("Recreation", ROOMS.CATEGORY.RECREATION.NAME, "roomRecreation", "ui_room_recreational");
			if (DlcManager.IsContentSubscribed("DLC3_ID"))
			{
				this.Bionic = this.Add("Bionic", ROOMS.CATEGORY.BIONIC.NAME, "roomBionic", "ui_room_bionicupkeep");
			}
			this.Bathroom = this.Add("Bathroom", ROOMS.CATEGORY.BATHROOM.NAME, "roomBathroom", "ui_room_bathroom");
			this.Hospital = this.Add("Hospital", ROOMS.CATEGORY.HOSPITAL.NAME, "roomHospital", "ui_room_hospital");
			this.Industrial = this.Add("Industrial", ROOMS.CATEGORY.INDUSTRIAL.NAME, "roomIndustrial", "ui_room_industrial");
			this.Agricultural = this.Add("Agricultural", ROOMS.CATEGORY.AGRICULTURAL.NAME, "roomAgricultural", "ui_room_agricultural");
			this.Park = this.Add("Park", ROOMS.CATEGORY.PARK.NAME, "roomPark", "ui_room_park");
			this.Science = this.Add("Science", ROOMS.CATEGORY.SCIENCE.NAME, "roomScience", "ui_room_science");
		}

		// Token: 0x0400966A RID: 38506
		public RoomTypeCategory None;

		// Token: 0x0400966B RID: 38507
		public RoomTypeCategory Food;

		// Token: 0x0400966C RID: 38508
		public RoomTypeCategory Sleep;

		// Token: 0x0400966D RID: 38509
		public RoomTypeCategory Recreation;

		// Token: 0x0400966E RID: 38510
		public RoomTypeCategory Bathroom;

		// Token: 0x0400966F RID: 38511
		public RoomTypeCategory Bionic;

		// Token: 0x04009670 RID: 38512
		public RoomTypeCategory Hospital;

		// Token: 0x04009671 RID: 38513
		public RoomTypeCategory Industrial;

		// Token: 0x04009672 RID: 38514
		public RoomTypeCategory Agricultural;

		// Token: 0x04009673 RID: 38515
		public RoomTypeCategory Park;

		// Token: 0x04009674 RID: 38516
		public RoomTypeCategory Science;
	}
}
