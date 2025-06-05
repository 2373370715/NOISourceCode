using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000965 RID: 2405
[AddComponentMenu("KMonoBehaviour/scripts/SymbolOverrideController")]
public class SymbolOverrideController : KMonoBehaviour
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06002AFD RID: 11005 RVA: 0x000C0645 File Offset: 0x000BE845
	public SymbolOverrideController.SymbolEntry[] GetSymbolOverrides
	{
		get
		{
			return this.symbolOverrides.ToArray();
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x06002AFE RID: 11006 RVA: 0x000C0652 File Offset: 0x000BE852
	// (set) Token: 0x06002AFF RID: 11007 RVA: 0x000C065A File Offset: 0x000BE85A
	public int version { get; private set; }

	// Token: 0x06002B00 RID: 11008 RVA: 0x001E97A0 File Offset: 0x001E79A0
	protected override void OnPrefabInit()
	{
		this.animController = base.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>() != null, "SymbolOverrideController requires KBatchedAnimController");
		DebugUtil.Assert(base.GetComponent<KBatchedAnimController>().usingNewSymbolOverrideSystem, "SymbolOverrideController requires usingNewSymbolOverrideSystem to be set to true. Try adding the component by calling: SymbolOverrideControllerUtil.AddToPrefab");
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			symbolEntry.sourceSymbol = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry.sourceSymbolBatchTag).GetSymbol(symbolEntry.sourceSymbolId);
			this.symbolOverrides[i] = symbolEntry;
		}
		this.atlases = new KAnimBatch.AtlasList(0, KAnimBatchManager.MaxAtlasesByMaterialType[(int)this.animController.materialType]);
		this.faceGraph = base.GetComponent<FaceGraph>();
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x001E9864 File Offset: 0x001E7A64
	public int AddSymbolOverride(HashedString target_symbol, KAnim.Build.Symbol source_symbol, int priority = 0)
	{
		if (source_symbol == null)
		{
			throw new Exception("NULL source symbol when overriding: " + target_symbol.ToString());
		}
		SymbolOverrideController.SymbolEntry symbolEntry = new SymbolOverrideController.SymbolEntry
		{
			targetSymbol = target_symbol,
			sourceSymbol = source_symbol,
			sourceSymbolId = new HashedString(source_symbol.hash.HashValue),
			sourceSymbolBatchTag = source_symbol.build.batchTag,
			priority = priority
		};
		int num = this.GetSymbolOverrideIdx(target_symbol, priority);
		if (num >= 0)
		{
			this.symbolOverrides[num] = symbolEntry;
		}
		else
		{
			num = this.symbolOverrides.Count;
			this.symbolOverrides.Add(symbolEntry);
		}
		this.MarkDirty();
		return num;
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x001E9918 File Offset: 0x001E7B18
	public bool RemoveSymbolOverride(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				this.symbolOverrides.RemoveAt(i);
				this.MarkDirty();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x001E9974 File Offset: 0x001E7B74
	public void RemoveAllSymbolOverrides(int priority = 0)
	{
		this.symbolOverrides.RemoveAll((SymbolOverrideController.SymbolEntry x) => x.priority >= priority);
		this.MarkDirty();
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x001E99AC File Offset: 0x001E7BAC
	public int GetSymbolOverrideIdx(HashedString target_symbol, int priority = 0)
	{
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			if (symbolEntry.targetSymbol == target_symbol && symbolEntry.priority == priority)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002B05 RID: 11013 RVA: 0x000C0663 File Offset: 0x000BE863
	public int GetAtlasIdx(Texture2D atlas)
	{
		return this.atlases.GetAtlasIdx(atlas);
	}

	// Token: 0x06002B06 RID: 11014 RVA: 0x001E99F8 File Offset: 0x001E7BF8
	public void ApplyOverrides()
	{
		if (this.requiresSorting)
		{
			this.symbolOverrides.Sort((SymbolOverrideController.SymbolEntry x, SymbolOverrideController.SymbolEntry y) => x.priority - y.priority);
			this.requiresSorting = false;
		}
		KAnimBatch batch = this.animController.GetBatch();
		DebugUtil.Assert(batch != null);
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.animController.batchGroupID);
		int count = batch.atlases.Count;
		this.atlases.Clear(count);
		DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.PooledDictionary pooledDictionary = DictionaryPool<HashedString, Pair<int, int>, SymbolOverrideController>.Allocate();
		ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.PooledList pooledList = ListPool<SymbolOverrideController.SymbolEntry, SymbolOverrideController>.Allocate();
		for (int i = 0; i < this.symbolOverrides.Count; i++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry = this.symbolOverrides[i];
			Pair<int, int> pair;
			if (pooledDictionary.TryGetValue(symbolEntry.targetSymbol, out pair))
			{
				int first = pair.first;
				if (symbolEntry.priority > first)
				{
					int second = pair.second;
					pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, second);
					pooledList[second] = symbolEntry;
				}
			}
			else
			{
				pooledDictionary[symbolEntry.targetSymbol] = new Pair<int, int>(symbolEntry.priority, pooledList.Count);
				pooledList.Add(symbolEntry);
			}
		}
		DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.PooledDictionary pooledDictionary2 = DictionaryPool<KAnim.Build, SymbolOverrideController.BatchGroupInfo, SymbolOverrideController>.Allocate();
		for (int j = 0; j < pooledList.Count; j++)
		{
			SymbolOverrideController.SymbolEntry symbolEntry2 = pooledList[j];
			SymbolOverrideController.BatchGroupInfo batchGroupInfo;
			if (!pooledDictionary2.TryGetValue(symbolEntry2.sourceSymbol.build, out batchGroupInfo))
			{
				batchGroupInfo = new SymbolOverrideController.BatchGroupInfo
				{
					build = symbolEntry2.sourceSymbol.build,
					data = KAnimBatchManager.Instance().GetBatchGroupData(symbolEntry2.sourceSymbol.build.batchTag)
				};
				Texture2D texture = symbolEntry2.sourceSymbol.build.GetTexture(0);
				int num = batch.atlases.GetAtlasIdx(texture);
				if (num < 0)
				{
					num = this.atlases.Add(texture);
				}
				batchGroupInfo.atlasIdx = num;
				pooledDictionary2[batchGroupInfo.build] = batchGroupInfo;
			}
			KAnim.Build.Symbol symbol = batchGroupData.GetSymbol(symbolEntry2.targetSymbol);
			if (symbol != null)
			{
				this.animController.SetSymbolOverrides(symbol.firstFrameIdx, symbol.numFrames, batchGroupInfo.atlasIdx, batchGroupInfo.data, symbolEntry2.sourceSymbol.firstFrameIdx, symbolEntry2.sourceSymbol.numFrames);
			}
		}
		pooledDictionary2.Recycle();
		pooledList.Recycle();
		pooledDictionary.Recycle();
		if (this.faceGraph != null)
		{
			this.faceGraph.ApplyShape();
		}
	}

	// Token: 0x06002B07 RID: 11015 RVA: 0x001E9C9C File Offset: 0x001E7E9C
	public void ApplyAtlases()
	{
		KAnimBatch batch = this.animController.GetBatch();
		this.atlases.Apply(batch.matProperties);
	}

	// Token: 0x06002B08 RID: 11016 RVA: 0x000C0671 File Offset: 0x000BE871
	public KAnimBatch.AtlasList GetAtlasList()
	{
		return this.atlases;
	}

	// Token: 0x06002B09 RID: 11017 RVA: 0x001E9CC8 File Offset: 0x001E7EC8
	public void MarkDirty()
	{
		if (this.animController != null)
		{
			this.animController.SetDirty();
		}
		int version = this.version + 1;
		this.version = version;
		this.requiresSorting = true;
	}

	// Token: 0x04001D1E RID: 7454
	public bool applySymbolOverridesEveryFrame;

	// Token: 0x04001D1F RID: 7455
	[SerializeField]
	private List<SymbolOverrideController.SymbolEntry> symbolOverrides = new List<SymbolOverrideController.SymbolEntry>();

	// Token: 0x04001D20 RID: 7456
	private KAnimBatch.AtlasList atlases;

	// Token: 0x04001D21 RID: 7457
	private KBatchedAnimController animController;

	// Token: 0x04001D22 RID: 7458
	private FaceGraph faceGraph;

	// Token: 0x04001D24 RID: 7460
	private bool requiresSorting;

	// Token: 0x02000966 RID: 2406
	[Serializable]
	public struct SymbolEntry
	{
		// Token: 0x04001D25 RID: 7461
		public HashedString targetSymbol;

		// Token: 0x04001D26 RID: 7462
		[NonSerialized]
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04001D27 RID: 7463
		public HashedString sourceSymbolId;

		// Token: 0x04001D28 RID: 7464
		public HashedString sourceSymbolBatchTag;

		// Token: 0x04001D29 RID: 7465
		public int priority;
	}

	// Token: 0x02000967 RID: 2407
	private struct SymbolToOverride
	{
		// Token: 0x04001D2A RID: 7466
		public KAnim.Build.Symbol sourceSymbol;

		// Token: 0x04001D2B RID: 7467
		public HashedString targetSymbol;

		// Token: 0x04001D2C RID: 7468
		public KBatchGroupData data;

		// Token: 0x04001D2D RID: 7469
		public int atlasIdx;
	}

	// Token: 0x02000968 RID: 2408
	private struct BatchGroupInfo
	{
		// Token: 0x04001D2E RID: 7470
		public KAnim.Build build;

		// Token: 0x04001D2F RID: 7471
		public int atlasIdx;

		// Token: 0x04001D30 RID: 7472
		public KBatchGroupData data;
	}
}
