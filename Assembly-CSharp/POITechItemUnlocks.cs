using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x020016E3 RID: 5859
public class POITechItemUnlocks : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>
{
	// Token: 0x060078E0 RID: 30944 RVA: 0x003213F4 File Offset: 0x0031F5F4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.locked;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.locked.PlayAnim("on", KAnim.PlayMode.Loop).ParamTransition<bool>(this.isUnlocked, this.unlocked, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.ParamTransition<bool>(this.seenNotification, this.unlocked.notify, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsFalse).ParamTransition<bool>(this.seenNotification, this.unlocked.done, GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.IsTrue);
		this.unlocked.notify.PlayAnim("notify", KAnim.PlayMode.Loop).ToggleStatusItem(Db.Get().MiscStatusItems.AttentionRequired, null).ToggleNotification(delegate(POITechItemUnlocks.Instance smi)
		{
			smi.notificationReference = EventInfoScreen.CreateNotification(POITechItemUnlocks.GenerateEventPopupData(smi), null);
			smi.notificationReference.Type = NotificationType.MessageImportant;
			return smi.notificationReference;
		});
		this.unlocked.done.PlayAnim("off");
	}

	// Token: 0x060078E1 RID: 30945 RVA: 0x003214DC File Offset: 0x0031F6DC
	private static string GetMessageBody(POITechItemUnlocks.Instance smi)
	{
		string text = "";
		foreach (TechItem techItem in smi.unlockTechItems)
		{
			text = text + "\n    • " + techItem.Name;
		}
		return string.Format(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.MESSAGEBODY, text);
	}

	// Token: 0x060078E2 RID: 30946 RVA: 0x00321550 File Offset: 0x0031F750
	private static EventInfoData GenerateEventPopupData(POITechItemUnlocks.Instance smi)
	{
		EventInfoData eventInfoData = new EventInfoData(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.NAME, POITechItemUnlocks.GetMessageBody(smi), smi.def.animName);
		int num = Mathf.Max(2, Components.LiveMinionIdentities.Count);
		GameObject[] array = new GameObject[num];
		using (IEnumerator<MinionIdentity> enumerator = Components.LiveMinionIdentities.Shuffle<MinionIdentity>().GetEnumerator())
		{
			for (int i = 0; i < num; i++)
			{
				if (!enumerator.MoveNext())
				{
					num = 0;
					array = new GameObject[num];
					break;
				}
				array[i] = enumerator.Current.gameObject;
			}
		}
		eventInfoData.minions = array;
		if (smi.def.loreUnlockId != null)
		{
			eventInfoData.AddOption(MISC.NOTIFICATIONS.POIRESEARCHUNLOCKCOMPLETE.BUTTON_VIEW_LORE, null).callback = delegate()
			{
				smi.sm.seenNotification.Set(true, smi, false);
				smi.notificationReference = null;
				Game.Instance.unlocks.Unlock(smi.def.loreUnlockId, true);
				ManagementMenu.Instance.OpenCodexToLockId(smi.def.loreUnlockId, false);
			};
		}
		eventInfoData.AddDefaultOption(delegate
		{
			smi.sm.seenNotification.Set(true, smi, false);
			smi.notificationReference = null;
		});
		eventInfoData.clickFocus = smi.gameObject.transform;
		return eventInfoData;
	}

	// Token: 0x04005AC2 RID: 23234
	public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State locked;

	// Token: 0x04005AC3 RID: 23235
	public POITechItemUnlocks.UnlockedStates unlocked;

	// Token: 0x04005AC4 RID: 23236
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter isUnlocked;

	// Token: 0x04005AC5 RID: 23237
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter pendingChore;

	// Token: 0x04005AC6 RID: 23238
	public StateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.BoolParameter seenNotification;

	// Token: 0x020016E4 RID: 5860
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005AC7 RID: 23239
		public List<string> POITechUnlockIDs;

		// Token: 0x04005AC8 RID: 23240
		public LocString PopUpName;

		// Token: 0x04005AC9 RID: 23241
		public string animName;

		// Token: 0x04005ACA RID: 23242
		public string loreUnlockId;
	}

	// Token: 0x020016E5 RID: 5861
	public new class Instance : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x060078E5 RID: 30949 RVA: 0x0032167C File Offset: 0x0031F87C
		public Instance(IStateMachineTarget master, POITechItemUnlocks.Def def) : base(master, def)
		{
			this.unlockTechItems = new List<TechItem>(def.POITechUnlockIDs.Count);
			foreach (string text in def.POITechUnlockIDs)
			{
				TechItem techItem = Db.Get().TechItems.TryGet(text);
				if (techItem != null)
				{
					this.unlockTechItems.Add(techItem);
				}
				else
				{
					DebugUtil.DevAssert(false, "Invalid tech item " + text + " for POI Tech Unlock", null);
				}
			}
		}

		// Token: 0x060078E6 RID: 30950 RVA: 0x00321720 File Offset: 0x0031F920
		public override void StartSM()
		{
			base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			this.UpdateUnlocked();
			base.StartSM();
			if (base.sm.pendingChore.Get(this) && this.unlockChore == null)
			{
				this.CreateChore();
			}
		}

		// Token: 0x060078E7 RID: 30951 RVA: 0x000F3EF6 File Offset: 0x000F20F6
		public override void StopSM(string reason)
		{
			base.Unsubscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
			base.StopSM(reason);
		}

		// Token: 0x060078E8 RID: 30952 RVA: 0x00321774 File Offset: 0x0031F974
		public void OnBuildingSelect(object obj)
		{
			if (!(bool)obj)
			{
				return;
			}
			if (!base.sm.seenNotification.Get(this) && this.notificationReference != null)
			{
				this.notificationReference.customClickCallback(this.notificationReference.customClickData);
			}
		}

		// Token: 0x060078E9 RID: 30953 RVA: 0x000AA038 File Offset: 0x000A8238
		private void ShowPopup()
		{
		}

		// Token: 0x060078EA RID: 30954 RVA: 0x003217C0 File Offset: 0x0031F9C0
		public void UnlockTechItems()
		{
			foreach (TechItem techItem in this.unlockTechItems)
			{
				if (techItem != null)
				{
					techItem.POIUnlocked();
				}
			}
			MusicManager.instance.PlaySong("Stinger_ResearchComplete", false);
			this.UpdateUnlocked();
		}

		// Token: 0x060078EB RID: 30955 RVA: 0x0032182C File Offset: 0x0031FA2C
		private void UpdateUnlocked()
		{
			bool value = true;
			using (List<TechItem>.Enumerator enumerator = this.unlockTechItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.IsComplete())
					{
						value = false;
						break;
					}
				}
			}
			base.sm.isUnlocked.Set(value, base.smi, false);
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x060078EC RID: 30956 RVA: 0x003218A0 File Offset: 0x0031FAA0
		public string SidescreenButtonText
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.ALREADY_RUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.NAME;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x060078ED RID: 30957 RVA: 0x003218F0 File Offset: 0x0031FAF0
		public string SidescreenButtonTooltip
		{
			get
			{
				if (base.sm.isUnlocked.Get(base.smi))
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_ALREADYRUMMAGED;
				}
				if (this.unlockChore != null)
				{
					return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP_OFF;
				}
				return UI.USERMENUACTIONS.OPEN_TECHUNLOCKS.TOOLTIP;
			}
		}

		// Token: 0x060078EE RID: 30958 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060078EF RID: 30959 RVA: 0x000F3F16 File Offset: 0x000F2116
		public bool SidescreenEnabled()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x060078F0 RID: 30960 RVA: 0x000F3F16 File Offset: 0x000F2116
		public bool SidescreenButtonInteractable()
		{
			return base.smi.IsInsideState(base.sm.locked);
		}

		// Token: 0x060078F1 RID: 30961 RVA: 0x00321940 File Offset: 0x0031FB40
		public void OnSidescreenButtonPressed()
		{
			if (this.unlockChore == null)
			{
				base.smi.sm.pendingChore.Set(true, base.smi, false);
				base.smi.CreateChore();
				return;
			}
			base.smi.sm.pendingChore.Set(false, base.smi, false);
			base.smi.CancelChore();
		}

		// Token: 0x060078F2 RID: 30962 RVA: 0x003219A8 File Offset: 0x0031FBA8
		private void CreateChore()
		{
			Workable component = base.smi.master.GetComponent<POITechItemUnlockWorkable>();
			Prioritizable.AddRef(base.gameObject);
			base.Trigger(1980521255, null);
			this.unlockChore = new WorkChore<POITechItemUnlockWorkable>(Db.Get().ChoreTypes.Research, component, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		}

		// Token: 0x060078F3 RID: 30963 RVA: 0x000F3F2E File Offset: 0x000F212E
		private void CancelChore()
		{
			this.unlockChore.Cancel("UserCancel");
			this.unlockChore = null;
			Prioritizable.RemoveRef(base.gameObject);
			base.Trigger(1980521255, null);
		}

		// Token: 0x060078F4 RID: 30964 RVA: 0x000AFE89 File Offset: 0x000AE089
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x060078F5 RID: 30965 RVA: 0x000AFED1 File Offset: 0x000AE0D1
		public int ButtonSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x04005ACB RID: 23243
		public List<TechItem> unlockTechItems;

		// Token: 0x04005ACC RID: 23244
		public Notification notificationReference;

		// Token: 0x04005ACD RID: 23245
		private Chore unlockChore;
	}

	// Token: 0x020016E6 RID: 5862
	public class UnlockedStates : GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State
	{
		// Token: 0x04005ACE RID: 23246
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State notify;

		// Token: 0x04005ACF RID: 23247
		public GameStateMachine<POITechItemUnlocks, POITechItemUnlocks.Instance, IStateMachineTarget, POITechItemUnlocks.Def>.State done;
	}
}
