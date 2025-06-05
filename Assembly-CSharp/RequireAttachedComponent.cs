using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020019E8 RID: 6632
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
	// Token: 0x17000912 RID: 2322
	// (get) Token: 0x06008A39 RID: 35385 RVA: 0x000FEDAB File Offset: 0x000FCFAB
	// (set) Token: 0x06008A3A RID: 35386 RVA: 0x000FEDB3 File Offset: 0x000FCFB3
	public Type RequiredType
	{
		get
		{
			return this.requiredType;
		}
		set
		{
			this.requiredType = value;
			this.typeNameString = this.requiredType.Name;
		}
	}

	// Token: 0x06008A3B RID: 35387 RVA: 0x000FEDCD File Offset: 0x000FCFCD
	public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string)
	{
		this.myAttachable = myAttachable;
		this.requiredType = required_type;
		this.typeNameString = type_name_string;
	}

	// Token: 0x06008A3C RID: 35388 RVA: 0x00369674 File Offset: 0x00367874
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.myAttachable != null)
		{
			using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.myAttachable).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent(this.requiredType))
					{
						return ProcessCondition.Status.Ready;
					}
				}
			}
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06008A3D RID: 35389 RVA: 0x000FEDEA File Offset: 0x000FCFEA
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return this.typeNameString;
	}

	// Token: 0x06008A3E RID: 35390 RVA: 0x000FEDF6 File Offset: 0x000FCFF6
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, this.typeNameString.ToLower());
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, this.typeNameString.ToLower());
	}

	// Token: 0x06008A3F RID: 35391 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04006855 RID: 26709
	private string typeNameString;

	// Token: 0x04006856 RID: 26710
	private Type requiredType;

	// Token: 0x04006857 RID: 26711
	private AttachableBuilding myAttachable;
}
