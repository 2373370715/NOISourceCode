using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001A22 RID: 6690
[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/SubstanceChunk")]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06008B52 RID: 35666 RVA: 0x0036D8D8 File Offset: 0x0036BAD8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Color color = base.GetComponent<PrimaryElement>().Element.substance.colour;
		color.a = 1f;
		base.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint, color);
		base.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint2, color);
	}

	// Token: 0x06008B53 RID: 35667 RVA: 0x0036D934 File Offset: 0x0036BB34
	private void OnRefreshUserMenu(object data)
	{
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), global::Action.NumActions, null, null, null, UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP, true), 1f);
	}

	// Token: 0x06008B54 RID: 35668 RVA: 0x0036D990 File Offset: 0x0036BB90
	private void OnRelease()
	{
		int gameCell = Grid.PosToCell(base.transform.GetPosition());
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		if (component.Mass > 0f)
		{
			SimMessages.AddRemoveSubstance(gameCell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x0400693E RID: 26942
	private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");

	// Token: 0x0400693F RID: 26943
	private static readonly KAnimHashedString symbolToTint2 = new KAnimHashedString("substance_tinter_cap");
}
