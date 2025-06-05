using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200191D RID: 6429
public class ClusterMapMeteorShower : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>
{
	// Token: 0x06008527 RID: 34087 RVA: 0x0035481C File Offset: 0x00352A1C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		default_state = this.traveling;
		this.traveling.DefaultState(this.traveling.unidentified).EventTransition(GameHashes.ClusterDestinationReached, this.arrived, null);
		this.traveling.unidentified.ParamTransition<bool>(this.IsIdentified, this.traveling.identified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsTrue);
		this.traveling.identified.ParamTransition<bool>(this.IsIdentified, this.traveling.unidentified, GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.IsFalse).ToggleStatusItem(Db.Get().MiscStatusItems.ClusterMeteorRemainingTravelTime, null);
		this.arrived.Enter(new StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State.Callback(ClusterMapMeteorShower.DestinationReached));
	}

	// Token: 0x06008528 RID: 34088 RVA: 0x000FBF90 File Offset: 0x000FA190
	public static void DestinationReached(ClusterMapMeteorShower.Instance smi)
	{
		smi.DestinationReached();
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x0400655E RID: 25950
	public StateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.BoolParameter IsIdentified;

	// Token: 0x0400655F RID: 25951
	public ClusterMapMeteorShower.TravelingState traveling;

	// Token: 0x04006560 RID: 25952
	public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State arrived;

	// Token: 0x0200191E RID: 6430
	public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
	{
		// Token: 0x0600852A RID: 34090 RVA: 0x003548DC File Offset: 0x00352ADC
		public List<Descriptor> GetDescriptors(GameObject go)
		{
			GameplayEvent gameplayEvent = Db.Get().GameplayEvents.Get(this.eventID);
			List<Descriptor> list = new List<Descriptor>();
			ClusterMapMeteorShower.Instance smi = go.GetSMI<ClusterMapMeteorShower.Instance>();
			if (smi != null && smi.sm.IsIdentified.Get(smi) && gameplayEvent is MeteorShowerEvent)
			{
				List<MeteorShowerEvent.BombardmentInfo> meteorsInfo = (gameplayEvent as MeteorShowerEvent).GetMeteorsInfo();
				float num = 0f;
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo in meteorsInfo)
				{
					num += bombardmentInfo.weight;
				}
				foreach (MeteorShowerEvent.BombardmentInfo bombardmentInfo2 in meteorsInfo)
				{
					GameObject prefab = Assets.GetPrefab(bombardmentInfo2.prefab);
					string formattedPercent = GameUtil.GetFormattedPercent((float)Mathf.RoundToInt(bombardmentInfo2.weight / num * 100f), GameUtil.TimeSlice.None);
					string txt = prefab.GetProperName() + " " + formattedPercent;
					Descriptor item = new Descriptor(txt, UI.GAMEOBJECTEFFECTS.TOOLTIPS.METEOR_SHOWER_SINGLE_METEOR_PERCENTAGE_TOOLTIP, Descriptor.DescriptorType.Effect, false);
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x04006561 RID: 25953
		public string name;

		// Token: 0x04006562 RID: 25954
		public string description;

		// Token: 0x04006563 RID: 25955
		public string description_Hidden;

		// Token: 0x04006564 RID: 25956
		public string name_Hidden;

		// Token: 0x04006565 RID: 25957
		public string eventID;

		// Token: 0x04006566 RID: 25958
		public int destinationWorldID;

		// Token: 0x04006567 RID: 25959
		public float arrivalTime;
	}

	// Token: 0x0200191F RID: 6431
	public class TravelingState : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State
	{
		// Token: 0x04006568 RID: 25960
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State unidentified;

		// Token: 0x04006569 RID: 25961
		public GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.State identified;
	}

	// Token: 0x02001920 RID: 6432
	public new class Instance : GameStateMachine<ClusterMapMeteorShower, ClusterMapMeteorShower.Instance, IStateMachineTarget, ClusterMapMeteorShower.Def>.GameInstance, ISidescreenButtonControl
	{
		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600852D RID: 34093 RVA: 0x000FBFB3 File Offset: 0x000FA1B3
		public WorldContainer World_Destination
		{
			get
			{
				return ClusterManager.Instance.GetWorld(this.DestinationWorldID);
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600852E RID: 34094 RVA: 0x000FBFC5 File Offset: 0x000FA1C5
		public string SidescreenButtonText
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identify";
				}
				return "Dev Hide";
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x0600852F RID: 34095 RVA: 0x000FBFEF File Offset: 0x000FA1EF
		public string SidescreenButtonTooltip
		{
			get
			{
				if (!base.smi.sm.IsIdentified.Get(base.smi))
				{
					return "Identifies the meteor shower";
				}
				return "Dev unidentify back";
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06008530 RID: 34096 RVA: 0x000FC019 File Offset: 0x000FA219
		public bool HasBeenIdentified
		{
			get
			{
				return base.sm.IsIdentified.Get(this);
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06008531 RID: 34097 RVA: 0x000FC02C File Offset: 0x000FA22C
		public float IdentifyingProgress
		{
			get
			{
				return this.identifyingProgress;
			}
		}

		// Token: 0x06008532 RID: 34098 RVA: 0x000FC034 File Offset: 0x000FA234
		public AxialI ClusterGridPosition()
		{
			return this.visualizer.Location;
		}

		// Token: 0x06008533 RID: 34099 RVA: 0x000FC041 File Offset: 0x000FA241
		public Instance(IStateMachineTarget master, ClusterMapMeteorShower.Def def) : base(master, def)
		{
			this.traveler.getSpeedCB = new Func<float>(this.GetSpeed);
			this.traveler.onTravelCB = new System.Action(this.OnTravellerMoved);
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x000FC080 File Offset: 0x000FA280
		private void OnTravellerMoved()
		{
			Game.Instance.Trigger(-1975776133, this);
		}

		// Token: 0x06008535 RID: 34101 RVA: 0x000FC092 File Offset: 0x000FA292
		protected override void OnCleanUp()
		{
			this.visualizer.Deselect();
			base.OnCleanUp();
		}

		// Token: 0x06008536 RID: 34102 RVA: 0x00354A28 File Offset: 0x00352C28
		public void Identify()
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress = 1f;
				base.sm.IsIdentified.Set(true, this, false);
				Game.Instance.Trigger(1427028915, this);
				this.RefreshVisuals(true);
				if (ClusterMapScreen.Instance.IsActive())
				{
					KFMOD.PlayUISound(GlobalAssets.GetSound("ClusterMapMeteor_Reveal", false));
				}
			}
		}

		// Token: 0x06008537 RID: 34103 RVA: 0x00354A90 File Offset: 0x00352C90
		public void ProgressIdentifiction(float points)
		{
			if (!this.HasBeenIdentified)
			{
				this.identifyingProgress += points;
				this.identifyingProgress = Mathf.Clamp(this.identifyingProgress, 0f, 1f);
				if (this.identifyingProgress == 1f)
				{
					this.Identify();
				}
			}
		}

		// Token: 0x06008538 RID: 34104 RVA: 0x000FC0A5 File Offset: 0x000FA2A5
		public override void StartSM()
		{
			base.StartSM();
			if (this.DestinationWorldID < 0)
			{
				this.Setup(base.def.destinationWorldID, base.def.arrivalTime);
			}
			this.RefreshVisuals(false);
		}

		// Token: 0x06008539 RID: 34105 RVA: 0x00354AE4 File Offset: 0x00352CE4
		public void RefreshVisuals(bool playIdentifyAnimationIfVisible = false)
		{
			if (this.HasBeenIdentified)
			{
				this.selectable.SetName(base.def.name);
				this.descriptor.description = base.def.description;
				this.visualizer.PlayRevealAnimation(playIdentifyAnimationIfVisible);
			}
			else
			{
				this.selectable.SetName(base.def.name_Hidden);
				this.descriptor.description = base.def.description_Hidden;
				this.visualizer.PlayHideAnimation();
			}
			base.Trigger(1980521255, null);
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00354B78 File Offset: 0x00352D78
		public void Setup(int destinationWorldID, float arrivalTime)
		{
			this.DestinationWorldID = destinationWorldID;
			this.ArrivalTime = arrivalTime;
			AxialI location = this.World_Destination.GetComponent<ClusterGridEntity>().Location;
			this.destinationSelector.SetDestination(location);
			this.traveler.RevalidatePath(false);
			int count = this.traveler.CurrentPath.Count;
			float num = arrivalTime - GameUtil.GetCurrentTimeInCycles() * 600f;
			this.Speed = (float)count / num * 600f;
		}

		// Token: 0x0600853B RID: 34107 RVA: 0x000FC0D9 File Offset: 0x000FA2D9
		public float GetSpeed()
		{
			return this.Speed;
		}

		// Token: 0x0600853C RID: 34108 RVA: 0x000FC0E1 File Offset: 0x000FA2E1
		public void DestinationReached()
		{
			System.Action onDestinationReached = this.OnDestinationReached;
			if (onDestinationReached == null)
			{
				return;
			}
			onDestinationReached();
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x000AFECA File Offset: 0x000AE0CA
		public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x000B1628 File Offset: 0x000AF828
		public bool SidescreenEnabled()
		{
			return false;
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x000AA7E7 File Offset: 0x000A89E7
		public bool SidescreenButtonInteractable()
		{
			return true;
		}

		// Token: 0x06008540 RID: 34112 RVA: 0x000FC0F3 File Offset: 0x000FA2F3
		public void OnSidescreenButtonPressed()
		{
			this.Identify();
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x000AFE89 File Offset: 0x000AE089
		public int HorizontalGroupID()
		{
			return -1;
		}

		// Token: 0x06008542 RID: 34114 RVA: 0x000FC0FB File Offset: 0x000FA2FB
		public int ButtonSideScreenSortOrder()
		{
			return SORTORDER.KEEPSAKES;
		}

		// Token: 0x0400656A RID: 25962
		[Serialize]
		public int DestinationWorldID = -1;

		// Token: 0x0400656B RID: 25963
		[Serialize]
		public float ArrivalTime;

		// Token: 0x0400656C RID: 25964
		[Serialize]
		private float Speed;

		// Token: 0x0400656D RID: 25965
		[Serialize]
		private float identifyingProgress;

		// Token: 0x0400656E RID: 25966
		public System.Action OnDestinationReached;

		// Token: 0x0400656F RID: 25967
		[MyCmpGet]
		private InfoDescription descriptor;

		// Token: 0x04006570 RID: 25968
		[MyCmpGet]
		private KSelectable selectable;

		// Token: 0x04006571 RID: 25969
		[MyCmpGet]
		private ClusterMapMeteorShowerVisualizer visualizer;

		// Token: 0x04006572 RID: 25970
		[MyCmpGet]
		private ClusterTraveler traveler;

		// Token: 0x04006573 RID: 25971
		[MyCmpGet]
		private ClusterDestinationSelector destinationSelector;
	}
}
