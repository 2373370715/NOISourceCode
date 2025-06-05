using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02001F09 RID: 7945
[AddComponentMenu("KMonoBehaviour/scripts/PopFX")]
public class PopFX : KMonoBehaviour
{
	// Token: 0x0600A6FD RID: 42749 RVA: 0x00402A94 File Offset: 0x00400C94
	public void Recycle()
	{
		this.icon = null;
		this.text = "";
		this.targetTransform = null;
		this.lifeElapsed = 0f;
		this.trackTarget = false;
		this.startPos = Vector3.zero;
		this.IconDisplay.color = Color.white;
		this.TextDisplay.color = Color.white;
		PopFXManager.Instance.RecycleFX(this);
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
		this.isLive = false;
		this.isActiveWorld = false;
		Game.Instance.Unsubscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
	}

	// Token: 0x0600A6FE RID: 42750 RVA: 0x00402B48 File Offset: 0x00400D48
	public void Spawn(Sprite Icon, string Text, Transform TargetTransform, Vector3 Offset, float LifeTime = 1.5f, bool TrackTarget = false)
	{
		this.icon = Icon;
		this.text = Text;
		this.targetTransform = TargetTransform;
		this.trackTarget = TrackTarget;
		this.lifetime = LifeTime;
		this.offset = Offset;
		if (this.targetTransform != null)
		{
			this.startPos = this.targetTransform.GetPosition();
			int num;
			int num2;
			Grid.PosToXY(this.startPos, out num, out num2);
			if (num2 % 2 != 0)
			{
				this.startPos.x = this.startPos.x + 0.5f;
			}
		}
		this.TextDisplay.text = this.text;
		this.IconDisplay.sprite = this.icon;
		this.canvasGroup.alpha = 1f;
		this.isLive = true;
		Game.Instance.Subscribe(1983128072, new Action<object>(this.OnActiveWorldChanged));
		this.SetWorldActive(ClusterManager.Instance.activeWorldId);
		this.Update();
	}

	// Token: 0x0600A6FF RID: 42751 RVA: 0x00402C34 File Offset: 0x00400E34
	private void OnActiveWorldChanged(object data)
	{
		global::Tuple<int, int> tuple = (global::Tuple<int, int>)data;
		if (this.isLive)
		{
			this.SetWorldActive(tuple.first);
		}
	}

	// Token: 0x0600A700 RID: 42752 RVA: 0x00402C5C File Offset: 0x00400E5C
	private void SetWorldActive(int worldId)
	{
		int num = Grid.PosToCell((this.trackTarget && this.targetTransform != null) ? this.targetTransform.position : (this.startPos + this.offset));
		this.isActiveWorld = (!Grid.IsValidCell(num) || (int)Grid.WorldIdx[num] == worldId);
	}

	// Token: 0x0600A701 RID: 42753 RVA: 0x00402CC0 File Offset: 0x00400EC0
	private void Update()
	{
		if (!this.isLive)
		{
			return;
		}
		if (!PopFXManager.Instance.Ready())
		{
			return;
		}
		this.lifeElapsed += Time.unscaledDeltaTime;
		if (this.lifeElapsed >= this.lifetime)
		{
			this.Recycle();
		}
		if (this.trackTarget && this.targetTransform != null)
		{
			Vector3 v = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * this.lifeElapsed));
			v.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v;
		}
		else
		{
			Vector3 v2 = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * (this.lifeElapsed / 2f)));
			v2.z = 0f;
			base.gameObject.rectTransform().anchoredPosition = v2;
		}
		this.canvasGroup.alpha = (this.isActiveWorld ? (1.5f * ((this.lifetime - this.lifeElapsed) / this.lifetime)) : 0f);
	}

	// Token: 0x040082E5 RID: 33509
	private float Speed = 2f;

	// Token: 0x040082E6 RID: 33510
	private Sprite icon;

	// Token: 0x040082E7 RID: 33511
	private string text;

	// Token: 0x040082E8 RID: 33512
	private Transform targetTransform;

	// Token: 0x040082E9 RID: 33513
	private Vector3 offset;

	// Token: 0x040082EA RID: 33514
	public Image IconDisplay;

	// Token: 0x040082EB RID: 33515
	public LocText TextDisplay;

	// Token: 0x040082EC RID: 33516
	public CanvasGroup canvasGroup;

	// Token: 0x040082ED RID: 33517
	private Camera uiCamera;

	// Token: 0x040082EE RID: 33518
	private float lifetime;

	// Token: 0x040082EF RID: 33519
	private float lifeElapsed;

	// Token: 0x040082F0 RID: 33520
	private bool trackTarget;

	// Token: 0x040082F1 RID: 33521
	private Vector3 startPos;

	// Token: 0x040082F2 RID: 33522
	private bool isLive;

	// Token: 0x040082F3 RID: 33523
	private bool isActiveWorld;
}
