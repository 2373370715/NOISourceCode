using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02001A23 RID: 6691
public class SubstanceTable : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x06008B57 RID: 35671 RVA: 0x000FFB82 File Offset: 0x000FDD82
	public List<Substance> GetList()
	{
		return this.list;
	}

	// Token: 0x06008B58 RID: 35672 RVA: 0x0036DA00 File Offset: 0x0036BC00
	public Substance GetSubstance(SimHashes substance)
	{
		int count = this.list.Count;
		for (int i = 0; i < count; i++)
		{
			if (this.list[i].elementID == substance)
			{
				return this.list[i];
			}
		}
		return null;
	}

	// Token: 0x06008B59 RID: 35673 RVA: 0x000FFB8A File Offset: 0x000FDD8A
	public void OnBeforeSerialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06008B5A RID: 35674 RVA: 0x000FFB8A File Offset: 0x000FDD8A
	public void OnAfterDeserialize()
	{
		this.BindAnimList();
	}

	// Token: 0x06008B5B RID: 35675 RVA: 0x0036DA48 File Offset: 0x0036BC48
	private void BindAnimList()
	{
		foreach (Substance substance in this.list)
		{
			if (substance.anim != null && (substance.anims == null || substance.anims.Length == 0))
			{
				substance.anims = new KAnimFile[1];
				substance.anims[0] = substance.anim;
			}
		}
	}

	// Token: 0x06008B5C RID: 35676 RVA: 0x000FFB92 File Offset: 0x000FDD92
	public void RemoveDuplicates()
	{
		this.list = this.list.Distinct(new SubstanceTable.SubstanceEqualityComparer()).ToList<Substance>();
	}

	// Token: 0x04006940 RID: 26944
	[SerializeField]
	private List<Substance> list;

	// Token: 0x04006941 RID: 26945
	public Material solidMaterial;

	// Token: 0x04006942 RID: 26946
	public Material liquidMaterial;

	// Token: 0x02001A24 RID: 6692
	private class SubstanceEqualityComparer : IEqualityComparer<Substance>
	{
		// Token: 0x06008B5E RID: 35678 RVA: 0x000FFBAF File Offset: 0x000FDDAF
		public bool Equals(Substance x, Substance y)
		{
			return x.elementID.Equals(y.elementID);
		}

		// Token: 0x06008B5F RID: 35679 RVA: 0x000FFBCD File Offset: 0x000FDDCD
		public int GetHashCode(Substance obj)
		{
			return obj.elementID.GetHashCode();
		}
	}
}
