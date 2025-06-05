using System;
using ProcGen;

namespace Database
{
	// Token: 0x020021EA RID: 8682
	public class Story : Resource, IComparable<Story>
	{
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x0600B8E9 RID: 47337 RVA: 0x0011BA99 File Offset: 0x00119C99
		// (set) Token: 0x0600B8EA RID: 47338 RVA: 0x0011BAA1 File Offset: 0x00119CA1
		public int HashId { get; private set; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x0600B8EB RID: 47339 RVA: 0x0011BAAA File Offset: 0x00119CAA
		public WorldTrait StoryTrait
		{
			get
			{
				if (this._cachedStoryTrait == null)
				{
					this._cachedStoryTrait = SettingsCache.GetCachedStoryTrait(this.worldgenStoryTraitKey, false);
				}
				return this._cachedStoryTrait;
			}
		}

		// Token: 0x0600B8EC RID: 47340 RVA: 0x0011BACC File Offset: 0x00119CCC
		public Story(string id, string worldgenStoryTraitKey, int displayOrder)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.kleiUseOnlyCoordinateOrder = -1;
			this.updateNumber = -1;
			this.sandboxStampTemplateId = null;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x0600B8ED RID: 47341 RVA: 0x00475F20 File Offset: 0x00474120
		public Story(string id, string worldgenStoryTraitKey, int displayOrder, int kleiUseOnlyCoordinateOrder, int updateNumber, string sandboxStampTemplateId)
		{
			this.Id = id;
			this.worldgenStoryTraitKey = worldgenStoryTraitKey;
			this.displayOrder = displayOrder;
			this.updateNumber = updateNumber;
			this.sandboxStampTemplateId = sandboxStampTemplateId;
			this.kleiUseOnlyCoordinateOrder = kleiUseOnlyCoordinateOrder;
			this.HashId = Hash.SDBMLower(id);
		}

		// Token: 0x0600B8EE RID: 47342 RVA: 0x00475F6C File Offset: 0x0047416C
		public int CompareTo(Story other)
		{
			return this.displayOrder.CompareTo(other.displayOrder);
		}

		// Token: 0x0600B8EF RID: 47343 RVA: 0x0011BB0A File Offset: 0x00119D0A
		public bool IsNew()
		{
			return this.updateNumber == LaunchInitializer.UpdateNumber();
		}

		// Token: 0x0600B8F0 RID: 47344 RVA: 0x0011BB19 File Offset: 0x00119D19
		public Story AutoStart()
		{
			this.autoStart = true;
			return this;
		}

		// Token: 0x0600B8F1 RID: 47345 RVA: 0x0011BB23 File Offset: 0x00119D23
		public Story SetKeepsake(string prefabId)
		{
			this.keepsakePrefabId = prefabId;
			return this;
		}

		// Token: 0x04009761 RID: 38753
		public const int MODDED_STORY = -1;

		// Token: 0x04009762 RID: 38754
		public int kleiUseOnlyCoordinateOrder;

		// Token: 0x04009764 RID: 38756
		public bool autoStart;

		// Token: 0x04009765 RID: 38757
		public string keepsakePrefabId;

		// Token: 0x04009766 RID: 38758
		public readonly string worldgenStoryTraitKey;

		// Token: 0x04009767 RID: 38759
		private readonly int displayOrder;

		// Token: 0x04009768 RID: 38760
		private readonly int updateNumber;

		// Token: 0x04009769 RID: 38761
		public string sandboxStampTemplateId;

		// Token: 0x0400976A RID: 38762
		private WorldTrait _cachedStoryTrait;
	}
}
