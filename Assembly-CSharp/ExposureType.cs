using System;
using System.Collections.Generic;

// Token: 0x020015C3 RID: 5571
public class ExposureType
{
	// Token: 0x040056D2 RID: 22226
	public string germ_id;

	// Token: 0x040056D3 RID: 22227
	public string sickness_id;

	// Token: 0x040056D4 RID: 22228
	public string infection_effect;

	// Token: 0x040056D5 RID: 22229
	public int exposure_threshold;

	// Token: 0x040056D6 RID: 22230
	public bool infect_immediately;

	// Token: 0x040056D7 RID: 22231
	public List<string> required_traits;

	// Token: 0x040056D8 RID: 22232
	public List<string> excluded_traits;

	// Token: 0x040056D9 RID: 22233
	public List<string> excluded_effects;

	// Token: 0x040056DA RID: 22234
	public int base_resistance;
}
