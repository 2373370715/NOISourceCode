using System;
using Klei.AI;
using UnityEngine;

// Token: 0x02000831 RID: 2097
public class EmoteReactable : Reactable
{
	// Token: 0x060024EC RID: 9452 RVA: 0x001D7F68 File Offset: 0x001D6168
	public EmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, range_width, range_height, true, globalCooldown, localCooldown, lifeSpan, max_initial_delay, ObjectLayer.NumLayers)
	{
	}

	// Token: 0x060024ED RID: 9453 RVA: 0x000BC839 File Offset: 0x000BAA39
	public EmoteReactable SetEmote(Emote emote)
	{
		this.emote = emote;
		return this;
	}

	// Token: 0x060024EE RID: 9454 RVA: 0x001D7F94 File Offset: 0x001D6194
	public EmoteReactable RegisterEmoteStepCallbacks(HashedString stepName, Action<GameObject> startedCb, Action<GameObject> finishedCb)
	{
		if (this.callbackHandles == null)
		{
			this.callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[this.emote.StepCount];
		}
		int stepIndex = this.emote.GetStepIndex(stepName);
		this.callbackHandles[stepIndex] = this.emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
		return this;
	}

	// Token: 0x060024EF RID: 9455 RVA: 0x000BC843 File Offset: 0x000BAA43
	public EmoteReactable SetExpression(Expression expression)
	{
		this.expression = expression;
		return this;
	}

	// Token: 0x060024F0 RID: 9456 RVA: 0x000BC84D File Offset: 0x000BAA4D
	public EmoteReactable SetThought(Thought thought)
	{
		this.thought = thought;
		return this;
	}

	// Token: 0x060024F1 RID: 9457 RVA: 0x000BC857 File Offset: 0x000BAA57
	public EmoteReactable SetOverideAnimSet(string animSet)
	{
		this.overrideAnimSet = Assets.GetAnim(animSet);
		return this;
	}

	// Token: 0x060024F2 RID: 9458 RVA: 0x001D7FEC File Offset: 0x001D61EC
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null || new_reactor == null)
		{
			return false;
		}
		Navigator component = new_reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving() && (-257 & 1 << (int)component.CurrentNavType) != 0 && this.gameObject != new_reactor;
	}

	// Token: 0x060024F3 RID: 9459 RVA: 0x001D8050 File Offset: 0x001D6250
	public override void Update(float dt)
	{
		if (this.emote == null || !this.emote.IsValidStep(this.currentStep))
		{
			return;
		}
		if (this.gameObject != null && this.reactor != null)
		{
			Facing component = this.reactor.GetComponent<Facing>();
			if (component != null)
			{
				component.Face(this.gameObject.transform.GetPosition());
			}
		}
		float timeout = this.emote[this.currentStep].timeout;
		if (timeout > 0f && timeout < this.elapsed)
		{
			this.NextStep(null);
			return;
		}
		this.elapsed += dt;
	}

	// Token: 0x060024F4 RID: 9460 RVA: 0x001D8104 File Offset: 0x001D6304
	protected override void InternalBegin()
	{
		this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
		this.emote.ApplyAnimOverrides(this.kbac, this.overrideAnimSet);
		if (this.expression != null)
		{
			this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
		}
		if (this.thought != null)
		{
			this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
		}
		this.NextStep(null);
	}

	// Token: 0x060024F5 RID: 9461 RVA: 0x001D8184 File Offset: 0x001D6384
	protected override void InternalEnd()
	{
		if (this.kbac != null)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			this.emote.RemoveAnimOverrides(this.kbac, this.overrideAnimSet);
			this.kbac = null;
		}
		if (this.reactor != null)
		{
			if (this.expression != null)
			{
				this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			if (this.thought != null)
			{
				this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
			}
		}
		this.currentStep = -1;
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x001D8228 File Offset: 0x001D6428
	protected override void InternalCleanup()
	{
		if (this.emote == null || this.callbackHandles == null)
		{
			return;
		}
		int num = 0;
		while (this.emote.IsValidStep(num))
		{
			this.emote[num].UnregisterCallbacks(this.callbackHandles[num]);
			num++;
		}
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x001D827C File Offset: 0x001D647C
	private void NextStep(HashedString finishedAnim)
	{
		if (this.emote.IsValidStep(this.currentStep) && this.emote[this.currentStep].timeout <= 0f)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			if (this.callbackHandles != null)
			{
				this.emote[this.currentStep].OnStepFinished(this.callbackHandles[this.currentStep], this.reactor);
			}
		}
		this.currentStep++;
		if (!this.emote.IsValidStep(this.currentStep) || this.kbac == null)
		{
			base.End();
			return;
		}
		EmoteStep emoteStep = this.emote[this.currentStep];
		if (emoteStep.anim != HashedString.Invalid)
		{
			this.kbac.Play(emoteStep.anim, emoteStep.mode, 1f, 0f);
			if (this.kbac.IsStopped())
			{
				emoteStep.timeout = 0.25f;
			}
		}
		if (emoteStep.timeout <= 0f)
		{
			this.kbac.onAnimComplete += this.NextStep;
		}
		else
		{
			this.elapsed = 0f;
		}
		if (this.callbackHandles != null)
		{
			emoteStep.OnStepStarted(this.callbackHandles[this.currentStep], this.reactor);
		}
	}

	// Token: 0x0400197D RID: 6525
	private KBatchedAnimController kbac;

	// Token: 0x0400197E RID: 6526
	public Expression expression;

	// Token: 0x0400197F RID: 6527
	public Thought thought;

	// Token: 0x04001980 RID: 6528
	public Emote emote;

	// Token: 0x04001981 RID: 6529
	private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;

	// Token: 0x04001982 RID: 6530
	protected KAnimFile overrideAnimSet;

	// Token: 0x04001983 RID: 6531
	private int currentStep = -1;

	// Token: 0x04001984 RID: 6532
	private float elapsed;
}
