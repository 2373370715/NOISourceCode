using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000991 RID: 2449
public class NoiseSplat : IUniformGridObject
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000C0D9D File Offset: 0x000BEF9D
	// (set) Token: 0x06002B9D RID: 11165 RVA: 0x000C0DA5 File Offset: 0x000BEFA5
	public int dB { get; private set; }

	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000C0DAE File Offset: 0x000BEFAE
	// (set) Token: 0x06002B9F RID: 11167 RVA: 0x000C0DB6 File Offset: 0x000BEFB6
	public float deathTime { get; private set; }

	// Token: 0x06002BA0 RID: 11168 RVA: 0x000C0DBF File Offset: 0x000BEFBF
	public string GetName()
	{
		return this.provider.GetName();
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x000C0DCC File Offset: 0x000BEFCC
	public IPolluter GetProvider()
	{
		return this.provider;
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x000C0DD4 File Offset: 0x000BEFD4
	public Vector2 PosMin()
	{
		return new Vector2(this.position.x - (float)this.radius, this.position.y - (float)this.radius);
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x000C0E01 File Offset: 0x000BF001
	public Vector2 PosMax()
	{
		return new Vector2(this.position.x + (float)this.radius, this.position.y + (float)this.radius);
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x001ECF74 File Offset: 0x001EB174
	public NoiseSplat(NoisePolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.dB = 0;
		this.radius = 5;
		if (setProvider.dB != null)
		{
			this.dB = (int)setProvider.dB.GetTotalValue();
		}
		int cell = Grid.PosToCell(setProvider.gameObject);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		setProvider.Clear();
		OccupyArea occupyArea = setProvider.occupyArea;
		this.baseExtents = occupyArea.GetExtents();
		this.provider = setProvider;
		this.position = setProvider.transform.GetPosition();
		if (setProvider.dBRadius != null)
		{
			this.radius = (int)setProvider.dBRadius.GetTotalValue();
		}
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int widthInCells = occupyArea.GetWidthInCells();
		int heightInCells = occupyArea.GetHeightInCells();
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
		this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x001ED15C File Offset: 0x001EB35C
	public NoiseSplat(IPolluter setProvider, float death_time = 0f)
	{
		this.deathTime = death_time;
		this.provider = setProvider;
		this.provider.Clear();
		this.position = this.provider.GetPosition();
		this.dB = this.provider.GetNoise();
		int cell = Grid.PosToCell(this.position);
		if (!NoisePolluter.IsNoiseableCell(cell))
		{
			this.dB = 0;
		}
		if (this.dB == 0)
		{
			return;
		}
		this.radius = this.provider.GetRadius();
		if (this.radius == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		Vector2I vector2I = new Vector2I(num - this.radius, num2 - this.radius);
		Vector2I vector2I2 = vector2I + new Vector2I(this.radius * 2, this.radius * 2);
		vector2I = Vector2I.Max(vector2I, Vector2I.zero);
		vector2I2 = Vector2I.Min(vector2I2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
		this.effectExtents = new Extents(vector2I.x, vector2I.y, vector2I2.x - vector2I.x, vector2I2.y - vector2I.y);
		this.baseExtents = new Extents(num, num2, 1, 1);
		this.AddNoise();
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x000C0E2E File Offset: 0x000BF02E
	public void Clear()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
		this.RemoveNoise();
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x001ED2A8 File Offset: 0x001EB4A8
	private void AddNoise()
	{
		int cell = Grid.PosToCell(this.position);
		int num = this.effectExtents.x + this.effectExtents.width;
		int num2 = this.effectExtents.y + this.effectExtents.height;
		int num3 = this.effectExtents.x;
		int num4 = this.effectExtents.y;
		int x = 0;
		int y = 0;
		Grid.CellToXY(cell, out x, out y);
		num = Math.Min(num, Grid.WidthInCells);
		num2 = Math.Min(num2, Grid.HeightInCells);
		num3 = Math.Max(0, num3);
		num4 = Math.Max(0, num4);
		for (int i = num4; i < num2; i++)
		{
			for (int j = num3; j < num; j++)
			{
				if (Grid.VisibilityTest(x, y, j, i, false))
				{
					int num5 = Grid.XYToCell(j, i);
					float dbforCell = this.GetDBForCell(num5);
					if (dbforCell > 0f)
					{
						float num6 = AudioEventManager.DBToLoudness(dbforCell);
						Grid.Loudness[num5] += num6;
						Pair<int, float> item = new Pair<int, float>(num5, num6);
						this.decibels.Add(item);
					}
				}
			}
		}
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x001ED3C0 File Offset: 0x001EB5C0
	public float GetDBForCell(int cell)
	{
		Vector2 vector = Grid.CellToPos2D(cell);
		float num = Mathf.Floor(Vector2.Distance(this.position, vector));
		if (vector.x >= (float)this.baseExtents.x && vector.x < (float)(this.baseExtents.x + this.baseExtents.width) && vector.y >= (float)this.baseExtents.y && vector.y < (float)(this.baseExtents.y + this.baseExtents.height))
		{
			num = 0f;
		}
		return Mathf.Round((float)this.dB - (float)this.dB * num * 0.05f);
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x001ED478 File Offset: 0x001EB678
	private void RemoveNoise()
	{
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			float num = Math.Max(0f, Grid.Loudness[pair.first] - pair.second);
			Grid.Loudness[pair.first] = ((num < 1f) ? 0f : num);
		}
		this.decibels.Clear();
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x001ED4F0 File Offset: 0x001EB6F0
	public float GetLoudness(int cell)
	{
		float result = 0f;
		for (int i = 0; i < this.decibels.Count; i++)
		{
			Pair<int, float> pair = this.decibels[i];
			if (pair.first == cell)
			{
				result = pair.second;
				break;
			}
		}
		return result;
	}

	// Token: 0x04001DDE RID: 7646
	public const float noiseFalloff = 0.05f;

	// Token: 0x04001DE1 RID: 7649
	private IPolluter provider;

	// Token: 0x04001DE2 RID: 7650
	private Vector2 position;

	// Token: 0x04001DE3 RID: 7651
	private int radius;

	// Token: 0x04001DE4 RID: 7652
	private Extents effectExtents;

	// Token: 0x04001DE5 RID: 7653
	private Extents baseExtents;

	// Token: 0x04001DE6 RID: 7654
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001DE7 RID: 7655
	private HandleVector<int>.Handle solidChangedPartitionerEntry;

	// Token: 0x04001DE8 RID: 7656
	private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
}
