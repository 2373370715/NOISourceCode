using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CE1 RID: 15585
	public static class ModifiersExtensions
	{
		// Token: 0x0600EF40 RID: 61248 RVA: 0x00145097 File Offset: 0x00143297
		public static Attributes GetAttributes(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetAttributes();
		}

		// Token: 0x0600EF41 RID: 61249 RVA: 0x004E9398 File Offset: 0x004E7598
		public static Attributes GetAttributes(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.attributes;
			}
			return null;
		}

		// Token: 0x0600EF42 RID: 61250 RVA: 0x001450A4 File Offset: 0x001432A4
		public static Amounts GetAmounts(this KMonoBehaviour cmp)
		{
			if (cmp is Modifiers)
			{
				return ((Modifiers)cmp).amounts;
			}
			return cmp.gameObject.GetAmounts();
		}

		// Token: 0x0600EF43 RID: 61251 RVA: 0x004E93C0 File Offset: 0x004E75C0
		public static Amounts GetAmounts(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.amounts;
			}
			return null;
		}

		// Token: 0x0600EF44 RID: 61252 RVA: 0x001450C5 File Offset: 0x001432C5
		public static Sicknesses GetSicknesses(this KMonoBehaviour cmp)
		{
			return cmp.gameObject.GetSicknesses();
		}

		// Token: 0x0600EF45 RID: 61253 RVA: 0x004E93E8 File Offset: 0x004E75E8
		public static Sicknesses GetSicknesses(this GameObject go)
		{
			Modifiers component = go.GetComponent<Modifiers>();
			if (component != null)
			{
				return component.sicknesses;
			}
			return null;
		}
	}
}
