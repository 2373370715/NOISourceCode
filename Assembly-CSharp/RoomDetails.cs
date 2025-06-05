using System;
using STRINGS;

// Token: 0x0200184D RID: 6221
public class RoomDetails
{
	// Token: 0x06008036 RID: 32822 RVA: 0x0033FD38 File Offset: 0x0033DF38
	public static string RoomDetailString(Room room)
	{
		string text = "";
		text = text + "<b>" + ROOMS.DETAILS.HEADER + "</b>";
		foreach (RoomDetails.Detail detail in room.roomType.display_details)
		{
			text = text + "\n    • " + detail.resolve_string_function(room);
		}
		return text;
	}

	// Token: 0x04006186 RID: 24966
	public static readonly RoomDetails.Detail AVERAGE_TEMPERATURE = new RoomDetails.Detail(delegate(Room room)
	{
		float num = 0f;
		if (num == 0f)
		{
			return string.Format(ROOMS.DETAILS.AVERAGE_TEMPERATURE.NAME, UI.OVERLAYS.TEMPERATURE.EXTREMECOLD);
		}
		return string.Format(ROOMS.DETAILS.AVERAGE_TEMPERATURE.NAME, GameUtil.GetFormattedTemperature(num, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
	});

	// Token: 0x04006187 RID: 24967
	public static readonly RoomDetails.Detail AVERAGE_ATMO_MASS = new RoomDetails.Detail(delegate(Room room)
	{
		float num = 0f;
		float num2 = 0f;
		if (num2 > 0f)
		{
			num /= num2;
		}
		else
		{
			num = 0f;
		}
		return string.Format(ROOMS.DETAILS.AVERAGE_ATMO_MASS.NAME, GameUtil.GetFormattedMass(num, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
	});

	// Token: 0x04006188 RID: 24968
	public static readonly RoomDetails.Detail ASSIGNED_TO = new RoomDetails.Detail(delegate(Room room)
	{
		string text = "";
		foreach (KPrefabID kprefabID in room.GetPrimaryEntities())
		{
			if (!(kprefabID == null))
			{
				Assignable component = kprefabID.GetComponent<Assignable>();
				if (!(component == null))
				{
					IAssignableIdentity assignee = component.assignee;
					if (assignee == null)
					{
						text += ((text == "") ? ("<color=#BCBCBC>    • " + kprefabID.GetProperName() + ": " + ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED) : ("\n<color=#BCBCBC>    • " + kprefabID.GetProperName() + ": " + ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED));
						text += "</color>";
					}
					else
					{
						text += ((text == "") ? ("    • " + kprefabID.GetProperName() + ": " + assignee.GetProperName()) : ("\n    • " + kprefabID.GetProperName() + ": " + assignee.GetProperName()));
					}
				}
			}
		}
		if (text == "")
		{
			text = ROOMS.DETAILS.ASSIGNED_TO.UNASSIGNED;
		}
		return string.Format(ROOMS.DETAILS.ASSIGNED_TO.NAME, text);
	});

	// Token: 0x04006189 RID: 24969
	public static readonly RoomDetails.Detail SIZE = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.SIZE.NAME, room.cavity.numCells));

	// Token: 0x0400618A RID: 24970
	public static readonly RoomDetails.Detail BUILDING_COUNT = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.BUILDING_COUNT.NAME, room.buildings.Count));

	// Token: 0x0400618B RID: 24971
	public static readonly RoomDetails.Detail CREATURE_COUNT = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.CREATURE_COUNT.NAME, room.cavity.creatures.Count + room.cavity.eggs.Count));

	// Token: 0x0400618C RID: 24972
	public static readonly RoomDetails.Detail PLANT_COUNT = new RoomDetails.Detail((Room room) => string.Format(ROOMS.DETAILS.PLANT_COUNT.NAME, room.cavity.plants.Count));

	// Token: 0x0400618D RID: 24973
	public static readonly RoomDetails.Detail EFFECT = new RoomDetails.Detail((Room room) => room.roomType.effect);

	// Token: 0x0400618E RID: 24974
	public static readonly RoomDetails.Detail EFFECTS = new RoomDetails.Detail((Room room) => room.roomType.GetRoomEffectsString());

	// Token: 0x0200184E RID: 6222
	public class Detail
	{
		// Token: 0x06008039 RID: 32825 RVA: 0x000F8F30 File Offset: 0x000F7130
		public Detail(Func<Room, string> resolve_string_function)
		{
			this.resolve_string_function = resolve_string_function;
		}

		// Token: 0x0400618F RID: 24975
		public Func<Room, string> resolve_string_function;
	}
}
