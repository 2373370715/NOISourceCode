using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000976 RID: 2422
[AddComponentMenu("KMonoBehaviour/scripts/AudioEventManager")]
public class AudioEventManager : KMonoBehaviour
{
	// Token: 0x06002B43 RID: 11075 RVA: 0x001EAFF4 File Offset: 0x001E91F4
	public static AudioEventManager Get()
	{
		if (AudioEventManager.instance == null)
		{
			if (App.IsExiting)
			{
				return null;
			}
			GameObject gameObject = GameObject.Find("/AudioEventManager");
			if (gameObject == null)
			{
				gameObject = new GameObject();
				gameObject.name = "AudioEventManager";
			}
			AudioEventManager.instance = gameObject.GetComponent<AudioEventManager>();
			if (AudioEventManager.instance == null)
			{
				AudioEventManager.instance = gameObject.AddComponent<AudioEventManager>();
			}
		}
		return AudioEventManager.instance;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x000C0A1D File Offset: 0x000BEC1D
	protected override void OnSpawn()
	{
		base.OnPrefabInit();
		this.spatialSplats.Reset(Grid.WidthInCells, Grid.HeightInCells, 16, 16);
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x000C0A3E File Offset: 0x000BEC3E
	public static float LoudnessToDB(float loudness)
	{
		if (loudness <= 0f)
		{
			return 0f;
		}
		return 10f * Mathf.Log10(loudness);
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x000C0A5A File Offset: 0x000BEC5A
	public static float DBToLoudness(float src_db)
	{
		return Mathf.Pow(10f, src_db / 10f);
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x000C0A6D File Offset: 0x000BEC6D
	public float GetDecibelsAtCell(int cell)
	{
		return Mathf.Round(AudioEventManager.LoudnessToDB(Grid.Loudness[cell]) * 2f) / 2f;
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x001EB064 File Offset: 0x001E9264
	public static string GetLoudestNoisePolluterAtCell(int cell)
	{
		float negativeInfinity = float.NegativeInfinity;
		string result = null;
		AudioEventManager audioEventManager = AudioEventManager.Get();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in audioEventManager.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			if (noiseSplat.GetLoudness(cell) > negativeInfinity)
			{
				result = noiseSplat.GetProvider().GetName();
			}
		}
		return result;
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x000C0A8C File Offset: 0x000BEC8C
	public void ClearNoiseSplat(NoiseSplat splat)
	{
		if (this.splats.Contains(splat))
		{
			this.splats.Remove(splat);
			this.spatialSplats.Remove(splat);
		}
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x000C0AB5 File Offset: 0x000BECB5
	public void AddSplat(NoiseSplat splat)
	{
		this.splats.Add(splat);
		this.spatialSplats.Add(splat);
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x001EB108 File Offset: 0x001E9308
	public NoiseSplat CreateNoiseSplat(Vector2 pos, int dB, int radius, string name, GameObject go)
	{
		Polluter polluter = this.GetPolluter(radius);
		polluter.SetAttributes(pos, dB, go, name);
		NoiseSplat noiseSplat = new NoiseSplat(polluter, 0f);
		polluter.SetSplat(noiseSplat);
		return noiseSplat;
	}

	// Token: 0x06002B4C RID: 11084 RVA: 0x001EB13C File Offset: 0x001E933C
	public List<AudioEventManager.PolluterDisplay> GetPollutersForCell(int cell)
	{
		this.polluters.Clear();
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2 pos = new Vector2((float)vector2I.x, (float)vector2I.y);
		foreach (object obj in this.spatialSplats.GetAllIntersecting(pos))
		{
			NoiseSplat noiseSplat = (NoiseSplat)obj;
			float loudness = noiseSplat.GetLoudness(cell);
			if (loudness > 0f)
			{
				AudioEventManager.PolluterDisplay item = default(AudioEventManager.PolluterDisplay);
				item.name = noiseSplat.GetName();
				item.value = AudioEventManager.LoudnessToDB(loudness);
				item.provider = noiseSplat.GetProvider();
				this.polluters.Add(item);
			}
		}
		return this.polluters;
	}

	// Token: 0x06002B4D RID: 11085 RVA: 0x001EB214 File Offset: 0x001E9414
	private void RemoveExpiredSplats()
	{
		if (this.removeTime.Count > 1)
		{
			this.removeTime.Sort((Pair<float, NoiseSplat> a, Pair<float, NoiseSplat> b) => a.first.CompareTo(b.first));
		}
		int num = -1;
		int num2 = 0;
		while (num2 < this.removeTime.Count && this.removeTime[num2].first <= Time.time)
		{
			NoiseSplat second = this.removeTime[num2].second;
			if (second != null)
			{
				IPolluter provider = second.GetProvider();
				this.FreePolluter(provider as Polluter);
			}
			num = num2;
			num2++;
		}
		for (int i = num; i >= 0; i--)
		{
			this.removeTime.RemoveAt(i);
		}
	}

	// Token: 0x06002B4E RID: 11086 RVA: 0x000C0AD0 File Offset: 0x000BECD0
	private void Update()
	{
		this.RemoveExpiredSplats();
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x001EB2D0 File Offset: 0x001E94D0
	private Polluter GetPolluter(int radius)
	{
		if (!this.freePool.ContainsKey(radius))
		{
			this.freePool.Add(radius, new List<Polluter>());
		}
		Polluter polluter;
		if (this.freePool[radius].Count > 0)
		{
			polluter = this.freePool[radius][0];
			this.freePool[radius].RemoveAt(0);
		}
		else
		{
			polluter = new Polluter(radius);
		}
		if (!this.inusePool.ContainsKey(radius))
		{
			this.inusePool.Add(radius, new List<Polluter>());
		}
		this.inusePool[radius].Add(polluter);
		return polluter;
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x001EB374 File Offset: 0x001E9574
	private void FreePolluter(Polluter pol)
	{
		if (pol != null)
		{
			pol.Clear();
			global::Debug.Assert(this.inusePool[pol.radius].Contains(pol));
			this.inusePool[pol.radius].Remove(pol);
			this.freePool[pol.radius].Add(pol);
		}
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x001EB3D8 File Offset: 0x001E95D8
	public void PlayTimedOnceOff(Vector2 pos, int dB, int radius, string name, GameObject go, float time = 1f)
	{
		if (dB > 0 && radius > 0 && time > 0f)
		{
			Polluter polluter = this.GetPolluter(radius);
			polluter.SetAttributes(pos, dB, go, name);
			this.AddTimedInstance(polluter, time);
		}
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x001EB414 File Offset: 0x001E9614
	private void AddTimedInstance(Polluter p, float time)
	{
		NoiseSplat noiseSplat = new NoiseSplat(p, time + Time.time);
		p.SetSplat(noiseSplat);
		this.removeTime.Add(new Pair<float, NoiseSplat>(time + Time.time, noiseSplat));
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x000C0AD8 File Offset: 0x000BECD8
	private static void SoundLog(long itemId, string message)
	{
		global::Debug.Log(" [" + itemId.ToString() + "] \t" + message);
	}

	// Token: 0x04001D79 RID: 7545
	public const float NO_NOISE_EFFECTORS = 0f;

	// Token: 0x04001D7A RID: 7546
	public const float MIN_LOUDNESS_THRESHOLD = 1f;

	// Token: 0x04001D7B RID: 7547
	private static AudioEventManager instance;

	// Token: 0x04001D7C RID: 7548
	private List<Pair<float, NoiseSplat>> removeTime = new List<Pair<float, NoiseSplat>>();

	// Token: 0x04001D7D RID: 7549
	private Dictionary<int, List<Polluter>> freePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001D7E RID: 7550
	private Dictionary<int, List<Polluter>> inusePool = new Dictionary<int, List<Polluter>>();

	// Token: 0x04001D7F RID: 7551
	private HashSet<NoiseSplat> splats = new HashSet<NoiseSplat>();

	// Token: 0x04001D80 RID: 7552
	private UniformGrid<NoiseSplat> spatialSplats = new UniformGrid<NoiseSplat>();

	// Token: 0x04001D81 RID: 7553
	private List<AudioEventManager.PolluterDisplay> polluters = new List<AudioEventManager.PolluterDisplay>();

	// Token: 0x02000977 RID: 2423
	public enum NoiseEffect
	{
		// Token: 0x04001D83 RID: 7555
		Peaceful,
		// Token: 0x04001D84 RID: 7556
		Quiet = 36,
		// Token: 0x04001D85 RID: 7557
		TossAndTurn = 45,
		// Token: 0x04001D86 RID: 7558
		WakeUp = 60,
		// Token: 0x04001D87 RID: 7559
		Passive = 80,
		// Token: 0x04001D88 RID: 7560
		Active = 106
	}

	// Token: 0x02000978 RID: 2424
	public struct PolluterDisplay
	{
		// Token: 0x04001D89 RID: 7561
		public string name;

		// Token: 0x04001D8A RID: 7562
		public float value;

		// Token: 0x04001D8B RID: 7563
		public IPolluter provider;
	}
}
