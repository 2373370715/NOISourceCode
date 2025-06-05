using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B45 RID: 2885
[AddComponentMenu("KMonoBehaviour/scripts/SpriteSheetAnimManager")]
public class SpriteSheetAnimManager : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x0600357E RID: 13694 RVA: 0x000C7573 File Offset: 0x000C5773
	public static void DestroyInstance()
	{
		SpriteSheetAnimManager.instance = null;
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x000C757B File Offset: 0x000C577B
	protected override void OnPrefabInit()
	{
		SpriteSheetAnimManager.instance = this;
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x0021BF78 File Offset: 0x0021A178
	protected override void OnSpawn()
	{
		for (int i = 0; i < this.sheets.Length; i++)
		{
			int key = Hash.SDBMLower(this.sheets[i].name);
			this.nameIndexMap[key] = new SpriteSheetAnimator(this.sheets[i]);
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x0021BFCC File Offset: 0x0021A1CC
	public void Play(string name, Vector3 pos, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, Quaternion.identity, size, colour);
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x0021BFF0 File Offset: 0x0021A1F0
	public void Play(string name, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		int name_hash = Hash.SDBMLower(name);
		this.Play(name_hash, pos, rotation, size, colour);
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000C7583 File Offset: 0x000C5783
	public void Play(int name_hash, Vector3 pos, Quaternion rotation, Vector2 size, Color32 colour)
	{
		this.nameIndexMap[name_hash].Play(pos, rotation, size, colour);
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000C75A1 File Offset: 0x000C57A1
	public void RenderEveryTick(float dt)
	{
		this.UpdateAnims(dt);
		this.Render();
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x0021C014 File Offset: 0x0021A214
	public void UpdateAnims(float dt)
	{
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.UpdateAnims(dt);
		}
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x0021C070 File Offset: 0x0021A270
	public void Render()
	{
		Vector3 zero = Vector3.zero;
		foreach (KeyValuePair<int, SpriteSheetAnimator> keyValuePair in this.nameIndexMap)
		{
			keyValuePair.Value.Render();
		}
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x000C75B0 File Offset: 0x000C57B0
	public SpriteSheetAnimator GetSpriteSheetAnimator(HashedString name)
	{
		return this.nameIndexMap[name.HashValue];
	}

	// Token: 0x04002500 RID: 9472
	public const float SECONDS_PER_FRAME = 0.033333335f;

	// Token: 0x04002501 RID: 9473
	[SerializeField]
	private SpriteSheet[] sheets;

	// Token: 0x04002502 RID: 9474
	private Dictionary<int, SpriteSheetAnimator> nameIndexMap = new Dictionary<int, SpriteSheetAnimator>();

	// Token: 0x04002503 RID: 9475
	public static SpriteSheetAnimManager instance;
}
