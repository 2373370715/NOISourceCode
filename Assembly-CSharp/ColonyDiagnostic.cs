using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200125E RID: 4702
public abstract class ColonyDiagnostic : ISim4000ms, IHasDlcRestrictions
{
	// Token: 0x06006009 RID: 24585 RVA: 0x000E313E File Offset: 0x000E133E
	public GameObject GetNextClickThroughObject()
	{
		if (this.aggregatedUniqueClickThroughObjects.Count == 0)
		{
			return null;
		}
		this.clickThroughIndex = (this.clickThroughIndex + 1) % this.aggregatedUniqueClickThroughObjects.Count;
		return this.aggregatedUniqueClickThroughObjects[this.clickThroughIndex];
	}

	// Token: 0x0600600A RID: 24586 RVA: 0x002B958C File Offset: 0x002B778C
	public ColonyDiagnostic(int worldID, string name)
	{
		this.worldID = worldID;
		this.name = name;
		this.id = base.GetType().Name;
		this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
		this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
		SimAndRenderScheduler.instance.Add(this, true);
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x0600600B RID: 24587 RVA: 0x000E317A File Offset: 0x000E137A
	// (set) Token: 0x0600600C RID: 24588 RVA: 0x000E3182 File Offset: 0x000E1382
	public int worldID { get; protected set; }

	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x0600600D RID: 24589 RVA: 0x000E318B File Offset: 0x000E138B
	// (set) Token: 0x0600600E RID: 24590 RVA: 0x000E3193 File Offset: 0x000E1393
	public bool IsWorldModuleInterior { get; private set; }

	// Token: 0x0600600F RID: 24591 RVA: 0x000C550D File Offset: 0x000C370D
	public void OnCleanUp()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x06006010 RID: 24592 RVA: 0x000E319C File Offset: 0x000E139C
	public void Sim4000ms(float dt)
	{
		this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
	}

	// Token: 0x06006011 RID: 24593 RVA: 0x002B96C4 File Offset: 0x002B78C4
	public DiagnosticCriterion[] GetCriteria()
	{
		DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
		this.criteria.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06006012 RID: 24594 RVA: 0x000E31B8 File Offset: 0x000E13B8
	// (set) Token: 0x06006013 RID: 24595 RVA: 0x000E31C0 File Offset: 0x000E13C0
	public ColonyDiagnostic.DiagnosticResult LatestResult
	{
		get
		{
			return this.latestResult;
		}
		private set
		{
			this.latestResult = value;
		}
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x000E31C9 File Offset: 0x000E13C9
	public virtual string GetAverageValueString()
	{
		if (this.tracker != null)
		{
			return this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)));
		}
		return "";
	}

	// Token: 0x06006015 RID: 24597 RVA: 0x000CBEB9 File Offset: 0x000CA0B9
	public virtual string GetCurrentValueString()
	{
		return "";
	}

	// Token: 0x06006016 RID: 24598 RVA: 0x000E31FA File Offset: 0x000E13FA
	protected void AddCriterion(string id, DiagnosticCriterion criterion)
	{
		if (!this.criteria.ContainsKey(id))
		{
			criterion.SetID(id);
			this.criteria.Add(id, criterion);
		}
	}

	// Token: 0x06006017 RID: 24599 RVA: 0x002B96FC File Offset: 0x002B78FC
	public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "", null);
		bool flag = false;
		if (!ClusterManager.Instance.GetWorld(this.worldID).IsDiscovered)
		{
			return diagnosticResult;
		}
		this.aggregatedUniqueClickThroughObjects.Clear();
		foreach (KeyValuePair<string, DiagnosticCriterion> keyValuePair in this.criteria)
		{
			if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, keyValuePair.Key))
			{
				ColonyDiagnostic.DiagnosticResult diagnosticResult2 = keyValuePair.Value.Evaluate();
				if (diagnosticResult2.opinion < diagnosticResult.opinion || (!flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal))
				{
					flag = true;
					diagnosticResult.opinion = diagnosticResult2.opinion;
					diagnosticResult.Message = diagnosticResult2.Message;
					diagnosticResult.clickThroughTarget = diagnosticResult2.clickThroughTarget;
					if (diagnosticResult2.clickThroughObjects != null)
					{
						foreach (GameObject item in diagnosticResult2.clickThroughObjects)
						{
							if (!this.aggregatedUniqueClickThroughObjects.Contains(item))
							{
								this.aggregatedUniqueClickThroughObjects.Add(item);
							}
						}
					}
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06006018 RID: 24600 RVA: 0x000E321E File Offset: 0x000E141E
	public void SetResult(ColonyDiagnostic.DiagnosticResult result)
	{
		this.LatestResult = result;
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x06006019 RID: 24601 RVA: 0x000E3227 File Offset: 0x000E1427
	protected string NO_MINIONS
	{
		get
		{
			return this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		}
	}

	// Token: 0x0600601A RID: 24602 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual string[] GetRequiredDlcIds()
	{
		return null;
	}

	// Token: 0x0600601B RID: 24603 RVA: 0x000AA765 File Offset: 0x000A8965
	public virtual string[] GetForbiddenDlcIds()
	{
		return null;
	}

	// Token: 0x040044C8 RID: 17608
	private int clickThroughIndex;

	// Token: 0x040044C9 RID: 17609
	private List<GameObject> aggregatedUniqueClickThroughObjects = new List<GameObject>();

	// Token: 0x040044CB RID: 17611
	public string name;

	// Token: 0x040044CC RID: 17612
	public string id;

	// Token: 0x040044CE RID: 17614
	public string icon = "icon_errand_operate";

	// Token: 0x040044CF RID: 17615
	private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();

	// Token: 0x040044D0 RID: 17616
	public ColonyDiagnostic.PresentationSetting presentationSetting;

	// Token: 0x040044D1 RID: 17617
	private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x040044D2 RID: 17618
	public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();

	// Token: 0x040044D3 RID: 17619
	public Tracker tracker;

	// Token: 0x040044D4 RID: 17620
	protected float trackerSampleCountSeconds = 4f;

	// Token: 0x0200125F RID: 4703
	public enum PresentationSetting
	{
		// Token: 0x040044D6 RID: 17622
		AverageValue,
		// Token: 0x040044D7 RID: 17623
		CurrentValue
	}

	// Token: 0x02001260 RID: 4704
	public struct DiagnosticResult
	{
		// Token: 0x0600601C RID: 24604 RVA: 0x000E3242 File Offset: 0x000E1442
		public DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion opinion, string message, global::Tuple<Vector3, GameObject> clickThroughTarget = null)
		{
			this.message = message;
			this.opinion = opinion;
			this.clickThroughTarget = null;
			this.clickThroughObjects = null;
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x0600601E RID: 24606 RVA: 0x000E3269 File Offset: 0x000E1469
		// (set) Token: 0x0600601D RID: 24605 RVA: 0x000E3260 File Offset: 0x000E1460
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x002B9864 File Offset: 0x002B7A64
		public string GetFormattedMessage()
		{
			switch (this.opinion)
			{
			case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WARNING_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
			case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WHITE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.POSITIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			}
			return this.message;
		}

		// Token: 0x040044D8 RID: 17624
		public ColonyDiagnostic.DiagnosticResult.Opinion opinion;

		// Token: 0x040044D9 RID: 17625
		public global::Tuple<Vector3, GameObject> clickThroughTarget;

		// Token: 0x040044DA RID: 17626
		public List<GameObject> clickThroughObjects;

		// Token: 0x040044DB RID: 17627
		private string message;

		// Token: 0x02001261 RID: 4705
		public enum Opinion
		{
			// Token: 0x040044DD RID: 17629
			Unset,
			// Token: 0x040044DE RID: 17630
			DuplicantThreatening,
			// Token: 0x040044DF RID: 17631
			Bad,
			// Token: 0x040044E0 RID: 17632
			Warning,
			// Token: 0x040044E1 RID: 17633
			Concern,
			// Token: 0x040044E2 RID: 17634
			Suggestion,
			// Token: 0x040044E3 RID: 17635
			Tutorial,
			// Token: 0x040044E4 RID: 17636
			Normal,
			// Token: 0x040044E5 RID: 17637
			Good
		}
	}
}
