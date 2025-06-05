using System;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000F1B RID: 3867
[AddComponentMenu("KMonoBehaviour/scripts/MonumentPart")]
public class MonumentPart : KMonoBehaviour
{
	// Token: 0x06004D6F RID: 19823 RVA: 0x000D6A25 File Offset: 0x000D4C25
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MonumentParts.Add(this);
		if (!string.IsNullOrEmpty(this.chosenState))
		{
			this.SetState(this.chosenState);
		}
		this.UpdateMonumentDecor();
	}

	// Token: 0x06004D70 RID: 19824 RVA: 0x00273EC0 File Offset: 0x002720C0
	[OnDeserialized]
	private void OnDeserializedMethod()
	{
		if (Db.GetMonumentParts().TryGet(this.chosenState) == null)
		{
			string id = "";
			if (this.part == MonumentPartResource.Part.Bottom)
			{
				id = "bottom_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Middle)
			{
				id = "mid_" + this.chosenState;
			}
			else if (this.part == MonumentPartResource.Part.Top)
			{
				id = "top_" + this.chosenState;
			}
			if (Db.GetMonumentParts().TryGet(id) != null)
			{
				this.chosenState = id;
			}
		}
	}

	// Token: 0x06004D71 RID: 19825 RVA: 0x000D6A57 File Offset: 0x000D4C57
	protected override void OnCleanUp()
	{
		Components.MonumentParts.Remove(this);
		this.RemoveMonumentPiece();
		base.OnCleanUp();
	}

	// Token: 0x06004D72 RID: 19826 RVA: 0x00273F4C File Offset: 0x0027214C
	public void SetState(string state)
	{
		MonumentPartResource monumentPartResource = Db.GetMonumentParts().Get(state);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		component.SwapAnims(new KAnimFile[]
		{
			monumentPartResource.AnimFile
		});
		component.Play(monumentPartResource.State, KAnim.PlayMode.Once, 1f, 0f);
		this.chosenState = state;
	}

	// Token: 0x06004D73 RID: 19827 RVA: 0x00273FA4 File Offset: 0x002721A4
	public bool IsMonumentCompleted()
	{
		bool flag = this.GetMonumentPart(MonumentPartResource.Part.Top) != null;
		bool flag2 = this.GetMonumentPart(MonumentPartResource.Part.Middle) != null;
		bool flag3 = this.GetMonumentPart(MonumentPartResource.Part.Bottom) != null;
		return flag && flag3 && flag2;
	}

	// Token: 0x06004D74 RID: 19828 RVA: 0x00273FE0 File Offset: 0x002721E0
	public void UpdateMonumentDecor()
	{
		GameObject monumentPart = this.GetMonumentPart(MonumentPartResource.Part.Middle);
		if (this.IsMonumentCompleted())
		{
			monumentPart.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.COMPLETE);
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject != monumentPart)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.NONE);
				}
			}
		}
	}

	// Token: 0x06004D75 RID: 19829 RVA: 0x0027406C File Offset: 0x0027226C
	public void RemoveMonumentPiece()
	{
		if (this.IsMonumentCompleted())
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
			{
				if (gameObject.GetComponent<MonumentPart>() != this)
				{
					gameObject.GetComponent<DecorProvider>().SetValues(BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE);
				}
			}
		}
	}

	// Token: 0x06004D76 RID: 19830 RVA: 0x002740E4 File Offset: 0x002722E4
	private GameObject GetMonumentPart(MonumentPartResource.Part requestPart)
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(base.GetComponent<AttachableBuilding>()))
		{
			MonumentPart component = gameObject.GetComponent<MonumentPart>();
			if (!(component == null) && component.part == requestPart)
			{
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x04003662 RID: 13922
	public MonumentPartResource.Part part;

	// Token: 0x04003663 RID: 13923
	public string stateUISymbol;

	// Token: 0x04003664 RID: 13924
	[Serialize]
	private string chosenState;
}
