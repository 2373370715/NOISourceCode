using System;

// Token: 0x02001373 RID: 4979
public static class GameSoundEvents
{
	// Token: 0x04004B93 RID: 19347
	public static GameSoundEvents.Event BatteryFull = new GameSoundEvents.Event("game_triggered.battery_full");

	// Token: 0x04004B94 RID: 19348
	public static GameSoundEvents.Event BatteryWarning = new GameSoundEvents.Event("game_triggered.battery_warning");

	// Token: 0x04004B95 RID: 19349
	public static GameSoundEvents.Event BatteryDischarged = new GameSoundEvents.Event("game_triggered.battery_drained");

	// Token: 0x02001374 RID: 4980
	public class Event
	{
		// Token: 0x06006600 RID: 26112 RVA: 0x000E7049 File Offset: 0x000E5249
		public Event(string name)
		{
			this.Name = name;
		}

		// Token: 0x04004B96 RID: 19350
		public HashedString Name;
	}
}
