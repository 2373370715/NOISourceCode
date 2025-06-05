using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02001291 RID: 4753
[AddComponentMenu("KMonoBehaviour/scripts/DupeGreetingManager")]
public class DupeGreetingManager : KMonoBehaviour, ISim200ms
{
	// Token: 0x06006117 RID: 24855 RVA: 0x000E3AA3 File Offset: 0x000E1CA3
	protected override void OnPrefabInit()
	{
		this.candidateCells = new Dictionary<int, MinionIdentity>();
		this.activeSetups = new List<DupeGreetingManager.GreetingSetup>();
		this.cooldowns = new Dictionary<MinionIdentity, float>();
	}

	// Token: 0x06006118 RID: 24856 RVA: 0x002BE528 File Offset: 0x002BC728
	public void Sim200ms(float dt)
	{
		if (GameClock.Instance.GetTime() / 600f < TuningData<DupeGreetingManager.Tuning>.Get().cyclesBeforeFirstGreeting)
		{
			return;
		}
		for (int i = this.activeSetups.Count - 1; i >= 0; i--)
		{
			DupeGreetingManager.GreetingSetup greetingSetup = this.activeSetups[i];
			if (!this.ValidNavigatingMinion(greetingSetup.A.minion) || !this.ValidOppositionalMinion(greetingSetup.A.minion, greetingSetup.B.minion))
			{
				greetingSetup.A.reactable.Cleanup();
				greetingSetup.B.reactable.Cleanup();
				this.activeSetups.RemoveAt(i);
			}
		}
		this.candidateCells.Clear();
		foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
		{
			if ((!this.cooldowns.ContainsKey(minionIdentity) || GameClock.Instance.GetTime() - this.cooldowns[minionIdentity] >= 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier) && this.ValidNavigatingMinion(minionIdentity))
			{
				for (int j = 0; j <= 2; j++)
				{
					int offsetCell = this.GetOffsetCell(minionIdentity, j);
					if (this.candidateCells.ContainsKey(offsetCell) && this.ValidOppositionalMinion(minionIdentity, this.candidateCells[offsetCell]))
					{
						this.BeginNewGreeting(minionIdentity, this.candidateCells[offsetCell], offsetCell);
						break;
					}
					this.candidateCells[offsetCell] = minionIdentity;
				}
			}
		}
	}

	// Token: 0x06006119 RID: 24857 RVA: 0x000E3AC6 File Offset: 0x000E1CC6
	private int GetOffsetCell(MinionIdentity minion, int offset)
	{
		if (!minion.GetComponent<Facing>().GetFacing())
		{
			return Grid.OffsetCell(Grid.PosToCell(minion), offset, 0);
		}
		return Grid.OffsetCell(Grid.PosToCell(minion), -offset, 0);
	}

	// Token: 0x0600611A RID: 24858 RVA: 0x002BE6D0 File Offset: 0x002BC8D0
	private bool ValidNavigatingMinion(MinionIdentity minion)
	{
		if (minion == null)
		{
			return false;
		}
		Navigator component = minion.GetComponent<Navigator>();
		return component != null && component.IsMoving() && component.CurrentNavType == NavType.Floor;
	}

	// Token: 0x0600611B RID: 24859 RVA: 0x002BE70C File Offset: 0x002BC90C
	private bool ValidOppositionalMinion(MinionIdentity reference_minion, MinionIdentity minion)
	{
		if (reference_minion == null)
		{
			return false;
		}
		if (minion == null)
		{
			return false;
		}
		Facing component = minion.GetComponent<Facing>();
		Facing component2 = reference_minion.GetComponent<Facing>();
		return this.ValidNavigatingMinion(minion) && component != null && component2 != null && component.GetFacing() != component2.GetFacing();
	}

	// Token: 0x0600611C RID: 24860 RVA: 0x002BE76C File Offset: 0x002BC96C
	private void BeginNewGreeting(MinionIdentity minion_a, MinionIdentity minion_b, int cell)
	{
		DupeGreetingManager.GreetingSetup greetingSetup = new DupeGreetingManager.GreetingSetup();
		greetingSetup.cell = cell;
		greetingSetup.A = new DupeGreetingManager.GreetingUnit(minion_a, this.GetReactable(minion_a));
		greetingSetup.B = new DupeGreetingManager.GreetingUnit(minion_b, this.GetReactable(minion_b));
		this.activeSetups.Add(greetingSetup);
	}

	// Token: 0x0600611D RID: 24861 RVA: 0x002BE7B8 File Offset: 0x002BC9B8
	private Reactable GetReactable(MinionIdentity minion)
	{
		if (DupeGreetingManager.emotes == null)
		{
			DupeGreetingManager.emotes = new List<Emote>
			{
				Db.Get().Emotes.Minion.Wave,
				Db.Get().Emotes.Minion.Wave_Shy,
				Db.Get().Emotes.Minion.FingerGuns
			};
		}
		Emote emote = DupeGreetingManager.emotes[UnityEngine.Random.Range(0, DupeGreetingManager.emotes.Count)];
		SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(minion.gameObject, "NavigatorPassingGreeting", Db.Get().ChoreTypes.Emote, 1000f, 20f, float.PositiveInfinity, 0f);
		selfEmoteReactable.SetEmote(emote).SetThought(Db.Get().Thoughts.Chatty);
		selfEmoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.BeginReacting), null);
		return selfEmoteReactable;
	}

	// Token: 0x0600611E RID: 24862 RVA: 0x002BE8B4 File Offset: 0x002BCAB4
	private void BeginReacting(GameObject minionGO)
	{
		if (minionGO == null)
		{
			return;
		}
		MinionIdentity component = minionGO.GetComponent<MinionIdentity>();
		Vector3 vector = Vector3.zero;
		foreach (DupeGreetingManager.GreetingSetup greetingSetup in this.activeSetups)
		{
			if (greetingSetup.A.minion == component)
			{
				if (greetingSetup.B.minion != null)
				{
					vector = greetingSetup.B.minion.transform.GetPosition();
					greetingSetup.A.minion.Trigger(-594200555, greetingSetup.B.minion);
					greetingSetup.B.minion.Trigger(-594200555, greetingSetup.A.minion);
					break;
				}
				break;
			}
			else if (greetingSetup.B.minion == component)
			{
				if (greetingSetup.A.minion != null)
				{
					vector = greetingSetup.A.minion.transform.GetPosition();
					break;
				}
				break;
			}
		}
		minionGO.GetComponent<Facing>().SetFacing(vector.x < minionGO.transform.GetPosition().x);
		minionGO.GetComponent<Effects>().Add("Greeting", true);
		this.cooldowns[component] = GameClock.Instance.GetTime();
	}

	// Token: 0x04004561 RID: 17761
	private const float COOLDOWN_TIME = 720f;

	// Token: 0x04004562 RID: 17762
	private Dictionary<int, MinionIdentity> candidateCells;

	// Token: 0x04004563 RID: 17763
	private List<DupeGreetingManager.GreetingSetup> activeSetups;

	// Token: 0x04004564 RID: 17764
	private Dictionary<MinionIdentity, float> cooldowns;

	// Token: 0x04004565 RID: 17765
	private static List<Emote> emotes;

	// Token: 0x02001292 RID: 4754
	public class Tuning : TuningData<DupeGreetingManager.Tuning>
	{
		// Token: 0x04004566 RID: 17766
		public float cyclesBeforeFirstGreeting;

		// Token: 0x04004567 RID: 17767
		public float greetingDelayMultiplier;
	}

	// Token: 0x02001293 RID: 4755
	private class GreetingUnit
	{
		// Token: 0x06006121 RID: 24865 RVA: 0x000E3AF9 File Offset: 0x000E1CF9
		public GreetingUnit(MinionIdentity minion, Reactable reactable)
		{
			this.minion = minion;
			this.reactable = reactable;
		}

		// Token: 0x04004568 RID: 17768
		public MinionIdentity minion;

		// Token: 0x04004569 RID: 17769
		public Reactable reactable;
	}

	// Token: 0x02001294 RID: 4756
	private class GreetingSetup
	{
		// Token: 0x0400456A RID: 17770
		public int cell;

		// Token: 0x0400456B RID: 17771
		public DupeGreetingManager.GreetingUnit A;

		// Token: 0x0400456C RID: 17772
		public DupeGreetingManager.GreetingUnit B;
	}
}
