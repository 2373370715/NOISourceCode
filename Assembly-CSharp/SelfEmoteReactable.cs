using System;
using UnityEngine;

// Token: 0x02000838 RID: 2104
public class SelfEmoteReactable : EmoteReactable
{
	// Token: 0x0600252D RID: 9517 RVA: 0x001D8C08 File Offset: 0x001D6E08
	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x001D8C28 File Offset: 0x001D6E28
	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != this.gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000BCAE5 File Offset: 0x000BACE5
	public void PairEmote(EmoteChore emoteChore)
	{
		this.chore = emoteChore;
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x001D8C60 File Offset: 0x001D6E60
	protected override void InternalEnd()
	{
		if (this.chore != null && this.chore.driver != null)
		{
			this.chore.PairReactable(null);
			this.chore.Cancel("Reactable ended");
			this.chore = null;
		}
		base.InternalEnd();
	}

	// Token: 0x040019A7 RID: 6567
	private EmoteChore chore;
}
