using System;
using UnityEngine;

// Token: 0x02000964 RID: 2404
public static class SymbolOverrideControllerUtil
{
	// Token: 0x06002AF7 RID: 10999 RVA: 0x000C0609 File Offset: 0x000BE809
	public static SymbolOverrideController AddToPrefab(GameObject prefab)
	{
		SymbolOverrideController result = prefab.AddComponent<SymbolOverrideController>();
		KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
		DebugUtil.Assert(component != null, "SymbolOverrideController must be added after a KBatchedAnimController component.");
		component.usingNewSymbolOverrideSystem = true;
		return result;
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x001E95A4 File Offset: 0x001E77A4
	public static void AddBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.AddSymbolOverride(new HashedString(symbol.hash.HashValue), symbol, priority);
		}
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x001E95F0 File Offset: 0x001E77F0
	public static void RemoveBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.RemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
		}
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x001E963C File Offset: 0x001E783C
	public static void TryRemoveBuildOverride(this SymbolOverrideController symbol_override_controller, KAnimFileData anim_file_data, int priority = 0)
	{
		for (int i = 0; i < anim_file_data.build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file_data.build.symbols[i];
			symbol_override_controller.TryRemoveSymbolOverride(new HashedString(symbol.hash.HashValue), priority);
		}
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000C062E File Offset: 0x000BE82E
	public static bool TryRemoveSymbolOverride(this SymbolOverrideController symbol_override_controller, HashedString target_symbol, int priority = 0)
	{
		return symbol_override_controller.GetSymbolOverrideIdx(target_symbol, priority) >= 0 && symbol_override_controller.RemoveSymbolOverride(target_symbol, priority);
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x001E9688 File Offset: 0x001E7888
	public static void ApplySymbolOverridesByAffix(this SymbolOverrideController symbol_override_controller, KAnimFile anim_file, string prefix = null, string postfix = null, int priority = 0)
	{
		for (int i = 0; i < anim_file.GetData().build.symbols.Length; i++)
		{
			KAnim.Build.Symbol symbol = anim_file.GetData().build.symbols[i];
			string text = HashCache.Get().Get(symbol.hash);
			if (prefix != null && postfix != null)
			{
				if (text.StartsWith(prefix) && text.EndsWith(postfix))
				{
					string text2 = text.Substring(prefix.Length, text.Length - prefix.Length);
					text2 = text2.Substring(0, text2.Length - postfix.Length);
					symbol_override_controller.AddSymbolOverride(text2, symbol, priority);
				}
			}
			else if (prefix != null && text.StartsWith(prefix))
			{
				symbol_override_controller.AddSymbolOverride(text.Substring(prefix.Length, text.Length - prefix.Length), symbol, priority);
			}
			else if (postfix != null && text.EndsWith(postfix))
			{
				symbol_override_controller.AddSymbolOverride(text.Substring(0, text.Length - postfix.Length), symbol, priority);
			}
		}
	}
}
