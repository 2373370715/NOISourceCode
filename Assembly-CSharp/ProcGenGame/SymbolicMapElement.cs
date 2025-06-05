using System;

namespace ProcGenGame
{
	// Token: 0x02002107 RID: 8455
	public interface SymbolicMapElement
	{
		// Token: 0x0600B3F1 RID: 46065
		void ConvertToMap(Chunk world, TerrainCell.SetValuesFunction SetValues, float temperatureMin, float temperatureRange, SeededRandom rnd);
	}
}
