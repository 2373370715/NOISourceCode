using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000C5A RID: 3162
public class ArtifactSelector : KMonoBehaviour
{
	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06003BB9 RID: 15289 RVA: 0x000CB050 File Offset: 0x000C9250
	public int AnalyzedArtifactCount
	{
		get
		{
			return this.analyzedArtifactCount;
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06003BBA RID: 15290 RVA: 0x000CB058 File Offset: 0x000C9258
	public int AnalyzedSpaceArtifactCount
	{
		get
		{
			return this.analyzedSpaceArtifactCount;
		}
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x000CB060 File Offset: 0x000C9260
	public List<string> GetAnalyzedArtifactIDs()
	{
		return this.analyzedArtifatIDs;
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x002398F8 File Offset: 0x00237AF8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		ArtifactSelector.Instance = this;
		this.placedArtifacts.Add(ArtifactType.Terrestrial, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Space, new List<string>());
		this.placedArtifacts.Add(ArtifactType.Any, new List<string>());
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x00239944 File Offset: 0x00237B44
	protected override void OnSpawn()
	{
		base.OnSpawn();
		int num = 0;
		int num2 = 0;
		foreach (string artifactID in this.analyzedArtifatIDs)
		{
			ArtifactType artifactType = this.GetArtifactType(artifactID);
			if (artifactType != ArtifactType.Space)
			{
				if (artifactType == ArtifactType.Terrestrial)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
		}
		if (num > this.analyzedArtifactCount)
		{
			this.analyzedArtifactCount = num;
		}
		if (num2 > this.analyzedSpaceArtifactCount)
		{
			this.analyzedSpaceArtifactCount = num2;
		}
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x000CB068 File Offset: 0x000C9268
	public bool RecordArtifactAnalyzed(string id)
	{
		if (this.analyzedArtifatIDs.Contains(id))
		{
			return false;
		}
		this.analyzedArtifatIDs.Add(id);
		return true;
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x000CB087 File Offset: 0x000C9287
	public void IncrementAnalyzedTerrestrialArtifacts()
	{
		this.analyzedArtifactCount++;
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x000CB097 File Offset: 0x000C9297
	public void IncrementAnalyzedSpaceArtifacts()
	{
		this.analyzedSpaceArtifactCount++;
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x002399D8 File Offset: 0x00237BD8
	public string GetUniqueArtifactID(ArtifactType artifactType = ArtifactType.Any)
	{
		List<string> list = new List<string>();
		foreach (string text in ArtifactConfig.artifactItems[artifactType])
		{
			if (!this.placedArtifacts[artifactType].Contains(text) && Game.IsCorrectDlcActiveForCurrentSave(Assets.GetPrefab(text.ToTag()).GetComponent<KPrefabID>()))
			{
				list.Add(text);
			}
		}
		string text2 = "artifact_officemug";
		if (list.Count == 0 && artifactType != ArtifactType.Any)
		{
			foreach (string text3 in ArtifactConfig.artifactItems[ArtifactType.Any])
			{
				if (!this.placedArtifacts[ArtifactType.Any].Contains(text3) && Game.IsCorrectDlcActiveForCurrentSave(Assets.GetPrefab(text3.ToTag()).GetComponent<KPrefabID>()))
				{
					list.Add(text3);
					artifactType = ArtifactType.Any;
				}
			}
		}
		if (list.Count != 0)
		{
			text2 = list[UnityEngine.Random.Range(0, list.Count)];
		}
		this.placedArtifacts[artifactType].Add(text2);
		return text2;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x000CB0A7 File Offset: 0x000C92A7
	public void ReserveArtifactID(string artifactID, ArtifactType artifactType = ArtifactType.Any)
	{
		if (this.placedArtifacts[artifactType].Contains(artifactID))
		{
			DebugUtil.Assert(true, string.Format("Tried to add {0} to placedArtifacts but it already exists in the list!", artifactID));
		}
		this.placedArtifacts[artifactType].Add(artifactID);
	}

	// Token: 0x06003BC3 RID: 15299 RVA: 0x000CB0E0 File Offset: 0x000C92E0
	public ArtifactType GetArtifactType(string artifactID)
	{
		if (this.placedArtifacts[ArtifactType.Terrestrial].Contains(artifactID))
		{
			return ArtifactType.Terrestrial;
		}
		if (this.placedArtifacts[ArtifactType.Space].Contains(artifactID))
		{
			return ArtifactType.Space;
		}
		return ArtifactType.Any;
	}

	// Token: 0x04002968 RID: 10600
	public static ArtifactSelector Instance;

	// Token: 0x04002969 RID: 10601
	[Serialize]
	private Dictionary<ArtifactType, List<string>> placedArtifacts = new Dictionary<ArtifactType, List<string>>();

	// Token: 0x0400296A RID: 10602
	[Serialize]
	private int analyzedArtifactCount;

	// Token: 0x0400296B RID: 10603
	[Serialize]
	private int analyzedSpaceArtifactCount;

	// Token: 0x0400296C RID: 10604
	[Serialize]
	private List<string> analyzedArtifatIDs = new List<string>();

	// Token: 0x0400296D RID: 10605
	private const string DEFAULT_ARTIFACT_ID = "artifact_officemug";
}
