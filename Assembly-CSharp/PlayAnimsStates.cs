using System;

// Token: 0x02000140 RID: 320
public class PlayAnimsStates : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>
{
	// Token: 0x060004AC RID: 1196 RVA: 0x0015FBDC File Offset: 0x0015DDDC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.animating;
		GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State root = this.root;
		string name = "Unused";
		string tooltip = "Unused";
		string icon = "";
		StatusItem.IconType icon_type = StatusItem.IconType.Info;
		NotificationType notification_type = NotificationType.Neutral;
		bool allow_multiples = false;
		StatusItemCategory main = Db.Get().StatusItemCategories.Main;
		root.ToggleStatusItem(name, tooltip, icon, icon_type, notification_type, allow_multiples, default(HashedString), 129022, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemName, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemTooltip, main);
		this.animating.Enter("PlayAnims", delegate(PlayAnimsStates.Instance smi)
		{
			smi.PlayAnims();
		}).OnAnimQueueComplete(this.done).EventHandler(GameHashes.TagsChanged, delegate(PlayAnimsStates.Instance smi, object obj)
		{
			smi.HandleTagsChanged(obj);
		});
		this.done.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((PlayAnimsStates.Instance smi) => smi.def.tag, false);
	}

	// Token: 0x04000361 RID: 865
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State animating;

	// Token: 0x04000362 RID: 866
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State done;

	// Token: 0x02000141 RID: 321
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x060004AE RID: 1198 RVA: 0x000ABD04 File Offset: 0x000A9F04
		public Def(Tag tag, bool loop, string anim, string status_item_name, string status_item_tooltip) : this(tag, loop, new string[]
		{
			anim
		}, status_item_name, status_item_tooltip)
		{
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x000ABD1C File Offset: 0x000A9F1C
		public Def(Tag tag, bool loop, string[] anims, string status_item_name, string status_item_tooltip)
		{
			this.tag = tag;
			this.loop = loop;
			this.anims = anims;
			this.statusItemName = status_item_name;
			this.statusItemTooltip = status_item_tooltip;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x000ABD49 File Offset: 0x000A9F49
		public override string ToString()
		{
			return this.tag.ToString() + "(PlayAnimsStates)";
		}

		// Token: 0x04000363 RID: 867
		public Tag tag;

		// Token: 0x04000364 RID: 868
		public string[] anims;

		// Token: 0x04000365 RID: 869
		public bool loop;

		// Token: 0x04000366 RID: 870
		public string statusItemName;

		// Token: 0x04000367 RID: 871
		public string statusItemTooltip;
	}

	// Token: 0x02000142 RID: 322
	public new class Instance : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.GameInstance
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x000ABD66 File Offset: 0x000A9F66
		public Instance(Chore<PlayAnimsStates.Instance> chore, PlayAnimsStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.tag);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0015FD08 File Offset: 0x0015DF08
		public void PlayAnims()
		{
			if (base.def.anims == null || base.def.anims.Length == 0)
			{
				return;
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < base.def.anims.Length; i++)
			{
				KAnim.PlayMode mode = KAnim.PlayMode.Once;
				if (base.def.loop && i == base.def.anims.Length - 1)
				{
					mode = KAnim.PlayMode.Loop;
				}
				if (i == 0)
				{
					component.Play(base.def.anims[i], mode, 1f, 0f);
				}
				else
				{
					component.Queue(base.def.anims[i], mode, 1f, 0f);
				}
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000ABD8B File Offset: 0x000A9F8B
		public void HandleTagsChanged(object obj)
		{
			if (!base.smi.HasTag(base.smi.def.tag))
			{
				base.smi.GoTo(null);
			}
		}
	}
}
