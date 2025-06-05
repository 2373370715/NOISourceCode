using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B3F RID: 2879
[AddComponentMenu("KMonoBehaviour/scripts/SnapOn")]
public class SnapOn : KMonoBehaviour
{
	// Token: 0x0600356A RID: 13674 RVA: 0x000C74B6 File Offset: 0x000C56B6
	protected override void OnPrefabInit()
	{
		this.kanimController = base.GetComponent<KAnimControllerBase>();
	}

	// Token: 0x0600356B RID: 13675 RVA: 0x0021B66C File Offset: 0x0021986C
	protected override void OnSpawn()
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.automatic)
			{
				this.DoAttachSnapOn(snapPoint);
			}
		}
	}

	// Token: 0x0600356C RID: 13676 RVA: 0x0021B6C8 File Offset: 0x002198C8
	public void AttachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					this.DoAttachSnapOn(snapPoint);
				}
			}
		}
	}

	// Token: 0x0600356D RID: 13677 RVA: 0x0021B75C File Offset: 0x0021995C
	public void DetachSnapOnByName(string name)
	{
		foreach (SnapOn.SnapPoint snapPoint in this.snapPoints)
		{
			if (snapPoint.pointName == name)
			{
				HashedString context = base.GetComponent<AnimEventHandler>().GetContext();
				if (!context.IsValid || !snapPoint.context.IsValid || context == snapPoint.context)
				{
					base.GetComponent<SymbolOverrideController>().RemoveSymbolOverride(snapPoint.overrideSymbol, 5);
					this.kanimController.SetSymbolVisiblity(snapPoint.overrideSymbol, false);
					break;
				}
			}
		}
	}

	// Token: 0x0600356E RID: 13678 RVA: 0x0021B814 File Offset: 0x00219A14
	private void DoAttachSnapOn(SnapOn.SnapPoint point)
	{
		SnapOn.OverrideEntry overrideEntry = null;
		KAnimFile buildFile = point.buildFile;
		string symbol_name = "";
		if (this.overrideMap.TryGetValue(point.pointName, out overrideEntry))
		{
			buildFile = overrideEntry.buildFile;
			symbol_name = overrideEntry.symbolName;
		}
		KAnim.Build.Symbol symbol = SnapOn.GetSymbol(buildFile, symbol_name);
		base.GetComponent<SymbolOverrideController>().AddSymbolOverride(point.overrideSymbol, symbol, 5);
		this.kanimController.SetSymbolVisiblity(point.overrideSymbol, true);
	}

	// Token: 0x0600356F RID: 13679 RVA: 0x0021B888 File Offset: 0x00219A88
	private static KAnim.Build.Symbol GetSymbol(KAnimFile anim_file, string symbol_name)
	{
		KAnim.Build.Symbol result = anim_file.GetData().build.symbols[0];
		KAnimHashedString y = new KAnimHashedString(symbol_name);
		foreach (KAnim.Build.Symbol symbol in anim_file.GetData().build.symbols)
		{
			if (symbol.hash == y)
			{
				result = symbol;
				break;
			}
		}
		return result;
	}

	// Token: 0x06003570 RID: 13680 RVA: 0x000C74C4 File Offset: 0x000C56C4
	public void AddOverride(string point_name, KAnimFile build_override, string symbol_name)
	{
		this.overrideMap[point_name] = new SnapOn.OverrideEntry
		{
			buildFile = build_override,
			symbolName = symbol_name
		};
	}

	// Token: 0x06003571 RID: 13681 RVA: 0x000C74E5 File Offset: 0x000C56E5
	public void RemoveOverride(string point_name)
	{
		this.overrideMap.Remove(point_name);
	}

	// Token: 0x040024E4 RID: 9444
	private KAnimControllerBase kanimController;

	// Token: 0x040024E5 RID: 9445
	public List<SnapOn.SnapPoint> snapPoints = new List<SnapOn.SnapPoint>();

	// Token: 0x040024E6 RID: 9446
	private Dictionary<string, SnapOn.OverrideEntry> overrideMap = new Dictionary<string, SnapOn.OverrideEntry>();

	// Token: 0x02000B40 RID: 2880
	[Serializable]
	public class SnapPoint
	{
		// Token: 0x040024E7 RID: 9447
		public string pointName;

		// Token: 0x040024E8 RID: 9448
		public bool automatic = true;

		// Token: 0x040024E9 RID: 9449
		public HashedString context;

		// Token: 0x040024EA RID: 9450
		public KAnimFile buildFile;

		// Token: 0x040024EB RID: 9451
		public HashedString overrideSymbol;
	}

	// Token: 0x02000B41 RID: 2881
	public class OverrideEntry
	{
		// Token: 0x040024EC RID: 9452
		public KAnimFile buildFile;

		// Token: 0x040024ED RID: 9453
		public string symbolName;
	}
}
