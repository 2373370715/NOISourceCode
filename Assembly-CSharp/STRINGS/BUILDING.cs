using System;

namespace STRINGS
{
	// Token: 0x02003130 RID: 12592
	public class BUILDING
	{
		// Token: 0x02003131 RID: 12593
		public class STATUSITEMS
		{
			// Token: 0x02003132 RID: 12594
			public class GUNKEMPTIERFULL
			{
				// Token: 0x0400CA85 RID: 51845
				public static LocString NAME = "Storage Full";

				// Token: 0x0400CA86 RID: 51846
				public static LocString TOOLTIP = "This building's internal storage is at maximum capacity\n\nIt must be emptied before its next use";
			}

			// Token: 0x02003133 RID: 12595
			public class MERCURYLIGHT_CHARGING
			{
				// Token: 0x0400CA87 RID: 51847
				public static LocString NAME = "Powering Up: {0}";

				// Token: 0x0400CA88 RID: 51848
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" levels are gradually increasing\n\nIf its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements continue to be met, it will reach maximum brightness in {0}"
				});
			}

			// Token: 0x02003134 RID: 12596
			public class MERCURYLIGHT_DEPLEATING
			{
				// Token: 0x0400CA89 RID: 51849
				public static LocString NAME = "Brightness: {0}";

				// Token: 0x0400CA8A RID: 51850
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" output is decreasing because its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are not being met\n\nIt will power off once its stores are depleted"
				});
			}

			// Token: 0x02003135 RID: 12597
			public class MERCURYLIGHT_DEPLEATED
			{
				// Token: 0x0400CA8B RID: 51851
				public static LocString NAME = "Powered Off";

				// Token: 0x0400CA8C RID: 51852
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is non-operational due to a lack of resources\n\nIt will begin to power up when its ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" requirements are met"
				});
			}

			// Token: 0x02003136 RID: 12598
			public class MERCURYLIGHT_CHARGED
			{
				// Token: 0x0400CA8D RID: 51853
				public static LocString NAME = "Fully Charged";

				// Token: 0x0400CA8E RID: 51854
				public static LocString TOOLTIP = "This building is functioning at maximum capacity";
			}

			// Token: 0x02003137 RID: 12599
			public class SPECIALCARGOBAYCLUSTERCRITTERSTORED
			{
				// Token: 0x0400CA8F RID: 51855
				public static LocString NAME = "Contents: {0}";

				// Token: 0x0400CA90 RID: 51856
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02003138 RID: 12600
			public class GEOTUNER_NEEDGEYSER
			{
				// Token: 0x0400CA91 RID: 51857
				public static LocString NAME = "No Geyser Selected";

				// Token: 0x0400CA92 RID: 51858
				public static LocString TOOLTIP = "Select an analyzed geyser to increase its output";
			}

			// Token: 0x02003139 RID: 12601
			public class GEOTUNER_CHARGE_REQUIRED
			{
				// Token: 0x0400CA93 RID: 51859
				public static LocString NAME = "Experimentation Needed";

				// Token: 0x0400CA94 RID: 51860
				public static LocString TOOLTIP = "This building requires a Duplicant to produce amplification data through experimentation";
			}

			// Token: 0x0200313A RID: 12602
			public class GEOTUNER_CHARGING
			{
				// Token: 0x0400CA95 RID: 51861
				public static LocString NAME = "Compiling Data";

				// Token: 0x0400CA96 RID: 51862
				public static LocString TOOLTIP = "Compiling amplification data through experimentation";
			}

			// Token: 0x0200313B RID: 12603
			public class GEOTUNER_CHARGED
			{
				// Token: 0x0400CA97 RID: 51863
				public static LocString NAME = "Data Remaining: {0}";

				// Token: 0x0400CA98 RID: 51864
				public static LocString TOOLTIP = "This building consumes amplification data while boosting a geyser\n\nTime remaining: {0} ({1} data per second)";
			}

			// Token: 0x0200313C RID: 12604
			public class GEOTUNER_GEYSER_STATUS
			{
				// Token: 0x0400CA99 RID: 51865
				public static LocString NAME = "";

				// Token: 0x0400CA9A RID: 51866
				public static LocString NAME_ERUPTING = "Target is Erupting";

				// Token: 0x0400CA9B RID: 51867
				public static LocString NAME_DORMANT = "Target is Not Erupting";

				// Token: 0x0400CA9C RID: 51868
				public static LocString NAME_IDLE = "Target is Not Erupting";

				// Token: 0x0400CA9D RID: 51869
				public static LocString TOOLTIP = "";

				// Token: 0x0400CA9E RID: 51870
				public static LocString TOOLTIP_ERUPTING = "The selected geyser is erupting and will receive stored amplification data";

				// Token: 0x0400CA9F RID: 51871
				public static LocString TOOLTIP_DORMANT = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";

				// Token: 0x0400CAA0 RID: 51872
				public static LocString TOOLTIP_IDLE = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";
			}

			// Token: 0x0200313D RID: 12605
			public class GEYSER_GEOTUNED
			{
				// Token: 0x0400CAA1 RID: 51873
				public static LocString NAME = "Geotuned ({0}/{1})";

				// Token: 0x0400CAA2 RID: 51874
				public static LocString TOOLTIP = "This geyser is being boosted by {0} out {1} of " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;
			}

			// Token: 0x0200313E RID: 12606
			public class RADIATOR_ENERGY_CURRENT_EMISSION_RATE
			{
				// Token: 0x0400CAA3 RID: 51875
				public static LocString NAME = "Currently Emitting: {ENERGY_RATE}";

				// Token: 0x0400CAA4 RID: 51876
				public static LocString TOOLTIP = "Currently Emitting: {ENERGY_RATE}";
			}

			// Token: 0x0200313F RID: 12607
			public class NOTLINKEDTOHEAD
			{
				// Token: 0x0400CAA5 RID: 51877
				public static LocString NAME = "Not Linked";

				// Token: 0x0400CAA6 RID: 51878
				public static LocString TOOLTIP = "This building must be built adjacent to a {headBuilding} or another {linkBuilding} in order to function";
			}

			// Token: 0x02003140 RID: 12608
			public class BAITED
			{
				// Token: 0x0400CAA7 RID: 51879
				public static LocString NAME = "{0} Bait";

				// Token: 0x0400CAA8 RID: 51880
				public static LocString TOOLTIP = "This lure is baited with {0}\n\nBait material is set during the construction of the building";
			}

			// Token: 0x02003141 RID: 12609
			public class NOCOOLANT
			{
				// Token: 0x0400CAA9 RID: 51881
				public static LocString NAME = "No Coolant";

				// Token: 0x0400CAAA RID: 51882
				public static LocString TOOLTIP = "This building needs coolant";
			}

			// Token: 0x02003142 RID: 12610
			public class ANGERDAMAGE
			{
				// Token: 0x0400CAAB RID: 51883
				public static LocString NAME = "Damage: Duplicant Tantrum";

				// Token: 0x0400CAAC RID: 51884
				public static LocString TOOLTIP = "A stressed Duplicant is damaging this building";

				// Token: 0x0400CAAD RID: 51885
				public static LocString NOTIFICATION = "Building Damage: Duplicant Tantrum";

				// Token: 0x0400CAAE RID: 51886
				public static LocString NOTIFICATION_TOOLTIP = "Stressed Duplicants are damaging these buildings:\n\n{0}";
			}

			// Token: 0x02003143 RID: 12611
			public class PIPECONTENTS
			{
				// Token: 0x0400CAAF RID: 51887
				public static LocString EMPTY = "Empty";

				// Token: 0x0400CAB0 RID: 51888
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400CAB1 RID: 51889
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02003144 RID: 12612
			public class CONVEYOR_CONTENTS
			{
				// Token: 0x0400CAB2 RID: 51890
				public static LocString EMPTY = "Empty";

				// Token: 0x0400CAB3 RID: 51891
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400CAB4 RID: 51892
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x02003145 RID: 12613
			public class ASSIGNEDTO
			{
				// Token: 0x0400CAB5 RID: 51893
				public static LocString NAME = "Assigned to: {Assignee}";

				// Token: 0x0400CAB6 RID: 51894
				public static LocString TOOLTIP = "Only {Assignee} can use this amenity";
			}

			// Token: 0x02003146 RID: 12614
			public class ASSIGNEDPUBLIC
			{
				// Token: 0x0400CAB7 RID: 51895
				public static LocString NAME = "Assigned to: Public";

				// Token: 0x0400CAB8 RID: 51896
				public static LocString TOOLTIP = "Any Duplicant can use this amenity";
			}

			// Token: 0x02003147 RID: 12615
			public class ASSIGNEDTOROOM
			{
				// Token: 0x0400CAB9 RID: 51897
				public static LocString NAME = "Assigned to: {0}";

				// Token: 0x0400CABA RID: 51898
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Any Duplicant assigned to this ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" can use this amenity"
				});
			}

			// Token: 0x02003148 RID: 12616
			public class AWAITINGSEEDDELIVERY
			{
				// Token: 0x0400CABB RID: 51899
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400CABC RID: 51900
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x02003149 RID: 12617
			public class AWAITINGBAITDELIVERY
			{
				// Token: 0x0400CABD RID: 51901
				public static LocString NAME = "Awaiting Bait";

				// Token: 0x0400CABE RID: 51902
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD;
			}

			// Token: 0x0200314A RID: 12618
			public class CLINICOUTSIDEHOSPITAL
			{
				// Token: 0x0400CABF RID: 51903
				public static LocString NAME = "Medical building outside Hospital";

				// Token: 0x0400CAC0 RID: 51904
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rebuild this medical equipment in a ",
					UI.PRE_KEYWORD,
					"Hospital",
					UI.PST_KEYWORD,
					" to more effectively quarantine sick Duplicants"
				});
			}

			// Token: 0x0200314B RID: 12619
			public class BOTTLE_EMPTIER
			{
				// Token: 0x0200314C RID: 12620
				public static class ALLOWED
				{
					// Token: 0x0400CAC1 RID: 51905
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400CAC2 RID: 51906
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}

				// Token: 0x0200314D RID: 12621
				public static class DENIED
				{
					// Token: 0x0400CAC3 RID: 51907
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400CAC4 RID: 51908
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a bottling station to bring to this location"
					});
				}
			}

			// Token: 0x0200314E RID: 12622
			public class CANISTER_EMPTIER
			{
				// Token: 0x0200314F RID: 12623
				public static class ALLOWED
				{
					// Token: 0x0400CAC5 RID: 51909
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400CAC6 RID: 51910
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}

				// Token: 0x02003150 RID: 12624
				public static class DENIED
				{
					// Token: 0x0400CAC7 RID: 51911
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400CAC8 RID: 51912
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a canister filling station to bring to this location"
					});
				}
			}

			// Token: 0x02003151 RID: 12625
			public class BROKEN
			{
				// Token: 0x0400CAC9 RID: 51913
				public static LocString NAME = "Broken";

				// Token: 0x0400CACA RID: 51914
				public static LocString TOOLTIP = "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
			}

			// Token: 0x02003152 RID: 12626
			public class CHANGESTORAGETILETARGET
			{
				// Token: 0x0400CACB RID: 51915
				public static LocString NAME = "Set Storage: {TargetName}";

				// Token: 0x0400CACC RID: 51916
				public static LocString TOOLTIP = "Waiting for a Duplicant to reassign this storage to {TargetName}";

				// Token: 0x0400CACD RID: 51917
				public static LocString EMPTY = "Empty";
			}

			// Token: 0x02003153 RID: 12627
			public class CHANGEDOORCONTROLSTATE
			{
				// Token: 0x0400CACE RID: 51918
				public static LocString NAME = "Pending Door State Change: {ControlState}";

				// Token: 0x0400CACF RID: 51919
				public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
			}

			// Token: 0x02003154 RID: 12628
			public class DISPENSEREQUESTED
			{
				// Token: 0x0400CAD0 RID: 51920
				public static LocString NAME = "Dispense Requested";

				// Token: 0x0400CAD1 RID: 51921
				public static LocString TOOLTIP = "Waiting for a Duplicant to dispense the item";
			}

			// Token: 0x02003155 RID: 12629
			public class SUIT_LOCKER
			{
				// Token: 0x02003156 RID: 12630
				public class NEED_CONFIGURATION
				{
					// Token: 0x0400CAD2 RID: 51922
					public static LocString NAME = "Current Status: Needs Configuration";

					// Token: 0x0400CAD3 RID: 51923
					public static LocString TOOLTIP = "Set this dock to store a suit or leave it empty";
				}

				// Token: 0x02003157 RID: 12631
				public class READY
				{
					// Token: 0x0400CAD4 RID: 51924
					public static LocString NAME = "Current Status: Empty";

					// Token: 0x0400CAD5 RID: 51925
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock is ready to receive a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						", either by manual delivery or from a Duplicant returning the suit they're wearing"
					});
				}

				// Token: 0x02003158 RID: 12632
				public class SUIT_REQUESTED
				{
					// Token: 0x0400CAD6 RID: 51926
					public static LocString NAME = "Current Status: Awaiting Delivery";

					// Token: 0x0400CAD7 RID: 51927
					public static LocString TOOLTIP = "Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD;
				}

				// Token: 0x02003159 RID: 12633
				public class CHARGING
				{
					// Token: 0x0400CAD8 RID: 51928
					public static LocString NAME = "Current Status: Charging Suit";

					// Token: 0x0400CAD9 RID: 51929
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						" is docked and refueling"
					});
				}

				// Token: 0x0200315A RID: 12634
				public class NO_OXYGEN
				{
					// Token: 0x0400CADA RID: 51930
					public static LocString NAME = "Current Status: No Oxygen";

					// Token: 0x0400CADB RID: 51931
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.OXYGEN.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200315B RID: 12635
				public class NO_FUEL
				{
					// Token: 0x0400CADC RID: 51932
					public static LocString NAME = "Current Status: No Fuel";

					// Token: 0x0400CADD RID: 51933
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.PETROLEUM.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200315C RID: 12636
				public class NO_COOLANT
				{
					// Token: 0x0400CADE RID: 51934
					public static LocString NAME = "Current Status: No Coolant";

					// Token: 0x0400CADF RID: 51935
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.WATER.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x0200315D RID: 12637
				public class NOT_OPERATIONAL
				{
					// Token: 0x0400CAE0 RID: 51936
					public static LocString NAME = "Current Status: Offline";

					// Token: 0x0400CAE1 RID: 51937
					public static LocString TOOLTIP = "This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
				}

				// Token: 0x0200315E RID: 12638
				public class FULLY_CHARGED
				{
					// Token: 0x0400CAE2 RID: 51938
					public static LocString NAME = "Current Status: Full Fueled";

					// Token: 0x0400CAE3 RID: 51939
					public static LocString TOOLTIP = "This suit is fully refueled and ready for use";
				}
			}

			// Token: 0x0200315F RID: 12639
			public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
			{
				// Token: 0x0400CAE4 RID: 51940
				public static LocString NAME = "Clearance: Vacancy Only";

				// Token: 0x0400CAE5 RID: 51941
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass only if there is room in a ",
					UI.PRE_KEYWORD,
					"Dock",
					UI.PST_KEYWORD,
					" to store their ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003160 RID: 12640
			public class SUITMARKERTRAVERSALANYTIME
			{
				// Token: 0x0400CAE6 RID: 51942
				public static LocString NAME = "Clearance: Always Permitted";

				// Token: 0x0400CAE7 RID: 51943
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass even if there is no room to store their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					"\n\nWhen all available docks are full, Duplicants will unequip their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					" and drop them on the floor"
				});
			}

			// Token: 0x02003161 RID: 12641
			public class SUIT_LOCKER_NEEDS_CONFIGURATION
			{
				// Token: 0x0400CAE8 RID: 51944
				public static LocString NAME = "Not Configured";

				// Token: 0x0400CAE9 RID: 51945
				public static LocString TOOLTIP = "Dock settings not configured";
			}

			// Token: 0x02003162 RID: 12642
			public class CURRENTDOORCONTROLSTATE
			{
				// Token: 0x0400CAEA RID: 51946
				public static LocString NAME = "Current State: {ControlState}";

				// Token: 0x0400CAEB RID: 51947
				public static LocString TOOLTIP = "Current State: {ControlState}\n\nAuto: Duplicants open and close this door as needed\nLocked: Nothing may pass through\nOpen: This door will remain open";

				// Token: 0x0400CAEC RID: 51948
				public static LocString OPENED = "Opened";

				// Token: 0x0400CAED RID: 51949
				public static LocString AUTO = "Auto";

				// Token: 0x0400CAEE RID: 51950
				public static LocString LOCKED = "Locked";
			}

			// Token: 0x02003163 RID: 12643
			public class CONDUITBLOCKED
			{
				// Token: 0x0400CAEF RID: 51951
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400CAF0 RID: 51952
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02003164 RID: 12644
			public class OUTPUTTILEBLOCKED
			{
				// Token: 0x0400CAF1 RID: 51953
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400CAF2 RID: 51954
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02003165 RID: 12645
			public class CONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400CAF3 RID: 51955
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400CAF4 RID: 51956
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02003166 RID: 12646
			public class SOLIDCONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400CAF5 RID: 51957
				public static LocString NAME = "Conveyor Rail Blocked";

				// Token: 0x0400CAF6 RID: 51958
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02003167 RID: 12647
			public class OUTPUTPIPEFULL
			{
				// Token: 0x0400CAF7 RID: 51959
				public static LocString NAME = "Output Pipe Full";

				// Token: 0x0400CAF8 RID: 51960
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Unable to flush contents, output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x02003168 RID: 12648
			public class CONSTRUCTIONUNREACHABLE
			{
				// Token: 0x0400CAF9 RID: 51961
				public static LocString NAME = "Unreachable Build";

				// Token: 0x0400CAFA RID: 51962
				public static LocString TOOLTIP = "Duplicants cannot reach this construction site";
			}

			// Token: 0x02003169 RID: 12649
			public class MOPUNREACHABLE
			{
				// Token: 0x0400CAFB RID: 51963
				public static LocString NAME = "Unreachable Mop";

				// Token: 0x0400CAFC RID: 51964
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x0200316A RID: 12650
			public class DEADREACTORCOOLINGOFF
			{
				// Token: 0x0400CAFD RID: 51965
				public static LocString NAME = "Cooling ({CyclesRemaining} cycles remaining)";

				// Token: 0x0400CAFE RID: 51966
				public static LocString TOOLTIP = "The radiation coming from this reactor is diminishing";
			}

			// Token: 0x0200316B RID: 12651
			public class DIGUNREACHABLE
			{
				// Token: 0x0400CAFF RID: 51967
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400CB00 RID: 51968
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x0200316C RID: 12652
			public class STORAGEUNREACHABLE
			{
				// Token: 0x0400CB01 RID: 51969
				public static LocString NAME = "Unreachable Storage";

				// Token: 0x0400CB02 RID: 51970
				public static LocString TOOLTIP = "Duplicants cannot reach this storage unit";
			}

			// Token: 0x0200316D RID: 12653
			public class PASSENGERMODULEUNREACHABLE
			{
				// Token: 0x0400CB03 RID: 51971
				public static LocString NAME = "Unreachable Module";

				// Token: 0x0400CB04 RID: 51972
				public static LocString TOOLTIP = "Duplicants cannot reach this rocket module";
			}

			// Token: 0x0200316E RID: 12654
			public class POWERBANKCHARGERINPROGRESS
			{
				// Token: 0x0400CB05 RID: 51973
				public static LocString NAME = "Recharging Power Bank: {0}";

				// Token: 0x0400CB06 RID: 51974
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently charging a ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" at {0}\n\nThe ",
					UI.PRE_KEYWORD,
					"Power Bank",
					UI.PST_KEYWORD,
					" will be dropped once charging is complete"
				});
			}

			// Token: 0x0200316F RID: 12655
			public class CONSTRUCTABLEDIGUNREACHABLE
			{
				// Token: 0x0400CB07 RID: 51975
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400CB08 RID: 51976
				public static LocString TOOLTIP = "This construction site contains cells that cannot be dug out";
			}

			// Token: 0x02003170 RID: 12656
			public class EMPTYPUMPINGSTATION
			{
				// Token: 0x0400CB09 RID: 51977
				public static LocString NAME = "Empty";

				// Token: 0x0400CB0A RID: 51978
				public static LocString TOOLTIP = "This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02003171 RID: 12657
			public class ENTOMBED
			{
				// Token: 0x0400CB0B RID: 51979
				public static LocString NAME = "Entombed";

				// Token: 0x0400CB0C RID: 51980
				public static LocString TOOLTIP = "Must be dug out by a Duplicant";

				// Token: 0x0400CB0D RID: 51981
				public static LocString NOTIFICATION_NAME = "Building entombment";

				// Token: 0x0400CB0E RID: 51982
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are entombed and need to be dug out:";
			}

			// Token: 0x02003172 RID: 12658
			public class ELECTROBANKJOULESAVAILABLE
			{
				// Token: 0x0400CB0F RID: 51983
				public static LocString NAME = "Power Remaining: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400CB10 RID: 51984
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use\n\nMaximum capacity: {JoulesCapacity}"
				});
			}

			// Token: 0x02003173 RID: 12659
			public class FABRICATORACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400CB11 RID: 51985
				public static LocString NAME = "Fabricator accepts mutant seeds";

				// Token: 0x0400CB12 RID: 51986
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fabricator is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x02003174 RID: 12660
			public class FISHFEEDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400CB13 RID: 51987
				public static LocString NAME = "Fish Feeder accepts mutant seeds";

				// Token: 0x0400CB14 RID: 51988
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fish feeder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as fish food"
				});
			}

			// Token: 0x02003175 RID: 12661
			public class INVALIDPORTOVERLAP
			{
				// Token: 0x0400CB15 RID: 51989
				public static LocString NAME = "Invalid Port Overlap";

				// Token: 0x0400CB16 RID: 51990
				public static LocString TOOLTIP = "Ports on this building overlap those on another building\n\nThis building must be rebuilt in a valid location";

				// Token: 0x0400CB17 RID: 51991
				public static LocString NOTIFICATION_NAME = "Building has overlapping ports";

				// Token: 0x0400CB18 RID: 51992
				public static LocString NOTIFICATION_TOOLTIP = "These buildings must be rebuilt with non-overlapping ports:";
			}

			// Token: 0x02003176 RID: 12662
			public class GENESHUFFLECOMPLETED
			{
				// Token: 0x0400CB19 RID: 51993
				public static LocString NAME = "Vacillation Complete";

				// Token: 0x0400CB1A RID: 51994
				public static LocString TOOLTIP = "The Duplicant has completed the neural vacillation process and is ready to be released";
			}

			// Token: 0x02003177 RID: 12663
			public class OVERHEATED
			{
				// Token: 0x0400CB1B RID: 51995
				public static LocString NAME = "Damage: Overheating";

				// Token: 0x0400CB1C RID: 51996
				public static LocString TOOLTIP = "This building is taking damage and will break down if not cooled";
			}

			// Token: 0x02003178 RID: 12664
			public class OVERLOADED
			{
				// Token: 0x0400CB1D RID: 51997
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400CB1E RID: 51998
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" is taking damage because there are too many buildings pulling ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from this circuit\n\nSplit this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" circuit into multiple circuits, or use higher quality ",
					UI.PRE_KEYWORD,
					"Wires",
					UI.PST_KEYWORD,
					" to prevent overloading"
				});
			}

			// Token: 0x02003179 RID: 12665
			public class LOGICOVERLOADED
			{
				// Token: 0x0400CB1F RID: 51999
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400CB20 RID: 52000
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" is taking damage\n\nLimit the output to one Bit, or replace it with ",
					UI.PRE_KEYWORD,
					"Logic Ribbon",
					UI.PST_KEYWORD,
					" to prevent further damage"
				});
			}

			// Token: 0x0200317A RID: 12666
			public class OPERATINGENERGY
			{
				// Token: 0x0400CB21 RID: 52001
				public static LocString NAME = "Heat Production: {0}/s";

				// Token: 0x0400CB22 RID: 52002
				public static LocString TOOLTIP = "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";

				// Token: 0x0400CB23 RID: 52003
				public static LocString LINEITEM = "    • {0}: {1}\n";

				// Token: 0x0400CB24 RID: 52004
				public static LocString OPERATING = "Normal operation";

				// Token: 0x0400CB25 RID: 52005
				public static LocString EXHAUSTING = "Excess produced";

				// Token: 0x0400CB26 RID: 52006
				public static LocString PIPECONTENTS_TRANSFER = "Transferred from pipes";

				// Token: 0x0400CB27 RID: 52007
				public static LocString FOOD_TRANSFER = "Internal Cooling";
			}

			// Token: 0x0200317B RID: 12667
			public class FLOODED
			{
				// Token: 0x0400CB28 RID: 52008
				public static LocString NAME = "Building Flooded";

				// Token: 0x0400CB29 RID: 52009
				public static LocString TOOLTIP = "Building cannot function at current saturation";

				// Token: 0x0400CB2A RID: 52010
				public static LocString NOTIFICATION_NAME = "Flooding";

				// Token: 0x0400CB2B RID: 52011
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are flooded:";
			}

			// Token: 0x0200317C RID: 12668
			public class NOTSUBMERGED
			{
				// Token: 0x0400CB2C RID: 52012
				public static LocString NAME = "Building Not Submerged";

				// Token: 0x0400CB2D RID: 52013
				public static LocString TOOLTIP = "Building cannot function unless submerged in liquid";
			}

			// Token: 0x0200317D RID: 12669
			public class GASVENTOBSTRUCTED
			{
				// Token: 0x0400CB2E RID: 52014
				public static LocString NAME = "Gas Vent Obstructed";

				// Token: 0x0400CB2F RID: 52015
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x0200317E RID: 12670
			public class GASVENTOVERPRESSURE
			{
				// Token: 0x0400CB30 RID: 52016
				public static LocString NAME = "Gas Vent Overpressure";

				// Token: 0x0400CB31 RID: 52017
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200317F RID: 12671
			public class DIRECTION_CONTROL
			{
				// Token: 0x0400CB32 RID: 52018
				public static LocString NAME = "Use Direction: {Direction}";

				// Token: 0x0400CB33 RID: 52019
				public static LocString TOOLTIP = "Duplicants will only use this building when walking by it\n\nCurrently allowed direction: <b>{Direction}</b>";

				// Token: 0x02003180 RID: 12672
				public static class DIRECTIONS
				{
					// Token: 0x0400CB34 RID: 52020
					public static LocString LEFT = "Left";

					// Token: 0x0400CB35 RID: 52021
					public static LocString RIGHT = "Right";

					// Token: 0x0400CB36 RID: 52022
					public static LocString BOTH = "Both";
				}
			}

			// Token: 0x02003181 RID: 12673
			public class WATTSONGAMEOVER
			{
				// Token: 0x0400CB37 RID: 52023
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400CB38 RID: 52024
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02003182 RID: 12674
			public class INVALIDBUILDINGLOCATION
			{
				// Token: 0x0400CB39 RID: 52025
				public static LocString NAME = "Invalid Building Location";

				// Token: 0x0400CB3A RID: 52026
				public static LocString TOOLTIP = "Cannot construct a building in this location";
			}

			// Token: 0x02003183 RID: 12675
			public class LIQUIDVENTOBSTRUCTED
			{
				// Token: 0x0400CB3B RID: 52027
				public static LocString NAME = "Liquid Vent Obstructed";

				// Token: 0x0400CB3C RID: 52028
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02003184 RID: 12676
			public class LIQUIDVENTOVERPRESSURE
			{
				// Token: 0x0400CB3D RID: 52029
				public static LocString NAME = "Liquid Vent Overpressure";

				// Token: 0x0400CB3E RID: 52030
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02003185 RID: 12677
			public class MANUALLYCONTROLLED
			{
				// Token: 0x0400CB3F RID: 52031
				public static LocString NAME = "Manually Controlled";

				// Token: 0x0400CB40 RID: 52032
				public static LocString TOOLTIP = "This Duplicant is under my control";
			}

			// Token: 0x02003186 RID: 12678
			public class LIMITVALVELIMITREACHED
			{
				// Token: 0x0400CB41 RID: 52033
				public static LocString NAME = "Limit Reached";

				// Token: 0x0400CB42 RID: 52034
				public static LocString TOOLTIP = "No more Mass can be transferred";
			}

			// Token: 0x02003187 RID: 12679
			public class LIMITVALVELIMITNOTREACHED
			{
				// Token: 0x0400CB43 RID: 52035
				public static LocString NAME = "Amount remaining: {0}";

				// Token: 0x0400CB44 RID: 52036
				public static LocString TOOLTIP = "This building will stop transferring Mass when the amount remaining reaches 0";
			}

			// Token: 0x02003188 RID: 12680
			public class MATERIALSUNAVAILABLE
			{
				// Token: 0x0400CB45 RID: 52037
				public static LocString NAME = "Insufficient Resources\n{ItemsRemaining}";

				// Token: 0x0400CB46 RID: 52038
				public static LocString TOOLTIP = "Crucial materials for this building are beyond reach or unavailable";

				// Token: 0x0400CB47 RID: 52039
				public static LocString NOTIFICATION_NAME = "Building lacks resources";

				// Token: 0x0400CB48 RID: 52040
				public static LocString NOTIFICATION_TOOLTIP = "Crucial materials are unavailable or beyond reach for these buildings:";

				// Token: 0x0400CB49 RID: 52041
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400CB4A RID: 52042
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02003189 RID: 12681
			public class MATERIALSUNAVAILABLEFORREFILL
			{
				// Token: 0x0400CB4B RID: 52043
				public static LocString NAME = "Resources Low\n{ItemsRemaining}";

				// Token: 0x0400CB4C RID: 52044
				public static LocString TOOLTIP = "This building will soon require materials that are unavailable";

				// Token: 0x0400CB4D RID: 52045
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x0200318A RID: 12682
			public class MELTINGDOWN
			{
				// Token: 0x0400CB4E RID: 52046
				public static LocString NAME = "Breaking Down";

				// Token: 0x0400CB4F RID: 52047
				public static LocString TOOLTIP = "This building is collapsing";

				// Token: 0x0400CB50 RID: 52048
				public static LocString NOTIFICATION_NAME = "Building breakdown";

				// Token: 0x0400CB51 RID: 52049
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are collapsing:";
			}

			// Token: 0x0200318B RID: 12683
			public class MISSINGFOUNDATION
			{
				// Token: 0x0400CB52 RID: 52050
				public static LocString NAME = "Missing Tile";

				// Token: 0x0400CB53 RID: 52051
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" beneath this building to regain function\n\nTile can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" of the Build Menu"
				});
			}

			// Token: 0x0200318C RID: 12684
			public class NEUTRONIUMUNMINABLE
			{
				// Token: 0x0400CB54 RID: 52052
				public static LocString NAME = "Cannot Mine";

				// Token: 0x0400CB55 RID: 52053
				public static LocString TOOLTIP = "This resource cannot be mined by Duplicant tools";
			}

			// Token: 0x0200318D RID: 12685
			public class NEEDGASIN
			{
				// Token: 0x0400CB56 RID: 52054
				public static LocString NAME = "No Gas Intake\n{GasRequired}";

				// Token: 0x0400CB57 RID: 52055
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400CB58 RID: 52056
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x0200318E RID: 12686
			public class NEEDGASOUT
			{
				// Token: 0x0400CB59 RID: 52057
				public static LocString NAME = "No Gas Output";

				// Token: 0x0400CB5A RID: 52058
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x0200318F RID: 12687
			public class NEEDLIQUIDIN
			{
				// Token: 0x0400CB5B RID: 52059
				public static LocString NAME = "No Liquid Intake\n{LiquidRequired}";

				// Token: 0x0400CB5C RID: 52060
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400CB5D RID: 52061
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02003190 RID: 12688
			public class NEEDLIQUIDOUT
			{
				// Token: 0x0400CB5E RID: 52062
				public static LocString NAME = "No Liquid Output";

				// Token: 0x0400CB5F RID: 52063
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02003191 RID: 12689
			public class LIQUIDPIPEEMPTY
			{
				// Token: 0x0400CB60 RID: 52064
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400CB61 RID: 52065
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02003192 RID: 12690
			public class LIQUIDPIPEOBSTRUCTED
			{
				// Token: 0x0400CB62 RID: 52066
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400CB63 RID: 52067
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x02003193 RID: 12691
			public class GASPIPEEMPTY
			{
				// Token: 0x0400CB64 RID: 52068
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400CB65 RID: 52069
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02003194 RID: 12692
			public class GASPIPEOBSTRUCTED
			{
				// Token: 0x0400CB66 RID: 52070
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400CB67 RID: 52071
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x02003195 RID: 12693
			public class NEEDSOLIDIN
			{
				// Token: 0x0400CB68 RID: 52072
				public static LocString NAME = "No Conveyor Loader";

				// Token: 0x0400CB69 RID: 52073
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be fed onto this Conveyor system for transport\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Loader",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003196 RID: 12694
			public class NEEDSOLIDOUT
			{
				// Token: 0x0400CB6A RID: 52074
				public static LocString NAME = "No Conveyor Receptacle";

				// Token: 0x0400CB6B RID: 52075
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be offloaded from this Conveyor system and will backup the rails\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02003197 RID: 12695
			public class SOLIDPIPEOBSTRUCTED
			{
				// Token: 0x0400CB6C RID: 52076
				public static LocString NAME = "Conveyor Rail Backup";

				// Token: 0x0400CB6D RID: 52077
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" cannot carry anymore material\n\nRemove material from the ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD,
					" to free space for more objects"
				});
			}

			// Token: 0x02003198 RID: 12696
			public class NEEDPLANT
			{
				// Token: 0x0400CB6E RID: 52078
				public static LocString NAME = "No Seeds";

				// Token: 0x0400CB6F RID: 52079
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x02003199 RID: 12697
			public class NEEDSEED
			{
				// Token: 0x0400CB70 RID: 52080
				public static LocString NAME = "No Seed Selected";

				// Token: 0x0400CB71 RID: 52081
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x0200319A RID: 12698
			public class NEEDPOWER
			{
				// Token: 0x0400CB72 RID: 52082
				public static LocString NAME = "No Power";

				// Token: 0x0400CB73 RID: 52083
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"All connected ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" sources have lost charge"
				});
			}

			// Token: 0x0200319B RID: 12699
			public class NOTENOUGHPOWER
			{
				// Token: 0x0400CB74 RID: 52084
				public static LocString NAME = "Insufficient Power";

				// Token: 0x0400CB75 RID: 52085
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building does not have enough stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to run"
				});
			}

			// Token: 0x0200319C RID: 12700
			public class POWERLOOPDETECTED
			{
				// Token: 0x0400CB76 RID: 52086
				public static LocString NAME = "Power Loop Detected";

				// Token: 0x0400CB77 RID: 52087
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Transformer's",
					UI.PST_KEYWORD,
					" ",
					UI.PRE_KEYWORD,
					"Power Output",
					UI.PST_KEYWORD,
					" has been connected back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200319D RID: 12701
			public class NEEDRESOURCE
			{
				// Token: 0x0400CB78 RID: 52088
				public static LocString NAME = "Resource Required";

				// Token: 0x0400CB79 RID: 52089
				public static LocString TOOLTIP = "This building is missing required materials";
			}

			// Token: 0x0200319E RID: 12702
			public class NEWDUPLICANTSAVAILABLE
			{
				// Token: 0x0400CB7A RID: 52090
				public static LocString NAME = "Printables Available";

				// Token: 0x0400CB7B RID: 52091
				public static LocString TOOLTIP = "I am ready to print a new colony member or care package";

				// Token: 0x0400CB7C RID: 52092
				public static LocString NOTIFICATION_NAME = "New Printables are available";

				// Token: 0x0400CB7D RID: 52093
				public static LocString NOTIFICATION_TOOLTIP = "The Printing Pod " + UI.FormatAsHotKey(global::Action.Plan1) + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:";
			}

			// Token: 0x0200319F RID: 12703
			public class NOAPPLICABLERESEARCHSELECTED
			{
				// Token: 0x0400CB7E RID: 52094
				public static LocString NAME = "Inapplicable Research";

				// Token: 0x0400CB7F RID: 52095
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the current ",
					UI.FormatAsLink("Research Focus", "TECH")
				});

				// Token: 0x0400CB80 RID: 52096
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle";

				// Token: 0x0400CB81 RID: 52097
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These buildings cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the selected ",
					UI.FormatAsLink("Research Focus", "TECH"),
					":"
				});
			}

			// Token: 0x020031A0 RID: 12704
			public class NOAPPLICABLEANALYSISSELECTED
			{
				// Token: 0x0400CB82 RID: 52098
				public static LocString NAME = "No Analysis Focus Selected";

				// Token: 0x0400CB83 RID: 52099
				public static LocString TOOLTIP = "Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to begin analysis";

				// Token: 0x0400CB84 RID: 52100
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Telescope", "TELESCOPE") + " idle";

				// Token: 0x0400CB85 RID: 52101
				public static LocString NOTIFICATION_TOOLTIP = "These buildings require an analysis focus:";
			}

			// Token: 0x020031A1 RID: 12705
			public class NOAVAILABLESEED
			{
				// Token: 0x0400CB86 RID: 52102
				public static LocString NAME = "No Seed Available";

				// Token: 0x0400CB87 RID: 52103
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" is not available"
				});
			}

			// Token: 0x020031A2 RID: 12706
			public class NOSTORAGEFILTERSET
			{
				// Token: 0x0400CB88 RID: 52104
				public static LocString NAME = "Filters Not Designated";

				// Token: 0x0400CB89 RID: 52105
				public static LocString TOOLTIP = "No resources types are marked for storage in this building";
			}

			// Token: 0x020031A3 RID: 12707
			public class NOSUITMARKER
			{
				// Token: 0x0400CB8A RID: 52106
				public static LocString NAME = "No Checkpoint";

				// Token: 0x0400CB8B RID: 52107
				public static LocString TOOLTIP = "Docks must be placed beside a " + BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces";
			}

			// Token: 0x020031A4 RID: 12708
			public class SUITMARKERWRONGSIDE
			{
				// Token: 0x0400CB8C RID: 52108
				public static LocString NAME = "Invalid Checkpoint";

				// Token: 0x0400CB8D RID: 52109
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been built on the wrong side of a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					"\n\nDocks must be placed beside a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					", opposite the side the checkpoint faces"
				});
			}

			// Token: 0x020031A5 RID: 12709
			public class NOFILTERELEMENTSELECTED
			{
				// Token: 0x0400CB8E RID: 52110
				public static LocString NAME = "No Filter Selected";

				// Token: 0x0400CB8F RID: 52111
				public static LocString TOOLTIP = "Select a resource to filter";
			}

			// Token: 0x020031A6 RID: 12710
			public class NOLUREELEMENTSELECTED
			{
				// Token: 0x0400CB90 RID: 52112
				public static LocString NAME = "No Bait Selected";

				// Token: 0x0400CB91 RID: 52113
				public static LocString TOOLTIP = "Select a resource to use as bait";
			}

			// Token: 0x020031A7 RID: 12711
			public class NOFISHABLEWATERBELOW
			{
				// Token: 0x0400CB92 RID: 52114
				public static LocString NAME = "No Fishable Water";

				// Token: 0x0400CB93 RID: 52115
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no edible ",
					UI.PRE_KEYWORD,
					"Fish",
					UI.PST_KEYWORD,
					" beneath this structure"
				});
			}

			// Token: 0x020031A8 RID: 12712
			public class NOPOWERCONSUMERS
			{
				// Token: 0x0400CB94 RID: 52116
				public static LocString NAME = "No Power Consumers";

				// Token: 0x0400CB95 RID: 52117
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"No buildings are connected to this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source"
				});
			}

			// Token: 0x020031A9 RID: 12713
			public class NOWIRECONNECTED
			{
				// Token: 0x0400CB96 RID: 52118
				public static LocString NAME = "No Power Wire Connected";

				// Token: 0x0400CB97 RID: 52119
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x020031AA RID: 12714
			public class PENDINGDECONSTRUCTION
			{
				// Token: 0x0400CB98 RID: 52120
				public static LocString NAME = "Deconstruction Errand";

				// Token: 0x0400CB99 RID: 52121
				public static LocString TOOLTIP = "Building will be deconstructed once a Duplicant is available";
			}

			// Token: 0x020031AB RID: 12715
			public class PENDINGDEMOLITION
			{
				// Token: 0x0400CB9A RID: 52122
				public static LocString NAME = "Demolition Errand";

				// Token: 0x0400CB9B RID: 52123
				public static LocString TOOLTIP = "Object will be permanently demolished once a Duplicant is available";
			}

			// Token: 0x020031AC RID: 12716
			public class PENDINGFISH
			{
				// Token: 0x0400CB9C RID: 52124
				public static LocString NAME = "Fishing Errand";

				// Token: 0x0400CB9D RID: 52125
				public static LocString TOOLTIP = "Spot will be fished once a Duplicant is available";
			}

			// Token: 0x020031AD RID: 12717
			public class PENDINGHARVEST
			{
				// Token: 0x0400CB9E RID: 52126
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400CB9F RID: 52127
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x020031AE RID: 12718
			public class PENDINGUPROOT
			{
				// Token: 0x0400CBA0 RID: 52128
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400CBA1 RID: 52129
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x020031AF RID: 12719
			public class PENDINGREPAIR
			{
				// Token: 0x0400CBA2 RID: 52130
				public static LocString NAME = "Repair Errand";

				// Token: 0x0400CBA3 RID: 52131
				public static LocString TOOLTIP = "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
			}

			// Token: 0x020031B0 RID: 12720
			public class PENDINGSWITCHTOGGLE
			{
				// Token: 0x0400CBA4 RID: 52132
				public static LocString NAME = "Settings Errand";

				// Token: 0x0400CBA5 RID: 52133
				public static LocString TOOLTIP = "Settings will be changed once a Duplicant is available";
			}

			// Token: 0x020031B1 RID: 12721
			public class PENDINGWORK
			{
				// Token: 0x0400CBA6 RID: 52134
				public static LocString NAME = "Work Errand";

				// Token: 0x0400CBA7 RID: 52135
				public static LocString TOOLTIP = "Building will be operated once a Duplicant is available";
			}

			// Token: 0x020031B2 RID: 12722
			public class POWERBUTTONOFF
			{
				// Token: 0x0400CBA8 RID: 52136
				public static LocString NAME = "Function Suspended";

				// Token: 0x0400CBA9 RID: 52137
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been toggled off\nPress ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume its use"
				});
			}

			// Token: 0x020031B3 RID: 12723
			public class PUMPINGSTATION
			{
				// Token: 0x0400CBAA RID: 52138
				public static LocString NAME = "Liquid Available: {Liquids}";

				// Token: 0x0400CBAB RID: 52139
				public static LocString TOOLTIP = "This pumping station has access to: {Liquids}";
			}

			// Token: 0x020031B4 RID: 12724
			public class PRESSUREOK
			{
				// Token: 0x0400CBAC RID: 52140
				public static LocString NAME = "Max Gas Pressure";

				// Token: 0x0400CBAD RID: 52141
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ambient ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is preventing this building from emitting gas\n\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x020031B5 RID: 12725
			public class UNDERPRESSURE
			{
				// Token: 0x0400CBAE RID: 52142
				public static LocString NAME = "Low Air Pressure";

				// Token: 0x0400CBAF RID: 52143
				public static LocString TOOLTIP = "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
			}

			// Token: 0x020031B6 RID: 12726
			public class STORAGELOCKER
			{
				// Token: 0x0400CBB0 RID: 52144
				public static LocString NAME = "Storing: {Stored} / {Capacity} {Units}";

				// Token: 0x0400CBB1 RID: 52145
				public static LocString TOOLTIP = "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
			}

			// Token: 0x020031B7 RID: 12727
			public class CRITTERCAPACITY
			{
				// Token: 0x0400CBB2 RID: 52146
				public static LocString NAME = "Storing: {Stored} / {Capacity} Critters";

				// Token: 0x0400CBB3 RID: 52147
				public static LocString TOOLTIP = "This container is storing <b>{Stored} {StoredUnits}</b> of a maximum <b>{Capacity} {CapacityUnits}</b>";

				// Token: 0x0400CBB4 RID: 52148
				public static LocString UNITS = "Critters";

				// Token: 0x0400CBB5 RID: 52149
				public static LocString UNIT = "Critter";
			}

			// Token: 0x020031B8 RID: 12728
			public class SKILL_POINTS_AVAILABLE
			{
				// Token: 0x0400CBB6 RID: 52150
				public static LocString NAME = "Skill Points Available";

				// Token: 0x0400CBB7 RID: 52151
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant has ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" available"
				});
			}

			// Token: 0x020031B9 RID: 12729
			public class TANNINGLIGHTSUFFICIENT
			{
				// Token: 0x0400CBB8 RID: 52152
				public static LocString NAME = "Tanning Light Available";

				// Token: 0x0400CBB9 RID: 52153
				public static LocString TOOLTIP = "There is sufficient " + UI.FormatAsLink("Light", "LIGHT") + " here to create pleasing skin crisping";
			}

			// Token: 0x020031BA RID: 12730
			public class TANNINGLIGHTINSUFFICIENT
			{
				// Token: 0x0400CBBA RID: 52154
				public static LocString NAME = "Insufficient Tanning Light";

				// Token: 0x0400CBBB RID: 52155
				public static LocString TOOLTIP = "The " + UI.FormatAsLink("Light", "LIGHT") + " here is not bright enough for that Sunny Day feeling";
			}

			// Token: 0x020031BB RID: 12731
			public class UNASSIGNED
			{
				// Token: 0x0400CBBC RID: 52156
				public static LocString NAME = "Unassigned";

				// Token: 0x0400CBBD RID: 52157
				public static LocString TOOLTIP = "Assign a Duplicant to use this amenity";
			}

			// Token: 0x020031BC RID: 12732
			public class UNDERCONSTRUCTION
			{
				// Token: 0x0400CBBE RID: 52158
				public static LocString NAME = "Under Construction";

				// Token: 0x0400CBBF RID: 52159
				public static LocString TOOLTIP = "This building is currently being built";
			}

			// Token: 0x020031BD RID: 12733
			public class UNDERCONSTRUCTIONNOWORKER
			{
				// Token: 0x0400CBC0 RID: 52160
				public static LocString NAME = "Construction Errand";

				// Token: 0x0400CBC1 RID: 52161
				public static LocString TOOLTIP = "Building will be constructed once a Duplicant is available";
			}

			// Token: 0x020031BE RID: 12734
			public class WAITINGFORMATERIALS
			{
				// Token: 0x0400CBC2 RID: 52162
				public static LocString NAME = "Awaiting Delivery\n{ItemsRemaining}";

				// Token: 0x0400CBC3 RID: 52163
				public static LocString TOOLTIP = "These materials will be delivered once a Duplicant is available";

				// Token: 0x0400CBC4 RID: 52164
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400CBC5 RID: 52165
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x020031BF RID: 12735
			public class WAITINGFORRADIATION
			{
				// Token: 0x0400CBC6 RID: 52166
				public static LocString NAME = "Awaiting Radbolts";

				// Token: 0x0400CBC7 RID: 52167
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires Radbolts to function\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to view this building's radiation port"
				});
			}

			// Token: 0x020031C0 RID: 12736
			public class WAITINGFORREPAIRMATERIALS
			{
				// Token: 0x0400CBC8 RID: 52168
				public static LocString NAME = "Awaiting Repair Delivery\n{ItemsRemaining}\n";

				// Token: 0x0400CBC9 RID: 52169
				public static LocString TOOLTIP = "These materials must be delivered before this building can be repaired";

				// Token: 0x0400CBCA RID: 52170
				public static LocString LINE_ITEM = string.Concat(new string[]
				{
					"• ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					": <b>{1}</b>"
				});
			}

			// Token: 0x020031C1 RID: 12737
			public class MISSINGGANTRY
			{
				// Token: 0x0400CBCB RID: 52171
				public static LocString NAME = "Missing Gantry";

				// Token: 0x0400CBCC RID: 52172
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Gantry", "GANTRY"),
					" must be built below ",
					UI.FormatAsLink("Command Capsules", "COMMANDMODULE"),
					" and ",
					UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE"),
					" for Duplicant access"
				});
			}

			// Token: 0x020031C2 RID: 12738
			public class DISEMBARKINGDUPLICANT
			{
				// Token: 0x0400CBCD RID: 52173
				public static LocString NAME = "Waiting To Disembark";

				// Token: 0x0400CBCE RID: 52174
				public static LocString TOOLTIP = "The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended";
			}

			// Token: 0x020031C3 RID: 12739
			public class REACTORMELTDOWN
			{
				// Token: 0x0400CBCF RID: 52175
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400CBD0 RID: 52176
				public static LocString TOOLTIP = "This reactor is spilling dangerous radioactive waste and cannot be stopped";
			}

			// Token: 0x020031C4 RID: 12740
			public class ROCKETNAME
			{
				// Token: 0x0400CBD1 RID: 52177
				public static LocString NAME = "Parent Rocket: {0}";

				// Token: 0x0400CBD2 RID: 52178
				public static LocString TOOLTIP = "This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x020031C5 RID: 12741
			public class HASGANTRY
			{
				// Token: 0x0400CBD3 RID: 52179
				public static LocString NAME = "Has Gantry";

				// Token: 0x0400CBD4 RID: 52180
				public static LocString TOOLTIP = "Duplicants may now enter this section of the rocket";
			}

			// Token: 0x020031C6 RID: 12742
			public class NORMAL
			{
				// Token: 0x0400CBD5 RID: 52181
				public static LocString NAME = "Normal";

				// Token: 0x0400CBD6 RID: 52182
				public static LocString TOOLTIP = "Nothing out of the ordinary here";
			}

			// Token: 0x020031C7 RID: 12743
			public class MANUALGENERATORCHARGINGUP
			{
				// Token: 0x0400CBD7 RID: 52183
				public static LocString NAME = "Charging Up";

				// Token: 0x0400CBD8 RID: 52184
				public static LocString TOOLTIP = "This power source is being charged";
			}

			// Token: 0x020031C8 RID: 12744
			public class MANUALGENERATORRELEASINGENERGY
			{
				// Token: 0x0400CBD9 RID: 52185
				public static LocString NAME = "Powering";

				// Token: 0x0400CBDA RID: 52186
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This generator is supplying energy to ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers"
				});
			}

			// Token: 0x020031C9 RID: 12745
			public class GENERATOROFFLINE
			{
				// Token: 0x0400CBDB RID: 52187
				public static LocString NAME = "Generator Idle";

				// Token: 0x0400CBDC RID: 52188
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source is idle"
				});
			}

			// Token: 0x020031CA RID: 12746
			public class PIPE
			{
				// Token: 0x0400CBDD RID: 52189
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400CBDE RID: 52190
				public static LocString TOOLTIP = "This pipe is delivering {Contents}";
			}

			// Token: 0x020031CB RID: 12747
			public class CONVEYOR
			{
				// Token: 0x0400CBDF RID: 52191
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400CBE0 RID: 52192
				public static LocString TOOLTIP = "This conveyor is delivering {Contents}";
			}

			// Token: 0x020031CC RID: 12748
			public class FABRICATORIDLE
			{
				// Token: 0x0400CBE1 RID: 52193
				public static LocString NAME = "No Fabrications Queued";

				// Token: 0x0400CBE2 RID: 52194
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x020031CD RID: 12749
			public class FABRICATOREMPTY
			{
				// Token: 0x0400CBE3 RID: 52195
				public static LocString NAME = "Waiting For Materials";

				// Token: 0x0400CBE4 RID: 52196
				public static LocString TOOLTIP = "Fabrication will begin once materials have been delivered";
			}

			// Token: 0x020031CE RID: 12750
			public class FABRICATORLACKSHEP
			{
				// Token: 0x0400CBE5 RID: 52197
				public static LocString NAME = "Waiting For Radbolts ({CurrentHEP}/{HEPRequired})";

				// Token: 0x0400CBE6 RID: 52198
				public static LocString TOOLTIP = "A queued recipe requires more Radbolts than are currently stored.\n\nCurrently stored: {CurrentHEP}\nRequired for recipe: {HEPRequired}";
			}

			// Token: 0x020031CF RID: 12751
			public class TOILET
			{
				// Token: 0x0400CBE7 RID: 52199
				public static LocString NAME = "{FlushesRemaining} \"Visits\" Remaining";

				// Token: 0x0400CBE8 RID: 52200
				public static LocString TOOLTIP = "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
			}

			// Token: 0x020031D0 RID: 12752
			public class TOILETNEEDSEMPTYING
			{
				// Token: 0x0400CBE9 RID: 52201
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400CBEA RID: 52202
				public static LocString TOOLTIP = "This amenity cannot be used while full\n\nEmptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND");
			}

			// Token: 0x020031D1 RID: 12753
			public class DESALINATORNEEDSEMPTYING
			{
				// Token: 0x0400CBEB RID: 52203
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400CBEC RID: 52204
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Salt", "SALT") + " to resume function";
			}

			// Token: 0x020031D2 RID: 12754
			public class MILKSEPARATORNEEDSEMPTYING
			{
				// Token: 0x0400CBED RID: 52205
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400CBEE RID: 52206
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Brackwax", "MILKFAT") + " to resume function";
			}

			// Token: 0x020031D3 RID: 12755
			public class HABITATNEEDSEMPTYING
			{
				// Token: 0x0400CBEF RID: 52207
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400CBF0 RID: 52208
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT"),
					" needs to be emptied of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"\n\n",
					UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER"),
					" can be used to transport and dispose of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" in designated areas"
				});
			}

			// Token: 0x020031D4 RID: 12756
			public class UNUSABLE
			{
				// Token: 0x0400CBF1 RID: 52209
				public static LocString NAME = "Out of Order";

				// Token: 0x0400CBF2 RID: 52210
				public static LocString TOOLTIP = "This amenity requires maintenance";
			}

			// Token: 0x020031D5 RID: 12757
			public class UNUSABLEGUNKED
			{
				// Token: 0x0400CBF3 RID: 52211
				public static LocString NAME = "Out of Order: Gunk";

				// Token: 0x0400CBF4 RID: 52212
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Someone dumped ",
					UI.FormatAsLink("Gunk", "LIQUIDGUNK"),
					" here instead of in a ",
					UI.FormatAsLink("Gunk Extractor", "GUNKEMPTIER"),
					"\n\nThis amenity requires maintenance"
				});
			}

			// Token: 0x020031D6 RID: 12758
			public class NORESEARCHSELECTED
			{
				// Token: 0x0400CBF5 RID: 52213
				public static LocString NAME = "No Research Focus Selected";

				// Token: 0x0400CBF6 RID: 52214
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});

				// Token: 0x0400CBF7 RID: 52215
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " selected";

				// Token: 0x0400CBF8 RID: 52216
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});
			}

			// Token: 0x020031D7 RID: 12759
			public class NORESEARCHORDESTINATIONSELECTED
			{
				// Token: 0x0400CBF9 RID: 52217
				public static LocString NAME = "No Research Focus or Starmap Destination Selected";

				// Token: 0x0400CBFA RID: 52218
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap)
				});

				// Token: 0x0400CBFB RID: 52219
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected";

				// Token: 0x0400CBFC RID: 52220
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", "[R]"),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", "[Z]")
				});
			}

			// Token: 0x020031D8 RID: 12760
			public class RESEARCHING
			{
				// Token: 0x0400CBFD RID: 52221
				public static LocString NAME = "Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}";

				// Token: 0x0400CBFE RID: 52222
				public static LocString TOOLTIP = "Research produced at this station will be invested in {Tech}";
			}

			// Token: 0x020031D9 RID: 12761
			public class TINKERING
			{
				// Token: 0x0400CBFF RID: 52223
				public static LocString NAME = "Tinkering: {0}";

				// Token: 0x0400CC00 RID: 52224
				public static LocString TOOLTIP = "This Duplicant is creating {0} to use somewhere else";
			}

			// Token: 0x020031DA RID: 12762
			public class VALVE
			{
				// Token: 0x0400CC01 RID: 52225
				public static LocString NAME = "Max Flow Rate: {MaxFlow}";

				// Token: 0x0400CC02 RID: 52226
				public static LocString TOOLTIP = "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
			}

			// Token: 0x020031DB RID: 12763
			public class VALVEREQUEST
			{
				// Token: 0x0400CC03 RID: 52227
				public static LocString NAME = "Requested Flow Rate: {QueuedMaxFlow}";

				// Token: 0x0400CC04 RID: 52228
				public static LocString TOOLTIP = "Waiting for a Duplicant to adjust flow rate";
			}

			// Token: 0x020031DC RID: 12764
			public class EMITTINGLIGHT
			{
				// Token: 0x0400CC05 RID: 52229
				public static LocString NAME = "Emitting Light";

				// Token: 0x0400CC06 RID: 52230
				public static LocString TOOLTIP = "Open the " + UI.FormatAsOverlay("Light Overlay", global::Action.Overlay5) + " to view this light's visibility radius";
			}

			// Token: 0x020031DD RID: 12765
			public class KETTLEINSUFICIENTSOLIDS
			{
				// Token: 0x0400CC07 RID: 52231
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Ice", "ICE");

				// Token: 0x0400CC08 RID: 52232
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires a minimum of {0} ",
					UI.FormatAsLink("Ice", "ICE"),
					" in order to function\n\nDeliver more ",
					UI.FormatAsLink("Ice", "ICE"),
					" to operate this building"
				});
			}

			// Token: 0x020031DE RID: 12766
			public class KETTLEINSUFICIENTFUEL
			{
				// Token: 0x0400CC09 RID: 52233
				public static LocString NAME = "Insufficient " + UI.FormatAsLink("Wood", "WOODLOG");

				// Token: 0x0400CC0A RID: 52234
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Colder ",
					UI.FormatAsLink("Ice", "ICE"),
					" increases the amount of ",
					UI.FormatAsLink("Wood", "WOODLOG"),
					" required for melting\n\nCurrent requirement: minimum {0} ",
					UI.FormatAsLink("Wood", "WOODLOG")
				});
			}

			// Token: 0x020031DF RID: 12767
			public class KETTLEINSUFICIENTLIQUIDSPACE
			{
				// Token: 0x0400CC0B RID: 52235
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400CC0C RID: 52236
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Ice Liquefier", "ICEKETTLE"),
					" needs to be emptied of ",
					UI.FormatAsLink("Water", "WATER"),
					" in order to resume function\n\nIt requires at least {2} of storage space in order to function properly\n\nCurrently storing {0} of a maximum {1} ",
					UI.FormatAsLink("Water", "WATER")
				});
			}

			// Token: 0x020031E0 RID: 12768
			public class KETTLEMELTING
			{
				// Token: 0x0400CC0D RID: 52237
				public static LocString NAME = "Melting Ice";

				// Token: 0x0400CC0E RID: 52238
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is currently melting stored ",
					UI.FormatAsLink("Ice", "ICE"),
					" to produce ",
					UI.FormatAsLink("Water", "WATER"),
					"\n\n",
					UI.FormatAsLink("Water", "WATER"),
					" output temperature: {0}"
				});
			}

			// Token: 0x020031E1 RID: 12769
			public class RATIONBOXCONTENTS
			{
				// Token: 0x0400CC0F RID: 52239
				public static LocString NAME = "Storing: {Stored}";

				// Token: 0x0400CC10 RID: 52240
				public static LocString TOOLTIP = "This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x020031E2 RID: 12770
			public class EMITTINGELEMENT
			{
				// Token: 0x0400CC11 RID: 52241
				public static LocString NAME = "Emitting {ElementType}: {FlowRate}";

				// Token: 0x0400CC12 RID: 52242
				public static LocString TOOLTIP = "Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031E3 RID: 12771
			public class EMITTINGCO2
			{
				// Token: 0x0400CC13 RID: 52243
				public static LocString NAME = "Emitting CO<sub>2</sub>: {FlowRate}";

				// Token: 0x0400CC14 RID: 52244
				public static LocString TOOLTIP = "Producing " + ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031E4 RID: 12772
			public class EMITTINGOXYGENAVG
			{
				// Token: 0x0400CC15 RID: 52245
				public static LocString NAME = "Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}";

				// Token: 0x0400CC16 RID: 52246
				public static LocString TOOLTIP = "Producing " + ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031E5 RID: 12773
			public class EMITTINGGASAVG
			{
				// Token: 0x0400CC17 RID: 52247
				public static LocString NAME = "Emitting {Element}: {FlowRate}";

				// Token: 0x0400CC18 RID: 52248
				public static LocString TOOLTIP = "Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031E6 RID: 12774
			public class EMITTINGBLOCKEDHIGHPRESSURE
			{
				// Token: 0x0400CC19 RID: 52249
				public static LocString NAME = "Not Emitting: Overpressure";

				// Token: 0x0400CC1A RID: 52250
				public static LocString TOOLTIP = "Ambient pressure is too high for {Element} to be released";
			}

			// Token: 0x020031E7 RID: 12775
			public class EMITTINGBLOCKEDLOWTEMPERATURE
			{
				// Token: 0x0400CC1B RID: 52251
				public static LocString NAME = "Not Emitting: Too Cold";

				// Token: 0x0400CC1C RID: 52252
				public static LocString TOOLTIP = "Temperature is too low for {Element} to be released";
			}

			// Token: 0x020031E8 RID: 12776
			public class PUMPINGLIQUIDORGAS
			{
				// Token: 0x0400CC1D RID: 52253
				public static LocString NAME = "Average Flow Rate: {FlowRate}";

				// Token: 0x0400CC1E RID: 52254
				public static LocString TOOLTIP = "This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031E9 RID: 12777
			public class WIRECIRCUITSTATUS
			{
				// Token: 0x0400CC1F RID: 52255
				public static LocString NAME = "Current Load: {CurrentLoadAndColor} / {MaxLoad}";

				// Token: 0x0400CC20 RID: 52256
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The current ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break"
				});
			}

			// Token: 0x020031EA RID: 12778
			public class WIREMAXWATTAGESTATUS
			{
				// Token: 0x0400CC21 RID: 52257
				public static LocString NAME = "Potential Load: {TotalPotentialLoadAndColor} / {MaxLoad}";

				// Token: 0x0400CC22 RID: 52258
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"How much wattage this network will draw if all ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers on the network become active at once"
				});
			}

			// Token: 0x020031EB RID: 12779
			public class NOLIQUIDELEMENTTOPUMP
			{
				// Token: 0x0400CC23 RID: 52259
				public static LocString NAME = "Pump Not In Liquid";

				// Token: 0x0400CC24 RID: 52260
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020031EC RID: 12780
			public class NOGASELEMENTTOPUMP
			{
				// Token: 0x0400CC25 RID: 52261
				public static LocString NAME = "Pump Not In Gas";

				// Token: 0x0400CC26 RID: 52262
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020031ED RID: 12781
			public class INVALIDMASKSTATIONCONSUMPTIONSTATE
			{
				// Token: 0x0400CC27 RID: 52263
				public static LocString NAME = "Station Not In Oxygen";

				// Token: 0x0400CC28 RID: 52264
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This station must be submerged in ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x020031EE RID: 12782
			public class PIPEMAYMELT
			{
				// Token: 0x0400CC29 RID: 52265
				public static LocString NAME = "High Melt Risk";

				// Token: 0x0400CC2A RID: 52266
				public static LocString TOOLTIP = "This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x020031EF RID: 12783
			public class ELEMENTEMITTEROUTPUT
			{
				// Token: 0x0400CC2B RID: 52267
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400CC2C RID: 52268
				public static LocString TOOLTIP = "This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031F0 RID: 12784
			public class ELEMENTCONSUMER
			{
				// Token: 0x0400CC2D RID: 52269
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400CC2E RID: 52270
				public static LocString TOOLTIP = "This object is utilizing ambient {ElementTypes} from the environment";
			}

			// Token: 0x020031F1 RID: 12785
			public class SPACECRAFTREADYTOLAND
			{
				// Token: 0x0400CC2F RID: 52271
				public static LocString NAME = "Spacecraft ready to land";

				// Token: 0x0400CC30 RID: 52272
				public static LocString TOOLTIP = "A spacecraft is ready to land";

				// Token: 0x0400CC31 RID: 52273
				public static LocString NOTIFICATION = "Space mission complete";

				// Token: 0x0400CC32 RID: 52274
				public static LocString NOTIFICATION_TOOLTIP = "Spacecrafts have completed their missions";
			}

			// Token: 0x020031F2 RID: 12786
			public class CONSUMINGFROMSTORAGE
			{
				// Token: 0x0400CC33 RID: 52275
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400CC34 RID: 52276
				public static LocString TOOLTIP = "This building is consuming {ElementTypes} from storage";
			}

			// Token: 0x020031F3 RID: 12787
			public class ELEMENTCONVERTEROUTPUT
			{
				// Token: 0x0400CC35 RID: 52277
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400CC36 RID: 52278
				public static LocString TOOLTIP = "This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x020031F4 RID: 12788
			public class ELEMENTCONVERTERINPUT
			{
				// Token: 0x0400CC37 RID: 52279
				public static LocString NAME = "Using {ElementTypes}: {FlowRate}";

				// Token: 0x0400CC38 RID: 52280
				public static LocString TOOLTIP = "This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}");
			}

			// Token: 0x020031F5 RID: 12789
			public class AWAITINGCOMPOSTFLIP
			{
				// Token: 0x0400CC39 RID: 52281
				public static LocString NAME = "Requires Flipping";

				// Token: 0x0400CC3A RID: 52282
				public static LocString TOOLTIP = "Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT");
			}

			// Token: 0x020031F6 RID: 12790
			public class AWAITINGWASTE
			{
				// Token: 0x0400CC3B RID: 52283
				public static LocString NAME = "Awaiting Compostables";

				// Token: 0x0400CC3C RID: 52284
				public static LocString TOOLTIP = "More waste material is required to begin the composting process";
			}

			// Token: 0x020031F7 RID: 12791
			public class BATTERIESSUFFICIENTLYFULL
			{
				// Token: 0x0400CC3D RID: 52285
				public static LocString NAME = "Batteries Sufficiently Full";

				// Token: 0x0400CC3E RID: 52286
				public static LocString TOOLTIP = "All batteries are above the refill threshold";
			}

			// Token: 0x020031F8 RID: 12792
			public class NEEDRESOURCEMASS
			{
				// Token: 0x0400CC3F RID: 52287
				public static LocString NAME = "Insufficient Resources\n{ResourcesRequired}";

				// Token: 0x0400CC40 RID: 52288
				public static LocString TOOLTIP = "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";

				// Token: 0x0400CC41 RID: 52289
				public static LocString LINE_ITEM = "• <b>{0}</b>";
			}

			// Token: 0x020031F9 RID: 12793
			public class JOULESAVAILABLE
			{
				// Token: 0x0400CC42 RID: 52290
				public static LocString NAME = "Power Available: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400CC43 RID: 52291
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use"
				});
			}

			// Token: 0x020031FA RID: 12794
			public class WATTAGE
			{
				// Token: 0x0400CC44 RID: 52292
				public static LocString NAME = "Wattage: {Wattage}";

				// Token: 0x0400CC45 RID: 52293
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031FB RID: 12795
			public class SOLARPANELWATTAGE
			{
				// Token: 0x0400CC46 RID: 52294
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400CC47 RID: 52295
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031FC RID: 12796
			public class MODULESOLARPANELWATTAGE
			{
				// Token: 0x0400CC48 RID: 52296
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400CC49 RID: 52297
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020031FD RID: 12797
			public class WATTSON
			{
				// Token: 0x0400CC4A RID: 52298
				public static LocString NAME = "Next Print: {TimeRemaining}";

				// Token: 0x0400CC4B RID: 52299
				public static LocString TOOLTIP = "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";

				// Token: 0x0400CC4C RID: 52300
				public static LocString UNAVAILABLE = "UNAVAILABLE";
			}

			// Token: 0x020031FE RID: 12798
			public class FLUSHTOILET
			{
				// Token: 0x0400CC4D RID: 52301
				public static LocString NAME = "{toilet} Ready";

				// Token: 0x0400CC4E RID: 52302
				public static LocString TOOLTIP = "This bathroom is ready to receive visitors";
			}

			// Token: 0x020031FF RID: 12799
			public class FLUSHTOILETINUSE
			{
				// Token: 0x0400CC4F RID: 52303
				public static LocString NAME = "{toilet} In Use";

				// Token: 0x0400CC50 RID: 52304
				public static LocString TOOLTIP = "This bathroom is occupied";
			}

			// Token: 0x02003200 RID: 12800
			public class WIRECONNECTED
			{
				// Token: 0x0400CC51 RID: 52305
				public static LocString NAME = "Wire Connected";

				// Token: 0x0400CC52 RID: 52306
				public static LocString TOOLTIP = "This wire is connected to a network";
			}

			// Token: 0x02003201 RID: 12801
			public class WIRENOMINAL
			{
				// Token: 0x0400CC53 RID: 52307
				public static LocString NAME = "Wire Nominal";

				// Token: 0x0400CC54 RID: 52308
				public static LocString TOOLTIP = "This wire is able to handle the wattage it is receiving";
			}

			// Token: 0x02003202 RID: 12802
			public class WIREDISCONNECTED
			{
				// Token: 0x0400CC55 RID: 52309
				public static LocString NAME = "Wire Disconnected";

				// Token: 0x0400CC56 RID: 52310
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This wire is not connecting a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumer to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" generator"
				});
			}

			// Token: 0x02003203 RID: 12803
			public class COOLING
			{
				// Token: 0x0400CC57 RID: 52311
				public static LocString NAME = "Cooling";

				// Token: 0x0400CC58 RID: 52312
				public static LocString TOOLTIP = "This building is cooling the surrounding area";
			}

			// Token: 0x02003204 RID: 12804
			public class COOLINGSTALLEDHOTENV
			{
				// Token: 0x0400CC59 RID: 52313
				public static LocString NAME = "Gas Too Hot";

				// Token: 0x0400CC5A RID: 52314
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02003205 RID: 12805
			public class COOLINGSTALLEDCOLDGAS
			{
				// Token: 0x0400CC5B RID: 52315
				public static LocString NAME = "Gas Too Cold";

				// Token: 0x0400CC5C RID: 52316
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02003206 RID: 12806
			public class COOLINGSTALLEDHOTLIQUID
			{
				// Token: 0x0400CC5D RID: 52317
				public static LocString NAME = "Liquid Too Hot";

				// Token: 0x0400CC5E RID: 52318
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02003207 RID: 12807
			public class COOLINGSTALLEDCOLDLIQUID
			{
				// Token: 0x0400CC5F RID: 52319
				public static LocString NAME = "Liquid Too Cold";

				// Token: 0x0400CC60 RID: 52320
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02003208 RID: 12808
			public class CANNOTCOOLFURTHER
			{
				// Token: 0x0400CC61 RID: 52321
				public static LocString NAME = "Minimum Temperature Reached";

				// Token: 0x0400CC62 RID: 52322
				public static LocString TOOLTIP = "This building cannot cool the surrounding environment below <b>{0}</b>";
			}

			// Token: 0x02003209 RID: 12809
			public class HEATINGSTALLEDHOTENV
			{
				// Token: 0x0400CC63 RID: 52323
				public static LocString NAME = "Target Temperature Reached";

				// Token: 0x0400CC64 RID: 52324
				public static LocString TOOLTIP = "This building cannot heat the surrounding environment beyond <b>{0}</b>";
			}

			// Token: 0x0200320A RID: 12810
			public class HEATINGSTALLEDLOWMASS_GAS
			{
				// Token: 0x0400CC65 RID: 52325
				public static LocString NAME = "Insufficient Atmosphere";

				// Token: 0x0400CC66 RID: 52326
				public static LocString TOOLTIP = "This building cannot operate in a vacuum";
			}

			// Token: 0x0200320B RID: 12811
			public class HEATINGSTALLEDLOWMASS_LIQUID
			{
				// Token: 0x0400CC67 RID: 52327
				public static LocString NAME = "Not Submerged In Liquid";

				// Token: 0x0400CC68 RID: 52328
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x0200320C RID: 12812
			public class BUILDINGDISABLED
			{
				// Token: 0x0400CC69 RID: 52329
				public static LocString NAME = "Building Disabled";

				// Token: 0x0400CC6A RID: 52330
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Press ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume use"
				});
			}

			// Token: 0x0200320D RID: 12813
			public class MISSINGREQUIREMENTS
			{
				// Token: 0x0400CC6B RID: 52331
				public static LocString NAME = "Missing Requirements";

				// Token: 0x0400CC6C RID: 52332
				public static LocString TOOLTIP = "There are some problems that need to be fixed before this building is operational";
			}

			// Token: 0x0200320E RID: 12814
			public class GETTINGREADY
			{
				// Token: 0x0400CC6D RID: 52333
				public static LocString NAME = "Getting Ready";

				// Token: 0x0400CC6E RID: 52334
				public static LocString TOOLTIP = "This building will soon be ready to use";
			}

			// Token: 0x0200320F RID: 12815
			public class WORKING
			{
				// Token: 0x0400CC6F RID: 52335
				public static LocString NAME = "Nominal";

				// Token: 0x0400CC70 RID: 52336
				public static LocString TOOLTIP = "This building is working as intended";
			}

			// Token: 0x02003210 RID: 12816
			public class GRAVEEMPTY
			{
				// Token: 0x0400CC71 RID: 52337
				public static LocString NAME = "Empty";

				// Token: 0x0400CC72 RID: 52338
				public static LocString TOOLTIP = "This memorial honors no one.";
			}

			// Token: 0x02003211 RID: 12817
			public class GRAVE
			{
				// Token: 0x0400CC73 RID: 52339
				public static LocString NAME = "RIP {DeadDupe}";

				// Token: 0x0400CC74 RID: 52340
				public static LocString TOOLTIP = "{Epitaph}";
			}

			// Token: 0x02003212 RID: 12818
			public class AWAITINGARTING
			{
				// Token: 0x0400CC75 RID: 52341
				public static LocString NAME = "Incomplete Artwork";

				// Token: 0x0400CC76 RID: 52342
				public static LocString TOOLTIP = "This building requires a Duplicant's artistic touch";
			}

			// Token: 0x02003213 RID: 12819
			public class LOOKINGUGLY
			{
				// Token: 0x0400CC77 RID: 52343
				public static LocString NAME = "Crude";

				// Token: 0x0400CC78 RID: 52344
				public static LocString TOOLTIP = "Honestly, Morbs could've done better";
			}

			// Token: 0x02003214 RID: 12820
			public class LOOKINGOKAY
			{
				// Token: 0x0400CC79 RID: 52345
				public static LocString NAME = "Quaint";

				// Token: 0x0400CC7A RID: 52346
				public static LocString TOOLTIP = "Duplicants find this art piece quite charming";
			}

			// Token: 0x02003215 RID: 12821
			public class LOOKINGGREAT
			{
				// Token: 0x0400CC7B RID: 52347
				public static LocString NAME = "Masterpiece";

				// Token: 0x0400CC7C RID: 52348
				public static LocString TOOLTIP = "This poignant piece stirs something deep within each Duplicant's soul";
			}

			// Token: 0x02003216 RID: 12822
			public class EXPIRED
			{
				// Token: 0x0400CC7D RID: 52349
				public static LocString NAME = "Depleted";

				// Token: 0x0400CC7E RID: 52350
				public static LocString TOOLTIP = "This building has no more use";
			}

			// Token: 0x02003217 RID: 12823
			public class COOLINGWATER
			{
				// Token: 0x0400CC7F RID: 52351
				public static LocString NAME = "Cooling Water";

				// Token: 0x0400CC80 RID: 52352
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is cooling ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" down to its freezing point"
				});
			}

			// Token: 0x02003218 RID: 12824
			public class EXCAVATOR_BOMB
			{
				// Token: 0x02003219 RID: 12825
				public class UNARMED
				{
					// Token: 0x0400CC81 RID: 52353
					public static LocString NAME = "Unarmed";

					// Token: 0x0400CC82 RID: 52354
					public static LocString TOOLTIP = "This explosive is currently inactive";
				}

				// Token: 0x0200321A RID: 12826
				public class ARMED
				{
					// Token: 0x0400CC83 RID: 52355
					public static LocString NAME = "Armed";

					// Token: 0x0400CC84 RID: 52356
					public static LocString TOOLTIP = "Stand back, this baby's ready to blow!";
				}

				// Token: 0x0200321B RID: 12827
				public class COUNTDOWN
				{
					// Token: 0x0400CC85 RID: 52357
					public static LocString NAME = "Countdown: {0}";

					// Token: 0x0400CC86 RID: 52358
					public static LocString TOOLTIP = "<b>{0}</b> seconds until detonation";
				}

				// Token: 0x0200321C RID: 12828
				public class DUPE_DANGER
				{
					// Token: 0x0400CC87 RID: 52359
					public static LocString NAME = "Duplicant Preservation Override";

					// Token: 0x0400CC88 RID: 52360
					public static LocString TOOLTIP = "Explosive disabled due to close Duplicant proximity";
				}

				// Token: 0x0200321D RID: 12829
				public class EXPLODING
				{
					// Token: 0x0400CC89 RID: 52361
					public static LocString NAME = "Exploding";

					// Token: 0x0400CC8A RID: 52362
					public static LocString TOOLTIP = "Kaboom!";
				}
			}

			// Token: 0x0200321E RID: 12830
			public class BURNER
			{
				// Token: 0x0200321F RID: 12831
				public class BURNING_FUEL
				{
					// Token: 0x0400CC8B RID: 52363
					public static LocString NAME = "Consuming Fuel: {0}";

					// Token: 0x0400CC8C RID: 52364
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}

				// Token: 0x02003220 RID: 12832
				public class HAS_FUEL
				{
					// Token: 0x0400CC8D RID: 52365
					public static LocString NAME = "Fueled: {0}";

					// Token: 0x0400CC8E RID: 52366
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}
			}

			// Token: 0x02003221 RID: 12833
			public class CREATURE_REUSABLE_TRAP
			{
				// Token: 0x02003222 RID: 12834
				public class NEEDS_ARMING
				{
					// Token: 0x0400CC8F RID: 52367
					public static LocString NAME = "Waiting to be Armed";

					// Token: 0x0400CC90 RID: 52368
					public static LocString TOOLTIP = "Waiting for a Duplicant to arm this trap\n\nOnly Duplicants with the " + DUPLICANTS.ROLES.RANCHER.NAME + " skill can arm traps";
				}

				// Token: 0x02003223 RID: 12835
				public class READY
				{
					// Token: 0x0400CC91 RID: 52369
					public static LocString NAME = "Armed";

					// Token: 0x0400CC92 RID: 52370
					public static LocString TOOLTIP = "This trap has been armed and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003224 RID: 12836
				public class SPRUNG
				{
					// Token: 0x0400CC93 RID: 52371
					public static LocString NAME = "Sprung";

					// Token: 0x0400CC94 RID: 52372
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02003225 RID: 12837
			public class CREATURE_TRAP
			{
				// Token: 0x02003226 RID: 12838
				public class NEEDSBAIT
				{
					// Token: 0x0400CC95 RID: 52373
					public static LocString NAME = "Needs Bait";

					// Token: 0x0400CC96 RID: 52374
					public static LocString TOOLTIP = "This trap needs to be baited before it can be set";
				}

				// Token: 0x02003227 RID: 12839
				public class READY
				{
					// Token: 0x0400CC97 RID: 52375
					public static LocString NAME = "Set";

					// Token: 0x0400CC98 RID: 52376
					public static LocString TOOLTIP = "This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003228 RID: 12840
				public class SPRUNG
				{
					// Token: 0x0400CC99 RID: 52377
					public static LocString NAME = "Sprung";

					// Token: 0x0400CC9A RID: 52378
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x02003229 RID: 12841
			public class ACCESS_CONTROL
			{
				// Token: 0x0200322A RID: 12842
				public class ACTIVE
				{
					// Token: 0x0400CC9B RID: 52379
					public static LocString NAME = "Access Restrictions";

					// Token: 0x0400CC9C RID: 52380
					public static LocString TOOLTIP = "Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD;
				}

				// Token: 0x0200322B RID: 12843
				public class OFFLINE
				{
					// Token: 0x0400CC9D RID: 52381
					public static LocString NAME = "Access Control Offline";

					// Token: 0x0400CC9E RID: 52382
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This door has granted Emergency ",
						UI.PRE_KEYWORD,
						"Access Permissions",
						UI.PST_KEYWORD,
						"\n\nAll Duplicants are permitted to pass through it until ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" is restored"
					});
				}
			}

			// Token: 0x0200322C RID: 12844
			public class REQUIRESSKILLPERK
			{
				// Token: 0x0400CC9F RID: 52383
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400CCA0 RID: 52384
				public static LocString TOOLTIP = "Only Duplicants with the {Skills} Skill can operate this building";

				// Token: 0x0400CCA1 RID: 52385
				public static LocString TOOLTIP_DLC3 = "Only Duplicants with the {Skills} Skill or {Boosters} can operate this building";
			}

			// Token: 0x0200322D RID: 12845
			public class DIGREQUIRESSKILLPERK
			{
				// Token: 0x0400CCA2 RID: 52386
				public static LocString NAME = "Skill-Required Dig";

				// Token: 0x0400CCA3 RID: 52387
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can mine this material:\n{Skills}"
				});

				// Token: 0x0400CCA4 RID: 52388
				public static LocString TOOLTIP_DLC3 = "Only Duplicants with the {Skills} Skill or {Boosters} can mine this material";
			}

			// Token: 0x0200322E RID: 12846
			public class COLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400CCA5 RID: 52389
				public static LocString NAME = "Colony Lacks {Skills} Skill";

				// Token: 0x0400CCA6 RID: 52390
				public static LocString TOOLTIP = "{Skills} Skill required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";

				// Token: 0x0400CCA7 RID: 52391
				public static LocString TOOLTIP_DLC3 = "{Skills} Skill or {Boosters} required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";
			}

			// Token: 0x0200322F RID: 12847
			public class CLUSTERCOLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400CCA8 RID: 52392
				public static LocString NAME = "Local Colony Lacks {Skills} Skill";

				// Token: 0x0400CCA9 RID: 52393
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP + ", or bring a Duplicant with the skill from another " + UI.CLUSTERMAP.PLANETOID;

				// Token: 0x0400CCAA RID: 52394
				public static LocString TOOLTIP_DLC3 = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP_DLC3 + ", or bring a Duplicant with this skill or booster from another " + UI.CLUSTERMAP.PLANETOID;
			}

			// Token: 0x02003230 RID: 12848
			public class WORKREQUIRESMINION
			{
				// Token: 0x0400CCAB RID: 52395
				public static LocString NAME = "Duplicant Operation Required";

				// Token: 0x0400CCAC RID: 52396
				public static LocString TOOLTIP = "A Duplicant must be present to complete this operation";
			}

			// Token: 0x02003231 RID: 12849
			public class SWITCHSTATUSACTIVE
			{
				// Token: 0x0400CCAD RID: 52397
				public static LocString NAME = "Active";

				// Token: 0x0400CCAE RID: 52398
				public static LocString TOOLTIP = "This switch is currently toggled <b>On</b>";
			}

			// Token: 0x02003232 RID: 12850
			public class SWITCHSTATUSINACTIVE
			{
				// Token: 0x0400CCAF RID: 52399
				public static LocString NAME = "Inactive";

				// Token: 0x0400CCB0 RID: 52400
				public static LocString TOOLTIP = "This switch is currently toggled <b>Off</b>";
			}

			// Token: 0x02003233 RID: 12851
			public class LOGICSWITCHSTATUSACTIVE
			{
				// Token: 0x0400CCB1 RID: 52401
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400CCB2 RID: 52402
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x02003234 RID: 12852
			public class LOGICSWITCHSTATUSINACTIVE
			{
				// Token: 0x0400CCB3 RID: 52403
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400CCB4 RID: 52404
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02003235 RID: 12853
			public class LOGICSENSORSTATUSACTIVE
			{
				// Token: 0x0400CCB5 RID: 52405
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400CCB6 RID: 52406
				public static LocString TOOLTIP = "This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x02003236 RID: 12854
			public class LOGICSENSORSTATUSINACTIVE
			{
				// Token: 0x0400CCB7 RID: 52407
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400CCB8 RID: 52408
				public static LocString TOOLTIP = "This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02003237 RID: 12855
			public class PLAYERCONTROLLEDTOGGLESIDESCREEN
			{
				// Token: 0x0400CCB9 RID: 52409
				public static LocString NAME = "Pending Toggle on Unpause";

				// Token: 0x0400CCBA RID: 52410
				public static LocString TOOLTIP = "This will be toggled when time is unpaused";
			}

			// Token: 0x02003238 RID: 12856
			public class FOOD_CONTAINERS_OUTSIDE_RANGE
			{
				// Token: 0x0400CCBB RID: 52411
				public static LocString NAME = "Unreachable food";

				// Token: 0x0400CCBC RID: 52412
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x02003239 RID: 12857
			public class TOILETS_OUTSIDE_RANGE
			{
				// Token: 0x0400CCBD RID: 52413
				public static LocString NAME = "Unreachable restroom";

				// Token: 0x0400CCBE RID: 52414
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Toilets",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x0200323A RID: 12858
			public class BUILDING_DEPRECATED
			{
				// Token: 0x0400CCBF RID: 52415
				public static LocString NAME = "Building Deprecated";

				// Token: 0x0400CCC0 RID: 52416
				public static LocString TOOLTIP = "This building is from an older version of the game and its use is not intended";
			}

			// Token: 0x0200323B RID: 12859
			public class TURBINE_BLOCKED_INPUT
			{
				// Token: 0x0400CCC1 RID: 52417
				public static LocString NAME = "All Inputs Blocked";

				// Token: 0x0400CCC2 RID: 52418
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine's ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are blocked, so it can't intake any ",
					ELEMENTS.STEAM.NAME,
					".\n\nThe ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are located directly below the foundation ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" this building is resting on."
				});
			}

			// Token: 0x0200323C RID: 12860
			public class TURBINE_PARTIALLY_BLOCKED_INPUT
			{
				// Token: 0x0400CCC3 RID: 52419
				public static LocString NAME = "{Blocked}/{Total} Inputs Blocked";

				// Token: 0x0400CCC4 RID: 52420
				public static LocString TOOLTIP = "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput";
			}

			// Token: 0x0200323D RID: 12861
			public class TURBINE_TOO_HOT
			{
				// Token: 0x0400CCC5 RID: 52421
				public static LocString NAME = "Turbine Too Hot";

				// Token: 0x0400CCC6 RID: 52422
				public static LocString TOOLTIP = "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
			}

			// Token: 0x0200323E RID: 12862
			public class TURBINE_BLOCKED_OUTPUT
			{
				// Token: 0x0400CCC7 RID: 52423
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400CCC8 RID: 52424
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A blocked ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" has stopped this turbine from functioning"
				});
			}

			// Token: 0x0200323F RID: 12863
			public class TURBINE_INSUFFICIENT_MASS
			{
				// Token: 0x0400CCC9 RID: 52425
				public static LocString NAME = "Not Enough {Src_Element}";

				// Token: 0x0400CCCA RID: 52426
				public static LocString TOOLTIP = "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
			}

			// Token: 0x02003240 RID: 12864
			public class TURBINE_INSUFFICIENT_TEMPERATURE
			{
				// Token: 0x0400CCCB RID: 52427
				public static LocString NAME = "{Src_Element} Temperature Below {Active_Temperature}";

				// Token: 0x0400CCCC RID: 52428
				public static LocString TOOLTIP = "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
			}

			// Token: 0x02003241 RID: 12865
			public class TURBINE_ACTIVE_WATTAGE
			{
				// Token: 0x0400CCCD RID: 52429
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400CCCE RID: 52430
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt is running at <b>{Efficiency}</b> of full capacity\n\nIncrease {Src_Element} ",
					UI.PRE_KEYWORD,
					"Mass",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" to improve output"
				});
			}

			// Token: 0x02003242 RID: 12866
			public class TURBINE_SPINNING_UP
			{
				// Token: 0x0400CCCF RID: 52431
				public static LocString NAME = "Spinning Up";

				// Token: 0x0400CCD0 RID: 52432
				public static LocString TOOLTIP = "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
			}

			// Token: 0x02003243 RID: 12867
			public class TURBINE_ACTIVE
			{
				// Token: 0x0400CCD1 RID: 52433
				public static LocString NAME = "Active";

				// Token: 0x0400CCD2 RID: 52434
				public static LocString TOOLTIP = "This turbine is running at <b>{0}RPM</b>";
			}

			// Token: 0x02003244 RID: 12868
			public class WELL_PRESSURIZING
			{
				// Token: 0x0400CCD3 RID: 52435
				public static LocString NAME = "Backpressure: {0}";

				// Token: 0x0400CCD4 RID: 52436
				public static LocString TOOLTIP = "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
			}

			// Token: 0x02003245 RID: 12869
			public class WELL_OVERPRESSURE
			{
				// Token: 0x0400CCD5 RID: 52437
				public static LocString NAME = "Overpressure";

				// Token: 0x0400CCD6 RID: 52438
				public static LocString TOOLTIP = "This well can no longer function due to excessive backpressure";
			}

			// Token: 0x02003246 RID: 12870
			public class NOTINANYROOM
			{
				// Token: 0x0400CCD7 RID: 52439
				public static LocString NAME = "Outside of room";

				// Token: 0x0400CCD8 RID: 52440
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02003247 RID: 12871
			public class NOTINREQUIREDROOM
			{
				// Token: 0x0400CCD9 RID: 52441
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400CCDA RID: 52442
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a {0} for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02003248 RID: 12872
			public class NOTINRECOMMENDEDROOM
			{
				// Token: 0x0400CCDB RID: 52443
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400CCDC RID: 52444
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"It is recommended to build this building inside a {0}\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x02003249 RID: 12873
			public class RELEASING_PRESSURE
			{
				// Token: 0x0400CCDD RID: 52445
				public static LocString NAME = "Releasing Pressure";

				// Token: 0x0400CCDE RID: 52446
				public static LocString TOOLTIP = "Pressure buildup is being safely released";
			}

			// Token: 0x0200324A RID: 12874
			public class LOGIC_FEEDBACK_LOOP
			{
				// Token: 0x0400CCDF RID: 52447
				public static LocString NAME = "Feedback Loop";

				// Token: 0x0400CCE0 RID: 52448
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" of an automated building connects back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" through the Automation grid"
				});
			}

			// Token: 0x0200324B RID: 12875
			public class ENOUGH_COOLANT
			{
				// Token: 0x0400CCE1 RID: 52449
				public static LocString NAME = "Awaiting Coolant";

				// Token: 0x0400CCE2 RID: 52450
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x0200324C RID: 12876
			public class ENOUGH_FUEL
			{
				// Token: 0x0400CCE3 RID: 52451
				public static LocString NAME = "Awaiting Fuel";

				// Token: 0x0400CCE4 RID: 52452
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x0200324D RID: 12877
			public class LOGIC
			{
				// Token: 0x0400CCE5 RID: 52453
				public static LocString LOGIC_CONTROLLED_ENABLED = "Enabled by Automation Grid";

				// Token: 0x0400CCE6 RID: 52454
				public static LocString LOGIC_CONTROLLED_DISABLED = "Disabled by Automation Grid";
			}

			// Token: 0x0200324E RID: 12878
			public class GANTRY
			{
				// Token: 0x0400CCE7 RID: 52455
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400CCE8 RID: 52456
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400CCE9 RID: 52457
				public static LocString EXTENDED = "Extended";

				// Token: 0x0400CCEA RID: 52458
				public static LocString RETRACTED = "Retracted";
			}

			// Token: 0x0200324F RID: 12879
			public class OBJECTDISPENSER
			{
				// Token: 0x0400CCEB RID: 52459
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400CCEC RID: 52460
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400CCED RID: 52461
				public static LocString OPENED = "Opened";

				// Token: 0x0400CCEE RID: 52462
				public static LocString CLOSED = "Closed";
			}

			// Token: 0x02003250 RID: 12880
			public class TOO_COLD
			{
				// Token: 0x0400CCEF RID: 52463
				public static LocString NAME = "Too Cold";

				// Token: 0x0400CCF0 RID: 52464
				public static LocString TOOLTIP = "Either this building or its surrounding environment is too cold to operate";
			}

			// Token: 0x02003251 RID: 12881
			public class CHECKPOINT
			{
				// Token: 0x0400CCF1 RID: 52465
				public static LocString LOGIC_CONTROLLED_OPEN = "Clearance: Permitted";

				// Token: 0x0400CCF2 RID: 52466
				public static LocString LOGIC_CONTROLLED_CLOSED = "Clearance: Not Permitted";

				// Token: 0x0400CCF3 RID: 52467
				public static LocString LOGIC_CONTROLLED_DISCONNECTED = "No Automation";

				// Token: 0x02003252 RID: 12882
				public class TOOLTIPS
				{
					// Token: 0x0400CCF4 RID: 52468
					public static LocString LOGIC_CONTROLLED_OPEN = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing";

					// Token: 0x0400CCF5 RID: 52469
					public static LocString LOGIC_CONTROLLED_CLOSED = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass";

					// Token: 0x0400CCF6 RID: 52470
					public static LocString LOGIC_CONTROLLED_DISCONNECTED = string.Concat(new string[]
					{
						"This Checkpoint has not been connected to an ",
						UI.PRE_KEYWORD,
						"Automation",
						UI.PST_KEYWORD,
						" grid"
					});
				}
			}

			// Token: 0x02003253 RID: 12883
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400CCF7 RID: 52471
				public static LocString LOGIC_CONTROLLED_STANDBY = "Incoming Radbolts: Ignore";

				// Token: 0x0400CCF8 RID: 52472
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Incoming Radbolts: Redirect";

				// Token: 0x0400CCF9 RID: 52473
				public static LocString NORMAL = "Normal";

				// Token: 0x02003254 RID: 12884
				public class TOOLTIPS
				{
					// Token: 0x0400CCFA RID: 52474
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400CCFB RID: 52475
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400CCFC RID: 52476
					public static LocString NORMAL = "Incoming Radbolts will be accepted and redirected";
				}
			}

			// Token: 0x02003255 RID: 12885
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400CCFD RID: 52477
				public static LocString LOGIC_CONTROLLED_STANDBY = "Launch Radbolt: Off";

				// Token: 0x0400CCFE RID: 52478
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Launch Radbolt: On";

				// Token: 0x0400CCFF RID: 52479
				public static LocString NORMAL = "Normal";

				// Token: 0x02003256 RID: 12886
				public class TOOLTIPS
				{
					// Token: 0x0400CD00 RID: 52480
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400CD01 RID: 52481
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400CD02 RID: 52482
					public static LocString NORMAL = string.Concat(new string[]
					{
						"Incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD,
						" will be accepted and redirected"
					});
				}
			}

			// Token: 0x02003257 RID: 12887
			public class AWAITINGFUEL
			{
				// Token: 0x0400CD03 RID: 52483
				public static LocString NAME = "Awaiting Fuel: {0}";

				// Token: 0x0400CD04 RID: 52484
				public static LocString TOOLTIP = "This building requires <b>{1}</b> of {0} to operate";
			}

			// Token: 0x02003258 RID: 12888
			public class FOSSILHUNT
			{
				// Token: 0x02003259 RID: 12889
				public class PENDING_EXCAVATION
				{
					// Token: 0x0400CD05 RID: 52485
					public static LocString NAME = "Awaiting Excavation";

					// Token: 0x0400CD06 RID: 52486
					public static LocString TOOLTIP = "Currently awaiting excavation by a Duplicant";
				}

				// Token: 0x0200325A RID: 12890
				public class EXCAVATING
				{
					// Token: 0x0400CD07 RID: 52487
					public static LocString NAME = "Excavation In Progress";

					// Token: 0x0400CD08 RID: 52488
					public static LocString TOOLTIP = "Currently being excavated by a Duplicant";
				}
			}

			// Token: 0x0200325B RID: 12891
			public class MEGABRAINTANK
			{
				// Token: 0x0200325C RID: 12892
				public class PROGRESS
				{
					// Token: 0x0200325D RID: 12893
					public class PROGRESSIONRATE
					{
						// Token: 0x0400CD09 RID: 52489
						public static LocString NAME = "Dream Journals: {ActivationProgress}";

						// Token: 0x0400CD0A RID: 52490
						public static LocString TOOLTIP = "Currently awaiting the Dream Journals necessary to restore this building to full functionality";
					}

					// Token: 0x0200325E RID: 12894
					public class DREAMANALYSIS
					{
						// Token: 0x0400CD0B RID: 52491
						public static LocString NAME = "Analyzing Dreams: {TimeToComplete}s";

						// Token: 0x0400CD0C RID: 52492
						public static LocString TOOLTIP = "Maximum Aptitude effect sustained while dream analysis continues";
					}
				}

				// Token: 0x0200325F RID: 12895
				public class COMPLETE
				{
					// Token: 0x0400CD0D RID: 52493
					public static LocString NAME = "Fully Restored";

					// Token: 0x0400CD0E RID: 52494
					public static LocString TOOLTIP = "This building is functioning at full capacity";
				}
			}

			// Token: 0x02003260 RID: 12896
			public class MEGABRAINNOTENOUGHOXYGEN
			{
				// Token: 0x0400CD0F RID: 52495
				public static LocString NAME = "Lacks Oxygen";

				// Token: 0x0400CD10 RID: 52496
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building needs ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" in order to function"
				});
			}

			// Token: 0x02003261 RID: 12897
			public class NOLOGICWIRECONNECTED
			{
				// Token: 0x0400CD11 RID: 52497
				public static LocString NAME = "No Automation Wire Connected";

				// Token: 0x0400CD12 RID: 52498
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to an ",
					UI.PRE_KEYWORD,
					"Automation",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02003262 RID: 12898
			public class NOTUBECONNECTED
			{
				// Token: 0x0400CD13 RID: 52499
				public static LocString NAME = "No Tube Connected";

				// Token: 0x0400CD14 RID: 52500
				public static LocString TOOLTIP = "The first section of tube extending from a " + BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward";
			}

			// Token: 0x02003263 RID: 12899
			public class NOTUBEEXITS
			{
				// Token: 0x0400CD15 RID: 52501
				public static LocString NAME = "No Landing Available";

				// Token: 0x0400CD16 RID: 52502
				public static LocString TOOLTIP = "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
			}

			// Token: 0x02003264 RID: 12900
			public class STOREDCHARGE
			{
				// Token: 0x0400CD17 RID: 52503
				public static LocString NAME = "Charge Available: {0}/{1}";

				// Token: 0x0400CD18 RID: 52504
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has <b>{0}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt consumes ",
					UI.FormatAsNegativeRate("{2}"),
					" per use"
				});
			}

			// Token: 0x02003265 RID: 12901
			public class NEEDEGG
			{
				// Token: 0x0400CD19 RID: 52505
				public static LocString NAME = "No Egg Selected";

				// Token: 0x0400CD1A RID: 52506
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Collect ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" from ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to incubate"
				});
			}

			// Token: 0x02003266 RID: 12902
			public class NOAVAILABLEEGG
			{
				// Token: 0x0400CD1B RID: 52507
				public static LocString NAME = "No Egg Available";

				// Token: 0x0400CD1C RID: 52508
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is not currently available"
				});
			}

			// Token: 0x02003267 RID: 12903
			public class AWAITINGEGGDELIVERY
			{
				// Token: 0x0400CD1D RID: 52509
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400CD1E RID: 52510
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD;
			}

			// Token: 0x02003268 RID: 12904
			public class INCUBATORPROGRESS
			{
				// Token: 0x0400CD1F RID: 52511
				public static LocString NAME = "Incubating: {Percent}";

				// Token: 0x0400CD20 RID: 52512
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" incubating cozily\n\nIt will hatch when ",
					UI.PRE_KEYWORD,
					"Incubation",
					UI.PST_KEYWORD,
					" reaches <b>100%</b>"
				});
			}

			// Token: 0x02003269 RID: 12905
			public class NETWORKQUALITY
			{
				// Token: 0x0400CD21 RID: 52513
				public static LocString NAME = "Scan Network Quality: {TotalQuality}";

				// Token: 0x0400CD22 RID: 52514
				public static LocString TOOLTIP = "This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s to increase surface coverage and improve network quality\n\n    • Surface Coverage: <b>{Coverage}</b>";
			}

			// Token: 0x0200326A RID: 12906
			public class DETECTORSCANNING
			{
				// Token: 0x0400CD23 RID: 52515
				public static LocString NAME = "Scanning";

				// Token: 0x0400CD24 RID: 52516
				public static LocString TOOLTIP = "This scanner is currently scouring space for anything of interest";
			}

			// Token: 0x0200326B RID: 12907
			public class INCOMINGMETEORS
			{
				// Token: 0x0400CD25 RID: 52517
				public static LocString NAME = "Incoming Object Detected";

				// Token: 0x0400CD26 RID: 52518
				public static LocString TOOLTIP = "Warning!\n\nHigh velocity objects on approach!";
			}

			// Token: 0x0200326C RID: 12908
			public class SPACE_VISIBILITY_NONE
			{
				// Token: 0x0400CD27 RID: 52519
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400CD28 RID: 52520
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x0200326D RID: 12909
			public class SPACE_VISIBILITY_REDUCED
			{
				// Token: 0x0400CD29 RID: 52521
				public static LocString NAME = "Reduced Visibility";

				// Token: 0x0400CD2A RID: 52522
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo operate at maximum speed, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x0200326E RID: 12910
			public class LANDEDROCKETLACKSPASSENGERMODULE
			{
				// Token: 0x0400CD2B RID: 52523
				public static LocString NAME = "Rocket lacks spacefarer module";

				// Token: 0x0400CD2C RID: 52524
				public static LocString TOOLTIP = "A rocket must have a spacefarer module";
			}

			// Token: 0x0200326F RID: 12911
			public class PATH_NOT_CLEAR
			{
				// Token: 0x0400CD2D RID: 52525
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400CD2E RID: 52526
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this rocket:\n    • {0}\n\nThis rocket requires a clear flight path for launch";

				// Token: 0x0400CD2F RID: 52527
				public static LocString TILE_FORMAT = "Solid {0}";
			}

			// Token: 0x02003270 RID: 12912
			public class RAILGUN_PATH_NOT_CLEAR
			{
				// Token: 0x0400CD30 RID: 52528
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400CD31 RID: 52529
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this " + UI.FormatAsLink("Interplanetary Launcher", "RAILGUN") + "\n\nThis launcher requires a clear path to launch payloads";
			}

			// Token: 0x02003271 RID: 12913
			public class RAILGUN_NO_DESTINATION
			{
				// Token: 0x0400CD32 RID: 52530
				public static LocString NAME = "No Delivery Destination";

				// Token: 0x0400CD33 RID: 52531
				public static LocString TOOLTIP = "A delivery destination has not been set";
			}

			// Token: 0x02003272 RID: 12914
			public class NOSURFACESIGHT
			{
				// Token: 0x0400CD34 RID: 52532
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400CD35 RID: 52533
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x02003273 RID: 12915
			public class ROCKETRESTRICTIONACTIVE
			{
				// Token: 0x0400CD36 RID: 52534
				public static LocString NAME = "Access: Restricted";

				// Token: 0x0400CD37 RID: 52535
				public static LocString TOOLTIP = "This building cannot be operated while restricted, though it can be filled\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003274 RID: 12916
			public class ROCKETRESTRICTIONINACTIVE
			{
				// Token: 0x0400CD38 RID: 52536
				public static LocString NAME = "Access: Not Restricted";

				// Token: 0x0400CD39 RID: 52537
				public static LocString TOOLTIP = "This building's operation is not restricted\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003275 RID: 12917
			public class NOROCKETRESTRICTION
			{
				// Token: 0x0400CD3A RID: 52538
				public static LocString NAME = "Not Controlled";

				// Token: 0x0400CD3B RID: 52539
				public static LocString TOOLTIP = "This building is not controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x02003276 RID: 12918
			public class BROADCASTEROUTOFRANGE
			{
				// Token: 0x0400CD3C RID: 52540
				public static LocString NAME = "Broadcaster Out of Range";

				// Token: 0x0400CD3D RID: 52541
				public static LocString TOOLTIP = "This receiver is too far from the selected broadcaster to get signal updates";
			}

			// Token: 0x02003277 RID: 12919
			public class LOSINGRADBOLTS
			{
				// Token: 0x0400CD3E RID: 52542
				public static LocString NAME = "Radbolt Decay";

				// Token: 0x0400CD3F RID: 52543
				public static LocString TOOLTIP = "This building is unable to maintain the integrity of the radbolts it is storing";
			}

			// Token: 0x02003278 RID: 12920
			public class TOP_PRIORITY_CHORE
			{
				// Token: 0x0400CD40 RID: 52544
				public static LocString NAME = "Top Priority";

				// Token: 0x0400CD41 RID: 52545
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This errand has been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					"\n\nThe colony will be in ",
					UI.PRE_KEYWORD,
					"Yellow Alert",
					UI.PST_KEYWORD,
					" until this task is completed"
				});

				// Token: 0x0400CD42 RID: 52546
				public static LocString NOTIFICATION_NAME = "Yellow Alert";

				// Token: 0x0400CD43 RID: 52547
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The following errands have been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02003279 RID: 12921
			public class HOTTUBWATERTOOCOLD
			{
				// Token: 0x0400CD44 RID: 52548
				public static LocString NAME = "Water Too Cold";

				// Token: 0x0400CD45 RID: 52549
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" is below <b>{temperature}</b>\n\nIt is draining so it can be refilled with warmer ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200327A RID: 12922
			public class HOTTUBTOOHOT
			{
				// Token: 0x0400CD46 RID: 52550
				public static LocString NAME = "Building Too Hot";

				// Token: 0x0400CD47 RID: 52551
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{temperature}</b>\n\nIt needs to cool before it can safely be used"
				});
			}

			// Token: 0x0200327B RID: 12923
			public class HOTTUBFILLING
			{
				// Token: 0x0400CD48 RID: 52552
				public static LocString NAME = "Filling Up: ({fullness})";

				// Token: 0x0400CD49 RID: 52553
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub is currently filling with ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\nIt will be available to use when the ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" level reaches <b>100%</b>"
				});
			}

			// Token: 0x0200327C RID: 12924
			public class WINDTUNNELINTAKE
			{
				// Token: 0x0400CD4A RID: 52554
				public static LocString NAME = "Intake Requires Gas";

				// Token: 0x0400CD4B RID: 52555
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A wind tunnel requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" at the top and bottom intakes in order to operate\n\nThe intakes for this wind tunnel don't have enough gas to operate"
				});
			}

			// Token: 0x0200327D RID: 12925
			public class TEMPORAL_TEAR_OPENER_NO_TARGET
			{
				// Token: 0x0400CD4C RID: 52556
				public static LocString NAME = "Temporal Tear not revealed";

				// Token: 0x0400CD4D RID: 52557
				public static LocString TOOLTIP = "This machine is meant to target something in space, but the target has not yet been revealed";
			}

			// Token: 0x0200327E RID: 12926
			public class TEMPORAL_TEAR_OPENER_NO_LOS
			{
				// Token: 0x0400CD4E RID: 52558
				public static LocString NAME = "Line of Sight: Obstructed";

				// Token: 0x0400CD4F RID: 52559
				public static LocString TOOLTIP = "This device needs a clear view of space to operate";
			}

			// Token: 0x0200327F RID: 12927
			public class TEMPORAL_TEAR_OPENER_INSUFFICIENT_COLONIES
			{
				// Token: 0x0400CD50 RID: 52560
				public static LocString NAME = "Too few Printing Pods {progress}";

				// Token: 0x0400CD51 RID: 52561
				public static LocString TOOLTIP = "To open the Temporal Tear, this device relies on a network of activated Printing Pods {progress}";
			}

			// Token: 0x02003280 RID: 12928
			public class TEMPORAL_TEAR_OPENER_PROGRESS
			{
				// Token: 0x0400CD52 RID: 52562
				public static LocString NAME = "Charging Progress: {progress}";

				// Token: 0x0400CD53 RID: 52563
				public static LocString TOOLTIP = "This device must be charged with a high number of Radbolts\n\nOperation can commence once this device is fully charged";
			}

			// Token: 0x02003281 RID: 12929
			public class TEMPORAL_TEAR_OPENER_READY
			{
				// Token: 0x0400CD54 RID: 52564
				public static LocString NOTIFICATION = "Temporal Tear Opener fully charged";

				// Token: 0x0400CD55 RID: 52565
				public static LocString NOTIFICATION_TOOLTIP = "Push the red button to activate";
			}

			// Token: 0x02003282 RID: 12930
			public class WARPPORTALCHARGING
			{
				// Token: 0x0400CD56 RID: 52566
				public static LocString NAME = "Recharging: {charge}";

				// Token: 0x0400CD57 RID: 52567
				public static LocString TOOLTIP = "This teleporter will be ready for use in {cycles} cycles";
			}

			// Token: 0x02003283 RID: 12931
			public class WARPCONDUITPARTNERDISABLED
			{
				// Token: 0x0400CD58 RID: 52568
				public static LocString NAME = "Teleporter Disabled ({x}/2)";

				// Token: 0x0400CD59 RID: 52569
				public static LocString TOOLTIP = "This teleporter cannot be used until both the transmitting and receiving sides have been activated";
			}

			// Token: 0x02003284 RID: 12932
			public class COLLECTINGHEP
			{
				// Token: 0x0400CD5A RID: 52570
				public static LocString NAME = "Collecting Radbolts ({x}/cycle)";

				// Token: 0x0400CD5B RID: 52571
				public static LocString TOOLTIP = "Collecting Radbolts from ambient radiation";
			}

			// Token: 0x02003285 RID: 12933
			public class INORBIT
			{
				// Token: 0x0400CD5C RID: 52572
				public static LocString NAME = "In Orbit: {Destination}";

				// Token: 0x0400CD5D RID: 52573
				public static LocString TOOLTIP = "This rocket is currently in orbit around {Destination}";
			}

			// Token: 0x02003286 RID: 12934
			public class WAITINGTOLAND
			{
				// Token: 0x0400CD5E RID: 52574
				public static LocString NAME = "Waiting to land on {Destination}";

				// Token: 0x0400CD5F RID: 52575
				public static LocString TOOLTIP = "This rocket is waiting for an available Rcoket Platform on {Destination}";
			}

			// Token: 0x02003287 RID: 12935
			public class INFLIGHT
			{
				// Token: 0x0400CD60 RID: 52576
				public static LocString NAME = "In Flight To {Destination_Asteroid}: {ETA}";

				// Token: 0x0400CD61 RID: 52577
				public static LocString TOOLTIP = "This rocket is currently traveling to {Destination_Pad} on {Destination_Asteroid}\n\nIt will arrive in {ETA}";

				// Token: 0x0400CD62 RID: 52578
				public static LocString TOOLTIP_NO_PAD = "This rocket is currently traveling to {Destination_Asteroid}\n\nIt will arrive in {ETA}";
			}

			// Token: 0x02003288 RID: 12936
			public class DESTINATIONOUTOFRANGE
			{
				// Token: 0x0400CD63 RID: 52579
				public static LocString NAME = "Destination Out Of Range";

				// Token: 0x0400CD64 RID: 52580
				public static LocString TOOLTIP = "This rocket lacks the range to reach its destination\n\nRocket Range: {Range}\nDestination Distance: {Distance}";
			}

			// Token: 0x02003289 RID: 12937
			public class ROCKETSTRANDED
			{
				// Token: 0x0400CD65 RID: 52581
				public static LocString NAME = "Stranded";

				// Token: 0x0400CD66 RID: 52582
				public static LocString TOOLTIP = "This rocket has run out of fuel and cannot move";
			}

			// Token: 0x0200328A RID: 12938
			public class SPACEPOIHARVESTING
			{
				// Token: 0x0400CD67 RID: 52583
				public static LocString NAME = "Extracting Resources: {0}";

				// Token: 0x0400CD68 RID: 52584
				public static LocString TOOLTIP = "Resources are being mined from this space debris";
			}

			// Token: 0x0200328B RID: 12939
			public class SPACEPOIWASTING
			{
				// Token: 0x0400CD69 RID: 52585
				public static LocString NAME = "Cannot store resources: {0}";

				// Token: 0x0400CD6A RID: 52586
				public static LocString TOOLTIP = "Some resources being mined from this space debris cannot be stored in this rocket";
			}

			// Token: 0x0200328C RID: 12940
			public class RAILGUNPAYLOADNEEDSEMPTYING
			{
				// Token: 0x0400CD6B RID: 52587
				public static LocString NAME = "Ready To Unpack";

				// Token: 0x0400CD6C RID: 52588
				public static LocString TOOLTIP = "This payload has reached its destination and is ready to be unloaded\n\nIt can be marked for unpacking manually, or automatically unpacked on arrival using a " + BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME;
			}

			// Token: 0x0200328D RID: 12941
			public class MISSIONCONTROLASSISTINGROCKET
			{
				// Token: 0x0400CD6D RID: 52589
				public static LocString NAME = "Guidance Signal: {0}";

				// Token: 0x0400CD6E RID: 52590
				public static LocString TOOLTIP = "Once transmission is complete, Mission Control will boost targeted rocket's speed";
			}

			// Token: 0x0200328E RID: 12942
			public class MISSIONCONTROLBOOSTED
			{
				// Token: 0x0400CD6F RID: 52591
				public static LocString NAME = "Mission Control Speed Boost: {0}";

				// Token: 0x0400CD70 RID: 52592
				public static LocString TOOLTIP = "Mission Control has given this rocket a {0} speed boost\n\n{1} remaining";
			}

			// Token: 0x0200328F RID: 12943
			public class TRANSITTUBEENTRANCEWAXREADY
			{
				// Token: 0x0400CD71 RID: 52593
				public static LocString NAME = "Smooth Ride Ready";

				// Token: 0x0400CD72 RID: 52594
				public static LocString TOOLTIP = "This building is stocked with speed-boosting " + ELEMENTS.MILKFAT.NAME + "\n\n{0} per use ({1} remaining)";
			}

			// Token: 0x02003290 RID: 12944
			public class NOROCKETSTOMISSIONCONTROLBOOST
			{
				// Token: 0x0400CD73 RID: 52595
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400CD74 RID: 52596
				public static LocString TOOLTIP = "Rockets must be mid-flight and not targeted by another Mission Control Station, or already boosted";
			}

			// Token: 0x02003291 RID: 12945
			public class NOROCKETSTOMISSIONCONTROLCLUSTERBOOST
			{
				// Token: 0x0400CD75 RID: 52597
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400CD76 RID: 52598
				public static LocString TOOLTIP = "Rockets must be mid-flight, within {0} tiles, and not targeted by another Mission Control Station or already boosted";
			}

			// Token: 0x02003292 RID: 12946
			public class AWAITINGEMPTYBUILDING
			{
				// Token: 0x0400CD77 RID: 52599
				public static LocString NAME = "Empty Errand";

				// Token: 0x0400CD78 RID: 52600
				public static LocString TOOLTIP = "Building will be emptied once a Duplicant is available";
			}

			// Token: 0x02003293 RID: 12947
			public class DUPLICANTACTIVATIONREQUIRED
			{
				// Token: 0x0400CD79 RID: 52601
				public static LocString NAME = "Activation Required";

				// Token: 0x0400CD7A RID: 52602
				public static LocString TOOLTIP = "A Duplicant is required to bring this building online";
			}

			// Token: 0x02003294 RID: 12948
			public class PILOTNEEDED
			{
				// Token: 0x0400CD7B RID: 52603
				public static LocString NAME = "Switching to Autopilot";

				// Token: 0x0400CD7C RID: 52604
				public static LocString TOOLTIP = "Autopilot will engage in {timeRemaining} if a Duplicant pilot does not assume control";
			}

			// Token: 0x02003295 RID: 12949
			public class AUTOPILOTACTIVE
			{
				// Token: 0x0400CD7D RID: 52605
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400CD7E RID: 52606
				public static LocString TOOLTIP = "This rocket has entered autopilot mode and will fly at reduced speed\n\nIt can resume full speed once a Duplicant pilot takes over";
			}

			// Token: 0x02003296 RID: 12950
			public class INFLIGHTPILOTED
			{
				// Token: 0x0400CD7F RID: 52607
				public static LocString NAME = "Piloted";

				// Token: 0x0400CD80 RID: 52608
				public static LocString DUPE_TOOLTIP = "Duplicant pilot's <b>Skill</b>: +{0} speed boost";

				// Token: 0x0400CD81 RID: 52609
				public static LocString ROBO_TOOLTIP = "Piloted by a " + UI.PRE_KEYWORD + "Robo-Pilot" + UI.PST_KEYWORD;
			}

			// Token: 0x02003297 RID: 12951
			public class INFLIGHTUNPILOTED
			{
				// Token: 0x0400CD82 RID: 52610
				public static LocString NAME = "Unpiloted";

				// Token: 0x0400CD83 RID: 52611
				public static LocString TOOLTIP = "Inactive rocket module: -{penalty} speed {modules}";

				// Token: 0x0400CD84 RID: 52612
				public static LocString ROBO_PILOT_ONLY_TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					" has run out of ",
					UI.PRE_KEYWORD,
					"Data Banks",
					UI.PST_KEYWORD,
					"\n\nThis rocket is stranded"
				});
			}

			// Token: 0x02003298 RID: 12952
			public class INFLIGHTAUTOPILOTED
			{
				// Token: 0x0400CD85 RID: 52613
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400CD86 RID: 52614
				public static LocString TOOLTIP = "This rocket's {modules} is inactive\n\nThis rocket has entered autopilot mode and will fly at reduced speed\n    •  -{penalty} speed";
			}

			// Token: 0x02003299 RID: 12953
			public class INFLIGHTSUPERPILOT
			{
				// Token: 0x0400CD87 RID: 52615
				public static LocString NAME = "Multi-Piloted";

				// Token: 0x0400CD88 RID: 52616
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This rocket is being piloted by a Duplicant and a ",
					UI.PRE_KEYWORD,
					"Robo-Pilot",
					UI.PST_KEYWORD,
					"\n    • Multi-Piloted: +{1} speed boost\n    • Duplicant pilot skill: +{0} speed boost"
				});
			}

			// Token: 0x0200329A RID: 12954
			public class ROCKETCHECKLISTINCOMPLETE
			{
				// Token: 0x0400CD89 RID: 52617
				public static LocString NAME = "Launch Checklist Incomplete";

				// Token: 0x0400CD8A RID: 52618
				public static LocString TOOLTIP = "Critical launch tasks uncompleted\n\nRefer to the Launch Checklist in the status panel";
			}

			// Token: 0x0200329B RID: 12955
			public class ROCKETCARGOEMPTYING
			{
				// Token: 0x0400CD8B RID: 52619
				public static LocString NAME = "Unloading Cargo";

				// Token: 0x0400CD8C RID: 52620
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rocket cargo is being unloaded into the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nLoading of new cargo will begin once unloading is complete"
				});
			}

			// Token: 0x0200329C RID: 12956
			public class ROCKETCARGOFILLING
			{
				// Token: 0x0400CD8D RID: 52621
				public static LocString NAME = "Loading Cargo";

				// Token: 0x0400CD8E RID: 52622
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cargo is being loaded onto the rocket from the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nRocket cargo will be ready for launch once loading is complete"
				});
			}

			// Token: 0x0200329D RID: 12957
			public class ROCKETCARGOFULL
			{
				// Token: 0x0400CD8F RID: 52623
				public static LocString NAME = "Platform Ready";

				// Token: 0x0400CD90 RID: 52624
				public static LocString TOOLTIP = "All cargo operations are complete";
			}

			// Token: 0x0200329E RID: 12958
			public class FLIGHTALLCARGOFULL
			{
				// Token: 0x0400CD91 RID: 52625
				public static LocString NAME = "All cargo bays are full";

				// Token: 0x0400CD92 RID: 52626
				public static LocString TOOLTIP = "Rocket cannot store any more materials";
			}

			// Token: 0x0200329F RID: 12959
			public class FLIGHTCARGOREMAINING
			{
				// Token: 0x0400CD93 RID: 52627
				public static LocString NAME = "Cargo capacity remaining: {0}";

				// Token: 0x0400CD94 RID: 52628
				public static LocString TOOLTIP = "Rocket can store up to {0} more materials";
			}

			// Token: 0x020032A0 RID: 12960
			public class ROCKET_PORT_IDLE
			{
				// Token: 0x0400CD95 RID: 52629
				public static LocString NAME = "Idle";

				// Token: 0x0400CD96 RID: 52630
				public static LocString TOOLTIP = "This port is idle because there is no rocket on the connected " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A1 RID: 12961
			public class ROCKET_PORT_UNLOADING
			{
				// Token: 0x0400CD97 RID: 52631
				public static LocString NAME = "Unloading Rocket";

				// Token: 0x0400CD98 RID: 52632
				public static LocString TOOLTIP = "Resources are being unloaded from the rocket into the local network";
			}

			// Token: 0x020032A2 RID: 12962
			public class ROCKET_PORT_LOADING
			{
				// Token: 0x0400CD99 RID: 52633
				public static LocString NAME = "Loading Rocket";

				// Token: 0x0400CD9A RID: 52634
				public static LocString TOOLTIP = "Resources are being loaded from the local network into the rocket's storage";
			}

			// Token: 0x020032A3 RID: 12963
			public class ROCKET_PORT_LOADED
			{
				// Token: 0x0400CD9B RID: 52635
				public static LocString NAME = "Cargo Transfer Complete";

				// Token: 0x0400CD9C RID: 52636
				public static LocString TOOLTIP = "The connected rocket has either reached max capacity for this resource type, or lacks appropriate storage modules";
			}

			// Token: 0x020032A4 RID: 12964
			public class CONNECTED_ROCKET_PORT
			{
				// Token: 0x0400CD9D RID: 52637
				public static LocString NAME = "Port Network Attached";

				// Token: 0x0400CD9E RID: 52638
				public static LocString TOOLTIP = "This module has been connected to a " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " and can now load and unload cargo";
			}

			// Token: 0x020032A5 RID: 12965
			public class CONNECTED_ROCKET_WRONG_PORT
			{
				// Token: 0x0400CD9F RID: 52639
				public static LocString NAME = "Incorrect Port Network";

				// Token: 0x0400CDA0 RID: 52640
				public static LocString TOOLTIP = "The attached " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " is not the correct type for this cargo module";
			}

			// Token: 0x020032A6 RID: 12966
			public class CONNECTED_ROCKET_NO_PORT
			{
				// Token: 0x0400CDA1 RID: 52641
				public static LocString NAME = "No Rocket Ports";

				// Token: 0x0400CDA2 RID: 52642
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					" has no ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					" attached\n\n",
					UI.PRE_KEYWORD,
					"Solid",
					UI.PST_KEYWORD,
					", ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					", and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME_PLURAL,
					" can be attached to load and unload cargo from a landed rocket's modules"
				});
			}

			// Token: 0x020032A7 RID: 12967
			public class CLUSTERTELESCOPEALLWORKCOMPLETE
			{
				// Token: 0x0400CDA3 RID: 52643
				public static LocString NAME = "Area Complete";

				// Token: 0x0400CDA4 RID: 52644
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Telescope",
					UI.PST_KEYWORD,
					" has analyzed all the space visible from its current location"
				});
			}

			// Token: 0x020032A8 RID: 12968
			public class ROCKETPLATFORMCLOSETOCEILING
			{
				// Token: 0x0400CDA5 RID: 52645
				public static LocString NAME = "Low Clearance: {distance} Tiles";

				// Token: 0x0400CDA6 RID: 52646
				public static LocString TOOLTIP = "Tall rockets may not be able to land on this " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x020032A9 RID: 12969
			public class MODULEGENERATORNOTPOWERED
			{
				// Token: 0x0400CDA7 RID: 52647
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400CDA8 RID: 52648
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine will generate ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" once traveling through space\n\nRight now, it's not doing much of anything"
				});
			}

			// Token: 0x020032AA RID: 12970
			public class MODULEGENERATORPOWERED
			{
				// Token: 0x0400CDA9 RID: 52649
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400CDAA RID: 52650
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine is extracting ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from the thruster\n\nIt will continue generating power as long as it travels through space"
				});
			}

			// Token: 0x020032AB RID: 12971
			public class INORBITREQUIRED
			{
				// Token: 0x0400CDAB RID: 52651
				public static LocString NAME = "Grounded";

				// Token: 0x0400CDAC RID: 52652
				public static LocString TOOLTIP = "This building cannot operate from the surface of a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " and must be in space to function";
			}

			// Token: 0x020032AC RID: 12972
			public class REACTORREFUELDISABLED
			{
				// Token: 0x0400CDAD RID: 52653
				public static LocString NAME = "Refuel Disabled";

				// Token: 0x0400CDAE RID: 52654
				public static LocString TOOLTIP = "This building will not be refueled once its active fuel has been consumed";
			}

			// Token: 0x020032AD RID: 12973
			public class RAILGUNCOOLDOWN
			{
				// Token: 0x0400CDAF RID: 52655
				public static LocString NAME = "Cleaning Rails: {timeleft}";

				// Token: 0x0400CDB0 RID: 52656
				public static LocString TOOLTIP = "This building automatically performs routine maintenance every {x} launches";
			}

			// Token: 0x020032AE RID: 12974
			public class FRIDGECOOLING
			{
				// Token: 0x0400CDB1 RID: 52657
				public static LocString NAME = "Cooling Contents: {UsedPower}";

				// Token: 0x0400CDB2 RID: 52658
				public static LocString TOOLTIP = "{UsedPower} of {MaxPower} are being used to cool the contents of this food storage";
			}

			// Token: 0x020032AF RID: 12975
			public class FRIDGESTEADY
			{
				// Token: 0x0400CDB3 RID: 52659
				public static LocString NAME = "Energy Saver: {UsedPower}";

				// Token: 0x0400CDB4 RID: 52660
				public static LocString TOOLTIP = "The contents of this food storage are at refrigeration temperatures\n\nEnergy Saver mode has been automatically activated using only {UsedPower} of {MaxPower}";
			}

			// Token: 0x020032B0 RID: 12976
			public class TELEPHONE
			{
				// Token: 0x020032B1 RID: 12977
				public class BABBLE
				{
					// Token: 0x0400CDB5 RID: 52661
					public static LocString NAME = "Babbling to no one.";

					// Token: 0x0400CDB6 RID: 52662
					public static LocString TOOLTIP = "{Duplicant} just needed to vent to into the void.";
				}

				// Token: 0x020032B2 RID: 12978
				public class CONVERSATION
				{
					// Token: 0x0400CDB7 RID: 52663
					public static LocString TALKING_TO = "Talking to {Duplicant} on {Asteroid}";

					// Token: 0x0400CDB8 RID: 52664
					public static LocString TALKING_TO_NUM = "Talking to {0} friends.";
				}
			}

			// Token: 0x020032B3 RID: 12979
			public class CREATUREMANIPULATORPROGRESS
			{
				// Token: 0x0400CDB9 RID: 52665
				public static LocString NAME = "Collected Species Data {0}/{1}";

				// Token: 0x0400CDBA RID: 52666
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires data from multiple ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" species to unlock its genetic manipulator\n\nSpecies scanned:"
				});

				// Token: 0x0400CDBB RID: 52667
				public static LocString NO_DATA = "No species scanned";
			}

			// Token: 0x020032B4 RID: 12980
			public class CREATUREMANIPULATORMORPHMODELOCKED
			{
				// Token: 0x0400CDBC RID: 52668
				public static LocString NAME = "Current Status: Offline";

				// Token: 0x0400CDBD RID: 52669
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot operate until it collects more ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x020032B5 RID: 12981
			public class CREATUREMANIPULATORMORPHMODE
			{
				// Token: 0x0400CDBE RID: 52670
				public static LocString NAME = "Current Status: Online";

				// Token: 0x0400CDBF RID: 52671
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is ready to manipulate ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x020032B6 RID: 12982
			public class CREATUREMANIPULATORWAITING
			{
				// Token: 0x0400CDC0 RID: 52672
				public static LocString NAME = "Waiting for a Critter";

				// Token: 0x0400CDC1 RID: 52673
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is waiting for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to get sucked into its scanning area"
				});
			}

			// Token: 0x020032B7 RID: 12983
			public class CREATUREMANIPULATORWORKING
			{
				// Token: 0x0400CDC2 RID: 52674
				public static LocString NAME = "Poking and Prodding Critter";

				// Token: 0x0400CDC3 RID: 52675
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is extracting genetic information from a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x020032B8 RID: 12984
			public class SPICEGRINDERNOSPICE
			{
				// Token: 0x0400CDC4 RID: 52676
				public static LocString NAME = "No Spice Selected";

				// Token: 0x0400CDC5 RID: 52677
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x020032B9 RID: 12985
			public class SPICEGRINDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400CDC6 RID: 52678
				public static LocString NAME = "Spice Grinder accepts mutant seeds";

				// Token: 0x0400CDC7 RID: 52679
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This spice grinder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x020032BA RID: 12986
			public class MISSILELAUNCHER_NOSURFACESIGHT
			{
				// Token: 0x0400CDC8 RID: 52680
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400CDC9 RID: 52681
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x020032BB RID: 12987
			public class MISSILELAUNCHER_PARTIALLYBLOCKED
			{
				// Token: 0x0400CDCA RID: 52682
				public static LocString NAME = "Limited Line of Sight";

				// Token: 0x0400CDCB RID: 52683
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x020032BC RID: 12988
			public class COMPLEXFABRICATOR
			{
				// Token: 0x020032BD RID: 12989
				public class COOKING
				{
					// Token: 0x0400CDCC RID: 52684
					public static LocString NAME = "Cooking {Item}";

					// Token: 0x0400CDCD RID: 52685
					public static LocString TOOLTIP = "This building is currently whipping up a batch of {Item}";
				}

				// Token: 0x020032BE RID: 12990
				public class PRODUCING
				{
					// Token: 0x0400CDCE RID: 52686
					public static LocString NAME = "Producing {Item}";

					// Token: 0x0400CDCF RID: 52687
					public static LocString TOOLTIP = "This building is carrying out its current production orders";
				}

				// Token: 0x020032BF RID: 12991
				public class RESEARCHING
				{
					// Token: 0x0400CDD0 RID: 52688
					public static LocString NAME = "Researching {Item}";

					// Token: 0x0400CDD1 RID: 52689
					public static LocString TOOLTIP = "This building is currently conducting important research";
				}

				// Token: 0x020032C0 RID: 12992
				public class ANALYZING
				{
					// Token: 0x0400CDD2 RID: 52690
					public static LocString NAME = "Analyzing {Item}";

					// Token: 0x0400CDD3 RID: 52691
					public static LocString TOOLTIP = "This building is currently analyzing a fascinating artifact";
				}

				// Token: 0x020032C1 RID: 12993
				public class UNTRAINING
				{
					// Token: 0x0400CDD4 RID: 52692
					public static LocString NAME = "Untraining {Duplicant}";

					// Token: 0x0400CDD5 RID: 52693
					public static LocString TOOLTIP = "Restoring {Duplicant} to a blissfully ignorant state";
				}

				// Token: 0x020032C2 RID: 12994
				public class TELESCOPE
				{
					// Token: 0x0400CDD6 RID: 52694
					public static LocString NAME = "Studying Space";

					// Token: 0x0400CDD7 RID: 52695
					public static LocString TOOLTIP = "This building is currently investigating the mysteries of space";
				}

				// Token: 0x020032C3 RID: 12995
				public class CLUSTERTELESCOPEMETEOR
				{
					// Token: 0x0400CDD8 RID: 52696
					public static LocString NAME = "Studying Meteor";

					// Token: 0x0400CDD9 RID: 52697
					public static LocString TOOLTIP = "This building is currently studying a meteor";
				}
			}

			// Token: 0x020032C4 RID: 12996
			public class REMOTEWORKERDEPOT
			{
				// Token: 0x020032C5 RID: 12997
				public class MAKINGWORKER
				{
					// Token: 0x0400CDDA RID: 52698
					public static LocString NAME = "Assembling Remote Worker";

					// Token: 0x0400CDDB RID: 52699
					public static LocString TOOLTIP = "This building is currently assembling a remote worker drone";
				}
			}

			// Token: 0x020032C6 RID: 12998
			public class REMOTEWORKTERMINAL
			{
				// Token: 0x020032C7 RID: 12999
				public class NODOCK
				{
					// Token: 0x0400CDDC RID: 52700
					public static LocString NAME = "No Dock Assigned";

					// Token: 0x0400CDDD RID: 52701
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This building must be assigned a ",
						UI.PRE_KEYWORD,
						"Remote Worker Dock",
						UI.PST_KEYWORD,
						" in order to function"
					});
				}
			}

			// Token: 0x020032C8 RID: 13000
			public class DATAMINER
			{
				// Token: 0x020032C9 RID: 13001
				public class PRODUCTIONRATE
				{
					// Token: 0x0400CDDE RID: 52702
					public static LocString NAME = "Production Rate: {RATE}";

					// Token: 0x0400CDDF RID: 52703
					public static LocString TOOLTIP = "This building is operating at {RATE} of its maximum speed\n\nProduction rate decreases at higher temperatures\n\nCurrent ambient temperature: {TEMP}";
				}
			}
		}

		// Token: 0x020032CA RID: 13002
		public class DETAILS
		{
			// Token: 0x0400CDE0 RID: 52704
			public static LocString USE_COUNT = "Uses: {0}";

			// Token: 0x0400CDE1 RID: 52705
			public static LocString USE_COUNT_TOOLTIP = "This building has been used {0} times";
		}
	}
}
