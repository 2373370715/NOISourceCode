using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020007DC RID: 2012
public class HappySinger : GameStateMachine<HappySinger, HappySinger.Instance>
{
	// Token: 0x06002391 RID: 9105 RVA: 0x001D26E8 File Offset: 0x001D08E8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false);
		this.overjoyed.DefaultState(this.overjoyed.idle).TagTransition(GameTags.Overjoyed, this.neutral, true).ToggleEffect("IsJoySinger").ToggleLoopingSound(this.soundPath, null, true, true, true).ToggleAnims("anim_loco_singer_kanim", 0f).ToggleAnims("anim_idle_singer_kanim", 0f).EventHandler(GameHashes.TagsChanged, delegate(HappySinger.Instance smi, object obj)
		{
			if (smi.musicParticleFX != null)
			{
				smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
			}
		}).Enter(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX = Util.KInstantiate(EffectPrefabs.Instance.HappySingerFX, smi.master.transform.GetPosition() + this.offset);
			smi.musicParticleFX.transform.SetParent(smi.master.transform);
			smi.CreatePasserbyReactable();
			smi.musicParticleFX.SetActive(!smi.HasTag(GameTags.Asleep));
		}).Update(delegate(HappySinger.Instance smi, float dt)
		{
			if (!smi.GetSpeechMonitor().IsPlayingSpeech() && SpeechMonitor.IsAllowedToPlaySpeech(smi.gameObject))
			{
				smi.GetSpeechMonitor().PlaySpeech(Db.Get().Thoughts.CatchyTune.speechPrefix, Db.Get().Thoughts.CatchyTune.sound);
			}
		}, UpdateRate.SIM_1000ms, false).Exit(delegate(HappySinger.Instance smi)
		{
			smi.musicParticleFX.SetActive(false);
			Util.KDestroyGameObject(smi.musicParticleFX);
			smi.ClearPasserbyReactable();
		});
	}

	// Token: 0x040017E2 RID: 6114
	private Vector3 offset = new Vector3(0f, 0f, 0.1f);

	// Token: 0x040017E3 RID: 6115
	public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x040017E4 RID: 6116
	public HappySinger.OverjoyedStates overjoyed;

	// Token: 0x040017E5 RID: 6117
	public string soundPath = GlobalAssets.GetSound("DupeSinging_NotesFX_LP", false);

	// Token: 0x020007DD RID: 2013
	public class OverjoyedStates : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x040017E6 RID: 6118
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x040017E7 RID: 6119
		public GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.State moving;
	}

	// Token: 0x020007DE RID: 2014
	public new class Instance : GameStateMachine<HappySinger, HappySinger.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06002395 RID: 9109 RVA: 0x000BB94D File Offset: 0x000B9B4D
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06002396 RID: 9110 RVA: 0x001D2884 File Offset: 0x001D0A84
		public void CreatePasserbyReactable()
		{
			if (this.passerbyReactable == null)
			{
				EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 0f, 600f, float.PositiveInfinity, 0f);
				Emote sing = Db.Get().Emotes.Minion.Sing;
				emoteReactable.SetEmote(sing).SetThought(Db.Get().Thoughts.CatchyTune).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor));
				emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.AddReactionEffect), null);
				this.passerbyReactable = emoteReactable;
			}
		}

		// Token: 0x06002397 RID: 9111 RVA: 0x000BB956 File Offset: 0x000B9B56
		public SpeechMonitor.Instance GetSpeechMonitor()
		{
			if (this.speechMonitor == null)
			{
				this.speechMonitor = base.master.gameObject.GetSMI<SpeechMonitor.Instance>();
			}
			return this.speechMonitor;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x000BB97C File Offset: 0x000B9B7C
		private void AddReactionEffect(GameObject reactor)
		{
			reactor.Trigger(-1278274506, null);
		}

		// Token: 0x06002399 RID: 9113 RVA: 0x000BB98A File Offset: 0x000B9B8A
		private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
		{
			return transition.end == NavType.Floor;
		}

		// Token: 0x0600239A RID: 9114 RVA: 0x000BB995 File Offset: 0x000B9B95
		public void ClearPasserbyReactable()
		{
			if (this.passerbyReactable != null)
			{
				this.passerbyReactable.Cleanup();
				this.passerbyReactable = null;
			}
		}

		// Token: 0x040017E8 RID: 6120
		private Reactable passerbyReactable;

		// Token: 0x040017E9 RID: 6121
		public GameObject musicParticleFX;

		// Token: 0x040017EA RID: 6122
		public SpeechMonitor.Instance speechMonitor;
	}
}
