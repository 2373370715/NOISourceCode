using System;
using System.Collections.Generic;
using KSerialization;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CF9 RID: 15609
	[SerializationConfig(MemberSerialization.OptIn)]
	[AddComponentMenu("KMonoBehaviour/scripts/Traits")]
	public class Traits : KMonoBehaviour, ISaveLoadable
	{
		// Token: 0x0600EFA7 RID: 61351 RVA: 0x0014549D File Offset: 0x0014369D
		public List<string> GetTraitIds()
		{
			return this.TraitIds;
		}

		// Token: 0x0600EFA8 RID: 61352 RVA: 0x001454A5 File Offset: 0x001436A5
		public void SetTraitIds(List<string> traits)
		{
			this.TraitIds = traits;
		}

		// Token: 0x0600EFA9 RID: 61353 RVA: 0x004EA72C File Offset: 0x004E892C
		protected override void OnSpawn()
		{
			foreach (string id in this.TraitIds)
			{
				if (Db.Get().traits.Exists(id))
				{
					Trait trait = Db.Get().traits.Get(id);
					if (Game.IsCorrectDlcActiveForCurrentSave(trait))
					{
						this.AddInternal(trait);
					}
				}
			}
			if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 15))
			{
				List<DUPLICANTSTATS.TraitVal> joytraits = DUPLICANTSTATS.JOYTRAITS;
				if (base.GetComponent<MinionIdentity>())
				{
					bool flag = true;
					foreach (DUPLICANTSTATS.TraitVal traitVal in joytraits)
					{
						if (this.HasTrait(traitVal.id))
						{
							flag = false;
						}
					}
					if (flag)
					{
						DUPLICANTSTATS.TraitVal random = joytraits.GetRandom<DUPLICANTSTATS.TraitVal>();
						Trait trait2 = Db.Get().traits.Get(random.id);
						this.Add(trait2);
					}
				}
			}
		}

		// Token: 0x0600EFAA RID: 61354 RVA: 0x001454AE File Offset: 0x001436AE
		private void AddInternal(Trait trait)
		{
			if (!this.HasTrait(trait))
			{
				this.TraitList.Add(trait);
				trait.AddTo(this.GetAttributes());
				if (trait.OnAddTrait != null)
				{
					trait.OnAddTrait(base.gameObject);
				}
			}
		}

		// Token: 0x0600EFAB RID: 61355 RVA: 0x004EA854 File Offset: 0x004E8A54
		public void Add(Trait trait)
		{
			DebugUtil.Assert(base.IsInitialized() || base.GetComponent<Modifiers>().IsInitialized(), "Tried adding a trait on a prefab, use Modifiers.initialTraits instead!", trait.Name, base.gameObject.name);
			if (trait.ShouldSave)
			{
				this.TraitIds.Add(trait.Id);
			}
			this.AddInternal(trait);
		}

		// Token: 0x0600EFAC RID: 61356 RVA: 0x004EA8B4 File Offset: 0x004E8AB4
		public bool HasTrait(string trait_id)
		{
			bool result = false;
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == trait_id)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600EFAD RID: 61357 RVA: 0x004EA914 File Offset: 0x004E8B14
		public bool HasTrait(Trait trait)
		{
			using (List<Trait>.Enumerator enumerator = this.TraitList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == trait)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600EFAE RID: 61358 RVA: 0x001454EA File Offset: 0x001436EA
		public void Clear()
		{
			while (this.TraitList.Count > 0)
			{
				this.Remove(this.TraitList[0]);
			}
		}

		// Token: 0x0600EFAF RID: 61359 RVA: 0x004EA96C File Offset: 0x004E8B6C
		public void Remove(Trait trait)
		{
			for (int i = 0; i < this.TraitList.Count; i++)
			{
				if (this.TraitList[i] == trait)
				{
					this.TraitList.RemoveAt(i);
					this.TraitIds.Remove(trait.Id);
					trait.RemoveFrom(this.GetAttributes());
					return;
				}
			}
		}

		// Token: 0x0600EFB0 RID: 61360 RVA: 0x004EA9CC File Offset: 0x004E8BCC
		public bool IsEffectIgnored(Effect effect)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.ignoredEffects != null && Array.IndexOf<string>(trait.ignoredEffects, effect.Id) != -1)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600EFB1 RID: 61361 RVA: 0x004EAA3C File Offset: 0x004E8C3C
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroup, out trait);
		}

		// Token: 0x0600EFB2 RID: 61362 RVA: 0x0014550E File Offset: 0x0014370E
		public bool IsChoreGroupDisabled(ChoreGroup choreGroup, out Trait disablingTrait)
		{
			return this.IsChoreGroupDisabled(choreGroup.IdHash, out disablingTrait);
		}

		// Token: 0x0600EFB3 RID: 61363 RVA: 0x004EAA54 File Offset: 0x004E8C54
		public bool IsChoreGroupDisabled(HashedString choreGroupId)
		{
			Trait trait;
			return this.IsChoreGroupDisabled(choreGroupId, out trait);
		}

		// Token: 0x0600EFB4 RID: 61364 RVA: 0x004EAA6C File Offset: 0x004E8C6C
		public bool IsChoreGroupDisabled(HashedString choreGroupId, out Trait disablingTrait)
		{
			foreach (Trait trait in this.TraitList)
			{
				if (trait.disabledChoreGroups != null)
				{
					ChoreGroup[] disabledChoreGroups = trait.disabledChoreGroups;
					for (int i = 0; i < disabledChoreGroups.Length; i++)
					{
						if (disabledChoreGroups[i].IdHash == choreGroupId)
						{
							disablingTrait = trait;
							return true;
						}
					}
				}
			}
			disablingTrait = null;
			return false;
		}

		// Token: 0x0400EB41 RID: 60225
		public List<Trait> TraitList = new List<Trait>();

		// Token: 0x0400EB42 RID: 60226
		[Serialize]
		private List<string> TraitIds = new List<string>();
	}
}
