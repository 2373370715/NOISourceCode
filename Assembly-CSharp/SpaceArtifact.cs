using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020019EB RID: 6635
[AddComponentMenu("KMonoBehaviour/scripts/SpaceArtifact")]
public class SpaceArtifact : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06008A48 RID: 35400 RVA: 0x00369810 File Offset: 0x00367A10
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.loadCharmed && DlcManager.IsExpansion1Active())
		{
			base.gameObject.AddTag(GameTags.CharmedArtifact);
			this.SetEntombedDecor();
		}
		else
		{
			this.loadCharmed = false;
			this.SetAnalyzedDecor();
		}
		this.UpdateStatusItem();
		Components.SpaceArtifacts.Add(this);
		this.UpdateAnim();
	}

	// Token: 0x06008A49 RID: 35401 RVA: 0x000FEEB6 File Offset: 0x000FD0B6
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.SpaceArtifacts.Remove(this);
	}

	// Token: 0x06008A4A RID: 35402 RVA: 0x000FEEC9 File Offset: 0x000FD0C9
	public void RemoveCharm()
	{
		base.gameObject.RemoveTag(GameTags.CharmedArtifact);
		this.UpdateStatusItem();
		this.loadCharmed = false;
		this.UpdateAnim();
		this.SetAnalyzedDecor();
	}

	// Token: 0x06008A4B RID: 35403 RVA: 0x000FEEF4 File Offset: 0x000FD0F4
	private void SetEntombedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(DECOR.BONUS.TIER0);
	}

	// Token: 0x06008A4C RID: 35404 RVA: 0x000FEF06 File Offset: 0x000FD106
	private void SetAnalyzedDecor()
	{
		base.GetComponent<DecorProvider>().SetValues(this.artifactTier.decorValues);
	}

	// Token: 0x06008A4D RID: 35405 RVA: 0x00369870 File Offset: 0x00367A70
	public void UpdateStatusItem()
	{
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, null);
			return;
		}
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.ArtifactEntombed, false);
	}

	// Token: 0x06008A4E RID: 35406 RVA: 0x000FEF1E File Offset: 0x000FD11E
	public void SetArtifactTier(ArtifactTier tier)
	{
		this.artifactTier = tier;
	}

	// Token: 0x06008A4F RID: 35407 RVA: 0x000FEF27 File Offset: 0x000FD127
	public ArtifactTier GetArtifactTier()
	{
		return this.artifactTier;
	}

	// Token: 0x06008A50 RID: 35408 RVA: 0x000FEF2F File Offset: 0x000FD12F
	public void SetUIAnim(string anim)
	{
		this.ui_anim = anim;
	}

	// Token: 0x06008A51 RID: 35409 RVA: 0x000FEF38 File Offset: 0x000FD138
	public string GetUIAnim()
	{
		return this.ui_anim;
	}

	// Token: 0x06008A52 RID: 35410 RVA: 0x003698D4 File Offset: 0x00367AD4
	public List<Descriptor> GetEffectDescriptions()
	{
		List<Descriptor> list = new List<Descriptor>();
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			Descriptor item = new Descriptor(STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), STRINGS.BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.PAYLOAD_DROP_RATE_TOOLTIP.Replace("{chance}", GameUtil.GetFormattedPercent(this.artifactTier.payloadDropChance * 100f, GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect, false);
			list.Add(item);
		}
		Descriptor item2 = new Descriptor(string.Format("This is an artifact from space", Array.Empty<object>()), string.Format("This is the tooltip string", Array.Empty<object>()), Descriptor.DescriptorType.Information, false);
		list.Add(item2);
		return list;
	}

	// Token: 0x06008A53 RID: 35411 RVA: 0x000FEF40 File Offset: 0x000FD140
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return this.GetEffectDescriptions();
	}

	// Token: 0x06008A54 RID: 35412 RVA: 0x00369984 File Offset: 0x00367B84
	private void UpdateAnim()
	{
		string s;
		if (base.gameObject.HasTag(GameTags.CharmedArtifact))
		{
			s = "entombed_" + this.uniqueAnimNameFragment.Replace("idle_", "");
		}
		else
		{
			s = this.uniqueAnimNameFragment;
		}
		base.GetComponent<KBatchedAnimController>().Play(s, KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06008A55 RID: 35413 RVA: 0x003699E8 File Offset: 0x00367BE8
	[OnDeserialized]
	public void OnDeserialize()
	{
		Pickupable component = base.GetComponent<Pickupable>();
		if (component != null)
		{
			component.deleteOffGrid = false;
		}
	}

	// Token: 0x0400685B RID: 26715
	public const string ID = "SpaceArtifact";

	// Token: 0x0400685C RID: 26716
	private const string charmedPrefix = "entombed_";

	// Token: 0x0400685D RID: 26717
	private const string idlePrefix = "idle_";

	// Token: 0x0400685E RID: 26718
	[SerializeField]
	private string ui_anim;

	// Token: 0x0400685F RID: 26719
	[Serialize]
	private bool loadCharmed = true;

	// Token: 0x04006860 RID: 26720
	public ArtifactTier artifactTier;

	// Token: 0x04006861 RID: 26721
	public ArtifactType artifactType;

	// Token: 0x04006862 RID: 26722
	public string uniqueAnimNameFragment;
}
