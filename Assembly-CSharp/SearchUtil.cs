using System;
using System.Collections.Generic;
using System.Text;
using Database;
using STRINGS;

// Token: 0x02001F6E RID: 8046
public static class SearchUtil
{
	// Token: 0x0600A9E5 RID: 43493 RVA: 0x0041339C File Offset: 0x0041159C
	private static void CacheMeaninglessWords()
	{
		if (SearchUtil.MeaninglessWords.Count != 0)
		{
			return;
		}
		ListPool<string, SearchUtil.MatchCache>.PooledList pooledList = ListPool<string, SearchUtil.MatchCache>.Allocate();
		SearchUtil.AddCommaDelimitedSearchTerms(SEARCH_TERMS.SUPPRESSED, pooledList);
		foreach (string item in pooledList)
		{
			SearchUtil.MeaninglessWords.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x0600A9E6 RID: 43494 RVA: 0x00112BD4 File Offset: 0x00110DD4
	public static bool IsPassingScore(int score)
	{
		return score >= 79;
	}

	// Token: 0x0600A9E7 RID: 43495 RVA: 0x00112BDE File Offset: 0x00110DDE
	public static string Canonicalize(string s)
	{
		return FuzzySearch.Canonicalize(s).ToUpper();
	}

	// Token: 0x0600A9E8 RID: 43496 RVA: 0x00413418 File Offset: 0x00411618
	public static string CanonicalizePhrase(string s)
	{
		string text = FuzzySearch.Canonicalize(s).ToUpper();
		FuzzySearch.Features features = FuzzySearch.GetFeatures();
		if ((features & (FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords)) == (FuzzySearch.Features)0)
		{
			return text;
		}
		string[] array = text.Split(FuzzySearch.TOKEN_SEPARATORS);
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = (features & FuzzySearch.Features.Suppress1And2LetterWords) > (FuzzySearch.Features)0;
		bool flag2 = (features & FuzzySearch.Features.SuppressMeaninglessWords) > (FuzzySearch.Features)0;
		if (flag2)
		{
			SearchUtil.CacheMeaninglessWords();
		}
		foreach (string text2 in array)
		{
			if ((!flag || text2.Length > 2) && (!flag2 || !SearchUtil.MeaninglessWords.Contains(text2)))
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendFormat(" {0}", text2);
				}
				else
				{
					stringBuilder.Append(text2);
				}
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A9E9 RID: 43497 RVA: 0x004134D8 File Offset: 0x004116D8
	public static void AddCommaDelimitedSearchTerms(string commaDelimitedSearchTerms, List<string> searchTerms)
	{
		foreach (string item in commaDelimitedSearchTerms.ToUpper().Split(SearchUtil.COMMA_DELIMETERS, StringSplitOptions.RemoveEmptyEntries))
		{
			searchTerms.Add(item);
		}
	}

	// Token: 0x0600A9EA RID: 43498 RVA: 0x00413510 File Offset: 0x00411710
	public static Dictionary<string, SearchUtil.TechCache> CacheTechs()
	{
		Dictionary<string, SearchUtil.TechCache> dictionary = new Dictionary<string, SearchUtil.TechCache>();
		ListPool<ComplexRecipe, SearchUtil.TechCache>.PooledList pooledList = ListPool<ComplexRecipe, SearchUtil.TechCache>.Allocate();
		Techs techs = Db.Get().Techs;
		for (int num = 0; num != techs.Count; num++)
		{
			Tech tech = (Tech)techs.GetResource(num);
			Dictionary<string, SearchUtil.TechItemCache> dictionary2 = new Dictionary<string, SearchUtil.TechItemCache>();
			foreach (TechItem techItem in tech.unlockedItems)
			{
				pooledList.Clear();
				BuildingDef.CollectFabricationRecipes(techItem.Id, pooledList);
				List<SearchUtil.NameDescCache> list = new List<SearchUtil.NameDescCache>();
				foreach (ComplexRecipe complexRecipe in pooledList)
				{
					list.Add(new SearchUtil.NameDescCache
					{
						name = new SearchUtil.MatchCache
						{
							text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
						},
						desc = new SearchUtil.MatchCache
						{
							text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
						}
					});
				}
				TechItem techItem2 = Db.Get().TechItems.Get(techItem.Id);
				SearchUtil.TechItemCache value = new SearchUtil.TechItemCache
				{
					nameDescSearchTerms = new SearchUtil.NameDescSearchTermsCache
					{
						nameDesc = new SearchUtil.NameDescCache
						{
							name = new SearchUtil.MatchCache
							{
								text = SearchUtil.Canonicalize(techItem2.Name)
							},
							desc = new SearchUtil.MatchCache
							{
								text = SearchUtil.CanonicalizePhrase(techItem2.description)
							}
						},
						searchTerms = techItem2.searchTerms
					},
					recipes = list
				};
				dictionary2[techItem.Id] = value;
			}
			SearchUtil.TechCache value2 = new SearchUtil.TechCache
			{
				tech = new SearchUtil.NameDescSearchTermsCache
				{
					nameDesc = new SearchUtil.NameDescCache
					{
						name = new SearchUtil.MatchCache
						{
							text = SearchUtil.Canonicalize(tech.Name)
						},
						desc = new SearchUtil.MatchCache
						{
							text = SearchUtil.CanonicalizePhrase(tech.desc)
						}
					},
					searchTerms = tech.searchTerms
				},
				techItems = dictionary2
			};
			dictionary[tech.Id] = value2;
		}
		pooledList.Recycle();
		return dictionary;
	}

	// Token: 0x0600A9EB RID: 43499 RVA: 0x00413774 File Offset: 0x00411974
	public static SearchUtil.BuildingDefCache MakeBuildingDefCache(BuildingDef def)
	{
		SearchUtil.NameDescSearchTermsCache nameDescSearchTerms = new SearchUtil.NameDescSearchTermsCache
		{
			nameDesc = new SearchUtil.NameDescCache
			{
				name = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(def.Name)
				},
				desc = new SearchUtil.MatchCache
				{
					text = SearchUtil.CanonicalizePhrase(def.Desc)
				}
			},
			searchTerms = def.SearchTerms
		};
		SearchUtil.MatchCache effect = new SearchUtil.MatchCache
		{
			text = SearchUtil.CanonicalizePhrase(def.Effect)
		};
		List<SearchUtil.NameDescCache> list = new List<SearchUtil.NameDescCache>();
		ListPool<ComplexRecipe, PlanBuildingToggle>.PooledList pooledList = ListPool<ComplexRecipe, PlanBuildingToggle>.Allocate();
		BuildingDef.CollectFabricationRecipes(def.PrefabID, pooledList);
		foreach (ComplexRecipe complexRecipe in pooledList)
		{
			list.Add(new SearchUtil.NameDescCache
			{
				name = new SearchUtil.MatchCache
				{
					text = SearchUtil.Canonicalize(complexRecipe.GetUIName(false))
				},
				desc = new SearchUtil.MatchCache
				{
					text = SearchUtil.CanonicalizePhrase(complexRecipe.description)
				}
			});
		}
		pooledList.Recycle();
		return new SearchUtil.BuildingDefCache
		{
			nameDescSearchTerms = nameDescSearchTerms,
			effect = effect,
			recipes = list
		};
	}

	// Token: 0x040085D0 RID: 34256
	public const int MATCH_SCORE_MIN = 0;

	// Token: 0x040085D1 RID: 34257
	public const int MATCH_SCORE_MAX = 100;

	// Token: 0x040085D2 RID: 34258
	public const int MATCH_SCORE_THRESHOLD = 79;

	// Token: 0x040085D3 RID: 34259
	private static readonly HashSet<string> MeaninglessWords = new HashSet<string>();

	// Token: 0x040085D4 RID: 34260
	private static readonly char[] COMMA_DELIMETERS = new char[]
	{
		' ',
		','
	};

	// Token: 0x040085D5 RID: 34261
	private const int LHS_GT_RHS = -1;

	// Token: 0x040085D6 RID: 34262
	private const int RHS_GT_LHS = 1;

	// Token: 0x02001F6F RID: 8047
	private interface IScore
	{
		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x0600A9ED RID: 43501
		int Score { get; }
	}

	// Token: 0x02001F70 RID: 8048
	private struct TieBreaker
	{
		// Token: 0x0600A9EE RID: 43502 RVA: 0x00112C0C File Offset: 0x00110E0C
		public TieBreaker(int _globalMax)
		{
			this.globalMax = _globalMax;
			this.globalMaxCmp = 0;
			this.localMaxScore = -1;
			this.localMaxCmp = 0;
		}

		// Token: 0x0600A9EF RID: 43503 RVA: 0x00112C2A File Offset: 0x00110E2A
		private int CacheLocalScore(int score, int cmp)
		{
			if (this.localMaxScore == -1 || this.localMaxScore < score)
			{
				this.localMaxScore = score;
				this.localMaxCmp = cmp;
			}
			return this.localMaxCmp;
		}

		// Token: 0x0600A9F0 RID: 43504 RVA: 0x00112C52 File Offset: 0x00110E52
		private int CacheScore(int score, int cmp)
		{
			if (score == this.globalMax)
			{
				this.globalMaxCmp = cmp;
				return this.globalMaxCmp;
			}
			return this.CacheLocalScore(score, cmp);
		}

		// Token: 0x0600A9F1 RID: 43505 RVA: 0x004138AC File Offset: 0x00411AAC
		public int Consider(int lhs, int rhs)
		{
			if (this.globalMaxCmp != 0)
			{
				return this.globalMaxCmp;
			}
			switch (-lhs.CompareTo(rhs))
			{
			case -1:
				return this.CacheScore(lhs, -1);
			case 0:
				if (this.localMaxScore != -1)
				{
					return this.localMaxCmp;
				}
				return 0;
			case 1:
				return this.CacheScore(rhs, 1);
			default:
				Debug.Assert(false);
				return 0;
			}
		}

		// Token: 0x0600A9F2 RID: 43506 RVA: 0x00413914 File Offset: 0x00411B14
		public int Consider<T>(T lhs, T rhs) where T : IComparable, SearchUtil.IScore
		{
			if (this.globalMaxCmp != 0)
			{
				return this.globalMaxCmp;
			}
			if (lhs == null)
			{
				if (rhs != null)
				{
					return this.CacheScore(rhs.Score, 1);
				}
				if (this.localMaxScore != -1)
				{
					return this.localMaxCmp;
				}
				return 0;
			}
			else
			{
				if (rhs == null)
				{
					return this.CacheScore(lhs.Score, -1);
				}
				switch (lhs.CompareTo(rhs))
				{
				case -1:
					return this.CacheScore(lhs.Score, -1);
				case 0:
					if (this.localMaxScore != -1)
					{
						return this.localMaxCmp;
					}
					return 0;
				case 1:
					return this.CacheScore(rhs.Score, 1);
				default:
					Debug.Assert(false);
					return 0;
				}
			}
		}

		// Token: 0x040085D7 RID: 34263
		private readonly int globalMax;

		// Token: 0x040085D8 RID: 34264
		private int globalMaxCmp;

		// Token: 0x040085D9 RID: 34265
		private int localMaxScore;

		// Token: 0x040085DA RID: 34266
		private int localMaxCmp;
	}

	// Token: 0x02001F71 RID: 8049
	public class MatchCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x0600A9F3 RID: 43507 RVA: 0x00112C73 File Offset: 0x00110E73
		public int Score
		{
			get
			{
				return this.FuzzyMatch.score;
			}
		}

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x0600A9F4 RID: 43508 RVA: 0x00112C80 File Offset: 0x00110E80
		// (set) Token: 0x0600A9F5 RID: 43509 RVA: 0x00112C88 File Offset: 0x00110E88
		public FuzzySearch.Match FuzzyMatch { get; private set; }

		// Token: 0x0600A9F6 RID: 43510 RVA: 0x004139F4 File Offset: 0x00411BF4
		public void Bind(string searchStringUpper)
		{
			try
			{
				this.FuzzyMatch = FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, this.text, null);
			}
			catch (Exception innerException)
			{
				throw new Exception("searchStringUpper: " + searchStringUpper + ", text: " + this.text, innerException);
			}
		}

		// Token: 0x0600A9F7 RID: 43511 RVA: 0x00112C91 File Offset: 0x00110E91
		public void Reset()
		{
			this.FuzzyMatch = FuzzySearch.Match.NONE;
		}

		// Token: 0x0600A9F8 RID: 43512 RVA: 0x00413A44 File Offset: 0x00411C44
		public int CompareTo(object obj)
		{
			SearchUtil.MatchCache matchCache = (SearchUtil.MatchCache)obj;
			return -this.Score.CompareTo(matchCache.Score);
		}

		// Token: 0x040085DB RID: 34267
		public string text;
	}

	// Token: 0x02001F72 RID: 8050
	public class NameDescCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x0600A9FA RID: 43514 RVA: 0x00112C9E File Offset: 0x00110E9E
		public void Bind(string searchStringUpper)
		{
			this.name.Bind(searchStringUpper);
			this.desc.Bind(searchStringUpper);
		}

		// Token: 0x0600A9FB RID: 43515 RVA: 0x00112CB8 File Offset: 0x00110EB8
		public void Reset()
		{
			this.name.Reset();
			this.desc.Reset();
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x0600A9FC RID: 43516 RVA: 0x00112CD0 File Offset: 0x00110ED0
		public int Score
		{
			get
			{
				return Math.Max(this.name.Score, this.desc.Score);
			}
		}

		// Token: 0x0600A9FD RID: 43517 RVA: 0x00413A70 File Offset: 0x00411C70
		public int CompareTo(object obj)
		{
			SearchUtil.NameDescCache nameDescCache = (SearchUtil.NameDescCache)obj;
			int score = this.Score;
			int score2 = nameDescCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.name, nameDescCache.name);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.desc, nameDescCache.desc);
		}

		// Token: 0x040085DD RID: 34269
		public SearchUtil.MatchCache name;

		// Token: 0x040085DE RID: 34270
		public SearchUtil.MatchCache desc;
	}

	// Token: 0x02001F73 RID: 8051
	public class NameDescSearchTermsCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x0600A9FF RID: 43519 RVA: 0x00112CED File Offset: 0x00110EED
		// (set) Token: 0x0600AA00 RID: 43520 RVA: 0x00112CF5 File Offset: 0x00110EF5
		public FuzzySearch.Match SearchTermsScore { get; private set; }

		// Token: 0x0600AA01 RID: 43521 RVA: 0x00112CFE File Offset: 0x00110EFE
		public void Bind(string searchStringUpper)
		{
			this.nameDesc.Bind(searchStringUpper);
			this.SearchTermsScore = FuzzySearch.ScoreTokens(searchStringUpper, this.searchTerms);
		}

		// Token: 0x0600AA02 RID: 43522 RVA: 0x00112D1E File Offset: 0x00110F1E
		public void Reset()
		{
			this.nameDesc.Reset();
			this.SearchTermsScore = FuzzySearch.Match.NONE;
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x0600AA03 RID: 43523 RVA: 0x00112D36 File Offset: 0x00110F36
		public int Score
		{
			get
			{
				return Math.Max(this.nameDesc.Score, this.SearchTermsScore.score);
			}
		}

		// Token: 0x0600AA04 RID: 43524 RVA: 0x00112D53 File Offset: 0x00110F53
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600AA05 RID: 43525 RVA: 0x00413AD0 File Offset: 0x00411CD0
		public int CompareTo(object obj)
		{
			SearchUtil.NameDescSearchTermsCache nameDescSearchTermsCache = (SearchUtil.NameDescSearchTermsCache)obj;
			int score = this.Score;
			int score2 = nameDescSearchTermsCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.name, nameDescSearchTermsCache.nameDesc.name);
			tieBreaker.Consider(this.SearchTermsScore.score, nameDescSearchTermsCache.SearchTermsScore.score);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDesc.desc, nameDescSearchTermsCache.nameDesc.desc);
		}

		// Token: 0x040085DF RID: 34271
		public SearchUtil.NameDescCache nameDesc;

		// Token: 0x040085E0 RID: 34272
		public IReadOnlyList<string> searchTerms;
	}

	// Token: 0x02001F74 RID: 8052
	public class BuildingDefCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x0600AA07 RID: 43527 RVA: 0x00112D62 File Offset: 0x00110F62
		// (set) Token: 0x0600AA08 RID: 43528 RVA: 0x00112D6A File Offset: 0x00110F6A
		public SearchUtil.NameDescCache BestRecipe { get; private set; }

		// Token: 0x0600AA09 RID: 43529 RVA: 0x00413B64 File Offset: 0x00411D64
		public void Bind(string searchStringUpper)
		{
			this.nameDescSearchTerms.Bind(searchStringUpper);
			this.effect.Bind(searchStringUpper);
			this.BestRecipe = null;
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Bind(searchStringUpper);
				if (this.BestRecipe == null || nameDescCache.CompareTo(this.BestRecipe) == -1)
				{
					this.BestRecipe = nameDescCache;
				}
			}
		}

		// Token: 0x0600AA0A RID: 43530 RVA: 0x00413BF4 File Offset: 0x00411DF4
		public void Reset()
		{
			this.nameDescSearchTerms.Reset();
			this.effect.Reset();
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Reset();
			}
			this.BestRecipe = null;
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600AA0B RID: 43531 RVA: 0x00112D73 File Offset: 0x00110F73
		public int Score
		{
			get
			{
				return Math.Max(this.nameDescSearchTerms.Score, Math.Max(this.effect.Score, (this.BestRecipe == null) ? 0 : this.BestRecipe.Score));
			}
		}

		// Token: 0x0600AA0C RID: 43532 RVA: 0x00112DAB File Offset: 0x00110FAB
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600AA0D RID: 43533 RVA: 0x00413C64 File Offset: 0x00411E64
		public int CompareTo(object obj)
		{
			SearchUtil.BuildingDefCache buildingDefCache = (SearchUtil.BuildingDefCache)obj;
			int score = this.Score;
			int score2 = buildingDefCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.name, buildingDefCache.nameDescSearchTerms.nameDesc.name);
			tieBreaker.Consider(this.nameDescSearchTerms.SearchTermsScore.score, buildingDefCache.nameDescSearchTerms.SearchTermsScore.score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.effect, buildingDefCache.effect);
			return tieBreaker.Consider<SearchUtil.MatchCache>(this.nameDescSearchTerms.nameDesc.desc, buildingDefCache.nameDescSearchTerms.nameDesc.desc);
		}

		// Token: 0x040085E2 RID: 34274
		public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;

		// Token: 0x040085E3 RID: 34275
		public SearchUtil.MatchCache effect;

		// Token: 0x040085E4 RID: 34276
		public List<SearchUtil.NameDescCache> recipes;
	}

	// Token: 0x02001F75 RID: 8053
	public class TechItemCache : IComparable, SearchUtil.IScore
	{
		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x0600AA0F RID: 43535 RVA: 0x00112DBA File Offset: 0x00110FBA
		// (set) Token: 0x0600AA10 RID: 43536 RVA: 0x00112DC2 File Offset: 0x00110FC2
		public SearchUtil.NameDescCache BestRecipe { get; private set; }

		// Token: 0x0600AA11 RID: 43537 RVA: 0x00413D28 File Offset: 0x00411F28
		public void Bind(string searchStringUpper)
		{
			this.nameDescSearchTerms.Bind(searchStringUpper);
			this.BestRecipe = null;
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Bind(searchStringUpper);
				if (this.BestRecipe == null || nameDescCache.CompareTo(this.BestRecipe) == -1)
				{
					this.BestRecipe = nameDescCache;
				}
			}
		}

		// Token: 0x0600AA12 RID: 43538 RVA: 0x00413DAC File Offset: 0x00411FAC
		public void Reset()
		{
			this.nameDescSearchTerms.Reset();
			foreach (SearchUtil.NameDescCache nameDescCache in this.recipes)
			{
				nameDescCache.Reset();
			}
			this.BestRecipe = null;
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x0600AA13 RID: 43539 RVA: 0x00112DCB File Offset: 0x00110FCB
		public int Score
		{
			get
			{
				return Math.Max(this.nameDescSearchTerms.Score, (this.BestRecipe == null) ? 0 : this.BestRecipe.Score);
			}
		}

		// Token: 0x0600AA14 RID: 43540 RVA: 0x00112DF3 File Offset: 0x00110FF3
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600AA15 RID: 43541 RVA: 0x00413E10 File Offset: 0x00412010
		public int CompareTo(object obj)
		{
			SearchUtil.TechItemCache techItemCache = (SearchUtil.TechItemCache)obj;
			int score = this.Score;
			int score2 = techItemCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.NameDescSearchTermsCache>(this.nameDescSearchTerms, techItemCache.nameDescSearchTerms);
			return tieBreaker.Consider<SearchUtil.NameDescCache>(this.BestRecipe, techItemCache.BestRecipe);
		}

		// Token: 0x040085E6 RID: 34278
		public SearchUtil.NameDescSearchTermsCache nameDescSearchTerms;

		// Token: 0x040085E7 RID: 34279
		public List<SearchUtil.NameDescCache> recipes;
	}

	// Token: 0x02001F76 RID: 8054
	public class TechCache : IComparable
	{
		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x0600AA17 RID: 43543 RVA: 0x00112E02 File Offset: 0x00111002
		// (set) Token: 0x0600AA18 RID: 43544 RVA: 0x00112E0A File Offset: 0x0011100A
		public SearchUtil.TechItemCache BestItem { get; private set; }

		// Token: 0x0600AA19 RID: 43545 RVA: 0x00413E70 File Offset: 0x00412070
		public void Bind(string searchStringUpper)
		{
			this.tech.Bind(searchStringUpper);
			this.BestItem = null;
			foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in this.techItems)
			{
				keyValuePair.Value.Bind(searchStringUpper);
				if (this.BestItem == null || keyValuePair.Value.CompareTo(this.BestItem) == -1)
				{
					this.BestItem = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0600AA1A RID: 43546 RVA: 0x00413F08 File Offset: 0x00412108
		public void Reset()
		{
			this.tech.Reset();
			foreach (KeyValuePair<string, SearchUtil.TechItemCache> keyValuePair in this.techItems)
			{
				keyValuePair.Value.Reset();
			}
			this.BestItem = null;
		}

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x0600AA1B RID: 43547 RVA: 0x00112E13 File Offset: 0x00111013
		public int Score
		{
			get
			{
				return Math.Max(this.tech.Score, (this.BestItem == null) ? 0 : this.BestItem.Score);
			}
		}

		// Token: 0x0600AA1C RID: 43548 RVA: 0x00112E3B File Offset: 0x0011103B
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600AA1D RID: 43549 RVA: 0x00413F74 File Offset: 0x00412174
		public int CompareTo(object obj)
		{
			SearchUtil.TechCache techCache = (SearchUtil.TechCache)obj;
			int score = this.Score;
			int score2 = techCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.NameDescSearchTermsCache>(this.tech, techCache.tech);
			return tieBreaker.Consider<SearchUtil.TechItemCache>(this.BestItem, techCache.BestItem);
		}

		// Token: 0x040085E9 RID: 34281
		public SearchUtil.NameDescSearchTermsCache tech;

		// Token: 0x040085EA RID: 34282
		public Dictionary<string, SearchUtil.TechItemCache> techItems;
	}

	// Token: 0x02001F77 RID: 8055
	public class SubcategoryCache : IComparable
	{
		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x0600AA1F RID: 43551 RVA: 0x00112E4A File Offset: 0x0011104A
		// (set) Token: 0x0600AA20 RID: 43552 RVA: 0x00112E52 File Offset: 0x00111052
		public SearchUtil.BuildingDefCache BestBuildingDef { get; private set; }

		// Token: 0x0600AA21 RID: 43553 RVA: 0x00413FD4 File Offset: 0x004121D4
		public void Bind(string searchStringUpper)
		{
			this.subcategory.Bind(searchStringUpper);
			this.BestBuildingDef = null;
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				buildingDefCache.Bind(searchStringUpper);
				if (this.BestBuildingDef == null || buildingDefCache.CompareTo(this.BestBuildingDef) == -1)
				{
					this.BestBuildingDef = buildingDefCache;
				}
			}
		}

		// Token: 0x0600AA22 RID: 43554 RVA: 0x00414058 File Offset: 0x00412258
		public void Reset()
		{
			this.subcategory.Reset();
			foreach (SearchUtil.BuildingDefCache buildingDefCache in this.buildingDefs)
			{
				buildingDefCache.Reset();
			}
			this.BestBuildingDef = null;
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x0600AA23 RID: 43555 RVA: 0x00112E5B File Offset: 0x0011105B
		public int Score
		{
			get
			{
				return Math.Max(this.subcategory.Score, (this.BestBuildingDef == null) ? 0 : this.BestBuildingDef.Score);
			}
		}

		// Token: 0x0600AA24 RID: 43556 RVA: 0x00112E83 File Offset: 0x00111083
		public bool IsPassingScore()
		{
			return this.Score >= 79;
		}

		// Token: 0x0600AA25 RID: 43557 RVA: 0x004140BC File Offset: 0x004122BC
		public int CompareTo(object obj)
		{
			SearchUtil.SubcategoryCache subcategoryCache = (SearchUtil.SubcategoryCache)obj;
			int score = this.Score;
			int score2 = subcategoryCache.Score;
			int num = -score.CompareTo(score2);
			if (num != 0)
			{
				return num;
			}
			SearchUtil.TieBreaker tieBreaker = new SearchUtil.TieBreaker(score);
			tieBreaker.Consider<SearchUtil.MatchCache>(this.subcategory, subcategoryCache.subcategory);
			return tieBreaker.Consider<SearchUtil.BuildingDefCache>(this.BestBuildingDef, subcategoryCache.BestBuildingDef);
		}

		// Token: 0x040085EC RID: 34284
		public SearchUtil.MatchCache subcategory;

		// Token: 0x040085ED RID: 34285
		public HashSet<SearchUtil.BuildingDefCache> buildingDefs;
	}
}
