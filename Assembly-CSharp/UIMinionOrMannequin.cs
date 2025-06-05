using System;
using System.Collections.Generic;
using Database;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020020A9 RID: 8361
public class UIMinionOrMannequin : KMonoBehaviour
{
	// Token: 0x17000B69 RID: 2921
	// (get) Token: 0x0600B246 RID: 45638 RVA: 0x0011866C File Offset: 0x0011686C
	// (set) Token: 0x0600B247 RID: 45639 RVA: 0x00118674 File Offset: 0x00116874
	public UIMinionOrMannequin.ITarget current { get; private set; }

	// Token: 0x0600B248 RID: 45640 RVA: 0x0011867D File Offset: 0x0011687D
	protected override void OnSpawn()
	{
		this.TrySpawn();
	}

	// Token: 0x0600B249 RID: 45641 RVA: 0x0043CF38 File Offset: 0x0043B138
	public bool TrySpawn()
	{
		bool flag = false;
		if (this.mannequin.IsNullOrDestroyed())
		{
			GameObject gameObject = new GameObject("UIMannequin");
			gameObject.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter = gameObject.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter.aspectRatio = 1f;
			this.mannequin = gameObject.AddOrGet<UIMannequin>();
			this.mannequin.TrySpawn();
			gameObject.SetActive(false);
			flag = true;
		}
		if (this.minion.IsNullOrDestroyed())
		{
			GameObject gameObject2 = new GameObject("UIMinion");
			gameObject2.AddOrGet<RectTransform>().Fill(Padding.All(10f));
			gameObject2.transform.SetParent(base.transform, false);
			AspectRatioFitter aspectRatioFitter2 = gameObject2.AddOrGet<AspectRatioFitter>();
			aspectRatioFitter2.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
			aspectRatioFitter2.aspectRatio = 1f;
			this.minion = gameObject2.AddOrGet<UIMinion>();
			this.minion.TrySpawn();
			gameObject2.SetActive(false);
			flag = true;
		}
		if (flag)
		{
			this.SetAsMannequin();
		}
		return flag;
	}

	// Token: 0x0600B24A RID: 45642 RVA: 0x00118686 File Offset: 0x00116886
	public UIMinionOrMannequin.ITarget SetFrom(Option<Personality> personality)
	{
		if (personality.IsSome())
		{
			return this.SetAsMinion(personality.Unwrap());
		}
		return this.SetAsMannequin();
	}

	// Token: 0x0600B24B RID: 45643 RVA: 0x0043D040 File Offset: 0x0043B240
	public UIMinion SetAsMinion(Personality personality)
	{
		this.mannequin.gameObject.SetActive(false);
		this.minion.gameObject.SetActive(true);
		this.minion.SetMinion(personality);
		this.current = this.minion;
		return this.minion;
	}

	// Token: 0x0600B24C RID: 45644 RVA: 0x001186A5 File Offset: 0x001168A5
	public UIMannequin SetAsMannequin()
	{
		this.minion.gameObject.SetActive(false);
		this.mannequin.gameObject.SetActive(true);
		this.current = this.mannequin;
		return this.mannequin;
	}

	// Token: 0x0600B24D RID: 45645 RVA: 0x0043D090 File Offset: 0x0043B290
	public MinionVoice GetMinionVoice()
	{
		return MinionVoice.ByObject(this.current.SpawnedAvatar).UnwrapOr(MinionVoice.Random(), null);
	}

	// Token: 0x04008CCC RID: 36044
	public UIMinion minion;

	// Token: 0x04008CCD RID: 36045
	public UIMannequin mannequin;

	// Token: 0x020020AA RID: 8362
	public interface ITarget
	{
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x0600B24F RID: 45647
		GameObject SpawnedAvatar { get; }

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x0600B250 RID: 45648
		Option<Personality> Personality { get; }

		// Token: 0x0600B251 RID: 45649
		void SetOutfit(ClothingOutfitUtility.OutfitType outfitType, IEnumerable<ClothingItemResource> clothingItems);

		// Token: 0x0600B252 RID: 45650
		void React(UIMinionOrMannequinReactSource source);
	}
}
