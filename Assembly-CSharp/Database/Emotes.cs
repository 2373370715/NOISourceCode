using System;
using Klei.AI;

namespace Database
{
	// Token: 0x020021A3 RID: 8611
	public class Emotes : ResourceSet<Resource>
	{
		// Token: 0x0600B7E5 RID: 47077 RVA: 0x0011B3E3 File Offset: 0x001195E3
		public Emotes(ResourceSet parent) : base("Emotes", parent)
		{
			this.Minion = new Emotes.MinionEmotes(this);
			this.Critter = new Emotes.CritterEmotes(this);
		}

		// Token: 0x0600B7E6 RID: 47078 RVA: 0x00469A5C File Offset: 0x00467C5C
		public void ResetProblematicReferences()
		{
			for (int i = 0; i < this.Minion.resources.Count; i++)
			{
				Emote emote = this.Minion.resources[i];
				for (int j = 0; j < emote.StepCount; j++)
				{
					emote[j].UnregisterAllCallbacks();
				}
			}
			for (int k = 0; k < this.Critter.resources.Count; k++)
			{
				Emote emote2 = this.Critter.resources[k];
				for (int l = 0; l < emote2.StepCount; l++)
				{
					emote2[l].UnregisterAllCallbacks();
				}
			}
		}

		// Token: 0x040094BF RID: 38079
		public Emotes.MinionEmotes Minion;

		// Token: 0x040094C0 RID: 38080
		public Emotes.CritterEmotes Critter;

		// Token: 0x020021A4 RID: 8612
		public class MinionEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600B7E7 RID: 47079 RVA: 0x0011B409 File Offset: 0x00119609
			public MinionEmotes(ResourceSet parent) : base("Minion", parent)
			{
				this.InitializeCelebrations();
				this.InitializePhysicalStatus();
				this.InitializeEmotionalStatus();
				this.InitializeGreetings();
			}

			// Token: 0x0600B7E8 RID: 47080 RVA: 0x00469B08 File Offset: 0x00467D08
			public void InitializeCelebrations()
			{
				this.ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "clapcheer_pre"
					},
					new EmoteStep
					{
						anim = "clapcheer_loop"
					},
					new EmoteStep
					{
						anim = "clapcheer_pst"
					}
				}, "anim_clapcheer_kanim");
				this.Cheer = new Emote(this, "Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cheer_pre"
					},
					new EmoteStep
					{
						anim = "cheer_loop"
					},
					new EmoteStep
					{
						anim = "cheer_pst"
					}
				}, "anim_cheer_kanim");
				this.ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "productive"
					}
				}, "anim_productive_kanim");
				this.ResearchComplete = new Emote(this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
				this.ThumbsUp = new Emote(this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim");
			}

			// Token: 0x0600B7E9 RID: 47081 RVA: 0x00469C48 File Offset: 0x00467E48
			private void InitializePhysicalStatus()
			{
				this.CloseCall_Fall = new Emote(this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
				this.Cold = new Emote(this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_cold_kanim");
				this.Cough = new Emote(this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim");
				this.Cough_Small = new Emote(this, "Small Cough", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_small"
					}
				}, "anim_slimelungcough_kanim");
				this.FoodPoisoning = new Emote(this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
				this.Hot = new Emote(this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
				this.IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "irritated_eyes"
					}
				}, "anim_irritated_eyes_kanim");
				this.MorningStretch = new Emote(this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
				this.Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_glare"
					}
				}, "anim_react_radiation_kanim");
				this.Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_itch"
					}
				}, "anim_react_radiation_kanim");
				this.Sick = new Emote(this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
				this.Sneeze = new Emote(this, "Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze"
					},
					new EmoteStep
					{
						anim = "sneeze_pst"
					}
				}, "anim_sneeze_kanim");
				this.WaterDamage = new Emote(this, "WaterDamage", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "zapped"
					}
				}, "anim_bionic_kanim");
				this.GrindingGears = new Emote(this, "GrindingGears", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react"
					}
				}, "anim_bionic_react_grinding_gears_kanim");
				this.Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze_short"
					},
					new EmoteStep
					{
						anim = "sneeze_short_pst"
					}
				}, "anim_sneeze_kanim");
			}

			// Token: 0x0600B7EA RID: 47082 RVA: 0x00469EE4 File Offset: 0x004680E4
			private void InitializeEmotionalStatus()
			{
				this.Concern = new Emote(this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
				this.Cringe = new Emote(this, "Cringe", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cringe_pre"
					},
					new EmoteStep
					{
						anim = "cringe_loop"
					},
					new EmoteStep
					{
						anim = "cringe_pst"
					}
				}, "anim_cringe_kanim");
				this.Disappointed = new Emote(this, "Disappointed", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_disappointed_kanim");
				this.Shock = new Emote(this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
				this.Sing = new Emote(this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
			}

			// Token: 0x0600B7EB RID: 47083 RVA: 0x00469FC4 File Offset: 0x004681C4
			private void InitializeGreetings()
			{
				this.FingerGuns = new Emote(this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
				this.Wave = new Emote(this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
				this.Wave_Shy = new Emote(this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
			}

			// Token: 0x040094C1 RID: 38081
			private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			};

			// Token: 0x040094C2 RID: 38082
			private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "idle_pre"
				},
				new EmoteStep
				{
					anim = "idle_default"
				},
				new EmoteStep
				{
					anim = "idle_pst"
				}
			};

			// Token: 0x040094C3 RID: 38083
			public Emote ClapCheer;

			// Token: 0x040094C4 RID: 38084
			public Emote Cheer;

			// Token: 0x040094C5 RID: 38085
			public Emote ProductiveCheer;

			// Token: 0x040094C6 RID: 38086
			public Emote ResearchComplete;

			// Token: 0x040094C7 RID: 38087
			public Emote ThumbsUp;

			// Token: 0x040094C8 RID: 38088
			public Emote CloseCall_Fall;

			// Token: 0x040094C9 RID: 38089
			public Emote Cold;

			// Token: 0x040094CA RID: 38090
			public Emote Cough;

			// Token: 0x040094CB RID: 38091
			public Emote Cough_Small;

			// Token: 0x040094CC RID: 38092
			public Emote FoodPoisoning;

			// Token: 0x040094CD RID: 38093
			public Emote Hot;

			// Token: 0x040094CE RID: 38094
			public Emote IritatedEyes;

			// Token: 0x040094CF RID: 38095
			public Emote MorningStretch;

			// Token: 0x040094D0 RID: 38096
			public Emote Radiation_Glare;

			// Token: 0x040094D1 RID: 38097
			public Emote Radiation_Itch;

			// Token: 0x040094D2 RID: 38098
			public Emote Sick;

			// Token: 0x040094D3 RID: 38099
			public Emote Sneeze;

			// Token: 0x040094D4 RID: 38100
			public Emote WaterDamage;

			// Token: 0x040094D5 RID: 38101
			public Emote Sneeze_Short;

			// Token: 0x040094D6 RID: 38102
			public Emote GrindingGears;

			// Token: 0x040094D7 RID: 38103
			public Emote Concern;

			// Token: 0x040094D8 RID: 38104
			public Emote Cringe;

			// Token: 0x040094D9 RID: 38105
			public Emote Disappointed;

			// Token: 0x040094DA RID: 38106
			public Emote Shock;

			// Token: 0x040094DB RID: 38107
			public Emote Sing;

			// Token: 0x040094DC RID: 38108
			public Emote FingerGuns;

			// Token: 0x040094DD RID: 38109
			public Emote Wave;

			// Token: 0x040094DE RID: 38110
			public Emote Wave_Shy;
		}

		// Token: 0x020021A5 RID: 8613
		public class CritterEmotes : ResourceSet<Emote>
		{
			// Token: 0x0600B7ED RID: 47085 RVA: 0x0011B42F File Offset: 0x0011962F
			public CritterEmotes(ResourceSet parent) : base("Critter", parent)
			{
				this.InitializePhysicalState();
				this.InitializeEmotionalState();
			}

			// Token: 0x0600B7EE RID: 47086 RVA: 0x0046A0A8 File Offset: 0x004682A8
			private void InitializePhysicalState()
			{
				this.Hungry = new Emote(this, "Hungry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_hungry"
					}
				}, null);
			}

			// Token: 0x0600B7EF RID: 47087 RVA: 0x0046A0E8 File Offset: 0x004682E8
			private void InitializeEmotionalState()
			{
				this.Angry = new Emote(this, "Angry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_angry"
					}
				}, null);
				this.Happy = new Emote(this, "Happy", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_happy"
					}
				}, null);
				this.Idle = new Emote(this, "Idle", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_idle"
					}
				}, null);
				this.Sad = new Emote(this, "Sad", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_sad"
					}
				}, null);
			}

			// Token: 0x040094DF RID: 38111
			public Emote Hungry;

			// Token: 0x040094E0 RID: 38112
			public Emote Angry;

			// Token: 0x040094E1 RID: 38113
			public Emote Happy;

			// Token: 0x040094E2 RID: 38114
			public Emote Idle;

			// Token: 0x040094E3 RID: 38115
			public Emote Sad;
		}
	}
}
