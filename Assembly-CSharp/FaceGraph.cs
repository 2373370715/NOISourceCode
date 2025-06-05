using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001523 RID: 5411
[AddComponentMenu("KMonoBehaviour/scripts/FaceGraph")]
public class FaceGraph : KMonoBehaviour
{
	// Token: 0x06007077 RID: 28791 RVA: 0x000EE0E7 File Offset: 0x000EC2E7
	public IEnumerator<Expression> GetEnumerator()
	{
		return this.expressions.GetEnumerator();
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06007078 RID: 28792 RVA: 0x000EE0F9 File Offset: 0x000EC2F9
	// (set) Token: 0x06007079 RID: 28793 RVA: 0x000EE101 File Offset: 0x000EC301
	public Expression overrideExpression { get; private set; }

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x0600707A RID: 28794 RVA: 0x000EE10A File Offset: 0x000EC30A
	// (set) Token: 0x0600707B RID: 28795 RVA: 0x000EE112 File Offset: 0x000EC312
	public Expression currentExpression { get; private set; }

	// Token: 0x0600707C RID: 28796 RVA: 0x000EE11B File Offset: 0x000EC31B
	public void AddExpression(Expression expression)
	{
		if (this.expressions.Contains(expression))
		{
			return;
		}
		this.expressions.Add(expression);
		this.UpdateFace();
	}

	// Token: 0x0600707D RID: 28797 RVA: 0x000EE13E File Offset: 0x000EC33E
	public void RemoveExpression(Expression expression)
	{
		if (this.expressions.Remove(expression))
		{
			this.UpdateFace();
		}
	}

	// Token: 0x0600707E RID: 28798 RVA: 0x000EE154 File Offset: 0x000EC354
	public void SetOverrideExpression(Expression expression)
	{
		if (expression != this.overrideExpression)
		{
			this.overrideExpression = expression;
			this.UpdateFace();
		}
	}

	// Token: 0x0600707F RID: 28799 RVA: 0x00305B04 File Offset: 0x00303D04
	public void ApplyShape()
	{
		KAnimFile anim = Assets.GetAnim(FaceGraph.HASH_HEAD_MASTER_SWAP_KANIM);
		bool should_use_sideways_symbol = this.ShouldUseSidewaysSymbol(this.m_controller);
		if (this.m_blinkMonitor == null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes);
			this.m_blinkMonitor = this.m_accessorizer.GetSMI<BlinkMonitor.Instance>();
			if (this.m_blinkMonitor != null)
			{
				this.m_blinkMonitor.eye_anim = accessory.Name;
			}
		}
		if (this.m_speechMonitor == null)
		{
			this.m_speechMonitor = this.m_accessorizer.GetSMI<SpeechMonitor.Instance>();
		}
		if (this.m_blinkMonitor.IsNullOrStopped() || !this.m_blinkMonitor.IsBlinking())
		{
			KAnim.Build.Symbol symbol = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Eyes).symbol;
			this.ApplyShape(symbol, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_EYES, should_use_sideways_symbol);
		}
		if (this.m_speechMonitor.IsNullOrStopped() || !this.m_speechMonitor.IsPlayingSpeech())
		{
			KAnim.Build.Symbol symbol2 = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.Mouth).symbol;
			this.ApplyShape(symbol2, this.m_controller, anim, FaceGraph.ANIM_HASH_SNAPTO_MOUTH, should_use_sideways_symbol);
			return;
		}
		this.m_speechMonitor.DrawMouth();
	}

	// Token: 0x06007080 RID: 28800 RVA: 0x00305C3C File Offset: 0x00303E3C
	private bool ShouldUseSidewaysSymbol(KBatchedAnimController controller)
	{
		KAnim.Anim currentAnim = controller.GetCurrentAnim();
		if (currentAnim == null)
		{
			return false;
		}
		int currentFrameIndex = controller.GetCurrentFrameIndex();
		if (currentFrameIndex <= 0)
		{
			return false;
		}
		KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(currentAnim.animFile.animBatchTag);
		KAnim.Anim.Frame frame;
		batchGroupData.TryGetFrame(currentFrameIndex, out frame);
		for (int i = 0; i < frame.numElements; i++)
		{
			KAnim.Anim.FrameElement frameElement = batchGroupData.GetFrameElement(frame.firstElementIdx + i);
			if (frameElement.symbol == FaceGraph.ANIM_HASH_SNAPTO_EYES && frameElement.frame >= FaceGraph.FIRST_SIDEWAYS_FRAME)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06007081 RID: 28801 RVA: 0x00305CCC File Offset: 0x00303ECC
	private void ApplyShape(KAnim.Build.Symbol variation_symbol, KBatchedAnimController controller, KAnimFile shapes_file, KAnimHashedString symbol_name_in_shape_file, bool should_use_sideways_symbol)
	{
		HashedString hashedString = FaceGraph.ANIM_HASH_NEUTRAL;
		if (this.currentExpression != null)
		{
			hashedString = this.currentExpression.face.hash;
		}
		KAnim.Anim anim = null;
		KAnim.Anim.FrameElement frameElement = default(KAnim.Anim.FrameElement);
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		while (num < shapes_file.GetData().animCount && !flag)
		{
			KAnim.Anim anim2 = shapes_file.GetData().GetAnim(num);
			if (anim2.hash == hashedString)
			{
				anim = anim2;
				KAnim.Anim.Frame frame;
				if (anim.TryGetFrame(shapes_file.GetData().build.batchTag, 0, out frame))
				{
					for (int i = 0; i < frame.numElements; i++)
					{
						frameElement = KAnimBatchManager.Instance().GetBatchGroupData(shapes_file.GetData().animBatchTag).GetFrameElement(frame.firstElementIdx + i);
						if (!(frameElement.symbol != symbol_name_in_shape_file))
						{
							if (flag2 || !should_use_sideways_symbol)
							{
								flag = true;
							}
							flag2 = true;
							break;
						}
					}
				}
			}
			num++;
		}
		if (anim == null)
		{
			DebugUtil.Assert(false, "Could not find shape for expression: " + HashCache.Get().Get(hashedString));
		}
		if (!flag2)
		{
			DebugUtil.Assert(false, "Could not find shape element for shape:" + HashCache.Get().Get(variation_symbol.hash));
		}
		KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(controller.batchGroupID).GetSymbol(symbol_name_in_shape_file);
		KAnim.Build.SymbolFrameInstance symbolFrameInstance = KAnimBatchManager.Instance().GetBatchGroupData(variation_symbol.build.batchTag).symbolFrameInstances[variation_symbol.firstFrameIdx + frameElement.frame];
		symbolFrameInstance.buildImageIdx = this.m_symbolOverrideController.GetAtlasIdx(variation_symbol.build.GetTexture(0));
		controller.SetSymbolOverride(symbol.firstFrameIdx, ref symbolFrameInstance);
	}

	// Token: 0x06007082 RID: 28802 RVA: 0x00305E7C File Offset: 0x0030407C
	private void UpdateFace()
	{
		Expression expression = null;
		if (this.overrideExpression != null)
		{
			expression = this.overrideExpression;
		}
		else if (this.expressions.Count > 0)
		{
			this.expressions.Sort((Expression a, Expression b) => b.priority.CompareTo(a.priority));
			expression = this.expressions[0];
		}
		if (expression != this.currentExpression || expression == null)
		{
			this.currentExpression = expression;
			this.m_symbolOverrideController.MarkDirty();
		}
		AccessorySlot headEffects = Db.Get().AccessorySlots.HeadEffects;
		if (this.currentExpression != null)
		{
			Accessory accessory = this.m_accessorizer.GetAccessory(Db.Get().AccessorySlots.HeadEffects);
			HashedString hashedString = HashedString.Invalid;
			foreach (Expression expression2 in this.expressions)
			{
				if (expression2.face.headFXHash.IsValid)
				{
					hashedString = expression2.face.headFXHash;
					break;
				}
			}
			Accessory accessory2 = (hashedString != HashedString.Invalid) ? headEffects.Lookup(hashedString) : null;
			if (accessory != accessory2)
			{
				if (accessory != null)
				{
					this.m_accessorizer.RemoveAccessory(accessory);
				}
				if (accessory2 != null)
				{
					this.m_accessorizer.AddAccessory(accessory2);
				}
			}
			this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, accessory2 != null);
			return;
		}
		this.m_controller.SetSymbolVisiblity(headEffects.targetSymbolId, false);
	}

	// Token: 0x0400548A RID: 21642
	private List<Expression> expressions = new List<Expression>();

	// Token: 0x0400548D RID: 21645
	[MyCmpGet]
	private KBatchedAnimController m_controller;

	// Token: 0x0400548E RID: 21646
	[MyCmpGet]
	private Accessorizer m_accessorizer;

	// Token: 0x0400548F RID: 21647
	[MyCmpGet]
	private SymbolOverrideController m_symbolOverrideController;

	// Token: 0x04005490 RID: 21648
	private BlinkMonitor.Instance m_blinkMonitor;

	// Token: 0x04005491 RID: 21649
	private SpeechMonitor.Instance m_speechMonitor;

	// Token: 0x04005492 RID: 21650
	private static HashedString HASH_HEAD_MASTER_SWAP_KANIM = "head_master_swap_kanim";

	// Token: 0x04005493 RID: 21651
	private static KAnimHashedString ANIM_HASH_SNAPTO_EYES = "snapto_eyes";

	// Token: 0x04005494 RID: 21652
	private static KAnimHashedString ANIM_HASH_SNAPTO_MOUTH = "snapto_mouth";

	// Token: 0x04005495 RID: 21653
	private static KAnimHashedString ANIM_HASH_NEUTRAL = "neutral";

	// Token: 0x04005496 RID: 21654
	private static int FIRST_SIDEWAYS_FRAME = 29;
}
