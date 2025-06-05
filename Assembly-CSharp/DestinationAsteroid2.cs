using System;
using ProcGen;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001CF5 RID: 7413
[AddComponentMenu("KMonoBehaviour/scripts/DestinationAsteroid2")]
public class DestinationAsteroid2 : KMonoBehaviour
{
	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06009AAD RID: 39597 RVA: 0x003C8B28 File Offset: 0x003C6D28
	// (remove) Token: 0x06009AAE RID: 39598 RVA: 0x003C8B60 File Offset: 0x003C6D60
	public event Action<ColonyDestinationAsteroidBeltData> OnClicked;

	// Token: 0x06009AAF RID: 39599 RVA: 0x00109243 File Offset: 0x00107443
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.button.onClick += this.OnClickInternal;
	}

	// Token: 0x06009AB0 RID: 39600 RVA: 0x003C8B98 File Offset: 0x003C6D98
	public void SetAsteroid(ColonyDestinationAsteroidBeltData newAsteroidData)
	{
		if (this.asteroidData == null || newAsteroidData.beltPath != this.asteroidData.beltPath)
		{
			this.asteroidData = newAsteroidData;
			ProcGen.World getStartWorld = newAsteroidData.GetStartWorld;
			KAnimFile kanimFile;
			Assets.TryGetAnim(getStartWorld.asteroidIcon.IsNullOrWhiteSpace() ? AsteroidGridEntity.DEFAULT_ASTEROID_ICON_ANIM : getStartWorld.asteroidIcon, out kanimFile);
			if (kanimFile != null)
			{
				this.asteroidImage.gameObject.SetActive(false);
				this.animController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				this.animController.initialMode = KAnim.PlayMode.Loop;
				this.animController.initialAnim = "idle_loop";
				this.animController.gameObject.SetActive(true);
				if (this.animController.HasAnimation(this.animController.initialAnim))
				{
					this.animController.Play(this.animController.initialAnim, KAnim.PlayMode.Loop, 1f, 0f);
				}
			}
			else
			{
				this.animController.gameObject.SetActive(false);
				this.asteroidImage.gameObject.SetActive(true);
				this.asteroidImage.sprite = this.asteroidData.sprite;
				this.imageDlcFrom.gameObject.SetActive(false);
			}
			Sprite sprite = null;
			if (DlcManager.IsDlcId(this.asteroidData.Layout.dlcIdFrom))
			{
				sprite = Assets.GetSprite(DlcManager.GetDlcSmallLogo(this.asteroidData.Layout.dlcIdFrom));
			}
			if (sprite != null)
			{
				this.imageDlcFrom.gameObject.SetActive(true);
				this.imageDlcFrom.sprite = sprite;
				return;
			}
			this.imageDlcFrom.gameObject.SetActive(false);
			this.imageDlcFrom.sprite = sprite;
		}
	}

	// Token: 0x06009AB1 RID: 39601 RVA: 0x00109262 File Offset: 0x00107462
	private void OnClickInternal()
	{
		DebugUtil.LogArgs(new object[]
		{
			"Clicked asteroid belt",
			this.asteroidData.beltPath
		});
		this.OnClicked(this.asteroidData);
	}

	// Token: 0x040078C8 RID: 30920
	[SerializeField]
	private Image asteroidImage;

	// Token: 0x040078C9 RID: 30921
	[SerializeField]
	private KButton button;

	// Token: 0x040078CA RID: 30922
	[SerializeField]
	private KBatchedAnimController animController;

	// Token: 0x040078CB RID: 30923
	[SerializeField]
	private Image imageDlcFrom;

	// Token: 0x040078CD RID: 30925
	private ColonyDestinationAsteroidBeltData asteroidData;
}
