using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002070 RID: 8304
[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour
{
	// Token: 0x0600B0B7 RID: 45239 RVA: 0x00433544 File Offset: 0x00431744
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeUntilNextSlide = this.timePerSlide;
		if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
		{
			this.imageTarget.color = Color.clear;
		}
		if (this.isExpandable)
		{
			this.button = base.GetComponent<KButton>();
			this.button.onClick += delegate()
			{
				if (this.onBeforePlay != null)
				{
					this.onBeforePlay();
				}
				SlideshowUpdateType slideshowUpdateType = this.updateType;
				if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites)
				{
					VideoScreen.Instance.PlaySlideShow(this.sprites);
					return;
				}
				if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
				{
					return;
				}
				VideoScreen.Instance.PlaySlideShow(this.files);
			};
		}
		if (this.nextButton != null)
		{
			this.nextButton.onClick += delegate()
			{
				this.nextSlide();
			};
		}
		if (this.prevButton != null)
		{
			this.prevButton.onClick += delegate()
			{
				this.prevSlide();
			};
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.onClick += delegate()
			{
				this.SetPaused(!this.paused);
			};
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				VideoScreen.Instance.Stop();
				if (this.onEndingPlay != null)
				{
					this.onEndingPlay();
				}
			};
		}
	}

	// Token: 0x0600B0B8 RID: 45240 RVA: 0x0043364C File Offset: 0x0043184C
	public void SetPaused(bool state)
	{
		this.paused = state;
		if (this.pauseIcon != null)
		{
			this.pauseIcon.gameObject.SetActive(!this.paused);
		}
		if (this.unpauseIcon != null)
		{
			this.unpauseIcon.gameObject.SetActive(this.paused);
		}
		if (this.prevButton != null)
		{
			this.prevButton.gameObject.SetActive(this.paused);
		}
		if (this.nextButton != null)
		{
			this.nextButton.gameObject.SetActive(this.paused);
		}
	}

	// Token: 0x0600B0B9 RID: 45241 RVA: 0x004336F4 File Offset: 0x004318F4
	private void resetSlide(bool enable)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		this.currentSlide = 0;
		if (enable)
		{
			this.imageTarget.color = Color.white;
			return;
		}
		if (this.transparentIfEmpty)
		{
			this.imageTarget.color = Color.clear;
		}
	}

	// Token: 0x0600B0BA RID: 45242 RVA: 0x00433740 File Offset: 0x00431940
	private Sprite loadSlide(string file)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Texture2D texture2D = new Texture2D(512, 768);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(File.ReadAllBytes(file));
		return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
	}

	// Token: 0x0600B0BB RID: 45243 RVA: 0x004337B0 File Offset: 0x004319B0
	public void SetFiles(string[] files, int loadFrame = -1)
	{
		if (files == null)
		{
			return;
		}
		this.files = files;
		bool flag = files.Length != 0 && files[0] != null;
		this.resetSlide(flag);
		if (flag)
		{
			int num = (loadFrame != -1) ? loadFrame : (files.Length - 1);
			string file = files[num];
			Sprite slide = this.loadSlide(file);
			this.setSlide(slide);
			this.currentSlideImage = slide;
		}
	}

	// Token: 0x0600B0BC RID: 45244 RVA: 0x00433808 File Offset: 0x00431A08
	public void updateSize(Sprite sprite)
	{
		Vector2 fittedSize = this.GetFittedSize(sprite, 960f, 960f);
		base.GetComponent<RectTransform>().sizeDelta = fittedSize;
	}

	// Token: 0x0600B0BD RID: 45245 RVA: 0x0011778F File Offset: 0x0011598F
	public void SetSprites(Sprite[] sprites)
	{
		if (sprites == null)
		{
			return;
		}
		this.sprites = sprites;
		this.resetSlide(sprites.Length != 0 && sprites[0] != null);
		if (sprites.Length != 0 && sprites[0] != null)
		{
			this.setSlide(sprites[0]);
		}
	}

	// Token: 0x0600B0BE RID: 45246 RVA: 0x00433834 File Offset: 0x00431A34
	public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
	{
		if (sprite == null || sprite.texture == null)
		{
			return Vector2.zero;
		}
		int width = sprite.texture.width;
		int height = sprite.texture.height;
		float num = maxWidth / (float)width;
		float num2 = maxHeight / (float)height;
		if (num < num2)
		{
			return new Vector2((float)width * num, (float)height * num);
		}
		return new Vector2((float)width * num2, (float)height * num2);
	}

	// Token: 0x0600B0BF RID: 45247 RVA: 0x001177CA File Offset: 0x001159CA
	public void setSlide(Sprite slide)
	{
		if (slide == null)
		{
			return;
		}
		this.imageTarget.texture = slide.texture;
		this.updateSize(slide);
	}

	// Token: 0x0600B0C0 RID: 45248 RVA: 0x001177EE File Offset: 0x001159EE
	public void nextSlide()
	{
		this.setSlideIndex(this.currentSlide + 1);
	}

	// Token: 0x0600B0C1 RID: 45249 RVA: 0x001177FE File Offset: 0x001159FE
	public void prevSlide()
	{
		this.setSlideIndex(this.currentSlide - 1);
	}

	// Token: 0x0600B0C2 RID: 45250 RVA: 0x004338A0 File Offset: 0x00431AA0
	private void setSlideIndex(int slideIndex)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		SlideshowUpdateType slideshowUpdateType = this.updateType;
		if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites)
		{
			if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
			{
				return;
			}
			if (slideIndex < 0)
			{
				slideIndex = this.files.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.files.Length;
			if (this.currentSlide == this.files.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				if (this.currentSlideImage != null)
				{
					UnityEngine.Object.Destroy(this.currentSlideImage.texture);
					UnityEngine.Object.Destroy(this.currentSlideImage);
					GC.Collect();
				}
				this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
				this.setSlide(this.currentSlideImage);
			}
		}
		else
		{
			if (slideIndex < 0)
			{
				slideIndex = this.sprites.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.sprites.Length;
			if (this.currentSlide == this.sprites.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				this.setSlide(this.sprites[this.currentSlide]);
				return;
			}
		}
	}

	// Token: 0x0600B0C3 RID: 45251 RVA: 0x004339CC File Offset: 0x00431BCC
	private void Update()
	{
		if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length == 0))
		{
			return;
		}
		if (this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length == 0))
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		this.timeUntilNextSlide -= Time.unscaledDeltaTime;
		if (this.timeUntilNextSlide <= 0f)
		{
			this.nextSlide();
		}
	}

	// Token: 0x04008B2C RID: 35628
	public RawImage imageTarget;

	// Token: 0x04008B2D RID: 35629
	private string[] files;

	// Token: 0x04008B2E RID: 35630
	private Sprite currentSlideImage;

	// Token: 0x04008B2F RID: 35631
	private Sprite[] sprites;

	// Token: 0x04008B30 RID: 35632
	public float timePerSlide = 1f;

	// Token: 0x04008B31 RID: 35633
	public float timeFactorForLastSlide = 3f;

	// Token: 0x04008B32 RID: 35634
	private int currentSlide;

	// Token: 0x04008B33 RID: 35635
	private float timeUntilNextSlide;

	// Token: 0x04008B34 RID: 35636
	private bool paused;

	// Token: 0x04008B35 RID: 35637
	public bool playInThumbnail;

	// Token: 0x04008B36 RID: 35638
	public SlideshowUpdateType updateType;

	// Token: 0x04008B37 RID: 35639
	[SerializeField]
	private bool isExpandable;

	// Token: 0x04008B38 RID: 35640
	[SerializeField]
	private KButton button;

	// Token: 0x04008B39 RID: 35641
	[SerializeField]
	private bool transparentIfEmpty = true;

	// Token: 0x04008B3A RID: 35642
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04008B3B RID: 35643
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04008B3C RID: 35644
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04008B3D RID: 35645
	[SerializeField]
	private KButton pauseButton;

	// Token: 0x04008B3E RID: 35646
	[SerializeField]
	private Image pauseIcon;

	// Token: 0x04008B3F RID: 35647
	[SerializeField]
	private Image unpauseIcon;

	// Token: 0x04008B40 RID: 35648
	public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;

	// Token: 0x04008B41 RID: 35649
	public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

	// Token: 0x02002071 RID: 8305
	// (Invoke) Token: 0x0600B0CB RID: 45259
	public delegate void onBeforeAndEndPlayDelegate();
}
