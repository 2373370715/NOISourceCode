using System;
using System.Collections.Generic;
using FuzzySharp;
using STRINGS;

// Token: 0x02000615 RID: 1557
public class FuzzySearch
{
	// Token: 0x06001B9C RID: 7068 RVA: 0x001B7B38 File Offset: 0x001B5D38
	public static FuzzySearch.Features GetFeatures()
	{
		FuzzySearch.Features features = FuzzySearch.Features.Initialism;
		if (Localization.GetLocale() == null)
		{
			features |= FuzzySearch.Features.Suppress1And2LetterWords;
			features |= FuzzySearch.Features.SuppressMeaninglessWords;
		}
		return features;
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x000B6761 File Offset: 0x000B4961
	public static string Canonicalize(string s)
	{
		return UI.StripLinkFormatting(UI.StripStyleFormatting(s));
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000B676E File Offset: 0x000B496E
	private static int ScoreImpl_Unchecked(string searchString, string candidate)
	{
		return Fuzz.Ratio(searchString, candidate);
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x000B6777 File Offset: 0x000B4977
	private static int ScoreImpl(string searchString, string candidate)
	{
		return FuzzySearch.ScoreImpl_Unchecked(searchString, candidate);
	}

	// Token: 0x06001BA0 RID: 7072 RVA: 0x001B7B58 File Offset: 0x001B5D58
	private static bool IsUpper(string s)
	{
		foreach (char c in s)
		{
			if (char.IsLetter(c) && !char.IsUpper(c))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001BA1 RID: 7073 RVA: 0x001B7B94 File Offset: 0x001B5D94
	private static FuzzySearch.Match ScoreTokens_Unchecked(string searchStringUpper, string[] tokens)
	{
		if (tokens.Length == 0)
		{
			return FuzzySearch.Match.NONE;
		}
		int? num = null;
		string token = null;
		int i = 0;
		while (i < tokens.Length)
		{
			string text = tokens[i];
			int num2 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, text);
			if (num == null)
			{
				goto IL_4A;
			}
			int num3 = num2;
			int? num4 = num;
			if (num3 > num4.GetValueOrDefault() & num4 != null)
			{
				goto IL_4A;
			}
			IL_56:
			i++;
			continue;
			IL_4A:
			num = new int?(num2);
			token = text;
			goto IL_56;
		}
		return new FuzzySearch.Match
		{
			score = num.Value,
			token = token
		};
	}

	// Token: 0x06001BA2 RID: 7074 RVA: 0x001B7C24 File Offset: 0x001B5E24
	private static FuzzySearch.Match ScoreTokens_Unchecked(string searchStringUpper, IReadOnlyList<string> tokens)
	{
		if (tokens.Count == 0)
		{
			return FuzzySearch.Match.NONE;
		}
		int? num = null;
		string token = null;
		foreach (string text in tokens)
		{
			int num2 = FuzzySearch.ScoreImpl_Unchecked(searchStringUpper, text);
			if (num != null)
			{
				int num3 = num2;
				int? num4 = num;
				if (!(num3 > num4.GetValueOrDefault() & num4 != null))
				{
					continue;
				}
			}
			num = new int?(num2);
			token = text;
		}
		return new FuzzySearch.Match
		{
			score = num.Value,
			token = token
		};
	}

	// Token: 0x06001BA3 RID: 7075 RVA: 0x000B6780 File Offset: 0x000B4980
	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, string[] tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

	// Token: 0x06001BA4 RID: 7076 RVA: 0x000B6789 File Offset: 0x000B4989
	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, IReadOnlyList<string> tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

	// Token: 0x06001BA5 RID: 7077 RVA: 0x001B7CD4 File Offset: 0x001B5ED4
	public static FuzzySearch.Match ScoreCanonicalCandidate(string searchStringUpper, string canonicalCandidate, string candidate = null)
	{
		FuzzySearch.Match match = new FuzzySearch.Match
		{
			score = Fuzz.WeightedRatio(searchStringUpper, canonicalCandidate),
			token = (candidate ?? canonicalCandidate)
		};
		if ((FuzzySearch.GetFeatures() & FuzzySearch.Features.Initialism) != (FuzzySearch.Features)0)
		{
			int num = Fuzz.TokenInitialismRatio(searchStringUpper, canonicalCandidate);
			if (num > match.score)
			{
				match.score = num;
			}
		}
		string[] tokens = canonicalCandidate.Split(FuzzySearch.TOKEN_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);
		FuzzySearch.Match match2 = FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
		if (match2.score <= match.score)
		{
			return match;
		}
		return match2;
	}

	// Token: 0x06001BA6 RID: 7078 RVA: 0x000B6792 File Offset: 0x000B4992
	public static FuzzySearch.Match CanonicalizeAndScore(string searchStringUpper, string candidate)
	{
		return FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, FuzzySearch.Canonicalize(candidate).ToUpper(), candidate);
	}

	// Token: 0x040011BA RID: 4538
	public const FuzzySearch.Features PHRASE_MUTATION_FEATURES = FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords;

	// Token: 0x040011BB RID: 4539
	public static readonly char[] TOKEN_SEPARATORS = new char[]
	{
		' ',
		'.',
		'\n',
		',',
		';',
		':',
		'?',
		'!',
		'-',
		'(',
		')',
		'[',
		']',
		'{',
		'}'
	};

	// Token: 0x02000616 RID: 1558
	[Flags]
	public enum Features
	{
		// Token: 0x040011BD RID: 4541
		Suppress1And2LetterWords = 1,
		// Token: 0x040011BE RID: 4542
		SuppressMeaninglessWords = 2,
		// Token: 0x040011BF RID: 4543
		Initialism = 4
	}

	// Token: 0x02000617 RID: 1559
	public struct Match
	{
		// Token: 0x040011C0 RID: 4544
		public int score;

		// Token: 0x040011C1 RID: 4545
		public string token;

		// Token: 0x040011C2 RID: 4546
		public static readonly FuzzySearch.Match NONE = new FuzzySearch.Match
		{
			score = 0,
			token = string.Empty
		};
	}
}
