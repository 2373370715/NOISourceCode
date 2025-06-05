using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;

// Token: 0x02000BC4 RID: 3012
public class RoomType : Resource
{
	// Token: 0x17000286 RID: 646
	// (get) Token: 0x060038E9 RID: 14569 RVA: 0x000C94DF File Offset: 0x000C76DF
	// (set) Token: 0x060038EA RID: 14570 RVA: 0x000C94E7 File Offset: 0x000C76E7
	public string tooltip { get; private set; }

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x060038EB RID: 14571 RVA: 0x000C94F0 File Offset: 0x000C76F0
	// (set) Token: 0x060038EC RID: 14572 RVA: 0x000C94F8 File Offset: 0x000C76F8
	public string description { get; set; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x060038ED RID: 14573 RVA: 0x000C9501 File Offset: 0x000C7701
	// (set) Token: 0x060038EE RID: 14574 RVA: 0x000C9509 File Offset: 0x000C7709
	public string effect { get; private set; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x060038EF RID: 14575 RVA: 0x000C9512 File Offset: 0x000C7712
	// (set) Token: 0x060038F0 RID: 14576 RVA: 0x000C951A File Offset: 0x000C771A
	public RoomConstraints.Constraint primary_constraint { get; private set; }

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x060038F1 RID: 14577 RVA: 0x000C9523 File Offset: 0x000C7723
	// (set) Token: 0x060038F2 RID: 14578 RVA: 0x000C952B File Offset: 0x000C772B
	public RoomConstraints.Constraint[] additional_constraints { get; private set; }

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x060038F3 RID: 14579 RVA: 0x000C9534 File Offset: 0x000C7734
	// (set) Token: 0x060038F4 RID: 14580 RVA: 0x000C953C File Offset: 0x000C773C
	public int priority { get; private set; }

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060038F5 RID: 14581 RVA: 0x000C9545 File Offset: 0x000C7745
	// (set) Token: 0x060038F6 RID: 14582 RVA: 0x000C954D File Offset: 0x000C774D
	public bool single_assignee { get; private set; }

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060038F7 RID: 14583 RVA: 0x000C9556 File Offset: 0x000C7756
	// (set) Token: 0x060038F8 RID: 14584 RVA: 0x000C955E File Offset: 0x000C775E
	public RoomDetails.Detail[] display_details { get; private set; }

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x060038F9 RID: 14585 RVA: 0x000C9567 File Offset: 0x000C7767
	// (set) Token: 0x060038FA RID: 14586 RVA: 0x000C956F File Offset: 0x000C776F
	public bool priority_building_use { get; private set; }

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x060038FB RID: 14587 RVA: 0x000C9578 File Offset: 0x000C7778
	// (set) Token: 0x060038FC RID: 14588 RVA: 0x000C9580 File Offset: 0x000C7780
	public RoomTypeCategory category { get; private set; }

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x060038FD RID: 14589 RVA: 0x000C9589 File Offset: 0x000C7789
	// (set) Token: 0x060038FE RID: 14590 RVA: 0x000C9591 File Offset: 0x000C7791
	public RoomType[] upgrade_paths { get; private set; }

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x060038FF RID: 14591 RVA: 0x000C959A File Offset: 0x000C779A
	// (set) Token: 0x06003900 RID: 14592 RVA: 0x000C95A2 File Offset: 0x000C77A2
	public string[] effects { get; private set; }

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06003901 RID: 14593 RVA: 0x000C95AB File Offset: 0x000C77AB
	// (set) Token: 0x06003902 RID: 14594 RVA: 0x000C95B3 File Offset: 0x000C77B3
	public int sortKey { get; private set; }

	// Token: 0x06003903 RID: 14595 RVA: 0x00229F50 File Offset: 0x00228150
	public RoomType(string id, string name, string description, string tooltip, string effect, RoomTypeCategory category, RoomConstraints.Constraint primary_constraint, RoomConstraints.Constraint[] additional_constraints, RoomDetails.Detail[] display_details, int priority = 0, RoomType[] upgrade_paths = null, bool single_assignee = false, bool priority_building_use = false, string[] effects = null, int sortKey = 0) : base(id, name)
	{
		this.tooltip = tooltip;
		this.description = description;
		this.effect = effect;
		this.category = category;
		this.primary_constraint = primary_constraint;
		this.additional_constraints = additional_constraints;
		this.display_details = display_details;
		this.priority = priority;
		this.upgrade_paths = upgrade_paths;
		this.single_assignee = single_assignee;
		this.priority_building_use = priority_building_use;
		this.effects = effects;
		this.sortKey = sortKey;
		if (this.upgrade_paths != null)
		{
			RoomType[] upgrade_paths2 = this.upgrade_paths;
			for (int i = 0; i < upgrade_paths2.Length; i++)
			{
				Debug.Assert(upgrade_paths2[i] != null, name + " has a null upgrade path. Maybe it wasn't initialized yet.");
			}
		}
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x0022A000 File Offset: 0x00228200
	public RoomType.RoomIdentificationResult isSatisfactory(Room candidate_room)
	{
		if (this.primary_constraint != null && !this.primary_constraint.isSatisfied(candidate_room))
		{
			return RoomType.RoomIdentificationResult.primary_unsatisfied;
		}
		if (this.additional_constraints != null)
		{
			RoomConstraints.Constraint[] additional_constraints = this.additional_constraints;
			for (int i = 0; i < additional_constraints.Length; i++)
			{
				if (!additional_constraints[i].isSatisfied(candidate_room))
				{
					return RoomType.RoomIdentificationResult.primary_satisfied;
				}
			}
		}
		return RoomType.RoomIdentificationResult.all_satisfied;
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0022A050 File Offset: 0x00228250
	public string GetCriteriaString()
	{
		string text = string.Concat(new string[]
		{
			"<b>",
			this.Name,
			"</b>\n",
			this.tooltip,
			"\n\n",
			ROOMS.CRITERIA.HEADER
		});
		if (this == Db.Get().RoomTypes.Neutral)
		{
			text = text + "\n    • " + ROOMS.CRITERIA.NEUTRAL_TYPE;
		}
		text += ((this.primary_constraint == null) ? "" : ("\n    • " + this.primary_constraint.name));
		if (this.additional_constraints != null)
		{
			foreach (RoomConstraints.Constraint constraint in this.additional_constraints)
			{
				text = text + "\n    • " + constraint.name;
			}
		}
		return text;
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x0022A128 File Offset: 0x00228328
	public string GetRoomEffectsString()
	{
		if (this.effects != null && this.effects.Length != 0)
		{
			string text = ROOMS.EFFECTS.HEADER;
			foreach (string id in this.effects)
			{
				Effect effect = Db.Get().effects.Get(id);
				text += Effect.CreateTooltip(effect, false, "\n    • ", false);
			}
			return text;
		}
		return null;
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x0022A194 File Offset: 0x00228394
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target, out List<EffectInstance> result)
	{
		result = null;
		if (this.primary_constraint == null)
		{
			return;
		}
		if (triggerer == null)
		{
			return;
		}
		if (this.effects == null)
		{
			return;
		}
		if (this.primary_constraint.building_criteria(triggerer))
		{
			result = new List<EffectInstance>();
			foreach (string effect_id in this.effects)
			{
				result.Add(target.Add(effect_id, true));
			}
		}
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x0022A204 File Offset: 0x00228404
	public void TriggerRoomEffects(KPrefabID triggerer, Effects target)
	{
		List<EffectInstance> list;
		this.TriggerRoomEffects(triggerer, target, out list);
	}

	// Token: 0x02000BC5 RID: 3013
	public enum RoomIdentificationResult
	{
		// Token: 0x0400277F RID: 10111
		all_satisfied,
		// Token: 0x04002780 RID: 10112
		primary_satisfied,
		// Token: 0x04002781 RID: 10113
		primary_unsatisfied
	}
}
