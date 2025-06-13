using System;
using System.Collections.Generic;
using FuzzySharp;
using STRINGS;

public class FuzzySearch
{
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

	public static string Canonicalize(string s)
	{
		return UI.StripLinkFormatting(UI.StripStyleFormatting(s));
	}

	private static int ScoreImpl_Unchecked(string searchString, string candidate)
	{
		return Fuzz.Ratio(searchString, candidate);
	}

	private static int ScoreImpl(string searchString, string candidate)
	{
		return FuzzySearch.ScoreImpl_Unchecked(searchString, candidate);
	}

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

	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, string[] tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

	public static FuzzySearch.Match ScoreTokens(string searchStringUpper, IReadOnlyList<string> tokens)
	{
		return FuzzySearch.ScoreTokens_Unchecked(searchStringUpper, tokens);
	}

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

	public static FuzzySearch.Match CanonicalizeAndScore(string searchStringUpper, string candidate)
	{
		return FuzzySearch.ScoreCanonicalCandidate(searchStringUpper, FuzzySearch.Canonicalize(candidate).ToUpper(), candidate);
	}

	public const FuzzySearch.Features PHRASE_MUTATION_FEATURES = FuzzySearch.Features.Suppress1And2LetterWords | FuzzySearch.Features.SuppressMeaninglessWords;

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

	[Flags]
	public enum Features
	{
		Suppress1And2LetterWords = 1,
		SuppressMeaninglessWords = 2,
		Initialism = 4
	}

	public struct Match
	{
		public int score;

		public string token;

		public static readonly FuzzySearch.Match NONE = new FuzzySearch.Match
		{
			score = 0,
			token = string.Empty
		};
	}
}
