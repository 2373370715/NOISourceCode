using System;
using System.Collections.Generic;

// Token: 0x02001525 RID: 5413
public interface IGroupProber
{
	// Token: 0x06007088 RID: 28808
	void Occupy(object prober, short serial_no, IEnumerable<int> cells);

	// Token: 0x06007089 RID: 28809
	void SetValidSerialNos(object prober, short previous_serial_no, short serial_no);

	// Token: 0x0600708A RID: 28810
	bool ReleaseProber(object prober);
}
