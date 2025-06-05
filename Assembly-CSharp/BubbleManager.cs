using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C9E RID: 3230
[AddComponentMenu("KMonoBehaviour/scripts/BubbleManager")]
public class BubbleManager : KMonoBehaviour, ISim33ms, IRenderEveryTick
{
	// Token: 0x06003D5C RID: 15708 RVA: 0x000CC27F File Offset: 0x000CA47F
	public static void DestroyInstance()
	{
		BubbleManager.instance = null;
	}

	// Token: 0x06003D5D RID: 15709 RVA: 0x000CC287 File Offset: 0x000CA487
	protected override void OnPrefabInit()
	{
		BubbleManager.instance = this;
	}

	// Token: 0x06003D5E RID: 15710 RVA: 0x0023EDAC File Offset: 0x0023CFAC
	public void SpawnBubble(Vector2 position, Vector2 velocity, SimHashes element, float mass, float temperature)
	{
		BubbleManager.Bubble item = new BubbleManager.Bubble
		{
			position = position,
			velocity = velocity,
			element = element,
			temperature = temperature,
			mass = mass
		};
		this.bubbles.Add(item);
	}

	// Token: 0x06003D5F RID: 15711 RVA: 0x0023EDFC File Offset: 0x0023CFFC
	public void Sim33ms(float dt)
	{
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		ListPool<BubbleManager.Bubble, BubbleManager>.PooledList pooledList2 = ListPool<BubbleManager.Bubble, BubbleManager>.Allocate();
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			bubble.position += bubble.velocity * dt;
			bubble.elapsedTime += dt;
			int num = Grid.PosToCell(bubble.position);
			if (!Grid.IsVisiblyInLiquid(bubble.position) || Grid.Element[num].id == bubble.element)
			{
				pooledList2.Add(bubble);
			}
			else
			{
				pooledList.Add(bubble);
			}
		}
		foreach (BubbleManager.Bubble bubble2 in pooledList2)
		{
			SimMessages.AddRemoveSubstance(Grid.PosToCell(bubble2.position), bubble2.element, CellEventLogger.Instance.FallingWaterAddToSim, bubble2.mass, bubble2.temperature, byte.MaxValue, 0, true, -1);
		}
		this.bubbles.Clear();
		this.bubbles.AddRange(pooledList);
		pooledList2.Recycle();
		pooledList.Recycle();
	}

	// Token: 0x06003D60 RID: 15712 RVA: 0x0023EF54 File Offset: 0x0023D154
	public void RenderEveryTick(float dt)
	{
		ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.PooledList pooledList = ListPool<SpriteSheetAnimator.AnimInfo, BubbleManager>.Allocate();
		SpriteSheetAnimator spriteSheetAnimator = SpriteSheetAnimManager.instance.GetSpriteSheetAnimator("liquid_splash1");
		foreach (BubbleManager.Bubble bubble in this.bubbles)
		{
			SpriteSheetAnimator.AnimInfo item = new SpriteSheetAnimator.AnimInfo
			{
				frame = spriteSheetAnimator.GetFrameFromElapsedTimeLooping(bubble.elapsedTime),
				elapsedTime = bubble.elapsedTime,
				pos = new Vector3(bubble.position.x, bubble.position.y, 0f),
				rotation = Quaternion.identity,
				size = Vector2.one,
				colour = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue)
			};
			pooledList.Add(item);
		}
		pooledList.Recycle();
	}

	// Token: 0x04002A62 RID: 10850
	public static BubbleManager instance;

	// Token: 0x04002A63 RID: 10851
	private List<BubbleManager.Bubble> bubbles = new List<BubbleManager.Bubble>();

	// Token: 0x02000C9F RID: 3231
	private struct Bubble
	{
		// Token: 0x04002A64 RID: 10852
		public Vector2 position;

		// Token: 0x04002A65 RID: 10853
		public Vector2 velocity;

		// Token: 0x04002A66 RID: 10854
		public float elapsedTime;

		// Token: 0x04002A67 RID: 10855
		public int frame;

		// Token: 0x04002A68 RID: 10856
		public SimHashes element;

		// Token: 0x04002A69 RID: 10857
		public float temperature;

		// Token: 0x04002A6A RID: 10858
		public float mass;
	}
}
